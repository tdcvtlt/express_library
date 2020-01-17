
<%

	Dim cn
	Dim rs, rs2
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	set rs2 = server.createobject("ADODB.Recordset")
	



DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

	cn.Open DBName, DBUser, DBPass
	
	if request("Function") = "go_Report" then
		sAns = "<table>"
		sAns = sAns & "<tr><th>ContractNumber</th><th>Prospect</th><th>Status</th><th>Status Date</th></tr>"
			
		rs.Open "SELECT DISTINCT a.ContractID FROM t_Usage a INNER JOIN t_Contract b ON a.ContractID = b.ContractID WHERE (b.StatusID IN (SELECT comboitemid FROM t_ComboItems WHERE comboname = 'ContractStatus' AND (comboitem = 'Cancelled' OR comboitem = 'CXL-Upgrade' OR comboitem = 'CXL-Dwngrade'))) AND (a.InDate > GETDATE()) and b.statusdate between '" & request("sDate") & "' and '" & request("eDate") & "' and a.contractid not in (Select contractid from t_CXLdContractReport)", cn, 3, 3
		If rs.EOF and rs.BOF then
		Else
			Do while not rs.EOF
				rs2.OPen "Select * from t_CXLdContractReport where 1=2", cn, 3, 3	
				rs2.AddNew
				rs2.Fields("ContractID") = rs.Fields("ContractID")
				rs2.Updatebatch
				rs2.Close
				rs.MoveNext
			Loop
		End If
		rs.Close
	
		rs.Open "Select e.CXLContractReportID, e.ContractStatus, e.ContractNumber, e.StatusDate, f.FirstName, f.LastName from (Select c.*, d.ComboItem as ContractStatus from (Select a.CXLContractReportID, b.ContractNumber, b.StatusID, b.StatusDate, b.ProspectID from t_CXLdContractReport a inner join t_Contract b on a.contractid = b.contractid where a.Removed = '0') c left outer join t_ComboItems d on c.statusid = d.comboitemid) e left outer join t_Prospect f on e.prospectid = f.prospectid", cn, 3, 3
		If rs.EOF and rs.BOF then
			sAns = sAns & "<tr><td colspan = '4'>No Data to Report</td></tr>"
		Else
			Do while not rs.EOF
				sAns = sAns & "<tr>"
				sAns = sAns & "<td>" & rs.Fields("COntractNumber") & "</td>"
				sAns = sAns & "<td>" & rs.Fields("LastName") & ", " & rs.fields("FirstName") & "</td>"
				sANs = sAns & "<td>" & rs.Fields("ContractStatus") & "</td>"
				sAns = sAns & "<td>" & rs.Fields("StatusDate") & "</td>"
				sAns = sAns & "<td><input type = 'button' name = 'go' value = 'Remove' onClick = " & Chr(10) & "remove_Contract('" & rs.Fields("CXLContractReportID") & "');" & Chr(10) & "></td>"
				sAns = sANs & "</tr>"
				rs.MoveNext
			Loop
		End If
		rs.Close
		
		sAns = sAns & "</table>"

	elseif request("Function") = "remove_Contract" then
		rs.Open "Select * from t_CXLdCOntractReport where CXLContractReportID = '" & request("ID") & "'", cn, 3, 3
		rs.FIelds("Removed") = 1
		rs.Updatebatch
		rs.Close
		sAns = "OK"
	end if
	
	cn.Close
	set rs = Nothing
	set rs2 = Nothing
	set cn = Nothing
	
	response.write sAns
%>