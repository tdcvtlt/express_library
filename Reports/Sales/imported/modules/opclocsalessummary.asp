

<html><head><meta http-equiv="Content-Language" content="en-us"><title>OPC Location Sales Summary</title><meta name="Microsoft Theme" content="none, default">
<body><p>&nbsp;</p><%

dim cn
dim rs

dim FDContractStatuses, PenderStatus, CanceledStatus
FDContractStatuses = "'Active', 'Suspense'"
PenderStatus = "'Pender', 'Developer', 'Pender-Inv'"
CanceledStatus = "'Canceled', 'Rescinded','CXL-Bankruptcy'"


'if request("campaign") = "ALL" then
'	campaign = "'MAL','MAL-TS'"
'elseif request("Campaign") = "MAL" then
'	campaign = "'MAL'"
'elseif request("campaign") = "MAL-TS" then'
'	campaign = "'MAL-TS'"
'elseif request("campaign") = "PRESTIGE" then
'    campaign = "'PRESTIGE'"
'elseif request("campaign") = "4K" then
'    campaign = "'4K'"
'elseif request("campaign") = "EMS" then
'    campaign = "'EMS'"
'end if

set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.recordset")

DBName = "CRMSData"
DBName = "CRMSNET"
DBUser = "asp"
DBPass = "aspnet"


cn.open DBName, DBUser, DBPass

cn.commandtimeout = 2500
server.scripttimeout = 10000
dim aTotal()
redim aTotal(19,1)
aTotal(0,0) = "ALL"
'Removed from sql as qp 3-15-13 RH
'"select count(distinct tr.PackageIssuedID) as PKG, t.salelocid /*as 'QualifiedTours' */ " & _
'			"from t_vendorrep2tour t " & _
'			"inner join t_tour tr on tr.tourid = t.tourid " & _
'			"inner join t_comboitems tstatus on tstatus.comboitemid = tr.statusid " & _
'			"where t.userid in (" & request("solicitor") & ") and t.salelocid > 0 " & _ 
'			"Group by t.salelocid " & _
sql = "select v.SalesLocationid,v.Location as Location,case when qp.PKG is null then 0 else qp.PKG end as 'PKGS Sold', case when qt.tours is null then 0 else qt.tours end as 'QualifiedTours', " & _
		"case when fds.sales is null then 0 else fds.sales end  as 'FDSales', case when ps.sales is null then 0 else ps.sales end as 'Pending Sales', case when fdv.volume is null then 0 else fdv.volume end as 'FDVolume', " & _
		"case when pv.volume is null then 0 else pv.volume end  as 'Pending Volume', case when cs.sales is null then 0 else cs.sales end as 'Canceled Sales', " & _
		"case when cv.volume is null then 0 else cv.volume end  as 'Canceled Volume', case when cps.sales is null then 0 else cps.sales end as 'CXLPenderSales', case when cpv.volume is null then 0 else cpv.volume end as 'CXLPenderVolume' " & _
"from t_vendorsaleslocations v " & _ 
"	left outer join ( " & _
		"select count(distinct t.tourid) as Tours, t.salelocid /*as 'QualifiedTours' */ " & _
			"from t_vendorrep2tour t " & _
			"inner join t_tour tr on tr.tourid = t.tourid " & _
			"inner join t_comboitems tstatus on tstatus.comboitemid = tr.statusid " & _
			"where tstatus.comboitem in ('OnTour', 'Showed') and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and t.userid in (" & request("solicitor") & ") and t.salelocid > 0 " & _ 
			"Group by t.salelocid " & _
		") qt on qt.salelocid = v.saleslocationid  " & _ 
"	left outer join ( " & _
		"select count(distinct t.tourid) as PKG, t.salelocid /*as 'QualifiedTours' */ " & _
			"from t_vendorrep2tour t " & _
			"inner join t_tour tr on tr.tourid = t.tourid " & _
			"inner join t_comboitems tstatus on tstatus.comboitemid = tr.statusid " & _
			"where (tstatus.comboitem in ('OnTour', 'Showed','Booked') or tstatus.comboitem like 'NQ%' ) and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and t.userid in (" & request("solicitor") & ") and t.salelocid > 0 " & _ 
			"Group by t.salelocid " & _
		") qp on qp.salelocid = v.saleslocationid  " & _ 
