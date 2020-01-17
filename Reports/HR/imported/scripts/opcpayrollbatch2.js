var sPath = 'modules/opcpayrollbatch2.asp';

function Get_Report(){
	var r = new ajaxRequest();
	var p = '';
	p += 'function=GetPayrollPersonnel';
	p += '&bID=' + encodeURI(document.forms[0].bID.value);
	p += '&Detail=' + document.forms[0].Detail.checked;
	r.setCallBackFunc(function(){ Get_Report_Ans(r);});
	r.doPost(sPath, p);
	document.getElementById("results").innerHTML = 'Loading, Please Wait...';
	document.getElementById("printable").disabled = true;
	document.forms[0].report.disabled = true	
}
function printable_version()
{
	//alert('hi');
	var mwin = window.open ('modules/opcpayrollbatch2.asp?function=GetPayrollPersonnel&bID=' + encodeURI(document.forms[0].bID.value) + '&Detail=' + document.forms[0].Detail.checked);
}

function Get_Report_Ans(r){
	alert(r.responseText);
	document.getElementById('results').innerHTML = r.responseText;
	document.getElementById("printable").disabled = false;
	document.forms[0].report.disabled = false;
}