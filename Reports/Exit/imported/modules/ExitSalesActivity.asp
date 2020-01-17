

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
	Dim rst, rst2
	Dim conID
	conID = 0
	set cn = server.createobject("ADODB.Connection")
	set rst = server.createobject("ADODB.Recordset")
	set rst2 = server.createobject("ADODB.Recordset")

	response.write "<H3>Exit Sales Activity</H3>"
	response.write "<table STYLE='BORDER-COLLAPSE:COLLAPSE;' BORDER='1PX'>"

DBName = "CRMSNET"
DBUser = "asp"
DBPass = "aspnet"

	cn.Open DBName,DBUser,DBPass
	cn.CommandTimeout = 0
	server.scripttimeout = 1000

	rst.OPen "Select e.OldValue, e.NewValue, e.DateCreated, c.ContractID from t_Event e inner join t_Contract c on e.KEYVALUE = c.ContractID AND E.KEYFIELD = 'CONTRACTID' where e.fieldname = 'StatusID' and (e.datecreated between '" & CDate(sDate) & "' and '" & CDate(eDate + 1) & "') and c.contractDate < '" & CDate(sDate) & "' and (c.ContractNumber like 'T%' or c.ContractNumber like 'U%') order by c.contractnumber, e.eventid desc", cn, 3, 3
	If rst.EOF and rst.BOF then
	Else
		Do while not rst.EOF
			If conID = 0 Then
				conID = rst.Fields("ContractID")

				If CSTR(rst.FIelds("OldValue")) = "Pender" and CSTR(rst.Fields("NewValue")) = "Active" then
					rst2.Open "Select g.ComboItem as contractstatus, h.* from t_ComboItems g inner join (SELECT e.FirstName AS ownFName, e.LastName AS ownLName, f.* FROM t_Prospect e INNER JOIN (SELECT a.FirstName AS repFName, a.LastName AS repLName, b.*  FROM t_Personnel a INNER JOIN (SELECT c.ProspectID, c.ContractNumber, c.ContractID, c.ContractDate, d .PersonnelID, c.StatusID FROM t_Contract c INNER JOIN  t_PersonnelTrans d ON c.ContractID = d.KEYVALUE AND D.KEYFIELD = 'CONTRACTID' WHERE  (d .TitleID = '16789') and (c.contractid = '" & rst.Fields("ContractID") & "')) b ON a.PersonnelID = b.PersonnelID) f ON  e.ProspectID = f.ProspectID) h on g.comboitemid = h.statusid ORDER BY h.ContractNumber", cn, 3, 3
					
					response.write "<tr><td>" & rst2.Fields("ContractNumber") & "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>"
					response.write "<td>" & rst2.Fields("ContractDate") & "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>"
					response.write "<td>" & rst2.Fields("OwnFName") & " " & rst2.Fields("OwnLName") & "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>"
					response.write "<td>" & rst2.Fields("repFName") & " " & rst2.FIelds("repLName") & "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>"
					response.write "<td>Pender-Active</td></tr>"

					rst2.Close

				End If
			Else
				If CSTR(rst.FIelds("OldValue")) = "Pender" and CSTR(rst.Fields("NewValue")) = "Active" and CSTR(rst.FIelds("ContractID")) <> CSTR(conID) then
					
					rst2.Open "Select g.ComboItem as contractstatus, h.* from t_ComboItems g inner join (SELECT e.FirstName AS ownFName, e.LastName AS ownLName, f.* FROM t_Prospect e INNER JOIN (SELECT a.FirstName AS repFName, a.LastName AS repLName, b.*  FROM t_Personnel a INNER JOIN (SELECT c.ProspectID, c.ContractNumber, c.ContractID, c.ContractDate, d .PersonnelID, c.StatusID FROM t_Contract c INNER JOIN  t_PersonnelTrans d ON c.ContractID = d.KEYVALUE AND D.KEYFIELD = 'CONTRACTID' WHERE  (d .TitleID = '16789') and (c.contractid = '" & rst.Fields("ContractID") & "')) b ON a.PersonnelID = b.PersonnelID) f ON  e.ProspectID = f.ProspectID) h on g.comboitemid = h.statusid ORDER BY h.ContractNumber", cn, 3, 3

					If not(rst2.EOF and rst2.BOF) then
					response.write "<tr><td>" & rst2.Fields("ContractNumber") & "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>"
					response.write "<td>" & rst2.Fields("ContractDate") & "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>"
					response.write "<td>" & rst2.Fields("OwnFName") & " " & rst2.Fields("OwnLName") & "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>"
					response.write "<td>" & rst2.Fields("repFName") & " " & rst2.FIelds("repLName") & "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>"
					response.write "<td>Pender-Active</td></tr>"

					End If

					rst2.Close
				End If

				conID = rst.Fields("ContractID")
			End If
			rst.MoveNext
		Loop
	End If
	rst.Close
	rst.OPen "Select g.ComboItem as contractstatus, h.* from t_ComboItems g inner join (SELECT e.FirstName AS ownFName, e.LastName AS ownLName, f.* FROM t_Prospect e INNER JOIN (SELECT a.FirstName AS repFName, a.LastName AS repLName, b.*  FROM t_Personnel a INNER JOIN (SELECT c.ProspectID, c.ContractNumber, c.ContractID, c.ContractDate, d .PersonnelID, c.StatusID FROM t_Contract c INNER JOIN  t_PersonnelTrans d ON c.ContractID = d.KEYVALUE AND D.KEYFIELD = 'CONTRACTID' WHERE (c.ContractNumber LIKE 'T%' or c.Contractnumber like 'U%') AND (d .TitleID = '16789') and (c.contractdate between '" & sDate & "' and '" & eDate & "')) b ON a.PersonnelID = b.PersonnelID) f ON  e.ProspectID = f.ProspectID) h on g.comboitemid = h.statusid ORDER BY h.ContractNumber", cn, 3, 3
	If rst.EOF and rst.BOF then
		response.write "No Sales in this Date Range"
	Else
		Do while not rst.EOF
			response.write "<tr><td>" & rst.Fields("ContractNumber") & "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>"
			response.write "<td>" & rst.Fields("ContractDate") & "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>"
			response.write "<td>" & rst.Fields("OwnFName") & " " & rst.Fields("OwnLName") & "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>"
			response.write "<td>" & rst.Fields("repFName") & " " & rst.FIelds("repLName") & "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>"
			If rst.Fields("ContractStatus") = "Active" then
				rst2.Open "Select * from t_Event where fieldname = 'status' and oldvalue = 'pender' and newvalue = 'active' and KEYFIELD = 'CONTRACTID' AND KEYVALUE = '" & rst.Fields("ContractID") & "'", cn, 3, 3
				If rst2.EOF and rst2.BOF then
					response.write "<td>" & rst.Fields("ContractStatus") & "</td></tr>"
				Else
					response.write "<td>Pender-Active</td></tr>"
				End If
				rst2.Close
			Else
				response.write "<td>" & rst.Fields("ContractStatus") & "</td></tr>"
			End If
			rst.MOveNext
		Loop
	End If
	rst.Close
	cn.Close
	set rst = Nothing
	set rst2 = nothing
	set cn = Nothing
	response.write "</table>"
%>

</BODY>

</HTML>