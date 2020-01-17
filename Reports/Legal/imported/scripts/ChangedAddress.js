function Run_Report(){
	var r = new ajaxRequest();
	r.setCallBackFunc(function(){Run_Report_Ans(r);});
	var p = '';
	p += 'sdate=' + document.forms[0].sdate.value;
	p += '&edate=' + document.forms[0].edate.value;
	document.getElementById('report').innerHTML = 'Loading, Please Wait....';
	r.doPost('modules/changedaddress.asp', p);
}

function Run_Report_Ans(r){
	document.getElementById('report').innerHTML = r.responseText;
}