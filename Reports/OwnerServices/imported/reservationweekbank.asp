
<html>

<head>
<title>Reservation Banking</title>
<script language="javascript" src="scripts/reservationweekbank.js"></script>
<script language="javascript" src="../../../scripts/ajaxRequest.js"></script>
<script language="javascript" src = "../../../scripts/scw.js"></script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="tlb;">
</head>

<body><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

<p align="center"><font size="6"><strong>
</strong></font><br>
</p>
<p align="center">&nbsp;</p>

</td></tr><!--msnavigation--></table><!--msnavigation--><table dir="ltr" border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td valign="top" width="1%">

</td><td valign="top" width="24"></td><!--msnavigation--><td valign="top">
<%
dim rs
dim cn

set rs = server.createobject("ADODB.Recordset")
set cn = server.createobject("ADODB.Connection")	

DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

cn.open DBName, DBUser, DBPass

%>
<form>
<table>
	<tr><td>&nbsp;</td><td>
	&nbsp;</td></tr>
<tr><td># Nights: </td><td><select name="nights" id="nights"><option value="0"></option><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option><option value="6">6</option><option value="7">7</option><option value="8">8</option><option value="9">9</option><option value="10">10</option></select></td></tr>
<tr><td>Type:</td><td>			<Select name='type' id="type"><option value = '0'></option><%
				rs.open "SELECT * FROM t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID WHERE ComboName = 'ReservationType'",cn,0,1
				
					do while not rs.eof
						response.write "<option value = '" & rs.fields("ComboItemID").value & "'>" & rs.fields("ComboItem").value & "</option>"
					rs.movenext
				loop
	
				rs.close
%></Select></td></tr>

<tr><td>Start Date: </td><td><input type = 'text' id = 'input_startdate' name = 'startdate' Readonly  onclick="scwShow(this,this);" value="<%=request("startdate")%>" size="20"></td></tr>
<tr><td>Start Date: </td><td><input type = 'text' id = 'input_enddate' name = 'enddate' Readonly  onclick="scwShow(this,this);" value="<%=request("enddate")%>" size="20"></td></tr>
<tr><td>Show Details:<input type="checkbox" name="detail" id="detail" value="ON" checked></td></tr>
<tr><td><input type = button value = "Run Report" onclick = "Run_Report();"></td><td><input type="button" value="Printable Version"   id="printable" disabled = "true" onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('report').innerHTML);"></td></tr></p>
</table>
</form>
<div id = "report"></div>
<%
cn.close

set rs = nothing
set cn = nothing
%>
<!--msnavigation--></td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

</td></tr><!--msnavigation--></table></body>

</html>