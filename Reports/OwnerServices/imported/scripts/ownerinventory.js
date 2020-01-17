var sModulePath = 'modules/ownerinventory.asp';
var sMessage = '';

function Send(){
	var req = new ajaxRequest();
	req.setCallBackFunc(function(){Send_Ans(req);});
	var params = '';
	var f = document.forms[0];
	params += 'Function=List';
	params += '&occupancyyear=' + f.occupancyyear.options[f.occupancyyear.selectedIndex].value;
	params += '&season=' + f.season.options[f.season.selectedIndex].value;
	params += '&unittype=' + f.unittype.options[f.unittype.selectedIndex].value;
	params += '&frequency=' + f.frequency.options[f.frequency.selectedIndex].value;
	req.doPost(sModulePath,params);
	document.getElementById('display').innerHTML = 'Running report .. Please wait';
}

function Send_Ans(req){
	document.getElementById('display').innerHTML = req.responseText
}