
<%
	Dim cn
	Dim rs
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	cn.Commandtimeout = 0
	server.scripttimeout = 10000


DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

	cn.Open DBNAME, dbuser, dbpass
	
	If request("function") = "Run_Report" then 
		response.write "<table>"
		response.write "<tr><th align = 'left'><u>Owner</u></th><th align = 'left'><u>City</u></th><th align = 'left'><u>State</u></th>"
		rs.Open "SELECT DISTINCT(PROSPECTID), LEFT(FirstName, 1) AS FirstName, LastName, City, State FROM v_Prospect WHERE (ProspectID IN (SELECT DISTINCT ProspectID FROM t_Contract WHERE Contractnumber NOT LIKE 'T%' AND contractnumber NOT LIKE 'U%' AND (StatusID IN (SELECT comboitemid FROM t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID WHERE comboname = 'ContractStatus' AND (comboitem = 'Active' OR comboitem = 'Suspense' OR comboitem = 'ReDeed' OR comboitem = 'Pender' OR comboitem = 'Pender-Inv'))))) ORDER BY State Asc", cn, 3, 3
		Do while not rs.EOF
			response.write "<tr>"
			response.write "<td>" & Trim(rs.Fields("FirstName")) & ". " & Trim(rs.Fields("LastName")) & "</td>"
			response.write "<td>" & rs.Fields("City") & "</td>"
			response.write "<td>" & rs.Fields("State") & "</td>"
			response.write "</tr>"
			rs.MoveNext
		Loop
		rs.Close
		response.write "</table>"
	end if
	
	cn.Close
	set rs = Nothing
	set cn = nothing

	
%>