
<HTML>
	<HEAD>
	<meta name="Microsoft Theme" content="none, default">
</HEAD>

		<BODY>
<%

		dim rs 
		dim cn
		dim sdate
		dim edate
		dim rsl
        dim DBName
        Dim DBUser
        Dim DBPass
        DBName = "CRMSNET"
        DBUser = "asp"
        DBPass = "aspnet"		
		set rsl = server.createobject("ADODB.Recordset")
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")
		sdate = request("sdate")
		edate = request("edate")
		cn.commandtimeout = 0
		server.scripttimeout = 10000
		cn.open DBName, DBUser, DBPass
%>	

<%
sSQL = "SELECT distinct tCamp.name as campaignname, f.frequency, ContractStatus.ComboItem as ContractStatus, t_Contract.ContractDate, t_Contract.StatusDate, t_Contract.ContractNumber, t_Contract.OccupancyDate, t_Prospect.LastName + ',' + t_Prospect.FirstName as Name, t_ProspectAddress.Address1 as address, t_ProspectAddress.City + ', ' + t_ComboItems.ComboItem as state, t_ProspectAddress.PostalCode as zip, ContractSaleType.ComboItem as ContractSalesType, ContractType.ComboItem as ContractType, t_Contract.StatusDate, ContractWeekType.ComboItem as ContractWeekType, t_Mortgage.SalesVolume, t_Mortgage.SalesPrice, t_Mortgage.DPTotal, INV.Balance, t_Mortgage.TotalFinanced, t_Contract.ContractID, BillingCode.ComboItem as Line, SPOUSEFIRSTNAME + ' ' + SPOUSELASTNAME AS SPOUSENAME, t_Prospect.ProspectID " & _
	"FROM  t_Contract " & _
	"INNER JOIN t_Prospect ON t_Contract.ProspectID = t_Prospect.ProspectID " & _ 
	"INNER JOIN t_Mortgage ON t_Contract.ContractID = t_Mortgage.ContractID " & _
    "LEFT OUTER JOIN V_INVOICES INV ON INV.KEYVALUE = T_MORTGAGE.DPInvoiceID AND INV.KEYFIELD='MORTGAGEDP' " & _
	"LEFT OUTER JOIN t_comboitems ContractStatus ON t_Contract.StatusID = ContractStatus.ComboItemID " & _ 
	"LEFT OUTER JOIN t_comboitems ContractSaleType ON t_Contract.SaleTypeID = ContractSaleType.ComboItemID " & _ 
	"LEFT OUTER JOIN t_comboitems ContractType ON t_Contract.TypeID = ContractType.ComboItemID " & _
	"LEFT OUTER JOIN t_comboitems ContractWeekType ON t_Contract.WeekTypeID = ContractWeekType.ComboItemID " & _
	"LEFT OUTER JOIN t_comboitems BillingCode ON t_Contract.BillingCodeID = BillingCode.ComboItemID " & _
    "LEFT OUTER JOIN t_ProspectAddress ON t_ProspectAddress.PROSPECTID = t_Prospect.PROSPECTID " & _
	"LEFT OUTER JOIN t_ComboItems ON t_ProspectAddress.Stateid = t_ComboItems.ComboItemID " & _
	"LEFT OUTER JOIN t_Frequency f on f.frequencyID = t_Contract.frequencyid " & _
	"LEFT OUTER JOIN t_Tour t ON t.tourid = t_Contract.Tourid " & _
	"LEFT OUTER JOIN t_Campaign tCamp on tCamp.campaignid = t.campaignid " & _
    "LEFT OUTER JOIN t_ComboItems tl on t.TourLocationID = tl.ComboitemID "
if request("Location")= "Richmond" then
	ssql=ssql & "WHERE t_Contract.ContractNumber NOT  LIKE 'T%' and t_Contract.ContractNumber Not Like 'U%' and t_Contract.ContractNumber like'%S%'"
elseif request("Location") = "Outfield" then
    ssql=ssql & "WHERE tl.ComboItem = 'Outfield' and t_Contract.ContractNumber NOT  LIKE 'T%' and t_Contract.ContractNumber Not Like 'U%' and t_Contract.ContractNumber not like'%S%' "
elseif request("Location") = "Woodbridge" then
	ssql=ssql & "WHERE tl.ComboItem = 'Woodbridge' and t_Contract.ContractNumber NOT  LIKE 'T%' and t_Contract.ContractNumber Not Like 'U%' and t_Contract.ContractNumber not like'%S%' "
else
	ssql=ssql & "WHERE tl.ComboItem = 'KCP' and t_Contract.ContractNumber NOT  LIKE 'T%' and t_Contract.ContractNumber Not Like 'U%' and t_Contract.ContractNumber not like'%S%' "
