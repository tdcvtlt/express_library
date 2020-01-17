<html><head><title>Today's Sales</title>
<script language="javascript" src="../../../scripts/ajaxRequest.js"></script>
<script type="text/javascript" language="javascript"  src="../../../scripts/scw.js"></script>
<script>

function Call_Report(){
		var tmp;
		var sdate;
		var edate;
		sdate = document.getElementById("StartDate").value;
		edate = document.getElementById("EndDate").value;
		/*
			alert(sdate);
			alert(edate);
		*/
		var req = new ajaxRequest();
		req.setCallBackFunc(function(){Call_Report_Ans(req);});
		req.doPost('modules/todaysales1.asp','sdate='+ sdate + '&edate=' + edate + '&location=' + document.forms[0].location.options[document.forms[0].location.selectedIndex].value);
		document.getElementById("report").innerHTML = 'Loading...   Please Wait,  This may take several minutes.....';
		

	}	
function Call_Report_Ans(req){
		bComplete = true;
		document.getElementById("report").innerHTML = req.responseText;
	}
	
</script>
</head><body>
    <form name="Date" method="POST" action="todayssales.asp" webbot-action="--WEBBOT-SELF--">
        <table><tr><td>Location:</td><td><select size="1" name="location"><option selected value="KCP">KCP</option><option value ="Outfield">Outfield</option><option value="Richmond">Richmond</option><option value="Woodbridge">Woodbridge</option></select></td></tr><tr><td>Start Date: </td><td><input type = 'text' onclick = "scwShow(this,this);" id = 'StartDate' name = 'startdate' Readonly value="<%=request("startdate")%>" size="20"> </td></tr><tr><td>End Date:</td><td><input type = 'text' onclick = "scwShow(this,this);" id = 'EndDate' name = 'enddate' value="<%=request("enddate")%>" size="20" readonly ></td></tr><tr><td colspan=2><input type = 'button' name = 'go' value = 'Run Report' onclick = "Call_Report()" ;><input type="button" value="Printable Version" name="B1" onclick ="var mWin = window.open(); mWin.document.write(document.getElementById('report').innerHTML);"></td></tr></table></form><div id="report"></div></body></html>