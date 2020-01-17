function Get_Report(){
	var req = new ajaxRequest();
	req.setCallBackFunc(function(){Get_Report_Ans(req);});
	req.doPost("modules/salesinventory.asp", "");
}

function Get_Report_Ans(req){
	document.getElementById('report').innerHTML = req.responseText;
}