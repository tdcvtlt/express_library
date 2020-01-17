
function Call_Report()
{
		var name;
		var vendor;
		var sdate;
		var edate;
		var i;
		var l;
		var ALL;
		var locid;
		var LOCS = '';
		var campaign;
		//alert('hi');
		
		ALL="";
		name = document.forms[0].name.options[document.forms[0].name.selectedIndex].value;
		if (name==0)
		{
			for (i=1;i<=document.forms[0].name.options.length-1;i++)
			{
					l=document.forms[0].name.options[i].value;
					(ALL=='')?ALL+=l:ALL+=',' + l;
					//ALL+=""+l+" or "
				//alert(document.forms[0].location.options[i].value);
			}
		}
		//ALL=ALL.substring(0,ALL.length-4);
		//alert(ALL);
		(ALL=='')?ALL=name:false;
		
		locid = document.forms[0].loc.options[document.forms[0].loc.selectedIndex].value;
		//alert(locid);
		if(locid == 0)
		{
			for (i=1;i<=document.forms[0].loc.options.length-1;i++)
			{
				if(LOCS == '')
				{
					LOCS = document.forms[0].loc.options[i].value;
				}
				else
				{
					LOCS = LOCS + ',' + document.forms[0].loc.options[i].value;
				}
			}
		}
		else
		{
			LOCS = locid;
		}
		
		if (document.forms[0].camp[0].checked)
		{
			campaign = 'ALL';
		}
		else if (document.forms[0].camp[1].checked)
		{
			campaign = 'MAL';
		}
		else
		{
			campaign = 'MAL-TS';
		}

		
		vendor = document.forms[0].vendor.options[document.forms[0].vendor.selectedIndex].value;
		sdate = document.getElementById("input_startdate").value;
		edate = document.getElementById("input_enddate").value;
		
		var req = new ajaxRequest();
		req.setCallBackFunc(function(){Call_Report_Ans(req);});
		req.doPost('modules/solsalessummary.asp','name='+ ALL + '&vendor=' + vendor + '&startdate='+ sdate + '&enddate=' + edate + '&location=' + LOCS + '&campaign=' + campaign);
		document.getElementById("report").innerHTML = 'Loading, Please Wait...';
		document.getElementById("printable").disabled = true;
		document.forms[0].go.disabled = true;
}	

function Call_Report_Ans(req){
		bComplete = true;
		document.getElementById("report").innerHTML = req.responseText;
		document.getElementById("printable").disabled = false;
		document.forms[0].go.disabled = false;
	}