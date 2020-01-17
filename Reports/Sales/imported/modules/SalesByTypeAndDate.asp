
<%

	dim cn
	dim rs
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	
	server.scripttimeout = 10000
	cn.commandtimeout = 0
	'cn.open "CRMSData", DBUser, DBPass




DBName = "CRMSNET"
DBUser = "asp"
DBPass = "aspnet"
    
    cn.open DBName, DBUser, DBPass

	invtype = request("invtype")
	
	sDate = request("sdate")
	eDate = request("eDate")
	
	if sDate & "" = "" then sDate = date
	if eDate & "" = "" then eDate = date
	
	IF REQUEST("INVTYPE") = "ESTATES" THEN
		response.write Estates
	ELSEIF REQUEST("INVTYPE") = "TOWNES" THEN
		response.write Townes
	ELSEIF REQUEST("INVTYPE") = "ALL_EXCEL" THEN
		ALL_EXCEL
	ELSE
		response.write Cottage
	END IF
	
	
	
	cn.close
	set rs = nothing
	set cn = nothing
	
	function Cottage
		sans =  "COTTAGE INVENTORY (" & sDate & " - " & eDate & ")"
		sql = "SELECT * FROM ( " & _
			  "		SELECT SIZE, SEASON, FREQUENCY, TOTALNUMBER, SUM(TOTALSOLD) AS TOTALSOLD, SUM(TOTALAVAIL) AS TOTALAVAIL ,SUM(TOTALVOLUME) AS TOTALVOLUME,  " & _
			  "		(SELECT SUM(TOTALNUMBER) FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "')  WHERE PENDER = 1 AND SIZE = A.SIZE AND SEASON = A.SEASON AND FREQUENCY = A.FREQUENCY) AS PENDERS, " & _
			  "		(SELECT SUM(TOTALVOLUME) FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "')  WHERE PENDER = 1 AND SIZE = A.SIZE AND SEASON = A.SEASON AND FREQUENCY = A.FREQUENCY) AS PENDERVOLUME " & _
			  "		FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "') A " & _
			  "		GROUP BY SIZE,SEASON,FREQUENCY,TOTALNUMBER " & _
			  "	) A WHERE A.TOTALSOLD > 0 OR A.TOTALAVAIL > 0  " & _
			  "	ORDER BY SEASON,SIZE,  FREQUENCY "
			  
		rs.open sql, cn, 0, 1
		sans = sans &  "<table style='border-collapse:collapse;' border=1>"
		sans = sans &  "<tr>"
		sans = sans &  "<th>SIZE</th>"
		sans = sans &  "<th>SEASON</th>"
		sans = sans &  "<th>FREQUENCY</th>"
		sans = sans &  "<th>NUM AVAILABLE</th>"
		sans = sans &  "<th>UNSOLD VOLUME</th>"
		sans = sans &  "<th>NUM SOLD</th>"
		sans = sans &  "<th>SOLD VOLUME</th>"
		sans = sans &  "<th>PENDERS</th>"
		sans = sans &  "<th>PENDER VOLUME</th>"
		sans = sans &  "<th>TOTAL VOLUME</th>"
		sans = sans &  "</tr>"
		
		'dim unsoldamount, unsoldtotal, soldtotal, pendertotal
		unsoldamount = unsoldtotal = soldtotal = pendertotal = 00
		do while not rs.eof
			if rs.fields("size").value = 3 then
				if rs.fields("Season").value = "RED" then
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(22900 * rs.fields("totalavail").value)
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(13900 * rs.fields("totalavail").value)
					else
						unsoldamount = formatcurrency(0)
					end if
				else
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(9900 * rs.fields("totalavail").value)
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(6900 * rs.fields("totalavail").value)
					else
						unsoldamount = formatcurrency(0)
					end if
				end if
			else
				unsoldamount = formatcurrency(0)
			end if
			unsoldtotal = unsoldtotal + ccur(unsoldamount)
			soldtotal = soldtotal + ccur(rs.fields("TotalVolume").value)
			if rs.fields("pendervolume").value & "" <> "" then pendertotal = pendertotal + ccur(rs.fields("pendervolume").value)
			
			sans = sans &  "<tr>"
			sans = sans &  "<td>" & rs.fields("Size").value & " BR</td>"
			sans = sans &  "<td align = right>" & rs.fields("Season").value & "</td>"
			sans = sans &  "<td align = right>" & rs.fields("Frequency").value & "</td>"
			sans = sans &  "<td align = right>" & rs.fields("totalavail").value & "</td>"
			sans = sans &  "<td align = right>" & unsoldamount
			sans = sans &  "</td>"
			sans = sans &  "<td align = right>" & rs.fields("totalsold").value & "</td>"
			sans = sans &  "<td align = right>" & formatcurrency(rs.fields("totalvolume").value) & "</td>"
			sans = sans &  "<td align = right>" 
				if rs.fields("penders").value & "" = "" then
					sans = sans &  "0" 
				else
					sans = sans &  rs.fields("penders").value 
				end if
			sans = sans &  "</td>"
			sans = sans &  "<td align = right>" 
				if rs.fields("pendervolume").value & "" = "" then
					sans = sans &  formatcurrency(0)
				else 
					sans = sans &  formatcurrency(rs.fields("pendervolume").value)
				end if
			sans = sans &  "</td>"
			sans = sans &  "<td align = right>" & formatcurrency(ccur(unsoldamount) + rs.fields("totalvolume").value) & "</td>"
			sans = sans &  "</tr>"
			rs.movenext
		loop
		sans = sans &  "<tr>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td align = right>" & formatcurrency(unsoldtotal) & "</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td align = right>" & formatcurrency(soldtotal) & "</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td align = right>" & formatcurrency(pendertotal) & "</td>"
		sans = sans &  "<td align = right>" & formatcurrency(unsoldtotal + soldtotal) & "</td>"
		sans = sans &  "</tr>"
		rs.close
		sans = sans &  "</table>"
		sans = sans &  "* TOTAL VOLUME DOES NOT INCLUDE PENDER VOLUME *"
		cottage = sans
		
	end function
	
	function Estates
		sans = "ESTATES INVENTORY (" & sDate & " - " & eDate & ")"
		sql = "SELECT * FROM ( " & _
			  "		SELECT SIZE, SEASON, FREQUENCY, TOTALNUMBER, SUM(TOTALSOLD) AS TOTALSOLD, SUM(TOTALAVAIL) AS TOTALAVAIL ,SUM(TOTALVOLUME) AS TOTALVOLUME,  " & _
			  "		(SELECT SUM(TOTALNUMBER) FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('estates','" & sDate & "','" & eDate & "')  WHERE PENDER = 1 AND SIZE = A.SIZE AND SEASON = A.SEASON AND FREQUENCY = A.FREQUENCY) AS PENDERS, " & _
			  "		(SELECT SUM(TOTALVOLUME) FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('estates','" & sDate & "','" & eDate & "')  WHERE PENDER = 1 AND SIZE = A.SIZE AND SEASON = A.SEASON AND FREQUENCY = A.FREQUENCY) AS PENDERVOLUME " & _
			  "		FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "') A " & _
			  "		GROUP BY SIZE,SEASON,FREQUENCY,TOTALNUMBER " & _
			  "	) A WHERE A.TOTALSOLD > 0 OR A.TOTALAVAIL > 0  " & _
			  "	ORDER BY SEASON,SIZE,  FREQUENCY "
			  
		rs.open sql, cn, 0, 1
		sans = sans &  "<table style='border-collapse:collapse;' border=1>"
		sans = sans &  "<tr>"
		sans = sans &  "<th>SIZE</th>"
		sans = sans &  "<th>SEASON</th>"
		sans = sans &  "<th>FREQUENCY</th>"
		sans = sans &  "<th>NUM AVAILABLE</th>"
		sans = sans &  "<th>UNSOLD VOLUME</th>"
		sans = sans &  "<th>NUM SOLD</th>"
		sans = sans &  "<th>SOLD VOLUME</th>"
		sans = sans &  "<th>PENDERS</th>"
		sans = sans &  "<th>PENDER VOLUME</th>"
		sans = sans &  "<th>TOTAL VOLUME</th>"
		sans = sans &  "</tr>"
		
		dim unsoldamount, unsoldtotal, soldtotal, pendertotal
		unsoldamount = unsoldtotal = soldtotal = pendertotal = 00
		do while not rs.eof
			if rs.fields("size").value = 1 then
				if rs.fields("Season").value = "RED" then
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(13900 * rs.fields("totalavail").value)
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(7200 * rs.fields("totalavail").value)
					else
						unsoldamount = formatcurrency(0)
					end if
				else
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(5900 * rs.fields("totalavail").value)
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(0 * rs.fields("totalavail").value)
					else
						unsoldamount = formatcurrency(0)
					end if
				end if
			elseif rs.fields("size").value = 3 then
				if rs.fields("Season").value = "RED" then
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(31900 * rs.fields("totalavail").value)
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(18900 * rs.fields("totalavail").value)
					else
						unsoldamount = formatcurrency(0)
					end if
				else
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(8900 * rs.fields("totalavail").value)
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(7900 * rs.fields("totalavail").value)
					else
						unsoldamount = formatcurrency(0)
					end if
				end if
			elseif rs.fields("size").value = 4 then
				if rs.fields("season").value = "RED" then
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(44900 * rs.fields("totalavail").value)
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(0 * rs.fields("totalavail").value)
					else
						unsoldamount = formatcurrency(16900 * rs.fields("totalavail").value)
					end if
				else
					if rs.fields("frequency").value = "Annual" then
						unsoldamount =formatcurrency(12900 * rs.fields("totalavail").value)
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(0 * rs.fields("totalavail").value)
					else
						unsoldamount = formatcurrency(0 * rs.fields("totalavail").value)
					end if
				end if
			else
				unsoldamount = formatcurrency(0)
			end if
			unsoldtotal = unsoldtotal + ccur(unsoldamount)
			soldtotal = soldtotal + ccur(rs.fields("TotalVolume").value)
			if rs.fields("pendervolume").value & "" <> "" then pendertotal = pendertotal + ccur(rs.fields("pendervolume").value)
			
			sans = sans &  "<tr>"
			sans = sans &  "<td>" & rs.fields("Size").value & " BR</td>"
			sans = sans &  "<td align = right>" & rs.fields("Season").value & "</td>"
			sans = sans &  "<td align = right>" & rs.fields("Frequency").value & "</td>"
			sans = sans &  "<td align = right>" & rs.fields("totalavail").value & "</td>"
			sans = sans &  "<td align = right>" & unsoldamount
			sans = sans &  "</td>"
			sans = sans &  "<td align = right>" & rs.fields("totalsold").value & "</td>"
			sans = sans &  "<td align = right>" & formatcurrency(rs.fields("totalvolume").value) & "</td>"
			sans = sans &  "<td align = right>" 
				if rs.fields("penders").value & "" = "" then
					sans = sans &  "0" 
				else
					sans = sans &  rs.fields("penders").value 
				end if
			sans = sans &  "</td>"
			sans = sans &  "<td align = right>" 
				if rs.fields("pendervolume").value & "" = "" then
					sans = sans &  formatcurrency(0)
				else 
					sans = sans &  formatcurrency(rs.fields("pendervolume").value)
				end if
			sans = sans &  "</td>"
			sans = sans &  "<td align = right>" & formatcurrency(ccur(unsoldamount) + rs.fields("totalvolume").value) & "</td>"
			sans = sans &  "</tr>"
			rs.movenext
		loop
		sans = sans &  "<tr>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td align = right>" & formatcurrency(unsoldtotal) & "</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td align = right>" & formatcurrency(soldtotal) & "</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td align = right>" & formatcurrency(pendertotal) & "</td>"
		sans = sans &  "<td align = right>" & formatcurrency(unsoldtotal + soldtotal) & "</td>"
		sans = sans &  "</tr>"
		rs.close
		sans = sans &  "</table>"
		sans = sans &  "* TOTAL VOLUME DOES NOT INCLUDE PENDER VOLUME *"
		estates = sans
	end function
	
	function Townes
		sans =  "TOWNES INVENTORY (" & sDate & " - " & eDate & ")"
		sql = "SELECT * FROM ( " & _
			  "		SELECT SIZE, SEASON, FREQUENCY, TOTALNUMBER, SUM(TOTALSOLD) AS TOTALSOLD, SUM(TOTALAVAIL) AS TOTALAVAIL ,SUM(TOTALVOLUME) AS TOTALVOLUME,  " & _
			  "		(SELECT SUM(TOTALNUMBER) FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "')  WHERE PENDER = 1 AND SIZE = A.SIZE AND SEASON = A.SEASON AND FREQUENCY = A.FREQUENCY) AS PENDERS, " & _
			  "		(SELECT SUM(TOTALVOLUME) FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "')  WHERE PENDER = 1 AND SIZE = A.SIZE AND SEASON = A.SEASON AND FREQUENCY = A.FREQUENCY) AS PENDERVOLUME " & _
			  "		FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "') A " & _
			  "		GROUP BY SIZE,SEASON,FREQUENCY,TOTALNUMBER " & _
			  "	) A WHERE A.TOTALSOLD > 0 OR A.TOTALAVAIL > 0  " & _
			  "	ORDER BY SEASON,SIZE,  FREQUENCY "
			  
		rs.open sql, cn, 0, 1
		sans = sans &  "<table style='border-collapse:collapse;' border=1>"
		sans = sans &  "<tr>"
		sans = sans &  "<th>SIZE</th>"
		sans = sans &  "<th>SEASON</th>"
		sans = sans &  "<th>FREQUENCY</th>"
		sans = sans &  "<th>NUM AVAILABLE</th>"
		sans = sans &  "<th>UNSOLD VOLUME</th>"
		sans = sans &  "<th>NUM SOLD</th>"
		sans = sans &  "<th>SOLD VOLUME</th>"
		sans = sans &  "<th>PENDERS</th>"
		sans = sans &  "<th>PENDER VOLUME</th>"
		sans = sans &  "<th>TOTAL VOLUME</th>"
		sans = sans &  "</tr>"
		
		'dim unsoldamount, unsoldtotal, soldtotal, pendertotal
		unsoldamount = unsoldtotal = soldtotal = pendertotal = 00
		do while not rs.eof
			if rs.fields("size").value = 2 then
				if rs.fields("Season").value = "RED" then
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(16900 * rs.fields("totalavail").value)
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(10900 * rs.fields("totalavail").value)
					else
						unsoldamount = formatcurrency(0)
					end if
				else
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(8900 * rs.fields("totalavail").value)
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(6900 * rs.fields("totalavail").value)
					else
						unsoldamount = formatcurrency(0)
					end if
				end if

			elseif rs.fields("size").value = 4 then
				if rs.fields("season").value = "RED" then
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(30900 * rs.fields("totalavail").value)
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(16900 * rs.fields("totalavail").value)
					else
						unsoldamount = formatcurrency(0 * rs.fields("totalavail").value)
					end if
				else
					if rs.fields("frequency").value = "Annual" then
						unsoldamount =formatcurrency(12900 * rs.fields("totalavail").value)
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(8900 * rs.fields("totalavail").value)
					else
						unsoldamount = formatcurrency(0 * rs.fields("totalavail").value)
					end if
				end if
			else
				unsoldamount = formatcurrency(0)
			end if
			unsoldtotal = unsoldtotal + ccur(unsoldamount)
			soldtotal = soldtotal + ccur(rs.fields("TotalVolume").value)
			if rs.fields("pendervolume").value & "" <> "" then pendertotal = pendertotal + ccur(rs.fields("pendervolume").value)
			
			sans = sans &  "<tr>"
			sans = sans &  "<td>" & rs.fields("Size").value & " BR</td>"
			sans = sans &  "<td align = right>" & rs.fields("Season").value & "</td>"
			sans = sans &  "<td align = right>" & rs.fields("Frequency").value & "</td>"
			sans = sans &  "<td align = right>" & rs.fields("totalavail").value & "</td>"
			sans = sans &  "<td align = right>" & unsoldamount & "</td>"
			sans = sans &  "<td align = right>" & rs.fields("totalsold").value & "</td>"
			sans = sans &  "<td align = right>" & formatcurrency(rs.fields("totalvolume").value) & "</td>"
			sans = sans &  "<td align = right>" 
				if rs.fields("penders").value & "" = "" then
					sans = sans &  "0" 
				else
					sans = sans &  rs.fields("penders").value 
				end if
			sans = sans &  "</td>"
			sans = sans &  "<td align = right>" 
				if rs.fields("pendervolume").value & "" = "" then
					sans = sans &  formatcurrency(0)
				else 
					sans = sans &  formatcurrency(rs.fields("pendervolume").value)
				end if
			sans = sans &  "</td>"
			sans = sans &  "<td align = right>" & formatcurrency(ccur(unsoldamount) + rs.fields("totalvolume").value) & "</td>"
			sans = sans &  "</tr>"
			rs.movenext
		loop
		sans = sans &  "<tr>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td align = right>" & formatcurrency(unsoldtotal) & "</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td align = right>" & formatcurrency(soldtotal) & "</td>"
		sans = sans &  "<td>&nbsp;</td>"
		sans = sans &  "<td align = right>" & formatcurrency(pendertotal) & "</td>"
		sans = sans &  "<td align = right>" & formatcurrency(unsoldtotal + soldtotal) & "</td>"
		sans = sans &  "</tr>"
		rs.close
		sans = sans &  "</table>"
		sans = sans &  "* TOTAL VOLUME DOES NOT INCLUDE PENDER VOLUME *"
		townes = sans
	end function
	
	
	FUNCTION ALL_EXCEL
		slink = Replace(Replace("Inventory-" & Session.SessionID & "-" & Date & "-" & Time & ".xls", "/", "-"), ":", "-")
	    sFilename = "C:\inetpub\wwwroot\crms\reports\contracts\" & slink
	    
		Set xlApp = server.createobject("Excel.Application")
	    Set wb = xlApp.Workbooks.open("c:\inetpub\wwwroot\crms\reports\contracts\inventorytemplate.xls")
	    wb.SaveAs sFilename
	    Set wb = xlApp.Workbooks.Open(sFilename)
	    Set wsActiveSheet = wb.Worksheets(1)
	    
