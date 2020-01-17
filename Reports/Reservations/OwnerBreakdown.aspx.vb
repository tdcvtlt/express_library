Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System
Partial Class Reports_Reservations_OwnerBreakdown
    Inherits System.Web.UI.Page


    Protected Sub Unnamed1_Click1(sender As Object, e As System.EventArgs)
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        Dim rs3 As Object
        Dim amtCollected As Double = 0
        Dim amtReceived As Double = 0
        Dim numNights As Integer = 0
        Dim amtPerNight As Double = 0
        Dim amtForUsage As Double = 0
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")
        rs3 = Server.CreateObject("ADODB.Recordset")

        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date

        Dim sql As String = ""
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")

        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0
        rs.Open("Select g.*, h.ComboItem as Category from (Select e.*, f.FirstName, f.LastName, (Select Top 1 Number from t_ProspectPhone where prospectid = f.ProspectID and active = 1) as HomePhone from (Select c.*, d.ProspectID, d.ContractNumber from (Select a.UsageID, a.ContractID, a.InDate, a.OutDate, a.AmountPromised, a.CategoryID, a.UsageYear, b.ComboItem as Status, rt.comboitem as RoomType, ut.comboitem as UnitType from t_usage a left outer join t_ComboItems b on a.statusid = b.comboitemid left outer join t_comboitems rt on rt.comboitemid = a.roomtypeid left outer join t_comboitems ut on ut.comboitemid = a.unittypeid where a.InDate between '" & sdate & "' and '" & edate & "' and a.typeid = (Select c.comboitemid from t_ComboItems c inner join t_Combos co on c.ComboID = co.COmboID where co.comboname = 'ReservationType' and c.comboitem = 'Rental')) c inner join t_Contract d on c.contractid = d.contractid) e inner join t_Prospect f on e.prospectid = f.prospectid) g left outer join t_ComboItems h on g.categoryid = h.comboitemid order by g.inDate, g.Status, g.LastName", cn, 3, 3)
        If rs.EOF And rs.BOF Then
            litReport.Text = "NO RECORDS FOUND IN THIS DATE RANGE."
        Else
            litReport.Text = "<table>"
            litReport.Text = litReport.Text & "<tr><th>Owner</th><th>HomePhone</th><th>KCP #</th><th>Usage Year</th><th>Room Type</th><th>Unit Type</th><th>Status</th><th>InDate</th><th>OutDate</th><th>Category</th><th>Nights Rented</th><th>Nights Avail</th><th>Amt Promised</th><th>Amt Collected</th><th>Balance Left For Collection</th><th>Reservations</th><th>Amt For Reservation</th></tr>"
            Do While Not rs.EOF
                litReport.Text = litReport.Text & "<tr>"
                litReport.Text = litReport.Text & "<td>" & rs.Fields("LastName").Value & ", " & rs.Fields("FirstName").Value & "</td>"
                litReport.Text = litReport.Text & "<td>" & rs.Fields("HomePhone").Value & "</td>"
                litReport.Text = litReport.Text & "<td>" & rs.Fields("ContractNumber").Value & "</td>"
                litReport.Text = litReport.Text & "<td>" & rs.Fields("UsageYear").Value & "</td>"
                litReport.Text = litReport.Text & "<td>" & rs.Fields("RoomType").Value & "</td>"
                litReport.Text = litReport.Text & "<td>" & rs.Fields("UnitType").Value & "</td>"
                litReport.Text = litReport.Text & "<td>" & rs.Fields("Status").Value & "</td>"
                litReport.Text = litReport.Text & "<td>" & rs.Fields("InDate").Value & "</td>"
                litReport.Text = litReport.Text & "<td>" & rs.Fields("OutDate").Value & "</td>"
                litReport.Text = litReport.Text & "<td>" & rs.Fields("Category").Value & "</td>"
                rs2.Open("Select count (*) as total from t_RoomAllocationMatrix where usageID = '" & rs.Fields("UsageID").Value & "' and reservationID > '0'", cn, 3, 3)
                litReport.Text = litReport.Text & "<td>" & rs2.Fields("total").Value & "</td>"
                rs2.Close()
                rs2.Open("Select count (*) as total from t_RoomAllocationMatrix where usageID = '" & rs.Fields("UsageID").Value & "' and (reservationid is null or reservationID <= '0')", cn, 3, 3)
                litReport.Text = litReport.Text & "<td>" & rs2.Fields("Total").Value & "</td>"
                rs2.Close()
                litReport.Text = litReport.Text & "<td>" & FormatCurrency(rs.Fields("AmountPromised").Value) & "</td>"
                amtCollected = 0
                rs2.Open("Select Distinct(ReservationID), Count(ReservationID) as NightsPerUsage from t_RoomALlocationMatrix where usageID = '" & rs.Fields("UsageID").Value & "' group by reservationid", cn, 3, 3)
                Do While Not rs2.EOF
                    If Not (IsDBNull(rs2.Fields("ReservationID").Value)) Then
                        If rs2.Fields("reservationID").Value > 0 Then
                            'Get Total Amount Received from Reservation
                            rs3.Open("Select Sum(a.Amount) as Total, Sum(a.Balance) as Balance from ufn_Financials(0,'RESERVATIONID'," & rs2.Fields("ReservationID").Value & ",0) a inner join t_invoices i on a.ID = i.invoiceid inner join t_FInTransCodes f on i.Fintransid = f.fintransid where f.RoomCharge = 1", cn, 3, 3)
                            If IsDBNull(rs3.Fields("Total").Value) And IsDBNull(rs3.Fields("Balance").Value) Then
                                amtReceived = 0
                            Else
                                amtReceived = rs3.Fields("Total").Value - rs3.Fields("Balance").Value
                            End If
                            rs3.Close()
                            'Get Number of Nights of Reservation
                            rs3.Open("Select DateDiff(d, CheckInDate, CheckOutDate) As Nights from t_Reservations where reservationid = '" & rs2.Fields("reservationID").Value & "'", cn, 3, 3)
                            If IsDBNull(rs3.Fields("Nights").Value) Then
                                numNights = 0
                            Else
                                numNights = rs3.Fields("Nights").Value
                            End If
                            rs3.Close()
                            'Calculate AmtCollected Per Night of Reservation
                            If numNights = 0 Then
                                amtPerNight = 0
                            Else
                                amtPerNight = amtReceived / numNights
                            End If
                            'Calculate amount collected that goes towards owners usage
                            amtForUsage = amtPerNight * rs2.Fields("NightsPerUsage").Value
                            'Update amtCollected
                            amtCollected = amtCollected + amtForUsage
                        End If
                    End If
                    rs2.MoveNext()
                Loop
                rs2.Close()
                litReport.Text = litReport.Text & "<td>" & Replace(Replace(FormatCurrency(amtCollected), "(", "-"), ")", "") & "</td>"
                litReport.Text = litReport.Text & "<td>" & Replace(Replace(FormatCurrency(rs.Fields("AmountPromised").Value - amtCollected), "(", "-"), ")", "") & "</td>"
                rs2.Open("Select c.*, d.FirstName As FirstName, d.LastName as LastName from (Select a.*, b.ProspectID from (Select Distinct(ReservationID), (Select Sum(Amount) as Amt from t_Invoices where UPPER(KeyField) = 'RESERVATIONID' and keyvalue = r.reservationid) as Amt from t_RoomAllocationMatrix r where r.usageid = '" & rs.Fields("UsageID").Value & "' and reservationid > 0) a inner join t_Reservations b on a.reservationid = b.reservationid) c left outer join t_Prospect d on c.ProspectID = d.ProspectiD", cn, 3, 3)
                If rs2.EOF And rs2.BOF Then
                    litReport.Text = litReport.Text & "<td>N/A</td><td>$0.00</td>"
                Else
                    litReport.Text = litReport.Text & "<td>"
                    Do While Not rs2.EOF
                        litReport.Text = litReport.Text & rs2.Fields("ReservationID").Value & " " & rs2.Fields("FirstName").Value & " " & rs2.FIelds("Lastname").Value & "<br>"
                        rs2.MoveNext()
                    Loop
                    litReport.Text = litReport.Text & "</td>"
                    rs2.MoveFirst()
                    litReport.Text = litReport.Text & "<td>"
                    Do While Not rs2.EOF
                        If IsDBNull(rs2.Fields("Amt").Value) Then
                            litReport.Text = litReport.Text & "$0.00<br>"
                        Else
                            litReport.Text = litReport.Text & FormatCurrency(rs2.Fields("Amt").Value * 1) & "<br>"
                        End If
                        rs2.Movenext()
                    Loop
                    litReport.Text = litReport.Text & "</td>"
                End If
                rs2.Close()
                litReport.Text = litReport.Text & "</tr>"
                rs.MoveNext()
            Loop
            litReport.Text = litReport.Text & "</table>"
        End If
        rs.Close()
        cn.Close()
        rs = Nothing
        rs2 = Nothing
        rs3 = Nothing
        cn = Nothing
    End Sub
End Class
