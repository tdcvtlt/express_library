
<%

dim cn
dim rs
set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")
cn.open "CRMSNet","asp","aspnet"



if request("Function") = "Get_Rooms" then
	sSQL = "select * from ufn_RoomMatrixRoomList(" & request("RoomListID") & ",'" & request("SearchRoom") & "','" & request("TypeFilter") & "')"
	rs.open sSQL, cn, 0, 1
	sAns = ""
	stype = ""
	lastid = 0
	do while not rs.eof
		if sAns <> "" then
			sAns = sAns & "," & rs.fields("RoomNumber").value
			stype = stype & "," & rs.fields("Type").value
		else
			sAns = sAns & rs.fields("RoomNumber").value
			stype = stype  & rs.fields("Type").value
		end if
		lastid = rs.fields("RoomListID").value
		rs.movenext
	loop
	sAns = sAns & "|&&|" & stype & "|&&|" & lastid-15
	rs.close
	response.write replace(sAns,"'","")
	
elseif request("Function") = "Get_Dates" then
	sAns = now
	response.write sAns
	
elseif request("Function") = "Get_Reservations" then
	
	'Set Start Date and sizes
	sDate = request("sDate")
	if sDate = "" then
		sDate = date
	else
		sDate = cDate(sDate)
	end if
	iHeight= request("iHeight")
	if iHeight = "" then iHeight = 0
	iWidth = request("iWidth")
	if iWidth = "" then iWidth = 0
	gridTop = request("GridTop")
	if gridTop = "" then gridTop = 0
	gridLeft = request("GridLeft")
	if gridLeft = "" then gridLeft = 0
	
	'Set rooms list
	aRooms = split(request("Rooms"),",")
	
	'Build SQL
	sSQL  = "Select r.checkindate, r.checkoutdate, p.lastname, p.firstname, r.reservationid, " & _
			"(select distinct roomnumber from t_Room where roomid in (select m.roomid from t_RoomAllocationMatrix m where m.reservationid = r.reservationid)) as RoomNumber " & _
			"from t_Reservations r inner join t_Prospect p on p.prospectid = r.prospectid " & _
			"where (r.checkindate>= '" & sDate & "' or r.checkoutdate <= '" & sDate + 30 & "') " & _
			"and r.reservationid in (select reservationid from t_RoomAllocationMatrix m2 inner join t_Room r3 on r3.roomid = m2.roomid where r3.roomnumber in ('" & Join(aRooms,"','") & "') and dateallocated between '" & sDate & "' and '" & sDate + 30 & "') " 
	
	sSQL = "select r.*, p.*, res.*, stat.comboitem, restype.comboitem as Type from t_Room r inner join " & _
		   "(select distinct m2.roomid,m2.reservationid from t_RoomAllocationMatrix m2 where dateallocated between '" & sDate & "' and '" & sDate + 29 & "') as re on re.roomid = r.roomid " & _
		   "inner join t_Reservations res on res.reservationid = re.reservationid " & _
		   "left outer join t_Prospect p on p.prospectid = res.prospectid " & _
		   "left outer join t_Comboitems stat on stat.comboitemid = res.statusid " & _
		   "left outer join t_Comboitems restype on restype.comboitemid = res.typeid " & _
		   "where roomnumber in ('" & Join(aRooms,"','") & "') and r.roomid in (select roomid from t_RoomAllocationMatrix where dateallocated between '" & sDate & "' and '" & sDate + 29 & "')"
	    
        'response.write sSQL & " " & Request("sDate")
        'return

		rs.open sSQL, cn, 0, 1
	
	resCount = 0
	aResIDs = ""
	res = ""
	do while not rs.eof
		
		'Send back reservation ids
		if aResIDs = "" then
			aResIDs = rs.fields("ReservationID").value
		else
			aResIDs = aResIDs & "," & rs.fields("ReservationID").value
		end if
		
		'Determine Tops and lefts based on Room and start date
	
	
		'Temp resdates for testing
		'res0sDate = "5/18/07"
		'res1sDate = "5/18/07"
		'res0room = "1-100A"
		'res1room = "13-333B"
		'res0nights = 7
		'res1nights = 3
	
		'Loop through rooms to find tops
		for i = 0 to ubound(aRooms)
			if ucase(rs.fields("RoomNumber").value & "") = ucase(aRooms(i)) then
				res0Top = cstr(iHeight * i + gridTop + (iHeight*2)) 'Adding 2 row heights for month and dates
			end if 
		next
	
		'Loop through Dates to find left and if dragable
		
		dragable = true
		reason = ""
		resStartDate = rs.fields("checkInDate").value
		if resStartDate < sDate then 
			resStartDate = sDate
			text = "<"
			dragable = false
		else
			if rs.fields("Type").value & "" = "" then 
				text = "U"
			else
				text = left(ucase(rs.fields("Type").value),1)
			end if
		end if
		res0nights = rs.fields("checkoutdate").value - resStartDate ' rs.fields("checkIndate").value

		if cdate(resStartDate) < cdate(sDate) then
			dragable = false
			reason = "Start Date (" & resStartDate & ") < (" & sDate + i & ")"
		end if
		for i = 0 to 29
			if cDate(resStartDate) < cDate(sDate) + i then
				resStartDate = resStartDate + 1
			else
				if cdate(resStartDate) = cdate(sDate) + i then
					res0left = iWidth * i + gridLeft + iWidth
				end if
			end if
		next
		if cDate(rs.fields("CheckOutDate").value) > cDate(sDate) + 30 then
			dragable = false
			reason = "Start Date (" & cDate(rs.fields("CheckOutDate").value) & ") > (" & sDate + 30 & ")"
		end if
		
		'Set widths (nights * column width(25))
		
		if rs.fields("CheckoutDate").value > sDate + 30 then res0nights = (sDate + 30) - rs.fields("CheckinDate").value
		res0Width = res0Nights * iWidth

	
		'Set values into comma separated strings
		if aResLefts = "" then
			aResLefts = res0left
		else 
			aResLefts = aResLefts & "," & res0left 
		end if
		if aResTops = "" then
			aResTops = res0Top
		else
			aResTops = aResTops & "," & res0Top 
		end if
		if aResWidths = "" then
			aResWidths = res0Width
		else
			aResWidths = aResWidths & "," & res0Width 
		end if
	
		'Create Reservation divs
		'res = res & "<a href='../marketing/editreservation.asp?reservationid=" & rs.fields("ReservationID").value & "'>"
		res = res & "<div id='res" & resCount  & "' style = 'position:absolute;overflow:none;border:thin black solid;"
		if rs.fields("Comboitem").value = "Completed" then
			res = res & "background-color:#000000;color:#FFFFFF;"
		elseif rs.fields("Comboitem").value = "In-House" then
			res = res & "background-color:#9933FF;color:#FFFFFF;"
		else
			res = res & "background-color:#FF0000;"
		end if
		res = res & "height:" & iHeight & "px;width:140px;top:300px;left:0px;' "
		if dragable then
			'res = res & "onmousedown='initializeDrag(this,event)' onmouseup='dropRes(this);'"
			res = res & "onclick='(!drag)?clicktest(this,event,true):null;'" 
		else
			'res = res & ""
			res = res & "onclick='(!drag)?clicktest(this,event,false):null;'" 
		end if
		res = res & " title='" & rs.fields("ReservationID").value & ",Guest: " & trim(rs.fields("lastname").value & "") & ", " & trim(rs.fields("Firstname").value & "") & " : In-Date: " & rs.fields("CheckInDate").value & " : Out-Date: " & rs.fields("CheckOUtDate").value & "'><b>" & text & "</b></div>"
		'res = res & "</a>"
	
		'Send back count of reservations
		resCount = resCount + 1
		
		rs.movenext
	loop
	rs.close
	
	'Add Out Of Service Rooms
	sSQL = "select r.*,m2.dateallocated from t_Room r inner join t_RoomAllocationMatrix m2 on m2.roomid = r.roomid where m2.dateallocated between '" & sDate & "' and '" & sDate + 30 & "' and m2.reservationid = -1 " & _
		   " and roomnumber in ('" & Join(aRooms,"','") & "') order by roomnumber,dateallocated"
	
	rs.open sSQL, cn, 0, 1
	
	oosStart = resCount
	dim dStart, dEnd, lastroom
	lastroom = ""
	ldoos = "1/1/1900"
	do while not rs.eof
	
		if lastroom <> rs.fields("roomnumber").value then
			if lastroom <> ""  then 
				'Determine Tops and lefts based on Room and start date
				'Loop through rooms to find tops
				'Loop through rooms to find tops
				for i = 0 to ubound(aRooms)
					if ucase(lastroom) = ucase(aRooms(i)) then
						res0Top = cstr(iHeight * i + gridTop + (iHeight*2)) 'Adding 2 row heights for month and dates
					end if 
				next
				
				'Make sure there is an end date
				if dEnd = "" and ldoos >= dStart then dEnd = ldoos
				
				'Make sure dEnd >= dStart
				if cdate(dEnd) < cdate(dStart) then dEnd = dStart	
						
				'Loop through Dates to find left and if dragable
				
				dragable = false
				reason = "Out Of Service"
				resStartDate = dStart
				if resStartDate < sDate then 
					resStartDate = sDate
					text = "<"
					dragable = false
				else
					text = "X"
				end if
				res0nights = dEnd - resStartDate ' rs.fields("checkIndate").value
		
				for i = 0 to 29
					if cDate(resStartDate) < cDate(sDate) + i then
						resStartDate = resStartDate + 1
					else
						if cdate(resStartDate) = cdate(sDate) + i then
							res0left = iWidth * i + gridLeft + iWidth
						end if
					end if
				next
				if cDate(dEnd) > cDate(sDate) + 30 then
					dragable = false
					reason = "Start Date (" & cDate(dEnd) & ") > (" & sDate + 30 & ")"
				end if
				
				'Set widths (nights * column width(25))
				
				if dEnd > sDate + 30 then res0nights = (sDate + 30) - dStart

				res0Width = res0Nights * iWidth
		
			
				'Set values into comma separated strings
				if aResLefts = "" then
					aResLefts = res0left
				else 
					aResLefts = aResLefts & "," & res0left 
				end if
				if aResTops = "" then
					aResTops = res0Top
				else
					aResTops = aResTops & "," & res0Top 
				end if
				if aResWidths = "" then
					aResWidths = res0Width
				else
					aResWidths = aResWidths & "," & res0Width 
				end if
			
			
			
				'Create Reservation divs
				'res = res & "<a href='../marketing/editreservation.asp?reservationid=" & rs.fields("ReservationID").value & "'>"
				res = res & "<div id='res" & resCount  & "' style = 'position:absolute;overflow:none;border:thin black solid;"
				res = res & "background-color:yellow;color:black;"
				res = res & "height:" & iHeight & "px;width:140px;top:300px;left:0px;' "
				res = res & " title='OUT OF SERVICE : Start-Date: " & dStart & " : End-Date: " & dEnd & "'><b>" & text & "</b></div>"
				'res = res & "</a>"				
				resCount = resCount + 1
			end if
			lastroom = rs.fields("roomnumber").value 
			dStart = rs.fields("dateallocated").value
		else
			dEnd = rs.fields("dateallocated").value
		end if
		ldoos = rs.fields("dateallocated").value		
		rs.movenext
	loop
	
	if not (rs.eof and rs.bof) then
		'Determine Tops and lefts based on Room and start date
				'Loop through rooms to find tops
				'Loop through rooms to find tops
				for i = 0 to ubound(aRooms)
					if ucase(lastroom) = ucase(aRooms(i)) then
						res0Top = cstr(iHeight * i + gridTop + (iHeight*2)) 'Adding 2 row heights for month and dates
					end if 
				next
			
				'Loop through Dates to find left and if dragable
				
				dragable = false
				reason = "Out Of Service"
				resStartDate = dStart
				if resStartDate < sDate then 
					resStartDate = sDate
					text = "<"
					dragable = false
				else
					text = "X"
				end if
				res0nights = dEnd - resStartDate ' rs.fields("checkIndate").value
		
				for i = 0 to 29
					if cDate(resStartDate) < cDate(sDate) + i then
						resStartDate = resStartDate + 1
					else
						if cdate(resStartDate) = cdate(sDate) + i then
							res0left = iWidth * i + gridLeft + iWidth
						end if
					end if
				next
				if cDate(dEnd) > cDate(sDate) + 30 then
					dragable = false
					reason = "Start Date (" & cDate(dEnd) & ") > (" & sDate + 30 & ")"
				end if
				
				'Set widths (nights * column width(25))
				
				if dEnd > sDate + 30 then res0nights = (sDate + 30) - dStart
				if resStartDate < sDate and dEnd > sDate + 30 then res0nights=30
				res0Width = res0Nights * iWidth
		
			
				'Set values into comma separated strings
				if aResLefts = "" then
					aResLefts = res0left
				else 
					aResLefts = aResLefts & "," & res0left 
				end if
				if aResTops = "" then
					aResTops = res0Top
				else
					aResTops = aResTops & "," & res0Top 
				end if
				if aResWidths = "" then
					aResWidths = res0Width
				else
					aResWidths = aResWidths & "," & res0Width 
				end if
			
				'Create Reservation divs
				'res = res & "<a href='../marketing/editreservation.asp?reservationid=" & rs.fields("ReservationID").value & "'>"
				res = res & "<div id='res" & resCount  & "' style = 'position:absolute;overflow:none;border:thin black solid;"
				res = res & "background-color:yellow;color:black;"
				res = res & "height:" & iHeight & "px;width:140px;top:300px;left:0px;' "
				res = res & " title='OUT OF SERVICE : Start-Date: " & dStart & " : End-Date: " & dEnd & "'><b>" & text & "</b></div>"
				'res = res & "</a>"				
				resCount = resCount + 1
	end if
	rs.close
	
	'Add Holding Reservations
	
	sSQL = "select  r.*, p.*, stat.comboitem, restype.comboitem as Type from t_Reservations r left outer join t_Comboitems rs on rs.comboitemid = r.statusid left outer join t_Prospect p on p.prospectid = r.prospectid left outer join t_RoomAllocationMatrix m2 on m2.reservationid = r.reservationid " & _
			" left outer join t_Comboitems stat on r.statusid = stat.comboitemid left outer join t_Comboitems restype on restype.comboitemid = r.typeid left outer join t_Comboitems loc on loc.comboitemid = r.reslocationid where m2.reservationid is null " & _
		   "and (checkoutdate > '" & sDate & "') and (checkindate < '" & sDate + 29 & "') and checkindate is not null and checkoutdate is not null  and rs.comboitem in ('Booked', 'In-House','Pending','Complete')" & _
		   " and loc.comboitem in ('kcp')" 
	

	rs.open sSQL, cn, 0, 1
	holdertop = 500
	resHoldersStart = resCount
	resHoldersName = ""
	do while not rs.eof
		'Send back reservation ids
		if aResIDs = "" then
			aResIDs = rs.fields("ReservationID").value
		else
			aResIDs = aResIDs & "," & rs.fields("ReservationID").value
		end if
		
		if resHoldersName = "" then
			resHoldersName = rs.fields("Lastname").value & ", " & rs.fields("Firstname").value
		else
			resHoldersName = resHoldersName & "|" & rs.fields("LastName").value & ", " & rs.fields("Firstname").value
		end if
		
		'Determine Tops and lefts based on Room and start date
	
	
		'Temp resdates for testing
		'res0sDate = "5/18/07"
		'res1sDate = "5/18/07"
		'res0room = "1-100A"
		'res1room = "13-333B"
		'res0nights = 7
		'res1nights = 3
	
		'Loop through rooms to find tops
		
		res0Top = cstr(holderTop) 'Adding 2 row heights for month and dates
		'holderTop=holderTop + iHeight
	
		'Loop through Dates to find left and if dragable
		
		dragable = true
		reason = ""
		resStartDate = rs.fields("checkInDate").value
		if resStartDate < sDate then 
			resStartDate = sDate
			text = "<"
			dragable = false
		else
			if rs.fields("Type").value & "" = "" then 
				text = "U"
			else
				text = left(ucase(rs.fields("Type").value),1)
			end if
		end if
		res0nights = rs.fields("checkoutdate").value - resStartDate ' rs.fields("checkIndate").value

		if cdate(resStartDate) < cdate(sDate) then
			dragable = false
			reason = "Start Date (" & resStartDate & ") < (" & sDate + i & ")"
		end if
		for i = 0 to 29
			if cDate(resStartDate) < cDate(sDate) + i then
				resStartDate = resStartDate + 1
			else
				if cdate(resStartDate) = cdate(sDate) + i then
					res0left = iWidth * i + gridLeft + iWidth
				end if
			end if
		next
		if cDate(rs.fields("CheckOutDate").value) > cDate(sDate) + 30 then
			dragable = false
			reason = "Start Date (" & cDate(rs.fields("CheckOutDate").value) & ") > (" & sDate + 30 & ")"
		end if
		
		'Set widths (nights * column width(25))
		
		if rs.fields("CheckoutDate").value > sDate + 30 then res0nights = (sDate + 30) - rs.fields("CheckinDate").value
		res0Width = res0Nights * iWidth

	
		'Set values into comma separated strings
		if aResLefts = "" then
			aResLefts = res0left
		else 
			aResLefts = aResLefts & "," & res0left 
		end if
		if aResTops = "" then
			aResTops = res0Top
		else
			aResTops = aResTops & "," & res0Top 
		end if
		if aResWidths = "" then
			aResWidths = res0Width
		else
			aResWidths = aResWidths & "," & res0Width 
		end if
	
		'Create Reservation divs
		'res = res & "<a href='../marketing/editreservation.asp?reservationid=" & rs.fields("ReservationID").value & "'>"


		res = res & "<div id='res" & resCount  & "' style = 'position:absolute;overflow:none;border:thin black solid; " 
		if rs.fields("Comboitem").value = "Completed" then
			res = res & "background-color:#000000;color:#FFFFFF;"
		else
			res = res & "background-color:#FF0000;"
		end if
		res = res & "height:" & iHeight & "px;width:140px;top:300px;left:0px;' "
		if dragable then
			'res = res & "onmousedown='initializeDrag(this,event)' onmouseup='dropRes(this);'"
			res = res & "onclick='(!drag)?clicktest(this,event,true):null;'" 
		else
			'res = res & ""
			res = res & "onclick='(!drag)?clicktest(this,event,false):null;'" 
		end if
		res = res & " title='" & rs.fields("ReservationID").value & ",Guest: " & trim(rs.fields("lastname").value & "") & ", " & trim(rs.fields("Firstname").value & "") & " : In-Date: " & rs.fields("CheckInDate").value & " : Out-Date: " & rs.fields("CheckOUtDate").value & "'><b>" & text & "</b></div>"
		'res = res & "</a>"
	
		'Send back count of reservations
		resCount = resCount + 1
		
		rs.movenext
	loop
	rs.close
		
	'Send data
	response.write resCount & "|&&|" & aResIDs & "|&&|" & aResTops & "|&&|" & aResLefts & "|&&|" & res & "|&&|" & aResWidths & "|&&|" & resHoldersStart & "|&&|" & resHoldersName & "|&&|" & oosStart
	
