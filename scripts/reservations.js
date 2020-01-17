var ReservationsLoaded = false;
var sSort = 'Number';

function Get_ProspectReservations(ID, reload){ //used on edit prospect
	if (!ReservationsLoaded || reload) {
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){Loading_ProsReservationsList(req);});
		req.doPost("modules/Reservations.asp","Function=ListPros&ProspectID=" + ID);
		document.getElementById("reservations").innerHTML = "Loading ...";
		ReservationsLoaded = true;
	}
}


function Loading_ProsReservationsList(req){ //used on edit prospect
	document.getElementById("reservations").innerHTML = req.responseText;
}

function Get_PackageReservations(ID, reload){ //used on edit package
	if (!ReservationsLoaded || reload) {
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){Loading_PackageReservationsList(req);});
		req.doPost("modules/Reservations.asp","Function=ListPackageIssued&PackageIssuedID=" + ID);
		document.getElementById("reservations").innerHTML = "Loading ...";
		ReservationsLoaded = true;
	}
}

function Loading_PackageReservationsList(req){ //used on edit package
	document.getElementById("reservations").innerHTML = req.responseText;
}

function Get_Reservation(ID,ProsID,PkgID,ConID,GroupID){
	if (ID == '')
	{
		location.href = 'reservations.htm';
	}
	else
	{
		var req = new ajaxRequest;
		//if (Field == '') {Field = 'na'};
		req.setCallBackFunc(function(){Loading_Reservation(req);});
		req.doPost("modules/Reservations.asp","Function=Get_Reservation&ReservationID=" + ID + "&ProspectID=" + ProsID + "&PackageIssuedID=" + PkgID + "&ContractID=" + ConID + "&GroupID=" + GroupID);
		document.getElementById("reservationstab").innerHTML = "<BR><BR><BR><BR><BR><BR>Requesting Information From Server...Please Wait";
		sID = ID;
	}
}

function Loading_Reservation(req){
	document.getElementById("reservationstab").innerHTML = req.responseText;
	//dlcalendar_start();
}

function Get_UsageReservations(usageID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Get_UsageReservations_Ans(req);});
	req.doPost("../modules/Reservations.asp","Function=Get_Usage_Reservations&UsageID=" + usageID);
	document.getElementById("reservations").innerHTML = 'Requesting information from the server.. Please wait';
}	
function Get_UsageReservations_Ans(req)
{
	document.getElementById("reservations").innerHTML = req.responseText;
}
function Get_ResOwner(ID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Loading_Owner(req);});
	req.doPost("modules/Reservations.asp","Function=GetReservationOwner&ReservationID=" + ID);
	document.getElementById("owner").innerHTML = 'Requesting information from the server.. Please wait';
}

function Loading_Owner(req){ //Used on Edit Package Issued.asp
	document.getElementById("owner").innerHTML = req.responseText;
}


function Filtering(){ //Filter for Reservations.htm
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Show_Reservations(req);});
	req.doPost("modules/Reservations.asp","Function=List&Sort=" + sSort + "&Filter=" + document.all.filtertext.value);
	document.getElementById('reservations').innerHTML = "Loading..."
}

function Get_Reservations(sort){ //Used on Reservations.htm 
	document.getElementById('reservations').innerHTML = "Queries are limited to 50 records"
	document.all.filtertext.value = '';
	sSort = sort;
	if(sort=='Number'){
		document.getElementById('filter').innerHTML = 'Enter Reservation Number:';
	} else if (sort=='ID') {
		document.getElementById('filter').innerHTML = 'Enter Reservation ID:';
	} else if (sort=='Guest') {
		document.getElementById('filter').innerHTML = 'Enter Guest Name:';
	} else if (sort=='Owner') {
		document.getElementById('filter').innerHTML = 'Enter Owner Name:';
	}
	document.all.filtertext.focus();
}

function Show_Reservations(req){ //Used on Reservations.htm 
	var res = document.getElementById("reservations");
	res.innerHTML = req.responseText
}

function val_ResLoc(resID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){val_ResLocAns(req, resID);});
	req.doPost("../modules/Reservations.asp","Function=Val_ResLoc&ID=" + resID);
}

function val_ResLocAns(req, ID)
{
	if (req.responseText == "valid")
	{
		popitup2('roomwizard.asp?reservationid=' + ID);
	}
	else if (req.responseText == "invalid")
	{
		alert('Reservation Location Must Be KCP In Order to Add Room!');
	}
	else
	{
		alert(req.responseText);
	}
}

function Get_ReservationFolios(ID)
{

	var req = new ajaxRequest;
	req.setCallBackFunc(function(){show_Res_Folios(req);});
	req.doPost("modules/Folios.asp","Function=Get_Folios&ID=" + ID);
	document.getElementById("folios").innerHTML = "Loading...";
}

function show_Res_Folios(req)
{
	document.getElementById("folios").innerHTML = req.responseText;
}

