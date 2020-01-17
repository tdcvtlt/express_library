<html><%
dim cn
dim rs
set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.recordset")


DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"


cn.open DBName, DBUser, DBPass
%> <head><title>Owner Inventory</title>
<script language="javascript" src="scripts/ownerinventory.js"></script>
<script language="javascript" src="../../../scripts/ajaxrequest.js"></script>

<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="none, default">

</head>
<body>
    <form><table style='border-collapse:collapse;'>
        <tr><td>Occupancy Year:</td><td><select name="occupancyyear">
            <option value="0">All</option>
            <%for i = 1998 to year(date)+2%>
                <option value='<%=i%>'><%=i%></option><%next%>
                </select> </td></tr><tr><td>Season:</td><td><select name="season">
                <%rs.open "select * from t_comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'season'",cn,3,3%><option value="0">All</option><%do while not rs.eof%><option value="<%=rs.fields("comboitemid").value%>"><%=rs.fields("comboitem").value%></option><%rs.movenext
				loop
				rs.close%></select> </td></tr><tr><td>Unit Type:</td><td><select name='unittype'><%rs.open "select * from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'UnitType' order by comboitem",cn,3,3%><option value="0">All</option><%do while not rs.eof%><option value="<%=rs.fields("comboitemid").value%>"><%=rs.fields("comboitem").value%></option><%rs.movenext
				loop
				rs.close%></select> </td></tr><tr><td>Frequency:</td><td><select name="frequency"><%rs.open "select * from t_frequency order by frequency asc",cn,3,3%><option value="0">All</option><%do while not rs.eof%><option value="<%=rs.fields("FrequencyID").value%>"><%=rs.fields("Frequency").value%></option><%rs.movenext
					loop
					rs.close%></select> </td></tr></table><table><tr><td><input type="button" value="Run Report" onclick="Send();"><input type='button' name='search0' id='printable' value='Printable Version' onclick='var mWin=window.open();mWin.document.write(document.getElementById("display").innerHTML);'></td></tr></table><br><div id='display'></div></form></body><%
cn.close
set rs = nothing
set cn = nothing
%> </html>