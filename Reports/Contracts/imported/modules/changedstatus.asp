
<%	

	'local grand tallying variables
	dim grd_price
	dim grd_down
	dim grd_equi
	dim grd_fin
	dim grd_cnt
	
	'locat tallying variables
	dim price
	dim down
	dim equi
	dim fin
	dim con_cnt
	
	'object variables
	dim oRs 
	dim oCn
	
	'html & sql variables
	dim writeln
	dim sql
	
	'status break
	dim stat_curr
	dim stat_last
	dim in_loop
	dim br_loop
	
	'parameters
	'dim date_begin
	'dim date_end
	'dim where_sql
	dim stat
	
	'Request Parameters
	'date_begin = Request("start")
	'date_end = Request("end")
	'where_sql = Request("where")
	status = request("status")
	
	
	'make connection
	Set oCn = Server.CreateObject("ADODB.Connection")
	oCn.ConnectionTimeout = 0
	'response.write status


    DBName = "CRMSNet"
    DBUser = "asp"
    DBPass = "aspnet"

	oCn.Open DBName, DBUser, DBPass
	
	
	'contruct recordset sql
	sql = 	"Select z.*, zz.UserFieldvalue as ConveyanceDate from (Select x.*, y.UserFieldValue as ConveyanceNumber from (select distinct a.contractid,b.statusid, b.contractnumber, b.statusDate, b.contractdate, d.lastname + ', ' + d.firstname as owner,a.newvalue,a.oldvalue, " & _
			"saletype = (select comboitem from t_comboitems where comboitemid = b.saletypeid), " & _
			"contracttype = (select comboitem from t_comboitems where comboitemid = b.contracttypeid), " & _
			"weektype = (select comboitem from t_comboitems where comboitemid = b.weektypeid), " & _
			"e.salesprice, e.dptotal, e.equity, e.totalfinanced, " & _
			"status = (select comboitem from t_comboitems where comboitemid = b.statusid) " & _
			"from t_event a " & _
			"inner join t_contract b " & _
			"on a.contractid = b.contractid " & _
			"left outer join t_comboitems c " & _
			"on b.saletypeid = c.comboitemid " & _
			"left outer join t_prospect d " & _
			"on b.prospectid = d.prospectid " & _
			"left outer join t_mortgage e " & _
			"on b.contractid = e.contractid " & _
			"where a.newvalue in (" & status & ") and a.datecreated between '" & request("startdate") & "' and '" & request("enddate") & "' and b.statusid in (select distinct comboitemid from t_comboitems where comboname = 'contractstatus' and comboitem = a.newvalue) ) x left outer join (Select RecordID, UserFieldValue from t_UserFieldsValue where userfieldid = (Select userfieldid from t_UserFields where fieldname = 'Conveyance Instr #')) y on x.contractid = y.recordid) z left outer join (Select RecordID, UserFieldValue from t_UserFieldsValue where userfieldid = (Select userfieldid from t_UserFields where fieldname = 'Conveyance Rec Date')) zz on z.contractid = zz.recordid order by status,contractdate" 


	sql = 	"select cs.comboitem as status, c.contractnumber, c.StatusDate, c.contractdate, p.lastname + ',' + p.firstname as owner,  " & _
			"			u.unitname, si.week, f.frequency, dotinstnum=( " & _
			"				select top 1 userfieldvalue  " & _
			"				from t_UserFieldsValue v " & _
			"					inner join t_UserFields uf on uf.userfieldid = v.userfieldid " & _
			"				where uf.tablename = '3' and uf.fieldname = 'Deed-of-Trust Instr #' " & _
			"					and v.recordid = c.contractid " & _
			"			), DOTRecDate=( " & _
			"				select top 1 userfieldvalue  " & _
			"				from t_UserFieldsValue v " & _
			"					inner join t_UserFields uf on uf.userfieldid = v.userfieldid " & _
			"				where uf.tablename = '3' and uf.fieldname = 'DOT Rec Date' " & _
			"					and v.recordid = c.contractid " & _
			"			), DOCInstNum=( " & _
			"				select top 1 userfieldvalue  " & _
			"				from t_UserFieldsValue v " & _
			"					inner join t_UserFields uf on uf.userfieldid = v.userfieldid " & _
			"				where uf.tablename = '3' and uf.fieldname = 'Conveyance Instr #' " & _
			"					and v.recordid = c.contractid " & _
			"			), DOCRecDate=( " & _
			"				select top 1 userfieldvalue  " & _
			"				from t_UserFieldsValue v " & _
			"					inner join t_UserFields uf on uf.userfieldid = v.userfieldid " & _
			"				where uf.tablename = '3' and uf.fieldname = 'Conveyance Rec Date' " & _
			"					and v.recordid = c.contractid " & _
			"			), CTE = left(ut.comboitem, 1) " & _
			"from t_Contract c  " & _
			"	inner join t_Event e on e.contractid = c.contractid " & _
			"	inner join t_Prospect p on p.prospectid = c.prospectid " & _
			"	left outer join t_SoldInventory s on s.contractid = c.contractid " & _
			"	left outer join t_Salesinventory si on si.salesinventoryid = s.salesinventoryid " & _
			"	left outer join t_Units u on u.unitid = si.unitid " & _
			"	left outer join t_Comboitems ut on u.unittypeid = ut.comboitemid " & _
			"	left outer join t_Frequency f on f.frequencyid = c.frequencyid " & _
			"	left outer join t_Comboitems cs on cs.comboitemid = c.statusid " & _		
			"where e.newvalue in (" & status & ") " & _
				"and e.datecreated between '" & request("startdate") & "' and '" & cdate(request("enddate")) + 1 & "' " & _
				"and cs.comboitem in ( " & status & ")"
	


    sql = 	"select cs.comboitem as status, c.contractnumber,  " & _
            "c.StatusDate, c.contractdate, p.lastname + ',' + p.firstname as owner,   " & _
            "						u.NAME, si.week, f.frequency, dotinstnum=(  " & _
            "							select top 1 UFVALUE " & _
            "							from t_UF_TABLES UT  " & _
            "								inner join t_UFIELDS UF on uf.UFTABLEID = UT.UFTABLEID " & _
            "								INNER JOIN T_UF_VALUE UV ON UV.UFID = UF.UFID  " & _
            "							where UT.UFTABLE = 'CONTRACT' and uf.UFNAME = 'Deed-of-Trust Instr #'  " & _
            "								and UV.KEYVALUE = c.contractid  " & _
            "						), DOTRecDate=(  " & _
            "							select top 1 UFVALUE   " & _
            "							from t_UF_TABLES UT " & _
            "								inner join t_UFIELDS UF ON uf.UFTABLEID = UT.UFTABLEID " & _
            "								INNER JOIN T_UF_VALUE UV ON UV.UFID = UF.UFID " & _
            "							where UT.UFTABLE = 'CONTRACT' and uf.UFNAME = 'DOT Rec Date'  " & _
            "								and UV.KEYVALUE = c.contractid  " & _
            "						), DOCInstNum=(  " & _
            "							select top 1 UFVALUE   " & _
            "							from T_UF_TABLES UT " & _
            "								inner join T_UFIELDS UF ON UT.UFTABLEID = UF.UFTABLEID " & _
            "								INNER JOIN T_UF_VALUE UV ON UF.UFID = UV.UFID  " & _
            "							where UT.UFTABLE = 'CONTRACT' and uf.UFNAME = 'Conveyance Instr #'  " & _
            "								and UV.KEYVALUE = c.contractid  " & _
            "						), DOCRecDate=(  " & _
            "							select top 1 UFVALUE " & _
            "							from t_UF_TABLES UT " & _
            "								inner join t_UFIELDS UF ON UT.UFTABLEID = UF.UFTABLEID " & _
            "								INNER JOIN T_UF_VALUE UV ON UF.UFID = UV.UFID  " & _
            "							where UT.UFTABLE = 'CONTRACT' and uf.UFNAME  = 'Conveyance Rec Date'  " & _
            "								and UV.KEYVALUE = c.contractid  " & _
            "						), CTE = left(ut.comboitem, 1)  " & _
            "			from t_Contract c   " & _
            "				inner join t_Event e on e.KEYVALUE = c.contractid  AND E.KEYFIELD = 'CONTRACTID' " & _
            "				inner join t_Prospect p on p.prospectid = c.prospectid  " & _
            "				left outer join t_SoldInventory s on s.contractid = c.contractid  " & _
            "				left outer join t_Salesinventory si on si.salesinventoryid = s.salesinventoryid  " & _
            "				left outer join t_Unit u on u.unitid = si.unitid  " & _
            "				left outer join t_Comboitems ut on u.Typeid = ut.comboitemid  " & _
            "				left outer join t_Frequency f on f.frequencyid = c.frequencyid  " & _
            "				left outer join t_Comboitems cs on cs.comboitemid = c.statusid  		" & _
            "			where e.newvalue in (" & status & " )  " & _
            "				and e.datecreated between ' " & request("startdate") & "' and '" & cdate(request("enddate")) + 1 & "'  " & _
            "				and cs.comboitem in ( " & status & ")" 


	Set oRs = Server.CreateObject("ADODB.RecordSet")
	'response.write request("status")
	oRs.Open sql, oCn, 0,1
	
	if not (oRs.BOF And oRs.EOF) then 
		
		writeln = "<table border=0 cellspacing=3 cellpadding=1>"
		writeln = writeln & "<hr>"
		
		in_loop = False
		br_loop = False
		call Label(stat_curr)
		Do While Not oRs.EOF
	
			'Group breaks
			'stat_curr = oRs.Fields("status")	
			
			'If stat_curr <> stat_last Then										
				
				'If in_loop = True Then
					'call Summ()
				'End If	
				
				'call Label(stat_curr)	
			'End If	
			
			
			'If stat_curr = stat_last Or in_loop = False Then
			
				'call Detail(oRs.Fields("ContractDate"), oRs.Fields("ContractNumber"), _
				'		oRs.Fields("Owner"), oRs.Fields("SaleType"), oRs.Fields("ContractType"), _
				'		oRs.Fields("WeekType"), oRs.Fields("SalesPrice"), oRs.Fields("DPTotal"), _
				'		oRs.Fields("Equity"), oRs.Fields("TotalFinanced"), oRs.Fields("ConveyanceNumber"), oRs.Fields("ConveyanceDate"))		
				call Detail(oRs.fields("ContractDate"), oRs.fields("ContractNumber"), _
						oRs.Fields("Owner"), oRs.fields("StatusDate"), oRs.fields("Name") & " " & oRs.fields("Week"), _ 
						oRs.fields("Frequency"), oRs.fields("ContractDate"), oRs.fields("DotInstNum"), _
						oRs.fields("DOTRecDate"), oRs.fields("Status"), oRs.fields("DOCInstNum"), _
						oRs.fields("DOCRecDate"), oRs.fields("CTE"))		
						br_loop = False									
			'End If			
			
							
			
			'If stat_curr <> stat_last Then
			
				'stat_last = stat_curr
				'br_loop = True
				
			'End If	
			
			in_loop = True
			
			oRs.MoveNext
			
		Loop
		
		If in_loop Then
			writeln = writeln & "</table>"
			'Call Summ()				'Display group total at bottom
			'Call Grand()			'Display grand total below
	
		End If	
		
	Else
	
		writeln = "<b>" & "No Records Match Your Criteria" & "</b>"
	End If		
	

	

	'Cleaning up...
	'oRs.Close
	oCn.Close
	
	Set oRs = Nothing
	Set oCnn = Nothing
	
	
	'html...
	response.write writeln	
	
	
	'THE END

