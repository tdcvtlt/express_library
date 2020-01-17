


<%
	dim rs
	dim cn
	
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	
	'DBName = "CRMSData"


    DBName = "CRMSNET"
    DBUser = "asp"
    DBPass = "aspnet"

	cn.open DBName, DBUser, DBPass

	rs.open "select c.Contractid, e.KEYVALUE, c.ContractNumber, e.OldValue, e.NewValue, e.FieldName, e.DateCreated, p.UserName "& _
			"from t_event e " & _
			"inner join t_contract c on c.prospectid = e.KEYVALUE " & _
			"inner join t_personnel p on p.personnelid = e.createdbyid " & _
			"where  (e.fieldname = 'address1' or e.fieldname = 'stateorprovince' or e.fieldname = 'Country/Region' or e.fieldname = 'postalcode' or e.fieldname = 'City') and e.datecreated between '" & request("sdate") & "' and '" & request("edate") & "' " & _
			"order by e.datecreated desc",cn,3,3
		
	with response
		if not(rs.eof) then
			.write "<table>"
			.write "<tr>"
			for i = 1 to rs.fields.count -1
				.write "<th>" & rs.fields(i).name & "</th>"
			next
			do while not(rs.eof)
				.write "<tr>"
				for i = 1 to rs.fields.count - 1
					.write "<td>" & rs.fields(i).value & "</td>"
				next
				.write "<td><a href = '../../editcontract.asp?contractid=" & rs.fields("Contractid").value & "'><img src='../../images/edit.gif'></a></td>"
				.write "</tr>"
				rs.movenext
			loop
			rs.close
		else
			.write "<b>There are no address changes in this date range....</b>"
		end if
		.write "</table>"
	end with
	
	cn.close
	
	set rs = nothing
	set cn = nothing	
%>