
<html>

<head>
<title>New Page 3</title>
<meta name="Microsoft Theme" content="none, default" />
<meta name="Microsoft Border" content="none, default" />
</head>

<body>&nbsp;
<%
dim cn
dim rs,rs1
dim sdate,edate

sdate = request("startdate")
edate = request("enddate")
set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")
set rs1 = server.createobject("ADODB.Recordset")






DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"


cn.open DBName, DBUser, DBPass

rs.open "select r.roomid, CharIndex('-',r.RoomNumber), r.RoomNumber, x.dateAllocated from t_room r inner join t_roomallocationmatrix x on x.roomid = r.roomid where x.reservationid = -1 and x.dateallocated between '" & sdate & "' and '" & edate & "' and r.RoomNumber not in ('1-109A', '1-109B', '1-111B', '1-200A', '1-200B', '12-117B', '12-119B', '12-100A', '12-102A', '14-103A', '14-103B', '14-103C') order by x.DateAllocated, CharIndex('-',r.RoomNumber), r.RoomNumber",cn,3,3
'rs.open "select roomid, dateallocated from t_roomallocationmatrix where reservationid = -1 and dateallocated between '" & sdate & "' and '" & edate & "' order by dateallocated desc",cn,3,3

response.write "<center><u><h2> Out of Service Rooms</h2></u></center><br><center>From: " & sdate & " To: " & edate & "</center>" 
response.write "<table align = center>"

dim lastdate
lastdate = ""

'if not(CheckSecurity("Reservations","Edit",0,0,Session("Userdbid"))) then
'	response.write "<center><b><u> Access Denied... </u></b></center>"
'else			
	if rs.eof and rs.bof then
		response.write "<center><b>There are no rooms out of service in this date range</b></center>"
	else
		do while not rs.eof
			response.write "<tr>"
			for i = 2 to rs.fields.count -1
				if lastdate <> rs.fields("dateallocated").value then
					lastdate = rs.fields("dateallocated").value
					response.write "<td><b><u>" & rs.fields("dateallocated").value & "</u></b></td>"
				end if
				response.write "</tr>"
			next
			response.write "<tr>"           
			response.write "<td><table><tr><td>" & rs.fields("RoomNumber").value & "</td><td><td><a href=https://crms.kingscreekplantation.com/crmsnet/Marketing/editRoom.aspx?roomid=" & rs.fields("RoomID").value & "><img src = 'https://crms.kingscreekplantation.com/crmsnet/images/edit.gif' alt=''></a></td></tr></table></td>"
			response.write "</tr>"
			rs.movenext
		loop
	end if
	response.write "</table>"
'end if
rs.close
cn.close

set rs = nothing 
set cn = nothing
%>				
</body></html>