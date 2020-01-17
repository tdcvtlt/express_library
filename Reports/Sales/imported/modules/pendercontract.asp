<% 'option explicit %>

<%
	dim DBName, DBUser, DBPass
	dim cn 
	dim rs
	dim sSQL
	dim bShowIndividuals, bShowRecords, i
	dim gtSold, gtVol, gtAvgLen, gtAvgPrice, gtAvgDwn, gtAvgCol, gtTotCol
	dim gtPend, gtPendVol, gtPendAvgLen, gtPendAvgPrice, gtPendAvgDwn, gtPendAvgCol, gtPendTotCol
	dim gtOWSold, gtOWVol, gtOWAvgLen, gtOWAvgPrice, gtOWAvgDwn, gtOWAvgCol, gtOWTotCol
	dim gtCanSold, gtCanVol, gtCanAvgLen, gtCanAvgPrice, gtCanAvgDwn, gtCanAvgCol, gtCanTotCol
	dim gtResSold, gtResVol, gtResAvgLen, gtResAvgPrice, gtResAvgDwn, gtResAvgCol, gtResTotCol
	dim gtOtherSold, gtOtherVol, gtOtherAvgLen, gtOtherAvgPrice, gtOtherAvgDwn, gtOtherAvgCol, gtOtherTotCol
	
	dim ptSold, ptVol, ptAvgLen, ptAvgPrice, ptAvgDwn, ptAvgCol, ptTotCol
	dim ptPend, ptPendVol, ptPendAvgLen, ptPendAvgPrice, ptPendAvgDwn, ptPendAvgCol, ptPendTotCol
	dim ptOWSold, ptOWVol, ptOWAvgLen, ptOWAvgPrice, ptOWAvgDwn, ptOWAvgCol, ptOWTotCol
	dim ptCanSold, ptCanVol, ptCanAvgLen, ptCanAvgPrice, ptCanAvgDwn, ptCanAvgCol, ptCanTotCol
	dim ptResSold, ptResVol, ptResAvgLen, ptResAvgPrice, ptResAvgDwn, ptResAvgCol, ptResTotCol
	dim ptOtherSold, ptOtherVol, ptOtherAvgLen, ptOtherAvgPrice, ptOtherAvgDwn, ptOtherAvgCol, ptOtherTotCol
	
	dim ptOther()
	dim gtOther()
	dim aptOther(), agtOther(), bFound, divCounter
	
	divCounter = 0

	redim aptOther(1)
	redim agtOther(1)
	redim ptOther(1)
	redim gtOther(1)
		
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	

	DBName = "CRMSNet"
    DBUser = "asp"
    DBPass="aspnet"
        
	cn.commandtimeout = 10000
	'server.requesttimeout = 0
	cn.open DBName, DBUser, DBPass
	
	if request("Function") = "Get_Personnel" then
		sSQL = "Select distinct personnelid, lastname + ', ' + firstname as personnel " & _
		"from t_Personnel " & _
		"where personnelid in ( " & _ 
			"select personnelid " & _
			"from t_Personneltrans t " & _
			"inner join t_Contract c on c.contractid = t.KEYVALUE AND T.KEYFIELD = 'CONTRACTID' " & _
			"where c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and (comboitem='Pender' or comboitem = 'Pender-Inv'))" & _
			"or c.contractid in ( " & _
					"select E.KEYVALUE from t_Event E where E.KEYVALUE > 0 AND E.KEYFIELD = 'CONTRACTID' and (oldvalue = 'pender' or oldvalue = 'Pender-inv') " & _
					") " & _
			") " & _
			"order by lastname + ', ' + firstname"
		rs.OPen sSQL, cn, 0, 1
		dim sAns
		sAns = "OK"
		Do while not rs.EOF
			sAns = sAns & "||" & rs.fields("PersonnelID").value & "|" & rs.fields("Personnel").value 
			rs.MoveNext
		Loop
		rs.Close
		response.write sAns
	else
		bShowIndividuals = request("ShowIndividuals")
		if bShowIndividuals = "" then
			bShowIndividuals = true
		end if
		
		bShowRecords= request("ShowRecords")
		if bShowRecords= "" then
			bShowRecords= true
		end if	
		
		sSQL = "select per.personnelid,cs.comboitem as status, per.lastname, per.firstname, c.contractid, m.salesvolume,m.salesprice, c.contractdate, c.statusdate, ( " & _
				"select sum(amount) from V_INVOICES V where V.KEYFIELD = 'MORTGAGEDP' AND V.KEYVALUE = m.mortgageid and V.TRANSDATE = c.contractdate " & _
				") as AmountCollected, m.DPTotal, ( " & _
				"select sum(amount) from V_INVOICES V2 where V2.KEYFIELD = 'MORGAGEDP' AND V2.KEYVALUE = m.mortgageid " & _
				") as TotalCollected " & _
			"from t_Personnel per  " & _
				"inner join t_Personneltrans t on t.personnelid = per.personnelid " & _
				"inner join t_Contract c on c.contractid = t.KEYVALUE AND T.KEYFIELD = 'contractid' " & _
				"inner join t_Mortgage m on m.contractid = c.contractid " & _
				"left outer join t_Comboitems cs on cs.comboitemid = c.statusid " & _
			"where c.contractdate between '" & request("sDate") & "' and '" & request("eDate") & "' " & _
				"and c.contractnumber not like 't%' and c.contractnumber not like 'u%' "
		if request("loc") = "Williamsburg" then
			sSQL = sSQL & "and c.contractnumber not like '%S%' " 
		elseif request("loc") = "Richmond" then
			sSQL = sSQL & "and c.contractnumber like '%S%' " 
		end if
		sSQL = sSQL & "and ( " & _
				"cs.comboitem in ('" & request("Status") & "') " & _  
				"or c.contractid in ( " & _
						"select E.KEYVALUE from t_Event E where E.KEYVALUE > 0 AND E.KEYFIELD = 'contractid' and oldvalue = '" & request("Status") & "' " & _
						") " & _
		    	")	 " & _
				"and t.titleid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'personneltitle' " 
		if request("title") & "" <> "" then
			if request("title") = 0 then
				sSQL = sSQL & " and comboitem = 'sales executive')  "
			else
				sSQL = sSQL & " and comboitemid = '" & request("Title") & "') "
			end if
		else
			sSQL = sSQL & " and Comboitem = 'sales executive') "
		end if
		
		sSQL = sSQL & "and (t.percentage > 0 or t.fixedamount > 0 ) " 
		
		if request("rep") <> "0" then
			sSQL = sSQL & "and per.personnelid = '" & request("rep") & "' "
		end if
		
		if request("Range") & "" <> "" then
			select case request("Range")
				case "0"
				
				case "8999"
					sSQL = sSQL & " and m.salesprice < 9000 "
				case "9000-12999"
					sSQL = sSQL & " and m.salesprice >= 9000 and m.salesprice < 13000 "
				case "13000-16999"
					sSQL = sSQL & " and m.salesprice < 17000 and m.salesprice >= 13000 "
				case "17000"
					sSQL = sSQL & " and m.salesprice >= 17000 "
			end select	
		end if
		
		sSQL = sSQL & "order by per.lastname,per.firstname, c.contractid "
	
		'response.write sSQL
		rs.open sSQL, cn, 0, 1	
	
		dim lastpersonnelid, lastname, firstname
		lastpersonnelid = 0
		
		if bShowRecords then Write_Start
		
		do while not rs.eof 
			if lastpersonnelid <> rs.fields("PersonnelID").value then 
				if lastpersonnelid <> 0 then
					if bShowRecords then Write_End
					if bShowIndividuals then Write_Total 
					if bShowRecords then Write_Start
				end if
				lastpersonnelid = rs.fields("PersonnelID").value
				Clear_PTs
				lastname = rs.fields("LastName").value & ""
				firstname = rs.fields("FirstName").value & ""
			end if
			if bShowRecords then Write_Line
			ptSold = ptSold +1 
			ptVol= ptVol + rs.fields("salesvolume").value
			ptAvgLen= 0 'ptAvgLen + (rs.fields("StatusDate").value - rs.fields("ContractDate").value)
			ptAvgPrice= ptAvgPrice + rs.fields("SalesPrice").value 
			ptAvgDwn= ptAvgDwn + rs.fields("DPTotal").value 
			if rs.fields("AmountCollected").value & "" <> "" then ptAvgCol= ptAvgCol + rs.fields("AmountCollected").value
			if rs.fields("TotalCollected").value & "" <> "" then ptTotCol= ptTotCol + rs.fields("TotalCollected").value
			
			gtSold = gtSold + 1
			gtVol= gtVol + rs.fields("salesvolume").value
			gtAvgLen= 0 'gtAvgLen + (rs.fields("StatusDate").value - rs.fields("ContractDate").value)
			gtAvgPrice= gtAvgPrice + rs.fields("SalesPrice").value 
			gtAvgDwn= gtAvgDwn + rs.fields("DPTotal").value 
			if rs.fields("AmountCollected").value & "" <> "" then gtAvgCol= gtAvgCol + rs.fields("AmountCollected").value
			if rs.fields("TotalCollected").value & "" <> "" then gtTotCol= gtTotCol + rs.fields("TotalCollected").value
	
			select case ucase(trim(rs.fields("Status").value & ""))
				case "ACTIVE", "SUSPENSE"
					ptOWSold = ptOWSold + 1
					ptOWVol = ptOWVol + rs.fields("salesvolume").value
					ptOWAvgLen = ptOWAvgLen + (rs.fields("StatusDate").value - rs.fields("ContractDate").value)
					ptOWAvgPrice = ptOWAvgPrice + rs.fields("SalesPrice").value 
					ptOWAvgDwn = ptOWAvgDwn + rs.fields("DPTotal").value 
					if rs.fields("AmountCollected").value & "" <> "" then ptOWAvgCol = ptOWAvgCol + rs.fields("AmountCollected").value
					if rs.fields("TotalCollected").value & "" <> "" then ptOWTotCol = ptOWTotCol + rs.fields("TotalCollected").value
					
					gtOWSold = gtOWSold + 1
					gtOWVol = gtOWVol + rs.fields("salesvolume").value
					gtOWAvgLen = gtOWAvgLen + (rs.fields("StatusDate").value - rs.fields("ContractDate").value)
					gtOWAvgPrice = gtOWAvgPrice + rs.fields("SalesPrice").value 
					gtOWAvgDwn = gtOWAvgDwn + rs.fields("DPTotal").value 
					if rs.fields("AmountCollected").value & "" <> "" then gtOWAvgCol = gtOWAvgCol + rs.fields("AmountCollected").value
					if rs.fields("TotalCollected").value & "" <> "" then gtOWTotCol = gtOWTotCol + rs.fields("TotalCollected").value
								
				case "PENDER", "PENDER-INV"
					ptPend= ptPend + 1
					ptPendVol= ptPendVol + rs.fields("salesvolume").value
					if request("Status") = "Pender" or request("Status") = "Pender-Inv" then
						ptPendAvgLen= 0 
					else
						ptPendAvgLen = ptPendAvgLen + (rs.fields("StatusDate").value - rs.fields("ContractDate").value)
					end if
					ptPendAvgPrice= ptPendAvgPrice + rs.fields("SalesPrice").value 
					ptPendAvgDwn= ptPendAvgDwn + rs.fields("DPTotal").value 
					if rs.fields("AmountCollected").value & "" <> "" then ptPendAvgCol= ptPendAvgCol + rs.fields("AmountCollected").value
					if rs.fields("TotalCollected").value & "" <> "" then ptPendTotCol= ptPendTotCol + rs.fields("TotalCollected").value
					
					gtPend= gtPend+ 1
					gtPendVol= gtPendVol+ rs.fields("salesvolume").value
					if request("Status") = "Pender" or request("Status") = "Pender-Inv" then
						gtPendAvgLen= 0 
					else
						gtPendAvgLen = gtPendAvgLen+ (rs.fields("StatusDate").value - rs.fields("ContractDate").value)
					end if
					gtPendAvgPrice= gtPendAvgPrice+ rs.fields("SalesPrice").value 
					gtPendAvgDwn= gtPendAvgDwn+ rs.fields("DPTotal").value 
					if rs.fields("AmountCollected").value & "" <> "" then gtPendAvgCol= gtPendAvgCol+ rs.fields("AmountCollected").value
					if rs.fields("TotalCollected").value & "" <> "" then gtPendTotCol= gtPendTotCol+ rs.fields("TotalCollected").value
	
				case "CXL-PENDER", "CANCELED"
					ptCanSold= ptCanSold + 1
					ptCanVol= ptCanVol + rs.fields("salesvolume").value
					ptCanAvgLen= ptCanAvgLen + (rs.fields("StatusDate").value - rs.fields("ContractDate").value)
					ptCanAvgPrice= ptCanAvgPrice + rs.fields("SalesPrice").value 
					ptCanAvgDwn= ptCanAvgDwn + rs.fields("DPTotal").value 
					if rs.fields("AmountCollected").value & "" <> "" then ptCanAvgCol= ptCanAvgCol + rs.fields("AmountCollected").value
					if rs.fields("TotalCollected").value & "" <> "" then ptCanTotCol= ptCanTotCol + rs.fields("TotalCollected").value
					
					gtCanSold= gtCanSold + 1
					gtCanVol= gtCanVol + rs.fields("salesvolume").value
					gtCanAvgLen= gtCanAvgLen + (rs.fields("StatusDate").value - rs.fields("ContractDate").value)
					gtCanAvgPrice= gtCanAvgPrice + rs.fields("SalesPrice").value 
					gtCanAvgDwn= gtCanAvgDwn + rs.fields("DPTotal").value 
					if rs.fields("AmountCollected").value & "" <> "" then gtCanAvgCol= gtCanAvgCol + rs.fields("AmountCollected").value
					if rs.fields("TotalCollected").value & "" <> "" then gtCanTotCol= gtCanTotCol + rs.fields("TotalCollected").value
				
				case "RES-PENDER", "RESCINDED"
					ptResSold= ptResSold + 1
					ptResVol= ptResVol + rs.fields("salesvolume").value
					ptResAvgLen= ptResAvgLen + (rs.fields("StatusDate").value - rs.fields("ContractDate").value)
					ptResAvgPrice= ptResAvgPrice + rs.fields("SalesPrice").value 
					ptResAvgDwn= ptResAvgDwn + rs.fields("DPTotal").value 
					if rs.fields("AmountCollected").value & "" <> "" then ptResAvgCol= ptResAvgCol + rs.fields("AmountCollected").value
					if rs.fields("TotalCollected").value & "" <> "" then ptResTotCol= ptResTotCol + rs.fields("TotalCollected").value
					
					gtResSold= gtResSold + 1
					gtResVol= gtResVol + rs.fields("salesvolume").value
					gtResAvgLen= gtResAvgLen + (rs.fields("StatusDate").value - rs.fields("ContractDate").value)
					gtResAvgPrice= gtResAvgPrice + rs.fields("SalesPrice").value 
					gtResAvgDwn= gtResAvgDwn + rs.fields("DPTotal").value 
					if rs.fields("AmountCollected").value & "" <> "" then gtResAvgCol= gtResAvgCol + rs.fields("AmountCollected").value
					if rs.fields("TotalCollected").value & "" <> "" then gtResTotCol= gtResTotCol + rs.fields("TotalCollected").value
				
				case else
					ptOtherSold= ptOtherSold + 1
					ptOtherVol= ptOtherVol + rs.fields("salesvolume").value
					ptOtherAvgLen= ptOtherAvgLen + (rs.fields("StatusDate").value - rs.fields("ContractDate").value)
					ptOtherAvgPrice= ptOtherAvgPrice + rs.fields("SalesPrice").value 
					ptOtherAvgDwn = ptOtherAvgDwn + rs.fields("DPTotal").value 
					if rs.fields("AmountCollected").value & "" <> "" then ptOtherAvgCol= ptOtherAvgCol + rs.fields("AmountCollected").value
					if rs.fields("TotalCollected").value & "" <> "" then ptOtherTotCol= ptOtherTotCol + rs.fields("TotalCollected").value
					
					gtOtherSold= gtOtherSold + 1
					gtOtherVol= gtOtherVol + rs.fields("salesvolume").value
					gtOtherAvgLen= gtOtherAvgLen + (rs.fields("StatusDate").value - rs.fields("ContractDate").value)
					gtOtherAvgPrice= gtOtherAvgPrice + rs.fields("SalesPrice").value 
					gtOtherAvgDwn= gtOtherAvgDwn + rs.fields("DPTotal").value 
					if rs.fields("AmountCollected").value & "" <> "" then gtOtherAvgCol= gtOtherAvgCol + rs.fields("AmountCollected").value
					if rs.fields("TotalCollected").value & "" <> "" then gtOtherTotCol= gtOtherTotCol + rs.fields("TotalCollected").value
					bFound = false
					for i = 0 to ubound(aptOther)
						if aptOther(i) = rs.fields("Status").value then
							ptOther(i) = ptOther(i) + 1
							bFound = true
							exit for
						end if
					next
					if not bFound then
						if ubound(aptOther) <> "" then
							redim preserve aptOther(ubound(aptOther)+1)
							redim preserve ptOther(ubound(ptOther)+1)
						end if
						aptOther(ubound(aptOther)) = rs.fields("Status").value	
						ptOther(ubound(aptOther)) = 1
					end if
					bFound = false
					for i = 0 to ubound(agtOther)
						if agtOther(i) = rs.fields("Status").value then
							gtOther(i) = gtOther(i) + 1
							bFound = true
							exit for
						end if
					next
					if not bFound then
						if ubound(agtOther) <> "" then
							redim preserve agtOther(ubound(agtOther)+1)
							redim preserve gtOther(ubound(gtOther)+1)
						end if
						agtOther(ubound(agtOther)) = rs.fields("Status").value	
						gtOther(ubound(agtOther)) = 1
					end if
			end select
			rs.movenext
		loop
		if bShowIndividuals then Write_Total
		lastname = "Grand Total"
		firstname = ""
		Write_Total
		rs.close
	end if
	cn.close
	set rs = nothing
	set cn = nothing
		
		
	function Clear_PTs
		ptSold=0 
		ptVol=0 
		ptAvgLen=0 
		ptAvgPrice=0 
		ptAvgDwn=0 
		ptAvgCol=0 
		ptTotCol=0
		ptPend=0 
		ptPendVol=0 
		ptPendAvgLen=0 
		ptPendAvgPrice=0 
		ptPendAvgDwn=0 
		ptPendAvgCol=0 
		ptPendTotCol=0
		ptOWSold=0 
		ptOWVol=0 
		ptOWAvgLen=0 
		ptOWAvgPrice=0 
		ptOWAvgDwn=0 
		ptOWAvgCol=0 
		ptOWTotCol=0
		ptCanSold=0 
		ptCanVol=0 
		ptCanAvgLen=0 
		ptCanAvgPrice=0 
		ptCanAvgDwn=0 
		ptCanAvgCol=0 
		ptCanTotCol=0
		ptResSold=0 
		ptResVol=0 
		ptResAvgLen=0 
		ptResAvgPrice=0 
		ptResAvgDwn=0 
		ptResAvgCol=0 
		ptResTotCol=0
		ptOtherSold=0 
		ptOtherVol=0 
		ptOtherAvgLen=0
		ptOtherAvgPrice=0 
		ptOtherAvgDwn=0 
		ptOtherAvgCol=0 
		ptOtherTotCol=0
		redim aptOther(1)
		redim ptOther(1)
	end function
	
	function Set_Values
		if lastname<>"Grand Total" then
			Sold = ptSold
			if Sold = "" then Sold = 0
			Vol = ptVol
			if Vol = "" then Vol = 0
			if Sold > 0 then
				AvgLen = ptAvgLen/Sold
				AvgPrice = ptAvgPrice/Sold
				AvgDown = ptAvgDwn/Sold
				AvgCol = ptTotCol/Sold 'ptAvgCol/Sold
				TotCol = ptTotCol'/Sold
			else
				AvgLen = 0
				AvgPrice=0
				AvgDown=0
				AvgCol=0
				TotCol=0
			end if	
			
			pSold = ptPend
			if pSold = "" then pSold = 0
			pVol = ptPendVol
			if pVol = "" then pVol = 0
			if pSold > 0 then
				pAvgLen = ptPendAvgLen/pSold
				pAvgPrice = ptPendAvgPrice/pSold
				pAvgDown = ptPendAvgDwn/pSold
				pAvgCol = ptPendTotCol/pSold
				pTotCol = ptPendTotCol'/pSold
			else
				pAvgLen = 0
				pAvgPrice=0
				pAvgDown=0
				pAvgCol=0
				pTotCol=0
			end if	
			
			owSold = ptOWSold
			if owSold = "" then owSold = 0
			owVol = ptOWVol
			if owVol = "" then owVol = 0
			if owSold > 0 then
				owAvgLen = ptOWAvgLen/owSold
				owAvgPrice = ptOWAvgPrice/owSold
				owAvgDown = ptOWAvgDwn/owSold
				owAvgCol = ptOWAvgCol/owSold
				owTotCol = ptOWTotCol '/owSold
			else
				owAvgLen = 0
				owAvgPrice=0
				owAvgDown=0
				owAvgCol=0
				owTotCol=0
			end if	
			
			cSold = ptCanSold
			if cSold = "" then cSold = 0
			cVol = ptCanVol
			if cVol = "" then cVol = 0
			if cSold > 0 then
				cAvgLen = ptCanAvgLen/cSold
				cAvgPrice = ptCanAvgPrice/cSold
				cAvgDown = ptCanAvgDwn/cSold
				cAvgCol = ptCanTotCol/cSold
				cTotCol = ptCanTotCol'/cSold
			else
				cAvgLen = 0
				cAvgPrice=0
				cAvgDown=0
				cAvgCol=0
				cTotCol=0
			end if	
			
			rSold = ptResSold
			if rSold = "" then rSold = 0
			rVol = ptResVol
			if rVol = "" then rVol = 0
			if rSold > 0 then
				rAvgLen = ptResAvgLen/rSold
				rAvgPrice = ptResAvgPrice/rSold
				rAvgDown = ptResAvgDwn/rSold
				rAvgCol = ptResTotCol/rSold
				rTotCol = ptResTotCol'/rSold
			else
				rAvgLen = 0
				rAvgPrice=0
				rAvgDown=0
				rAvgCol=0
				rTotCol=0
			end if	
			
			oSold = ptOtherSold
			if oSold = "" then oSold = 0
			oVol = ptOtherVol
			if oVol = "" then oVol = 0
			if oSold > 0 then
				oAvgLen = ptOtherAvgLen/oSold
				oAvgPrice = ptOtherAvgPrice/oSold
				oAvgDown = ptOtherAvgDwn/oSold
				oAvgCol = ptOtherTotCol/oSold
				oTotCol = ptOtherTotCol'/oSold
			else
				oAvgLen = 0
				oAvgPrice=0
				oAvgDown=0
				oAvgCol=0
				oTotCol=0
			end if	
			
		else
			Sold = gtSold
			if Sold = "" then Sold = 0
			Vol = gtVol
			if Vol = "" then Vol = 0
			if Sold > 0 then
				AvgLen = gtAvgLen/Sold
				AvgPrice = gtAvgPrice/Sold
				AvgDown = gtAvgDwn/Sold
				AvgCol = gtTotCol/Sold
				TotCol = gtTotCol'/Sold
			else
				AvgLen = 0
				AvgPrice=0
				AvgDown=0
				AvgCol=0
				TotCol=0
			end if	
			
			pSold = gtPend
			if pSold = "" then pSold = 0
			pVol = gtPendVol
			if pVol = "" then pVol = 0
			if pSold > 0 then
				pAvgLen = gtPendAvgLen/pSold
				pAvgPrice = gtPendAvgPrice/pSold
				pAvgDown = gtPendAvgDwn/pSold
				pAvgCol = gtPendTotCol/pSold
				pTotCol = gtPendTotCol'/pSold
			else
				pAvgLen = 0
				pAvgPrice=0
				pAvgDown=0
				pAvgCol=0
				pTotCol=0
			end if	
			
			owSold = gtOWSold
			if owSold = "" then owSold = 0
			owVol = gtOWVol
			if owVol = "" then owVol = 0
			if owSold > 0 then
				owAvgLen = gtOWAvgLen/owSold
				owAvgPrice = gtOWAvgPrice/owSold
				owAvgDown = gtOWAvgDwn/owSold
				owAvgCol = gtOWTotCol/owSold
				owTotCol = gtOWTotCol'/owSold
			else
				owAvgLen = 0
				owAvgPrice=0
				owAvgDown=0
				owAvgCol=0
				owTotCol=0
			end if	
			
			cSold = gtCanSold
			if cSold = "" then cSold = 0
			cVol = gtCanVol
			if cVol = "" then cVol = 0
			if cSold > 0 then
				cAvgLen = gtCanAvgLen/cSold
				cAvgPrice = gtCanAvgPrice/cSold
				cAvgDown = gtCanAvgDwn/cSold
				cAvgCol = gtCanTotCol/cSold
				cTotCol = gtCanTotCol'/cSold
			else
				cAvgLen = 0
				cAvgPrice=0
				cAvgDown=0
				cAvgCol=0
				cTotCol=0
			end if	
			
			rSold = gtResSold
			if rSold = "" then rSold = 0
			rVol = gtResVol
			if rVol = "" then rVol = 0
			if rSold > 0 then
				rAvgLen = gtResAvgLen/rSold
				rAvgPrice = gtResAvgPrice/rSold
				rAvgDown = gtResAvgDwn/rSold
				rAvgCol = gtResTotCol/rSold
				rTotCol = gtResTotCol'/rSold
			else
				rAvgLen = 0
				rAvgPrice=0
				rAvgDown=0
				rAvgCol=0
				rTotCol=0
			end if	
			
			oSold = gtOtherSold
			if oSold = "" then oSold = 0
			oVol = gtOtherVol
			if oVol = "" then oVol = 0
			if oSold > 0 then
				oAvgLen = gtOtherAvgLen/oSold
				oAvgPrice = gtOtherAvgPrice/oSold
				oAvgDown = gtOtherAvgDwn/oSold
				oAvgCol = gtOtherTotCol/oSold
				oTotCol = gtOtherTotCol'/oSold
			else
				oAvgLen = 0
				oAvgPrice=0
				oAvgDown=0
				oAvgCol=0
				oTotCol=0
			end if	
		end if
		
		if AvgLen & "" = "" then AvgLen = 0
		if AvgPrice & "" = "" then AvgPrice = 0
		if AvgDown & "" = "" then AvgDown = 0
		if AvgCol & "" = "" then AvgCol = 0
		if TotCol & "" = "" then TotCol = 0
		
		if pAvgLen & "" = "" then pAvgLen = 0
		if pAvgPrice & "" = "" then pAvgPrice = 0
		if pAvgDown & "" = "" then pAvgDown = 0
		if pAvgCol & "" = "" then pAvgCol = 0
		if pTotCol & "" = "" then pTotCol = 0
		
		if owAvgLen & "" = "" then owAvgLen = 0
		if owAvgPrice & "" = "" then owAvgPrice = 0
		if owAvgDown & "" = "" then owAvgDown = 0
		if owAvgCol & "" = "" then owAvgCol = 0
		if owTotCol & "" = "" then owTotCol = 0
		
		if cAvgLen & "" = "" then cAvgLen = 0
		if cAvgPrice & "" = "" then cAvgPrice = 0
		if cAvgDown & "" = "" then cAvgDown = 0
		if cAvgCol & "" = "" then cAvgCol = 0
		if cTotCol & "" = "" then cTotCol = 0
		
		if rAvgLen & "" = "" then rAvgLen = 0
		if rAvgPrice & "" = "" then rAvgPrice = 0
		if rAvgDown & "" = "" then rAvgDown = 0
		if rAvgCol & "" = "" then rAvgCol = 0
		if rTotCol & "" = "" then rTotCol = 0
		
		if oAvgLen & "" = "" then oAvgLen = 0
		if oAvgPrice & "" = "" then oAvgPrice = 0
		if oAvgDown & "" = "" then oAvgDown = 0
		if oAvgCol & "" = "" then oAvgCol = 0
		if oTotCol & "" = "" then oTotCol = 0
		
		AvgLen = round(AvgLen) & " Days"
		AvgPrice = formatcurrency(AvgPrice)
		AvgDown = formatcurrency(AvgDown)
		AvgCol = formatcurrency(AvgCol)
		TotCol = formatcurrency(TotCol)
		Vol = formatcurrency(Vol)
		
		pAvgLen = round(pAvgLen) & " Days"
		pAvgPrice = formatcurrency(pAvgPrice)
		pAvgDown = formatcurrency(pAvgDown)
		pAvgCol = formatcurrency(pAvgCol)
		pTotCol = formatcurrency(pTotCol)
		pVol = formatcurrency(pVol)
		
		owAvgLen = round(owAvgLen) & " Days"
		owAvgPrice = formatcurrency(owAvgPrice)
		owAvgDown = formatcurrency(owAvgDown)
		owAvgCol = formatcurrency(owAvgCol)
		owTotCol = formatcurrency(owTotCol)
		owVol = formatcurrency(owVol)
		
		cAvgLen = round(cAvgLen) & " Days"
		cAvgPrice = formatcurrency(cAvgPrice)
		cAvgDown = formatcurrency(cAvgDown)
		cAvgCol = formatcurrency(cAvgCol)
		cTotCol = formatcurrency(cTotCol)
		cVol = formatcurrency(cVol)
		
		rAvgLen = round(rAvgLen) & " Days"
		rAvgPrice = formatcurrency(rAvgPrice)
		rAvgDown = formatcurrency(rAvgDown)
		rAvgCol = formatcurrency(rAvgCol)
		rTotCol = formatcurrency(rTotCol)
		rVol = formatcurrency(rVol)
		
		oAvgLen = round(oAvgLen) & " Days"
		oAvgPrice = formatcurrency(oAvgPrice)
		oAvgDown = formatcurrency(oAvgDown)
		oAvgCol = formatcurrency(oAvgCol)
		oTotCol = formatcurrency(oTotCol)
		oVol = formatcurrency(oVol)
		
	end function
