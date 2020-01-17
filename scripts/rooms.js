var usages;
var rooms;
var sRoomMod = "modules/rooms.asp";
var currentID = 0;
var sID;
var actLyr = 'roomstab';
var actRow = 'tabrow1';
var aLyrs = new Array('roomstab','maintrequesttab','eventstab','notestab','usertab','amentab');



function Swap_Demographics(lyr,rows)
{
	if (actLyr != lyr){
		for(i=0;i<aLyrs.length;i++){
			document.getElementById(aLyrs[i]).style.visibility = 'hidden';
			document.getElementById(aLyrs[i]).style.display = "none";
		}
		document.getElementById(lyr).style.visibility = 'visible';
		document.getElementById(lyr).style.display = "";
		actLyr = lyr;
		if (actRow !=rows) {
			document.getElementById(rows).style.top = '175px';
			document.getElementById(actRow).style.top = '145px';
			actRow = rows;
		}
	}
}

function Filtering(){ //Filter for rooms.htm
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Show_Rooms(req);});
	req.doPost("modules/Rooms.asp","Function=List&Filter=" + document.all.filtertext.value);
	document.getElementById("rooms").innerHTML = "Loading..."
}

function Show_Rooms(req) //Used on rooms.htm
{
	document.getElementById("rooms").innerHTML = req.responseText;
}

function Get_Room(ID, unitID)
{	
	//alert(ID);
	//alert(unitID);
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Get_Room_Ans(req);});
	req.doPost("modules/Rooms.asp","Function=Load_Room&RoomID=" + ID + "&UnitID=" + unitID);
	//alert('hi');
	document.getElementById("rooms").innerHTML = "Loading...";
	sID = ID;
}

function Get_Room_Ans(req)
{
	//alert('hi again');
	document.getElementById("rooms").innerHTML = req.responseText;
}

function Refresh_Room(ID, unitID){
	Get_Room(id, unitID);
}

function Get_MaintReq(ID)
{	
	//alert(ID);
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Get_MaintReq_Ans(req);});
	req.doPost("modules/Rooms.asp","Function=MaintReq&RoomID=" + ID);
	//alert('hi');
	document.getElementById("maintenance").innerHTML = "Loading..."
}

function Get_MaintReq_Ans(req)
{
	//alert('hi again');
	document.getElementById("maintenance").innerHTML = req.responseText;
}

function Refresh_MaintReq(ID){
	Get_MaintReq(ID);
}

function Get_Amenities(ID)
{	
	//alert(ID);
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Get_Amenities_Ans(req);});
	req.doPost("modules/Rooms.asp","Function=Amens&RoomID=" + ID);
	//alert('hi');
	document.getElementById("amenities").innerHTML = "Loading..."
}

function Get_Amenities_Ans(req)
{
	//alert('hi again');
	document.getElementById("amenities").innerHTML = req.responseText;
}

function Refresh_UserFields(ID){
	Get_UserFields('Room', ID, true);
}

function Get_LockOut(ID)
{	
	//alert(ID);
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Get_LockOut_Ans(req);});
	req.doPost("modules/Rooms.asp","Function=LockOut&RoomID=" + ID);
	//alert('hi');
	document.getElementById("rooms").innerHTML = "Loading..."
}

function Get_LockOut_Ans(req)
{
	//alert('hi again');
	document.getElementById("rooms").innerHTML = req.responseText;
}

function show_Owner()
{
	document.getElementById("rooms").innerHTML = "";
	document.getElementById("rooms").style.top = "175px";
	document.getElementById("ownersearch").style.visibility = "visible";
	document.getElementById("roomsearch").style.visibility = "hidden";
}

//used on RoomWizard.asp
function show_Room()
{
	document.getElementById("rooms").innerHTML = "";
	document.getElementById("rooms").style.top = "130px";
	document.getElementById("ownersearch").style.visibility = "hidden";
	document.getElementById("roomsearch").style.visibility = "visible";
}

