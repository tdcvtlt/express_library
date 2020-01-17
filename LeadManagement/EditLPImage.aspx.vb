
Partial Class LeadManagement_EditLPImage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oLPI As New clsLeadProgram2Image
            oLPI.ID = Request("ID")
            oLPI.Load()
            siLocation.Connection_String = Resources.Resource.cns
            siLocation.Label_Caption = ""
            siLocation.ComboItem = "LeadProgramLocation"
            siLocation.Selected_ID = oLPI.LocationID
            siLocation.Load_Items()
            siLocation.Selected_ID = oLPI.LocationID
            txtURL.Text = oLPI.URL
            siLocation.Selected_ID = oLPI.LocationID
            cbActive.Checked = oLPI.Active
            cbPicture.Checked = oLPI.Image
            oLPI = Nothing
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim oLPI As New clsLeadProgram2Image
        Dim oLP As New clsLeadProgram
        oLPI.ID = Request("ID")
        oLPI.Load()
        oLPI.UserID = Session("UserDBID")
        If Request("ID") = 0 Then
            oLPI.LeadProgramID = Request("LPID")
            oLP.Update_Version(Request("LPID"), siLocation.Selected_ID, 0.001)
            'Update Version
        Else
            If siLocation.Selected_ID <> oLPI.LocationID Or txtURL.Text <> oLPI.URL Or cbActive.Checked <> oLPI.Active Or cbPicture.Checked <> oLPI.Image Then
                If siLocation.Selected_ID <> oLPI.LocationID Then
                    oLP.Update_Version(oLPI.LeadProgramID, oLPI.LocationID, 0.001)
                End If
                oLP.Update_Version(oLPI.LeadProgramID, siLocation.Selected_ID, 0.001)
            End If
        End If
        oLPI.URL = txtURL.Text
        oLPI.LocationID = siLocation.Selected_ID
        oLPI.Active = cbActive.Checked
        oLPI.Image = cbPicture.Checked
        oLPI.Save()
        oLPI = Nothing
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.Refresh_Images();window.close();", True)

    End Sub
End Class