dim Sold,Vol,AvgLen,AvgPrice,AvgDown,AvgCol,TotCol
dim pSold, pVol,pAvgLen,pAvgPrice,pAvgDown,pAvgCol,pTotCol
dim owSold, owVol,owAvgLen,owAvgPrice,owAvgDown,owAvgCol,owTotCol
dim cSold,cVol,cAvgLen,cAvgPrice,cAvgDown,cAvgCol,cTotCol
dim rSold,rVol,rAvgLen,rAvgPrice,rAvgDown,rAvgCol,rTotCol
dim oSold,oVol,oAvgLen,oAvgPrice,oAvgDown,oAvgCol,oTotCol
	function Write_Total
		Set_Values
%>

<table border="1" id="table1">
	<tr>
		<td colspan="9"><b><%if lastname <> "Grand Total" then: response.write lastname & ", " & firstname:else: response.write lastname:end if%></b></td>
	</tr>
	<tr>
		<td align="center">&nbsp;</td>
		<td align="center"><b>Num.</b></td>
		<td align="center"><b>Vol.</b></td>
		<td align="center"><b>%</b></td>
		<td align="center"><b>Avg. Length</b></td>
		<td align="center"><b>Avg. Sales</b></td>
		<td align="center"><b>Avg. Down</b></td>
		<td align="center"><b>Avg. Collected</b></td>
		<td align="center"><b>Total Collected</b></td>
	</tr>
	<tr>
		<td><b>Total</b></td>
		<td align="right"><%=Sold%></td>
		<td align="right"><%=Vol%></td>
		<td align="right">&nbsp;</td>
		<td align="right"><%=AvgLen%></td>
		<td align="right"><%=AvgPrice%></td>
		<td align="right"><%=AvgDown%></td>
		<td align="right"><%=AvgCol%></td>
		<td align="right"><%=TotCol%></td>
	</tr>
	<tr>
		<td><b>Pending</b></td>
		<td align="right"><%=pSold%></td>
		<td align="right"><%=pVol%></td>
		<td align="right"><%if Sold > 0 then: response.write formatpercent(pSold/Sold):else:response.write formatpercent(0):end if%></td>
		<td align="right"><%=pAvgLen%></td>
		<td align="right"><%=pAvgPrice%></td>
		<td align="right"><%=pAvgDown%></td>
		<td align="right"><%=pAvgCol%></td>
		<td align="right"><%=pTotCol%></td>
	</tr>
	<tr>
		<td><b>OW</b></td>
		<td align="right"><%=owSold%></td>
		<td align="right"><%=owVol%></td>
		<td align="right"><%if Sold > 0 then: response.write formatpercent(owSold/Sold):else:response.write formatpercent(0):end if%></td>
		<td align="right"><%=owAvgLen%></td>
		<td align="right"><%=owAvgPrice%></td>
		<td align="right"><%=owAvgDown%></td>
		<td align="right"><%=owAvgCol%></td>
		<td align="right"><%=owTotCol%></td>	</tr>
	<tr>
		<td><b>CXL</b></td>
		<td align="right"><%=cSold%></td>
		<td align="right"><%=cVol%></td>
		<td align="right"><%if Sold > 0 then: response.write formatpercent(cSold/Sold):else:response.write formatpercent(0):end if%></td>
		<td align="right"><%=cAvgLen%></td>
		<td align="right"><%=cAvgPrice%></td>
		<td align="right"><%=cAvgDown%></td>
		<td align="right"><%=cAvgCol%></td>
		<td align="right"><%=cTotCol%></td>
	</tr>
	<tr>
		<td><b>Rescind</b></td>
		<td align="right"><%=rSold%></td>
		<td align="right"><%=rVol%></td>
		<td align="right"><%if Sold > 0 then: response.write formatpercent(rSold/Sold):else:response.write formatpercent(0):end if%></td>
		<td align="right"><%=rAvgLen%></td>
		<td align="right"><%=rAvgPrice%></td>
		<td align="right"><%=rAvgDown%></td>
		<td align="right"><%=rAvgCol%></td>
		<td align="right"><%=rTotCol%></td>
	</tr>
	<tr>
		<td><b>Other</b></td>
		<td align="right">