elseif request("Function") = "Update_Reservation" then
	r = split(request("Rooms"),",")
	t = split(request("RoomTypes"),",")
	dim oldUsageAssignment
	dim newUsageAssignment
	oldUsageAssignment = -1
	newUsageAssignment = -1
	if request("row2") < 16 then
		sSQL = "select * from t_RoomAllocationMatrix where reservationid = '" & request("ID") & "' and roomid in (select roomid from t_Room where roomnumber = '" & r(request("Row2")) & "')"
		'response.write sSQL
		rs.open sSQL, cn, 0,1
		if rs.eof and rs.bof then
			oldUsageAssignment = 0
		else
			oldUsageAssignment = rs.fields("typeid").value
		end if
		rs.close
	end if	
	if request("row1") < 16 then
		sSQL = "SELECT * from t_RoomAllocationMatrix where DateAllocated >= (select checkindate from t_Reservations where reservationid = '" & request("ID") & "') and DateAllocated < (select checkoutdate from t_Reservations where reservationid = '" & request("ID") & "') and roomid in (select roomid from t_Room where roomnumber = '" & r(request("Row1")) & "')"
		rs.open sSQL, cn, 0, 1
		if rs.eof and rs.bof then
			newUsageAssignment = 0
		else
			newUsageAssignment = rs.fields("TypeID").value
		end if
		rs.close
	end if
	if newUsageAssignment & "" = "" then newUsageAssignment = 0
	if oldUsageAssignment & "" = "" then oldUsageAssignment = 0
	if request("row2") < 16 then
	 	sSQL = "UPDATE t_RoomAllocationMatrix set reservationid = 0"
	 	if newUsageAssignment > 0 then
	 		sSQL = sSQL & ", TypeID = " & newUsageAssignment & " " 
	 	end if
	 	sSQL = SSQL & " where reservationid = '" & request("ID") & "' " & _
	 		"and roomid in (select roomid from t_Room where roomnumber = '" & r(request("Row2")) & "')"
		cn.execute sSQL
	end if
	if request("row1") < 16 then
		sSQL = "UPDATE t_RoomAllocationMatrix set reservationid = '" & request("ID") & "'" 
		if oldUsageAssignment > -1 then
			sSQL = sSQL & ", TypeID = " & oldUsageAssignment 
		end if
		sSQL = sSQL & " where DateAllocated >= (select checkindate from t_Reservations where reservationid = '" & request("ID") & "') and DateAllocated < (select checkoutdate from t_Reservations where reservationid = '" & request("ID") & "') and roomid in (select roomid from t_Room where roomnumber = '" & r(request("Row1")) & "')"
		cn.execute sSQL
	end if
	if request("OverLap") = "true" then
		if request("row2") < 16 then 
			sSQL = "UPDATE t_RoomAllocationMatrix set reservationid = '" & request("OverLapID") & "', typeid = " & newUsageAssignment & _
				 " where DateAllocated >= (select checkindate from t_Reservations where reservationid = '" & request("OverLapID") & "') " & _
				 " and DateAllocated < (select checkoutdate from t_Reservations where reservationid = '" & request("OverLapID") & "') " & _
				 "and roomid in (select roomid from t_Room where roomnumber = '" & r(request("Row2")) & "')"
			cn.execute sSQL
		end if
	end if

	response.write "OK" ' -- Row1 = " & request("Row1") & " -- OldRoom = " & r(request("Row2")) & "-" & t(request("Row2")) & " -- NewRoom = " & r(request("Row1")) & "-" & t(request("Row1")) 
	
