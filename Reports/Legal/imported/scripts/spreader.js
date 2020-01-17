function Run_Report(){
	document.forms[0].printable.disabled = true;
	var req = new ajaxRequest();
	req.setCallBackFunc(function(){Show_Report(req);});
	req.doPost("modules/spreader.asp","f=report&exhibitnumber=" + encodeURI(document.forms[0].ExhibitNumber.options[document.forms[0].ExhibitNumber.selectedIndex].value));
	document.getElementById("report").innerHTML = "Loading... Please wait";
}

function Show_Report(req){
	document.getElementById('report').innerHTML = req.responseText;
	document.forms[0].printable.disabled = false;
}

function Load(){
	var r = new ajaxRequest();
	r.setCallBackFunc(function(){Load_Ans(r);});
	var params = "f=load";
	r.doPost("modules/spreader.asp", params);
}

function Load_Ans(r){
	var f = document.forms[0].ExhibitNumber;
	var t = r.responseText.split('||');
	if (t.length) {
		for(i=f.options.length;i>-1;i--){
			f.options[i] = null;
		}
		for(i=0;i<t.length;i++){
			var d = t[i].split('|');
			f.options[f.options.length] = new Option(d[0],d[1]);
		}
		alert(r.responsetext);
	} else {
		alert(r.responseText);
	}
}
