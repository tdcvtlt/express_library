Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports ClosedXML.Excel
Imports System.IO

Partial Class Reports_Marketing_RandomDrawing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Fill_Vendors()
            'btnExport.Enabled = False
        End If
    End Sub

    Private Sub Fill_Vendors()
        cblVendors.DataSource = New SqlDataSource(Resources.Resource.cns, "select distinct v.vendorid, v.vendor from t_Leads l inner join t_Vendor v on v.vendorid = l.vendorid order by v.vendor")
        cblVendors.DataValueField = "VendorID"
        cblVendors.DataTextField = "Vendor"
        cblVendors.DataBind()
    End Sub

    Protected Sub btnList_Click(sender As Object, e As EventArgs) Handles btnList.Click
        Dim v As String = VendorIDs()
        If dfstart.selected_date <> "" And dfend.selected_date <> "" And v <> "" Then
            'gvEntries.DataSource = New SqlDataSource(Resources.Resource.cns, Get_SQL)
            'gvEntries.DataBind()
            'btnExport.Enabled = True
            Generate_Report()
        End If
    End Sub

    Private Function Get_SQL() As String
        Return "exec sp_RandomDrawing '" & dfstart.selected_date & "','" & dfend.selected_date & "', '" & VendorIDs() & "'"
    End Function

    Private Function VendorIDs() As String
        Dim ret As String = ""
        For Each i As ListItem In cblVendors.Items
            If i.Selected Then
                ret &= IIf(ret = "", i.Value, "," & i.Value)
            End If
        Next
        Return ret
    End Function


    Private Sub Loop_Records(ByRef dr As SqlDataReader, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        While dr.Read
            If row = 1 Then
                For col = 1 To dr.VisibleFieldCount
                    ws.Cell(row, col).SetValue(dr.GetName(col - 1))
                Next
                row += 1
            End If
            For col = 1 To dr.VisibleFieldCount
                ws.Cell(row, col).SetValue(dr.Item(col - 1))
            Next
            row += 1
        End While
        dr.Close()
    End Sub


    Private Sub Generate_Report()
        Dim res As HttpResponse = Response
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("sp_RandomDrawing", cn)
        Dim xlWB As New XLWorkbook
        cm.CommandType = CommandType.StoredProcedure
        cm.Parameters.AddWithValue("@SDate", dfStart.Selected_Date)
        cm.Parameters.AddWithValue("@EDate", dfEnd.Selected_Date)
        cm.Parameters.AddWithValue("@VendorIDS", VendorIDs)

        cn.Open()


        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Sheet1"))
        cn.Close()
        cn = Nothing
        cm = Nothing

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""RandomDrawing.xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub
End Class
