<!--#include file="dbconnections.inc" -->
<!--#include file="security.asp" -->
<%

Dim cn
Dim rs
set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")
set rs2 = server.createobject("ADODB.Recordset")
set rs3 = server.createobject("ADODB.Recordset")
cn.Open DBName, DBUser, DBPass
cn.CommandTimeout = 0

if request("function") = "Val_Owner_Usage" then
	'***** Need to Get ResTypeID, InDate, OutDate of reservation *****'
	rs.Open "Select a.CheckInDate, a.CheckOutDate, a.TypeID, a.ProspectID, b.ComboItem as resType from t_Reservations a left outer join t_ComboItems b on a.typeid = b.comboitemid where a.reservationid = '" & request("resid") & "'", cn, 3, 3
	if Date > rs.Fields("CheckIndate") and Date < rs.Fields("CheckOutDate") then
		inDate = Date
	else
		inDate = rs.Fields("CheckInDate")
	end if
	outDate = rs.Fields("CheckOutDate")
	resType = rs.Fields("ResType")
	prosID = rs.Fields("ProspectID")
	resTypeID = rs.Fields("TypeID")
	rs.Close
	proceed = true
	If resType = "Marketing" or resType = "Rental" or resType = "Developer" or resType = "Vendor" Then
		rs.Open "Select * from t_AccountItems where finTransID > 0 and amount > 20 and reservationid = '" & request("resid") & "'", cn, 3, 3
		If rs.EOF and rs.BOF then
			sANs = "N$You must have an Invoice Before Inserting a Room."
			proceed = False
		Else
			proceed = true
		End If
		rs.Close
	End If
	
	If proceed = true Then
		if request("sort") = "Name" then
			if resType = "Exchange" or resType = "Points" or resType = "NALJR" then
				if instr(request("Filter"), ",") > 0 then
					rs.Open "Select UsageID from t_Usage where ContractID in (Select contractid from t_Contract where prospectid in (Select prospectid from t_Prospect where lastname like '" & replace(trim(left(request("Filter"),instr(request("Filter"),",")-1)),"'","''") & "%' and firstname like '" & replace(trim(right(request("Filter"),len(request("Filter"))-instr(request("Filter"),","))),"'","''") & "%')) and usageyear = '" & request("usageyear") & "' and indate = '" & inDate & "' and outdate = '" & outdate & "' and typeid = '" & resTypeID & "'", cn, 3, 3
				else
					rs.Open "Select UsageID from t_Usage where ContractID in (Select contractid from t_Contract where prospectid in (Select prospectid from t_Prospect where lastname like '" & replace(trim(request("Filter")),"'","''") & "%')) and usageyear = '" & request("usageyear") & "' and indate = '" & inDate & "' and outdate = '" & outdate & "' and typeid = '" & resTypeID & "'", cn, 3, 3
				end if
			else
				if instr(request("Filter"), ",") > 0 then
					rs.Open "Select UsageID from t_Usage where ContractID in (Select contractid from t_Contract where prospectid in (Select prospectid from t_Prospect where lastname like '" & replace(trim(left(request("Filter"),instr(request("Filter"),",")-1)),"'","''") & "%' and firstname like '" & replace(trim(right(request("Filter"),len(request("Filter"))-instr(request("Filter"),","))),"'","''") & "%')) and usageyear = '" & request("usageyear") & "' and typeid = '" & resTypeID & "'", cn, 3, 3
				else
					rs.Open "Select UsageID from t_Usage where ContractID in (Select contractid from t_Contract where prospectid in (Select prospectid from t_Prospect where lastname like '" & replace(trim(request("Filter")),"'","''") & "%')) and usageyear = '" & request("usageyear") & "' and typeid = '" & resTypeID & "'", cn, 3, 3
				end if
			end if
		elseif request("sort") = "IIMember" then
			rs.Open "Select UsageID from t_Usage where ContractID in (Select contractid from t_Contract where prospectid in (Select recordid from t_UserFieldsValue where userfieldid = (select userfieldid from t_Userfields where fieldname = 'II Membership Number' and tablename = (select tableid from t_UserFieldTables where TableName = 'Prospect')) and UserFieldValue = '" & request("Filter") & "')) and usageyear = '" & request("usageyear") & "' and typeid = '" & resTypeID & "'", cn, 3, 3
		elseif request("sort") = "RCIMember" then
			rs.Open "Select UsageID from t_Usage where ContractID in (Select contractid from t_Contract where prospectid in (Select recordid from t_UserFieldsValue where userfieldid = (select userfieldid from t_Userfields where fieldname = 'RCI Membership Number' and tablename = (select tableid from t_UserFieldTables where TableName = 'Prospect')) and UserFieldValue = '" & request("Filter") & "')) and usageyear = '" & request("usageyear") & "' and typeid = '" & resTypeID & "'", cn, 3, 3
		elseif request("sort") = "ICEMember" then
			rs.Open "Select UsageID from t_Usage where ContractID in (Select contractid from t_Contract where prospectid in (Select recordid from t_UserFieldsValue where userfieldid = (select userfieldid from t_Userfields where fieldname = 'ICE Membership Number' and tablename = (select tableid from t_UserFieldTables where TableName = 'Prospect')) and UserFieldValue = '" & request("Filter") & "')) and usageyear = '" & request("usageyear") & "' and typeid = '" & resTypeID & "'", cn, 3, 3
		elseif request("sort") = "RCIPoints" then
			rs.Open "Select UsageID from t_Usage where ContractID in (Select contractid from t_Contract where prospectid in (Select recordid from t_UserFieldsValue where userfieldid = (select userfieldid from t_Userfields where fieldname = 'RCI Points Membership Number' and tablename = (select tableid from t_UserFieldTables where TableName = 'Prospect')) and UserFieldValue = '" & request("Filter") & "')) and usageyear = '" & request("usageyear") & "' and typeid = '" & resTypeID & "'", cn, 3, 3		
		elseif request("sort") = "Year" then
			rs.Open "Select UsageID from t_Usage where ContractID in (Select contractid from t_Contract where prospectid = '" & prosid & "') and usageyear = '" & request("usageyear") & "' and typeid = '" & resTypeID & "'", cn, 3, 3
		elseif request("sort") = "contract" then
			rs.Open "Select UsageID from t_Usage where ContractID in (Select contractid from t_Contract where contractnumber = '" & request("filter") & "') and usageyear = '" & request("usageyear") & "' and typeid = '" & resTypeID & "'", cn, 3, 3
		end if
	
		if rs.EOF and rs.BOF then
			sAns = "N$There is no usage matching the requirements you submitted"
		else
			sAns = "<table>"
			Do while not rs.EOF
				rs2.Open "Select Distinct(RoomID) from t_RoomAllocationMatrix where usageid = '" & rs.Fields("UsageID") & "'", cn, 3, 3
				If rs2.EOF and rs2.BOF then
				Else
					Do while not rs2.EOF
						rs3.Open "Select * from ufn_UsageRooms (" & rs2.Fields("RoomID") & ", '" & inDate & "', '" & (outDate - 1) & "', " & rs.Fields("UsageID") & ")", cn, 3, 3
						If (rs3.EOF and rs3.BOF) or IsNull(rs3.Fields("RoomID")) then
						Else
							if rs3.Fields("Available") = "available" then
								rooms = "found"
								if usage = "" then
									usage = rs.Fields("UsageID")
								else
									usage = usage & "|" & rs.Fields("UsageID")
								end if							
								sAns = sAns & "<tr><td><input type = 'checkbox' id = '" & rs.Fields("UsageID") & "' name = '" & rs.Fields("UsageID") & "'> " & rs3.Fields("RoomNumber") & "</td><td>" & rs3.Fields("RoomType") & "</td><td>" & rs3.Fields("RoomSubType") & "</td></tr>"
							end if
						End If
						rs3.Close
						rs2.MoveNext
					Loop
				End If
				rs2.Close
				rs.MoveNext
			Loop
			If rooms = "" then
				sAns = "N$No Rooms"
			Else
				sAns = sAns & "<tr><td><input type = 'button' name = 'go' value = 'Submit' onClick = 'sub_Usage_Room();'></td></tr>"
				sAns = sAns & "</table>"
				sAns = usage & "$" & sAns
			End If
		End If
		rs.Close
	End If

