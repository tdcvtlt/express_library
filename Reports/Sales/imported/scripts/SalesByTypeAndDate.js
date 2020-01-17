function run_report(){
	document.getElementById('report').innerHTML = "Loading... Please Wait";
	var r = new ajaxRequest();
	r.setCallBackFunc(function(){run_report_ans(r);});
	r.doPost("modules/salesbytypeanddate.asp","invtype=" + document.forms[0].invtype.options[document.forms[0].invtype.selectedIndex].value + '&sdate=' + document.forms[0].sdate.value + '&edate=' + document.forms[0].edate.value);
}

function run_report_ans(r){
	document.getElementById('report').innerHTML = r.responseText;
}