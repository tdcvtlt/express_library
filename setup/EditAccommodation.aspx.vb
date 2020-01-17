
Partial Class setup_EditAccommodation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            siState.Label_Caption = ""
            siState.Connection_String = Resources.Resource.cns
            siState.ComboItem = "State"
            siState.Load_Items()
            Dim oAccom As New clsAccom
            oAccom.AccomID = Request("AccommodationID")
            oAccom.Load()
            siAccomLoc.Label_Caption = ""
            siAccomLoc.Connection_String = Resources.Resource.cns
            siAccomLoc.ComboItem = "ReservationLocation"
            siAccomLoc.Selected_ID = oAccom.AccomLocationID
            siAccomLoc.Load_Items()
            oAccom = Nothing
            Load_Values(Request("AccommodationID"))
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Private Sub Load_Values(ByVal accomID As Integer)
        Dim oAccom As New clsAccom
        oAccom.AccomID = accomID
        oAccom.Load()
        txtAccomID.Text = accomID
        txtAccomName.Text = oAccom.AccomName
        txtDesc.Text = oAccom.Description
        txtOnlineDesc.Text = oAccom.OnlineDescription
        txtAddress.Text = oAccom.Address
        txtCity.Text = oAccom.City
        txtPostal.Text = oAccom.PostalCode
        txtURL.Text = oAccom.URL
        siState.Selected_ID = oAccom.StateID
        siAccomLoc.Selected_ID = oAccom.AccomLocationID
        cbActive.Checked = oAccom.Active
        oAccom = Nothing
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim oAccom As New clsAccom
        oAccom.AccomID = txtAccomID.Text
        oAccom.Load()
        oAccom.UserID = Session("UserDBID")
        oAccom.AccomName = txtAccomName.Text
        oAccom.Description = txtDesc.Text
        oAccom.OnlineDescription = txtOnlineDesc.Text
        oAccom.Address = txtAddress.Text
        oAccom.City = txtCity.Text
        oAccom.StateID = siState.Selected_ID
        oAccom.PostalCode = txtPostal.Text
        oAccom.URL = txtURL.Text
        oAccom.AccomLocationID = siAccomLoc.Selected_ID
        oAccom.Active = cbActive.Checked
        oAccom.Save()
        If Request("AccommodationID") = 0 Then
            Response.Redirect("editAccommodation.aspx?accommodationid=" & oAccom.AccomID)
        Else
            Load_Values(oAccom.AccomID)
        End If
        oAccom = Nothing
    End Sub

    Protected Sub LinkButtonResortRoomTypes_Click(sender As Object, e As System.EventArgs) Handles LinkButtonResortRoomTypes.Click
        If txtAccomID.Text > 0 Then
            Dim oAccom2Resort As New clsAccom2Resort
            gvResortRoomTypes.DataSource = oAccom2Resort.Get_Accom_Resort_RoomTypes(txtAccomID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvResortRoomTypes.DataKeyNames = sKeys
            gvResortRoomTypes.DataBind()
            oAccom2Resort = Nothing
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub LinkButtonHotelRoomTypes_Click(sender As Object, e As System.EventArgs) Handles LinkButtonHotelRoomTypes.Click
        If txtAccomID.Text > 0 Then
            Dim oAccom2Roomtype As New clsAccom2RoomType
            gvHotelRoomTypes.DataSource = oAccom2Roomtype.Get_Accom_Hotel_RoomTypes(txtAccomID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvHotelRoomTypes.DataKeyNames = sKeys
            gvHotelRoomTypes.DataBind()
            oAccom2Roomtype = Nothing
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub gvResortRoomTypes_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvResortRoomTypes.SelectedIndexChanged
        Dim row As GridViewRow = gvResortRoomTypes.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/editResortAccom.aspx?ID=" & row.Cells(1).Text & "','win01',350,350);", True)
    End Sub

    Protected Sub gvHotelRoomTypes_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvHotelRoomTypes.SelectedIndexChanged
        Dim row As GridViewRow = gvHotelRoomTypes.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/editHotelAccom.aspx?ID=" & row.Cells(1).Text & "','win01',350,350);", True)
    End Sub

    Protected Sub btnAddResort_Click(sender As Object, e As System.EventArgs) Handles btnAddResort.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/editResortAccom.aspx?ID=0&AccomID=" & txtAccomID.Text & "','win01',350,350);", True)
    End Sub

    Protected Sub btnAddHotel_Click(sender As Object, e As System.EventArgs) Handles btnAddHotel.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/editHotelAccom.aspx?ID=0','win01',350,350);", True)
    End Sub

    Protected Sub LinkButtonAccom_Click(sender As Object, e As System.EventArgs) Handles LinkButtonAccom.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub LinkButtonDirections_Click(sender As Object, e As System.EventArgs) Handles LinkButtonDirections.Click
        If txtAccomID.Text > 0 Then
            Dim oAccom As New clsAccom
            oAccom.AccomID = txtAccomID.Text
            oAccom.Load()
            CKEditor1.Text = oAccom.Directions
            oAccom = Nothing
            MultiView1.ActiveViewIndex = 3
        End If
    End Sub

    Protected Sub LinkButtonCheckIn_Click(sender As Object, e As System.EventArgs) Handles LinkButtonCheckIn.Click
        If txtAccomID.Text > 0 Then
            Dim oLoc As New clsAccom2CheckInLocation
            gvCheckInLocations.DataSource = oLoc.Get_CheckIn_Locations(txtAccomID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvCheckInLocations.DataKeyNames = sKeys
            gvCheckInLocations.DataBind()
            oLoc = Nothing
            MultiView1.ActiveViewIndex = 4
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim oAccom As New clsAccom
        oAccom.AccomID = txtAccomID.Text
        oAccom.Load()
        oAccom.UserID = Session("UserDBID")
        oAccom.Directions = CKEditor1.Text
        oAccom.Save()
        oAccom = Nothing
    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/editAccom2CheckIn.aspx?ID=0&AccomID=" & txtAccomID.Text & "','win01',350,350);", True)
    End Sub

    Protected Sub gvCheckInLocations_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvCheckInLocations.SelectedIndexChanged
        Dim row As GridViewRow = gvCheckInLocations.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/editAccom2CheckIn.aspx?ID=" & row.Cells(1).Text & "','win01',350,350);", True)

    End Sub
End Class
