
<html><head><meta http-equiv="Content-Language" content="en-us">
<title>Solicitor Sales Summary</title><meta name="Microsoft Theme" content="none, default">
<body><p>&nbsp;</p><%

DBName = "CRMSNET"
DBUser = "asp"
DBPass = "aspnet"

dim cn
dim rs
dim FDContractStatuses, PenderStatus, CanceledStatus
FDContractStatuses = "'Active', 'Suspense'"
PenderStatus = "'Pender', 'Developer','Pender-Inv' "
CanceledStatus = "'Canceled', 'Rescinded','CXL-Bankruptcy'"
if request("campaign") = "ALL" then
	campaign = "'MAL','MAL-TS','EMS'"
elseif request("Campaign") = "MAL" then
	campaign = "'MAL','EMS'"
else
	campaign = "'MAL-TS'"
end if
server.scripttimeout = 10000

set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.recordset")

'DBName = "CRMSData"
'cn.Open "CRMSData", "asp", "aspnet"
cn.open DBName, DBUser, DBPass

cn.commandtimeout = 26000
dim aTotal()
redim aTotal(19,i)
aTotal(0,0) = "ALL"

sql = "select p.personnelid, p.lastname + ', ' + p.firstname as personnel, " & _
	"case when qt.tours is null then 0 else qt.tours end as 'QualifiedTours', case when nq.tours is null then 0 else nq.tours end as 'NQs', " & _
	"case when fd.sales is null then 0 else fd.sales end as 'FDSales', case when ps.sales is null then 0 else ps.sales end as 'Pending Sales', " & _
	"case when fdv.volume is null then 0 else fdv.volume end as 'FDVolume', case when pv.volume is null then 0 else pv.volume end as 'Pending Volume', " & _
	"case when cs.sales is null then 0 else cs.sales end as 'Canceled Sales', case when cv.volume is null then 0 else cv.volume end as 'Canceled Volume', " & _
	"case when cps.sales is null then 0 else cps.sales end as 'CXLPenderSales', case when cpv.volume is null then 0 else cpv.volume end as 'CXLPenderVolume' " & _ 
"from t_personnel p " & _
"	left outer join " & _
		"( " & _
			"select count(distinct t.tourid) as 'Tours',plt.personnelid " & _
			"from t_vendorrep2tour t " & _
			"left outer join t_tour tr on tr.tourid = t.tourid " & _
			"left outer join t_personneltrans plt on plt.KEYVALUE = tr.tourid AND PLT.KEYFIELD = 'TOURID' " & _
			"left outer join t_contract c on c.tourid = t.tourid " & _
			"left outer join t_comboitems tstatus on tstatus.comboitemid = tr.Statusid " & _
            "where tstatus.comboitem in ('Showed','ontour') and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and tr.campaignid in (select campaignid from t_Campaign where Name in (" & campaign & ")) " & _
			"group by plt.personnelid " & _
		") qt on qt.personnelid = p.personnelid " & _
"	left outer join " & _
		"( " & _
               "select count(distinct t.tourid) as 'Tours', plt.personnelid " & _
               "from t_vendorrep2tour t " & _
               "left outer join t_Tour tr on tr.tourid = t.tourid " & _
               "left outer join t_personneltrans plt on plt.KEYVALUE = tr.tourid AND PLT.KEYFIELD = 'TOURID'  " & _
               "left outer join t_contract c on c.tourid = t.tourid " & _
               "left outer join t_comboitems tstatus on tstatus.comboitemid = tr.statusid " & _
               "where tstatus.comboitem in ('NQ - No tour', 'NQ - Toured') and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and plt.titleid in (select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' and comboitem = 'Solicitor') and t.salelocid in (" & request("location") & ")  and tr.campaignid in (select campaignid from t_Campaign where Name in (" & campaign & ")) " & _
               "group by plt.personnelid " & _
    	") nq on nq.personnelid = p.personnelid " & _
