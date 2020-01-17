<%
class PartAssignment

	Public Function Master_Part_List
		Dim itemList, itemsSearch
		
	End Function
	
	'*** Pass through function that calls the Query_Categories function of GPInventory Class
	'*** Returns value received from GPInventory
	Public Function FilterParts (filterLvl, filters)
		Dim sFilter, filter
		set filter = new GPInventory
		sFilter = filter.Query_Categories(filterLvl, filters)
		FilterParts = sFilter
	End Function
	
	'*** Pass through function that calls the Search_Parts function of GPInventory Class
	'*** Returns value received from GPInventory
	Public Function Part_Search	(filter)
		Dim sFilter, filterLvl, filters, pFilter
		if filter = "" then
			filterLvl = 0
		else
			sFilter = Split(filter, "|")
			for i = 1 to UBound(sFilter)
				if sFilter(i) = "0" then
					exit for
				end if
			next
			filterLvl = i
		end if
		set filters = new GPInventory
		pFilter = filters.Search_Parts(filterLvl, filter)
		Part_Search = pFilter
	End Function
	
	'*** Pass through function that calls the List_Parts function of GPInventory Class
	'*** Returns value received from GPInventory
	Public Function List_Parts (requestID)
		Dim gpInventory, parts
		set gpInventory = new GPInventory
		parts = gpInventory.List_Parts(requestID)
		List_Parts = parts
	End Function
	
	Public Function List_Parts_Moved (requestID)
		Dim gpInventory, partsMoved
		set gpInventory = new GPInventory
		partsMoved = gpInventory.List_Parts_Moved(requestID)
		List_Parts_Moved = partsMoved
	End Function
	
	Public Function List_Parts_Loaned (requestID)
		Dim gpInventory, partsLoaned
		set gpInventory = new GPInventory
		partsLoaned = gpInventory.List_Parts_Loaned(requestID)
		List_Parts_Loaned = partsLoaned
	End Function
	
	Public Function Parts_On_Loan(rooms, returnDate)
		Dim gpInventory, partsLoanedList
		set gpInventory = new GPInventory
		partsLoanedList = gpInventory.Parts_On_Loan(rooms, returnDate)
		Parts_On_Loan = partsLoanedList
	End Function
	
	Public Function Check_Loan_Availability(itemID, qty)
		Dim pRequest, pRequestString
		set pRequest = new PurchaseRequest
		pRequestString = pRequest.Check_Loan_Availability(itemID, qty)
		Check_Loan_Availability = pRequestString
	End Function
	
	'*** Pass through function that calls the ??? function of PurchaseRequest class
	'*** Returns value received from PurchaseRequest
	Public Function Check_Part_Availability(itemID, qty)
		Dim pRequest, pRequestString
		set pRequest = new PurchaseRequest
		pRequestString = pRequest.Check_Part_Availability(itemID, qty)
		Check_Part_Availability = pRequestString
	End Function

	Public Function Qty_On_Hand(item)
		Dim gpInventory, qtyOnHand
		set gpInventory = new GPInventory
		qtyOnHand = gpInventory.Qty_On_Hand(item)
		Qty_On_Hand = qtyOnHand	
	End Function
	
	Public Function Item_Par_Qty(item)
		Dim gpInventory, parQty
		set gpInventory = new GPInventory
		parQty = gpInventory.Item_Par_Qty(item)
		Item_Par_Qty = parQty	
	End Function
end class
%>