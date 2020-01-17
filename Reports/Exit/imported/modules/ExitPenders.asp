
<html>

<head>
<meta name="Microsoft Theme" content="none, default">
</head>

<body>

<%
	Dim cn
	Dim rs, rs2, rs3	
	Dim sDate
	Dim eDate
	sDate = CDate(request("sDate"))
	eDate = CDate(request("eDate"))
	
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	set rs2 = server.createobject("ADODB.Recordset")
	set rs3 = server.createobject("ADODB.Recordset")	
	cn.CommandTimeout = 0
	server.scripttimeout = 10000


DBName = "CRMSNET"
DBUser = "asp"
DBPass = "aspnet"


	cn.Open DBName, DBUser, DBPass
	response.write "<table STYLE='BORDER-COLLAPSE:COLLAPSE;' BORDER='1PX'>"
	rs.Open "SELECT DISTINCT a.ComboItem AS ContractStatus FROM t_ComboItems a INNER JOIN (SELECT c.StatusID FROM t_Event e INNER JOIN t_Contract c ON e.KEYVALUE = c.ContractID AND E.KEYFIELD = 'CONTRACTID' WHERE (e.OldValue = 'Pender') AND (c.ContractNumber LIKE 'T%' or c.ContractNumber like 'U%') AND (e.DateCreated between '" & sDate & "' and '" & eDate + 1 & "')) b ON a.ComboItemID = b.StatusID", cn, 3, 3
	Do while not rs.EOF
		response.write "<tr><td><B>" & rs.Fields("ContractStatus") & "</B></td></tr>"
		rs2.Open "SELECT m.MortgageID AS MortgageID, m.DPTotal as DownPayment, n.FirstName, n.lastname, n.contractstatus, n.ContractNumber, n.ContractDate, n.campaign, n.contractid FROM t_Mortgage m INNER JOIN (SELECT g.FirstName AS FirstName, g.LastName AS lastname, h.* FROM t_Prospect g INNER JOIN (SELECT e.*, f.Name AS campaign FROM t_Campaign f INNER JOIN (SELECT a.ComboItem AS contractstatus, b.* FROM t_ComboItems a INNER JOIN (SELECT e.eventid, e.OldValue, e.NewValue, c.ContractNumber, e.DateCreated, c.CampaignID, c.StatusID, c.contractid, c.ContractDate, c.ProspectID, e.FieldName FROM t_Event e INNER JOIN t_Contract c ON e.KEYVALUE = c.ContractID  AND E.KEYFIELD = 'CONTRACTID' WHERE (e.OldValue = 'Pender') AND (c.ContractNumber LIKE 'T%') AND (e.DateCreated between '" & sDate & "' and '" & eDate + 1 & "')) b ON a.ComboItemID = b.StatusID) e ON e.CampaignID = f.CampaignID) h ON g.ProspectID = h.ProspectID) n ON m.ContractID = n.contractid where n.contractstatus = '" & rs.Fields("ContractStatus") & "' ORDER BY  n.ContractNumber", cn, 3, 3
		Do While not rs2.EOF
			response.write "<tr><td>&nbsp</td><td>" & rs2.Fields("COntractNumber") & "</td><td>" & rs2.Fields("ContractDate") & "</td><td>" & rs2.Fields("Campaign") & "</td><td>" & rs2.Fields("LastName") & ", " & rs2.Fields("FirstName") & "</td>"
			rs3.Open "Select OldValue, NewValue, DateCreated from t_Event where FieldName = 'Status' AND KEYFIELD = 'CONTRACTID' AND KEYVALUE = '" & rs2.Fields("ContractID") & "'", cn, 3, 3
			If rs3.EOF and rs3.BOF then
				response.write "<td>&nbsp</td>"
			Else
				response.write "<td><table>"
				Do while not rs3.EOF
					response.write "<tr><td>" & rs3.Fields("DateCreated") & "</td><td>" & rs3.Fields("OldValue") & " - " & rs3.Fields("NewValue") & "</td></tr>"
					rs3.MoveNext
				Loop
				response.write "</table></td>"
			End If
			rs3.Close


			rs3.Open "Select ABS(sum(amount)) AS AMOUNT from v_payments v where v.keyfield = 'mortgagedp'and v.keyvalue = '" & rs2.Fields("MortgageID") & "' ", cn, 3, 3

			If (rs3.EOF and rs3.BOF) then
				response.write "<td>$0.00</td>"
				gpAmtPaid = gpAmtPaid + 0
				totalAmtPaid = totalAmtPaid + 0
				response.write "<td>$" &  FormatNumber(rs2.Fields("DownPayment"), 2) & "</td>"
			else

                balance = rs3.Fields("Amount")

				response.write "<td>" & FormatCurrency(balance) & "</td>"
				gpAmtPaid = gpAmtPaid + balance
				totalAmtPaid = totalAmtPaid + balance
				response.write "<td>$" &  FormatNumber((rs2.Fields("DownPayment") - balance), 2) & "</td>"
			end if
			rs3.Close
			response.write "</tr>"
			rs3.OPen "Select DateCreated, Note from t_Note where KEYFIELD = 'CONTRACTID' AND KEYVALUE = '" & rs2.Fields("ContractID") & "' ORDER BY DATECREATED DESC", cn, 3, 3
			If rs3.EOF and rs3.BOF then
			Else
				response.write "<tr><td></td><td></td><td colspan = '12'><table>"
				Do while not rs3.EOF
					response.write "<tr><td>" & CDate(rs3.Fields("DateCreated")) & "&nbsp&nbsp</td><td>" & rs3.Fields("Note") & "</td></tr>"
					rs3.MoveNext
				Loop
				response.write "</table></td></tr>"
			End If
			rs3.Close
			gpCount = gpCount + 1
			totalCount = totalCount + 1
			balance = 0
			rs2.MoveNext
		Loop
		rs2.Close
		response.write "<tr><td></td><td colspan = 2><B>" & rs.Fields("ContractStatus") & " Totals:</B></td><td colspan = 2><B>Count: " &  gpCount & "</B></td><td colspan = 2><B>Amount Paid: $" &  FormatNumber(gpAmtPaid, 2) & "</B></td></tr>"
		gpAmtPaid = 0
		gpCount = 0
		rs.MoveNext
	Loop
	response.write "<tr><td><B>Grand Totals:</B></td><td colspan = 2><B>Count: " &  totalCount & "</B></td><td colspan = 2><B>Amount Paid: $" &  FormatNumber(totalAmtPaid, 2) & "</B></td></tr>"
	response.write "</table>"
	rs.Close
	cn.Close
	set rs = Nothing
	set rs2 = nothing
	set rs3 = Nothing
	set cn = Nothing
%>	
</body></html>