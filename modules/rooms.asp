<!--#include file="dbconnections.inc" -->
<!--#include file="security.asp" -->
<%
Dim cn 
Dim rs, rs2, rs3, rs4
set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")
set rs2 = server.createobject("ADODB.Recordset")
set rs3 = server.createobject("ADODB.Recordset")
set rs4 = server.createobject("ADODB.Recordset")
cn.Open DBName, DBUser, DBPass


if request("function") = "List" then
'**********Security Check ***********************
	on error resume next
	if not(CheckSecurity("Rooms", "List")) then
		err.description = "<BR><BR><BR><BR><BR><BR><BR><BR><BR>Access Denied"
		err.raise -1
	end if
	if err <> 0 then
		response.write err.description
		err.clear
		cn.close
		set cn = nothing
		response.end
	end if
'**********End Security Check *******************
	If request("Filter") = "" then
		rs.OPen "Select Top 50 RoomID, RoomNumber from t_Room order by roomnumber asc", cn, 3, 3
	Else
		rs.Open "Select Top 50 RoomID, RoomNumber from t_Room where roomnumber like '" & request("Filter") & "%' order by roomnumber", cn, 3, 3
	End If	
	sAns = "<table border = 0><tr><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>RoomID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Room Number</th></tr>"
	
	do while not rs.eof
		sAns = sAns & "<tr>"
		for i = 0 to rs.fields.count -1
			sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & rs.fields(i).value & "&nbsp;</td>"
		next
		sAns = sAns & "<td><a href = 'editRoom.asp?roomid=" & rs.fields("RoomID").value & "'><img src = 'images/edit.gif'></a></td>"
		sAns = sAns & "</tr>"
		rs.movenext
	loop
	sAns = sAns & "</table>"
	rs.close
elseif request("Function") = "LockOut" then
		rs3.open "select comboitem from t_comboitems where (comboitemid = (select unittypeid from t_units where (unitid = (select unitid from t_room where roomid = '" & request("roomid") & "'))))",cn,3,3
		if rs3.fields("comboitem").value = "Cottage" then
			if rs3.bof or rs3.eof then
				response.write "<script>alert('You must assign a Unit First!')</script>"
			else
				sAns = "<table border=0><tr><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>RoomID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>RoomNumber</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>UnitID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>LockOutID</th><th></th></tr>"
				rs.open "select * from t_room where (unitid = (select unitid from t_room where roomid = '" & request("roomid") & "')) and (roomid <> '" & request("roomid") & "')",cn,3,3
				'rs.open "select * from t_room where unitid = '" & request("unitid") & "' and roomid <> '" & request("roomid") & "' union select * from t_room where unitid in (select unitid from t_unit2unit where unit2id = '" & request("unitid") & "') ",cn,3,3
					'if request("unitID") & "" = "" or request("unitID") = "0" then
					'	response.write "<script>alert('You must assign a Unit First!')</script>"
					'else
						if rs.eof and rs.bof then
							sAns = sAns &  "<tr>"
							sAns = sAns & "<td colspan = 4>There are no rooms availabe for LockOut</td>"
							sAns = sAns & "</tr>"
						else
							do while not rs.eof
								sAns = sAns & "<tr>"
								for i = 0 to rs.fields.count -9
									sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & rs.fields(i).value & "&nbsp;</td>"
								next
			 					sAns = sAns & "<td><input type='hidden' name='room'><input type = radio value='" & rs.fields("roomnumber").value & "' onclick = " & chr(34) & "window.opener.document.forms[0].lockout.value = '" & rs.fields("RoomNumber").value & "';window.opener.document.forms[0].lockoutid.value = '" & rs.fields("RoomID").value & "';alert('You have selected a lockout');window.close();" & chr(34) & "></td>"
								sAns = sAns & "</tr>"
								rs.movenext
							loop
							rs.close
						end if
						sAns = sAns & "</table>"
					'end if
			end if	
		elseif rs3.fields("comboitem").value & "" = "" or rs3.fields("comboitem") = "0" then
			response.write "<script>You must have a unit tie first.</script>"
		else
				sAns = "<table border=0><tr><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>RoomID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>RoomNumber</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>UnitID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>LockOutID</th><th></th></tr>"
				rs.open "select * from t_room where (roomid in (select roomid from t_room where unitid in (select unit2id from t_unit2unit where unitid in (select unitid from t_room where roomid = '" & request("roomid") & "'))))",cn,3,3		
						if rs.eof and rs.bof then
							sAns = sAns &  "<tr>"
							sAns = sAns & "<td colspan = 4>There are no rooms availabe for LockOut</td>"
							sAns = sAns & "</tr>"
						else
							do while not rs.eof
								sAns = sAns & "<tr>"
								for i = 0 to rs.fields.count -9
									sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & rs.fields(i).value & "&nbsp;</td>"
								next
			 					sAns = sAns & "<td><input type = radio onclick = " & chr(34) & "window.opener.document.forms[0].lockout.value='" & rs.fields("RoomNumber").value & "';window.opener.document.forms[0].lockoutid.value='" & rs.fields("RoomID").value & "';alert('You have selected a lockout');window.close();" & chr(34) & "></td>"
								sAns = sAns & "</tr>"
								rs.movenext
							loop
							rs.close
						end if
						sAns = sAns & "</table>"

				rs3.close
		end if
