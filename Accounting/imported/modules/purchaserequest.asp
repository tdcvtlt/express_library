<!--#include file="dbconnections.inc" -->
<%
class PurchaseRequest


	Public Function Check_Loan_Availability(itemID, qty)
		Dim response
		Dim cn2
		Dim rs3
		Dim gpInventory
		set cn2 = server.createobject("ADODB.Connection")
		set rs3 = server.createobject("ADODB.Recordset")
		cn2.Open DBName, DBUser, DBPass
		set gpInventory = new GPInventory
		'***** Get number of parts marked installed but not imported
		rs3.Open "Select Sum(Qty) as Parts from t_RequestParts where ItemNumber = '" & itemID & "' and statusID = (Select comboitemid from t_ComboItems where comboname = 'PartStatus' and comboitem = 'Installed') and GPImported = '0'", cn2, 3, 3
		If rs3.Fields("Parts") = 0 or IsNull(rs3.Fields("Parts")) then
			partsInstalled = 0
		Else
			partsInstalled = rs3.FIelds("Parts")
		End If
		rs3.Close
		'***** Get number of parts being loaned out currently ****'
		rs3.Open "Select sum(Qty) As PartsLoaned from t_RequestPartLoan where ItemNumber = '" & itemID & "' and statusID = (Select comboitemid from t_ComboItems where comboname = 'PartLoanStatus' and comboitem = 'OnLoan')", cn2, 3, 3
		If IsNull(rs3.Fields("PartsLoaned")) or rs3.Fields("PartsLoaned") = 0 then
			partsLoaned = 0
		Else
			partsLoaned = rs3.Fields("PartsLoaned")
		End If
		rs3.Close
		'**** Get GP on Hand Parts *****'
		gpOnHand = gpInventory.Qty_On_Hand(itemID)
		trueOnHand = CDBL(gpOnHand) - CDBL(partsInstalled) - CDBL(partsLoaned)
		if CDBL(trueOnHand) - CDBL(qty) > 0 then
			response = "OK"
		else
			response = "NO"
		end if
		cn2.Close
		set rs3 = nothing
		set cn2 = nothing		
		Check_Loan_Availability = response

	End Function
	
	Public Function Check_Part_Availability(itemID, qty)
		Dim response
		Dim cn
		Dim rs
		Dim gpInventory
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		cn.Open DBName, DBUser, DBPass
		set gpInventory = new GPInventory
		'***** Get number of parts marked installed but not imported
		rs.Open "Select Sum(Qty) as Parts from t_RequestParts where ItemNumber = '" & itemID & "' and statusID = (Select comboitemid from t_ComboItems where comboname = 'PartStatus' and comboitem = 'Installed') and GPImported = '0'", cn, 3, 3
		If rs.Fields("Parts") = 0 or IsNull(rs.Fields("Parts")) then
			partsInstalled = 0
		Else
			partsInstalled = rs.FIelds("Parts")
		End If
		rs.Close
		'***** Get number of parts being loaned out currently ****'
		rs.Open "Select sum(Qty) As PartsLoaned from t_RequestPartLoan where ItemNumber = '" & itemID & "' and statusID = (Select comboitemid from t_ComboItems where comboname = 'PartLoanStatus' and comboitem = 'OnLoan')", cn, 3, 3
		If IsNull(rs.Fields("PartsLoaned")) or rs.Fields("PartsLoaned") = 0 then
			partsLoaned = 0
		Else
			partsLoaned = rs.Fields("PartsLoaned")
		End If
		rs.Close
		'**** Get GP on Hand Parts *****'
		gpOnHand = gpInventory.Qty_On_Hand(itemID)
		trueOnHand = CDBL(gpOnHand) - CDBL(partsInstalled) - CDBL(partsLoaned)
		'**** Now get number of parts assigned to a work order - Part spoken for but not installed yet****'
		rs.Open "Select Sum(Qty) As QtyAssigned from t_RequestParts where ItemNumber = '" & itemID & "' and StatusID = (Select comboitemid from t_ComboItems where comboname = 'PartStatus' and comboitem = 'Assigned')", cn, 3, 3
		If IsNull(rs.Fields("QtyAssigned")) then
			qtyAssigned = 0
		Else
			qtyAssigned = rs.Fields("QtyAssigned")
		End If
		rs.Close
		availOnHand = trueOnHand - qtyAssigned
		
		If CDBL(availOnHand) >= CDBL(qty) then
			response = "0"
			'**** Check to see if we fall below par, if so create an order *****'
			'**** Get the Par Amount and compare it amount on hand after this qty has been assigned
			amtAfter = availOnHand - qty
			itemPar = gpInventory.Item_Par_Qty(itemID)
			'**** Get total being ordered *****'
			rs.Open "Select Sum(b.Qty) As QtyRequested from t_PurchaseRequest a inner join t_PurchaseRequestItems b on a.purchaserequestid = b.purchaserequestid where b.ItemNumber = '" & itemID & "' and (a.Approved = 0 or a.Approved = 1) and a.Received = 0", cn, 3, 3			
			If (rs.EOF and rs.BOF) or IsNull(rs.Fields("QtyRequested")) then
			Else
				amtAfter = amtAfter + CDBL(rs.Fields("QtyRequested"))
			End If
			rs.Close
			if CDBL(amtAfter) < CDBL(itemPar) then
				underParAmt = CDBL(itemPar) - CDBL(amtAfter)
				'***** Create A PR with the minimum order amount ****'
				minOrderAmt = gpInventory.Min_Order_Amt(itemID)
				'***** Create PR *****'
				rs.Open "Select * from t_PurchaseRequest where 1=2", cn, 3, 3
				rs.AddNew
				rs.Fields("Approved") = 0
				rs.Fields("POID") = 0
				rs.Fields("DateCreated") = Now
				rs.UpdateBatch
				rs.MoveFirst
				PRID = rs.Fields("PurchaseRequestID")
				rs.Close
				'**** Create Purchase RequestItem ******'
				rs.Open "Select * from t_PurchaseRequestItems where 1=2", cn, 3, 3
				rs.AddNew
				rs.Fields("ItemNumber") = itemID
				If CDBL(underParAmt) > CDBL(minOrderAmt) then
					rs.Fields("Qty") = underParAmt
					rs.Fields("Amount") = gpInventory.Item_Order_Amt(itemID, underParAmt)
				Else
					rs.Fields("Qty") = minOrderAmt
					rs.Fields("Amount") = gpInventory.Item_Order_Amt(itemID, minOrderAmt) 
				End If
				rs.Fields("Note") = "Availonhand greater than qty"
				rs.Fields("PurchaseRequestID") = PRID
				rs.UpdateBatch
				rs.Close
			end if	
			'**** End Par Check ******'
		Else
			'***** Get a total count being ordered, and a total count that are actually requested *****'
			totalQtyAssigned = 0
			totalQtyRequested = 0
			qtyLeft = qty - availOnHand
			totalQtyAssigned = qtyLeft
			rs.open "Select a.PurchaseRequestID, b.Qty As QtyRequested, (Select Sum(Qty) from t_RequestParts where ItemNumber = '" & itemID & "' and PRID = a.PurchaseRequestID and statusid in (Select comboitemid from t_ComboItems where comboname = 'PartStatus' and (comboitem = 'Assigned' or comboitem = 'Requested'))) as QtyAssigned from t_PurchaseRequest a inner join t_PurchaseRequestItems b on a.purchaserequestid = b.purchaserequestid where b.ItemNumber = '" & itemID & "' and (Approved = 0 or Approved = 1) and Received = 0", cn, 3, 3 
			Do while not rs.EOF
				If IsNull(rs.Fields("QtyRequested")) then
					qtyRequested = 0
				Else
					qtyRequested = rs.Fields("QtyRequested")
				End If
				If IsNull(rs.Fields("QtyAssigned")) then
					qtyAssigned = 0
				Else
					qtyAssigned = rs.Fields("QtyAssigned")
				End If
				totalQtyRequested = totalQtyRequested + qtyRequested
				totalQtyAssigned = totalQtyAssigned + qtyAssigned
				If qtyLeft > 0 then 
					If CDBL(qtyRequested) > CDBL(qtyAssigned) then
						qtyUnAssigned = qtyRequested - qtyAssigned
						if CDBL(qtyUnAssigned) >= CDBL(qtyLeft) then
							PRCount = PRCount + 1
							response = response & "|" & rs.Fields("PurchaseRequestID") & "|" & qtyLeft
							qtyLeft = 0
						else
							PRCount = PRCount + 1
							response = response & "|" & rs.Fields("PurchaseRequestID") & "|" & qtyUnAssigned 
							qtyLeft = qtyLeft - qtyUnAssigned
						end if
						'if qtyLeft = 0 then
						'	Exit Do
						'end if
					End If
				End If
				rs.MoveNext
			Loop			
			rs.Close
			
			'**** If there is still a qty that needs to be ordered create a PR, ****'
			'**** but we have to look at min order amount first if min order amt > qtyleft ****'
			'**** we order the min order amt, else order the qty left *****'
			If qtyLeft > 0 then
				minOrderAmt = gpInventory.Min_Order_Amt(itemID)
				'***** Create PR *****'
				rs.Open "Select * from t_PurchaseRequest where 1=2", cn, 3, 3
				rs.AddNew
				rs.Fields("Approved") = 0
				rs.Fields("POID") = 0
				rs.UpdateBatch
				rs.MoveFirst
				PRID = rs.Fields("PurchaseRequestID")
				response = response & "|" & rs.Fields("PurchaseRequestID") & "|" & qtyLeft
				rs.Close
				'**** Create Purchase RequestItem ******'
				rs.Open "Select * from t_PurchaseRequestItems where 1=2", cn, 3, 3
				rs.AddNew
				rs.Fields("ItemNumber") = itemID
				if CDBL(minOrderAmt) >= CDBL(qtyLeft) then
					totalQtyRequested = CDBL(totalQtyRequested) + CDBL(minOrderAmt)
					rs.Fields("Qty") = minOrderAmt
					rs.Fields("Amount") = gpInventory.Item_Order_Amt(itemID, minOrderAmt)
				Else
					totalQtyRequested = totalQtyRequested + qtyLeft
					rs.Fields("Qty") = qtyLeft
					rs.Fields("Amount") = gpInventory.Item_Order_Amt(itemID, qtyLeft)
				End If
				rs.Fields("PurchaseRequestID") = PRID
				rs.UpdateBatch
				rs.Close
				PRCount = PRCount + 1
			End If
			response = PRCount & response
			
			'******** Check to see if we fall under par ******'
			totalLeftOver = CDBL(totalQtyRequested) - CDBL(totalQtyAssigned)
			itemPar = gpInventory.Item_Par_Qty(itemID)
			if CDBL(totalLeftOver) < CDBL(itemPar) then
				underParAmt = CDBL(itemPar) - CDBL(totalLeftOver)
				'***** Create A PR with the minimum order amount ****'
				minOrderAmt = gpInventory.Min_Order_Amt(itemID)
				'***** Create PR *****'
				rs.Open "Select * from t_PurchaseRequest where 1=2", cn, 3, 3
				rs.AddNew
				rs.Fields("Approved") = 0
				rs.Fields("POID") = 0
				rs.UpdateBatch
				rs.MoveFirst
				PRID = rs.Fields("PurchaseRequestID")
				rs.Close
				'**** Create Purchase RequestItem ******'
				rs.Open "Select * from t_PurchaseRequestItems where 1=2", cn, 3, 3
				rs.AddNew
				rs.Fields("ItemNumber") = itemID
				If CDBL(underParAmt) > CDBL(minOrderAmt) then
					rs.Fields("Qty") = underParAmt
					rs.Fields("Amount") = gpInventory.Item_Order_Amt(itemID, underParAmt)
				Else
					rs.Fields("Qty") = minOrderAmt
					rs.Fields("Amount") = gpInventory.Item_Order_Amt(itemID, minOrderAmt)
				End If
				rs.Fields("Note") = "AmtReq: " & totalQtyRequested & " AmtAssign: " & totalQtyAssigned & " LeftOver: " & totalLeftOver & " UnderParAmt: " & underParAmt & " Par: " & itemPar & " MinOrderAmt: " & minOrderAmt  			
				rs.Fields("PurchaseRequestID") = PRID
				rs.UpdateBatch
				rs.Close			
			end if			
			
			'******** End Par Check *********'
		End If
		Check_Part_Availability = response
	End Function

	Public Function get_Request_Items(requestID)
		Dim gpInventory
		Dim requestItems
		set gpInventory = new GPInventory
		requestItems = gpInventory.get_Request_Items(requestID)
		get_Request_Items = requestItems			
	End Function
	
	Public Function Min_Order_Amt(itemID)
		Dim gpInventory
		Dim requestItems
		set gpInventory = new GPInventory
		requestItems = gpInventory.Min_Order_Amt(itemID)
		Min_Order_Amt = requestItems			
	End Function

	Public Function Item_Order_Amt(itemID, qty)
		Dim gpInventory
		Dim requestItems
		set gpInventory = new GPInventory
		requestItems = gpInventory.Item_Order_Amt(itemID, qty)
		Item_Order_Amt = requestItems	
	End Function	

	Public Function Validate_Part(itemID)
		Dim gpInventory
		Dim requestItems
		set gpInventory = new GPInventory
		requestItems = gpInventory.Validate_Part(itemID)
		Validate_Part = requestItems		
	End Function

	Public Function get_Item_Description(itemID)
		Dim gpInventory
		Dim requestItems
		set gpInventory = new GPInventory
		requestItems = gpInventory.get_Item_Description(itemID)
		get_Item_Description = requestItems		
	End Function
	
	Public Function Vendor_Lookup(vendorID)
		Dim gpInventory
		Dim vendor
		set gpInventory = new GPInventory
		vendor = gpInventory.Vendor_Lookup(vendorID)
		Vendor_Lookup = vendor		
	End Function
	
end class
%>