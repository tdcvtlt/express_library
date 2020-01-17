function go_Report()
{
	//var req = new ajaxRequest();
	//req.setCallBackFunc(function(){go_Report_Ans(req);});
	//req.doPost("modules/OPC-OSLabels.asp","sDate=" + document.forms[0].sDate.value + "&eDate=" + document.forms[0].eDate.value);
	window.open("modules/OPC-OSLabels.asp?sDate=" + document.forms[0].sDate.value + "&eDate=" + document.forms[0].eDate.value);
}

function go_Report_Ans(req)
{
	alert(req.responseText);
	document.getElementById("report").innerHTML = req.resonseText;
}