function switch_Disp(disp)
{
	if (disp == "name")
	{
		document.getElementById("display").innerHTML = "Owners Name:";
	}
	else if (disp == "owner name")
	{
		document.getElementById("display").innerHTML = "Name:";
	}
	else if (disp == "iimember")
	{
		document.getElementById("display").innerHTML = "II Membership Number:";
	}
	else if (disp == "rcipointsmember")
	{
		document.getElementById("display").innerHTML = "RCI Points Membership Number:";
	}
	else if (disp == "rcimember")
	{
		document.getElementById("display").innerHTML = "RCI Membership Number:";
	}
	else if (disp == "icemember")
	{
		document.getElementById("display").innerHTML = "ICE Membership Number:";
	}
	else if (disp == "contract")
	{
		document.getElementById("display").innerHTML = "Enter Contract Number:";	
	}
	document.forms[0].filter.value = "";
	document.getElementById("rooms").innerHTML = "";
}

function val_Owner(type)
{
	if (type == "exchange" || type == "points" || type == "naljr")
	{
		if (document.forms[0].filter.value == "")
		{
			if (document.getElementById("display").innerHTML == "Owners Name:")
			{
				alert('Please Enter an Owners Name');
			}
			else
			{
				alert('Please Enter a Membership Number');
			}
			return false;
		}
		else
		{
			return true;
		}
	}
	else
	{
		if (document.forms[0].filter.value == "")
		{
			alert('Please Enter an Owners Name');
			return false;
		}
		else
		{
			return true;
		}
	}
}

function val_Owner_Usage()
{
	var sort;
	var filter;
	var error = '';
	var proceed;
	if (document.getElementById("display").innerHTML == "Owners Name:")
	{
		if (document.forms[0].filter.value == '')
		{
			error = 'Please Enter A Name';
			proceed = false;
		}
		else
		{
			proceed = true;
			sort = "Name";
			filter = document.forms[0].filter.value;
		}
	}
	else if (document.getElementById("display").innerHTML == "Name:")
	{
		if (document.forms[0].filter.value == '')
		{
			error = 'Please Enter A Name';
			proceed = false;
		}
		else
		{
			proceed = true;
			sort = "Name";
			filter = document.forms[0].filter.value;
			document.getElementById("rooms").style.top = "200px";		
		}
	}
	else if (document.getElementById("display").innerHTML == "II Membership Number:")
	{
		if (document.forms[0].filter.value == '')
		{
			error = 'Please Enter A Memebership Number';
			proceed = false;
		}
		else
		{
			proceed = true;
			sort = "IIMember";
			filter = document.forms[0].filter.value;
		}
	}
	else if (document.getElementById("display").innerHTML == "RCI Membership Number:")
	{
		if (document.forms[0].filter.value == '')
		{
			error = 'Please Enter A Memebership Number';
			proceed = false;
		}
		else
		{
			proceed = true;
			sort = "RCIMember";
			filter = document.forms[0].filter.value;
		}
	}
	else if (document.getElementById("display").innerHTML == "RCI Points Membership Number:")
	{
		if (document.forms[0].filter.value == '')
		{
			error = 'Please Enter A Memebership Number';
			proceed = false;
		}
		else
		{
			proceed = true;
			sort = "RCIPoints";
			filter = document.forms[0].filter.value;
		}
	}
	else if (document.getElementById("display").innerHTML == "ICE Membership Number:")
	{
		if (document.forms[0].filter.value == '')
		{
			error = 'Please Enter A Memebership Number';
			proceed = false;
		}
		else
		{
			proceed = true;
			sort = "ICEMember";
			filter = document.forms[0].filter.value;
		}
	}
	else if (document.getElementById("display").innerHTML == "Enter Contract Number:") 
	{
		if (document.forms[0].filter.value == '')
		{
			error = 'Please Enter A ContractNumber';
			proceed = false;
		}
		else
		{
			proceed = true;
			sort = "contract";
			filter = document.forms[0].filter.value;
			document.getElementById("rooms").style.top = "200px";
		}
	}
	else
	{
		proceed = true;
		sort = "Year";
		filter = '';
		document.getElementById("rooms").style.top = "175px";
		
	}

	if (proceed == true)
	{
		var req = new ajaxRequest();
		req.setCallBackFunc(function(){val_Usage_Ans(req);});
		req.doPost("../modules/usage.asp","Function=Val_Owner_Usage&filter=" + filter + "&sort=" + sort + "&usageyear=" + document.forms[0].usageyear.value + "&resID=" + document.forms[0].resID.value);
		document.getElementById("rooms").innerHTML = "Searching...";
		document.getElementById("rooms").style.visibility = "visible";
	}
	else
	{
		alert(error);
	}
}

