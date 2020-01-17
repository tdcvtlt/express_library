
Partial Class LeadManagement_LeadTemplates
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gvTemplates.datasource = (New clsLeadTemplates).List(False)
        gvTemplates.databind()
    End Sub

    Protected Sub lbAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAdd.Click
        Response.Redirect("editleadtemplate.aspx?leadtemplateid=0")
    End Sub
End Class
