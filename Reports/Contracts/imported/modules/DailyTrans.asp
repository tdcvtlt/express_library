
<HTML>

<head>
<meta name="Microsoft Theme" content="none, default">
</head>

<BODY>
<font size = '5'>Daily Trans Report</font>
<br>
<%
	if request("sDate") = request("eDate") then response.write request("sDate") else response.write request("sDate") & " - " & request("eDate") end if
%>
<br>
<br>
<%
DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"
	Function Credit_Card_Type(strCardNumber)
		Dim strCard
		strCard = Trim(strCardNumber)

		'Clean up card Number
		strCard = Replace(strCard, "-", "")
		strCard = Replace(strCard, " ", "")
		strCard = Replace(strCard, ".", "")

		Select Case CLng(Left(strCard, 1))
			Case 1
				If CLng(Left(strCard, 4)) = 1800 Then
					Credit_Card_Type = "JCB"
				Else
					Credit_Card_Type = "Invalid"
				End If
			Case 2
				If CLng(Left(strCard, 4)) = 2131 Then
					Credit_Card_Type = "JCB"
				Else
					Credit_Card_Type = "Invalid"
				End If
			Case 3
				If CLng(Left(strCard, 3)) > 299 And CLng(Left(strCard, 3)) < 306 Then
					Credit_Card_Type = "Diners Club"
				ElseIf CLng(Left(strCard, 2)) = 36 Or CLng(Left(strCard, 2)) = 38 Then
					Credit_Card_Type = "Diners Club"
				ElseIf CLng(Left(strCard, 2)) = 34 Or CLng(Left(strCard, 2)) = 37 Then
					Credit_Card_Type = "Amex"
				Else
					Credit_Card_Type = "JCB"
				End If
			Case 4
				Credit_Card_Type = "Visa"
			Case 5
				If CLng(Left(strCard, 2)) > 50 Or CLng(Left(strCard, 2)) < 56 Then
					Credit_Card_Type = "MasterCard"
				Else
					Credit_Card_Type = "Invalid Card Number"
				End If
			Case 6
				If CLng(Left(strCard, 4)) = 6011 Then
					Credit_Card_Type = "Discover"
				Else
					Credit_Card_Type = "Invalid Card Number"
				End If
			Case Else
				Credit_Card_Type = "Invalid Card Number"
		End Select
	End Function


	Dim cn
	Dim rst, rst2
	Dim amt
	set cn = server.createobject("ADODB.Connection")
	set rst = server.createobject("ADODB.Recordset")
	set rst2 = server.createobject("ADODB.Recordset")
	cn.Open DBName, DBUser, DBPass
	Dim loc
	loc = request("loc")
	
	dim creditcards
	creditcards = "'AMEX','AMEX - Refund','DinersClub','DinersClub - Refund','Discover','Discover - Refund','MasterCard','MasterCard - Refund','VISA','VISA - Refund','CreditCard','CreditCard - Refund'" 

	if request("loc") = "KCP" then
		rst.Open "Select g.*, h.COmboItem as pMethod from (Select e.*, f.FirstName as FirstName, f.LastName as LastName from (Select c.*, d.ContractNumber from (Select a.*, b.ContractID as conID from (Select * from t_AccountItems where transactiondate between '" & request("sDate") & "' and '" & request("eDate") & "' and paid = '1') a inner join t_Mortgage b on a.mortgageid = b.mortgageid) c inner join t_Contract d on c.conid = d.contractid) e inner join t_prospect f on e.prospectid = f.prospectid) g left outer join t_ComboItems h on g.paymentmethodid = h.comboitemid where ContractNumber NOT LIKE 'T%' and ContractNumber NOT LIKE 'S%'  and ContractNumber NOT LIKE 'AS%' and comboitem in (" & creditcards & ") and amount > 0 order by ComboItem asc", cn, 3, 3
	else
		rst.Open "Select g.*, h.COmboItem as pMethod from (Select e.*, f.FirstName as FirstName, f.LastName as LastName from (Select c.*, d.ContractNumber from (Select a.*, b.ContractID as conID from (Select * from t_AccountItems where transactiondate between '" & request("sDate") & "' and '" & request("eDate") & "' and paid = '1') a inner join t_Mortgage b on a.mortgageid = b.mortgageid) c inner join t_Contract d on c.conid = d.contractid) e inner join t_prospect f on e.prospectid = f.prospectid) g left outer join t_ComboItems h on g.paymentmethodid = h.comboitemid where ContractNumber NOT LIKE 'T%' and (ContractNumber LIKE 'S%' OR ContractNumber LIKE 'AS%') and comboitem in (" & creditcards & ") and amount > 0 order by ComboItem asc", cn, 3, 3	
	end if
	If rst.EOF and rst.BOF then
		response.write "<table>"
		response.write "<tr><td colspan = '10'>No Credit Card Transactions For Today</td></tr>"
		response.write "</table>"
	Else
		response.write "<table>"
		response.write "<tr><td colspan = '10'><B>Credit Card Payments</B></td></tr>"
		Do while not rst.EOF
			response.write "<tr><td>" & rst.Fields("ContractNumber") & "&nbsp&nbsp&nbsp&nbsp</td>"
			response.write "<td>" & rst.Fields("FirstName") & " " & rst.Fields("LastName") & "&nbsp&nbsp&nbsp&nbsp</td>"
			if rst.Fields("RefundFlag") or (rst.Fields("AdjFlag") and Not(rst.Fields("PaymentFlag"))) then
				amt = amt - rst.Fields("Amount")
				response.write "<td>" & CDBL(rst.Fields("Amount") * -1) & "</td>"
			else
				amt = amt + rst.Fields("Amount")
				response.write "<td>" & CDBL(rst.Fields("Amount")) & "</td>"
			end if
			response.write "<td>" & rst.Fields("pMethod") & "</td>"
			response.write "<td>" & rst.Fields("Reference") & "&nbsp&nbsp&nbsp&nbsp</td>"
			if rst.Fields("DPFlag") then
				response.write "<td>Down Payment</td>"
			else
				response.write "<td>Closing Costs</td>"
			end if
			'response.write "<td>" & rst.Fields("Description") & "&nbsp&nbsp&nbsp&nbsp</td>"
			
			rst2.Open "Select c.*, d.ComboItem as ccType from (Select a.Number, a.TypeID, ba.batch from t_CreditCard a inner join t_CCTrans b on a.creditcardid = b.creditcardid left outer join t_CCBatch ba on ba.batchid = b.batchid  where b.Applyto Like '%" & rst.Fields("AccountItemID") & "%') c left outer join t_ComboItems d on c.typeid = d.comboitemid", cn, 3, 3
			if rst2.EOF and rst2.BOF then
				response.write "<td></td>"
			elseif IsNull(rst2.Fields("Number")) or rst2.Fields("Number") = "" then
				response.write "<td></td><td></td>"
			else
				response.write "<td>" & rst2.fields("Batch").value & "</td>"
				response.write "<td>" & rst2.Fields("ccType") & "</td>"
				response.write "<td>" & Left(Trim(rst2.Fields("Number")), 4) & "X" & Right(Trim(rst2.Fields("Number")), 4) & "</td>"
			end if
			rst2.Close
			response.write "<td>" & rst.Fields("TransactionDate") & "</td>"
			response.write "</tr>"
			rst.MoveNext
		Loop
		response.write "<tr><td colspan = '3'><B>Credit Card Totals: $" & Replace(Replace(Replace(FormatCurrency(amt), "(", "-"), ")", ""), "$", "") & "</B></td></tr>"
		response.write "</table>"
	End If
	rst.Close
	response.write "<br>"
	grandamt = amt
	amt = 0
	If request("loc") = "KCP" then
		rst.Open "Select g.*, h.COmboItem as pMethod from (Select e.*, f.FirstName as FirstName, f.LastName as LastName from (Select c.*, d.ContractNumber from (Select a.*, b.ContractID as conID from (Select * from t_AccountItems where transactiondate between '" & request("sDate") & "' and '" & request("eDate") & "' and paid = '1') a inner join t_Mortgage b on a.mortgageid = b.mortgageid) c inner join t_Contract d on c.conid = d.contractid) e inner join t_prospect f on e.prospectid = f.prospectid) g left outer join t_ComboItems h on g.paymentmethodid = h.comboitemid where ContractNumber NOT LIKE 'T%' AND ContractNumber NOT LIKE 'S%' and ContractNumber NOT LIKE 'AS%' and comboitem not in (" & creditcards & ") and amount > 0 order by ComboItem asc", cn, 3, 3
	Else
		rst.Open "Select g.*, h.COmboItem as pMethod from (Select e.*, f.FirstName as FirstName, f.LastName as LastName from (Select c.*, d.ContractNumber from (Select a.*, b.ContractID as conID from (Select * from t_AccountItems where transactiondate between '" & request("sDate") & "' and '" & request("eDate") & "' and paid = '1') a inner join t_Mortgage b on a.mortgageid = b.mortgageid) c inner join t_Contract d on c.conid = d.contractid) e inner join t_prospect f on e.prospectid = f.prospectid) g left outer join t_ComboItems h on g.paymentmethodid = h.comboitemid where ContractNumber NOT LIKE 'T%' AND (ContractNumber LIKE 'S%' OR ContractNumber LIKE 'AS%') and comboitem not in (" & creditcards & ") and amount > 0 order by ComboItem asc", cn, 3, 3	
	End If
	If rst.EOF and rst.BOF then
		response.write "<table>"
		response.write "<tr><td colspan = '10'>No Non Credit Card Transactions For Today</td></tr>"
		response.write "</table>"
	Else
		response.write "<table>"
		response.write "<tr><td colspan = '10'><B>Non Credit Card Payments</B></td></tr>"
		Do while not rst.EOF
			response.write "<tr><td>" & rst.Fields("ContractNumber") & "&nbsp&nbsp&nbsp&nbsp</td>"
			response.write "<td>" & rst.Fields("FirstName") & " " & rst.Fields("LastName") & "&nbsp&nbsp&nbsp&nbsp</td>"
			if rst.Fields("RefundFlag") or (rst.Fields("AdjFlag") and Not(rst.Fields("PaymentFlag"))) then
				amt = amt - rst.Fields("Amount")
				response.write "<td>-" & rst.Fields("Amount") & "</td>"
			else
				amt = amt + rst.Fields("Amount")
				response.write "<td>" & rst.Fields("Amount") & "</td>"
			end if
			response.write "<td>" & rst.Fields("pMethod") & "</td>"
			response.write "<td>" & rst.Fields("Reference") & "&nbsp&nbsp&nbsp&nbsp</td>"
			if rst.Fields("DPFlag") then
				response.write "<td>Down Payment</td>"
			else
				response.write "<td>Closing Costs</td>"
			end if
			'response.write "<td>" & rst.Fields("Description") & "&nbsp&nbsp&nbsp&nbsp</td>"
			
			response.write "<td>" & rst.Fields("TransactionDate") & "</td>"
			response.write "</tr>"
			rst.MoveNext
		Loop
		response.write "<tr><td colspan = '3'><B>Non Credit Card Totals: $" & Replace(Replace(Replace(FormatCurrency(amt), "(", "-"), ")", ""), "$", "") & "</B></td></tr>"
		response.write "</table>"
	End If
	rst.Close

	grandamt = grandamt + amt
	response.write "<br>"
	response.write "<table><tr><td><H2>Grand Total: $" & Replace(Replace(Replace(FormatCurrency(grandamt), "(", "-"), ")", ""), "$", "") & "</H2></td></tr></table>"
	cn.Close
	set rst = Nothing
	set rst2 = Nothing
	set cn = Nothing
%>

</BODY>
</HTML>