'Table Detail
Function Detail(con_date, con_num, owner, status_Date, unit_week, freq, dot_date, dot_inst, dot_rec, reason,doc_inst, doc_rec_date, cte)
	
	on error resume next
	condate = cdate(con_Date)
	if err <> 0 then
		condate = ""
		err.clear
	end if
	on error goto 0
	
	'Group totals
	price = price + pric
	down = down + dow
	equi = equi + equ
	fin = fin + fi
	
	con_cnt = con_cnt + 1
	temp = split(owner, ",")
	
	writeln = writeln & "<tr>"
	writeln = writeln & "<td align='left'>" 
		if condate & "" <> "" then 
			writeln = writeln & Trim(FormatDateTime(condate, 2)) 
		else
			writeln = writeln & "&nbsp;"
		end if
	writeln = writeln & "</td>"
	writeln = writeln & "<td>" & con_num & "&nbsp;</td>"
	writeln = writeln & "<td>" & temp(0) & "&nbsp;</td>"
	writeln = writeln & "<td>" & temp(1) & "&nbsp;</td>"
	writeln = writeln & "<td>" & status_date & "&nbsp;</td>"
	writeln = writeln & "<td>" & unit_week & "&nbsp;</td>"
	writeln = writeln & "<td>" & freq & "&nbsp;</td>"
	writeln = writeln & "<td>" & dot_date & "&nbsp;</td>"
	writeln = writeln & "<td>" & dot_inst & "&nbsp;</td>"
	writeln = writeln & "<td>" & dot_rec & "&nbsp;</td>"
	writeln = writeln & "<td>" & reason & "&nbsp;</td>"
	writeln = writeln & "<td>" & doc_inst & "&nbsp;</td>"
	writeln = writeln & "<td>" & doc_rec_date & "&nbsp;</td>"
	writeln = writeln & "<td>" & cte & "&nbsp;</td>"
	'writeln = writeln & "<td>" & owner & "&nbsp;</td>"
	'writeln = writeln & "<td>" & sale & "&nbsp;</td>"
	'writeln = writeln & "<td>" & con & "&nbsp;</td>"
	'writeln = writeln & "<td>" & week & "&nbsp;</td>"
	'writeln = writeln & "<td align='right'>" & FormatCurrency(pric) & "&nbsp;</td>"
	'writeln = writeln & "<td align='right'>" & FormatCurrency(dow) & "&nbsp;</td>"
	'if equ & "" <> "" then
	'	writeln = writeln & "<td align='right'>" & FormatCurrency(equ) & "</td>"			
	'else
	'	writeln = writeln & "<td align='right'>" & FormatCurrency(0) & "</td>"			
	'end if
	'writeln = writeln & "<td align='right'>" & FormatCurrency(fi) & "</td>"
	writeln = writeln & "</tr>"
	
