<!--#include file="../../../includes/dbconnections.inc" --><!--#include file="../../../modules/security.asp" --><%
	dim cn
	dim rs
	
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	
	cn.open DBName, DBUser, DBPass
	
	rs.open "Select * from t_Units u inner join t_SalesInventory i on i.unitid = u.unitid left outer join t_Soldinventory s on s.salesinventoryid = i.salesinventoryid left outer join t_Frequency f on f.frequencyid = s.frequencyid order by u.unitname, i.week", cn, 3, 3
	
	response.write "<table><tr><th>Unit</th><th>Week</th><th>Sold</th><th>frequency</th></tr>"
	do while not rs.eof
		response.write "<tr><td>"
		response.write rs.fields("UnitName").value 
		response.write "</td><td>" 
		response.write rs.fields("Week").value
		response.write "</td><td>"
		response.write rs.fields("soldinventoryid").value & ""
		response.write "</td><td>"
		response.write rs.fields("Frequency").value 
		response.write "</td><td></tr>"
		rs.movenext
	loop
	response.write "</table>"
	
	rs.close
	cn.close
	
	set rs = nothing
	set cn = nothing

%>