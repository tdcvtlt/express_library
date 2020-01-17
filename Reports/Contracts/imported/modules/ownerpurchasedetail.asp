

<html>

<head>
<title>ownerpurchasedetail.asp</title>
<meta name="Microsoft Theme" content="none, default">
</head>

<body>
<%
DBName = "CRMSNET"
DBUser = "asp"
DBPass = "aspnet"
dim cn
dim rs
dim rs1


dim hphone

hphone = Split(request("hphone"), "|")
set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.recordset")
set rs1 = server.createobject("ADODB.recordset") 
cn.commandtimeout = 10000
cn.open DBName, DBUser, DBPass

for i = 0 to UBound(hphone)

rs.open  "Select x.*,pa.Address1, pa.City, pa.PostalCode, pa.StateID as StateOrProvince from " & _
"(SELECT  t_Contract.ContractID,t_Contract.OccupancyDate, p.ProspectID, pp.Number as HomePhone, p.FirstName + ' ' + p.LastName as Name, p.SpouseFirstName + ' ' + p.SpouseLastName as SpouseName, t_Contract.ContractNumber, t_Mortgage.Terms, t_Mortgage.SalesPrice, t_Contract.ContractDate, t_Contract.FrequencyID,  t_Mortgage.TotalFinanced, t_Contract.StatusID, t_Contract.SubStatusID, PaymentFee,'' as Status, bc.ComboItem as BillingCode, apr, " & _
" (Select top 1 AddressID from t_prospectAddress where prospectid = p.ProspectID order by activeflag desc, contractaddress desc, addressid desc) as AddressID " & _
" FROM   t_Contract " & _
"INNER JOIN t_Mortgage ON t_Contract.ContractID = t_Mortgage.ContractID " & _
"LEFT OUTER JOIN t_ComboItems bc on t_Contract.BillingCodeID = bc.CombOitemID " & _
 "INNER JOIN t_Prospect p on t_contract.ProspectID = p.ProspectID " & _
 "Inner join (select * from t_Prospectphone where number = '" & hphone(i) & "' and active = 1) pp on pp.prospectid = t_Contract.prospectid " & _
 "WHERE  (pp.Number = '" & hphone(i) & "') OR t_Contract.ContractID in (Select contractid from t_ContractCoOwner a inner join t_ProspectPhone b on a.prospectid = b.prospectid where b.number = '" & hphone(i) & "')) " & _
 "x left outer join t_ProspectAddress pa on x.AddressID = pa.AddressID order by x.ContractID desc" ,cn, 3, 3

if not rs.eof and not rs.bof then
%>
<TABLE align = right colspan = 4 >
	<tr>
		
		<td align = right colspan = 3>
			<b>
				<%=rs.fields("Name").value%>
				<% if rs.fields("SpouseName").value & "" <> "" then %>
					<br>
				<% end if %>
				<%=rs.fields("SpouseName").value%>
			</b>
		</td>
	</tr>
	
	<tr>
		<td align = right colspan = 3>
			<%=rs.fields("Address1").value%>
		</td>
	</tr>
	<tr>
	
		<td align = right colspan=3>
			<%=rs.fields("City").value%>,

		<%
		
				rs1.open "Select * from t_ComboItems where comboitemid = '" & rs.Fields("StateorProvince") & "'",cn,3,3
				if rs1.EOF and rs1.BOF then
				else
					response.write rs1.Fields("ComboItem")
				end if
				rs1.close

		%>
				&nbsp;<%=rs.fields("PostalCode").value%>
		</td>
	</tr>
	
	<tr colspan = 3>
		<td align = right colspan = 4>
		<%=Left(rs.fields("homephone").value,3)+ "-" + Mid(rs.fields("homephone").value,4,3)+ "-" + Right(rs.fields("homephone").value,4)%>
			
		</td>
	</tr>
	
	<%
		rs1.Open "Select b.FirstName, b.Lastname from t_ContractCoOwner a inner join t_prospect b on a.prospectid = b.prospectid where (a.ContractID in (Select x.contractid from t_Contract x inner join t_prospectphone y on x.prospectid = y.prospectid where y.number = '" & hphone(i) & "') or a.ContractID in (Select contractid from t_ContractCoOwner s inner join t_Prospectphone t on s.prospectid = t.prospectid where t.number = '" & hphone(i) & "')) and b.prospectid <> '" & rs.Fields("ProspectiD") & "'", cn, 3, 3
		If rs1.EOF and rs1.BOF then
		Else
			Do while not rs1.EOF
	%>
			<tr colspan = '3'>
			<td align = right colspan = 4><%=rs1.Fields("FirstName") & " " & rs1.Fields("LastName")%></td>
			</tr>
	<%
				rs1.MoveNext
			Loop
		End If
		rs1.Close
	%>
