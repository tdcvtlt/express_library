
<%
dim cn, rs
set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")
set rs2 = server.createobject("ADODB.Recordset")


DBName = "CRMSNET"
DBUser = "asp"
DBPass = "aspnet"

cn.open DBName, DBUser, DBPass

rs.open "select c.statusdate,i.* from v_ContractInventory i inner join t_Contract c on c.contractid = i.contractid where i.contractnumber in (select contractnumber from t_Contract where prospectid = 8521183) order by i.SaleType, i.contractnumber", cn, 0, 1
response.write "<table>"
response.write "<tr>"
response.write "<th>KCP</th>"
response.write "<th>Redeed Date</th>"
'response.write "<th>Name</th>"
response.write "<th>Unit</th>"
response.write "<th>Week</th>"
response.write "<th>Frequency</th>"
response.write "<th>Occ. Year</th>"
response.write "<th>Type</th>"
response.write "<th>Size</th>"
response.write "</tr>"
do while not rs.eof
	rs2.open "Select distinct p.lastname + ', ' + p.firstname as Name, si.Week, u.name, s.occupancyyear from t_Contract c inner join t_Prospect p on p.prospectid = c.prospectid inner join t_Soldinventory s on s.contractid = c.contractid inner join t_Salesinventory si on si.salesinventoryid = s.salesinventoryid inner join t_Unit u on u.unitid = si.unitid where c.contractid = " & rs.fields("ContractID"), cn, 0, 1
	do while not rs2.eof
		response.write "<tr>"
		response.write "<td>" & rs.fields("ContractNumber").value & "</td>"
		response.write "<td>" & rs.fields("StatusDate").value & "</td>"
'		response.write "<td>" & rs2.fields("Name").value & "</td>"
		response.write "<td>" & rs2.fields("Name").value & "</td>"
		response.write "<td>" & rs2.fields("Week").value & "</td>"
		response.write "<td>" & rs.fields("Frequency").value & "</td>"
		response.write "<td>" & rs2.fields("OccupancyYear").value & "</td>"
		response.write "<td>" & rs.fields("SaleType").value & "</td>"
		response.write "<td>" & rs.fields("BD").value & "</td>"
		response.write "</tr>"
		rs2.movenext
	loop
	rs2.close
	rs.movenext
loop
rs.close
cn.close

response.write "</table>"
set rs = nothing
set rs2 = nothing
set cn = nothing

%>