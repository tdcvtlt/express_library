
Partial Class setup_AccomCheckInLocations
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim oLoc As New clsAccomCheckInLocations
        gvLocations.DataSource = oLoc.List(txtFilter.Text)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvLocations.DataKeyNames = sKeys
        gvLocations.DataBind()
        oLoc = Nothing
    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
        Response.Redirect("EditAccomCheckInLocation.aspx?ID=0")
    End Sub

    Protected Sub gvLocations_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvLocations.SelectedIndexChanged
        Dim row As GridViewRow = gvLocations.SelectedRow
        Response.Redirect("EditAccomCheckInLocation.aspx?ID=" & row.Cells(1).Text)
    End Sub
End Class
