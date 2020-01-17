function go_Report()
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){go_Report_Ans(req);});
	req.doPost("modules/OwnerNotebook.asp","Function=Run_Report");
	document.getElementById("report").innerHTML = "Loading This May Take Several Minutes....<br><img src = 'https://crms.kingscreekplantation.com/crmsnet/images/progressbar.gif'>";
}

function go_Report_Ans(req)
{
	document.getElementById("report").innerHTML = req.responseText;
}
