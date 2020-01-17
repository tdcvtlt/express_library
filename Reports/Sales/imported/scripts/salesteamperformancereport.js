var sModPath = "modules/salesteamperformancereport.asp";

window.attachEvent('onload',Load_me);

function Load_me(){
	window.detachEvent('onload',Load_me);
	//alert('Getting Teams');
	Get_Teams();
}

function Valid(){
	var f = document.forms[0];
	var sErr = '';
	
	sErr += (f.sdate.value=='')?'Please enter a beginning date.\n':'';
	sErr += (f.edate.value=='')?'Please enter an ending date.\n':'';
	
	(sErr=='')?null:alert(sErr);
	return (sErr=='')?true:false;

}

function Run_Report(){
	if(Valid()){
		var f = document.forms[0];
		var r = new ajaxRequest();
		r.setCallBackFunc(function(){Run_Report_Ans(r);});
		var params = 'f=runreport';
		params += '&sdate=' + f.sdate.value;
		params += '&edate=' + f.edate.value;
		params += '&grp=' + f.group.options[f.group.selectedIndex].value;
		r.doPost(sModPath,params);
		document.getElementById("report").innerHTML = "Loading... Please Wait";
		f.printable.disabled = true;
	}
}

function Run_Report_Ans(r){
	document.getElementById("report").innerHTML = r.responseText;
	document.forms[0].printable.disabled = false;
}

function Get_Teams(){
	var r = new ajaxRequest();
	r.setCallBackFunc(function(){Get_Teams_Ans(r);});
	r.doPost(sModPath,"f=Get_Teams");
}

function Get_Teams_Ans(r){
	var f = document.forms[0].group;
	var t = r.responseText.split('|&&|');
	alert(t.length);
	for (i=0;i<t.length;i++){
		var item = t[i].split('|');
		f.options[f.options.length] = new Option(item[1],item[0]);
	}
}

function Printable(){
	var w = window.open()
	w.document.write(document.getElementById("report").innerHTML);
}