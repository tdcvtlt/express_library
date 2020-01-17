
<%
	Dim cn
	Dim rs
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	set rs2 = server.createobject("ADODB.Recordset")
    
    dim lastTourID
    lastTourID = 0

	subTypeTours = 0
	subTypeFDSales = 0
	subTypePDSales = 0
	subTypeCXLSales = 0
	subTypePDVolume = 0
	subTypeFDVolume = 0
	subTypeCXLVolume = 0
	ownerTours = 0
	ownerFDSales = 0
	ownerPDSales = 0
	ownerCXLSales = 0
	ownerPDVolume = 0
	ownerFDVolume = 0
	ownerCXLVolume = 0

	grandOwnerTours = 0
	grandOwnerFDSales = 0
	grandOwnerPDSales = 0
	grandOwnerCXLSales = 0
	grandOwnerFDVolume = 0
	grandOwnerPDVolume = 0
	grandOwnerCXLVolume = 0
	grandTours = 0
	grandPDSales = 0
	grandFDSales = 0
	grandCXLSales = 0
	grandPDvolume = 0
	grandFDVolume = 0
	grandCXLVolume = 0


	subtype = "test"
	
	cn.CommandTimeout = 0



DBName = "CRMSNET"
DBUser = "asp"
DBPass = "aspnet"
    
    'cn.Open "CRMSData", "asp", "aspnet"

	cn.Open DBName, DBUser, DBPass
	
	server.scripttimeout = 10000
	response.write "<table>"
	response.write "<tr><th><b><u>TourID</b></u></th><th><b><u>TourDate</b></u></th><th><b><u>TourStatus</b></u></th><th><b><u>Campaign</u></b></th><th><b><u>KCP#</b></u></th><th><b><u>Volume</b></u></th><th><b><u>ContractStatus</b></u></th><th><b><u>ContractType</b></u></th><th><b><u>ResType</b></u></th><th><b><u>ResSubType</b></u></th><th><b><u>Line</b></u></th><th><b><u>Sales Rep</u></b></th><th><b><u>Solicitor</b></u></th></tr>"
	dim sql
    sql = "SELECT     t.TourID, t.TourDate, ts.ComboItem AS TourStatus, camp.Name as Campaign, c.ContractNumber, m.SalesVolume, cs.ComboItem AS ContractStatus, rt.ComboItem AS ResType,  " & _
                      "rst.ComboItem AS ResSubType, tst.ComboItem AS TourSubType, ct.ComboItem AS ContractType, se.SalesRep, os.SalesRep2 " & _
"FROM         t_Tour AS t LEFT OUTER JOIN " & _
                      "t_Reservations AS r ON t.ReservationID = r.ReservationID LEFT OUTER JOIN " & _
                      "t_Contract AS c ON c.TourID = t.TourID LEFT OUTER JOIN " & _
                      "t_Campaign camp on t.CampaignID = camp.CampaignID LEFT OUTER JOIN " & _
                      "t_Mortgage AS m ON c.ContractID = m.ContractID LEFT OUTER JOIN " & _
                      "t_ComboItems AS ts ON t.StatusID = ts.ComboItemID LEFT OUTER JOIN " & _
                      "t_ComboItems AS rt ON r.TypeID = rt.ComboItemID LEFT OUTER JOIN " & _
                      "t_ComboItems AS rst ON r.SubTypeID = rst.ComboItemID LEFT OUTER JOIN " & _
                      "t_ComboItems AS tst ON t.SubTypeID = tst.ComboItemID LEFT OUTER JOIN " & _
                      "t_ComboItems AS ct ON c.TypeID = ct.ComboItemID LEFT OUTER JOIN " & _
                      "t_ComboItems AS cs ON c.StatusID = cs.ComboItemID LEFT OUTER JOIN  " & _
					  "(Select p.FirstName + ' ' + p.LastName as SalesRep, pt.keyvalue " & _
