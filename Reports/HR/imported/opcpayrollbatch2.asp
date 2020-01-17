<!--#include file="../../includes/dbconnections.inc" -->
<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<title>OPC Payroll Batch</title>
<script language = 'javascript' src = 'scripts/opcpayrollbatch2.js'></script>
<script langyage = 'javascript' src = '../../scripts/ajaxRequest.js'></script>
<link rel="stylesheet" type="text/css" href="css/opcpayrollbatch.css" media = "all">
<!--mstheme--><link rel="stylesheet" type="text/css" href="../../_themes/arctic/arct1011.css"><meta name="Microsoft Theme" content="arctic 1011, default">
<meta name="Microsoft Border" content="tlb, default">
</head>

<body><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td><table border="0" cellspacing="0" cellpadding="0" width="100%" height="19"><tr><td align="left" valign="top" width="138">&nbsp;</td><td align="left" valign="top">
	 </td><td align="left" valign="top">&nbsp;</td></tr></table></td></tr><!--msnavigation--></table><!--msnavigation--><table dir="ltr" border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td valign="top" width="1%"><p>
</p><p>&nbsp;</p></td><td valign="top" width="24"></td><!--msnavigation--><td valign="top">
<form>
	<table>
		<tr>
			<td>
				<b>BatchID</b>
			</td>
			<td>
				<!--<input type = text name = bID id = bID>-->
				<select name = bID id = bID>
					<option value = 0></option>
					<%
						dim rs,cn
						
						set rs = server.createobject("ADODB.Recordset")
						set cn = server.createobject("ADODB.Connection")
						DBName = "CRMSData"
						cn.open DBName,DBUser,DBPass
						
						rs.open " select * from t_payrollBatch where companyid in (select comboitemid from t_comboitems where comboname = 'PayrollCompany' and comboitem = 'VRC') order by payrollbatchid desc",cn,3,3
						if rs.eof and rs.bof then
						else
							do while not rs.eof
								response.write "<option value = '" & rs.fields("PayrollBatchID").value & "'>" & rs.fields("BatchName").value & "</option>"
								rs.movenext
							loop
							rs.close
						end if
						cn.close
						set rs = nothing
						set cn = nothing
					%>
				</select> 
			</td>
		</tr>
		<tr>
			<td>
				Show Details:
			</td>
			<td>
				<input type="checkbox" name="Detail" id="Detail" checked>
			</td>
		</tr>
		<tr>
			<td>
				<input type = 'button' name = 'report' value = 'Run Report' onclick ='Get_Report();'>
			</td>
			<td>
				<input type="button" value="Printable Version" id="printable" disabled = "true" onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('results').innerHTML);">			
				<input type="button" value="Printable Version" id="printable2" onclick = "printable_version();">			
			</td>
		</tr>
	</table>	
<div id = results></div>
</form>

<!--msnavigation--></td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td><p>&nbsp;</p></td></tr><!--msnavigation--></table></body>
</html>
