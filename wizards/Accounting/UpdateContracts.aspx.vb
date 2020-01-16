Imports System.Data.SqlClient
Imports System.Data
Imports System

Partial Class wizards_Accounting_UpdateContracts
    Inherits System.Web.UI.Page

    Structure TextBoxes
        Dim text As String
        Dim first As TextBox
        Dim second As TextBox
        Dim third As TextBox
    End Structure
    Dim boxes As New List(Of TextBoxes)


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Request("id") <> "" Then

        End If

        If IsPostBack Then
            'Build()
            Build_Table()
            'Clicked()
        Else
            'Build()
            Build_Table()

            For Each row As TableRow In table1.Rows
                If row.Cells.Count = 4 Then
                    If row.Cells(0).Text <> "KCP" Then
                        CType(row.Cells(1).Controls(0), TextBox).Text = (New clsUserFields).Get_UserField_Value((New clsUserFields).Get_UserFieldID(3, "Conveyance Instr #"), (New clsContract).Get_Contract_ID(row.Cells(0).Text))
                        CType(row.Cells(2).Controls(0), TextBox).Text = (New clsUserFields).Get_UserField_Value((New clsUserFields).Get_UserFieldID(3, "Conveyance Rec Date"), (New clsContract).Get_Contract_ID(row.Cells(0).Text))
                        CType(row.Cells(3).Controls(0), TextBox).Text = (New clsUserFields).Get_UserField_Value((New clsUserFields).Get_UserFieldID(3, "Conveyance Type"), (New clsContract).Get_Contract_ID(row.Cells(0).Text))
                    End If
                End If
            Next
        End If
    End Sub

    Protected Sub Clicked()
        Dim ufFirstID As Integer = (New clsUserFields).Get_UserFieldID(3, "Conveyance Instr #")
        Dim ufSecondID As Integer = (New clsUserFields).Get_UserFieldID(3, "Conveyance Rec Date")
        Dim ufThirdID As Integer = (New clsUserFields).Get_UserFieldID(3, "Conveyance Type")
        For Each item As TextBoxes In boxes
            Dim id As String = item.second.ID.Split("-")(0)
            Dim kcp As String = item.text
            Dim ConInstNum As String = item.first.Text
            Dim DateRec As String = item.second.Text
            Dim ConvType As String = item.third.Text
            Dim ContractID As Integer = (New clsContract).Get_Contract_ID(kcp)
            For i = 1 To 3
                Dim uf As New clsUserFields
                Dim ufid As Integer = IIf(i = 1, ufFirstID, IIf(i = 2, ufSecondID, ufThirdID))
                Dim ufvalue As String = IIf(i = 1, item.first.Text, IIf(i = 2, item.second.Text, item.third.Text))
                uf.ID = uf.Get_UserField_Value_ID(ufid, ContractID)
                uf.UFID = ufid
                uf.UFValue = ufvalue
                uf.KeyValue = ContractID
                uf.UserID = Session("UserDBID")
                uf.Save()
                uf = Nothing
            Next
        Next
        Response.Redirect("CancellationWiz.aspx")
    End Sub


    Private Sub Build()
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select c.contractnumber, b.batch2contractid from t_CancellationBatch2Contract b inner join t_Contract c on c.contractid = b.contractid where batchid = " & Request("ID") & " and b.dateremoved is null order by c.contractnumber", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        Dim tr As TableRow
        Dim tc As TableCell
        da.Fill(ds, "KCP")

        If ds.Tables("KCP").Rows.Count > 0 Then
            For Each row As DataRow In ds.Tables("KCP").Rows
                Dim textboxrow As New TextBoxes
                With textboxrow
                    .text = row("ContractNumber")
                    .first = New TextBox With {.ID = row("batch2contractid") & "-Conveyance", .Text = (New clsUserFields).Get_UserField_Value((New clsUserFields).Get_UserFieldID(3, "Conveyance Instr #"), (New clsContract).Get_Contract_ID(.Text))}
                    .second = New TextBox With {.ID = row("batch2contractid") & "-daterecorded", .Text = (New clsUserFields).Get_UserField_Value((New clsUserFields).Get_UserFieldID(3, "Conveyance Rec Date"), (New clsContract).Get_Contract_ID(.Text))}
                    .second.Attributes.Add("onclick", "scwShow(this,this);")
                    .third = New TextBox With {.ID = row("batch2contractid") & "-conveyancetype", .Text = (New clsUserFields).Get_UserField_Value((New clsUserFields).Get_UserFieldID(3, "Conveyance Type"), (New clsContract).Get_Contract_ID(.Text))}
                End With
                boxes.Add(textboxrow)


            Next

        End If

        cn = Nothing
        cm = Nothing
        da = Nothing
        ds = Nothing
        tr = Nothing
        tc = Nothing
    End Sub

    Protected Sub Build_Table()
        Dim tr As TableRow
        Dim tc As TableCell
        For Each row As TextBoxes In boxes
            tr = New TableRow
            tr.EnableViewState = True
            tr.Cells.Add(New TableCell With {.Text = row.text})
            tc = New TableCell
            tc.Controls.Add(row.first)
            tr.Cells.Add(tc)
            tc = New TableCell
            tc.Controls.Add(row.second)
            tr.Cells.Add(tc)
            tc = New TableCell
            tc.Controls.Add(row.third)
            tr.Cells.Add(tc)
            table1.Rows.Add(tr)
        Next

        tc = New TableCell
        tr = New TableRow
        tr.EnableViewState = True
        tc = New TableCell
        tc.Controls.Add(New Button With {.ID = "btnSubmit", .Text = "Submit"})
        tr.Cells.Add(tc)
        table1.Rows.Add(tr)
        table1.ID = "MyTable"
    End Sub

    Protected Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        'Dim keys As List(Of String) = Request.Form.AllKeys.Where(Function(key) key.Contains("Conveyance")).ToList()
        'For Each key As String In keys
        Me.Build()
        '    Exit For
        'Next
    End Sub

    Private Sub wizards_Accounting_UpdateContracts_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If IsPostBack Then Clicked()
    End Sub
End Class
