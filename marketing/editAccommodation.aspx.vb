
Partial Class marketing_editAccommodation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Load_Lookups()
            Set_Values()
        End If
    End Sub

    Protected Sub Load_Lookups()
        For i = 0 To 50
            ddNumAdults.Items.Add(i)
            ddNumChildren.Items.Add(i)
        Next
        For i = 0 To 99
            ddPromoNights.Items.Add(i)
            ddAddNights.Items.Add(i)
        Next
        Dim oCombo As New clsComboItems
        ddResLocation.Items.Add(New ListItem("", 0))
        ddResLocation.AppendDataBoundItems = True
        ddResLocation.DataSource = oCombo.Load_ComboItems("ReservationLocation")
        ddResLocation.DataTextField = "ComboItem"
        ddResLocation.DataValueField = "ComboItemID"
        ddResLocation.DataBind()
        oCombo = Nothing
        siGuestType.Connection_String = Resources.Resource.cns
        siGuestType.ComboItem = "AccGuestType"
        siGuestType.Label_Caption = ""
        siGuestType.Load_Items()
        siRoomType.Connection_String = Resources.Resource.cns
        siRoomType.ComboItem = "AccomRoomType"
        siRoomType.Label_Caption = ""
        siRoomType.Load_Items()
    End Sub

    Protected Sub Set_Values()
        Dim oAccom As New clsAccommodation
        Dim oRes As New clsReservations
        Dim oAcc As New clsAccom
        Dim oAccLoc As New clsAccom2CheckInLocation
        oAccom.AccommodationID = Request("AccommodationID")
        oAccom.Load()
        If oAccom.AccommodationID = 0 Then
            oRes.ReservationID = Request("ReservationID")
        Else
            oRes.ReservationID = oAccom.ReservationID
        End If
        oRes.Load()
        ddResLocation.SelectedValue = oRes.ResLocationID
        ddAccom.Items.Add(New ListItem("", 0))
        ddAccom.DataSource = oAcc.Accoms_By_Location(oRes.ResLocationID, oAccom.AccomID)
        ddAccom.DataTextField = "AccomName"
        ddAccom.DataValueField = "AccomID"
        ddAccom.AppendDataBoundItems = True
        ddAccom.DataBind()
        ddAccom.SelectedValue = oAccom.AccomID
        ddCheckIn.Items.Add(New ListItem("", 0))
        ddCheckIn.DataSource = oAccLoc.CheckIn_Locations_By_Accom(oAccom.AccomID, oAccom.CheckInLocationID)
        ddCheckIn.DataTextField = "Location"
        ddCheckIn.DataValueField = "ID"
        ddCheckIn.AppendDataBoundItems = True
        ddCheckIn.DataBind()
        ddCheckIn.SelectedValue = oAccom.CheckInLocationID
        oAccLoc = Nothing
        oAcc = Nothing
        oRes = Nothing
        txtAccomID.Text = oAccom.AccommodationID
        txtConfirmation.Text = oAccom.ConfirmationNumber
        siGuestType.Selected_ID = oAccom.GuestTypeID
        siRoomType.Selected_ID = oAccom.RoomTypeID
        dteArrivalDate.Selected_Date = oAccom.ArrivalDate
        dteDepartureDate.Selected_Date = oAccom.DepartureDate
        ddNumAdults.SelectedValue = oAccom.NumberAdults
        ddNumChildren.SelectedValue = oAccom.NumberChildren
        ddPromoNights.SelectedValue = oAccom.PromoNights
        txtPromoRate.Text = oAccom.PromoRate
        ddAddNights.SelectedValue = oAccom.AdditionalNights
        txtAddRate.Text = oAccom.AdditionalRate
        txtRoomCost.Text = oAccom.RoomCost
        chkSmoking.Checked = oAccom.Smoking
        oAccom = Nothing
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If CheckSecurity("Accommodations", "Edit", , , Session("UserDBID")) Then
            Dim oAccom As New clsAccommodation
            Dim bProceed As Boolean = True
            If Request("AccommodationID") = 0 Then
                If oAccom.Accom_Count(Request("ReservationID")) > 0 Then
                    If Not (CheckSecurity("Accommodations", "AddExtra", , , Session("UserDBID"))) Then
                        bProceed = False
                    End If
                End If
            End If
            If bProceed Then
                oAccom.AccommodationID = Request("AccommodationID")
                oAccom.Load()
                If Request("TourID") <> "" Then
                    oAccom.TourID = Request("TourID")
                End If
                If Request("ReservationID") <> "" Then
                    oAccom.ReservationID = Request("ReservationID")
                End If
                oAccom.ConfirmationNumber = txtConfirmation.Text
                oAccom.GuestTypeID = siGuestType.Selected_ID
                oAccom.RoomTypeID = siRoomType.Selected_ID
                oAccom.ArrivalDate = dteArrivalDate.Selected_Date
                oAccom.DepartureDate = dteDepartureDate.Selected_Date
                oAccom.NumberAdults = ddNumAdults.SelectedValue
                oAccom.NumberChildren = ddNumChildren.SelectedValue
                oAccom.PromoNights = ddPromoNights.SelectedValue
                oAccom.PromoRate = txtPromoRate.Text
                oAccom.AdditionalNights = ddAddNights.SelectedValue
                oAccom.RoomCost = txtRoomCost.Text
                oAccom.Smoking = chkSmoking.Checked
                oAccom.AccomID = ddAccom.SelectedValue
                oAccom.CheckInLocationID = ddCheckIn.SelectedValue
                oAccom.Save()
                oAccom = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Accom();window.close();", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('You Do Not Have Permission To Add An Extra Accommodation.');", True)
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('You Do Not Have Permission To Edit Accommodations.');", True)
        End If
    End Sub

    Protected Sub ddResLocation_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddResLocation.SelectedIndexChanged
        ddAccom.Items.Clear()
        ddCheckIn.Items.Clear()
        Dim oAcc As New clsAccom
        ddAccom.Items.Add(New ListItem("", 0))
        ddCheckIn.Items.Add(New ListItem("", 0))
        ddAccom.DataSource = oAcc.Accoms_By_Location(ddResLocation.SelectedValue, 0)
        ddAccom.DataTextField = "AccomName"
        ddAccom.DataValueField = "AccomID"
        ddAccom.AppendDataBoundItems = True
        ddAccom.DataBind()
        oAcc = Nothing
    End Sub

    Protected Sub ddAccom_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddAccom.SelectedIndexChanged
        ddCheckIn.Items.Clear()
        Dim oAccLoc As New clsAccom2CheckInLocation
        ddCheckIn.Items.Add(New ListItem("", 0))
        ddCheckIn.DataSource = oAccLoc.CheckIn_Locations_By_Accom(ddAccom.SelectedValue, 0)
        ddCheckIn.DataTextField = "Location"
        ddCheckIn.DataValueField = "ID"
        ddCheckIn.AppendDataBoundItems = True
        ddCheckIn.DataBind()
        oAccLoc = Nothing
    End Sub
End Class
