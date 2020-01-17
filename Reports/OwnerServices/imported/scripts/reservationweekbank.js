
function Run_Report(){
	var sdate = document.forms[0].startdate.value;
	var edate = document.forms[0].enddate.value;
	var nights = (document.forms[0].nights.selectedIndex>0)?document.forms[0].nights.options[document.forms[0].nights.selectedIndex].value:'';
	var type = (document.forms[0].type.selectedIndex>0)?document.forms[0].type.options[document.forms[0].type.selectedIndex].value:'';
	document.forms[0].printable.disabled = true;
	var req = new ajaxRequest();
	//alert(nights)
	req.setCallBackFunc(function(){Show_Report(req);});
	req.doPost("modules/reservationweekbank.asp",'startdate='+ sdate + '&enddate=' + edate + '&nights=' + nights + '&type=' + type + '&detail=' + document.forms[0].detail.checked);
	document.getElementById("report").innerHTML = "Loading... Please wait";
}

function Show_Report(req){
	document.getElementById('report').innerHTML = req.responseText;
	document.forms[0].printable.disabled = false;
}