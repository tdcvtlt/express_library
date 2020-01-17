function add_NSStatus()
{
	var cItem;
	if (document.forms[0].nostat.length != 0)
	{
		if (document.forms[0].nostat.value == 'ALL')
		{
			for (i=document.forms[0].nostat.length - 1;i>0;i--)
			{
				cItem = new Option (document.forms[0].nostat.options[i].text, document.forms[0].nostat.options[i].value);
				document.forms[0].nostatus.options[document.forms[0].nostatus.length] = cItem;
				document.forms[0].nostat.options[i].removeNode();
			}			
			document.forms[0].nostat.options[0].removeNode();
		}
		else
		{
			cItem = new Option (document.forms[0].nostat.options[document.forms[0].nostat.selectedIndex].text, document.forms[0].nostat.value);
			document.forms[0].nostatus.options[document.forms[0].nostatus.length] = cItem;
			document.forms[0].nostat.options[document.forms[0].nostat.selectedIndex].removeNode();
		}
	}
}

function remove_NSStatus()
{
	if (document.forms[0].nostatus.options.selectedIndex == -1)
	{
		alert('Select a Status to Remove');
	}
	else
	{
		var cItem;
		cItem = new Option (document.forms[0].nostatus.options[document.forms[0].nostatus.selectedIndex].text, document.forms[0].nostatus.value);
		document.forms[0].nostat.options[document.forms[0].nostat.length] = cItem;
		document.forms[0].nostatus.options[document.forms[0].nostatus.selectedIndex].removeNode();
	}
}

function add_NQStatus()
{
	var cItem;
	if (document.forms[0].nqstat.length != 0)
	{
		if (document.forms[0].nqstat.value == 'ALL')
		{
			for (i=document.forms[0].nqstat.length - 1;i>0;i--)
			{
				cItem = new Option (document.forms[0].nqstat.options[i].text, document.forms[0].nqstat.options[i].value);
				document.forms[0].nqstatus.options[document.forms[0].nqstatus.length] = cItem;
				document.forms[0].nqstat.options[i].removeNode();
			}			
			document.forms[0].nqstat.options[0].removeNode();
		}
		else
		{
			cItem = new Option (document.forms[0].nqstat.options[document.forms[0].nqstat.selectedIndex].text, document.forms[0].nqstat.value);
			document.forms[0].nqstatus.options[document.forms[0].nqstatus.length] = cItem;
			document.forms[0].nqstat.options[document.forms[0].nqstat.selectedIndex].removeNode();
		}
	}
}

function remove_NQStatus()
{
	if (document.forms[0].nqstatus.options.selectedIndex == -1)
	{
		alert('Select a Status to Remove');
	}
	else
	{
		var cItem;
		cItem = new Option (document.forms[0].nqstatus.options[document.forms[0].nqstatus.selectedIndex].text, document.forms[0].nqstatus.value);
		document.forms[0].nqstat.options[document.forms[0].nqstat.length] = cItem;
		document.forms[0].nqstatus.options[document.forms[0].nqstatus.selectedIndex].removeNode();
	}
}

function add_CanStatus()
{
	var cItem;
	if (document.forms[0].canstat.length != 0)
	{
		if (document.forms[0].canstat.value == 'ALL')
		{
			for (i=document.forms[0].canstat.length - 1;i>0;i--)
			{
				cItem = new Option (document.forms[0].canstat.options[i].text, document.forms[0].canstat.options[i].value);
				document.forms[0].canstatus.options[document.forms[0].canstatus.length] = cItem;
				document.forms[0].canstat.options[i].removeNode();
			}			
			document.forms[0].canstat.options[0].removeNode();
		}
		else
		{
			cItem = new Option (document.forms[0].canstat.options[document.forms[0].canstat.selectedIndex].text, document.forms[0].canstat.value);
			document.forms[0].canstatus.options[document.forms[0].canstatus.length] = cItem;
			document.forms[0].canstat.options[document.forms[0].canstat.selectedIndex].removeNode();
		}
	}
}

function remove_CanStatus()
{
	if (document.forms[0].canstatus.options.selectedIndex == -1)
	{
		alert('Select a Status to Remove');
	}
	else
	{
		var cItem;
		cItem = new Option (document.forms[0].canstatus.options[document.forms[0].canstatus.selectedIndex].text, document.forms[0].canstatus.value);
		document.forms[0].canstat.options[document.forms[0].canstat.length] = cItem;
		document.forms[0].canstatus.options[document.forms[0].canstatus.selectedIndex].removeNode();
	}
}

