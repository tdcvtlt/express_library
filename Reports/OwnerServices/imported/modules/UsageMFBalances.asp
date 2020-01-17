

<%
	Dim cn
	Dim rs
	
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	


DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

	cn.Open DBName, DBUser, DBPass
	
	if request("Function") = "run_Report" then
		sAns = "<table style='border-collapse:collapse;' border='1px'>"
		sAns = sAns & "<tr><th>KCP#</th><th>Prospect</th><th>UsageID</th><th>MF Balance</th><th>Week Type</th><th>Frequency</th></tr>"

		rs.OPen "select c.ContractNumber, p.LastName + ', ' + p.FirstName Owner, u.UsageID, Sum(v.Balance) Balance, f.Frequency, " & _
                "(select comboItem from t_comboitems where comboitemid = c.weektypeid) WeekType from t_contract c " & _
                "inner join t_usage u on c.contractid = u.contractid " & _
                "inner join t_prospect p on c.prospectid = p.prospectid " & _
                "left outer join t_Frequency f on c.FrequencyID = f.FrequencyID " & _
                "inner join vw_Invoices v on v.keyvalue = c.contractid " & _
                "and v.keyfield = 'contractid' " & _
                "where u.StatusId in (17626) " & _
                "and u.usageyear = '" & request("usageyear") & "'" & _
                "and (v.Balance > 0 and v.Balance IS NOT NULL) " & _
                "and v.Invoice = '" & request("INVOICE") & "'" & _
                "group by UsageID, ContractNumber, LastName, FirstName, weektypeid, frequency " & _
                "order by p.LastName", cn, 3, 3

		If rs.EOF and rs.BOF then
			sAns = sAns & "<tr><td colspan = '5'>No Records to Report.</td></tr>"
		Else
			Do while not rs.EOF
				sAns = sAns & "<tr>"
				sAns = sAns & "<td style='width:100px'>" & rs.Fields("ContractNumber") & "</td>"
				sANs = sAns & "<td style='width:180px'>" & rs.Fields("Owner") & "</td>"
				sAns = sAns & "<td style='width:70px;text-align:right;'>" & rs.Fields("UsageID") & "</td>"
				sAns = sAns & "<td style='width:100px;text-align:right; '>" & FormatCurrency(rs.Fields("Balance"), 2) & "</td>"
				sAns = sAns & "<td style='width:100px;'><span style='padding-left:15px;text-align:left;'>" & rs.fields("WeekType") & "</span></td>"
                sAns = sAns & "<td styel='width:100px;'>" & rs.Fields("Frequency") & "</td>"
				sAns = sANs & "</tr>"
				rs.MoveNExt
			Loop
		End If
		rs.Close
		sAns = sAns & "</table>"
	end if

	cn.Close
	response.write sAns
%>	