
Partial Class LeadManagement_EditLeadTemplate
    Inherits System.Web.UI.Page

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        LoadList()
    End Sub

    Private Sub LoadList()
        gvMappings.DataSource = (New clsLeadTemplateMapping).List(IIf(Request("LeadTemplateID") <> "" And IsNumeric(Request("LeadTemplateID")), Request("LeadTemplateID"), 0))
        gvMappings.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oLT As New clsLeadTemplates
            For i = 1 To 30
                ddColumns.Items.Add(New ListItem(i, i))
                ddPhoneColumn.Items.Add(New ListItem(i, i))
            Next
            LoadList()
            oLT.LeadTemplateID = IIf(Request("LeadTemplateID") <> "" And IsNumeric(Request("LeadTemplateID")), Request("LeadTemplateID"), 0)
            oLT.Load()
            txtName.Text = oLT.Name
            ddColumns.SelectedValue = oLT.NumberColumns
            ddPhoneColumn.SelectedValue = oLT.PhoneColumn
            oLT = Nothing
        End If
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        If Not (CheckSecurity("LeadTemplates", "Edit", , , Session("UserDBID"))) Then
            Dim oTemplate As New clsLeadTemplates
            Dim id As Integer = 0
            oTemplate.LeadTemplateID = IIf(Request("Leadtemplateid") & "" = "", 0, Request("LeadTemplateID"))
            oTemplate.Load()
            oTemplate.Name = txtName.Text
            oTemplate.NumberColumns = ddColumns.SelectedValue
            oTemplate.PhoneColumn = ddPhoneColumn.SelectedValue
            oTemplate.Save()
            id = oTemplate.LeadTemplateID
            oTemplate = Nothing
            Response.Redirect("editleadtemplate.aspx?leadtemplateid=" & id)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AD", "alert('Access Denied');", True)
        End If
    End Sub

    Protected Sub lbCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCancel.Click
        Response.Redirect("LeadTemplates.aspx")
    End Sub
End Class
