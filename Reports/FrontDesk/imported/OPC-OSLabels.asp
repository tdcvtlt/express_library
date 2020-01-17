<html>

<head>
<title>OPC-OS Labels</title>
<meta name="GENERATOR" content="Microsoft FrontPage 6.0">

<script language="javascript" src="../../../scripts/ajaxRequest.js"></script>
<script language="javascript" src="../../../scripts/scw.js"></script>
<script language="javascript" src="scripts/opc-oslabels.js"></script>
<meta name="Microsoft Theme" content="none, default">
</head>

<body><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td><table border="0" cellspacing="0" cellpadding="0" width="100%" height="19"><tr><td align="left" valign="top" width="138">&nbsp;</td><td align="left" valign="top">
	
<form>
	<table>
		<tr>
			<td>Start Date:</td>
			<td><input type = 'text' name = 'sDate' readonly onclick = "scwShow(this, this);"></td>
		</tr>
		<tr>
			<td>End Date:</td>
			<td><input type = 'text' name = 'eDate' readonly onclick = "scwShow(this, this);"></td>
		</tr>
		<tr>
			<td colspan = '2'><input type = 'button' onclick = "go_Report();" value = "Submit"></td>
		</tr>
	</table>
Clicking submit will open a new window in which the labels will be loaded...<br>
<table>
	<tr>
		<td bgcolor="#66FF66" style="color: #66FF66; border-color: #66FF66">...</td>	
		<td>OPC-OS Pre-Booked</td>	
	</tr>
    <tr>
        <td bgcolor="#A020F0" style="color: #A020F0; border-color: #A020F0">...</td>
        <td>Other Pre-Booked</td>
    </tr>
	<tr>
		<td bgcolor="#0066FF" style="color: #0066FF; border-color: #0066FF">...</td>
		<td>RCI Owner</td>
	</tr>
	<tr>
		<td bgcolor="#FFFF00" style="color: #FFFF00; border-color: #FFFF00">...</td>
		<td>II Owner</td>
	</tr>
	<tr>
		<td bgcolor = #000000 style="color: #000000; border-color: #000000">...</td>
		<td>KCP Owner</td>
	</tr>
	<tr>
		<td bgcolor= "#FF99FF" style="color: #FF99FF; border-color: #FF99FF">...</td>	
		<td>Non-Owner</td>
	</tr>
    <tr>
        <td bgcolor="#FF0000" style="color: #FF0000; border-color: #FF0000">...</td>
        <td>Do Not Sell</td>
    </tr>
</table>
<font color = #66FF66></font>
</form>

</body>

</html>
