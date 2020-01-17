
<%		
	Server.ScriptTimeout = 10000
		
	dim oRs
	dim oCn	
	dim ln
	dim sql
	dim date_begin
	dim date_end	
	dim cls_tot
	dim i
	dim cnt_con
	
	date_begin 	= Request("date_begin")
	date_end  	= Request("date_end")
		
	
	sql = 	"select status = (select comboitem from t_comboitems where comboitemid = a.statusid), " & _
			"a.contractnumber, rtrim(c.lastname) + ', ' + rtrim(c.firstname) as owner, " & _
			"saletype = (select comboitem from t_comboitems where comboitemid = a.saletypeid), " & _
			"type = (select comboitem from t_comboitems where comboitemid = a.typeid), " & _
			"b.dptotal, b.totalfinanced, b.cctotal, b.salesvolume, b.salesprice, b.discount, " & _
			"a.ContractDate, " & _
			"a.StatusDate, " & _			
			"b.origsellingprice, null as 'User13', null as 'Equity' " & _
			"from t_contract a " & _
			"inner join t_mortgage b " & _
			"on a.contractid = b.contractid " & _
			"inner join t_prospect c " & _
			"on a.prospectid = c.prospectid " & _
			"left outer join t_comboitems d " & _
			"on a.saletypeid = d.comboitemid " & _
			"left outer join t_comboitems e " & _
			"on e.comboitemid = a.statusid " & _
			"where  (a.statusdate between '" & CDate(date_begin) & "' and '" & CDate(date_end) & "') " & _
			"and (e.comboitem like '%can%' or e.comboitem like '%cxl%' or e.comboitem like 'res%') " & _
            "and a.contractnumber not like 't%' and a.contractnumber not like 'u%' " & _
            "ORDER BY a.ContractDate DESC, rtrim(c.LastName)"


	Set oCn = Server.CreateObject("ADODB.Connection")
	oCn.ConnectionTimeout = 0
	    
    
    DBName = "CRMSNet"
    DBUser = "asp"
    DBPass = "aspnet"

	oCn.Open DBName, DBUser, DBPass		
	
	Set oRs = Server.CreateObject("ADODB.RecordSet")
	oRs.CursorType = 3
	oRs.CursorLocation = 3
	oRs.Open sql, oCn, 3, 3	
	
	
	response.ContentType = "application/vnd.ms-excel"
	response.AddHeader "Content-Disposition", "attachment; filename=CancelledContracts.xls"
	
	
	If Not (oRs.BOF And oRs.EOF) Then
	
		response.write  "<strong>" & FormatDateTime(Now, 1) & "</strong>"
		
		response.write  "<br/>"
		response.write  "<table border=0  cellspacing=0 width='100%'>" 
		
		response.write  "<tr><th width=320>" & "Owner" & "</th><th width=40 align='center'>" & "Days" & "</th>"		    
		response.write  "<th width=90>" & "KCP" & "</th>"
		response.write  "<th>" & "Cancel Date" & "</th>"
		response.write  "<th width=120>" & "Type" & "</th>" 
		
		response.write  "<th width=80>" & "Volume" & "</th><th width=110 align='justify'>" & "Discount" & "</th>" 		
		response.write  "<th width=120>" & "Closing Cost" & "</th><th width=130 align='justify'>" & "Total Price" & "</th>" 		
		response.write  "<th width=120 align='justify'>" & "Down Total" & "</th><th width=120 align='justify'>" & "Equity" & "</th>" 						
		response.write  "<th width=100 align='justify'>" & "Financed" & "</th></tr>" 		
				

		Redim cls_tot(1, 8)
						
		While Not oRs.EOF
		
			response.write  "<tr>"
			
			response.write  "<td class='bg'>" & oRs.Fields("Owner") & "</td>"
			response.write  "<td align='left' class='days'>" & DateDiff("d", oRs.Fields("ContractDate"), Now) & "</td>"
			response.write  "<td class='kcp'>" & oRs.Fields("ContractNumber") & "</td>"
			if oRs.Fields("StatusDate") = oRs.Fields("ContractDate") then
				response.write  "<td>" & oRs.Fields("User13") & "</td>"
			else
				response.write  "<td>" & oRs.Fields("StatusDate") & "</td>"
			end if
			response.write  "<td align =  center>" & oRs.Fields("Type") & "</td>"						
			
			response.write  "<td class='volume' align='right'>" & FormatCurrency(oRs.Fields("SalesVolume")) & "</td>"						
			response.write  "<td class='discount' align='right'>" 
			if oRs.fields("Discount").value & "" <> "" then
				response.write  FormatCurrency(oRs.Fields("Discount")) 
			else
				response.write  formatcurrency(0)
			end if
			response.write  "</td>"																
			response.write  "<td class='closing' align='right'>" & FormatCurrency(oRs.Fields("CCTotal")) & "</td>"									
			response.write  "<td class='alignright' align='right'>" & FormatCurrency(oRs.Fields("OrigSellingPrice")) & "</td>"		
			response.write  "<td class='down' align='right'>" & FormatCurrency(oRs.Fields("DPTotal")) & "</td>"		
			response.write  "<td align='right'>" 
			if oRs.fields("Equity").value & "" <> "" then
				response.write  FormatCurrency(oRs.Fields("Equity")) 
			else
				response.write  formatcurrency(0)
			end if
			response.write   "</td>"		
			response.write  "<td class='financed' align='right'>" & FormatCurrency(oRs.Fields("TotalFinanced")) & "</td>"		
			
			response.write  "</tr>"
									
			cnt_con = cnt_con + 1
			
			dim j			
			For j = 0 To oRs.Fields.Count - 1
				
				Select Case lcase(trim(oRs.Fields(j).Name & ""))
				Case "salesvolume"
					if oRs.fields("SalesVolume") & "" <> "" then cls_tot(1,1) = cls_tot(1,1) + oRs.Fields("Salesvolume")

				Case "discount"
					
					if oRs.fields("Discount") & "" <> "" then cls_tot(1,2) = cls_tot(1,2) + oRs.Fields("Discount")

				Case "cctotal"
					
					if oRs.fields("CCTotal") & "" <> "" then cls_tot(1,3) = cls_tot(1,3) + oRs.Fields("CCTotal")

				Case "origsellingprice"
				
					if oRs.fields("OrigSellingPrice") & "" <> "" then cls_tot(1,4) = cls_tot(1,4) + oRs.Fields("OrigSellingPrice")	
					
				Case "dptotal"
				
					if oRs.fields("DPTotal") & "" <> "" then cls_tot(1,5) = cls_tot(1,5) + oRs.Fields("DPTotal")							


				Case "equity"
				
					if oRs.fields("Equity") & "" <> "" then cls_tot(1,6) = cls_tot(1,6) + oRs.Fields("Equity")									
						
				Case "totalfinanced"
				
					if oRs.fields("TotalFinanced") & "" <> "" then cls_tot(1,7) = cls_tot(1,7) + oRs.Fields("TotalFinanced")									

				End Select
				
			Next						
			
			oRs.MoveNext
		Wend
		
		response.write  "<tr><td colspan=12>&nbsp;</td></tr>"
		response.write  "<tr>"
		response.write  "<td colspan=4 align='right'><strong>" & "Total Contracts : " & cnt_con & "</strong></td>"
		response.write  "<td align='right'><strong>" & "Totals : " & "</strong></td>"
		
		For i = 1 to 7
			
			If i Mod 2 = 0 Then
				if cls_tot(1, i) & "" <> "" then
					response.write  "<td align='right'><font color='blue'>" & FormatCurrency(cls_tot(1, i)) & "</font></td>"	
				else
					response.write  "<td align='right'><font color='blue'>" & FormatCurrency(0) & "</font></td>"	
				end if
			Else
				response.write  "<td align='right'><font color='red'>" & FormatCurrency(cls_tot(1, i)) & "</font></td>"	
			End If	
		Next		
		response.write  "</tr>"		

	Else
		
		response.write  "<br/><br/><br/><br/><br/>"
		response.write  "<strong>" & "No Records Matching Your Criteria Found!" & "</strong>"
	
	End If	
%>