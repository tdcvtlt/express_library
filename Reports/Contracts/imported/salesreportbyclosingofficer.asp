<%
dim strTitle
strTitle = "'Closing Officer','Closer'"
%> <html><head><title>Closing Officer Report</title>
<script language = "javascript" src = "../../../scripts/scw.js"></script>
<script language = "javascript" src = "../../../scripts/ajaxRequest.js"></script>
<script language = "javascript" src = "scripts/salesreportbyclosingofficer.js"></script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="none, default">
</head><body><div id = choices style="visibility:visible; position:absolute; left:172px; top:127px"><form><table><tr><td>Start Date:</td><td><input type = text name = sDate onclick = "scwShow(this, this);" readonly></td></tr><tr><td>End Date:</td><td><input type = text name = eDate onclick = "scwShow(this, this);" readonly></td></tr><tr><td>Status:</td><td><select name = status><%
						dim cn
						dim rs
						set cn = server.createobject("ADODB.Connection")
						set rs = server.createobject("ADODB.Recordset")

DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

						cn.open DBName, DBUser, DBPass

						rs.open "select * from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'ContractStatus' order by ComboName", cn,3,3
						do while not rs.eof
							response.write "<option value='" & rs.fields("ComboItemID").value & "'>" & rs.fields("ComboItem").value & "</option>"
							rs.movenext
						loop
						rs.close
%></select> <input type = button value = "Add" onclick = "Add_Status();"> <input type = button value = "Remove" onclick = "Remove_Status();"> </td></tr><tr><td colspan = 2><select name = statusarray size = 10 style="width: 550; height: 166"></select></td></tr><tr><td>Officer:</td><td><select name = officer><%
						rs.open "select p.lastname + ', ' + p.firstname as Name, p.personnelid from t_Personnel p where p.personnelid in (select distinct personnelid from t_PersonnelTrans where titleid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'personneltitle' and comboitem in (" & strTitle & "))) order by p.lastname, p.firstname", cn,3,3
						do while not rs.eof
							response.write "<option value='" & rs.fields("PersonnelID").value & "'>" & rs.fields("Name").value & "</option>"
							rs.movenext
						loop
						rs.close
						cn.close
						set rs = nothing
						set cn = nothing
%></select> <input type = button value = "Add" onclick = "Add_Officer();"> <input type = button value = "Remove" onclick = "Remove_Officer();"> </td></tr><tr><td colspan = 2><select name = officerarray  size = 10 style="width: 550; height: 166"></select></td></tr><tr><td colspan=2><input type = button value = 'Run Report' onclick = "Run_Report();"></td></tr></table></form></div><div id = options style = "position:absolute; left:174px; top:128px; visibility:hidden; width:550px"><input type=button value = 'View Options' onclick = "document.getElementById('options').style.visibility = 'hidden';document.getElementById('report').style.visibility='hidden';document.getElementById('choices').style.visibility='visible';"> <input type=button value = 'Printable Version' onclick = "var mWin = window.open(''); mWin.document.write(document.getElementById('report').innerHTML);"> </div><div id = report style= "position:absolute; left:175px; top:161px; visibility:hidden">report</div></body></html>