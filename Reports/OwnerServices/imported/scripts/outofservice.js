function RunReport(){
	var sdate = document.forms[0].startdate.value;
	var edate = document.forms[0].enddate.value;
	var req = new ajaxRequest();
	req.setCallBackFunc(function(){ShowReport(req);});
	req.doPost("modules/outofservice.asp",'startdate='+ sdate + '&enddate=' + edate);
	document.getElementById("report").innerHTML = "Loading... Please wait";
}

function ShowReport(req){
	document.getElementById('report').innerHTML = req.responseText;
}