function get_Res_Rooms(ID)
{
 	var req = new ajaxRequest;
	req.setCallBackFunc(function(){show_Res_Rooms(req);});
	req.doPost("../modules/Reservations.asp","Function=Get_Rooms&ID=" + ID);
//	document.getElementById("rooms").innerHTML = "Loading..."
}

function show_Res_Rooms(req)
{
	document.getElementById("rooms").innerHTML = req.responseText;
}

function remove_Room(resID, roomID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){remove_Room_Ans(req, resID);});
	req.doPost("../modules/Reservations.asp","Function=Remove_Room&ID=" + resID + "&RoomID=" + roomID);
}

function remove_Room_Ans(req, ID)
{
	if (req.responseText == "OK")
	{
		get_Res_Rooms(ID);
	}
	else
	{
		alert(req.responseText);
	}
}

function Interface_Check_In(ID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Interface_Check_In_Ans(req, ID);});
	req.doPost("modules/Reservations.asp","Function=CheckIn&reservationid=" + ID);
}

function Interface_Check_In_Ans(req)
{
	if (req.responseText != 'OK')
	{
		alert(req.responseText);
	}
}
function Check_In(ID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Check_In_Ans(req, ID);});
	req.doPost("../modules/Reservations.asp","Function=CheckIn&reservationid=" + ID);
}

function Val_Check_Out(ID, fName, lName, outDate, curDate)
{
	var response;
	if (Date.parse(outDate) > Date.parse(curDate))
	{
		response = confirm("This Reservation is Not Scheduled to Check Out Until " + outDate + ". \n Do you wish to Check Out ReservationID " + ID + " - " + fName + " " + lName + "?");	
	}
	else
	{
		response = confirm("Do you wish to Check Out ReservationID " + ID + " - " + fName + " " + lName + "?");	
	}
	if (response == true)
	{
		Check_Out(ID);
	}
}

function Val_Room_CheckIn(ID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Val_Room_CheckIn_Ans(req, ID);});
	req.doPost("modules/Reservations.asp","Function=Val_Res_CheckIn&ReservationID=" + ID);
}
function Val_Room_CheckIn_Ans(req, ID)
{
	if (req.responseText == 'OK')
	{
		popitup('storecreditcard.asp?reservationid=' + ID);
	}
	else
	{
		alert(req.responseText);
	}
}

function Check_In_Ans(req, ID)
{
	if (req.responseText != 'OK')
	{
		alert(req.responseText);
	}
	else
	{	
		//document.getElementById("ChkOutBtn").disabled = false;
		//document.getElementById("ChkInBtn").disabled = true;
		//document.getElementById("IFCheckIn").style.visibility = "visible";
		//if (document.forms[0].TypeID.options[document.forms[0].TypeID.selectedIndex].text == 'Rental' || document.form[0].TypeID.options[document.forms[0].TypeID.selectedIndex].text == 'Marketing')
		//{
	    //	document.getElementById("ExtendBtn").style.visibility = "visible";
		//}
		//Get_Reservation(ID, '0', '0', '0', '0');
	    Refresh_Res();
	}
}

function Check_Out(ID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Check_Out_Ans(req, ID);});
	req.doPost("modules/Reservations.asp","Function=CheckOut&reservationid=" + ID);
}

function Check_Out_Ans(req, ID)
{
	if (req.responseText == 'OK')
	{
		document.getElementById("ChkInBtn").style.visibility = "hidden";
		document.getElementById("ChkOutBtn").style.visibility = "hidden";
		document.getElementById("IFCheckIn").style.visibility = "hidden";
		document.getElementById("ExtendBtn").style.visibility = "hidden";
		Get_Reservation(ID, '0', '0', '0', '0');
	}
	else if (req.responseText == 'Taxes')
	{
		alert('Taxes Have Been Assessed and Need to Be Collected.');
		document.getElementById("ChkInBtn").style.visibility = "hidden";
		document.getElementById("ChkOutBtn").style.visibility = "hidden";
		document.getElementById("IFCheckIn").style.visibility = "hidden";
		document.getElementById("ExtendBtn").style.visibility = "hidden";
		Get_Reservation(ID, '0', '0', '0', '0');	
	}
	else
	{
		alert(req.responseText);
	}
}

function Get_Waitlist()
{
	if (document.forms[0].sDate.value == "")
	{
		alert('Please Select a Start Date');
	}
	else if (document.forms[0].eDate.value == "")
	{
		alert('Please Select an End Date');
	}
	else
	{
		var sort;
		sort = "hi";
		if (document.forms[0].stay[0].checked)
		{
			sort = "owner";
		}
		else
		{
			sort = "getaway";
		}
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){Get_Waitlist_Ans(req);});
		req.doPost("modules/Reservations.asp","Function=Get_Waitlist&sDate=" + document.forms[0].sDate.value + "&eDate=" + document.forms[0].eDate.value + "&UnitType=" + document.forms[0].unittype.value + "&BD=" + document.forms[0].BD.value + "&sort=" + sort);
		document.getElementById("waitlist").innerHTML = "Loading...";
	}
}