elseif request("Function") = "Amens" then
	response.write "<b>Not Yet Implemented</b>"
elseif request("Function") = "MaintReq" then

	sAns = "<table border=0><tr><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>RequestID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>EntryDate</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Room Number</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Subject</th><th></th></tr>"
	if request("roomid") = "" or request("roomid") = "0" then
	else
		rs.open "Select req.requestid, req.entrydate, r.roomnumber,req.subject from t_request req inner join t_room r on r.roomid = req.roomid where r.roomid = '" & request("roomid") & "'",cn,3,3
		do while not rs.eof
			sAns = sAns & "<tr>"
			for i = 0 to rs.fields.count -1
				sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & rs.fields(i).value & "&nbsp;</td>"
			next
 			sAns = sAns & "<td><a href = 'Maintenance/editrequest.asp?requestid=" & rs.fields("requestid").value & "'><img src='images/edit.gif'></a></td>"
			sAns = sAns & "</tr>"
			rs.movenext
		loop
		rs.close
	end if
	sAns = sAns & "</table>"

elseif request("Function") = "Load_Room" then
	on error goto 0
	If request("RoomID") = "" or request("RoomID") = "0" then
		cn.begintrans
		rs.Open "Select * from t_Room where 1=2", cn, 3, 3	
		rs.AddNew
	Else
		rs.Open "Select a.*, b.UnitName from t_Room a left outer join t_Units b on a.unitid = b.unitid where roomid = '" & request("RoomID") & "'", cn, 3, 3
	End If

%>
<html>

<head>
<title>Edit Room</title>
<meta name="GENERATOR" content="Microsoft FrontPage 6.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<script language="JavaScript">
<!--
function FP_preloadImgs() {//v1.0
 var d=document,a=arguments; if(!d.FP_imgs) d.FP_imgs=new Array();
 for(var i=0; i<a.length; i++) { d.FP_imgs[i]=new Image; d.FP_imgs[i].src=a[i]; }
}
// -->
</script>


<meta name="Microsoft Theme" content="none, default">
<body onload="FP_preloadImgs(/*url*/'../../images/buttonCB.jpg', /*url*/'../../images/buttonCA.jpg', /*url*/'images/buttonCB.jpg', /*url*/'images/buttonCA.jpg')">

<form>
<table border="0" width="646" id="table1">
<tr>
	<td>RoomID:</td><td><input type = 'text' name = 'roomid' value = '<%=rs.Fields("RoomID")%>' readonly></td>
	<td>Unit:</td>
	<td>
	<%
		if (request("roomid") = "0" or request("roomid") = "") then
			if rs.fields("unitid").value & "" = "" then
	%>
				<Select name = 'unitid'><option></option>
	<%
					rs2.Open "Select unitid, unitname from t_Units order by unitname asc", cn, 3, 3
					DO while not rs2.EOF
						if cstr(request("unitID") & "") = cstr(rs2.fields("unitid")) then
							response.write "<option selected value = '" & rs2.Fields("UnitID").value & "'>" & rs2.Fields("unitName").value & "</option>"
						else
							response.write "<option value = '" & rs2.Fields("UnitID").value & "'>" & rs2.Fields("unitName").value & "</option>"
						end if
						rs2.MoveNext
					Loop
					rs2.Close
	%>
				</Select>
				<a href='editUnit.asp?unitID=<%=request("unitID")%>'><img src="images/edit.gif"></a>
	<%
			
			else
	%>
					<input type = 'text' name = 'unitname' value = '<%=rs.fields("UnitName").value%>' readonly>
					<input type = 'hidden' name = 'unitid' value = '<%=rs.fields("unitid").value%>'>
					<a href='editUnit.asp?unitID=<%=rs.fields("unitID").value%>'><img src="images/edit.gif"></a>
	<%	
			end if			
		elseif (request("roomid") > "0" or request("roomid") <> "") then
			if rs.fields("unitid").value & "" = "" then
	%>
				<Select name = 'unitid'><option></option>
	<%
					rs2.Open "Select unitid, unitname from t_Units order by unitname asc", cn, 3, 3
					DO while not rs2.EOF
	%>
						<option value = '<%=rs2.Fields("UnitID")%>'><%=rs2.Fields("unitName")%></option>
	<%
						rs2.MoveNext
					Loop
					rs2.Close
	%>
				</Select>
				<a href='editUnit.asp?unitID=<%=request("unitID")%>'><img src="images/edit.gif"></a>
	<%	
				
			else
	%>
					<input type = 'text' name = 'unitname' value = '<%=rs.fields("UnitName").value%>' readonly>
					<input type = 'hidden' name = 'unitid' value = '<%=rs.fields("unitid").value%>'>
					<a href='editUnit.asp?unitID=<%=rs.fields("unitID").value%>'><img src="images/edit.gif"></a>
	<%	
			end if	
		end if
	%>
			
	</td>