function val_Usage_Ans(req)
{
	var answer = req.responseText.split('$');
	if (answer[0] != "N")
	{
		usages = answer[0].split('|');
	}
	document.getElementById("rooms").innerHTML = answer[1];			
	document.getElementById("rooms").style.visibility = "visible";
}

function sub_Usage_Room()
{
	var found = false;
	var usageids = "";
	for (i=0;i<usages.length;i++)
	{
		if (document.getElementById(usages[i]).checked)
		{
			found = true;
			if (usageids == "")
			{
				usageids = usages[i];
			}
			else
			{
				usageids = usageids + "|" + usages[i];
			}
		}
	}

	if (found == false)
	{
		alert('Please Select A Room');
	}
	else
	{
		var	resID = document.forms[0].resID.value;
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){sub_Usage_Room_Ans(req, resID);});
		req.doPost("../modules/reservations.asp","Function=Add_Usage_Room&ResID=" + resID + "&usage=" + usageids);
	}
}

function sub_Usage_Room_Ans(req, ID)
{
	window.opener.get_Res_Rooms(ID);
	window.close();
}

function get_Avail_Rooms(roomType)
{
	var proceed = true;
	if (document.forms[0].extend.value == 'yes')
	{
		if (document.forms[0].newOutDate.value == '')
		{
			alert('Please Select A New Check Out Date');
			proceed = false;
		}
	}
	
	if (proceed == true)
	{
		document.getElementById("rooms").innerHTML = "";
		document.getElementById("rooms").style.visibility = "hidden";
		if ((document.forms[0].BD.value == '3' || document.forms[0].BD.value == '1') && document.forms[0].unittype.options[document.forms[0].unittype.selectedIndex].text == 'Townes')
		{
			document.getElementById("rooms").innerHTML = "No Rooms Match This Criteria";
			document.getElementById("rooms").style.visibility = "visible";
		}
		else if (document.forms[0].BD.value == '1' && document.forms[0].unittype.options[document.forms[0].unittype.selectedIndex].text == 'Estates')
		{
			document.getElementById("rooms").innerHTML = "No Rooms Match This Criteria";
			document.getElementById("rooms").style.visibility = "visible";	
		}
		else if ((document.forms[0].BD.value == '1BD-DWN' || document.forms[0].BD.value == '1BD-UP') && (document.forms[0].unittype.options[document.forms[0].unittype.selectedIndex].text != 'Estates'))
		{
			document.getElementById("rooms").innerHTML = "No Rooms Match This Criteria";
			document.getElementById("rooms").style.visibility = "visible";
		}
		else if (document.forms[0].BD.value == '4' && document.forms[0].unittype.options[document.forms[0].unittype.selectedIndex].text == 'Cottage')
		{
			document.getElementById("rooms").innerHTML = "No Rooms Match This Criteria";
			document.getElementById("rooms").style.visibility = "visible";
		}
		else
		{
			var searchspares = '';
			var newOutDate = '';
			var	resID = document.forms[0].resID.value;
			var req = new ajaxRequest();
			if (document.forms[0].searchspare.checked)
			{
				searchspares = "yes";
			}
			else
			{
				searchspares = "no";
			}
			if(document.forms[0].extend.value == 'yes')
			{
				newOutDate = document.forms[0].newOutDate.value;
			}
			
			req.setCallBackFunc(function(){avail_Rooms_Ans(req);});
			//alert(document.forms[0].invtype.value);
			//req.doPost("modules/usage.asp", "Function=Get_Avail_Rooms&ResID=" + resID + "&BD=" + document.forms[0].BD.value + "&unittypeid=" + document.forms[0].unittype.value + "&unittype=" + document.forms[0].unittype.options[document.forms[0].unittype.selectedIndex].text + "&searchspares=" + searchspares);
			req.doPost("../modules/usage.asp", "Function=Get_Avail_Rooms&ResID=" + resID + "&BD=" + document.forms[0].BD.value + "&unittypeid=" + document.forms[0].unittype.value + "&unittype=" + document.forms[0].unittype.options[document.forms[0].unittype.selectedIndex].text + "&searchspares=" + searchspares + "&extend=" + document.forms[0].extend.value + "&newOutDate=" + newOutDate + "&inventory=" + document.forms[0].invtype.value);
			document.getElementById("rooms").innerHTML = "Searching..."
			document.getElementById("rooms").style.visibility = "visible";
		}
	}
}

