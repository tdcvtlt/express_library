
<%

dim cn, rs
dim aNames(), aNameIDs(), aTours(), aSales(), aVol(), aTeams()
redim aNames(1)
redim aNameIDs(1)
redim aTours(1)
redim aSales(1)
redim aVol(1)
redim aTeams(1)

aNames(0) = ""
aNameIDs(0) = 0
aTours(0) = 0
aSales(0) = 0
aVol(0) = 0
aTeams(0) = ""
aNames(1) = ""
aNameIDs(1) = 0
aTours(1) = 0
aSales(1) = 0
aVol(1) = 0
aTeams(1) = ""

set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")
'DBName = "CRMSData"


DBName = "CRMSNET"
DBUser = "asp"
DBPass = "aspnet"


cn.open DBName, DBUser, DBPass

if request("f") = "runreport" then
	if request("grp") = "0" then
		sWhere = ""
	else
		sWhere = " Where c.comboitemid = '" & request("grp") & "'"
	end if
	sql = "select * " & _
			"from t_Personnel p " & _
			"	inner join t_Personnel2Teams t on t.personnelid = p.personnelid " & _
			"	inner join t_Comboitems c on c.comboitemid = t.teamid " 
	sql = sql & sWhere
	sql = sql & " order by c.comboitem"

	rs.open sql, cn, 0, 1
	if rs.eof and rs.bof then
		response.write "No personnel found for the selected team(s)."
		rs.close
	else
		do while not rs.eof
			'Save Name and ID
			bFound = false
			for i = 0 to ubound(aNameIDs)
				if aNameIDs(i) = 0 or aNameIDs(i) = rs.fields("PersonnelID").value then
					aNameIDs(i) = rs.fields("PersonnelID").value
					aNames(i) = rs.fields("LastName").value & ", " & rs.fields("Firstname").value 
					aTeams(i) = rs.fields("Comboitem").value & ""
					bFound = true
					exit for
				end if
			next
			if not bFound then
				redim preserve aNameIDs(ubound(aNameIDs)+1)
				redim preserve aNames(ubound(aNames)+1)
				redim preserve aTours(ubound(aTours)+1)
				redim preserve aSales(ubound(aSales)+1)
				redim preserve aVol(ubound(aVol)+1)
				redim preserve aTeams(ubound(aTeams)+1)
				aNameIDs(ubound(aNameIDs)) = rs.fields("PersonnelID").value
				aNames(ubound(aNames)) = rs.fields("Lastname").value & ", " & rs.fields("Firstname").value
				aTours(ubound(aTours)) = 0
				aSales(ubound(aSales)) = 0
				aVol(ubound(aVol)) = 0
				aTeams(ubound(aTeams)) = rs.fields("Comboitem").value & ""
			end if			
			rs.movenext
		loop
		rs.close
		
		
		'Get Tours for selected personnel
		for i = 0 to ubound(aNameIDs)
			sql = "Select count(distinct p.KEYVALUE) as Tours " & _
					"from t_PersonnelTrans p " & _
					"inner join t_Tour t on t.tourid = p.KEYVALUE AND P.KEYFIELD = 'TOURID' " & _
					"inner join t_Comboitems ts on ts.comboitemid = t.statusid " & _
					"where ts.comboitem in ('Showed','OnTour') " & _
					"and t.tourdate between '" & request("sdate") & "' and '" & request("edate") & "' "& _ 
					"and p.personnelid = '" & aNameIDs(i) & "' " & _
					"and p.Percentage = 0 " & _
					"and p.fixedamount = 0 " & _ 
					"and p.titleid in (" & _
						"select comboitemid " & _
						"from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID " & _
						"where comboname = 'personneltitle' " & _
						"and comboitem = 'sales executive' " & _
						")"
			rs.open sql, cn, 0,1 
			if rs.eof and rs.bof then
			else
				aTours(i) = rs.fields("Tours").value
			end if
			rs.close
		next
		
		'Get Sales for selected personnel
		sql = "select p.personnelid, count(distinct pt.contractid) as Contracts " & _
				"from t_Personnel p " & _
				"inner join t_Personneltrans pt on pt.personnelid = p.personnelid " & _
				"inner join t_Contract c on c.contractid = pt.CONTRACTID " & _
				"Where ( " & _
						"( " & _
							"c.statusdate between '" & request("sdate") & "' and '" & request("edate") & "' " & _
							"and c.contractdate < '" & request("sdate") & "' " & _
							"and c.contractid in (" & _
								"select KEYVALUE " & _
								"from t_Event " & _
								"where KEYVALUE > 0 AND KEYFIELD = 'CONTRACTID' " & _
									"and oldvalue in ('pender','developer','pender-inv')" & _
							")" & _
						")" & _
						"or (" & _
							"c.contractdate between '" & request("sdate") & "' and '" & request("edate") & "' " & _
							")" & _
					") " & _
					"and pt.contractid > 0 " & _
					"and pt.titleid in (" & _
							"select comboitemid " & _
							"from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID " & _
							"where comboname = 'personneltitle' " & _
							"and comboitem='Sales Executive' " & _
						")" & _
					"and c.statusid in (" & _
						"select comboitemid " & _
						"from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID  " & _
						"where comboname = 'contractstatus' " & _
						"and comboitem in ('Active', 'Suspense') " & _
					") " & _
					"and c.contractnumber not like '%t%' and c.contractnumber not like '%u%' " & _
				"Group by p.personnelid"
		rs.open sql, cn, 0, 1
		do while not rs.eof
			for i = 0 to ubound(aNameIDs)
				if aNameIDs(i) = rs.fields("PersonnelID").value then
					aSales(i) = rs.fields("Contracts").value
					exit for
				end if
			next
			rs.movenext
		loop
		rs.close
		
		'Get Volume
		SQL = "Select p.personnelID, ( " & _
					"Select Sum(salesvolume) " & _
					"from t_Mortgage " & _
					"where mortgageid in (" & _
						"select distinct mortgageid " & _
						"from t_contract c " & _
							"inner join t_Personneltrans t on t.contractid = c.contractid " & _
							"inner join t_Mortgage m on m.contractid = c.contractid " & _
						"where (" & _
							"c.contractdate between '" & request("sDate") & "' and '" & request("eDate") & "' " & _
							"or (" & _
								"c.statusdate between '" & request("sDate") & "' and '" & request("eDate") & "' " & _
								"and c.contractdate < '" & request("sDate") & "' " & _
								"and c.contractid in (" & _
									"select KEYVALUE " & _
									"from t_Event " & _
									"where KEYVALUE > 0 AND KEYFIELD = 'contractid' " & _
										"and oldvalue in ('pender','developer','pender-inv') " & _ 
									")" & _
								")" & _
							") " & _
							"and t.titleid in (" & _
								"select comboitemid " & _
								"from t_comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID " & _
								"where comboname = 'personneltitle' " & _
									"and comboitem = 'sales executive'" & _
								") " & _
							"and c.statusid in (" & _
								"select comboitemid " & _
								"from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID " & _
								"where comboname = 'contractstatus' " & _
								"	and comboitem in ('active','suspense')" & _
							") " & _
							"and t.personnelid = p.personnelid " & _
							"and c.contractid not in (select contractid from t_Contract where contractnumber like '%t%' and contractnumber not like '%u%') " & _
						") " & _
					") as Volume " & _
				"from t_Personnel p " & _
					"inner join t_PersonnelTrans pt on pt.personnelid = p.personnelid " & _
          			"inner join t_Contract c on c.contractid = pt.contractid " & _ 
          			"inner join t_Mortgage m on m.contractid = c.contractid " & _
          		"where (" & _
          				"(" & _
          					"pt.datecreated >= '" & request("sDate") & "' and pt.datecreated < '" & request("eDate") & "' " & _
          					"and c.statusdate < '" & request("eDate") & "'" & _
          				") " & _
          				"or (" & _
          					"c.contractdate < '" & request("sDate") & "' " & _
          					"and c.statusdate between '" & request("sDate") & "' and '" & cdate(request("eDate"))  + 1 & "' " & _
          					"and c.contractid in (" & _
          						"select KEYVALUE " & _
          						"from t_Event " & _
          						"where KEYVALUE > 0 AND KEYFIELD = 'CONTRACTID' " & _
          						"and oldvalue in ('pender','developer','pender-inv')" & _
          						")" & _
          					")" & _
          				") " & _
          				"and  pt.ContractID > 0 " & _
				        "and pt.titleid in (" & _
								"select comboitemid " & _
								"from t_comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID " & _
								"where comboname = 'personneltitle' " & _
									"and comboitem = 'sales executive'" & _
								") " & _
						"and c.statusid in (" & _
								"select comboitemid " & _
								"from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID " & _
								"where comboname = 'contractstatus' " & _
								"	and comboitem in ('active','suspense')" & _
							") " & _
						"and c.contractid not in (select contractid from t_Contract where contractnumber like '%t%' and contractnumber not like '%u%') " & _
				"group by p.personnelid"
  		rs.open sql, cn, 0, 1
  		do while not rs.eof
  			for i = 0 to ubound(aNameIDs)
  				if aNameIDs(i) = rs.fields("PersonnelID").value then
  					if rs.fields("Volume").value & "" = "" then
  						aVol(i) = 0
  					else
  						aVol(i) = rs.fields("Volume").value
  					end if
  					exit for
  				end if
  			next
  			rs.movenext
  		loop
  		rs.close
	end if
