
Partial Class PropertyManagement_Projects_Areas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            gvAreas.DataSource = (New clsProjectAreas).List
            gvAreas.DataBind()
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If CheckSecurity("Projects", "List", , , Session("UserDBID")) Then
            Dim oAreas As New clsProjectAreas
            gvAreas.DataSource = oAreas.Query(100, ddFilter.SelectedValue, txtFilter.Text, "")
            gvAreas.DataBind()

            lblError.Text = oAreas.Error_Message
            oAreas = Nothing
        Else
            lblError.Text = "ACCESS DENIED"
        End If
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Response.Redirect("editarea.aspx?id=0")
    End Sub
End Class