"	left outer join " & _
		"( " & _
		"select count(distinct c.contractid) as Sales, t.salelocid /*as 'FDSales'*/ " & _
			"from t_contract c " & _
			"inner join t_tour tr on tr.tourid = c.tourid " & _
			"inner join t_vendorrep2tour t on t.tourid = tr.tourid " & _
			"left outer join t_comboitems type on type.comboitemid = c.typeid " & _
			"where ((c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & fdcontractstatuses & ")) and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "') or (c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & fdcontractstatuses & ")) and c.StatusDate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and c.ContractDate < '" & request("StartDate") & "' and c.originallywrittendate between '" & request("StartDate") & "' and '" & request("EndDate") & "')) and c.contractnumber not like 't%' and c.contractnumber not like 'u%' and t.userid in (" & request("solicitor") & ") " & _ 
			"group by t.salelocid " & _ 
		") fds on fds.salelocid = v.saleslocationid " & _ 
"	left outer join " & _
		"( " & _	 	
			"select count(distinct c.contractid) as Sales, t.salelocid /*as 'Pending Sales'*/ " & _
			"from t_contract c " & _
			"inner join t_tour tr on tr.tourid = c.tourid " & _
			"inner join t_vendorrep2tour t on t.tourid = tr.tourid " & _
			"left outer join t_comboitems type on type.comboitemid = c.typeid " & _
			"where c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & penderstatus & "))  and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and t.userid in (" & request("solicitor") & ") " & _
			"group by t.salelocid " & _
		") ps on ps.salelocid = v.saleslocationid " & _
"	left outer join " & _
		"( " & _ 
			"select sum(m.Salesvolume) as 'Volume', t.salelocid " & _
			"from t_mortgage m " & _
			"inner join t_contract c on c.contractid = m.contractid " & _
			"inner join t_tour tr on tr.tourid = c.tourid " & _
			"inner join t_vendorrep2tour t on t.tourid = tr.tourid " & _
			"left outer join t_comboitems type on type.comboitemid = c.Typeid " & _
			"where ((c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & fdcontractstatuses & ")) and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "') or (c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & fdcontractstatuses & ")) and c.StatusDate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and c.ContractDate < '" & request("StartDate") & "' and c.originallywrittendate between '" & request("StartDate") & "' and '" & request("EndDate") & "')) and c.contractnumber not like 't%' and c.contractnumber not like 'u%' and t.userid in (" & request("solicitor") & ") " & _ 
			"group by t.salelocid " & _
		") fdv on fdv.salelocid = v.saleslocationid " & _
"	left outer join " & _
		"( " & _
			"select sum(m.Salesvolume) as Volume, t.salelocid " & _
			"from t_mortgage m " & _
				"inner join t_contract c on c.contractid = m.contractid " & _
				"inner join t_tour tr on tr.tourid = c.tourid " & _
				"inner join t_vendorrep2tour t on t.tourid = tr.tourid " & _
				"left outer join t_comboitems type on type.comboitemid = c.typeid " & _
			"where c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & penderstatus & ")) and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "'  and t.userid in (" & request("solicitor") & ") " & _ 
			"group by t.salelocid " & _
		") pv on pv.salelocid = v.saleslocationid " & _
"	left outer join " & _
		"( " & _
			"select count(distinct c.contractid) as Sales, t.salelocid " & _
			"from t_contract c " & _
				"inner join t_tour tr on tr.tourid = c.tourid " & _
				"inner join t_vendorrep2tour t on t.tourid = tr.tourid " & _
				"inner join t_comboitems status on status.comboitemid = c.statusid " & _
			"where c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & canceledstatus & ")) and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "'  and t.userid in (" & request("solicitor") & ") " & _ 
			"group by t.salelocid " & _
		") cs on cs.salelocid = v.saleslocationid " & _
"	left outer join " & _
		"( " & _
			"select sum(m.SalesVolume) as 'Volume', t.salelocid " & _
			"from t_mortgage m " & _
				"inner join t_contract c on c.contractid = m.contractid " & _
				"inner join t_tour tr on tr.tourid = c.tourid " & _
				"inner join t_vendorrep2tour t on t.tourid = tr.tourid " & _
				"inner join t_comboitems status on status.comboitemid = c.statusid " & _
			"where c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & canceledstatus & ")) and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "'  and t.userid in (" & request("solicitor") & ") " & _ 
			"group by t.salelocid " & _
		") cv on cv.salelocid = v.saleslocationid " & _
"	left outer join " & _
		"( " & _
			"select count(distinct c.contractid) as 'Sales', t.salelocid " & _ 
			"from t_contract c " & _ 
				"inner join t_tour tr on tr.tourid = c.tourid " & _
				"inner join t_VendorRep2Tour t on c.tourid = t.tourid " & _
				"inner join t_comboitems status on status.comboitemid = c.statusid " & _
			"where c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem = 'CXL-Pender') and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "'  and t.userid in (" & request("solicitor") & ") " & _ 
			"group by t.salelocid " & _
		") cps on cps.salelocid = v.saleslocationid " & _
