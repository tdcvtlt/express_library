<%
    DBName = "CRMSNET"
    DBUser = "asp"
    DBPass = "aspnet"
%>
<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<title>OPC Payroll Batch</title>
<script language = 'javascript' src = 'scripts/opcpayrollbatch.js'></script>
<script langyage = 'javascript' src = '../../../scripts/ajaxRequest.js'></script>
</head>

<body>
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
						DBName = "CRMSNet"
						cn.open DBName,DBUser,DBPass
						
						rs.open " select * from t_payrollBatch where companyid in (select comboitemid from t_comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'PayrollCompany' and comboitem = 'VRC') order by payrollbatchid desc",cn,3,3
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
                <input type = 'button' name = 'report' value = 'Run Report (Excel)' onclick ='Get_Report_Excel();'>
			</td>
			<td>
				<input type="button" value="Printable Version" id="printable" disabled = "true" onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('results').innerHTML);">			
				<input type="button" value="PayStub Version" id="printable2" onclick = "printable_version();">
			</td>
		</tr>
	</table>	
<div id = results></div>
</form>
</html>
