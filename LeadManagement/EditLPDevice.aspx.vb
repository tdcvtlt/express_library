
Partial Class LeadManagement_EditLPDevice
    Inherits System.Web.UI.Page

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim oLPD As New clsLeadProgram2Device
        oLPD.ID = Request("ID")
        oLPD.UserID = Session("UserDBID")
        oLPD.Load()
        If Request("ID") = 0 Then
            oLPD.LeadProgramID = Request("LPID")
        End If
        oLPD.Device = txtDevice.Text
        oLPD.Active = cbActive.Checked
        oLPD.Save()
        oLPD = Nothing
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.Refresh_Devices();window.close();", True)

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oLPD As New clsLeadProgram2Device
            oLPD.ID = Request("ID")
            oLPD.Load()
            txtDevice.Text = oLPD.Device
            cbActive.Checked = oLPD.Active
            oLPD = Nothing
        End If
    End Sub
End Class
