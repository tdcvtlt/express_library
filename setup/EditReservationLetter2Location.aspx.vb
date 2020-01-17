
Partial Class setup_EditReservationLetter2Location
    Inherits System.Web.UI.Page
    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim oResLetter2Loc As New clsReservationLetter2Location
        oResLetter2Loc.ID = Request("ID")
        oResLetter2Loc.UserID = Session("UserDBID")
        oResLetter2Loc.Load()
        If Request("ID") = 0 Then
            oResLetter2Loc.ResLetterID = Request("LetterID")
        End If
        oResLetter2Loc.ResLocationID = siLocation.Selected_ID
        oResLetter2Loc.Active = cbActive.Checked
        oResLetter2Loc.Save()
        oResLetter2Loc = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Locations();window.close();", True)
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oResLetter2Loc As New clsReservationLetter2Location
            oResLetter2Loc.ID = Request("ID")
            oResLetter2Loc.Load()
            cbActive.Checked = oResLetter2Loc.Active
            siLocation.Connection_String = Resources.Resource.cns
            siLocation.ComboItem = "ReservationLocation"
            siLocation.Selected_ID = oResLetter2Loc.ResLocationID
            siLocation.Label_Caption = ""
            siLocation.Load_Items()
            oResLetter2Loc = Nothing
        End If
    End Sub
End Class
