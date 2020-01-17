// JavaScript Document
function Get_Report(batch){
	if (document.Report.input_startdate.value == ''){
	 alert('Please Select A Start Date');
	} else if (document.Report.input_enddate.value == ''){
	 alert('Please Select An Ending Date');
	} else {
    var req = new ajaxRequest;
  	req.setCallBackFunc(function(){Show_Report(req);});
  	req.doPost("modules/PersonnelPerformanceReport.asp","OWs=" + document.forms[0].ows.checked + "&Title=" + document.Report.Title.options[document.Report.Title.selectedIndex].value + "&sDate=" + document.Report.input_startdate.value + "&eDate=" + document.Report.input_enddate.value);
  	document.getElementById('result').innerHTML = "Loading ... ";
  }
}

function Show_Report(req){
	document.getElementById('result').innerHTML = req.responseText;
	document.getElementById('result').style.display = "block";	
}
