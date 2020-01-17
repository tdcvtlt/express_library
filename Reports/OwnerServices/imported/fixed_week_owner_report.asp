
<html>

<head>
<title>Fixed Week Owner Report</title>
<meta name="Microsoft Theme" content="none, default" />
<meta name="Microsoft Border" content="none, default" />
</head>
<%
dim cn
dim rs

set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.recordset")


DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"


cn.open DBName, DBUser, DBPass
rs.open "SELECT p.LastName + ', ' + p.FirstName as OwnerName, u.Name as UnitName, x.Week as Week, f.frequency as Frequency, s.occupancyyear as occupancyyear, c.ContractNumber, st.comboItem SubType " & _
		"FROM t_prospect p " & _
		"INNER JOIN t_contract c on c.prospectid = p.prospectid " & _
		"INNER JOIN t_soldinventory s on s.contractid = c.contractid " & _
		"INNER JOIN t_salesinventory x on x.salesinventoryid = s.salesinventoryid " & _
		"INNER JOIN t_unit u on u.unitid = x.unitid " & _
		"INNER JOIN t_frequency f on f.frequencyid = s.frequencyid " & _
		"INNER JOIN t_comboitems z on c.weektypeid = z.comboitemid " & _
        "INNER JOIN T_COMBOS CO ON Z.COMBOID = CO.COMBOID " & _
        "inner join t_comboItems st on st.comboItemID = c.subtypeID " & _
		"WHERE z.Comboitem = 'Fixed' AND CO.COMBONAME = 'WeekType' " & _
		"ORDER BY Frequency, LastName, Week",cn,3,3
%>
<body>
<input type="button" value="Printable Version" name="B1" onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('report').innerHTML);" />
<div id='report'>
	<table style="border-collapse:collapse;" border="1px">
		<tr>
			<td><b><u>Owner Name</u></b></td>
			<td><b><u>Contract Number</u></b></td>
			<td><b><u>Unit Address</u></b></td>
			<td><b><u>Week</u></b></td>
			<td><b><u>Frequency</u></b></td>
			<td><b><u>Occupancy Year</u></b></td>
            <td><b><u>Contract Sub-Type</u></b></td>
		</tr>
	<%do while not rs.eof%>
		<tr>
			<td><%=rs.fields("OwnerName").value%></td>
			<td><%=rs.fields("ContractNumber").value%></td>
			<td><%=rs.fields("UnitName").value%></td>
			<td><%=rs.fields("Week").value%></td>
			<td><%=rs.fields("Frequency").value%></td>
			<td><%=rs.fields("occupancyyear").value%></td>
            <td><%=rs.fields("SubType").value%></td>
		</tr>
	<%rs.movenext
		LOOP%>
	</table>
</div>
</body>
<%
rs.close
cn.close

set rs = nothing
set cn = nothing
%></html>