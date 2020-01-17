
<html>

<head>
<title>Add Room Wizard</title>
<link type="text/css" rel="Stylesheet" href="../styles/master.css" />
<script type ="text/javascript" language = "javascript" src = "../scripts/rooms.js"></script>
<script type ="text/javascript" language = "javascript" src = "../scripts/general.js"></script>
<script type ="text/javascript" language = "javascript" src = "../scripts/ajaxRequest.js"></script>
</head>
<%

	DIm cn
	Dim rs
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	DBName = "CRMSNet"
	DBUser = "asp"
	DBPass = "aspnet"
	cn.OPen DBName, DBUser, DBPass
	
	rs.OPen "Select b.ComboItem as resType from t_Reservations a left outer join t_ComboItems b on a.typeid = b.comboitemid where reservationid = '" & request("reservationid") & "'", cn, 3, 3
	resType = rs.Fields("resType")
	rs.Close
%>
<body> <!-- onload="FP_preloadImgs(/*url*/'images/button143.jpg',/*url*/'images/button144.jpg',/*url*/'images/buttonE11.jpg',/*url*/'images/buttonE12.jpg',/*url*/'images/buttonE13.jpg',/*url*/'images/buttonE14.jpg',/*url*/'images/buttonE16.jpg',/*url*/'images/buttonF12.jpg',/*url*/'images/button226.jpg',/*url*/'images/button227.jpg')">-->
<form>
<center><b>Add Room Wizard</b></center><br>
<%
	if resType = "Exchange" or resType = "Points" or resType = "NALJR" Then
%>
		<input type = 'radio' name = 'filtertype' value = 'ownerfilter' onclick = "show_Owner();" /> Owner Usage <input type = 'radio' name = 'filtertype' value = 'roomfilter' onclick = "show_Room();"> Room Filter
<%		
	elseif resType = "Rental" or resType = "Developer" or resType = "Vendor" Then
%>
		<input type = 'radio' name = 'filtertype' value = 'roomfilter' onclick = "show_Room();" /> Room Filter
<%
	elseif resType = "Marketing" Then
%>
		<input type = 'radio' name = 'filtertype' value = 'ownerfilter' onclick = "show_Owner();" /> Owner Usage <input type = 'radio' name = 'filtertype' value = 'roomfilter' onclick = "show_Room();"> Room Filter
<%
	elseif resType = "Owner" or resType = "TrialOwner" or resType = "PlanWithTan" Then
%>
		<input type = 'radio' name = 'filtertype' value = 'ownerfilter' onclick = "show_Owner();document.forms[0].ownusagetype[0].checked=true;document.getElementById('usageyr').style.top='30px';document.getElementById('diffowner').style.visibility='hidden';" /> Owner Usage <input type = 'radio' name = 'filtertype' value = 'roomfilter' onclick = "show_Room();document.getElementById('diffowner').style.visibility ='hidden';" /> Room Filter
<%
	end if
%>
<br />
<div id = "rooms" style = "visibility:hidden;position:absolute; left:16px; top:165px"></div>
<div id = "ownersearch" style="position: absolute; left:11px; top:92px; visibility:hidden">
<%
	if resType = "Exchange" or resType = "Points" or resType = "NALJR" Then
