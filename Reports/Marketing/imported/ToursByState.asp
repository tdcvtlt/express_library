
<html>

<head>
<title>Tours By State</title>
<script language = "javascript" src="scripts/ToursByState.js"></script>
<script language = "javascript" src="../../../scripts/ajaxRequest.js"></script>
<script language = "javascript" src="../../../scripts/scw.js"></script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="none, default">
</head>
<%
dim cn
dim rs

set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")

DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

cn.open DBName, DBUser,DBPass
%>
<body>
<form onsubmit="return false;">
<table>
	<tr>
		<td>
			StartDate:
			<input type = text name = sdate id = sdate onclick="scwShow(this,this);" readonly>
		</td>
	</tr>
	<tr>
		<td>
			EndDate:
			<input type = text name = edate id = edate onclick="scwShow(this,this);" readonly>
		</td>	
	</tr>
</table>
<table>
	<tr>
		<td>
			No Show Status:
		</td>
		<td>
			NQ Status:
		</td>
		<td>
			Qualified Status:
		</td>
		<td>
			Cancelled Status:
		</td>
	</tr>
	<tr>		
		<td valign="top">
			<select name = nostat><option value="ALL">ALL</option>
				<%
					rs.open "Select * from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'TourStatus' and comboitem like 'NO%' and active =1",cn, 0,1
					do while not rs.eof
						response.write "<option value ='" & rs.fields("Comboitemid").value & "'>" & rs.fields("Comboitem").value & "</option>"
						rs.movenext
					loop
					rs.close
				%>
			</select>
		</td>
		<td valign="top" align="left">
			<select name='nqstat'>
				<option value='ALL'>All</option>
				<%
				rs.open "select * from t_comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'TourStatus' and comboitem like 'NQ%' and active = 1 order by comboitem",cn,3,3
				do while not rs.eof
					response.write "<option value = '" & rs.fields("comboitemid").value & "'>" & rs.fields("comboitem").value & "</option>"
				rs.movenext
				loop
				rs.close
				%>
			</select>
		</td>
		<td valign="top">
			<select name='qualstat'>
			<option value='ALL'>All</option>
			<%
			rs.open "select * from t_comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'TourStatus' and comboitem in ('OnTour','Booked','Showed') order by comboitem",cn,3,3
			do while not rs.eof
				response.write "<option value = '" & rs.fields("comboitemid").value & "'>" & rs.fields("comboitem").value & "</option>"
			rs.movenext
			loop
			rs.close
			%>
			</select>
		</td>
		<td valign="top">
			<select name='canstat'>
				<option value="ALL">All</option>
			<%
			rs.open "select * from t_comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'TourStatus' and comboitem like 'canc%' and active = 1 order by comboitem",cn,3,3
			do while not rs.eof
				response.write "<option value = '" & rs.fields("comboitemid").value & "'>" & rs.fields("comboitem").value & "</option>"
			rs.movenext
			loop
			rs.close
			%>				
			</select>
		</td>		
	</tr>
	<tr>
		<td>
			<select name = nostatus size = 5></select>
		</td>

		<td>
			<select name = 'nqstatus' size = "5"></select>
		</td>
		<td>
			<select name = 'qualstatus' size = "5"></select>
		</td>
		<td valign="top">
			<select name = 'canstatus' size = "5"></select>
		</td>
	</tr>
	<tr>
		<td>
			<br>
			<input type=button value = "ADD" onclick = "add_NSStatus();">
			<br>
			<input type=button value = "Remove" onclick="remove_NSStatus();">
			<br>
		</td>
		<td>
			<br>
			<input type = 'button' name = 'go' value = 'Add' onclick = "add_NQStatus();">
			<br>
			<input type = 'button' name = 'go' value = 'Remove' onclick = "remove_NQStatus();">
			<br>
		</td>
		<td>
			<br>
			<input type = 'button' name = 'go' value = 'Add' onclick = "add_QualStatus();">
			<br>
			<input type = 'button' name = 'go' value = 'Remove' onclick = "remove_QualStatus();">
			<br>
		</td>
		<td>
			<br>
			<input type = 'button' name = 'go' value = 'Add' onclick = "add_CanStatus();">
			<br>
			<input type = 'button' name = 'go' value = 'Remove' onclick = "remove_CanStatus();">
			<br>
		</td>
	</tr>	
</table>
<table>
	<tr>
		<td>
			<input type = button value = 'Get Report' onclick="Get_report();"><input type="button" value = "Create Excel" onclick = "Report_Excel();">
		</td>
	</tr>
</table>
<hr>
</form>
<%
cn.close

set rs = nothing
set cn = nothing
%>
<div id="initstatus" style = "display:none;"><img src="../../images/progressbar.gif"></div>
<div id ="report"></div>

</table>

</body>

</html>
