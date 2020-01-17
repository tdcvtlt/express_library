<!--#include file="security.asp" -->
<!--#include file="dbconnections.inc" -->
<%

dim cn 
dim rs, rs2, rs3, rsR, rsG
dim sAns
dim i
dim sSQL
dim myCDO

set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")
set rs2 = server.createobject("ADODB.Recordset")
set rs3 = server.createobject("ADODB.Recordset")
set rsR = server.createobject("ADODB.Recordset")
set rsG = server.createobject("ADODB.Recordset")

cn.CommandTimeout = 0
server.scripttimeout = 10000
'cn.ConnectionString = "data source=RS-SQL-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096;"
cn.open DBName, DBUser, DBPass
'cn.OPen "CRMSNet", "asp", "aspnet"

function new_Usage_ID (resID, roomID, resTypeID, inDate, outDate)
	'set cn2 = server.createobject("ADODB.Connection")
	'cn2.Open DBName, DBUser, DBPass
	
	rs.Open "Select ComboItemID from t_ComboItems where comboname = 'UsageStatus' and comboitem = 'Used'", cn, 3, 3
	uStatusID = rs.Fields("ComboItemID")
	rs.Close

	rs.Open "Select comboitemid from t_CombOItems where comboname = 'ReservationType' and comboitem = 'Spare'", cn, 3, 3
	spareID = rs.Fields("ComboItemID")
	rs.close
	
	rs.OPen "Select a.TypeID, a.UnitID, b.typeid from t_Room a inner join t_Unit b on a.unitid = b.unitid where roomid = '" & roomID & "'", cn, 3, 3
	rTypeID = rs.Fields("TypeID")
	uTypeID = rs.FIelds("typeid")
	rs.Close
	
	days = datediff("d",inDate, outDate)
	
	rs.Open "Select contractid from t_Contract where contractnumber = 'KCPSPARE'", cn, 3, 3
	spareconid = rs.Fields("ContractID")
	rs.Close
	
'	cn2.BeginTrans
	rs.OPen "Select * from t_Usage where 1=2", cn, 3, 3
	rs.AddNew
	rs.Fields("TypeID") = resTypeID
	rs.Fields("UsageYear") = Year(Date)
	rs.Fields("ContractID") = spareconid
	rs.Fields("RoomTypeID") = rTypeID
	rs.Fields("Days") = days
	rs.Fields("typeid") = uTypeID
	rs.FIelds("InDate") = inDate
	rs.Fields("outDate") = outDate
	rs.Fields("DateCreated") = Now
	rs.Fields("StatusID") = uStatusID
	rs.Updatebatch
	rs.MoveFirst
	usageID = rs.Fields("usageID")
	rs.Close
	
	'Update roomallocation matrix to assign room to new usage
	rs.Open "Select * from t_RoomAllocationMatrix where roomid = '" & roomID & "' and dateallocated between '" & inDate & "' and '" & outDate - 1 & "'", cn, 3, 3
	Do while not rs.EOF
		if rs.FIelds("UsageID") <> "0" then 
			error = "true"
		elseif rs.Fields("TypeID") <> spareid then
			error = "true"
		else
			rs.Fields("UsageID") = usageID
		End If
		rs.MoveNext
	Loop
	rs.Close
	
	if error = "true" then
'		cn2.RollBackTrans
		usageID = 0
	else
'		cn2.CommitTrans
	end if

'	cn2.Close
'	set cn2 = Nothing
	new_Usage_ID = usageID

end function


if request("Function") = "Add_Etour" then
	on error resume next
	rs.open "Select * from t_EtourManifest where reservationId = '" & request("ID") & "'", cn, 3, 3
	if rs.eof and rs.bof then
		rs.addnew 
		rs.fields("ReservationID").value = request("ID")
		rs.update
	end if
	rs.close
	cn.close
	set rs  = nothing
	set cn = nothing
	if err <> 0 then
		response.write "NO|" & err.number & "-" & err.description
		err.clear
	else
		response.write "OK"
	end if
	on error goto 0
	response.end
end if
%>



<%



if lcase(request("Submitting") & "") = "true" then
'**********Security Check ***********************
	'on error resume next
	if not(CheckSecurity("Reservations", "Edit")) and request("PackageIssuedID") <> "0" and request("PackageIssuedID") <> "" then
		'err.raise 
		err.number = -1
		err.description = "Your request to overwrite an Reservation record was Denied"
	end if
	if err = 0 then
		if request("PackageIssuedID") = "0" or request("PackageIssuedID") = "" then
			if not(CheckSecurity("Reservations", "Add")) then
				'err.raise 
				err.number = -1
				err.description = "Your request to Create a Reservation record was Denied"
			end if
		end if
	end if	
'**********End Security Check *******************
	cn.begintrans
	if request("ReservationID") = "0" or request("ReservationID") = "" then
		rs.open "Select * from t_Reservations where 1=2",cn,1,3
		rs.addnew
		bNew = True
	else
		rs.open "Select * from t_Reservations where reservationid = " & request("ReservationID"),cn,1,3
		if rs.eof and rs.bof then
			rs.addnew
			bNew = True
		end if
	end if
	for i = 0 to rs.fields.count -1
		if rs.fields(i).name = "ReservationID"  or rs.fields(i).name = "rowguid" or rs.fields(i).name = "StatusDate" or rs.fields(i).name = "Confirmed" or rs.fields(i).name = "DateConfirmed" or rs.fields(i).name = "ConfirmedByID"  then
			'response.write rs.fields(i).name & " = " & request(rs.fields(i).name) & "<BR>"
		elseif rs.fields(i).name = "CheckInDate" or rs.fields(i).name = "CheckOutDate" or rs.fields(i).name = "TypeID" or rs.fields(i).name = "ResLocationID" then 
			if cstr(rs.fields(i).value & "") <> cstr(request(rs.fields(i).name) & "") then
				if not bNew then
					'**********Check to make sure no rooms assigned**************'
					rs2.OPen "Select * from t_RoomAllocationmatrix where reservationid = '" & request("reservationid") & "'", cn, 3, 3
					if rs2.EOF and rs2.BOF then
						If right(rs.fields(i).name, 2) = "ID" then
							Create_Event "ReservationID",request("ReservationID"),Get_Lookup(rs.fields(i).value &""),Get_Lookup(request(rs.fields(i).name)),"Change",rs.fields(i).name						
						else
							Create_Event "ReservationID",request("ReservationID"),rs.fields(i).value,request(rs.fields(i).name),"Change",rs.fields(i).name                  
						end if
					else
						'err.raise 
						err.number = -1
						err.description = "There is a Room assigned to this reservation. Please Remove the Room before Making Changes."
					end if
					rs2.Close
				end if
				rs.fields(i).value = request(rs.fields(i).name)
			end if
		elseif rs.Fields(i).name = "LockInventory" then
			if not bNew then
				'********If Inventory is Locked make sure user has ability to unlock********'
				If rs.Fields("LockInventory") and request("LockInventory") <> "on" then
					If Not(CheckSecurity("Reservations","UnlockInventory")) then
						err.number = -1
						err.description = "You do not have permission to UnLock Inventory"
					Else
						Create_Event "ReservationID",request("ReservationID"),"Locked","UnLocked","Change","Inventory"
					End If
				ElseIf Not(rs.Fields("LockInventory")) and request("LockInventory") = "on" then
					Create_Event "ReservationID",request("ReservationID"),"UnLocked","Locked","Change","Inventory"					
				End If
			end if
			if request("LockInventory") = "on" then	
				rs.Fields(i).value = 1
			Else
				rs.Fields(i).value = 0
			End If
		else
			if cstr(rs.fields(i).value & "") <> cstr(request(rs.fields(i).name) & "") then
				if not bNew then 
					'create event
					if (right(rs.fields(i).name, 2) = "ID" and rs.fields(i).name <> "LocationID" and rs.Fields(i).name <> "ProspectiD" and rs.Fields(i).name <> "TourID" and rs.Fields(i).name <> "PackageIssuedID") then 
						Create_Event "ReservationID",request("ReservationID"),Get_Lookup(rs.fields(i).value &""),Get_Lookup(request(rs.fields(i).name)),"Change",rs.fields(i).name
					Else
						Create_Event "ReservationID",request("reservationID"),rs.fields(i).value,request(rs.fields(i).name),"Change",rs.fields(i).name
					end if
					if rs.Fields(i).name = "StatusID" then
						rs.Fields("StatusDate") = Date
					End if
				end if
				'Update
				rs.fields(i).value = request(rs.fields(i).name)
				'response.write rs.fields(i).name & " = " & request(rs.fields(i).name) & "<BR>"
			else
				'response.write rs.fields(i).name & " = " & request(rs.fields(i).name) & " - Same as Default<BR>"
			end if
		end if
	next 

	rs.update
	dim resID
	resID = rs.fields("ReservationID").value
	rs.close
	
	if request("GroupID") <> "" and bNew then
		rs.OPen "Select * from t_Res2Group where 1=2", cn, 3, 3
		rs.AddNew
		rs.Fields("GroupReservationID") = request("GroupID")
		rs.Fields("reservationid") = resID
		rs.Updatebatch
		rs.Close
	end if
	
	if err <> 0 then
		cn.rollbacktrans
		if err.number = -1 then 
			response.write "<script>alert('" & err.description & "');window.history.back(1);</script>"
		else
			response.write "<script>alert('An error occured writing to the database. " & err.number & " - " & err.description & "');</script>"
		end if
		'cn.close
		'set rs = nothing
		'set cn = nothing
	else
		if bNew then
			Create_Event "ReservationID", resID,"","","Create",""
			rs.Open "Select b.ComboItem as Department from t_Personnel a left outer join t_CombOItems b on a.departmentid = b.comboitemid where a.personnelid = '" & Session("UserID") & "'", cn, 3, 3
			If rs.EOF and rs.BOF then
			ElseIf rs.Fields("Department") = "AC Front Desk" then
				sTo = "roommoves@kingscreekplantation.com"
				sFrom = "administrator@kingscreekplantation.com"
				strEmail = "ReservationID " & resID & " created by " & Session("UserName") & ". InDate: " & request("CheckinDate") & ". OutDate " & request("CheckoutDate") & "."
				sBody = strEmail
				sSubject = "Front Desk Reservation Created"
				Send_Mail sFrom, sTo, sSubject, sBody
'				set myCDO = server.createobject("CDONTS.NewMail")
'				mycdo.to = "roommoves@kingscreekplantation.com"
'				mycdo.subject = "Front Desk Reservation Created"
'				mycdo.from = "administrator@kingscreekplantation.com"
'				mycDO.body = strEmail
'				myCDO.send
'				set myCDO = nothing				
			End If
		end if
		cn.committrans
		'cn.rollbacktrans
		cn.close
		set rs = nothing
		set cn = nothing

		if request("reload") = "" or request("reload") = "false" then
			response.redirect "../editreservation.asp?reservationid=" & resID
		else
			if request("Url") = "" then
				response.redirect "../"
			else
				response.redirect request("Url")
			end if
		end if
	end if
	'response.write "ProspectID = " & request("ProspectID")
	'response.end
end if

if request("Function") = "List" then
'**********Security Check ***********************
	on error resume next
	if not(CheckSecurity("Reservations", "List")) then
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
	select case request("Sort")
		case "ID"
			if request("filter") = "" then
				rs.open "select top 50 r.reservationid, reservationnumber, l.comboitem as status, g.lastname + ', ' + g.firstname as Guest, checkindate, checkoutdate,t.comboitem as Type, p.lastname + ', ' + p.firstname as Owner  from t_reservations r left outer join (select g.*, t_res2guest.reservationid from t_Guest g inner join t_Res2Guest on t_Res2Guest.GuestID = g.guestid where t_Res2Guest.PrimaryFlag=1) as g on g.reservationid=r.reservationid left outer join t_Prospect p on p.prospectid = r.prospectid left outer join t_ComboItems rl on rl.comboitemid = reslocationid left outer join t_ComboItems l on l.comboitemid = r.statusid left outer join t_Comboitems t on t.comboitemid = r.typeid order by r.ReservationID",cn,3,3
			else
				rs.open "select top 50 r.reservationid, reservationnumber, l.comboitem as status, g.lastname + ', ' + g.firstname as Guest, checkindate, checkoutdate,t.comboitem as Type, p.lastname + ', ' + p.firstname as Owner  from t_reservations r left outer join (select g.*, t_res2guest.reservationid from t_Guest g inner join t_Res2Guest on t_Res2Guest.GuestID = g.guestid where t_Res2Guest.PrimaryFlag=1) as g on g.reservationid=r.reservationid left outer join t_Prospect p on p.prospectid = r.prospectid left outer join t_ComboItems rl on rl.comboitemid = reslocationid left outer join t_ComboItems l on l.comboitemid = r.statusid left outer join t_Comboitems t on t.comboitemid = r.typeid where r.reservationid like '" & replace(request("Filter"),"'","''") & "%' order by r.ReservationID",cn,3,3
			end if	
		case "Guest"
			if request("filter") = "" then
				rs.open "select top 50 r.reservationid, reservationnumber, l.comboitem as status, g.lastname + ', ' + g.firstname as Guest, checkindate, checkoutdate,t.comboitem as Type, p.lastname + ', ' + p.firstname as Owner  from t_reservations r left outer join (select g.*, t_res2guest.reservationid from t_Guest g inner join t_Res2Guest on t_Res2Guest.GuestID = g.guestid where t_Res2Guest.PrimaryFlag=1) as g on g.reservationid=r.reservationid left outer join t_Prospect p on p.prospectid = r.prospectid left outer join t_ComboItems rl on rl.comboitemid = reslocationid left outer join t_ComboItems l on l.comboitemid = r.statusid left outer join t_Comboitems t on t.comboitemid = r.typeid order by g.lastname,g.firstname",cn,3,3
			else
				if (instr(request("filter"),",")) > 0 then
					rs.open "select top 50 r.reservationid, reservationnumber, l.comboitem as status, g.lastname + ', ' + g.firstname as Guest, checkindate, checkoutdate,t.comboitem as Type, p.lastname + ', ' + p.firstname as Owner  from t_reservations r left outer join (select g.*, t_res2guest.reservationid from t_Guest g inner join t_Res2Guest on t_Res2Guest.GuestID = g.guestid where t_Res2Guest.PrimaryFlag=1) as g on g.reservationid=r.reservationid left outer join t_Prospect p on p.prospectid = r.prospectid left outer join t_ComboItems rl on rl.comboitemid = reslocationid left outer join t_ComboItems l on l.comboitemid = r.statusid left outer join t_Comboitems t on t.comboitemid = r.typeid where (g.lastname) like '" & replace(trim(left(request("Filter"),instr(request("Filter"),",")-1)),"'","''") & "%' and g.firstname like '" & replace(trim(right(request("Filter"),len(request("Filter"))-instr(request("Filter"),","))),"'","''") & "%' order by r.ReservationID",cn,3,3
				else
					rs.open "select top 50 r.reservationid, reservationnumber, l.comboitem as status, g.lastname + ', ' + g.firstname as Guest, checkindate, checkoutdate,t.comboitem as Type, p.lastname + ', ' + p.firstname as Owner  from t_reservations r left outer join (select g.*, t_res2guest.reservationid from t_Guest g inner join t_Res2Guest on t_Res2Guest.GuestID = g.guestid where t_Res2Guest.PrimaryFlag=1) as g on g.reservationid=r.reservationid left outer join t_Prospect p on p.prospectid = r.prospectid left outer join t_ComboItems rl on rl.comboitemid = reslocationid left outer join t_ComboItems l on l.comboitemid = r.statusid left outer join t_Comboitems t on t.comboitemid = r.typeid where (g.lastname) like '" & replace(request("Filter"),"'","''") & "%' order by r.ReservationID",cn,3,3
				end if
			end if	
		case "Owner"
			if request("filter") = "" then
				rs.open "select top 50 r.reservationid, reservationnumber, l.comboitem as status, g.lastname + ', ' + g.firstname as Guest, checkindate, checkoutdate,t.comboitem as Type, p.lastname + ', ' + p.firstname as Owner  from t_reservations r left outer join (select g.*, t_res2guest.reservationid from t_Guest g inner join t_Res2Guest on t_Res2Guest.GuestID = g.guestid where t_Res2Guest.PrimaryFlag=1) as g on g.reservationid=r.reservationid left outer join t_Prospect p on p.prospectid = r.prospectid left outer join t_ComboItems rl on rl.comboitemid = reslocationid left outer join t_ComboItems l on l.comboitemid = r.statusid left outer join t_Comboitems t on t.comboitemid = r.typeid order by p.lastname,p.firstname",cn,3,3
			else
				if (instr(request("filter"),",")) > 0 then
					rs.open "select top 50 r.reservationid, reservationnumber, l.comboitem as status, g.lastname + ', ' + g.firstname as Guest, checkindate, checkoutdate,t.comboitem as Type, p.lastname + ', ' + p.firstname as Owner  from t_reservations r left outer join (select g.*, t_res2guest.reservationid from t_Guest g inner join t_Res2Guest on t_Res2Guest.GuestID = g.guestid where t_Res2Guest.PrimaryFlag=1) as g on g.reservationid=r.reservationid left outer join t_Prospect p on p.prospectid = r.prospectid left outer join t_ComboItems rl on rl.comboitemid = reslocationid left outer join t_ComboItems l on l.comboitemid = r.statusid left outer join t_Comboitems t on t.comboitemid = r.typeid where (p.lastname) like '" & replace(trim(left(request("Filter"),instr(request("Filter"),",")-1)),"'","''") & "%' and p.firstname like '" & replace(trim(right(request("Filter"),len(request("Filter"))-instr(request("Filter"),","))),"'","''") & "%' order by Owner asc, r.ReservationID",cn,3,3
				else
					rs.open "select top 50 r.reservationid, reservationnumber, l.comboitem as status, g.lastname + ', ' + g.firstname as Guest, checkindate, checkoutdate,t.comboitem as Type, p.lastname + ', ' + p.firstname as Owner  from t_reservations r left outer join (select g.*, t_res2guest.reservationid from t_Guest g inner join t_Res2Guest on t_Res2Guest.GuestID = g.guestid where t_Res2Guest.PrimaryFlag=1) as g on g.reservationid=r.reservationid left outer join t_Prospect p on p.prospectid = r.prospectid left outer join t_ComboItems rl on rl.comboitemid = reslocationid left outer join t_ComboItems l on l.comboitemid = r.statusid left outer join t_Comboitems t on t.comboitemid = r.typeid where p.lastname like '" & replace(request("Filter"),"'","''") & "%' order by Owner asc, r.ReservationID",cn,3,3
				end if
			end if	
		case else
			if request("filter") = "" then
				rs.open "select top 50 r.reservationid, reservationnumber, l.comboitem as status, g.lastname + ', ' + g.firstname as Guest, checkindate, checkoutdate,t.comboitem as Type, p.lastname + ', ' + p.firstname as Owner  from t_reservations r left outer join (select g.*, t_res2guest.reservationid from t_Guest g inner join t_Res2Guest on t_Res2Guest.GuestID = g.guestid where t_Res2Guest.PrimaryFlag=1) as g on g.reservationid=r.reservationid left outer join t_Prospect p on p.prospectid = r.prospectid left outer join t_ComboItems rl on rl.comboitemid = reslocationid left outer join t_ComboItems l on l.comboitemid = r.statusid left outer join t_Comboitems t on t.comboitemid = r.typeid order by r.ReservationNumber",cn,3,3
			else
				rs.open "select top 50 r.reservationid, reservationnumber, l.comboitem as status, g.lastname + ', ' + g.firstname as Guest, checkindate, checkoutdate,t.comboitem as Type, p.lastname + ', ' + p.firstname as Owner  from t_reservations r left outer join (select g.*, t_res2guest.reservationid from t_Guest g inner join t_Res2Guest on t_Res2Guest.GuestID = g.guestid where t_Res2Guest.PrimaryFlag=1) as g on g.reservationid=r.reservationid left outer join t_Prospect p on p.prospectid = r.prospectid left outer join t_ComboItems rl on rl.comboitemid = reslocationid left outer join t_ComboItems l on l.comboitemid = r.statusid left outer join t_Comboitems t on t.comboitemid = r.typeid where r.reservationnumber like '" & replace(request("Filter"),"'","''") & "%' order by r.ReservationID",cn,3,3
			end if	
	end select
	
	
	sAns = "<table border = 0><tr><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>ID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Number</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Status</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Guest</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>In</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Out</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Type</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Owner</th><th></th></tr>"
	
	do while not rs.eof
		sAns = sAns & "<tr>"
		for i = 0 to rs.fields.count -1
			if instr(rs.fields(i).name, "date") > 0 then
				sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & left(rs.fields(i).value & "", instr(rs.fields(i).value & " ", " ")) & "&nbsp;</td>"
			else
				sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & rs.fields(i).value & "&nbsp;</td>"
			end if
		next
		sAns = sAns & "<td><a href = 'editreservation.asp?reservationid=" & rs.fields("ReservationID").value & "'><img src = 'images/edit.gif'></a></td>"
		sAns = sAns & "</tr>"
		rs.movenext
	loop
	sAns = sAns & "</table>"
	rs.close
	