elseif request("Function") = "Get_Usage" then
	sDate = request("sDate")
	aRooms = split(request("Rooms"), ",")
	
	sAns = ""
	
	for i = 0 to ubound(aRooms)
		sSQL = "select * from t_RoomAllocationMatrix r inner join t_Room on t_Room.roomid = r.roomid left outer join t_Usage u on u.usageid = r.usageid left outer join t_Comboitems t on t.comboitemid = r.typeid where dateallocated between '" & cstr(sDate) & "' and '" & cstr(cDate(sDate) + 29) & "' and roomnumber='" & aRooms(i) & "' order by dateallocated"
		
		rs.open sSQL, cn, 0, 1
		x = 0
		sDate = cDate(sDate)	
		do while not rs.eof
			bDone = false
			do while not bDone
				if rs.fields("DateAllocated").value = sDate + x then
					select case trim(rs.fields("Comboitem").value  & "")
						case "Marketing"
							sAns = sAns & "#ffbbff" & "-|-"
						case "Use","Owner"
							sAns = sAns & "#bbffbb" & "-|-"
						case "Banking", "Exchange"
							sAns = sAns & "#FFCC66" & "-|-"
						case "Rental"
							sAns = sAns & "#33ccff" & "-|-"
						case "Developer"
							sAns = sAns & "#bbffff" & "-|-"
						case "NALJR"
							sAns = sAns & "#ffffbb" & "-|-"
						case "PlanWithTan"
							sAns = sAns & "#bbff00" & "-|-"
						case "Points"
							sAns = sAns & "#00ffbb" & "-|-"
						case "SRental"
							sAns = sAns & "#aaaaaa" & "-|-"
						case "TrialOwner"
							sAns = sAns & "#66ff66" & "-|-"
						case "Vendor"
							sAns = sAns & "#ff66ff" & "-|-"
						case "PointsExchange"
							sAns = sAns & "#CC9900" & "-|-"
                        case "GBT"
                            sAns = sAns & "#AFC2F2" & "-|-"
						case else
							sAns = sAns & "#FFFFFF"  & "-|-"
					end select
					bDone = true
				else
					sAns = sAns & "#FFFFFF" & "-|-"
					if x = 29 then exit do
				end if
				x = x + 1
			loop
			rs.movenext
		loop
		rs.close
		do while x < 30 
			sAns = sAns & "#FFFFFF" & "-|-"
			x = x + 1
		loop
	next
	if len(sAns) > 3 then sAns = left(sAns, len(sAns)-3)
	response.write sAns

