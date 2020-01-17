
<%
	Dim cn
	Dim rs
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	
	
DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

	cn.Open DBName, DBUser, DBPass
	
	
	if request("function") = "go_Report" then
		sAns = "<table style='border-collapse:collapse;' border='1px'>"
		sAns = sAns & "<tr><th><u>UsageID</u></th><th><u>ContractNumber</u></th><th><u>Owner</u></th><th><u>InDate</u></th><th><u>Unit Type</u></th><th><u>RoomType</u></th><th><u>Category</u></th></tr>"
		
		rs.Open "SELECT l.FirstName, l.LastName, k.ContractNumber, k.UsageID, k.InDate, k.RoomType, k.unittype, k.Category FROM (SELECT i.*, j.ComboItem AS Category FROM (SELECT g.*, h.ComboItem AS unittype FROM (SELECT e.*, f.ComboItem AS RoomType FROM (SELECT a.*, b.ProspectID AS ProspectID, b.ContractNumber AS ContractNumber FROM (SELECT UsageID, InDate, RoomTypeID, ContractID, CategoryID, UnitTypeID FROM (SELECT *, (SELECT COUNT(DISTINCT (ReservationID)) FROM t_roomAllocationMatrix WHERE usageid = c.usageid and reservationid is not null and reservationid <> '0' and reservationid <> '-1') AS Reservations FROM t_Usage c WHERE (ContractID NOT IN (SELECT contractid FROM t_contract WHERE contractnumber = 'Plan with tan' OR contractnumber = 'KCP Pool' OR contractnumber = 'TransTax' OR contractnumber = 'kcp spare' OR contractnumber = 'kcp developer')) AND (TypeID = (SELECT comboitemid FROM t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID WHERE comboname = 'ReservationType' AND comboitem = 'Rental'))) x WHERE (Reservations = '0')) a INNER JOIN t_Contract b ON a.ContractID = b.ContractID) e LEFT OUTER JOIN t_ComboItems f ON e.RoomTypeID = f.ComboItemID) g LEFT OUTER JOIN t_ComboItems h ON g.UnitTypeID = h.ComboItemID) i LEFT OUTER JOIN t_ComboItems j ON i.CategoryID = j.ComboItemID) k LEFT OUTER JOIN t_Prospect l ON k.ProspectID = l.ProspectID where indate between '" & request("sDate") & "' and '" & request("eDate") & "' order by indate, lastname", cn, 3, 3
		If rs.EOF and rs.BOF then
			sAns = sANs & "<tr><td colspan = '6'>No Usages in this time frame</td></tr>"
		Else
			Do while not rs.EOF
				sAns = sANs & "<tr>"
				sAns = sANs & "<td>" & rs.Fields("UsageID") & "</td>"
				sAns = sAns & "<td>" & rs.Fields("ContractNumber") & "</td>"
				sAns = sAns & "<td>" & rs.Fields("LastName") & ", " & rs.Fields("FirstName") & "</td>"
				sAns = sAns & "<td>" & rs.Fields("InDate") & "</td>"
				sAns = sAns & "<td>" & rs.Fields("UnitType") & "</td>"
				sAns = sAns & "<td>" & rs.Fields("RoomType") & "</td>"
				sAns = sAns & "<td>" & rs.Fields("Category") & "</td>"
				sAns = sAns & "</tr>"
				rs.MoveNext
			Loop
		End If
		rs.Close
		sAns = sAns & "</table>"	
	end if
	cn.Close
	set rs = Nothing
	set cn = Nothing
	response.write sAns
%>	