
Partial Class LeadManagement_LeadProgramEntryForms
    Inherits System.Web.UI.Page

    Protected Sub btnList_Click(sender As Object, e As EventArgs) Handles btnList.Click
        Dim oLPE As New clsLeadProgramEntryForm
        gvEntryForms.DataSource = oLPE.List_Forms
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvEntryForms.DataKeyNames = sKeys
        gvEntryForms.DataBind()
        oLPE = Nothing
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Response.Redirect("EditLPEntryForm.aspx?ID=0")
    End Sub

    Protected Sub gvEntryForms_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvEntryForms.SelectedIndexChanged
        Dim row As GridViewRow = gvEntryForms.SelectedRow
        Response.Redirect("EditLPEntryForm.aspx?ID=" & row.Cells(1).Text)
    End Sub
End Class