End Function
Function Detail_old(con_date, con_num, owner, sale, con, week, pric, dow, equ, fi, convnum, convdate)
	
	
	
	'Group totals
	price = price + pric
	down = down + dow
	equi = equi + equ
	fin = fin + fi
	
	con_cnt = con_cnt + 1
	
	writeln = writeln & "<tr>"
	writeln = writeln & "<td align='left'>" & con_date & "</td>"
	writeln = writeln & "<td>" & con_num & "</td>"
	writeln = writeln & "<td>" & owner & "</td>"
	writeln = writeln & "<td>" & sale & "</td>"
	writeln = writeln & "<td>" & con & "</td>"
	writeln = writeln & "<td>" & week & "</td>"
	if pric & "" <> "" then
		writeln = writeln & "<td align='right'>" & FormatCurrency(pric) & "</td>"
	else
		writeln = writeln & "<td align='right'>" & FormatCurrency(0) & "</td>"
	end if
	if dow & "" <> "" then
		writeln = writeln & "<td align='right'>" & FormatCurrency(dow) & "</td>"
	else
		writeln = writeln & "<td align='right'>" & FormatCurrency(0) & "</td>"
	end if
	if equ & "" <> "" then
		writeln = writeln & "<td align='right'>" & FormatCurrency(equ) & "</td>"			
	else
		writeln = writeln & "<td align='right'>" & FormatCurrency(0) & "</td>"			
	end if
	if fi & "" <> "" then
		writeln = writeln & "<td align='right'>" & FormatCurrency(fi) & "</td>"
	else
		writeln = writeln & "<td align='right'>" & FormatCurrency(0) & "</td>"
	end if 
	writeln = writeln & "<td>" & convnum & "" & "</td>"
	writeln = writeln & "<td>" & convdate & "" & "</td>"
	writeln = writeln & "</tr>"
	