"	left outer join " & _
		"(select sum(m.Salesvolume) as 'Volume',t.salelocid " & _
			"from t_mortgage m " & _
				"inner join t_contract c on c.contractid = m.contractid " & _
				"inner join t_VendorRep2Tour t on c.tourid = t.tourid " & _
				"inner join t_tour tr on tr.tourid = c.tourid " & _
				"inner join t_personneltrans plt on plt.KEYVALUE = tr.tourid AND PLT.KEYFIELD = 'TOURID' " & _
				"left outer join t_comboitems type on type.comboitemid = c.typeid " & _
			"where c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem = 'CXL-Pender') and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and t.userid in (" & request("solicitor") & ") and plt.titleid in (select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' and comboitem = 'Solicitor') " & _ 
			"group by t.salelocid " & _
		") cpv on cpv.salelocid = v.saleslocationid " & _
"where vendorid in (select vendorid from t_Vendor where vendorid in (" & request("vendor") & ")) and saleslocationid in (" & request("Loc") & ") order by QualifiedTours desc, location "
'response.write sql
'response.end
rs.open sql,cn, 0,1 

%> 
<b>For DateRange Between <%=request("StartDate")%> and <%=request("EndDate")%></b> 
<TABLE style='border-collapse:collapse;' align = center cellspacing = 5 border = 1><tr><th><b>Location </b></th><th><b>PKGS Sold</b></th><th><b>Qualified Tours </b></th><th><b>Show %</b></th><th><b>Full Down VPG </b></th><th><b>Closing% </b></th><th><b>Full Down Sales </b></th><th><b>Full Down Volume </b></th><th><b>Full Down % </b></th><th><b>Pending Sales </b></th><th width="46"><b>Pending Volume </b></th><th><b>Pending % </b></th><th><b>Cancel Sales </b></th><th><b>Cancel Volume </b></th><th><b>Cancel % </b></th><th><b>Total Sales </b></th><th><b>Total Volume </b></th><th><b>Full Down ASP </b></th><th><b>CXLPenderSales</b></th><th><b>CXLPenderVolume</b></th></tr><%
dim fdv
dim closing
dim fda
dim cancel
dim pending
dim fd
dim sales
dim tv
dim cxl
dim fullDown

cxl = 0
fdv = 0
closing = 0
fda = 0
cancel = 0
pending = 0
fd = 0
sales = 0
tv = 0
fullDown = 0

dim i 
for i = 1 to 19
	aTotal(i,0) = 0
next
i=0
do while not rs.eof
	if IsNull(rs.Fields("FDVolume")) then
		fullDown = 0
	else
		fullDown = rs.Fields("FDVolume")
	end if
	i=i+1
	if i>ubound(aTotal,2) then
		redim preserve aTotal(19,i)
	end if
	if rs.fields("QualifiedTours").value <= 0 and fullDown = 0 and rs.Fields("FDSales") = 0 then
	
	else
