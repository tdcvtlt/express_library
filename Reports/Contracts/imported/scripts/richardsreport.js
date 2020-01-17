function caller(){
	var date_begin;
	var date_end;
	
	var ajx = new ajaxRequest();
		
	date_begin = document.Main.date_start.value;
	date_end = document.Main.date_end.value;
				
	//Processing...
	if (date_begin.length == 0 || date_end.length == 0)	{
		document.getElementById("display").innerHTML = 
		"<div id='loader' class='loader'><b>Date Range Invalid... Please try again!</b></div>";
		
		document.getElementById("display").style.display = "block";		
	}
	else{				
		document.getElementById("display").innerHTML = "<div id='loader' class='loader'><b>Loading... Please wait!</b></div>";	
		document.getElementById("display").style.display = "block";
				
		ajx.setCallBackFunc(function(){
		    //display(ajx);		   
		    document.getElementById("display").innerHTML = this.responseText;
		    document.getElementById("display").style.display = "block";
		});					
		ajx.doPost('modules/richardsreport.asp', 'start=' + date_begin + '&end=' + date_end);					
	}
}

function display(ajx)
{
	var post = new Array();
	
	document.getElementById("display").innerHTML = ajx.responseText;
	document.getElementById("display").style.display = "block";
}

