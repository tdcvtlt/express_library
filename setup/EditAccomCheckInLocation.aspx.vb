
Partial Class setup_EditAccomCheckInLocation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oLoc As New clsAccomCheckInLocations
            oLoc.ID = Request("ID")
            oLoc.Load()
            siLocations.Connection_String = Resources.Resource.cns
            siLocations.ComboItem = "AccomCheckInLocations"
            siLocations.Selected_ID = oLoc.CheckInLocationID
            siLocations.Load_Items()
            CKEditor1.Text = oLoc.Directions
            MultiView1.ActiveViewIndex = 0
            oLoc = Nothing
        End If
    End Sub
    Protected Sub LinkButtonAccom_Click(sender As Object, e As System.EventArgs) Handles LinkButtonAccom.Click
        MultiView1.ActiveViewIndex = 0
    End Sub


    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim oLoc As New clsAccomCheckInLocations
        oLoc.ID = Request("ID")
        oLoc.UserID = Session("userDBID")
        oLoc.Load()
        oLoc.CheckInLocationID = siLocations.Selected_ID
        oLoc.Directions = CKEditor1.Text
        oLoc.Save()
        Response.Redirect("EditAccomCheckInLocation.aspx?ID=" & oLoc.ID)
        oLoc = Nothing
    End Sub
End Class