</TABLE>

<p><br>
&nbsp;</p>
<table width="198">
	<tr>
		<td bordercolordark="#000000" align="center" valign="top" style="border: 1px solid #000000" width="190">
			<b>
				Current Owner Activity
			</b>
		</td>
	</tr>
</table>

<%
end if
do while not rs.eof
	
%>

<b><span lang="en-us"><font size="7">
_______________________________</font></span></b>



<table>
	<tr>
		<td align = right><b>
			Contract #:
		</b>
		</td>
		
		
		
		<td align = left width="0">
			<%=rs.fields("ContractNumber").value%>
		</td>
		
		
		
		<td align = right>
			<b>Week/Unit: 
		</b> 
		</td>
		
		
		
		<td align = left>
			<%
						

				rs1.open "select name as unitname, week, t_Salesinventory.salesinventoryid from t_salesinventory " & _
                         "inner join t_unit on t_salesinventory.unitid = t_unit.unitid " & _
                         "inner join t_Soldinventory on t_Soldinventory.salesinventoryid = t_SalesInventory.SalesInventoryID " & _
                         "where contractid = '" & rs.fields("ContractID").value & "' order by unitname",cn,3,3
				do while not rs1.eof
					response.write rs1.fields("week").value
			%>
						/
			<%
					response.write rs1.fields("unitname").value
					
					rs1.movenext
					if rs1.eof then
					else
						response.write "<br>"
					end if
				loop
				rs1.close

			%>
			
		</td>
		
		<td>&nbsp;</td>
		
		<td align = right>
			<b>Monthly Payments:
		</b>
		</td>
		
		
		
		<td align = left>
<% 
	dim ans
	dim Rate
	dim mType
	
	mType = 0
	Rate = cdbl(rs.fields("apr").value/1200)
	PV = cdbl(rs.fields("TotalFinanced").value)
	Terms = cdbl(rs.fields("Terms").value)
	
	if rate <=0 or PV <= 0 or Terms < 1 then 
		ans = "0"
	else
		ans = ccur(round((Rate*(mType+PV*(1+Rate)^Terms))/((1-Rate*mType)*(1-(1+Rate)^Terms)),2)) * -1
	end if
	if len(cstr(ans))-instr(cstr(ans),".") = 1 then
		ans = cstr(ans) & "0"
	end if
	
	

	response.write formatcurrency(ans)
