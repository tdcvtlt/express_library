

Partial Class Add_Ins_CreateFixedWeekReservations
    Inherits System.Web.UI.Page

    Protected Sub btnRun_Click(sender As Object, e As EventArgs) Handles btnRun.Click
        gvResults.DataSource = (New clsFixedWeekCreation).Create_Fixed_Week("2016", Session("UserDBID"))
        gvResults.DataBind()
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        ExportToExcel(sender, e, gvResults)
    End Sub

    Protected Sub ExportToExcel(sender As Object, e As EventArgs, ByRef GridView1 As GridView)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Response.Write("<html><head><title>Reservations</title></head><body><table>")
        Response.Write("<tr>")
        For Each cell As TableCell In GridView1.HeaderRow.Cells
            Response.Write("<td>" & cell.Text & "</td>")
        Next
        Response.Write("</tr>")
        For Each row As GridViewRow In GridView1.Rows
            Response.Write("<tr>")
            For Each cell As TableCell In row.Cells
                Response.Write("<td>" & cell.Text & "</td>")
            Next
            Response.Write("</tr>")
        Next
        Response.Write("</table></body></html>")
        Response.Flush()
        Response.[End]()

    End Sub
End Class
