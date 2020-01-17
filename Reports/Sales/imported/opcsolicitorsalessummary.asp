

<html>
<head>

<title>OPC Solicitor Summary</title>

<script language="javascript" src = "../../../scripts/ajaxRequest.js"></script>
<script language="javascript" src = "../../../scripts/scw.js"></script>
<script language="javascript" src = "scripts/solicitorsalessummary.js"></script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="tlb;default">
</head>

<body><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

<p align="center"><font size="6"><strong>
</strong></font><br>
</p>
<p align="center">&nbsp;</p>

</td></tr><!--msnavigation--></table><!--msnavigation--><table dir="ltr" border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td valign="top" width="1%">

</td><td valign="top" width="24"></td><!--msnavigation--><td valign="top">
<form method="POST"  onsubmit="return false;">
<table>
	<tr>
		<td>
					Please Select A Solicitor: 
		</td>
		<td>
					<select size="1" name="name"><option value=0>ALL</option>
<%

dim DBName
dim DBUser
dim DBPass


DBName = "CRMSNET"
DBUser = "asp"
DBPass = "aspnet"

	dim rs
	dim rst
	dim cn
	'DBName = "CRMSData"
	set cn = server.createobject("ADODB.Connection")
	set rst = server.createobject("ADODB.Recordset")
	set rs = server.createobject("ADODB.Recordset")
	
	cn.OPen DBName,DBUser,DBPass

	'cn.Open "CRMSData", "asp", "aspnet"
    'rst.OPen "Select firstname + ' ' + lastname as personnel, personnelid from t_Personnel where vendorid = '8' or vendorid = '39' order by firstname asc", cn, 3, 3

    dim sql
    
    sql = "select firstname + ' ' + lastname AS Personnel, p.PersonnelID from t_Vendor v inner join t_Vendor2Personnel v2p on v.vendorid = v2p.vendorid inner join t_personnel p on v2p.personnelid = p.personnelid where v2p.vendorid = 8 or v2p.vendorid = 25 order by p.firstname"

	rst.open sql, cn, 3, 3

	Do while not rst.EOF
		if request("Personnel") = cstr(rst.fields("PersonnelID").value) then
			response.write "<option selected value = '" & rst.fields("PersonnelID").value & "'>" & rst.fields("Personnel").value & "</option>"
		else
			response.write "<option value = '" & rst.fields("PersonnelID").value & "'>" & rst.fields("Personnel").value & "</option>"
		end if
		'response.write "<option value = '" & rst.Fields("PersonnelID") & "'>" & rst.Fields("FirstName") & " " & rst.Fields("LastName") & "</option>"
		rst.MoveNext
	Loop
	rst.Close

%> 
</select></td></tr>

<tr><td>Please Select A Location:</td><td><select name = 'loc'><option value = 0>ALL</option>

<%

	'rst.OPen "Select SalesLocationID, Location from t_VendorSalesLocations where vendorid = '20' or vendorid = '39' order by location asc", cn, 3, 3

    
    sql = "select SalesLocationID, Location from t_vendorsaleslocations where vendorid = 8 or vendorid = 25"
    rst.open sql, cn, 3, 3


	Do while not rst.EOF
		response.write "<option value = '" & rst.fields("SalesLocationID").value & "'>" & rst.fields("Location").value & "</option>"
		rst.MoveNext
	Loop
	rst.Close
%>

</select></td></tr>

<tr><td>Please Select A Vendor:</td><td><select size="1" name="vendor"><option></option>
<%

	
	'rs.OPen "Select Vendor, Vendorid from t_Vendor where (vendorid = '20' or vendorid = '39') order by Vendor asc", cn, 3, 3    
    
    sql = "select Vendor, VendorID from t_Vendor where vendorid = 8 or vendorid = 25"

    rs.open sql, cn, 3, 3


	Do while not rs.EOF
		if request("Vendor") = cstr(rs.fields("VendorID").value) then
			response.write "<option selected value = '" & rs.fields("VendorID").value & "'>" & rs.fields("Vendor").value & "</option>"
		else
			response.write "<option value = '" & rs.fields("VendorID").value & "'>" & rs.fields("Vendor").value & "</option>"
		end if
		'response.write "<option value = '" & rs.Fields("PersonnelID") & "'>" & rs.Fields("FirstName") & " " & rs.Fields("LastName") & "</option>"
		rs.MoveNext
	Loop
	rs.Close
	

	

%></td></tr></table>
<%
cn.close		
set rs = nothing
set rst = nothing 
set cn = nothing
%>
<table><tr><td>Start Date: </td><td><input type = 'text' id = 'input_startdate' name = 'startdate' Readonly  onclick="scwShow(this,this);" value="<%=request("startdate")%>" size="20"></td></tr>
<tr><td>End Date:</td><td><input type = 'text' id = 'input_enddate' name = 'enddate' value="<%=request("enddate")%>" size="20" readonly onclick="scwShow(this,this);"></td></tr>
<tr><td colspan = '2'><input type = 'radio' name = 'camp' value = 'Both' checked>MAL/MAL-TS <input type = 'radio' name = 'camp' value = 'MAL'>MAL <input type = 'radio' name = 'camp' value = 'MAL-TS'> MAL-TS</td></tr>
<tr><td colspan=2><input type = 'button' name = 'go' value = 'Run Report' onclick = 'Call_Report();'><input type="button" value="Printable Version"   id="printable" disabled = "true" onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('report').innerHTML);"></td></tr></table></form>
<div id = "report"></div>