<!--#Include file = "dbconnections.inc"-->
<!--#include file="PurchaseRequest.asp" -->
<!--#include file="PartAssignment.asp" -->
<!--#include file="GPInventory.asp" -->

<%
	Dim cn
	Dim rs
	
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	server.scripttimeout = 10000
	cn.Open DBName, DBUser, DBPass
	
	if request("Function") = "Run_Report" then
		response.ContentType = "application/vnd.ms-excel"
		response.AddHeader "Content-Disposition", "attachment; filename=Master_Inventory_List.xls"
		sAns = "<table id=table-parts>"
		sAns = sAns & "<thead><tr><th><u>Item Number</u></th><th><u>Item Desc.</u></th><th><u>Qty On Hand</u></th><th><u>Qty Assigned</u></th><th><u>Qty Available</u></th><th><u>Qty On Order</u></th><th><u>Qty Pending PR Approval</u></th><th><u>Qty Requested</u></th></tr></thead><tbody>"
		
		'***** This is used to perform the actual search for parts once all filters have been selected
		'***** Creates a PartAssignment object passing it the filter string seperated by "|" charachter
		'***** Once parts are returned the string is split by "|" character and values displayed in a table
		Dim partsearch '***** Part Assignment Object
		Dim parts '***** string returned from PartAssignment object seperated by "|" character
	
		'***** request("filter") contains the other filter material passed from jscript select boxes
		dim filter
		filter = request("filter")
		set partsearch = new PartAssignment
		parts = partsearch.Part_Search(filter)
		
		if parts = "" then
			sAns = sAns & "<tr colspan = '8'><td>No Parts Matching Your Search Criteria</td></tr>"
		else
			'****** Split the array and go by 2 value 1 is the ItemNumber, value 2 is the Item Description
			sParts = Split(parts, "|")
			for i = 0 to UBound(sParts) step 2
				sAns = sAns & "<tr>"
				sAns = sAns & "<td>" & sParts(i) & "</td>"
				sAns = sAns & "<td>" & sParts(i + 1) & "</td>"
				qtyOnHand = partsearch.Qty_On_Hand(sParts(i))
				rs.Open "Select Sum(qty) as Installed from t_requestParts where itemNumber = '" & sParts(i) & "' and GPImported = '0' and statusid in (Select comboitemid from t_ComboItems c inner join t_Combos co on c.ComboID = co.COmboID where co.comboname = 'PartStatus' and c.comboitem = 'Installed')", cn, 3, 3
				If IsNull(rs.Fields("Installed")) or (rs.EOF and rs.BOF) then 
					installed = 0
				Else
					installed = rs.Fields("Installed")
				End If
				rs.Close
				trueOnHand = CDBL(qtyOnHand) - CDBL(installed)
				rs.Open "Select sum(qty) As Assigned from t_RequestParts where itemNumber = '" & sParts(i) & "' and statusid in (Select comboitemid from t_ComboItems c inner join t_Combos co on c.ComboID = co.COmboID where co.comboname = 'PartStatus' and c.comboitem = 'Assigned')", cn, 3, 3
				If IsNull(rs.Fields("Assigned")) then
					assigned = 0
				Else
					assigned = rs.Fields("Assigned")
				End If
				rs.Close
				avail = CDBL(trueOnHand) - CDBL(assigned)
				rs.open "Select Sum(qty) As Requested from t_RequestParts where itemNumber = '" & sParts(i) & "' and statusid in (Select comboitemid from t_ComboItems c inner join t_Combos co on c.ComboID = co.COmboID where co.comboname = 'PartStatus' and comboitem = 'Requested')", cn, 3, 3 
				If IsNull(rs.Fields("Requested")) then
					requested = 0
				Else
					requested = rs.Fields("Requested")
				End If
				rs.Close
				
				rs.Open "Select Sum(a.Qty) As Ordered from t_PurchaseRequestItems a inner join t_PurchaseRequest b on a.PurchaseRequestID = b.PurchaseRequestID where a.ItemNumber = '" & sParts(i) & "' and (b.Approved = '1' and b.Received = '0')", cn, 3, 3
				If IsNull(rs.Fields("Ordered")) then
					ordered = 0
				Else
					ordered = rs.FIelds("Ordered")
				End If
				rs.Close
				
				rs.Open "Select Sum(Qty) As PRAssigned from t_RequestParts where itemNumber = '" & sParts(i) & "' and statusid in (Select comboitemid from t_ComboItems c inner join t_Combos co on c.ComboID = co.COmboID where co.comboname = 'PartStatus' and comboitem = 'Assigned') and PRID > 0", cn, 3, 3
				If IsNull(rs.FIelds("PRAssigned")) then
					prAssigned = 0
				Else
					prAssigned = rs.FIelds("PRAssigned")
				End If
				rs.Close
				totalOrdered = CDBL(ordered) '- CDBL(prAssigned)

				rs.open "Select Sum(Qty) As PRRequested from t_PurchaseRequestItems a inner join t_PurchaseRequest b on a.PurchaseRequestID = b.PurchaseRequestID where b.Approved = '0' and a.ItemNumber = '" & sParts(i) & "'", cn, 3, 3
				If IsNull(rs.Fields("PRRequested")) then
					prRequested = 0
				Else
					prRequested = rs.FIelds("PRRequested")
				End If
				rs.Close
				
				sAns = sANs & "<td align = right>" & trueOnHand & "</td>"
				sAns = sANs & "<td align = right>" & assigned & "</td>"
				sANs = sAns & "<td align = right>" & avail & "</td>"
				sAns = sAns & "<td align = right>" & totalOrdered & "</td>"
				sAns = sAns & "<td align = right>" & prRequested & "</td>"
				sANs = sANs & "<td align = right>" & requested & "</td>"
				sAns = sANs & "</tr>"
			next
		end if
		sANs = sAns & "</tbody></table>"
	end if
	response.write sAns
	cn.close
	set rs = nothing
	set cn = nothing	
%>