"						from t_PersonnelTrans pt  " & _
							"inner join t_Personnel p on pt.PersonnelID = p.PersonnelID  " & _
						"where PT.KEYFIELD = 'TOURID' " & _ 
							"and pt.titleid = ( " & _
								"Select comboitemID  " & _
								"from t_ComboItems A  " & _
									"INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID  " & _
								"where comboname = 'PersonnelTitle' and comboitem = 'Sales Executive' " & _
							")) se on se.keyvalue = t.tourid LEFT OUTER JOIN  " & _
						"(Select p.FirstName + ' ' + p.LastName as SalesRep2, pt.keyvalue " & _
							"from t_PersonnelTrans pt  " & _
								"inner join t_Personnel p on pt.PersonnelID = p.PersonnelID  " & _
							"where  PT.KEYFIELD = 'TOURID' " & _ 
								"and PT.titleid = ( " & _
									"Select comboitemID  " & _
									"from t_ComboItems A  " & _
										"INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID  " & _
									"where comboname = 'PersonnelTitle' and comboitem = 'OnSite Solicitor') " & _
						") OS on OS.Keyvalue = t.tourid " & _
"WHERE     (t.CampaignID in " & _
                          "(SELECT     CampaignID " & _
                            "FROM          t_Campaign " & _
                            "WHERE      (Name LIKE 'OPC-OS%' or name = 'KSDP' or name like 'OPCOS%'))) AND (t.SubTypeID NOT IN " & _
                          "(SELECT     A.ComboItemID " & _
                            "FROM          t_ComboItems AS A INNER JOIN " & _
                                                   "t_Combos AS B ON A.ComboID = B.ComboID " & _
                            "WHERE      (B.ComboName = 'TourSubtype') AND (A.ComboItem = 'Exit'))) AND (t.TourDate BETWEEN '" & request("sDate") & "' AND  " & _
                      "'" & request("eDate") & "') AND (ts.ComboItem IN ('Ontour', 'Showed')) OR " & _
                      "(t.CampaignID in " & _
                          "(SELECT     CampaignID " & _
                            "FROM          t_Campaign AS t_Campaign_1 " & _
                            "WHERE      (Name LIKE 'OPC-OS%' and name = 'KSDP' or name like 'OPCOS%'))) AND (t.SubTypeID NOT IN " & _
                          "(SELECT     A.ComboItemID " & _
                            "FROM          t_ComboItems AS A INNER JOIN " & _
                                                   "t_Combos AS B ON A.ComboID = B.ComboID " & _
                            "WHERE      (B.ComboName = 'TourSubtype') AND (A.ComboItem = 'Exit'))) AND (t.TourDate BETWEEN '" & request("sDate") & "' AND  " & _
                      "'" & request("eDate") & "') AND (c.ContractNumber IS NOT NULL) " & _
