<%
if request("Function") = "Run_Report" then 
%> <html><head><meta http-equiv="Content-Language" content="en-us"><title>Sales Report By Closing Officer</title><meta name="Microsoft Theme" content="none, default">
</head><body><%
	dim cn
	dim rs, rsL
	dim strTitles, strPersonnelID, sDate, eDate, strStatus
	sDate = request("sDate")
	eDate = request("eDate")
	strStatus = request("Status")
	strTitles = "'Closer'"
	strPersonnelID = request("PersonnelIDs")
	
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	set rsL = server.createobject("ADODB.Recordset")

    'Server.ScriptTimeout = 5000
	cn.CommandTimeout = 5000

DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"


	cn.open DBNAME, DBUser, DBpass

	dim sSQL
    
	sSQL = "select c.contractdate, c.contractnumber, st.comboitem as SaleType, ct.comboitem as ContractType, p.lastname + ', ' + p.firstname as OwnerName, " & _
			"m.salesvolume, m.salesprice, m.dptotal, m.totalfinanced, (select sum(amount) from v_payments v where v.keyvalue = m.mortgageid and v.keyfield = 'mortgagedp') as balance, status.comboitem as status, per.lastname + ', ' + per.firstname as Closer " & _
			"from t_PersonnelTrans pt inner join t_Personnel per on per.personnelid = pt.personnelid " & _
			"inner join t_ComboItems Title on Title.comboitemid = pt.titleid " & _
			"inner join t_Contract c on c.contractid = pt.KEYVALUE AND PT.KEYFIELD = 'CONTRACTID' " & _
			"inner join t_Mortgage m on m.contractid = c.contractid " & _
			"left outer join t_ComboItems st on st.comboitemid = c.saletypeid " & _
			"left outer join t_ComboItems ct on ct.comboitemid = c.typeid " & _
			"inner join t_Prospect p on p.prospectid = c.prospectid " & _
			"left outer join t_ComboItems status on status.comboitemid = c.statusid " & _
			"where Title.comboitem in (" & strTitles & ") " & _
			"and pt.personnelid in (" & strPersonnelID & ") " & _
			"and (c.contractdate between '" & sDate & "' and '" & eDate & "' or c.statusdate between '" & sDate & "' and '" & eDate & "') " & _
			"and status.comboitemid in (" & strStatus & ") order by per.lastname, per.firstname, status.comboitem" 

	rs.open sSQL, cn, 3, 3


	dim sLastOfficer, sLastStatus, lCounterGrp1, lCounterGrp2, sGrp1, sGrp2, bresetstatus, sGrp1Totals
	lCounterGrp1 = 0
	lCounterGrp2 = 0
	sGrp1 = 0
	sGrp2 = 0
	sLastOfficer = ""
	sLastStatus = ""
	bresetstatus = false
	sGrp1Totals = ""



	do while not rs.eof		
%> <%
	'****** Begin Last Closer Check *******
		if sLastOfficer <> rs.fields("Closer").value then
			if sLastOfficer <> "" then
				bresetstatus = true
				sGrp1Totals = "<tr><td colspan = 3>Totals for " & sLastOfficer & "</td><td>" & lCounterGrp1 & "</td><td>" & formatcurrency(sGrp1) & "</td><td colspan=5>&nbsp;</td></tr>"
			else
				sLastOfficer = rs.fields("Closer").value
				response.write "<b>" & sLastOfficer	 & "</b><br><hr>"
			end if 
		end if
	
	'****** End Last Closer Check *******
%> <%
	'****** Begin Last Status Check *******
		if sLastStatus <> rs.fields("Status").value or bresetstatus then
			if sLastStatus <> "" then
%> <tr><td style="border-bottom-style: solid; border-bottom-width: 1px">&nbsp;</td> <td colspan = 3 style="border-bottom-style: solid; border-bottom-width: 1px"><b>Total for <%=sLastStatus%></b></td> <td style="border-bottom-style: solid; border-bottom-width: 1px"><b><%=lCounterGrp2%></b></td> <td style="border-bottom-style: solid; border-bottom-width: 1px"><b><%=formatcurrency(sGrp2) %></b></td> <td colspan = 4 style="border-bottom-style: solid; border-bottom-width: 1px">&nbsp;</td></tr> <% 
				sGrp2 = 0
				lCounterGrp2 = 0
				if bresetstatus= true then
%> <tr><td colspan = 3 style="border-bottom-style: solid; border-bottom-width: 1px"><b>Totals for <%=sLastOfficer%></b></td> <td style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px"><b><%=lCounterGrp1%></b></td> <td style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px"><b><%=formatcurrency(sGrp1)%></b></td> <td colspan=5 style="border-bottom-style: solid; border-bottom-width: 1px">&nbsp;</td></tr> <%
					sGrp1 = 0
					lCounterGrp1 = 0
					sLastOfficer = rs.fields("Closer").value
					response.write "</table><p style='page-break-after:always'></p><b>" & sLastOfficer	 & "</b><br><hr>"
					bresetstatus = false				
				end if
					 
