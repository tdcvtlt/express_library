function RunReport()
{
	var f = document.forms[0];
	if (f.sdate.value>f.edate.value)
	{
		alert("Please enter a valid date range.");
	}
	else
	{
		var req = new ajaxRequest();
		req.setCallBackFunc(function(){RunReport_Ans(req);});
		var params = '';
		params += "Function=RunReport";
		params += "&SDate=" + f.sdate.value;
		params += "&EDate=" + f.edate.value;
		req.doPost("modules/checkinsbyday.asp",params);
		document.getElementById("report").innerHTML = "Loading report...Please wait.";
	}
}

function RunReport_Ans(req)
{
	document.getElementById("report").innerHTML = req.responseText;	
}
