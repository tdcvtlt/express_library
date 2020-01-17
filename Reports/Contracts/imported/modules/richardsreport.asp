<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd">
<%	

	'local grand variables
	dim tot_cot					'// running total for cottage
	dim tot_2bd					'// running total for 2 br
	dim tot_4bd					'// running total for 4 br
	dim tot_1bde				'// running total for 1 br Estate
	dim tot_2bde				'// running total for 2 br Estate
	dim tot_3bde				'// running total for 3 br Estate
	dim tot_4bde				'// running total for 4 br Estate
	
	dim cots					'// total cottages count
	dim bd2s					'// total 2 bedroom count
	dim bd4s					'// total 4 bedroom count
	dim bd1e					'// total 1 bedroom Estate
	dim bd2e					'// total 2 bedroom Estate
	dim bd3e					'// total 3 bedroom Estate
	dim bd4e					'// total 4 bedroom Estate
	
	'object variables
	dim oRs 
	dim oCn
	
	'html & sql variables
	dim writeln
	dim sql
	
	'parameters
	dim date_begin
	dim date_end
		
	'Group break
	dim id_curr
	dim id_last		
	dim in_loop
	
		

	date_begin = Request("start")
	date_end = Request("end")
	

	Set oCn = Server.CreateObject("ADODB.Connection")
	oCn.ConnectionTimeout = 0


    DBNAME = "CRMSNet"
    DBUser = "asp"
    DBpass = "aspnet"

	oCn.Open DBName, DBUser, DBPass

	sql = 	"select b.contractid, h.comboitem as salestype, b.statusdate, i.comboitem as contractstatus,  " & _
			"b.contractnumber, g.salesvolume, " & _
			"sum(cast(left(f.comboitem,1) as int)) as SubType, e.comboitem as unittype " & _
			"from (((((((t_soldinventory a " & _
			"left outer join t_contract b " & _
			"on a.contractid = b.contractid) " & _
			"left outer join t_salesinventory c " & _
			"on a.salesinventoryid = c.salesinventoryid) " & _
			"inner join t_unit d " & _
			"on c.unitid = d.unitid) " & _
			"left outer join t_comboitems e " & _
			"on d.typeid = e.comboitemid) " & _
			"left outer join t_comboitems f " & _
			"on d.subtypeid = f.comboitemid) " & _
			"inner join t_mortgage g " & _
			"on g.contractid = b.contractid) " & _
			"left outer join t_comboitems h " & _
			"on b.saletypeid = h.comboitemid)" & _
			"left outer join t_comboitems i " & _
			"on b.statusid = i.comboitemid " & _
			"where b.statusdate is not null " & _
			"and ((b.contractdate between '" & date_begin & "' and '" & date_end & "') " & _
			"or (b.statusdate between '" & date_begin & "' and '" & date_end & "' and b.contractid in " & _
            "(select keyvalue from t_Event where keyvalue > 0 and keyfield = 'contractid' and (oldvalue = 'pender' or oldvalue = 'pender-inv')) " & _
            "and b.contractdate < '" & date_begin & "')) " & _
			"and (i.comboitem = 'active' or i.comboitem = 'suspense') " & _
			"and (h.comboitem = 'Cottage Golf New' or h.comboitem = 'Cottage Golf Upgrade' " & _
			"or h.comboitem = 'Townhouse New' or h.comboitem = 'Townhouse Upgrade' or h.comboitem = 'Estate' or h.comboitem = 'Estate ReWrite' ) " & _
			"and b.contractnumber not like 'a%' " & _
			"group by b.contractid, b.contractnumber,h.comboitem,b.statusdate,i.comboitem, g.salesvolume,e.comboitem " & _
			"order by b.contractid, b.contractnumber"
			

	'Make Object and Run Open
	Set oRs = Server.CreateObject("ADODB.RecordSet")
	set rs = Server.CreateObject("ADODB.RecordSet")
	oRs.Open sql, oCn, 3, 3
	

	If Not (oRs.BOF And oRs.EOF) Then
		
		writeln = "<table border='1px' cellspacing='3' cellpadding='1' width='100%' style='border-collapse:collapse;'>"
		writeln = writeln & "<col width='50'>"			'Contract#
		writeln = writeln & "<col width='125'>"			'Running total calculated field		
		writeln = writeln & "<col width='150'>"			'Contract Status
		writeln = writeln & "<col width='100'>"			'Cottage
		writeln = writeln & "<col width='100'>"			'2 BD
		writeln = writeln & "<col width='100'>"			'4 BD
		writeln = writeln & "<col width='185'>"			'Sales Type
						
		in_loop = False
		
		tot_cot = 0
		tot_2bd = 0
		tot_4bd = 0
		tot_1bde = 0
		tot_2bde = 0
		tot_3bde = 0
		tot_4bde = 0
		
		
		dim i: dim b_2bd: dim b_cot: dim b_show: dim cnt_townes: dim cnt_estates				
		dim con_num
		dim con_stat
		dim c24
		dim con_type		
		dim type_id
		dim b_l_cot			'// cottage buffer
		dim b_l_2bd			'// 2bd buffer
		
		cnt_townes = 0
		cnt_estates = 0
		cnt_cottage = 0
		cots = 0
		bd2s = 0
		bd4s = 0
		bd1e = 0
		bd2e = 0
		bd3e = 0
		bd4e = 0
		
		Call Label()
				
		Do While Not oRs.EOF
			rs.open "Select top 1 * from t_Event where keyvalue = '" & oRs.fields("ContractID").value & "' and keyfield = 'contractid' and datecreated > '" & cdate(oRs.fields("StatusDate").value) & "' and newvalue = '" & oRs.fields("ContractStatus").value & "' and fieldname = 'ContractStatus' order by datecreated desc", ocn, 0, 1
			if rs.eof and rs.bof then
				bCont2 = true
			else
				if (lcase(trim(rs.fields("Oldvalue").value & "")) = "pender" or lcase(trim(rs.Fields("Oldvalue").value & "")) = "pender-inv") and cdate(oRs.fields("StatusDate").value) <= cdate(date_end) then 
					bCont2 = true
				else
					bCont2 = false
				end if
			end if
			rs.close
			'bCont2 = true
			if bCont2 then 
				cot = 0
				e1 = 0
				e2 = 0 
				e3 = 0
				e4 = 0
				t2 = 0
				t4 = 0	
				Select Case oRs.Fields("UnitType")
					'Townes
					Case "Townes"
						Select Case oRs.fields("SubType").value
							Case "2"
								if oRs.fields("SalesVolume").value & "" <> "" then 
									tot_2bd = tot_2bd + oRs.fields("SalesVolume").value
									t2 = oRs.fields("SalesVolume").value
								end if
								bd2s = bd2s + 1
								
							Case "4"
								if oRs.fields("SalesVolume").value & "" <> "" then 
									tot_4bd = tot_4bd + oRs.fields("SalesVolume").value
									t4 = oRs.fields("SalesVolume").value
								end if
								bd4s = bd4s + 1
							Case Else
						End Select			
						
						b_show = true
							
					'Cottage										
					Case "Cottage"
						if InStr(1, oRs.Fields("SalesType"), "Golf", 1) > 0 then
							cots = cots + 1
							if oRs.fields("SalesVolume").value & "" <> "" then 
								tot_cot = tot_cot + oRs.fields("SalesVolume").value
								cot = oRs.fields("SalesVolume").value
							end if
						end if
						
					'Estate
					Case "Estates"
					
						Select Case oRs.fields("SubType").value
							case "1"
								bd1e = bd1e + 1
								if oRs.fields("SalesVolume").value & "" <> "" then 
									tot_1bde = tot_1bde + oRs.fields("SalesVolume").value
									e1 = oRs.fields("SalesVolume").value
								end if
							case "2"
								bd2e = bd2e + 1
								if oRs.fields("SalesVolume").value & "" <> "" then 
									tot_2bde = tot_2bde + oRs.fields("SalesVolume").value
									e2 = oRs.fields("SalesVolume").value
								end if
							case "3"
								bd3e = bd3e + 1
								if oRs.fields("SalesVolume").value & "" <> "" then 
									tot_3bde = tot_3bde + oRs.fields("SalesVolume").value
									e3 = oRs.fields("SalesVolume").value
								end if
							case "4"
								bd4e = bd4e + 1
								if oRs.fields("SalesVolume").value & "" <> "" then 
									tot_4bde = tot_4bde + oRs.fields("SalesVolume").value
									e4 = oRs.fields("SalesVolume").value
								end if
							case else
						end select
						b_show = true
				End Select	
				
											
				If b_show Then
					Call Details()					
				End If	
			end if
			oRs.MoveNext
			
		Loop
		
		'Must display last record
		'Call Details()
	
		'Display summaries
		Call Summarize()	
	Else
	
		writeln = "<b>" & "No Records Match Your Criteria" & "</b>"
	
	End If	
		
	'Cleaning up...
	'oRs.Close
	oCn.Close
	
	set rs = nothing
	Set oRs = Nothing
	Set oCnn = Nothing
				
	
	'HTML...
	Response.Write writeln
	
	'''THE END!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
				


Function Label()
	'
	'Label header
	'
	writeln = writeln & "<tr><td colspan=11>&nbsp;</td></tr>"
		
	writeln = writeln & "<tr>"

	writeln = writeln & "<td><b><font color='darkblue'>" & "Contract #" & "</font></b></td>"
	writeln = writeln & "<td align='right'><b><font color='darkblue'>" & "Cumulative" & "</font></b></td>"
	
	writeln = writeln & "<td align='left'><b><font color='darkblue'>" & "Contract Status" & "</font></b></td>"
	
	writeln = writeln & "<td align='right'><b><font color='darkblue'>" & "Cottage" & "</font></b></td>"
	writeln = writeln & "<td align='right'><b><font color='darkblue'>" & "2 BD Townes" & "</font></b></td>"
	writeln = writeln & "<td align='right'><b><font color='darkblue'>" & "4 BD Townes" & "</font></b></td>"
	writeln = writeln & "<td align='right'><b><font color='darkblue'>" & "1 BD Estates" & "</font></b></td>"
	writeln = writeln & "<td align='right'><b><font color='darkblue'>" & "2 BD Estates" & "</font></b></td>"
	writeln = writeln & "<td align='right'><b><font color='darkblue'>" & "3 BD Estates" & "</font></b></td>"
	writeln = writeln & "<td align='right'><b><font color='darkblue'>" & "4 BD Estates" & "</font></b></td>"
	writeln = writeln & "<td align='left'><b><font color='darkblue'>" & "Sales Type" & "</font></b></td>"	
	
	writeln = writeln & "</tr>"	
	
End Function


Function Details()
	writeln = writeln & "<tr>" 
		
	writeln = writeln & "<td><i><b>" & oRs.fields("ContractNumber").value & "</b></i></td>"
	writeln = writeln & "<td align='right'>" 
	Select Case oRs.fields("UnitType").value
		case "Cottage"
			writeln = writeln & FormatCurrency(tot_cot)
		case "Townes"
			Select Case oRs.fields("SubType").value 
				Case "2"
					writeln = writeln & FormatCurrency(tot_2bd)
				Case "4"
					writeln = writeln & FormatCurrency(tot_4bd)
			End Select 
		case "Estates"
			Select Case oRs.fields("SubType").value
				Case "1"
					writeln = writeln & FormatCurrency(tot_1bde)
				Case "2" 
					writeln = writeln & FormatCurrency(tot_2bde)
				Case "3" 
					writeln = writeln & FormatCurrency(tot_3bde)
				Case "4"
					writeln = writeln & FormatCurrency(tot_4bde)
			end select
	End Select
	writeln = writeln & "</td>"

	writeln = writeln & "<td align='left'>" & Space(15) & oRs.fields("ContractStatus").value & "</td>"
	writeln = writeln & "<td align='right'>" & FormatCurrency(cot) & "</td>"
	writeln = writeln & "<td align='right'>" & FormatCurrency(t2) & "</td>"
	writeln = writeln & "<td align='right'>" & FormatCurrency(t4) & "</td>"
	writeln = writeln & "<td align='right'>" & FormatCurrency(e1) & "</td>"
	writeln = writeln & "<td align='right'>" & FormatCurrency(e2) & "</td>"
	writeln = writeln & "<td align='right'>" & FormatCurrency(e3) & "</td>"
	writeln = writeln & "<td align='right'>" & FormatCurrency(e4) & "</td>"

	writeln = writeln & "<td align='left'>" & oRs.fields("UnitType").value & "</td>"

	writeln = writeln & "</tr>"		
End Function

				
Function Summarize()	

	writeln = writeln & "<tr><td colspan=11>&nbsp;</td></tr>"
	
	writeln = writeln & "<tr>"	
	
	writeln = writeln & "<td colspan='3'>&nbsp;</td>"
	writeln = writeln & "<td align='right'><b><font color='red'>" & FormatCurrency(tot_cot) & "</font></b></td>"
	writeln = writeln & "<td align='right'><b><font color='red'>" & FormatCurrency(tot_2bd) & "</font></b></td>"
	writeln = writeln & "<td align='right'><b><font color='red'>" & FormatCurrency(tot_4bd) & "</font></b></td>"
	writeln = writeln & "<td align='right'><b><font color='red'>" & FormatCurrency(tot_1bde) & "</font></b></td>"
	writeln = writeln & "<td align='right'><b><font color='red'>" & FormatCurrency(tot_2bde) & "</font></b></td>"
	writeln = writeln & "<td align='right'><b><font color='red'>" & FormatCurrency(tot_3bde) & "</font></b></td>"
	writeln = writeln & "<td align='right'><b><font color='red'>" & FormatCurrency(tot_4bde) & "</font></b></td>"
	writeln = writeln & "<td>&nbsp;</td>"
	
	writeln = writeln & "</tr>"	
	
	writeln = writeln & "<tr><td colspan=11>&nbsp;</td></tr>"
	writeln = writeln & "<tr><td colspan=11>&nbsp;</td></tr>"
	writeln = writeln & "<tr><td colspan=11>&nbsp;</td></tr>"
	
	writeln = writeln & "<tr>"	
	
	writeln = writeln & "<td style='width:100px'><b>" & "Cottage : " & "</b></td>"
	writeln = writeln & "<td align='right'><b>" & cots & "</b></td>"
	writeln = writeln & "<td><b>" & FormatCurrency(tot_cot) & "</b></td>"
	
	writeln = writeln & "</tr>"	
	
	writeln = writeln & "<tr>"	
	
	writeln = writeln & "<td><b>" & "2 BD Townes : " & "</b></td>"
	writeln = writeln & "<td align='right'><b>" & bd2s & "</b></td>"
	writeln = writeln & "<td><b>" & FormatCurrency(tot_2bd) & "</b></td>"
	
	writeln = writeln & "</tr>"	


	writeln = writeln & "<tr>"	
	
	writeln = writeln & "<td><b>" & "4 BD Townes : " & "</b></td>"
	writeln = writeln & "<td align='right'><b>" & bd4s & "</b></td>"
	writeln = writeln & "<td><b>" & FormatCurrency(tot_4bd) & "</b></td>"
	
	writeln = writeln & "</tr>"		


	writeln = writeln & "<tr>"	
	
	writeln = writeln & "<td><b>" & "1 BD Estates : " & "</b></td>"
	writeln = writeln & "<td align='right'><b>" & bd1e & "</b></td>"
	writeln = writeln & "<td><b>" & FormatCurrency(tot_1bde) & "</b></td>"
	
	writeln = writeln & "</tr>"		


	writeln = writeln & "<tr>"	
	
	writeln = writeln & "<td><b>" & "2 BD Estates : " & "</b></td>"
	writeln = writeln & "<td align='right'><b>" & bd2e & "</b></td>"
	writeln = writeln & "<td><b>" & FormatCurrency(tot_2bde) & "</b></td>"
	
	writeln = writeln & "</tr>"		


	writeln = writeln & "<tr>"	
	
	writeln = writeln & "<td><b>" & "3 BD Estates : " & "</b></td>"
	writeln = writeln & "<td align='right'><b>" & bd3e & "</b></td>"
	writeln = writeln & "<td><b>" & FormatCurrency(tot_3bde) & "</b></td>"
	
	writeln = writeln & "</tr>"		


	writeln = writeln & "<tr>"	
	
	writeln = writeln & "<td><b>" & "4 BD Estates : " & "</b></td>"
	writeln = writeln & "<td align='right'><b>" & bd4e & "</b></td>"
	writeln = writeln & "<td><b>" & FormatCurrency(tot_4bde) & "</b></td>"
	
	writeln = writeln & "</tr>"	

	
	
End Function	
	
	
%>