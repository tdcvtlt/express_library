<%
DBName = "CRMSNet"
DBUser = "asp"
DBPass = "aspnet"
	dim cn
	dim rs
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	cn.open DBName, DBUser, DBPass
	
	dim sSQL
	sSQL = "SELECT t_SalesInventory.typeid as InventoryTypeID, t_SalesInventory.WeekTypeID, t_Unit.name as UnitName, t_SalesInventory.Week, t_SoldInventory.FrequencyID, t_SoldInventory.OccupancyYear, t_ComboItems.ComboItem, t_SalesInventory.StatusID as Status FROM   ((t_Unit t_Unit INNER JOIN t_SalesInventory t_SalesInventory ON t_Unit.UnitID=t_SalesInventory.UnitID) INNER JOIN t_ComboItems t_ComboItems ON t_SalesInventory.WeekTypeID=t_ComboItems.ComboItemID) LEFT OUTER JOIN t_SoldInventory t_SoldInventory ON t_SalesInventory.SalesInventoryID=t_SoldInventory.SalesInventoryID ORDER BY t_Unit.Name, t_SalesInventory.Week " 't_SalesInventory.InventoryTypeID"
	rs.open sSQL, cn, 3, 3 '"select * from t_Unit u inner join t_Salesinventory i on i.unitid = u.unitid left outer join t_Soldinventory s on s.salesinventoryid = i.salesinventoryid order by unitname, week", cn, 3, 3
	
	dim lastUnit
	dim lastWeek
	lastUnit = ""
	lastWeek = 0
	response.write "<table border = 1>"
	dim bAnnual
	dim bBiAnnual
	dim bTriAnnual
	dim bOddSold
	dim bEvenSold
	dim bAnnualSold
	dim bTri1Sold
	dim bTri2Sold
	dim bTri3Sold
	dim bHolder
	dim i, units
	i=0
	units = 0
	do while not rs.eof 
		
		
		'Check last week
		if lastWeek <> rs.fields("Week").value then
			dim sWrite
			sWrite = "<tr><td>&nbsp;</td><td style='border-bottom:solid black thin;'>" & lastWeek & "</td><td style='border-bottom:solid black thin;'>"
'******
			if bBiAnnual then
				if bOddSold then
    			    sWrite = sWrite & "Even"
    			else
        			sWrite = sWrite & "Odd"
				end if
			else
				if bTriAnnual then
    				if bTri3Sold then
            			sWrite = sWrite & "None"
        			else
        				if bTri2Sold then
                			sWrite = sWrite & "Triannual - 1 - " & Calc_Tri_Availability(bTri1YearSold,bTri2YearSold) 
            			else
                			sWrite = sWrite & "Triannual - 2 - " & Calc_Tri_Availability(bTri1YearSold,bTri1YearSold)
        				end if
    				end if
    			else
        			sWrite = sWrite & "All Avail"
				end if
			end if
			sWrite = sWrite & "</td></tr>"
'******
			lastWeek = rs.fields("Week").value
			if i > 0 then 
				if ((bAnnual and bAnnualSold) or (bBiAnnual and bOddSold and bEvenSold) or (bTriAnnual and bTri3Sold)) then
				else
					response.write sWrite '"<tr><td>&nbsp</td><td>" & lastweek & "</td><td>&nbsp</td></tr>"
				end if
			end if
			bAnnual = false
			bBiAnnual = false
			bTriAnnual = false
			bOddSold = false
			bEvenSold = false
			bAnnualSold = false
			bTri1Sold = false
			bTri2Sold = false
			bTri3Sold = false
			bTri1YearSold = ""
			bTri2YearSold = ""

		end if
		
		'Check last unit
		if lastUnit <> rs.fields("UnitName").value then 
			lastUnit = rs.fields("UnitName").value
			if units mod 4 = 0 and units <> 0 then
				response.write "</table></td></tr><tr><td valign = top><table width='100%'>"
			elseif units = 0 then
				response.write "<tr><td valign = top><table width='100%'>"
			else 
				response.write "</table></td><td valign=top><table width='100%'>"
			end if
			units = units + 1
			response.write "<tr><td colspan=3><b>" & lastUnit & "</b></td></tr>"
		end if
		
		'Check availability
		if isnull(rs.fields("FrequencyID").value) then
			bHolder = false
		else
		    if rs.fields("FrequencyID").value = 1 then
		    	bAnnual = true
		        bAnnualSold = true
		    else
		    	if rs.fields("FrequencyID").value = 2 then
		        	bBiAnnual = true
		            if rs.fields("OccupancyYear").value mod 2 = 0 then
		            	bEvenSold = true
		            else
		                bOddSold= true
		            end if
		        else
		            if rs.fields("FrequencyID").value = 3 then
		            	bTriAnnual = true
		                if bTri1Sold and bTri2Sold then
		                	bTri3Sold = true
		                else
		                    if bTri1Sold then
		                    	bTri2Sold = true
		                    	bTri2YearSold = rs.fields("OccupancyYear").value
		                    else
		                        bTri1Sold = true
		                        bTri1YearSold = rs.fields("OccupancyYear").value
		              		end if
		              	end if
		            end if
		        end if
			end if
		end if
		rs.movenext
		i=i+1
		
	loop
	response.write "</table></td></tr></table>"
	rs.close
	cn.close
	set rs = nothing
	set cn = nothing

function Calc_Tri_Availability(byval strStart, byval strEnd)
	if strStart = "" then
		ans = ""
	else
		if strEnd = "" then
			strEnd = strStart
		end if
		if strStart = strEnd then
			'Means that 2 occ years are avail
			'Determine if this or next or after next are avail
			if strStart >= year(date) then
				do while strStart >= year(date)
					strStart = strStart - 3
				loop
				strStart = strStart + 4
				strEnd = strStart + 1
			else
				do while strStart <= year(date)
					strStart = strStart + 3
				loop
				strStart = strStart -2
				strEnd = strStart + 1
			end if
			if strEnd = year(date) + 3 then
				strEnd = strStart
				strStart = year(date)
			end if
			strEnd = ", " & strEnd
		else
			diff = abs(strStart - strEnd)
			if strStart > strEnd then
				strStart = strStart + diff
			else
				strStart = strEnd + diff
			end if
			if strStart >= year(date) then
				do while strStart >= year(date)
					strStart = strStart - 3
				loop
				strStart = strStart + 3
			else
				do while strStart <= year(date)
					strStart = strStart + 3
				loop
				strStart = strStart -3
			end if
			strEnd = ""
		end if
		ans = strStart & strEnd
	end if
	Calc_Tri_Availability = ans
end function

%>