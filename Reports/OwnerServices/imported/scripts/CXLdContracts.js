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
		req.doPost("modules/CXLdContracts.asp","Function=go_Report&sDate=" + document.forms[0].sDate.value + "&eDate=" + document.forms[0].eDate.value);
		document.forms[0].go.disabled = true;
		document.getElementById("report").innerHTML = "Loading....";
	}
	else
	{
		alert(err);
	}
}

function remove_Contract(ID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){remove_Contract_Ans(req);});
	req.doPost("modules/CXLdContracts.asp","Function=remove_Contract&ID=" + ID);
}

function remove_Contract_Ans(req)
{
	go_Report();
}

function go_Report_Ans(req)
{
	document.getElementById("report").innerHTML = req.responseText;
	document.forms[0].go.disabled = false;
}
