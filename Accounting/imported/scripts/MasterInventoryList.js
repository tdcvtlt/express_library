function run_Report() {
    
	var filters;
	if (document.forms.filtersearch.filter1.value == '0')
	{
		filters = "";
	}
	else
	{
		filters = document.forms.filtersearch.filter1.value + '|' + document.forms.filtersearch.filter2.value + '|' + document.forms.filtersearch.filter3.value + '|' + document.forms.filtersearch.filter4.value;
	}
	var req = new ajaxRequest;
	document.forms.filtersearch.printRpt.disabled = true;
	req.setCallBackFunc(function () {
	    run_Report_Ans(req);	    
	});
	req.doPost("modules/MasterInventoryList.asp","Function=Run_Report&filter=" + filters);
	document.getElementById("report").innerHTML = "Searching Inventory. This May Take Several Minutes...<br><img src = '../images/progressbar.gif'>";	
}

function run_Report_Ans(req) {    

    if (window.location.toString().indexOf('action=pm') != -1) {
       
        var t = $(req.responseText);

        $('thead tr th:nth-child(3)', $(t)).hide();
        $('tbody tr td:nth-child(3)', $(t)).hide();
        $('thead tr th:nth-child(4)', $(t)).hide();
        $('tbody tr td:nth-child(4)', $(t)).hide();
        $('thead tr th:nth-child(5)', $(t)).hide();
        $('tbody tr td:nth-child(5)', $(t)).hide();
        $('thead tr th:nth-child(6)', $(t)).hide();
        $('tbody tr td:nth-child(6)', $(t)).hide();
        $('thead tr th:nth-child(7)', $(t)).hide();
        $('tbody tr td:nth-child(7)', $(t)).hide();
        $('thead tr th:nth-child(8)', $(t)).hide();
        $('tbody tr td:nth-child(8)', $(t)).hide();
        
        $('tbody tr td:nth-child(1)', $(t)).html(function () { return '<a href=#>' + $(this).text() + '</a>' });

        $('tbody tr td:nth-child(1)', $(t)).bind('click', function (e) {
            var tb = window.opener.document.getElementById('item_great');
            $(tb).val($(this).text());               
            window.close();
        });

        document.getElementById('report').innerHTML = '';
        $(document.getElementById('report')).append($(t));
        
    } else {
        document.getElementById('report').innerHTML = req.responseText;      
    }

    document.forms.filtersearch.goE.disabled = false;
    document.forms.filtersearch.goE.value = 'Excel';
    document.forms.filtersearch.printRpt.disabled = false;

}


function run_Report_Excel()
{
    //var x = document.getElementById('report').innerHTML;
    document.forms.filtersearch.goE.disabled = true;
    document.forms.filtersearch.goE.value = 'Preparing Spreadsheet';
    var sURL = "modules/MasterInventoryList.asp?Function=Run_Report&filter=";
	var filters;
	if (document.forms.filtersearch.filter1.value == '0')
	{
		filters = "";
	}
	else
	{
		filters = document.forms.filtersearch.filter1.value + '|' + document.forms.filtersearch.filter2.value + '|' + document.forms.filtersearch.filter3.value + '|' + document.forms.filtersearch.filter4.value;
	}
	sURL += filters;
	var mWin = window.navigate(sURL);
	//document.getElementById("report").innerHTML = "Preparing Spreadsheet. This May Take Several Minutes...<br><img src = '../images/progressbar.gif'>";
	
}