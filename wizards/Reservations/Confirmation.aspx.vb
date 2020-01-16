Imports System.Reflection
Imports System.Web.Script.Serialization
Imports clsReservationWizard

Partial Class wizard_Reservations_Prompt
    Inherits System.Web.UI.Page

    Private package_base As New Base_Package
    Private wiz As New Wizard

#Region "Event Handlers"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_data = CType(ph.FindControl("LB_WIZ_DATA"), Label)

        If Session("wizData" + Session("wizGuid")) <> Nothing Then
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))
        ElseIf wiz_data.Text.Length = 0 Then
            wiz = New Wizard()
            wiz.GUID_TIMESTAMP = DateTime.Now.Ticks.ToString()
        Else
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(wiz_data.Text)
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Session("wizGuid") = wiz.GUID_TIMESTAMP

        With New clsEmail
            spEmail.InnerHtml = String.Format("({0})", .Get_Primary_Email(wiz.Prospect.Prospect_ID))
        End With
    End Sub

    Protected Sub btFinish_Click(sender As Object, e As EventArgs) Handles btFinish.Click
        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_data = CType(ph.FindControl("LB_WIZ_DATA"), Label)
        wiz_data.Text = String.Empty
        wiz = New Wizard()
        wiz.GUID_TIMESTAMP = DateTime.Now.Ticks.ToString()
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Session("wizGuid") = wiz.GUID_TIMESTAMP
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub btSubmit_Click(sender As Object, e As EventArgs) Handles btSubmit.Click
        If cbConfEmail.Checked And cbConfPrint.Checked Then
            EmailLetter(wiz.Reservation.ReservationID)
            PrintLetter(wiz.Reservation.ReservationID)

        ElseIf cbConfEmail.Checked Then
            EmailLetter(wiz.Reservation.ReservationID)

        ElseIf cbConfPrint.Checked Then
            PrintLetter(wiz.Reservation.ReservationID)
        End If
    End Sub

