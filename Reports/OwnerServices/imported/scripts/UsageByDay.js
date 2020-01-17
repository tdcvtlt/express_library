function RunReport()
{
	var req = new ajaxRequest();
	req.setCallBackFunc(function() {RunReport_Ans(req);});
	var params = '';
	params += 'Function=RunReport';
	params += '&SDate=' + document.forms[0].sdate.value;
	params += '&EDate=' + document.forms[0].edate.value;
	req.doPost("modules/usagebyday.asp",params);
	document.getElementById("report").innerHTML = "Loading report... Please wait...";
}

function RunReport_Ans(req)
{
	document.getElementById("report").innerHTML = req.responseText;
}