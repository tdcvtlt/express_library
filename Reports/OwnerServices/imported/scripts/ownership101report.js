function Run_Report(){
	var req = new ajaxRequest();
	var sdate = document.forms[0].startdate.value;
	var edate = document.forms[0].enddate.value;
	req.setCallBackFunc(function(){Run_Report_Ans(req);});
	req.doPost('modules/ownership101report.asp?sdate=' + sdate + '&edate=' + edate);
	document.getElementById('report').innerHTML = "Loading... Please Wait";
}

function Run_Report_Ans(req){
	//alert('Hello');
	document.getElementById('Report').innerHTML = req.responseText;
}