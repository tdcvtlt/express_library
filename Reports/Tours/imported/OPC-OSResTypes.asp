<html>

<head>
<title>OPC-OS Res Types</title>
<meta name="GENERATOR" content="Microsoft FrontPage 6.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<script language="Javascript" src="../../../scripts/ajaxRequest.js"></script>
<script language="Javascript" src="scripts/OPC-OSResTypes.js"></script>
<script language="javascript" src = "../../../scripts/scw.js"></script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="none, default">
</head>

<body>
<form>
<table>
<tr>
	<td>Start Date:</td>
	<td><input type = 'text' name = 'sDate' onClick = "scwShow(this, this);" readonly></td>
</tr>
<tr>
	<td>End Date:</td>
	<td><input type = 'text' name = 'eDate' onclick = "scwShow(this, this);" readonly></td>
</tr>
<tr>
	<td colspan = '2'><input type = 'button' name = 'go' value = 'Submit' onclick = "run_Report();"><input type="button" value="Printable Version" name="B1" onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('report').innerHTML);"></td>
</tr>
</table>

<div id = 'report'></div>
</form>

</body>

</html>
