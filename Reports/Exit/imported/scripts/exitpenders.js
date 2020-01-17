function Run_Report(){
	var req=new ajaxRequest();
	req.setCallBackFunc(function(){Show_Report(req);});
	req.doPost("modules/exitpenders.asp","sDate=" + document.forms[0].startdate.value + "&edate=" + document.forms[0].enddate.value);
	document.getElementById("report").innerHTML = "Loading... Please Wait";
}

function Show_Report(req){
	document.getElementById("report").innerHTML = req.responseText;
	document.forms[0].printable.disabled = false;
}