<%
	'option explicit
	dim DBName, DBUser, DBPass, DBDialer, DBInventory, DBInvUser, DBInvPass
		dim tTours
		dim payTours
		dim tPayAmount
		dim totalPay
		dim tPremIssued
		dim cpremCBAmount
		dim cbAmount
		dim spiffAmount
		dim tBonusAmount
		dim tdepAmount
		dim refDepositAmount
		dim nonrefDepositAmount
%>


<%
'**********Security Check ***********************
'	on error resume next
'	if not(CheckSecurity("HRReports", "OPCPayrollBatch")) then
'		err.description = "<BR><BR><BR><BR><BR><BR><BR><BR><BR>Access Denied"
'		err.raise -1
'	end if
'	if err <> 0 then
'		response.write err.description
'		err.clear
'		cn.close
'		set cn = nothing
'		response.end
'	end if
'**********End Security Check *******************

 if request("function") = "GetPayrollPersonnel" then
 	dim cn,rs,bID,rsDetail,rsPrem, bDetail,i,x,v,dID,rsDeposit,y
 	server.scripttimeout = 10000
 	set cn = server.createobject("ADODB.Connection")
 	set rs = server.createobject("ADODB.Recordset")
 	set rsDetail = server.createobject("ADODB.Recordset")
 	set rsPrem = server.createobject("ADODB.Recordset")
 	set rsDeposit = server.createobject("ADODB.Recordset")
 	
 	bDetail = cbool(request("Detail"))
 	bID = request("bID")
 	DBName = "CRMSNet"
    DBUser = "asp"
    DBPass = "aspnet"
 	cn.open DBName, DBUser,DBPass
 	cn.commandtimeout = 26000
    if request("Excel") = "true" then
        response.ContentType = "application/vnd.ms-excel"
	    response.AddHeader "Content-Disposition", "attachment; filename=opcpayrollbatch.xls"
    end if
