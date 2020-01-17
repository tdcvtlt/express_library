<!--#include file="dbconnections.inc" -->
<%

'Option Explicit

dim cnHist, rsHist
set cnHist = server.createobject("ADODB.Connection")
set rsHist = server.createobject("ADODB.Recordset")
'DBname = "CRMSNet"
'DBUser = "asp"
'DBPass = "aspnet"
cnHist.open DBName,DBUser,DBPass
rsHist.open "Select * from t_SiteHit where IPAddress = '" & request.servervariables("REMOTE_HOST") & "'", cnHist, 1,3
if rsHist.eof and rsHist.bof then
	rsHist.addnew
	rsHist.fields("IPAddress").value = request.servervariables("REMOTE_ADDR")
	rsHist.fields("HOSTNAME").value = request.servervariables("REMOTE_HOST")
	rsHist.update
end if
dim sID 
sID = rsHist.fields("SiteHitID").value
rsHist.close
rsHist.open "Select * From t_SiteHitHist where 1=2", cnHist, 3, 3
rsHist.addnew
rsHist.fields("SiteHitID").value = sID
rsHist.fields("Protocol").value = request.servervariables("SERVER_PROTOCOL")
rsHist.fields("URL").value = right(request.servervariables("URL"), 255)
rsHist.fields("UserName").value = session("UserName")
rsHist.update
rsHist.close
cnHist.close
set rsHist = nothing
set cnHist = nothing


'if session("UserName") = "" or session("UserID") = "" then
'	response.write LoginMessage
'	response.end
'end if


Function LoginMessage
	dim lmessage
	lmessage= "<div style='position: absolute; width: 698px; height: 593px; z-index: 1; left:177px; top:215px; overflow:scroll'>Not Currently Logged In<BR>"
	dim path, tmp, backslash
	path = request.servervariables("HTTP_REFERER") 
	if instr(lcase(path),"http:")>0 then
		path = replace(lcase(path),"http:","https:")
	end if
	backslash = 0
	for i = 1 to len(path)
		if mid(path,i,1) = "/" then
			backslash = backslash + 1
		end if
		if backslash = 4 then
			tmp = left(path, i)
			exit for
		end if
	next
	path = tmp & "login.htm"
	'response.write path & "<br>"
	'response.write tmp & "<br>"
	lmessage = lmessage &  "<a href='" & path & "'>Click here to Login</a></div>"
	LoginMessage = lmessage
end function

function Send_Mail(sfrom, sto, ssubject, sbody)
	
	'Delivery Status Notifications
	Const cdoDSNDefault = 0 'None
	Const cdoDSNNever = 1 'None
	Const cdoDSNFailure = 2 'Failure
	Const cdoDSNSuccess = 4 'Success
	Const cdoDSNDelay = 8 'Delay
	Const cdoDSNSuccessFailOrDelay = 14 'Success, failure or delay

	if sfrom = "" then
		sfrom = "MISTasks@kingscreekplantation.com"
	end if
	
	if sto = "" then
		sto = "rhill@kingscreekplantation.com"
	end if
	
'	Set objEmail = server.CreateObject("CDO.Message")
'	objEmail.From = sfrom
'	objEmail.To = sto
'	objEmail.Subject = ssubject
'	objEmail.Textbody = sbody
'	objEmail.Send
'	set objEmail = nothing

	Dim ObjSendMail
	Dim iConf
	Dim Flds
     
	Set ObjSendMail = Server.CreateObject("CDO.Message")
	Set iConf = CreateObject("CDO.Configuration")
	Set Flds = iConf.Fields
	     
	Flds("http://schemas.microsoft.com/cdo/configuration/sendusing") = 1
	     
	'**** Path below may need to be changed if it is not correct
	Flds("http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory") = "c:\inetpub\mailroot\pickup"
	Flds.Update
	     
	Set ObjSendMail.Configuration = iConf
	ObjSendMail.To = sTo '"someone@someone.net"
	ObjSendMail.Subject = sSubject '"this is the subject"
	ObjSendMail.From = sFrom '"someone@someone.net"
	     
	' we are sending a text email.. simply switch the comments around to send an html email instead
	ObjSendMail.HTMLBody = sBody '"this is the body"
	'ObjSendMail.TextBody = "this is the body"
	
	'objMessage.AddAttachment "c:\temp\readme.txt"
	
	ObjSendMail.DSNOptions = cdoDSNSuccessFailOrDelay
	ObjSendMail.Send
	     
	Set ObjSendMail = Nothing