%>
		</td>
		
		
		
	<tr>
		<td align = right>
			<b>Date of Sale:
		</b>
		</td>
		
		
		
		<td width="0" align = left>
			<%=rs.fields("ContractDate").value%>
		</td>
		
		
		
		<td align = right>
			<b>Sales Price:
		</b>
		</td>
		
		
		
		<td align = left>
			<%=formatcurrency(rs.fields("SalesPrice").value)%>
		</td>
		
		<td>&nbsp;</td>
		
		<td align = right>
			<b>Term:
		</b>
		</td>
		
		
		
		<td align = left>
			<%=rs.fields("Terms").value%>
		</td>
		
	<tr>
		<td align = right>
			<b>Status:
		</b>
		</td>
		
		
		
		<td width="0" align = left>
			<%
				rs1.open "Select * from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboitemid = " & rs.fields("statusid").value & " and comboname = 'contractstatus' order by comboitem",cn,3,3
				
					if rs1.bof and rs1.eof then
						
					else
						response.write rs1.fields("comboitem").value
					end if
					
					
					rs1.close
			%>
		</td>
		
		
		
		<td align = right>
			<b>Frequency:
		</b>
		</td>
		
		
		
		<td align = left>
			<%
				If rs.fields("FrequencyID").value = 1 Then
					response.write "Annual"
				ElseIf rs.fields("FrequencyID").value = 2 Then
                    if rs.fields("OccupancyDate").value & "" = "" then
                    	response.write "Not Assigned"
                    else	
                        If year(rs.fields("OccupancyDate").value) Mod 2 = 0 Then
              				response.write "BiEnnial - Even"
              			Else
      		        		response.write "BiEnnial - Odd"
             			end if
					end if	
				Else
			    	response.write "TriEnnial"
			    		
			    end if
			    
			%>
		</td>
        <td>&nbsp</td>
        <td align = "right"><b>Billing Code:</b></td>
        <td><%=rs.Fields("BillingCode") %></td>
	</tr>
    <tr>
        		<td align = right>
			<b>Sub Status:
		</b>
		</td>
		
		
		
		<td width="0" align = left>
			<%
				rs1.open "Select * from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboitemid = " & rs.fields("substatusid").value & " and comboname = 'contractsubstatus' order by comboitem",cn,3,3
				
					if rs1.bof and rs1.eof then
						
					else
						response.write rs1.fields("comboitem").value
					end if
					
					
					rs1.close
			%>
		</td>
    </tr>
    <tr>
        <td align="right">
            <b>Usage(s):</b>
        </td>
        <td align="left">
            <%
                rs1.open "select u.UsageYear, ut.ComboItem as Type,ust.ComboItem as SubType, u.indate as [From], u.outdate as [To], s.ComboItem as Status from t_Usage u inner join t_ComboItems ust on ust.ComboItemID=u.SubTypeID inner join t_ComboItems ut on ut.ComboItemID=u.TypeID inner join t_Comboitems s on s.ComboItemID=u.StatusID where u.UsageYear >= YEAR(getdate())  and u.Contractid=" & rs.fields("ContractID").value, cn, 0, 1 
                if rs1.bof and rs1.eof then
                    response.write "N/A"
                else
                    response.Write "<table border=1><tr>"
                    for each fld in rs1.fields
                        response.write "<th>" & Server.HTMLEncode(fld.name) & "</th>"
                    next
                    response.write "</tr>"
                    do while not rs1.eof
                        response.write "<tr>"
                        for each fld in rs1.fields
                            response.write "<td>" & Server.HTMLEncode(fld.value & "") & "</td>"
                        next
                        response.write "</tr>"
                        rs1.movenext
                    loop
                    response.write "</table>"
                end if  
                rs1.close  
            %>
        </td>
    </tr>
</table>
<%

	rs.movenext
loop


%>
<p><b><span lang="en-us"><font size="4">
___________________________________________________________________________________</font></span></b><br>
		<br>
		
	
</p>
<form>
		<b><span lang="en-us">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		</span>I/We decline the Towns/Estates at King's Creek Plantation pre-development offer.
		<br>
		<br>
				<span lang="en-us">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		</span>I/We understand that the special pre-development pricing, Governor's Club and Golf membership are
				
		offered TODAY and TODAY ONLY.
		<br>
		<br>		<span lang="en-us">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		</span>I/We understand that the above information will be entered into a database and can become a<span lang="en-us">
		</span>permanent record.
		<br>
		<br>
		<br>
		REASON FOR DECLINING PRE-DEVELOPMENT OFFER:</b>
		<br>
		<br>
		<span lang="en-us">-----------------------------------------------------------------------------------------------------------------------------</span><br>
		<br>
		<span lang="en-us">-----------------------------------------------------------------------------------------------------------------------------</span><br>
		<br>
		<br>
		<br>
		________________________________________
		<br>
		<b>Signature</b>
		<br>
		<br>
		
		
		
		
		________________________________________
		<br>
		<b>Signature</b>																			
		<br>
		<br>
		<b><span lang="en-us">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
		</span>This offer was declined on <%response.write(FormatDateTime(date(),vblongdate))%></b>
	</form>

<%
	
	rs.close
	response.write "<P style = 'page-break-after: always'>&nbsp</P>"
	next
	
	cn.close
	set rs = nothing
	set cn = nothing
	set rs1 = nothing

%>




</body>

</html>