elseif request("Function") = "allocate" then
	if cdate(request("sdate")) <> cdate(request("edate")) then	
		rs.Open "Select Distinct(b.ComboItem) as ResType from t_RoomAllocationMatrix a left outer join t_ComboItems b on a.TypeID = b.ComboItemID where a.RoomID in (" & request("Rooms") & ") and dateallocated between '" & request("sdate") & "' and '" & cdate(request("edate")) - 1 & "'", cn, 3, 3		
	else
		rs.Open "Select Distinct(b.ComboItem) as ResType from t_RoomAllocationMatrix a left outer join t_ComboItems b on a.TypeID = b.ComboItemID where a.RoomID in (" & request("Rooms") & ") and dateallocated between '" & request("sdate") & "' and '" & cdate(request("edate")) & "'", cn, 3, 3	
	end if
	proceed = true
	Do while not rs.EOF
		if rs.Fields("ResType") & "" = "" then
			If Not(CheckSecurity("Rooms","AllocateNew")) then
				proceed = false
				sAns = "Access Denied."
				exit Do
			End if
		else
			If Not(CheckSecurity("Rooms","AllocateFrom" & rs.Fields("ResType"))) Then
				proceed = false
				sAns = "Access Denied."
				exit Do
			End If
		end if
		rs.MoveNext
	Loop
	rs.Close
	
	if proceed = true then
		rs.Open "Select ComboItem as ResType from t_comboItems where comboitemid = '" & request("Type") & "'", cn, 3, 3
		restype = rs.Fields("ResType")
		rs.Close
		
		If Not(CheckSecurity("Rooms","AllocateTo" & restype)) Then
			proceed = false
			sAns = "Access Denied."
		End If
	end if
	
	if proceed = true then
		call allocate
	else
		response.write sAns
	end if
end if

cn.close
set rs = nothing
set cn = nothing

function allocate()
	dim r
	if cdate(request("sdate")) <> cdate(request("edate")) then 
		cn.execute "update t_RoomAllocationMatrix set typeid='" & request("Type") & "' where roomid in (" & request("Rooms") & ") and dateallocated between '" & request("sdate") & "' and '" & cdate(request("edate")) - 1 & "'", r
	else 
		cn.execute "update t_RoomAllocationMatrix set typeid='" & request("Type") & "' where roomid in (" & request("Rooms") & ") and dateallocated between '" & request("sdate") & "' and '" & cdate(request("edate")) & "'", r
	end if
	response.write "Allocated (" & r & " record(s) updated)"
end function

function BuildLastRoom()
				
end function
%>
