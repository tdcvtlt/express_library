
Partial Class Reports_CustomerService_vstTourInvoice
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        litReport.Text = String.Empty
        show_report()

    End Sub


    Private Sub show_report()
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
        sANs = "<H2>VST Tour Invoice " & sdate & " - " & edate & "</H2>"
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
        rs.Open("SELECT k.*, l.FirstName AS Firstname, l.LastName AS Lastname FROM (SELECT i.*, j.ContractNumber AS ContractNumber FROM (SELECT g.*, h.ComboItem AS ReservationLocation FROM (SELECT e.*, f.ComboItem AS ResStatus FROM (SELECT c.*,  d .CheckInDate AS CheckInDate, d.DateBooked as DateBooked, d .CheckOutDate AS CheckOutDate, d .StatusID AS StatusID, d .ResLocationID AS ResLocationID, DateDiff(d, CheckInDate, CheckOutDate) As ResNights FROM (SELECT a.TourDate, a.TourID, a.ReservationID, (select comboitem from t_comboItems where comboitemID = a.TourLocationID) [TourLocation], a.ProspectID, coalesce(b.ComboItem, '') AS TourStatus FROM t_Tour a LEFT OUTER JOIN t_ComboItems b ON a.StatusID = b.ComboItemID WHERE (a.CampaignID in (SELECT campaignid FROM t_Campaign c WHERE c.Name = 'VST') AND a.tourdate between '" & sdate & "' and '" & edate & "' AND a.SubTypeID NOT IN (SELECT c.comboitemid FROM t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID WHERE co.comboname = 'TourSubType' AND c.comboitem = 'Exit'))) c LEFT OUTER JOIN t_Reservations d ON c.ReservationID = d.ReservationID) e LEFT OUTER JOIN t_ComboItems f ON e.StatusID = f.ComboItemID) g LEFT OUTER JOIN t_ComboItems h ON g.ResLocationID = h.ComboItemID) i LEFT OUTER JOIN t_Contract j ON i.TourID = j.TourID) k LEFT OUTER JOIN t_Prospect l ON k.ProspectID = l.ProspectID order by tourdate asc", cn, 3, 3)

        If rs.BOF And rs.EOF Then
            sANs = sANs & "<tr><td colspan = '8'>No Tours In This Date Range</td></tr>"
            litReport.Text = sANs
            Return
        End If

        Do While Not rs.EOF

            If IsDBNull(rs.Fields("reservationLocation")) = False Then
                If (String.Compare(rs.Fields("reservationLocation").Value.ToString().ToUpper(), "KCP") = 0 Or _
                    String.Compare(rs.Fields("reservationLocation").Value.ToString().ToUpper(), "WILLIAMSBURG") = 0) Then

                    sANs = sANs & "<tr>"
                    sANs = sANs & "<td>" & rs.Fields("TourID").Value & "</td>"
                    sANs = sANs & "<td>" & rs.Fields("TourDate").Value & "</td>"
                    sANs = sANs & "<td>" & rs.Fields("TourStatus").Value & "</td>"
                    sANs = sANs & "<td>" & rs.Fields("FirstName").Value & " " & rs.Fields("LastName").Value & "</td>"
                    sANs = sANs & "<td>" & rs.Fields("DateBooked").Value & "</td>"
                    sANs = sANs & "<td>" & rs.Fields("ResStatus").Value & "</td>"

                    If String.Compare(rs.Fields("reservationLocation").Value.ToString().ToUpper(), "WILLIAMSBURG") = 0 Then

                        rs2.OPen("Select c.*, d.ComboItem as RoomType from (Select b.AccomName As Accommodation, a.RoomTypeID, a.ArrivalDate, a.DepartureDate, DateDiff(d, a.ArrivalDate, a.DepartureDate) As Nights, Month(a.ArrivalDate) As Month from t_Accommodation a inner join t_Accom b on a.AccomID = b.Accomid where a.reservationid = '" & rs.Fields("ReservationID").Value & "') c left outer join t_ComboItems d on c.RoomTypeID = d.ComboItemID", cn, 3, 3)

                        If rs2.EOF And rs2.BOF Then
                            sANs = sANs & "<td></td>"
                            sANs = sANs & "<td>0</td>"
                            AccomCost = 0
                        Else
                            sANs = sANs & "<td>" & rs2.Fields("Accommodation").Value & "</td>"
                            sANs = sANs & "<td>" & rs.Fields("RESNights").Value & "</td>"


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
                                rs2.Open("Select Sum(p.Amount) as CBAmount from t_Invoices i inner join t_Payment2Invoice pi on i.InvoiceID = pi.InvoiceID inner join t_Payments p on pi.PaymentID = p.PaymentID inner join t_COmboItems pm on p.MethodID = pm.ComboItemID where i.KeyField = 'ReservationID' and i.KeyValue = " & rs.Fields("ReservationID").Value & " and pm.ComboItem = 'Chargeback - VST'", cn, 3, 3)

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

                    'Response.Write(String.Format("<br/>{0}", rs.Fields("reservationLocation").Value.ToString()))

                End If

            End If
            rs.MoveNext()

        Loop

        sANs = sANs & "<tr>"
        sANs = sANs & "<td colspan = 10 align = right><B>Grand Total:</B></td>"
        sANs = sANs & "<td align = right><B>" & Replace(Replace(FormatCurrency(grandTotal), "(", "-"), ")", "") & "</B></td>"
        sANs = sANs & "</tr>"

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
        Response.AddHeader("Content-Disposition", "attachment; filename=vstTourInvoice.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub
End Class
