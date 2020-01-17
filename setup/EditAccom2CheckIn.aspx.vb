
Partial Class setup_EditAccom2CheckIn
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oLoc As New clsAccom2CheckInLocation
            oLoc.ID = Request("ID")
            oLoc.Load()
            cbActive.Checked = oLoc.Active
            siLocations.Connection_String = Resources.Resource.cns
            siLocations.Label_Caption = ""
            siLocations.ComboItem = "AccomCheckInLocations"
            siLocations.Selected_ID = oLoc.CheckInLocationID
            siLocations.Load_Items()
            oLoc = Nothing
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim oLoc As New clsAccom2CheckInLocation
        oLoc.ID = Request("ID")
        oLoc.UserID = Session("UserDBID")
        oLoc.Load()
        If Request("ID") = 0 Then
            oLoc.AccomID = Request("AccomID")
        End If
        oLoc.Active = cbActive.Checked
        oLoc.CheckInLocationID = siLocations.Selected_ID
        oLoc.Save()
        oLoc = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_CheckIns();window.close();", True)
    End Sub
End Class