function avail_Rooms_Ans(req)
{
	var answer = req.responseText.split('$');
	if (answer[0] != "N")
	{
		rooms = answer[0].split('|');
	}
	
	document.getElementById("rooms").innerHTML = answer[1];			

}

function sub_Room()
{
	var found = false;
	var roomids = "";
	for (i=0;i<rooms.length;i++)
	{
		if (document.getElementById(rooms[i]).checked)
		{
			found = true;
			if (roomids == "")
			{
				roomids = rooms[i];
			}
			else
			{
				roomids = roomids + "|" + rooms[i];
			}
		}
	}
	
	if (found == false)
	{
		alert('Please Select A Room');
	}
	else
	{	
		var resID = document.forms[0].resID.value;
		var extend = document.forms[0].extend.value;
		var inventory = document.forms[0].invtype.value;
		var newOutDate = '';
		if(extend == 'yes')
		{
			newOutDate = document.forms[0].newOutDate.value;
		}
		var req = new ajaxRequest();
		req.setCallBackFunc(function(){sub_Room_Ans(req, resID, extend);});
		req.doPost("../modules/reservations.asp","Function=Add_Room&ResID=" + resID + "&room=" + roomids + "&extend=" + extend + "&newOutDate=" + newOutDate + "&inventory=" + inventory);
	}
}

function sub_Room_Ans(req, ID, extend)
{
	if (req.responseText == "OK")
	{
		if (extend == 'yes')
		{
			window.opener.get_Res_Rooms(ID);
			window.opener.Get_Reservation(ID, '0', '0', '0', '0');
		}
		else
		{		
			window.opener.get_Res_Rooms(ID);
		}
		window.close();
	}
	else
	{	
		alert(req.responseText);
	}
}

function reset_Phones()
{
	document.getElementById("ResetPhones").disabled = true;
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){reset_Phones_Ans(req);});
	req.doPost("../modules/Reservations.asp","Function=Reset_Phones");
}	

function reset_Phones_Ans(req)
{
	if (req.responseText != "OK")
	{
		alert(req.responseText);
		document.getElementById("ResetPhones").disabled = false;	
	}
	else
	{
		document.getElementById("display").style.visibility = 'visible';
	}
}

function go_out_service()
{	
	var service = '';
	var spares = '';
	document.getElementById("results").innerHTML = '';
	document.getElementById("results").style.visibility = 'hidden';
	if (document.forms[0].sDate.value == '')
	{
		alert('Please Select A Start Date');
	}
	else if (document.forms[0].eDate.value == '')
	{
		alert('Please Select An End Date');
	}
	else if (document.forms[0].sparerooms.length == 0)
	{
		alert('Please Enter a Room to Take In/Out of Service');
	}
	else
	{
		if (document.forms[0].service[0].checked)
		{
			service = 'out';
		}
		else
		{
			service = 'in';
		}
		for (i=0;i<document.forms[0].sparerooms.length;i++)
		{
			if (i == 0)
			{
				spares = document.forms[0].sparerooms.options[i].value;
			}
			else
			{
				spares = spares + '|' + document.forms[0].sparerooms.options[i].value;
			}
		}
		note = document.forms[0].Note.value.replace(/ /g, '%20');
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){Out_Of_Service_Ans(req);});
		req.doPost("../../modules/Units.asp","Function=Out_Of_Service&RoomID=" + spares + "&sDate=" + document.forms[0].sDate.value + "&eDate=" + document.forms[0].eDate.value + "&service=" + service + "&note=" + note);
		document.getElementById("results").innerHTML = "Processing...";
		document.getElementById("results").style.visibility = 'visible';
	}
}

