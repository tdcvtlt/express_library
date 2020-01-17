

<%
Response.Buffer=true
on error goto 0
server.scripttimeout = 10000

dim Title
dim sDate
dim eDate
dim sAns 
dim cn
dim rs
dim sSQL
dim oTours
dim oContracts
dim oContractsVol
dim oPenders
dim oPendersVol
dim aPersonnelID()
dim aPersonnelLName()
dim aPersonnelFname()
dim sActiveContractStatuses
dim sPendingContractStatuses
dim sExitContracts
dim sExitTours
dim sTourStatuses
dim sOWs
dim sTitle
dim TourTotal
dim ActiveContractTotal
dim PenderContractTotal
dim ActiveContractVolTotal
dim PenderContractVolTotal
dim i

Title = request("Title")
sDate = request("SDate")
eDate = request("EDate")
if sDate <> "" then
  sDate = cDate(sDate)
end if
if eDate <> "" then
  eDate = cDate(eDate) + 1
end if

sActiveContractStatuses = "(Select Comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where ComboName = 'ContractStatus' and comboitem in ('Active','Suspense'))"
sPendingContractStatuses = "(Select Comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where ComboName = 'ContractStatus' and comboitem in ('Pender','Developer','Pender-Inv'))"
sTitle = "(select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboitem = '" & Title & "' and comboname = 'PersonnelTitle')"
sTourStatuses = "(select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'TourStatus' and comboitem in ('Showed', 'OnTour', 'Be Back'))"
sOWs = "(select KEYVALUE from t_Event where KEYVALUE > 0 AND KEYFIELD = 'CONTRACTID' and (oldvalue = 'pender' or oldvalue = 'Pender-Inv'))"

'Allow exits if title is exit otherwise eliminate them from totals
if instr(sTitle,"Exit") > 0 then
  sExitContracts = "(0)"
  sExitTours = "(0)"
else
  sExitContracts = "(Select Contractid from t_Contract where contractnumber like 't%' and contractnumber not like 'T%')"
  sExitTours = "(Select TourID from t_Tour where subtypeid in (select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'toursubtype' and comboitem = 'exit') and tourdate >= '" & sDate & "' and tourdate < '" & eDate & "')"
end if

redim aPersonnelID(1)
redim aPersonnelLName(1)
redim aPersonnelFName(1)

aPersonnelID(0) = 0
aPersonnelID(1) = 0
aPersonnelLName(0) = 0
aPersonnelLName(1) = 0
aPersonnelFName(0) = 0
aPersonnelFName(1) = 0


if Title = "" then
  sAns = "<TABLE STYLE='BORDER-COLLAPSE:COLLAPSE;' BORDER='1PX'><TR><TH>No Title Specified</TH></TR></TABLE>"
end if

if sAns = "" then
  set cn = server.createobject("ADODB.Connection")
  set rs = server.createobject("ADODB.Recordset")
  set oTours = server.createobject("Scripting.Dictionary")
  set oContracts = server.createobject("Scripting.Dictionary")
  set oPenders = server.createobject("Scripting.Dictionary")
  set oContractsVol = server.createobject("Scripting.Dictionary")
  set oPendersVol = server.createobject("Scripting.Dictionary")


DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"


  cn.open DBName, DBUser, DBPass
  cn.commandtimeout = 0
  
  'Select Personnel and place into arrays
  sSQL = "Select Distinct p.personnelid, p.lastname, p.firstname from t_Personnel p inner join t_PersonnelTrans pt on pt.PERSONNELID = p.personnelid " & _
          "where (pt.datecreated > '" & sDate & "' and pt.datecreated < '" & eDate & "' " & _
          " and pt.TITLEID in " & sTitle & ") or (pt.TITLEID in " & sTitle & " and pt.KEYVALUE in (" & _
          "select contractid from t_Contract c where c.statusdate between '" & sDate & "' and '" & eDate & "' and c.statusid in " & sActiveContractStatuses & _
          " and c.contractid in (select KEYVALUE from t_Event where KEYVALUE > 0 AND KEYFIELD = 'CONTRACTID' AND  (oldvalue = 'pender' or oldvalue = 'Pender-inv')))) order by p.lastname asc"
          
	
  rs.open sSQL, cn, 3, 3
  i=0
  do while not rs.eof
    if i > UBound(aPersonnelID) then
      redim preserve aPersonnelID(i)
      redim preserve aPersonnelLName(i)
      redim preserve aPersonnelFName(i)
    end if
    aPersonnelID(i) = rs.fields("PersonnelID").value
    aPersonnelLName(i) = trim(rs.fields("LastName").value & "")
    aPersonnelFName(i) = trim(rs.fields("FirstName").value & "")
    i=i+1
    rs.movenext
  loop
  rs.close
    
    
  'Select Tour Counts
  sSQL = "Select p.personnelID, Count(Distinct pt.KEYVALUE) as Tours from t_Personnel p inner join t_PersonnelTrans pt on pt.personnelid = p.personnelid   " & _
          "inner join t_Tour t on t.tourid = pt.KEYVALUE  " & _
          "where pt.keyfield = 'tourid' and  pt.datecreated > '" & sDate & "' and pt.datecreated < '" & eDate & "' and  pt.KEYVALUE > 0 " & _
          "and pt.TITLEID in " & sTitle & " and pt.KEYVALUE not in " & sExitTours & " and t.statusid in " & sTourStatuses & " group by p.personnelid"
  
  rs.open sSQL, cn, 3, 3
  do while not rs.eof
    oTours(rs.fields("PersonnelID").value) = rs.fields("Tours").value
    rs.movenext
  loop
  rs.close
  
  'Select Active and OW Contract Counts
  if cstr(request("OWs")) = "true" then
  	sSQL = "Select p.personnelID, Count(Distinct pt.KEYVALUE) as Contracts from t_Personnel p inner join t_PersonnelTrans pt on pt.PERSONNELID = p.personnelid  " & _
          "inner join t_Contract c on c.contractid = pt.KEYVALUE  " & _
          "where ((c.contractdate < '" & sDate & "' and c.statusdate between '" & sDate & "' and '" & eDate & "' and c.contractid in (select KEYVALUE from t_Event where KEYVALUE > 0 AND KEYFIELD = 'CONTRACTID' AND (oldvalue = 'pender' or oldvalue = 'Pender-Inv'))) or (c.contractdate between '" & sDate & "' and  '" & eDate & "')) and  pt.KEYVALUE > 0 " & _
          "and pt.titleid in " & sTitle & " and pt.keyfield = 'contractid'  and c.statusid in " & sActiveContractStatuses & " and c.contractid not in " & sExitContracts & " group by p.personnelid"
  else
  	sSQL = "Select p.personnelID, Count(Distinct pt.KEYVALUE) as Contracts from t_Personnel p inner join t_PersonnelTrans pt on pt.PERSONNELID = p.personnelid  " & _
          "inner join t_Contract c on c.contractid = pt.KEYVALUE  " & _
          "where (c.contractdate between '" & sDate & "' and '" & eDate & "') and  pt.KEYVALUE > 0 " & _
          "and pt.titleid in " & sTitle & " and pt.keyfield = 'contractid' and c.statusid in " & sActiveContractStatuses & " and c.contractid not in " & sExitContracts & " and c.contractid not in " & sOWs & " group by p.personnelid"
  	'sSQL = "Select p.personnelID, Count(Distinct pt.ContractID) as Contracts from t_Personnel p inner join t_PersonnelTrans pt on pt.personnelid = p.personnelid " & _
    '      "inner join t_Contract c on c.contractid = pt.contractid " & _
    '      "where ((c.statusdate > '" & sDate & "' and c.statusdate < '" & eDate & "' and c.contractid not in (select contractid from t_Event where contractid > 0 and (oldvalue = 'pender' or oldvalue = 'Pender-Inv'))) or (pt.datecreated > '" & sDate & "' and pt.datecreated < '" & eDate & "' and c.statusdate < '" & eDate & "')) and  pt.ContractID > 0 " & _
    '      "and pt.titleid in " & sTitle & " and c.statusid in " & sActiveContractStatuses & " and c.contractid not in " & sExitContracts & " and c.contractid not in " & sOWs & " group by p.personnelid"
  end if

  rs.open sSQL, cn, 3, 3
  do while not rs.eof
    oContracts(rs.fields("PersonnelID").value) = rs.fields("Contracts").value
    rs.movenext
  loop
  rs.close
  
  'Select Pender Contract Counts
  sSQL = "Select p.personnelID, Count(Distinct pt.KEYVALUE) as Penders from t_Personnel p inner join t_PersonnelTrans pt on pt.PERSONNELID = p.personnelid   " & _
          "inner join t_Contract c on c.contractid = pt.KEYVALUE " & _
          "where c.contractdate between '" & sDate & "' and '" & eDate & "' and  pt.KEYVALUE > 0 " & _
          "and pt.titleid in " & sTitle & " and pt.keyfield = 'contractid'  and c.statusid in " & sPendingContractStatuses & " and c.contractid not in " & sExitContracts & " group by p.personnelid"

  rs.open sSQL, cn, 3, 3
  do while not rs.eof
    oPenders(rs.fields("PersonnelID").value) = rs.fields("Penders").value
    rs.movenext
  loop
  rs.close
  
  'Select Active and OW Contract Volume
  if cstr(request("OWs")) = "true" then
  	sSQL = "Select p.personnelID, (Select Sum(salesvolume) from t_Mortgage where mortgageid in (select distinct mortgageid from t_contract c inner join t_Personneltrans t on t.KEYVALUE = c.contractid AND T.KEYFIELD = 'CONTRACTID' inner join t_Mortgage m on m.contractid = c.contractid where (c.contractdate between '" & sDate & "' and '" & eDate & "' or (c.statusdate between '" & sDate & "' and '" & eDate & "' and c.contractdate < '" & sDate & "' and c.contractid in (select KEYVALUE from t_Event E where E.KEYVALUE > 0 and (oldvalue = 'pender' or oldvalue = 'Pender-Inv')))) and t.titleid in " & sTitle & " and c.statusid in " & sActiveContractStatuses & " and t.personnelid = p.personnelid and c.contractid not in " & sExitContracts & ")) as Volume from t_Personnel p inner join t_PersonnelTrans pt on pt.personnelid = p.personnelid " & _
          "inner join t_Contract c on c.contractid = pt.KEYVALUE  inner join t_Mortgage m on m.contractid = c.contractid " & _
          "where ((pt.datecreated >= '" & sDate & "' and pt.datecreated < '" & eDate & "' and c.statusdate < '" & eDate & "') or (c.contractdate < '" & sDate & "' and c.statusdate between '" & sDate & "' and '" & eDate  + 1 & "' and c.contractid in (select KEYVALUE from t_Event E where E.KEYVALUE > 0 AND E.KEYFIELD = 'CONTRACTID' and (oldvalue = 'pender' or oldvalue = 'Pender-Inv')))) and  pt.KEYVALUE > 0 " & _
          "and pt.titleid in " & sTitle & " and c.statusid in " & sActiveContractStatuses & " and c.contractid not in " & sExitContracts & " group by p.personnelid"
  else
  	sSQL = "Select p.personnelID, (Select Sum(salesvolume) from t_Mortgage where mortgageid in (select distinct mortgageid from t_contract c inner join t_Personneltrans t on t.KEYVALUE = c.contractid AND T.KEYFIELD = 'CONTRACTID' inner join t_Mortgage m on m.contractid = c.contractid where (c.contractdate between '" & sDate & "' and '" & eDate & "' ) and t.titleid in " & sTitle & " and c.statusid in " & sActiveContractStatuses & " and t.personnelid = p.personnelid and c.contractid not in " & sExitContracts & ")) as Volume from t_Personnel p inner join t_PersonnelTrans pt on pt.personnelid = p.personnelid " & _
          "inner join t_Contract c on c.contractid = pt.KEYVALUE  inner join t_Mortgage m on m.contractid = c.contractid " & _
          "where ((pt.datecreated >= '" & sDate & "' and pt.datecreated < '" & eDate & "' and c.statusdate < '" & eDate & "') or (c.contractdate < '" & sDate & "' and c.statusdate >= '" & sDate & "' and c.statusdate < '" & eDate & "' and c.contractid in (select KEYVALUE from t_Event E where E.KEYVALUE > 0 AND E.KEYFIELD = 'CONTRACTID' and (oldvalue = 'pender' or oldvalue = 'Pender-Inv')))) and  pt.KEYVALUE > 0 " & _
          "and pt.titleid in " & sTitle & " and c.statusid in " & sActiveContractStatuses & " and c.contractid not in " & sExitContracts & " and c.contractid not in " & sOWs & " group by p.personnelid"
          
	'sSQL = "Select p.personnelID, (Select Sum(salesvolume) from t_Mortgage where mortgageid in (select distinct mortgageid from t_contract c inner join t_Personneltrans t on t.contractid = c.contractid inner join t_Mortgage m on m.contractid = c.contractid where (c.contractdate between '" & sDate & "' and '" & eDate & "' or (c.statusdate between '" & sDate & "' and '" & eDate & "' and c.contractdate < '" & sDate & "' and c.contractid in (select contractid from t_Event where contractid > 0 and (oldvalue = 'pender' or oldvalue = 'Pender-Inv')))) and t.titleid in " & sTitle & " and c.statusid in " & sActiveContractStatuses & " and t.personnelid = p.personnelid and c.contractid not in " & sExitContracts & " and c.contractid not in " & sOWs & ")) as Volume from t_Personnel p inner join t_PersonnelTrans pt on pt.personnelid = p.personnelid " & _
    '      "inner join t_Contract c on c.contractid = pt.contractid  inner join t_Mortgage m on m.contractid = c.contractid " & _
    '      "where ((pt.datecreated >= '" & sDate & "' and pt.datecreated < '" & eDate & "' and c.statusdate < '" & eDate & "') or (c.contractdate < '" & sDate & "' and c.statusdate >= '" & sDate & "' and c.statusdate < '" & eDate & "' and c.contractid in (select contractid from t_Event where contractid > 0 and (oldvalue = 'pender' or oldvalue = 'Pender-Inv')))) and  pt.ContractID > 0 " & _
    '      "and pt.titleid in " & sTitle & " and c.statusid in " & sActiveContractStatuses & " and c.contractid not in " & sExitContracts & " and c.contractid not in " & sOWs & " group by p.personnelid"
  end if

  'Due to query result difference when selecting and versuses selecting individually modified to loop through the personnelids and query for each
  'rs.open sSQL, cn, 3, 3
  'do while not rs.eof
  '  oContractsVol(rs.fields("PersonnelID").value) = rs.fields("Volume").value
  '  rs.movenext
  'loop
  'rs.close
  
  for i = 0 to ubound(aPersonnelID)
  	rs.open "select * from (" & sSQL & ") a where a.personnelid = " & aPersonnelID(i), cn, 0, 1
  	if rs.eof and rs.bof then
  	else
	  	if rs.fields("Volume").value & "" = "" then 
	  		oContractsVol(aPersonnelID(i)) = 0
	  	else
	  		oContractsVol(aPersonnelID(i)) = rs.fields("Volume").value
	  	end if
	end if
  	rs.close
  next
  
  
  'Select Pender Contract Volume
  sSQL = "Select p.personnelID, (Select Sum(salesvolume) from t_Mortgage where mortgageid in (select distinct mortgageid from t_contract c inner join t_Personneltrans t on t.KEYVALUE = c.contractid AND T.KEYFIELD = 'CONTRACTID' inner join t_Mortgage m on m.contractid = c.contractid where c.statusdate between '" & sDate & "' and '" & eDate & "' and t.titleid in " & sTitle & " and c.statusid in " & sPendingContractStatuses & " and t.personnelid = p.personnelid and c.contractid not in " & sExitContracts & ")) as Volume from t_Personnel p inner join t_PersonnelTrans pt on pt.personnelid = p.personnelid " & _
          "inner join t_Contract c on c.contractid = pt.KEYVALUE and pt.keyfield = 'contractid' inner join t_Mortgage m on m.contractid = c.contractid " & _
          "where c.contractdate between  '" & sDate & "' and '" & eDate & "' and  pt.KEYVALUE > 0 " & _
          "and pt.titleid in " & sTitle & " and c.statusid in " & sPendingContractStatuses & " and c.contractid not in " & sExitContracts & " group by p.personnelid"

  rs.open sSQL, cn, 3, 3
  do while not rs.eof
    oPendersVol(rs.fields("PersonnelID").value) = rs.fields("Volume").value
    rs.movenext
  loop
  rs.close



  cn.close
  set rs = nothing
  set cn = nothing
  
  TourTotal = 0
  ActiveContractTotal = 0
  PenderContractTotal = 0
  ActiveContractVolTotal = 0
  PenderContractVolTotal = 0

  sAns = "<TABLE STYLE = 'BORDER-COLLAPSE:COLLAPSE;' border = '1PX'>"
  if eDate <> "" and sDate <> "" then
    sAns = sAns & "<TR><TH colspan = 6>" & Title & "'s from " & sDate & " to " & eDate -1
    if cstr(request("OWs")) = "false" then
    	sAns = sAns & "<br>Excluding OWs "
    end if
    sAns = sAns & "</TH></TR>"
  elseif eDate <> "" and sDate = "" then
    sAns = sAns & "<TR><TH colspan = 6>" & Title & "'s for " & eDate -1 
    if cstr(request("OWs")) = "false" then
    	sAns = sAns & "<br>Excluding OWs "
    end if
    sAns = sAns & "</TH></TR>"
  elseif sDate <> "" and eDate = "" then
    sAns = sAns & "<TR><TH colspan = 6>" & Title & "'s for " & sDate 
    if cstr(request("OWs")) = "false" then
    	sAns = sAns & "<br>Excluding OWs "
    end if
    sAns = sAns & "</TH></TR>"
  else
    sAns = sAns & "<TR><TH colspan = 6>" & Title & "'s" 
    if cstr(request("OWs")) = "false" then
    	sAns = sAns & "<br>Excluding OWs "
    end if
    sAns = sAns & "</TH></TR>"
  end if
  sAns = sAns & "<TR>"
  sAns = sAns & "<TH>Personnel</TH>"
  sAns = sAns & "<TH>Tours</TH>"
  sAns = sAns & "<TH>Active Contracts</TH>"
  sAns = sAns & "<TH>Active Contract Volume</TH>"
  sAns = sAns & "<TH>VPG</TH>"
  sAns = sAns & "<TH>Pending Contracts</TH>"
  sAns = sAns & "<TH>Pending Contract Volume</TH>"
  sAns = sAns & "</TR>"
  for i = 0 to ubound(aPersonnelID)
	if oTours(aPersonnelID(i)) & "" <> "" or oContracts(aPersonnelID(i)) & "" <> "" or oPenders(aPersonnelID(i)) & "" <> "" then
	    sAns = sAns & "<TR>"
	    'sAns = sAns & "<TD>" & aPersonnelID(i) & "</TD>"
	    sAns = sAns & "<TD>" & aPersonnelLName(i) & ", " & aPersonnelFName(i) & "</TD>"
	    if oTours(aPersonnelID(i)) = "" then
	      sAns = sAns & "<TD align = right>0</TD>"
	    else
	      sAns = sAns & "<TD align = right>" & oTours(aPersonnelID(i)) & "</TD>"
	      TourTotal = TourTotal + oTours(aPersonnelID(i))
	    end if
	    if oContracts(aPersonnelID(i)) = "" then
	      sAns = sAns & "<TD align = right>0</TD>"
	    else
	      sAns = sAns & "<TD align = right>" & oContracts(aPersonnelID(i)) & "</TD>"
	      ActiveContractTotal = ActiveContractTotal + oContracts(aPersonnelID(i))
	    end if
	    if oContractsVol(aPersonnelID(i)) = "" or isnull(oContractsVol(aPersonnelID(i))) then
	      sAns = sAns & "<TD align = right>$0.00</TD>"
	    else
	      sAns = sAns & "<TD align = right>" & formatcurrency(oContractsVol(aPersonnelID(i))) & "</TD>"
	      ActiveContractVolTotal = ActiveContractVolTotal + oContractsVol(aPersonnelID(i))
	    end if
	    if oTours(aPersonnelID(i)) & "" <> ""  and oContractsVol(aPersonnelID(i)) & "" <> "" then
	    	sAns = sAns & "<TD align=right>" & formatcurrency(oContractsVol(aPersonnelID(i))/oTours(aPersonnelID(i))) & "</TD>"
	    else
	    	sAns = sAns & "<TD align=right>" & formatcurrency(0) & "</TD>"
	    end if
	    if oPenders(aPersonnelID(i)) = "" then
	      sAns = sAns & "<TD align = right>0</TD>"
	    else
	      sAns = sAns & "<TD align = right>" & oPenders(aPersonnelID(i)) & "</TD>"
	      PenderContractTotal = PenderContractTotal + oPenders(aPersonnelID(i))
	    end if
	    if trim(oPendersVol(aPersonnelID(i)) & "") = "" then
	      sAns = sAns & "<TD align = right>$0.00</TD>"
	    else
	      sAns = sAns & "<TD align = right>" & formatcurrency(oPendersVol(aPersonnelID(i))) & "</TD>"
	      PenderContractVolTotal = PenderContractVolTotal + oPendersVol(aPersonnelID(i))
	    end if
	    sAns = sAns & "</TR>"
	end if
  next
  sAns = sAns & "<TR>"
  sAns = sAns & "<TD><B>Totals</B></TD>"
  sAns = sAns & "<TD align = right><B>" & TourTotal& "</B></TD>"
  sAns = sAns & "<TD align = right><B>" & ActiveContractTotal & "</B></TD>"
  if ActiveContractVolTotal= "" then
  	ActiveContractVolTotal=0
  end if
  sAns = sAns & "<TD align = right><B>" & formatcurrency(ActiveContractVolTotal) & "</B></TD>"	 
  if TourTotal <> 0 then 
  	sAns = sANs & "<TD align = right><B>" & formatcurrency(ActiveContractVolTotal/TourTotal) & "</B></TD>"
  else
  	sAns = sANs & "<TD align = right><B>" & formatcurrency(0) & "</B></TD>"
  end if
  sAns = sAns & "<TD align = right><B>" & PenderContractTotal & "</B></TD>"
  if trim(PenderContractVolTotal & "") = "" then 
  	PenderContractVolTotal=0
  end if
  sAns = sAns & "<TD align = right><B>" & formatcurrency(PenderContractVolTotal) & "</B></TD>"
  sAns = sAns & "</TR>"
  sAns = sAns & "</TABLE>"
  'sAns= sAns & request("OWs")
end if

response.write sAns
%>