<%on error resume next%>
			<%if oSold > 0 then%>
				<%if lastname <> "Grand Total" then%>
					<a href = "javascript:void(Show_Others('<%if isArray(aptOther) then response.write Join(aptOther,"|")%>', '<%if isArray(ptOther) then response.write Join(ptOther,"|")%>','oSold<%=divCounter%>'));"><%=oSold%></a>
				<%else%>
					
					<a href = "javascript:void(Show_Others('<%if isArray(agtOther) then response.write Join(agtOther,"|")%>', '<%if isArray(gtOther) then response.write Join(gtOther,"|")%>','oSold<%=divCounter%>'));"><%=oSold%></a>
				<%end if%>
			<%else%>
				<%=oSold%>
			<%end if%>
		</td>
		<td align="right"><%=oVol%></td>
		<td align="right"><%if Sold > 0 then: response.write formatpercent(oSold/Sold):else:response.write formatpercent(0):end if%></td>
		<td align="right"><%=oAvgLen%></td>
		<td align="right"><%=oAvgPrice%></td>
		<td align="right"><%=oAvgDown%></td>
		<td align="right"><%=oAvgCol%></td>
		<td align="right"><%=oTotCol%></td>
	</tr>
</table>
    <%if oSold>0 then%>
    	<div id='oSold<%=divCounter%>' style='display:none'>
			<%
				if lastname <> "Grand Total" then
					Write_Others_Table aptOther, ptOther
				else
					Write_Others_Table agtOther, gtOther
				end if
			%>
		</div>
    	<%divCounter=divCounter+1%>
    <%end if%>
<p><br>
&nbsp;</p>

    
<%
	end function
	
	function Write_Start
		response.write "<table border=1><tr>"
		for i = 0 to rs.fields.count -1 
			response.write "<td>" & rs.fields(i).name & "</td>"
		next
		response.write "</tr>"
	end function
	
	function Write_Line
		response.write "<tr>"
		for i = 0 to rs.fields.count -1 
			response.write "<td>" & rs.fields(i).value & "&nbsp;" & "</td>"
		next
		response.write "</tr>"
	end function
	
	function Write_End
		response.write "</table><br>"
	end function
	
	function Write_Others_Table(aStatus,aValue)
%>
		<table  border=1 width="191">
		<tr>
			<td colspan=2 align=center><b>Other Breakout</b></td>
		</tr>
		<tr>
			<td width="100" align="left"><b>Status</b></td><td align="right"><b>Num.</b></td>
		</tr>
<%		
		for i = 0 to ubound(aStatus)
			if aValue(i) <> "" then
%>
				<tr>
					<td align="left"><b><%=aStatus(i)%></b></td>
					<td align="right"><%=aValue(i)%></td>
				</tr>
<%
			end if
		next
%>
		</table>
<%
	end function
%>