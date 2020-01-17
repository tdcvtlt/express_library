
<html>

<head>
<title>Resrvation Week Bank</title>
<%

dim cn
dim rs
dim dicRoomTypes
dim aRoomTypes
dim night
dim types
dim bDetail

night = clng(request("nights"))
types = clng(request("type"))
bDetail = cbool(request("detail"))
%>
<meta name="Microsoft Theme" content="none, default">
</head>
<body><h2>Reservation Bank<br>
</h2>

<%
set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.Recordset")
set dicRoomTypes = server.createobject("scripting.dictionary")

server.scripttimeout = 10000

redim aRoomTypes(i)


DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

cn.open DBName,DBUser, DBPass


rs.open "Select distinct t.comboitem + ' ' + u.comboitem as RoomType from t_room r inner join t_comboitems t on t.comboitemid = r.typeid inner join t_comboitems u on u.comboitemid = r.SubTypeID /*where r.statusid not in (select comboitemid from t_comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'roomstatus' and comboitem = 'offline')*/", cn, 0, 1
do while not rs.eof
	RoomTypeCount = RoomTypeCount + 1
	if RoomTypeCount > UBound(aRoomTypes) then
		redim preserve aRoomTypes(RoomTypeCount)
	end if
	aRoomTypes(RoomTypeCount) = rs.fields("RoomType").value
	dicRoomTypes(aRoomTypes(RoomTypeCount)) = 0
	dicRoomTypes("T-" & aRoomTypes(RoomTypeCount)) = 0
	dicRoomTypes("E-" & aRoomTypes(RoomTypeCount)) = 0
	rs.movenext
loop
rs.close
	if bDetail then
		response.write "<table style='border-collapse:collapse;' border='1px'>"
			response.write "<tr>"
				response.write "<th>Date Allocated</th><th>RoomNumber</th><th>RoomSubType</th><th>UsageType</th><th>RoomTypeID</th><th>RoomType</th><th>DayOfWeek</th><th>RoomTYPEi</th>"
			response.write "</tr>"

	end if
