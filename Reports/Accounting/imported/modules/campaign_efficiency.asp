
<%
	dim cn, rs, sdate, edate, arrPrems, arrLocs(), rowms, colms, rowp, colp, rowc, colc, rowt, colt, rowe, cole
	
	
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	
	DBName = "CRMSNet"
    DBUser = "asp"
    DBPass="aspnet"
	cn.open DBName, DBUser, DBPass
	
	server.scripttimeout = 10000
	
	
	if request("f") = "report" then
		response.write "Start Time: " & now & "<br />"
		
		'****** Create Excel Doc ******
		dim xlApp
		sdate = request("sdate")
		edate = request("edate")
		
		slink = Replace(Replace("Campaign_Efficiency-" & Session.SessionID & "-" & Date & "-" & Time & ".xls", "/", "-"), ":", "-")
	    sFilename = "C:\inetpub\wwwroot\crms\reports\contracts\" & slink
	    
		Set xlApp = server.createobject("Excel.Application")
	    Set wb = xlApp.Workbooks.open("c:\inetpub\wwwroot\crms\reports\contracts\campaign_template2.xls")
	    wb.SaveAs sFilename
	    Set wb = xlApp.Workbooks.Open(sFilename)
	    
	    '******* Set to Sheet 1 ******
	    Set wsActiveSheet = wb.Worksheets(1)
	    
	    dim row, col
	    col = 1
	    row = 2
	    rowms = 4
		colms = 1
		rowp = 2
		rowc = 5
		rowt = 5
		rowe = 5
		rowh = 4
		colp = 1
		colc = 1
		colt = 1
		cole = 1 
		colh = 1

	    '****** Get Campaigns ******'	    
	    dim arrCampaigns
		arrCampaigns = Split("MAL,TNT,SAVEON,FLVA,MDI", ",")
		'arrCampaigns = Split("TNT,SAVEON,FLVA,MDI", ",")
		
		'****** Premiums ******'
		Get_Premium_Array
	    
	    'for pages = 1 to 7
			for i = 0 to ubound(arrCampaigns)
				'Reset Columns
				col = 1
				
				'Increment Row
				row = row + 1
				
				'Write Campaign Name
				Write_Cell arrCampaigns(i), "Totals"
				
				'*** Get Locations ***
				
				Get_Locations arrCampaigns(i)
				bCheckForNulls = false
				for y = 0 to ubound(arrLocs)
					'For Totals Write "ALL" for Location
					if y = 0 then
						Write_Cell "ALL", "Totals"
					else
						bCheckForNulls = true
						row = row + 1
						col = 1
						Write_Cell "", "Totals"
						Write_Cell arrLocs(y), "Totals"
					end if
					
					'Get_Tours
					Get_Tours arrCampaigns(i), arrLocs(y)
									
					'Get_Sales
					Get_Sales arrCampaigns(i), arrLocs(y)
					
					'Get_OWs
					Get_OWs arrCampaigns(i), arrLocs(y)
					
					'Get_Sales_Volume
					Get_SalesVol arrCampaigns(i), arrLocs(y)
					
					'Get_OW_Volume
					Get_OWVol arrCampaigns(i), arrLocs(y)
					
					'Write Total Sales formula
					Write_Formula "=RC[-4]+RC[-3]", "Totals"
					
					'Write Total Volume formula
					Write_Formula "=RC[-3]+RC[-2]", "Totals"
					
					Get_Tours_Sales_By_Marital_Status arrCampaigns(i), arrLocs(y)
					
					Get_Premiums arrCampaigns(i), arrLocs(y)
					
					Get_Cottages arrCampaigns(i), arrLocs(y)
					
					Get_Townes arrCampaigns(i), arrLocs(y)
					
					Get_Estates arrCampaigns(i), arrLocs(y)
					
				next
				if bCheckForNulls then
					row = row + 1
					col = 1
					Write_Cell "", "Totals"
					Write_Cell "Unknown", "Totals"
					
					Get_Tours arrCampaigns(i), "Unknown"
					Get_Sales arrCampaigns(i), "Unknown"
					Get_OWs arrCampaigns(i), "Unknown"
					Get_SalesVol arrCampaigns(i), "Unknown"
					Get_OWVol arrCampaigns(i), "Unknown"
					Write_Formula "=RC[-4]+RC[-3]", "Totals"
					Write_Formula "=RC[-3]+RC[-2]", "Totals"
					Get_Tours_Sales_By_Marital_Status arrCampaigns(i), "Unknown"
					Get_Premiums arrCampaigns(i), "Unknown"
					Get_Cottages arrCampaigns(i), "Unknown"
					Get_Townes arrCampaigns(i), "Unknown"
					Get_Estates arrCampaigns(i), "Unknown"
				end if
			next
		'next
	    
	    'wsActiveSheet.Cells.Select
	    'wsActiveSheet.Cells.EntireColumn.AutoFit
	   
	   	'wsactivesheet.cells(1,"A").select

		wb.Save
	    wb.Close
	    Set xlApp = Nothing
	    Set wb = Nothing
	    Set wsActiveSheet = Nothing

   		response.write "End Time: " & now & "<br />"

	    sAns = "Your Excel document has been created.<br>Click on the file name to open.<br>"
	    'sAns = sAns & "<a target = '_blank' href = '../contracts/" & slink & "'>" & slink & "</a>"
		sAns = sAns & "<a target = '_blank' href = 'https://www.kingscreekmarketing.com/crms/reports/contracts/" & slink & "'>" & slink & "</a>"
		response.write sAns
		
	end if
	
	cn.close
	
	set cn = nothing
	set rs = nothing
	

	function Write_Cell(val, sheet)
		select case sheet
			case "Totals"
				Set wsActiveSheet = wb.Worksheets("Totals")
				wsactivesheet.cells(row,col).value = val
				col = col + 1
			case "Marital Status"
				Set wsActiveSheet = wb.Worksheets("Marital Status")
				wsactivesheet.cells(rowms,colms).value = val
				colms = colms + 1
			case "Premiums"
				Set wsActiveSheet = wb.Worksheets("Premiums")
				wsactivesheet.cells(rowp,colp).value = val
				colp = colp + 1
			case "Cottages"
				Set wsActiveSheet = wb.Worksheets("Cottages")
				wsactivesheet.cells(rowc,colc).value = val
				colc = colc + 1
			case "Townes"
				Set wsActiveSheet = wb.Worksheets("Townes")
				wsactivesheet.cells(rowt,colt).value = val
				colt = colt + 1
			case "Estates"
				Set wsActiveSheet = wb.Worksheets("Estates")
				wsactivesheet.cells(rowe,cole).value = val
				cole = cole + 1
			case "Hotel"
				Set wsActiveSheet = wb.Worksheets("Hotel")
				wsactivesheet.cells(rowh,colh).value = val
				colh = colh + 1
		end select
		
	end function
	
	function Write_Formula(formula, sheet)
		select case sheet
			case "Totals"
				Set wsActiveSheet = wb.Worksheets("Totals")
				wsactivesheet.cells(row,col).FormulaR1C1 = formula
				col = col + 1
			case "Marital Status"
				Set wsActiveSheet = wb.Worksheets("Marital Status")
				wsactivesheet.cells(rowms,colms).FormulaR1C1 = formula
				colms = colms + 1
			case "Premiums"
				Set wsActiveSheet = wb.Worksheets("Premiums")
				wsactivesheet.cells(rowp,colp).FormulaR1C1 = formula
				colp = colp + 1
			case "Cottages"
				Set wsActiveSheet = wb.Worksheets("Cottages")
				wsactivesheet.cells(rowc,colc).FormulaR1C1 = formula
				colc = colc + 1
			case "Townes"
				Set wsActiveSheet = wb.Worksheets("Townes")
				wsactivesheet.cells(rowt,colt).FormulaR1C1 = formula
				colt = colt + 1
			case "Estates"
				Set wsActiveSheet = wb.Worksheets("Estates")
				wsactivesheet.cells(rowe,cole).FormulaR1C1 = formula
				cole = cole + 1
			case "Hotel"
				Set wsActiveSheet = wb.Worksheets("Hotel")
				wsactivesheet.cells(rowh,colh).FormulaR1C1 = formula
				colh = colh + 1
		end select
	end function
	



	function Get_Tours(camp,loc)
		sql = "SELECT COUNT(DISTINCT T.TOURID) " & _
				"FROM T_TOUR T " & _
				"	INNER JOIN T_COMBOITEMS TS ON TS.COMBOITEMID = T.StatusID " & _
				"	LEFT OUTER JOIN T_COMBOITEMS TST ON TST.COMBOITEMID = T.SubTypeID " & _
				"	LEFT OUTER JOIN T_CONTRACT C ON C.TOURID = T.TOURID " & _
				"	INNER JOIN T_CAMPAIGN CAMP ON CAMP.CAMPAIGNID = T.CAMPAIGNID " & _
				"	LEFT OUTER JOIN T_VENDORREP2TOUR R ON R.TOURID = T.TOURID " & _
				"	LEFT OUTER JOIN T_VENDORSALESLOCATIONS VSL ON VSL.SALESLOCATIONID = R.SALELOCID " & _
				"WHERE CAMP.NAME = '" & camp & "' " & _
				"	AND (TS.COMBOITEM IN ('ONTOUR','SHOWED') OR C.CONTRACTID IS NOT NULL) " & _
				"	AND T.TOURDATE BETWEEN '" & sdate & "' AND '" & edate & "' " & _
				"	AND TST.COMBOITEM <> 'EXIT' "
		if LOC <> "Unknown" and  loc <> "" then
			sql = sql  & "	AND VSL.LOCATION = '" & loc & "'"
		elseif loc = "Unknown" then
			sql = sql  & "	AND (R.TOURID IS NULL OR VSL.LOCATION = '')"
		end if
		
		rs.open sql, cn, 0 , 1
		if rs.eof and rs.bof then
			val = 0
		else
			val = rs.fields(0).value
		end if
		rs.close
		
		if val & "" = "" then val = 0
		
		Write_Cell val, "Totals"

	end function
	
	function Get_Sales(camp,loc)
		sql = "SELECT COUNT(DISTINCT C.CONTRACTID) " & _
				"FROM T_CONTRACT C " & _
				"	INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID " & _
				"	INNER JOIN T_SOLDINVENTORY S ON S.CONTRACTID = C.CONTRACTID " & _
				"	LEFT OUTER JOIN T_VENDORREP2TOUR R ON R.TOURID = C.TOURID " & _
				"	LEFT OUTER JOIN T_VENDORSALESLOCATIONS VSL ON VSL.SALESLOCATIONID = R.SALELOCID " & _
				"WHERE C.CONTRACTDATE BETWEEN '" & sdate & "'  AND '" & edate & "' " & _
				"	AND C.TOURID IN ( " & _
				"		SELECT DISTINCT T.TOURID " & _
				"		FROM T_TOUR T  " & _
				"			INNER JOIN T_COMBOITEMS TS ON TS.COMBOITEMID = T.StatusID " & _
				"			LEFT OUTER JOIN T_CONTRACT C ON C.TOURID = T.TOURID " & _
				"		WHERE T.TOURDATE BETWEEN '" & sdate & "' AND '" & edate & "'  " & _
				"			AND (TS.COMBOITEM IN ('SHOWED', 'ONTOUR') OR C.CONTRACTID IS NOT NULL) " & _
				"			AND T.CAMPAIGNID IN (SELECT CAMPAIGNID FROM T_CAMPAIGN WHERE NAME = '" & camp & "') " & _
				"	) AND CS.COMBOITEM IN ('ACTIVE','SUSPENSE') " 
		if LOC <> "Unknown" and  loc <> "" then
			sql = sql  & "	AND VSL.LOCATION = '" & loc & "'"
		elseif loc = "Unknown" then
			sql = sql  & "	AND (R.TOURID IS NULL OR VSL.LOCATION = '')"
		end if
		
		rs.open sql, cn, 0, 1
		if rs.eof and rs.bof then 
			val = 0
		else
			val = rs.fields(0).value
		end if
		rs.close
		
		if val & "" = "" then val = 0
		
		Write_Cell val, "Totals"

	end function
	
	function Get_OWs(camp,loc)
		sql = "	SELECT COUNT(DISTINCT C.CONTRACTID)  " & _
				"FROM T_CONTRACT C  " & _
				"	INNER JOIN T_TOUR T ON T.TOURID = C.TOURID  " & _
				"	INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID  " & _
				"	INNER JOIN T_SOLDINVENTORY S ON S.CONTRACTID = C.CONTRACTID  " & _
				"	LEFT OUTER JOIN T_VENDORREP2TOUR R ON R.TOURID = C.TOURID " & _
				"	LEFT OUTER JOIN T_VENDORSALESLOCATIONS VSL ON VSL.SALESLOCATIONID = R.SALELOCID " & _
				"WHERE C.CONTRACTDATE < '" & sdate & "'  " & _
				"	AND ORIGINALLYWRITTENDATE BETWEEN '" & sdate & "' AND '" & edate & "' " & _
				"	AND C.STATUSDATE BETWEEN '" & sdate & "' AND '" & edate & "' " & _
				"	AND T.CAMPAIGNID IN (SELECT CAMPAIGNID FROM T_CAMPAIGN WHERE NAME = '" & camp & "') " & _
				"	AND CS.COMBOITEM IN ('ACTIVE', 'SUSPENSE') " 
		
		if LOC <> "Unknown" and  loc <> "" then
			sql = sql  & "	AND VSL.LOCATION = '" & loc & "'"
		elseif loc = "Unknown" then
			sql = sql  & "	AND (R.TOURID IS NULL OR VSL.LOCATION = '')"
		end if
		
		rs.open sql, cn, 0, 1
		if rs.eof and rs.bof then 
			val = 0
		else
			val = rs.fields(0).value
		end if
		rs.close
		
		if val & "" = "" then val = 0
		
		Write_Cell val, "Totals"

	end function
	
	function Get_SalesVol(camp, loc)
		sql = "SELECT SUM(M.SALESVOLUME) " & _
				"FROM T_MORTGAGE M " & _
				"WHERE M.CONTRACTID IN ( " & _
					"SELECT DISTINCT C.CONTRACTID " & _
					"FROM T_CONTRACT C " & _
					"	INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID " & _
					"	INNER JOIN T_SOLDINVENTORY S ON S.CONTRACTID = C.CONTRACTID " & _
					"	LEFT OUTER JOIN T_VENDORREP2TOUR R ON R.TOURID = C.TOURID " & _
					"	LEFT OUTER JOIN T_VENDORSALESLOCATIONS VSL ON VSL.SALESLOCATIONID = R.SALELOCID " & _
					"WHERE C.CONTRACTDATE BETWEEN '" & sdate & "'  AND '" & edate & "' " & _
					"	AND C.TOURID IN ( " & _
					"		SELECT DISTINCT T.TOURID " & _
					"		FROM T_TOUR T  " & _
					"			INNER JOIN T_COMBOITEMS TS ON TS.COMBOITEMID = T.StatusID " & _
					"			LEFT OUTER JOIN T_CONTRACT C ON C.TOURID = T.TOURID " & _
					"		WHERE T.TOURDATE BETWEEN '" & sdate & "' AND '" & edate & "'  " & _
					"			AND (TS.COMBOITEM IN ('SHOWED', 'ONTOUR') OR C.CONTRACTID IS NOT NULL) " & _
					"			AND T.CAMPAIGNID IN (SELECT CAMPAIGNID FROM T_CAMPAIGN WHERE NAME = '" & camp & "') " & _
					"	) AND CS.COMBOITEM IN ('ACTIVE','SUSPENSE') " 
			if LOC <> "Unknown" and  loc <> "" then
				sql = sql  & "	AND VSL.LOCATION = '" & loc & "'"
			elseif loc = "Unknown" then
				sql = sql  & "	AND (VSL.LOCATION IS NULL OR VSL.LOCATION = '')"
			end if
			SQL = SQL & ")"
			
		rs.open sql, cn, 0, 1
		if rs.eof and rs.bof then 
			val = 0
		else
			val = rs.fields(0).value
		end if
		rs.close
		
		if val & "" = "" then val = 0
		
		Write_Cell val, "Totals"


	end function
	
	function Get_OWVol(camp,loc)
		sql = "SELECT SUM(M.SALESVOLUME) " & _
				"FROM T_MORTGAGE M " & _
				"WHERE M.CONTRACTID IN ( " & _
					"SELECT DISTINCT C.CONTRACTID  " & _
					"FROM T_CONTRACT C  " & _
					"	INNER JOIN T_TOUR T ON T.TOURID = C.TOURID  " & _
					"	INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID  " & _
					"	INNER JOIN T_SOLDINVENTORY S ON S.CONTRACTID = C.CONTRACTID  " & _
					"	LEFT OUTER JOIN T_VENDORREP2TOUR R ON R.TOURID = C.TOURID " & _
					"	LEFT OUTER JOIN T_VENDORSALESLOCATIONS VSL ON VSL.SALESLOCATIONID = R.SALELOCID " & _
					"WHERE C.CONTRACTDATE < '" & sdate & "'  " & _
					"	AND ORIGINALLYWRITTENDATE BETWEEN '" & sdate & "' AND '" & edate & "' " & _
					"	AND C.STATUSDATE BETWEEN '" & sdate & "' AND '" & edate & "' " & _
					"	AND T.CAMPAIGNID IN (SELECT CAMPAIGNID FROM T_CAMPAIGN WHERE Name = '" & camp & "') " & _
					"	AND CS.COMBOITEM IN ('ACTIVE', 'SUSPENSE') " 
			
			if LOC <> "Unknown" and  loc <> "" then
				sql = sql  & "	AND VSL.LOCATION = '" & loc & "'"
			elseif loc = "Unknown" then
				sql = sql  & "	AND (VSL.LOCATION IS NULL OR VSL.LOCATION = '')"
			end if
			sql = sql & ")"
		
		rs.open sql, cn, 0, 1
		if rs.eof and rs.bof then 
			val = 0
		else
			val = rs.fields(0).value
		end if
		rs.close
		
		if val & "" = "" then val = 0
		
		Write_Cell val, "Totals"

	end function
	
	function Get_Tours_Sales_By_Marital_Status(camp, loc)
		'get all marital statuses
		
		Write_Cell camp, "Marital Status"
		if loc <> "" then
			Write_Cell loc, "Marital Status"
		else
			Write_Cell "ALL", "Marital Status"
		end if
			
		
		rs.open "Select comboitem from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'maritalstatus' and active = 1 order by comboitem", cn, 0, 1
		sTemp = ""
		do while not rs.eof
			if sTemp <> "" then sTemp = sTemp & ","
			sTemp = sTemp &  rs.fields(0).value
			rs.movenext
		loop
		rs.close
		if sTemp <> "" then
			aMS = split(sTemp,",")
			for x = 0 to ubound(aMs)
				sql = "SELECT COUNT(DISTINCT T.TOURID) " & _
						"FROM T_TOUR T  " & _
						"	INNER JOIN T_PROSPECT P ON P.PROSPECTID = T.PROSPECTID " & _
						"	LEFT OUTER JOIN T_COMBOITEMS MS ON MS.COMBOITEMID = P.MARITALSTATUSID " & _
						"	INNER JOIN T_COMBOITEMS TS ON TS.COMBOITEMID = T.StatusID " & _
						"	LEFT OUTER JOIN T_COMBOITEMS TST ON TST.COMBOITEMID = T.SubTypeID " & _
						"	LEFT OUTER JOIN T_CONTRACT C ON C.TOURID = T.TOURID " & _
						"	LEFT OUTER JOIN T_VENDORREP2TOUR R ON R.TOURID = C.TOURID " & _
						"	LEFT OUTER JOIN T_VENDORSALESLOCATIONS VSL ON VSL.SALESLOCATIONID = R.SALELOCID " & _
						"WHERE T.CAMPAIGNID IN (SELECT CAMPAIGNID FROM T_CAMPAIGN WHERE Name = '" & CAMP & "') " & _
						"	AND (TS.COMBOITEM IN ('ONTOUR','SHOWED') OR C.CONTRACTID IS NOT NULL) " & _
						"	AND T.TOURDATE BETWEEN '" & SDATE & "' AND '" & EDATE & "' " & _
						"	AND TST.COMBOITEM <> 'EXIT' " & _
						"	AND MS.COMBOITEM = '" & AMS(X) & "'"
				if LOC <> "Unknown" and  loc <> "" then
					sql = sql  & "	AND VSL.LOCATION = '" & loc & "'"
				elseif loc = "Unknown" then
					sql = sql  & "	AND (VSL.LOCATION IS NULL OR VSL.LOCATION = '')"
				end if
				
				rs.open sql, cn, 0, 1
				if rs.eof and rs.bof then
					val = 0
				else
					val = rs.fields(0).value
				end if
				rs.close
				
				if val & "" = "" then val = 0
				Write_Cell val, "Marital Status"

				
				sql = "SELECT COUNT(DISTINCT C.CONTRACTID) " & _
						"FROM T_CONTRACT C " & _
						"	INNER JOIN T_PROSPECT P ON P.PROSPECTID = C.PROSPECTID " & _
						"	LEFT OUTER JOIN T_COMBOITEMS MS ON MS.COMBOITEMID = P.MARITALSTATUSID " & _
						"	INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID " & _
						"	INNER JOIN T_SOLDINVENTORY S ON S.CONTRACTID = C.CONTRACTID " & _
						"	LEFT OUTER JOIN T_VENDORREP2TOUR R ON R.TOURID = C.TOURID " & _
						"	LEFT OUTER JOIN T_VENDORSALESLOCATIONS VSL ON VSL.SALESLOCATIONID = R.SALELOCID " & _
						"WHERE C.CONTRACTDATE BETWEEN '" & sdate & "'  AND '" & edate & "' " & _
						"	AND C.TOURID IN ( " & _
						"		SELECT DISTINCT T.TOURID " & _
						"		FROM T_TOUR T  " & _
						"			INNER JOIN T_COMBOITEMS TS ON TS.COMBOITEMID = T.StatusID " & _
						"			LEFT OUTER JOIN T_CONTRACT C ON C.TOURID = T.TOURID " & _
						"		WHERE T.TOURDATE BETWEEN '" & sdate & "' AND '" & edate & "'  " & _
						"			AND (TS.COMBOITEM IN ('SHOWED', 'ONTOUR') OR C.CONTRACTID IS NOT NULL) " & _
						"			AND T.CAMPAIGNID IN (SELECT CAMPAIGNID FROM T_CAMPAIGN WHERE Name = '" & camp & "') " & _
						"	) AND CS.COMBOITEM IN ('ACTIVE','SUSPENSE') " & _
						"  AND MS.COMBOITEM = '" & AMS(X) & "' " 
				if LOC <> "Unknown" and  loc <> "" then
					sql = sql  & "	AND VSL.LOCATION = '" & loc & "'"
				elseif loc = "Unknown" then
					sql = sql  & "	AND (VSL.LOCATION IS NULL OR VSL.LOCATION = '')"
				end if
				
				rs.open sql, cn, 0, 1
				if rs.eof and rs.bof then
					val = 0
				else
					val = rs.fields(0).value
				end if
				rs.close
				
				if val & "" = "" then val = 0
				Write_Cell val, "Marital Status"


			next
			
			'****** UNKNOWNS ******
			sql = "SELECT COUNT(DISTINCT T.TOURID) " & _
					"FROM T_TOUR T  " & _
					"	INNER JOIN T_PROSPECT P ON P.PROSPECTID = T.PROSPECTID " & _
					"	LEFT OUTER JOIN T_COMBOITEMS MS ON MS.COMBOITEMID = P.MARITALSTATUSID " & _
					"	INNER JOIN T_COMBOITEMS TS ON TS.COMBOITEMID = T.StatusID " & _
					"	LEFT OUTER JOIN T_COMBOITEMS TST ON TST.COMBOITEMID = T.SubTypeID " & _
					"	LEFT OUTER JOIN T_CONTRACT C ON C.TOURID = T.TOURID " & _
					"	LEFT OUTER JOIN T_VENDORREP2TOUR R ON R.TOURID = C.TOURID " & _
					"	LEFT OUTER JOIN T_VENDORSALESLOCATIONS VSL ON VSL.SALESLOCATIONID = R.SALELOCID " & _
					"WHERE T.CAMPAIGNID IN (SELECT CAMPAIGNID FROM T_CAMPAIGN WHERE Name = '" & CAMP & "') " & _
					"	AND (TS.COMBOITEM IN ('ONTOUR','SHOWED') OR C.CONTRACTID IS NOT NULL) " & _
					"	AND T.TOURDATE BETWEEN '" & SDATE & "' AND '" & EDATE & "' " & _
					"	AND TST.COMBOITEM <> 'EXIT' " & _
					"	AND (MS.COMBOITEM IS NULL OR MS.COMBOITEM = '')"
			if LOC <> "Unknown" and  loc <> "" then
				sql = sql  & "	AND VSL.LOCATION = '" & loc & "'"
			elseif loc = "Unknown" then
				sql = sql  & "	AND (VSL.LOCATION IS NULL OR VSL.LOCATION = '')"
			end if
			
			rs.open sql, cn, 0, 1
			if rs.eof and rs.bof then
				val = 0
			else
				val = rs.fields(0).value
			end if
			rs.close
			
			if val & "" = "" then val = 0
			Write_Cell val, "Marital Status"

			
			sql = "SELECT COUNT(DISTINCT C.CONTRACTID) " & _
					"FROM T_CONTRACT C " & _
					"	INNER JOIN T_PROSPECT P ON P.PROSPECTID = C.PROSPECTID " & _
					"	LEFT OUTER JOIN T_COMBOITEMS MS ON MS.COMBOITEMID = P.MARITALSTATUSID " & _
					"	INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID " & _
					"	INNER JOIN T_SOLDINVENTORY S ON S.CONTRACTID = C.CONTRACTID " & _
					"	LEFT OUTER JOIN T_VENDORREP2TOUR R ON R.TOURID = C.TOURID " & _
					"	LEFT OUTER JOIN T_VENDORSALESLOCATIONS VSL ON VSL.SALESLOCATIONID = R.SALELOCID " & _
					"WHERE C.CONTRACTDATE BETWEEN '" & sdate & "'  AND '" & edate & "' " & _
					"	AND C.TOURID IN ( " & _
					"		SELECT DISTINCT T.TOURID " & _
					"		FROM T_TOUR T  " & _
					"			INNER JOIN T_COMBOITEMS TS ON TS.COMBOITEMID = T.StatusID " & _
					"			LEFT OUTER JOIN T_CONTRACT C ON C.TOURID = T.TOURID " & _
					"		WHERE T.TOURDATE BETWEEN '" & sdate & "' AND '" & edate & "'  " & _
					"			AND (TS.COMBOITEM IN ('SHOWED', 'ONTOUR') OR C.CONTRACTID IS NOT NULL) " & _
					"			AND T.CAMPAIGNID IN (SELECT CAMPAIGNID FROM T_CAMPAIGN WHERE Name = '" & camp & "') " & _
					"	) AND CS.COMBOITEM IN ('ACTIVE','SUSPENSE') " & _
					"	AND (MS.COMBOITEM IS NULL OR MS.COMBOITEM = '')"
			if LOC <> "Unknown" and  loc <> "" then
				sql = sql  & "	AND VSL.LOCATION = '" & loc & "'"
			elseif loc = "Unknown" then
				sql = sql  & "	AND (VSL.LOCATION IS NULL OR VSL.LOCATION = '')"
			end if
			
			rs.open sql, cn, 0, 1
			if rs.eof and rs.bof then
				val = 0
			else
				val = rs.fields(0).value
			end if
			rs.close
			
			if val & "" = "" then val = 0
			Write_Cell val, "Marital Status"
			
		end if
		rowms = rowms + 1
		colms = 1
	end function
	
	function Get_Premiums(camp,loc)
		
		Write_Cell camp, "Premiums"
		if loc <> "" then
			Write_Cell loc, "Premiums"
		else
			Write_Cell "ALL","Premiums"
		end if
		
		for x = 0 to ubound(arrPrems)
			wsactivesheet.cells(1,colp).value = arrPrems(x)
			
			sql = "SELECT SUM(P.COSTEA * P.QTYISSUED) " & _
					"FROM T_PREMIUMISSUED P " & _
					"	INNER JOIN T_PREMIUM PR ON PR.PREMIUMID = P.PREMIUMID " & _
					"WHERE P.KEYFIELD = 'TOURID' AND P.KEYVALUE IN (" & _
						"SELECT DISTINCT T.TOURID " & _
						"FROM T_TOUR T " & _
						"	INNER JOIN T_COMBOITEMS TS ON TS.COMBOITEMID = T.StatusID " & _
						"	LEFT OUTER JOIN T_COMBOITEMS TST ON TST.COMBOITEMID = T.SubTypeID " & _
						"	LEFT OUTER JOIN T_CONTRACT C ON C.TOURID = T.TOURID " & _
						"	INNER JOIN T_CAMPAIGN CAMP ON CAMP.CAMPAIGNID = T.CAMPAIGNID " & _
						"	LEFT OUTER JOIN T_VENDORREP2TOUR R ON R.TOURID = T.TOURID " & _
						"	LEFT OUTER JOIN T_VENDORSALESLOCATIONS VSL ON VSL.SALESLOCATIONID = R.SALELOCID " & _
						"WHERE CAMP.Name = '" & camp & "' " & _
						"	AND T.TOURDATE BETWEEN '" & sdate & "' AND '" & edate & "' " & _
						"	AND TST.COMBOITEM <> 'EXIT' "
				if LOC <> "Unknown" and  loc <> "" then
					sql = sql  & "	AND VSL.LOCATION = '" & loc & "'"
				elseif loc = "Unknown" then
					sql = sql  & "	AND (VSL.LOCATION IS NULL OR VSL.LOCATION = '')"
				end if
			sql = sql & ") and pr.premiumname = '" & arrPrems(x) & "'"
			
			rs.open sql, cn, 0, 1
			if rs.eof and rs.bof then
				val = 0
			else
				val = rs.fields(0).value
			end if
			rs.close
			if val & "" = "" then val = 0
			
			Write_Cell val, "Premiums"

			
		next
	
	colp = 1
	rowp = rowp + 1
		
	end function 
	
	function Get_Premium_Array()
		sql = "select distinct p.premiumname " & _
				"from t_Premium p " & _
				"inner join t_Premiumissued i on i.premiumid = p.premiumid " & _
				"where i.dateissued between '" & sdate & "' and '" & edate & "' " & _
				"order by p.premiumname "
		sTemp = ""
		rs.open sql, cn, 0, 1
		do while not rs.eof 
			if sTemp <> "" then sTemp = sTemp & ","
			sTemp = sTemp & rs.fields(0).value
			rs.movenext
		loop
		rs.close
		
		if sTemp <> "" then
			arrPrems = split(sTemp,",")
		end if	
	end function
	
	function Get_Locations(camp)
		redim arrLocs(0)
		arrLocs(0) = ""
		
		sql = "SELECT DISTINCT VSL.LOCATION " & _
				"FROM T_VENDORSALESLOCATIONS VSL  " & _
				"	INNER JOIN T_VENDORCAMPAIGNS VC ON VC.VENDORID = VSL.VENDORID  " & _
				"	INNER JOIN T_VENDORREP2TOUR R ON VSL.SALESLOCATIONID = R.SALELOCID " & _
				"	INNER JOIN T_TOUR T ON T.TOURID = R.TOURID " & _
				"	INNER JOIN T_CAMPAIGN C ON C.CAMPAIGNID = T.CAMPAIGNID " & _
				"WHERE VC.CampaignName = '" & camp & "'  " & _
					"AND (VSL.ACTIVE = 1 OR VSL.DATEDEACTIVATED BETWEEN '" & sdate & "' AND '" & edate & "') " & _
					"AND C.Name = '" & camp & "' " & _
				"ORDER BY VSL.LOCATION "
		rs.open sql, cn, 0, 1
		if rs.eof and rs.bof then
		else
			do while not rs.eof 
				redim preserve arrLocs(ubound(arrLocs) + 1)
				arrLocs(ubound(arrLocs)) = trim(rs.fields(0).value & "")
				rs.movenext
			loop
		end if
		rs.close
	end function
	
	function Get_Cottages(camp, loc)
		
		Write_Cell camp, "Cottages"
		Write_Cell loc, "Cottages"
		
		for n = 1 to 5
			for c = 1 to 3
				rs.open Get_Breakout(camp, loc, "Cottage", c, n), cn, 0, 1
				if rs.eof and rs.bof then
					val = 0
				else
					val = rs.fields(0).value
				end if
				rs.close
				if val & "" = "" then val = 0
				Write_Cell val, "Cottages"
				rs.open Get_Breakout_Sales(camp, loc, "Cottage", c, n), cn, 0, 1
				if rs.eof and rs.bof then
					val = 0
				else
					val = rs.fields(0).value
				end if
				rs.close
				if val & "" = "" then val = 0
				Write_Cell val, "Cottages"
			next
		next
		Write_Formula "=RC[-30]+RC[-24]+RC[-18]+RC[-12]+RC[-6]", "Cottages"
		Write_Formula "=RC[-30]+RC[-24]+RC[-18]+RC[-12]+RC[-6]", "Cottages"
		Write_Formula "=RC[-30]+RC[-24]+RC[-18]+RC[-12]+RC[-6]", "Cottages"
		Write_Formula "=RC[-30]+RC[-24]+RC[-18]+RC[-12]+RC[-6]", "Cottages"
		Write_Formula "=RC[-30]+RC[-24]+RC[-18]+RC[-12]+RC[-6]", "Cottages"
		Write_Formula "=RC[-30]+RC[-24]+RC[-18]+RC[-12]+RC[-6]", "Cottages"
		Write_Formula "=RC[-6]+RC[-4]+RC[-2]", "Cottages"
		Write_Formula "=RC[-6]+RC[-4]+RC[-2]", "Cottages"
		rowc = rowc + 1
		colc = 1
	end function
	
	function Get_Townes(camp, loc)
		
		Write_Cell camp, "Townes"
		Write_Cell loc, "Townes"
		
		for n = 1 to 5
			for c = 2 to 4 step 2
				rs.open Get_Breakout(camp, loc, "Townes", c, n), cn, 0, 1
				if rs.eof and rs.bof then
					val = 0
				else
					val = rs.fields(0).value
				end if
				rs.close
				if val & "" = "" then val = 0
				Write_Cell val, "Townes"
				rs.open Get_Breakout_Sales(camp, loc, "Townes", c, n), cn, 0, 1
				if rs.eof and rs.bof then
					val = 0
				else
					val = rs.fields(0).value
				end if
				rs.close
				if val & "" = "" then val = 0
				Write_Cell val, "Townes"
			next
		next
		Write_Formula "=RC[-20]+RC[-16]+RC[-12]+RC[-8]+RC[-4]", "Townes"
		Write_Formula "=RC[-20]+RC[-16]+RC[-12]+RC[-8]+RC[-4]", "Townes"
		Write_Formula "=RC[-20]+RC[-16]+RC[-12]+RC[-8]+RC[-4]", "Townes"
		Write_Formula "=RC[-20]+RC[-16]+RC[-12]+RC[-8]+RC[-4]", "Townes"
		Write_Formula "=RC[-4]+RC[-2]", "Townes"
		Write_Formula "=RC[-4]+RC[-2]", "Townes"
		rowt = rowt + 1
		colt = 1
	end function
	
	function Get_Estates(camp, loc)
		
		Write_Cell camp, "Estates"
		Write_Cell loc, "Estates"
		
		for n = 1 to 5
			for c = 1 to 4
				rs.open Get_Breakout(camp, loc, "Estates", c, n), cn, 0, 1
				if rs.eof and rs.bof then
					val = 0
				else
					val = rs.fields(0).value
				end if
				rs.close
				if val & "" = "" then val = 0
				Write_Cell val, "Estates"
				rs.open Get_Breakout_Sales(camp, loc, "Estates", c, n), cn, 0, 1
				if rs.eof and rs.bof then
					val = 0
				else
					val = rs.fields(0).value
				end if
				rs.close
				if val & "" = "" then val = 0
				Write_Cell val, "Estates"
			next
		next
		Write_Formula "=RC[-40]+RC[-32]+RC[-24]+RC[-16]+RC[-8]", "Estates"
		Write_Formula "=RC[-40]+RC[-32]+RC[-24]+RC[-16]+RC[-8]", "Estates"
		Write_Formula "=RC[-40]+RC[-32]+RC[-24]+RC[-16]+RC[-8]", "Estates"
		Write_Formula "=RC[-40]+RC[-32]+RC[-24]+RC[-16]+RC[-8]", "Estates"
		Write_Formula "=RC[-40]+RC[-32]+RC[-24]+RC[-16]+RC[-8]", "Estates"
		Write_Formula "=RC[-40]+RC[-32]+RC[-24]+RC[-16]+RC[-8]", "Estates"
		Write_Formula "=RC[-40]+RC[-32]+RC[-24]+RC[-16]+RC[-8]", "Estates"
		Write_Formula "=RC[-40]+RC[-32]+RC[-24]+RC[-16]+RC[-8]", "Estates"
		Write_Formula "=RC[-8]+RC[-6]+RC[-4]+RC[-2]", "Estates"
		Write_Formula "=RC[-8]+RC[-6]+RC[-4]+RC[-2]", "Estates"
		rowe = rowe + 1
		cole = 1
	end function
	
	function Get_Hotels(camp, loc)
		dim r, aCottages(2)
		aCottages(0) = "1 BD"
		aCottages(1) = "2 BD"
		aCottages(2) = "3 BD"
		
		Write_Cell camp, "Cottages"
		Write_Cell loc, "Cottages"
		
		for n = 1 to 5
			for c = 0 to ubound(aCottages)
				rs.open Get_Breakout(camp, loc, "Cottage", aCottages(c), n), cn, 0, 1
				if rs.eof and rs.bof then
					val = 0
				else
					val = rs.fields(0).value
				end if
				rs.close
				if val & "" = "" then val = 0
				Write_Cell val, "Cottages"
			next
		next
	end function
	
	function Get_Breakout(camp, loc, ut, size, nights)
		sql = "SELECT COUNT(DISTINCT TOURID) " & _
				"FROM ( " & _
					"SELECT DISTINCT T.TOURID, sum(cast(left(types.rt,1) as int)) AS RT, types.ut, datediff(dd,re.checkindate, re.checkoutdate) AS NIGHTS " & _
					"	FROM T_TOUR T  " & _
					"		INNER JOIN T_COMBOITEMS TS ON TS.COMBOITEMID = T.StatusID  " & _
					"		LEFT OUTER JOIN T_COMBOITEMS TST ON TST.COMBOITEMID = T.SubTypeID  " & _
					"		LEFT OUTER JOIN T_CONTRACT C ON C.TOURID = T.TOURID  " & _
					"		INNER JOIN T_CAMPAIGN CAMP ON CAMP.CAMPAIGNID = T.CAMPAIGNID  " & _
					"		LEFT OUTER JOIN T_VENDORREP2TOUR R ON R.TOURID = T.TOURID  " & _
					"		LEFT OUTER JOIN T_VENDORSALESLOCATIONS VSL ON VSL.SALESLOCATIONID = R.SALELOCID  " & _
					"		INNER JOIN T_RESERVATIONS RE ON RE.TOURID = T.TOURID " & _
					"		INNER JOIN ( " & _
					"			SELECT DISTINCT ROOM.ROOMID, RM.RESERVATIONID, RT.COMBOITEM AS RT, UT.COMBOITEM AS UT " & _
					"			FROM T_ROOMALLOCATIONMATRIX RM  " & _
					"				INNER JOIN T_ROOM ROOM ON ROOM.ROOMID = RM.ROOMID " & _
					"				INNER JOIN T_COMBOITEMS RT ON RT.COMBOITEMID = ROOM.TYPEID " & _
					"				INNER JOIN T_UNIT U ON U.UNITID = ROOM.UNITID " & _
					"				INNER JOIN T_COMBOITEMS UT ON UT.COMBOITEMID = U.TYPEID " & _
					"		) AS TYPES ON TYPES.RESERVATIONID = RE.RESERVATIONID " & _
					"	WHERE CAMP.Name = '" & camp & "'  " & _
					"		AND (TS.COMBOITEM IN ('ONTOUR','SHOWED') OR C.CONTRACTID IS NOT NULL)  " & _
					"		AND T.TOURDATE BETWEEN '" & sdate & "' AND '" & edate & "'  " & _
					"		AND (tst.comboitem is null or TST.COMBOITEM <> 'EXIT' ) "
		if LOC <> "Unknown" and  loc <> "" then
			sql = sql  & "	AND VSL.LOCATION = '" & loc & "'"
		elseif loc = "Unknown" then
			sql = sql  & "	AND (R.TOURID IS NULL OR VSL.LOCATION = '') "
		end if			
		
		sql = sql & "	GROUP BY T.TOURID, TYPES.UT, DATEDIFF(DD,RE.CHECKINDATE, RE.CHECKOUTDATE) " & _
				") A " & _
				"WHERE RT = '" & LEFT(size & "", 1) & "' " & _
				"AND UT = '" & ut & "' " 
		if nights > 4 then
			sql = sql  & "AND NIGHTS > 4 "
		else
			sql = sql  & "AND NIGHTS = " & nights
		end if
		
		
		
		Get_Breakout = sql
	
	end function
	
	function Get_Breakout_Sales(camp, loc, ut, size, nights)
		sql = "SELECT COUNT(DISTINCT C.CONTRACTID) " & _
				"FROM T_CONTRACT C " & _
				"	INNER JOIN T_SOLDINVENTORY SI ON SI.CONTRACTID = C.CONTRACTID " & _
				"	INNER JOIN T_COMBOITEMS CS ON CS.COMBOITEMID = C.STATUSID " & _
				"WHERE C.TOURID IN (" & _
				"SELECT DISTINCT TOURID " & _
				"FROM ( " & _
					"SELECT DISTINCT T.TOURID, sum(cast(left(types.rt,1) as int)) AS RT, types.ut, datediff(dd,re.checkindate, re.checkoutdate) AS NIGHTS " & _
					"	FROM T_TOUR T  " & _
					"		INNER JOIN T_COMBOITEMS TS ON TS.COMBOITEMID = T.StatusID  " & _
					"		LEFT OUTER JOIN T_COMBOITEMS TST ON TST.COMBOITEMID = T.SubTypeID  " & _
					"		LEFT OUTER JOIN T_CONTRACT C ON C.TOURID = T.TOURID  " & _
					"		INNER JOIN T_CAMPAIGN CAMP ON CAMP.CAMPAIGNID = T.CAMPAIGNID  " & _
					"		LEFT OUTER JOIN T_VENDORREP2TOUR R ON R.TOURID = T.TOURID  " & _
					"		LEFT OUTER JOIN T_VENDORSALESLOCATIONS VSL ON VSL.SALESLOCATIONID = R.SALELOCID  " & _
					"		INNER JOIN T_RESERVATIONS RE ON RE.TOURID = T.TOURID " & _
					"		INNER JOIN ( " & _
					"			SELECT DISTINCT ROOM.ROOMID, RM.RESERVATIONID, RT.COMBOITEM AS RT, UT.COMBOITEM AS UT " & _
					"			FROM T_ROOMALLOCATIONMATRIX RM  " & _
					"				INNER JOIN T_ROOM ROOM ON ROOM.ROOMID = RM.ROOMID " & _
					"				INNER JOIN T_COMBOITEMS RT ON RT.COMBOITEMID = ROOM.TYPEID " & _
					"				INNER JOIN T_UNIT U ON U.UNITID = ROOM.UNITID " & _
					"				INNER JOIN T_COMBOITEMS UT ON UT.COMBOITEMID = U.TYPEID " & _
					"		) AS TYPES ON TYPES.RESERVATIONID = RE.RESERVATIONID " & _
					"	WHERE CAMP.Name = '" & camp & "'  " & _
					"		AND (TS.COMBOITEM IN ('ONTOUR','SHOWED') OR C.CONTRACTID IS NOT NULL)  " & _
					"		AND T.TOURDATE BETWEEN '" & sdate & "' AND '" & edate & "'  " & _
					"		AND (tst.comboitem is null or TST.COMBOITEM <> 'EXIT' ) "
		if LOC <> "Unknown" and  loc <> "" then
			sql = sql  & "	AND VSL.LOCATION = '" & loc & "'"
		elseif loc = "Unknown" then
			sql = sql  & "	AND (R.TOURID IS NULL OR VSL.LOCATION = '') "
		end if			
		
		sql = sql & "	GROUP BY T.TOURID, TYPES.UT, DATEDIFF(DD,RE.CHECKINDATE, RE.CHECKOUTDATE) " & _
				") A " & _
				"WHERE RT = '" & LEFT(size & "", 1) & "' " & _
				"AND UT = '" & ut & "' " 
		if nights > 4 then
			sql = sql  & "AND NIGHTS > 4 "
		else
			sql = sql  & "AND NIGHTS = " & nights
		end if
		
		SQL = SQL & ")"
	
		Get_Breakout_Sales = sql
	
	end function
%>
