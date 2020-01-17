function run_Report()
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){run_Report_Ans(req);});
	req.doPost("modules/UsageMFBalances.asp?Function=run_Report&INVOICE=" + document.forms[0].fintransid.value + "&usageyear=" + document.forms[0].usageyear.value);
	document.getElementById("results").innerHTML = "Loading..."
	document.forms[0].go.disabled = true;

	//  document.forms[0].fintransid.options[document.forms[0].fintransid.selectedIndex].text
}

function run_Report_Ans(req)
{
	document.getElementById("results").innerHTML = req.responseText;
	document.forms[0].go.disabled = false;

	
}
