
<html>
<head>

<title>OPC Location Report</title>

<script language="javascript" src = "../../../scripts/ajaxRequest.js" type="text/javascript"></script>
<script language="javascript" src = "../../../scripts/scw.js" type="text/javascript"></script>
<script language="javascript" src = "scripts/oplocsalessummary.js" type="text/javascript"></script>
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
<form method="POST" action="../../--WEBBOT-SELF--" onsubmit="return false;">
<table>
	<tr>
		<td>
					Please Select A Location: 
		</td>
		<td>
					<select size="1" name="location"><option value= "0">ALL</option>
<%
		dim rst
		dim rs 
		dim cn
		
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		set rst = server.createobject("ADODB.Recordset")
		
        DBName = "CRMSNET"
        DBUser = "asp"
        DBPass = "aspnet"

		cn.open DBName, DBUser, DBPass

		rs.OPen "Select SalesLocationID, Location from t_VendorSalesLocations where vendorid in (Select vendorid from t_Vendor where vendor in ('OPC','4K','EMS','Prestige')) order by location asc", cn, 3, 3
		Do while not rs.EOF
		if request("Location") = cstr(rs.fields("Saleslocationid").value) then
			response.write "<option selected value = '" & rst.fields("Saleslocationid").value & "'>" & rs.fields("Location").value & "</option>"
		else
			response.write "<option value = '" & rs.fields("SalesLocationid").value & "'>" & rs.fields("Location").value & "</option>"
		end if
			'response.write "<option value = '" & rs.Fields("SalesLocationID") & "'>" & rs.FIelds("Location") & "</option>"
			rs.MovenExt
		Loop
		rs.close

%> 
</select></td></tr>

<tr><td>Please Select A Solicitor:</td><td><select name = 'sol'><option value = "0">ALL</option>
<%
		rs.OPen "Select A.PersonnelID, FirstName, LastName from t_Personnel A INNER JOIN T_VENDOR2PERSONNEL B ON A.PERSONNELID = B.PERSONNELID  where vendorid in (Select vendorid from t_Vendor where vendor in ('OPC','4K','EMS','Prestige')) order by firstname asc", cn, 3, 3
		'rs.Open "Select PersonnelID, Firstname, LastName from t_Personnel where personnelid in (Select PersonnelID from t_Personnel2Dept where departmentid in (Select comboitemid from t_ComboItems where comboname = 'Department' and comboitem like 'OPC%')) order by firstname asc", cn, 3, 3
		Do while not rs.EOF
			response.write "<option value = '" & rs.fields("PersonnelID").value & "'>" & rs.fields("FirstName").value & " " & rs.Fields("LastName") & "</option>"
			rs.MovenExt
		Loop
		rs.close

%>
</select></td></tr>

<tr><td>Please Select A Vendor:</td><td><select size="1" name="vendor"><option value = "0">ALL</option>
<%

	
	rst.OPen "Select Vendor, Vendorid from t_Vendor where vendor in ('OPC','EMS','4K','Prestige') order by Vendor asc", cn, 3, 3
	Do while not rst.EOF
		if request("Vendor") = cstr(rst.fields("VendorID").value) then
			response.write "<option selected value = '" & rst.fields("VendorID").value & "'>" & rst.fields("Vendor").value & "</option>"
		else
			response.write "<option value = '" & rst.fields("VendorID").value & "'>" & rst.fields("Vendor").value & "</option>"
		end if
		'response.write "<option value = '" & rst.Fields("PersonnelID") & "'>" & rst.Fields("FirstName") & " " & rst.Fields("LastName") & "</option>"
		rst.MoveNext
	Loop
	rst.Close
	

	

%></select></td></tr></table>
<%
cn.close		
set rs = nothing
set rst = nothing 
set cn = nothing
%>

<table><tr><td>Start Date: </td>
    <td><input type = 'text' id = 'input_startdate' readonly="readonly"  onclick="scwShow(this,this);" value="<%=request("startdate")%>" size="20" /></td></tr>
    <tr><td>End Date:</td><td><input type = 'text' id = 'input_enddate' name = 'enddate' value="<%=request("enddate")%>" size="20"  readonly="readonly" onclick="scwShow(this,this);" /></td></tr>
    <!--<tr><td colspan = '2'><input type = 'radio' name = 'camp' value = 'Both' checked="checked" />MAL/MAL-TS 
                        <input type = 'radio' name = 'camp' value = 'MAL' />MAL 
                        <input type = 'radio' name = 'camp' value = 'MAL-TS' /> MAL-TS
                        <input type = 'radio' name = 'camp' value = 'PRESTIGE' /> PRESTIGE
                        <input type = 'radio' name = 'camp' value = '4K' /> 4K
                        <input type = 'radio' name = 'camp' value = 'EMS' /> EMS</td>-->
                        </tr>
<tr><td colspan=2><input type = 'button' name = 'go' value = 'Run Report' onclick = 'Call_Report();' />
                <input type="button" value="Printable Version" id="printable" disabled="disabled" onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('report').innerHTML);" /></td></tr></table></form>
<div id="initstatus" style = "display:none;"><img src="../../../images/progressbar.gif" alt="" /></div>
<div id = "report"></div>

<!--msnavigation--></td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

</td></tr><!--msnavigation--></table></body>

</html>