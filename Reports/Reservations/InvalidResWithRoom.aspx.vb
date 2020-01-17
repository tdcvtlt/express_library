
Partial Class Reports_Reservations_InvalidResWithRoom
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        Dim sAns As String = ""
        Dim i As Integer = 0
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")
        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0

        rs.Open("SELECT g.ReservationID, g.FirstName, g.LastName, g.CheckInDate, g.CheckOutDate, g.Type, h.ComboItem AS Status FROM (SELECT e.*, f.ComboItem AS Type FROM (SELECT c.*, d .FirstName AS FirstName, d .LastName AS LastName FROM (Select TypeID,StatusID,CheckInDate,CheckOUtDate,ReservationID,ProspectID from t_Reservations where reservationid in (SELECT DISTINCT a.ReservationID FROM t_Reservations a INNER JOIN t_RoomAllocationMatrix b ON a.ReservationID = b.ReservationID WHERE (a.StatusID NOT IN (SELECT c.comboitemid FROM t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID WHERE co.comboname = 'ReservationStatus' AND (c.comboitem = 'Booked' OR c.comboitem = 'In-House' OR c.comboitem = 'Completed' OR c.comboitem = 'Pending Payment'))) OR (a.StatusID IS NULL))) c LEFT OUTER JOIN t_Prospect d ON c.ProspectID = d .ProspectID) e LEFT OUTER JOIN t_ComboItems f ON e.typeid = f.comboitemid) g LEFT OUTER JOIN t_ComboItems h ON g.StatusID = h.ComboItemID where g.CheckInDate >= getDate() order by g.CheckInDate", cn, 3, 3)
        If rs.EOF And rs.BOF Then
            sAns = "No Invalid Reservations to Report At this Time."
        Else
            sAns = "<table>"
            sANs = sANs & "<tr><th>ReservationID</th><th>Prospect</th><th>CheckIn</th><th>CheckOut</th><th>Type</th><th>Status</th><th>Room(s)</th></tr>"
            Do While Not rs.EOF
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><a href = '../../editReservation.aspx?reservationid=" & rs.Fields("ReservationID").Value & "'>" & rs.Fields("ReservationID").Value & "</a></td>"
                sAns = sAns & "<td>" & rs.Fields("Lastname").Value & ", " & rs.Fields("FIrstName").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("CheckInDate").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("CheckOUtDate").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("Type").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("Status").Value & "</td>"
                rs2.Open("Select Distinct(b.RoomNumber) from t_RoomAllocationMatrix a inner join t_Room b on a.roomid = b.roomid where a.reservationid = '" & rs.Fields("reservationID").Value & "'", cn, 3, 3)
                sAns = sAns & "<td>"
                i = 0
                Do While Not rs2.EOF
                    If i > 0 Then
                        sAns = sAns & "<br>"
                    End If
                    sAns = sAns & rs2.Fields("RoomNumber").Value
                    i = i + 1
                    rs2.MoveNext()
                Loop
                sANs = sANs & "</td>"
                rs2.Close()
                sAns = sAns & "</tr>"
                rs.MoveNExt()
            Loop
            sANs = sANs & "</table>"
        End If
        rs.Close()

        cn.Close()
        rs = Nothing
        rs2 = Nothing
        cn = Nothing
        litReport.Text = sAns

    End Sub
End Class