elseif request("Function") = "Val_ResLoc" then
	rs.Open "Select comboitemid from t_ComboItems ci inner join t_COmbos c on ci.comboid = c.comboid where c.comboname = 'ReservationLocation' and ci.comboitem = 'KCP'", cn, 3, 3
	locid = rs.Fields("ComboItemID")
	rs.Close
	
	rs.OPen "Select ResLocationID from t_Reservations where reservationid = '" & request("ID") & "'", cn, 3, 3
	If CSTR(rs.Fields("ResLocationID") & "") <> CSTR(locid) then
		sAns = "The Reservation Location Must Be KCP to Add Room"
	Else
		sAns = "valid"
	End If
	rs.Close
	
	If sAns = "valid" then
		'***Make Sure Reservation has an In-Date/Out-Date***'
		rs.Open "Select CheckInDate, CheckOutDate from t_Reservations where reservationid = '" & request("ID") & "'", cn, 3, 3
		If (rs.Fields("CheckInDate") & "" = "") and (rs.Fields("CheckOutDate") & "" = "") then
			sAns = "Reservations Must Have a Check-In/Check-Out Date to Add Room"
		ElseIf rs.Fields("CheckInDate") & "" = "" then
			sAns = "Reservations Must Have a Check-In Date to Add Room"
		ElseIf rs.Fields("CheckOUtDate") & "" = "" then
			sAns = "Reservations Must Have a Check-Out Date to Add Room"
		End If
		rs.Close
	End If
	
	If sAns = "valid" then
		rs.Open "Select a.ComboItem from t_Reservations b left outer join t_ComboItems a on b.typeid = a.comboitemid where b.reservationid = '" & request("ID") & "'", cn, 3, 3
		If rs.EOF and rs.BOF then
			sAns = "Please Select a Reservation Type"
		ElseIf rs.Fields("ComboItem") & "" = "" then
			sAns = "Please Select a Reservation Type"
		Else
			resType = rs.Fields("ComboItem")
		'	typeCheck = "Add" & rs.Fields("ComboItem") & "Room"
		'	'****Security Check********'
		'	If not(CheckSecurity("Reservations", typeCheck)) then
		'		sAns = "You Do Not Have Permission to Add a Room to A " & rs.Fields("ComboItem") & " Reservation"
		'	End If
		End If
		rs.Close
	End If
	
	if sAns = "valid" then
		if resType = "Marketing" or resType = "Rental" or resType = "Developer" or resType = "Vendor" then
			rs.Open "Select DateDiff(" & Chr(34) & "d" & Chr(34) & ",CheckInDate,CheckOutDate) as nights from t_Reservations where reservationid = '" & request("ID") & "'", cn, 3, 3
			transamt = rs.Fields("nights") * 2
			rs.Close
		
			'****** Make sure there is an invoice of at least numnights * 2 thats a room charge*********'
			rs.Open "Select sum(Amount) as Amt from t_AccountItems where reservationid = '" & request("ID") & "' and fintransid in (Select financialtranscodeid from t_FinancialTransactionCodes where roomcharge = '1' and transtypeid in (Select comboitemid from t_ComboItems where comboname = 'transcodetype' and comboitem = 'reservationtrans'))", cn, 3, 3
			If (rs.EOF and rs.BOF) or IsNull(rs.Fields("Amt")) then
				sAns = "You must insert a room charge before assigning a room"
			elseif rs.Fields("Amt") < transamt then
				sAns = "Room Charge Amount is Insufficient"
			end if
			rs.Close
		end if
	end if
	
	'****Check to make sure reservation is not a past date reservation.******'
	if sAns = "valid" then
		rs.Open "Select r.CheckInDate, rs.ComboItem as ResStatus from t_Reservations r left outer join t_ComboItems rs on r.StatusID = rs.ComboItemID where r.ReservationID = '" & request("ID") & "'", cn, 3, 3
		If rs.Fields("ResStatus") & "" <> "In-House" and CDate(rs.Fields("CheckInDate")) < CDate(Date - 1) Then
			sAns = "You Can Not Add a Room to A Past Date Reservation."
		End If
		rs.Close
	end if


elseif request("Function") = "Get_Rooms" then
	sAns = "<table border=0><tr><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>RoomID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Room</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Type</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>In Date</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Out Date</th></tr>"
	if request("ID") = "" or request("ID") = "0" then
	else
		rs.OPen "Select b.comboitem as Status from t_Reservations a left outer join t_ComboItems b on a.statusid = b.comboitemid where a.reservationid = '" & request("ID") & "'", cn, 3, 3
		resStatus = rs.Fields("Status") & ""
		rs.Close
		
		rs.Open "Select CheckInDate, CheckOutDate from t_Reservations where reservationid = '" & request("ID") & "'", cn, 3, 3
		If rs.EOF and rs.BOF then
			spare = "false"
		ElseIf IsNull(rs.Fields("CheckInDate")) or IsNull(rs.Fields("CheckOutDate")) then
			spare = "false"
		else
			if Date >= rs.Fields("CheckInDate") and Date < rs.Fields("CheckOutDate") then
				spare = "true"
				inDate = rs.Fields("CheckInDate")
				outDate = rs.Fields("CheckOutDate") - 1
			else
				spare = "false"
			end if
			inDate = rs.Fields("CheckInDate")
			outDate = rs.Fields("CheckOutDate") - 1
		end if
		rs.Close
		rs.OPen "Select b.comboitem as Status from t_Reservations a left outer join t_ComboItems b on a.statusid = b.comboitemid where a.reservationid = '" & request("ID") & "'", cn, 3, 3
		resStatus = rs.Fields("Status") & ""
		rs.Close
		
		rs.Open "Select c.RoomID, c.RoomNumber, d.ComboItem from (Select Distinct(a.roomid), b.RoomNumber, b.TypeID from t_RoomAllocationMatrix a inner join t_ROom b on a.roomid = b.roomid where a.reservationid = '" & request("ID") & "') c left outer join t_ComboItems d on c.typeid = d.comboitemid", cn, 3, 3
		Do while not rs.EOF
			rs2.Open "Select * from t_RoomAllocationMatrix where roomid = '" & rs.Fields("RoomID") & "' and reservationid = '" & request("ID") & "' order by dateallocated asc", cn, 3, 3
			sDate = rs2.Fields("DateAllocated")
			tempDate = rs2.Fields("DateAllocated")
			Do while not rs2.EOF
				if CDate(tempDate) <> rs2.Fields("DateAllocated") then
					sAns = sAns & "<tr>"
					for i = 0 to rs.fields.count - 1
						sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & rs.fields(i).value & "</td>"
					next
					sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & sDate & "</td>"
					sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & tempDate & "</td>"
					if (Date >= sDate AND Date <= (tempDate - 1)) OR Date <= sDate Then
						sAns = sAns & "<td><input type = 'button' value = 'Remove' name = 'remove' onClick = " & Chr(34) & "remove_Room('" & request("ID") & "', '" & rs.Fields("RoomID") & "');" & Chr(34) & "></td>"
					end if
					if spare = "true" and (Date >= sDate AND Date <= (tempDate - 1)) then
						if CheckSecurity("reservations", "SwapRoom") then
							sAns = sAns & "<td><input type = 'button' value = 'Swap Room' name = 'spare' onClick = " & Chr(34) & "popitup2('movetospare.asp?reservationid=" & request("ID") & "&date=" & inDate & "&outDate=" & outDate & "&roomID=" & rs.FIelds("RoomID") & "');" & Chr(34) & "></td>"
						end if
					end if
					sAnd = sANs & "</tr>"
					sDate = rs2.Fields("DateAllocated")
					tempDate = sDate + 1
				else
					tempDate = tempDate + 1
				end if
				rs2.MoveNext
			Loop
			rs2.Close
			sAns = sAns & "<tr>"
			for i = 0 to rs.fields.count - 1
				sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & rs.fields(i).value & "</td>"
			next
			sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & sDate & "</td>"
			sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & tempDate & "</td>"
			if (Date >= sDate AND Date < tempDate) OR Date <= sDate Then
				sAns = sAns & "<td><input type = 'button' value = 'Remove' name = 'remove' onClick = " & Chr(34) & "remove_Room('" & request("ID") & "', '" & rs.Fields("RoomID") & "');" & Chr(34) & "></td>"
			end if
			if spare = "true" and (Date >= sDate AND Date < tempDate) then
				if CheckSecurity("reservations", "SwapRoom") then
						sAns = sAns & "<td><input type = 'button' value = 'Swap Room' name = 'spare' onClick = " & Chr(34) & "popitup2('movetospare.asp?reservationid=" & request("ID") & "&date=" & inDate & "&outDate=" & outDate & "&roomID=" & rs.FIelds("RoomID") & "');" & Chr(34) & "></td>"
				end if
			end if
			if resStatus = "Booked" and Date < inDate then
				if CheckSecurity("reservations","SwapRoomBooked") then
						sAns = sAns & "<td><input type = 'button' value = 'Swap Room' name = 'spare' onClick = " & Chr(34) & "popitup2('movetospare.asp?reservationid=" & request("ID") & "&type=booked&date=" & inDate & "&outDate=" & outDate & "&roomID=" & rs.FIelds("RoomID") & "');" & Chr(34) & "></td>"
				end if
			end if					
			sAnd = sANs & "</tr>"
			
			

			
			'sAns = sAns & "<tr>"
			'for i = 0 to rs.fields.count - 1
			'	sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & rs.fields(i).value & "</td>"
			'next
			'rs2.Open "Select top 1 DateAllocated from t_RoomAllocationMatrix where reservationid = '" & request("ID") & "' and roomid = '" & rs.Fields("RoomID") & "' order by dateallocated desc", cn, 3, 3
			'If Date <= rs2.FIelds("DateAllocated") then
				'xxxxxxif resStatus <> "In-House" then
			'		sAns = sAns & "<td><input type = 'button' value = 'Remove' name = 'remove' onClick = " & Chr(34) & "remove_Room('" & request("ID") & "', '" & rs.Fields("RoomID") & "');" & Chr(34) & "></td>"
				'xxxxxxend if
			'	if spare = "true" then
			'		if CheckSecurity("reservations", "SwapRoom") then
			'			sAns = sAns & "<td><input type = 'button' value = 'Swap Room' name = 'spare' onClick = " & Chr(34) & "popitup2('movetospare.asp?reservationid=" & request("ID") & "&date=" & inDate & "&outDate=" & outDate & "&roomID=" & rs.FIelds("RoomID") & "');" & Chr(34) & "></td>"
			'			'xxxxxxsAns = sAns & "<td><input type = 'button' value = 'Move to Spare' name = 'spare' onClick = " & Chr(34) & "popitup2('movetospare.asp?reservationid=" & request("ID") & "&date=" & inDate & "&outDate=" & outDate & "&roomID=" & rs.Fields("RoomID") & "');" & Chr(34) & "></td>"
			'		end if
			'	end if
			'End If
			'rs2.Close
			'sAns = sAns & "</tr>"
			rs.MoveNext
		Loop
		rs.Close
	end if
	sAns = sAns & "</table>"
elseif request("Function") = "Add_Usage_Room" then
	rs.Open "Select CheckInDate, CheckOutDate from t_Reservations where reservationid = '" & request("ResID") & "'", cn, 3, 3
	if Date > CDate(rs.Fields("CheckInDate")) and Date < CDate(rs.Fields("CheckOutDate")) then
		inDate = Date
	Else
		inDate = rs.Fields("CheckInDate")
	End If
	outDate = rs.Fields("CheckOutDate")
	rs.Close
	
	usage = Split(request("usage"), "|")
	for i = 0 to UBound(usage)
		rs.Open "Select * from t_RoomAllocationMatrix where usageid = '" & usage(i) & "' and dateallocated between '" & inDate & "' and '" & outDate - 1 & "'", cn, 3, 3
		Do while not rs.EOF
			rs.Fields("ReservationID") = request("ResID")
			rs.UpdateBatch
			rs.MoveNext
		Loop
		rs.Close
		rs.Open "Select Distinct(b.RoomNumber) from t_RoomAllocationMatrix a inner join t_Room  b on a.roomid = b.roomid where a.usageID = '" & usage(i) & "' and dateallocated between '" & inDate & "' and '" & outDate - 1 & "'", cn, 3, 3
		Do while not rs.EOF
			Create_Event "ReservationID",request("ResID"),"",rs.Fields("RoomNumber"),"Add","Room"
			rs.moveNext
		Loop
		rs.Close				
	next
elseif request("Function") = "Add_Room" then
	if request("extend") = "yes" and Not(CheckSecurity("Reservations","ExtendStay")) then
		sANs = "You Do Not Have Permission to Extend Stay"
	else
		rooms = Split(request("room"), "|")
		rs.Open "Select CheckinDate, CheckoutDate, typeID from t_Reservations where reservationid = '" & request("ResID") & "'", cn, 3, 3
		origInDate = rs.Fields("CheckIndate")
		if request("extend") = "yes" then
			if CDate(request("newOutDate")) <= rs.Fields("CheckOutDate") then
				error = "true"
				sAns = "New Out Date must be Greater than Old Out Date"
			end if 
			inDate = rs.Fields("CheckOutDate")
			outDate = CDate(request("newOutDate"))
		else
			if Date > CDate(rs.Fields("CheckInDate")) and Date < CDate(rs.Fields("CheckOutDate")) then
				inDate = Date
			Else
				inDate = rs.Fields("CheckInDate")
			End If
			outDate = rs.Fields("CheckOutDate")
		end if
		rs.Close
		typeID = request("inventory")
		rs.Open "Select comboitem from t_ComboItems where comboitemid = '" & typeID & "'", cn, 3, 3
		restypename = rs.Fields("ComboItem")
		typeCheck = "Add" & rs.Fields("ComboItem") & "Room"
		'****Security Check********'
		If not(CheckSecurity("Reservations", typeCheck)) then
			error = "true"		
			sAns = "You Do Not Have Permission to Add a Room to A " & rs.Fields("ComboItem") & " Reservation"
		End If
		rs.Close
		
		if (restypename = "Rental" or restypename = "Marketing" or restypename = "Developer" or restypename = "Vendor") and extend = "yes" then
			'****** Need to see if there is enough money to cover the transient tax of the extend. ******'
			transAmt = DateDiff("d",origInDate,outDate) * 2
			rs.Open "Select Sum(Amount) as Amt from t_AccountItems where reservationid = '" & request("ResID") & "' and fintransid in (Select financialtranscodeid from t_FinancialtransactionCodes where roomcharge = '1' and transtypeid in (Select comboitemid from t_ComboItems where comboname = 'TransCodeType' and comboitem = 'ReservationTrans'))", cn, 3, 3
			If rs.EOF and rs.BOF or IsNull(rs.Fields("Amt")) then
				sAns = "You must insert a Room Charge"
				error = "true"
			ElseIf (rs.Fields("Amt")) < transAmt then
				sAns = "Insufficient Room Charge Amount"
				error = "true"
			End If
			rs.Close
		end if
		
		if error <> "true" then
			error = "false"
			cn.begintrans
			for i = 0 to UBound(rooms)
				room = Split(rooms(i), "-")
				For j = 0 to UBound(room)
					'If room is a spare have to create a usage to assign the room too
					'Usage will be under contract "KCPSPARE"
					rs.OPen "Select b.comboitem as resType from t_RoomAllocationMatrix a left outer join t_ComboItems b on a.typeid = b.comboitemid where a.roomid = '" & room(j) & "' and a.dateallocated between '" & inDate & "' and '" & outDate - 1 & "'", cn, 3, 3
					resType = rs.Fields("resType") & ""
					rs.Close
					
					if resType = "Spare" then
						usageID = new_Usage_ID (request("resID"), room(j), typeID, inDate, outDate)
						if usageID = 0 then
							error = "true"
						end if
					end if

					rs.Open "Select * from t_RoomAllocationMatrix where roomid = '" & room(j) & "' and dateallocated between '" & inDate & "' and '" & outDate - 1 & "'", cn, 3, 3
					Do while not rs.EOF
						if (IsNull(rs.Fields("ReservationID")) or rs.Fields("ReservationID") = "0") and rs.Fields("UsageID") > 0 then
							rs.Fields("ReservationID") = request("ResID")
							rs.UpdateBatch
						else
							error = "true"
						end if
						rs.MoveNext
					Loop
					rs.Close
				next
			next
			if error = "true" then
				cn.rollbacktrans
				sAns = "One or more rooms are now unavailable. Please Requery and select another room."
			else
				for j = 0 to UBound(room)
					rs.Open "Select roomnumber from t_Room where roomid = '" & room(j) & "'", cn, 3, 3
					newRoomNumber = rs.Fields("RoomNumber")
					rs.Close
					Create_Event "ReservationID",request("ResID"),"",newRoomNumber,"Add","Room"		
				next
				if request("extend") = "yes" then
					rs.Open "Select CheckOutDate from t_reservations where reservationid = '" & request("ResID") & "'", cn, 3, 3
					rs.Fields("CheckOutDate") = CDate(request("newOutDate"))
					rs.UpdateBatch
					rs.Close
					Create_Event "ReservationID",request("ResID"),inDate,newOutDate,"Change","CheckOutDate"
				end if
				cn.CommitTrans
				sAns = "OK"
			end if
		end if
	end if
