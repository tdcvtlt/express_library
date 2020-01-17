<!--#Include file = "dbconnections.inc"-->
<!--#include file="PurchaseRequest.asp" -->
<!--#include file="PartAssignment.asp" -->
<!--#include file="GPInventory.asp" -->
<%

Dim cn, rs
set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")
set rs2 = server.createobject("ADODB.Recordset")
cn.Open DBName, DBUser, DBPass
server.scripttimeout = 10000

if request("Function") = "Filter" then
	sAns = "<table>"
	sAns = sAns & "<tr><th><u>RequestID</u></th><th><u>Amount</u></th><th><u>Status</u></th></tr>"
	
	if request("filtertext") = "" then
		if request("sort") = "ALL" then
			rs.Open "Select a.*, (Select Sum(Amount) from t_PurchaseRequestItems where purchaseRequestID = a.PurchaseRequestID) As Amount from t_PurchaseRequest a order by PurchaseRequestID asc", cn, 3, 3
		else
			rs.OPen "Select a.*, (Select Sum(Amount) from t_PurchaseRequestItems where purchaseRequestID = a.PurchaseRequestID) As Amount from t_PurchaseRequest a where Approved = '" & request("sort") & "' order by PurchaseRequestID asc", cn,3,3
		end if
	else
		if request("Sort") = "ALL" then
			rs.Open "Select a.*, (Select Sum(Amount) from t_PurchaseRequestItems where purchaseRequestID = a.PurchaseRequestID) As Amount from t_PurchaseRequest a where PurchaseRequestID like '" & request("filtertext") & "%' order by PurchaseRequestID asc", cn, 3, 3
		else
			rs.open "Select a.*, (Select Sum(Amount) from t_PurchaseRequestItems where purchaseRequestID = a.PurchaseRequestID) As Amount from t_PurchaseRequest a where PurchaseRequestID LIKE '" & request("filtertext") & "%' and Approved = '" & request("Sort") &  "' order by PurchaseRequestID asc", cn, 3, 3
		end if
	end if
	
	If rs.EOF and rs.BOF then
		sAns = sANs & "<tr><td colspan = '4'>No Purchase Requests Found.</td></tr>"
	Else
		Do While not rs.EOF
			sAns = sAns & "<tr>"
			sAns = sAns & "<td>" & rs.FIelds("PurchaseRequestID") & "</td>"
			If IsNull(rs.Fields("Amount")) then
				sAns = sAns & "<td align = right>$0.00</td>"
			Else
				sAns = sAns & "<td align = right>" & FormatCurrency(rs.Fields("Amount"), 2) & "</td>"
			End If
			sAns = sAns & "<td>"
			If rs.Fields("Approved") = 0 then
				sAns = sAns & "Pending"
			ElseIf rs.Fields("Approved") = 1 then
				sAns = sAns & "Approved"
			Else
				sAns = sAns & "Denied"
			End If
			sANs = sAns & "</td>"
			sAns = sAns & "<td><a href = 'editPurchaseRequest.asp?requestid=" & rs.fields("PurchaseRequestID").value & "'><img src = 'images/edit.gif'></a></td>"	
			sAns = sAns & "</tr>"
			rs.MoveNExt
		Loop
	End If
	rs.Close
	sAns = sAns & "</table>"	
elseif request("Function") = "get_Purchase_Request_Items" then
	response.write "<form name = " & Chr(34) & "requestItems" & Chr(34) & ">"
'	sAns = "<form name = " & Chr(34) & "requestItems" & Chr(34) & ">"
	response.write "<table>"
'	sAns = sAns & "<table>"
	response.write "<tr><th><u>ItemNumber</u></th><th><u>Description</u></th><th><u>Qty</u></th><th><u>Amount</u></th><th><u>Location</u></th><th><u>Purpose</u></th></tr>"
'	sANs = sAns & "<tr><th><u>ItemNumber</u></th><th><u>Description</u></th><th><u>Qty</u></th><th><u>Amount</u></th><th><u>Location</u></th><th><u>Purpose</u></th></tr>"
	If request("RequestID") = "" or request("RequestID") = "0" then
	Else
		dim pRequest
		dim pRequestItems
		set pRequest = new PurchaseRequest
		rs.Open "Select * from t_PurchaseRequestItems where PurchaseRequestID = '" & request("RequestID") & "' order by Item2RequestID asc", cn, 3, 3
		Do while not rs.EOF
			response.write "<tr>"
