﻿
Partial Class marketing_EditTeeTime
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            For i = 1 To 10
                ddGuests.Items.Add(New ListItem(i, i))
            Next
            For i = 1 To 12
                ddHour.Items.Add(New ListItem(i, i))
            Next
            For i = 0 To 59
                If i < 10 Then
                    ddMinutes.Items.Add(New ListItem("0" & i, i))
                Else
                    ddMinutes.Items.Add(New ListItem(i, i))
                End If
            Next
            ddAMPM.Items.Add(New ListItem("AM", "AM"))
            ddAMPM.Items.Add(New ListItem("PM", "PM"))

            Dim oGolf As New clsGolfCourse
            ddCourse.DataSource = oGolf.List_Active_Courses(Request("ID"))
            ddCourse.DataTextField = "Course"
            ddCourse.DataValueField = "ID"
            ddCourse.DataBind()
            oGolf = Nothing

            siStatus.Connection_String = Resources.Resource.cns
            siStatus.Label_Caption = ""
            siStatus.ComboItem = "TeeTimeStatus"
            siStatus.Load_Items()

            Dim oGolf2Res As New clsRes2Golf
            oGolf2Res.Res2GolfID = Request("ID")
            oGolf2Res.Load()
            ddCourse.SelectedValue = oGolf2Res.GolfID
            ddGuests.SelectedValue = oGolf2Res.Guests
            siStatus.Selected_ID = oGolf2Res.StatusID
            dteDate.Selected_Date = oGolf2Res.GolfDate
            txtPhone.Text = oGolf2Res.ContactNumber
            txtCXLContact.Text = oGolf2Res.CancelContact
            txtBookingContact.Text = oGolf2Res.BookingContact
            If Request("ID") = 0 Then
                ddMinutes.SelectedValue = 0
                ddHour.SelectedValue = 12
                ddAMPM.SelectedValue = "PM"
            Else
                If oGolf2Res.TeeTime < 100 Then
                    ddAMPM.SelectedValue = "AM"
                    ddHour.SelectedValue = 12
                ElseIf oGOlf2Res.TeeTime > 99 And oGolf2Res.TeeTime < 1200 Then
                    ddAMPM.SelectedValue = "AM"
                    ddHour.SelectedValue = (oGolf2Res.TeeTime - Right(oGolf2Res.TeeTime, 2)) / 100
                ElseIf oGolf2Res.TeeTime > 1200 And oGolf2Res.TeeTime < 1300 Then
                    ddAMPM.SelectedValue = "PM"
                    ddHour.SelectedValue = 12
                Else
                    ddAMPM.SelectedValue = "PM"
                    ddHour.SelectedValue = (oGolf2Res.TeeTime - (1200 + Right(oGolf2Res.TeeTime, 2))) / 100
                End If
                ddMinutes.SelectedValue = Right(oGolf2Res.TeeTime, 2)
            End If
            oGolf2Res = Nothing
        End If
    End Sub


    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim oCombo As New clsComboItems
        If oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Cancelled" And txtCXLContact.Text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "alert", "alert('Please Enter Who Cancelled the Tee Time at the Golf Course.');", True)
        Else
            Dim oGolf2Res As New clsRes2Golf
            oGolf2Res.UserID = Session("UserDBID")
            oGolf2Res.Res2GolfID = Request("ID")
            oGolf2Res.Load()
            oGolf2Res.GolfID = ddCourse.SelectedValue
            oGolf2Res.Guests = ddGuests.SelectedValue
            oGolf2Res.GolfDate = CDate(dteDate.Selected_Date).ToShortDateString
            oGolf2Res.ContactNumber = txtPhone.Text
            oGolf2Res.CancelContact = txtCXLContact.Text
            oGolf2Res.BookingContact = txtBookingContact.Text
            If oGolf2Res.StatusID <> siStatus.Selected_ID And Request("ID") > 0 Then
                oGolf2Res.StatusDate = System.DateTime.Now
            End If
            If oCombo.Lookup_Comboitem(siStatus.Selected_ID) = "No Show" Then
                If Request("ID") = 0 Or (oCombo.Lookup_ComboItem(oGolf2Res.StatusID) <> "No Show") Then
                    Dim oGOlf As New clsGolfCourse
                    oGOlf.GolfID = ddCourse.SelectedValue
                    oGOlf.Load
                    Dim oinvoice As New clsInvoices
                    oinvoice.KeyField = "ReservationID"
                    If Request("ID") = 0 Then
                        oinvoice.Keyvalue = Request("ReservationID")
                    Else
                        oinvoice.KeyValue = oGolf2Res.ReservationID
                    End If
                    oinvoice.FinTransID = oGOlf.NoShowInvoiceID
                    oinvoice.Amount = ddGuests.SelectedValue * oGOlf.NoShowInvoiceCost
                    oinvoice.UserID = Session("UserDBID")
                    oinvoice.TransDate = System.DateTime.Now.ToShortDateString
                    oinvoice.DueDate = System.DateTime.Now.AddDays(30).ToShortDateString
                    oinvoice.Reference = "No Show Golf AutoGenerated"
                    oinvoice.Adjustment = False
                    oinvoice.PosNeg = False
                    Dim oRes As New clsReservations
                    If Request("ID") = 0 Then
                        oRes.ReservationID = Request("ReservationID")
                    Else
                        oRes.ReservationID = oGolf2Res.ReservationID
                    End If
                    oRes.Load
                    oinvoice.ProspectID = oRes.ProspectID
                    oRes = Nothing
                    oinvoice.save
                    oinvoice = Nothing
                    oGOlf = Nothing
                End If
            End If
            oGolf2Res.StatusID = siStatus.Selected_ID
            If ddHour.SelectedValue = 12 Then
                If ddAMPM.SelectedValue = "AM" Then
                    oGolf2Res.TeeTime = ddMinutes.SelectedValue
                Else
                    oGolf2Res.TeeTime = (ddHour.SelectedValue * 100) + ddMinutes.SelectedValue
                End If
            Else
                If ddAMPM.SelectedValue = "PM" Then
                    oGolf2Res.TeeTime = ((ddHour.SelectedValue * 100) + ddMinutes.SelectedValue) + 1200
                Else
                    oGolf2Res.TeeTime = (ddHour.SelectedValue * 100) + ddMinutes.SelectedValue
                End If
            End If
            If Request("ID") = 0 Then
                oGolf2Res.ReservationID = Request("ReservationID")
                oGolf2Res.CreatedByID = Session("UserDBID")
                oGolf2Res.DateCreated = System.DateTime.Now
                oGolf2Res.StatusDate = System.DateTime.Now
            End If
            oGolf2Res.Save()
            oGolf2Res = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.Refresh_Golf();window.close();", True)
        End If
        oCombo = Nothing
    End Sub
End Class
