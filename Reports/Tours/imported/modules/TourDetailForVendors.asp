

<html>
		<head>
			<title>Tour Details For Vendors
			</title>
			<script language="javascript" src="../../../TestPages/scripts/scw.js"></script>
								<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="tlb;">
</head>
	<body><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

<p align="center"><font size="6"><strong>
</strong></font><br>
</p>
<p align="center">&nbsp;</p>

</td></tr><!--msnavigation--></table><!--msnavigation--><table dir="ltr" border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td valign="top" width="1%">

</td><td valign="top" width="24"></td><!--msnavigation--><td valign="top">
<%
DBName = "CRMSNet"
dbuser = "asp"
dbpass="aspnet"
dim rs 
		dim cn

		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")

		cn.open DBName, DBUser, DBPass

	if request("Location") = "0" then
		rs.OPen "Select comboitemid from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'TourLocation'", cn, 3, 3
		Do while not rs.EOF
			if tourLocs = "" then
				tourLocs = rs.Fields("ComboItemID")
			Else
				tourLocs = tourLocs & "','" & rs.Fields("ComboItemID") 
			End If
			rs.MoveNext
		Loop
		rs.Close
	Else
		tourLocs = request("Location")
	End If

dim sSQL
sSQL = "select TourDate, TourID, p.LastName + ',' + p.FirstName as 'Name', c.Name as 'Campaign', tt.comboitem as 'TourType', status.comboitem as 'TourStatus', tl.comboitem as 'TourLocation', tw.ComboItem as Wave,  ts.comboitem as 'TourSubType' " & _
"from t_Tour t " & _ 
"left outer join t_prospect p on p.ProspectID = t.ProspectID " & _
"left outer join t_ComboItems tl on tl.ComboItemID = t.TourLocationID " & _
"left outer join t_Campaign c on c.CampaignID = t.CampaignID " & _
"left outer join t_ComboItems tt on tt.ComboItemID = t.TypeID " & _
"left outer join t_ComboItems ts on ts.ComboItemID = t.SubTypeID " & _
"left outer join t_ComboItems status on status.ComboItemID = t.StatusID " & _
"left outer join t_ComboItems tw on t.TourTime = tw.ComboItemID " & _
"where status.comboitem in ('Be Back', 'No Tour', 'NQ - No Tour', 'NQ - Toured', 'OnTour', 'Showed', 'No Tour - Overage','Showed - Overage') and status.comboitem <> 'Duplicated' and (ts.comboitem is null or ts.comboitem not like 'Exit%') and tl.Comboitemid  in ('" & tourLocs & "') and tourdate >= '" & request("StartDate") & "' and tourdate < '" & cdate(request("EndDate")) + 1 & "' " & _
"order by c.name, t.tourdate, p.lastname + ',' + p.firstname "

rs.open sSQL,cn, 3, 3

%>
<TABLE cellspacing = 2  >

<tr align = center cellspacing = 10>
<th  valign="bottom"><u>TourDate</u></th>
<th  valign="bottom"><u>TourID</u></th>
<th valign="bottom"><u>&nbsp;Name</u></th>
<th  valign="bottom"><u>Campaign </u></th>
<th  valign="bottom">
<u>Tour Type</u></th><th valign="bottom"><u>Tour Status</u></th>
<th  valign="bottom"><u>Location</u></th>
<th  valign="bottom">
<u>Wave</u></th><th valign="bottom"><u>Sales Team</u></th>
<th valign="bottom"><u>Tour Sub Type</u></th>
</tr>
<%

dim lastCampaignname
dim lastDate
dim DateTotal
dim CampaignTotal
dim SubTotal


lastCampaignname = ""
lastDate = ""
DateTotal = 0
CampaignTotal = 0
GrandTotal = 0
do while not rs.eof
	if lastDate <> rs.fields("TourDate").value  then
		'Write your total for the campaign if not on first campaign
		if lastDate <> "" then 
%>
<tr><td colspan = 2>&nbsp;</td>
	<td colspan = 8><b>Total for this Date = <%=DateTotal%></b></td></tr>
<%		end if
		'lastDate = rs.fields("TourDate").value
		DateTotal = 0
	end if
	if lastCampaignname <> trim(rs.fields("Campaign").value & "" )  then
		'Write your total for the campaign if not on first campaign
		if lastCampaignname <> "" then 
%>
<tr><td colspan = 1 >&nbsp;</td>
	<td colspan = 9><b>Total for this Campaign (<%=lastCampaignname%>) = <%=CampaignTotal%></b></td></tr>

<%
		end if
		lastCampaignname = trim(rs.fields("Campaign").value & "" )
		CampaignTotal = 0
%>

<tr align = center>
	<td align = center><b><%=rs.fields("Campaign").value%></b></td>
	<td >&nbsp;</td><td >&nbsp;</td><td>&nbsp;</td>
	<td >&nbsp;</td><td>&nbsp;</td><td >&nbsp;</td>
	<td >&nbsp;</td><td >&nbsp;</td><td>&nbsp;</td>
</tr>
<%
	end if
	if lastDate <> rs.fields("TourDate").value  then
		lastDate = rs.fields("TourDate").value
		DateTotal = 0