%>
		<ul id = "menu"><li><a href = "javascript:void(switch_Disp('name'));">name</a></li></ul><!--<img border="0" id="img2" src="images/button33.jpg" height="20" width="100" alt="By Name" onmouseover="FP_swapImg(1,0,/*id*/'img2',/*url*/'images/button34.jpg')" onmouseout="FP_swapImg(0,0,/*id*/'img2',/*url*/'images/button33.jpg')" onmousedown="FP_swapImg(1,0,/*id*/'img2',/*url*/'images/button35.jpg')" onmouseup="FP_swapImg(0,0,/*id*/'img2',/*url*/'images/button34.jpg')" fp-style="fp-btn: Border Bottom 1" fp-title="By Name" onclick = "switch_Disp('name');"><img border="0" id="img1" src="images/button145.jpg" height="20" width="100" alt="II Number" onmouseover="FP_swapImg(1,0,/*id*/'img1',/*url*/'images/button143.jpg')" onmouseout="FP_swapImg(0,0,/*id*/'img1',/*url*/'images/button145.jpg')" onmousedown="FP_swapImg(1,0,/*id*/'img1',/*url*/'images/button144.jpg')" onmouseup="FP_swapImg(0,0,/*id*/'img1',/*url*/'images/button143.jpg')" fp-style="fp-btn: Border Bottom 1" fp-title="II Number" onclick = "switch_Disp('iimember');"><img border="0" id="img3" src="images/buttonDF1.jpg" height="20" width="100" alt="RCI Number" onmouseover="FP_swapImg(1,0,/*id*/'img3',/*url*/'images/buttonE11.jpg')" onmouseout="FP_swapImg(0,0,/*id*/'img3',/*url*/'images/buttonDF1.jpg')" onmousedown="FP_swapImg(1,0,/*id*/'img3',/*url*/'images/buttonE12.jpg')" onmouseup="FP_swapImg(0,0,/*id*/'img3',/*url*/'images/buttonE11.jpg')" fp-style="fp-btn: Border Bottom 1" fp-title="RCI Number" onclick = "switch_Disp('rcimember');"><img border="0" id="img4" src="images/buttonE15.jpg" height="20" width="100" alt="ICE Number" onmouseover="FP_swapImg(1,0,/*id*/'img4',/*url*/'images/buttonE13.jpg')" onmouseout="FP_swapImg(0,0,/*id*/'img4',/*url*/'images/buttonE15.jpg')" onmousedown="FP_swapImg(1,0,/*id*/'img4',/*url*/'images/buttonE14.jpg')" onmouseup="FP_swapImg(0,0,/*id*/'img4',/*url*/'images/buttonE13.jpg')" fp-style="fp-btn: Border Bottom 1" fp-title="ICE Number" onclick = "switch_Disp('icemember');"><img border="0" id="img6" src="images/button228.jpg" height="20" width="114" alt="RCI Points Number" onmouseover="FP_swapImg(1,0,/*id*/'img6',/*url*/'images/button226.jpg')" onmouseout="FP_swapImg(0,0,/*id*/'img6',/*url*/'images/button228.jpg')" onmousedown="FP_swapImg(1,0,/*id*/'img6',/*url*/'images/button227.jpg')" onmouseup="FP_swapImg(0,0,/*id*/'img6',/*url*/'images/button226.jpg')" fp-style="fp-btn: Border Bottom 1; fp-proportional: 0" fp-title="RCI Points Number" onclick = "switch_Disp('rcimember');"><br><br>-->
		<span id = "display">Owners Name:</span><input type = 'text' name = 'filter'>    Usage Year: <select name = 'usageyear'>
		
		<%
			for i = 2006 to 2012
		%>
				<option value = '<%=i%>'><%=i%></option>
		<%
			next 
		%>
		</select> 
		<input type = button name = 'go' value = 'Search' onclick = "if (val_Owner('exchange')){val_Owner_Usage();}">
<%		
	elseif resType = "Rental" Then
%>
		<span id = "display">Owners Name:</span><input type = 'text' name = 'filter'>    Usage Year: <select name = 'usageyear'>
		<%
			for i = 2006 to 2012
		%>
				<option value = '<%=i%>'><%=i%></option>
		<%
			next 
		%>
		</select>
		<input type = button name = 'go' value = 'Search' onclick = "if (val_Owner('rental')){val_Owner_Usage();}">
<%
	elseif resType = "Marketing" Then
%>
		<input type = 'radio' name = 'filtertype' value = 'ownerfilter'> Owner Usage <input type = 'radio' name = 'filtertype' value = 'roomfilter'> Room Filter
		<input type = button name = 'go' value = 'Search'>
<%
	elseif resType = "Owner" or resType = "TrialOwner" or resType = "PlanWithTan" Then
%>
		<input type = 'radio' name = 'ownusagetype' value = 'sameowner' onclick = "document.getElementById('diffowner').style.visibility = 'hidden';document.getElementById('usageyr').style.left = '2px';document.getElementById('usageyr').style.top = '30px';document.getElementById('display').innerHTML='';document.getElementById('rooms').innerHTML = '';" checked = "checked" />Same Owner <input type = 'radio' name = 'ownusagetype' value = 'diffowner' onclick = "document.getElementById('diffowner').style.visibility = 'visible';document.getElementById('usageyr').style.left = '2px';document.getElementById('usageyr').style.top = '73px';document.getElementById('display').innerHTML='Name:';document.getElementById('rooms').innerHTML = '';document.forms[0].filter.value = '';" />Different Owner
		<div id = 'diffowner' style = 'position:rel; left:7px; top:29px;visibility:hidden; width:576px'>
		<!--<img border="0" id="img2" src="images/button33.jpg" height="20" width="100" alt="By Name" onmouseover="FP_swapImg(1,0,/*id*/'img2',/*url*/'images/button34.jpg')" onmouseout="FP_swapImg(0,0,/*id*/'img2',/*url*/'images/button33.jpg')" onmousedown="FP_swapImg(1,0,/*id*/'img2',/*url*/'images/button35.jpg')" onmouseup="FP_swapImg(0,0,/*id*/'img2',/*url*/'images/button34.jpg')" fp-style="fp-btn: Border Bottom 1" fp-title="By Name" onclick = "switch_Disp('name');"><img border="0" id="img5" src="images/buttonD18.jpg" height="20" width="100" alt="By KCP#" onmouseover="FP_swapImg(1,0,/*id*/'img5',/*url*/'images/buttonE16.jpg')" onmouseout="FP_swapImg(0,0,/*id*/'img5',/*url*/'images/buttonD18.jpg')" onmousedown="FP_swapImg(1,0,/*id*/'img5',/*url*/'images/buttonF12.jpg')" onmouseup="FP_swapImg(0,0,/*id*/'img5',/*url*/'images/buttonE16.jpg')" fp-style="fp-btn: Border Bottom 1" fp-title="By KCP#" onclick = "switch_Disp('contract');"> <br>-->
		<span id = "display"></span><input type = 'text' name = 'filter' />
		</div>
		 <div id ="usageyr" style = "position:absolute;left=2px;top=30px">
		 Usage Year: <select name = 'usageyear'>
		<%
			for i = 2006 to 2012
		%>
				<option value = '<%=i%>'><%=i%></option>
		<%
			next 
		%>
		</select><input type = "button" name = "go" value = "Search" onClick = "val_Owner_Usage();" />
		</div>
