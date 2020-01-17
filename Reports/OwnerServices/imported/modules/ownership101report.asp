

<%
dim cn
dim rs
dim sdate,edate

sdate = request("sdate")
edate = request("edate")
set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")


DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"


cn.open DBName, DBUser, DBPass

rs.open "select * from " & _
		"(select count(distinct contractid) as ContractCount " & _
		"from t_contract c " & _
		"where (c.contractdate between '" & sdate & "' and '" & edate & "' or c.statusdate between '" & sdate & "' and '" & edate & "') and c.contractnumber not like '%T%' and contractnumber not like '%N%' and c.statusid in ( " & _
		"select comboitemid from t_comboitems " & _ 
		"where comboname = 'ContractStatus' and comboitem in ('Active','Suspense','Developer')) " & _
		") as e, " & _
		"(select count(distinct contractid) as TotalCalled " & _
		"from t_ownership101 " & _
		"where contractid in (select contractid " & _
		"from t_contract c " & _
		"where (c.contractdate between '" & sdate & "' and '" & edate & "' or c.statusdate between '" & sdate & "' and '" & edate & "') and c.contractnumber not like '%T%' and contractnumber not like '%N%' and c.statusid in ( " & _
		"select comboitemid from t_comboitems  " & _
		"where comboname = 'ContractStatus' and comboitem in ('Active','Suspense','Developer'))) " & _
		") as a, " & _
		"(select count(distinct contractid) as TotalConfirmed " & _
		"from t_ownership101 " & _
		"where confirmed = 1 and contractid in (select contractid  " & _
		"from t_contract c  " & _
		"where (c.contractdate between '" & sdate & "' and '" & edate & "' or c.statusdate between '" & sdate & "' and '" & edate & "') and c.contractnumber not like '%T%' and contractnumber not like '%N%' and c.statusid in ( " & _
		"select comboitemid from t_comboitems  " & _
		"where comboname = 'ContractStatus' and comboitem in ('Active','Suspense','Developer'))) " & _
		") as b, " & _
		"(select count(distinct contractid) as TotalMessage " & _
		"from t_ownership101 " & _
		"where confirmed = 0 and messagedate is not null and contractid in (select contractid  " & _
		"from t_contract c  " & _
		"where (c.contractdate between '" & sdate & "' and '" & edate & "' or c.statusdate between '" & sdate & "' and '" & edate & "') and c.contractnumber not like '%T%' and contractnumber not like '%N%' and c.statusid in ( " & _
		"select comboitemid from t_comboitems  " & _
		"where comboname = 'ContractStatus' and comboitem in ('Active','Suspense','Developer')))  " & _
		") as c, " & _
		"(select count(distinct contractid) as 'N/A' from t_ownership101 " & _
		"where confirmed = 0 and messagedate is null and contractid in (select contractid  " & _
		"from t_contract c  " & _
		"where (c.contractdate between '" & sdate & "' and '" & edate & "' or c.statusdate between '" & sdate & "' and '" & edate & "') and c.contractnumber not like '%T%' and contractnumber not like '%N%' and c.statusid in ( " & _
		"select comboitemid from t_comboitems  " & _
		"where comboname = 'ContractStatus' and comboitem in ('Active','Suspense','Developer'))) " & _
		") as d ",cn,3,3


response.write "<table>"
response.write "<tr>"
for i = 0 to rs.fields.count -1
	response.write "<u><b><th cellspacing = 10>" & rs.fields(i).name & "</th></b></u>"
next

do while not rs.eof
	response.write "<tr>"
	for i = 0 to rs.fields.count -1
		response.write "<td align = center>" & rs.fields(i).value & "</td>"
	next
	response.write "</tr>"
	rs.movenext
loop
response.write "</table>"
rs.close
cn.close
set rs = nothing
set cn = nothing
%> 
