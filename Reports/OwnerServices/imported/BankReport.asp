
<html>

<head>
<title></title>
<script language="javascript" src = "../../../scripts/scw.js"></script>
<script language="javascript" src = "../../../scripts/ajaxRequest.js"></script>
<script language="javascript" src = "scripts/BankReport.js"></script>
<script language="JavaScript">
<!--
function FP_swapImg() {//v1.0
 var doc=document,args=arguments,elm,n; doc.$imgSwaps=new Array(); for(n=2; n<args.length;
 n+=2) { elm=FP_getObjectByID(args[n]); if(elm) { doc.$imgSwaps[doc.$imgSwaps.length]=elm;
 elm.$src=elm.src; elm.src=args[n+1]; } }
}

function FP_preloadImgs() {//v1.0
 var d=document,a=arguments; if(!d.FP_imgs) d.FP_imgs=new Array();
 for(var i=0; i<a.length; i++) { d.FP_imgs[i]=new Image; d.FP_imgs[i].src=a[i]; }
}

function FP_getObjectByID(id,o) {//v1.0
 var c,el,els,f,m,n; if(!o)o=document; if(o.getElementById) el=o.getElementById(id);
 else if(o.layers) c=o.layers; else if(o.all) el=o.all[id]; if(el) return el;
 if(o.id==id || o.name==id) return o; if(o.childNodes) c=o.childNodes; if(c)
 for(n=0; n<c.length; n++) { el=FP_getObjectByID(id,c[n]); if(el) return el; }
 f=o.forms; if(f) for(n=0; n<f.length; n++) { els=f[n].elements;
 for(m=0; m<els.length; m++){ el=FP_getObjectByID(id,els[n]); if(el) return el; } }
 return null;
}
// -->
</script>
<meta name="Microsoft Theme" content="none, default">
<meta name="Microsoft Border" content="none, default">
</head>

<body onload="FP_preloadImgs(/*url*/'../../../images/Accounting/Funding/Images/button19A.jpg', /*url*/'../../../images/Accounting/Funding/Images/button199.jpg')">
<%








'security block goes here



'




dim cn
dim rs
dim rsl

set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.recordset")
set rsl = server.createobject("ADODB.recordset")


DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

cn.open DBName, DBuser, DBPass
%>
<form method="POST" action="BankReport.asp" onsubmit="return false;" webbot-action="--WEBBOT-SELF--">


<table width="267">
	<tr>
		<td width="103">
			Unit Size:
		</td>
		<td width="154">
			<Select name='unitsize'><option></option><%
				for i = 1 to 4
					response.write i & " BD"
					'if request("UnitSize") = cstr(i & "BD") then
					'	response.write "<option selected value = '" & i & "BD" & "'>" & i &" BD" & "</option>"
					'else
						response.write "<option value = '" & i & "BD" & "'>" & i &" BD" & "</option>"
					'end if
				next
%></Select>
		</td>
		<td>
			Unit Type:
		</td>
		<td width="103">
		<Select name='unittype'><option></option><%

				rsl.open "SELECT * from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID WHERE ComboName = 'InventoryType' ",cn,0,1
			
					do while not rsl.eof

						'if request("UnitType") = cstr(rsl.fields("ComboItemID").value) then
						'	response.write "<option selected value = '" & rsl.fields("ComboItemID").value & "'>" & rsl.fields("ComboItem").value & "</option>"
						'else
							response.write "<option value = '" & rsl.fields("ComboItemID").value & "'>" & rsl.fields("ComboItem").value & "</option>"
						'end if

					rsl.movenext
				loop
	
				rsl.close

%></Select>
		</td>
	</tr>
	<tr>
		<td>
			Exchange Co.
		</td>
		<td width="154">
			<Select name='exchange'><option value = '0'></option><%
				rsl.open "SELECT * FROM t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID WHERE ComboName = 'ReservationSubType'",cn,0,1
				
					do while not rsl.eof
	
						'if request("Exchange") = cstr(rsl.fields("ComboItemID").value) then
						'	response.write "<option selected value = '" & rsl.fields("ComboItemID").value & "'>" & rsl.fields("ComboItem").value & "</option>"
						'else
							response.write "<option value = '" & rsl.fields("ComboItemID").value & "'>" & rsl.fields("ComboItem").value & "</option>"
						'end if

					rsl.movenext
				loop
	
				rsl.close
