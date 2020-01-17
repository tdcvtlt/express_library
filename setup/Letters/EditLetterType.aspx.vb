
Partial Class setup_Letters_EditLetterType
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            MultiView1.ActiveViewIndex = 0
        End If
        
        Load_All()
    End Sub

    Private Sub Load_All()
        gvViews.DataSource = (New clsLetterViews).List(IIf(Request("ID") = "" Or Not (IsNumeric(Request("ID"))), 0, Request("ID")))
        gvViews.DataBind()
        lblLetterType.Text = (New clsComboItems).Lookup_ComboItem(IIf(Request("ID") = "" Or Not (IsNumeric(Request("ID"))), 0, Request("ID")))
        gvTags.DataSource = (New clsLetterTags).List_Setup(IIf(Request("ID") = "" Or Not (IsNumeric(Request("ID"))), 0, Request("ID")))
        gvTags.DataBind()
    End Sub

    Protected Sub lbViews_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbViews.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub lbTags_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTags.Click
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        Load_All()
    End Sub
End Class