%> <%aTotal(0,i) = rs.fields("Location").value%> <%
				aTotal(1,i) = rs.fields("PKGS Sold").value
				aTotal(1,0) = aTotal(1,0) + rs.fields("PKGS Sold").value
				aTotal(2,i)=rs.fields("QualifiedTours").value
				aTotal(2,0) = aTotal(2,0) + aTotal(2,i)
				if aTotal(1,i) > 0 then
					aTotal(3,i) = aTotal(2,i)/aTotal(1,i)
				else
					aTotal(3,i) = 0
				end if
				if aTotal(1,0) > 0 then
					aTotal(3,0) = aTotal(2,0)/aTotal(1,0)
				else
					aTotal(3,0) = 0
				end if
				aTotal(6,i) = rs.fields("FDSales").value
				aTotal(6,0) = aTotal(6,0) + rs.fields("FDSales").value
			%> <%
				if rs.fields("QualifiedTours").value = 0 then
					fdv = 0
				else
					fdv = formatcurrency((fullDown)/(rs.fields("QualifiedTours").value),2,,,-2) 
				end if
				aTotal(4,i) = fdv
				'aTotal(4,0) = aTotal(4,0) + fdv
			%> <%
				if rs.fields("FDSales").value = 0 or rs.Fields("QualifiedTours") = 0 then
					closing = 0
				else
					closing = rs.fields("FDSales").value/rs.fields("QualifiedTours").value
				end if
				
			%> <%


				aTotal(9,0) = aTotal(9,0) + rs.fields("Pending Sales").value
				if aTotal(2,0) = 0 then
					aTotal(5,0) = 0
				else
					aTotal(5,0) = aTotal(6,0)/aTotal(2,0)
				end if

				aTotal(9,i) = rs.fields("Pending Sales").value
				if atotal(2,i) = 0 then
					aTotal(5,i) = 0
				else
					aTotal(5,i) = aTotal(6,i)/aTotal(2,i)
				end if
			%> <%

				aTotal(7,i) = fullDown
				aTotal(7,0) = aTotal(7,0) + aTotal(7,i)
				if atotal(2,0) = 0 then
					aTotal(4,0) = 0
				else
					aTotal(4,0) = aTotal(7,0)/aTotal(2,0)
				end if				
			%> <%
				aTotal(10,i) = rs.fields("Pending Volume").value
				aTotal(10,0) = aTotal(10,0) + aTotal(10,i)
			%> <%
				aTotal(12,i) = rs.fields("Canceled Sales").value
				aTotal(12,0) = aTotal(12,0) + aTotal(12,i)
			%> <%
				aTotal(13,i) = rs.fields("Canceled Volume").value
				aTotal(13,0) = aTotal(13,0) + aTotal(13,i)
			%> <%
				sales = (rs.fields("Canceled Sales").value + rs.fields("Pending Sales").value) + rs.fields("FDSales").value
			%> <%
				aTotal(15,i) = sales
				aTotal(15,0) = aTotal(15,0) + aTotal(15,i)
			%> <%
				tv = rs.fields("Canceled Volume").value + rs.fields("Pending Volume").value + fullDown
			%> <%
				aTotal(16,i) = tv
				aTotal(16,0) = aTotal(16,0) + aTotal(16,i)
			%> <%
				if fullDown = 0 or tv = 0 then
					fd = 0
				else
					fd = fullDown/tv 
				end if
			%> <%
				aTotal(8,i) = fd
				if aTotal(16,0) > 0 then
					aTotal(8,0) = aTotal(7,0) / aTotal(16,0)
				else
					aTotal(8,0) = 0
				end if
			%> <%
				if rs.fields("Pending Volume").value = 0 or tv = 0 then
					pending = 0
				else
					pending = rs.fields("Pending Volume").value/tv 
				end if
			%> <%
				aTotal(11,i) = pending
				if aTotal(16,0) > 0 then
					aTotal(11,0) = aTotal(10,0)/ aTotal(16,0)
				else
					aTotal(11,0) = 0
				end if
			%> <%
				if rs.fields("Canceled Volume").value = 0 or tv = 0 then
					cancel = 0
				else
					cancel = rs.fields("Canceled Volume").value/tv 
				end if
			%> <%
				aTotal(14,i) = cancel
				if aTotal(16,0) > 0 then
					aTotal(14,0) = aTotal(13,0) / aTotal(16,0)
				else
					aTotal(14,0) = 0
				end if
			%> <%
				if fullDown = 0 or rs.fields("FDSales").value = 0 then
					fda = 0
				else
					fda = fullDown/rs.fields("FDSales").value 
				end if
			%> <%
				aTotal(17,i) = fda
				if aTotal(6,0) > 0 then 
					aTotal(17,0) = aTotal(7,0) / aTotal(6,0)
				else
					aTotal(17,0) = 0
				end if
				aTotal(18,i) = rs.fields("CXLPenderSales").value				
				aTotal(18,0) = aTotal(18,0) + aTotal(18,i)
				aTotal(19,i) = rs.fields("CXLPenderVolume").value
				aTotal(19,0) = aTotal(19,0) + aTotal(19,i)

			%> <%
	end if
	rs.movenext
loop

for i = 0 to ubound(aTotal,2)
	if aTotal(1,i) <> "" then 
		response.write "<tr>"
		dim x
		for x = 0 to ubound(aTotal,1)
			response.write "<td align = 'center'>"
			select case x
				case 4,7,10,13,16,17,19
					response.write formatcurrency( aTotal(x,i))
				case 3,5,8,11,14
					response.write formatpercent( aTotal(x,i))
				case else
					response.write aTotal(x,i)
			end select
			response.write "</td>"
		next
		response.write "</tr>"
	end if
next
%> </TABLE><%
rs.close
cn.close
set rs = nothing
set cn = nothing
response.write now
%> </head></body></html>