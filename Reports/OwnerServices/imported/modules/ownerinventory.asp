

<%
dim cn
dim rs
set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.recordset")


DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

cn.open DBname, DBUser, DBPass

if request("function") = "List" then
	sSQL = "select p.Lastname + ',' + p.FirstName as Owner, c.ContractNumber, c.OccupancyDate, so.OccupancyYear, se.ComboItem as Season, ut.ComboItem as UnitType, ust.ComboItem as UnitSubType, cst.ComboItem as ContractSubType, f.Frequency from t_Contract c inner join t_Prospect p on p.ProspectID = c.ProspectID left outer join t_ComboItems se on se.ComboItemID = c.SeasonID inner join t_Frequency f on f.FrequencyID = c.FrequencyID left outer join t_ComboItems st on st.ComboItemID = c.StatusID left outer join t_ComboItems cst on cst.ComboItemID = c.SaleSubTypeID inner join t_SoldInventory so on so.ContractID = c.ContractID inner join t_SalesInventory sa on sa.SalesInventoryID = so.SalesInventoryID inner join t_Unit u on u.UnitID = sa.UnitID inner join t_ComboItems ut on ut.ComboItemid = u.TypeID left outer join t_ComboItems ust on ust.ComboItemID = u.SubTypeID where st.ComboItem in ('active','redeed','suspense','developer')"
		if request("occupancyyear") = "0" then
		else
			sSQL = sSQL & " and so.OccupancyYear = '" & request("occupancyyear") & "'"
		end if
		if request("season") = "0" then
		else
			sSQL = sSQL & " and se.ComboItemID = '" & request("season") & "'"
		end if
		if request("unittype") = "0" then
		else
			sSQL = sSQL & " and ut.ComboItemID = '" & request("unittype") & "'"
		end if
		if request("frequency") = "0" then
		else
			sSQL = sSQL & " and f.FrequencyID = '" & request("frequency") & "'"
		end if
	sSQL = sSQL & " order by c.ContractNumber"
	rs.open sSQL,cn,3,3
	'response.write sSQL
%>

<div>
<strong style='color:Red'>
<%=rs.recordcount%>
</strong> 

Total Records
</div>

	<table style='border-collapse:collapse;' border='1px'>
		<tr>
			<th align="left">Owner Name</th>
			<th>Contract Number</th>
			<th>Occupancy Year(C/I)</th>
			<th>Season</th>
			<th>Contract Type</th>
			<th>Unit Type</th>
			<th>Unit Sub Type</th>
			<th>Frequency</th>
		</tr>
	<%do while not rs.eof%>
		<tr>
			<td><%=rs.fields("owner").value%></td>
			<td align='center'><%=rs.fields("ContractNumber")%></td>
			<td align='center'><%=right(rs.fields("occupancydate"),4)%>/<%=rs.fields("occupancyyear").value%></td>
			<td><%=rs.fields("season").value%></td>
			<td><%=rs.fields("contractsubtype").value%></td>
			<td><%=rs.fields("unittype").value%></td>
			<td><%=rs.fields("unitsubtype").value%></td>
			<td><%=rs.fields("frequency").value%></td>
		</tr>
		<%
		rs.movenext
		loop
		rs.close
		%>
	</table>
<%
else
	response.write "Function is unknown"
end if
cn.close
set rs = nothing
set cn = nothing
%>