End Function

'Tabel Label
Function Label(stat)

	writeln = writeln & "<tr>"
	writeln = writeln & "<td><b><font size=5 color='darkgray'>" & stat & "</font></b></td>"
	writeln = writeln & "<td colspan=9>&nbsp;</td>"
	writeln = writeln & "</tr>"
	
	writeln = writeln & "<tr><td colspan=10>&nbsp;</td></tr>"
	
	writeln = writeln & "<tr>"
	writeln = writeln & "<td><b><font color='blue'>" & "Date" & "</font></b></td>"
	writeln = writeln & "<td align='left' width='10px'><b><font color='blue'>" & "Contract#" & "</font></b></td>"
	writeln = writeln & "<td align='center' width='100px'><b><font color='blue'>" & "Last Name" & "</font></b></td>"
	writeln = writeln & "<td align='center' width='100px'><b><font color='blue'>" & "First Name" & "</font></b></td>"
	writeln = writeln & "<td align = 'center' width = '100px'><b><font color = 'blue'>" & "Status Date" & "</font></b></td>"
	writeln = writeln & "<td align='center' width='230px'><b><font color='blue'>" & "Unit Week" & "</font></b></td>"
	writeln = writeln & "<td align='center' width='10px'><b><font color='blue'>" & "Freq" & "</font></b></td>"
	writeln = writeln & "<td align='center' width='230px'><b><font color='blue'>" & "DOT Date" & "</font></b></td>"
	writeln = writeln & "<td align='center' width='230px'><b><font color='blue'>" & "DOT Inst. #" & "</font></b></td>"
	writeln = writeln & "<td align='center' width='230px'><b><font color='blue'>" & "DOT Rec." & "</font></b></td>"
	writeln = writeln & "<td align='center' width='230px'><b><font color='blue'>" & "Reason" & "</font></b></td>"
	writeln = writeln & "<td align='center' width='230px'><b><font color='blue'>" & "DOC Inst. #" & "</font></b></td>"
	writeln = writeln & "<td align='center' width='230px'><b><font color='blue'>" & "DOC Rec. Date" & "</font></b></td>"
	writeln = writeln & "<td align='center' width='230px'><b><font color='blue'>" & "COTTAGE/TOWNES/ESTATES (C/T/E)" & "</font></b></td>"
	'writeln = writeln & "<td width='190px'><b><font color='blue'>" & "Sale Type" & "</font></b></td>"
	'writeln = writeln & "<td width='130px'><b><font color='blue'>" & "Contract Type" & "</font></b></td>"
	'writeln = writeln & "<td width='110px'><b><font color='blue'>" & "Week Type" & "</font></b></td>"
	'writeln = writeln & "<td width='90px' align='right'><b><font color='blue'>" & "Price" & "</font></b></td>"
	'writeln = writeln & "<td width='150px' align='right'><b><font color='blue'>" & "Down Payment" & "</font></b></td>"
	'writeln = writeln & "<td width='60px' align='right'><b><font color='blue'>" & "Equity" & "</font></b></td>"
	'writeln = writeln & "<td width='120px' align='right'><b><font color='blue'>" & "Financed" & "</font></b></td>"
	
	writeln = writeln & "</tr>"
	
	writeln = writeln & "<tr><td colspan=10>&nbsp;</td></tr>"
	
	 		
