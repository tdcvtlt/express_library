
Partial Class Maintenance_Refurbs
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            gvRefurbs.DataSource = (New clsRefurb).List
            gvRefurbs.DataBind()
        End If
    End Sub
    Protected Sub gvQuickChecks_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvRefurbs.SelectedIndexChanged
        Dim row As GridViewRow = gvRefurbs.SelectedRow
        Response.Redirect("EditRefurb.aspx?id=" & row.Cells(1).Text)
    End Sub
    Protected Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click
        Response.Redirect("EditRefurb.aspx?id=0")
    End Sub
End Class