%></Select>
		</td>
		<td>
			Deposit Year
		</td>
		<td width="103">
			<Select name='deposityear'><option></option><%
				for i = -3 to 2 step 1
					response.write year(date) + i
					'if request("DepositYear") = cstr(year(date) + i) then
					'	response.write "<option selected value = '" & year(date) + i & "'>" & year(date) + i & "</option>"
					'else
						response.write "<option value = '" & year(date) + i & "'>" & year(date) + i & "</option>"
					'end if
				next
%></Select>
	 	</td>
	 </tr>
	 <tr>
	 	<td>
	 		Year Used
	 	</td>
	 	<td width="154">
	 	<Select name='yearused'><option value = 0></option><%
				for i = -2 to 2 step 1
					response.write year(date) + i
					'if request("yearused") = cstr(year(date) + i) then
					'	response.write "<option selected value = '" & year(date) + i & "'>" & year(date) + i & "</option>"
					'else
						response.write "<option value = '" & year(date) + i & "'>" & year(date) + i & "</option>"
					'end if
				next
%></Select>
	 	</td>
		<td>
			Season
		</td>
		<td width="103">
			<select name='season'><option></option><%
				rsl.open "SELECT * from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where ComboName = 'Season'",cn,0,1
			
					do while not rsl.eof

						'if request("usage") = cstr(rsl.fields("ComboItemID").value) then
						'	response.write "<option selected value = '" & rsl.fields("ComboItemID").value & "'>" & rsl.fields("ComboItem").value & "</option>"
						'else
							response.write "<option value = '" & rsl.fields("ComboItemID").value & "'>" & rsl.fields("ComboItem").value & "</option>"
						'end if

					rsl.movenext
				loop
	
				rsl.close
%></select>
		</td>
	</tr>
	<tr>
		<td>
			Usage
		</td>
		<td width="154">
<select name='usage'><option></option><%
				rsl.open "SELECT * from t_frequency",cn,0,1
			
					do while not rsl.eof

						'if request("frequency") = cstr(rsl.fields("FrequencyID").value) then
						'	response.write "<option selected value = '" & rsl.fields("FrequencyID").value & "'>" & rsl.fields("Frequency").value & "</option>"
						'else
							response.write "<option value = '" & rsl.fields("FrequencyID").value & "'>" & rsl.fields("Frequency").value & "</option>"
						'end if

					rsl.movenext
				loop
		
				rsl.close

%></select>		</td>
		<td>
			Status
		</td>
		<td colspan=2>
<Select name='status'><option></option><%
				rsl.open "SELECT * FROM t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID WHERE ComboName = 'BankingStatus'",cn,0,1
			
					do while not rsl.eof

						'if request("status") = cstr(rsl.fields("ComboItemID").value) then
						'	response.write "<option selected value = '" & rsl.fields("ComboItemID").value & "'>" & rsl.fields("ComboItem").value & "</option>"
						'else
							response.write "<option value = '" & rsl.fields("ComboItemID").value & "'>" & rsl.fields("ComboItem").value & "</option>"
						'end if

					rsl.movenext
				loop
	
				rsl.close

%></Select>		</td>
	</tr>
	<tr>
		<td>
			<img border="0" id="img1" src="../../../images/Accounting/Funding/Images/button198.jpg" height="20" width="100" alt="List" 
            onmouseover="FP_swapImg(1,0,/*id*/'img1',/*url*/'../../../images/Accounting/Funding/Images/button199.jpg')" 
            onmouseout="FP_swapImg(0,0,/*id*/'img1',/*url*/'../../../images/Accounting/Funding/Images/button198.jpgg')" 
            onmousedown="FP_swapImg(1,0,/*id*/'img1',/*url*/'../../../images/Accounting/Funding/Images/button19A.jpg')" 
            onmouseup="FP_swapImg(0,0,/*id*/'img1',/*url*/'../../../images/Accounting/Funding/Images/button199.jpg')" 
            fp-style="fp-btn: Embossed Rectangle 5" fp-title="List" onclick = "List();">
		</td>
	</tr>
</table>
</form>
<%
cn.close

set rs = nothing
set rsl = nothing
set cn = nothing
%>

 
<div id = "report"></div>

 
</body>
</html>







 