%>
	<table border=1>
<%
	sLastTeam  = ""
	dim aTotals(2)
	for i = 0 to ubound(aNameIDs)
		if sLastTeam <> aTeams(i) then
			if sLastTeam <> "" then
				'write totals
%>
			<tr>
				<td><b>Totals for <%=sLastTeam%></b></td>
				<td align=right><b><%=aTotals(0)%></b></td>
				<td align=right><b><%=aTotals(1)%></b></td>
				<td align=right><b><%=formatcurrency(aTotals(2))%></b></td>
				<td align=right>
					<b>
					<% 
						if aTotals(0) > 0 then
							response.write formatcurrency(aTotals(2)/aTotals(0))
						else
							response.write formatcurrency(0)
						end if				
					%>
				</b>
				</td>
			</tr>
			</table>
			<table border =1>
<%
				'clear totals
				for x = 0 to 2
					aTotals(x) = 0
				next
			end if
			sLastTeam = aTeams(i)			
%>
			<tr>
				<th colspan=5 align = left><%=aTeams(i)%></th>
			</tr>
			<tr>
				<td><b>Name</b></td>
				<td><b>Tours</b></td>
				<td><b>Sales</b></td>
				<td><b>Volume</b></td>
				<td><b>VPG</b></td>
			</tr>
