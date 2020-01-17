Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports ClosedXML.Excel

Partial Class Reports_Accounting_OpenWorkOrders
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim ds As New SqlDataSource(Resources.Resource.cns, Get_SQL(1))
        gbWorkOrders.DataSource = ds
        gbWorkOrders.DataBind()
        ds = Nothing
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gbWorkOrders.Rows.Count > 0 Then
            Generate_Report()
        End If
    End Sub

    Private Function Get_SQL(index As Integer) As String
        Return "select r.RequestID,  rs.comboitem as RequestStatus, rp.ItemNumber, b.ItemDesc, rp.QTY, s.ComboItem as ItemStatus " & _
                "from t_RequestParts rp " & _
                    "inner join t_ComboItems s on s.ComboItemID = rp.StatusID " & _
                    "inner join t_Request r on r.RequestID = rp.RequestID " & _
                    "inner join t_ComboItems rs on rs.ComboItemID = r.StatusID " & _
                    "inner join [RS-SQL-02].KCPOA.dbo.IV00101 b on RTrim(rp.ItemNumber) = RTrim(b.ITEMNMBR) " & _
                "where GPImported=0 and s.ComboItem in ('Assigned','Requested') and b.itemtype = '1'"
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
        Dim cm As New SqlCommand(Get_SQL(1), cn)
        Dim xlWB As New XLWorkbook
        cm.CommandTimeout = 10000
        cn.Open()

        cm.CommandText = Get_SQL(1)
        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Sheet1"))
        cn.Close()
        cn = Nothing
        cm = Nothing

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""OpenWorkOrders.xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub
End Class
