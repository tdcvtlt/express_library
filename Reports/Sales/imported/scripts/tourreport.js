function Report(){
	if (Valid()) {
		var r = new ajaxRequest();
		r.setCallBackFunc(function(){Report_Ans(r);});
		var params = "f=report";
		params += "&sdate=" + document.forms[0].sdate.value;
		document.getElementById('report').innerHTML = document.getElementById('status').innerHTML;
		r.doPost("modules/tourreport.asp", params);
	}
}

function Report_Ans(r){
	document.getElementById('report').innerHTML = r.responseText;
}

function Valid(){
	var sErr = '';
	sErr += (document.forms[0].sdate.value == '')?'Please enter a Date':'';
	
	(sErr == '')?null:alert(sErr);
	
	return (sErr=='')?true:false;
}