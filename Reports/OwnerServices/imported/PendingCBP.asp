

<html>
<%
dim cn
dim rs

DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.recordset")

cn.open DBName, DBUser, DBPass
%>
<head>
<title>Check By Phone Payments</title>

<script language="javascript" src="scripts/CheckByPhone.js"></script>
<script language="javascript" src="../../../scripts/scw.js"></script>
<script language="javascript" src="../../../scripts/ajaxrequest.js"></script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="none, default">
</head>

<body>

<form>
	<table>
		<tr>
			<td><b>Contract Number:</b></td>
			<td><input type="text" name="contractnumber"></td>
		</tr>
		<tr>
			<td><b>Status:</b></td>
			<td>
				<select name='statusid'>
					<option value="0"></option>
					<%
					rs.open "SELECT * FROM t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID WHERE ComboName = 'CheckPayStatus' and Active = '1'",cn,0,1
					do while not rs.eof
						response.write "<option value='" & rs.fields("ComboItemID").value & "'>" & rs.fields("ComboItem").value & "</option>"
					rs.movenext
					loop
					rs.close
					%>
				</select>
			</td>
		</tr>
		<tr>
			<td><b>Date To Run:</b></td>
			<td><input type="text" name="startdate" size="20" readonly onclick = "scwShow(this,this);">&nbsp;-&nbsp;<input type="text" name="enddate" size="20" readonly onclick = "scwShow(this,this);"></td>
		</tr>
		<tr>
			<td colspan='2'><input type="button" name="run" value="Run Report" onclick="RunReport();"></td>
		</tr>
	</table>
	<div id="report"></div>
</form>



</body>
<%
cn.close

set rs = nothing
set cn = nothing
%>
</html>