elseif request("function") = "Get_Avail_Rooms" then
	proceed = true
	rs.Open "Select a.CheckInDate, a.CheckOutDate, DateDiff(" & Chr(34) & "d"& Chr(34) & ", CheckInDate, CheckOutDate) As Nights, a.TypeID, b.ComboItem as resType from t_Reservations a left outer join t_ComboItems b on a.typeid = b.comboitemid where a.reservationid = '" & request("resid") & "'", cn, 3, 3
	nights = rs.Fields("Nights")
	transAmt = nights * 2
	if request("extend") = "yes" then
		if CDate(request("newOutDate")) <= rs.Fields("CheckoutDate") then
			sAns = "N$The New Check Out Date Must Be Greater than the Current Check Out Date."
			proceed = false
		end if
		inDate = rs.Fields("CheckOutDate")
		outDate = CDate(request("newOutDate"))
		nights = Datediff("d",rs.Fields("CheckInDate"),outDate) 
		transAmt = nights * 2
	else
		if Date > rs.Fields("CheckIndate") and Date < rs.Fields("CheckOutDate") then
			inDate = Date
		else
			inDate = rs.Fields("CheckInDate")
		end if
		outDate = rs.Fields("CheckOutDate")
	end if
	rs.Close

	rs.Open "Select comboitemid, comboitem from t_ComboItems where comboitemid = '" & request("inventory") & "'", cn, 3, 3
	resType = rs.Fields("ComboItem")
	resTypeID = rs.Fields("ComboItemID")
	rs.Close
	
	typeCheck = "Add" & resType & "Room"
	'****Security Check********'
	If not(CheckSecurity("Reservations", typeCheck)) then
		sAns = "N$You Do Not Have Permission to Add " & resType & " Inventory to a Reservation"
		proceed = false
	End If
	
	'******* Make Sure marketing and rental and developer have enough to cover transient tax
	if request("extend") = "yes" and (resType = "Marketing" or resType = "Rental" or resType = "Developer" or resType = "Vendor") and proceed = true then
		rs.Open "Select Sum(Amount) as Amt from t_AccountItems where reservationid = '" & request("ResID") & "' and fintransid in (Select financialtranscodeid from t_FinancialtransactionCodes where roomcharge = '1' and transtypeid in (Select comboitemid from t_ComboItems where comboname = 'TransCodeType' and comboitem = 'ReservationTrans'))", cn, 3, 3
		If rs.EOF and rs.BOF or IsNull(rs.Fields("Amt")) then
			if resType = "Developer" then
				If not(CheckSecurity("Reservations","DeveloperExtendWaiver")) then
					sAns = "N$You must insert a Room Charge"
					proceed = false
				End if
			else
				sAns = "N$You must insert a Room Charge"
				proceed = false
			end if
		ElseIf (rs.Fields("Amt")) < transAmt then
			if resType = "Developer" then
				If not(CheckSecurity("Reservations","DeveloperExtendWaiver")) then
					sAns = "N$Insufficient Room Charge Amount"
					proceed = false
				End If
			Else
				sAns = "N$Insufficient Room Charge Amount"
				proceed = false
			End If				
		End If
		rs.Close
	end if
	
	If request("searchspares") = "yes" and Not(CheckSecurity("Reservations","AddSpareRoom")) then
		sAns = "N$You Do Not Have Access to Search Spares"
	Else
		If proceed = true then
			rs.Open "Select * from ufn_RoomsAvailable ('" & request("BD") & "', " & request("unittypeid") & ", '" & inDate & "', '" & (outdate - 1) & "', " & resTypeID & ", '" & request("unittype") & "', '', 0) where available = 'available'", cn, 3, 3
			If rs.EOF and rs.BOF then
				sAns = "N$No Rooms Available"
			ElseIf rs.Fields("RoomID") = 0 then	
				sAns = "N$No Rooms Available"
			Else
				sAns = "<table>"
				Do while not rs.EOF
					if request("BD") = "3" or (request("BD") = "4" and request("unittype") = "Townes") then
						if rooms = "" then
							rooms = rs.Fields("RoomID") & "-" & rs.Fields("RoomID2")
						else
							rooms = rooms & "|" & rs.Fields("RoomID") & "-" & rs.Fields("RoomID2")
						end if
						if IsNull(rs.Fields("RoomSubType1")) Then
							sAns = sAns & "<tr><td><input type = 'checkbox' id = '" & rs.Fields("RoomID") & "-" & rs.Fields("RoomID2") & "' name = '" & rs.Fields("roomID") & "'></td><td>" & rs.Fields("RoomNumber") & "<br>" & rs.Fields("RoomNumber2") & "</td><td>" & rs.Fields("RoomType1") & "<br>" & rs.Fields("RoomType2") & "</td><td>" & rs.Fields("RoomSubType1a") & "<br>" & rs.Fields("RoomSubType2") & "</td>"						
						else
							sAns = sAns & "<tr><td><input type = 'checkbox' id = '" & rs.Fields("RoomID") & "-" & rs.Fields("RoomID2") & "' name = '" & rs.Fields("roomID") & "'></td><td>" & rs.Fields("RoomNumber") & "<br>" & rs.Fields("RoomNumber2") & "</td><td>" & rs.Fields("RoomType1") & "<br>" & rs.Fields("RoomType2") & "</td><td>" & rs.Fields("RoomSubType1") & "<br>" & rs.Fields("RoomSubType2") & "</td>"
						end if
						if resType = "Rental" then
							sAns = sAns & "<td>"
							rs2.Open "Select Distinct(ComboItem) As Category from (Select b.CategoryID from t_RoomAllocationMatrix a inner join t_Usage b on a.usageid = b.usageid where a.roomid = '" & rs.Fields("RoomID") & "' and DateAllocated between '" & inDate & "' and '" & outDate - 1 & "') c left outer join t_ComboItems d on c.CategoryID = d.ComboItemID", cn, 3, 3
							Do while not rs2.EOF
								sAns = sAns & "Category " & rs2.Fields("Category") & "<br>"
								rs2.MoveNExt
							Loop
							rs2.Close
							rs2.Open "Select Distinct(ComboItem) As Category from (Select b.CategoryID from t_RoomAllocationMatrix a inner join t_Usage b on a.usageid = b.usageid where a.roomid = '" & rs.Fields("RoomID2") & "' and DateAllocated between '" & inDate & "' and '" & outDate - 1 & "') c left outer join t_ComboItems d on c.CategoryID = d.ComboItemID", cn, 3, 3
							Do while not rs2.EOF
								sAns = sAns & "Category " & rs2.Fields("Category") & "<br>"
								rs2.MoveNExt
							Loop
							rs2.Close
							sAns = sAns & "</td></tr>"
							'sAns = sAns & "<td><a href = " & Chr(34) & "javascript:popitmodal2('usageinfo.asp?rooms=" & rs.Fields("RoomID") & "," & rs.Fields("RoomID2") & "&startDate=" & inDate & "&endDate=" & outDate - 1 & "');" & Chr(34) & ">Usage Info</a></td></tr>"
						else
							sAns = sAns & "</tr>"
						end if
					elseif request("BD") = "4" and request("unittype") <> "Townes" then	
						if rooms = "" then
							rooms = rs.Fields("RoomID") & "-" & rs.Fields("RoomID2") & "-" & rs.Fields("RoomID3")
						else
							rooms = rooms & "|" & rs.Fields("RoomID") & "-" & rs.Fields("RoomID2") & "-" & rs.Fields("RoomID3")
						end if
						sAns = sAns & "<tr><td><input type = 'checkbox' id = '" & rs.Fields("RoomID") & "-" & rs.Fields("RoomID2") & "-" & rs.Fields("RoomID3") & "' name = '" & rs.Fields("roomID") & "'></td><td>" & rs.Fields("RoomNumber") & "<br>" & rs.Fields("RoomNumber2") & "<br>" & rs.Fields("RoomNumber3") & "</td><td>" & rs.Fields("RoomType1") & "<br>" & rs.Fields("RoomType2") & "<br>" & rs.Fields("RoomType3") & "</td><td>" & rs.Fields("RoomSubType1") & "<br>" & rs.Fields("RoomSubType2") & "<br>" & rs.Fields("RoomSubType3") & "</td>"
						if resType = "Rental" then
							sAns = sAns & "<td>"
							rs2.Open "Select Distinct(ComboItem) As Category from (Select b.CategoryID from t_RoomAllocationMatrix a inner join t_Usage b on a.usageid = b.usageid where a.roomid = '" & rs.Fields("RoomID") & "' and DateAllocated between '" & inDate & "' and '" & outDate - 1 & "') c left outer join t_ComboItems d on c.CategoryID = d.ComboItemID", cn, 3, 3
							Do while not rs2.EOF
								sAns = sAns & "Category " & rs2.Fields("Category") & "<br>"
								rs2.MoveNExt
							Loop
							rs2.Close
							rs2.Open "Select Distinct(ComboItem) As Category from (Select b.CategoryID from t_RoomAllocationMatrix a inner join t_Usage b on a.usageid = b.usageid where a.roomid = '" & rs.Fields("RoomID2") & "' and DateAllocated between '" & inDate & "' and '" & outDate - 1 & "') c left outer join t_ComboItems d on c.CategoryID = d.ComboItemID", cn, 3, 3
							Do while not rs2.EOF
								sAns = sAns & "Category " & rs2.Fields("Category") & "<br>"
								rs2.MoveNExt
							Loop
							rs2.Close
							rs2.Open "Select Distinct(ComboItem) As Category from (Select b.CategoryID from t_RoomAllocationMatrix a inner join t_Usage b on a.usageid = b.usageid where a.roomid = '" & rs.Fields("RoomID3") & "' and DateAllocated between '" & inDate & "' and '" & outDate - 1 & "') c left outer join t_ComboItems d on c.CategoryID = d.ComboItemID", cn, 3, 3
							Do while not rs2.EOF
								sAns = sAns & "Category " & rs2.Fields("Category") & "<br>"
								rs2.MoveNExt
							Loop
							rs2.Close
							sAns = sAns & "</td></tr>"
