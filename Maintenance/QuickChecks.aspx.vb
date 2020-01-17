
Partial Class Maintenance_QuickChecks
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            gvQuickChecks.DataSource = (New clsMaintQuickCheck).List
            gvQuickChecks.DataBind()
        End If
    End Sub
    Protected Sub gvQuickChecks_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvQuickChecks.SelectedIndexChanged
        Dim row As GridViewRow = gvQuickChecks.SelectedRow
        Response.Redirect("EditQuickCheck.aspx?id=" & row.Cells(1).Text)
    End Sub
    Protected Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click
        Response.Redirect("EditQuickCheck.aspx?id=0")
    End Sub
End Class
