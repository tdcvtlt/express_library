
<html>

<head>
<title>Master Inventory List</title>
<script type="text/javascript" src="../../scripts/ajaxRequest.js"></script>
<script type="text/javascript" src="scripts/MasterInventoryList.js"></script>
<script type="text/javascript" src="scripts/PurchaseRequests.js"></script>
<script type="text/javascript" src="../../scripts/jquery-1.7.1.min.js"></script>

</head>
<!--#include file="modules/dbconnections.inc" -->
<!--#include file="modules/GPInventory.asp" -->
<!--#include file="modules/PartAssignment.asp" -->
<body>
<form name = 'filtersearch'>
<table>
	<tr>
	<td>Filter 1:</td>
		<td><select name = 'filter1' onchange = "get_Sub_Filters('1', '0');">
			<option value = '0'>ALL</option>
			<%
			set newFilter = new PartAssignment
			filters = Split(newFilter.FilterParts(0, ""), "|")
			For i = 0 to UBound(filters)						
			%>	
					<option value = '<%=filters(i)%>'><%=filters(i)%></option>
			<%
			next 
			%>					
			</select>
		</td>
	</tr>
	<tr>
		<td>Filter 2:</td>
		<td><select name = 'filter2' onchange = "get_Sub_Filters('2', '0');">
				<option value = '0'></option>
			</select>
		</td>
	</tr>
	<tr>
		<td>Filter 3:</td>
		<td><select name = 'filter3' onchange = "get_Sub_Filters('3', '0');">
				<option value = '0'></option>
			</select>
		</td>
	</tr>
	<tr>
		<td>Filter 4:</td>
		<td><select name = 'filter4'>
				<option value = '0'></option>
			</select>
		</td>
	</tr>
</table>
<input type = button name = 'go' value = 'Search' onClick = "run_Report(); disabled"><input type = 'button' id="goE" value = 'Excel' disabled onClick = "run_Report_Excel();"><input type = 'button' name = 'printRpt' value = 'Printable Version' disabled onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('report').innerHTML);">
<div id = 'report'></div>
</form>


<script type="text/javascript">

    $(function () {

        
    });
</script>

</body>

</html>
