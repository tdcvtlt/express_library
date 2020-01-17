function Run_Report()
{
	if (document.forms[0].startdate.value == '')
	{
		alert('Please Select a Start Date');
	}
	else if (document.forms[0].enddate.value == '')
	{
		alert('Please Select An End Date');
	}
	else
	{	
		var req=new ajaxRequest();
		req.setCallBackFunc(function(){Show_Report(req);});
		req.doPost("modules/retentionreport.asp","sDate=" + document.forms[0].startdate.value + "&edate=" + document.forms[0].enddate.value);
		document.getElementById("report").innerHTML = "Loading... Please Wait";
		document.forms[0].B1.disabled = true;
		document.forms[0].B2.disabled = true;
	}
}

function Show_Report(req)
{
	document.getElementById("report").innerHTML = req.responseText;
	document.forms[0].B1.disabled = false;
	document.forms[0].B2.disabled = false;
}