function Get_Waitlist_Ans(req)
{
	document.getElementById("waitlist").innerHTML = req.responseText;
}

function Add_Waitlist_Guest(ID, name)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Get_Pros_Contracts_Ans(req, ID, name);});
	req.doPost("modules/contracts.asp","Function=ListProsContracts&ProspectID=" + ID);
}

function Get_Pros_Contracts_Ans(req, ID, name)
{
	document.getElementById("prospects").innerHTML = "<br><table><tr><td>" + name + "</td></tr><tr><td colspan = '2'>" +  req.responseText + "</td></tr></table>" +  "<input type = hidden name = prospectid value = " + ID + ">" +  document.getElementById("selection").innerHTML;
	//window.resizeTo(460,286);
}

function Add_To_Waitlist()
{
	if (document.getElementById("season").value == '0')
	{
		alert('Please Select a Season');
	}
	else
	{
		var sort;
		sort = "hi";
		if (document.forms[0].stay[0].checked)
		{
			sort = "owner";
		}
		else
		{
			sort = "getaway";
		}
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){Add_To_Waitlist_Ans(req);});
		req.doPost("modules/Reservations.asp","Function=Add_To_Waitlist&ProspectID=" + document.getElementById("prospectid").value + "&ContractID=" + document.getElementById("owncontracts").value + "&sDate=" + document.getElementById("sDate").value + "&eDate=" + document.getElementById("eDate").value + "&sort=" + sort + "&unittype=" + document.getElementById("unittype").value + "&bedrooms=" + document.getElementById("BD").value + "&season=" + document.getElementById("season").value);	
	}
}

function Add_To_Waitlist_Ans(req)
{
	if(req.responseText != "OK")
	{
		alert(req.responseText);
	}
	else
	{
		window.close();
	}
}

function Remove_From_Waitlist(ID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Remove_From_Waitlist_Ans(req);});
	req.doPost("modules/Reservations.asp","Function=Remove_From_WaitList&ID=" + ID);
}	

function Remove_From_Waitlist_Ans(req)
{
	if (req.responseText == "OK")
	{
		Get_Waitlist();
	}
	else
	{
		alert(req.responseText);
	}
}

function Waitlist_Pros_Select()
{
	if (document.forms[0].stay[0].checked)
	{
		popitup2('addWaitList.asp?List=Owner');
	}
	else
	{
		popitup2('addWaitList.asp?List=Getaway');
	}
}

function Get_CheckOut_Dates()
{
	if (document.getElementById("TotalNights").value == 0)
	{
		days = 7;
		document.getElementById("TotalNights").value = 7;
	}
	else
	{
		days = document.getElementById("TotalNights").value;
	}
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Get_CheckOut_Date_Ans(req);});
	req.doPost("modules/Reservations.asp","Function=Get_Out_Date&InDate=" + document.forms[0].CheckInDate.value + "&Nights=" + days);
}

function Change_Out_Date()
{
	if (document.forms[0].CheckInDate.value != "")
	{
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){Get_CheckOut_Date_Ans(req);});
		req.doPost("modules/Reservations.asp","Function=Get_Out_Date&InDate=" + document.forms[0].CheckInDate.value + "&Nights=" + document.forms[0].TotalNights.value);
	}
}
function Get_CheckOut_Date_Ans(req)
{
	document.forms[0].CheckOutDate.value = req.responseText;
}

function Get_Res_Balance(resID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Get_Res_Balance_Ans(req);});
	req.doPost("modules/financials.asp","Function=Get_Res_Balance&ResID=" + resID);
}

function Get_Res_Balance_Ans(req) 
{
	document.getElementById("resBalance").innerHTML = req.responseText;
}

function Check_Status()
{
	//alert(document.forms[0].StatusID.value);
	var r = new ajaxRequest();
	r.setCallBackFunc(function(){Check_Status_Ans(r);});
	r.doPost("modules/Reservations.asp","Function=StatusChecker&stat=" + document.forms[0].StatusID.value + "&resid=" + document.forms[0].reservationid.value);
}

function Check_Status_Ans(r)
{
	var ansArr = r.responseText;
	var statArr = ansArr.split("|")
	if (statArr[0] == "Cancelled")
	{
		if (statArr[1] == "Room")
		{
			input_box=confirm("The Room(s) must be removed From the reservation before changing to Cancelled" + "\nAre you sure you want to continue?");
			if (input_box==true)
			{ 
				location.href = 'editreservation.asp?reservationid=' + document.forms[0].reservationid.value;
				//var r = new ajaxRequest();
				//r.setCallBackFunc(function(){Removal_Reply(r);});
				//r.doPost("modules/Reservations.asp","Function=Room_Remover&res=" + document.forms[0].reservationid.value);
				//alert('Removing Rooms');
			}
			else
			{
				location.href = 'editreservation.asp?reservationid=' + document.forms[0].reservationid.value;
			}
		}
	}
}

function Removal_Reply(r){
	alert(r.responseText);
}