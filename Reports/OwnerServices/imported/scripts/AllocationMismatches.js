function go_Report()
{
	var err = "";
	if (document.forms[0].sDate.value == '')
	{
		err = "Please Enter a Start Date.\n";
	}
	else if (document.forms[0].sDate.value == '')
	{
		err += "Please Enter An End Date.";
	}
	
	if (err == "")
	{
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){go_Report_Ans(req);});
		req.doPost("modules/AllocationMismatches.asp","Function=Run_Report&sDate=" + document.forms[0].sDate.value + "&eDate=" + document.forms[0].eDate.value);
		document.getElementById("report").innerHTML = "Loading...";
	}
	else
	{
		alert(err);
	}
}

function go_Report_Ans(req)
{
	document.getElementById("report").innerHTML = req.responseText;
}
