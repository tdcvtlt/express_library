
Partial Class LeadManagement_EditLPHTML
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oLPH As New clsLeadProgram2HTML
            oLPH.ID = Request("ID")
            oLPH.Load()
            txtPath.Text = oLPH.FilePath
            cbHTML.Checked = oLPH.isHTML
            cbSideBar.Checked = oLPH.sidebar
            cbTerms.Checked = oLPH.terms
            cbActive.Checked = oLPH.Active
            oLPH = Nothing
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim oLPH As New clsLeadProgram2HTML
        Dim oLP As New clsLeadProgram
        oLPH.ID = Request("ID")
        oLPH.Load()
        oLPH.UserID = Session("UserDBID")
        If Request("ID") = 0 Then
            oLPH.LeadProgramID = Request("LPID")
            oLP.Update_All_Versions(Request("LPID"), 0.01)
            'Update Version
        Else
            If txtPath.Text <> oLPH.FilePath Or cbHTML.Checked <> oLPH.isHTML Or cbActive.Checked <> oLPH.Active Or cbSideBar.Checked <> oLPH.sidebar Or cbTerms.Checked <> oLPH.terms Then
                oLP.Update_All_Versions(oLPH.LeadProgramID, 0.01)
            End If
        End If
        oLPH.FilePath = txtPath.Text
        oLPH.isHTML = cbHTML.Checked
        oLPH.Active = cbActive.Checked
        oLPH.sidebar = cbSideBar.Checked
        oLPH.terms = cbTerms.Checked
        oLPH.Save()
        oLPH = Nothing
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.Refresh_HTML();window.close();", True)

    End Sub
End Class
