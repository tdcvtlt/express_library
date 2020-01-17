
Partial Class security_PersonnelGroups
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oPersGroup As New clsPersonnelGroup
        gvPersonnelGroups.DataSource = oPersGroup.List(txtGroup.Text)
        gvPersonnelGroups.DataBind()
        oPersGroup = Nothing
    End Sub

    Protected Sub gvPersonnelGroups_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvPersonnelGroups.SelectedIndexChanged
        Dim row As GridViewRow = gvPersonnelGroups.SelectedRow
        Response.Redirect("EditPersonnelGroup.aspx?GroupID=" & row.Cells(1).Text)
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        Response.Redirect("EditPersonnelGroup.aspx?GroupID=0")
    End Sub
End Class