elseif request("Function") = "Remove_Room" then
	rs.Open "Select e.*, f.ComboItem as Type from (Select c.FirstName, c.LastName,d.checkindate, d.Status, d.TypeID from (Select a.ProspectID, a.TypeID, a.CheckInDate, b.comboitem as Status from t_reservations a left outer join t_ComboItems b on a.statusid = b.comboitemid where a.reservationid = '" & request("ID") & "') d left outer join t_Prospect c on c.prospectid = d.prospectid) e left outer join t_COmboItems f on e.typeid = f.comboitemid", cn, 3, 3
	status = rs.Fields("Status") & ""
	inDate = rs.Fields("CheckIndate")
	prosName = Left(rs.Fields("FirstName"), 10) & " " & Left(rs.Fields("LastName"), 10)
	resType = rs.Fields("Type")
	rs.Close

	typeCheck = "Remove" & resType & "Room"
	'****Security Check********'
		If not(CheckSecurity("Reservations", typeCheck)) then
			sAns = "You Do Not Have Permission to Remove a Room from " & resType & " Reservations"
			error = "True"
		End If
	'****End Security Check*****'
	
	'******** Make sure inventory is not locked *********'
	rs.Open "Select LockInventory from t_Reservations where reservationid = '" & request("ID") & "'", cn, 3, 3
	If rs.Fields("LockInventory") then
		sAns = "The Inventory For This Reservation Is Locked And Can Not Be Removed."
		error = "True"
	End If
	rs.Close
	
	
	If error <> "True" then
		if status = "In-House" then
			inDate = Date
			rs.OPen "Select * from t_RoomAllocationMatrix where reservationid = '" & request("ID") & "' and RoomID = '" & request("RoomID") & "' and dateallocated >= '" & inDate & "'", cn, 3, 3
			Do while not rs.EOF
				rs.Fields("ReservationID") = 0
				rs.MoveNext
			Loop
			rs.UpdateBatch
			rs.Close
			
			rs.OPen "Select Phone from t_Room where roomid = '" & request("RoomID") & "'", cn, 3, 3
			ext = rs.Fields("Phone")
			rs.Close
			
			rs.OPen "Select * from t_RoomMessages where 1=2", cn, 3, 3
			rs.AddNew
			rs.Fields("SiteID") = 1
			rs.Fields("Action") = "CHECKOUT"
			rs.Fields("GuestName") = prosName
			rs.Fields("Flag") = 0
			rs.Fields("Extension") = ext
			rs.Fields("PBXDateIn") = Date
			rs.Fields("Username") = Session("UserID")
			rs.Fields("RoomID") = request("RoomID")
			rs.UpdateBatch
			rs.Close	
			
			'** Mark Room As Dirty
			rs.open "Select * from t_Comboitems where comboname = 'roomstatus' and comboitem = 'dirty'", cn, 0, 1
			DirtyID = rs.fields("Comboitemid").value
			rs.close
			
			rs.open "Select * from t_Room where roomid = '" & request("RoomID") & "'", cn, 3, 3
			if not(rs.eof and rs.bof) then
				rs.fields("StatusID").value = DirtyID 
				rs.update
			end if
			rs.close
		else
			rs.OPen "Select * from t_RoomAllocationMatrix where reservationid = '" & request("ID") & "' and RoomID = '" & request("RoomID") & "'", cn, 3, 3
			Do while not rs.EOF
				rs.Fields("ReservationID") = 0
				rs.MoveNext
			Loop
			rs.UpdateBatch
			rs.Close
		end if
		rs.Open "Select RoomNumber from t_Room where roomid = '" & request("RoomID") & "'", cn, 3, 3
		oldRoomNumber = rs.Fields("RoomNumber")
		rs.Close
		Create_Event "ReservationID",request("ID"),oldRoomNumber,"","Remove","Room"		
		sAns = "OK"
	end if
elseif request("Function") = "Search_Spares_Booked" then
	rs.Open "Select b.ComboItem as subtype from t_Reservations a left outer join t_ComboItems b on a.subtypeid = b.comboitemid where a.reservationid = '" & request("resID") & "'", cn, 3, 3
	if rs.EOF and rs.BOF then
		subType = ""
	Else
		subType = rs.Fields("SubType")
	end if
	rs.Close
	rs.OPen "Select d.ComboItem as UnitStyle from (Select b.styleid from t_Room a inner join t_Unit b on a.unitid = b.unitid where a.roomid = '" & request("RoomID") & "') c left outer join t_ComboItems d on c.styleid = d.comboitemid", cn, 3, 3 
	If rs.EOF and rs.BOF or IsNull(rs.Fields("UnitStyle")) Then
		unitStyle = ""
	Else
		unitStyle = rs.Fields("UnitStyle")
	End If
	rs.Close
	
	sANs = "<table>"
	sAns = sANs & "<tr><td colspan = '8'>Swappable Rooms</td></tr>"
	rs.Open "Select m.*, n.ComboItem as RoomStatus from (SELECT k.StatusID, k.RoomID, k.Nights, k.RoomNumber, k.RoomType, k.RoomSubType, k.UnitType, l.ComboItem AS UnitStyle FROM (SELECT i.*, j.ComboItem AS UnitType FROM (SELECT g.*, h.styleid AS styleid, h.typeid AS typeid FROM (SELECT e.*, f.ComboItem AS RoomSubType FROM (SELECT c.*, d .ComboItem AS RoomType FROM (SELECT a.*, b.RoomNumber AS RoomNumber, b.TypeID AS TypeID, b.SubTypeID AS SubTypeID, b.UnitID AS UnitID, b.StatusID as StatusID FROM (SELECT DISTINCT RoomID, COUNT(AllocationID) AS Nights FROM t_RoomAllocationMatrix z LEFT OUTER JOIN t_Reservations y ON z.reservationid = y.reservationid WHERE ((z.ReservationID IS NULL OR z.reservationid = '0' OR z.reservationid = '" & request("resID") & "') OR (y.checkindate = '" & request("inDate") & "' AND y.checkoutdate = '" & CDate(request("OutDate")) + 1 & "' AND y.LockInventory = '0' AND y.ReservationID <> '" & request("resID") & "' AND y.statusid = (SELECT comboitemid FROM t_ComboItems WHERE comboname = 'reservationstatus' AND comboitem = 'booked'))) AND (RoomID IN (SELECT roomid FROM t_Room WHERE subtypeid = (Select subtypeid from t_Room where roomid = '" & Request("RoomID") & "') AND typeid IN (SELECT comboitemid FROM t_ComboItems WHERE comboname = 'RoomType' AND comboitem = (SELECT comboitem FROM t_ComboItems WHERE comboitemid = (SELECT typeid FROM t_Room WHERE RoomID = '" & request("RoomID") & "'))))) AND (DateAllocated BETWEEN '" & request("inDate") & "' AND '" & CDate(request("OutDate")) & "') AND (z.TypeID <> (SELECT comboitemid FROM t_CombOItems WHERE comboname = 'ReservationType' AND comboitem = 'spare') OR z.TypeID Is Null) GROUP BY RoomID) a INNER JOIN t_Room b ON a.RoomID = b.RoomID) c LEFT OUTER JOIN t_ComboItems d ON c.TypeID = d .ComboItemID) e LEFT OUTER JOIN t_ComboItems f ON e.SubTypeID = f.ComboItemID) g LEFT OUTER JOIN t_Unit h ON g.UnitID = h.UnitID where h.typeid = (Select typeid from t_Unit where unitid = (Select UnitID from t_Room where roomid = '" & request("RoomID") & "'))) i LEFT OUTER JOIN t_ComboItems j ON i.typeid = j.ComboItemID) k LEFT OUTER JOIN t_ComboItems l ON k.styleid = l.ComboItemID WHERE (k.Nights = DATEDIFF(d, '" & request("inDate") & "', '" & CDate(request("outDate")) + 1 & "')) AND (k.RoomID <> '" & request("RoomID") & "')) m left outer join t_ComboItems n on m.statusid = n.comboitemid ORDER BY CHARINDEX('-', m.RoomNumber), m.RoomNumber", cn, 3, 3

	If rs.EOF and rs.BOF then
		sANs = sANs & "<tr><td colspan = '8'>No Rooms Available</td></tr>"
	Else
		Do while not rs.EOF
			'sAns = sAns & "<tr><td><input type = 'radio' name 'room' onclick = " & Chr(34) & "Add_Spare('" & rs.Fields("RoomID") & "', '" & request("ResID") & "', '" & request("indate") & "', '" & request("outDate") & "', '" & request("RoomID") & "', 'swap');" & Chr(34) & "</td>"
			sAns = sAns & "<tr><td><input type = 'radio' name 'room' onclick = " & Chr(34) & "Check_Swap('" & rs.Fields("RoomID") & "', '" & request("ResID") & "', '" & request("indate") & "', '" & request("outDate") & "', '" & request("RoomID") & "', 'swap');" & Chr(34) & "</td>"
			sAns = sAns & "<td>" & rs.FIelds("RoomNumber") & "</td>"
			sANs = sAns & "<td>" & rs.FIelds("RoomType") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("RoomSubtype") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("UnitType") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("UnitStyle") & "</td>"
			if rs.Fields("RoomStatus") = "Dirty" then
				sAns = sAns & "<td><font color = '#FF3300'>" & rs.Fields("RoomStatus") & "</font></td>"
			else
				sAns = sAns & "<td>" & rs.Fields("RoomStatus") & "</td>"			
			end if
			sAns = sAns & "</tr>"
			rs.MoveNExt
		Loop
	End If
	rs.Close
	sAns = sAns & "</table>"

elseif request("Function") = "Search_Spares" then
	rs.Open "Select b.ComboItem as subtype from t_Reservations a left outer join t_ComboItems b on a.subtypeid = b.comboitemid where a.reservationid = '" & request("resID") & "'", cn, 3, 3
	if rs.EOF and rs.BOF then
		subType = ""
	Else
		subType = rs.Fields("SubType")
	end if
	rs.Close
	rs.OPen "Select d.ComboItem as UnitStyle from (Select b.styleid from t_Room a inner join t_Unit b on a.unitid = b.unitid where a.roomid = '" & request("RoomID") & "') c left outer join t_ComboItems d on c.styleid = d.comboitemid", cn, 3, 3 
	If rs.EOF and rs.BOF or IsNull(rs.Fields("UnitStyle")) Then
		unitStyle = ""
	Else
		unitStyle = rs.Fields("UnitStyle")
	End If
	rs.Close
	
	sANs = "<table>"
	sAns = sANs & "<tr><td colspan = '8'>Swappable Rooms</td></tr>"
	'If subType = "RCI" then
	'	rs.Open "Select m.*, n.ComboItem as RoomStatus from (SELECT k.StatusID, k.RoomID, k.Nights, k.RoomNumber, k.RoomType, k.RoomSubType, k.UnitType, l.ComboItem AS UnitStyle FROM (SELECT i.*, j.ComboItem AS UnitType FROM (SELECT g.*, h.styleid AS styleid, h.typeid AS typeid FROM (SELECT e.*, f.ComboItem AS RoomSubType FROM (SELECT c.*, d .ComboItem AS RoomType FROM (SELECT a.*, b.RoomNumber AS RoomNumber, b.TypeID AS TypeID, b.SubTypeID AS SubTypeID, b.UnitID AS UnitID, b.StatusID as StatusID FROM(SELECT DISTINCT RoomID, COUNT(AllocationID) AS Nights FROM t_RoomAllocationMatrix z LEFT OUTER JOIN t_Reservations y ON z.reservationid = y.reservationid WHERE ((z.ReservationID IS NULL OR z.reservationid = '0' OR z.reservationid = '" & request("resID") & "') OR (y.checkindate = '" & request("inDate") & "' AND y.checkoutdate = '" & CDate(request("OutDate")) + 1 & "' AND y.LockInventory = '0' AND y.ReservationID <> '" & request("resID") & "' AND y.statusid = (SELECT comboitemid FROM t_ComboItems WHERE comboname = 'reservationstatus' AND comboitem = 'booked'))) AND (RoomID IN (SELECT roomid FROM t_Room WHERE typeid IN (SELECT comboitemid FROM t_ComboItems WHERE comboname = 'RoomType' AND comboitem = (SELECT comboitem FROM t_ComboItems WHERE comboitemid = (SELECT typeid FROM t_Room WHERE RoomID = '" & request("RoomID") & "'))))) AND (DateAllocated BETWEEN '" & request("inDate") & "' AND '" & CDate(request("OutDate")) & "') AND (z.TypeID <> (SELECT comboitemid FROM t_CombOItems WHERE comboname = 'ReservationType' AND comboitem = 'spare') OR z.TypeID Is Null) GROUP BY RoomID) a INNER JOIN t_Room b ON a.RoomID = b.RoomID) c LEFT OUTER JOIN t_ComboItems d ON c.TypeID = d .ComboItemID) e LEFT OUTER JOIN t_ComboItems f ON e.SubTypeID = f.ComboItemID) g LEFT OUTER JOIN t_Unit h ON g.UnitID = h.UnitID where h.typeid = (Select typeid from t_Unit where unitid = (Select UnitID from t_Room where roomid = '" & request("RoomID") & "'))) i LEFT OUTER JOIN t_ComboItems j ON i.typeid = j.ComboItemID) k LEFT OUTER JOIN t_ComboItems l ON k.styleid = l.ComboItemID WHERE (k.Nights = DATEDIFF(d, '" & request("inDate") & "', '" & CDate(request("outDate")) + 1 & "')) AND (k.RoomID <> '" & request("RoomID") & "') AND (l.ComboItem <> 'Chesapeake' or l.ComboItem Is NULL)) m left outer join t_ComboItems n on m.statusid = n.comboitemid ORDER BY CHARINDEX('-', m.RoomNumber), m.RoomNumber", cn, 3, 3
	'ElseIf unitStyle = "Chesapeake" then
	'	rs.Open "Select m.*, n.ComboItem as RoomStatus from (SELECT k.StatusID, k.RoomID, k.Nights, k.RoomNumber, k.RoomType, k.RoomSubType, k.UnitType, l.ComboItem AS UnitStyle FROM (SELECT i.*, j.ComboItem AS UnitType FROM (SELECT g.*, h.styleid AS styleid, h.typeid AS typeid FROM (SELECT e.*, f.ComboItem AS RoomSubType FROM (SELECT c.*, d .ComboItem AS RoomType FROM (SELECT a.*, b.RoomNumber AS RoomNumber, b.TypeID AS TypeID, b.SubTypeID AS SubTypeID, b.UnitID AS UnitID, b.StatusID as StatusID FROM(SELECT DISTINCT RoomID, COUNT(AllocationID) AS Nights FROM t_RoomAllocationMatrix z LEFT OUTER JOIN t_Reservations y ON z.reservationid = y.reservationid WHERE ((z.ReservationID IS NULL OR z.reservationid = '0' OR z.reservationid = '" & request("resID") & "') OR (y.checkindate = '" & request("inDate") & "' AND y.checkoutdate = '" & CDate(request("OutDate")) + 1 & "' AND y.LockInventory = '0' AND y.ReservationID <> '" & request("resID") & "' AND y.statusid = (SELECT comboitemid FROM t_ComboItems WHERE comboname = 'reservationstatus' AND comboitem = 'booked') AND (y.subtypeid is null or y.subtypeid <> (Select comboitemid from t_ComboItems where comboname = 'ReservationSubType' and comboitem = 'RCI')))) AND (RoomID IN (SELECT roomid FROM t_Room WHERE typeid IN (SELECT comboitemid FROM t_ComboItems WHERE comboname = 'RoomType' AND comboitem = (SELECT comboitem FROM t_ComboItems WHERE comboitemid = (SELECT typeid FROM t_Room WHERE RoomID = '" & request("RoomID") & "'))))) AND (DateAllocated BETWEEN '" & request("inDate") & "' AND '" & CDate(request("OutDate")) & "') AND (z.TypeID <> (SELECT comboitemid FROM t_CombOItems WHERE comboname = 'ReservationType' AND comboitem = 'spare') OR z.TypeID Is Null) GROUP BY RoomID) a INNER JOIN t_Room b ON a.RoomID = b.RoomID) c LEFT OUTER JOIN t_ComboItems d ON c.TypeID = d .ComboItemID) e LEFT OUTER JOIN t_ComboItems f ON e.SubTypeID = f.ComboItemID) g LEFT OUTER JOIN t_Unit h ON g.UnitID = h.UnitID where h.typeid = (Select typeid from t_Unit where unitid = (Select UnitID from t_Room where roomid = '" & request("RoomID") & "'))) i LEFT OUTER JOIN t_ComboItems j ON i.typeid = j.ComboItemID) k LEFT OUTER JOIN t_ComboItems l ON k.styleid = l.ComboItemID WHERE (k.Nights = DATEDIFF(d, '" & request("inDate") & "', '" & CDate(request("outDate")) + 1 & "')) AND (k.RoomID <> '" & request("RoomID") & "')) m left outer join t_ComboItems n on m.statusid = n.comboitemid ORDER BY CHARINDEX('-', m.RoomNumber), m.RoomNumber", cn, 3, 3	
	'Else
		rs.Open "Select m.*, n.ComboItem as RoomStatus from (SELECT k.StatusID, k.RoomID, k.Nights, k.RoomNumber, k.RoomType, k.RoomSubType, k.UnitType, l.ComboItem AS UnitStyle FROM (SELECT i.*, j.ComboItem AS UnitType FROM (SELECT g.*, h.styleid AS styleid, h.typeid AS unittypetypeid FROM (SELECT e.*, f.ComboItem AS RoomSubType FROM (SELECT c.*, d .ComboItem AS RoomType FROM (SELECT a.*, b.RoomNumber AS RoomNumber, b.TypeID AS TypeID, b.SubTypeID AS SubTypeID, b.UnitID AS UnitID, b.StatusID as StatusID FROM(SELECT DISTINCT RoomID, COUNT(AllocationID) AS Nights FROM t_RoomAllocationMatrix z LEFT OUTER JOIN t_Reservations y ON z.reservationid = y.reservationid WHERE ((z.ReservationID IS NULL OR z.reservationid = '0' OR z.reservationid = '" & request("resID") & "') OR (y.checkindate = '" & request("inDate") & "' AND y.checkoutdate = '" & CDate(request("OutDate")) + 1 & "' AND y.LockInventory = '0' AND y.ReservationID <> '" & request("resID") & "' AND y.statusid = (SELECT ci.comboitemid FROM t_ComboItems ci inner join t_combos c on ci.comboid = c.comboid WHERE c.comboname = 'reservationstatus' AND comboitem = 'booked'))) AND (RoomID IN (SELECT roomid FROM t_Room WHERE typeid IN (SELECT comboitemid FROM t_ComboItems cii inner join t_Combos cc on cii.comboid = cc.comboid WHERE cc.comboname = 'RoomType' AND comboitem = (SELECT comboitem FROM t_ComboItems WHERE comboitemid = (SELECT typeid FROM t_Room WHERE RoomID = '" & request("RoomID") & "'))))) AND (DateAllocated BETWEEN '" & request("inDate") & "' AND '" & CDate(request("OutDate")) & "') AND (z.TypeID <> (SELECT comboitemid FROM t_CombOItems cs inner join t_Combos ccs on cs.comboid = ccs.comboid WHERE ccs.comboname = 'ReservationType' AND comboitem = 'spare') OR z.TypeID Is Null) GROUP BY RoomID) a INNER JOIN t_Room b ON a.RoomID = b.RoomID) c LEFT OUTER JOIN t_ComboItems d ON c.TypeID = d .ComboItemID) e LEFT OUTER JOIN t_ComboItems f ON e.SubTypeID = f.ComboItemID) g LEFT OUTER JOIN t_Unit h ON g.UnitID = h.UnitID where h.typeid = (Select typeid from t_Unit where unitid = (Select UnitID from t_Room where roomid = '" & request("RoomID") & "'))) i LEFT OUTER JOIN t_ComboItems j ON i.typeid = j.ComboItemID) k LEFT OUTER JOIN t_ComboItems l ON k.styleid = l.ComboItemID WHERE (k.Nights = DATEDIFF(d, '" & request("inDate") & "', '" & CDate(request("outDate")) + 1 & "')) AND (k.RoomID <> '" & request("RoomID") & "')) m left outer join t_ComboItems n on m.statusid = n.comboitemid ORDER BY CHARINDEX('-', m.RoomNumber), m.RoomNumber", cn, 3, 3
	'End If

	If rs.EOF and rs.BOF then
		sANs = sANs & "<tr><td colspan = '8'>No Rooms Available</td></tr>"
	Else
		Do while not rs.EOF
			'sAns = sAns & "<tr><td><input type = 'radio' name 'room' onclick = " & Chr(34) & "Add_Spare('" & rs.Fields("RoomID") & "', '" & request("ResID") & "', '" & request("indate") & "', '" & request("outDate") & "', '" & request("RoomID") & "', 'swap');" & Chr(34) & "</td>"
			sAns = sAns & "<tr><td><input type = 'radio' name 'room' onclick = " & Chr(34) & "Check_Swap('" & rs.Fields("RoomID") & "', '" & request("ResID") & "', '" & request("indate") & "', '" & request("outDate") & "', '" & request("RoomID") & "', 'swap');" & Chr(34) & "</td>"
			sAns = sAns & "<td>" & rs.FIelds("RoomNumber") & "</td>"
			sANs = sAns & "<td>" & rs.FIelds("RoomType") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("RoomSubtype") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("UnitType") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("UnitStyle") & "</td>"
			if rs.Fields("RoomStatus") = "Dirty" then
				sAns = sAns & "<td><font color = '#FF3300'>" & rs.Fields("RoomStatus") & "</font></td>"
			else
				sAns = sAns & "<td>" & rs.Fields("RoomStatus") & "</td>"			
			end if
			sAns = sAns & "</tr>"
			rs.MoveNExt
		Loop
	End If
	rs.Close
	sAns = sAns & "<tr><td><br></td></tr>"
	sANs = sAns & "<tr><td colspan = '8'>Spare Rooms</td></tr>"
	'if subType = "RCI" then
	'	rs.Open "Select m.*, n.ComboItem as RoomStatus from (SELECT k.RoomID, k.Nights, k.RoomNumber, k.RoomType, k.RoomSubType, k.UnitType, k.StatusID, l.ComboItem AS UnitStyle FROM (SELECT i.*, j.ComboItem AS UnitType FROM (SELECT g.*, h.styleid AS styleid, h.typeid AS typeid FROM (SELECT e.*, f.ComboItem AS RoomSubType FROM (SELECT c.*, d .ComboItem AS RoomType  FROM (SELECT a.*, b.RoomNumber AS RoomNumber, b.TypeID AS TypeID, b.SubTypeID AS SubTypeID, b.UnitID AS UnitID, b.StatusID as StatusID FROM (SELECT DISTINCT RoomID, COUNT(AllocationID) AS Nights FROM t_RoomAllocationMatrix WHERE (ReservationID is null or reservationid = '0') AND (usageid = '0' or usageid is null) and (RoomID IN (SELECT roomid FROM t_Room WHERE typeid IN (SELECT comboitemid FROM t_ComboItems WHERE comboname = 'RoomType' AND comboitem LIKE (SELECT LEFT(comboitem, 3) FROM t_ComboItems WHERE comboitemid = (SELECT typeid FROM t_Room WHERE RoomID = '" & request("RoomID") & "')) + '%'))) AND (DateAllocated BETWEEN  '" & request("inDate") & "' AND '" & request("outDate") & "') AND (TypeID = (SELECT comboitemid FROM t_CombOItems WHERE comboname = 'ReservationType' AND comboitem = 'spare')) GROUP BY RoomID) a INNER JOIN t_Room b ON a.RoomID = b.RoomID) c LEFT OUTER JOIN t_ComboItems d ON c.TypeID = d .ComboItemID) e LEFT OUTER JOIN t_ComboItems f ON e.SubTypeID = f.ComboItemID) g LEFT OUTER JOIN t_Unit h ON g.UnitID= h.UnitID) i LEFT OUTER JOIN t_ComboItems j ON i.typeid = j.ComboItemID) k LEFT OUTER JOIN t_ComboItems l ON k.styleid = l.ComboItemID WHERE (k.Nights = DATEDIFF(d, '" & request("indate") & "', '" & CDate(request("outDate")) + 1 & "')) and (l.comboitem <> 'Chesapeake' or l.comboitem is null)) m left outer join t_ComboItems n on m.statusid = n.comboitemid ORDER BY CHARINDEX('-', m.RoomNumber), m.RoomNumber", cn, 3, 3	
	'else
		rs.Open "Select m.*, n.ComboItem as RoomStatus from (SELECT k.RoomID, k.Nights, k.RoomNumber, k.RoomType, k.RoomSubType, k.UnitType, k.StatusID, l.ComboItem AS UnitStyle FROM (SELECT i.*, j.ComboItem AS UnitType FROM (SELECT g.*, h.styleid AS styleid, h.typeid AS unittypeid FROM (SELECT e.*, f.ComboItem AS RoomSubType FROM (SELECT c.*, d .ComboItem AS RoomType  FROM (SELECT a.*, b.RoomNumber AS RoomNumber, b.TypeID AS TypeID, b.SubTypeID AS SubTypeID, b.UnitID AS UnitID, b.StatusID as StatusID FROM (SELECT DISTINCT RoomID, COUNT(AllocationID) AS Nights FROM t_RoomAllocationMatrix WHERE (ReservationID is null or reservationid = '0') AND (usageid = '0' or usageid is null) and (RoomID IN (SELECT roomid FROM t_Room WHERE typeid IN (SELECT comboitemid FROM t_ComboItems ci inner join t_Combos c on ci.comboid = c.comboid WHERE c.comboname = 'RoomType' AND comboitem LIKE (SELECT LEFT(comboitem, 3) FROM t_ComboItems WHERE comboitemid = (SELECT typeid FROM t_Room WHERE RoomID = '" & request("RoomID") & "')) + '%'))) AND (DateAllocated BETWEEN  '" & request("inDate") & "' AND '" & request("outDate") & "') AND (TypeID = (SELECT comboitemid FROM t_CombOItems cc inner join t_Combos ccc on cc.comboid = ccc.comboid WHERE ccc.comboname = 'ReservationType' AND comboitem = 'spare')) GROUP BY RoomID) a INNER JOIN t_Room b ON a.RoomID = b.RoomID) c LEFT OUTER JOIN t_ComboItems d ON c.TypeID = d .ComboItemID) e LEFT OUTER JOIN t_ComboItems f ON e.SubTypeID = f.ComboItemID) g LEFT OUTER JOIN t_Unit h ON g.UnitID= h.UnitID) i LEFT OUTER JOIN t_ComboItems j ON i.typeid = j.ComboItemID) k LEFT OUTER JOIN t_ComboItems l ON k.styleid = l.ComboItemID WHERE (k.Nights = DATEDIFF(d, '" & request("indate") & "', '" & CDate(request("outDate")) + 1 & "'))) m left outer join t_ComboItems n on m.statusid = n.comboitemid ORDER BY CHARINDEX('-', m.RoomNumber), m.RoomNumber", cn, 3, 3
	'end if
	If rs.EOF and rs.BOF then
		sANs = sAns & "<tr><td colspan = '8'>No Rooms Available</td></tr>" 
	Else
		Do while not rs.EOF
			sAns = sAns & "<tr><td><input type = 'radio' name 'room' onclick = " & Chr(34) & "Add_Spare('" & rs.Fields("RoomID") & "', '" & request("ResID") & "', '" & request("indate") & "', '" & request("outDate") & "', '" & request("RoomID") & "', 'spare');" & Chr(34) & "</td>"
			sAns = sAns & "<td>" & rs.FIelds("RoomNumber") & "</td>"
			sANs = sAns & "<td>" & rs.FIelds("RoomType") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("RoomSubtype") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("UnitType") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("UnitStyle") & "</td>"
			if rs.Fields("RoomStatus") = "Dirty" then
				sAns = sAns & "<td><font color = '#FF3300'>" & rs.Fields("RoomStatus") & "</font></td>"
			else
				sAns = sAns & "<td>" & rs.Fields("RoomStatus") & "</td>"			
			end if
			sAns = sAns & "</tr>"
			rs.MoveNExt
		Loop
	End If
	rs.Close
	sAns = sAns & "</table>"
