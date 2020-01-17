
Partial Class online_OwnerFAQs
    Inherits System.Web.UI.Page

    Protected Sub gvFAQ_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFAQ.SelectedIndexChanged
        Response.Redirect("editownerfaq.aspx?id=" & gvFAQ.Rows(gvFAQ.SelectedIndex).Cells(1).Text)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim ds As New SqlDataSource(Resources.Resource.cns, "Select * from t_OwnerFAQ order by FAQID")
            gvFAQ.DataSource = ds
            gvFAQ.DataBind()
            ds = Nothing
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("editownerfaq.aspx?id=0")
    End Sub
End Class