'			 "	inner join t_payrollprocessingpersonnel2batch tp on tp.personnelid = pd.personnelid " & _
 	rs.open "select distinct p.PersonnelID,d.comboitemid,p.lastname + ', ' + p.firstname as Personnel,d.comboitem as Department " & _
			 "	from t_personnel2dept pd " & _
			 "	inner join t_Personnel p on p.personnelid = pd.personnelid " & _
			 "	inner join t_comboitems d on d.comboitemid = pd.departmentid " & _
			 "	inner join t_payrollprocessingtour2personnel tp on tp.personnelid = pd.personnelid " & _ 	
			 "	where tp.batchid = '" & bID & "'and p.active =1 and pd.active = 1 and d.comboitem like 'OPC%' " & _
			 "	order by personnel,department",cn,3,3
 	if rs.eof and rs.bof then
 		response.write "There are no records to retrieve for this BatchID"
 	else
		response.write "<table width = '80%'>"

 		do while not rs.eof	
			response.write "<tr>"
				for i = 2 to rs.fields.count -1
					response.write "<th>" & rs.fields(i).name & "</th>"
				next
			response.write "<td><b>Payable Tours</b></td>"
			response.write "<td><b>Tour Pay</b></td>"
			response.write "<td><b>Total Pay Amount</b></td>"
			response.write "<td><b>Prem # Issued</b></td>"
			response.write "<td><b>Prem Charge Back Amount</b></td>"
			response.write "<td><b>Net Pay Amount</b></td>"						
			response.write "<td><b>Charge Back Amount</b></td>"
			response.write "<td><b>Spiff Amount</b></td>"
			response.write "<td><b>Bonus Amount</b></td>"
			response.write "<td><b>Deposit Amount</b></td>"
			response.write "<td><b>Issued Deposits</b></td>"
			response.write "<td><b>Non-Issued Deposits</b></td>"
			response.write "</tr>" 		
 			with response
				.write "<tr>"			
				for i = 2 to rs.fields.count -1
					.write "<td>" & rs.fields(i).value & "</td>"	
				next
					getPersonnelTotals rs.fields("personnelid").value, bID, rs.fields("comboitemid").value
					.write "<td>" & payTours & "</td>"
					.write "<td> " & formatCurrency(tPayAmount) & "</td>"
					
					if totalPay = "0" then 
						.write "<td> " & formatCurrency(totalPay) & "</td>"
					else 
						.write "<td>" & formatcurrency(tPayAmount * payTours) & "</td>"
					end if
					.write "<td>" & tPremIssued & "</td>"
					.write "<td> " & formatcurrency(cPremCBAmount) & "</td>"					
					.write "<td>" & formatcurrency(totalPay) & "</td>"
					'.write "<td>" & formatcurrency(totalPay + tdepAmount) & "</td>"
					.write "<td>" & formatcurrency(cbAmount) & "</td>"
					.write "<td>" & formatcurrency(spiffAmount) & "</td>"
					.write "<td> " & formatcurrency(tBonusAmount) & "</td>"
					.write "<td>" & formatcurrency(tdepAmount) & "</td>"
					.write "<td>" & formatcurrency(refDepositAmount) & "</td>"
					.write "<td>" & formatcurrency(nonrefDepositAmount) & "</td>"
					.write "</tr>"
					.write "<tr>"				

					if bDetail then
						with response
							rsDetail.open "select distinct tr.tourid,c.departmentid,t.tourdate,c.name as campaignname, s.comboitem as tourstatus, l.comboitem as Location, t.tourtime, p.Lastname + ', ' + p.Firstname as Prospect " & _  
											"from t_payrollprocessingtourpersonnelrecords tr " & _
											"inner join t_payrollprocessingtours pt on pt.tourid = tr.tourid " & _
											"inner join t_payrollprocessingtour2personnel ttp on ttp.processingid = pt.processingid " & _
											"inner join t_tour t on t.tourid = tr.tourid " & _
											"inner join t_prospect p on p.prospectid = t.prospectid " & _
											"inner join t_comboitems s on s.comboitemid = pt.statusid " & _
											"inner join t_campaign c on c.campaignid = t.campaignid " & _
											"inner join t_comboitems l on l.comboitemid = t.tourlocationid " & _
											"where ttp.batchid = '" & bID & "' and ttp.personnelid = '" & rs.fields("personnelID").value & "' and ttp.Departmentid = '" & rs.fields("comboitemid").value & "' order by t.tourdate",cn,3,3
							if rs.eof and rs.bof then
							else
								do while not rsDetail.eof
									.write "<td>&nbsp;</td>"
									for x = 0 to rsDetail.fields.count - 1
										.write "<td>" & rsDetail.fields(x).value & "</td>"
									next
									.write "</tr>"	
										rsPrem.open "select distinct pp.processingpremiumid, ps.Comboitem as Status, pp.tourid, pp.premiumID, p.PremiumName,pp.qtyissued,pp.ChargeBackCost as Cost, pp.QTYIssued * pp.ChargeBackCost as TotalCost " & _
													"from t_payrollprocessingtourpremiums pp " & _
													"inner join (select * from t_premiumissued where keyfield = 'tourid') pi on pi.keyvalue = pp.tourid " & _
													"inner join t_premium p on p.premiumid = pp.premiumid " & _
													"inner join t_comboitems ps on ps.comboitemid = pp.statusid " & _
													"where pi.keyvalue in " & _
													"( " & _
													"	select distinct(tourid) from t_payrollprocessingtours pt " & _
													"	left outer join t_payrollprocessingtour2personnel tp on tp.processingid = pt.processingid " & _
													"	where pt.batchid = '" & bID & "' and tp.departmentid = '" & rs.fields("comboitemid").value & "' and tp.personnelid = '" & rs.fields("personnelid").value & "' and pt.tourid = '" & rsDetail.fields("TourID").value & "' " & _
													") and pp.statusid in " & _
													"( " & _
													"	select comboitemid " & _
													"	from t_comboitems " & _
													"	where comboitem = 'Issued' " & _
													")  and pp.chargebackcost >= 0 and pp.batchid = '" & bID & "' and p.premiumname not like '%dep%' ",cn,3,3
										
										if rsPrem.eof and rsPrem.bof then
										else
											do while not rsPrem.eof
												.write "<tr><td>&nbsp;</td><td>&nbsp;</td>"
												for v = 1 to rsPrem.fields.count -1
													if rsPrem.fields(v).name = "Cost" or rsPrem.fields(v).name = "TotalCost" then
														.write "<td>" & formatcurrency(rsPrem.fields(v).value) & "</td>"
													else
														.write "<td>" & rsPrem.fields(v).value & "</td>"
													end if
												next
												.write "</tr>"
												rsPrem.movenext
											loop
										end if
										rsPrem.close

										rsDeposit.open "select distinct pp.processingpremiumid, ps.Comboitem as Status, pp.tourid, pp.premiumID, p.PremiumName,case when ps.comboitem in ('Issued','Prepared') then pp.qtyissued else pp.qtyassigned end as qtyissued,pp.ChargeBackCost as Cost, case when ps.comboitem in ('Issued','Prepared') then pp.qtyissued else pp.qtyassigned end * pp.ChargeBackCost as TotalCost " & _
													"from t_payrollprocessingtourpremiums pp " & _
													"inner join (select * from t_premiumissued where keyfield = 'tourid') pi on pi.keyvalue = pp.tourid " & _
													"inner join t_premium p on p.premiumid = pp.premiumid " & _
													"inner join t_comboitems ps on ps.comboitemid = pp.statusid " & _
													"where pi.keyvalue in " & _
													"( " & _
													"	select distinct(tourid) from t_payrollprocessingtours pt " & _
													"	left outer join t_payrollprocessingtour2personnel tp on tp.processingid = pt.processingid " & _
													"	where pt.batchid = '" & bID & "' and tp.departmentid = '" & rs.fields("comboitemid").value & "' and tp.personnelid = '" & rs.fields("personnelid").value & "' and pt.tourid = '" & rsDetail.fields("TourID").value & "' " & _
													") and pp.chargebackcost >=0 and pp.batchid = '" & bID & "' and p.premiumname like '%dep%'",cn,3,3										

										if rsDeposit.eof and rsDeposit.bof then
										else
											do while not rsDeposit.eof
												.write "<tr><td>&nbsp;</td><td>&nbsp;</td>"
												for y = 1 to rsDeposit.fields.count -1 
													if rsDeposit.fields(y).name = "Cost" or rsDeposit.fields(y).name = "TotalCost" then
														.write "<td>" & formatcurrency(rsDeposit.fields(y).value) & "</td>"
													else
														.write "<td>" & rsDeposit.fields(y).value & "</td>"
													end if
												next
												rsDeposit.movenext
												.write "</tr>"
											loop
										end if
										rsDeposit.close													
									rsDetail.movenext
								loop
							end if
						end with
						rsDetail.close
					end if
					.write "</tr>"								
				.write "<td style='page-break-before: always'>&nbsp;</td></tr>"
 			end with
 			rs.movenext
 		loop
 		response.write "</table>"
 		rs.close
 	end if
 else 
 end if
 