'			sAns = sAns & "<tr>"
			response.write "<td>" & rs.Fields("ItemNumber") & "</td>"
'			sAns = sAns & "<td>" & rs.Fields("ItemNumber") & "</td>"
			response.write "<td>" & pRequest.get_Item_Description(rs.Fields("ItemNumber")) & "</td>"
'			sAns = sAns & "<td>" & pRequest.get_Item_Description(rs.Fields("ItemNumber")) & "</td>"
			response.write "<td><select name = 'qty" & rs.Fields("Item2RequestID") & "' id = 'qty" & rs.Fields("Item2RequestID") & "'>"
'			sAns = sAns & "<td><select name = 'qty" & rs.Fields("Item2RequestID") & "' id = 'qty" & rs.Fields("Item2RequestID") & "'>"
			for j = 1 to 999
				if CSTR(j) = CSTR(rs.Fields("Qty")) then
					response.write "<option value = " & j & " selected>" & j & "</option>"
'					sAns = sAns & "<option value = " & j & " selected>" & j & "</option>"
				else					
					response.write "<option value = " & j & ">" & j & "</option>"
'					sAns = sAns & "<option value = " & j & ">" & j & "</option>"					
				end if
			next 
			response.write "</select></td>"
'			sAns = sAns & "</select></td>"				
			response.write "<td align = 'right'><input type = 'text' size = '8' name = 'Amount" & rs.Fields("Item2RequestID") & "' id = 'Amount" & rs.Fields("Item2RequestID") & "' value = '" & rs.Fields("Amount") & "' align = 'right'></td>"
'			sAns = sAns & "<td align = 'right'><input type = 'text' size = '8' name = 'Amount" & rs.Fields("Item2RequestID") & "' id = 'Amount" & rs.Fields("Item2RequestID") & "' value = '" & rs.Fields("Amount") & "' align = 'right'></td>"
			response.write "<td><input type = 'text' size = '15' id = 'Location" & rs.Fields("Item2RequestID") & "' value = '" & rs.Fields("Location") & "" & "'></td>"
'			sAns = sAns & "<td><input type = 'text' size = '15' id = 'Location" & rs.Fields("Item2RequestID") & "' value = '" & rs.Fields("Location") & "" & "'></td>"
			response.write "<td><input type = 'text' size = '8' id = 'Purpose" & rs.Fields("Item2RequestID") & "' value = '" & rs.Fields("Purpose") & "" & "'></td>"
'			sAns = sAns & "<td><input type = 'text' size = '8' id = 'Purpose" & rs.Fields("Item2RequestID") & "' value = '" & rs.Fields("Purpose") & "" & "'></td>"
			rs2.Open "Select Approved from t_PurchaseRequest where purchaserequestid = '" & request("RequestID") & "'", cn, 3, 3
			If rs2.Fields("Approved") = "0" then
				response.write "<td><input type = 'button' name = 'go' value = 'Save' onClick = " & Chr(34) & "save_pItem('" & rs.Fields("Item2RequestID") & "','" & request("RequestID") & "');" & Chr(34) & ";>  <input type = 'button' name = 'rem' value = 'Remove' onClick = " & Chr(34) & "remove_pItem('" & rs.Fields("Item2RequestID") & "', '" & request("RequestID") & "');" & Chr(34) & ";</td>"				
'				sAns = sAns & "<td><input type = 'button' name = 'go' value = 'Save' onClick = " & Chr(34) & "save_pItem('" & rs.Fields("Item2RequestID") & "','" & request("RequestID") & "');" & Chr(34) & ";>  <input type = 'button' name = 'rem' value = 'Remove' onClick = " & Chr(34) & "remove_pItem('" & rs.Fields("Item2RequestID") & "', '" & request("RequestID") & "');" & Chr(34) & ";</td>"				
			end if
			rs2.Close		
			response.write "</tr>"	
'			sAns = sANs & "</tr>"
			rs.MoveNext
		Loop
		rs.Close
	End If
	response.write "</table>"
'	sAns = sAns & "</table>"
	response.write "<input type = 'hidden' name = 'test' value = 'testing'>"
'	sAns = sAns & "<input type = 'hidden' name = 'test' value = 'testing'>"
	response.write "</form>"
