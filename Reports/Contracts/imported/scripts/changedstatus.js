function Add_Status(){
	var d = document.forms[0].stat;
	var s = document.forms[0].status;
	if (d.options[d.selectedIndex].value == 'ALL'){
		for (i=d.options.length-1;i>-1;i--){
			if (d.options[i].value != 'ALL'){
				s.options[s.options.length] = new Option(d.options[i].text, d.options[i].value);
				d.options[i] = null;
			}
		}
	} else {
		s.options[s.options.length] = new Option(d.options[d.selectedIndex].text, d.options[d.selectedIndex].value);
		d.options[d.selectedIndex] = null;
	}
}

function Remove_Status(){
	var d = document.forms[0].stat;
	var s = document.forms[0].status;
	d.options[d.options.length] = new Option(s.options[s.selectedIndex].text,s.options[s.selectedIndex].value);
	s.options[s.selectedIndex] = null;
}

function Remove_All(){
	var d = document.forms[0].status;
	var s = document.forms[0].stat;
	for (i=d.options.length-1;i>-1;i--){
		if (d.options[i].value != 'ALL'){
			s.options[s.options.length] = new Option(d.options[i].text, d.options[i].value);
			d.options[i] = null;
		}
	}

}

function Call_Report()
{
	var startdate;
	var enddate;
	var params = '';
	var status;
	if (document.forms[0].startdate.value == '')
	{
		alert('Please Enter a Start Date');
	}
	else if (document.forms[0].enddate.value == '')
	{
		alert('Please Enter an End Date');
	}
	else if (document.forms[0].status.options.length == 0)
	{
		alert('Please Select At Least One Status');
	}
	else
	{
		var req = new ajaxRequest;
		startdate = document.forms[0].startdate.value;
		enddate = document.forms[0].enddate.value;
		params += 'startdate=' + startdate;
		params += '&enddate=' + enddate;
		for (i=0; i < document.forms[0].status.options.length;i=i+1)
		{
			if (i == 0)
			{
				status = "'" + document.forms[0].status.options[i].value + "'";
			}
			else
			{
				status = status + ",'" + document.forms[0].status.options[i].value + "'";
			}
		}
		//alert(status);
		params += '&status=' + status;
		req.setCallBackFunc(function(){Call_Report_Ans(req);});
		req.doPost('modules/changedstatus.asp',params);
		document.getElementById("report").innerHTML = 'Loading, Please Wait...';
		document.getElementById("printable").disabled = true;
		document.forms[0].run.disabled = true;
	}
}

function Call_Report_Ans(req)
{
		document.getElementById("report").innerHTML = req.responseText;
		document.getElementById("printable").disabled = false;
		document.forms[0].run.disabled = false;

}	
