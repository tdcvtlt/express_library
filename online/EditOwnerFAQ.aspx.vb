
Partial Class online_EditOwnerFAQ
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("ID") <> "" And Request("ID") <> "0" Then
                Dim oFAQ As New clsOwnerFAQ
                oFAQ.FAQID = Request("ID")
                oFAQ.Load()
                txtQuestion.Text = oFAQ.Question
                txtAnswer.Text = oFAQ.Answer
                txtFAQID.Text = oFAQ.FAQID
                cbActive.Checked = oFAQ.Active
                oFAQ = Nothing
            Else
                txtFAQID.Text = 0
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CheckSecurity("OnlineFAQ", "Edit", , , Session("UserDBID")) Then
            Dim oFAQ As New clsOwnerFAQ
            oFAQ.FAQID = txtFAQID.Text
            oFAQ.Load()
            oFAQ.Question = txtQuestion.Text
            oFAQ.Answer = txtAnswer.Text
            oFAQ.Active = cbActive.Checked
            oFAQ.Save()
            oFAQ = Nothing
            Response.Redirect("ownerfaqs.aspx")
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Access", "alert('Access Denied');", True)
        End If
    End Sub
End Class
