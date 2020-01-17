<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html><head><meta http-equiv="Content-Type" content="text/html; charset=windows-1250">
<title></title><meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="tlb;default">
</head><body><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

<p align="center"><font size="6"><strong>
</strong></font><br>
</p>
<p align="center">&nbsp;</p>

</td></tr><!--msnavigation--></table><!--msnavigation--><table dir="ltr" border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td valign="top" width="1%">

</td><td valign="top" width="24"></td><!--msnavigation--><td valign="top"><script language="Javascript" src="../../../scripts/ajaxRequest.js"></script>
<script language="Javascript" src="scripts/PersonnelPerformanceReport.js"></script>
<script language="Javascript" src="../../../scripts/scw.js"></script>
<form name = 'Report'><%
	dim cn
	dim rst
%> Personnel Title: <select name = 'Title'><%
  set cn = server.createObject("ADODB.Connection")
	set rst = server.createObject("ADODB.Recordset")
  cn.Open "CRMSNet", "asp", "aspnet"
  rst.OPen "Select Distinct(ComboItem) from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelTitle' order by ComboItem asc", cn, 3, 3
  If not(rst.EOF and rst.BOF) then
    Do while not rst.EOF
      response.write "<option value = '" & replace(trim(rst.Fields("ComboItem").value)," ","%20") & "'>" & rst.Fields("ComboItem") & "</option>"
      rst.MoveNext
    Loop
  end if
  rst.Close
  cn.Close
  set rst = Nothing
  set cn = Nothing
%></select> <br>StartDate: <input type = 'text' id = 'input_startdate' name = 'startdate' Readonly onclick="scwShow(this,this);"> <br>End Date: <input type = 'text' id = 'input_enddate' name = 'enddate' Readonly onclick="scwShow(this,this);"> <br><input type="checkbox" name="ows" value="ON" checked> Include OWs<br><input type = 'hidden' name = 'rpt' value = '<%=request("rpt")%>'><input type = 'button' name = 'go' value = 'Run Report' onClick = 'Get_Report();'> <input type="button" value="Printable Version" name="B1" onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('result').innerHTML);"></form><div id = 'result'></div><script type="text/javascript" language="javascript"  src="scripts/calendar.js"></script> 
<p><!--msnavigation--></td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

</td></tr><!--msnavigation--></table></body></html>