"	left outer join " & _
		"( " & _		
			"select count(distinct c.contractid) as 'Sales', plt.personnelid " & _
			"from t_contract c " & _
			"left outer join t_VendorRep2Tour t on c.tourid = t.tourid " & _
			"left outer join t_tour tr on tr.tourid = t.tourid " & _
			"left outer join t_personneltrans plt on plt.KEYVALUE = tr.tourid AND PLT.KEYFIELD = 'TOURID'  " & _
			"left outer join t_comboitems type on type.comboitemid = c.typeid " & _
			"where ((c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & fdcontractstatuses & ")) and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "') or (c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & fdcontractstatuses & ")) and c.StatusDate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and c.ContractDate < '" & request("StartDate") & "' and c.originallywrittendate between '" & request("StartDate") & "' and '" & request("EndDate") & "')) and c.contractnumber not like 't%' and c.contractnumber not like 'u%' and plt.titleid in (select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' and comboitem = 'Solicitor') and t.salelocid in (" & request("location") & ") and tr.campaignid in (Select campaignid from t_Campaign where name in (" & campaign & ")) " & _
			"group by plt.personnelid " & _
		") fd on fd.personnelid = p.personnelid " & _
"	left outer join " & _
		"( " & _		
			"select count(distinct c.contractid) as 'Sales', plt.personnelid " & _
			"from t_contract c " & _
			"left outer join t_VendorRep2Tour t on c.tourid = t.tourid " & _
			"left outer join t_tour tr on tr.tourid = t.tourid " & _
			"inner join t_personneltrans plt on plt.KEYVALUE = tr.tourid AND PLT.KEYFIELD = 'TOURID' " & _
			"left outer join t_comboitems type on type.comboitemid = c.typeid " & _
			"where c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & penderstatus & ")) and c.contractnumber not like 't%' and c.contractnumber not like 'u%' and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and plt.titleid in (select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' and comboitem = 'Solicitor') and t.salelocid in (" & request("location") & ") and tr.campaignid in (Select campaignid from t_Campaign where name in (" & campaign & ")) " & _  
			"group by plt.personnelid " & _
		") ps on ps.personnelid = p.personnelid " & _
"	left outer join " & _
		"( " & _
			"select sum(m.Salesvolume) as 'Volume', plt.personnelid " & _
			"from t_mortgage m " & _
			"left outer join t_contract c on c.contractid = m.contractid " & _
			"left outer join t_VendorRep2Tour t on c.tourid = t.tourid " & _
			"left outer join t_tour tr on tr.tourid = t.tourid " & _
			"inner join t_personneltrans plt on plt.KEYVALUE = tr.tourid AND PLT.KEYFIELD = 'TOURID' " & _
			"left outer join t_comboitems type on type.comboitemid = c.typeid " & _
			"where ((c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & fdcontractstatuses & ")) and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "') or (c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & fdcontractstatuses & ")) and c.StatusDate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and c.ContractDate < '" & request("StartDate") & "' and c.originallywrittendate between '" & request("StartDate") & "' and '" & request("EndDate") & "')) and c.contractnumber not like 't%' and c.contractnumber not like 'u%' and plt.titleid in (select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' and comboitem = 'Solicitor') and t.salelocid in (" & request("location") & ") and tr.campaignid in (Select campaignid from t_Campaign where name in (" & campaign & ")) " & _
			"group by plt.personnelid " & _
		") fdv on fdv.personnelid = p.personnelid " & _
"	left outer join " & _
		"( " & _
			"select sum(m.Salesvolume) as 'Volume', plt.personnelid " & _
			"from t_mortgage m " & _
			"left outer join t_contract c on c.contractid = m.contractid " & _
			"left outer join t_VendorRep2Tour t on c.tourid = t.tourid " & _
			"left outer join t_tour tr on tr.tourid = t.tourid " & _
			"inner join t_personneltrans plt on plt.KEYVALUE = tr.tourid AND PLT.KEYFIELD = 'TOURID'  " & _
			"left outer join t_comboitems type on type.comboitemid = c.typeid " & _
			"where c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where B.comboname = 'contractstatus' and comboitem in (" & penderstatus & ")) and c.contractnumber not like 't%' and c.contractnumber not like 'u%' and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and plt.titleid in (select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' and comboitem = 'Solicitor') and t.salelocid in (" & request("location") & ") and  tr.campaignid in (Select campaignid from t_Campaign where name in (" & campaign & ")) " & _
			"group by plt.personnelid " & _
		") pv on pv.personnelid = p.personnelid " & _
"	left outer join " & _
		"( " & _
			"select count(distinct c.contractid) as 'Sales', plt.personnelid " & _
			"from t_contract c " & _
			"left outer join t_VendorRep2Tour t on c.tourid = t.tourid " & _
			"left outer join t_tour tr on tr.tourid = t.tourid " & _
			"inner join t_personneltrans plt on plt.KEYVALUE = tr.tourid AND PLT.KEYFIELD = 'TOURID' " & _
			"left outer join t_comboitems status on status.comboitemid = c.statusid " & _
			"where  c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & canceledstatus & ")) and c.contractnumber not like 't%' and c.contractnumber not like 'u%' and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and plt.titleid in (select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' and comboitem = 'Solicitor') and t.salelocid in (" & request("location") & ") and tr.campaignid in (Select campaignid from t_Campaign where Name in (" & campaign & ")) " & _ 
			"group by plt.personnelid " & _
		") cs on cs.personnelid = p.personnelid " & _
"	left outer join " & _
		"( " & _
			"select sum(m.SalesVolume) as 'Volume', plt.personnelid " & _
			"from t_mortgage m " & _
			"inner join t_contract c on c.contractid = m.contractid " & _
			"left outer join t_VendorRep2Tour t on c.tourid = t.tourid " & _
			"left outer join t_tour tr on tr.tourid = t.tourid " & _
			"inner join t_personneltrans plt on plt.KEYVALUE = tr.tourid AND PLT.KEYFIELD = 'TOURID' " & _
			"left outer join t_comboitems status on status.comboitemid = c.statusid " & _
			"where c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem in (" & canceledstatus & ")) and c.contractnumber not like 't%' and c.contractnumber not like 'u%' and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and plt.titleid in (select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' and comboitem = 'Solicitor') and t.salelocid in (" & request("location") & ") and tr.campaignid in (Select campaignid from t_Campaign where name in (" & campaign & ")) " & _
			"group by plt.personnelid " & _
		") cv on cv.personnelid = p.personnelid " & _
"	left outer join " & _
		"( " & _
			"select count(distinct c.contractid) as 'Sales', plt.personnelid " & _ 
			"from t_contract c " & _ 
				"left outer join t_VendorRep2Tour t on c.tourid = t.tourid " & _
				"left outer join t_tour tr on tr.tourid = c.tourid " & _
				"inner join t_personneltrans plt on plt.KEYVALUE = tr.tourid AND PLT.KEYFIELD = 'TOURID'" & _
				"left outer join t_comboitems status on status.comboitemid = c.statusid " & _
			"where c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem = 'CXL-Pender') and c.contractnumber not like 't%' and c.contractnumber not like 'u%' and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and plt.KEYVALUE in (select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' and comboitem = 'Solicitor') and t.salelocid in (" & request("location") & ") and tr.campaignid in (Select campaignid from t_Campaign where name in (" & campaign & ")) " & _
			"group by plt.personnelid " & _
		") cps on cps.personnelid = p.personnelid " & _
"	left outer join " & _
		"( " & _
			"select sum(m.Salesvolume) as 'Volume', plt.personnelid " & _
			"from t_mortgage m " & _
			"inner join t_contract c on c.contractid = m.contractid " & _
			"left outer join t_VendorRep2Tour t on c.tourid = t.tourid " & _
			"left outer join t_tour tr on tr.tourid = c.tourid " & _
			"inner join t_personneltrans plt on PLT.KEYVALUE = tr.tourid AND PLT.KEYFIELD = 'TOURID'" & _
			"left outer join t_comboitems type on type.comboitemid = c.typeid " & _
			"where c.statusid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'contractstatus' and comboitem = 'CXL-Pender') and c.contractnumber not like 't%' and c.contractnumber not like 'u%' and tr.tourdate between '" & request("StartDate") & "' and '" & request("EndDate") & "' and plt.titleid in (select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' and comboitem = 'Solicitor') and t.salelocid in (" & request("location") & ") and tr.campaignid in (Select campaignid from t_Campaign where name in (" & campaign & ")) " & _
			"group by plt.personnelid " & _
		") cpv on cpv.personnelid = p.personnelid " & _
        "INNER JOIN T_VENDOR2PERSONNEL V2P ON V2P.PERSONNELID = P.PERSONNELID " & _        
"where V2P.vendorid in (Select vendorid from t_Vendor where vendorid = '" & request("vendor") & "') and p.personnelid in (" & request("name") & ") " & _   
"order by QualifiedTours desc "

rs.open sql,cn, 0,1

%>
<b>For DateRange Between <%=request("StartDate")%> and <%=request("EndDate")%></b>
 <TABLE style="border-collapse:collapse;" align = center cellspacing = 5 border = 1><tr><th><b>Personnel </b></th><th><b>Qualified Tours </b></th><th><b>Full Down VPG </b></th><th><b>Closing% </b></th><th><b>Full Down Sales </b></th><th><b>Full Down Volume </b></th><th><b>Full Down % </b></th><th><b>Pending Sales </b></th><th><b>Pending Volume </b></th><th><b>Pending % </b></th><th><b>Cancel Sales </b></th><th><b>Cancel Volume </b></th><th><b>Cancel % </b></th><th><b>Total Sales </b></th><th><b>Total Volume </b></th><th><b>Full Down ASP </b></th><th><b>CXLPender Sales</b></th><th><b>CXLPender Volume </b></th><th><b>NQ Tours</b></th><th><b>NQTour %</b></th></tr><%
dim fdv
dim closing
dim fda
dim cancel
dim pending
dim fd
dim sales
dim tv
dim cxl
dim cxla
dim cxls
dim fullDown

cxls = 0
cxla = 0
fdv = 0
closing = 0
fda = 0
cancel = 0
pending = 0
fd = 0
sales = 0
tv = 0
cxl = 0
fullDown = 0


dim i 
for i = 1 to 19
	aTotal(i,0) = 0
next
i=0
do while not rs.eof
	if isNull(rs.Fields("FDVolume")) then
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
			aTotal(0,i) = rs.fields("Personnel").value%> <%
			aTotal(1,i)=rs.fields("QualifiedTours").value
			aTotal(1,0) = aTotal(1,0) + aTotal(1,i)
			if rs.fields("QualifiedTours").value = 0 then
				fdv = 0
			else
				fdv = formatcurrency((fullDown)/(rs.fields("QualifiedTours").value),2,,,-2) 
			end if
			aTotal(2,i) = fdv
			'aTotal(2,0) = aTotal(2,0) + fdv
			if rs.fields("FDSales").value = 0 or rs.Fields("QualifiedTours") = 0 then
				closing = 0
			else
				closing = rs.fields("FDSales").value /rs.fields("QualifiedTours").value
			end if
			
			aTotal(4,0) = aTotal(4,0) + rs.fields("FDSales").value
			aTotal(7,0) = aTotal(7,0) + rs.fields("Pending Sales").value
			if aTotal(1,0) = 0 then
				aTotal(3,0) = 0
			else
				aTotal(3,0) = aTotal(4,0)/aTotal(1,0)
			end if
			aTotal(4,i) = rs.fields("FDSales").value
			aTotal(7,i) = rs.fields("Pending Sales").value
			if atotal(1,i) = 0 then
				aTotal(3,i) = 0
			else
				aTotal(3,i) = aTotal(4,i)/aTotal(1,i)
			end if
			aTotal(16,0) = aTotal(16,0) + rs.fields("CXLPenderSales").value
			aTotal(17,0) = aTotal(17,0) + rs.fields("CXLPenderVolume").value
			aTotal(5,i) = fullDown
			aTotal(5,0) = aTotal(5,0) + aTotal(5,i)
			if atotal(1,0) = 0 then
				aTotal(2,0) = 0
			else
				aTotal(2,0) = aTotal(5,0)/aTotal(1,0)
			end if
			aTotal(8,i) = rs.fields("Pending Volume").value
			aTotal(8,0) = aTotal(8,0) + aTotal(8,i)
			aTotal(10,i) = rs.fields("Canceled Sales").value
			aTotal(10,0) = aTotal(10,0) + aTotal(10,i)
			aTotal(11,i) = rs.fields("Canceled Volume").value
			aTotal(11,0) = aTotal(11,0) + aTotal(11,i)
			sales = (rs.fields("Canceled Sales").value + rs.fields("Pending Sales").value) + rs.fields("FDSales").value
			aTotal(13,i) = sales
			aTotal(13,0) = aTotal(13,0) + aTotal(13,i)
			tv = rs.fields("Canceled Volume").value + rs.fields("Pending Volume").value + fullDown
			aTotal(14,i) = tv
			aTotal(14,0) = aTotal(14,0) + aTotal(14,i)
			if fullDown = 0 or tv = 0 then
				fd = 0
			else
				fd = fullDown/tv 
			end if
			aTotal(6,i) = fd
			if aTotal(14,0) > 0 then
				aTotal(6,0) = aTotal(5,0) / aTotal(14,0)
			else
				aTotal(6,0) = 0
			end if
			if rs.fields("Pending Volume").value = 0 or tv = 0 then
				pending = 0
			else
				pending = rs.fields("Pending Volume").value/tv 
			end if
			aTotal(9,i) = pending
			if aTotal(14,0) > 0 then
				aTotal(9,0) = aTotal(8,0)/ aTotal(14,0)
			else
				aTotal(9,0) = 0
			end if
			if rs.fields("Canceled Volume").value = 0 or tv = 0 then
				cancel = 0
			else
				cancel = rs.fields("Canceled Volume").value/tv 
			end if
			aTotal(12,i) = cancel
			if aTotal(14,0) > 0 then
				aTotal(12,0) = aTotal(11,0) / aTotal(14,0)
			else
				aTotal(12,0) = 0
			end if
			if fullDown = 0 or rs.fields("FDSales").value = 0 then
				fda = 0
			else
				fda = fullDown/rs.fields("FDSales").value 
			end if
			aTotal(15,i) = fda
			if aTotal(4,0) > 0 then 
				aTotal(15,0) = aTotal(5,0) / aTotal(4,0)
			else
				aTotal(15,0) = 0
			end if
			aTotal(17,i) = rs.fields("CXLPenderVolume").value
			aTotal(16,i) = rs.fields("CXLPenderSales").value
			aTotal(18,i) = rs.fields("NQs").value
			aTotal(18,0) = aTotal(18,0) + aTotal(18,i)
			if aTotal(18,0) > 0 then
				aTotal(19,0) = aTotal(18,0) / aTotal(1,0)
			else
				aTotal(19,0) = 0
			end if
			if aTotal(18,i) > 0 then
				if aTotal(1,i) > 0 then
					aTotal(19,i) = aTotal(18,i) / aTotal(1,i)
				else
					aTotal(19,i) = 0
				end if	
			else
				aTotal(19,i) = 0
			end if			
	end if
	rs.movenext
loop

for i = 0 to ubound(aTotal,2)
	if aTotal(1,i) <> "" then 
		response.write "<tr>"
		dim x
		for x = 0 to 19
			response.write "<td align = 'center'>"
			select case x
				case 2,5,8,11,14,15,17
					response.write formatcurrency( aTotal(x,i))
				case 3,6,9,12,19
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