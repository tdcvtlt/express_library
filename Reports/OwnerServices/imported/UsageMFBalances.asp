
<html>

<head>
<title>Usages With MF Balance</title>
<meta name="GENERATOR" content="Microsoft FrontPage 6.0" />
<meta name="ProgId" content="FrontPage.Editor.Document" />
<script type="text/javascript" language="javascript" src = "../../../scripts/ajaxRequest.js"></script>
<script type="text/javascript" language="javascript" src = "scripts/UsageMFBalances.js"></script>
<meta name="Microsoft Theme" content="none, default" /> 
<meta name="Microsoft Border" content="none, default" />
</head>

<body>
<form action="">
<table>
	<tr>
		<td>Usage Year:</td>
		<td><select name = 'usageyear'>
			<%
				for i = 2006 to 2019
			%>
				<option value = '<%=i%>'><%=i%></option>
			<%
				next
			%>
			</select>
		</td>	
	</tr>

	<tr>
		<td>MF Invoice:</td>
		<td><select name = 'fintransid'>
			<%
				Dim cn
				Dim rs
				set cn = server.createobject("ADODB.Connection")
				set rs = server.createobject("ADODB.Recordset")


                DBNAME = "CRMSNet"
                DBUser = "asp"
                DBpass = "aspnet"

				cn.Open DBName, DBUser, DBPass
				rs.Open "Select a.FinTransID, b.ComboItem from t_fintranscodes a  " & _
                        "inner join t_ComboItems b on a.transcodeid = b.comboitemid " & _
                        "where a.TransTypeID in (Select comboitemid from t_ComboItems a " & _
                        "inner join t_combos b on a.comboid = b.comboid where comboname = 'TransCodeType' " & _
                        "and comboitem = 'MFTrans') order by b.comboitem asc", cn, 3, 3
				Do while not rs.EOF
				%>
					<!--
                    <option value = '<%=rs.Fields("FinTransID")%>'><%=rs.Fields("ComboItem")%></option>
                    -->
                    <option value = '<%=rs.Fields("ComboItem")%>'><%=rs.Fields("ComboItem")%></option>
				<%
					rs.MoveNext
				Loop
				rs.Close
				cn.Close
				set rs = Nothing
				set cn = Nothing


			%>
		</select></td>
	</tr>
	<tr>
		<td><input type = 'button' name = 'go' value = 'Submit' onclick = 'run_Report();' /></td>
	</tr>
</table>
<div id = 'results'></div>
</form>

</body>
</html>