%>
<tr>
	<td colspan="2" align="left"><b></b><%=rs.fields("TourDate").value%></b></td>
	<td>&nbsp;</td><td >&nbsp;</td><td >&nbsp;</td>
	<td >&nbsp;</td><td >&nbsp;</td><td>&nbsp;</td>
	<td >&nbsp;</td><td >&nbsp;</td>
</tr>
<%
	end if

%>


<tr>
	<td></td><td><%=rs.fields("TourID").value%></td>
	<td align = center  ><%=rs.fields("Name").value%></td>
	<td align = center ><%=rs.fields("Campaign").value%></td>
	<td align = center  ><%=rs.fields("TourType").value%></td>
	<td align = center ><%=rs.fields("TourStatus").value%></td>
	<td align = center  ><%=rs.fields("TourLocation").value%></td>
	<td align = center><%=rs.fields("Wave").value%></td>
	<td align = center  ><%=""%></td>
	<td align = center  ><%=rs.fields("TourSubType").value%></td>
</tr>

<%
	DateTotal = DateTotal + 1
	CampaignTotal = CampaignTotal + 1
	GrandTotal = GrandTotal + 1
	rs.movenext	
loop
%>
<tr><td colspan = 2>&nbsp;</td>
	<td colspan = 8><b>Total for this Date = <%=DateTotal%></b></td></tr>
<tr><td colspan = 1 >&nbsp;</td>
	<td colspan = 9><b>Total for this Campaign (<%=lastCampaignname%>) = <%=CampaignTotal%></b></td></tr>
<tr><td colspan =10>
	<p align="center"></td></tr>

</TABLE>
&nbsp;

<table border="1" id="table1" align = left>
			<tr>
				<td align="right"><b><span lang="en-us">Sub Total:</span></b></td>
				<td align = right><b><%=GrandTotal%></b></td>
			</tr>


<%
dim MArketing
dim NQ
dim GrandTotal
dim InHouse
dim NQT
dim iExit

Marketing = 0
NQ = 0
SubTotal = 0
InHouse = 0
NQT = 0
iExit = 0
NoTour = 0
rs.close
rs.open sSQL, cn, 3, 3
do while not rs.eof
	if instr(rs.fields("TourType").value,"Mktg") > 0 then 
		Marketing = Marketing + 1
			
	else 
		if instr(rs.fields("TourStatus").value, "NQ - No Tour") >0 and not rs.fields("TourType").value = "Mktg In-House" and not rs.fields("TourType").value = "In-House" then
			NQ = NQ + 1
			
		else 
			if instr(trim(rs.fields("TourStatus").value) , "NQ - Toured") >0 and not rs.fields("TourType").value = "Mktg In-House" and not rs.fields("TourTYpe").value  = "In-House" then
				NQT = NQT + 1
			elseif instr(trim(rs.Fields("TourStatus").value), "No Tour") > 0 and not rs.Fields("TourType").value = "Mktg In-House" and not rs.Fields("TourType").value = "In-House" then 
				NoTour = NoTour + 1
			else
				if instr(rs.fields("TourType").value,"In-House") > 0 then
					InHouse	= InHouse + 1
			
				else
					'if instr( rs.fields("SalesTeam").value & "",  "Exit Conversions")>0 then
						iExit = iExit + 1
			
					'end if
				end if
			end if
		end if
	end if				
SubTotal = SubTotal + 1
GrandTotal = SubTotal - NQ - Marketing - InHouse - NQT - iExit - NoTour		

		
%>
<%

rs.movenext
loop
	
%>
			<tr>
				<td align="right"><b><span lang="en-us">Marketing:</span></b></td>
				<td align = right ><b>- <%=MArketing%></b></td>
			</tr>
			<tr>
				<td height="24"  align="right"><span lang="en-us"><b>NQ-No Tour:</b>:</span></td>
				<td height="24" align = right><b>- <%=NQ%></b></td>
			</tr>	
			<tr>
				<td align="right"><b><span lang="en-us">NQ-Toured:</span></b></td>
				<td align = right ><b>- <%=NQT%></b></td>
			</tr>
			<tr>
				<td align="right"><b><span lang="en-us">No Tour:</span></b></td>
				<td align = right><b>- <%=NoTour%></b></td>
			</tr>
			<tr>
				<td align="right"><b><span lang="en-us">In House:</span></b></td>
				<td align = right><b>- <%=InHouse%></b></td>
			</tr>
			<tr>
				<td align="right"><b><span lang="en-us">Exit Conversion:</span></b></td>
				<td align = right><b>- <%=iExit%></b></td>
			</tr>
		&nbsp;
			<tr>
				<td  align="right"><b><span lang="en-us">Grand Total:</span></b></td>
				<td align = right><b><font size="5"></b><%=GrandTotal%></font></b></td>
			</tr>
		</table>
		
		
		

<%	
rs.close
cn.close
set rs = nothing
set cn = nothing
	
	
%>
	
	<TABLE align = left>
		<tr>
			<td>
				<u>
					Date:<%response.write date%>
				</u>
			</td>
		</tr>
		<tr>
			<td>
				<u>
					Time:<%response.write time%>
				</u>
			</td>
		</tr>
	</TABLE>
		
		
		<!--msnavigation--></td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

</td></tr><!--msnavigation--></table></body>
</html>