End Function

Function Label_old(stat)

	writeln = writeln & "<tr>"
	writeln = writeln & "<td><b><font size=5 color='darkgray'>" & stat & "</font></b></td>"
	writeln = writeln & "<td colspan=9>&nbsp;</td>"
	writeln = writeln & "</tr>"
	
	writeln = writeln & "<tr><td colspan=10>&nbsp;</td></tr>"
	
	writeln = writeln & "<tr>"
	writeln = writeln & "<td><b><font color='blue'>" & "Date" & "</font></b></td>"
	writeln = writeln & "<td align='left' width='10px'><b><font color='blue'>" & "Contract#" & "</font></b></td>"
	writeln = writeln & "<td align='center' width='230px'><b><font color='blue'>" & "Owner Name" & "</font></b></td>"
	writeln = writeln & "<td width='190px'><b><font color='blue'>" & "Sale Type" & "</font></b></td>"
	writeln = writeln & "<td width='130px'><b><font color='blue'>" & "Contract Type" & "</font></b></td>"
	writeln = writeln & "<td width='110px'><b><font color='blue'>" & "Week Type" & "</font></b></td>"
	writeln = writeln & "<td width='90px' align='right'><b><font color='blue'>" & "Price" & "</font></b></td>"
	writeln = writeln & "<td width='150px' align='right'><b><font color='blue'>" & "Down Payment" & "</font></b></td>"
	writeln = writeln & "<td width='60px' align='right'><b><font color='blue'>" & "Equity" & "</font></b></td>"
	writeln = writeln & "<td width='120px' align='right'><b><font color='blue'>" & "Financed" & "</font></b></td>"
	writeln = writeln & "<td width='120px' align='right'><b><font color='blue'>" & "Conveyance Instr #" & "</font></b></td>"
	writeln = writeln & "<td width='120px' align='right'><b><font color='blue'>" & "Conveyance Rec Date" & "</font></b></td>"
	writeln = writeln & "</tr>"
	
	writeln = writeln & "<tr><td colspan=10>&nbsp;</td></tr>"
	
	 		