function Out_Of_Service_Ans(req)
{
	document.getElementById("results").innerHTML = req.responseText;
	document.getElementById("results").style.visibility = 'visible';
}

function Populate_Spare()
{
	if (document.forms[0].rooms.length != 0)
	{
		var cItem;
		var nodeItem;
		cItem = new Option (document.forms[0].rooms.options[document.forms[0].rooms.selectedIndex].text, document.forms[0].rooms.value);
		document.forms[0].sparerooms.options[document.forms[0].sparerooms.length] = cItem;
		nodeItem = document.forms[0].rooms.selectedIndex;
		document.forms[0].rooms.options[document.forms[0].rooms.selectedIndex].removeNode();
		if (nodeItem < document.forms[0].rooms.options.length)
		{
			document.forms[0].rooms.selectedIndex = nodeItem;
		}
	}
}

function UnPopulate_Spare()
{
	if (document.forms[0].sparerooms.options.selectedIndex != -1)
	{
		var cItem;
		cItem = new Option (document.forms[0].sparerooms.options[document.forms[0].sparerooms.selectedIndex].text, document.forms[0].sparerooms.value);
		document.forms[0].rooms.options[document.forms[0].rooms.length] = cItem;
		document.forms[0].sparerooms.options[document.forms[0].sparerooms.selectedIndex].removeNode();

	}
	else
	{
		alert('Select a Room to Remove');
	}
}

function Allocate_Spares()
{	
	var spares = '';
	document.getElementById("results").innerHTML = '';
	document.getElementById("results").style.visibility = 'hidden';
	if (document.forms[0].sDate.value == '')
	{
		alert('Please Select A Start Date');
	}
	else if (document.forms[0].eDate.value == '')
	{
		alert('Please Select An End Date');
	}
	else if (document.forms[0].sparerooms.length == 0)
	{
		alert('Please Enter A Room to Allocate as A Spare');
	}
	else
	{
		for (i=0;i<document.forms[0].sparerooms.length;i++)
		{
			if (i == 0)
			{
				spares = document.forms[0].sparerooms.options[i].value;
			}
			else
			{
				spares = spares + '|' + document.forms[0].sparerooms.options[i].value;
			}
		}
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){Allocate_Spares_Ans(req);});
		req.doPost("../../modules/Units.asp","Function=Allocate_Spares&RoomID=" + spares + "&sDate=" + document.forms[0].sDate.value + "&eDate=" + document.forms[0].eDate.value);
		document.getElementById("results").innerHTML = "Processing...";
		document.getElementById("results").style.visibility = "visible";
	}
}

function Allocate_Spares_Ans(req)
{
	document.getElementById("results").innerHTML = req.responseText;
	document.getElementById("results").style.visibility = "visible";
}

function search_Spares(resID, roomID, outDate)
{
	if (document.forms[0].reason.value == '')
	{
		alert('Please Enter a Reason Before Searching');
	}
	else
	{
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){search_Spares_Ans(req);});
		if (document.forms[0].type.value == 'booked')
		{
			req.doPost("../modules/reservations.asp","Function=Search_Spares_Booked&inDate=" + document.forms[0].inDate.value + "&outDate=" + outDate + "&resID=" + resID + "&roomID=" + roomID);		
		}
		else
		{
			req.doPost("../modules/reservations.asp","Function=Search_Spares&inDate=" + document.forms[0].inDate.value + "&outDate=" + outDate + "&resID=" + resID + "&roomID=" + roomID);
		}
		document.getElementById("spares").innerHTML = "Searching...";
		document.forms[0].go.disabled = true;
	}
}

