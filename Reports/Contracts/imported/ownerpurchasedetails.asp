<html><head><title></title>
<script language="javascript" type="text/javascript" src="../../../scripts/ajaxRequest.js"></script>

<script language="javascript" type="text/javascript">

function Call_Report(){
		var hphone;
		if (document.forms[0].phones.options.length == 0)
		{
			alert('PLease enter at least one number');
		}
		else
		{
			for (i=0; i < document.forms[0].phones.options.length;i=i+1)
			{
				if (i == 0)
				{
					hphone = document.forms[0].phones.options[i].value;
				}
				else
				{
					hphone = hphone + '|' + document.forms[0].phones.options[i].value;
				}
			}
	
			//hphone = document.getElementById("hphone").value;
			//alert(hphone);
			var req = new ajaxRequest();
			req.setCallBackFunc(function(){Call_Report_Ans(req);});
			req.doPost('modules/ownerpurchasedetail.asp','hphone='+ hphone);
			document.getElementById("report").innerHTML = 'Loading...   Please Wait,  This may take several minutes.....';
			document.forms[0].printable.disabled = true;
		}
	}	
function Call_Report_Ans(req){
		var post = new Array();
		bComplete = true;
		document.getElementById("report").innerHTML = req.responseText;
		document.getElementById("report").style.display = "block";
		document.forms[0].printable.disabled = false;
	}
	
function Add_Number()
{
	var cItem;
	var addItem;
	for (i=0; i < document.forms[0].phones.options.length;i=i+1)
	{
		if (document.forms[0].phones.options[i].value == document.forms[0].hphone.value)
		{
			alert('That phone number is already entered');
			addItem = "false"
			break;
		}
	}
	if (addItem != "false")
	{
		cItem = new Option(document.forms[0].hphone.value, document.forms[0].hphone.value);
		document.forms[0].phones.options[document.forms[0].phones.length] = cItem;
		document.forms[0].hphone.value = '';
		document.forms[0].hphone.focus();
	}
}

function Remove_Number()
{
	if (document.forms[0].phones.options.selectedIndex == -1)
	{
		alert('Select A Phone Number to Remove');
	}
	else
	{
		document.forms[0].phones.options[document.forms[0].phones.selectedIndex].removeNode();
	}
}

function Clear_Numbers()
{
	for (i=0; i < document.forms[0].phones.options.length;i=i+0)
	{
		document.forms[0].phones.options[i].removeNode();
	}	
}
</script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="tlb;default">
</head><body><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

<p align="center"><font size="6"><strong>
</strong></font><br>
</p>
<p align="center">&nbsp;</p>

</td></tr><!--msnavigation--></table><!--msnavigation--><table dir="ltr" border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td valign="top" width="1%">

</td><td valign="top" width="24"></td><!--msnavigation--><td valign="top"><h2 height="49" width="1078" align="center">&nbsp;</h2><form name="fmreport" onsubmit = "Call_Report();return false;">Please Enter A Phone Number : <input type = text id = 'hphone' name = 'hphone' VALUE = "<%=hphone%>" onSubmit = 'Add_Number();'> <input type = button name = 'add' value = 'Add Number' onclick = 'Add_Number();'> <br><select name = 'phones' size = 6 style = width:410 multiple></select> <input type = 'button' name = 'remove' value = 'Delete Number' onclick = 'Remove_Number();'>&nbsp; <br><input type = 'button' name = 'gethphone' value = 'Run Report' onclick = 'Call_Report();'> <input type="button" value="Printable Version" name="printable" onclick ="var mWin = window.open('');mWin.document.write(document.getElementById('report').innerHTML);" disabled = true> <input type = button name = 'clear' value = 'Clear Numbers' onclick = 'Clear_Numbers();'></form><div id="report"></div><!--msnavigation--></td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

</td></tr><!--msnavigation--></table></body></html>