End Function

'Group Total
Function Summ()
	
	'Running totals
	grd_price = grd_price + price
	grd_down = grd_down + down
	grd_equi = grd_equi + equi
	grd_fin = grd_fin + fin
	grd_cnt = grd_cnt + con_cnt
	
	writeln = writeln & "<tr><td colspan=10>&nbsp;</td></tr>"
	
	'Buid table row
	writeln = writeln & "<tr>"

	writeln = writeln & "<td colspan=4><b><i>" & "Total Contracts Changed : " &  con_cnt & "</i></b></td>"
	writeln = writeln & "<td colspan=2><b><i>" & "Sub Total : " & "</i></b></td>"
	if price & "" <> "" then
		writeln = writeln & "<td colspan=1 align='right'><b><i><font color='green'>" & FormatCurrency(price) & "</font></i></b></td>"
	else
		writeln = writeln & "<td colspan=1 align='right'><b><i><font color='green'>" & FormatCurrency(0) & "</font></i></b></td>"
	end if
	if down & "" <> "" then
		writeln = writeln & "<td align='right'><b><i><font color='green'>" & FormatCurrency(down) & "</font></i></b></td>"
	else
		writeln = writeln & "<td align='right'><b><i><font color='green'>" & FormatCurrency(0) & "</font></i></b></td>"
	end if
	if equi & "" <> "" then
		writeln = writeln & "<td align='right'><b><i><font color='green'>" & FormatCurrency(equi) & "</font></i></b></td>"
	else
		writeln = writeln & "<td align='right'><b><i><font color='green'>" & FormatCurrency(0) & "</font></i></b></td>"
	end if
	if fin & "" <> "" then
		writeln = writeln & "<td align='right'><b><i><font color='green'>" & FormatCurrency(fin) & "</font></i></b></td>"
	else
		writeln = writeln & "<td align='right'><b><i><font color='green'>" & FormatCurrency(0) & "</font></i></b></td>"
	end if

	writeln = writeln & "</tr>"
		
	
	'Clear for next group
	price = 0: down = 0: equi = 0: fin = 0: con_cnt = 0		
	
End Function


'Grand Total
Function Grand()

	writeln = writeln & "<tr><td colspan=10>&nbsp;</td></tr>"
	writeln = writeln & "<tr><td colspan=10>&nbsp;</td></tr>"
	writeln = writeln & "<tr><td colspan=10>&nbsp;</td></tr>"
	
	
	'Buid table row
	writeln = writeln & "<tr>"

	writeln = writeln & "<td colspan=4><b>" & "Total Contracts : " &  grd_cnt & "</b></td>"
	writeln = writeln & "<td colspan=2><b>" & "Grand Totals : " & "</b></td>"
	'if grd_price & "" <> "" then
		writeln = writeln & "<td colspan=1 align='right'><b><font color='red'><u>" & grd_price & "</u></font></b></td>"
	'else
	'	writeln = writeln & "<td colspan=1 align='right'><b><font color='red'><u>" & FormatCurrency(0) & "</u></font></b></td>"
	'end if
	'if grd_down & "" <> "" then
		writeln = writeln & "<td align='right'><b><font color='red'><u>" & grd_down & "</u></font></b></td>"
	'else
	'	writeln = writeln & "<td align='right'><b><font color='red'><u>" & Formatcurrency(0) & "</u></font></b></td>"
	'end if
	'if grd_equi & "" <> "" then
		writeln = writeln & "<td align='right'><b><font color='red'><u>" & grd_equi & "</u></font></b></td>"
	'else
	'	writeln = writeln & "<td align='right'><b><font color='red'><u>" & Formatcurrency(0) & "</u></font></b></td>"
	'end if
	'if grd_fin & "" <> "" then
		writeln = writeln & "<td align='right'><b><font color='red'><u>" & grd_fin & "</u></font></b></td>"
	'else
	'	writeln = writeln & "<td align='right'><b><font color='red'><u>" & Formatcurrency(0) & "</u></font></b></td>"
	'end if

	writeln = writeln & "</tr>"
			
End Function

%>