elseif request("Function") = "Check_Swappable" then
	rs.Open "Select reservationid from t_RoomAllocationMatrix where roomid = '" & request("roomid") & "' and dateallocated between '" & request("indate") & "' and '" & request("outdate") & "'", cn, 3, 3
	If rs.EOF and rs.BOF then
		resID = 0
	Else
		resID = rs.Fields("ReservationID")
	End If
	rs.Close
	
	If resID <> 0 then
		rs.Open "Select * from t_RoomAllocationMatrix where reservationid = '" & resID & "' and roomid in (SELECT RoomID FROM t_Room WHERE (RoomID IN (SELECT lockoutid FROM t_Room WHERE roomid = '" & request("roomid") & "')) OR (RoomID IN (SELECT roomid FROM t_Room WHERE lockoutid = '" & request("roomid") & "')) OR (RoomID IN (SELECT roomid FROM t_Room WHERE unitid IN (SELECT unit2id FROM t_Unit2Unit WHERE unitid = (SELECT unitid FROM t_Room WHERE roomid = '" & request("roomid") & "'))))) and roomid <> '" & request("roomid") & "'", cn, 3, 3
		If rs.EOF and rs.BOF then
			sAns = "OK"
		Else
			sAns = "This room is part of a Multi-Bedroom reservation. Do you wish to continue swapping anyway?"
		End If
		rs.Close
	Else
		sAns = "OK"
	End If
elseif request("Function") = "Move_To_Spare" then
	on error resume next
	if Not(CheckSecurity("reservations", "SwapRoom")) then
		sANs = "Access Denied"
	else
		cn.BeginTrans
		'********** FIND OUT RESERVATION STATUS****************'
		rs.Open "Select comboitem as status from t_Reservations a left outer join t_ComboItems b on a.statusid = b.comboitemid where a.reservationid = '" & request("resID") & "'", cn, 3, 3
		resStatus = rs.Fields("Status")
		rs.Close
		'********** FIND ROOM NUMBER FOR NEW ROOM AND OLD ROOM ************'
		rs.Open "Select RoomNumber from t_Room where roomid = '" & request("roomid") & "'", cn, 3, 3
		newRoomNumber = rs.Fields("RoomNumber")
		rs.Close
		rs.Open "Select RoomNumber from t_Room where roomid = '" & request("oldRoomID") & "'", cn, 3, 3
		oldRoomNumber = rs.Fields("RoomNumber")
		rs.Close
		
		if CSTR(Trim(request("type"))) = "spare" then
			if Not(CheckSecurity("reservations","MoveToSpare")) then
				error = "Access Denied"
			else
				rs.Open "Select * from t_RoomAllocationMatrix where reservationid = '" & request("resID") & "' and roomID = '" & request("oldRoomID") & "' and dateallocated between '" & request("inDate") & "' and '" & request("outDate") & "'", cn, 3, 3
				Do while not rs.EOF
					rs2.Open "Select * from t_RoomAllocationMatrix where roomID = '" & request("RoomID") & "' and dateAllocated = '" & rs.Fields("DateALlocated") & "'", cn, 3, 3
					If (Not(IsNull(rs2.Fields("usageID"))) and rs2.Fields("UsageID") > 0) or (Not(IsNull(rs2.Fields("reservationID"))) and rs2.Fields("reservationid") > 0) then
						error = "Room Has Been Reserved or Assigned to Usage"
					Else 
						If usages = "" then
							usages = rs.Fields("UsageID")
						else
							If Instr(usages, rs.Fields("UsageID")) = 0 then
								usages = usages & "," & rs.Fields("UsageID")
							end if
						end if
						rs2.Fields("UsageID") = rs.Fields("UsageID")
						rs2.Fields("ReservationID") = request("resID") 
						rs.Fields("ReservationID") = -1
						rs.Fields("UsageID") = 0
					End If
					rs2.UpdateBatch
					rs2.Close
					rs.UpdateBatch
					rs.MoveNext
				Loop
				rs.Close 		
			
				'*********** CREATE EVENT IN THE RESERVATION TO SHOW ROOM MOVE ************'
				Create_Event "ReservationID",request("ResID"),oldRoomNumber,newRoomNumber,"Move","Room"						
				'*********** CREATE EVENT IN THE USAGE(S) TO SHOW ROOM MOVE
				usageIDs = split(usages, ",")
				For i = 0 to UBound(usageIDs) - 1
					Create_Event "UsageID",usageIDs(i),oldRoomNumber,newRoomNumber,"Move","Room"						
				Next
				
				'*********** CREATE NOTE RECORDS TO SHOW REASON FOR MOVE *******************'
				rs.Open "Select * from t_Notes where 1=2", cn, 3, 3
				rs.AddNew 
				rs.Fields("ReservationID") = resA
				rs.Fields("Note") = request("resID")
				rs.fields("DateCreated") = Now
				rs.fields("CreatedById") = Session("UserID")
				rs.UpdateBatch
				rs.Close
			
				'*********** CREATE MAINTENANCE REQUEST FOR ROOM MOVED TO SPARE ************'
				rs.Open "Select comboitemid from t_ComboItems where comboname = 'WorkOrderStatus' and comboitem = 'Not Started'", cn, 3, 3
				woStatus = rs.Fields("ComboItemID")
				rs.Close
				rs.Open "Select comboitemid from t_ComboItems where comboname = 'RequestArea' and comboitem = 'Guest'", cn, 3, 3
				woArea = rs.Fields("ComboItemID")
				rs.Close
				'** CREATE REQUEST ****'
				rs.Open "Select * from t_Request where 1=2", cn, 3, 3
				rs.AddNew
				rs.Fields("EnteredByID") = Session("UserID")
				rs.Fields("EntryDate") = Now
				rs.Fields("RequestAreaID") = woArea
				rs.FIelds("RoomID") = request("OldRoomID")
				rs.Fields("StatusID") = woStatus
				rs.Fields("Subject") = "Guest Moved To Spare"
				rs.Fields("Description") = request("reason")
				rs.UpdateBatch
				rs.Close
			end if
		else
			rs.Open "Select * from t_RoomAllocationMatrix where reservationid = '" & request("resID") & "' and roomID = '" & request("oldRoomID") & "' and dateallocated between '" & request("inDate") & "' and '" & request("outDate") & "'", cn, 3, 3
			Do while not rs.EOF
				resA = rs.Fields("ReservationID")
				usageA = rs.Fields("UsageID")
				typeA = rs.Fields("TypeID")
				If usages = "" then
					usages = rs.Fields("UsageID")
				else
					If Instr(usages, rs.Fields("UsageID")) = 0 then
						usages = usages & "," & rs.Fields("UsageID")
					end if
				end if
				rs2.Open "Select * from t_RoomAllocationMatrix where roomID = '" & request("RoomID") & "' and dateAllocated = '" & rs.Fields("DateALlocated") & "'", cn, 3, 3
				If Instr(usages, rs2.Fields("UsageID")) = 0 then
					usagesB = usagesB & "," & rs2.Fields("UsageID")
				end if
				res2 = rs2.Fields("ReservationID")
				rs.Fields("ReservationID") = rs2.Fields("ReservationID")
				rs.Fields("UsageID") = rs2.Fields("UsageID")
				rs.Fields("TypeID") = rs2.Fields("TypeID")
				rs2.Fields("ReservationID") = resA
				rs2.Fields("UsageID") = usageA
				rs2.Fields("TypeID") = typeA
				rs2.UpdateBatch
				rs2.Close
				rs.UpdateBatch
				rs.MoveNext
			Loop
			rs.Close 	
				
			'*********** CREATE EVENT IN THE RESERVATIONS TO SHOW ROOM MOVE ************'
			Create_Event "ReservationID",request("ResID"),oldRoomNumber,newRoomNumber,"Move","Room"						
			if res2 <> "0" and res2 <> "" then
				Create_Event "ReservationID",res2,newRoomNumber,oldRoomNumber,"Move","Room"
			end if
			'*********** CREATE EVENT IN THE USAGE(S) TO SHOW ROOM MOVE
			usageIDs = split(usages, ",")
			usageIDsB = split(usagesB, ",")
			For i = 0 to UBound(usageIDs)
				Create_Event "UsageID",usageIDs(i),oldRoomNumber,newRoomNumber,"Move","Room"						
			Next
			For i = 0 to UBound(usageIDsB)
				if usageIDsB(i) <> "0" and usageIDsB(i) <> "" then
					Create_Event "UsageID",CDBL(usageIDsB(i)),newRoomNumber,oldRoomNumber,"Move","Room"
				end if
			Next
		
			'*********** CREATE NOTE RECORDS TO SHOW REASON FOR MOVE *******************'
			rs.Open "Select * from t_Notes where 1=2", cn, 3, 3
			rs.AddNew 
			rs.Fields("ReservationID") = resA
			rs.Fields("Note") = request("reason")
			rs.fields("DateCreated") = Now
			rs.fields("CreatedById") = Session("UserID")
			rs.UpdateBatch
			rs.Close
		
		end if
		
		If resStatus = "In-House" and (request("type") = "swap" or (request("type") = "spare" and CheckSecurity("reservations","MoveToSpare"))) then
			'******** CHECK OUT OLD ROOM and CHECK IN NEW ROOM *********'
			rs.Open "Select comboitemid from t_ComboItems where comboname = 'RoomStatus' and comboitem = 'Occupied'", cn, 3, 3
			OccupiedID = rs.Fields("ComboItemID")
			rs.Close
			rs.Open "Select comboitemid from t_ComboItems where comboname = 'RoomStatus' and comboitem = 'Dirty'", cn, 3, 3
			dirtyID = rs.Fields("ComboItemID")
			rs.Close
			rs.Open "Select Phone from t_Room where roomid = '" & request("oldRoomID") & "'", cn, 3, 3
			oldPhone = rs.Fields("Phone")
			rs.Close
			rs.OPen "Select Phone from t_ROom where roomid = '" & request("RoomID") & "'", cn, 3, 3
			newPhone = rs.FIelds("Phone")
			rs.Close
			rs.Open "Select Left(b.FirstName, 10) + ' ' + Left(b.LastName, 10) as Prospect from t_Reservations a inner join t_Prospect b on a.prospectid = b.prospectid where a.reservationid = '" & request("resID") & "'", cn, 3, 3
			prosName = rs.Fields("Prospect")
			rs.Close
			'****** CHECKOUT OLD ROOM ***********'
			rs.Open "Select * from t_RoomMessages where 1=2", cn, 3, 3
			rs.AddNew
			rs.Fields("SiteID") = 1
			rs.Fields("Action") = "CHECKOUT"
			rs.Fields("GuestName") = prosName
			rs.Fields("Flag") = 0
			rs.Fields("Extension") = oldPhone
			rs.Fields("PBXDateIn") = Date
			rs.Fields("Username") = Session("UserID")
			rs.Fields("RoomID") = request("oldRoomID")
			rs.UpdateBatch
			rs.Close
			rs.Open "Select StatusID from t_Room where roomid = '" & request("roomid") & "'", cn, 3, 3
			rs.Fields("StatusID") = DirtyID
			rs.UpdateBatch
			rs.Close
			'***********CHECK IN NEW ROOM AND THEN MARK DIRTY ***********'
			rs.Open "Select * from t_RoomMessages where 1=2", cn, 3, 3
			rs.AddNew
			rs.Fields("SiteID") = 1
			rs.Fields("Action") = "CHECKIN"
			rs.Fields("GuestName") = prosName
			rs.Fields("Flag") = 0
			rs.Fields("Extension") = newPhone
			rs.Fields("PBXDateIn") = Date
			rs.Fields("Username") = Session("UserID")
			rs.Fields("RoomID") = request("RoomID")
			rs.UpdateBatch
			rs.Close
			rs.Open "Select StatusID from t_Room where roomid = '" & request("roomid") & "'", cn, 3, 3
			rs.Fields("StatusID") = OccupiedID
			rs.UpdateBatch
			rs.Close
		End If
						
		if err <> 0 then
			cn.RollBackTrans
			sAns = err.Description
		elseif error <> "" then
			cn.RollBackTrans
			sAns = error
		else
			cn.CommitTrans
			'************* SEND EMAIL TO ALISON/DANA TO INFORM THEM OF ROOM MOVE****************'
			if request("type") = "spare" then
				strEmail = "ReservationID: " & request("resID") & " moved to spare room " & newRoomNumber & " from room " & oldRoomNumber & " for the date range " & request("inDate") & " - " & request("outDate") & " by " & Session("UserName") & ". Reason: " & request("reason") & "."
			else
				if res2 <> "" and res2 <> "0" then
					strEmail = "ReservationID: " & request("resID") & " swapped to room " & newRoomNumber & " from room " & oldRoomNumber & "(tied to a ReservationID: " & res2 & ") for the date range " & request("inDate") & " - " & request("outDate") & " by " & Session("UserName") & ". Reason: " & request("reason") & "."
				else				
					strEmail = "ReservationID: " & request("resID") & " swapped to room " & newRoomNumber & " from room " & oldRoomNumber & "(not tied to a reservation) for the date range " & request("inDate") & " - " & request("outDate") & " by " & Session("UserName") & ". Reason: " & request("reason") & "."
				end if			
			end if
			
			sTo = "roommoves@kingscreekplantation.com"
			sFrom = "administrator@kingscreekplantation.com"
			sBody = strEmail
			sSubject = "Front Desk Room Move"
			Send_Mail sFrom, sTo, sSubject, sBody
			
