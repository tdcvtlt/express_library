
function filtering()
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){filtering_Ans(req);});
	req.doPost("modules/PurchaseRequests.asp","Function=Filter&filtertext=" + document.forms[0].filtertext.value + "&sort=" + document.forms[0].sort.value);
	document.getElementById("pRequests").innerHTML = "Loading Requests...<br><img src = '../images/progressbar.gif'>";
}

function filtering_Ans(req)
{
	document.getElementById("pRequests").innerHTML = req.responseText;
}

function get_Purchase_Request(requestID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){get_Purchase_Request_Ans(req);});
	req.doPost("modules/PurchaseRequests.asp","Function=get_Purchase_Request&RequestID=" + requestID);
}

function get_Purchase_Request_Ans(req)
{
	document.getElementById("request").innerHTML = req.responseText;
}

function get_Request_Items(requestID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){get_Request_Items_Ans(req);});
	req.doPost("modules/PurchaseRequests.asp","Function=get_Purchase_Request_Items&RequestID=" + requestID);
	document.getElementById("requestParts").innerHTML = "Loading Request Items...<br><img src = '../images/progressbar.gif'>";	
}

function get_Request_Items_Ans(req)
{
	document.getElementById("requestParts").innerHTML = req.responseText;
}

function get_Approval_Buttons(requestID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){get_Approval_Buttons_Ans(req);});
	req.doPost("modules/PurchaseRequests.asp","Function=get_Approval_Buttons&requestID=" + requestID);
}

function get_Approval_Buttons_Ans(req)
{
	document.getElementById("submitOptions").innerHTML = req.responseText;
}

function pop_AddItem(requestID)
{
	if (document.forms.requestInfo.status.value != "Pending")
	{
		alert('Items May Only Be Added to Pending Purchase Requests.');
	}
	else
	{
		popitup2('addPRItem.asp?RequestID=' + requestID + '&isManual=' + document.forms.requestInfo.isManual.value);
	}
}
function save_pItem(ID, requestID)
{
	var err = '';
	if (document.getElementById("Amount" + ID).value == '')
	{
		err += 'An Amount Must Be Entered Prior to Saving.\n';
	}
	else if (isNaN(document.getElementById("Amount" + ID).value))
	{
		err += 'Please Enter a Valid Cost Amount.\n';
	}
	else if (document.getElementById("Amount" + ID).value < .01)
	{
		err += 'Item Amount Must Be Greater Than Zero.\n';
	}
	else
	{
		var ch;
		var decPlaces = 0;
		var decFound = '';
		checkStr = document.getElementById("Amount" + ID).value;
		for (i = 0;  i < checkStr.length;  i++)
 	 	{
    		ch = checkStr.charAt(i);
    		if (ch == ".")
    		{
    			decFound = "Y";
    		}
    		else
    		{
    			if (decFound == "Y")
    			{
    				decPlaces++;
    			}
    		}
   		}
   		if (decPlaces > 2)
   		{
   			err += 'Please Enter A Valid Cost Amount.\n';
   		}
	}
	if (err != '')
	{
		alert(err);
	}
	else
	{
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){save_pItem_Ans(req, requestID);});
		req.doPost("modules/PurchaseRequests.asp","Function=Save_PurchaseRequest_Item&ItemID=" + ID + "&RequestID=" + requestID + "&Qty=" + document.getElementById("Qty" + ID).value + "&Amount=" + document.getElementById("Amount" + ID).value + "&Location=" + encodeURIComponent(document.getElementById("Location" + ID).value) + "&Purpose=" + encodeURIComponent(document.getElementById("Purpose" + ID).value)); 
		document.getElementById("requestParts").innerHTML = "Updating Item...<br><img src = '../images/progressbar.gif'>";	
	}
}

function save_pItem_Ans(req, requestID)
{
	if (req.responseText != "OK")
	{
		alert(req.responseText);
	}
	get_Request_Items(requestID);
}

function remove_pItem(ID, requestID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){remove_pItem_Ans(req, requestID);});
	req.doPost("modules/PurchaseRequests.asp","Function=Remove_PurchaseRequest_Item&ItemID=" + ID + "&RequestID=" + requestID); 
	document.getElementById("requestParts").innerHTML = "Updating Item...<br><img src = '../images/progressbar.gif'>";	
}

function remove_pItem_Ans(req, requestID)
{
	if (req.responseText != "OK")
	{
		alert(req.responseText);
	}
	get_Request_Items(requestID);
}

