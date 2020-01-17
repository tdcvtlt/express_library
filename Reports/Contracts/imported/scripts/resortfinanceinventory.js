function run_Report()
{
	var req = new ajaxRequest();
	req.setCallBackFunc(function(){run_Report_Ans(req);});
	req.doPost("modules/resortfinanceinventory.asp","");
	document.getElementById("results").innerHTML = "Loading...";
}

function run_Report_Ans(req)
{
	document.getElementById("results").innerHTML = req.responseText;
}
