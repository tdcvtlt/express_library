
<%
dim cn
dim rs
dim rs2

set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.recordset")
set rs2 = server.createobject("ADODB.Recordset")


DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

cn.open DBName, DBUser, DBPass

if request("Function") = "RunReport" then
		rs.open "SELECT u.UsageID, t.ComboItem as UsageType, st.ComboItem as UsageSubType, r.RoomNumber, rt.ComboItem as RoomType, rst.ComboItem as RoomSubType, u.InDate, u.ContractID, pros.LastName + ', ' + pros.FirstName as ProspectName, " & _
				"case " & _
					"when DATEPART(dw,u.InDate) = '1' then " & _
						"'SUN' " & _
					"when DATEPART(dw,u.InDate) = '2' then " & _
						"'MON' " & _
					"when DATEPART(dw,u.InDate) = '3' then " & _
						"'TUE' " & _
					"when DATEPART(dw,u.InDate) = '4' then " & _
						"'WED' " & _
					"when DATEPART(dw,u.InDate) = '5' then " & _
						"'THU' " & _
					"when DATEPART(dw,u.InDate) = '6' then " & _
						"'FRI' " & _
					"when DATEPART(dw,u.InDate) = '7' then " & _
						"'SAT' " & _
				"end as UsageDay, " & _
                "coalesce((select DATEDIFF(d, min(dateallocated), max(dateallocated)) + 1 from t_RoomAllocationMatrix " & _
                "where UsageID = u.UsageID and reservationid > 0), 0) as NumberOfNights " & _
				"FROM t_Usage u " & _
				"INNER JOIN t_ComboItems t on t.ComboItemID = u.TypeID " & _
				"Left Outer JOIN t_ComboItems st on st.ComboItemID = u.SubTypeID " & _
				"INNER JOIN (SELECT DISTINCT UsageID, RoomID FROM t_RoomAllocationMatrix WHERE DateAllocated between '" & request("SDate") & "' and '" & request("EDate") & "') as a on a.UsageID = u.UsageID " & _
				"INNER JOIN t_Room r on r.RoomID = a.RoomID " & _
				"INNER JOIN t_ComboItems rt on rt.ComboItemID = r.TypeID " & _
				"INNER JOIN t_ComboITems rst on rst.ComboItemID = r.SubTypeID " & _
				"INNER JOIN t_Contract c on c.ContractID = u.ContractID " & _
				"INNER JOIN t_Prospect pros on pros.ProspectID = c.ProspectID " & _
				"WHERE  t.ComboItem in ('Owner','Exchange','Rental', 'PointsExchange', 'TrialOwner', 'Points') and u.InDate between '" & request("SDate") & "' and '" & request("EDate") & "' and " & _
					"(case " & _
						"when DATEPART(dw,u.InDate) = '1' then " & _
							"'SUN' " & _
						"when DATEPART(dw,u.InDate) = '2' then " & _
							"'MON' " & _
						"when DATEPART(dw,u.InDate) = '3' then " & _
							"'TUE' " & _
						"when DATEPART(dw,u.InDate) = '4' then " & _
							"'WED' " & _
						"when DATEPART(dw,u.InDate) = '5' then " & _
							"'THU' " & _
						"when DATEPART(dw,u.InDate) = '6' then " & _
							"'FRI' " & _
						"when DATEPART(dw,u.InDate) = '7' then " & _
							"'SAT' " & _
					"end) <> rst.ComboItem " & _
				"ORDER BY t.ComboItem, st.ComboItem",cn,3,3
%>
<table border='1PX' style='border-collapse:collapse;'>
	<tr>
		<td><b>Prospect</b></td>
		<td><b>Usage Type</b></td>
		<td><b>Usage Sub Type</b></td>
		<td><b>Room Number</b></td>
		<td><b>Room Type</b></td>
		<td><b>Room Sub Type</b></td>
		<td><b>In Date</b></td>
		<td><b>Usage Day</b></td>
        <td><b>Number Of Nights</b></td>
		<td style="width:0px;"><b>UsageID</b></td>
	</tr>
	<%do while not rs.eof%>
	<tr>
		<td><%=rs.fields("ProspectName").value%></td>
		<td><%=rs.fields("UsageType").value%></td>
		<td><%=rs.fields("UsageSubType").value%></td>
		<td><%=rs.fields("RoomNumber").value%></td>
		<td><%=rs.fields("RoomType").value%></td>
		<td><%=rs.fields("RoomSubType").value%></td>
		<td><%=rs.fields("InDate").value%></td>
		<td><%=rs.fields("UsageDay").value%></td>
        <td><%=rs.fields("NumberOfNights").value%></td>
		<td style="width:0px">
            <a href="<%=rs.fields("ContractID").value%>&usageid=<%=rs.fields("UsageID").value%>"><%=rs.fields("UsageID").value%></a> 
        </td>
	</tr>
	<%
		rs2.open "select note from t_note where keyfield = 'USAGEID' and  keyvalue = '" & rs.fields("UsageID").value & "' ",cn,3,3
		do while not rs2.eof
			response.write "<tr colspan = 9>"
			response.write "<td>&nbsp;</td><td><b>NOTE:</b></td><td colspan = 7>" & rs2.fields("Note").value & "</td>"
			response.write "</tr>"
			rs2.movenext
		loop
		rs2.close
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
set rs2 = nothing
set cn = nothing
%>




