var sMod = "modules/campaign_efficiency.asp";

function Run_Report(){
	if (Valid()){
		var r = new ajaxRequest();
		r.setCallBackFunc(function(){Run_Report_Ans(r);});
		var params = "f=report";
		params += "&sdate=" + document.forms[0].sdate.value;
		params += "&edate=" + document.forms[0].edate.value;
		document.getElementById('report').innerHTML = document.getElementById('status').innerHTML;
		document.forms[0].sub_button.disabled=true;
		r.doPost(sMod, params);
	}
}

function Run_Report_Ans(r){
	document.getElementById('report').innerHTML = r.responseText;
	document.forms[0].sub_button.disabled = false;
}

function Valid(){
	var f = document.forms[0];
	var sErr = '';
	
	sErr += (f.sdate.value =='')?'Please select a Start Date.\n':'';
	sErr += (f.edate.value =='')?'Please select an End Date.\n':'';
	
	(sErr=='')?null:alert(sErr);
	
	return (sErr=='')?true:false;
}