function add_QualStatus()
{
	var cItem;
	if (document.forms[0].qualstat.length != 0)
	{
		if (document.forms[0].qualstat.value == 'ALL')
		{
			for (i=document.forms[0].qualstat.length - 1;i>0;i--)
			{
				cItem = new Option (document.forms[0].qualstat.options[i].text, document.forms[0].qualstat.options[i].value);
				document.forms[0].qualstatus.options[document.forms[0].qualstatus.length] = cItem;
				document.forms[0].qualstat.options[i].removeNode();
			}			
			document.forms[0].qualstat.options[0].removeNode();
		}
		else
		{
			cItem = new Option (document.forms[0].qualstat.options[document.forms[0].qualstat.selectedIndex].text, document.forms[0].qualstat.value);
			document.forms[0].qualstatus.options[document.forms[0].qualstatus.length] = cItem;
			document.forms[0].qualstat.options[document.forms[0].qualstat.selectedIndex].removeNode();
		}
	}
}

function remove_QualStatus()
{
	if (document.forms[0].qualstatus.options.selectedIndex == -1)
	{
		alert('Select a Status to Remove');
	}
	else
	{
		var cItem;
		cItem = new Option (document.forms[0].qualstatus.options[document.forms[0].qualstatus.selectedIndex].text, document.forms[0].qualstatus.value);
		document.forms[0].qualstat.options[document.forms[0].qualstat.length] = cItem;
		document.forms[0].qualstatus.options[document.forms[0].qualstatus.selectedIndex].removeNode();
	}
}

function Get_QualStatuses(){
	var s = document.forms[0].qualstatus;
	var ans = '0';
	for (i=0;i<s.options.length;i++){
		ans += ',' + s.options[i].value;
	}
	return ans;
}

function Get_NSStatuses(){
	var s = document.forms[0].nostatus;
	var ans = '0';
	for (i=0;i<s.options.length;i++){
		ans += ',' + s.options[i].value;
	}
	return ans;
}

function Get_NQStatuses(){
	var s = document.forms[0].nqstatus;
	var ans = '0';
	for (i=0;i<s.options.length;i++){
		ans += ',' + s.options[i].value;
	}
	return ans;
}

function Get_CanStatuses(){
	var s = document.forms[0].canstatus;
	var ans = '0';
	for (i=0;i<s.options.length;i++){
		ans += ',' + s.options[i].value;
	}
	return ans;
}

function Get_report(){
	if (document.getElementById('sdate').value == '')
	{
		alert('Please Enter a Start Date');
	}
	else if (document.getElementById('edate').value == '')
	{
		alert('Please Enter an End Date');
	}
	else if (document.forms[0].nqstatus.options.length == 0)
	{
		alert('Please Select At Least One NQ Status');
	}
	else if (document.forms[0].nostatus.options.length == 0)
	{
		alert('Please Select At Least One No Show Status');
	}
	else if (document.forms[0].canstatus.options.length == 0)
	{
		alert('Please Select At Least One Cancelled Status');
	}
	else if (document.forms[0].qualstatus.options.length == 0)
	{
		alert('Please Select At Least One Qualified Status');
	}
	else
	{
		var r = new ajaxRequest();
		r.setCallBackFunc(function(){Get_report_ans(r);});
		var p = 'function=Get_Report';
		p += '&sdate=' + document.getElementById('sdate').value;
		p += '&edate=' + document.getElementById('edate').value;
		p += '&nqstatus=' + Get_NQStatuses();
		p += '&nsstatus=' + Get_NSStatuses();
		p += '&canstatus=' + Get_CanStatuses();
		p += '&qualstatus=' + Get_QualStatuses();
		r.doPost('modules/ToursByState.asp',p);
		document.getElementById("initstatus").style.display = '';
		document.getElementById("report").style.display = 'none';
	}
}

function Get_report_ans(r){
	document.getElementById("report").style.display = '';
	document.getElementById('report').innerHTML = r.responseText;
	document.getElementById("initstatus").style.display = 'none';
}


function Report_Excel()
{
	if (document.getElementById('sdate').value == '')
	{
		alert('Please Enter a Start Date');
	}
	else if (document.getElementById('edate').value == '')
	{
		alert('Please Enter an End Date');
	}
	else if (document.forms[0].nostatus.length == 0)
	{
		alert('Please Select At Least One Size');
	}
	else if (document.forms[0].nqstatus.length == 0)
	{
		alert('Please Select At Least One Unit Type');
	}
	else if (document.forms[0].canstatus.options.length == 0)
	{
		alert('Please Select At Least One Category');
	}
	else if (document.forms[0].qualstatus.options.length == 0)
	{
		alert('Please Select At Least One Usage Year');
	}
	else
	{
		var params = 'function=Get_Report';
		params += '&sdate=' + document.getElementById('sdate').value;
		params += '&edate=' + document.getElementById('edate').value;
		params += '&nqstatus=' + Get_NQStatuses();
		params += '&nsstatus=' + Get_NSStatuses();
		params += '&canstatus=' + Get_CanStatuses();
		params += '&qualstatus=' + Get_QualStatuses();
		window.navigate('modules/ToursByState.asp?' + params);
	}
}