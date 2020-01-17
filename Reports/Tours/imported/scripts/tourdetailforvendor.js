function Run_Report(){
	var req = new ajaxRequest();
	req.setCallBackFunc(function(){Show_Report(req);});
	req.doPost("modules/tourdetailforvendors.asp","StartDate=" + document.forms[0].startdate.value + "&EndDate=" + document.forms[0].enddate.value + "&Location=" + document.forms[0].location.options[document.forms[0].location.selectedIndex].value);
	document.getElementById("report").innerHTML = "Loading Report... Please Wait";
}

function Show_Report(req){
	document.getElementById("report").innerHTML = req.responseText;
}