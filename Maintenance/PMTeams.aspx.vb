
Partial Class Maintenance_PMTeams
    Inherits System.Web.UI.Page

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim oPMTeam As New clsPMTeams
        gvTeams.DataSource = oPMTeam.List_Teams(txtFilter.Text)
        Dim sKeys(0) As String
        sKeys(0) = "PMTeamID"
        gvTeams.DataKeyNames = sKeys
        gvTeams.DataBind()
        oPMTeam = Nothing
    End Sub

    Protected Sub gvTeams_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvTeams.SelectedIndexChanged
        Dim row As GridViewRow = gvTeams.SelectedRow
        Response.Redirect("EditPMTeam.aspx?TeamID=" & row.Cells(1).Text)
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Response.Redirect("EditPMTeam.aspx?TeamID=0")
    End Sub
End Class