'			set myCDO = server.createobject("CDONTS.NewMail")
'			mycdo.to = "roommoves@kingscreekplantation.com"
'			mycdo.subject = "Front Desk Room Move"
'			mycdo.from = "administrator@kingscreekplantation.com"
'			mycDO.body = strEmail
'			myCDO.send
'			set myCDO = nothing
			sAns = "OK" 
		end if
	end if
elseif request("Function") = "Val_Res_CheckIn" then
	'******** MAKE SURE RESERVATION HAS A ROOM ASSIGNED*************'
	rs.Open "Select * from t_RoomAllocationMatrix where reservationid = '" & request("ReservationID") & "'", cn,3,3
	If rs.EOF and rs.BOF then
		rs.Close
		sANs = "Must Assign A Room In Order To Check In"
	Else
		rs.Close
		'***********MAKE SURE RESERVATION DOES NOT HAVE A BALANCE*******************'
		'***********EXPEDIA AND LEISURE LINK AND TNT AND MAL-CB AND EXIT AND ORBITZ AND Premier Vacations and CWorld INVOICES ARE ALLOWED TO HAVE A BALANCE****************'
		'***********A CLEANING FEE INVOICE CAN HAVE A BALANCE ONLY IF IT IS ALONG WITH ONE OF THE INVOICES MENTIONED ABOVE*********************'
		rs.OPen "Select Sum(Balance) as Balance from t_AccountItems where reservationid = '" & request("ReservationID") & "' and fintransid > 0 and fintransid not in (Select financialtranscodeid from t_FinancialTransactionCodes where transcodeid in (Select comboitemid from t_ComboItems where comboname = 'TransCode' and (comboitem = 'Expedia Billing' or comboitem = 'Plan With Tan' or comboitem = 'Leisure Link Billing' or comboitem = 'TNT' or comboitem = 'MAL-CB' or comboitem = 'Exit' or comboitem = 'Orbitz Billing' or comboitem = 'Premier Vacations' or comboitem = 'ICE' or comboitem = 'CWorld' or comboitem = 'Vacation Club')))", cn, 3, 3
		'************way to let guests check in with pending charges **********'
		If Not(IsNull(rs.Fields("Balance"))) and rs.Fields("Balance") > 0 then
			rs.Close
			rs.Open "Select * from t_AccountItems where reservationid = '" & request("ReservationID") & "' and fintransid = (Select financialtranscodeid from t_FInancialTransactionCodes where transcodeid = (Select comboitemid from t_ComboItems where comboname = 'TransCode' and comboitem = 'Cleaning Fee') and transtypeid = (Select comboitemid from t_ComboItems where comboname = 'TransCodeType' and comboitem = 'ReservationTrans'))", cn, 3, 3
			If rs.EOF and rs.BOF then	
				'**** No Cleaning Fee So Pull all items not the invoices mentioned above and total the balance*****'
				'**** Then Find any pending forces tied to these items and store them in a string
				'********Default value for sAns**********
				sAns = "Reservation Balance Must be Paid Before Checking In." 
				rs.Close
				rs.Open "Select * from t_AccountItems where reservationid = '" & request("ReservationID") & "' and fintransid > '0' and fintransid not in (Select financialtranscodeid from t_FinancialTransactionCodes where transcodeid in (Select comboitemid from t_ComboItems where comboname = 'TransCode' and (comboitem = 'Expedia Billing' or comboitem = 'Plan With Tan' or comboitem = 'Leisure Link Billing' or comboitem = 'TNT' or comboitem = 'MAL-CB' or comboitem = 'Exit' or comboitem = 'Orbitz Billing' or comboitem = 'Premier Vacations' or comboitem = 'ICE' or comboitem = 'CWorld' or comboitem = 'Vacation Club')))", cn, 3, 3
				balance = 0
				Do while not rs.EOF
					balance = balance + rs.Fields("balance")	
					rs2.Open "Select CCTransID from t_CCTrans where Approved = '0' and Imported = '0' and (ApplyTo = '" & rs.Fields("AccountItemID") & "' or applyto like '%," & rs.Fields("AccountItemID") & ",%' or ApplyTo like '" & rs.Fields("AccountItemID") & ",%' or applyto like '%," & rs.Fields("AccountItemID") & "') and (transtype = 'Force' or transtype = 'Charge')", cn, 3, 3
					Do while not rs2.EOF
						if instr(cctransid, rs2.FIelds("CCTransID")) = 0 then
							if cctransid = "" then
								cctransid = "'" & rs2.Fields("CCTransID") & "'"
							else
								cctransid = cctransid & ",'" & rs2.Fields("CCTransID") & "'"			
							end if
						end if
						rs2.MoveNext
					Loop	
					rs2.Close
					rs.MoveNext
				Loop
				rs.Close
				'***** If there is a balance check to see if there are pending charges and compare the sums
				if balance > 0 and cctransid <> "" then
					rs.Open "Select Sum(Amount) as CCAmt from t_CCTrans where cctransid in (" & cctransid & ")", cn, 3, 3
					If Not(IsNull(rs.Fields("CCAmt"))) and rs.Fields("CCAmt") > 0 then
						If CDBL(rs.Fields("CCAmt")) >= CDBL(balance) then
							sAns = "OK"
							End If
					End If
					rs.Close
				end if
			Else
				rs.Close
				rs.OPen "Select * from t_AccountItems where reservationid = '" & request("ReservationID") & "' and fintransid in (Select financialtranscodeid from t_FinancialTransactionCodes where transcodeid in (Select comboitemid from t_ComboItems where comboname = 'TransCode' and (comboitem = 'Expedia Billing' or comboitem = 'Plan With Tan' or comboitem = 'Leisure Link Billing' or comboitem = 'TNT' or comboitem = 'MAL-CB' or comboitem = 'Exit' or comboitem = 'Orbitz Billing' or comboitem = 'Premier Vacations' or comboitem = 'ICE' or comboitem = 'CWorld' or comboitem = 'Vacation Club')))", cn, 3, 3
				If rs.EOF and rs.BOF then
					'******** There are invoices other than a cleaning fee that need to be paid
					'******** So Check the balances of these invoices and find pending charges
					sAns = "Reservation Balance Must be Paid Before Checking In."
					rs.Close
					rs.Open "Select * from t_AccountItems where reservationid = '" & request("ReservationID") & "' and fintransid > '0' and fintransid not in (Select financialtranscodeid from t_FinancialTransactionCodes where transcodeid in (Select comboitemid from t_ComboItems where comboname = 'TransCode' and (comboitem = 'Expedia Billing' or comboitem = 'Plan With Tan' or comboitem = 'Leisure Link Billing' or comboitem = 'TNT' or comboitem = 'MAL-CB' or comboitem = 'Exit' or comboitem = 'Orbitz Billing' or comboitem = 'Premier Vacations' or comboitem = 'ICE' or comboitem = 'CWorld' or comboitem = 'Vacation Club')))", cn, 3, 3
					balance = 0
					Do while not rs.EOF
						balance = balance + rs.Fields("Balance")				
						rs2.Open "Select CCTransID from t_CCTrans where Approved = '0' and Imported = '0' and (ApplyTo = '" & rs.Fields("AccountItemID") & "' or applyto like '%," & rs.Fields("AccountItemID") & ",%' or ApplyTo like '" & rs.Fields("AccountItemID") & ",%' or applyto like '%," & rs.Fields("AccountItemID") & "') and (transtype = 'Force' or transtype = 'Charge')", cn, 3, 3
						Do while not rs2.EOF
							if instr(cctransid, rs2.FIelds("CCTransID")) = 0 then
								if cctransid = "" then
									cctransid = "'" & rs2.Fields("CCTransID") & "'"
								else
									cctransid = cctransid & ",'" & rs2.Fields("CCTransID") & "'"			
								end if
							end if
							rs2.MoveNext
						Loop	
						rs2.Close
						rs.MoveNext
					Loop
					rs.Close
					'***** If there is a balance check to see if there are pending charges and compare the sums
					if balance > 0 and cctransid <> "" then
						rs.Open "Select Sum(Amount) as CCAmt from t_CCTrans where cctransid in (" & cctransid & ")", cn, 3, 3
						If Not(IsNull(rs.Fields("CCAmt"))) and rs.Fields("CCAmt") > 0 then
							If CDBL(rs.Fields("CCAmt")) >= CDBL(balance) then
								sAns = "OK"
								End If
							End If
						rs.Close
					end if
				Else
					'****** Need to make sure there are no invoices other than cleaning fee and the above mentioned with a balance
					sAns = "Reservation Balance Must be Paid Before Checking In." 
					rs.Close
					rs.Open "Select * from t_AccountItems where reservationid = '" & request("ReservationID") & "' and fintransid > '0' and fintransid not in (Select financialtranscodeid from t_FInancialTransactionCodes where transcodeid in (Select comboitemid from t_ComboItems where comboname = 'TransCode' and (comboitem = 'Expedia Billing' or comboitem = 'Plan With Tan' or comboitem = 'Leisure Link Billing' or comboitem = 'TNT' or comboitem = 'MAL-CB' or comboitem = 'Exit' or comboitem = 'Cleaning Fee' or comboitem = 'Orbitz Billing' or comboitem = 'Premier Vacations' or comboitem = 'ICE' or comboitem = 'CWorld' or comboitem = 'Vacation Club')))", cn, 3, 3
					If rs.EOF and rs.BOF then
						sAns = "OK"
					Else
						balance = 0
						Do while not rs.EOF
							balance = balance + rs.Fields("Balance")				
							rs2.Open "Select CCTransID from t_CCTrans where Approved = '0' and Imported = '0' and (ApplyTo = '" & rs.Fields("AccountItemID") & "' or applyto like '%," & rs.Fields("AccountItemID") & ",%' or ApplyTo like '" & rs.Fields("AccountItemID") & ",%' or applyto like '%," & rs.Fields("AccountItemID") & "') and (transtype = 'Force' or transtype = 'Charge')", cn, 3, 3
							Do while not rs2.EOF
								if instr(cctransid, rs2.FIelds("CCTransID")) = 0 then
									if cctransid = "" then
										cctransid = "'" & rs2.Fields("CCTransID") & "'"
									else
										cctransid = cctransid & ",'" & rs2.Fields("CCTransID") & "'"			
									end if
								end if
								rs2.MoveNext
							Loop	
							rs2.Close
							rs.MoveNext
						Loop
						rs.Close
						'***** If there is a balance check to see if there are pending charges and compare the sums
						if balance > 0 and cctransid <> "" then
							rs.Open "Select Sum(Amount) as CCAmt from t_CCTrans where cctransid in (" & cctransid & ")", cn, 3, 3
							If Not(IsNull(rs.Fields("CCAmt"))) and rs.Fields("CCAmt") > 0 then
								If CDBL(rs.Fields("CCAmt")) >= CDBL(balance) then
									sAns = "OK"
									End If
								End If
							rs.Close
						end if
					End If
				End If
			End If					
		Else
			rs.Close
			sANs = "OK"
		End If	
	End If
	
	
	'********** Check In Validation Method Replace 4/8/08
		'If Not(IsNull(rs.Fields("Balance"))) and rs.Fields("Balance") > 0 then
		'	rs.Close
		'	rs.Open "Select * from t_AccountItems where reservationid = '" & request("ReservationID") & "' and fintransid = (Select financialtranscodeid from t_FInancialTransactionCodes where transcodeid = (Select comboitemid from t_ComboItems where comboname = 'TransCode' and comboitem = 'Cleaning Fee') and transtypeid = (Select comboitemid from t_ComboItems where comboname = 'TransCodeType' and comboitem = 'ReservationTrans'))", cn, 3, 3
		'	If rs.EOF and rs.BOF then				
		'		sAns = "Reservation Balance Must be Paid Before Checking In."
		'	Else
		'		rs.Close
		'		rs.OPen "Select * from t_AccountItems where reservationid = '" & request("ReservationID") & "' and fintransid in (Select financialtranscodeid from t_FinancialTransactionCodes where transcodeid in (Select comboitemid from t_ComboItems where comboname = 'TransCode' and (comboitem = 'Expedia Billing' or comboitem = 'Leisure Link Billing' or comboitem = 'TNT' or comboitem = 'MAL-CB' or comboitem = 'Exit' or comboitem = 'Orbitz Billing' or comboitem = 'Premier Vacations' or comboitem = 'ICE' or comboitem = 'CWorld' or comboitem = 'Vacation Club')))", cn, 3, 3
		'		If rs.EOF and rs.BOF then
		'			sAns = "Reservation Balance Must be Paid Before Checking In."
		'		Else
		'			sAns = "OK"
		'		End If
		'	End If					
		'Else
		'	sANs = "OK"
		'End If	
		'rs.Close
	'********** End Old Code ************
elseif request("Function") = "CheckIn" then
'**********Security Check ***********************
	'on error resume next
	if not(CheckSecurity("Reservations", "CheckIn")) then
		err.raise 
		err.number = -1
		err.description = "You Do Not Have Check-In Permissions"
	end if
'**********End Security Check *******************


		'**** Get Prospect Name ****'
		rs.Open "Select FirstName, LastName from t_Prospect where prospectid = (select prospectid from t_Reservations where reservationid = '" & request("reservationid") & "')", cn, 3, 3
		If rs.EOF and rs.BOF then
			prosName = "N/A"
		Else
			prosName = Left(rs.Fields("FirstName"), 10) & " " & Left(rs.Fields("LastName"), 10) & ""
		End If
		rs.Close
		
		'**** Get ComboItemID for In-House res status ****'
		rs.Open "Select ci.comboitemid from t_ComboItems ci inner join t_Combos c on ci.comboid = c.comboid where c.comboname = 'ReservationStatus' and ci.comboitem = 'In-House'", cn, 3, 3
		resStatusID = rs.Fields("ComboItemID")
		rs.Close
		
		'**** Get ComboItemID from Occupied Room status ****'
		rs.OPen "Select ci.COmboItemID from t_ComboItems ci inner join t_Combos c on ci.comboid = c.comboid where c.comboname = 'RoomStatus' and ci.comboitem = 'Occupied'", cn, 3, 3
		roomStatusID = rs.Fields("ComboItemID")
		rs.Close
		
		'**** Mark Room As Occupied then Create Check In Entries for Phone Interface for each room in reservation****'
		cn.BeginTrans
		rs.Open "Select Distinct RoomID from t_RoomAllocationMatrix where reservationid = '" & request("ReservationID") & "' and dateallocated >= '" & Date & "'", cn, 3, 3
		Do while not rs.EOF
			rs2.Open "Select Phone, StatusID from t_Room where roomid = '" & rs.Fields("RoomID") & "'", cn, 3, 3
			ext = rs2.Fields("Phone")
			Create_Event "RoomID",rs.Fields("roomid"),Get_Lookup(rs2.Fields("StatusID")),Get_Lookup(roomStatusID),"Change","StatusID"
			rs2.Fields("StatusID") = roomStatusID
			rs2.UpdateBatch
			rs2.Close
			
			rs2.Open "Select * from t_RoomMessages where 1=2", cn, 3, 3
			rs2.AddNew
			rs2.Fields("SiteID") = 1
			rs2.Fields("Action") = "CHECKIN"
			rs2.Fields("GuestName") = prosName
			rs2.Fields("Flag") = 0
			rs2.Fields("Extension") = ext
			rs2.Fields("PBXDateIn") = Date
			rs2.Fields("Username") = Session("UserID")
			rs2.Fields("RoomID") = rs.Fields("RoomID")
			rs2.UpdateBatch
			rs2.Close
			
			rs.MoveNext
		Loop
		rs.Close
			
		'**** Update ReservationStatus to In-House
		rs.Open "Select StatusID, StatusDate from t_Reservations where reservationid = '" & request("reservationid") & "'", cn, 3, 3
		Create_Event "ReservationID",request("reservationid"),Get_Lookup(rs.Fields("StatusID")),Get_Lookup(resStatusID),"Change","StatusID"
		rs.Fields("StatusID") = resStatusID
		rs.Fields("StatusDate") = Date
		rs.Updatebatch
		rs.Close
		Create_Event "ReservationID",request("reservationID"),"","","CHECKIN", ""
		
		if err <> 0 then
			cn.RollBackTrans
			if err.number = -1 then
				sAns = err.description
			else
				sAns = "An Error Occured Writng to the Database. " & err.description & ""		
			end if
		else
			cn.CommitTrans
			sAns = "OK"
		end if
elseif request("Function") = "CheckOut" then
'**********Security Check ***********************
'	on error resume next
	if not(CheckSecurity("Reservations", "CheckOut")) then
		'err.raise
		err.number = -1
		err.description = "You Do Not Have Check-Out Permissions"
	end if
