function Run_Report(){
	var req=new ajaxRequest();
	req.setCallBackFunc(function(){Show_Report(req);});
	req.doPost("modules/exitcontractactivity.asp","sDate=" + document.forms[0].startdate.value + "&edate=" + document.forms[0].enddate.value + "&loc=" + document.forms[0].location.options[document.forms[0].location.selectedIndex].value);
	document.getElementById("report").innerHTML = "Loading... Please Wait";

	$('#report table td:nth-child(4)').css('background-color', 'red');	

}

function Show_Report(req){
    document.getElementById("report").innerHTML = req.responseText;



}