function Approve(requestID)
{
	var answer2;
	var answer3;
	var answer = confirm("Are You Sure You Wish to Approve the Purchase Request?");
	if (answer)
	{
		answer2 = confirm("Are You Absolutely Sure You Wish to Approve the Purchase Request?\nChanges Can Not Be Made After Request Is Approved!");
		if (answer2)
		{
			answer3 = confirm("Final Warning: Once Purchase Request Is Approved, Changes Can Not Be Made.\nDo You Wish to Approve?");
			if (answer3)
			{	
				var req = new ajaxRequest;
				req.setCallBackFunc(function(){Approve_Ans(req, requestID);});
				req.doPost("modules/PurchaseRequests.asp","Function=Approve&RequestID=" + requestID);
			}
		}
	}	
}

function Approve_Ans(req, requestID)
{
	if (req.responseText != "OK")
	{
		alert(req.responseText);
	}
	else
	{
		window.navigate ('editPurchaseRequest.asp?requestid=' + requestID);
	}
}

function Deny(requestID)
{
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Deny_Ans(req, requestID);});
	req.doPost("modules/PurchaseRequests.asp","Function=Deny&RequestID=" + requestID);
}

function Deny_Ans(req, requestID)
{
	if (req.responseText == "OK")
	{
		alert("OK");
	}
	else
	{
		window.navigate ('editPurchaseRequest.asp?requestid=' + requestID);
	}
}

function Save(requestID)
{
	var vendorDesc = '';
	if (document.forms.requestInfo.VendorID.value == "-1")
	{
		vendorDesc = encodeURI(encodeURIComponent(document.forms.requestInfo.vendorDesc.value));
	}
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Save_Ans(req, requestID);});
	req.doPost("modules/PurchaseRequests.asp","Function=Save&RequestID=" + requestID + "&VendorID=" + encodeURIComponent(document.forms.requestInfo.VendorID.value) + "&VendorDesc=" + vendorDesc);
}

function Save_Ans(req, requestID)
{
	var response;
	response = req.responseText.split("|");
	if (response[0] != "OK")
	{
		alert(response[0]);
	}
	else
	{
		window.navigate ('editPurchaseRequest.asp?requestid=' + response[1]);
	}
}

function Print_PR(ID)
{
	window.open ('PurchaseRequestForm.asp?requestid=' + ID);
}

function get_Sub_Filters(lvl, ID)
{
	//Clear all select boxes underneath changed filter
	//Get Values of all filterboxes above changed filter
	var filtervalue;
	if (lvl == '1')
	{
		for (i=document.forms.filtersearch.filter2.length;i>1;i--)
		{
			document.forms.filtersearch.filter2.options[i - 1].removeNode();
		}
		document.forms.filtersearch.filter2.value = 0;
		for (i=document.forms.filtersearch.filter3.length;i>1;i--)
		{
			document.forms.filtersearch.filter3.options[i - 1].removeNode();
		}
		document.forms.filtersearch.filter3.value = 0;
		for (i=document.forms.filtersearch.filter4.length;i>1;i--)
		{
			document.forms.filtersearch.filter4.options[i - 1].removeNode();
		}
		document.forms.filtersearch.filter4.value = 0;
		if (document.forms.filtersearch.filter1.value == '0')
		{
			filtervalue = '';
		}
		else
		{		
			filtervalue = document.forms.filtersearch.filter1.value;	
		}		
	}
	else if (lvl == '2')
	{
		for (i=document.forms.filtersearch.filter3.length;i>1;i--)
		{
			document.forms.filtersearch.filter3.options[i - 1].removeNode();
		}
		document.forms.filtersearch.filter3.value = 0;
		for (i=document.forms.filtersearch.filter4.length;i>1;i--)
		{
			document.forms.filtersearch.filter4.options[i - 1].removeNode();
		}
		document.forms.filtersearch.filter4.value = 0;
		if (document.forms.filtersearch.filter2.value == '0')
		{
			filtervalue = '';
		}
		else
		{		
			filtervalue = document.forms.filtersearch.filter1.value + '|' + document.forms.filtersearch.filter2.value;	
		}
	}
	else
	{
		for (i=document.forms.filtersearch.filter4.length;i>1;i--)
		{
			document.forms.filtersearch.filter4.options[i - 1].removeNode();
		}
		document.forms.filtersearch.filter4.value = 0;
		if (document.forms.filtersearch.filter3.value == '0')
		{
			filtervalue = '';
		}
		else
		{
			filtervalue = document.forms.filtersearch.filter1.value + '|' + document.forms.filtersearch.filter2.value + '|' + document.forms.filtersearch.filter3.value;	
		}
	}

	if (filtervalue != '')
	{
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){get_Sub_Filters_Ans(req, lvl);});
		req.doPost("modules/PurchaseRequests.asp","function=Filter_Parts&level=" + lvl + "&filter=" + filtervalue);
	}
}

