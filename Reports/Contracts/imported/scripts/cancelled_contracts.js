function caller(){
	var date_begin;
	var date_end;		
	var ajx = new ajaxRequest();
		
	date_begin = document.Main.date_start.value;
	date_end = document.Main.date_end.value;

	if (document.Main.date_start.value.length == 0 || document.Main.date_end.value.length == 0)	{
		document.getElementById("display").innerHTML = 
		"<div id='loader' class='loader'><b>Date Range Invalid... Please try again!</b></div>";
		
		document.getElementById("display").style.display = "block";				
	}	
	else{
			
		document.getElementById("display").innerHTML = 
		"<div id='loader' class='loader'><b>Loading... Please wait!</b></div>";	
		document.getElementById("display").style.display = "block";
		
		ajx.setCallBackFunc(function(){
		    document.getElementById("display").innerHTML = ajx.responseText;
		    document.getElementById("display").style.display = "block";	
		});

		ajx.doPost("modules/cancelled_contracts.asp", "date_begin=" +
                    document.Main.date_start.value + "&date_end=" + 
                    document.Main.date_end.value);			
	}		
}


function Run_Excel()
{
	var date_begin;
	var date_end;		
			
	date_begin = document.Main.date_start.value;
	date_end = document.Main.date_end.value;
		
	
	if (date_begin.length == 0 || date_end.length == 0)
	{
		document.getElementById("display").innerHTML = 
		"<div id='loader' class='loader'><b>Date Range Invalid... Please try again!</b></div>";
		
		document.getElementById("display").style.display = "block";				
	}	
	else
	{
			
		document.getElementById("display").innerHTML = 
		"<div id='loader' class='loader'><b>Loading... Please wait!</b></div>";	
		document.getElementById("display").style.display = "block";
				
		window.open("modules/cancelled_contracts.asp?date_begin=" + date_begin + "&date_end=" + date_end);					
	}		
}