End Function


function Get_Campaign(CampaignID)
	dim cnLook
	dim rsLook
	set cnLook= server.createobject("ADODB.Connection")
	set rsLook= server.createobject("ADODB.Recordset")
	cnLook.open DBName,DBUser,DBPass
	rsLook.open "Select * From t_Campaign where campaignID = '" & campaignID & "'", cnLook, 3, 3
	if rsLook.eof and rsLook.bof then
		Get_Campaign = ""
	else
		Get_Campaign= rsLook.fields("campaignname").value & ""
	end if
	rsLook.close
	cnLook.close
	set rsLook = nothing
	set cnLook = nothing

end function

function Get_Frequency(FrequencyID)
	dim cnLook
	dim rsLook
	set cnLook= server.createobject("ADODB.Connection")
	set rsLook= server.createobject("ADODB.Recordset")
	cnLook.open DBName,DBUser,DBPass
	rsLook.open "Select * From t_Frequency where FrequencyID = '" & FrequencyID  & "'", cnLook, 3, 3
	if rsLook.eof and rsLook.bof then
		Get_Frequency = ""
	else
		Get_Frequency = rsLook.fields("frequency").value & ""
	end if
	rsLook.close
	cnLook.close
	set rsLook = nothing
	set cnLook = nothing

end function

function Get_Lookup(ComboItemID)
	dim cnLook
	dim rsLook
	set cnLook= server.createobject("ADODB.Connection")
	set rsLook= server.createobject("ADODB.Recordset")
	cnLook.open DBName,DBUser,DBPass
	rsLook.open "Select * From t_ComboItems where comboitemid = '" & ComboItemID & "'", cnLook, 3, 3
	if rsLook.eof and rsLook.bof then
		Get_Lookup = ""
	else
		Get_Lookup= rsLook.fields("ComboItem").value & ""
	end if
	rsLook.close
	cnLook.close
	set rsLook = nothing
	set cnLook = nothing

end function

Function Credit_Card_Type(strCardNumber)
    Dim strCard, ccType
    strCard = Trim(strCardNumber)
    
    'Clean up card Number
    strCard = Replace(strCard, "-", "")
    strCard = Replace(strCard, " ", "")
    strCard = Replace(strCard, ".", "")

    Select Case CLng(Left(strCard, 1))
        Case 1
            If CLng(Left(strCard, 4)) = 1800 Then
                ccType = "JCB"
            Else
                ccType = "Invalid Card Number"
            End If
        Case 2
            If CLng(Left(strCard, 4)) = 2131 Then
                ccType = "JCB"
            Else
                ccType = "Invalid Card Number"
            End If
        Case 3
            If CLng(Left(strCard, 3)) > 299 And CLng(Left(strCard, 3)) < 306 Then
                ccType = "Diners Club"
            ElseIf CLng(Left(strCard, 2)) = 36 Or CLng(Left(strCard, 2)) = 38 Then
                ccType = "Diners Club"
            ElseIf CLng(Left(strCard, 2)) = 34 Or CLng(Left(strCard, 2)) = 37 Then
                ccType = "Amex"
            Else
                ccType = "JCB"
            End If
        Case 4
            ccType = "Visa"
        Case 5
            If CLng(Left(strCard, 2)) > 50 Or CLng(Left(strCard, 2)) < 56 Then
                ccType = "MasterCard"
            Else
                ccType = "Invalid Card Number"
            End If
        Case 6
            If CLng(Left(strCard, 4)) = 6011 Then
                ccType = "Discover"
            Else
                ccType = "Invalid Card Number"
            End If
        Case Else
            ccType = "Invalid Card Number"
    End Select
    
    set cn3 = server.createobject("ADODB.Connection")
    set rs3 = server.createobject("ADODB.Recordset")
    cn3.Open DBName, DBUser, DBPass
    if ccType = "Invalid Card Number" then
    	ccType = 0
    Else 
    	rs3.OPen "Select ComboItemID from t_ComboItems where comboName = 'CreditCardType' and comboitem = '" & ccType & "'", cn3, 3, 3
    	If rs3.EOF and rs3.BOF then
    		ccType = 999
    	Else	
    		ccType = rs3.Fields("ComboItemID")
    	End If 
		rs3.Close
	End If
	cn3.Close
	set rs3 = Nothing
	set cn3 = Nothing
	Credit_Card_Type = ccType