"ORDER BY tst.ComboItem, t.TourDate, t.tourid"
	'rs.open "SELECT t.TourID, t.TourDate, ts.ComboItem AS TourStatus, c.ContractNumber, m.SalesVolume, cs.ComboItem AS ContractStatus, rt.ComboItem AS ResType, rst.ComboItem AS ResSubType, tst.ComboItem as TourSubType, ct.ComboItem as ContractType FROM t_Tour t LEFT OUTER JOIN t_Reservations r ON t.TourID = r.TourID LEFT OUTER JOIN t_Contract c ON c.TourID = t.TourID LEFT OUTER JOIN t_Mortgage m ON c.ContractID = m.ContractID LEFT OUTER JOIN t_ComboItems ts ON t.StatusID = ts.ComboItemID LEFT OUTER JOIN t_ComboItems rt ON r.TypeID = rt.ComboItemID LEFT OUTER JOIN t_ComboItems rst ON r.SubTypeID = rst.ComboItemID LEFT OUTER JOIN t_ComboItems tst ON t.SubTypeID = tst.ComboItemID LEFT OUTER JOIN t_ComboItems ct ON c.TypeID = ct.ComboItemID LEFT OUTER JOIN t_ComboItems cs ON c.StatusID = cs.ComboItemID  WHERE (t.CampaignID = (SELECT campaignid FROM t_Campaign WHERE name = 'OPC-OS')) AND (t.SubTypeID NOT IN (SELECT comboitemid FROM t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID WHERE comboname = 'TourSubtype' AND comboitem LIKE '%Exit%')) AND (t.TourDate BETWEEN '" & request("sDate") & "' AND '" & request("eDate") & "') and (ts.ComboItem in ('Ontour','Showed') or c.ContractNumber is not null) ORDER BY tst.comboitem asc, t.TourDate", cn, 3, 3
    rs.open sql, cn, 0, 1
	If rs.EOF and rs.BOF then
		response.write "<tr><td colspan = '10'>No Tours in the Date Range</td></tr>"
	Else
		Do while not rs.EOF
			if subType <> rs.Fields("TourSubType") & "" then
				if subType = "test" then
					if rs.Fields("TourSubType") & "" <> "" then
						response.write "<tr><td colspan = '10'><B>" & rs.Fields("TourSubType") & "</B></td></tr>"
					else					
						response.write "<tr><td colspan = '10'><B>N/A</B></td></tr>"
					end if
					subType = rs.Fields("TourSubType") & ""
				else
					response.write "<tr>"
					response.write "<td colspan = '2'><b>Owner Totals:</B></td>"
					response.write "<td colspan = '1'><B>Tours: " & ownerTours & "</B></td>"
					response.write "<td colspan = '4'><B>FD Sales: " & ownerFDSales & " PD Sales: " & ownerPDSales & " CXL Sales: " & ownerCXLSales & "</B></td>"
					response.write "<td colspan = '6'><B>FD Volume: " & FormatCurrency(ownerFDVolume, 2) & " PD Volume: " & FormatCurrency(ownerPDVolume, 2) & " CXL Volume: " & FormatCurrency(ownerCXLVolume, 2) & "</B></td>"
					response.write "</tr>"				
					response.write "<tr>"
					response.write "<td colspan = '2'><b>Non-Owner Totals:</B></td>"
					response.write "<td colspan = '1'><B>Tours: " & subTypeTours & "</B></td>"
					response.write "<td colspan = '4'><B>FD Sales: " & subTypeFDSales & " PD Sales: " & subTypePDSales & " CXL Sales: " & subTypeCXLSales & "</B></td>"					
					response.write "<td colspan = '6'><B>FD Volume: " & FormatCurrency(subTypeFDVolume, 2) & " PD Volume: " & FormatCurrency(subTypePDVolume, 2) & " CXL Volume: " & FormatCurrency(subTypeCXLVolume, 2) & "</B></td>"
					response.write "</tr>"	
					response.write "<tr>"
					if subType & "" <> "" then
						response.write "<td colspan = '2'><B>" & subType & " Totals:</B></td>"
					else
						response.write "<td colspan = '2'><B>N/A Totals:</B></td>"
					end if
					response.write "<td colspan  = '1'><B>Tours: " & subTypeTours + ownerTours & "</B></td>"
					response.write "<td colspan = '4'><B>FD Sales: " & subTypeFDSales + ownerFDSales & " PD Sales: " & ownerPDSales + subTypePDSales & " CXL Sales: " & ownerCXLSales + subTypeCXLSales & "</B></td>"
					response.write "<td colspan = '6'><B>FD Volume: " & FormatCurrency((ownerFDVolume + subTypeFDVolume), 2) & " PD Volume: " & FormatCurrency((ownerPDVolume + subTypePDVolume), 2) & " CXL Volume: " & FormatCurrency((ownerCXLVolume + subTypeCXLVolume), 2) & "</B></td>"
					response.write "</tr>"
					response.write "<tr><td><br></td></tr>"
					subTypeTours = 0
					subTypeFDSales = 0
					subTypePDSales = 0
					subTypeCXLSales = 0
					subTypePDVolume = 0
					subTypeFDVolume = 0
					subTypeCXLVolume = 0
					ownerTours = 0
					ownerFDSales = 0
					ownerPDSales = 0
					ownerCXLSales = 0
					ownerPDVolume = 0
					ownerFDVolume = 0
					ownerCXLVolume = 0
					if rs.Fields("TourSubType") & "" <> "" then
						response.write "<tr><td colspan = '10'><B>" & rs.Fields("TourSubType") & "</B></td></tr>"
					else					
						response.write "<tr><td colspan = '10'><B>N/A</B></td></tr>"
					end if
					subType = rs.Fields("TourSubType") & ""
				end if
			end if					
			response.write "<tr>"
			response.write "<td>" & rs.Fields("TourID") & "</td>"
			response.write "<td>" & rs.Fields("TourDate") & "</td>"
			response.write "<td>" & rs.Fields("TourStatus") & "</td>"
            response.write "<td>" & rs.Fields("Campaign") & "</td>"
			response.write "<td>" & rs.Fields("ContractNumber") & "</td>"
			response.write "<td>" & rs.Fields("SalesVolume") & "</td>"
			response.write "<td>" & rs.Fields("ContractStatus") & "</td>"
			response.write "<td>" & rs.Fields("ContractType") & "</td>"
			If rs.Fields("ResType") & "" = "" then
				rs2.Open "Select rt.CombOitem as ResType, rst.ComboItem as ressubType from t_Reservations a left outer join t_CombOItems rt on a.TypeID = rt.ComboItemID left outer join t_ComboItems rst on a.SUbTypeID = rst.ComboItemID where reservationid in (Select ReservationID from t_Reservations where checkindate < '" & rs.fields("TourDate") & "' and checkoutdate > '" & rs.Fields("TourDate") & "' and prospectid = (Select prospectid from t_Tour where tourid = '" & rs.Fields("TourID") & "'))", cn, 3, 3
				
                If rs2.EOF and rs2.BOF then
                    if lastTourID & "" <> rs.fields("TourID").value & "" then
					subTypeTours = subTypeTours + 1
					grandTours = grandTours + 1
					if rs.Fields("ContractStatus") = "Pender" or rs.Fields("ContractStatus") = "Pender-INV" or rs.Fields("ContractStatus") = "Developer" then
						subTypePDSales = subTypePDSales + 1
						grandPDSales = grandPDSales + 1
						subTypePDVolume = subTypePDVolume + rs.Fields("SalesVolume") 
						grandPDVolume = grandPDVolume + rs.Fields("SalesVolume")
					elseif rs.Fields("ContractStatus") = "Active" or rs.Fields("ContractStatus") = "Suspense" then
						subTypeFDSales = subTypeFDSales + 1
						grandFDSales = grandFDSales + 1
						subTypeFDVolume = subTypeFDVolume + rs.Fields("SalesVolume") 
						grandFDVolume = grandFDVolume + rs.Fields("SalesVolume")					
					elseif rs.Fields("ContractStatus") = "Cancelled" or rs.Fields("ContractStatus") = "Rescinded" or UCASE(Left(rs.Fields("ContractStatus"), 3)) = "CXL" then
						subTypeCXLSales = subTypeCXLSales + 1
						grandCXLSales = grandCXLSales + 1
						subTypeCXLVolume = subTypeCXLVolume + rs.Fields("SalesVolume") 
						grandCXLVolume = grandCXLVolume + rs.Fields("SalesVolume")					
					end if
                    else
                        lastTourID = rs.fields("TourID").value
                    end if
					response.write "<td></td>"
					response.write "<td></td>"
				Else
                    if lastTourID & "" <> rs.fields("TourID").value & "" then
					if rs2.Fields("ResType") = "Owner" or rs2.Fields("ResType") = "Points" then
						ownerTours = ownerTours + 1
						grandOwnerTours = grandOwnerTours + 1
						grandTours = grandTours + 1
						if rs.Fields("ContractStatus") = "Pender" or rs.Fields("ContractStatus") = "Pender-INV" or rs.Fields("ContractStatus") = "Developer" then
							ownerPDSales = ownerPDSales + 1
							grandOwnerPDSales = grandOwnerPDSales + 1
							grandPDSales = grandPDSales + 1
							ownerPDVolume = ownerPDVolume + rs.Fields("SalesVolume") 
							grandPDVolume = grandPDVolume + rs.Fields("SalesVolume")
							grandOwnerPDVolume = grandOwnerPDVolume + rs.Fields("SalesVolume")
						elseif rs.Fields("ContractStatus") = "Active" or rs.Fields("ContractStatus") = "Suspense" then
							ownerFDSales = ownerFDSales + 1
							grandOwnerFDSales = grandOwnerFDSales + 1
							grandFDSales = grandFDSales + 1
							ownerFDVolume = ownerFDVolume + rs.Fields("SalesVolume") 
							grandOwnerFDVolume = grandOwnerFDVolume + rs.Fields("SalesVolume")
							grandFDVolume = grandFDVolume + rs.Fields("SalesVolume")					
						elseif rs.Fields("ContractStatus") = "Cancelled" or rs.Fields("ContractStatus") = "Rescinded" or UCASE(Left(rs.Fields("ContractStatus"), 3)) = "CXL" then
							ownerCXLSales = ownerCXLSales + 1
							grandOwnerCXLSales = grandOwnerCXLSales + 1
							grandCXLSales = grandCXLSales + 1
							ownerCXLVolume = ownerCXLVolume + rs.Fields("SalesVolume") 
							grandOwnerCXLVolume = grandOwnerCXLVolume + rs.Fields("SalesVolume")
							grandCXLVolume = grandCXLVolume + rs.Fields("SalesVolume")					
						end if					
					Else
						subTypeTours = subTypeTours + 1
						grandTours = grandTours + 1
						if rs.Fields("ContractStatus") = "Pender" or rs.Fields("ContractStatus") = "Pender-INV" or rs.Fields("ContractStatus") = "Developer" then
							subTypePDSales = subTypePDSales + 1
							grandPDSales = grandPDSales + 1
							subTypePDVolume = subTypePDVolume + rs.Fields("SalesVolume") 
							grandPDVolume = grandPDVolume + rs.Fields("SalesVolume")
						elseif rs.Fields("ContractStatus") = "Active" or rs.Fields("ContractStatus") = "Suspense" then
							subTypeFDSales = subTypeFDSales + 1
							grandFDSales = grandFDSales + 1
							subTypeFDVolume = subTypeFDVolume + rs.Fields("SalesVolume") 
							grandFDVolume = grandFDVolume + rs.Fields("SalesVolume")					
						elseif rs.Fields("ContractStatus") = "Cancelled" or rs.Fields("ContractStatus") = "Rescinded" or UCASE(Left(rs.Fields("ContractStatus"), 3)) = "CXL" then
							subTypeCXLSales = subTypeCXLSales + 1
							grandCXLSales = grandCXLSales + 1
							subTypeCXLVolume = subTypeCXLVolume + rs.Fields("SalesVolume") 
							grandCXLVolume = grandCXLVolume + rs.Fields("SalesVolume")					
						end if
					End If
                    else
                        lastTourID = rs.fields("TourID").value
                    end if				
					response.write "<td>" & rs2.Fields("ResType") & "</td>"
					response.write "<td>" & rs2.Fields("ResSubtype") & "</td>"
				End if
				rs2.Close
			Else
                if lastTourID & "" <> rs.fields("TourID").value & "" then
					if rs.Fields("ResType") = "Owner" or rs.Fields("ResType") = "Points" then
						ownerTours = ownerTours + 1
						grandOwnerTours = grandOwnerTours + 1
						grandTours = grandTours + 1
						if rs.Fields("ContractStatus") = "Pender" or rs.Fields("ContractStatus") = "Pender-INV" or rs.Fields("ContractStatus") = "Developer" then
							ownerPDSales = ownerPDSales + 1
							grandOwnerPDSales = grandOwnerPDSales + 1
							grandPDSales = grandPDSales + 1
							ownerPDVolume = ownerPDVolume + rs.Fields("SalesVolume") 
							grandPDVolume = grandPDVolume + rs.Fields("SalesVolume")
							grandOwnerPDVolume = grandOwnerPDVolume + rs.Fields("SalesVolume")
						elseif rs.Fields("ContractStatus") = "Active" or rs.Fields("ContractStatus") = "Suspense" then
							ownerFDSales = ownerFDSales + 1
							grandOwnerFDSales = grandOwnerFDSales + 1
							grandFDSales = grandFDSales + 1
							ownerFDVolume = ownerFDVolume + rs.Fields("SalesVolume") 
							grandOwnerFDVolume = grandOwnerFDVolume + rs.Fields("SalesVolume")
							grandFDVolume = grandFDVolume + rs.Fields("SalesVolume")					
						elseif rs.Fields("ContractStatus") = "Cancelled" or rs.Fields("ContractStatus") = "Rescinded" or UCASE(Left(rs.Fields("ContractStatus"), 3)) = "CXL" then
							ownerCXLSales = ownerCXLSales + 1
							grandOwnerCXLSales = grandOwnerCXLSales + 1
							grandCXLSales = grandCXLSales + 1
							ownerCXLVolume = ownerCXLVolume + rs.Fields("SalesVolume") 
							grandOwnerCXLVolume = grandOwnerCXLVolume + rs.Fields("SalesVolume")
							grandCXLVolume = grandCXLVolume + rs.Fields("SalesVolume")					
						end if					
					Else
						subTypeTours = subTypeTours + 1
						grandTours = grandTours + 1
						if rs.Fields("ContractStatus") = "Pender" or rs.Fields("ContractStatus") = "Pender-INV" or rs.Fields("ContractStatus") = "Developer" then
							subTypePDSales = subTypePDSales + 1
							grandPDSales = grandPDSales + 1
							subTypePDVolume = subTypePDVolume + rs.Fields("SalesVolume") 
							grandPDVolume = grandPDVolume + rs.Fields("SalesVolume")
						elseif rs.Fields("ContractStatus") = "Active" or rs.Fields("ContractStatus") = "Suspense" then
							subTypeFDSales = subTypeFDSales + 1
							grandFDSales = grandFDSales + 1
							subTypeFDVolume = subTypeFDVolume + rs.Fields("SalesVolume") 
							grandFDVolume = grandFDVolume + rs.Fields("SalesVolume")					
						elseif rs.Fields("ContractStatus") = "Cancelled" or rs.Fields("ContractStatus") = "Rescinded" or UCASE(Left(rs.Fields("ContractStatus"), 3)) = "CXL" then
							subTypeCXLSales = subTypeCXLSales + 1
							grandCXLSales = grandCXLSales + 1
							subTypeCXLVolume = subTypeCXLVolume + rs.Fields("SalesVolume") 
							grandCXLVolume = grandCXLVolume + rs.Fields("SalesVolume")					
						end if
					End If
                else
                    lastTourID = rs.fields("TourID").value
                end if
				response.write "<td>" & rs.Fields("ResType") & "</td>"
				response.write "<td>" & rs.Fields("ResSubType") & "</td>"
			End If
			response.write "<td>" & rs.Fields("TourSubType") & "</td>"
			'rs2.Open "Select p.FirstName + ' ' + p.LastName as SalesRep from t_PersonnelTrans pt inner join t_Personnel p on pt.PersonnelID = p.PersonnelID where pt.KEYVALUE = '" & rs.Fields("TourID") & "' AND PT.KEYFIELD = 'TOURID' and pt.titleid = (Select comboitemID from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' and comboitem = 'Sales Executive')", cn, 3, 3
			'If rs2.EOF and rs2.BOF then
			'	response.write "<td></td>"
			'Else
				response.write "<td>" & rs.Fields("SalesRep") & "&nbsp;</td>"
			'End If
			'rs2.Close
			'rs2.Open "Select p.FirstName + ' ' + p.LastName as SalesRep from t_PersonnelTrans pt inner join t_Personnel p on pt.PersonnelID = p.PersonnelID where pt.KEYVALUE = '" & rs.Fields("TourID") & "' AND PT.KEYFIELD = 'TOURID' and PT.titleid = (Select comboitemID from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' and comboitem = 'OnSite Solicitor')", cn, 3, 3
			'If rs2.EOF and rs2.BOF then
			'	response.write "<td></td>"
			'Else
				response.write "<td>" & rs.Fields("SalesRep2") & "&nbsp;</td>"
			'End If
			'rs2.Close
			response.write "</tr>"
			rs.moveNext
		Loop

		response.write "<tr>"
		response.write "<td colspan = '2'><b>Owner Totals:</B></td>"
		response.write "<td colspan = '1'><B>Tours: " & ownerTours & "</B></td>"
		response.write "<td colspan = '4'><B>FD Sales: " & ownerFDSales & " PD Sales: " & ownerPDSales & " CXL Sales: " & ownerCXLSales & "</B></td>"
		response.write "<td colspan = '6'><B>FD Volume: " & FormatCurrency(ownerFDVolume, 2) & " PD Volume: " & FormatCurrency(ownerPDVolume, 2) & " CXL Volume: " & FormatCurrency(ownerCXLVolume, 2) & "</B></td>"
		response.write "</tr>"				
		response.write "<tr>"
		response.write "<td colspan = '2'><b>Non-Owner Totals:</B></td>"
		response.write "<td colspan = '1'><B>Tours: " & subTypeTours & "</B></td>"
		response.write "<td colspan = '4'><B>FD Sales: " & subTypeFDSales & " PD Sales: " & subTypePDSales & " CXL Sales: " & subTypeCXLSales & "</B></td>"					
		response.write "<td colspan = '6'><B>FD Volume: " & FormatCurrency(subTypeFDVolume, 2) & " PD Volume: " & FormatCurrency(subTypePDVolume, 2) & " CXL Volume: " & FormatCurrency(subTypeCXLVolume, 2) & "</B></td>"
		response.write "</tr>"	
		response.write "<tr>"
		if subType <> "" then
			response.write "<td colspan = '2'><B>" & subType & " Totals:</B></td>"
		else
			response.write "<td colspan = '2'><B>N/A Totals:</B></td>"
		end if
		response.write "<td colspan  = '1'><B>Tours: " & subTypeTours + ownerTours & "</B></td>"
		response.write "<td colspan = '4'><B>FD Sales: " & subTypeFDSales + ownerFDSales & " PD Sales: " & ownerPDSales + subTypePDSales & " CXL Sales: " & ownerCXLSales + subTypeCXLSales & "</B></td>"
		response.write "<td colspan = '6'><B>FD Volume: " & FormatCurrency((ownerFDVolume + subTypeFDVolume), 2) & " PD Volume: " & FormatCurrency((ownerPDVolume + subTypePDVolume), 2) & " CXL Volume: " & FormatCurrency((ownerCXLVolume + subTypeCXLVolume), 2) & "</B></td>"
		response.write "</tr>"
		response.write "<tr><td><br></td></tr>"
		
		
		
		response.write "<tr>"
		response.write "<td colspan = '2'><b>Grand Owner Totals:</B></td>"
		response.write "<td colspan = '1'><B>Tours: " & grandownerTours & "</B></td>"
		response.write "<td colspan = '4'><B>FD Sales: " & grandownerFDSales & " PD Sales: " & grandownerPDSales & " CXL Sales: " & grandownerCXLSales & "</B></td>"
		response.write "<td colspan = '6'><B>FD Volume: " & FormatCurrency(grandownerFDVolume, 2) & " PD Volume: " & FormatCurrency(grandownerPDVolume, 2) & " CXL Volume: " & FormatCurrency(grandownerCXLVolume, 2) & "</B></td>"
		response.write "</tr>"				
		response.write "<tr>"
		response.write "<td colspan = '2'><b>Grand Non-Owner Totals:</B></td>"
		response.write "<td colspan = '1'><B>Tours: " & grandTours - grandOwnerTours & "</B></td>"
		response.write "<td colspan = '4'><B>FD Sales: " & grandFDSales - grandOwnerFDSales & " PD Sales: " & grandPDSales - grandOwnerPDSales & " CXL Sales: " & grandCXLSales - grandOwnerCXLSales & "</B></td>"					
		response.write "<td colspan = '6'><B>FD Volume: " & FormatCurrency((grandFDVolume - grandOwnerFDVolume), 2) & " PD Volume: " & FormatCurrency((grandPDVolume - grandOwnerPDVolume), 2) & " CXL Volume: " & FormatCurrency((grandCXLVolume - grandOwnerCXLVolume), 2) & "</B></td>"
		response.write "</tr>"	
		response.write "<tr>"
		response.write "<td colspan = '2'><B>Grand Totals:</B></td>"
		response.write "<td colspan  = '1'><B>Tours: " & grandTours & "</B></td>"
		response.write "<td colspan = '4'><B>FD Sales: " & grandFDSales & " PD Sales: " & grandPDSales & " CXL Sales: " & grandCXLSales & "</B></td>"
		response.write "<td colspan = '6'><B>FD Volume: " & FormatCurrency(grandFDVolume, 2) & " PD Volume: " & FormatCurrency(grandPDVolume, 2) & " CXL Volume: " & FormatCurrency(grandCXLVolume, 2) & "</B></td>"
		response.write "</tr>"
	End If
	rs.Close

	response.write "</table>"	
	cn.Close
	set rs = Nothing
	set rs2 = Nothing
	set cn = Nothing
	
	response.write  sAns
%>