'	sAns = sAns & "</form>"
elseif request("Function") = "get_Approval_Buttons" then
	if request("RequestID") = ""  or request("RequestID") = "0" then
		sAns = "<input type = 'button' value = 'Save Changes' onClick = " & Chr(34) & "Save('" & request("requestID") & "');" & Chr(34) & ">"
	else
		rs.OPen "Select Approved from t_PurchaseRequest where PurchaseRequestID = '" & request("RequestID") & "'", cn, 3, 3
		If rs.Fields("Approved") = "0" then
			sAns = "<input type = 'button' value = 'Save' onClick = " & Chr(34) & "Save('" & request("requestID") & "');" & Chr(34) & ">"
			sAns = sAns & "           <input type = 'button' value = 'Print Request' onClick = " & Chr(34) & "Print_PR('" & request("requestID") & "');" & Chr(34) & ">"
			sAns = sAns & "           <input type = 'button' value = 'Approve' onClick = " & Chr(34) & "Approve('" & request("requestID") & "');" & Chr(34) & ">"
			sAns = sAns & "           <input type = 'button' value = 'Deny' onClick = " & Chr(34) & "Deny('" & request("requestID") & "');" & Chr(34) & ">"
		End If
		rs.Close
	end if
elseif request("Function") = "Save_PurchaseRequest_Item" then
	if not(CheckSecurity("PurchaseRequests", "EditPR")) then
		 sAns = "You Do Not Have Persmission to Edit Purchase Requests."
	end if
	
	if sAns = "" then
		rs.Open "Select Sum(b.Qty) As QtyOrdered, (Select Sum(Qty) from t_RequestParts where PRID = '" & request("RequestID") & "' and itemnumber = (Select ItemNumber from t_PurchaseRequestItems where Item2RequestID = '" & request("ItemID") & "')) As QtyRequested from t_PurchaseRequest a inner join t_PurchaseRequestItems b on a.PurchaseRequestID = b.PurchaseRequestID where a.PurchaseRequestID = '" & request("RequestID") & "' and b.Item2RequestID <> '" & request("ItemID") & "' and b.ItemNumber = (Select ItemNumber from t_PurchaseRequestItems where Item2RequestID = '" & request("ItemID") & "')", cn, 3, 3
		If IsNull(rs.Fields("QtyRequested")) then
			qtyRequested = 0
		Else
			qtyRequested = rs.Fields("QtyRequested")
		End If
		If IsNull(rs.Fields("QtyOrdered")) then
			qtyOrdered = 0
		Else
			qtyOrdered = rs.Fields("QtyOrdered")
		End If
		rs.Close	
	
		If qtyRequested = 0 or (qtyOrdered - qtyRequested) > 0 then
			rs.Open "Select Qty, Amount, Location, Purpose from t_PurchaseRequestItems where Item2RequestID = '" & request("itemID") & "'", cn, 3, 3
			rs.Fields("Qty") = request("Qty") 
			rs.Fields("Amount") = request("Amount")
			rs.Fields("Location") = request("Location")
			rs.Fields("Purpose") = request("Purpose")
			rs.Updatebatch
			sAns = "OK"
		Else
			qtyNeeded = qtyRequested - qtyOrdered
			if (CDBL(request("Qty")) < CDBL(qtyNeeded)) then
				sAns = "The Quantity Ordered Must Be At Least " & qtyNeeded & " To Fulfill Maintenance Request Needs."
			else
				rs.Open "Select Qty, Amount, Location, Purpose from t_PurchaseRequestItems where Item2RequestID = '" & request("itemID") & "'", cn, 3, 3
				rs.Fields("Qty") = request("Qty") 
				rs.Fields("Amount") = request("Amount")
				rs.Fields("Location") = request("Location")
				rs.Fields("Purpose") = request("Purpose")
				rs.Updatebatch
				sAns = "OK"
			end if		
		End If
	End If