'**********End Security Check *******************
	'**** Get Prospect Name ****'
	rs.Open "Select FirstName, LastName from t_Prospect where prospectid = (select prospectid from t_Reservations where reservationid = '" & request("reservationid") & "')", cn, 3, 3
	If rs.EOF and rs.BOF then
		prosName = "N/A"
	Else
		prosName = Left(rs.Fields("FirstName"), 10) & " " & Left(rs.Fields("LastName"), 10) & ""
	End If
	rs.Close
	
	'**** Get ComboItemID for Completed res status ****'
	rs.Open "Select comboitemid from t_ComboItems where comboname = 'ReservationStatus' and comboitem = 'Completed'", cn, 3, 3
	resStatusID = rs.Fields("ComboItemID")
	rs.Close
	
	'***** Get CheckOUtDate for reservation *******'
	rs.OPen "Select checkoutdate from t_Reservations where reservationid = '" & request("reservationid") & "'", cn, 3, 3
	coDate = rs.Fields("CheckOutDate") 
	rs.Close
	
	'**** Create Check-Out Entries for Phone Interface for each room in reservation****'
	cn.BeginTrans
	if CDate(coDate) = CDate(Date) then
		rs.Open "Select Distinct RoomID from t_RoomAllocationMatrix where reservationid = '" & request("ReservationID") & "' and dateallocated = '" & Date - 1 & "'", cn, 3, 3
	else	
		rs.Open "Select Distinct RoomID from t_RoomAllocationMatrix where reservationid = '" & request("ReservationID") & "' and dateallocated = '" & Date & "'", cn, 3, 3
	end if
	Do while not rs.EOF
		'Mark Room Dirty
		rs2.open "Select comboitemid from t_Comboitems where comboname = 'RoomStatus' and comboitem = 'dirty' and active = 1", cn, 0, 1
		if rs2.eof and rs2.bof then
			stat = 0
		else
			stat = rs2.fields("ComboitemID").value
		end if
		rs2.close
		
		rs2.Open "Select Phone, statusid from t_Room where roomid = '" & rs.Fields("RoomID") & "'", cn, 3, 3
		ext = rs2.Fields("Phone")
		Create_Event "RoomID",rs.Fields("roomid"),Get_Lookup(rs2.Fields("StatusID")),Get_Lookup(stat),"Change","StatusID"
		rs2.fields("StatusID").value = stat
		rs2.update
		rs2.Close
		
		rs2.Open "Select * from t_RoomMessages where 1=2", cn, 3, 3
		rs2.AddNew
		rs2.Fields("SiteID") = 1
		rs2.Fields("Action") = "CHECKOUT"
		rs2.Fields("GuestName") = prosName
		rs2.Fields("Flag") = 0
		rs2.Fields("Extension") = ext
		rs2.Fields("PBXDateIn") = Date
		rs2.Fields("Username") = Session("UserID")
		rs2.Fields("RoomID") = rs.Fields("RoomID")
		rs2.UpdateBatch
		rs2.Close
		
		rs.MoveNext
	Loop
	rs.Close
		
	'**** Update ReservationStatus to Completed
	rs.Open "Select StatusID, CheckOutDate, StatusDate from t_Reservations where reservationid = '" & request("reservationid") & "'", cn, 3, 3
	If rs.Fields("CheckOutDate") > Date then
		Create_Event "ReservationID",request("reservationid"),rs.Fields("CheckOUtDate"),Date,"Change","CheckOutDate"
		rs.Fields("CheckOutDate") = Date
		'******Loop Through Room Allocation Matrix and Free up Inventory*******'
		rs2.Open "Select * from t_RoomAllocationMatrix where reservationid = '" & request("reservationid") & "' and DateAllocated >= '" & Date & "'", cn, 3, 3
		Do while not rs2.EOF
			rs2.Fields("ReservationID") = 0
			rs2.UpdateBatch
			rs2.MoveNext
		Loop
		rs2.Close
	End If
	Create_Event "ReservationID",request("reservationid"),Get_Lookup(rs.Fields("StatusID")),Get_Lookup(resStatusID),"Change","StatusID"
	rs.Fields("StatusID") = resStatusID
	rs.Fields("StatusDate") = Date
	rs.Updatebatch
	rs.Close
	Create_Event "ReservationID",request("reservationID"),"","","CHECKOUT", ""
	
	'Check for Taxes
	rs.Open "Execute SP_RES_CHECK_OUT_TAXES " & request("reservationid"), cn, 3, 3
	If rs.Fields("Taxes") = 1 then
		sAns = "Taxes"
	End If
	rs.Close
	
	if err <> 0 then
		cn.RollBackTrans
		if err.number = -1 then
			sAns = err.description
		else
			sAns = "An Error Occured Writng to the Database. " & err.description & ""		
		end if
	else
		cn.CommitTrans
		if sAns = "" then
			sAns = "OK"
		end if
	end if
elseif request("Function") = "Interface_Check_In" then
	On Error Resume Next
	cn.BeginTrans
	
	'**** Get Prospect Name ****'
	rs.Open "Select FirstName, LastName from t_Prospect where prospectid = (select prospectid from t_Reservations where reservationid = '" & request("reservationid") & "')", cn, 3, 3
	prosName = Left(rs.Fields("FirstName"), 10) & " " & Left(rs.Fields("LastName"), 10) & ""
	rs.Close
	
	rs.Open "Select Distinct RoomID from t_RoomAllocationMatrix where reservationid = '" & request("ReservationID") & "' and dateallocated >= '" & Date & "'", cn, 3, 3
	Do while not rs.EOF
		'******* Get Extension of Room ******'
		rs2.Open "Select Phone from t_Room where roomid = '" & rs.Fields("RoomID") & "'", cn, 3, 3
		ext = rs2.Fields("Phone")
		rs2.Close
		
		rs2.Open "Select * from t_RoomMessages where 1=2", cn, 3, 3
		rs2.AddNew
		rs2.Fields("SiteID") = 1
		rs2.Fields("Action") = "CHECKIN"
		rs2.Fields("GuestName") = prosName
		rs2.Fields("Flag") = 0
		rs2.Fields("Extension") = ext
		rs2.Fields("PBXDateIn") = Date
		rs2.Fields("Username") = Session("UserID")
		rs2.Fields("RoomID") = rs.Fields("RoomID")
		rs2.UpdateBatch
		rs2.Close
		
		rs.MoveNext
	Loop
	rs.Close

	if err <> 0 then
		cn.RollBackTrans
		sAns = err.Description
	else
		cn.CommitTrans
		sAns = "OK"
	end if

elseif request("Function") = "Get_Usage_Reservations" then
	sAns = "<table border=0><tr><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>ReservationID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Prospect</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>InDate</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>OutDate</th></tr>"
	rs.Open "Select a.ReservationID, a.CheckInDate, a.CheckOutDate, b.FirstName, b.LastName from t_Reservations a left outer join t_Prospect b on a.prospectid = b.prospectid where a.reservationid in (Select Distinct(ReservationID) from t_RoomAllocationmatrix where usageid = '" & request("UsageID") & "')", cn, 3, 3
	Do while not rs.EOF
		sAns = sAns & "<tr><td>" & rs.Fields("ReservationID") & "</td><td>" & rs.Fields("FirstName") & " " & rs.Fields("LastName") & "</td><td>" & rs.Fields("CheckInDate") & "</td><td>" & rs.Fields("CheckOutDate") & "</td><td><img src = '../images/edit.gif' onclick = " & Chr(34) & "window.opener.navigate('../editReservation.asp?reservationid=" & rs.Fields("ReservationID") & "');window.close();" & Chr(34) & "></td></tr>"
		rs.MoveNext
	Loop
	rs.Close
	sAns = sAns & "</table>"
elseif request("Function") = "Reset_Phones" then
'**********Security Check ***********************
	on error resume next
	if not(CheckSecurity("Sys", "ResetPhones")) then
		'err.raise
		err.number = -1
		err.description = "Access Denied"
	end if
'**********End Security Check *******************

	'******* Get All In-House Reservations, Guests ********"
	cn.BeginTrans
	rs.Open "Select a.ReservationID, b.FirstName, b.LastName from t_Reservations a inner join t_Prospect b on a.prospectid = b.prospectid where a.statusid = (Select comboitemid from t_ComboItems where comboname = 'ReservationStatus' and comboitem = 'In-House') and a.reslocationid = (select comboitemid from t_ComboItems where comboname = 'ReservationLocation' and comboitem = 'KCP')", cn, 3, 3
	If rs.EOF and rs.BOF then
	Else
		Do while not rs.EOF
			rs2.Open "Select Distinct a.RoomID, b.Phone from t_RoomAllocationMatrix a inner join t_Room b on a.roomid = b.roomid where a.reservationid = '" & rs.Fields("ReservationID") & "' and a.DateAllocated >= '" & Date & "'", cn, 3, 3
			Do while not rs2.EOF
				rs3.Open "Select * from t_RoomMessages where 1=2", cn, 3, 3
				rs3.AddNew
				rs3.Fields("SiteID") = 1
				rs3.Fields("Action") = "CHECKIN"
				rs3.Fields("GuestName") = Left(rs.Fields("FirstName"), 10) & " " & Left(rs.Fields("LastName"), 10)
				rs3.Fields("Flag") = 0
				rs3.Fields("Extension") = rs2.Fields("Phone")
				rs3.Fields("PBXDateIn") = Date
				rs3.Fields("Username") = Session("UserID")
				rs3.Fields("RoomID") = rs2.Fields("RoomID")
				rs3.UpdateBatch
				rs3.Close
				rs2.MoveNext
			Loop
			rs2.Close
			rs.MoveNext
		Loop
	End If
	rs.Close
	
	if err <> 0 then
		cn.RollBackTrans
		if err.number = -1 then
			sAns = err.description
		else
			sAns = "An Error Occured Writng to the Database. " & err.description & ""		
		end if
	else
		cn.CommitTrans
		sAns = "OK"
	end if
elseif request("Function") = "Get_Waitlist" then
	if request("sort") = "owner" then
		if request("unittype") = "ALL" then
			if request("BD") = "ALL" then
				rs.Open "Select k.*, l.ComboItem as ReqSeason from (Select i.*, j.ComboItem As ConSeason from (Select g.*, h.ComboItem as UnitType from (Select e.*, f.UserName from (Select c.*, d.ContractNumber, d.SeasonID as ConSeasonID from (Select a.*, b.FirstName, b.LastName from t_ReservationWaitList a inner join t_Prospect b on a.prospectid = b.prospectid where OwnerStay = '1' and a.StartDate between Cast('" & request("sDate") & "' as datetime) and Cast('" & request("eDate") & "' as datetime) and a.active = '1') c inner join t_Contract d on c.contractid = d.contractid) e left outer join t_Personnel f on e.createdbyid = f.personnelid) g inner join t_ComboItems h on g.typeid = h.comboitemid) i left outer join t_ComboItems j on i.conseasonid = j.comboitemid) k left outer join t_ComboItems l on k.seasonid = l.comboitemid order by k.datecreated asc, waitlistid asc", cn, 3, 3
			else
				rs.Open "Select k.*, l.ComboItem as ReqSeason from (Select i.*, j.ComboItem As ConSeason from (Select g.*, h.ComboItem as UnitType from (Select e.*, f.UserName from (Select c.*, d.ContractNumber, d.SeasonID as ConSeasonID from (Select a.*, b.FirstName, b.LastName from t_ReservationWaitList a inner join t_Prospect b on a.prospectid = b.prospectid where a.OwnerStay = '1' and a.Bedrooms = '" & request("BD") & "' and a.StartDate between Cast('" & request("sDate") & "' as datetime) and Cast('" & request("eDate") & "' as datetime) and a.active = '1') c inner join t_Contract d on c.contractid = d.contractid) e left outer join t_Personnel f on e.createdbyid = f.personnelid) g inner join t_ComboItems h on g.typeid = h.comboitemid) i left outer join t_ComboItems j on i.conseasonid = j.comboitemid) k left outer join t_ComboItems l on k.seasonid = l.comboitemid order by k.datecreated asc, waitlistid asc", cn, 3, 3		
			end if		
		else
			if request("BD") = "ALL" then
				rs.Open "Select k.*, l.ComboItem as ReqSeason from (Select i.*, j.ComboItem As ConSeason from (Select g.*, h.ComboItem as UnitType from (Select e.*, f.UserName from (Select c.*, d.ContractNumber, d.SeasonID as ConSeasonID from (Select a.*, b.FirstName, b.LastName from t_ReservationWaitList a inner join t_Prospect b on a.prospectid = b.prospectid where OwnerStay = '1' and a.typeid = '" & request("unittype") & "' and a.StartDate between Cast('" & request("sDate") & "' as datetime) and Cast('" & request("eDate") & "' as datetime) and a.active = '1') c inner join t_Contract d on c.contractid = d.contractid) e left outer join t_Personnel f on e.createdbyid = f.personnelid) g inner join t_ComboItems h on g.typeid = h.comboitemid) i left outer join t_ComboItems j on i.conseasonid = j.comboitemid) k left outer join t_ComboItems l on k.seasonid = l.comboitemid order by k.datecreated asc, waitlistid asc", cn, 3, 3
			else
				rs.Open "Select k.*, l.ComboItem as ReqSeason from (Select i.*, j.ComboItem As ConSeason from (Select g.*, h.ComboItem as UnitType from (Select e.*, f.UserName from (Select c.*, d.ContractNumber, d.SeasonID as ConSeasonID from (Select a.*, b.FirstName, b.LastName from t_ReservationWaitList a inner join t_Prospect b on a.prospectid = b.prospectid where a.OwnerStay = '1' and a.typeid = '" & request("unittype") & "' and a.Bedrooms = '" & request("BD") & "' and a.StartDate between Cast('" & request("sDate") & "' as datetime) and Cast('" & request("eDate") & "' as datetime) and a.active = '1') c inner join t_Contract d on c.contractid = d.contractid) e left outer join t_Personnel f on e.createdbyid = f.personnelid) g inner join t_ComboItems h on g.typeid = h.comboitemid) i left outer join t_ComboItems j on i.conseasonid = j.comboitemid) k left outer join t_ComboItems l on k.seasonid = l.comboitemid order by k.datecreated asc, waitlistid asc", cn, 3, 3		
			end if
		end if
	else
		if request("unittype") = "ALL" then
			if request("BD") = "ALL" then
				rs.Open "Select k.*, l.ComboItem as ReqSeason from (Select i.*, j.ComboItem As ConSeason from (Select g.*, h.ComboItem as UnitType from (Select e.*, f.UserName from (Select c.*, d.ContractNumber, d.SeasonID as ConSeasonID  from (Select a.*, b.FirstName, b.LastName from t_ReservationWaitList a inner join t_Prospect b on a.prospectid = b.prospectid where a.OwnerStay = '0' and a.StartDate between Cast('" & request("sDate") & "' as datetime) and Cast('" & request("eDate") & "' as datetime) and a.active = '1') c left outer join t_Contract d on c.contractid = d.contractid) e left outer join t_Personnel f on e.createdbyid = f.personnelid) g inner join t_ComboItems h on g.typeid = h.comboitemid) i left outer join t_ComboItems j on i.conseasonid = j.comboitemid) k left outer join t_ComboItems l on k.seasonid = l.comboitemid order by k.datecreated asc, waitlistid asc", cn, 3, 3
			else
				rs.Open "Select k.*, l.ComboItem as ReqSeason from (Select i.*, j.ComboItem As ConSeason from (Select g.*, h.ComboItem as UnitType from (Select e.*, f.UserName from (Select c.*, d.ContractNumber, d.SeasonID as ConSeasonID from (Select a.*, b.FirstName, b.LastName from t_ReservationWaitList a inner join t_Prospect b on a.prospectid = b.prospectid where a.OwnerStay = '0' and a.Bedrooms = '" & request("BD") & "' and a.StartDate between Cast('" & request("sDate") & "' as datetime) and Cast('" & request("eDate") & "' as datetime) and a.active = '1') c left outer join t_Contract d on c.contractid = d.contractid) e left outer join t_Personnel f on e.createdbyid = f.personnelid) g inner join t_ComboItems h on g.typeid = h.comboitemid) i left outer join t_ComboItems j on i.conseasonid = j.comboitemid) k left outer join t_ComboItems l on k.seasonid = l.comboitemid order by k.datecreated asc, waitlistid asc", cn, 3, 3		
			end if	
		else
			if request("BD") = "ALL" then
				rs.Open "Select k.*, l.ComboItem as ReqSeason from (Select i.*, j.ComboItem As ConSeason from (Select g.*, h.ComboItem as UnitType from (Select e.*, f.UserName from (Select c.*, d.ContractNumber, d.SeasonID as ConSeasonID from (Select a.*, b.FirstName, b.LastName from t_ReservationWaitList a inner join t_Prospect b on a.prospectid = b.prospectid where a.OwnerStay = '0' and a.typeid = '" & request("unittype") & "' and a.StartDate between Cast('" & request("sDate") & "' as datetime) and Cast('" & request("eDate") & "' as datetime) and a.active = '1') c left outer join t_Contract d on c.contractid = d.contractid) e left outer join t_Personnel f on e.createdbyid = f.personnelid) g inner join t_ComboItems h on g.typeid = h.comboitemid) i left outer join t_ComboItems j on i.conseasonid = j.comboitemid) k left outer join t_ComboItems l on k.seasonid = l.comboitemid order by k.datecreated asc, waitlistid asc", cn, 3, 3
			else
				rs.Open "Select k.*, l.ComboItem as ReqSeason from (Select i.*, j.ComboItem As ConSeason from(Select g.*, h.ComboItem as UnitType from (Select e.*, f.UserName from (Select c.*, d.ContractNumber, d.SeasonID as ConSeasonID from (Select a.*, b.FirstName, b.LastName from t_ReservationWaitList a inner join t_Prospect b on a.prospectid = b.prospectid where a.OwnerStay = '0' and a.typeid = '" & request("unittype") & "' and a.Bedrooms = '" & request("BD") & "' and a.StartDate between Cast('" & request("sDate") & "' as datetime) and Cast('" & request("eDate") & "' as datetime) and a.active = '1') c inner join t_Contract d on c.contractid = d.contractid) e left outer join t_Personnel f on e.createdbyid = f.personnelid) g left outer join t_ComboItems h on g.typeid = h.comboitemid) i left outer join t_ComboItems j on i.conseasonid = j.comboitemid) k left outer join t_ComboItems l on k.seasonid = l.comboitemid order by k.datecreated asc, waitlistid asc", cn, 3, 3		
			end if			
		end if
	end if
	if rs.EOF and rs.BOF then
		sAns = "No Reservations Waiting in this Date Range."
	else
		sAns = "<table>"
		sAns = sAns & "<tr><th><u>Date Created</u></th><th><u>Owner</u></th><th><u>KCP #</u></th><th><u>InDate</u></th><th><u>OutDate</u></th><th><u>Unit Type</u></th><th><u>BR</u></th><th><u>Stay Type</u></th><th><u>Requested Season</u></th><th><u>Contract Season</u></th><th><u>CreatedBy</u></th></tr>"
		Do while not rs.EOF
			sAns = sAns & "<tr>"
			sAns = sAns & "<td>" & rs.Fields("DateCreated") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("FirstName") & " " & rs.Fields("LastName") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("ContractNumber") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("StartDate") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("EndDate") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("UnitType") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("Bedrooms") & "</td>"
			if request("sort") = "owner" then
				sAns = sAns & "<td>Owner Stay</td>"
			else
				sAns = sAns & "<td>Getaway</td>"
			end if
			sAns = sAns & "<td>" & rs.Fields("ReqSeason") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("ConSeason") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("UserName") & "</td>"
			sAns = sAns & "<td><input type = 'button' name = 'go' value = 'Remove' onclick = " & Chr(34) & "Remove_From_Waitlist('" & rs.Fields("WaitListID") & "');" & Chr(34) & "></td>"
			sAns = sAns & "</tr>"
			rs.MoveNext
		Loop
		sAns = sAns & "</table>"
	end if
	rs.Close
elseif request("Function") = "Add_To_Waitlist" then
	'On Error Resume Next
	cn.BeginTrans
	rs.Open "Select * from t_ReservationWaitlist where 1=2", cn, 3, 3
	rs.AddNew
	rs.Fields("ProspectID") = request("ProspectID")
	rs.Fields("ContractID") = request("ContractID")
	rs.Fields("StartDate") = CDate(request("sDate"))
	rs.Fields("EndDate") = CDate(request("eDate"))
	rs.Fields("typeid") = request("unittype")
	rs.Fields("Bedrooms") = request("bedrooms")
	rs.Fields("Active") = 1
	if request("sort") = "owner" then
		rs.Fields("OwnerStay") = 1
	else
		rs.Fields("OwnerStay") = 0
	end if
	rs.Fields("DateCreated") = Date
	rs.Fields("CreatedById") = Session("UserID")
	rs.Fields("SeasonID") = request("season")
	rs.UpdateBatch
	rs.Close
	
	if err<>0 then
		cn.RollbackTrans
		sAns = err.Description
	else
		cn.CommitTrans
		sAns = "OK"
	end if

elseif request("Function") = "Remove_From_WaitList" then
	on error resume next
	cn.BeginTrans
	rs.Open "Select * from t_ReservationWaitlist where waitlistid = '" & request("ID") & "'", cn, 3, 3
	rs.Fields("Active") = "0"
	rs.UpdateBatch
	rs.Close
	if err <> 0 then
		cn.RollBackTrans
		sAns = err.description
	else
		cn.committrans
		sAns = "OK"
	end if