End Function


function Get_CreditCard(number, exp, cvv, address, city, state, zip, name, swipe)
	set cn2 = server.createobject("ADODB.Connection")
	set rs2 = server.createobject("ADODB.Recordset")
	cn2.Open DBName, DBUser, DBPass
	rs2.Open "Select * from t_CreditCard where number = '" & number & "' and Expiration = '" & exp & "'", cn2, 1, 3
	If rs2.EOF and rs2.BOF then
		rs2.AddNew
		rs2.Fields("TypeID") = Credit_Card_Type(number)
		rs2.Fields("Expiration") = exp
		rs2.Fields("Number") = Trim(number)
		rs2.Fields("Security") = Trim(cvv)
		rs2.fields("String") = swipe
	End If
'		rs2.Fields("Number") = Trim(number)
'		rs2.Fields("TypeID") = Credit_Card_Type(number)
'		rs2.Fields("Expiration") = exp
'		rs2.Fields("Security") = Trim(cvv)
		rs2.Fields("Address") = Trim(address)
		rs2.Fields("City") = Trim(city)
		if state = "" or IsNull(state) then
			rs2.Fields("StateID") = 0
		else
			rs2.Fields("StateID") = state
		end if
		if zip = "" then
			rs2.fields("PostalCode") = 0
		else
			rs2.Fields("PostalCode") = zip
		end if
		rs2.Fields("NameOnCard") = Trim(name)
'		rs2.fields("String") = swipe
		rs2.Updatebatch
		'rs2.MoveFirst
'	End If
	ccID = rs2.Fields("CreditCardID")
	rs2.Close
	cn2.Close
	set rs2 = Nothing
	set cn2 = Nothing
	Get_CreditCard = ccID
End Function

function Get_BatchID(strAccount)
	dim rsBatch
	set rsBatch = server.createobject("ADODB.Recordset")
	rsBatch.open "Select * from t_CCBatch where settled = 0 and datesettled is null and account = '" & strAccount & "'", cn, 3, 3
	if rsBatch.eof and rsBatch.bof then
		sTemp = 0
	else
		sTemp = rsBatch.fields("BatchID").value
	end if
	rsBatch.close
	set rsBatch = nothing
	Get_BatchID = sTemp
end function

function Create_Event(IDField,ID,OldVal,NewVal,EventType,FieldName)
	dim cnE
	dim rsE
	dim bExists
	set cnE = server.createobject("ADODB.Connection")
	set rsE = server.createobject("ADODB.Recordset")
	cnE.open DBName,DBUser,DBPass
	
	bExists = false
	if ucase(EventType) = "VIEW" then
		rsE.open "Select top 1 * from t_Event where " & IDField & " = '" & ID & "' and CreatedByID = '" & session("UserID") & "' and eventtype = 'view' and datediff(hh,datecreated, getDate()) < 1", cnE, 0, 1
		if rsE.eof and rsE.bof then
			bExists = false
		else
			bExists = true
		end if
		rsE.close
	end if

	if not bExists then
		rsE.open "Select * from t_Event where 1=2", cnE, 3, 3
		rsE.addnew
		rsE.fields("KeyField") = IDField
		rsE.Fields("KeyValue") = ID
		rsE.fields("OldValue").value = left(trim(OldVal & ""), rsE.fields("OldValue").definedSize)
		rsE.fields("NewValue").value = NewVal
		rsE.fields("Type").value= EventType
		rsE.fields("FieldName").value = FieldName
		rsE.fields("CreatedByID").value = Session("UserDBID")
		rsE.fields("DateCreated").value = now
		rsE.update
		rsE.close
	end if
	
	cnE.close
	set cnE = nothing
	set rsE = nothing
