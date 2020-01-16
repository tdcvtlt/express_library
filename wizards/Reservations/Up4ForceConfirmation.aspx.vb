Imports System.Reflection
Imports System.Web.Script.Serialization
Imports clsReservationWizard

Partial Class wizards_Reservations_Up4ForceConfirmation
    Inherits System.Web.UI.Page
    Private package_base As New Base_Package
    Private wiz As New Wizard

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_data = CType(ph.FindControl("LB_WIZ_DATA"), Label)

        If Session("wizData" + Session("wizGuid")) <> Nothing Then
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))
        Else
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(wiz_data.Text)
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Session("wizGuid") = wiz.GUID_TIMESTAMP

        With New clsReservations
            .ReservationID = wiz.Reservation.ReservationID
            .Load()
            tourID.Text = .TourID
        End With
        reservationID.Text = wiz.Reservation.ReservationID
        Dim tour_time = ""
        Dim tour_date As DateTime = DateTime.Now

        With New clsTour
            .TourID = tourID.Text
            .Load()
            tour_time = .TourTime
            tour_date = .TourDate
        End With

        With New clsComboItems
            .ID = tour_time
            .Load()
            tour_time = .Description
        End With
        tourDate.Text = String.Format("{0} - {1}", tour_date.ToShortDateString,
                   tour_time)

    End Sub
    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_data = CType(ph.FindControl("LB_WIZ_DATA"), Label)
        wiz_data.Text = String.Empty
        wiz = New Wizard()
        wiz.GUID_TIMESTAMP = DateTime.Now.Ticks.ToString()
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Session("wizGuid") = wiz.GUID_TIMESTAMP
        Response.Redirect("Default.aspx")
    End Sub
End Class
