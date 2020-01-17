<html>

<head>
<title>Owner Notebook</title>
<meta name="GENERATOR" content="Microsoft FrontPage 6.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<script language="javascript" src = "../../../scripts/ajaxRequest.js"></script>
<script language="javascript" src = "scripts/OwnerNotebook.js"></script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="none, default">
</head>

<body>
	<table>
		<tr>
			<td colspan = '2'><input type = 'button' name = 'go' value = 'Run Report' onclick = "go_Report();">
            <input type="button" value="Printable Version" name="B1" onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('report').innerHTML);"></td>
		</tr>
	</table>
<div id = "report"></div>
</body>

</html>
