

<%
dim cn
dim rs

set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.recordset")


DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

cn.open DBName, DBUser, DBPass


if request("Function") = "Save" then
	cn.begintrans
	if request("checkbyphoneid") = "0" or request("checkbyphoneid") = "" then
		rs.open "select * from t_CheckByphone where 1=2",cn,1,3
		rs.addnew
		bNew = True
	else
		rs.open "select * from t_checkbyphone where checkbyphoneid = '" & request("checkbyphoneid") & "'",cn,1,3
		if rs.eof and rs.bof then
			rs.addnew
			bNew = True
		end if
	end if
	
	rs.fields("EnteredByID").value = session("USerID")
	rs.fields("AccountLastName").value = request("LName")
	rs.fields("AccountMiddleInit").value = request("MidInit")
	rs.fields("AccountFirstName").value = request("FName")
	rs.fields("ContractNumber").value = request("ContractNumber")
	rs.fields("RoutingNumber").value = request("RoutingNumber")
	rs.fields("AccountNumber").value = request("AccountNumber")
	if request("CheckingFlag") = "true" then rs.fields("CheckingFlag").value = 1
	if request("SavingsFlag") = "true" then rs.fields("SavingsFlag").value = 1
	rs.fields("Amount").value = request("Amount")
	rs.fields("DateToRun").value = request("DateToRun")
	rs.fields("StatusID").value = request("StatusID")
	rs.fields("DateCompleted").value = request("DateCompleted")
	rs.fields("TransactionID").value = request("TransactionID")
	rs.update
	if err <> 0 then
		cn.rollbacktrans
	else
		cn.committrans
	end if
	rs.close
	

elseif request("function") = "RunReport" then
	if request("ContractNumber") = "" then
		sSQL =	"SELECT ck.CheckByPhoneID, ck.ContractNumber, ck.Amount, s.ComboItem as Status, ck.DateToRun, ck.DateCompleted, ck.TransactionID, p.UserName " & _
				"FROM t_CheckByPhone ck " & _
				"LEFT JOIN t_ComboItems s on s.ComboItemID = ck.StatusID " & _
				"INNER JOIN t_Personnel p on p.PersonnelID = ck.EnteredByID"
		
		if request("StatusID") = 0 then
		else
			sSQL = sSQL & " and ck.StatusID = '" & request("StatusID") & "'"
		end if
		
		if request("StartDate") = "" and request("EndDate") = "" then
		else
			sSQL = sSQL & " and ck.DateToRun between '" & request("StartDate") & "' and '" & request("EndDate") & "'"
		end if
	else
		sSQL =	"SELECT ck.CheckByPhoneID, ck.ContractNumber, ck.Amount, s.ComboItem as Status, ck.DateToRun, ck.DateCompleted, ck.TransactionID, p.UserName " & _
				"FROM t_CheckByPhone ck " & _
				"LEFT JOIN t_ComboItems s on s.ComboItemID = ck.StatusID " & _
				"INNER JOIN t_Personnel p on p.PersonnelID = ck.EnteredByID " & _
				"WHERE ck.ContractNumber = '" & request("ContractNumber") & "'"
	end if

	rs.open sSQL,cn,3,3
%>
	<table style='border-collapse:collapse;border:1px solid green;' border='1px'>
		<tr>
			<td><b><u>Contract Number</u></b></td>
			<td><b><u>Amount</u></b></td>
			<td><b><u>Status</u></b></td>
			<td><b><u>Date To Run</u></b></td>
			<td><b><u>Date Completed</u></b></td>
			<td><b><u>Transaction ID</u></b></td>
			<td><b><u>Entered By</u></b></td>
		</tr>
		<%
		do while not rs.eof
		%>
		<tr>
			<td><%=rs.fields("ContractNumber").value%></td>
			<td><%=FORMATCURRENCY(rs.fields("Amount").value)%></td>
			<td><%=rs.fields("Status").value%></td>
			<td><%=rs.fields("DateToRun").value%></td>
			<td><%=rs.fields("DateCompleted").value%></td>
			<td><%=rs.fields("TransactionID").value%></td>
			<td><%=rs.fields("UserName").value%></td>
			
            <td><a runat="server" href= "http://crms.kingscreekplantation.com/crmsnet/Add-Ins/CheckByPhone.aspx?CheckByPhoneID=<%= rs.fields("CheckByPhoneID").value %>"><img runat="server" src="http://crms.kingscreekplantation.com/crmsnet/images/edit.gif" alt="" /></a></td>
		</tr>
		<%
		rs.movenext
		loop
		rs.close
		%>
	</table>
<%	

else
	response.write "Incorrect Function"
end if




cn.close

set rs = nothing
set cn = nothing
%>

