
function EnterCheckPay(){
	var req = new ajaxRequest();
	req.setCallBackFunc(function() {EnterCheckPay_Ans(req);});
	var f = document.forms[0];
	var params = '';
	params += 'Function=Save';
	params += '&CheckByPhoneID=' + f.checkbyphoneid.value;
	params += '&LName=' + f.lname.value;
	params += '&MidInit=' + f.midinit.value;
	params += '&FName=' + f.fname.value;
	params += '&ContractNumber=' + f.contractnumber.value;
	params += '&RoutingNumber=' + f.routingnumber.value;
	params += '&AccountNumber=' + f.accountnumber.value;
	params += '&CheckingFlag=' + f.checking.checked;
	params += '&SavingsFlag=' + f.savings.checked;
	params += '&Amount=' + f.amount.value;
	params += '&DateToRun=' + f.datetorun.value;
	params += '&StatusID=' + f.statusid.options[f.statusid.selectedIndex].value;
	params += '&DateCompleted=' + f.datecompleted.value;
	params += '&TransactionID=' + f.transactionid.value
	req.doPost("modules/checkbyphone.asp",params)
}

function EnterCheckPay_Ans(req){	
	window.navigate("PendingCBP.asp");
}

function RunReport() {
    var req = new ajaxRequest();
	req.setCallBackFunc(function() {RunReport_Ans(req);});
	var f = document.forms[0];
	var params = '';
	params += 'Function=RunReport';
	params += '&ContractNumber=' + f.contractnumber.value;
	params += '&StatusID=' + f.statusid.options[f.statusid.selectedIndex].value;
	params += '&StartDate=' + f.startdate.value;
	params += '&EndDate=' + f.enddate.value;	
    req.doPost("modules/checkbyphone.asp", params);
	document.getElementById("report").innerHTML = "Running report test, please wait.";
}

function RunReport_Ans(req){
	document.getElementById("report").innerHTML = req.responseText;
}