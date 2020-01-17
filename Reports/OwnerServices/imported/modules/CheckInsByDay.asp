<%
dim cn
dim rs

set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.recordset")

DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

cn.open DBName, DBUser, DBPass

if request("Function") = "RunReport" then
	rs.open "SELECT r.ReservationID, r.CheckInDate, DateDiff(dd,r.CheckInDate, r.CheckoutDate) as Nights, t.ComboItem as ResType, st.ComboItem as ResSubType, datepart(dw,r.CheckInDate) as DayNumber, m.RoomNumber, rt.ComboItem as RoomType, rst.ComboItem as RoomSubType, pros.LastName + ', ' + pros.FirstName as ProspectName, " & _
				"case " & _
						"when datepart(dw,r.CheckInDate) = '1' then " & _
							"'SUN' " & _
						"when datepart(dw,r.CheckInDate) = '2' then " & _
							"'MON' " & _
						"when datepart(dw,r.CheckIndate) = '3' then " & _
							"'TUE' " & _
						"when datepart(dw,r.CheckIndate) = '4' then " & _
							"'WED' " & _
						"when datepart(dw,r.CheckIndate) = '5' then " & _
							"'THU' " & _
						"when datepart(dw,r.CheckIndate) = '6' then " & _
							"'FRI' " & _
						"when datepart(dw,r.CheckIndate) = '7' then " & _
							"'SAT' " & _
				"end as CheckInDay " & _
			"FROM t_Reservations r " & _
			"INNER JOIN t_ComboItems t on t.ComboItemID = r.TypeID " & _
			"INNER JOIN t_ComboItems st on st.ComboItemID = r.SubTypeID " & _
			"INNER JOIN (SELECT DISTINCT ReservationID, RoomID FROM t_RoomAllocationMatrix WHERE DateAllocated between '" & request("SDate") & "' and '" & request("EDate") & "') as a on a.ReservationID = r.ReservationID " & _
			"INNER JOIN t_Room m on m.RoomID = a.RoomID " & _
			"INNER JOIN t_ComboItems rt on rt.ComboItemID = m.TypeID " & _
			"INNER JOIN t_ComboItems rst on rst.ComboItemID = m.SubTypeID " & _
			"INNER JOIN t_Prospect pros on pros.ProspectID = r.ProspectID " & _
			"WHERE t.ComboItem in ('Exchange','Owner') and r.CheckInDate between '" & request("SDate") & "' and '" & request("EDate") & "' and " & _
				"(case " & _
						"when datepart(dw,r.CheckInDate) = '1' then " & _
							"'SUN' " & _
						"when datepart(dw,r.CheckInDate) = '2' then " & _
							"'MON' " & _
						"when datepart(dw,r.CheckIndate) = '3' then " & _
							"'TUE' " & _
						"when datepart(dw,r.CheckIndate) = '4' then " & _
							"'WED' " & _
						"when datepart(dw,r.CheckIndate) = '5' then " & _
							"'THU' " & _
						"when datepart(dw,r.CheckIndate) = '6' then " & _
							"'FRI' " & _
						"when datepart(dw,r.CheckIndate) = '7' then " & _
							"'SAT' " & _
				"end) <> rst.ComboItem " & _
			"ORDER BY t.ComboItem, DayNumber, a.ReservationID",cn,3,3
%>
<table style="border-collapse:collapse;" border="1px">
	<tr>
		<td><b>Prospect</b></td>
		<td><b>Res Type</b></td>
		<td><b>Res Sub Type</b></td>
		<td><b>Check In Day</b></td>
		<td><b>Check In Date</b></td>
        <td><b>Nights</b></td>
		<td><b>Room Number</b></td>
		<td><b>Room Type</b></td>
		<td><b>Room Sub Type</b></td>
		<td><b>ReservationID</b></td>
	</tr>
	<%do while not rs.eof%>
	<tr>
		<td><%=rs.fields("ProspectName").value%></td>
		<td><%=rs.fields("ResType").value%></td>
		<td><%=rs.fields("ResSubType").value%></td>
		<td><%=rs.fields("CheckInDay").value%></td>
		<td><%=rs.fields("CheckInDate").value%></td>
		<td><%=rs.Fields("Nights").value%></td>
        <td><%=rs.fields("RoomNumber").value%></td>
		<td><%=rs.fields("RoomType").value%></td>
		<td><%=rs.fields("RoomSubType").value%></td>
		<td><a href="https://crms.kingscreekplantation.com/crmsnet/marketing/EditReservation.aspx?reservationid=<%=rs.fields("ReservationID").value%>"><%=rs.fields("ReservationID").value%></a></td>        
	</tr>
	<%
	rs.movenext
	loop
	rs.close
	%>
</table>	

<%
else
	response.write "Function is Unknown"
end if



cn.close

set rs = nothing
set cn = nothing
%>