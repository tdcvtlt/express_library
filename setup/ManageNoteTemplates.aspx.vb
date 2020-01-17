
Partial Class setup_ManageNoteTemplates
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            LoadTemplates()
        End If
    End Sub
    Private Sub LoadTemplates()
        gvTemplates.DataSource = (New clsNoteTemplates).List(Session("UserDBID"), False, False)
        gvTemplates.DataBind()

    End Sub
    Protected Sub lbRefresh_Click(sender As Object, e As EventArgs) Handles lbRefresh.Click
        LoadTemplates()
    End Sub
End Class