</tr>
<tr>
	<td>Room Number:</td><td><input type = 'text' name = 'roomnumber' value = '<%=rs.Fields("RoomNumber")%>'></td>
	<td>Lockout:</td><td>
		<%

		if rs.fields("lockoutid").value & "" = "" or rs.fields("lockoutid").value = "0" then
		%> <input type="hidden" name="lockoutid" value="0"><input type = 'text' name = 'lockout' readonly value =""><input type = button name = 'lock' value = '...' onclick="if ('<%=rs.fields("unitid").value & ""%>'=='0' || '<%=rs.fields("unitid").value & ""%>'==''){alert('Please assign a room and unit first');}else{popitup('modules/rooms.asp?function=LockOut&roomid=' + document.forms[0].roomid.value);}"> <%
		else
			rs4.open "select * from t_room where roomid = '" & rs.fields("lockoutid").value & "'",cn,0,1
			if rs4.eof and rs4.bof then
		%> <input type="hidden" name="lockoutid" value="0"><input type = 'text' name = 'lockout' readonly value ="none cannot find with id of: <%=rs.fields("lockoutid").value%>"><input type = button name = 'lock' value = '...' onclick="if ('<%=rs.fields("unitid").value & ""%>'=='0' || '<%=rs.fields("unitid").value & ""%>'==''){alert('Please assign a room and unit first');}else{popitup('modules/rooms.asp?function=LockOut&roomid=' + document.forms[0].roomid.value);}"> 
			<%else%> <input type="hidden" name="lockoutid" value="<%=rs.fields("lockoutid").value%>"><input type = 'text' name = 'lockout' readonly value ="<%=rs4.fields("roomnumber").value%>"><input type = button name = 'lock' value = '...' onclick="if ('<%=rs.fields("unitid").value & ""%>'=='0' || '<%=rs.fields("unitid").value & ""%>'==''){alert('Please assign a room and unit first');}else{popitup('modules/rooms.asp?function=LockOut&roomid=' + document.forms[0].roomid.value);}"> <%
			end if
			rs4.close
			
		end if
		
		%>
</td></tr>
</tr>
<tr>
	<td>Type:</td><td><select name = 'typeid'>
	<%
		rs2.OPen "Select ComboItemID, ComboItem from t_ComboItems where comboname = 'RoomType' order by comboitem asc", cn, 3, 3
		Do while not rs2.EOF
			if rs2.Fields("ComboItemID") = rs.Fields("TypeID") then
			%>
				<option value = '<%=rs2.Fields("ComboItemID")%>' selected><%=rs2.Fields("ComboItem")%></option>
			<%
			else
			%>
				<option value = '<%=rs2.Fields("ComboItemID")%>'><%=rs2.Fields("ComboItem")%></option>
			<%
			end if
			rs2.MoveNext
		Loop
		rs2.Close
	%>				
	</select></td>
	<td>SubType:</td><td><select name = 'subtypeid'><option value = 0></option>
	<%
		rs2.OPen "Select ComboItemID, ComboItem from t_ComboItems where comboname = 'RoomSubType' and active = 1 order by comboitem asc", cn, 3, 3
		Do while not rs2.EOF
			if rs2.Fields("ComboItemID") = rs.Fields("SubTypeID") then
			%>
				<option value = '<%=rs2.Fields("ComboItemID")%>' selected><%=rs2.Fields("ComboItem")%></option>
			<%
			else
			%>
				<option value = '<%=rs2.Fields("ComboItemID")%>'><%=rs2.Fields("ComboItem")%></option>
			<%
			end if
			rs2.MoveNext
		Loop
		rs2.Close
	%>				
	</select></td>