end function

Function CheckSecurity(sArea, sItem) 
    'Return True if permission is granted
    'Returns False if not granted and/or is missing
    'Dim rstSecurity 
	'dim cnSecurity
	'set cnSecurity = server.createobject("ADODB.Connection")
	'set rstSecurity = server.createobject("ADODB.Recordset")

	'cnSecurity.open DBName, DBUser, DBPass

    'rstSecurity.Open "Select * from t_SecurityItem2User u inner join t_SecurityItem i on i.SecurityItemID = u.SecurityItemID inner join t_Securitygroups g on g.GroupID = i.SecurityGroupid where g.groupname = '" & sArea & "' and u.personnelid = '" & session("UserID") & "' and i.Item = '" & sItem & "'", cnSecurity, 3, 3
    'If rstSecurity.EOF And rstSecurity.BOF Then
    '    CheckSecurity = False
    'Else
        CheckSecurity = True
    'End If
    'rstSecurity.Close
    'cnSecurity.close
    'Set rstSecurity= Nothing
    'set cnSecurity = nothing
End Function

Public Sub Set_Menu_Security()
    frmMain.mnuFile.Enabled = CheckSecurity("Menu", "File")
    frmMain.mnuSalesContracts.Enabled = CheckSecurity("Menu", "Contract")
    frmMain.mnuMarketing.Enabled = CheckSecurity("Menu", "Marketing")
    frmMain.mnuSales.Enabled = CheckSecurity("Menu", "Sales")
    frmMain.mnuReservations.Enabled = CheckSecurity("Menu", "Reservations")
    frmMain.mnuAccounting.Enabled = CheckSecurity("Menu", "Accounting")
    frmMain.mnuFrontDesk.Enabled = CheckSecurity("Menu", "Front Desk")
    frmMain.mnuFacilities.Enabled = CheckSecurity("Menu", "Facilities")
    frmMain.mnuPayroll.Enabled = CheckSecurity("Menu", "Payroll")
    frmMain.mnuSetup.Enabled = CheckSecurity("Menu", "Setup")
End Sub

if request("Function") <> "" then
	dim cnS
	dim rsS
	set cnS = server.createobject("ADODB.Connection")
	set rsS = server.createobject("ADODB.Recordset")
	set rs2 = server.createobject("ADODB.Recordset")
	set rs3 = server.createobject("ADODB.Recordset")
	cnS.open DBName, DBUser, DBPass
end if

if request("Function") = "Get_Areas" then
	on error resume next
	if Not(CheckSecurity("Security", "ViewPersonnelSettings")) then
		err.raise -1
		err.description ="Access Denied"
	end if
	
	if err <> 0 then 
		response.write err.description
		err.clear
		response.end
	end if
	response.write "<form name = 'secform'><table><tr><td>Areas</td><td>Items</td></tr>"
	response.write "<tr><td valign=top>"
	response.write "<Select onclick = 'Get_Items(this);' onchange = 'Get_Group_Items(this);' name = 'areas' size = 20>"
	rsS.open "Select * from t_SecurityGroups order by GroupName", cnS, 3, 3
	do while not rsS.eof 
		response.write "<option value = '" & rsS.fields("GroupID").value & "'>" & rsS.fields("GroupName").value & "</option>"
		rsS.movenext
	loop
	rsS.close
	response.write "</Select>"
	response.write "</td><td valign = top id = 'items'></td></tr></table>"
	response.write "<br>Area Name:&nbsp;<input type = 'text' value= '' name = 'areaname'><input type = button value = 'Add Area' onclick = " & chr(34) & "if(document.secform.areaname.value==''){alert('Please enter an area name');}else{Add_Area(document.secform.areaname.value);}" & chr(34) & ">" 
	response.write "<br>Item Name:&nbsp;<input type = 'text' value= '' name = 'itemname'><input type = button value = 'Add Item' onclick = " & chr(34) & "if(document.secform.itemname.value==''){alert('Please enter an Item name');}else{if(document.secform.areas.selectedIndex > -1){Add_Item(document.secform.areas.options[document.secform.areas.selectedIndex].value);}else{alert('Please select an area');}}" & chr(34) & ">" 
	response.write "</form>"