%> </table><hr><%			
				bresetstatus = false
			end if
			sLastStatus = rs.fields("Status").value 
			response.write "<b>" & sLastStatus 	 & "</b>"
%> <table border = 0 width = '100%' ><tr><td style="border-bottom-style: solid; border-bottom-width: 1px"><b>Contract Date</b></td><td style="border-bottom-style: solid; border-bottom-width: 1px"><b>KCP #</b></td><td style="border-bottom-style: solid; border-bottom-width: 1px"><b>Sale Type</b></td><td style="border-bottom-style: solid; border-bottom-width: 1px"><b>Contract Type</b></td><td style="border-bottom-style: solid; border-bottom-width: 1px"><b>Owner</b></td><td style="border-bottom-style: solid; border-bottom-width: 1px" align="right"><b>Sales Vol</b></td><td style="border-bottom-style: solid; border-bottom-width: 1px" align="right"><b>Sales Price</b></td><td style="border-bottom-style: solid; border-bottom-width: 1px" align="right"><b>Down Payment</b></td><td style="border-bottom-style: solid; border-bottom-width: 1px" align="right"><b>Amt Financed</b></td><td style="border-bottom-style: solid; border-bottom-width: 1px" align="right"><b>Down Balance</b></td></tr><%
		end if
	'****** End Last Status Check *******
%> <tr><td><%=rs.fields("ContractDate").value%></td><td><%=rs.fields("ContractNumber").value%></td><td><%=rs.fields("SaleType").value%></td><td><%=rs.fields("ContractType").value%></td><td><%=rs.fields("OwnerName").value%></td><td align="right"><%
						if rs.fields("Salesvolume").value & "" <> "" then
							response.write formatcurrency(rs.fields("SalesVolume").value)
						else
							response.write formatcurrency(0)
						end if
					%></td><td align="right"><%
						if rs.fields("SalesPrice").value & "" <> "" then
							response.write formatcurrency(rs.fields("SalesPrice").value)
						else
							response.write formatcurrency(0)
						end if
					%></td><td align="right"><%
						if rs.fields("DPTotal").value & "" <> "" then
							response.write formatcurrency(rs.fields("DPTotal").value)
						else
							response.write formatcurrency(0)
						end if

					%></td><td align="right"><%
						if rs.fields("TotalFinanced").value & "" <> "" then
							response.write formatcurrency(rs.fields("TotalFinanced").value)
						else
							response.write formatcurrency(0)
						end if

					%></td><td align="right"><%
						if rs.fields("Balance").value & "" <> "" then
							response.write formatcurrency(rs.fields("Balance").value)
						else
							response.write formatcurrency(0)
						end if

					%></td></tr><%



	'******** Begin Inventory *********
		rsL.open "Select u.name, i.week from t_Unit u inner join t_Salesinventory i on i.unitid = u.unitid inner join t_Soldinventory s on s.salesinventoryid = i.salesinventoryid where s.contractid in (select contractid from t_Contract where contractnumber = '" & rs.fields("ContractNumber").value & "')", cn,3,3
		do while not rsL.eof		
%> <tr><td>&nbsp;</td><td>Unit/Week: </td><td colspan  = 8>
            <%=rsL.fields("Name").value & " / " & rsL.fields("week").value%></td></tr><%
			rsL.movenext
		loop
%> <tr><td colspan =10 style="border-bottom-style: solid; border-bottom-width: 1px">&nbsp;</td></tr><%		

		rsL.close
		sGrp1 = sGrp1 + rs.fields("SalesVolume").value
		sGrp2 = sGrp2 + rs.fields("SalesVolume").value
		lCounterGrp1 = lCounterGrp1 + 1
		lCounterGrp2 = lCounterGrp2 + 1



		rs.movenext
	loop
%> <tr><td style="border-bottom-style: solid; border-bottom-width: 1px">&nbsp;</td><td colspan = 3 style="border-bottom-style: solid; border-bottom-width: 1px"><b>Total for <%=sLastStatus%></b></td><td style="border-bottom-style: solid; border-bottom-width: 1px"><b><%=lCounterGrp2%></b></td><td style="border-bottom-style: solid; border-bottom-width: 1px"><b><%=formatcurrency(sGrp2) %></b></td><td colspan = 4 style="border-bottom-style: solid; border-bottom-width: 1px">&nbsp;</td></tr><tr><td colspan = 3 style="border-bottom-style: solid; border-bottom-width: 1px"><b>Totals for <%=sLastOfficer%></b></td><td style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px"><b><%=lCounterGrp1%></b></td><td style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px"><b><%=formatcurrency(sGrp1)%></b></td><td colspan=5 style="border-bottom-style: solid; border-bottom-width: 1px">&nbsp;</td></tr><%
	response.write "</table>"
	rs.close
	cn.close
	set rs = nothing
	set rsL = nothing
	set cn = nothing

end if

%> </body></html>

