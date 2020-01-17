
Partial Class LeadManagement_EditLPEntryForm
    Inherits System.Web.UI.Page

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim oLPE As New clsLeadProgramEntryForm
        oLPE.ID = txtID.Text
        oLPE.Load()
        oLPE.Description = txtDesc.Text
        oLPE.HTMLPath = txtHTML.Text
        oLPE.JSPath = txtJS.Text
        oLPE.SideBarPath = txtSB.Text
        oLPE.TermsPath = txtTerms.Text
        If txtID.Text = 0 Then
            oLPE.DateCreated = System.DateTime.Now
        End If
        oLPE.UserID = Session("UserDBID")
        oLPE.Save()
        Response.Redirect("EditLPEntryForm.aspx?ID=" & oLPE.ID)
        oLPE = Nothing
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oLPE As New clsLeadProgramEntryForm
            oLPE.ID = Request("ID")
            oLPE.Load()
            txtID.Text = Request("ID")
            txtDesc.Text = oLPE.Description
            txtHTML.Text = oLPE.HTMLPath
            txtJS.Text = oLPE.JSPath
            txtSB.Text = oLPE.SideBarPath
            txtTerms.Text = oLPE.TermsPath
            oLPE = Nothing
        End If
    End Sub
End Class
