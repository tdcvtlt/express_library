
<HTML>

<head>
<meta name="Microsoft Theme" content="none, default">
</head>

<BODY>

<%

	Dim sDate
	Dim eDate
	sDate = CDate(request("sDate"))
	eDate = CDate(request("eDate"))
	Dim cn
	Dim rst, rst2, rst3 
	set cn = server.createobject("ADODB.Connection")
	set rst = server.createobject("ADODB.Recordset")
	set rst2 = server.createobject("ADODB.Recordset")
	set rst3 = server.createobject("ADODB.Recordset")
	Dim sv
	Dim mortgageid
	Dim totalVol
	totalVol = 0
	Dim totalBalance
	Dim statusVol
	statusVol = 0
	Dim statusBalance
	Dim OW
	Dim conID
	OW = ""

	response.write "<H3>Exit Contract Activity</H3>"
	response.write "<table id='parent' style='border-collapse:collapse;' border='1px'>"



DBName = "CRMSNET"
DBUser = "asp"
DBPass = "aspnet"

	cn.Open DBName, DBUser, DBPass
	cn.CommandTimeout = 0
	server.scripttimeout = 1000

	rst.OPen "Select e.OldValue, e.NewValue, e.DateCreated, c.ContractID from t_Event e inner join t_Contract c on e.KEYVALUE = c.ContractID AND E.KEYFIELD = 'CONTRACTID' where e.fieldname = 'StatusID' and (e.datecreated between '" & CDate(sDate) & "' and '" & CDate(eDate + 1) & "') and (c.ContractNumber like 'T%' or c.ContractNumber like 'U%') order by c.contractnumber, e.eventid desc", cn, 3, 3
	If rst.EOF and rst.BOF then
	Else
		Do while not rst.EOF
			If conID = 0 Then
				conID = rst.Fields("ContractID")
				If CSTR(rst.FIelds("OldValue")) = "Pender" and CSTR(rst.Fields("NewValue")) = "Active" then
					If OW = "" Then
						OW = rst.Fields("ContractID")
					Else
						OW = OW & "," & rst.Fields("ContractID")
					End If
				End If
			Else
				If CSTR(rst.FIelds("OldValue")) = "Pender" and CSTR(rst.Fields("NewValue")) = "Active" and CSTR(rst.FIelds("ContractID")) <> CSTR(conID) then
					conID = rst.Fields("ContractID")
					If OW = "" Then
						OW = rst.Fields("ContractID")
					Else
						OW = OW & "," & rst.Fields("ContractID")
					End If
				End If
				conID = rst.Fields("ContractID")
			End If
			rst.MoveNext
		Loop
	End If
	rst.Close
	
	

	If OW <> "" Then
		response.write "<tr><td colspan = '2'><B>Pender - Active</B></td></tr>"
		rst2.OPen "SELECT z.FirstName AS persFirstName, z.LastName AS persLastName, y.* FROM (SELECT p.FirstName AS FirstName, p.LastName AS LastName, a.* FROM t_Prospect p Inner JOIN (SELECT c.ContractNumber, c.ContractDate, c.ProspectID, c.ContractID, d .personnelid FROM t_Contract c Left Outer JOIN (Select KEYVALUE, personnelid from t_PersonnelTrans where titleid = '16789' AND KEYFIELD = 'CONTRACTID') d ON c.contractid = d .KEYVALUE WHERE (c.ContractID in (" & OW & "))) a ON a.ProspectID = p.ProspectID) y Left Outer join t_Personnel z ON z.PersonnelID = y.personnelid order by y.contractnumber", cn, 3, 3
		Do while not rst2.EOF
			response.write "<tr><td>" & Trim(rst2.Fields("ContractNumber")) & "&nbsp&nbsp&nbsp&nbsp</td><td>" & rst2.Fields("ContractDate") & "</td><td>" & rst2.Fields("persFirstName") & " " & rst2.Fields("perslastName") & "</td><td>" & rst2.Fields("FirstName") & " " & rst2.Fields("LastName") & "</td>"
			rst3.Open "Select OldValue, NewValue, DateCreated from t_Event where FieldName = 'StatusID' and KEYVALUE = '" & rst2.Fields("ContractID") & "' AND KEYFIELD = 'CONTRACTID'", cn, 3, 3
			If rst3.EOF and rst3.BOF then
				response.write "<td>&nbsp</td>"
			Else
				response.write "<td><table>"
				Do while not rst3.EOF
					response.write "<tr><td>" & rst3.Fields("DateCreated") & "</td><td>" & rst3.Fields("OldValue") & " - " & rst3.Fields("NewValue") & "</td></tr>"
					rst3.MoveNext
				Loop
				response.write "</table></td>"
			End If
			rst3.Close
			rst3.open "Select mortgageid, salesvolume from t_Mortgage where contractid = '" & rst2.Fields("ContractID") & "'", cn,3 , 3
			If ((rst3.EOF and rst3.BOF) or isnull(rst3.Fields("SalesVolume"))) Then
				response.write "<td>&nbsp</td>"
				mortgageid = 0
				sv = 0
				statusVol = statusVol + sv
				totalVol = totalVol + sv
			Else
				mortgageid = rst3.Fields("MortgageID")
				response.write "<td align = right>$" & FormatNumber(rst3.Fields("SalesVolume"), 2) & "</td>"
				sv = rst3.Fields("SalesVolume")
				statusVol = statusVol + sv
				totalVol = totalVol + sv
			End If
			rst3.Close
			balance = 0
			If mortgageid <> 0 then
                '
				'rst3.Open "Select * from t_AccountItems where mortgageid = '" & mortgageid & "' and paid = '1'", cn, 3, 3
                rst3.open "select sum(amount) AS Amount from " & _
                            "v_payments v where v.keyfield = 'mortgagedp' " & _
                            " and v.keyvalue = '" & mortgageid & "'", cn, 3, 3

				If (rst3.EOF and rst3.BOF) or isNull(rst3.Fields("Amount")) then
					balance = 0
				Else
                    balance = rst3.Fields("Amount")
				End If

				rst3.Close
			End If

			statusBalance = statusBalance + (sv + balance)
			totalBalance = totalBalance + (sv + Balance)
			response.write "<td align = right>$" & FormatNumber((sv + balance), 2) & "</td>"
			response.write "</tr>"

			rst3.OPen "Select DateCreated, Note from t_Note where KEYFIELD = 'CONTRACTID' AND KEYVALUE = '" & rst2.Fields("ContractID") & "' ORDER BY DateCreated DESC", cn, 3, 3
			If rst3.EOF and rst3.BOF then
			Else
				response.write "<tr><td></td><td colspan = '12'><table>"
				Do while not rst3.EOF
					response.write "<tr><td>" & CDate(rst3.Fields("DateCreated")) & "</td><td>" & rst3.Fields("Note") & "</td></tr>"
				rst3.MoveNext
				Loop
				response.write "</table></td></tr>"
			End If
			rst3.Close
			rst2.MoveNext
		Loop
		rst2.Close
		response.write "<tr><td></td><td></td><td></td><td></td><td align = right><B>Pender - Active Totals: &nbsp&nbsp&nbsp</td><td align = right><b>$" & FormatNumber(statusVol, 2) & "</td><td align = right><b>$" & FormatNumber(statusBalance, 2) & "</td></tr>"
	End If

	If OW = "" then
		OW = "''"
	end if
	rst.OPen "SELECT DISTINCT c.ComboItem, c.ComboItemID FROM t_ComboItems c INNER JOIN (SELECT statusid FROM t_Contract WHERE (contractnumber LIKE 'T%' or contractnumber LIKE 'U%') AND statusdate BETWEEN '" & CDate(sDate) & "' and '" & CDate(eDate) & "') a ON a.statusid = c.ComboItemID", cn, 3, 3
	If rst.EOF and rst.BOF then
		response.write "No Sales in this Date Range"
	Else
		Do while not rst.EOF
			statusBalance = 0
			statusVol = 0
			response.write "<tr><td colspan = '2'><B>" & rst.Fields("ComboItem") & "</td></tr>"
			rst2.OPen "SELECT z.FirstName AS persFirstName, z.LastName AS persLastName, y.* FROM (SELECT p.FirstName AS FirstName, p.LastName AS LastName, a.* FROM t_Prospect p INNER JOIN (SELECT c.ContractNumber, c.ContractDate, c.ProspectID, c.ContractID, d .personnelid FROM t_Contract c LEFT OUTER JOIN (Select KEYVALUE, personnelid From t_PersonnelTrans where KEYFIELD = 'CONTRACTID' AND titleid = '16789') d ON c.contractid = d .KEYVALUE WHERE (c.ContractNumber LIKE 'T%' or c.ContractNumber LIKE 'U%') AND (c.statusDate BETWEEN '" & sDate & "' AND '" & eDate & "') AND (c.StatusID = '" & rst.Fields("ComboItemId") & "') and not (c.contractid in (" & OW & "))) a ON a.ProspectID = p.ProspectID) y left outer join t_Personnel z ON z.PersonnelID = y.personnelid ORDER BY y.ContractNumber", cn, 3, 3
			Do while not rst2.EOF
				response.write "<tr><td>" & Trim(rst2.Fields("ContractNumber")) & "&nbsp&nbsp&nbsp&nbsp</td><td>" & rst2.Fields("ContractDate") & "</td><td>" & rst2.Fields("persFirstName") & " " & rst2.Fields("perslastName") & "</td><td>" & rst2.Fields("FirstName") & " " & rst2.Fields("LastName") & "</td>"
				rst3.Open "Select OldValue, NewValue, DateCreated from t_Event where KEYFIELD = 'StatusID' and KEYVALUE = '" & rst2.Fields("ContractID") & "'", cn, 3, 3
				If rst3.EOF and rst3.BOF then
					response.write "<td>&nbsp</td>"
				Else
					response.write "<td><table>"
					Do while not rst3.EOF
						response.write "<tr><td>" & rst3.Fields("DateCreated") & "</td><td>" & rst3.Fields("OldValue") & " - " & rst3.Fields("NewValue") & "</td></tr>"
						rst3.MoveNext
					Loop
					response.write "</table></td>"
				End If
				rst3.Close

				rst3.open "Select mortgageid, salesvolume from t_Mortgage where contractid = '" & rst2.Fields("ContractID") & "'", cn,3 , 3
				If ((rst3.EOF and rst3.BOF) or isnull(rst3.Fields("SalesVolume"))) Then
					response.write "<td>&nbsp</td>"
					mortgageid = 0
					sv = 0
					statusVol = statusVol + sv
					totalVol = totalVol + sv
				Else
					mortgageid = rst3.Fields("MortgageID")
					response.write "<td align = right>$" & FormatNumber(rst3.Fields("SalesVolume"), 2) & "</td>"
					sv = rst3.Fields("SalesVolume")
					statusVol = statusVol + sv
					totalVol = totalVol + sv
				End If
				rst3.Close
				balance = 0
				If mortgageid <> 0 then


                    rst3.open "select sum(amount) AS Amount from " & _
                                "v_payments v where v.keyfield = 'mortgagedp' " & _
                                " and v.keyvalue = '" & mortgageid & "'", cn, 3, 3
					
					
					If (rst3.EOF and rst3.BOF) or isNull(rst3.Fields("Amount")) then
						balance = 0
					Else
                        balance = rst3.Fields("Amount")
					End If
					rst3.Close
				
				End If

				statusBalance = statusBalance + (sv + balance)
				totalBalance = totalBalance + (sv + Balance)
				response.write "<td align = right>$" & FormatNumber((sv + balance), 2) & "</td>"
				response.write "</tr>"

				rst3.OPen "Select DateCreated, Note from t_Note where KEYFIELD = 'CONTRACTID' AND KEYVALUE = '" & rst2.Fields("ContractID") & "' ORDER BY DateCreated DESC", cn, 3, 3
				If rst3.EOF and rst3.BOF then
				Else
					response.write "<tr><td></td><td colspan = '12'><table>"
					Do while not rst3.EOF
						response.write "<tr><td>" & CDate(rst3.Fields("DateCreated")) & "</td><td>" & rst3.Fields("Note") & "</td></tr>"
					rst3.MoveNext
					Loop
					response.write "</table></td></tr>"
				End If
				rst3.Close
				rst2.MoveNext
			Loop
			rst2.Close
			response.write "<tr><td></td><td></td><td></td><td></td><td align = right><B>" & rst.Fields("ComboItem") & " Totals: &nbsp&nbsp&nbsp</td><td align = right><b>$" & FormatNumber(statusVol, 2) & "</td><td align = right><b>$" & FormatNumber(statusBalance, 2) & "</td></tr>"
			rst.MoveNext
		Loop
	End If
	
	
	response.write "<tr><td></td><td></td><td></td><td></td><td align = right><H3>Grand Totals: &nbsp&nbsp&nbsp<H3></td><td align = right><H3>$" & FormatNumber(totalVol, 2) & "</H3></td><td align = right><H3>$" & FormatNumber(totalBalance, 2) & "</H3></td></tr>"
	rst.Close
	cn.Close
	set rst2 = Nothing
	Set rst3 = Nothing
	set cn = Nothing
	set rst = Nothing
	response.write "</table>"
%>

</BODY>

</HTML>