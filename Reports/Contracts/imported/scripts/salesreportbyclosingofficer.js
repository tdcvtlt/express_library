function Run_Report(){
	document.getElementById("report").style.visibility = 'visible';
	document.getElementById("choices").style.visibility = 'hidden';
	document.getElementById("options").style.visibility = 'visible';
	var req = new ajaxRequest();
	var arrPerIDs = Get_Officers();
	var arrStatus = Get_Status();
	req.setCallBackFunc(function(){Show_Report(req);});
	req.doPost("modules/salesreportbyclosingofficer.asp","Function=Run_Report&sDate=" + document.forms[0].sDate.value + "&eDate=" + document.forms[0].eDate.value + "&Status=" + arrStatus + "&PersonnelIDs=" + arrPerIDs);
	document.getElementById("report").innerHTML = "Loading... Please wait";	
}

function Show_Report(req){
	document.getElementById("report").innerHTML = req.responseText;
}

function Add_Status(){
	if (document.forms[0].status.value != ''){
		var item = new Option(document.forms[0].status.options[document.forms[0].status.selectedIndex].text, document.forms[0].status.value);
		document.forms[0].statusarray.options[document.forms[0].statusarray.length] = item;
		document.forms[0].status.options[document.forms[0].status.selectedIndex] = null;
	}
}

function Remove_Status(){
	if (document.forms[0].statusarray.selectedIndex >-1){
		var item = new Option(document.forms[0].statusarray.options[document.forms[0].statusarray.selectedIndex].text, document.forms[0].statusarray.options[document.forms[0].statusarray.selectedIndex].value);
		document.forms[0].statusarray.options[document.forms[0].statusarray.selectedIndex] = null;
		document.forms[0].status.options[document.forms[0].status.length] = item;
		Sort(document.forms[0].status);
		document.forms[0].status.selectedIndex = 0;
	}
}

function Sort(item){
	var bDone = false;
	var aItem = new Array();
	var aItemVal = new Array();
	while (!bDone){
	bDone = true;
	for (i=0;i<item.length;i++){
		if (i != item.length -1){	
			if (item.options[i].text.charAt(0) > item.options[i+1].text.charAt(0)){
				aItem[0] = item.options[i+1].text;
				aItem[1] = item.options[i].text;
				aItemVal[0] = item.options[i+1].value
				aItemVal[1] = item.options[i].value
				var nItem = new Option(aItem[0],aItemVal[0]);
				item.options[i] = nItem;
				nItem = new Option(aItem[1],aItemVal[1]);
				item.options[i+1] = nItem;
				bDone = false;
			}	
		}
	}
	}
}

function Add_Officer(){
	if (document.forms[0].officer.value != ''){
		var item = new Option(document.forms[0].officer.options[document.forms[0].officer.selectedIndex].text, document.forms[0].officer.value);
		document.forms[0].officerarray.options[document.forms[0].officerarray.length] = item;
	}
}

function Remove_Officer(){
	if (document.forms[0].officerarray.selectedIndex > -1){
		document.forms[0].officerarray.options[document.forms[0].officerarray.selectedIndex] = null;
	}
}

function Get_Officers(){
	var reply = '';
	if (document.forms[0].officerarray.length > 0){
		for (i=0;i<document.forms[0].officerarray.length; i++){
			document.forms[0].officerarray.options[i].selected = true;
			if (reply != ''){
				reply += ",'" + document.forms[0].officerarray.options[i].value + "'";
			} else {
				reply += "'" + document.forms[0].officerarray.options[i].value + "'";
			}
		}
	} 
	return reply;	
}

function Get_Status(){
	var reply = '';
	if (document.forms[0].statusarray.length > 0){
		for (i=0;i<document.forms[0].statusarray.length; i++){
			document.forms[0].statusarray.options[i].selected = true;
			if (reply!=''){
				reply += ",'" + document.forms[0].statusarray.options[i].value + "'";
			} else {
				reply += "'" + document.forms[0].statusarray.options[i].value + "'";
			}
		}
	} 
	return reply;
}