

<%

dim cn
dim rs

set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")

DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

cn.open DBName, DBUser, DBPass
cn.CommandTimeout = 0

server.scripttimeout = 10000

If request("Function") = "Run_Report" then
	sAns = "<Table>"
	sAns = sAns & "<tr><th>RoomNumber</th><th>DateAllocated</th><th>AllocationType</th><th>UsageID</th><th>UsageType</th><th>ReservationID</th></tr>"
	rs.Open "SELECT * FROM (SELECT a.DateAllocated, b.RoomNumber, a.ReservationID, a.UsageID, c.ComboItem AS AllocatedType, ut.ComboItem AS UsageType, rt.ComboItem AS reservationType FROM t_RoomAllocationMatrix a INNER JOIN t_Room b ON a.RoomID = b.RoomID LEFT OUTER JOIN t_ComboItems c ON a.TypeID = c.ComboItemID LEFT OUTER JOIN t_Usage u ON a.UsageID = u.UsageID LEFT OUTER JOIN t_ComboItems ut ON u.TypeID = ut.ComboItemID LEFT OUTER JOIN t_Reservations r ON a.ReservationID = r.ReservationID LEFT OUTER JOIN t_ComboItems rt ON r.TypeID = rt.ComboItemID) x WHERE DateAllocated between '" & request("sDate") & "' and '" & request("eDate") & "' AND (AllocatedType <> UsageType) AND NOT((AllocatedType = 'Developer') OR (UsageType = 'Marketing')) ORDER BY CHARINDEX('-', RoomNumber), RoomNumber, DateAllocated", cn,0,1
	If rs.EOF and rs.BOF then
		sAns = sAns & "<tr><td colspan = '6'>No Records to Report</td></tr>"
	Else
		Do while not rs.EOF
			sAns = sAns & "<tr>"
			sAns = sANs & "<td>" & rs.Fields("RoomNumber") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("DateAllocated") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("AllocatedType") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("UsageID") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("UsageType") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("ReservationID") & "" & "</td>"
			sAns = sAns & "</tr>"
			rs.MoveNext
		Loop
	End If
	rs.Close
	sAns = sAns & "</table>"
End If
cn.Close
set cn = Nothing
set rs = nothing
response.write sAns
%>