function run_Report()
{
	var req = new ajaxRequest();
	req.setCallBackFunc(function(){run_Report_Ans(req);});
	req.doPost("modules/OPC-OSLedger.asp","function=Run_Report&PersonnelID=" + document.forms[0].repID.value + "&sDate=" + document.forms[0].sDate.value + "&eDate=" + document.forms[0].eDate.value);
	document.getElementById("results").innerHTML = "Loading...";
}

function run_Report_Ans(req)
{
	document.getElementById("results").innerHTML = req.responseText;
}

function Report_Excel()
{
	var params = 'function=Run_Report';
	params += '&sdate=' + document.forms[0].sDate.value;
	params += '&edate=' + document.forms[0].eDate.value;
	params += '&CampaignID=' + document.forms[0].CampaignID.value;
	window.navigate('modules/OPC-OSLedger.asp?' + params);
}	