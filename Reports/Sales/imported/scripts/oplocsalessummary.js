
function Call_Report()
{
		var loc;
		var vendor = '';
		var sdate;
		var edate;
		var i;
		var l;
		var ALL;
		var solid;
		var SOLS = '';
		var campaign = '';
		
		
		
		ALL="";
		loc = document.forms[0].location.options[document.forms[0].location.selectedIndex].value;
		if (loc==0)
		{
			for (i=1;i<=document.forms[0].location.options.length-1;i++)
			{
					l=document.forms[0].location.options[i].value;
					(ALL=='')?ALL+=l:ALL+=',' + l;
			}
		}
		(ALL=='')?ALL=loc:false;
		
		solid = document.forms[0].sol.options[document.forms[0].sol.selectedIndex].value;
		if (solid==0)
		{
			for (i=1;i<=document.forms[0].sol.options.length-1;i++)
			{
					if(SOLS == '')
					{
						SOLS = document.forms[0].sol.options[i].value;
					}
					else
					{
						SOLS = SOLS + ',' + document.forms[0].sol.options[i].value;
					}
			}
		}
		else
		{
			SOLS = solid;
		}

		if (document.forms[0].vendor.options[document.forms[0].vendor.selectedIndex].value == 0) {
		    for (i = 1; i < document.forms[0].vendor.options.length; i++) {
		        if (vendor == '') {
		            vendor = document.forms[0].vendor.options[i].value;
		        }
		        else {
		            vendor = vendor + ',' + document.forms[0].vendor.options[i].value;
		        }
		    }
		}
		else {
		    vendor = document.forms[0].vendor.options[document.forms[0].vendor.selectedIndex].value;
		}

        /*
		//if (document.forms[0].camp[0].checked)
		//{
		//	campaign = 'ALL';
		//}
		//else if (document.forms[0].camp[1].checked)
		//{
		//	campaign = 'MAL';
		//}
		//else if (document.forms[0].camp[2].checked)
		//{
		//	campaign = 'MAL-TS';
		}
        else if (document.forms[0].camp[3].checked) {
            campaign = 'PRESTIGE';
        } else if (document.forms[0].camp[4].checked) {
            campaign = '4K';
        } else if (document.forms[0].camp[5].checked) {
            campaign = 'EMS';
        }        
        */             		

		
		sdate = document.getElementById("input_startdate").value;
		edate = document.getElementById("input_enddate").value;
		
		var req = new ajaxRequest();
		req.setCallBackFunc(function(){Call_Report_Ans(req);});
		req.doPost('modules/opclocsalessummary.asp','loc='+ ALL + '&vendor=' + vendor + '&startdate='+ sdate + '&enddate=' + edate + '&solicitor=' + SOLS + '&campaign=' + campaign);
		document.getElementById("report").innerHTML = 'Loading, Please Wait...';
		document.getElementById("printable").disabled = true;
		document.forms[0].go.disabled = true;
		document.getElementById("initstatus").style.display = '';
}	

function Call_Report_Ans(req){
		bComplete = true;
		document.getElementById("report").innerHTML = req.responseText;
		document.getElementById("printable").disabled = false;
		document.getElementById("initstatus").style.display = 'none';
		document.forms[0].go.disabled = false;
	}