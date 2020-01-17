Imports System.Data
Imports System.IO
Imports ClosedXML.Excel
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Security
Imports System.Web
Imports System.Configuration
Imports System

Partial Class Accounting_CreateEquiantMFFile
    Inherits System.Web.UI.Page

    Protected Sub GetWorkBook_Click(sender As Object, e As EventArgs) Handles GetWorkBook.Click
        lblErr.Text = ""
        If dfStart.Selected_Date <> "" And dfEnd.Selected_Date <> "" Then
            Generate_Report()
            'lblErr.Text = Get_SQL(1)
        Else
            lblErr.Text = "Please select a date range"
        End If
    End Sub

    Private Sub Loop_Records(ByRef dr As SqlDataReader, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        Dim col As Integer = 1
        While dr.Read
            If row = 1 Then
                For col = 1 To dr.VisibleFieldCount
                    ws.Cell(row, col).SetValue(dr.GetName(col - 1))
                Next
                row += 1
            End If
            For col = 1 To dr.VisibleFieldCount
                If dr.GetName(col - 1) = "DueDate" Then
                    Dim val As String = (dr.Item(col - 1) & "")
                    If val.Contains(" ") Then
                        val = val.ToString.Split(" ")(0)
                    End If
                    If val = "" Then
                        val = "2/1/" & dr("TransactionCode").ToString.Substring(4, 4)
                    End If
                    ws.Cell(row, col).SetValue(val)
                ElseIf dr.GetName(col - 1) = "TransactionDate" Or dr.GetName(col - 1) = "StatementDate" Then
                    Dim val As String = (dr.Item(col - 1) & "").ToString.Split(" ")(0)
                    '    ws.Cell(row, col).SetValue(oMort.APR)
                    'ElseIf dr.GetName(col - 1) = "DownPayment" Then
                    '    If dr("Equity") Is System.DBNull.Value Then
                    '        ws.Cell(row, col).SetValue(dr.Item(col - 1))
                    '    Else
                    '        ws.Cell(row, col).SetValue(dr.Item(col - 1) - dr("Equity"))
                    '    End If

                    'ElseIf dr.GetName(col - 1) = "SSN1" Or dr.GetName(col - 1) = "SSN2" Or dr.GetName(col - 1) = "SSN3" Then
                    '    Dim val As String = dr.Item(col - 1) & ""
                    '    val = val.Replace("-", "")
                    '    If val.Length = 9 Then
                    '        val = val.Insert(5, "-").Insert(3, "-")
                    '    End If
                    ws.Cell(row, col).SetValue(val)
                Else
                    ws.Cell(row, col).SetValue(dr.Item(col - 1))
                End If
            Next
            row += 1
            GC.Collect()
        End While
        dr.Close()
    End Sub


    Private Sub Generate_Report()
        Dim res As HttpResponse = Response
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand(Get_SQL(1), cn)
        Dim xlWB As New XLWorkbook
        cm.CommandTimeout = 10000
        cn.Open()

        cm.CommandText = Get_SQL(1)
        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Charges"))


        cn.Close()
        cn = Nothing
        cm = Nothing

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""EquiantCharges.xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblErr.Text = ""
        If Not (IsPostBack) Then
            Dim lst As New List(Of ListItem)
            Dim cn As New SqlConnection(Resources.Resource.cns)
            Dim cm As New SqlCommand("", cn)
            Dim da As New SqlDataAdapter(cm)
            Dim ds As New DataSet

            cm.CommandText = "Select '- ALL -' as MFTrans union Select Distinct(tc.Comboitem) as MFTrans from t_FinTransCodes f inner join t_ComboItems tc on f.TransCodeID = tc.ComboItemID inner join t_ComboItems tt on f.TransTypeID = tt.ComboItemID where tt.ComboItem = 'MFTrans' and tc.ComboItem like 'MF%'"
            da.Fill(ds, "TC")
            lbSelectedTC.Items.Clear()
            lbTC.Items.Clear()
            For Each dr As DataRow In ds.Tables("TC").Rows
                lst.Add(New ListItem With {.Value = dr("MFTrans"), .Text = dr("MFTrans")})
            Next
            lbTC.DataSource = lst
            lbTC.DataBind()
            lst.Clear()
            ds = Nothing
            da = Nothing
            cm = Nothing
            cn = Nothing
        Else
            'lblErr.Text = Get_SQL(1)
        End If
    End Sub

    Private Function Get_SQL(ByVal index As Integer) As String
        Dim ret As String = ""
        Dim trans As String = ""
        For Each item As ListItem In lbSelectedTC.Items
            If item.Text = "- ALL -" Then
                trans = ""
                For Each newitem As ListItem In lbSelectedTC.Items
                    If newitem.Text <> "- ALL -" Then
                        trans &= If(trans = "", "'" & newitem.Text & "'", ",'" & newitem.Text & "'")
                    End If
                Next
                For Each nextitem As ListItem In lbTC.Items
                    trans &= If(trans = "", "'" & nextitem.Text & "'", ",'" & nextitem.Text & "'")
                Next
                Exit For
            Else
                trans &= If(trans = "", "'" & item.Text & "'", ",'" & item.Text & "'")
            End If
        Next
        Select Case index
            Case 1

                ret = "select  case when i.saletype = 'Estates' then '131100' + CAST(c.contractid as varchar(10)) when i.SaleType='Townes' then '133100' + CAST(c.contractid as varchar(10)) else '132100' + CAST(c.contractid as varchar(10)) end as Account, " & _
                        "   'CHG' as Type, 'LF' as Category, inv.Amount, 'CHG-20' + RIGHT(inv.invoice,2) as 'TransactionCode', inv.TransDate as 'TransactionDate', inv.DueDate, Case when invoice = 'MF16' then DATEADD(dd,2,TransDate) when invoice = 'MF17' then '11/15/2017' else DATEADD(dd,1,getdate()) end as 'StatementDate' " & _
                        "   From v_Invoices inv  " & _
                        "	inner join t_Contract c on inv.keyvalue = c.ContractID " & _
                        "	inner join v_ContractInventory i on i.ContractID = c.ContractID " & _
                        "Where inv.keyfield = 'ContractID' and inv.invoice = 'Late Fee' and inv.Reference in (" & trans.Replace("MF", "LF") & ") and inv.TransDate between '" & dfStart.Selected_Date & "' and '" & dfEnd.Selected_Date & "'"
            Case Else

        End Select
        Return ret
    End Function


    Private Sub ReOrderDropDown(ByRef dd As DropDownList)
        'Dim listCopy As New List(Of ListItem)
        'For Each item As ListItem In dd.Items
        '    listCopy.Add(item)
        'Next
        'dd.Items.Clear()
        'For Each item In listCopy.Sort()
        '    dd.Items.Add(item)
        'Next
    End Sub

    Protected Sub addTC_Click(sender As Object, e As EventArgs) Handles addTC.Click
        If Not (IsNothing(lbTC.SelectedItem)) Then
            lbSelectedTC.Items.Add(lbTC.SelectedItem)
            lbTC.Items.Remove(lbTC.SelectedItem)
            SortList(lbSelectedTC)
        End If
    End Sub

    Protected Sub remTC_Click(sender As Object, e As EventArgs) Handles remTC.Click
        If Not (IsNothing(lbSelectedTC.SelectedItem)) Then
            lbTC.Items.Add(lbSelectedTC.SelectedItem)
            lbSelectedTC.Items.Remove(lbSelectedTC.SelectedItem)
            SortList(lbTC)
        End If
    End Sub

    Private Sub SortList(ByRef lb As ListBox)
        If lb.Items.Count > 1 Then
            Dim items As New List(Of ListItem)
            For Each item In lb.Items
                items.Add(item)
            Next
            lb.Items.Clear()
            items.Sort(Function(x As ListItem, y As ListItem)
                                           Return x.Text.CompareTo(y.Text)
                                       End Function)
            lb.DataSource = items
            lb.DataBind()
        End If
    End Sub
End Class