elseif request("Function") = "Remove_PurchaseRequest_Item" then
	if not(CheckSecurity("PurchaseRequests", "EditPR")) then
		 sAns = "You Do Not Have Persmission to Edit Purchase Requests."
	end if
	
	if sAns = "" then
		'**** Check to see if PR has been Approved ****'
		rs.Open "Select Approved from t_PurchaseRequest where PurchaseRequestID = '" & request("RequestID") & "'", cn, 3, 3
		If rs.Fields("Approved") = "1" Then
			sAns = "Items Can Not Be Removed From Approved Purchase Requests."
		End If
		rs.Close
	end if
		
	if sAns = "" then
		rs.Open "Select Sum(b.Qty) As QtyOrdered, (Select Sum(Qty) from t_RequestParts where PRID = '" & request("RequestID") & "' and itemnumber = (Select ItemNumber from t_PurchaseRequestItems where Item2RequestID = '" & request("ItemID") & "')) As QtyRequested from t_PurchaseRequest a inner join t_PurchaseRequestItems b on a.PurchaseRequestID = b.PurchaseRequestID where a.PurchaseRequestID = '" & request("RequestID") & "' and b.Item2RequestID <> '" & request("ItemID") & "' and b.ItemNumber = (Select ItemNumber from t_PurchaseRequestItems where Item2RequestID = '" & request("ItemID") & "')", cn, 3, 3
		If IsNull(rs.Fields("QtyRequested")) then
			qtyRequested = 0
		Else
			qtyRequested = rs.Fields("QtyRequested")
		End If
		If IsNull(rs.Fields("QtyOrdered")) then
			qtyOrdered = 0
		Else
			qtyOrdered = rs.Fields("QtyOrdered")
		End If
		rs.Close	
		
		if qtyRequested = 0 or (qtyOrdered - qtyRequested) >= 0 then
			rs.Open "Select * from t_PurchaseRequestItems where Item2RequestID = '" & request("ItemID") & "'", cn, 3, 3
			rs.Delete
			rs.UpdateBatch
			rs.Close		
			sAns = "OK" 
		else
			qtyNeeded = qtyRequested - qtyOrdered
			sAns = "Item Is Tied To Pending Maintenance Requests And Can Not Be Removed."
		end if	
	end if
elseif request("Function") = "Approve" then
	if not(CheckSecurity("PurchaseRequests", "ApprovePR")) then
		 sAns = "You Do Not Have Approve/Deny Purchase Requests."
	end if
	
	if sAns = "" then
		'**** Check to see if a Vendor has been assigned ****'
		rs.Open "Select VendorID from t_PurchaseRequest where PurchaseRequestID = '" & request("RequestID") & "'", cn, 3, 3
		If rs.Fields("VendorID") = "0" or rs.Fields("VendorID") = "-1" then
			sAns = "Please Assign a Vendor."
		End If
		rs.Close
	end if	
	
	if sAns = "" then
		'**** Make Sure the Purchase Request has Items *****'
		rs.Open "Select Count(*) As ItemCount from t_PurchaseRequestItems where PurchaseRequestID = '" & request("RequestID") & "'", cn, 3, 3
		If rs.Fields("ItemCount") = 0 or IsNull(rs.Fields("ItemCount")) then
			sAns = "Please Add Items to Purchase Request Before Approving."
		End If
		rs.Close
	end if	
	'**** Do we need to check Price Contraints for Approving?*****'
	
	
	'**** End Price Constraint Check *****'

	'**** Approve ****'
	if sAns = "" then
		rs.Open "Select * from t_PurchaseRequest where PurchaseRequestID = '" & request("RequestID") & "'", cn, 3, 3
		rs.Fields("Approved") = "1"
		rs.UpdateBatch
		rs.Close
		sAns = "OK"
	end if	
elseif request("Function") = "Deny" then
	if not(CheckSecurity("PurchaseRequests", "ApprovePR")) then
		 sAns = "You Do Not Have Permission to Approve/Deny Purchase Requests."
	end if
	if sAns = "" then
		rs.Open "Select * from t_PurchaseRequest where PurchaseRequestID = '" & request("RequestID") & "'", cn, 3, 3
		rs.Fields("Approved") = "-1"
		rs.UpdateBatch
		rs.Close
		
		'****** Update Part Requests tied to this PR as cancelled ******'
		rs.Open "Select comboitemid from t_ComboItems where comboname = 'PartStatus' and comboitem = 'Cancelled'", cn,3,3
		cancelledid = rs.Fields("CombOItemID")
		rs.Close
		rs.Open "Select * from t_RequestParts where PRID = '" & request("RequestID") & "'", cn, 3, 3
		Do while not rs.EOF
			rs.Fields("PRID") = 0
			rs.Fields("StatusID") = cancelledID
			rs.UpdateBatch
			rs.Movenext
		Loop
		rs.Close
		
		sAns = "OK"
	end if