function getPersonnelTotals(pID,bID,dID)
 	dim rsTotal
 	set rsTotal = server.createobject("ADODB.Recordset")
 	
 	rsTotal.open "select * from t_payrollprocessingpaysummary where personnelid = '" & pID & "' and batchid = '" & bID & "' and departmentid = '" & dID & "'",cn,3,3
 		if rsTotal.eof and rsTotal.bof then
 			totalPay = 0
 			spiffAmount = 0
 			cbAmount = 0
 			tBonusAmount = 0
 			depAmount = 0
 			cPremCBAmount = 0
 			refDepositAmount = 0
 			nonrefDepositAmount = 0
 		else
 			if rsTotal.fields("PremiumsTotal").value & "" = "" then
 				cPremCBAmount = 0
 			else
 				cPremCBAmount = rsTotal.fields("PremiumsTotal").value
 			end if 
 			if rsTotal.fields("TourDeposits").value & "" = "" then
 				tdepAmount = 0
 			else
 				tdepAmount = rsTotal.fields("TourDeposits").value
 			end if 
	 		if rsTotal.fields("TourPay").value & "" = "" then
	 			totalPay = 0
	 		else
	 			totalPay = rsTotal.fields("TourPay").value
	 		end if
	 		if rsTotal.fields("Spiff").value & "" = "" then
	 			spiffAmount = 0
	 		else
	 			spiffAmount = rsTotal.fields("Spiff").value
	 		end if
	 		if rsTotal.fields("ChargeBackAmount").value & "" = "" then
	 			cbAmount = 0
	 		else
	 			cbAmount = rsTotal.fields("ChargeBackAmount").value
	 		end if
	 		if rsTotal.fields("Bonus").value = null or rsTotal.fields("Bonus").value & "" = "" then
	 			tBonusAmount = 0
	 		else
	 			tBonusAmount = rsTotal.fields("Bonus").value
	 		end if
	 		if rsTotal.fields("RefundableDeposits").value = null or rsTotal.fields("RefundableDeposits").value & "" = "" then
	 			refDepositAmount = 0
	 		else
	 			refDepositAmount = rsTotal.fields("RefundableDeposits").value
	 		end if
	 		if rsTotal.fields("NonRefundableDeposits").value = null or rsTotal.fields("NonRefundableDeposits").value & "" = "" then
	 			nonrefDepositAmount = 0
	 		else
	 			nonrefDepositAmount = rsTotal.fields("NonRefundableDeposits").value
	 		end if

	 	end if
 	rsTotal.close
 	
	rsTotal.open "select count(distinct tourid) as tCount from t_payrollprocessingtours where processingid in (select processingid from t_payrollprocessingtour2personnel where batchid = '" & bID & "'and personnelid = '" & pID & "' and departmentid = '" & dID & "' and payamount > 0) and batchid = '" & bID & "' ",cn,3,3
		if rsTotal.eof and rsTotal.bof then
			payTours = 0
		else
			if rsTotal.fields("tCount").value & "" = "" then
				payTours = 0
			else
				payTours =  rsTotal.fields("tCount").value
			end if
		end if
	rsTotal.close	
 	
 	rsTotal.open "select distinct PayAmount as PayAmount from t_payrollprocessingtour2personnel where personnelid = '" & pID & "' and batchid = '" & bID & "' and departmentid = '" & dID & "' and payamount > 0",cn,3,3
 		if rsTotal.eof and rsTotal.bof then
 			tPayAmount = 0
 		else
 			if rsTotal.fields("PayAmount").value & "" = "" then
 				tPayAmount = 0
 			else
 				tPayAmount = rsTotal.fields("PayAmount").value
 			end if
 		end if
 	rsTotal.close 	
 	
	rsTotal.open "select sum(QTYIssued) as tPremIssued " & _
				 "	from " & _
				 "	( " & _
				 "		select distinct pp.tourid,pp.QTYIssued,pp.premiumid " & _
				 "		from t_payrollprocessingtourpremiums pp " & _
				 "		inner join t_premium p on p.premiumid = pp.premiumid " & _
				 "		where tourid in " & _
				 "		( " & _
				 "			select distinct (tourid) from t_payrollprocessingtours pt " & _
				 "			left outer join t_payrollprocessingtour2personnel tp on tp.processingid = pt.processingid " & _
				 "			where pt.batchid = '" & bID & "' and tp.departmentid = '" & dID & "' and tp.personnelid = '" & pID & "' " & _
				 "		) and pp.statusid in " & _
				 "		( " & _
				 "			select comboitemid " & _
				 "			from t_comboitems " & _
				 "			where comboitem = 'Issued' " & _
				 "		) and p.premiumname not like 'DEP%' group by tourid, pp.qtyissued, pp.premiumid " & _
				 "	) as tPremIssued ",cn,3,3
 		if rsTotal.eof and rsTotal.bof then
 			tPremIssued = o
 		else
 			if rsTotal.fields("tPremIssued").value & "" = "" then
 				tPremIssued = 0
 			else	
 				tPremIssued = rsTotal.fields("tPremIssued").value
 			end if
 		end if
 	rsTotal.close
 	set rsTotal = nothing
end function

cn.close
set rs = nothing
set rsPrem = nothing
set rsDetail = nothing
set rsDeposit = Nothing
set cn = nothing
%>