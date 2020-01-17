var sPath = "modules/BankReport.asp";
var div;
var currentID = 0;
var sMessage='';

function List(){
	var req = new ajaxRequest();
	req.setCallBackFunc(function(){List_Ans(req);});
	//alert(document.forms[0].status.selectedIndex);
	var status = (document.forms[0].status.selectedIndex>0)?document.forms[0].status.options[document.forms[0].status.selectedIndex].value:'';
	var exchange = (document.forms[0].exchange.selectedIndex>0)?document.forms[0].exchange.options[document.forms[0].exchange.selectedIndex].value:'';
	var unitsize = (document.forms[0].unitsize.selectedIndex>0)?document.forms[0].unitsize.options[document.forms[0].unitsize.selectedIndex].value:'';
	var usage = (document.forms[0].usage.selectedIndex>0)?document.forms[0].usage.options[document.forms[0].usage.selectedIndex].value:'';
	var yearused = (document.forms[0].yearused.selectedIndex>0)?document.forms[0].yearused.options[document.forms[0].yearused.selectedIndex].value:'';
	var deposityear = (document.forms[0].deposityear.selectedIndex>0)?document.forms[0].deposityear.options[document.forms[0].deposityear.selectedIndex].value:'';
	var season = (document.forms[0].season.selectedIndex>0)?document.forms[0].season.options[document.forms[0].season.selectedIndex].value:'';
	var unittype = (document.forms[0].unittype.selectedIndex>0)?document.forms[0].unittype.options[document.forms[0].unittype.selectedIndex].value:'';
	req.doPost(sPath, "Function=List&status=" + status + "&exchange=" + exchange + "&unitsize=" + unitsize + "&usage=" + usage + "&yearused=" + yearused + "&deposityear=" + deposityear + "&season=" + season + "&unittype=" + unittype);
	div = document.getElementById('report');
	div.innerHTML = "Loading... Please Wait";
}

function List_Ans(req){
	div.innerHTML = req.responseText;

}