</tr>
<tr>
	<td>Status:</td>
	<td><select name = 'statusid'><option value = "0"></option>
	<%
		rs2.OPen "Select ComboItemID, ComboItem from t_ComboItems where comboname = 'RoomStatus' order by comboitem asc", cn, 3, 3
		Do while not rs2.EOF
			if rs2.Fields("ComboItemID") = rs.Fields("StatusID") then
			%>
				<option value = '<%=rs2.Fields("ComboItemID")%>' selected > <%=rs2.Fields("ComboItem")%></option>
			<%
			else
			%>
				<option value = '<%=rs2.Fields("ComboItemID")%>'><%=rs2.Fields("ComboItem")%></option>
			<%
			end if
			rs2.MoveNext
		Loop
		rs2.Close
	%>				
	</select></td>	
	<td>Status Date:</td><td><input type = 'text' name = 'statusdate' value = '<%if IsNull(rs.Fields("StatusDate")) or rs.Fields("StatusDate") = "" then response.write Date else response.write rs.Fields("StatusDate") end if%>'></td>
</tr>
<tr>
	<td>Maint. Status:</td>
	<td><select name = 'maintenancestatusid'><option value = "0"></option>
	<%
		rs2.OPen "Select ComboItemID, ComboItem from t_ComboItems where comboname = 'RoomMaintenanceStatus' order by comboitem asc", cn, 3, 3
		Do while not rs2.EOF
			if rs2.Fields("ComboItemID") = rs.Fields("MaintenanceStatusID") then
			%>
				<option value = '<%=rs2.Fields("ComboItemID")%>' selected > <%=rs2.Fields("ComboItem")%></option>
			<%
			else
			%>
				<option value = '<%=rs2.Fields("ComboItemID")%>'><%=rs2.Fields("ComboItem")%></option>
			<%
			end if
			rs2.MoveNext
		Loop
		rs2.Close
	%>				
	</select></td>	
	<td>Maint. Status Date:</td><td><input type = 'text' name = 'maintenancestatusdate' value = '<%if IsNull(rs.Fields("MaintenanceStatusDate")) or rs.Fields("MaintenanceStatusDate") = "" then response.write Date else response.write rs.Fields("MaintenanceStatusDate") end if%>'></td>
</tr>
<tr>
	<td>Extension:</td><td><input type = 'text' name = 'phone' value = '<%=rs.Fields("Phone")%>'></td>
	<td>Max Occupancy:</td><td><input type = 'text' name= 'maxoccupancy' maxlength = '2' value='<%=rs.Fields("maxoccupancy").value%>'></td>
</tr>
<tr>
	<td></td>
</tr>
</table>
</form>

</body>

</html>
<%
	if request("RoomID") = "" or request("RoomID") = "0"  then
		cn.rollbacktrans
	end if
	'rs.Close