function get_Sub_Filters_Ans(req, lvl)
{
	if (req.responseText == "")
	{
	}
	else
	{
		var cItem;
		var filters = req.responseText.split('|');	
		for (i=0;i<filters.length;i++)
		{	
			cItem = new Option(filters[i], filters[i]);
			if (lvl == '1')
			{
				document.forms.filtersearch.filter2.options[document.forms.filtersearch.filter2.length] = cItem;
			}
			else if (lvl == '2')
			{
				document.forms.filtersearch.filter3.options[document.forms.filtersearch.filter3.length] = cItem;			
			}
			else
			{
				document.forms.filtersearch.filter4.options[document.forms.filtersearch.filter4.length] = cItem;						
			}
		}
	}
}

function part_Search(ID)
{
	var filters;
	filters = document.forms.filtersearch.filter1.value + '|' + document.forms.filtersearch.filter2.value + '|' + document.forms.filtersearch.filter3.value + '|' + document.forms.filtersearch.filter4.value;
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){part_Search_Ans(req);});
	req.doPost("modules/PurchaseRequests.asp","function=Part_Search&RequestID=" + ID + "&filter=" + filters);	
	document.getElementById("contentright").innerHTML = "Searching...";
}

function part_Search_Ans(req)
{
	document.getElementById("contentright").innerHTML = req.responseText;
}

function Assign_Part(itemID, requestID)
{
	//alert(itemID + ', ' + encodeURI(itemID) + ' , ' + requestID + ', ' + document.forms.filtersearch.qty.value);
	var req = new ajaxRequest;
	req.setCallBackFunc(function(){Assign_Part_Ans(req, requestID);});
	req.doPost("modules/PurchaseRequests.asp","function=Assign_Part&requestID=" + requestID + "&ItemID=" + encodeURI(itemID) + "&qty=" + document.forms.filtersearch.qty.value + "&isManual=1");
}

function Assign_Part_Ans(req, requestID)
{
	if (req.responseText != "OK")
	{
		alert(req.responseText);
	}
	else
	{
		window.opener.get_Request_Items(requestID);
		window.close();
	}
} 

function add_Manual_Part(requestID)
{
	var err = '';
	if (document.forms.filtersearch.PartNumber.value == '')
	{
		err += "Please Enter a Part Number.";
	}
	if (document.forms.filtersearch.cost.value == '')
	{
		err += 'An Amount Must Be Entered Prior to Saving.\n';
	}
	else if (isNaN(document.forms.filtersearch.cost.value))
	{
		err += 'Please Enter a Valid Cost Amount.\n';
	}
	else if (document.forms.filtersearch.cost.value < .01)
	{
		err += 'Item Amount Must Be Greater Than Zero.\n';
	}
	else
	{
		var ch;
		var decPlaces = 0;
		var decFound = '';
		checkStr = document.forms.filtersearch.cost.value;
		for (i = 0;  i < checkStr.length;  i++)
 	 	{
    		ch = checkStr.charAt(i);
    		if (ch == ".")
    		{
    			decFound = "Y";
    		}
    		else
    		{
    			if (decFound == "Y")
    			{
    				decPlaces++;
    			}
    		}
   		}
   		if (decPlaces > 2)
   		{
   			err += 'Please Enter A Valid Cost Amount.\n';
   		}
	}
	
	if (err != '')
	{
		alert(err);
	}
	else
	{
		var req = new ajaxRequest;
		req.setCallBackFunc(function(){Assign_Part_Ans(req, requestID);});
		req.doPost("modules/PurchaseRequests.asp","Function=Add_Manual_Part&RequestID=" + requestID + "&ItemID=" + document.forms.filtersearch.PartNumber.value + "&Qty=" + document.forms.filtersearch.manualqty.value + "&Amount=" + document.forms.filtersearch.cost.value);
	}
}

function Select_Vendor(ID, Name)
{
	document.forms.requestInfo.VendorID.value = ID;
	document.forms.requestInfo.Vendor.value = Name;
	if (ID == "-1")
	{
		document.getElementById("VendorDesc").display = "";
		document.forms.requestInfo.vendorDesc.value = Name;	
	}
}