elseif request("Function") = "Get_Group_Areas" then
	on error resume next
	if Not(CheckSecurity("Security", "ViewPersonnelSettings")) then
		err.raise -1
		err.description ="Access Denied"
	end if
	
	if err <> 0 then 
		response.write err.description
		err.clear
		response.end
	end if
	response.write "<form name = 'secform'><table><tr><td>Areas</td><td>Items</td></tr>"
	response.write "<tr><td valign = top>"
	response.write "<Select onclick = 'Get_Group_Items(this);' onchange = 'Get_Group_Items(this);' name = 'areas' size = 20>"
	rsS.open "Select * from t_SecurityGroups order by GroupName", cnS, 3, 3
	do while not rsS.eof 
		response.write "<option value = '" & rsS.fields("GroupID").value & "'>" & rsS.fields("GroupName").value & "</option>"
		rsS.movenext
	loop
	rsS.close
	response.write "</Select>"
	response.write "</td><td valign = top id = 'items'></td></tr></table>"
	response.write "<br>Area Name:&nbsp;<input type = 'text' value= '' name = 'areaname'><input type = button value = 'Add Area' onclick = " & chr(34) & "if(document.secform.areaname.value==''){alert('Please enter an area name');}else{Add_Area(document.secform.areaname.value);}" & chr(34) & ">" 
	response.write "<br>Item Name:&nbsp;<input type = 'text' value= '' name = 'itemname'><input type = button value = 'Add Item' onclick = " & chr(34) & "if(document.secform.itemname.value==''){alert('Please enter an Item name');}else{if(document.secform.areas.selectedIndex > -1){Add_Item(document.secform.areas.options[document.secform.areas.selectedIndex].value);}else{alert('Please select an area');}}" & chr(34) & ">" 
	response.write "</form>"
elseif request("Function") = "Get_Items" then
	if request("GroupID") = "" or request("GroupID") = "0" or request("PersonnelID") = "" or request("PersonnelID") = "0" then
		response.write "No Area Selected and/or No PersonnelID Provided"
	else
		rsS.open "Select i.*, u.personnelid From t_SecurityItem i left outer join (select * From t_SecurityItem2User where personnelid = '" & request("PersonnelID") & "') u on u.securityitemid = i.securityitemid where  i.securitygroupid = '" & request("GroupID") & "'", cnS, 3, 3
		response.write "<table border = 1><tr><td>Allow</td><td>Item</td></tr>"
		do while not rsS.eof
			if rsS.fields("PersonnelID") & "" <> "" then
				response.write "<tr><td><input checked onclick = 'Update_Item(this);' type = 'checkbox' value = '" & rsS.fields("SecurityItemID").value & "'></td><td>" & rsS.fields("Item").value & "</td></tr>"
			else
				response.write "<tr><td><input onclick = 'Update_Item(this);' type = 'checkbox' value = '" & rsS.fields("SecurityItemID").value & "'></td><td>" & rsS.fields("Item").value & "</td></tr>"
			end if
			rsS.movenext
		loop
		response.write "</table>"
		rsS.close
	end if
elseif request("Function") = "Get_Group_Items" then
	if request("GroupID") = "" or request("GroupID") = "0" or request("PersonnelGroupID") = "" or request("PersonnelGroupID") = "0" then
		response.write "No Area Selected and/or No PersonnelID Provided"
	else
		rsS.open "Select i.*, u.personnelgroupid From t_SecurityItem i left outer join (select * From t_SecurityItem2group where personnelgroupid = '" & request("PersonnelGroupID") & "') u on u.securityitemid = i.securityitemid where  i.securitygroupid = '" & request("GroupID") & "'", cnS, 3, 3
		response.write "<table border = 1><tr><td>Allow</td><td>Item</td></tr>"
		do while not rsS.eof
			if rsS.fields("PersonnelGroupID") & "" <> "" then
				response.write "<tr><td><input checked onclick = 'Update_Group_Item(this);' type = 'checkbox' value = '" & rsS.fields("SecurityItemID").value & "'></td><td>" & rsS.fields("Item").value & "</td></tr>"
			else
				response.write "<tr><td><input onclick = 'Update_Group_Item(this);' type = 'checkbox' value = '" & rsS.fields("SecurityItemID").value & "'></td><td>" & rsS.fields("Item").value & "</td></tr>"
			end if
			rsS.movenext
		loop
		response.write "</table>"
		rsS.close
	end if
