Imports System
Imports System.Data
Imports System.Data.SqlClient

Partial Class Reports_CustomerService_DSTInvoice
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)

        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim edate As String = Me.dteEDate.Selected_Date

        Me.ErrMessage.InnerText = String.Empty

        If (String.IsNullOrEmpty(sdate) Or String.IsNullOrEmpty(edate)) Then
            Me.ErrMessage.InnerText = "Invalid Date Range"
            Return
        End If




        Dim sql As String = String.Format( _
            "Select * from (SELECT a.ReservationID, d.FirstName, d.LastName, " & _
            "b.TourID, b.TourDate, c.Name, a.Datebooked, a.CheckInDate, " & _
            "DATEDIFF(dd, a.CheckInDate, a.CheckOutDate) AS Nights, " & _
            "(Select (Sum(Amount) + Sum(Adjustment)) * -1 from v_Payments where keyfield = v.Keyfield and keyvalue = v.KeyValue and transdate between '{0}' and '{1}'  and invoice not like '%Tax' and isadjustment = 0) as  AmountPaid, SUM(v.Balance) 'Remaining', " & _
            "(Select Top 1 Number from t_ProspectPhone where prospectID = d.ProspectID and Active = 1) as HomePhone  " & _
            "FROM t_Reservations a " & _
            "LEFT OUTER JOIN t_Tour b ON a.TourID = b.TourID  " & _
            "LEFT OUTER JOIN t_Campaign c ON b.CampaignID = c.CampaignID  " & _
            "LEFT OUTER JOIN t_Prospect d ON a.ProspectID = d.ProspectID " & _
            "inner join v_invoices v on v.keyvalue = a.reservationid and v.keyfield = 'reservationid' " & _
            "and v.accountname = '~0015~' " & _
            "WHERE a.SourceID IN (SELECT c.comboitemid FROM t_comboitems c inner join t_Combos co on c.ComboID = co.ComboID " & _
            "WHERE co.comboname = 'ReservationSource' AND (c.comboitem = 'DST' or c.comboitem = 'DSTY')) " & _
            "group by a.ReservationId, d.LastName, d.FirstName, b.TourId, b.TourDate, c.Name, a.DateBooked, a.CheckInDate, a.CheckOutDate, d.ProspectId, v.Keyfield, v.KeyValue " & _
            ") xx where xx.amountpaid > 0 order by xx.reservationid ", sdate, edate)

        Dim html As New StringBuilder()

        Using cnn As New SqlConnection(Resources.Resource.cns)
            Using cmd As New SqlCommand(sql, cnn)
		
		cmd.CommandTimeout = 0
                cnn.Open()

                Dim rdr As SqlDataReader = cmd.ExecuteReader()
                Dim dic As New Dictionary(Of Integer, ArrayList)

                If rdr.HasRows = True Then
                    Do While (rdr.Read())

                        Dim arl As New ArrayList()

                        arl.Add( _
                            New With {.ReservationID = rdr("ReservationID"), _
                                            .First = IIf(rdr("FirstName").Equals(DBNull.Value), String.Empty, rdr("FirstName")), _
                                            .Last = IIf(rdr("LastName").Equals(DBNull.Value), String.Empty, rdr("LastName")), _
                                            .TourID = IIf(rdr("TourID").Equals(DBNull.Value), String.Empty, rdr("TourID")), _
                                            .TourDate = IIf(rdr("TourDate").Equals(DBNull.Value), String.Empty, rdr("TourDate")), _
                                            .Campaign = IIf(rdr("Name").Equals(DBNull.Value), String.Empty, rdr("Name")), _
                                            .DateBooked = IIf(rdr("DateBooked").Equals(DBNull.Value), String.Empty, rdr("DateBooked")), _
                                            .CheckInDate = IIf(rdr("CheckInDate").Equals(DBNull.Value), String.Empty, rdr("CheckInDate")), _
                                            .Nights = IIf(rdr("Nights").Equals(DBNull.Value), String.Empty, rdr("Nights")), _
                                            .AmountPaid = IIf(rdr("AmountPaid").Equals(DBNull.Value), String.Empty, rdr("AmountPaid")), _
                                            .Balance = IIf(rdr("Remaining").Equals(DBNull.Value), String.Empty, rdr("Remaining")), _
                                            .Phone = IIf(rdr("HomePhone").Equals(DBNull.Value), String.Empty, rdr("HomePhone"))})

                        dic.Add(Convert.ToInt32(rdr("ReservationID")), arl)
                    Loop
                End If

                rdr.Close()

                Dim header() As String = {"Name", "Phone", "Reservation #", "Amount Paid", "Balance Due", _
                                          "Check IN", "# Nights", "Unit Size", "Tour#", "Tour Date", "Date Booked"}

                html.Append("<table style='border-collapse:collapse;' border=1px><tr>")
                For Each s As String In header
                    html.AppendFormat("<td>{0}</td>", s)
                Next
                html.Append("</tr>")

                For Each kvp As KeyValuePair(Of Integer, ArrayList) In dic

                    cmd.CommandText = String.Format("Select Sum(Cast(Left(RoomType, 1) as Integer)) as BD, UnitType " & _
                                                    "from (Select Distinct(a.RoomID), d.Comboitem as RoomType, e.Comboitem as " & _
                                                    "unitType from t_RoomAllocationMatrix a inner join t_Room b on a.RoomID = b.RoomID " & _
                                                    "left outer join t_Unit c on b.UnitID = c.UnitID Left outer join t_Comboitems d " & _
                                                    "on b.TypeID = d.ComboItemID left outer join t_ComboItems e on c.TypeID = e.ComboItemID " & _
                                                    "where a.ReservationID = {0}) ee group by UnitType", kvp.Key)

                    rdr = cmd.ExecuteReader()

                    rdr.Read()

                    Dim unit As String = String.Empty

                    If rdr.HasRows = True Then
                        unit = rdr.Item("BD") & " - " & rdr.Item("UnitType")
                    End If


                    Dim ar As ArrayList = kvp.Value

                    html.AppendFormat("<tr><td>{0}</td>" & _
                                        "<td>{1}</td>" & _
                                        "<td>{2}</td>" & _
                                        "<td>{3:c}</td>" & _
                                        "<td>{4:c}</td>" & _
                                        "<td>{5:d}</td>" & _
                                        "<td>{6}</td>" & _
                                        "<td>{7}</td>" & _
                                        "<td>{8}</td>" & _
                                        "<td>{9}</td>" & _
                                        "<td>{10:d}</td></tr>", _
                                         ar(0).First & " " & ar(0).Last, ar(0).Phone, ar(0).ReservationID, _
                                        ar(0).AmountPaid, ar(0).Balance, ar(0).CheckInDate, ar(0).Nights, _
                                        unit, ar(0).TourID, ar(0).TourDate, ar(0).DateBooked)
                                        


                    rdr.Close()
                Next

                litReport.Text = html.ToString()

                cnn.Close()
            End Using
        End Using
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=DSTInvoice.xls")
        Response.Write(litReport.Text)
        Response.End()

    End Sub
End Class
