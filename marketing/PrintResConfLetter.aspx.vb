
Partial Class marketing_PrintResConfLetter
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oRes As New clsReservations
            Dim oLetter As New clsReservationLetters
            Dim body As String = ""
            Dim oPros As New clsProspect
            Dim oAdd As New clsAddress
            Dim oCombo As New clsComboItems
            Dim oAccom As New clsAccom
            Dim oAcc As New clsAccommodation
            Dim oTour As New clsTour
            Dim size As Integer = 0
            Dim rmSize As String = ""
            oRes.ReservationID = Request("ResID")
            oRes.Load()
            oPros.Prospect_ID = oRes.ProspectID
            oPros.Load()
            oAdd.AddressID = oAdd.Get_Address_ID(oPros.Prospect_ID)
            oAdd.Load()
            oLetter.ReservationLetterID = Request("LetterID")
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
                body = Replace(body, "<ACCOMDIRECTIONS>", "")
                body = Replace(body, "<CHECKINDIRECTIONS>", "")
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
                If oCombo.Lookup_ComboItem(oTour.StatusID) = "Booked" Then
                    body = Replace(body, "<TOURDATE>", CDate(oTour.TourDate).ToShortDateString)
                    Dim tTime As String
                    tTime = oCombo.Lookup_ComboItem(oTour.TourTime)
                    If CInt(tTime) > 1259 Then
                        tTime = CInt(tTime) - 1200 & " PM"
                    ElseIf CInt(tTime) > 1159 And CInt(tTime) <= 1259 Then
                        tTime = CInt(tTime) & " PM"
                    Else
                        tTime = tTime & " AM"
                    End If
                    body = Replace(body, "<TOURTIME>", tTime)
                Else
                    body = Replace(body, "<TOURDATE>", "N/A")
                    body = Replace(body, "<TOURTIME>", "N/A")
                End If

                Dim oPremIss As New clsPremiumIssued
                body = Replace(body, "<GIFTS>", oPremIss.Get_Gifts(oTour.TourID))
                oPremIss = Nothing
            End If
            body = body & "</body></html>"
            oAdd = Nothing
            oTour = Nothing
            oCombo = Nothing
            oAccom = Nothing
            oAcc = Nothing
            oPros = Nothing
            Label1.Text = body
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Print", "window.print();", True)
        End If
    End Sub
End Class