'********** COTTAGE **************;

		invtype = "Cottage"
		unsoldtotal = soldtotal = pendertotal = 0
	    wsactivesheet.range("A1").select
	    
	    
	    sql = "SELECT * FROM ( " & _
			  "		SELECT SIZE, SEASON, FREQUENCY, TOTALNUMBER, SUM(TOTALSOLD) AS TOTALSOLD, SUM(TOTALAVAIL) AS TOTALAVAIL ,SUM(TOTALVOLUME) AS TOTALVOLUME,  " & _
			  "		(SELECT SUM(TOTALNUMBER) FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "')  WHERE PENDER = 1 AND SIZE = A.SIZE AND SEASON = A.SEASON AND FREQUENCY = A.FREQUENCY) AS PENDERS, " & _
			  "		(SELECT SUM(TOTALVOLUME) FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "')  WHERE PENDER = 1 AND SIZE = A.SIZE AND SEASON = A.SEASON AND FREQUENCY = A.FREQUENCY) AS PENDERVOLUME " & _
			  "		FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "') A " & _
			  "		GROUP BY SIZE,SEASON,FREQUENCY,TOTALNUMBER " & _
			  "	) A WHERE A.TOTALSOLD > 0 OR A.TOTALAVAIL > 0  " & _
			  "	ORDER BY SEASON,SIZE,  FREQUENCY "
			  
		rs.open sql, cn, 0, 1

		unsoldamount = 0
		unsoldtotal = 0
		soldtotal = 0
		pendertotal = 0
		mftotal = 0
		
		row = 1
	    column = 0
		wsactivesheet.cells(row,1).value = "COTTAGE INVENTORY (" & sDate & " - " & eDate & ")"
		row = row + 1
		wsactivesheet.cells(row,1).value = "SIZE"
		wsactivesheet.cells(row,1).interior.colorindex = 15
		wsactivesheet.cells(row,1).interior.pattern = 1
		wsactivesheet.cells(row,1).font.bold = true
		wsactivesheet.cells(row,1).HorizontalAlignment = -4108
        wsactivesheet.cells(row,1).VerticalAlignment = -4108
        wsactivesheet.cells(row,1).WrapText = False
        wsactivesheet.cells(row,1).Orientation = 0
        wsactivesheet.cells(row,1).AddIndent = False
        wsactivesheet.cells(row,1).IndentLevel = 0
        wsactivesheet.cells(row,1).ShrinkToFit = False
        wsactivesheet.cells(row,1).ReadingOrder = -5002
        wsactivesheet.cells(row,1).MergeCells = False
        
		wsactivesheet.cells(row,2).value = "SEASON"
		wsactivesheet.cells(row,2).interior.colorindex = 15
		wsactivesheet.cells(row,2).interior.pattern = 1
		wsactivesheet.cells(row,2).font.bold = true
		wsactivesheet.cells(row,2).HorizontalAlignment = -4108
        wsactivesheet.cells(row,2).VerticalAlignment = -4108
        wsactivesheet.cells(row,2).WrapText = False
        wsactivesheet.cells(row,2).Orientation = 0
        wsactivesheet.cells(row,2).AddIndent = False
        wsactivesheet.cells(row,2).IndentLevel = 0
        wsactivesheet.cells(row,2).ShrinkToFit = False
        wsactivesheet.cells(row,2).ReadingOrder = -5002
        wsactivesheet.cells(row,2).MergeCells = False
        
        wsactivesheet.cells(row,3).value = "FREQUENCY"
		wsactivesheet.cells(row,3).interior.colorindex = 15
		wsactivesheet.cells(row,3).interior.pattern = 1
		wsactivesheet.cells(row,3).font.bold = true
		wsactivesheet.cells(row,3).HorizontalAlignment = -4108
        wsactivesheet.cells(row,3).VerticalAlignment = -4108
        wsactivesheet.cells(row,3).WrapText = False
        wsactivesheet.cells(row,3).Orientation = 0
        wsactivesheet.cells(row,3).AddIndent = False
        wsactivesheet.cells(row,3).IndentLevel = 0
        wsactivesheet.cells(row,3).ShrinkToFit = False
        wsactivesheet.cells(row,3).ReadingOrder = -5002
        wsactivesheet.cells(row,3).MergeCells = False
        
        wsactivesheet.cells(row,4).value = "NUM AVAILABLE"
		wsactivesheet.cells(row,4).interior.colorindex = 15
		wsactivesheet.cells(row,4).interior.pattern = 1
		wsactivesheet.cells(row,4).font.bold = true
		wsactivesheet.cells(row,4).HorizontalAlignment = -4108
        wsactivesheet.cells(row,4).VerticalAlignment = -4108
        wsactivesheet.cells(row,4).WrapText = False
        wsactivesheet.cells(row,4).Orientation = 0
        wsactivesheet.cells(row,4).AddIndent = False
        wsactivesheet.cells(row,4).IndentLevel = 0
        wsactivesheet.cells(row,4).ShrinkToFit = False
        wsactivesheet.cells(row,4).ReadingOrder = -5002
        wsactivesheet.cells(row,4).MergeCells = False
        
        wsactivesheet.cells(row,5).value = "UNSOLD VOLUME"
		wsactivesheet.cells(row,5).interior.colorindex = 15
		wsactivesheet.cells(row,5).interior.pattern = 1
		wsactivesheet.cells(row,5).font.bold = true
		wsactivesheet.cells(row,5).HorizontalAlignment = -4108
        wsactivesheet.cells(row,5).VerticalAlignment = -4108
        wsactivesheet.cells(row,5).WrapText = True
        wsactivesheet.cells(row,5).Orientation = 0
        wsactivesheet.cells(row,5).AddIndent = False
        wsactivesheet.cells(row,5).IndentLevel = 0
        wsactivesheet.cells(row,5).ShrinkToFit = False
        wsactivesheet.cells(row,5).ReadingOrder = -5002
        wsactivesheet.cells(row,5).MergeCells = False
        
        wsactivesheet.cells(row,6).value = "NUM SOLD"
		wsactivesheet.cells(row,6).interior.colorindex = 15
		wsactivesheet.cells(row,6).interior.pattern = 1
		wsactivesheet.cells(row,6).font.bold = true
		wsactivesheet.cells(row,6).HorizontalAlignment = -4108
        wsactivesheet.cells(row,6).VerticalAlignment = -4108
        wsactivesheet.cells(row,6).WrapText = True
        wsactivesheet.cells(row,6).Orientation = 0
        wsactivesheet.cells(row,6).AddIndent = False
        wsactivesheet.cells(row,6).IndentLevel = 0
        wsactivesheet.cells(row,6).ShrinkToFit = False
        wsactivesheet.cells(row,6).ReadingOrder = -5002
        wsactivesheet.cells(row,6).MergeCells = False
        
        wsactivesheet.cells(row,7).value = "SOLD VOLUME"
		wsactivesheet.cells(row,7).interior.colorindex = 15
		wsactivesheet.cells(row,7).interior.pattern = 1
		wsactivesheet.cells(row,7).font.bold = true
		wsactivesheet.cells(row,7).HorizontalAlignment = -4108
        wsactivesheet.cells(row,7).VerticalAlignment = -4108
        wsactivesheet.cells(row,7).WrapText = True
        wsactivesheet.cells(row,7).Orientation = 0
        wsactivesheet.cells(row,7).AddIndent = False
        wsactivesheet.cells(row,7).IndentLevel = 0
        wsactivesheet.cells(row,7).ShrinkToFit = False
        wsactivesheet.cells(row,7).ReadingOrder = -5002
        wsactivesheet.cells(row,7).MergeCells = False
        
        wsactivesheet.cells(row,8).value = "PENDERS"
		wsactivesheet.cells(row,8).interior.colorindex = 15
		wsactivesheet.cells(row,8).interior.pattern = 1
		wsactivesheet.cells(row,8).font.bold = true
		wsactivesheet.cells(row,8).HorizontalAlignment = -4108
        wsactivesheet.cells(row,8).VerticalAlignment = -4108
        wsactivesheet.cells(row,8).WrapText = False
        wsactivesheet.cells(row,8).Orientation = 0
        wsactivesheet.cells(row,8).AddIndent = False
        wsactivesheet.cells(row,8).IndentLevel = 0
        wsactivesheet.cells(row,8).ShrinkToFit = False
        wsactivesheet.cells(row,8).ReadingOrder = -5002
        wsactivesheet.cells(row,8).MergeCells = False
        
        wsactivesheet.cells(row,9).value = "PENDER VOLUME"
		wsactivesheet.cells(row,9).interior.colorindex = 15
		wsactivesheet.cells(row,9).interior.pattern = 1
		wsactivesheet.cells(row,9).font.bold = true
		wsactivesheet.cells(row,9).HorizontalAlignment = -4108
        wsactivesheet.cells(row,9).VerticalAlignment = -4108
        wsactivesheet.cells(row,9).WrapText = True
        wsactivesheet.cells(row,9).Orientation = 0
        wsactivesheet.cells(row,9).AddIndent = False
        wsactivesheet.cells(row,9).IndentLevel = 0
        wsactivesheet.cells(row,9).ShrinkToFit = False
        wsactivesheet.cells(row,9).ReadingOrder = -5002
        wsactivesheet.cells(row,9).MergeCells = False
        
        wsactivesheet.cells(row,10).value = "TOTAL VOLUME"
		wsactivesheet.cells(row,10).interior.colorindex = 15
		wsactivesheet.cells(row,10).interior.pattern = 1
		wsactivesheet.cells(row,10).font.bold = true
		wsactivesheet.cells(row,10).HorizontalAlignment = -4108
        wsactivesheet.cells(row,10).VerticalAlignment = -4108
        wsactivesheet.cells(row,10).WrapText = True
        wsactivesheet.cells(row,10).Orientation = 0
        wsactivesheet.cells(row,10).AddIndent = False
        wsactivesheet.cells(row,10).IndentLevel = 0
        wsactivesheet.cells(row,10).ShrinkToFit = False
        wsactivesheet.cells(row,10).ReadingOrder = -5002
        wsactivesheet.cells(row,10).MergeCells = False
        
        wsactivesheet.cells(row,11).value = "CURRENT SALES PRICE"
		wsactivesheet.cells(row,11).interior.colorindex = 15
		wsactivesheet.cells(row,11).interior.pattern = 1
		wsactivesheet.cells(row,11).font.bold = true
		wsactivesheet.cells(row,11).HorizontalAlignment = -4108
        wsactivesheet.cells(row,11).VerticalAlignment = -4108
        wsactivesheet.cells(row,11).WrapText = True
        wsactivesheet.cells(row,11).Orientation = 0
        wsactivesheet.cells(row,11).AddIndent = False
        wsactivesheet.cells(row,11).IndentLevel = 0
        wsactivesheet.cells(row,11).ShrinkToFit = False
        wsactivesheet.cells(row,11).ReadingOrder = -5002
        wsactivesheet.cells(row,11).MergeCells = False
        
        wsactivesheet.cells(row,12).value = ""
		wsactivesheet.cells(row,13).value = "MF PER UNIT"
		wsactivesheet.cells(row,13).interior.colorindex = 15
		wsactivesheet.cells(row,13).interior.pattern = 1
		wsactivesheet.cells(row,13).font.bold = true
		wsactivesheet.cells(row,13).HorizontalAlignment = -4108
        wsactivesheet.cells(row,13).VerticalAlignment = -4108
        wsactivesheet.cells(row,13).WrapText = True
        wsactivesheet.cells(row,13).Orientation = 0
        wsactivesheet.cells(row,13).AddIndent = False
        wsactivesheet.cells(row,13).IndentLevel = 0
        wsactivesheet.cells(row,13).ShrinkToFit = False
        wsactivesheet.cells(row,13).ReadingOrder = -5002
        wsactivesheet.cells(row,13).MergeCells = False
        
        wsactivesheet.cells(row,14).value = "MF FOR UNSOLD UNITS"
		wsactivesheet.cells(row,14).interior.colorindex = 15
		wsactivesheet.cells(row,14).interior.pattern = 1
		wsactivesheet.cells(row,14).font.bold = true
		wsactivesheet.cells(row,14).HorizontalAlignment = -4108
        wsactivesheet.cells(row,14).VerticalAlignment = -4108
        wsactivesheet.cells(row,14).WrapText = True
        wsactivesheet.cells(row,14).Orientation = 0
        wsactivesheet.cells(row,14).AddIndent = False
        wsactivesheet.cells(row,14).IndentLevel = 0
        wsactivesheet.cells(row,14).ShrinkToFit = False
        wsactivesheet.cells(row,14).ReadingOrder = -5002
        wsactivesheet.cells(row,14).MergeCells = False
		
		do while not rs.eof
			column = 0
			if rs.fields("size").value = 3 then
				if rs.fields("Season").value = "RED" then
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(22900 * rs.fields("totalavail").value)
						csp = 22900
						mfamount = 595
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(13900 * rs.fields("totalavail").value)
						csp = 13900
						mfamount = 595
					else
						unsoldamount = formatcurrency(0)
						mfamount = 595
					end if
				else
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(9900 * rs.fields("totalavail").value)
						csp = 9900
						mfamount = 595
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(6900 * rs.fields("totalavail").value)
						csp = 6900
						mfamount = 595
					else
						unsoldamount = formatcurrency(0)
						csp = 0
						mfamount = 595
					end if
				end if
			else
				unsoldamount = formatcurrency(0)
				mfamount = 595
			end if
			unsoldtotal = unsoldtotal + ccur(unsoldamount)
			soldtotal = soldtotal + ccur(rs.fields("TotalVolume").value)
			if rs.fields("pendervolume").value & "" <> "" then pendertotal = pendertotal + ccur(rs.fields("pendervolume").value)
			
			row = row + 1
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("Size").value & " BR"
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("Season").value
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("Frequency").value
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("totalavail").value 
			column = column + 1
			wsactivesheet.cells(row,column).value = unsoldamount
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("totalsold").value 
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(rs.fields("totalvolume").value) 
			column = column + 1
			if rs.fields("penders").value & "" = "" then
				wsactivesheet.cells(row,column).value =  "0" 
			else
				wsactivesheet.cells(row,column).value = rs.fields("penders").value 
			end if
			column = column + 1
			if rs.fields("pendervolume").value & "" = "" then
				wsactivesheet.cells(row,column).value =  formatcurrency(0)
			else 
				wsactivesheet.cells(row,column).value = formatcurrency(rs.fields("pendervolume").value)
			end if
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(ccur(unsoldamount) + rs.fields("totalvolume").value)
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(csp)
			column = column + 1
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(mfamount)
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(mfamount * rs.fields("totalavail").value)
			mftotal = mftotal + (mfamount * rs.fields("totalavail").value)
			rs.movenext
		loop
		row = row + 1 
		column = 1
		column = column + 1
		column = column + 1
		column = column + 1
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(unsoldtotal) 
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		column = column + 1
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(soldtotal)
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With 
		column = column + 1
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(pendertotal)
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(unsoldtotal + soldtotal)
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		column = column + 1
		column = column + 1
		column = column + 1
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(mftotal)
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		rs.close
		