<%
	end if
%>	
</div>

<div id = "roomsearch" style = "position: absolute; left:11px; top:92px; visibility:hidden">
<%
	if resType = "Exchange" or resType = "Owner" or resType = "NALJR" or resType = "Marketing" or resType = "Rental" or resType = "TrialOwner" or resType = "Developer" or resType = "PlanWithTan" or resType = "Vendor" Then
%>
		BD: <select name = 'BD' onchange = "document.getElementById('rooms').innerHTML = '';">
		<option value = '1'>1</option>
		<option value = '1BD-DWN'>1BD-DWN</option>
		<option value = '1BD-UP'>1BD-UP</option>
		<option value = '2'>2</option>
		<option value = '3'>3</option>
		<option value = '4'>4</option>
		</select> 
		
		Unit Type: <select name = 'unittype' onchange = "document.getElementById('rooms').innerHTML = '';">
		<%
			rs.OPen "Select ci.comboitemid, ci.comboitem from t_ComboItems ci inner join t_Combos c on ci.comboid = c.comboid where ci.active = 1 and c.comboname = 'UnitType'", cn, 3, 3
			Do while not rs.EOF
			%>
				<option value = '<%=rs.Fields("ComboItemID")%>'><%=rs.Fields("COmboItem")%></option> 
			<%
				rs.MoveNext
			Loop
			rs.Close
		%>
		</select>
		
		Inv Type: <select name = 'invtype' onchange = "document.getElementById('rooms').innerHTML = '';">
		<%
			rs.OPen "Select ci.comboitemid, ci.comboitem from t_ComboItems ci inner join t_Combos c on ci.comboid = c.comboid where ci.active = 1 and c.comboname = 'ReservationType'", cn, 3, 3
			Do while not rs.EOF
				if cstr(resType) = cstr(rs.Fields("ComboItem")) then
				%>
					<option selected = "selected" value = '<%=rs.Fields("ComboItemID")%>'><%=rs.Fields("ComboItem")%></option>
				<%
				else
				%>
					<option value = '<%=rs.Fields("ComboItemID")%>'><%=rs.Fields("ComboItem")%></option>
				<%
				end if
				rs.MoveNext
			Loop
			rs.Close
		%>
		</select> Spares:<input type = 'checkbox' name = 'searchspare' id = 'searchspare' /><input type = "button" name = 'go' value = 'Search' onclick = "get_Avail_Rooms('<%=resType%>');" />
<%		
	elseif resType = "Rental" Then
%>
		
		BD: <select name = 'BD'>
		<option value = '1'>1</option>
		<option value = '1BD-DWN'>1BD-DWN</option>
		<option value = '1BD-UP'>1BD-UP</option>
		<option value = '2'>2</option>
		<option value = '3'>3</option>
		<option value = '4'>4</option>
		</select> 
		
		Unit Type: <select name = 'unittype'>
		<%
			rs.OPen "Select comboitemid, comboitem from t_ComboItems where active = 1 and comboname = 'UnitType'", cn, 3, 3
			Do while not rs.EOF
			%>
				<option value = '<%=rs.Fields("ComboItemID")%>'><%=rs.Fields("COmboItem")%></option> 
			<%
				rs.MoveNext
			Loop
			rs.Close
		%>
		</select>
		
		Inventory: <select name = 'invtype'>
		<%
			rs.Open "Select comboitemid, comboitem from t_ComboItems where active = '1' and comboname = 'ReservationType' order by comboitem asc", cn, 3, 3	
			Do while not rs.EOF
				if resType = rs.Fields("ComboItem") then
				%>
					<option selected value = '<%=rs.Fields("ComboItemID")%>'><%=rs.Fields("ComboItem")%></option>
				<%
				else
				%>
					<option selected value = '<%=rs.Fields("ComboItemID")%>'><%=rs.Fields("ComboItem")%></option>
				<%
				end if
				rs.MoveNext
			Loop
			rs.Close
		%>
		</select>
		Search Spares:<input type = 'checkbox' name = 'searchspare' /><input type = "button" name = "go" value = "Search" onclick = "get_Avail_Rooms('Rental');" />
		
		<!--Category Usage: <select name = 'category'>
		<option value = '0'>ALL</option>
		<option value = '1'>1</option>
		<option value = '2'>2</option>
		<option value = '3'>3</option>
		<option value = '4'>4</option>
		</select> --> 

		
<%
	end if
%>	
</div>
<input type = 'hidden' name = 'resID' value = '<%=request("reservationid")%>' />
<input type = 'hidden' name = 'extend' value = '' />
<input type = 'hidden' name = 'newOutDate' value = '' />
</form>
</body></html>