elseif request("Function") = "Save" then
	if not(CheckSecurity("PurchaseRequests", "EditPR")) then
		 sAns = "You Do Not Have Persmission to Edit Purchase Requests."
	end if

	if sANs = "" then 
		rs.OPen "Select * from t_PurchaseRequest where PurchaseRequestID = '" & request("RequestID") & "'", cn, 3, 3
		If rs.EOF and rs.BOF then
			rs.AddNew
			rs.Fields("DateCreated") = Now
			rs.Fields("CreatedById") = Session("UserID")
		End If
		If request("VendorID") = "" then
			rs.Fields("VendorID") = "0"
		Else
			rs.Fields("VendorID") = request("VendorID")
		End If
		rs.Fields("VendorDescription") = request("VendorDesc")
		rs.UpdateBatch
		rs.MoveFirst
		requestID = rs.Fields("PurchaseRequestID")
		rs.Close	
	
		sAns = "OK|" & requestID
	end if
elseif request("function") = "Filter_Parts" then
	'******* This is used for finding the sub filters. A PartAssignment object is created
	'******* and is passed the filter level indicating number of levels to filter and the 
	'******* values for each filter level. The returned value is sent back to javascript for parsing
	dim filter, category
	filter = request("filter")
	dim filterparts, result
	set filterparts = new PartAssignment
	result = filterparts.FilterParts (request("level"), filter)
	sAns = result
elseif request("function") = "Part_Search" then
	'***** This is used to perform the actual search for parts once all filters have been selected
	'***** Creates a PartAssignment object passing it the filter string seperated by "|" charachter
	'***** Once parts are returned the string is split by "|" character and values displayed in a table
	Dim partsearch '***** Part Assignment Object
	Dim parts '***** string returned from PartAssignment object seperated by "|" character
	
	'***** request("filter") contains the other filter material passed from jscript select boxes
	filter = request("filter")
	set partsearch = new PartAssignment
	parts = partsearch.Part_Search(filter)

	sAns = "<table>"
	sAns = sAns & "<tr><th align = 'left'>Item Number</th><th align = 'left'>Item Description</th></tr>"
	if parts = "" then
		sAns = sAns & "<tr><td>No Parts Matching Your Search Criteria</td></tr>"
	else
		'****** Split the array and go by 2 value 1 is the ItemNumber, value 2 is the Item Description
		sParts = Split(parts, "|")
		for i = 0 to UBound(sParts) step 2
			sAns = sAns & "<tr>"
			sAns = sAns & "<td>" & sParts(i) & "</td>"
			sAns = sAns & "<td>" & sParts(i + 1) & "</td>"
			sAns = sAns & "<td><input type = 'button' name = 'go' value = 'Select' onClick = " & Chr(34) & "Assign_Part('" & sParts(i) & "', '" & request("RequestID") & "');" & Chr(34) & "></td>"
			sAns = sANs & "</tr>"
		next
	end if
	sANs = sAns & "</table>"	
elseif request("function") = "Assign_Part" then

	
	if not(CheckSecurity("PurchaseRequests", "EditPR")) then
		 sAns = "You Do Not Have Persmission to Edit Purchase Requests."
	end if

	if sAns = "" then		
		Dim pReq
		Dim pReqString
		set pReq = new PurchaseRequest
		pReqString = pReq.Min_Order_Amt(request("ItemID"))
		If(CDBL(pReqString) > CDBL(request("Qty"))) Then
			sAns = "Minimum Order Amount for This Item is " & pReqString
		End If
	end if
		
	If sAns = "" then
		rs.Open "Select * from t_PurchaseRequestItems where 1=2", cn, 3, 3
		rs.AddNew
		rs.Fields("PurchaseRequestID") = request("RequestID")
		rs.Fields("ItemNumber") = request("ItemID")
		rs.Fields("Qty") = request("Qty")
		rs.Fields("Amount") = pReq.Item_Order_Amt(request("ItemID"), request("Qty"))
		rs.UpdateBatch
		rs.Close
		sAns = "OK"
		rs.Open "Select isManual from t_PurchaseRequest where purchaserequestid = '" & request("RequestID") & "'", cn, 3, 3
		rs.Fields("isManual") = 0'"1"
		rs.UpdateBatch
		rs.Close
	End If		
	