#End Region
#Region "Subs/Functions"
    Private Sub Navigate(bt As Button)
        Dim gt = Me.Master.GetType()
        Dim mi As MethodInfo = gt.GetMethod("Navigate")
        Dim parameters = mi.GetParameters()

        Dim dir = Convert.ToInt32(bt.Attributes("nav"))
        Dim parametersArray() As Object = New Object() {Request.PhysicalPath.Substring(Request.PhysicalPath.LastIndexOf("\") + 1).ToLower(), dir}
        Dim r = mi.Invoke(Me.Master, parametersArray)
        Response.Redirect(r.ToString())
    End Sub
    Private Sub EmailLetter(reservationID As Int32)
        Dim oEmail As New clsEmail
        Dim oRes As New clsReservations
        Dim letterID As Integer = 0
        Dim emailTo As String = ""
        oRes.ReservationID = reservationID
        oRes.Load()
        emailTo = oEmail.Get_Primary_Email(oRes.ProspectID)
        If emailTo <> "" Then
            Dim oLetter As New clsReservationLetters
            letterID = oLetter.Find_Letter(reservationID)
            If letterID > 0 Then
                Dim body As String = ""
                Dim oPros As New clsProspect
                Dim oAdd As New clsAddress
                Dim oCombo As New clsComboItems
                Dim oAccom As New clsAccom
                Dim oAcc As New clsAccommodation
                Dim oTour As New clsTour
                Dim size As Integer = 0
                Dim rmSize As String = ""
                oPros.Prospect_ID = oRes.ProspectID
                oPros.Load()
                oAdd.AddressID = oAdd.Get_Address_ID(oPros.Prospect_ID)
                oAdd.Load()
                oLetter.ReservationLetterID = letterID
                oLetter.Load()
                body = "<html><body>" & Server.HtmlDecode(oLetter.LetterText)
                body = Replace(body, "<DATE>", DateTime.Now.ToShortDateString)
                body = Replace(body, "<NAME>", oPros.First_Name & " " & oPros.Last_Name)
                body = Replace(body, "<ADDRESS>", oAdd.Address1)
                body = Replace(body, "<CITY>", oAdd.City)
                body = Replace(body, "<STATE>", oCombo.Lookup_ComboItem(oAdd.StateID))
                body = Replace(body, "<ZIP>", oAdd.PostalCode)
                body = Replace(body, "<COUNTRY>", oCombo.Lookup_ComboItem(oAdd.CountryID))
                body = Replace(body, "<SOURCE>", oCombo.Lookup_ComboItem(oRes.SourceID))
                body = Replace(body, "<RESID>", oRes.ReservationID)
                body = Replace(body, "<RESNUMBER>", oRes.ReservationNumber)
                If IsDBNull(oRes.CheckInDate) Or oRes.CheckInDate.ToString = "" Then
                    body = Replace(body, "<CHECKIN>", "N/A")
                    body = Replace(body, "<CHECKOUT>", "N/A")
                    body = Replace(body, "<DAYS>", "N/A")
                    body = Replace(body, "<NIGHTS>", "N/A")
                Else
                    body = Replace(body, "<CHECKIN>", CDate(oRes.CheckInDate).ToShortDateString)
                    body = Replace(body, "<CHECKOUT>", CDate(oRes.CheckOutDate).ToShortDateString)
                    body = Replace(body, "<DAYS>", DateDiff(DateInterval.Day, CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate)) + 1)
                    body = Replace(body, "<NIGHTS>", DateDiff(DateInterval.Day, CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate)))
                End If
                size = oRes.Get_BD_Count(oRes.ReservationID)
                If size = 0 Then
                    rmSize = "N/A"
                Else
                    rmSize = size & "BD"
                End If
                body = Replace(body, "<SIZE>", rmSize)
                body = Replace(body, "<BALANCE>", oRes.Get_Total_Balance(oRes.ReservationID))
                body = Replace(body, "<DEPOSIT>", oRes.Get_Total_Payments(oRes.ReservationID))
                body = Replace(body, "<TOTAL>", oRes.Get_Inv_Amount(oRes.ReservationID) - oRes.Get_Total_Adjustments(oRes.ReservationID))
                If oAcc.Get_Accom_By_Res(oRes.ReservationID) = 0 Then
                    body = Replace(body, "<ACCOMSIZE>", "N/A")
                    body = Replace(body, "<ACCOMNAME>", "N/A")
                    body = Replace(body, "<ACCOMADDRESS>", "N/A")
                    body = Replace(body, "<ACCOMCITY>", "N/A")
                    body = Replace(body, "<ACCOMSTATE>", "N/A")
                    body = Replace(body, "<ACCOMZIP>", "N/A")
                    body = Replace(body, "<ACCOMDIRECTIONS>", "XX")
                    body = Replace(body, "<CHECKINDIRECTIONS>", "XX")
                Else
                    oAcc.AccommodationID = oAcc.Get_Accom_By_Res(oRes.ReservationID)
                    oAcc.Load()
                    body = Replace(body, "<ACCOMSIZE>", oCombo.Lookup_ComboItem(oAcc.RoomTypeID))
                    oAccom.AccomID = oAcc.AccomID
                    oAccom.Load()
                    body = Replace(body, "<ACCOMNAME>", oAccom.AccomName)
                    body = Replace(body, "<ACCOMADDRESS>", oAccom.Address)
                    body = Replace(body, "<ACCOMCITY>", oAccom.City)
                    body = Replace(body, "<ACCOMSTATE>", oCombo.Lookup_ComboItem(oAccom.StateID))
                    body = Replace(body, "<ACCOMZIP>", oAccom.PostalCode)
                    body = Replace(body, "<ACCOMDIRECTIONS>", Server.HtmlDecode(oAccom.Directions) & " " & oAccom.AccomID)
                    If oAcc.CheckInLocationID = 0 Then
                        body = Replace(body, "<CHECKINDIRECTIONS>", "")
                    Else
                        Dim oAccomCheckIN As New clsAccomCheckInLocations
                        If oAccomCheckIN.Lookup_By_ID(oAcc.CheckInLocationID) = 0 Then
                            body = Replace(body, "<CHECKINDIRECTIONS>", "")
                        Else
                            oAccomCheckIN.ID = oAccomCheckIN.Lookup_By_ID(oAcc.CheckInLocationID)
                            oAccomCheckIN.Load()
                            body = Replace(body, "<CHECKINDIRECTIONS>", Server.HtmlDecode(oAccomCheckIN.Directions))
                        End If
                        oAccomCheckIN = Nothing
                    End If

                End If
                If oTour.Get_Tour_By_Res(oRes.ReservationID) > 0 Then
                    oTour.TourID = oTour.Get_Tour_By_Res(oRes.ReservationID)
                    oTour.Load()
                    If IsDBNull(oTour.TourDate) Or oTour.TourDate.ToString = "" Then
                        body = Replace(body, "<TOURDATE>", "N/A")
                        body = Replace(body, "<TOURTIME>", "N/A")
                    Else
                        body = Replace(body, "<TOURDATE>", CDate(oTour.TourDate).ToShortDateString)
                        Dim tTime As String
                        tTime = oCombo.Lookup_ComboItem(oTour.TourTime)
                        If CInt(tTime) > 1259 Then
                            tTime = CInt(tTime) - 1200 & " PM"
                        ElseIf CInt(tTime) > 1159 Then
                            tTime = CInt(tTime) & " PM"
                        Else
                            tTime = tTime & " AM"
                        End If
                        body = Replace(body, "<TOURTIME>", tTime)
                    End If
                    Dim oPremIss As New clsPremiumIssued
                    body = Replace(body, "<GIFTS>", oPremIss.Get_Gifts(oTour.TourID))
                    oPremIss = Nothing
                End If
                body = body & "</body></html>"
                Send_Mail(emailTo, oLetter.EmailAddress, oLetter.Subject, body, True)
                Dim oUpload As New clsUploadedDocs
                oUpload.FileID = 0
                oUpload.Load()
                oUpload.KeyField = "ReservationID"
                oUpload.KeyValue = oRes.ReservationID
                oUpload.Name = "Confirmation Letter Sent " & System.DateTime.Now.ToShortDateString
                oUpload.DateUploaded = System.DateTime.Now
                oUpload.ContentText = body
                oUpload.UploadedByID = Session("UserDBID")
                oUpload.Save()
                oUpload = Nothing
                Dim oNote As New clsNotes
                oNote.NoteID = 0
                oNote.Load()
                oNote.UserID = Session("UserDBID")
                oNote.CreatedByID = Session("UserDBID")
                oNote.DateCreated = System.DateTime.Now
                oNote.KeyField = "ReservationID"
                oNote.KeyValue = oRes.ReservationID
                oNote.Note = "Emailed Confirmation Letter to " & emailTo & " on " & System.DateTime.Now.ToShortDateString & "."
                oNote.Save()
                oNote = Nothing
                oAdd = Nothing
                oTour = Nothing
                oCombo = Nothing
                oAccom = Nothing
                oAcc = Nothing
                oPros = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Confirmation Letter Sent.');", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Reservation Does Not Have a Confirmation Letter.');", True)
            End If
            oLetter = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Prospect Does Not Have A Primary Email Address.');", True)
        End If
        oRes = Nothing
        oEmail = Nothing
    End Sub
    Private Sub PrintLetter(reservationID As Int32)
        Dim letterID As Integer = 0
        Dim oLetter As New clsReservationLetters
        letterID = oLetter.Find_Letter(reservationID)
        If letterID > 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/PrintResConfLetter.aspx?ResID=" & reservationID & "&LetterID=" & letterID & "','win01',690,450);", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Reservation Does Not Have a Confirmation Letter.');", True)
        End If
        oLetter = Nothing
    End Sub
#End Region


End Class
