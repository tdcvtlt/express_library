<html>

<head>
<title>Out of Service Rooms</title>
<script language="javascript" type="text/javascript" src="scripts/outofservice.js"></script>
<script language="javascript" type="text/javascript" src="../../../scripts/scw.js"></script>
<script language="javascript" type="text/javascript" src="../../../scripts/ajaxRequest.js"></script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="none, default">
</head>

<body>
<form>
<table>
	<tr>
		<td>
			<b>Select a Date Range:</b>
		</td>
	</tr>
	<tr>
		<td>
			<input type="text"  id = "input_startdate" name="startdate" onclick="scwShow(this,this);" value="<%=request("startdate")%>" readonly>
		</td>
	</tr>
	<tr>
		<td>
			<input type="text"  id = "input_startdate" name="enddate" onclick="scwShow(this,this);" value="<%=request("startdate")%>" readonly>
		</td>
	</tr>
	<tr>
		<td>
			<input type="button" Value="Run Report" onclick="RunReport();">
		</td>
	</tr>
</table>
</form>
<div id="report"></div>

</body>
</html>