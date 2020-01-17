
Partial Class mis_ITSecRequests
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim oSecRequest As New clsPersonnelSecurityRequest
        gvRequests.DataSource = oSecRequest.List(txtRequest.Text)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvRequests.DataKeyNames = sKeys
        gvRequests.DataBind()
        oSecRequest = Nothing
    End Sub

    Protected Sub gvRequests_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvRequests.SelectedIndexChanged
        Dim row As GridViewRow = gvRequests.SelectedRow
        Response.Redirect("EditITSecRequest.aspx?id=" & row.Cells(1).Text)
    End Sub

    Protected Sub gvRequests_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(7).Text = CDate(e.Row.Cells(7).Text).ToShortDateString
                e.Row.Cells(2).Text = CDate(e.Row.Cells(2).Text).ToShortDateString
            End If
        End If
    End Sub
End Class