elseif request("Function") = "Update_Items" then
	if request("ItemID") = "" or request("ItemID") = "0" or request("PersonnelID") = "" or request("PersonnelID") = "0" or request("Allow") = "" then
	else
		rsS.open "Select * from t_SecurityItem2User where SecurityItemID = '" & request("ItemID") & "' and PersonnelID = '" & request("PersonnelID") & "'", cnS, 3, 3
		select case ucase(request("Allow") & "")
			case "TRUE"
				if rsS.eof and rsS.bof then
					rsS.addnew
					rsS.fields("SecurityItemID").value = request("ItemID")
					rsS.fields("PersonnelID").value = request("PersonnelID")
					rsS.update
				end if
			case "FALSE"
				do while not rsS.eof
					rsS.delete
					rsS.movenext
				loop
			case else
		end select
		rsS.close
	end if
elseif request("Function") = "Update_Group_Items" then
	if request("ItemID") = "" or request("ItemID") = "0" or request("PersonnelGroupID") = "" or request("PersonnelGroupID") = "0" or request("Allow") = "" then
	else
		rsS.open "Select * from t_SecurityItem2Group where SecurityItemID = '" & request("ItemID") & "' and PersonnelGroupID = '" & request("PersonnelGroupID") & "'", cnS, 3, 3
		select case ucase(request("Allow") & "")
			case "TRUE"
				if rsS.eof and rsS.bof then
					rsS.addnew
					rsS.fields("SecurityItemID").value = request("ItemID")
					rsS.fields("PersonnelGroupID").value = request("PersonnelGroupID")
					rsS.update
				end if
				'***** Add Item to each Personnel in the group ******'
				rs2.Open "Select PersonnelID from t_Personnel2Group where personnelgroupid = '" & request("PersonnelGroupID") & "'", cnS, 3, 3
				Do while not rs2.EOF
					rs3.Open "Select * from t_SecurityItem2User where SecurityItemID = '" & request("ItemID") & "' and PersonnelID = '" & rs2.Fields("PersonnelID") & "'", cnS, 3, 3
					If rs3.EOF and rs3.BOF then
						rs3.AddNew
						rs3.Fields("PersonnelID") = rs2.Fields("PersonnelID")
						rs3.Fields("SecurityItemID") = request("ItemID")
						rs3.Update
					End If
					rs3.Close
					rs2.MoveNext
				Loop
				rs2.Close
						
			case "FALSE"
				do while not rsS.eof
					rs2.Open "Select PersonnelID from t_Personnel2Group where personnelgroupid = '" & rsS.Fields("PersonnelGroupID") & "'", cnS, 3, 3
					Do while not rs2.EOf
						rs3.Open "Select * from t_SecurityItem2User where SecurityItemID = '" & request("ItemID") & "' and PersonnelID = '" & rs2.Fields("PersonnelID") & "' and securityitemid not in (Select securityitemid from t_SecurityItem2Group where securityitemid = '" & request("ItemID") & "' and PersonnelGroupID in (Select personnelgroupid from t_Personnel2Group where personnelgroupid <> '" & rsS.Fields("PersonnelGroupID") & "' and personnelid = '" & rs2.Fields("PersonnelID") & "'))", cnS, 3, 3 
						Do while not rs3.EOF
							rs3.Delete
							rs3.MoveNext
						Loop
						rs3.Close
						rs2.MoveNext
					Loop
					rsS.delete
					rsS.movenext
				loop
				
			case else
		end select
		rsS.close
	end if