elseif request("function") = "Save" then
	on error resume next
	dim roomid
	if CheckSecurity("Rooms", "Edit") then
		cn.begintrans
		if request("roomid") = "" or request("roomid") = "0" then
			rs.open "select * from t_room where 1=2 ", cn, 1, 3
			rs.addnew
			rs.fields("StatusDate").value = now
			bNew = True
		else
			rs.open "Select * from t_Room where roomid = '" & request("roomid") & "'", cn, 1,3
			if rs.eof and rs.bof then
				rs.addnew
				bNew = true
			end if	
		end if
		rs.fields("RoomNumber").value = request("roomnumber")
		rs.fields("Unitid").value = request("unitid")
		rs.fields("LockOutID").value = request("lockoutid")
		rs.fields("LocationID").value = 1
		rs.fields("StatusID").value = request("statusid")
		'if cstr(rs.fields("MaintenanceStatusID").value & "") <> cstr(request("MaintenanceStatusID")) then rs.fields("MaintenanceStatusDate").value = now
		rs.fields("MaintenanceStatusID").value = request("MaintenanceStatusID")
		rs.fields("Phone").value = request("phone")
		rs.fields("TypeID").value = request("typeid")
		rs.fields("SubTypeID").value = request("subtypeid")
		if request("maxoccupancy") = "" then 
			rs.fields("MaxOccupancy").value = 0
		else
			rs.fields("MaxOccupancy").value = request("maxoccupancy")
		end if
		rs.update
		roomid = rs.fields("roomid").value
		rs.MoveFirst
		for i = 0 to rs.fields.count -2
			if rs.fields(i).name = "RoomID" or rs.Fields(i).name = "rowguid" or rs.fields(i).name = "LocationID" or rs.Fields(i).name = "ResLocationID" then
			else
				if cstr(rs.fields(i).value & "") <> cstr(request(rs.fields(i).name) & "") then
					if not bNew then 
						'create event
						if (right(rs.fields(i).name, 2) = "ID" and rs.fields(i).name <> "LocationID" and rs.Fields(i).name <> "ProspectiD" and rs.Fields(i).name <> "TourID" and rs.Fields(i).name <> "PackageIssuedID") then 
							Create_Event "RoomID",request("RoomID"),Get_Lookup(rs.fields(i).value &""),Get_Lookup(request(rs.fields(i).name)),"Change",rs.fields(i).name
						Else
							Create_Event "RoomID",request("RoomID"),rs.fields(i).value,request(rs.fields(i).name),"Change",rs.fields(i).name
						end if
					end if
					'Update
					'rs.fields(i).value = request(rs.fields(i).name)
				else
				end if
			end if
		next 
		rs.close
		
		if err <> 0 then
			cn.rollbacktrans
			sAns = "ERROR " & err.number & " - " & err.description
			err.clear			
		else
			cn.committrans
			sAns = "Saved" 
		end if
	'elseif CheckSecurity("Rooms", "EditStatus") then
	'	if request("roomid") = "0" or request("roomid") = "" then
	'		sAns = "Access Denied"
	'	else
	'		cn.BeginTrans
	'		rs.Open "Select StatusID, StatusDate from t_Room where roomid = '" & request("roomID") & "'", cn, 3, 3
	'		if rs.Fields("StatusID") <> request("StatusID") then
	'			Create_Event "RoomID",request("RoomID"),Get_Lookup(rs.fields("StatusID").value &""),Get_Lookup(request("StatusID")),"Change","StatusID"
	'			rs.Fields("StatusID") = request("StatusID")
	'			rs.Fields("StatusDate") = Now
	'		end if
	'		rs.UpdateBatch
	'		rs.Close
	'		if err <> 0 then
	'			cn.rollbacktrans
	'			sAns = "ERROR " & err.number & " - " & err.description
	'			err.clear			
	'		else
	'			cn.committrans
	'			sAns = "Saved" 
	'		end if
	'	end if
	else
		if (request("roomid") = "0" or request("roomid") = "") or (Not(CheckSecurity("Rooms", "EditStatus"))  and Not(CheckSecurity("Rooms","EditMaintStatus"))) then
			sAns = "Access Denied"
		else
			cn.BeginTrans
			rs.Open "Select StatusID, StatusDate from t_Room where roomid = '" & request("roomID") & "'", cn, 3, 3
			If CSTR(rs.Fields("StatusID")) <> CSTR(request("StatusID")) then
				if Not(CheckSecurity("Rooms","EditStatus")) then
					sAns = "You do Not Have Access to Edit Room Status."
				else
					Create_Event "RoomID",request("RoomID"),Get_Lookup(rs.fields("StatusID").value &""),Get_Lookup(request("StatusID")),"Change","StatusID"
					rs.Fields("StatusID") = request("StatusID")
					rs.Fields("StatusDate") = Now
				end if
			End If				
			rs.UPdateBatch
			rs.Close
			
			if sAns = "" then
				rs.OPen "Select MaintenanceStatusID, MaintenanceStatusdate from t_Room where roomid = '" & request("roomID") & "'", cn, 3, 3
				if CSTR(rs.fields("MaintenanceStatusID").value) <> CSTR(request("MaintenanceStatusID")) then
					if Not(CheckSecurity("Rooms","EditMaintStatus")) then
						sAns = "You Do Not Have Permission to Edit Maintenance Status."
					else
						Create_Event "RoomID",request("RoomID"),Get_Lookup(rs.fields("MaintenanceStatusID").value &""),Get_Lookup(request("MaintenanceStatusID")),"Change","MaintenanceStatusID"
						rs.Fields("MaintenanceStatusID") = request("MaintenanceStatusID")
						rs.Fields("MaintenanceStatusDate") = Now
					end if
				end if
				rs.UpdateBatch
				rs.Close
			end if								

			if sAns = "" then
				cn.CommitTrans
				sAns = "Saved"
			Else
				cn.RollBackTrans
			End If
		end if
	end if
end if


cn.Close
set rs = Nothing
set rs2 = Nothing
set cn = Nothing

response.write sAns
%>