end if
ssql = ssql & " and (t_Contract.ContractDate between '" & sdate & "' and '" & edate & "' OR  " & _
			"(t_Contract.StatusDate between '" & sdate & "' and '" & edate & "' and  " & _ 
			"t_Contract.contractid in " & _
			"(select KEYVALUE AS contractid from t_Event where T_EVENT.KEYFIELD='CONTRACTID' AND t_event.KEYVALUE >0 and " & _
			"(t_event.oldvalue in ('Pender','developer','Pender-inv') and t_event.newvalue in ('active','suspense','rescinded','cxl-pender','cxl-upgrade','res-pender','Developer') or " & _
			"t_Event.oldvalue in ('active','suspense') and t_Event.newvalue in ('cxl-downgrade','cxl-upgrade','canceled','developer','kick','pender','rescinded','pender-inv', 'cxl-bankruptcy')) " & _
			"and t_Event.datecreated between '" & sdate & "' and '" & cdate(edate) + 1 & "'))) "
sSQL = sSQL & 	"ORDER BY BillingCode.ComboItem, ContractStatus.ComboItem, ContractNumber"
rs.open  sSQL ,cn, 0, 1
%>
<table align = right>
	<tr>
		<td>
			<%=response.write (now)%>
		</td>
	</tr>
	<tr>
		<td><b>
			Start Date:
		</b></td>
		<td align = left>
			<%=response.write (sdate)%>
		</td>
	</tr>
	<tr>
		<td><b>
			End Date:
		</b></td>
		<td align = left>
			<%=response.write (edate)%>
		</td>
	</tr>
</table>
<br>
<br>
<br>
<%
dim lastline
dim laststatus


lastline = ""
laststatus = ""
do while not rs.eof



%>	
<%


	
	'****** BEGIN LINE ******
	if lastline <> rs.fields("Line").value & "" then
		lastline = rs.fields("Line").value & ""
		laststatus = ""
%>

<b><%=rs.fields("line").value%></b>
<%
	end if
	'****** END LINE ******
	'****** BEGIN STATUS ******
	if laststatus <> rs.fields("ContractStatus").value & "" then
		laststatus = rs.fields("ContractStatus").value & ""
		
%>
<b><%=rs.fields("ContractStatus").value%></b>
<%
	end if
	'****** END STATUS ******
%>	
<TABLE cellspacing = 2 >
<%
	dim rsPersonnel
	set rsPersonnel = server.createobject("ADODB.Recordset")
	rsPersonnel.open "Select p.lastname + ', ' + p.firstname as Name, l.comboitem as Title from t_personneltrans t inner join t_Personnel p on p.personnelid = t.KEYVALUE  inner join t_Comboitems l on l.comboitemid = t.titleid where T.KEYFIELD='CONTRACTID' AND t.KEYVALUE = '" & rs.fields("ContractID").value & "' and l.comboitem = 'Sales executive'", cn, 0, 1
	if rsPersonnel.eof and rsPersonnel.bof then
%>
		<th colspan=8>	
<%
	else
%>
		<th colspan=8>Sales Exec: <%=rsPersonnel.fields("Name").value%><br>
	
<%
	end if
	rsPersonnel.close
	rsPersonnel.open "Select p.lastname + ', ' + p.firstname as Name, l.comboitem as Title from t_personneltrans t inner join t_Personnel p on p.personnelid = t.KEYVALUE  inner join t_Comboitems l on l.comboitemid = t.titleid where T.KEYFIELD='CONTRACTID' AND t.KEYVALUE = '" & rs.fields("ContractID").value & "' and l.comboitem = 'TO'", cn, 0, 1
	if rsPersonnel.eof and rsPersonnel.bof then
%>
		</th>
<%
	else
%>
		TO: <%=rsPersonnel.fields("Name").value%></th>
<%
	end if 
	rsPersonnel.close
	set rsPersonnel = nothing
%>
		
		
		<th><u>Status Date</u></th>
		<th><u>Sales Volume</u></th>
		<th><u>Sales Price </u></th>
		<th><u>Down</u></th><th><u>Balance</u></th>
		<th align = right><u>Financed</u></th>
	

	

	<tr></tr>
	<tr><td colspan=2></td><td colspan=11><b>Tour Campaign:</b> <%=rs.fields("campaignname").value%></td></tr>
	<tr>
		<td></td><td ><%=rs.fields("ContractDate").value%></td>
		<td ></td><td ><b><%=rs.fields("ContractNumber").value%></b></td>
		<td align = center  ><%=rs.fields("ContractSalesType").value%></td>
		<td align = right ><%=rs.fields("ContractType").value%> - <%=rs.fields("Frequency").value%></td>
		<td align = center  ><%=rs.fields("OccupancyDate").value%></td>
		<td align = center ><%=rs.fields("Name").value%><br><%=rs.fields("SpouseName").value%></td>
		<td align= center><%=rs.Fields("StatusDate").value %></td>
        <td align = center  >

