
<%
	Dim cn
	Dim rs
	Dim totalDue
	totalDue = 0
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	
DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

	cn.Open DBName, DBUser, DBPass
	
	if request("Function") = "go_Report" then
		sAns = "<H2>Busch Gardens Invoice " & request("sDate") & " - " & request("eDate") & "</H2>"
		sAns = sAns & "<table>"
		sAns = sAns & "<tr>"
		sAns = sAns & "<th><u>ReservationID</u></th>"
		sAns = sAns & "<th><u>Guest</u></th>"
		sAns = sAns & "<th><u>CheckIn</u></th>"
		sAns = sAns & "<th><u>Nights</u></th>"
		sAns = sAns & "<th><u>Reservation Status</u></th>"
'		sAns = sANs & "<th><u>TourID</u></th>"
'		sAns = sAns & "<th><u>Tour Status</u></th>"
'		sAns = sAns & "<th><u>Contract Number</u></th>"
'		sAns = sAns & "<th><u>Sales Volume</u></th>"
		sAns = sAns & "<th><u>Balance Due</u></th>"
		sAns = sAns & "<th><u>Room Size</u></th>"
		sAns = sAns & "</tr>"	
		rs.Open "SELECT a.ReservationID, p.FirstName, p.LastName, a.CheckInDate, a.CheckOutDate, DateDiff(dd, a.CheckInDate, a.CheckOutDate) as Nights, c.ComboItem AS ResStatus, b.TourID, d.ComboItem AS TourStatus, s.ContractNumber, m.SalesVolume,(SELECT Case When SUM(Amount) is null then 0 else Sum(Amount) end  FROM v_Payments V WHERE V.KEYVALUE = a.ReservationID AND V.KEYFIELD = 'RESERVATIONID') AS BalanceDue, (SELECT SUM(Cast(g.Rooms AS integer)) FROM (SELECT DISTINCT rm.RoomID, LEFT(rt.ComboItem, 1) AS Rooms FROM t_RoomAllocationMatrix rm INNER JOIN t_Room r ON rm.RoomID = r.RoomID INNER JOIN t_ComboItems rt ON r.TypeID = rt.ComboItemID WHERE ReservationID = a.ReservationID AND rm.DateAllocated = a.CheckInDate) g) AS RoomSize FROM t_Reservations a LEFT OUTER JOIN (SELECT * FROM t_Tour WHERE subTypeid <> (SELECT comboitemid FROM t_comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID WHERE comboname = 'TourSubType' AND comboitem = 'Exit') AND tourlocationid = (SELECT comboitemid FROM t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID WHERE comboname = 'TourLocation' AND comboitem = 'KCP')) b ON a.ProspectID = b.ProspectID LEFT OUTER JOIN t_Prospect p ON a.ProspectID = p.ProspectID LEFT OUTER JOIN t_ComboItems c ON a.StatusID = c.ComboItemID LEFT OUTER JOIN t_ComboItems d ON b.StatusID = d.ComboItemID LEFT OUTER JOIN t_Contract s ON s.TourID = b.TourID LEFT OUTER JOIN t_Mortgage m ON m.ContractID = s.ContractID WHERE (a.SourceID = (SELECT comboitemid FROM t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID WHERE comboname = 'ReservationSource' AND comboitem = 'BG')) and a.CheckOutDate between '" & request("sDate") & "' and '" & request("edate") & "'", cn, 0, 1
		If rs.EOF and rs.BOF then	
			sAns = sAns & "<tr><td colspan = '7'>No Reservations in Date Range</td></tr>"
		Else
			Do while not rs.EOF
				sAns = sAns & "<tr>"
				sAns = sAns & "<td>" & rs.Fields("ReservationID") & "</td>"
				sAns = sAns & "<td>" & rs.Fields("LastName") & ", " & rs.Fields("Firstname") & "</td>"
				sAns = sAns & "<td>" & rs.Fields("CheckInDate") & "</td>"
				sAns = sAns & "<td>" & rs.Fields("Nights") & "</td>"
				sAns = sAns & "<td>" & rs.Fields("ResStatus") & "</td>"
'				sAns = sAns & "<td>" & rs.Fields("TourID") & "</td>"
'				sAns = sAns & "<td>" & rs.Fields("TourStatus") & "</td>"
'				sAns = sAns & "<td>" & rs.Fields("ContractNumber") & "</td>"
'				If IsNull(rs.Fields("Contractnumber")) Then
'					sAns = sAns & "<td>" & rs.Fields("SalesVolume") & "</td>"
'				Else
'					sAns = sAns & "<td>" & FormatCurrency(rs.Fields("SalesVolume"), 2) & "</td>"
'				End If
				sAns = sAns & "<td align = 'center'>" & FormatCurrency(rs.Fields("BalanceDue"), 2) & "</td>"
				sAns = sAns & "<td align = 'center'>" & rs.Fields("RoomSize") & "</td>"
				totalDue = totalDue + rs.Fields("BalanceDue")
				sAns = sAns & "</tr>"
				rs.MoveNext
			Loop
		End If
		rs.Close
		sAns = sAns & "<tr><td colspan = '7' align = 'right'><B>Total Balance Due: " & FormatCurrency(totalDue, 2) & "</B></td></tr>"	
		sAns = sAns & "</table>"
	end if
	
	cn.Close
	set rs = Nothing
	set cn = Nothing
	response.write sAns
%>
		
		
		