
<%		
	Server.ScriptTimeout = 10000
	
	'Server variables
	dim rs
	dim cn

	'Html & sql variables
	dim sql

	'Parameters
	dim date_begin
	dim date_end
	date_begin 	= Request("sDate")
	date_end  	= Request("eDate")
		
	'//SQL string
	sql = 	"select status = (select comboitem from t_comboitems where comboitemid = a.statusid), " & _
			"a.contractnumber, rtrim(c.lastname) + ', ' + rtrim(c.firstname) as owner, " & _
			"saletype = (select comboitem from t_comboitems where comboitemid = a.saletypeid), " & _
			"type = (select comboitem from t_comboitems where comboitemid = a.typeid), " & _
			"b.dptotal, b.totalfinanced, b.cctotal, b.salesvolume, b.salesprice, b.discount, " & _
			"a.ContractDate, " & _
			"a.StatusDate, " & _
			"null as User13, " & _
			"b.origsellingprice, null as equity " & _
			"from t_contract a " & _
			"inner join t_mortgage b " & _
			"on a.contractid = b.contractid " & _
			"inner join t_prospect c " & _
			"on a.prospectid = c.prospectid " & _
			"left outer join t_comboitems d " & _
			"on a.saletypeid = d.comboitemid " & _
			"left outer join t_comboitems e " & _
			"on e.comboitemid = a.statusid " & _
			"where  ((a.statusdate between '" & CDate(date_begin) & "' and '" & CDate(date_end) & "') " & _
			"and (e.comboitem like '%can%' or e.comboitem like '%cxl%' or e.comboitem like 'res%')and a.contractnumber not like 't%' and a.contractnumber not like 'U%'"


	sql2 = "Select c.*, rtrim(d.LastName) + ', ' + rtrim(d.FirstName) as owner from (Select contractnumber, statusdate, contractdate, prospectid, b.ComboItem as Status from t_Contract a inner join t_ComboItems b on a.statusid = b.comboitemid where (b.comboitem like '%can%' or b.comboitem like '%cxl%' or b.comboitem like 'res%') and (a.StatusDate between '" & CDate(date_begin) & "' and '" & CDate(date_end) & "' )) c inner join t_Prospect d on c.prospectid = d.prospectid"
	
	'Make CRMS connection
	Set cn = Server.CreateObject("ADODB.Connection")
	cn.ConnectionTimeout = 0
	

    DBName = "CRMSNET"
    DBUser = "asp"
    DBPass = "aspnet"

	cn.Open DBName, DBUser, DBPass
		
	
	'Make Object and Run Open
	Set rs = Server.CreateObject("ADODB.RecordSet")
	rs.CursorType = 3
	rs.CursorLocation = 3
	rs.Open sql2, cn, 3, 3	
		
	'//-- Check for empty recordset
	If Not (rs.BOF And rs.EOF) Then
		sAns = "<table border = '1' style='border-collapse:collapse;'>"
		sAns = sAns & "<tr><th>KCP</th><th>Owner</th><th>Cancel Date</th><th>Scan&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</th><th>Shred Date</th><th>Verifier&nbsp&nbsp&nbsp</th></tr>"
	
		Do while not rs.EOF
			sAns = sAns & "<tr>"
			sAns = sAns & "<td>" & rs.Fields("ContractNumber") & "</td>"
			sAns = sAns & "<td>" & rs.Fields("owner") & "</td>"
			if rs.Fields("StatusDate") = rs.Fields("ContractDate") then
				sAns = sAns & "<td>" & rs.Fields("User13") & "</td>"
			else
				sAns = sANs & "<td>" & rs.Fields("StatusDate") & "</td>"
			end if
			sAns = sAns & "<td>&nbsp</td>"
			sAns = sAns & "<td>&nbsp</td>"
			sAns = sAns & "<td>&nbsp</td>"			
			sAns = sAns & "</tr>"
			rs.MoveNext
		Loop
		sAns = sAns & "</table>"
	Else
		sAns = "No Records Matching Your Criteria Found!"
	End If	
	
	rs.Close
	cn.Close 
	set rs = Nothing
	set cn = Nothing
	Response.Write sAns
%>