elseif request("Function") = "Add_Manual_Part" then
	'**** Check to see if Item is in GP, if so do minorder checks and then add Item
	'**** Otherwise create Purchase requestitem with manual entry - boooooo **'
	Dim purReq
	Dim purReqString
	set purReq = new PurchaseRequest
	
	purReqString = purReq.Validate_Part(request("ItemID"))
	if purReqString = "yes" then '*** Item is in GP, check min order amt ***'
		purReqString = purReq.Min_Order_Amt(request("ItemID"))
		If (CDBL(purReqString) > CDBL(request(request("Qty")))) then
			sAns = "Minimum Order Amount for This Item is " & pReqString
		End If
	end if
	
	if sAns = "" then
		rs.OPen "Select * from t_PurchaseRequestItems where 1=2", cn, 3, 3
		rs.AddNew
		rs.FIelds("PurchaseRequestID") = request("RequestID")
		rs.Fields("ItemNumber") = request("ItemID")
		rs.Fields("Qty") = request("Qty")
		rs.Fields("Amount") = request("Amount")
		rs.UPdateBatch
		rs.Close
		sAns = "OK"
		if purReqString = "yes" then
			rs.Open "Select isManual from t_PurchaseRequest where purchaserequestid = '" & request("RequestID") & "'", cn, 3, 3
			rs.Fields("isManual") = 0 '"1"
			rs.UpdateBatch
			rs.Close
		else
			rs.Open "Select isManual from t_PurchaseRequest where purchaserequestid = '" & request("RequestID") & "'", cn, 3, 3
			rs.Fields("isManual") = 0 '"2"
			rs.UpdateBatch
			rs.Close
		end if
	end if
elseif request("Function") = "get_Purchase_Request" then
	If request("RequestID") = "" or request("RequestID") = "0" then
		rs.Open "Select * from t_PurchaseRequest where 1=2", cn, 3, 3
		rs.AddNew	
	Else
		rs.Open "Select * from t_PurchaseRequest where PurchaseRequestID = '" & request("RequestID") & "'", cn, 3, 3
	End If
%>
	<form name = 'requestInfo'>
	<table width="761">
		<tr>
			<td width="131">Purchase Request ID:</td>
			<td width="155"><input type = 'text' name = 'RequestID' value = '<%=rs.Fields("PurchaseRequestID")%>' readonly></td>
			<td width="96">Date Created:</td>
			<td width="361"><input type = 'text' name = 'dateCreated' value = '<%=rs.Fields("DateCreated")%>' readonly></td>
		</tr>
		<tr>
			<td width="131">Purchase Order ID:</td>
			<td width="155"><input type = 'text' name = 'POID' value = '<%=rs.Fields("POID")%>' readonly></td>
		</tr>
		<tr>
			<td width="131">Vendor:</td>
			<td colspan = '3'><input type = 'text' name = 'Vendor' <%if rs.Fields("VendorID") = "0" or rs.Fields("VendorID") = "-1" then %> value = '' <% else %> value = '<%=rs.Fields("VendorID")%>' <%end if%> readonly> <%if request("RequestID") = "" or request("RequestID") = "" or rs.Fields("Approved") = "0" then%> <input type = 'button' name = 'go' value = 'Add Vendor' onclick = "popitup2('../Maintenance/vendorfilter.asp');"> <% end if%><input type = 'hidden' name = 'VendorID' <%if request("RequestID") = "" or request("RequestID") = "0" then %> value = '0' <%else%> value = '<%=rs.Fields("VendorID")%>' <%end if%>></td>
		</tr>	
		<tr id = 'vendorDesc' <%if rs.Fields("VendorID") <> "-1" then%> style = "display:none" <%end if%>>
			<td width="131">Vendor Description:</td>
			<td width="155"><input type = 'text' name = 'VendorDesc' value = '<%=rs.Fields("VendorDescription")%>' readonly></td>
		</tr>
		<tr>
			<td width="131">Status:</td>
			<td colspan = '3'>
			<%
				if request("requestID") = "" or request("requestID") = "0" then
			%>
				<input type = 'text' name = 'status' value = 'Pending' readonly>
			<%
				elseif rs.Fields("Approved") = "0" then
			%>
				<input type = 'text' name = 'status' value = 'Pending' readonly>
			<%
				elseif rs.Fields("Approved") = "1" then
			%>
				<input type = 'text' name = 'status' value = 'Approved' readonly>
			<%
				else
			%>
				<input type = 'text' name = 'status' value = 'Denied' readonly>
			<%
				end if
			%>
			</td>
		</tr>
	</table>
	<input type = 'hidden' name = 'isManual' value = '<%=rs.Fields("IsManual")%>'>
	</form>
<%
end if

cn.Close
set rs = Nothing
set rs2 = Nothing
set cn = Nothing
response.write sAns



%>