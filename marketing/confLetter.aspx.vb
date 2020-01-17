Imports Microsoft.Office
Imports Microsoft.Office.Core
Imports Microsoft.Office.Interop.Word
'Imports Microsoft.Office.Tools.Word
Imports Microsoft.VisualBasic
Partial Class marketing_confLetter
    Inherits System.Web.UI.Page

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        '        Dim wordApp As New Microsoft.Interop.Word.Application
        '       Dim wordDoc As New Microsoft.Interop.Word.Document
        '      '        wordApp = Server.CreateObject("Word.Application")
        '       wordDoc = Server.CreateObject("Word.Document")
        '     wordDoc = wordApp.Documents.Open(rbLetters.SelectedItem.Value)

        Try
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Scripting", Get_Script, True)
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Execute", "setTimeout('mPrinting()',1000);", True)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

    Private Function Get_Script() As String
        Dim guest As String = ""
        Dim address1 As String = ""
        Dim address2 As String = ""
        Dim tTime As String = ""
        Dim tDate As String = ""
        Dim source As String = ""
        Dim inDate As String = ""
        Dim outDate As String = ""
        Dim gifts As String = ""
        Dim hotel As String = ""
        Dim days As Integer = 0
        Dim nights As Integer = 0
        Dim size As Integer = 0
        Dim rmSize As String = ""
        Dim total As Double = 0
        Dim dep As Double = 0
        Dim owed As Double = 0
        Dim oRes As New clsReservations
        Dim oPros As New clsProspect
        Dim oAdd As New clsAddress
        Dim oCombo As New clsComboItems
        Dim oTour As New clsTour
        Dim oPremIss As New clsPremiumIssued
        Dim oAccom As New clsAccommodation


        Try

            oRes.ReservationID = Request("ReservationID")
            oRes.Load()
            oPros.Prospect_ID = oRes.ProspectID
            oPros.Load()
            guest = oPros.First_Name & " " & oPros.Last_Name.Replace("'", "")
            If oAdd.Get_Address_ID(oRes.ProspectID) = 0 Then
                address1 = "N/A"
                address2 = "N/A"
            Else
                oAdd.AddressID = oAdd.Get_Address_ID(oPros.Prospect_ID)
                oAdd.Load()
                address1 = oAdd.Address1
                address2 = oAdd.City & ", " & oCombo.Lookup_ComboItem(oAdd.StateID) & " " & oAdd.PostalCode
            End If
            If oRes.SourceID = 0 Then
                source = "N/A"
            Else
                source = oCombo.Lookup_ComboItem(oRes.SourceID)
            End If
            If oRes.TourID > 0 Then
                oTour.TourID = oRes.TourID
                oTour.Load()
                If oCombo.Lookup_ComboItem(oTour.StatusID) = "OpenEnded" Then
                    tDate = "N/A"
                    tTime = "N/A"
                Else
                    tDate = CDate(oTour.TourDate).ToShortDateString
                    tTime = oCombo.Lookup_ComboItem(oTour.TourTime)
                    If CInt(tTime) > 1259 Then
                        tTime = CInt(tTime) - 1200 & " PM"
                    Else
                        tTime = tTime & " AM"
                    End If
                End If
                gifts = oPremIss.Get_Gifts(oTour.TourID)
            Else
                Dim tourID As Integer = oTour.Get_Tour_By_Res(oRes.ReservationID)
                If tourID > 0 Then
                    oTour.TourID = tourID
                    oTour.Load()
                    If oCombo.Lookup_ComboItem(oTour.StatusID) = "OpenEnded" Then
                        tDate = "N/A"
                        tTime = "N/A"
                    Else
                        tDate = CDate(oTour.TourDate).ToShortDateString
                        tTime = oCombo.Lookup_ComboItem(oTour.TourTime)
                        If CInt(tTime) > 1259 Then
                            tTime = CInt(tTime) - 1200 & " PM"
                        Else
                            tTime = tTime & " AM"
                        End If
                    End If
                    gifts = oPremIss.Get_Gifts(oTour.TourID)
                Else
                    tDate = "N/A"
                    tTime = "N/A"
                    gifts = "N/A"
                End If
            End If
            size = oRes.Get_BD_Count(oRes.ReservationID)
            If size = 0 Then
                rmSize = "N/A"
            Else
                rmSize = size & "BD"
            End If
            If IsDBNull(oRes.CheckInDate) Or oRes.CheckInDate = "" Then
                inDate = "N/A"
                outDate = "N/A"
                days = 0
                nights = 0
            Else
                inDate = CDate(oRes.CheckInDate).ToShortDateString
                outDate = CDate(oRes.CheckOutDate).ToShortDateString
                days = DateDiff(DateInterval.Day, CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate).AddDays(1))
                nights = DateDiff(DateInterval.Day, CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate))
            End If
            hotel = oAccom.Get_Accom_Name(oRes.ReservationID)
            owed = oRes.Get_Total_Balance(oRes.ReservationID)
            dep = oRes.Get_Total_Payments(oRes.ReservationID)
            total = oRes.Get_Inv_Amount(oRes.ReservationID) - oRes.Get_Total_Adjustments(oRes.ReservationID)
            oRes = Nothing
            oTour = Nothing
            oCombo = Nothing
            oAdd = Nothing
            oPremIss = Nothing
            oPros = Nothing
            oAccom = Nothing
            Dim sScript As String = ""
            sScript &= "function mPrinting(){" & vbCrLf
            sScript &= "var temp = document.getElementById('ScriptControl1');" & vbCrLf
            sScript &= "var mstatement = 'Dim wApp:';" & vbCrLf
            sScript &= "mstatement += 'set wApp = createobject(""Word.application""):';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents.open(""" & Replace(rbLetters.SelectedItem.Value, "\", "\\") & """):';" & vbCrLf
            sScript &= "mstatement += 'wApp.Visible=True:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Activate:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<LDATE>"").Value = """ & System.DateTime.Now.ToShortDateString & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<Address1>"").Value = """ & address1.Replace("'", "\'") & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<Address2>"").Value = """ & address2.Replace("'", "\'") & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<GUEST>"").Value = """ & guest & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<RESID>"").Value = """ & Request("ReservationID") & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<TOTAL>"").Value = """ & FormatCurrency(total, 2) & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<OWED>"").Value = """ & FormatCurrency(owed, 2) & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<DEP>"").Value = """ & FormatCurrency(dep, 2) & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<SIZE>"").Value = """ & rmSize & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<CHECKIN>"").Value = """ & inDate & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<CHECKOUT>"").Value = """ & outDate & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<TOURDATE>"").Value = """ & tDate & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<TOURTIME>"").Value = """ & tTime & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<DAY>"").Value = """ & days & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<NIGHT>"").Value = """ & nights & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<GIFTS>"").Value = """ & gifts & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<SOURCE>"").Value = """ & source & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Variables(""<HOTEL>"").Value = """ & hotel & """:';" & vbCrLf
            sScript &= "mstatement += 'wApp.Documents(1).Fields.Update:';" & vbCrLf
            sScript &= "mstatement += 'set wApp = nothing:';" & vbCrLf
            sScript &= "temp.ExecuteStatement(mstatement);" & vbCrLf
            sScript &= "window.close();" & vbCrLf
            sScript &= "}" & vbCrLf

            'Label1.Text = sScript

            Return sScript

        Catch ex As Exception

            Response.Write(String.Format("{0}:{1}: {2}", ex.InnerException, ex.Source, ex.StackTrace))
        Finally

        End Try

        Return String.Format("At {0} Severe Error", DateTime.Now.TimeOfDay)
    End Function
End Class
