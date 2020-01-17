
Partial Class Reports_CustomerService_VTE_DYDInvoice
    Inherits System.Web.UI.Page



    Private Sub Old_Code()
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        Dim rs3 As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")
        rs3 = Server.CreateObject("ADODB.Recordset")
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim AccomCost As Double = 0
        Dim sANs As String = ""
        Dim BD As Integer = 0
        Dim counter As Integer = 0
        Dim premCost As Double = 0
        Dim tourFee As Double = 0
        Dim tourCost As Double = 0
        Dim grandTotal As Double = 0
        Dim unitType As String = ""
        sANs = "<H2>VTE-DYD Invoice " & sdate & " - " & edate & "</H2>"
        sANs = sANs & "<table>"
        sANs = sANs & "<tr>"
        sANs = sANs & "<th><u>TourID</u></th>"
        sANs = sANs & "<th><u>TourDate</u></th>"
        sANs = sANs & "<th><u>TourStatus</u></th>"
        sANs = sANs & "<th><u>Prospect</u></th>"
        sANs = sANs & "<th><u>Date Booked</u></th>"
        sANs = sANs & "<th><u>Reservation Status</u></th>"
        sANs = sANs & "<th><u>Accommodations</u></th>"
        sANs = sANs & "<th><u>Nights</u></th>"
        sANs = sANs & "<th><u>Accomm. Cost</u></th>"
        sANs = sANs & "<th><u>Gifts</u></th>"
        sANs = sANs & "<th><u>Gift Cost</u></th>"
        sANs = sANs & "<th><u>Tour Fee</u></th>"
        sANs = sANs & "<th><u>Tour Total</u></th>"
        sANs = sANs & "</tr>"


        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0
        rs.Open("SELECT k.*, l.FirstName AS Firstname, l.LastName AS Lastname FROM (SELECT i.*, j.ContractNumber AS ContractNumber FROM (SELECT g.*, h.ComboItem AS ReservationLocation FROM (SELECT e.*, f.ComboItem AS ResStatus FROM (SELECT c.*, d .ReservationID AS ReservationID, d .CheckInDate AS CheckInDate, d.DateBooked as DateBooked, d .CheckOutDate AS CheckOutDate, d .StatusID AS StatusID, d .ResLocationID AS ResLocationID, DateDiff(d, CheckInDate, CheckOutDate) As ResNights FROM (SELECT a.TourDate, a.TourID, a.ProspectID, b.ComboItem AS TourStatus FROM t_Tour a LEFT OUTER JOIN t_ComboItems b ON a.StatusID = b.ComboItemID WHERE (a.CampaignID = (SELECT campaignid FROM t_Campaign WHERE name = 'VTE-DYD') AND a.tourdate between '" & sdate & "' and '" & edate & "' AND a.SubTypeID NOT IN (SELECT c.comboitemid FROM t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID WHERE co.comboname = 'TourSubType' AND c.comboitem = 'Exit'))) c LEFT OUTER JOIN t_Reservations d ON c.TourID = d .TourID) e LEFT OUTER JOIN t_ComboItems f ON e.StatusID = f.ComboItemID) g LEFT OUTER JOIN t_ComboItems h ON g.ResLocationID = h.ComboItemID) i LEFT OUTER JOIN t_Contract j ON i.TourID = j.TourID) k LEFT OUTER JOIN t_Prospect l ON k.ProspectID = l.ProspectID order by tourdate asc", cn, 3, 3)
        If rs.EOF And rs.BOF Then
            sANs = sANs & "<tr><td colspan = '8'>No Tours In This Date Range</td></tr>"
        Else
            Do While Not rs.EOF
                sANs = sANs & "<tr>"
                sANs = sANs & "<td>" & rs.Fields("TourID").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("TourDate").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("TourStatus").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("FirstName").Value & " " & rs.Fields("LastName").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("DateBooked").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("ResStatus").Value & "</td>"
                If rs.Fields("ReservationLocation").Value & "" = "Williamsburg" Then
                    rs2.OPen("Select c.*, d.ComboItem as RoomType from (Select b.ComboItem As Accommodation, a.RoomTypeID, a.ArrivalDate, a.DepartureDate, DateDiff(d, a.ArrivalDate, a.DepartureDate) As Nights, Month(a.ArrivalDate) As Month from t_Accommodation a inner join t_ComboItems b on a.LodgingID = b.comboitemid where a.reservationid = '" & rs.Fields("ReservationID").Value & "') c left outer join t_ComboItems d on c.RoomTypeID = d.ComboItemID", cn, 3, 3)
                    If rs2.EOF And rs2.BOF Then
                        sANs = sANs & "<td></td>"
                        sANs = sANs & "<td>0</td>"
                        AccomCost = 0
                    Else
                        sANs = sANs & "<td>" & rs2.Fields("Accommodation").Value & "</td>"
                        sANs = sANs & "<td>" & rs2.Fields("Nights").Value & "</td>"
                        sdate = CDate(rs2.Fields("ArrivalDate").Value)
                        Do While CDate(sdate) < CDate(rs2.Fields("DepartureDate").Value)
                            If rs2.Fields("Accommodation").Value = "Days Inn East" Then
                                If Month(sdate) = 1 Or Month(sdate) = 2 Or (Month(sdate) = 3 And Day(sdate) < 26) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 39
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 39
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf (Month(sdate) = 3 And Day(sdate) > 25) Or (Month(sdate) = 4 And Day(sdate) < 2) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 69
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 69
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf (Month(sdate) = 4 And Day(sdate) > 1) Or (Month(sdate) = 5 And Day(sdate) < 28) Or (Month(sdate) = 5 And Day(sdate) = 31) Or (Month(sdate) = 6 And Day(sdate) < 25) Or (Month(sdate) = 9 And (Day(sdate) = 1 Or Day(sdate) = 2)) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 49
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 49
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 5 And (Day(sdate) > 27 And Day(sdate) < 31) Or (Month(sdate) = 9 And (Day(sdate) > 2 And Day(sdate) < 6)) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 99
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 99
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf (Month(sdate) = 6 And Day(sdate) > 24) Or Month(sdate) = 8 Or (Month(sdate) = 7 And (Day(sdate) = 1 Or Day(sdate) > 4)) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 85
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 85
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 7 And (Day(sdate) > 1 And Day(sdate) < 5) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 109
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 109
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 10 Or Month(sdate) = 11 Or Month(sdate) = 12 Or (Month(sdate) = 9 And Day(sdate) > 5) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 49
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 49
                                    Else
                                        AccomCost = 0
                                    End If
                                Else
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 99
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 99
                                    Else
                                        AccomCost = 0
                                    End If
                                End If
                            ElseIf rs2.Fields("Accommodation").Value = "Days Inn Historic" Then
                                If Month(sdate) = 1 Or Month(sdate) = 2 Or (Month(sdate) = 3 And Day(sdate) < 26) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 39
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 39
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf (Month(sdate) = 3 And Day(sdate) > 25) Or (Month(sdate) = 4 And Day(sdate) < 2) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 69
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 69
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf (Month(sdate) = 4 And Day(sdate) > 1) Or (Month(sdate) = 5 And Day(sdate) < 28) Or (Month(sdate) = 5 And Day(sdate) = 31) Or (Month(sdate) = 6 And Day(sdate) < 25) Or (Month(sdate) = 9 And (Day(sdate) = 1 Or Day(sdate) = 2)) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 49
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 49
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 5 And (Day(sdate) > 27 And Day(sdate) < 31) Or (Month(sdate) = 9 And (Day(sdate) > 2 And Day(sdate) < 6)) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 99
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 99
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf (Month(sdate) = 6 And Day(sdate) > 24) Or Month(sdate) = 8 Or (Month(sdate) = 7 And (Day(sdate) = 1 Or Day(sdate) > 4)) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 79
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 79
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 7 And (Day(sdate) > 1 And Day(sdate) < 5) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 109
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 109
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 10 Or Month(sdate) = 11 Or Month(sdate) = 12 Or (Month(sdate) = 9 And Day(sdate) > 5) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 49
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 49
                                    Else
                                        AccomCost = 0
                                    End If
                                Else
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 99
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 99
                                    Else
                                        AccomCost = 0
                                    End If
                                End If
                            ElseIf rs2.Fields("Accommodation").Value = "WBG - Best Western" Then
                                If Month(sdate) = 1 Or Month(sdate) = 2 Or (Month(sdate) = 3 And Day(sdate) < 26) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 39
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 39
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf (Month(sdate) = 3 And Day(sdate) > 25) Or (Month(sdate) = 4 And Day(sdate) < 2) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 79
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 79
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf (Month(sdate) = 4 And Day(sdate) > 1) Or (Month(sdate) = 5 And Day(sdate) < 28) Or (Month(sdate) = 5 And Day(sdate) = 31) Or (Month(sdate) = 6 And Day(sdate) < 25) Or (Month(sdate) = 9 And (Day(sdate) = 1 Or Day(sdate) = 2)) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 49
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 49
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 5 And (Day(sdate) > 27 And Day(sdate) < 31) Or (Month(sdate) = 9 And (Day(sdate) > 2 And Day(sdate) < 6)) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 109
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 109
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf (Month(sdate) = 6 And Day(sdate) > 24) Or Month(sdate) = 8 Or (Month(sdate) = 7 And (Day(sdate) = 1 Or Day(sdate) > 4)) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 85
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 85
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 7 And (Day(sdate) > 1 And Day(sdate) < 5) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 119
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 119
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 10 Or Month(sdate) = 11 Or Month(sdate) = 12 Or (Month(sdate) = 9 And Day(sdate) > 5) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 49
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 49
                                    Else
                                        AccomCost = 0
                                    End If
                                Else
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 99
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 99
                                    Else
                                        AccomCost = 0
                                    End If
                                End If
                            ElseIf rs2.Fields("Accommodation").Value = "WBG - Springhill Suites" Then
                                If Month(sdate) = 1 Or Month(sdate) = 2 Or (Month(sdate) = 3 And Day(sdate) < 21) Then
                                    If Weekday(sdate) > 4 Then
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 74
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 74
                                        Else
                                            AccomCost = 0
                                        End If
                                    Else
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 74
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 74
                                        Else
                                            AccomCost = 0
                                        End If
                                    End If
                                ElseIf (Month(sdate) = 3 And Day(sdate) > 20) Or (Month(sdate) = 4 And Day(sdate) < 14) Then
                                    If Weekday(sdate) > 4 Then
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 97
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 97
                                        Else
                                            AccomCost = 0
                                        End If
                                    Else
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 74
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 74
                                        Else
                                            AccomCost = 0
                                        End If
                                    End If
                                ElseIf (Month(sdate) = 4 And Day(sdate) > 13) Or Month(sdate) = 5 Or (Month(sdate) = 6 And Day(sdate) < 16) Then
                                    If Weekday(sdate) > 4 Then
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 97
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 97
                                        Else
                                            AccomCost = 0
                                        End If
                                    Else
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 97
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 97
                                        Else
                                            AccomCost = 0
                                        End If
                                    End If
                                ElseIf (Month(sdate) = 6 And Day(sdate) > 15) Or Month(sdate) = 7 Or Month(sdate) = 8 Then
                                    If Weekday(sdate) > 4 Then
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 124
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 124
                                        Else
                                            AccomCost = 0
                                        End If
                                    Else
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 124
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 124
                                        Else
                                            AccomCost = 0
                                        End If
                                    End If
                                ElseIf Month(sdate) = 9 Or Month(sdate) = 10 Or (Month(sdate) = 11 And Day(sdate) < 3) Then
                                    If Weekday(sdate) > 4 Then
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 97
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 97
                                        Else
                                            AccomCost = 0
                                        End If
                                    Else
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 97
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 97
                                        Else
                                            AccomCost = 0
                                        End If
                                    End If
                                Else
                                    If Weekday(sdate) > 4 Then
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 74
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 74
                                        Else
                                            AccomCost = 0
                                        End If
                                    Else
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 74
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 74
                                        Else
                                            AccomCost = 0
                                        End If
                                    End If
                                End If
                            ElseIf rs2.Fields("Accommodation").Value = "WBG - Residence Inn" Then
                                If Month(sdate) = 1 Or Month(sdate) = 2 Or (Month(sdate) = 3 And Day(sdate) < 21) Then
                                    If Weekday(sdate) > 4 Then
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 82
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 82
                                        Else
                                            AccomCost = 0
                                        End If
                                    Else
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 82
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 82
                                        Else
                                            AccomCost = 0
                                        End If
                                    End If
                                ElseIf (Month(sdate) = 3 And Day(sdate) > 20) Or (Month(sdate) = 4 And Day(sdate) < 14) Then
                                    If Weekday(sdate) > 4 Then
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 112
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 112
                                        Else
                                            AccomCost = 0
                                        End If
                                    Else
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 82
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 82
                                        Else
                                            AccomCost = 0
                                        End If
                                    End If
                                ElseIf (Month(sdate) = 4 And Day(sdate) > 13) Or Month(sdate) = 5 Or (Month(sdate) = 6 And Day(sdate) < 16) Then
                                    If Weekday(sdate) > 4 Then
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 114
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 114
                                        Else
                                            AccomCost = 0
                                        End If
                                    Else
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 114
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 114
                                        Else
                                            AccomCost = 0
                                        End If
                                    End If
                                ElseIf (Month(sdate) = 6 And Day(sdate) > 15) Or Month(sdate) = 7 Or Month(sdate) = 8 Then
                                    If Weekday(sdate) > 4 Then
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 144
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 144
                                        Else
                                            AccomCost = 0
                                        End If
                                    Else
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 144
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 144
                                        Else
                                            AccomCost = 0
                                        End If
                                    End If
                                ElseIf Month(sdate) = 9 Or Month(sdate) = 10 Or (Month(sdate) = 11 And Day(sdate) < 3) Then
                                    If Weekday(sdate) > 4 Then
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 112
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 112
                                        Else
                                            AccomCost = 0
                                        End If
                                    Else
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 112
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 112
                                        Else
                                            AccomCost = 0
                                        End If
                                    End If
                                Else
                                    If Weekday(sdate) > 4 Then
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 82
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 82
                                        Else
                                            AccomCost = 0
                                        End If
                                    Else
                                        If rs.Fields("ResStatus").Value = "Completed" Then
                                            AccomCost = AccomCost + 82
                                        ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                            AccomCost = 82
                                        Else
                                            AccomCost = 0
                                        End If
                                    End If
                                End If
                            ElseIf rs2.Fields("Accommodation").Value = "WBG - Fairfield Inn" Then
                                If Month(sdate) = 1 Or Month(sdate) = 2 Or Month(sdate) = 3 Then
                                    If rs2.Fields("RoomType").Value & "" = "Suite" Then
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 85
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 85
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 75
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 75
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    Else
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 65
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 65
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 55
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 55
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    End If
                                ElseIf Month(sdate) = 4 Or Month(sdate) = 5 Or Month(sdate) = 6 Then
                                    If rs2.Fields("RoomType").Value & "" = "Suite" Then
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 112
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 112
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 102
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 102
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    Else
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 92
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 92
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 82
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 82
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    End If
                                ElseIf Month(sdate) = 7 Or Month(sdate) = 8 Or (Month(sdate) = 9 And Day(sdate) < 7) Then
                                    If rs2.Fields("RoomType").Value & "" = "Suite" Then
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 127
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 127
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 114
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 114
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    Else
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 107
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 107
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 94
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 94
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    End If
                                Else
                                    If rs2.Fields("RoomType").Value & "" = "Suite" Then
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 99
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 89
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 89
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    Else
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 79
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 79
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 69
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 69
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    End If
                                End If
                            ElseIf rs2.Fields("Accommodation").Value = "Clarion" Then
                                If Month(sdate) = 1 Or Month(sdate) = 2 Or (Month(sdate) = 3 And Day(sdate) < 15) Then
                                    If rs2.Fields("RoomType").Value & "" = "Suite" Then
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 69.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 69.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 59.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 59.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    Else
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 49.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 49.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 39.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 39.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    End If
                                ElseIf (Month(sdate) = 3 And Day(sdate) > 14) Or Month(sdate) = 4 Or (Month(sdate) = 5 And Day(sdate) < 21) Then
                                    If rs2.Fields("RoomType").Value & "" = "Suite" Then
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 74.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 74.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 64.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 64.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    Else
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 59.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 59.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 49.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 49.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    End If
                                ElseIf (Month(sdate) = 5 And Day(sdate) > 20) Or Month(sdate) = 6 Or Month(sdate) = 7 Or Month(sdate) = 8 Or (Month(sdate) = 9 And Day(sdate) = 1) Then
                                    If rs2.Fields("RoomType").Value & "" = "Suite" Then
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 109.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 109.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 89.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 89.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    Else
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 99.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 99.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 69.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 69.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    End If
                                Else
                                    If rs2.Fields("RoomType").Value & "" = "Suite" Then
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 74.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 74.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 64.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 64.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    Else
                                        If Weekday(sdate) > 4 Then
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 59.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 59.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        Else
                                            If rs.Fields("ResStatus").Value = "Completed" Then
                                                AccomCost = AccomCost + 49.99
                                            ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                                AccomCost = 49.99
                                            Else
                                                AccomCost = 0
                                            End If
                                        End If
                                    End If
                                End If
                            Else
                                If Month(sdate) = 1 Or Month(sdate) = 2 Or Month(sdate) = 3 Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 39
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 39
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 4 And (Day(sdate) < 3 Or Day(sdate) > 18) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 49
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 49
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 4 And (Day(sdate) > 2 Or Day(sdate) < 19) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 69
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 69
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 5 And (Day(sdate) < 22 Or Day(sdate) > 24) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 49
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 49
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 5 And (Day(sdate) > 21 Or Day(sdate) < 25) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 99
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 99
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 6 Or Month(sdate) = 8 Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 79
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 79
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 7 And (Day(sdate) < 2 Or Day(sdate) > 5) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 79
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 79
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 7 And (Day(sdate) > 1 Or Day(sdate) < 6) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 109
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 109
                                    Else
                                        AccomCost = 0
                                    End If
                                ElseIf Month(sdate) = 9 And (Day(sdate) > 3 Or Day(sdate) < 7) Then
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 99
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 99
                                    Else
                                        AccomCost = 0
                                    End If
                                Else
                                    If rs.Fields("ResStatus").Value = "Completed" Then
                                        AccomCost = AccomCost + 49
                                    ElseIf rs.Fields("ResStatus").Value = "No Show" Then
                                        AccomCost = 49
                                    Else
                                        AccomCost = 0
                                    End If
                                End If
                            End If
                            sdate = CDate(sdate).AddDays(1)
                        Loop
                    End If
                    sANs = sANs & "<td>" & FormatCurrency(AccomCost) & "</td>"
                    rs2.Close()
                Else
                    rs2.Open("SELECT g.RoomType, h.ComboItem AS UnitType FROM (SELECT e.*, f.TypeID AS UnitTypeID FROM (SELECT c.*, d .ComboItem AS RoomType FROM (SELECT DISTINCT a.RoomID, b.TypeID, b.UnitID FROM t_RoomAllocationMatrix a INNER JOIN t_Room b ON a.RoomID = b.RoomID WHERE (a.ReservationID = '" & rs.Fields("ReservationID").Value & "') and a.DateAllocated = '" & rs.Fields("CheckInDate").Value & "') c LEFT OUTER JOIN t_ComboItems d ON c.TypeID = d .ComboItemID) e LEFT OUTER JOIN t_Unit f ON e.UnitID = f.UnitID) g LEFT OUTER JOIN t_ComboItems h ON g.UnitTypeID = h.ComboItemID", cn, 3, 3)
                    If rs2.EOF And rs2.BOF Then
                        sANs = sANs & "<td></td>"
                        sANs = sANs & "<td>0</td>"
                        AccomCost = 0
                    Else
                        BD = 0
                        unitType = rs2.Fields("UnitType").Value
                        Do While Not rs2.EOF
                            BD = BD + Left(rs2.Fields("RoomType").Value, 1)
                            'if rs2.Fields("RoomType") = "1BD" and rs2.Fields("UnitType") = "Cottage" then
                            '	If rs.Fields("ResStatus") = "No Show" then
                            '		AccomCost = AccomCost + 79
                            '	Else
                            '		AccomCost = AccomCost + (rs.Fields("ResNights") * 79)
                            '	End If
                            'elseif rs2.Fields("RoomType") = "1BD-DWN" then
                            '	If rs.Fields("ResStatus") = "No Show" then
                            '		AccomCost = AccomCost + 99
                            '	Else
                            '		AccomCost = AccomCost + (rs.fields("ResNights") * 99)
                            '	End If
                            'elseif rs2.Fields("RoomType") = "1BD-UP" then
                            '	If rs.Fields("ResStatus") = "No Show" then
                            '		AccomCost = AccomCost + 119
                            '	Else
                            '		AccomCost = AccomCost + (rs.Fields("ResNights") * 119)
                            '	End IF
                            'elseif rs2.Fields("RoomType") = "2BD" and (rs2.Fields("UnitType") = "Cottage" or rs2.Fields("UnitType") = "Townes") then
                            '	If rs.Fields("ResStatus") = "No Show" then	
                            '		AccomCost = AccomCost + 129
                            '	Else							
                            '		AccomCost = AccomCost + (rs.Fields("ResNights") * 129)
                            '	End If
                            'elseif rs2.Fields("RoomType") = "2BD" and rs2.Fields("UnitType") = "Estates" then
                            '	If rs.Fields("ResStatus") = "No Show" then
                            '		AccomCost = AccomCost + 169
                            '	Else
                            '		AccomCost = AccomCost + (rs.Fields("ResNights") * 169)
                            '	End If
                            'end if
                            rs2.MoveNExt()
                        Loop
                        sANs = sANs & "<td>" & BD & "BD " & unitType & "</td>"
                        sANs = sANs & "<td>" & rs.Fields("ResNights").Value & "</td>"
                        If rs.Fields("ResStatus").Value <> "Completed" And rs.Fields("ResStatus").Value <> "No Show" Then
                            AccomCost = 0
                        Else
                            rs2.Close()
                            rs2.Open("Select Sum(p.Amount) as CBAmount from t_Invoices i inner join t_Payment2Invoice pi on i.InvoiceID = pi.InvoiceID inner join t_Payments p on pi.PaymentID = p.PaymentID inner join t_COmboItems pm on p.MethodID = pm.ComboItemID where i.KeyField = 'ReservationID' and i.KeyValue = " & rs.Fields("ReservationID").value & " and pm.ComboItem = 'Chargeback - VTE'", cn, 3, 3)
                            'rs2.Open("Select Sum(Amount) as CBAmount from t_AccountItems where paymentmethodid = (Select comboitemid from t_ComboItems where comboname = 'PaymentMethod' and comboitem = 'ChargeBack - VTE-DYD') and reservationid = '" & rs.FIelds("ReservationID") & "'", cn, 3, 3)
                            If (rs2.EOF And rs2.BOF) Or IsDBNull(rs2.Fields("CBAmount").Value) Then
                                AccomCost = 0
                            Else
                                AccomCost = rs2.Fields("CBAmount").Value
                            End If
                        End If
                    End If
                    rs2.Close()
                    sANs = sANs & "<td>" & FormatCurrency(AccomCost) & "</td>"
                End If

                sANs = sANs & "<td>"
                counter = 0
                premCost = 0
                rs2.Open("Select a.TotalCost, b.PremiumName from t_PremiumIssued a inner join t_Premium b on a.premiumid = b.premiumid where a.KeyField = 'TourID' and a.KeyValue = '" & rs.Fields("TourID").value & "' and a.statusid in (Select c.comboitemid from t_ComboItems c inner join t_Combos co on c.COmboID = co.CombOID where co.comboname = 'PremiumStatus' and (c.comboitem = 'Issued' or c.comboitem = 'Prepared'))", cn, 3, 3)
                If rs2.EOF And rs2.BOF Then
                    premCost = 0
                Else
                    counter = 0
                    Do While Not rs2.EOF
                        If counter = 0 Then
                            sANs = sANs & rs2.Fields("PremiumName").Value
                        Else
                            sANs = sANs & "<br>" & rs2.Fields("PremiumName").Value
                        End If
                        premCost = premCost + rs2.Fields("TotalCost").Value
                        counter = counter + 1
                        rs2.MoveNext()
                    Loop
                End If
                rs2.Close()
                sANs = sANs & "</td>"
                sANs = sANs & "<td>" & FormatCurrency(premCost) & "</td>"
                If rs.Fields("TourStatus").Value = "No Tour - Overage" Or rs.Fields("TourStatus").Value = "Showed" Or rs.Fields("TourStatus").Value = "OnTour" Or (rs.Fields("TourStatus").Value = "NQ - Toured" And Not (IsDBNull(rs.Fields("ContractNumber").Value))) Then
                    tourFee = 375
                Else
                    tourFee = 0
                End If
                sANs = sANs & "<td align = right>" & FormatCurrency(tourFee) & "</td>"
                tourCost = tourFee - AccomCost - premCost
                sANs = sANs & "<td align = right>" & Replace(Replace(FormatCurrency(tourCost), "(", "-"), ")", "") & "</td>"
                grandTotal = grandTotal + tourCost
                sANs = sANs & "</tr>"
                tourFee = 0
                AccomCost = 0
                premCost = 0
                rs.MoveNext()
            Loop
            sANs = sANs & "<tr>"
            sANs = sANs & "<td colspan = 10 align = right><B>Grand Total:</B></td>"
            sANs = sANs & "<td align = right><B>" & Replace(Replace(FormatCurrency(grandTotal), "(", "-"), ")", "") & "</B></td>"
            sANs = sANs & "</tr>"
        End If
        rs.Close()
        sANs = sANs & "</table>"
        cn.Close()
        cn = Nothing
        rs = Nothing
        rs2 = Nothing
        litReport.Text = sANs
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        Dim rs3 As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")
        rs3 = Server.CreateObject("ADODB.Recordset")
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim AccomCost As Double = 0
        Dim sANs As String = ""
        Dim BD As Integer = 0
        Dim counter As Integer = 0
        Dim premCost As Double = 0
        Dim tourFee As Double = 0
        Dim tourCost As Double = 0
        Dim grandTotal As Double = 0
        Dim unitType As String = ""
        sANs = "<H2>VTE-DYD Invoice " & sdate & " - " & edate & "</H2>"
        sANs = sANs & "<table>"
        sANs = sANs & "<tr>"
        sANs = sANs & "<th><u>TourID</u></th>"
        sANs = sANs & "<th><u>TourDate</u></th>"
        sANs = sANs & "<th><u>TourStatus</u></th>"
        sANs = sANs & "<th><u>Prospect</u></th>"
        sANs = sANs & "<th><u>Date Booked</u></th>"
        sANs = sANs & "<th><u>Reservation Status</u></th>"
        sANs = sANs & "<th><u>Accommodations</u></th>"
        sANs = sANs & "<th><u>Nights</u></th>"
        sANs = sANs & "<th><u>Accomm. Cost</u></th>"
        sANs = sANs & "<th><u>Gifts</u></th>"
        sANs = sANs & "<th><u>Gift Cost</u></th>"
        sANs = sANs & "<th><u>Tour Fee</u></th>"
        sANs = sANs & "<th><u>Tour Total</u></th>"
        sANs = sANs & "</tr>"


        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0
        rs.Open("SELECT k.*, l.FirstName AS Firstname, l.LastName AS Lastname FROM (SELECT i.*, j.ContractNumber AS ContractNumber FROM (SELECT g.*, h.ComboItem AS ReservationLocation FROM (SELECT e.*, f.ComboItem AS ResStatus FROM (SELECT c.*, d .ReservationID AS ReservationID, d .CheckInDate AS CheckInDate, d.DateBooked as DateBooked, d .CheckOutDate AS CheckOutDate, d .StatusID AS StatusID, d .ResLocationID AS ResLocationID, DateDiff(d, CheckInDate, CheckOutDate) As ResNights FROM (SELECT a.TourDate, a.TourID, a.ProspectID, b.ComboItem AS TourStatus FROM t_Tour a LEFT OUTER JOIN t_ComboItems b ON a.StatusID = b.ComboItemID WHERE (a.CampaignID = (SELECT campaignid FROM t_Campaign WHERE name = 'VTE-DYD') AND a.tourdate between '" & sdate & "' and '" & edate & "' AND a.SubTypeID NOT IN (SELECT c.comboitemid FROM t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID WHERE co.comboname = 'TourSubType' AND c.comboitem = 'Exit'))) c LEFT OUTER JOIN t_Reservations d ON c.TourID = d .TourID) e LEFT OUTER JOIN t_ComboItems f ON e.StatusID = f.ComboItemID) g LEFT OUTER JOIN t_ComboItems h ON g.ResLocationID = h.ComboItemID) i LEFT OUTER JOIN t_Contract j ON i.TourID = j.TourID) k LEFT OUTER JOIN t_Prospect l ON k.ProspectID = l.ProspectID order by tourdate asc", cn, 3, 3)
        If rs.EOF And rs.BOF Then
            sANs = sANs & "<tr><td colspan = '8'>No Tours In This Date Range</td></tr>"
        Else
            Do While Not rs.EOF
                sANs = sANs & "<tr>"
                sANs = sANs & "<td>" & rs.Fields("TourID").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("TourDate").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("TourStatus").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("FirstName").Value & " " & rs.Fields("LastName").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("DateBooked").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("ResStatus").Value & "</td>"

                If rs.Fields("ReservationLocation").Value & "" = "Williamsburg" Then

                    rs2.OPen("Select c.*, d.ComboItem as RoomType from (Select b.ComboItem As Accommodation, a.RoomTypeID, a.ArrivalDate, a.DepartureDate, DateDiff(d, a.ArrivalDate, a.DepartureDate) As Nights, Month(a.ArrivalDate) As Month from t_Accommodation a inner join t_ComboItems b on a.LodgingID = b.comboitemid where a.reservationid = '" & rs.Fields("ReservationID").Value & "') c left outer join t_ComboItems d on c.RoomTypeID = d.ComboItemID", cn, 3, 3)

                    If rs2.EOF And rs2.BOF Then
                        sANs = sANs & "<td></td>"
                        sANs = sANs & "<td>0</td>"
                        AccomCost = 0
                    Else
                        sANs = sANs & "<td>" & rs2.Fields("Accommodation").Value & "</td>"
                        sANs = sANs & "<td>" & rs.Fields("ResNights").Value & "</td>"

                        Dim r_cost As Object
                        r_cost = Server.CreateObject("ADODB.Recordset")

                        Try
                            r_cost.Open("Select Sum(p.Amount) as CBAmount from t_Invoices i inner join t_Payment2Invoice pi on i.InvoiceID = pi.InvoiceID inner join t_Payments p on pi.PaymentID = p.PaymentID inner join t_COmboItems pm on p.MethodID = pm.ComboItemID where i.KeyField = 'ReservationID' and i.KeyValue = " & rs.Fields("ReservationID").Value & " and i.fintransid = 489", cn, 3, 3)
                            If (r_cost.EOF And r_cost.BOF) Or IsDBNull(r_cost.Fields("CBAmount").Value) Then
                                AccomCost = 0
                            Else
                                AccomCost = r_cost.Fields("CBAmount").Value
                            End If
                        Catch ex As Exception
                            Response.Write(ex.Message)
                        End Try

                        r_cost.Close()


                    End If
                    sANs = sANs & "<td>" & FormatCurrency(AccomCost) & "</td>"
                    rs2.Close()

                Else

                    rs2.Open("SELECT g.RoomType, h.ComboItem AS UnitType FROM (SELECT e.*, f.TypeID AS UnitTypeID FROM (SELECT c.*, d .ComboItem AS RoomType FROM (SELECT DISTINCT a.RoomID, b.TypeID, b.UnitID FROM t_RoomAllocationMatrix a INNER JOIN t_Room b ON a.RoomID = b.RoomID WHERE (a.ReservationID = '" & rs.Fields("ReservationID").Value & "') and a.DateAllocated = '" & rs.Fields("CheckInDate").Value & "') c LEFT OUTER JOIN t_ComboItems d ON c.TypeID = d .ComboItemID) e LEFT OUTER JOIN t_Unit f ON e.UnitID = f.UnitID) g LEFT OUTER JOIN t_ComboItems h ON g.UnitTypeID = h.ComboItemID", cn, 3, 3)
                    If rs2.EOF And rs2.BOF Then
                        sANs = sANs & "<td></td>"
                        sANs = sANs & "<td>0</td>"
                        AccomCost = 0
                    Else
                        BD = 0
                        unitType = rs2.Fields("UnitType").Value
                        Do While Not rs2.EOF
                            BD = BD + Left(rs2.Fields("RoomType").Value, 1)                           
                            rs2.MoveNExt()
                        Loop
                        sANs = sANs & "<td>" & BD & "BD " & unitType & "</td>"
                        sANs = sANs & "<td>" & rs.Fields("ResNights").Value & "</td>"

                        If rs.Fields("ResStatus").Value <> "Completed" And rs.Fields("ResStatus").Value <> "No Show" Then
                            AccomCost = 0
                        Else
                            rs2.Close()
                            rs2.Open("Select Sum(p.Amount) as CBAmount from t_Invoices i inner join t_Payment2Invoice pi on i.InvoiceID = pi.InvoiceID inner join t_Payments p on pi.PaymentID = p.PaymentID inner join t_COmboItems pm on p.MethodID = pm.ComboItemID where i.KeyField = 'ReservationID' and i.KeyValue = " & rs.Fields("ReservationID").value & " and pm.ComboItem = 'Chargeback - VTE'", cn, 3, 3)
                            If (rs2.EOF And rs2.BOF) Or IsDBNull(rs2.Fields("CBAmount").Value) Then
                                AccomCost = 0
                            Else
                                AccomCost = rs2.Fields("CBAmount").Value
                            End If
                        End If
                    End If
                    rs2.Close()
                    sANs = sANs & "<td>" & FormatCurrency(AccomCost) & "</td>"
                End If

                sANs = sANs & "<td>"
                counter = 0
                premCost = 0
                rs2.Open("Select a.TotalCost, b.PremiumName from t_PremiumIssued a inner join t_Premium b on a.premiumid = b.premiumid where a.KeyField = 'TourID' and a.KeyValue = '" & rs.Fields("TourID").value & "' and a.statusid in (Select c.comboitemid from t_ComboItems c inner join t_Combos co on c.COmboID = co.CombOID where co.comboname = 'PremiumStatus' and (c.comboitem = 'Issued' or c.comboitem = 'Prepared'))", cn, 3, 3)
                If rs2.EOF And rs2.BOF Then
                    premCost = 0
                Else
                    counter = 0
                    Do While Not rs2.EOF
                        If counter = 0 Then
                            sANs = sANs & rs2.Fields("PremiumName").Value
                        Else
                            sANs = sANs & "<br>" & rs2.Fields("PremiumName").Value
                        End If
                        premCost = premCost + rs2.Fields("TotalCost").Value
                        counter = counter + 1
                        rs2.MoveNext()
                    Loop
                End If
                rs2.Close()
                sANs = sANs & "</td>"
                sANs = sANs & "<td>" & FormatCurrency(premCost) & "</td>"
                If rs.Fields("TourStatus").Value = "No Tour - Overage" Or rs.Fields("TourStatus").Value = "Showed" Or rs.Fields("TourStatus").Value = "OnTour" Or (rs.Fields("TourStatus").Value = "NQ - Toured" And Not (IsDBNull(rs.Fields("ContractNumber").Value))) Then
                    tourFee = 375
                Else
                    tourFee = 0
                End If
                sANs = sANs & "<td align = right>" & FormatCurrency(tourFee) & "</td>"
                tourCost = tourFee - AccomCost - premCost
                sANs = sANs & "<td align = right>" & Replace(Replace(FormatCurrency(tourCost), "(", "-"), ")", "") & "</td>"
                grandTotal = grandTotal + tourCost
                sANs = sANs & "</tr>"
                tourFee = 0
                AccomCost = 0
                premCost = 0
                rs.MoveNext()
            Loop
            sANs = sANs & "<tr>"
            sANs = sANs & "<td colspan = 10 align = right><B>Grand Total:</B></td>"
            sANs = sANs & "<td align = right><B>" & Replace(Replace(FormatCurrency(grandTotal), "(", "-"), ")", "") & "</B></td>"
            sANs = sANs & "</tr>"
        End If
        rs.Close()
        sANs = sANs & "</table>"
        cn.Close()
        cn = Nothing
        rs = Nothing
        rs2 = Nothing
        litReport.Text = sANs

    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=VTE-DYDInvoice.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub
End Class
