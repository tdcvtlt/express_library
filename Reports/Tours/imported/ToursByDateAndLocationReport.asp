<HTML><HEAD><title>Tours By Date and Location</title>

    <script language="javascript" src="../../../scripts/scw.js"></script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="tlb;">
</HEAD>


<BODY><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

<p align="center"><font size="6"><strong>
</strong></font><br>
</p>
<p align="center">&nbsp;</p>

</td></tr><!--msnavigation--></table><!--msnavigation--><table dir="ltr" border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td valign="top" width="1%">

</td><td valign="top" width="24"></td><!--msnavigation--><td valign="top"><form method="POST" action="ToursByDateAndLocationReport.asp" webbot-action="--WEBBOT-SELF--"><p><span lang="en-us" align = right><br><br>Please Select A Location:&nbsp;&nbsp; <select size="1" name="Location"><option value=0></option><%

		dim rs 
		dim cn
        
		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")


	DBName = "CRMSNet"
    DBUser = "asp"
    DBPass="aspnet"
        
		cn.open DBName, DBUser, DBPass

		dim rsL
		set rsL = server.createobject("ADODB.Recordset")

		rsL.open "Select * from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID  where comboname = 'tourlocation' order by comboitem", cn, 3, 3
		
        do while not rsL.eof
			if request("Location") = cstr(rsL.fields("Comboitemid").value) then
				response.write "<option selected value = '" & rsL.fields("ComboItemID").value & "'>" & rsL.fields("ComboItem").value & "</option>"
			else
				response.write "<option value = '" & rsL.fields("ComboItemID").value & "'>" & rsL.fields("ComboItem").value & "</option>"
			end if
			rsL.movenext
		loop
		rsL.close
		set rsL = nothing
%></select></span></p>&nbsp;
<table><tr><td>Start Date: </td><td><input type = 'text' id = 'input_startdate' name = 'startdate' Readonly value="<%=request("startdate")%>" size="20" onclick = "scwShow(this, this);"> </td></tr><tr><td>End Date:</td><td><input type = 'text' id = 'input_enddate' name = 'enddate' readonly value="<%=request("enddate")%>" size="20" onclick = "scwShow(this, this);" ></td></tr><tr><td><input type = 'submit' name = 'go' value = 'Run Report' onclick="true"></td></tr></table></form><%


dim sql


sql =   "select distinct TourDate,TourID,  s.ComboItem as Status, P.LastName, P.FirstName, " & _
        "t_Comboitems.comboitem as Location, Name, PH.NUMBER, Address1 as Address ,City, PostalCode   " & _
        "				from t_tour   " & _
        "					left outer join t_prospect  P" & _
        "						on t_tour.prospectid = P.prospectid  " & _
        "							left outer join t_comboitems  " & _
        "						on t_tour.tourlocationid = t_comboitems.comboitemid  " & _
        "						left outer join t_comboitems s  " & _
        "						on t_tour.statusid = s.comboitemid  " & _						
        "					left outer join t_campaign  " & _
        "				on t_tour.campaignid = t_campaign.campaignid  " & _
        "				INNER JOIN T_PROSPECTPHONE PH ON P.PROSPECTID = PH.PROSPECTID " & _
        "				INNER JOIN T_PROSPECTADDRESS PA ON P.PROSPECTID = PA.PROSPECTID " & _
        "			where PH.ACTIVE = 1 AND tourdate between   '" & request("StartDate") & "' and '" & request("EndDate") & "' and t_Comboitems.Comboitemid  = '" & request("Location") & "' " & _
        "		order by tourdate,s.comboitem, lastname asc " 


    rs.open sql, cn, 3, 3

dim lastTourDate
lastTourDate = ""
do while not rs.eof
	if lastTourDate <> rs.fields(0).value then
		response.write "</Table><h2><b>" & rs.fields(0).value & "</b><h2>"
		response.write "<TABLE style='border-collapse:collapse;' align = center cellspacing = 1  border = 1>"
		response.write "<tr align = center cellspacing = 10>"
		for i = 1 to rs.fields.count -1
			response.write "<th>" & rs.fields(i).name & "</th>"
		next
		response.write "</tr>"
		lastTourDate = rs.fields(0).value
	end if
	response.write "<tr>"
	for i = 1 to rs.fields.count -1
		response.write "<td align = center spacing = 10>" & rs.fields(i).value & "</td>"
	next
	response.write "</tr>"
	rs.movenext
loop
response.write  "</TABLE>"
rs.close
cn.close
set rs = nothing
set cn = nothing
response.write now

%> <table><tr></tr></table><script type="text/javascript" language="javascript"  src="scripts/calendar.js"></script>
	
<!--msnavigation--></td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

</td></tr><!--msnavigation--></table></body></html>