elseif request("Function") = "In_House_Guests" then
	sAns = "<table>"
	if request("sort") = "name" then
		sANs = sAns & "<tr><th align = 'left' width = '150px'><u>Guest</u></th><th><u>State</u></th><th width = '150px' align = 'left'><u>Room(s)</u></th><th align = 'left' width = '150px'><u>Extension</u></th><th align = 'left' width = '150px'><u>InDate</u></th><th align = 'left' width = '150px'><u>OutDate</u></th><th><u>Type/SubType</u></th><th><u>Tour Campaign</u></th><th><u>ReservationID</u></th></tr>"
		rs.Open "Select e.*, f.COmboItem as ResSubTYpe from (Select c.*, d.COmboItem as ResType from (Select a.ReservationID, b.ProspectID, b.FirstName, b.LastName,State.Comboitem as State, a.CheckInDate, a.CheckOutDate, a.TypeID, a.SubTypeID from t_Reservations a inner join t_Prospect b on a.ProspectID = b.ProspectID left outer join t_Comboitems state on state.comboitemid = b.stateorprovince where a.statusid in (Select comboitemid from t_ComboItems where comboname = 'ReservationStatus' and comboitem = 'In-House') and reslocationid in (Select comboitemid from t_ComboItems where comboname = 'ReservationLocation' and comboitem = 'KCP')) c left outer join t_ComboItems d on c.TypeID = d.comboitemid) e left outer join t_ComboItems f on e.subtypeid = f.comboitemid order by e.Lastname asc", cn, 3, 3
		Do while not rs.EOF
			sAns = sAns & "<tr style='border-top:thin solid black;'>"
			sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("LastName") & ", " & rs.Fields("FirstName") & "</td>"
			sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.fields("State").value & "</td>"
			sAns = sAns & "<td style='border-top:thin solid black;'>"
			if CDate(rs.Fields("CheckOUtDate")) = Date then
				sDate = Cdate(Date - 1)
			else
				sDate = Date
			end if
			'***** Older QUery replace 4/1/2008 *******'
			'rs2.Open "Select RoomNumber, left(ut.comboitem, 1) as Unittype from t_Room r left outer join t_Unit u on u.unitid = r.unitid left outer join t_Comboitems ut on ut.comboitemid = u.typeid where roomid in (Select Distinct(RoomID) from t_RoomAllocationmatrix where reservationid = '" & rs.Fields("ReservationID") & "') order by CHARINDEX('-',RoomNumber), RoomNumber", cn, 3, 3
			rs2.Open "Select RoomNumber, left(ut.comboitem, 1) as Unittype from t_Room r left outer join t_Unit u on u.unitid = r.unitid left outer join t_Comboitems ut on ut.comboitemid = u.typeid where roomid in (Select Distinct(RoomID) from t_RoomAllocationmatrix where reservationid = '" & rs.Fields("ReservationID") & "' and DateALlocated = '" & sDate & "') order by CHARINDEX('-',RoomNumber), RoomNumber", cn, 3, 3
			Do while not rs2.EOF
				sAns = sAns & rs2.Fields("RoomNumber") & " - " & rs2.fields("UnitType").value & "<br>"
				rs2.MoveNext
			Loop
			rs2.Close
			sAns = sAns & "</td>"
			sAns = sAns & "<td style='border-top:thin solid black;'>"
			rs2.Open "Select Phone from t_Room where roomid in (Select Distinct(RoomID) from t_RoomAllocationmatrix where reservationid = '" & rs.Fields("ReservationID") & "' and dateallocated = '" & sDate & "') order by CHARINDEX('-',RoomNumber), RoomNumber", cn, 3, 3
			Do while not rs2.EOF
				sAns = sAns & rs2.Fields("Phone") & "<br>"
				rs2.MoveNext
			Loop
			rs2.Close			
			sANs = sAns & "</td>"	
			sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("CheckInDate") & "</td>"			
			sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("CheckOutDate") & "</td>"
			sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("ResType") & "/" & rs.Fields("ResSubType") & "</td>"
			rs2.Open "Select a.CampaignName from t_Campaign a inner join (Select campaignID from t_Tour where prospectid = '" & rs.Fields("ProspectID") & "' and tourdate between '" & rs.Fields("CheckInDate") & "' and '" & rs.Fields("CheckOutDate") & "') b on a.campaignid = b.campaignID", cn, 3, 3
			If rs2.EOF and rs2.BOF then
				sAns = sAns & "<td style='border-top:thin solid black;'>&nbsp</td>"
			else
				sAns = sAns & "<td style='border-top:thin solid black;'>" & rs2.Fields("CampaignName") & "</td>"
			end if
			rs2.Close
			sAns = sAns & "<td style='border-top:thin solid black;' align = right>" & rs.Fields("ReservationID") & "</td>"
			sAns = sAns & "<td><a href = '../editReservation.asp?reservationid=" & rs.Fields("reservationID") & "'><img src = '../images/edit.gif'></a></td>"
			sAns = sAns & "</tr>"
			rs2.open "select * from t_Guest where guestid in (select guestid from t_Res2Guest where reservationid = '" & rs.fields("ReservationID").value & "')", cn, 0, 1
			if rs2.eof and rs2.bof then
			else
				sAns = sAns & "<tr>"
				sAns = sAns & "<td colspan = 9>Additional Guests</td>"
				sAns = sAns & "</tr>"
				do while not rs2.eof
					sAns = sANs & "<tr>"
					sAns = sAns & "<td colspan = 9>" & rs2.fields("Lastname").value & ", " & rs2.fields("Firstname").value & "</td>"
					sAns = sANs & "</tr>"
					rs2.movenext
				loop
			end if
			rs2.close
			rs.MoveNext
		Loop
		rs.Close
	else
		sANs = sAns & "<tr><th width = '100px' align = 'left'><u>Room</u></th><th align = 'left'><u>Guest</u></th><th><u>State</u></th><th width = '150px' align = 'left'><u>Phone</u></th><th width = '150px' align = 'left'><u>InDate</u></th><th width = '150px' align = 'left'><u>OutDate</u></th><th><u>Type/SubType</u></th><th><u>Tour Campaign</u></th><th><u>ReservationID</u></th></tr>"
		'******* Older Query replace 4/1/2008
		'rs.Open "Select i.*, j.ComboItem as ResSubType from (Select g.*, h.ComboItem as ResType from (Select e.*, f.FirstName, f.Lastname, state.comboitem as state  from (Select c.*, d.ProspectID, d.CheckInDate, d.CheckOutDate, d.TypeID, d.SubTypeID from (Select a.*, b.RoomNumber, b.Phone, left(ut.comboitem,1) as unittype from (Select Distinct(RoomID), ReservationID from t_RoomAllocationmatrix where dateallocated = '" & Date & "' and reservationid in (Select reservationid from t_Reservations where statusid in (Select comboitemid from t_Comboitems where comboname = 'ReservationStatus' and comboitem = 'In-House'))) a inner join t_Room b on a.roomid = b.roomid left outer join t_Unit unit on unit.unitid = b.unitid left outer join t_Comboitems ut on ut.comboitemid = unit.typeid) c inner join t_Reservations d on c.reservationid = d.reservationid) e inner join t_Prospect f on e.prospectid = f.prospectid left outer join t_Comboitems state on state.comboitemid = f.stateorprovince) g left outer join t_CombOItems h on g.TypeID = h.comboitemid) i left outer join t_ComboItems j on i.subtypeid = j.comboitemid Order by CHARINDEX('-', i.RoomNumber), i.RoomNumber", cn, 3, 3 
		rs.Open "Select z.* from (Select i.*, j.ComboItem as ResSubType from (Select g.*, h.ComboItem as ResType from (Select e.*, f.FirstName, f.Lastname, state.comboitem as state  from (Select c.*, d.ProspectID, d.CheckInDate, d.CheckOutDate, d.TypeID, d.SubTypeID from (Select a.*, b.RoomNumber, b.Phone, left(ut.comboitem,1) as unittype from (Select Distinct(RoomID), ReservationID from t_RoomAllocationmatrix where dateallocated = '" & Date & "' and reservationid in (Select reservationid from t_Reservations where statusid in (Select comboitemid from t_Comboitems where comboname = 'ReservationStatus' and comboitem = 'In-House'))) a inner join t_Room b on a.roomid = b.roomid left outer join t_Unit unit on unit.unitid = b.unitid left outer join t_Comboitems ut on ut.comboitemid = unit.typeid) c inner join t_Reservations d on c.reservationid = d.reservationid) e inner join t_Prospect f on e.prospectid = f.prospectid left outer join t_Comboitems state on state.comboitemid = f.stateorprovince) g left outer join t_CombOItems h on g.TypeID = h.comboitemid) i left outer join t_ComboItems j on i.subtypeid = j.comboitemid UNION Select i.*, j.ComboItem as ResSubType from (Select g.*, h.ComboItem as ResType from (Select e.*, f.FirstName, f.Lastname, state.comboitem as state  from (Select c.*, d.ProspectID, d.CheckInDate, d.CheckOutDate, d.TypeID, d.SubTypeID from (Select a.*, b.RoomNumber, b.Phone, left(ut.comboitem,1) as unittype from (Select Distinct(RoomID), ReservationID from t_RoomAllocationmatrix where dateallocated = '" & Date - 1 & "' and reservationid in (Select reservationid from t_Reservations where statusid in (Select comboitemid from t_Comboitems where comboname = 'ReservationStatus' and comboitem = 'In-House') and checkoutDate = '" & Date & "')) a inner join t_Room b on a.roomid = b.roomid left outer join t_Unit unit on unit.unitid = b.unitid left outer join t_Comboitems ut on ut.comboitemid = unit.typeid) c inner join t_Reservations d on c.reservationid = d.reservationid) e inner join t_Prospect f on e.prospectid = f.prospectid left outer join t_Comboitems state on state.comboitemid = f.stateorprovince) g left outer join t_CombOItems h on g.TypeID = h.comboitemid) i left outer join t_ComboItems j on i.subtypeid = j.comboitemid) z Order by CHARINDEX('-', z.RoomNumber), z.RoomNumber", cn, 3, 3 
		Do while not rs.EOF
			sAns = sAns & "<tr style='border-top:thin solid black;'>"
			sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("RoomNumber") & " - " & rs.fields("Unittype").value & "</td>"
			sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("LastName") & ", " & rs.Fields("FirstName") & "</td>"
			sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.fields("State").value & "</td>"
			sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("Phone") & "</td>"
			sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("CheckInDate") &"</td>"
			sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("CheckOutDate") & "</td>"
			sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("ResType") & "/" & rs.Fields("ResSubType") & "</td>"
			rs2.Open "Select a.CampaignName from t_Campaign a inner join (Select campaignID from t_Tour where prospectid = '" & rs.Fields("ProspectID") & "' and tourdate between '" & rs.Fields("CheckInDate") & "' and '" & rs.Fields("CheckOutDate") & "') b on a.campaignid = b.campaignID", cn, 3, 3
			If rs2.EOF and rs2.BOF then
				sAns = sAns & "<td style='border-top:thin solid black;'>&nbsp</td>"
			else
				sAns = sAns & "<td style='border-top:thin solid black;'>" & rs2.Fields("CampaignName") & "</td>"
			end if
			rs2.Close
			sAns = sAns & "<td style='border-top:thin solid black;' align = right>" & rs.Fields("ReservationID") & "</td>"
			sAns = sAns & "<td><a href = '../editReservation.asp?reservationid=" & rs.Fields("reservationID") & "'><img src = '../images/edit.gif'></a></td>"
			sAns = sAns & "</tr>"
			rs2.open "select * from t_Guest where guestid in (select guestid from t_Res2Guest where reservationid = '" & rs.fields("ReservationID").value & "')", cn, 0, 1
			if rs2.eof and rs2.bof then
			else
				sAns = sAns & "<tr>"
				sAns = sAns & "<td colspan = 9>Additional Guests</td>"
				sAns = sAns & "</tr>"
				do while not rs2.eof
					sAns = sANs & "<tr>"
					sAns = sAns & "<td colspan = 9>" & rs2.fields("Lastname").value & ", " & rs2.fields("Firstname").value & "</td>"
					sAns = sANs & "</tr>"
					rs2.movenext
				loop
			end if
			rs2.close
			rs.MoveNext
		Loop
		rs.Close
	end if
	sAns = sAns & "</table>"
elseif request("Function") = "ListPros" then
	If request("ProspectID") = "" or request("ProspectID") = "0" then
		sAns = "<table border = 0><tr><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>ID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Number</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Location</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Check-In</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Check-Out</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Date Booked</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Status</th><th></th></tr>"
	Else
		rsR.open "select reservationid, resnumber, rl.comboitem as reslocation, checkindate, checkoutdate, datebooked, l.comboitem as status from t_reservations r left outer join t_ComboItems rl on rl.comboitemid = reslocationid left outer join t_ComboItems l on l.comboitemid = r.statusid where prospectid = " & request("ProspectID"),cn,3,3
		rsG.open "select g.ProspectID, g.GroupReservationID, g.CheckInDate, g.CheckOutDate from t_GroupReservations g where g.ProspectID = '" & request("prospectid") & "' order by g.GroupReservationID desc",cn,3,3
		'**************PROSPECT HAS NEITHER RESERVATIONS OR GROUPS************************
		if (rsR.EOF and rsR.BOF) and (rsG.EOF and rsG.BOF) then
			sAns = "No Reservations"
		
		'**************PROSPECT HAS RESERVATIONS BUT NO GROUPS****************************
		elseif not(rsR.eof and rsR.bof) and (rsG.eof and rsG.eof) then
			sAns = "<table border = 0><tr><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>ID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Number</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Location</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Check-In</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Check-Out</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Date Booked</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Status</th><th></th></tr>"
			if request("prospectid")= "" or request("prospectid") = "0" then
			else
			do while not rsR.eof
				sAns = sAns & "<tr>"
				for i = 0 to rsR.fields.count-1
					sAns = sAns & "<td style='border-bottom:solid thin black'>" & rsR.fields(i).value & "</td>"
				next
				sAns = sAns & "<td><a href='editreservation.asp?reservationid=" & rsR.fields("reservationid").value & "'><img src='images/edit.gif'></a></td>"
				sAns = sAns & "</tr>"
				rsR.movenext
			LOOP
			rsR.close
			end if
			
		'***************PROSPECT HAS NO RESERVATIONS BUT HAS GROUPS**************************
		elseif (rsR.eof and rsR.bof) and not(rsG.eof and rsG.bof) then
			sAns = "<table><tr><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>ID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Check-In</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Check-Out</th></tr>"
			if request("prospectid") = "" or request("prospectid") = "0" then
			else
			do while not rsG.eof
				sAns = sAns & "<tr>"
				for i = 1 to rsG.fields.count-1
					sAns = sAns & "<td style='border-bottom:solid thin black'>" & rsG.fields(i).value & "</td>"
				next
				sAns = sAns & "<td><a href='editresgroup.asp?groupid=" & rsG.fields("GroupReservationID").value & "'><img src='images/edit.gif'></a></td>"
				sAns = sAns & "</tr>"
				rsG.movenext
			LOOP
			rsG.close
			end if
	
		'****************PROSPECT HAS BOTH RESERVATIONS AND GROUPS******************************
		elseif not(rsR.eof and rsR.bof) and not(rsG.eof and rsG.bof) then
			sAns = "<h4><u>Reservations</u></h4><br><table border = 0><tr><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>ID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Number</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Location</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Check-In</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Check-Out</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Date Booked</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Status</th><th></th></tr>"
			if request("prospectid")= "" or request("prospectid") = "0" then
			else
			do while not rsR.eof
				sAns = sAns & "<tr>"
				for i = 0 to rsR.fields.count-1
					sAns = sAns & "<td style='border-bottom:solid thin black'>" & rsR.fields(i).value & "</td>"
				next
				sAns = sAns & "<td><a href='editreservation.asp?reservationid=" & rsR.fields("reservationid").value & "'><img src='images/edit.gif'></a></td>"
				sAns = sAns & "</tr>"
				rsR.movenext
			LOOP
			sAns = sAns & "</table>"
			rsR.close
			sAns = sAns & "<h4><u>Groups</u></h4><br><table><tr><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>ID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Check-In</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Check-Out</th></tr>"
			do while not rsG.eof
				sAns = sAns & "<tr>"
				for i = 1 to rsG.fields.count-1
					sAns = sAns & "<td style='border-bottom:solid thin black'>" & rsG.fields(i).value & "</td>"
				next
				sAns = sAns & "<td><a href='editresgroup.asp?groupid=" & rsG.fields("GroupReservationID").value & "'><img src='images/edit.gif'></a></td>"
				sAns = sAns & "</tr>"
				rsG.movenext
			LOOP
			rsG.close
			end if
		else
			response.write "You messed up somewhere, dude!"
		end if
	end if
elseif request("Function") = "GetReservationOwner" then
	if request("ReservationID") = "" or request("ReservationID") = "0" then
	else
		rs.open "Select p.lastname + ', ' + p.firstname as Name, p.prospectid from t_Prospect p where prospectid in (select prospectid from t_Reservations where reservationid = '" & request("ReservationID") & "')",cn,3,3
		if rs.eof and rs.bof then
			response.write "No Owner Found"
		else
			response.write rs.fields("Name").value & " <a href = 'editpros.asp?prosid=" & rs.fields("prospectid").value & "'><img src = 'images/edit.gif'></a>"
			sAns = ""
		end if
		rs.close
	end if	
	
elseif request("Function") = "Get_Out_Date" then
	sAns = Cdate(CDate(request("InDate")) + request("Nights"))
elseif request("Function") = "ListPackageIssued" then
	sAns = "<table border = 0><tr><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>ID</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Number</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Location</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Check-In</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Check-Out</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Date Booked</th><th STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>Status</th><th></th></tr>"
	if request("PackageIssuedID") = "" or request("PackageIssuedID") = "0" then
	else
		rs.open "select reservationid, resnumber, rl.comboitem as reslocation, checkindate, checkoutdate, datebooked, l.comboitem as status from t_reservations r left outer join t_ComboItems rl on rl.comboitemid = reslocationid left outer join t_ComboItems l on l.comboitemid = r.statusid where PackageIssuedID = " & request("PackageIssuedID"),cn,3,3
		do while not rs.eof
			sAns = sAns & "<tr>"
			for i = 0 to rs.fields.count -1
				sAns = sAns & "<td STYLE='BORDER-BOTTOM:SOLID THIN BLACK'>" & rs.fields(i).value & "&nbsp;</td>"
			next
			sAns = sAns & "<td><a href = 'editreservation.asp?reservationid=" & rs.fields("ReservationID").value & "'><img src = 'images/edit.gif'></a></td>"
			sAns = sAns & "</tr>"
			rs.movenext
		loop
		rs.close
	end if
	sAns = sAns & "</table>"
	
elseif request("Function") = "Get_Docs" then
	rs.open "Select Path,FileID, Name, DateUploaded from t_ReservationUploads u where u.reservationid = '" & request("ReservationID") & "' order by u.dateuploaded desc",cn,0,1
	response.write "<table><tr>"
	for i = 1 to rs.fields.count -1
		response.write "<th>" & rs.fields(i).name & "</th>"
	next
	response.write "<th></th></tr>"
	do while not rs.eof
		response.write "<tr>"
		for i = 1 to rs.fields.count -1
			response.write "<td>" & rs.fields(i).value & "</td>"
		next
		sName = ""
		for i = 1 to len(cstr(rs.fields("Path").value & "")) 
			if left(right(cstr(rs.fields("Path").value & ""), i), 1) = "\" then
				sName = right(cstr(rs.fields("Path").value), i -1)
				exit for
			end if
		next
		response.write "<td><img src = 'images/edit.gif' onclick = " & chr(34) & "var mwin = window.open();mwin.navigate('scannedcontracts/scannedcontracts/" & Replace(sName, "'", "\'") & "');" & Chr(34) & "></td>"
		'response.write "<td><input type='button' value = 'rename' onclick='rename(" & rs.fields("FileID").value & ");'></td>"
		response.write "</tr>"
		rs.movenext
	loop
	rs.close
	response.write "</table>"

elseif request("Function") = "StatusChecker" then
	rs2.open "select comboitem from t_comboitems where comboitemid = '" & request("stat") & "' ",cn,3,3
	stat = rs2.fields("comboitem").value
	rs2.close
	if stat = "Cancelled" then
		rs2.open "Select * from t_roomallocationmatrix where reservationid = '" & request("resid") & "' ",cn,3,3
		if rs2.eof and rs2.bof then
			
		else
			room = "Room"
		end if
		rs2.close
		response.write stat & "|" & room & "|" & request("ID") '"Room must be Removed before Changing Staus to Cancelled"
	end if
