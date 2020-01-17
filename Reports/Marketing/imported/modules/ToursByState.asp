
<%
if request("function") = "Get_Report" then
	dim cn
	dim rs
	dim nsstat,nqstat,canstat,qualstat
	
	nsstat = request("nsstatus")
	canstat = request("canstatus")
	nqstat = request("nqstatus")
	qualstat = request("qualstatus")
	
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	
DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"
	
	cn.open DBName,DBUser,DBPass
	cn.commandtimeout = 2500
	server.scripttimeout = 10000
	
	'******Create Excel******************
	
	response.ContentType = "application/vnd.ms-excel"
	response.AddHeader "Content-Disposition", "attachment; filename=toursbystate.xls"	
	
	'*********Open Recordset******************
	
	rs.open "select distinct(s.comboitem) as State, " & _
				"	case when Club.ClubCount is null then " & _
				"		0 " & _
				"	else " & _
				"		Club.ClubCount " & _
				"	end as ClubCount, " & _
				"	case when tCount.tCount is null then " & _
				"		0 " & _
				"	else " & _
				"		tCount.tCount " & _
				"	end as tCount, " & _
				"	case when QT.QTCount is null then " & _
				"		0 " & _
				"	else " & _
				"		QT.QTCount " & _
				"	end as QTCount, " & _
				"	case when QT.QTCount / tCount.tCount is null then " & _
				"		0 " & _
				"	else " & _
				"		CAST(QT.QTCount AS DECIMAL(5,1)) / cast(tCount.tCount as decimal(5,1)) " & _
				"	end as QualPer, " & _
				"	case when NS.NSCount is null then " & _
				"		0 " & _
				"	else " & _
				"		NS.NSCount " & _
				"	end as NSCount, " & _
				"	case when NS.NSCount / tCount.tCount is null then " & _
				"		0 " & _
				"	else " & _
				"		CAST(NS.NSCount AS DECIMAL(5,1)) / cast(tCount.tCount as decimal(5,1)) " & _
				"	end as NSPer, " & _
				"	case when NQ.NQCount is null then " & _
				"		0 " & _
				"	else " & _
				"		NQ.NQCount " & _
				"	end as NQCount, " & _
				"	case when NQ.NQCount / tCount.tCount is null then " & _
				"		0 " & _
				"	else " & _
				"		CAST(NQ.NQCount AS DECIMAL(5,1)) / cast(tCount.tCount as decimal(5,1)) " & _
				"	end as NQPer, " & _
				"	case when Can.CancCount is null then " & _
				"		0 " & _
				"	else " & _
				"		Can.CancCount " & _
				"	end as CancCount, " & _
				"	case when Can.CancCount / tCount.tCount is null then " & _
				"		0 " & _
				"	else " & _
				"		CAST(Can.CancCount AS DECIMAL(5,1)) / cast(tCount.tCount as decimal(5,1)) " & _
				"	end as CancPer, " & _
				"	case when Other.OtherCount is null then " & _
				"		0 " & _
				"	else " & _
				"		Other.OtherCount " & _
				"	end as OtherCount, " & _
				" 	case when Other.OtherCount / tCount.tCount is null then " & _
				"		0 " & _
				"	else " & _
				"		CAST(Other.OtherCount AS DECIMAL(5,1)) / cast(tCount.tCount as decimal(5,1)) " & _
				"	end as OtherPer, " & _
				" 	case when tCon.tConCount is null then " & _
				"		0 " & _
				"	else " & _
				"		tCon.tConCount " & _
				"	end as tConCount, " & _
				"	case when tCon.tConCount / tCount.tCount is null then " & _
				"		0 " & _
				"	else " & _
				"		CAST(tCon.tConCount AS DECIMAL(5,1)) / cast(tCount.tCount as decimal(5,1)) " & _
				"	end as TotConPer, " & _
				"	case when ACon.AConCount is null then " & _
				"		0 " & _
				"	else " & _
				"		ACon.AConCount " & _
				"	end as AConCount, " & _
				"	case when ACon.AConCount / tCount.tCount is null then " & _
				"		0 " & _
				"	else " & _
				"		CAST(Acon.AConCount AS DECIMAL(5,1)) / cast(tCount.tCount as decimal(5,1)) " & _
				"	end as ActPer, " & _
				"	case when PCon.pConCount is null then " & _
				"		0 " & _
				"	else " & _
				"		PCon.pConCount " & _
				"	end as pConCount, " & _
				"	case when PCon.pConCount / tCount.tCount is null then " & _
				"		0 " & _
				"	else " & _
				"		CAST(PCon.pConCount AS DECIMAL(5,1)) / cast(tCount.tCount as decimal(5,1)) " & _
				"	end as penPer, " & _
				"	case when OtherCon.OtherConCount is null then " & _
				"		0 " & _
				"	else " & _
				"		OtherCon.OtherConCount " & _ 
				"	end as OtherConCount, " & _
				"	case when OtherCon.OtherConCount / tCount.tCount is null then " & _
				"		0 " & _
				"	else " & _
				"		CAST(OtherCon.OtherConCount AS DECIMAL(5,1)) / cast(tCount.tCount as decimal(5,1)) " & _
				"	end as OtherConPer " & _
				"from t_tour t " & _
				"inner join t_prospect p on p.prospectid = t.prospectid INNER JOIN T_PROSPECTADDRESS ON   " & _
				"inner join t_comboitems s on s.comboitemid = p.stateorprovince " & _
				"left outer join " & _
				"	(select count(t.tourid) as tCount,p.stateorprovince from t_tour t inner join t_prospect p on p.prospectid = t.prospectid inner join t_comboitems s on s.comboitemid = p.stateorprovince where t.tourdate between '" & request("sdate") & "' and '" & request("edate") & "' and T.SubTypeID not in (select comboitemid from t_comboitems where comboitem like '%Exit%') and t.tourlocationid not in (select comboitemid from t_comboitems where comboitem like '%Club%') and (T.StatusID is not null and T.StatusID <> 0) group by p.stateorprovince) tCount on tCount.stateorprovince = s.comboitemid " & _
				"left outer join " & _
				"	(select count(t.tourid) as NSCount,p.stateorprovince from t_tour t inner join t_prospect p on p.prospectid = t.prospectid where t.tourdate between '" & request("sdate") & "' and '" & request("edate") & "' and T.StatusID in (" & nsstat & ") and T.SubTypeID not in (select comboitemid from t_comboitems where comboitem like '%Exit%') and t.tourlocationid not in (select comboitemid from t_comboitems where comboitem like '%Club%') and (T.StatusID is not null and T.StatusID <> 0) group by p.stateorprovince) NS on NS.stateorprovince = s.comboitemid " & _
				"left outer join " & _
				"	(select count(t.tourid) as CancCount,p.stateorprovince from t_tour t inner join t_prospect p on p.prospectid = t.prospectid where t.tourdate between '" & request("sdate") & "' and '" & request("edate") & "' and T.StatusID in (" & canstat & ") and T.SubTypeID not in (select comboitemid from t_comboitems where comboitem like '%Exit%') and t.tourlocationid not in (select comboitemid from t_comboitems where comboitem like '%Club%') and (T.StatusID is not null and T.StatusID <> 0) group by p.stateorprovince) Can on Can.stateorprovince = s.comboitemid " & _
				"left outer join " & _
				"	(select count(t.tourid) as QTCount,p.stateorprovince from t_tour t inner join t_prospect p on p.prospectid = t.prospectid where t.tourdate between '" & request("sdate") & "' and '" & request("edate") & "' and T.StatusID in (" & qualstat & ") and T.SubTypeID not in (select comboitemid from t_comboitems where comboitem like '%Exit%') and t.tourlocationid not in (select comboitemid from t_comboitems where comboitem like '%Club%') and (T.StatusID is not null and T.StatusID <> 0) group by p.stateorprovince) QT on QT.stateorprovince = s.comboitemid " & _
				"left outer join " & _
				"	(select count(t.tourid) as NQCount,p.stateorprovince from t_tour t inner join t_prospect p on p.prospectid = t.prospectid where t.tourdate between '" & request("sdate") & "' and '" & request("edate") & "' and T.StatusID in (" & nqstat & ") and T.SubTypeID not in (select comboitemid from t_comboitems where comboitem like '%Exit%') and t.tourlocationid not in (select comboitemid from t_comboitems where comboitem like '%Club%') and (T.StatusID is not null and T.StatusID <> 0) group by p.stateorprovince) NQ on NQ.stateorprovince = s.comboitemid " & _
				"left outer join " & _
				"	(select count(t.tourid) as OtherCount,p.stateorprovince from t_tour t inner join t_prospect p on p.prospectid = t.prospectid where t.tourdate between '" & request("sdate") & "' and '" & request("edate") & "' and T.StatusID not in (" & nqstat & ") and T.StatusID not in (" & canstat & ") and T.StatusID not in (" & nsstat & ") and T.StatusID not in (" & qualstat & ") and T.SubTypeID not in (select comboitemid from t_comboitems where comboitem like '%Exit%') and t.tourlocationid not in (select comboitemid from t_comboitems where comboitem like '%Club%') and (T.StatusID is not null and T.StatusID <> 0) group by p.stateorprovince) Other on Other.stateorprovince = s.comboitemid " & _
				"left outer join " & _
				"	(select count(t.tourid) as ClubCount,p.stateorprovince from t_tour t inner join t_prospect p on p.prospectid = t.prospectid where t.tourdate between '" & request("sdate") & "' and '" & request("edate") & "' and T.SubTypeID not in (select comboitemid from t_comboitems where comboitem like '%Exit%') and t.tourlocationid in (select comboitemid from t_comboitems where comboname = 'TourLocation' and comboitem = 'VacationClub') and (T.StatusID is not null and T.StatusID <> 0) group by p.stateorprovince) Club on Club.stateorprovince = s.comboitemid " & _
				"left outer join " & _
				"	(select count(c.contractid) as AConCount,p.stateorprovince from t_tour t inner join t_prospect p on p.prospectid = t.prospectid inner join t_contract c on c.tourid = t.tourid where t.tourdate between '" & request("sdate") & "' and '" & request("edate") & "' and c.statusid in (select comboitemid from t_comboitems where comboitem in ('Active','Suspense')) and T.SubTypeID not in (select comboitemid from t_comboitems where comboitem like '%Exit%') and t.tourlocationid not in (select comboitemid from t_comboitems where comboitem like '%Club%') and (T.StatusID is not null and T.StatusID <> 0) and c.contractnumber not like 'U%' and c.contractnumber not like 'T%' group by p.stateorprovince) ACon on Acon.stateorprovince = s.comboitemid " & _
				"left outer join " & _
				"	(select count(c.contractid) as pConCount,p.stateorprovince from t_tour t inner join t_prospect p on p.prospectid = t.prospectid inner join t_contract c on c.tourid = t.tourid where t.tourdate between '" & request("sdate") & "' and '" & request("edate") & "' and c.statusid in (select comboitemid from t_comboitems where comboitem = 'Pender')and T.SubTypeID not in (select comboitemid from t_comboitems where comboitem like '%Exit%') and t.tourlocationid not in (select comboitemid from t_comboitems where comboitem like '%Club%') and (T.StatusID is not null and T.StatusID <> 0) and c.contractnumber not like 'U%' and c.contractnumber not like 'T%' group by p.stateorprovince) PCon on PCon.stateorprovince = s.comboitemid " & _
				"left outer join " & _
				"	(select count(c.contractid) as OtherConCount,p.stateorprovince from t_tour t inner join t_prospect p on p.prospectid = t.prospectid inner join t_contract c on c.tourid = t.tourid where t.tourdate between '" & request("sdate") & "' and '" & request("edate") & "' and c.statusid in (select comboitemid from t_comboitems where comboitem not in ('Pender','Active','Suspense')) and T.SubTypeID not in (select comboitemid from t_comboitems where comboitem like '%Exit%') and t.tourlocationid not in (select comboitemid from t_comboitems where comboitem like '%Club%') and (T.StatusID is not null and T.StatusID <> 0) and c.contractnumber not like 'U%' and c.contractnumber not like 'T%' group by p.stateorprovince) OtherCon on OtherCon.stateorprovince = s.comboitemid " & _
				"left outer join " & _
				"	(select count(c.contractid) as tConCount,p.stateorprovince from t_tour t inner join t_prospect p on p.prospectid = t.prospectid inner join t_contract c on c.tourid = t.tourid where t.tourdate between '" & request("sdate") & "' and '" & request("edate") & "' and T.SubTypeID not in (select comboitemid from t_comboitems where comboitem like '%Exit%') and t.tourlocationid not in (select comboitemid from t_comboitems where comboitem like '%Club%') and (T.StatusID is not null and T.StatusID <> 0) and c.contractnumber not like 'U%' and c.contractnumber not like 'T%' group by p.stateorprovince) tCon on tCon.stateorprovince = s.comboitemid " & _
				"where t.tourdate between '" & request("sdate") & "' and '" & request("edate") & "' " & _
				"group by s.comboitem,tCount.tCount,Club.ClubCount,NQ.NQCount,QT.QTCount,ACon.AConCount,PCon.pConCount,tCon.tConCount,NS.NSCount,Other.OtherCount,OtherCon.OtherConCount,Can.CancCount ",cn,3,3


			if rs.bof and rs.eof then
				
			else
				with response
					.write "<table border = 1>"
					.write "<tr border = 1>"
						.write "<td colspan = 21 align = center><b>Tours By State: " & request("sdate") & " and " & request("edate") & "</b></td>"
					.write "</tr>"
					.write "<tr border = 1>"
					.write "<td colspan = 2>&nbsp;</td>"
					.write "<td colspan = 11 align = center><b>Tours</b></td>"
					.write "<td colspan = 8 align = center><b>Contracts</b></td>"
					.write "</tr>"
					.write "<tr>"
					.write "<td align = center>&nbsp;</td>"
					.write "<td align = center>&nbsp;</td>"
					.write "<td align = center>&nbsp;</td>"					
					.write "<td colspan = 2 align = center><b>Qualified</b></td>"
					.write "<td colspan = 2 align = center><b>No Show</b></td>"
					.write "<td colspan = 2 align = center><b>Not Qualified</b></td>"
					.write "<td colspan = 2 align = center><b>Cancelled</b></td>"
					.write "<td colspan = 2 align = center><b>Other</b></td>"
					.write "<td colspan = 2 align = center><b>Total</b></td>"
					.write "<td colspan = 2 align = center><b>Active</b></td>"
					.write "<td colspan = 2 align = center><b>Pender</b></td>"
					.write "<td colspan = 2 align = center><b>Other</b></td>"
					.write "</tr>"
					.write "<tr>"
					.write "<td align = center><b>State</b></td>"
					.write "<td align = center><b>Club Count</b></td>"
					.write "<td align = center><b>KCP Total</b></td>"										
					.write "<td align = center><b>Count</b></td>"
					.write "<td align = center><i>%</i></td>"
					.write "<td align = center><b>Count</b></td>"
					.write "<td align = center><i>%</i></td>"
					.write "<td align = center><b>Count</b></td>"
					.write "<td align = center><i>%</i></td>"
					.write "<td align = center><b>Count</b></td>"
					.write "<td align = center><i>%</i></td>"
					.write "<td align = center><b>Count</b></td>"
					.write "<td align = center><i>%</i></td>"
					.write "<td align = center><b>Count</b></td>"
					.write "<td align = center><i>%</i></td>"
					.write "<td align = center><b>Count</b></td>"
					.write "<td align = center><i>%</i></td>"
					.write "<td align = center><b>Count</b></td>"
					.write "<td align = center><i>%</i></td>"
					.write "<td align = center><b>Count</b></td>"
					.write "<td align = center><i>%</i></td>"																																								
					.write "</tr>"
					
					'.write "<tr border = 1>"
					'for i = o to rs.fields.count -1
					'	.write "<td><b>" & rs.fields(i).name & "</b></td>"
					'next
					'.write "</tr>"
					do while not rs.eof
						.write "<tr border = 1>"
						for i = 0 to rs.fields.count -1
							if rs.fields(i).name = "penPer" or rs.fields(i).name = "ActPer" or rs.fields(i).name = "TotConPer" or rs.fields(i).name = "QualPer" or rs.fields(i).name = "NQPer" or rs.fields(i).name = "NSPer" or rs.fields(i).name = "CancPer" or rs.fields(i).name = "OtherConPer" or rs.fields(i).name = "OtherPer" then
								.write "<td><i>" & Formatpercent(CDBL(rs.fields(i).value)) & "</i></td>"
							else
								.write "<td>" & rs.fields(i).value & "</td>"
							end if
						next
						rs.movenext
						.write "</tr>"
					loop
				end with
			end if
	rs.close					
	cn.close
	
	set rs = nothing
	set cn = nothing
'response.write "Whoop Whoop"
else
	response.write "Wrong Function"
end if
%>
