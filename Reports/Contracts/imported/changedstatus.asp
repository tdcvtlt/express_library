
<html>

<head>
<title>Contracts With Changed Status</title>
	<script language="javascript" src="scripts/changedstatus.js"></script>
	<script language="javascript" src="../../../scripts/ajaxRequest.js"></script>
	<script language="javascript" src="../../../scripts/scw.js"></script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="none, default">
</head>

<%
dim cn
dim rs

set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")


    DBName = "CRMSNet"
    DBUser = "asp"
    DBPass = "aspnet"

cn.open DBName, DBUser, DBPass
%> 

<body>
<form>
<table>
	<tr>
		<td colspan="3">
			<b>Transaction Date:</b>
		</td>
	</tr>
	<tr>
		<td colspan="3">
			<b>Start Date:</b>

			<input type = 'text' id = 'startdate' name = 'startdate' value="<%=request("startdate")%>" size="20" readonly onclick="scwShow(this,this);">
		</td>
	</tr>
	<tr>
		<td colspan="3">
			<b>End Date:</b>

			<input type = 'text' id = 'enddate' name = 'enddate' value="<%=request("enddate")%>" size="20" readonly onclick="scwShow(this,this);">
		</td>


	<tr>
		<td colspan="3">
			Contract Status:</td>
		</tr>


	<tr>
		<td>
			<select name = stat size = 10><option value="ALL">ALL</option>
				<%
					rs.open "Select * from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and active =1",cn, 0,1
					do while not rs.eof
						response.write "<option value ='" & rs.fields("Comboitem").value & "'>" & rs.fields("Comboitem").value & "</option>"
						rs.movenext
					loop
					rs.close
				
				%>
			</select>
		</td>
		<td valign=top>
			<input type=button value = "ADD >>" onclick = "Add_Status();"><br>
			<input type=button value = "Remove <<" onclick="Remove_Status();"><br>
			<input type=button value = "Remove All<<" onclick = "Remove_All();">
		</td>
		<td>
			<select name = "status" size = 10></select>
		</td>
		</tr>


	
	<tr>
		<td colspan="3">
			<input type="button" name="run" value="Run" onclick="Call_Report();"><input type="button" value="Printable Version"   id="printable" disabled = "true" onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('report').innerHTML);">
		</td>
	</tr>

</table>

</form>
<div id="report"></div>

</body></html>