
<html>

<head>
<title>OPC-OS Ledger</title>
<meta name="GENERATOR" content="Microsoft FrontPage 6.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<script language = "javascript" src = "scripts/OPC-OSLedger.js"></script>
<script language = "javascript" src = "../../../scripts/scw.js"></script>
<script language = "javascript" src = "../../../scripts/ajaxRequest.js"></script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="none, default">
</head>

<body>
<form>
<table>	
	<tr>
		<td>Start Date:</td>
		<td><input type = 'text' name = 'sDate' onclick = "scwShow(this, this);" readonly></td>
	</tr>
	<tr>
		<td>End Date:</td>
		<td><input type = 'text' name = 'eDate' onclick = "scwShow(this, this);" readonly></td>
	</tr>
	<tr>
		<td>Solicitor:</td>
		<td><select name = 'repID'>
				<option value = '0'>ALL</option>
				<%
				Dim cn
				Dim rs
				set cn = server.createobject("ADODB.Connection")
				set rs = server.createobject("ADODB.Recordset")
				cn.Open "CRMSNET", "ASP","aspnet"
				rs.open "Select a.PersonnelID, a.FirstName, a.LastName from t_Personnel2Dept b inner join t_Personnel a on a.personnelid = b.personnelid where b.DepartmentID = (Select comboitemid from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'Department' and comboitem = 'OPC-OS') order by a.Lastname asc", cn, 3, 3
				Do while not rs.EOF
				%>
				<option value = '<%=rs.Fields("PersonnelID")%>'><%=rs.Fields("LastName") & ", " & rs.FIelds("Firstname")%></option>
				<%
				rs.MoveNext
				Loop
				rs.Close
				cn.Close
				set rs = Nothing
				set cn = nothing
				%>
			</select>
		</td>
	</tr>
	<tr>
		<td><input type = 'button' name = 'go' value = 'Submit' onclick = "run_Report();"></td>
	</tr>
</table>
</form>
<div id = "results"></div>
</body>

</html>