for i = 1 to RoomTypeCount
	dim sSQL
	sSQL =  "select x.dateallocated,r.roomnumber, t.comboitem + ' ' + s.comboitem as RoomType, u.comboitem as UsageType, u.comboitemid,ut.comboitem as UnitType, " & _
				"case(datepart(dw,x.dateallocated)) " & _
    		        "when 1 then 'Sun' " & _
    		        "when 2 then 'Mon'  " & _
    		        "when 3 then 'Tue'  " & _
    		        "when 4 then 'Wed'  " & _
    		        "when 5 then 'Thu'  " & _
    		        "when 6 then 'Fri'  " & _
    		        "when 7 then 'Sat'  " & _
    	        "end  " & _
 			"as DayOfWeek " & _
			"from t_roomallocationmatrix x  " & _
			"inner join t_room r on r.roomid = x.roomid " & _
			"inner join t_comboitems t on t.comboitemid = r.typeid " & _
			"inner join t_comboitems s on s.comboitemid = r.subtypeid " & _
			"inner join t_comboitems u on u.comboitemid = x.typeid " & _
			"inner join t_unit i on i.unitid = r.unitid " & _
			"inner join t_comboitems ut on ut.comboitemid = i.typeid " & _
			"where x.dateallocated between '" & request("startdate") & "' and '" & request("enddate") & "' and (x.usageid = 0 or  x.usageid is null) and u.comboitemid = '" & types & "' and t.comboitem + ' ' + s.comboitem = '" & aRoomTypes(i) & "' /*and r.statusid not in (select comboitemid from t_comboitems where comboname = 'roomstatus' and comboitem = 'offline')*/ and (x.reservationid <> '-1' or x.reservationid is null)" 
			
	rs.open sSQL & "order by r.roomnumber,x.dateallocated",cn, 0, 1
	
	dim cCount
	dim lastroom
	dim lastdate
	dim sCheckInDay
	dim bWeekStarted
	dim rCount
	dim tCount
	
	cCount = 1
	rCount = 0
	lastroom = "1-99A"
	lastdate = cdate("1/1/1998")
	sCheckInDay = ""
	bWeekStarted = false
	
	dim bTownes,bEstates,bCottages 
	bTownes = false
	bEstates = false
	bCottages = false

	Do while not rs.eof
		if bDetail then
			response.write "<tr>"
			for z = 0 to rs.fields.count - 1
				response.write "<td>" & rs.fields(z).value & "</td>"
			next
			response.write "<td>" & cCount & "</td>"
			response.write "</tr>"
		end if
		if lastroom <> rs.fields("roomnumber").value then
			select case  clng(left(rs.fields("RoomNumber").value, instr(rs.fields("RoomNumber").value,"-") -1))
				case 1,2,3,4,5,6,7,8,9
					bCottages = true
					btownes = false
					bestates = false
				case 10,11,12,13
					bTownes = true
					bestates = false
					bcottages = false
				case else
					bEstates = true
					btownes = false
					bcottages = false
			end select	
			lastroom = rs.fields("roomnumber").value
			'rCount = 1
			cCount = 1
			lastdate = cdate("1/1/1998")
			bWeekStarted = false
		end if
		if night = 7 then
			sCheckInDay = ucase(left(right(aRoomTypes(i),len(aRoomTypes(i))-instr(aRoomTypes(i), " ")),3))
			if ucase(trim(rs.fields("DayOfWeek").value)) = trim(sCheckInDay) then
				bWeekStarted = true
				cCount = 1
			end if
		else
			'cCount = 1
			bWeekStarted = true
		end if
		if bWeekStarted then
			if ucase(trim(rs.fields("DayOfWeek").value)) = trim(sCheckInDay) then
				cCount = 1
			end if
			if cdate(lastdate) + 1 <> cdate(rs.fields("dateallocated").value) then
				cCount = 1
				if night <> cCount then
					if ucase(trim(rs.fields("DayOfWeek").value)) <> trim(sCheckInDay) then
						bWeekStarted = false
					end if
				end if
			else
			end if
			cCount = cCount  + 1
			lastdate = rs.fields("dateallocated").value
			if cCount = night then
				tCount = tCount + 1
				if bTownes then
					dicRoomTypes("T-" & aRoomTypes(i)) = dicRoomTypes("T-" & aRoomTypes(i)) +1
				elseif bEstates then
					dicRoomTypes("E-" & aRoomTypes(i)) = dicRoomTypes("E-" & aRoomTypes(i)) +1
				else
					dicRoomTypes(aRoomTypes(i)) = dicRoomTypes(aRoomTypes(i)) +1
				end if			
					
				if night = 7 then
					if ucase(trim(rs.fields("DayOfWeek").value)) <> trim(sCheckInDay) then
						bWeekStarted = false
					end if
				end if
			end if
		end if	
		rs.movenext
	loop

	rs.close
next
response.write "<table><tr><th>Room Type</th><th>Weeks</th></tr>"
for i = 0 to RoomTypeCount
	if dicRoomTypes(aRoomTypes(i)) <33 and dicRoomTypes(aRoomTypes(i)) <>0 then
		response.write "<tr><td>Cottages -" & aRoomTypes(i) & "</td><td>" & dicRoomTypes(aRoomTypes(i)) & "</td></tr>"
	elseif dicRoomTypes(aRoomTypes(i)) <>0 then
		response.write "<tr><td>Cottages -" & aRoomTypes(i) & "</td><td>" & dicRoomTypes(aRoomTypes(i)) & "</td></tr>"
	end if
	if dicRoomTypes("T-" & aRoomTypes(i)) <> 0 then
		response.write "<tr><td>Townes -" & aRoomTypes(i) & "</td><td>" & dicRoomTypes("T-" & aRoomTypes(i)) & "</td></tr>"
	end if
	if dicRoomTypes("E-" & aRoomTypes(i)) <> 0 then
		response.write "<tr><td>Estates -" & aRoomTypes(i) & "</td><td>" & dicRoomTypes("E-" & aRoomTypes(i)) & "</td></tr>"
	end if
next
response.write "</table>"
response.write request("sdate") & "<br>" & request("edate")

cn.close
set rs = nothing
set cn = nothing

%>



</body></html>
