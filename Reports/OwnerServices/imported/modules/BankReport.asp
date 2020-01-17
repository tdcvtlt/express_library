
<html><head><title>New Page 2</title><meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="none, default">
</head><body><%



dim cn
dim rs

set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.recordset")

DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

cn.open DBName, DBUser, DBPass

rs.open "select * from t_bankedunits where statusid like '" & request("status") & "%' and unitsize like '" & request("unitsize") & "%' and Exchangeid like '" & request("exchange") & "%' and usageyear like '" & request("yearused") & "%' and seasonid like '" & request("season") & "%' and unittypeid like '" & request("unittype") & "%' and deposityear like '" & request("deposityear") & "%' and frequencyid like '" & request("usage") & "%' ",cn,3,3

response.write rs.recordcount 

rs.close
cn.close
set rs = nothing
set cn = nothing

%> &nbsp;</body></html>