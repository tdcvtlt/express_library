
	<%
    dim rs
	dim rst
	dim cn
	
	set cn = server.createobject("Adodb.Connection")
	set rst = server.createobject("ADODB.Recordset")
	set rs = server.createobject("ADODB.Recordset")
	'DBName = "CRMSData"
	
	DBName = "CRMSNet"
    DBUser = "asp"
    DBPass="aspnet"
        
    cn.commandtimeout = 0
	cn.OPen DBName,DBUser,DBPass
	
%> <html><head><title>Pender Contracts</title>
<script language = "javascript" src = "../../../scripts/ajaxRequest.js"></script>
<script language="javascript" src = "../../../scripts/scw.js"></script>
<script language = "javascript" src = "scripts/pendercontract.js"></script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="none, default">
</head><body onload="Get_Personnel();"><form onsubmit="return false;"><p><table><tr><td>Location: </td><td><select name = 'loc'><option value = 0>ALL</option><option value = "Williamsburg">Williamsburg</option><option value = "Richmond">Richmond</option></select></td></tr><tr><td>Range:</td><td><select name = 'range'><option value = 0>ALL</option><option value = "8999">0-$8,999</option><option value = "9000-12999">$9,000 - $12,999</option><option value="13000-16999">$13,000-$16,999</option><option value="17000">$17,000+</option></select></td></tr><tr><td>Start Date:</td><td><input type = 'text' id = 'startdate' name = 'sdate' Readonly onchange="alert('Changed');" onclick="scwShow(this,this);" value="<%=request("startdate")%>" size="20"></td></tr><tr><td>End Date:</td><td><input type = 'text' id = 'edate' name = 'edate' value="<%=request("enddate")%>" size="20" readonly onclick="scwShow(this,this);"></td></tr><tr><td>Contract Status:</td><td><select size="1" name="status"><%
	rst.open "Select comboitemid, comboitem from t_Comboitems a inner join t_combos b on a.comboid = b.comboid where comboname = 'ContractStatus' order by comboitem",cn,0,1
	do while not rst.eof
		if rst.fields("ComboItem").value & "" = "Pender" then
			response.write "<option selected value = '" & rst.fields("ComboItem").value & "'>" & rst.fields("Comboitem").value & "</option>"
		else
			response.write "<option value = '" & rst.fields("ComboItem").value & "'>" & rst.fields("Comboitem").value & "</option>"
		end if
		rst.movenext
	loop
	rst.close
%></select></td></tr><tr><td>Sales Rep:</td><td><select size="1" name="rep"><option value=0>ALL</option><%	
	

%></select></td></tr><tr><td>Title:</td><td><select name = 'title'><option value = 0>ALL (Shows Sales Executive Titles Only)</option><%
	rst.open "Select comboitemid, comboitem from t_Comboitems a inner join t_combos b on a.comboid = b.comboid where comboname = 'personneltitle' order by comboitem",cn,0,1
	do while not rst.eof
		response.write "<option value = '" & rst.fields("ComboItemID").value & "'>" & rst.fields("Comboitem").value & "</option>"
		rst.movenext
	loop
	rst.close
%></select></td></tr><tr><td>Show Individuals:</td><td><input type="checkbox" name="ind" value="ON"></td></tr><tr><td>Show Records:</td><td><input type="checkbox" name="rec" value="ON"></td></tr><tr><td><input type = 'button' name = 'go' value = 'Run Report' onclick = 'Call_Report();'></td><td><input type="button" value="Printable Version"   id="printable" disabled = "true" onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('report').innerHTML);"></td></tr></table><br>&nbsp;</p></form><div id = "report"></div></form></body></html>