function search_Spares_Ans(req)
{
	document.getElementById("spares").innerHTML = req.responseText;
	document.forms[0].go.disabled = false;
}

function Check_Swap(roomID, resID, inDate, outDate, oldRoomID, type)
{
	if (document.forms[0].reason.value == '')
	{
		alert('Please Enter a Reason Before Selecting Room');
	}
	else
	{
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){Check_Swap_Ans(req, roomID, resID, inDate, outDate, oldRoomID, type);});
		req.doPost("modules/reservations.asp","Function=Check_Swappable&resID=" + resID + "&roomID=" + roomID + "&inDate=" + inDate + "&outDate=" + outDate + "&oldRoomID=" + oldRoomID);
	}
}
function Check_Swap_Ans(req, roomID, resID, inDate, outDate, oldRoomID, type)
{
	if (req.responseText == "OK")
	{
		Add_Spare(roomID, resID, inDate, outDate, oldRoomID, type);
	}
	else
	{
		var proceed = confirm(req.responseText);
		if (proceed == true)
		{
			Add_Spare(roomID, resID, inDate, outDate, oldRoomID, type);
		}
	}
}

	
function Add_Spare(roomID, resID, inDate, outDate, oldRoomID, type)
{
	if (document.forms[0].reason.value == '')
	{
		alert('Please Enter a Reason Before Selecting Room');
	}
	else
	{
		var req = new ajaxRequest;
		var reason;
		reason = encodeURIComponent(document.forms[0].reason.value);
		req.setCallBackFunc(function(){Add_Spare_Ans(req, resID);});
		req.doPost("modules/reservations.asp","Function=Move_To_Spare&resID=" + resID + "&roomID=" + roomID + "&inDate=" + inDate + "&outDate=" + outDate + "&oldRoomID=" + oldRoomID + "&type=" + type + "&reason=" + reason);
	}
}

function Add_Spare_Ans(req, resID)
{
	if (req.responseText == "OK")
	{
		window.opener.get_Res_Rooms(resID);
		window.close();
	}
	else
	{
		alert(req.responseText);
	}
}

function Save(){
	//alert('You are about to save');
	if (Valid()){
		var f = document.forms[0];
		var s = new ajaxRequest();
		s.setCallBackFunc(function(){Save_Ans(s, f.roomid.value);});
		var params = "function=Save";
		params += "&roomid=" + f.roomid.value;
		params += "&unitid=" + f.unitid.value;
		params += "&roomnumber=" + f.roomnumber.value;
		params += "&LockoutID=" + f.lockoutid.value;
		params += "&StatusID=" + f.statusid.options[f.statusid.selectedIndex].value;
		params += "&MaintenanceStatusID=" + f.maintenancestatusid.options[f.maintenancestatusid.selectedIndex].value;
		params += "&Phone=" + encodeURI(f.phone.value);
		params += "&typeid=" + f.typeid.options[f.typeid.selectedIndex].value;
		params += "&subtypeid=" + f.subtypeid.options[f.subtypeid.selectedIndex].value;
		params += "&maxoccupancy=" + f.maxoccupancy.value;
		//alert(params);
		s.doPost(sRoomMod,params);
	}
}

function Save_Ans(s, ID){
	//alert(s.responseText);
	var t = s.responseText.split('|&&|');
	if (t[0]!='Saved')
	{
		alert(t[0]);
	}
	window.navigate('editRoom.asp?roomid=' + ID);
	//rooms.htm');
}

function Valid(){
	var sErr = '';
	var f = document.forms[0];
	sErr += (f.roomnumber.value =='')?'Please enter room number.\n':'';
	sErr += (f.unitid.value =='')?'Please select a unit.\n':'';
	(sErr != '')?alert(sErr):null;
	return (sErr=='')?true:false;
}

function Refresh_Notes()
{
	Get_RoomNotes(sID, true);
}