'							sAns = sAns & "<td><a href = " & Chr(34) & "javascript:popitmodal2('usageinfo.asp?rooms=" & rs.Fields("RoomID") & "," & rs.Fields("RoomID2") & "," & rs.Fields("RoomID3") & "&startDate=" & inDate & "&endDate=" & outDate - 1 & "');" & Chr(34) & ">Usage Info</a></td></tr>"
						else
							sAns = sAns & "</tr>"
						end if
					else			
						if rooms = "" then
							rooms = rs.Fields("RoomID")
						else
							rooms = rooms & "|" &  rs.Fields("RoomID") 
						end if
						If IsNull(rs.Fields("roomsubtype1")) then
							sAns = sAns & "<tr><td><input type = 'checkbox' id = '" & rs.Fields("RoomID") & "' name = '" & rs.Fields("roomID") & "'>" & rs.Fields("RoomNumber") & "</td><td>" & rs.Fields("RoomType1") & "</td><td>" & rs.Fields("RoomSubType1a") & "</td>"						
						Else
							sAns = sAns & "<tr><td><input type = 'checkbox' id = '" & rs.Fields("RoomID") & "' name = '" & rs.Fields("roomID") & "'>" & rs.Fields("RoomNumber") & "</td><td>" & rs.Fields("RoomType1") & "</td><td>" & rs.Fields("RoomSubType1") & "</td>"
						End If
						if resType = "Rental" then
							sAns = sAns & "<td>"
							rs2.Open "Select Distinct(ComboItem) As Category from (Select b.CategoryID from t_RoomAllocationMatrix a inner join t_Usage b on a.usageid = b.usageid where a.roomid = '" & rs.Fields("RoomID") & "' and DateAllocated between '" & inDate & "' and '" & outDate - 1 & "') c left outer join t_ComboItems d on c.CategoryID = d.ComboItemID", cn, 3, 3
							Do while not rs2.EOF
								sAns = sAns & "Category " & rs2.Fields("Category") & "<br>"
								rs2.MoveNExt
							Loop
							rs2.Close
							sAns = sAns & "</td></tr>"
							'sAns = sAns & "<td><a href = " & Chr(34) & "javascript:popitmodal2('usageinfo.asp?rooms=" & rs.Fields("RoomID") & "&startDate=" & inDate & "&endDate=" & outDate - 1 & "');" & Chr(34) & ">Usage Info</a></td></tr>"
						else
							sAns = sAns & "</tr>"
						end if
					end if
					rs.MoveNext
				Loop
			End If
			rs.Close
			
			if request("searchspares") = "yes" then
				rs.Open "SELECT g.RoomID, g.RoomNumber, g.RoomType, h.ComboItem AS UnitType FROM (SELECT e.*, f.UnitTypeID AS UnitTypeID FROM (SELECT c.*, d .ComboItem AS RoomType FROM (SELECT a.*, b.RoomNumber AS RoomNumber, b.TypeID AS typeid, b.UnitID AS UnitID FROM (SELECT DISTINCT RoomID, COUNT(AllocationID) AS Nights FROM t_RoomAllocationMatrix WHERE (TypeID = (SELECT comboitemid FROM t_ComboItems WHERE comboname = 'ReservationType' AND comboitem = 'Spare')) AND (DateAllocated BETWEEN '" & inDate & "' AND '" & CDate(outDate - 1) & "') AND (reservationid IS NULL OR reservationid = '0') GROUP BY RoomID) a LEFT OUTER JOIN  t_Room b ON a.RoomID = b.RoomID WHERE (a.Nights = DATEDIFF(d, '" & inDate & "', '" & outDate & "'))) c LEFT OUTER JOIN t_ComboItems d ON c.typeid = d .ComboItemID) e LEFT OUTER JOIN t_Unit f ON e.unitid = f.unitid) g LEFT OUTER JOIN t_ComboItems h ON g.UnitTypeID = h.ComboItemID ORDER BY CHARINDEX('-', g.RoomNumber), g.RoomNumber", cn, 3, 3	
				If rs.EOF and rs.BOF then
				Else
					if sAns = "N$No Rooms Available" then
						sAns = "<table>"
					else
						sAns = sAns & "<tr><td><br></td></tr>"
					end if								
					sANs = sAns & "<tr><td colspan = '4'><u><b>Spares</b></u></td></tr>"	
					Do while not rs.EOF
						if rooms = "" then
							rooms = rs.Fields("RoomID")
						else
							rooms = rooms & "|" &  rs.Fields("RoomID") 
						end if
						sAns = sAns & "<tr><td><input type = 'checkbox' id = '" & rs.Fields("RoomID") & "' name = '" & rs.Fields("roomID") & "'>" & rs.Fields("RoomNumber") & "</td><td>" & rs.Fields("RoomType") & "</td><td>" & rs.Fields("UnitType") & "</td></tr>"
						rs.MoveNext
					Loop
					sAns = sAns & "<tr><td colspan = '2'><input type = 'button' name 'go' value = 'Submit' onClick = 'sub_Room();'></td></tr>"
					sAns = sAns & "</table>"
					sAns = rooms & "$" & sAns
				End If
				rs.Close
			else
				if sAns <> "N$No Rooms Available" then
					sAns = sAns & "<tr><td colspan = '2'><input type = 'button' name 'go' value = 'Submit' onClick = 'sub_Room();'></td></tr>"
					sAns = sAns & "</table>"
					sAns = rooms & "$" & sAns
				end if
			end if	
		End If
	End If	
	
end if


cn.Close
set rs = Nothing
set rs2 = Nothing
set rs3 = Nothing
set cn = Nothing

response.write sAns
%>