elseif request("Function") = "Room_Remover" then
		rs2.OPen "Select distinct(reservationid) as ResID from t_RoomAllocationMatrix where reservationid = '" & request("res") & "' ", cn, 3, 3
		if rs2.bof and rs2.eof then
			response.write "Wrong ID"
		else
			response.write "OK"
		end if
		rs2.close

'		Do while not rs2.EOF
'			rs2.Fields("ReservationID") = 0
'			rs2.MoveNext
'		Loop
'		rs2.UpdateBatch
'		rs2.Close
'
'		rs2.Open "Select RoomNumber from t_Room where roomid = '" & request("RoomID") & "'", cn, 3, 3
'		oldRoomNumber = rs2.Fields("RoomNumber")
'		rs2.Close
'		Create_Event "ReservationID",request("ID"),oldRoomNumber,"","Remove","Room"		
'		sAns = "OK"
elseif request("Function") = "Get_Reservation" then
'**********Security Check ***********************
	on error resume next
	if not(CheckSecurity("Reservations", "View")) then
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
	rs2.Open "Select comboitemid from t_COmboITems where comboname = 'ReservationStatus' and comboitem = 'In-House'", cn, 3, 3
	inHouse = rs2.Fields("ComboItemID")
	rs2.Close
	rs2.open "Select comboitemid, comboitem from t_comboitems where comboname = 'ReservationStatus' and comboitem = 'Completed'", cn, 3, 3
	completedid = rs2.fields("ComboItemID")
	completed = rs2.fields("ComboItem")
	rs2.close
	response.write "<script type= 'text/Javascript' src='../scripts/scw.js'></script>"
	if request("ReservationID") = "0" OR request("ReservationID") = "" then
		cn.begintrans
		If request("ProspectID") <> "" and request("ProspectID") <> "0" then
			rs.OPen "Select firstname, lastname, prospectid from t_Prospect where prospectid = '" & request("ProspectID") & "'", cn, 3, 3
			pFname = rs.Fields("FirstName")
			pLname = rs.Fields("LastName")
			pID = rs.Fields("prospectID")
			rs.Close
		ElseIf request("PackageIssuedID") <> "" and request("PackageIssuedID") <> "0" Then
			rs.Open "Select p.firstname, p.lastname, p.prospectid from t_Prospect p inner join t_PackageIssued i on i.prospectid = p.prospectid where i.packageissuedid = '" & request("PackageIssuedID") & "'", cn, 3, 3
			pFname = rs.Fields("FirstName")
			pLname = rs.Fields("LastName")
			pID = rs.Fields("prospectID")
			pkgID = request("PackageIssuedID")
			rs.Close
		ElseIf request("ContractID") <> "" and request("ContractID") <> "0" Then
			rs.Open "Select p.FirstName, p.LastName, p.ProspectID, c.ContractNumber from t_Contract c inner join t_Prospect p on c.prospectid = p.prospectid where c.contractid = '" & request("ContractID") & "'", cn, 3, 3
			pFname = rs.Fields("Firstname")
			pLname = rs.Fields("lastName")
			pID = rs.Fields("ProspectID")
			conNum = rs.Field("contractnumber")
			conID = request("ContractID")		
		End If 
		rs.OPen "Select * from t_Reservations where 1=2", cn, 3, 3
		rs.AddNew
	Else
		rs.open "Select r.*, p.lastname, p.firstname, c.contractnumber from t_Reservations r left outer join t_Prospect p on p.prospectid = r.prospectid left outer join t_Contract c on c.contractid = r.contractid where reservationid = '" & replace(request("ReservationID"),"'","''") & "'", cn, 3, 3
		pkgID = rs.Fields("PackageIssuedID") & ""
		pID = rs.Fields("ProspectID") & ""
		conID = rs.Fields("ContractID") & ""
		conNum = rs.Fields("ContractNumber") & ""
		Create_Event "ReservationID", request("reservationID"),"","","View",""
	End If
%>

<div id = "reservation" style="position: absolute; width: 126px;  z-index: 0; left: 177px; top: 215px">
	<form method="POST" name="reservation" id="reservationform" action="modules/reservations.asp">
	<table border="0" id="table1" width="661">
		<tr>
			<td>Owner:</td>
					<td>
						<%if request("reservationid") = "0" or request("reservationid") = "" then%>
							<a href = "editpros.asp?ProsID=<%=pID%>"><%=pFname & " " & pLname%></a>
						<%else%>
							<a href = "editpros.asp?ProsID=<%=rs.Fields("ProspectID")%>"><%=rs.Fields("FirstName") & " " & rs.Fields("LastName")%></a>
						<%end if%>
					</td>
					<td>Contract:</td>
			<td width="148">
				<%if conid = "0" or conid = "" then%>
					Not Assigned
				<%else%>
					<a href = "editcontract.asp?ContractID=<%=conID%>"><%=conNum%></a>
				<%end if%>
			</td>
		</tr>
		<tr>
			<td>Reservation ID:</td>
			<td>
			<input type="text" name="reservationid" size="20" value="<%=rs.fields("ReservationID").value%>" readonly></td>
			<td>Status:</td>
			<td width="148">
			<%
			if rs.fields("statusID").value = completedid then 
			%>
			<select name ="StatusID" readonly>
				<%
					set rsL = server.createobject("ADODB.Recordset")
					rsL.open "Select * from t_Comboitems where comboname = 'ReservationStatus' and (comboitemid = '" & completedid & "') order by comboitem",cn,3,3
					do while not rsL.eof
						if rs.fields("StatusID").value = rsL.fields("ComboItemID").value then
						%>
						<option selected value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						else
						%>
						<option value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						end if
						rsL.movenext
					loop
					rsL.close
					set rsL = nothing
				%>			
			</select>
			<%
			elseif rs.fields("statusID").value = inHouse then
			%>
			<select name ="StatusID" readonly>
				<%
					set rsL = server.createobject("ADODB.Recordset")
					rsL.open "Select * from t_Comboitems where comboname = 'ReservationStatus' and (comboitemid = '" & inHouse & "') order by comboitem",cn,3,3
					do while not rsL.eof
						if rs.fields("StatusID").value = rsL.fields("ComboItemID").value then
						%>
						<option selected value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						else
						%>
						<option value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						end if
						rsL.movenext
					loop
					rsL.close
					set rsL = nothing
				%>			
			</select>
			<%
			else
			%>			
			<select size="1" name="StatusID" onchange="Check_Status();"><option value = 0></option>
				<%
					set rsL = server.createobject("ADODB.Recordset")
					rsL.open "Select * from t_Comboitems where comboname = 'ReservationStatus' and (Active = 1 or comboitemid = '" & rs.Fields("StatusID") & "') order by comboitem",cn,3,3
					do while not rsL.eof
						if rs.fields("StatusID").value = rsL.fields("ComboItemID").value then
						%>
						<option selected value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						else
						%>
						<option value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						end if
						rsL.movenext
					loop
					rsL.close
					set rsL = nothing
				%>

				</select>
			<%
			end if
			%>
			</td>
		</tr>
		<tr>
			<td>Reservation Number:</td>
			<td>
			<input type="text" name="ReservationNumber" size="20" value="<%=rs.fields("ReservationNumber").value%>"></td>
			<td>Status Date:</td>
			<td width="148">
			<input type = "text" name = "StatusDate" size = "20" value ="<%If request("reservationid") = "0" then response.write Date Else response.write rs.Fields("StatusDate") End If%>" Readonly>
			</td>
		</tr>
		<tr>
			<td>Location:</td>
			<td><select size="1" name="ResLocationID"><option value = 0></option>
				<%
					set rsL = server.createobject("ADODB.Recordset")
					rsL.open "Select * from t_Comboitems where comboname = 'ReservationLocation' and (Active = 1 or comboitemid = '" & rs.Fields("resLocationID") & "') order by comboitem",cn,3,3
					do while not rsL.eof
						if rs.fields("ResLocationID").value = rsL.fields("ComboItemID").value then
						%>
						<option selected value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						else
						%>
						<option value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						end if
						rsL.movenext
					loop
					rsL.close
					set rsL = nothing
				%>

				</select></td>
			<td>Type:</td>
			<td width="148"><select size="1" name="TypeID"><option value = 0></option>
				<%
					set rsL = server.createobject("ADODB.Recordset")
					rsL.open "Select * from t_Comboitems where comboname = 'ReservationType' and (Active = 1 or comboitemid = '" & rs.Fields("TypeID") & "') order by comboitem",cn,3,3
					do while not rsL.eof
						if cstr(rs.fields("TypeID").value & "") = cstr(rsL.fields("ComboItemID").value & "") then
						%>
						<option selected value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						else
						%>
						<option value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						end if
						rsL.movenext
					loop
					rsL.close
					set rsL = nothing
				%>

				</select></td>
		</tr>
		<tr>
			<td>Check-In:</td>
			<td>
			<input type= text name= CheckInDate size="20" value="<%=rs.fields("CheckInDate").value%>" onClick = 'scwShow(this, this);'>
			</td>
			<td>Sub-Type:</td>
			<td width="148">
			<select size="1" name="SubTypeID"><option value = 0></option>
				<%
					set rsL = server.createobject("ADODB.Recordset")
					rsL.open "Select * from t_Comboitems where comboname = 'ReservationSubType' and (Active = 1 or comboitemid = '" & rs.Fields("SubTypeID") & "')  order by comboitem",cn,3,3
					do while not rsL.eof
						if cstr(rs.fields("SubTypeID").value & "") = cstr(rsL.fields("ComboItemID").value & "") then
						%>
						<option selected value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						else
						%>
						<option value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						end if
						rsL.movenext
					loop
					rsL.close
					set rsL = nothing
				%>

				</select></td>
		</tr>
		<tr>
			<td>Check-Out:</td>
			<td>
			<input type="text" name="CheckOutDate" size="20" value="<%=rs.fields("CheckOutDate").value%>" readonly></td>
			<td># Adults:</td>
			<td width="148">
			<select name = "NumberAdults">
			<%
				for i = 0 to 50
					if CSTR(rs.fields("NumberAdults") & "") = CSTR(i) Then
						response.write "<option selected value = '" & i & "'>" & i & "</option>"
					Else	
						response.write "<option value = '" & i & "'>" & i & "</option>"
					End If
				next 
			%></select></td>
		</tr>
		<tr>
			<td>Total Nights:</td>
			<td><select name = 'TotalNights' id = 'TotalNights' onchange = "Change_Out_Date();">
				<%
					for i = 0 to 50
						if rs.Fields("CheckIndate").value & "" <> "" and rs.Fields("CheckInDate").value & "" <> "" then
							if i = DateDiff("d", rs.Fields("CheckInDate").value, rs.Fields("CheckOutDate").value) then
							%>
								<option value = '<%=i%>' selected><%=i%></option>
							<%
							else
							%>
								<option value = '<%=i%>'><%=i%></option>
							<%
							end if
						else
							if i = 0 then
							%>
								<option value = '<%=i%>' selected><%=i%></option>
							<%
							else
							%>
								<option value = '<%=i%>'><%=i%></option>
							<%
							end if
						end if
					next
				%>
			</select>
			
			</td>
			<td># Children:</td>
			<td width="148">
						<select name = "NumberChildren">
			<%
				for i = 0 to 50
					if CSTR(rs.fields("NumberChildren") & "") = CSTR(i) Then
						response.write "<option selected value = '" & i & "'>" & i & "</option>"
					Else	
						response.write "<option value = '" & i & "'>" & i & "</option>"
					End If
				next 
			%></select></td>
		</tr>
		<tr>
			<td>Date Booked:</td>
			<td>
			<input type="text" name="DateBooked" size="20" value="<%=rs.fields("DateBooked").value%>" readonly onclick = "scwShow(this, this);"></td>
			<td>Source</td>
			<td width="148"><select size="1" name="SourceID"><option value = 0></option>
				<%
					set rsL = server.createobject("ADODB.Recordset")
					rsL.open "Select * from t_Comboitems where comboname = 'ReservationSource' order by comboitem",cn,3,3
					do while not rsL.eof
						if cstr(rs.fields("SourceID").value & "") = cstr(rsL.fields("ComboItemID").value & "") then
						%>
						<option selected value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						else
						%>
						<option value = "<%=rsL.fields("ComboItemID").value%>"><%=rsL.fields("ComboItem").value%></option>
						<%
						end if
						rsL.movenext
					loop
					rsL.close
					set rsL = nothing
				%>

				</select></td>
		</tr>
		<tr>
		<% if (Not(IsNull(rs.Fields("PackageIssuedID")))  and rs.Fields("PackageIssuedID") > "0") or (request("PackageIssuedID") <> "" and request("PackageIssuedID") <> "0") then
		%>
		<td>Package:</td><td>
		<%
				set rsL = server.createobject("ADODB.Recordset")				
				if request("packageissuedid") <> "" and request("packageissuedid") <> "0" then		
					pkgIssID = request("Packageissuedid")	
					rsL.OPen "Select a.package from t_Package a inner join (Select packageid from t_PackageIssued where packageissuedid = '" & request("PackageIssuedID") & "') b on a.packageid = b.packageid", cn, 3, 3
				else
					pkgIssID = rs.Fields("PackageIssuedID")
					rsL.OPen "Select a.package from t_Package a inner join (Select packageid from t_PackageIssued where packageissuedid = '" & rs.Fields("PackageIssuedID") & "') b on a.packageid = b.packageid", cn, 3, 3
				end if
				If rsL.EOF and rsL.BOF then
				Else	
					response.write rsL.Fields("Package")
				End If
				response.write "&nbsp<a href = 'editpackageissued.asp?packageissuedid=" & pkgIssID & "'><img src = 'images/edit.gif'></a>"		
				rsL.Close
				set rsL = Nothing				
		%></td>
		<% end if %>
		<%
			if request("groupid") <> "0" and request("groupid") <> "" then
			%>
				<td>GroupID:</td><td><%=request("GroupID")%> <a href = 'editResGroup.asp?groupid=<%=request("groupid")%>'><img src = 'images/edit.gif'></td>
				<input type = "hidden" name = "GroupID" value = '<%=request("GroupID")%>'>
			<%
			else
				set rsL = server.createobject("ADODB.Recordset")
				rsL.open "Select GroupReservationID from t_Res2Group where reservationid = '" & request("reservationid") & "'", cn, 3, 3
				If rsL.EOF and rsL.BOF then
				%>
					<input type = 'hidden' name = 'GroupID' value = "">
				<%
				else
				%>
					<td>GroupID:</td><td><%=rsL.Fields("GroupReservationID")%>  <a href = 'editResGroup.asp?groupid=<%=rsL.Fields("GroupreservationID")%>'><img src = 'images/edit.gif'></a></td>
					<input type = 'hidden' name = 'GroupID' value = '<%rsL.Fields("GroupReservationID")%>'>
				<%
				end if
				rsL.Close
				set rsL = Nothing
			end if
		%>
		</tr>
		<tr>
			<td>Lock Inventory:</td>
			<td><input type = 'checkbox' name = 'LockInventory' <% if rs.Fields("LockInventory") then response.write "CHECKED" end if%>></td>
		<%
			If request("ReservationID") = "" or request("ReservationID") = "0" then
			Else
				set rsL = server.createobject("ADODB.Recordset")
				rsL.open "Select c.*, d.FirstName, d.LastName from (Select a.*, b.ContractNumber, b.ProspectID from (Select ContractID, UsageID from t_Usage where usageid in (Select Distinct(UsageID) from t_RoomAllocationMatrix where reservationid = '" & replace(request("ReservationID"),"'","''") & "')) a inner join t_Contract b on a.contractid = b.contractid) c left outer join t_Prospect d on c.prospectid = d.prospectid", cn, 3, 3
				'rsL.OPen "Select a.ContractID, a.ContractNumber, b.FirstName, b.Lastname from t_Contract a left outer join t_Prospect b on a.prospectid = b.prospectid where a.contractid in (Select ContractID from t_Usage where usageid in (Select Distinct(UsageID) from t_RoomAllocationMatrix where reservationid = '" & replace(request("ReservationID"),"'","''") & "'))", cn, 3, 3
				If rsL.EOF and rsL.BOF then
				%>
				<%
				Else
				%>
				<td>Usage(s):</td>
					<td>
						<%
						Do while not rsL.EOF
						%>	
							<%=rsL.Fields("FirstName") & " " & rsL.Fields("Lastname") & " - " & rsL.Fields("ContractNumber")%><a href="javascript:void(popitup2('contracts/addusage.asp?cid=<%=rsL.fields("ContractID")%>&contractid=<%=rsL.fields("ContractID")%>&usageid=<%=rsL.fields("UsageID").value%>&reservationid=<%=request("ReservationID")%>'));"><img src='./images/edit.gif'></a><br>
						<%	rsL.MoveNext
						Loop
						%>
					</td>
				<%
				End If
				rsL.Close
				set rsL = Nothing
			End If
		%>	
		</tr>
		<% if request("ReservationID") <> "" and request("ReservationID") <> "0" then
				set rsL = server.createobject("ADODB.Recordset")
				rsL.open "Select comboitem from t_ComboItems where comboitemid = '" & rs.Fields("ResLocationID") & "'", cn, 3, 3
				If rs.EOF and rs.BOF then
				Else
					if rsL.Fields("ComboItem") = "FLA-5" or rsL.Fields("ComboItem") = "Daytona" or rsL.Fields("ComboItem") = "Orlando" then
					%>
						<tr><td colspan=2><input type = button value = 'Add to E-Tour Manifest' onclick = 'Add_Etour();'></td></tr>
					<%
					end if
				End If
				rsL.Close
				set rsL = Nothing
			end if
		%>					
	<tr>
		<td>
			<a href="javascript:void(0)" onclick="window.open('ConfirmationLetters/Modules/ResortStay.asp?function=print&reservationid=' + <%=request("reservationid")%>);">Print Letter</a>
		</td>
		<td>
			<a href="javascript:void(0)" onclick="window.open('ConfirmationLetters/Modules/ResortStay.asp?function=email&reservationid=' + <%=request("reservationid")%>);">Email Letter</a>
		</td>
	</tr>
	<tr>
		<td>
			<a href="javascript:void(0)" onclick="window.open('Reports/FrontDesk/modules/regcards.asp?function=GetResCard&reservationid=' + <%=request("reservationid")%>);">Print Registration Card</a>
		</td>
		<td>
			<a href="javascript:void(0)" onclick="popitup('reports/reservations/option.asp?function=options&reservationid=' + <%=request("reservationid")%>);">Print Rental Letter</a>
		</td>		
	</tr>
	</table>
	<input type="hidden" name="ContractID" value="<%=conID%>">
	<input type="hidden" name="GuestID" value="<%=rs.fields("GuestID").value%>">
	<input type="hidden" name="LocationID" value="1">
	<input type="hidden" name="PackageIssuedID" value="<%=pkgID%>">
	<input type="hidden" name="prospectid" value="<%=pID%>">
	<input type="hidden" name="TourID" value="<%=rs.fields("TourID").value%>">
	<input type="hidden" name="VendorID" value="<%=rs.fields("VendorID").value%>">
	<input type="hidden" name="submitting" value="">
	<input type="hidden" name="url" value="">
	<input type="hidden" name="reload" value="false">
	</form>
</div>

</div>

<%
	if request("ReservationID") = "" or request("ReservationID") = "0"  then
		cn.rollbacktrans
	end if

	rs.close	
end if

response.write sAns

cn.close
set rs = nothing
set rs2 = Nothing
set rs3 = Nothing
set rsR = nothing
set rsG = nothing
set cn = nothing
%>