<%
	if rs.fields("SalesVolume").value & "" = "" then
		response.write formatcurrency(0)
	else 
		response.write formatcurrency(rs.fields("SalesVolume").value)
	end if
%>
		</td>
		<td align = center >
<%
	if rs.fields("SalesPrice").value & "" = "" then
		response.write formatcurrency(0)
	else
		response.write formatcurrency(rs.fields("SalesPrice").value)
	end if

%>
		</td>
		<td align = center  >
<%
	if rs.fields("DPTotal").value &"" = "" then
		response.write formatcurrency(0)
	else 
		response.write formatcurrency(rs.fields("DPTotal").value)
	end if
%>
		</td>
		<td align = center  >
<%
	if rs.fields("Balance").value & "" = "" then
		response.write formatcurrency(0)
	else 
		response.write formatcurrency(rs.fields("Balance").value)
	end if
%>
		</td>
		<td align = center  >
<%
	if rs.fields("TotalFinanced").value & "" = "" then
		response.write formatcurrency(0)
	else 
		response.write formatcurrency(rs.fields("TotalFinanced").value)
	end if
%>
		</td>
	</tr>
	<tr></tr>
	<tr>
		<td colspan = 8 align = right>



<%
	'*********************BEGIN CO-OWNER*********************
	dim id 
	dim rso
	dim pid
	
	pid = rs.fields("ProspectID").value	
	id = rs.fields("ContractID").value
	set rso = server.createobject("ADODB.Recordset")
				
	If Len(id) > 0 Then
		
		rso.Open 	"select distinct b.lastname + ', ' + b.firstname as coowner from t_contractcoowner a " & _
					"inner join t_prospect b on a.prospectid = b.prospectid " & _
					"where a.contractid = '" & id & "' and b.prospectid <> '" & pid & "' ", cn, 0, 1
						
		
			if not (rso.bof and rso.eof) then
				
			do While Not rso.EOF 
				
				if rso.fields("CoOwner").value = rs.fields("name").value then		
				else
						Response.Write rso.fields("CoOwner").value & "<br>"				
						
				end if
				rso.MoveNext
			loop

		End If						
	End If
		
	rso.Close
	set rso = nothing
	
	'*********************END CO-OWNER*********************
		
%>





			
		</td>
	</tr>
	
	<tr>
		<td></td><td ></td>
		<td align = center  ></td>
		<td align = center  ></td>
		<td align = center  ></td>
		<td align = center ><%=rs.fields("ContractWeekType").value%></td>
		<td></td>
		<td align = right colspan="2"><%=rs.fields("Address").value%></td>
		<td align = center colspan = "2"><%=rs.fields("State").value & ""%></td>
		
		<td align = left colspan = "1"><%=rs.fields("zip").value%></td>
		
	</tr>
	<tr></tr>
	<tr></tr>
	<tr></tr>
<%
	'****** BEGIN TEST ******
%>
	<tr>
		<td></td><td ></td>
		<td align = center  ></td>
		<td align = center  ></td>
		<td align = center  ></td>
		<td align = center ></td>
		<td align = center  ></td>
		<td align = center ></td>
		<td align = center  ></td>
		<td align = center  ></td>
		<td align = center ></td>
	</tr>

	<tr></tr>
	<tr></tr>
<%
	'****** BEGIN INVENTORY ******
	rsl.open "Select * from t_Soldinventory s inner join t_Salesinventory i on i.salesinventoryid = s.salesinventoryid inner join t_Unit u on u.unitid = i.unitid where s.contractid = '" & rs.fields("ContractID").value & "'",cn,0, 1
	do while not rsl.eof
%>
	<tr>
		<td></td><td ></td>
		<td align = center  ></td>
		<td align = center  ></td>
		<td align = center colspan = "3"><%=rsl.fields("Name").value%></td>
		
		<td align = center ><%=rsl.fields("Week").value%></td>
		<td align = center  ></td>
		<td align = center  ></td>
		<td align = center ></td>
	</tr>
	<tr></tr>
<%
		rsl.movenext
	loop
	rsl.close
	'****** END INVENTORY ******

	'****** BEGIN NOTES ******
	rsl.open "Select * from t_Note where KEYFIELD='CONTRACTID' AND KEYVALUE = '" & rs.fields("ContractID").value & "'",cn,0, 1
	do while not rsl.eof

%>
	<tr></tr>
	<tr></tr>
	<tr></tr>
	<tr></tr>
	<tr>
		<td></td><td ></td>
		<td align = center  ></td>
		<td align = center  ></td>
		<td align = center  ></td>
		<td align = left colspan = 8><%=rsl.fields("Note").value%></td>

	</tr>
	
<%
		rsl.movenext
	loop
	rsl.close
	'****** END Notes ******
%>
</TABLE>
<hr>
<%
	rs.movenext
loop
	
	rs.close
	cn.close
	set rs = nothing
	set rs1 = nothing
	set cn = nothing
	response.write now

%>
 
	</body>
</html>