<!--#include file="dbconnections.inc" -->
<%
class GPInventory
	Private cn
	Private rs

	'**** Gets all Parts assinged to a work order including description, status, qty
	'**** ID paramater is the workorderID from CRMSbeing queried. 
	'**** returns the requested values in a string with "|" delimeter
	Public Function List_Parts(ID) 
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		Dim parts
		parts = ""
		cn.Open DBName, DBUser, DBPass
		rs.Open "Select c.Part2RequestID, c.ItemNumber, c.ITEMDESC, c.Qty, d.ComboItem as PartStatus, c.PRID from (Select a.Part2RequestID, a.ItemNumber, b.ITEMDESC, a.Qty, a.StatusID, a.PRID from t_RequestParts a inner join [RS-SQL-02].KCPOA.dbo.IV00101 b on RTrim(a.ItemNumber) = RTrim(b.ITEMNMBR) where a.RequestID = '" & ID & "') c inner join t_ComboItems d on c.statusid = d.comboitemid order by part2requestid asc", cn, 3, 3
		Do while not rs.EOF
			if parts = "" then
				parts = rs.Fields("Part2RequestID") & "|" & rs.Fields("ItemNumber") & "|" & rs.Fields("ITEMDESC") & "|" & rs.Fields("Qty") & "|" & rs.Fields("PartStatus") & "|" & rs.Fields("PRID")
			else
				parts = parts & "|" & rs.Fields("Part2RequestID") & "|" & rs.Fields("ItemNumber") & "|" & rs.Fields("ITEMDESC") & "|" & rs.Fields("Qty") & "|" & rs.Fields("PartStatus") & "|" & rs.Fields("PRID")			
			end if
			rs.MoveNext
		Loop
		rs.Close
		cn.Close
		set rs = Nothing
		set cn = Nothing
		List_Parts = parts	
	End Function

	Public Function Parts_On_Loan(rooms, returnDate)
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		Dim partsLoanedList
		partsLoanedList = ""
		cn.Open DBName, DBUser, DBPass
		
		rs.Open "Select e.*, f.RoomNumber As RoomNumber from (Select c.RequestPartLoanID, c.ItemNumber, c.ITEMDESC, c.Qty, c.RoomID, c.ReservationID, c.DatePickedUp, d.ComboItem as PartStatus from (Select a.RequestPartLoanID, a.RoomId, a.ItemNumber, b.ITEMDESC, a.Qty, a.StatusID, a.ReservationID, a.DatePickedUp from t_RequestPartLoan a inner join [RS-SQL-02].KCPOA.dbo.IV00101 b on RTrim(a.ItemNumber) = RTrim(b.ITEMNMBR) where a.RoomID in (" & rooms & ") and a.DatePickedUp <= '" & returnDate & "' and a.StatusID in (Select comboitemid from t_ComboItems where comboname = 'PartLoanStatus' and comboitem = 'OnLoan')) c inner join t_ComboItems d on c.statusid = d.comboitemid) e left outer join t_Room f on e.RoomID = f.RoomID order by CHARINDEX('-', RoomNumber), RoomNumber asc, DatePickedUp asc", cn, 3, 3
		Do while not rs.EOF
			if partsLoanedList = "" then
				partsLoanedList = rs.Fields("RoomNumber") & "|" & rs.Fields("ITEMDESC") & "|" & rs.Fields("Qty") & "|" & rs.Fields("ReservationID") &  "|" & rs.Fields("DatePickedUp")
			else
				partsLoanedList = partsLoanedList & "|" & rs.Fields("RoomNumber") & "|" & rs.Fields("ITEMDESC") & "|" & rs.Fields("Qty") & "|" & rs.Fields("ReservationID") &  "|" & rs.Fields("DatePickedUp")			
			end if
			rs.MoveNext
		Loop
		rs.Close
		cn.Close
		set rs = Nothing
		set cn = Nothing
		Parts_On_Loan = partsLoanedList	
	End Function
	
	Public Function List_Parts_Loaned(ID)
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		Dim partsLoaned
		partsLoaned = ""
		cn.Open DBName, DBUser, DBPass
		rs.Open "Select e.*, f.RoomNumber As RoomNumber from (Select c.RequestPartLoanID, c.ItemNumber, c.ITEMDESC, c.Qty, c.RoomID, c.ReservationID, c.DatePickedUp, d.ComboItem as PartStatus from (Select a.RequestPartLoanID, a.RoomId, a.ItemNumber, b.ITEMDESC, a.Qty, a.StatusID, a.ReservationID, a.DatePickedUp from t_RequestPartLoan a inner join [RS-SQL-02].KCPOA.dbo.IV00101 b on RTrim(a.ItemNumber) = RTrim(b.ITEMNMBR) where a.RequestID = '" & ID & "') c inner join t_ComboItems d on c.statusid = d.comboitemid) e left outer join t_Room f on e.RoomID = f.RoomID order by requestpartloanid asc", cn, 3, 3
		Do while not rs.EOF
			if partsLoaned = "" then
				partsLoaned = rs.Fields("RequestPartLoanID") & "|" & rs.Fields("ItemNumber") & "|" & rs.Fields("ITEMDESC") & "|" & rs.Fields("Qty") & "|" & rs.Fields("ReservationID") &  "|" & rs.Fields("RoomNumber") & "|" & rs.Fields("DatePickedUp") & "|" & rs.Fields("PartStatus")
			else
				partsLoaned = partsLoaned & "|" & rs.Fields("RequestPartLoanID") & "|" & rs.Fields("ItemNumber") & "|" & rs.Fields("ITEMDESC") & "|" & rs.Fields("Qty") & "|" & rs.Fields("ReservationID") & "|" & rs.Fields("RoomNumber") & "|" & rs.Fields("DatePickedUp") & "|" & rs.Fields("PartStatus")			
			end if
			rs.MoveNext
		Loop
		rs.Close
		cn.Close
		set rs = Nothing
		set cn = Nothing
		List_Parts_Loaned = partsLoaned	
	End Function
	
	Public Function List_Parts_Moved(ID)
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		Dim partsMoved
		partsMoved = ""
		cn.Open DBName, DBUser, DBPass
		rs.Open "Select e.*, f.RoomNumber As RoomNumber from (Select c.RequestPartMoveID, c.ItemNumber, c.ITEMDESC, c.Qty, c.RoomFrom, d.ComboItem as PartStatus from (Select a.RequestPartMoveID, a.RoomFrom, a.ItemNumber, b.ITEMDESC, a.Qty, a.StatusID from t_RequestPartMoves a inner join [RS-SQL-02].KCPOA.dbo.IV00101 b on RTrim(a.ItemNumber) = RTrim(b.ITEMNMBR) where a.RequestID = '" & ID & "') c inner join t_ComboItems d on c.statusid = d.comboitemid) e left outer join t_Room f on e.RoomFrom = f.RoomID order by requestpartmoveid asc", cn, 3, 3
		Do while not rs.EOF
			if partsMoved = "" then
				partsMoved = rs.Fields("RequestPartMoveID") & "|" & rs.Fields("ItemNumber") & "|" & rs.Fields("ITEMDESC") & "|" & rs.Fields("Qty") & "|" & rs.Fields("RoomNumber") & "|" & rs.Fields("PartStatus")
			else
				partsMoved = partsMoved & "|" & rs.Fields("RequestPartMoveID") & "|" & rs.Fields("ItemNumber") & "|" & rs.Fields("ITEMDESC") & "|" & rs.Fields("Qty") & "|" & rs.Fields("RoomNumber") & "|" & rs.Fields("PartStatus")			
			end if
			rs.MoveNext
		Loop
		rs.Close
		cn.Close
		set rs = Nothing
		set cn = Nothing
		List_Parts_Moved = partsMoved	
	End Function		
	
	
	'**** Returns Subfilters based on parent filters passed in
	'**** filterlvl paramater is the level to filter
	'**** filters paramater is a string with all parent filters with a "|" delimeter
	'**** returns a list of filters split by a "|" delimeter
	Public Function Query_Categories (filterLvl, filters)
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		cn.Open DBInventory, DBInvUser, DBInvPass
		filterSplit = Split(filters, "|")
		sFilters = ""
		Select Case filterLvl
			Case 0 'Gets the Item Classes
				rs.Open "Select Distinct ITMCLSCD As Category from IV00101 order by ITMCLSCD ASC", cn, 3, 3
			Case 1 'Gets all UserField 1 with matching ItemClass
				rs.Open "Select Distinct USCATVLS_1 As Category from IV00101 where RTRIM(ITMCLSCD) = '" & Trim(filterSplit(0)) & "' ORDER BY USCATVLS_1 ASC", cn, 3, 3
			Case 2 'Gets all UserField2 with matchin ItemClass and UserField1
				rs.Open "Select Distinct USCATVLS_2 As Category from IV00101 where RTRIM(ITMCLSCD) = '" & Trim(filterSplit(0)) & "' and RTRIM(USCATVLS_1) = '" & Trim(filterSplit(1)) & "' ORDER BY USCATVLS_2 ASC", cn, 3, 3
			Case 3 'Gets all UserField3 with matchin ItemClass and UserField1 and UserField2
				rs.Open "Select Distinct USCATVLS_3 As Category from IV00101 where RTRIM(ITMCLSCD) = '" & Trim(filterSplit(0)) & "' and RTRIM(USCATVLS_1) = '" & Trim(filterSplit(1)) & "' and RTRIM(USCATVLS_2) = '" & Trim(filterSplit(2)) & "' ORDER BY USCATVLS_3 ASC", cn, 3, 3
		End Select
	
		Do while not rs.EOF
			if sFilters = "" then
				sFilters = Trim(rs.Fields("Category")) & ""
			Else
				sFilters = sFilters & "|" & Trim(rs.Fields("Category"))
			End If
			rs.MoveNExt
		Loop
		rs.Close
		cn.Close
		set rs = Nothing
		set cn = Nothing
		Query_Categories = sFilters
	End Function
	
	'**** returns all parts matching the filtered criteria passed in
	'**** filterlvl parameter is the number of filter levels to search
	'**** filter is the filtering criteria with a "|" delimeter
	'**** returns a string with partnumber and part description seperated by a "|" delimeter
	Public Function Search_Parts(filterLvl, filter)
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		cn.Open DBInventory, DBInvUser, DBInvPass
		filterSplit = Split(filter, "|")
		sFilters = ""		
		Select Case filterLvl
			Case 0
				rs.Open "Select ITEMNMBR, ITEMDESC from IV00101 where ItemType = '1' Order by ITEMNMBR", cn, 3, 3
			Case 1 
				rs.Open "Select ITEMNMBR, ITEMDESC from IV00101 where RTRIM(ITMCLSCD) = '" & Trim(filterSplit(0)) & "' and ITEMTYPE = '1' Order by ITEMNMBR", cn, 3, 3
			Case 2
				rs.Open "Select ITEMNMBR, ITEMDESC from IV00101 where RTRIM(ITMCLSCD) = '" & Trim(filterSplit(0)) & "' and RTRIM(USCATVLS_1) = '" & Trim(filterSplit(1)) & "' and ITEMTYPE = '1' Order by ITEMNMBR", cn, 3, 3
			Case 3	
				rs.Open "Select ITEMNMBR, ITEMDESC from IV00101 where RTRIM(ITMCLSCD) = '" & Trim(filterSplit(0)) & "' and RTRIM(USCATVLS_1) = '" & Trim(filterSplit(1)) & "' and RTRIM(USCATVLS_2) = '" & Trim(filterSplit(2)) & "' and ITEMTYPE = '1' Order by ITEMNMBR", cn, 3, 3
			Case 4
				rs.Open "Select ITEMNMBR, ITEMDESC from IV00101 where RTRIM(ITMCLSCD) = '" & Trim(filterSplit(0)) & "' and RTRIM(USCATVLS_1) = '" & Trim(filterSplit(1)) & "' and RTRIM(USCATVLS_2) = '" & Trim(filterSplit(2)) & "' and RTRIM(USCATVLS_3) = '" & Trim(filterSplit(3)) & "' and ITEMTYPE = '1' Order by ITEMNMBR", cn, 3, 3
		End Select
	
		Do while not rs.EOF
			if sFilters = "" then
				sFilters = Trim(rs.Fields("ITEMNMBR")) & "|" & Trim(rs.Fields("ITEMDESC"))
			else
				sFilters = sFilters & "|" & Trim(rs.Fields("ITEMNMBR")) & "|" & Trim(rs.Fields("ITEMDESC")) 
			end if	
			rs.MoveNext
		Loop
		rs.Close
		cn.Close
		set rs = nothing
		set cn = nothing
		Search_Parts = sFilters
	End Function

	Public Function Qty_On_Hand(itemID)
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		cn.Open DBInventory, DBInvUser, DBInvPass
		rs.Open "Select Sum(QTYONHND) As QTYONHND from IV00102 where ITEMNMBR = '" & itemID & "' and RTRIM(LOCNCODE) = 'WAREHOUSE'", cn, 3, 3 'and RTRIM(LOCNCODE) = 'WAREHOUSE'", cn, 3, 3
		If IsNull(rs.Fields("QTYONHND")) then
			qtyOnHand = 0
		Else
			qtyOnHand = rs.Fields("QTYONHND")
		End If
		rs.Close
		cn.Close
		set rs = Nothing
		set cn = Nothing
		Qty_On_Hand = qtyOnHand	
	End Function

	Public Function Min_Order_Amt(itemID)
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		cn.Open DBInventory, DBInvUser, DBInvPass
		rs.Open "Select MNMMORDRQTY from IV00102 where ITEMNMBR = '" & itemID & "' and RTRIM(LOCNCODE) = 'WAREHOUSE'", cn, 3, 3
		If IsNull(rs.Fields("MNMMORDRQTY")) then
			minOrderAmt = 0
		Else
			minOrderAmt = rs.Fields("MNMMORDRQTY")
		End If
		rs.Close
		cn.Close
		set rs = Nothing
		set cn = Nothing
		Min_Order_Amt = minOrderAmt
	End Function
	
	Public Function Item_Order_Amt(itemID, qty)
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		cn.Open DBInventory, DBInvUser, DBInvPass
		rs.Open "Select CURRCOST from IV00101 where ITEMNMBR = '" & itemID & "'", cn, 3, 3
		If IsNull(rs.Fields("CURRCOST")) then
			itemOrderAmt = 0
		Else
			itemOrderAmt = CDBL(rs.Fields("CURRCOST")) '* CDBL(qty)
		End If
		rs.Close
		cn.Close
		set rs = Nothing
		set cn = Nothing
		Item_Order_Amt = itemOrderAmt
	End Function

	'Public Function get_Request_Items(requestID)
	'	set cn = server.createobject("ADODB.Connection")
	'	set rs = server.createobject("ADODB.Recordset")
	'	Dim items
	'	items = ""
	'	cn.Open DBName, DBUser, DBPass
	'	rs.Open "Select a.Item2RequestID, a.ItemNumber, b.ITEMDESC, a.Qty, a.Amount from t_PurchaseRequestItems a left outer join [RS-SQL-02].KCPOA.dbo.IV00101 b on RTrim(a.ItemNumber) = RTrim(b.ITEMNMBR) where a.PurchaseRequestID = '" & requestID & "' order by a.Item2RequestID asc", cn, 3, 3
	'	Do while not rs.EOF
	'		if parts = "" then
	'			parts = rs.Fields("Item2RequestID") & "|" & rs.Fields("ItemNumber") & "x" & "|" & rs.Fields("ITEMDESC") & "" & "|" & rs.Fields("Qty") & "|" & rs.Fields("Amount")
	'		else
	'			parts = parts & "|" & rs.Fields("Item2RequestID") & "|" & rs.Fields("ItemNumber") & "x" & "|" & rs.Fields("ITEMDESC") & "" & "|" & rs.Fields("Qty") & "|" & rs.Fields("Amount")			
	'		end if
	'		rs.MoveNext
	'	Loop
	'	rs.Close
	'	cn.Close
	'	set rs = Nothing
	'	set cn = Nothing
	'	get_Request_Items = parts
	'End Function
	
	Public Function Validate_Part(itemID)
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		Dim valAns
		valAns = ""
		cn.OPen DBInventory, DBInvUser, DBInvPass
		rs.OPen "Select * from IV00101 where RTrim(ITEMNMBR) = '" & trim(itemID) & "' and ITEMTYPE = '1'", cn, 3, 3
		If rs.EOF and rs.BOF then
			valAns = "NO"
		Else
			valAns = "Yes"
		End If
		rs.CLose
		cn.Close
		set rs = Nothing
		set cn = Nothing
		Validate_Part = valAns	
	End Function

	Public Function get_Item_Description(itemID)
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		Dim valAns
		valAns = ""
		cn.OPen DBInventory, DBInvUser, DBInvPass
		rs.OPen "Select ITEMDESC from IV00101 where RTrim(ITEMNMBR) = '" & trim(itemID) & "'", cn, 3, 3
		If (rs.EOF and rs.BOF) or IsNull(rs.Fields("ITEMDESC")) then
			valAns = "N/A"
		Else
			valAns = rs.Fields("ITEMDESC")
		End If
		rs.CLose
		cn.Close
		set rs = Nothing
		set cn = Nothing
		get_Item_Description = valAns			
	End Function
	
	Public function Get_Vendor_List(filter)
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		
		ans = ""
		
		cn.open DBInventory, DBInvUser, DBInvPass
		rs.open "Select top 100 VENDORID, VENDNAME from PM00200 where VENDNAME like '" & filter & "%' order by VENDNAME", cn, 0, 1
		
		if rs.eof and rs.bof then
			ans =  "0|No Vendors"
		else
			i = 0
			do while not rs.eof 
				if i > 0 then ans = ans & "|"
				ans = ans & rs.fields(0).value & "|" & rs.fields(1).value & ""
				i = 1
				rs.movenext
			loop
		end if
		rs.close
		
		Get_Vendor_List = ans
		
		cn.close
		set cn = nothing
		set rs = nothing
	End Function
	
	Public Function Vendor_Lookup (vendorID)
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		cn.OPen DBInventory, DBInvUser, DBInvPass
		rs.Open "Select VENDNAME from PM00200 where VENDORID = '" & vendorID & "'", cn, 3, 3
		If rs.EOF and rs.BOF then
			vendor = "N/A"
		Else
			vendor = rs.Fields("VENDNAME")
		End If
		rs.Close
		cn.Close
		set rs = Nothing
		set cn = Nothing
		Vendor_Lookup = vendor
	End Function
	
	
	Public Function Item_Par_Qty(itemID)
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		cn.Open DBInventory, DBInvUser, DBInvPass
		rs.Open "Select SFTYSTCKQTY from IV00102 where ITEMNMBR = '" & itemID & "' and RTRIM(LOCNCODE) = 'WAREHOUSE'", cn, 3, 3
		If IsNull(rs.Fields("SFTYSTCKQTY")) then
			parAmt = 0
		Else
			parAmt = rs.Fields("SFTYSTCKQTY")
		End If
		rs.Close
		cn.Close
		set rs = Nothing
		set cn = Nothing
		Item_Par_Qty = parAmt	
	End Function
end class
%>