'*********** TOWNES ****************'		
		invtype = "Townes"
		row = row + 2
		column = 0
		
		wsactivesheet.cells(row,1).value = "TOWNES INVENTORY (" & sDate & " - " & eDate & ")"

		row = row + 1
		
		wsactivesheet.cells(row,1).value = "SIZE"
		wsactivesheet.cells(row,1).interior.colorindex = 15
		wsactivesheet.cells(row,1).interior.pattern = 1
		wsactivesheet.cells(row,1).font.bold = true
		wsactivesheet.cells(row,1).HorizontalAlignment = -4108
        wsactivesheet.cells(row,1).VerticalAlignment = -4108
        wsactivesheet.cells(row,1).WrapText = True
        wsactivesheet.cells(row,1).Orientation = 0
        wsactivesheet.cells(row,1).AddIndent = False
        wsactivesheet.cells(row,1).IndentLevel = 0
        wsactivesheet.cells(row,1).ShrinkToFit = False
        wsactivesheet.cells(row,1).ReadingOrder = -5002
        wsactivesheet.cells(row,1).MergeCells = False
        
		wsactivesheet.cells(row,2).value = "SEASON"
		wsactivesheet.cells(row,2).interior.colorindex = 15
		wsactivesheet.cells(row,2).interior.pattern = 1
		wsactivesheet.cells(row,2).font.bold = true
		wsactivesheet.cells(row,2).HorizontalAlignment = -4108
        wsactivesheet.cells(row,2).VerticalAlignment = -4108
        wsactivesheet.cells(row,2).WrapText = True
        wsactivesheet.cells(row,2).Orientation = 0
        wsactivesheet.cells(row,2).AddIndent = False
        wsactivesheet.cells(row,2).IndentLevel = 0
        wsactivesheet.cells(row,2).ShrinkToFit = False
        wsactivesheet.cells(row,2).ReadingOrder = -5002
        wsactivesheet.cells(row,2).MergeCells = False
        
        wsactivesheet.cells(row,3).value = "FREQUENCY"
		wsactivesheet.cells(row,3).interior.colorindex = 15
		wsactivesheet.cells(row,3).interior.pattern = 1
		wsactivesheet.cells(row,3).font.bold = true
		wsactivesheet.cells(row,3).HorizontalAlignment = -4108
        wsactivesheet.cells(row,3).VerticalAlignment = -4108
        wsactivesheet.cells(row,3).WrapText = True
        wsactivesheet.cells(row,3).Orientation = 0
        wsactivesheet.cells(row,3).AddIndent = False
        wsactivesheet.cells(row,3).IndentLevel = 0
        wsactivesheet.cells(row,3).ShrinkToFit = False
        wsactivesheet.cells(row,3).ReadingOrder = -5002
        wsactivesheet.cells(row,3).MergeCells = False
        
        wsactivesheet.cells(row,4).value = "NUM AVAILABLE"
		wsactivesheet.cells(row,4).interior.colorindex = 15
		wsactivesheet.cells(row,4).interior.pattern = 1
		wsactivesheet.cells(row,4).font.bold = true
		wsactivesheet.cells(row,4).HorizontalAlignment = -4108
        wsactivesheet.cells(row,4).VerticalAlignment = -4108
        wsactivesheet.cells(row,4).WrapText = True
        wsactivesheet.cells(row,4).Orientation = 0
        wsactivesheet.cells(row,4).AddIndent = False
        wsactivesheet.cells(row,4).IndentLevel = 0
        wsactivesheet.cells(row,4).ShrinkToFit = False
        wsactivesheet.cells(row,4).ReadingOrder = -5002
        wsactivesheet.cells(row,4).MergeCells = False
        
        wsactivesheet.cells(row,5).value = "UNSOLD VOLUME"
		wsactivesheet.cells(row,5).interior.colorindex = 15
		wsactivesheet.cells(row,5).interior.pattern = 1
		wsactivesheet.cells(row,5).font.bold = true
		wsactivesheet.cells(row,5).HorizontalAlignment = -4108
        wsactivesheet.cells(row,5).VerticalAlignment = -4108
        wsactivesheet.cells(row,5).WrapText = True
        wsactivesheet.cells(row,5).Orientation = 0
        wsactivesheet.cells(row,5).AddIndent = False
        wsactivesheet.cells(row,5).IndentLevel = 0
        wsactivesheet.cells(row,5).ShrinkToFit = False
        wsactivesheet.cells(row,5).ReadingOrder = -5002
        wsactivesheet.cells(row,5).MergeCells = False
        
        wsactivesheet.cells(row,6).value = "NUM SOLD"
		wsactivesheet.cells(row,6).interior.colorindex = 15
		wsactivesheet.cells(row,6).interior.pattern = 1
		wsactivesheet.cells(row,6).font.bold = true
		wsactivesheet.cells(row,6).HorizontalAlignment = -4108
        wsactivesheet.cells(row,6).VerticalAlignment = -4108
        wsactivesheet.cells(row,6).WrapText = True
        wsactivesheet.cells(row,6).Orientation = 0
        wsactivesheet.cells(row,6).AddIndent = False
        wsactivesheet.cells(row,6).IndentLevel = 0
        wsactivesheet.cells(row,6).ShrinkToFit = False
        wsactivesheet.cells(row,6).ReadingOrder = -5002
        wsactivesheet.cells(row,6).MergeCells = False
        
        wsactivesheet.cells(row,7).value = "SOLD VOLUME"
		wsactivesheet.cells(row,7).interior.colorindex = 15
		wsactivesheet.cells(row,7).interior.pattern = 1
		wsactivesheet.cells(row,7).font.bold = true
		wsactivesheet.cells(row,7).HorizontalAlignment = -4108
        wsactivesheet.cells(row,7).VerticalAlignment = -4108
        wsactivesheet.cells(row,7).WrapText = True
        wsactivesheet.cells(row,7).Orientation = 0
        wsactivesheet.cells(row,7).AddIndent = False
        wsactivesheet.cells(row,7).IndentLevel = 0
        wsactivesheet.cells(row,7).ShrinkToFit = False
        wsactivesheet.cells(row,7).ReadingOrder = -5002
        wsactivesheet.cells(row,7).MergeCells = False
        
        wsactivesheet.cells(row,8).value = "PENDERS"
		wsactivesheet.cells(row,8).interior.colorindex = 15
		wsactivesheet.cells(row,8).interior.pattern = 1
		wsactivesheet.cells(row,8).font.bold = true
		wsactivesheet.cells(row,8).HorizontalAlignment = -4108
        wsactivesheet.cells(row,8).VerticalAlignment = -4108
        wsactivesheet.cells(row,8).WrapText = True
        wsactivesheet.cells(row,8).Orientation = 0
        wsactivesheet.cells(row,8).AddIndent = False
        wsactivesheet.cells(row,8).IndentLevel = 0
        wsactivesheet.cells(row,8).ShrinkToFit = False
        wsactivesheet.cells(row,8).ReadingOrder = -5002
        wsactivesheet.cells(row,8).MergeCells = False
        
        wsactivesheet.cells(row,9).value = "PENDER VOLUME"
		wsactivesheet.cells(row,9).interior.colorindex = 15
		wsactivesheet.cells(row,9).interior.pattern = 1
		wsactivesheet.cells(row,9).font.bold = true
		wsactivesheet.cells(row,9).HorizontalAlignment = -4108
        wsactivesheet.cells(row,9).VerticalAlignment = -4108
        wsactivesheet.cells(row,9).WrapText = True
        wsactivesheet.cells(row,9).Orientation = 0
        wsactivesheet.cells(row,9).AddIndent = False
        wsactivesheet.cells(row,9).IndentLevel = 0
        wsactivesheet.cells(row,9).ShrinkToFit = False
        wsactivesheet.cells(row,9).ReadingOrder = -5002
        wsactivesheet.cells(row,9).MergeCells = False
        
        wsactivesheet.cells(row,10).value = "TOTAL VOLUME"
		wsactivesheet.cells(row,10).interior.colorindex = 15
		wsactivesheet.cells(row,10).interior.pattern = 1
		wsactivesheet.cells(row,10).font.bold = true
		wsactivesheet.cells(row,10).HorizontalAlignment = -4108
        wsactivesheet.cells(row,10).VerticalAlignment = -4108
        wsactivesheet.cells(row,10).WrapText = True
        wsactivesheet.cells(row,10).Orientation = 0
        wsactivesheet.cells(row,10).AddIndent = False
        wsactivesheet.cells(row,10).IndentLevel = 0
        wsactivesheet.cells(row,10).ShrinkToFit = False
        wsactivesheet.cells(row,10).ReadingOrder = -5002
        wsactivesheet.cells(row,10).MergeCells = False
        
        wsactivesheet.cells(row,11).value = "CURRENT SALES PRICE"
		wsactivesheet.cells(row,11).interior.colorindex = 15
		wsactivesheet.cells(row,11).interior.pattern = 1
		wsactivesheet.cells(row,11).font.bold = true
		wsactivesheet.cells(row,11).HorizontalAlignment = -4108
        wsactivesheet.cells(row,11).VerticalAlignment = -4108
        wsactivesheet.cells(row,11).WrapText = True
        wsactivesheet.cells(row,11).Orientation = 0
        wsactivesheet.cells(row,11).AddIndent = False
        wsactivesheet.cells(row,11).IndentLevel = 0
        wsactivesheet.cells(row,11).ShrinkToFit = False
        wsactivesheet.cells(row,11).ReadingOrder = -5002
        wsactivesheet.cells(row,11).MergeCells = False
        
        wsactivesheet.cells(row,12).value = ""
		wsactivesheet.cells(row,13).value = "MF PER UNIT"
		wsactivesheet.cells(row,13).interior.colorindex = 15
		wsactivesheet.cells(row,13).interior.pattern = 1
		wsactivesheet.cells(row,13).font.bold = true
		wsactivesheet.cells(row,13).HorizontalAlignment = -4108
        wsactivesheet.cells(row,13).VerticalAlignment = -4108
        wsactivesheet.cells(row,13).WrapText = True
        wsactivesheet.cells(row,13).Orientation = 0
        wsactivesheet.cells(row,13).AddIndent = False
        wsactivesheet.cells(row,13).IndentLevel = 0
        wsactivesheet.cells(row,13).ShrinkToFit = False
        wsactivesheet.cells(row,13).ReadingOrder = -5002
        wsactivesheet.cells(row,13).MergeCells = False
        
        wsactivesheet.cells(row,14).value = "MF FOR UNSOLD UNITS"
		wsactivesheet.cells(row,14).interior.colorindex = 15
		wsactivesheet.cells(row,14).interior.pattern = 1
		wsactivesheet.cells(row,14).font.bold = true
		wsactivesheet.cells(row,14).HorizontalAlignment = -4108
        wsactivesheet.cells(row,14).VerticalAlignment = -4108
        wsactivesheet.cells(row,14).WrapText = True
        wsactivesheet.cells(row,14).Orientation = 0
        wsactivesheet.cells(row,14).AddIndent = False
        wsactivesheet.cells(row,14).IndentLevel = 0
        wsactivesheet.cells(row,14).ShrinkToFit = False
        wsactivesheet.cells(row,14).ReadingOrder = -5002
        wsactivesheet.cells(row,14).MergeCells = False
		
		sql = "SELECT * FROM ( " & _
			  "		SELECT SIZE, SEASON, FREQUENCY, TOTALNUMBER, SUM(TOTALSOLD) AS TOTALSOLD, SUM(TOTALAVAIL) AS TOTALAVAIL ,SUM(TOTALVOLUME) AS TOTALVOLUME,  " & _
			  "		(SELECT SUM(TOTALNUMBER) FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "')  WHERE PENDER = 1 AND SIZE = A.SIZE AND SEASON = A.SEASON AND FREQUENCY = A.FREQUENCY) AS PENDERS, " & _
			  "		(SELECT SUM(TOTALVOLUME) FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "')  WHERE PENDER = 1 AND SIZE = A.SIZE AND SEASON = A.SEASON AND FREQUENCY = A.FREQUENCY) AS PENDERVOLUME " & _
			  "		FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "') A " & _
			  "		GROUP BY SIZE,SEASON,FREQUENCY,TOTALNUMBER " & _
			  "	) A WHERE A.TOTALSOLD > 0 OR A.TOTALAVAIL > 0  " & _
			  "	ORDER BY SEASON,SIZE,  FREQUENCY "
			  
		rs.open sql, cn, 0, 1
		
		unsoldamount = 0
		unsoldtotal = 0
		soldtotal = 0
		pendertotal = 0
		mftotal = 0
		
		do while not rs.eof
			column = 0
			if rs.fields("size").value = 2 then
				if rs.fields("Season").value = "RED" then
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(16900 * rs.fields("totalavail").value)
						csp = 16900
						mfamount = 475
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(10900 * rs.fields("totalavail").value)
						csp = 10900
						mfamount = 475
					else
						unsoldamount = formatcurrency(0)
						csp = 0
						mfamount = 475
					end if
				else
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(8900 * rs.fields("totalavail").value)
						csp = 8900
						mfamount = 475
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(6900 * rs.fields("totalavail").value)
						csp = 6900
						mfamount = 475
					else
						unsoldamount = formatcurrency(0)
						csp = 0
						mfamount = 475
					end if
				end if

			elseif rs.fields("size").value = 4 then
				if rs.fields("season").value = "RED" then
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(30900 * rs.fields("totalavail").value)
						csp = 30900
						mfamount = 765
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(16900 * rs.fields("totalavail").value)
						csp = 16900
						mfamount = 765
					else
						unsoldamount = formatcurrency(0 * rs.fields("totalavail").value)
						csp = 0
						mfamount = 765
					end if
				else
					if rs.fields("frequency").value = "Annual" then
						unsoldamount =formatcurrency(12900 * rs.fields("totalavail").value)
						csp = 12900
						mfamount = 765
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(8900 * rs.fields("totalavail").value)
						csp = 8900
						mfamount = 765
					else
						unsoldamount = formatcurrency(0 * rs.fields("totalavail").value)
						csp = 0
						mfamount = 765
					end if
				end if
			else
				unsoldamount = formatcurrency(0)
				csp = 0
				mfamount = 0
			end if
			unsoldtotal = unsoldtotal + ccur(unsoldamount)
			soldtotal = soldtotal + ccur(rs.fields("TotalVolume").value)
			if rs.fields("pendervolume").value & "" <> "" then pendertotal = pendertotal + ccur(rs.fields("pendervolume").value)
			
			row = row + 1
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("Size").value & " BR"
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("Season").value
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("Frequency").value
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("totalavail").value 
			column = column + 1
			wsactivesheet.cells(row,column).value = unsoldamount
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("totalsold").value 
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(rs.fields("totalvolume").value) 
			column = column + 1
			if rs.fields("penders").value & "" = "" then
				wsactivesheet.cells(row,column).value =   "0" 
			else
				wsactivesheet.cells(row,column).value = rs.fields("penders").value 
			end if
			column = column + 1
			if rs.fields("pendervolume").value & "" = "" then
				wsactivesheet.cells(row,column).value = formatcurrency(0)
			else 
				wsactivesheet.cells(row,column).value = formatcurrency(rs.fields("pendervolume").value)
			end if
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(ccur(unsoldamount) + rs.fields("totalvolume").value) 
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(csp)
			column = column + 1
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(mfamount)
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(mfamount * rs.fields("totalavail").value)
			mftotal = mftotal + (rs.fields("totalavail").value * mfamount)
			rs.movenext
		loop
		row = row + 1
		column = 0
		column = column + 1
		column = column + 1
		column = column + 1
		column = column + 1
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(unsoldtotal) 
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
	    With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		column = column + 1
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(soldtotal) 
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		column = column + 1
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(pendertotal) 
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(unsoldtotal + soldtotal) 
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		column = column + 1
		column = column + 1
		column = column + 1
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(mftotal)
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		rs.close