elseif request("Function") = "Add_Item" then
	if request("AreaID") <> "" and request("AreaID") <> "0" and request("ItemName") <> "" then
		on error resume next
		rsS.open "Select * from t_SecurityItem i where securitygroupid in (select securitygroupid from t_SecurityGroups where securitygroupid = '" & request("AreaID") & "') and Item = '" & request("ItemName") & "'", cnS, 3, 3
		if rsS.eof and rsS.bof then
			rsS.addnew
			rsS.fields("SecurityGroupID").value = request("AreaID")
			rsS.fields("Item").value = request("ItemName")
			rsS.update
		end if
		rsS.close
		if err <> 0 then
			response.write err.number & " - " & err.description
			err.clear
		else
			response.write "OK"
		end if
	end if
elseif request("Function") = "Add_Area" then
	if request("AreaName") <> "" then 
		on error resume next
		rsS.open "Select * from t_SecurityGroups where GroupName='" & request("AreaName") & "'", cnS, 3, 3
		if rsS.eof and rsS.bof then
			rsS.addnew
			rsS.fields("GroupName").value = request("AreaName")
			'rsS.fields("LocationID").value = 1
			rsS.update
		end if
		rsS.close
		if err <> 0 then
			response.write err.number & " - " & err.description
			err.clear
		else 
			response.write "OK"
		end if
	end if
elseif request("Function") = "Remove_Personnel_From_Group" then
	rsS.Open "Select * from t_SecurityItem2User where PersonnelID = '" & request("PersonnelID") & "' and securityitemid in (Select securityitemid from t_SecurityItem2Group where PersonnelGroupID in (Select personnelgroupid from t_Personnel2Group where personnelgroupid = '" & request("PersonnelGroupID") & "' and personnelid = '" & request("PersonnelID") & "')) and securityitemid not in (Select SecurityItemID from t_SecurityItem2Group where personnelgroupid in (Select personnelgroupid from t_Personnel2Group where personnelgroupid <> '" & request("PersonnelGroupID") & "' and personnelid = '"  & request("PersonnelID") & "'))", cnS, 3, 3
	Do while not rsS.eof
		rsS.Delete 'adAffectCurrent
		rsS.requery
		'rsS.MoveNext
	Loop
	rss.UpdateBatch
	rsS.Close
	
	rsS.Open "Select * from t_Personnel2Group where personnelid = '" & request("PersonnelID") & "' and personnelGroupID = '" & request("PersonnelGroupID") & "'", cnS, 3, 3
	Do while not rsS.EOF
		rsS.Delete 'adAffectCurrent
		rsS.Requery
	Loop
	rss.UpdateBatch
	rsS.Close
elseif request("Function") = "Add_Personnel2Group" then
	rsS.Open "Select * from t_Personnel2Group where personnelid = '" & request("PersonnelID") & "' and personnelgroupid = '" & request("PersonnelGroupID") & "'", cnS, 3, 3
	If rsS.EOF and rsS.BOF then
		rsS.AddNew
		rss.Fields("PersonnelID") = request("PersonnelID")
		rsS.fields("PersonnelGroupID") = request("PersonnelGroupID")
		rsS.Update
	end if	
	rsS.Close
	
	rsS.Open "Select SecurityItemID from t_SecurityItem2Group where personnelgroupid = '" & request("PersonnelGroupID") & "'", cnS, 3, 3
	Do while not rsS.EOF
		rs2.Open "Select * from t_SecurityItem2User where personnelID = '" & request("PersonnelID") & "' and securityItemID = '" & rsS.Fields("SecurityItemID") & "'", cnS, 3, 3
		If rs2.EOF and rs2.BOF then
			rs2.AddNew
			rs2.Fields("PersonnelID") = request("PersonnelID")
			rs2.Fields("SecurityITemID") = rsS.Fields("SecurityItemID")
			rs2.UpdateBatch
		End If
		rs2.Close
		rsS.MoveNext
	Loop
	rsS.Close
end if

if request("Function") <> "" then
	cnS.close
	set rsS = nothing
	set rs2 = Nothing
	set cnS = nothing
end if

%>