<%
		end if
		aTotals(0) = aTotals(0) + aTours(i)
		aTotals(1) = aTotals(1) + aSales(i)
		aTotals(2) = aTotals(2) + aVol(i)
		if aTours(i) > 0 or aSales(i)> 0 then
%>
		

		<tr>
			<td><%=aNames(i)%></td>
			<td align=right><%=aTours(i)%></td>
			<td align=right><%=aSales(i)%></td>
			<td align=right><%=formatcurrency(aVol(i))%></td>
			<td align=right><%
				if aTours(i) > 0 then
					response.write formatcurrency(aVol(i)/aTours(i))
				else
					response.write formatcurrency(0)
				end if
				%></td>
		</tr>
<%
		end if
	next
%>
			<tr>
				<td><b>Totals for <%=sLastTeam%></b></td>
				<td align="right"><b><%=aTotals(0)%></b></td>
				<td align="right"><b><%=aTotals(1)%></b></td>
				<td align="right"><b><%=formatcurrency(aTotals(2))%></b></td>
				<td align="right">
					<b>
					<% 
						if aTotals(0) > 0 then
							response.write formatcurrency(aTotals(2)/aTotals(0))
						else
							response.write formatcurrency(0)
						end if				
					%>
				</b>
				</td>
			</tr>
			</table>

<%
	
elseif request("f") = "Get_Teams" then
	sql = "Select * from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'salesteam' and active = 1"
	rs.open sql,cn,0,1
	sans = ""
	do while not rs.eof
		if sAns <> "" then sAns = sAns & "|&&|"
		sAns = sAns & rs.fields("comboitemid").value & "|" & rs.fields("Comboitem").value
		rs.movenext
	loop
	rs.close
	response.write sAns
end if
cn.close

set rs = nothing
set cn = nothing

%>