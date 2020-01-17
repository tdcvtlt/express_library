


<%

DBName = "CRMSNET"
DBUser = "asp"
DBPass = "aspnet"

if request("Function") = "Run_Report" then

	Dim cn
	DIm rs, rs2, rs3	
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	set rs2 = server.createobject("ADODB.Recordset")
	set rs3 = server.createobject("ADODB.Recordset")
	cn.CommandTimeout = 0
	cn.Open DBname, DBUser, DBPass
	response.ContentType = "application/vnd.ms-excel"
	response.AddHeader "Content-Disposition", "attachment; filename=toursbydatecreated.xls"
	saleCount = 0
	tourCount = 0
	totalsalescount = 0
	totaltourcount = 0
	sAns = "<table>"
	if request("PersonnelID") = "0" then
		rs.open "Select b.PersonnelID, b.FirstName, b.LastName from t_Personnel2Dept a inner join t_PErsonnel b on a.personnelid = b.personnelid where a.Departmentid =(Select comboitemid from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'Department' and comboitem = 'OPC-OS') order by LastName asc", cn, 3, 3
	Else
		rs.Open "Select PersonnelID, FirstName, LastName from t_Personnel where personnelid = '" & request("PersonnelID") & "'", cn, 3, 3
	End If
	
	Do while not rs.EOF
		saleCount = 0
		tourCount = 0
		sAns = sANs & "<tr><td colspan = '5'><H3>" & rs.Fields("LastName") & ", " & rs.Fields("FirstName") & "</H3></td></tr>"
		rs2.Open "Select Distinct(t.TourID), t.TourDate, p.FirstName, p.LastName, c.ContractNumber, ts.ComboItem as tourStatus from t_PersonnelTrans pt inner join t_Tour t on pt.Keyvalue = t.Tourid and pt.keyfield='tourid' left outer join t_Prospect p on t.prospectid = p.ProspectID left outer join t_Contract c on t.tourid = c.tourid left outer join t_Comboitems ts on t.Statusid = ts.ComboItemID where t.TourDate between '" & request("sDate") & "' and '" & request("eDate") & "' and pt.TitleID in (Select ComboItemID from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'PersonnelTitle' and (comboitem = 'OnSite Solicitor' or comboitem = 'Operator')) and pt.Personnelid = '" & rs.Fields("PersonnelID") & "' order by ts.ComboItem asc, t.TourDate asc", cn, 3, 3 
		if rs2.EOF and rs2.BOF then
			sAns = sAns & "<tr><td colspan = '5'>No Tours for this Rep.</td></tr>"
		else
			sAns = sAns & "<tr><td><b><u>TourID</u></b></td><td><b><u>Prospect</u></b></td><td><b><u>Tour Date</u></b></td><td><b><u>TourStatus</u></b></td><td><b><u>Contract</u></b></td></tr>"
			do while not rs2.eof
				sAns = sAns & "<tr>"
				sAns = sAns & "<td>" & rs2.Fields("TourID") & "</td>"
				sAns = sAns & "<td>" & rs2.FIelds("LastName") & ", " & rs2.Fields("FirstName") & "</td>"
				sAns = sAns & "<td>" & rs2.Fields("TourDate") & "</td>"
				sAns = sANs & "<td>" & rs2.Fields("TourStatus") & "</td>"
				sAns = sAns & "<td>" & rs2.Fields("ContractNumber") & "" & "</td>"
				sAns = sAns & "</tr>"
				If rs2.Fields("ContractNumber") & "" <> "" then
					saleCount = saleCount + 1
					totalSalesCount = totalSalesCount + 1
				End If
				tourCount = tourCount + 1
				totalTourCount = totalTourCount + 1
				'Premiums
				rs3.Open "Select b.PremiumName, a.TotalCost, a.QtyIssued, ps.ComboItem as PremiumStatus from t_PremiumIssued a inner join t_Premium b on a.Premiumid = b.PremiumID left outer join t_ComboItems ps on a.Statusid = ps.ComboItemID where a.keyfield='tourid' and a.keyvalue = '" & rs2.Fields("tourID") & "'", cn, 3, 3
				If rs3.EOF and rs3.BOF then
				Else
					sAns = sAns & "<tr>"
					sAns = sAns & "<td></td>"
					sAns = sAns & "<td colspan = '4'>"
					sAns = sAns & "<table>"
					sAns = sAns & "<tr><td><b>Premium</b></td><td><b>QtyIssued</b></td><td><b>TotalCost</b></td><td><b>Status</b></td></tr>"
					Do while not rs3.EOF
						sAns = sAns & "<tr>"
						sAns = sAns & "<td>" & rs3.Fields("PremiumName") & "</td>"
						sAns = sAns & "<td align = 'center'>" & rs3.Fields("QtyIssued") & "</td>"
						sAns = sANs & "<Td>" & formatCurrency(rs3.Fields("TotalCost"), 2) & "</td>"
						sAns = sAns & "<td>" & rs3.Fields("premiumStatus") & "</td>"
						sAns = sAns & "</tr>"
						rs3.MoveNext
					Loop
					sAns = sAns & "</table>"
					sAns = sAns & "</td>"
					sAns = sAns & "</tr>"
				End If
				rs3.Close			
				rs2.MoveNext
			Loop
			sAns = sANs & "<tr><td colspan = '2'><H3>" & rs.Fields("Lastname") & ", " & rs.Fields("Firstname") & " Totals:</h3></td><td><H3>Tours: " & tourCount & "</H3></td><td><h3>Sales: " & saleCount & "</h3></td></tr>"
		End If
		rs2.Close	
		sAns = sAns & "<tr></tr>"
		rs.MoveNext
	Loop	
	rs.Close
	sAns = sAns & "<tr>"
	sAns = sAns & "<td colspan = '2'><H2>Grand Totals:</H2></td>"
	sAns = sAns & "<td><H2>Tours: " & totalTourCount & "</H2></td>"
	sAns = sAns & "<td></td>"
	sAns = sAns & "<td><H2>Sales: " & totalSalesCount & "</H2></td>"
	sANs = sANs & "</tr>"
	sAns = sAns & "</table>"
	
	cn.Close
	set rs = Nothing
	set rs2 = Nothing
	set rs3 = nothing
	set cn = Nothing
End If

response.write sAns

%>