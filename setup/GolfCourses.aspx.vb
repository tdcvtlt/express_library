
Partial Class setup_GolfCourses
    Inherits System.Web.UI.Page

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim oGolf As New clsGolfCourse
        gvGolf.DataSource = oGolf.List_Courses(txtFilter.Text)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvGolf.DataKeyNames = sKeys
        gvGolf.DataBind()
        oGolf = Nothing
    End Sub
    Protected Sub gvGolf_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvGolf.SelectedIndexChanged
        Dim row As GridViewRow = gvGolf.SelectedRow
        Response.Redirect("EditGolfCourse.aspx?ID=" & row.Cells(1).Text)
    End Sub
    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Response.Redirect("EditGolfCourse.aspx?ID=0")
    End Sub
End Class
