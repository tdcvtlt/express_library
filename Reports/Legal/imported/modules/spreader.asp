
<%
dim rs 
dim cn
server.scripttimeout = 10000
set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")

    DBName = "CRMSNET"
    DBUser = "asp"
    DBPass = "aspnet"

'DBName = "CRMSData"
cn.open DBName, DBUser, DBPass
cn.CommandTimeout = 0

if request("f") = "report" then 
	rs.open    "SELECT t_Contract.ContractID, t_Contract.ContractNumber as KCP#,cs.comboitem as Status,cst.SaleType, P.LastName, P.FirstName, P.SpouseLastName + ', ' + P.SpouseFirstName as [Co-Owner], PA.Address1 as Address, PA.City, State.ComboItem as State, PA.PostalCode as Zip, t_Mortgage.SalesPrice as [Sales Price], t_Mortgage.DPTotal as [Total Down], t_Mortgage.SalesPrice - t_Mortgage.DPTotal as [Face Value], t_Mortgage.TotalFinanced As [Amount on Note], t_Unit.Name + '/' + cast(t_SalesInventory.Week as varchar) as UnitWeek, f.frequency as Frequency, t_Contract.ContractDate as [Contract Date],v_ContractUserFields.DeedOfTrustInstrumentNumber, v_ContractUserFields.DeedRecDate, v_ContractUserFields.ConveyanceType, v_ContractUserFields.ConveyanceRecDate, v_ContractUserFields.DOTRecDate  " & _
	 			"FROM   ((((((t_Contract t_Contract " & _ 
	 				"LEFT OUTER JOIN t_SoldInventory t_SoldInventory ON t_SoldInventory.ContractID = t_Contract.ContractID) " & _ 
	 					"LEFT OUTER JOIN t_SalesInventory t_SalesInventory ON t_SoldInventory.SalesInventoryID = t_SalesInventory.SalesInventoryID) " & _ 
	 						"INNER JOIN t_Mortgage t_Mortgage ON t_Contract.ContractID = t_Mortgage.ContractID) " & _ 
	 							"INNER JOIN t_Prospect P  ON t_Contract.ProspectID = p.ProspectID " & _ 
                                "LEFT OUTER JOIN T_PROSPECTADDRESS PA ON P.PROSPECTID = PA.PROSPECTID) " & _                               
	 						"INNER JOIN v_ContractUserFields v_ContractUserFields ON t_Contract.ContractID = v_ContractUserFields.ContractID) " & _ 
	 					"LEFT OUTER JOIN t_ComboItems State ON pa.StateID = State.ComboItemID) " & _ 
	 					"LEFT OUTER JOIN t_Frequency f on f.frequencyid = t_Contract.frequencyid " & _
	 				"LEFT OUTER JOIN t_Unit t_Unit ON t_SalesInventory.UnitID = t_Unit.UnitID " & _ 
	 				"LEFT OUTER JOIN t_Comboitems cs on cs.comboitemid = t_Contract.statusid " & _
	 				"LEFT OUTER JOIN v_ContractInventory cst on cst.contractid = t_Contract.contractid " & _
	 			"WHERE  v_ContractUserFields.ExhibitNumber = '" & request("exhibitnumber") & "' " & _
	   "ORDER BY t_Contract.ContractNumber " ,cn, 3, 3
	
	
	response.write "<TABLE align = center cellspacing = 2   border = 1>"
	response.write "<tr align = center cellspacing = 10>"
	for i = 1 to rs.fields.count -1
		response.write "<th>"
		response.write rs.fields(i).name 
		response.write "</th>"
	next
	
	response.write "</tr>"
	
	do while not rs.eof
		response.write "<tr>"
		for i = 1 to rs.fields.count -1
			response.write "<td align = center spacing = 10>"
			if rs.fields(i).name = "KCP#" then
				response.write "<a href = '../../editcontract.asp?contractid=" & rs.fields("ContractID").value & "'>" & rs.fields(i).value & "</a>"
			elseif rs.fields(i).name = "Sales Price" or rs.fields(i).name = "Total Down" or rs.fields(i).name = "Face Value" or rs.Fields(i).name = "Amount on Note" then
				response.write formatcurrency(rs.fields(i).value)
			else
				response.write rs.fields(i).value
			end if
			response.write "&nbsp;</td>"
		next
		response.write "</tr>"
		rs.movenext
	loop
	response.write  "</TABLE>"
	rs.close
	response.write "E#" & request("ExhibitNumber")
	
elseif request("f") = "load" then
	
		dim userfieldid
		rs.open "select UFID from t_UFIELDS where UFNAME = 'Exhibit Number'", cn, 0, 1
		if rs.eof and rs.bof then 
			userfieldid = 0
		else 
			userfieldid = rs.fields("UFID").value
		end if
		rs.close
		'rs.open "Select distinct v.UserFieldValue from t_UserFieldsValue v where v.userfieldid = " & userfieldid & " order by v.userfieldvalue",cn,3,3

        rs.open "Select distinct UV.UFVALUE from t_UF_VALUE UV where UV.UFID = " &  userfieldid & " ORDER BY UV.UFVALUE ", cn, 3, 3


		i = 0
		do while not rs.eof
			if i > 0 then response.write "||"
			response.write trim(rs.fields("UFVALUE").value) & "|" & trim(rs.fields("UFVALUE").value)
			i=1
			rs.movenext
		loop
		rs.close

end if
cn.close
set rs = nothing
set cn = nothing
%>