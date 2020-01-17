<%
	dim cn, rs
	
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	


DBName = "CRMSNET"
DBUser = "asp"
DBPass = "aspnet"

	cn.open DBName, DBUser, DBPass
	
	if request("F") = "report" then
		sql = "select p.firstname + ' ' +  p.lastname as Guest,t.TourID, c.name as Campaign, ts.comboitem as [Tour Status], per.firstname + ' ' + per.lastname as Rep, tit.comboitem as Title, cs.comboitem as [Contract Status], m.salesvolume as Volume " & _ 
				"from t_Prospect p  " & _ 
				"	inner join t_Tour t on t.prospectid = p.prospectid " & _ 
				"	left outer join t_Campaign c on c.campaignid = t.campaignid " & _ 
				"	left outer join t_Comboitems ts on ts.comboitemid =t.statusid " & _ 
				"	left outer join t_Personneltrans pt on pt.KEYVALUE = T.tourid AND PT.KEYFIELD = 'tourid'  " & _ 
				"	left outer join t_Personnel per on per.personnelid = pt.personnelid " & _ 
				"	left outer join t_Comboitems tit on tit.comboitemid = pt.titleid " & _ 
				"	left outer join t_Contract con on con.tourid = t.tourid " & _ 
				"	left outer join t_Mortgage m on m.contractid = con.contractid " & _ 
				"	left outer join t_Comboitems cs on cs.comboitemid = con.statusid " & _ 
				"where t.tourdate = '" & request("sdate") & "' " & _ 
				"	and tit.comboitem in ('Sales Executive', 'to', 'exit sales executive') " & _ 
				"	and (cs.comboitem is null or cs.comboitem in ('pender', 'active', 'suspense','pender-inv')) " & _ 
				"order by t.tourid, tit.comboitem"
	
		rs.open sql, cn, 0, 1
		if rs.eof and rs.bof then
			response.write "NO RECORDS"
		else
			response.write "<table STYLE='BORDER-COLLAPSE:COLLAPSE;' border = 1>"
			response.write "<tr>"
			for i = 0 to rs.fields.count - 1
				response.write "<th>" & rs.fields(i).name & "</th>"
			next
			response.write "</tr>"
			
			lasttourid = 0
			do while not rs.eof
				if lasttourid <> rs.fields("TourID").value then
					lasttourid = rs.fields("Tourid").value
					if rs.fields("Volume").value & "" <> "" then
						gtotal = gtotal + ccur(rs.fields("Volume").value)			
					else
						gtotal = gtotal + 0
					end if
					tcount = tcount + 1
				end if
				
				response.write "<tr>"
				for i = 0 to rs.fields.count - 1
					if i = rs.fields.count - 1 then
						if rs.fields(i).value & "" = "" then 
							response.write "<td align = 'right'>" & formatcurrency(0) & "</td>"
						else
							response.write "<td align = 'right'>" & formatcurrency(rs.fields(i).value) & "</td>"
						end if
					else
						response.write "<td align = 'right'>&nbsp;" & rs.fields(i).value & "</td>"
					end if
				next
				response.write "</tr>"
				
				rs.movenext
			loop
			response.write "<tr>"
			response.write "<td>Totals:</td>"
			response.write "<td>" & tcount & "</td>"
			for i = 0 to rs.fields.count - 4
				response.write "<td>&nbsp;</td>"
			next
			response.write "<td>" & formatcurrency(gtotal) & "</td>"
			response.write "</tr>"
			response.write "</table>"
		end if
		rs.close
		
	end if


	cn.close
	set cn = nothing
	set rs = nothing

%>