'************ Estates *************'

		invtype = "Estates"
		unsoldamount = unsoldtotal = soldtotal = pendertotal = 0
		row = row + 2
		column = 0
		
		wsactivesheet.cells(row,1).value = "ESTATES INVENTORY (" & sDate & " - " & eDate & ")"
		row = row + 1
		
		wsactivesheet.cells(row,1).value = "SIZE"
		wsactivesheet.cells(row,1).interior.colorindex = 15
		wsactivesheet.cells(row,1).interior.pattern = 1
		wsactivesheet.cells(row,1).font.bold = true
		wsactivesheet.cells(row,1).HorizontalAlignment = -4108
        wsactivesheet.cells(row,1).VerticalAlignment = -4108
        wsactivesheet.cells(row,1).WrapText = True
        wsactivesheet.cells(row,1).Orientation = 0
        wsactivesheet.cells(row,1).AddIndent = False
        wsactivesheet.cells(row,1).IndentLevel = 0
        wsactivesheet.cells(row,1).ShrinkToFit = False
        wsactivesheet.cells(row,1).ReadingOrder = -5002
        wsactivesheet.cells(row,1).MergeCells = False
        
		wsactivesheet.cells(row,2).value = "SEASON"
		wsactivesheet.cells(row,2).interior.colorindex = 15
		wsactivesheet.cells(row,2).interior.pattern = 1
		wsactivesheet.cells(row,2).font.bold = true
		wsactivesheet.cells(row,2).HorizontalAlignment = -4108
        wsactivesheet.cells(row,2).VerticalAlignment = -4108
        wsactivesheet.cells(row,2).WrapText = True
        wsactivesheet.cells(row,2).Orientation = 0
        wsactivesheet.cells(row,2).AddIndent = False
        wsactivesheet.cells(row,2).IndentLevel = 0
        wsactivesheet.cells(row,2).ShrinkToFit = False
        wsactivesheet.cells(row,2).ReadingOrder = -5002
        wsactivesheet.cells(row,2).MergeCells = False
        
        wsactivesheet.cells(row,3).value = "FREQUENCY"
		wsactivesheet.cells(row,3).interior.colorindex = 15
		wsactivesheet.cells(row,3).interior.pattern = 1
		wsactivesheet.cells(row,3).font.bold = true
		wsactivesheet.cells(row,3).HorizontalAlignment = -4108
        wsactivesheet.cells(row,3).VerticalAlignment = -4108
        wsactivesheet.cells(row,3).WrapText = True
        wsactivesheet.cells(row,3).Orientation = 0
        wsactivesheet.cells(row,3).AddIndent = False
        wsactivesheet.cells(row,3).IndentLevel = 0
        wsactivesheet.cells(row,3).ShrinkToFit = False
        wsactivesheet.cells(row,3).ReadingOrder = -5002
        wsactivesheet.cells(row,3).MergeCells = False
        
        wsactivesheet.cells(row,4).value = "NUM AVAILABLE"
		wsactivesheet.cells(row,4).interior.colorindex = 15
		wsactivesheet.cells(row,4).interior.pattern = 1
		wsactivesheet.cells(row,4).font.bold = true
		wsactivesheet.cells(row,4).HorizontalAlignment = -4108
        wsactivesheet.cells(row,4).VerticalAlignment = -4108
        wsactivesheet.cells(row,4).WrapText = True
        wsactivesheet.cells(row,4).Orientation = 0
        wsactivesheet.cells(row,4).AddIndent = False
        wsactivesheet.cells(row,4).IndentLevel = 0
        wsactivesheet.cells(row,4).ShrinkToFit = False
        wsactivesheet.cells(row,4).ReadingOrder = -5002
        wsactivesheet.cells(row,4).MergeCells = False
        
        wsactivesheet.cells(row,5).value = "UNSOLD VOLUME"
		wsactivesheet.cells(row,5).interior.colorindex = 15
		wsactivesheet.cells(row,5).interior.pattern = 1
		wsactivesheet.cells(row,5).font.bold = true
		wsactivesheet.cells(row,5).HorizontalAlignment = -4108
        wsactivesheet.cells(row,5).VerticalAlignment = -4108
        wsactivesheet.cells(row,5).WrapText = True
        wsactivesheet.cells(row,5).Orientation = 0
        wsactivesheet.cells(row,5).AddIndent = False
        wsactivesheet.cells(row,5).IndentLevel = 0
        wsactivesheet.cells(row,5).ShrinkToFit = False
        wsactivesheet.cells(row,5).ReadingOrder = -5002
        wsactivesheet.cells(row,5).MergeCells = False
        
        wsactivesheet.cells(row,6).value = "NUM SOLD"
		wsactivesheet.cells(row,6).interior.colorindex = 15
		wsactivesheet.cells(row,6).interior.pattern = 1
		wsactivesheet.cells(row,6).font.bold = true
		wsactivesheet.cells(row,6).HorizontalAlignment = -4108
        wsactivesheet.cells(row,6).VerticalAlignment = -4108
        wsactivesheet.cells(row,6).WrapText = True
        wsactivesheet.cells(row,6).Orientation = 0
        wsactivesheet.cells(row,6).AddIndent = False
        wsactivesheet.cells(row,6).IndentLevel = 0
        wsactivesheet.cells(row,6).ShrinkToFit = False
        wsactivesheet.cells(row,6).ReadingOrder = -5002
        wsactivesheet.cells(row,6).MergeCells = False
        
        wsactivesheet.cells(row,7).value = "SOLD VOLUME"
		wsactivesheet.cells(row,7).interior.colorindex = 15
		wsactivesheet.cells(row,7).interior.pattern = 1
		wsactivesheet.cells(row,7).font.bold = true
		wsactivesheet.cells(row,7).HorizontalAlignment = -4108
        wsactivesheet.cells(row,7).VerticalAlignment = -4108
        wsactivesheet.cells(row,7).WrapText = True
        wsactivesheet.cells(row,7).Orientation = 0
        wsactivesheet.cells(row,7).AddIndent = False
        wsactivesheet.cells(row,7).IndentLevel = 0
        wsactivesheet.cells(row,7).ShrinkToFit = False
        wsactivesheet.cells(row,7).ReadingOrder = -5002
        wsactivesheet.cells(row,7).MergeCells = False
        
        wsactivesheet.cells(row,8).value = "PENDERS"
		wsactivesheet.cells(row,8).interior.colorindex = 15
		wsactivesheet.cells(row,8).interior.pattern = 1
		wsactivesheet.cells(row,8).font.bold = true
		wsactivesheet.cells(row,8).HorizontalAlignment = -4108
        wsactivesheet.cells(row,8).VerticalAlignment = -4108
        wsactivesheet.cells(row,8).WrapText = True
        wsactivesheet.cells(row,8).Orientation = 0
        wsactivesheet.cells(row,8).AddIndent = False
        wsactivesheet.cells(row,8).IndentLevel = 0
        wsactivesheet.cells(row,8).ShrinkToFit = False
        wsactivesheet.cells(row,8).ReadingOrder = -5002
        wsactivesheet.cells(row,8).MergeCells = False
        
        wsactivesheet.cells(row,9).value = "PENDER VOLUME"
		wsactivesheet.cells(row,9).interior.colorindex = 15
		wsactivesheet.cells(row,9).interior.pattern = 1
		wsactivesheet.cells(row,9).font.bold = true
		wsactivesheet.cells(row,9).HorizontalAlignment = -4108
        wsactivesheet.cells(row,9).VerticalAlignment = -4108
        wsactivesheet.cells(row,9).WrapText = True
        wsactivesheet.cells(row,9).Orientation = 0
        wsactivesheet.cells(row,9).AddIndent = False
        wsactivesheet.cells(row,9).IndentLevel = 0
        wsactivesheet.cells(row,9).ShrinkToFit = False
        wsactivesheet.cells(row,9).ReadingOrder = -5002
        wsactivesheet.cells(row,9).MergeCells = False
        
        wsactivesheet.cells(row,10).value = "TOTAL VOLUME"
		wsactivesheet.cells(row,10).interior.colorindex = 15
		wsactivesheet.cells(row,10).interior.pattern = 1
		wsactivesheet.cells(row,10).font.bold = true
		wsactivesheet.cells(row,10).HorizontalAlignment = -4108
        wsactivesheet.cells(row,10).VerticalAlignment = -4108
        wsactivesheet.cells(row,10).WrapText = True
        wsactivesheet.cells(row,10).Orientation = 0
        wsactivesheet.cells(row,10).AddIndent = False
        wsactivesheet.cells(row,10).IndentLevel = 0
        wsactivesheet.cells(row,10).ShrinkToFit = False
        wsactivesheet.cells(row,10).ReadingOrder = -5002
        wsactivesheet.cells(row,10).MergeCells = False
        
        wsactivesheet.cells(row,11).value = "CURRENT SALES PRICE"
		wsactivesheet.cells(row,11).interior.colorindex = 15
		wsactivesheet.cells(row,11).interior.pattern = 1
		wsactivesheet.cells(row,11).font.bold = true
		wsactivesheet.cells(row,11).HorizontalAlignment = -4108
        wsactivesheet.cells(row,11).VerticalAlignment = -4108
        wsactivesheet.cells(row,11).WrapText = True
        wsactivesheet.cells(row,11).Orientation = 0
        wsactivesheet.cells(row,11).AddIndent = False
        wsactivesheet.cells(row,11).IndentLevel = 0
        wsactivesheet.cells(row,11).ShrinkToFit = False
        wsactivesheet.cells(row,11).ReadingOrder = -5002
        wsactivesheet.cells(row,11).MergeCells = False
        
        wsactivesheet.cells(row,12).value = ""
		wsactivesheet.cells(row,13).value = "MF PER UNIT"
		wsactivesheet.cells(row,13).interior.colorindex = 15
		wsactivesheet.cells(row,13).interior.pattern = 1
		wsactivesheet.cells(row,13).font.bold = true
		wsactivesheet.cells(row,13).HorizontalAlignment = -4108
        wsactivesheet.cells(row,13).VerticalAlignment = -4108
        wsactivesheet.cells(row,13).WrapText = True
        wsactivesheet.cells(row,13).Orientation = 0
        wsactivesheet.cells(row,13).AddIndent = False
        wsactivesheet.cells(row,13).IndentLevel = 0
        wsactivesheet.cells(row,13).ShrinkToFit = False
        wsactivesheet.cells(row,13).ReadingOrder = -5002
        wsactivesheet.cells(row,13).MergeCells = False
        
        wsactivesheet.cells(row,14).value = "MF FOR UNSOLD UNITS"
		wsactivesheet.cells(row,14).interior.colorindex = 15
		wsactivesheet.cells(row,14).interior.pattern = 1
		wsactivesheet.cells(row,14).font.bold = true
		wsactivesheet.cells(row,14).HorizontalAlignment = -4108
        wsactivesheet.cells(row,14).VerticalAlignment = -4108
        wsactivesheet.cells(row,14).WrapText = True
        wsactivesheet.cells(row,14).Orientation = 0
        wsactivesheet.cells(row,14).AddIndent = False
        wsactivesheet.cells(row,14).IndentLevel = 0
        wsactivesheet.cells(row,14).ShrinkToFit = False
        wsactivesheet.cells(row,14).ReadingOrder = -5002
        wsactivesheet.cells(row,14).MergeCells = False
        
		sql = "SELECT * FROM ( " & _
			  "		SELECT SIZE, SEASON, FREQUENCY, TOTALNUMBER, SUM(TOTALSOLD) AS TOTALSOLD, SUM(TOTALAVAIL) AS TOTALAVAIL ,SUM(TOTALVOLUME) AS TOTALVOLUME,  " & _
			  "		(SELECT SUM(TOTALNUMBER) FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('estates','" & sDate & "','" & eDate & "')  WHERE PENDER = 1 AND SIZE = A.SIZE AND SEASON = A.SEASON AND FREQUENCY = A.FREQUENCY) AS PENDERS, " & _
			  "		(SELECT SUM(TOTALVOLUME) FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('estates','" & sDate & "','" & eDate & "')  WHERE PENDER = 1 AND SIZE = A.SIZE AND SEASON = A.SEASON AND FREQUENCY = A.FREQUENCY) AS PENDERVOLUME " & _
			  "		FROM [CRMSData].[dbo].[ufnUnitSalesByDate] ('" & invtype & "','" & sDate & "','" & eDate & "') A " & _
			  "		GROUP BY SIZE,SEASON,FREQUENCY,TOTALNUMBER " & _
			  "	) A WHERE A.TOTALSOLD > 0 OR A.TOTALAVAIL > 0  " & _
			  "	ORDER BY SEASON,SIZE,  FREQUENCY "
			  
		rs.open sql, cn, 0, 1
		
		dim unsoldamount, unsoldtotal, soldtotal, pendertotal
		unsoldamount = 0
		unsoldtotal = 0
		soldtotal = 0
		pendertotal = 0
		mftotal = 0
		
		do while not rs.eof
			column = 0
			if rs.fields("size").value = 1 then
				if rs.fields("Season").value = "RED" then
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(13900 * rs.fields("totalavail").value)
						csp = 13900
						mfamount = 300
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(7200 * rs.fields("totalavail").value)
						csp = 7200
						mfamount = 300
					else
						csp = 0
						unsoldamount = formatcurrency(0)
						mfamount = 300
					end if
				else
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(5900 * rs.fields("totalavail").value)
						csp = 5900
						mfamount = 300
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(0 * rs.fields("totalavail").value)
						csp = 0
						mfamount = 300
					else
						csp = 0 
						unsoldamount = formatcurrency(0)
						mfamount = 300
					end if
				end if
			elseif rs.fields("size").value = 3 then
				if rs.fields("Season").value = "RED" then
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(31900 * rs.fields("totalavail").value)
						csp = 31900
						mfamount = 595
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(18900 * rs.fields("totalavail").value)
						csp = 18900
						mfamount = 595
					else
						unsoldamount = formatcurrency(0)
						csp = 0
						mfamount = 595
					end if
				else
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(8900 * rs.fields("totalavail").value)
						csp = 8900
						mfamount = 595
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(7900 * rs.fields("totalavail").value)
						csp = 7900
						mfamount = 595
					else
						unsoldamount = formatcurrency(0)
						csp = 0
						mfamount = 595
					end if
				end if
			elseif rs.fields("size").value = 4 then
				if rs.fields("season").value = "RED" then
					if rs.fields("frequency").value = "Annual" then
						unsoldamount = formatcurrency(44900 * rs.fields("totalavail").value)
						csp = 44900
						mfamount = 895
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(0 * rs.fields("totalavail").value)
						csp = 0
						mfamount = 895
					else
						unsoldamount = formatcurrency(16900 * rs.fields("totalavail").value)
						csp = 16900
						mfamount = 895
					end if
				else
					if rs.fields("frequency").value = "Annual" then
						unsoldamount =formatcurrency(12900 * rs.fields("totalavail").value)
						csp = 12900
						mfamount = 895
					elseif rs.fields("frequency").value = "Biennial" then
						unsoldamount = formatcurrency(0 * rs.fields("totalavail").value)
						csp = 0
						mfamount = 895
					else
						unsoldamount = formatcurrency(0 * rs.fields("totalavail").value)
						csp = 0
						mfamount = 895
					end if
				end if
			else
				unsoldamount = formatcurrency(0)
				csp  = 0
				mfamount = 0
			end if
			unsoldtotal = unsoldtotal + ccur(unsoldamount)
			soldtotal = soldtotal + ccur(rs.fields("TotalVolume").value)
			if rs.fields("pendervolume").value & "" <> "" then pendertotal = pendertotal + ccur(rs.fields("pendervolume").value)
			
			row = row + 1
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("Size").value & " BR"
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("Season").value 
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("Frequency").value
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("totalavail").value
			column = column + 1
			wsactivesheet.cells(row,column).value = unsoldamount
			column = column + 1
			wsactivesheet.cells(row,column).value = rs.fields("totalsold").value 
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(rs.fields("totalvolume").value) 
			column = column + 1
			if rs.fields("penders").value & "" = "" then
				wsactivesheet.cells(row,column).value =  "0" 
			else
				wsactivesheet.cells(row,column).value = rs.fields("penders").value 
			end if
			column = column + 1
			if rs.fields("pendervolume").value & "" = "" then
				wsactivesheet.cells(row,column).value = formatcurrency(0)
			else 
				wsactivesheet.cells(row,column).value = formatcurrency(rs.fields("pendervolume").value)
			end if
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(ccur(unsoldamount) + rs.fields("totalvolume").value) 
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(csp)
			column = column + 1
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(mfamount)
			column = column + 1
			wsactivesheet.cells(row,column).value = formatcurrency(mfamount * rs.fields("totalavail").value)
			mftotal = mftotal + (rs.fields("totalavail").value * mfamount)
			rs.movenext
		loop
		row = row + 1
		column = 0
		column = column + 1
		column = column + 1
		column = column + 1
		column = column + 1
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(unsoldtotal)
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		column = column + 1
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(soldtotal)
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		column = column + 1
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(pendertotal)
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(unsoldtotal + soldtotal) 
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		column = column + 1
		column = column + 1
		column = column + 1
		column = column + 1
		wsactivesheet.cells(row,column).value = formatcurrency(mftotal)
		wsactivesheet.cells(row,column).font.bold = true
		wsactivesheet.cells(row,column).Borders(5).LineStyle = -4142
	    wsactivesheet.cells(row,column).Borders(6).LineStyle = -4142
		With wsactivesheet.cells(row,column).Borders(7)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
	    With wsactivesheet.cells(row,column).Borders(8)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(9)
	        .LineStyle = -4119
	        .Weight = 4
	        .ColorIndex = -4105
	    End With
	    With wsactivesheet.cells(row,column).Borders(10)
	        .LineStyle = 1
	        .Weight = 2
	        .ColorIndex = 16
	    End With
		rs.close
		
		
		wsActiveSheet.Cells.Select
	    wsActiveSheet.Cells.EntireColumn.AutoFit
	   
	   	wsactivesheet.cells(1,"A").select
	    wb.Save
	    wb.Close
	    Set xlApp = Nothing
	    Set wb = Nothing
	    Set wsActiveSheet = Nothing
		
		sAns = "Your Excel document has been created.<br>Click on the file name to open.<br>"
		sAns = sAns & "<a target = '_blank' href = '../contracts/" & slink & "'>" & slink & "</a>"
		response.write sAns
END FUNCTION
%>
		