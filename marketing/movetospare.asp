<html>

<head>
<title>Move To Spare</title>
<script type ="text/javascript" language="javascript" src = "../scripts/ajaxRequest.js"></script>
<script type ="text/javascript" language="javascript" src = "../scripts/rooms.js"></script>
</head>

<body>
<form>

<table width="407">
	<tr>
		<td width="95">Reason:</td>
		<td colspan = '4'><input type = 'text' name = 'reason' size = '41' /></td>
	</tr>
	<%
		if request("type") = "booked" then
	%>
		<input type = 'hidden' name = 'inDate' value = '<%=request("date")%>'>
		<tr><td width="230"><input type = 'button' name = 'go' value = 'Search' onclick = "search_Spares('<%=request("ReservationID")%>', '<%=request("RoomID")%>', '<%=request("outDate")%>');"></td></tr>
	<%
		else
	%>	
	<tr>
		<td width="95">Date Moved:</td>
		<td width="68"><select name = 'inDate'>
			<%
				if Date = CDate(request("date")) then
			%>
					<option value = '<%=Date%>'><%=Date%></option>
			<%
				else
			%>
					<option value = '<%=Date%>'><%=Date%></option>
					<option value = '<%=Date - 1%>'><%=Date - 1%></option>
			<%
				end if
			%>
			</select>
		</td>
		<td width="230"><input type = 'button' name = 'go' value = 'Search' onclick = "search_Spares('<%=request("ReservationID")%>', '<%=request("RoomID")%>', '<%=request("outDate")%>');"></td>
	</tr>
	<% end if%>
</table>
<br />
<div id = "spares"></div>

<input type = 'hidden' name = 'type' value = '<%=request("type")%>'>
</form>


</body>

</html>
