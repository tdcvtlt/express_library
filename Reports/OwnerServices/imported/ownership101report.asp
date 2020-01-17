<!--#include file="../../includes/dbconnections.inc" -->
<!--#include file="../../modules/security.asp" -->
<html>

<head>
<title>OwnerShip Report</title>
<script language="javascript" src="scripts/ownership101report.js"></script>
<script language="javascript" src="../../TestPages/scripts/scw.js"></script>
<script language="javascript" src="../../TestPages/scripts/ajaxRequest.js"></script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="none, default">
</head>


<body>
<form>
	<table>
		<tr>
			<td>
				Date Range
			</td>
		</tr>
		<tr>
			<td>
				Start Date
			</td>
			<td>
				<input type="text" name="startdate" value='<%=request("startdate")%>' onclick="scwShow(this,this);" readonly>
			</td>
		</tr>
		<tr>
			<td>
				End Date
			</td>
			<td>
				<input type="text" name="enddate" value='<%=request("enddate")%>' onclick="scwShow(this,this);" readonly>
			</td>
		</tr>
		<tr>
			<td>
				<input type="button" value="Run Report" onclick="Run_Report();">
			</td>
		</tr>
	</table>
</form>
<div id ="Report"></div>
</body></html>