

<html>

<head>
<title>Tour Detail For Vendor</title>
<script language="javascript" src = "../../../scripts/scw.js"></script>
<script language="javascript" src = "../../../scripts/ajaxRequest.js"></script>
<script language="javascript" src = "scripts/tourdetailforvendor.js"></script>

</head>

<body>
<h3>Tour Detail For Vendor</h3>
	<form method="POST" action="tourDetailforvendors.asp" onsubmit="return false;">
     Please Select A Location:&nbsp;&nbsp; <select size="1" name="location"><option value=0>ALL</option>

<%

		dim rs 
		dim cn

		set cn = server.createobject("ADODB.Connection")
		set rs = server.createobject("ADODB.Recordset")

		cn.open "CRMSNet", "asp", "aspnet"

		dim rsL
		set rsL = server.createobject("ADODB.Recordset")
		rsL.open "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where (active = 1 or comboitemid = '" & request("Location") & "') and comboname = 'tourlocation' order by comboitem", cn, 3, 3
		do while not rsL.eof
			if request("Location") = cstr(rsL.fields("Comboitemid").value) then
				response.write "<option selected value = '" & rsL.fields("ComboItemID").value & "'>" & rsL.fields("ComboItem").value & "</option>"
			else
				response.write "<option value = '" & rsL.fields("ComboItemID").value & "'>" & rsL.fields("ComboItem").value & "</option>"
			end if
			rsL.movenext
		loop
		rsL.close
		set rsL = nothing
		cn.close
		set cn = nothing
%>          
</select>
<table><tr><td>Start Date: </td><td><input type = 'text' id = 'input_startdate' name = 'startdate' Readonly  onclick="scwShow(this,this);" value="<%=request("startdate")%>" size="20"></td></tr>
<tr><td>End Date:</td><td><input type = 'text' id = 'input_enddate' name = 'enddate' value="<%=request("enddate")%>" size="20" readonly onclick="scwShow(this,this);"></td></tr>
<tr><td colspan=2>
						<input type = 'submit' name = 'go' value = 'Run Report' onclick="Run_Report();">
                        <input type="button" value="Printable Version" name="B1" onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('report').innerHTML);">
                        </td></tr></table>
                    </form>
<div id = "report"></div>

</body>

</html>
