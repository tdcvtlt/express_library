
function Call_Report(){
	if (Valid()){
		var req = new ajaxRequest();
		req.setCallBackFunc(function(){Call_Report_Ans(req);});
		var params = "Function=Report";
		params += "&Rep=" + encodeURI(document.forms[0].rep.options[document.forms[0].rep.selectedIndex].value);
		params += "&Loc=" + encodeURI(document.forms[0].loc.options[document.forms[0].loc.selectedIndex].value);
		params += "&sDate=" + document.forms[0].sdate.value;
		params += "&eDate=" + document.forms[0].edate.value;
		params += "&ShowIndividuals=" + document.forms[0].ind.checked;
		params += "&ShowRecords=" + document.forms[0].rec.checked;
		params += "&Title=" + document.forms[0].title.options[document.forms[0].title.selectedIndex].value;
		params += "&Range=" + document.forms[0].range.options[document.forms[0].range.selectedIndex].value;
		params += "&Status=" + document.forms[0].status.options[document.forms[0].status.selectedIndex].value;
		req.doPost("modules/pendercontract.asp",params);
		document.getElementById("report").innerHTML = "Loading... Please Wait";
		document.getElementById("printable").disabled = true;
	}
}

function Call_Report_Ans(req){
	document.getElementById("report").innerHTML = req.responseText;
	req = null;
	document.getElementById("printable").disabled = false;
}

function Valid(){
	(document.forms[0].rep.options[document.forms[0].rep.selectedIndex].value >0)?document.forms[0].ind.checked=true:null;
	var sErr = '';
	if (document.forms[0].sdate.value == ''){
		sErr = 'Please enter a start date.\n';
	} else if (document.forms[0].edate.value == ''){
		sErr = 'Please enter an ending date.\n';
	}
	if (sErr != ''){
		alert(sErr);
		return false;
	} else {
		return true;
	}
}

function Get_Personnel(){
	var req = new ajaxRequest();
	req.setCallBackFunc(function(){Get_Personnel_Ans(req);});
	var params = "Function=Get_Personnel";
	params += "&sDate=" + document.forms[0].sdate.value;
	params += "&eDate=" + document.forms[0].edate.value;
	params += "&Title=" + document.forms[0].title.options[document.forms[0].title.selectedIndex].value;
	req.doPost("modules/pendercontract.asp",params);
}

function Get_Personnel_Ans(req){
	Clear_Personnel();
	//alert(req.responseText);
	var tmp = req.responseText.split("||");
	if (tmp[0]=="OK"){
		for (i=1;i<tmp.length;i++){
			var det = tmp[i].split('|');
			var opt = new Option(det[1],det[0]);
			document.forms[0].rep.options[document.forms[0].rep.options.length]=opt;
		}	
	} else {
		alert(tmp[1]);
	}
	req = null;
}

function Clear_Personnel(){
	for (i=1;i<document.forms[0].rep.options.length;i++){
		document.forms[0].rep.options[i] = null;
	}
}

function Show_Others(status,values,div){
	//var tmp = status.split('|');
	//var tmp2 = values.split('|');
	//var message = "";
	//for(i=0;i<tmp.length;i++){
	//	(tmp2[i]=='')?null:message+=tmp[i] + ' : ' + tmp2[i] + '\n';
	//}
	if (document.getElementById(div).style.display==''){
		document.getElementById(div).style.display='none';	
	}else{
		document.getElementById(div).style.display='';	
	}
}