
<html>
<%
dim cn
dim rs, rs2

set cn = server.createobject("ADODB.connection")
set rs = server.createobject("ADODB.recordset")
set rs2 = server.createobject("ADODB.recordset")

DBNAME = "CRMSNet"
DBUser = "asp"
DBpass = "aspnet"

cn.open DBName, DBUser, DBPass
cn.begintrans

rs.open "SELECT * FROM t_CheckByPhone WHERE CheckByPhoneID = '" & request("CheckByPhoneID") & "'",cn,3,3

if rs.eof and rs.bof then
	rs.addnew
end if

%>
<head>
<title>Edit Check By Phone</title>
<script language="javascript" src="scripts/checkbyphone.js"></script>
<script language="javascript" src="../../../scripts/scw.js"></script>
<script language="javascript" src="../../../scripts/ajaxrequest.js"></script>
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

<body onload="FP_preloadImgs(/*url*/'Images/button4C.jpg', /*url*/'Images/button4D.jpg', /*url*/'Images/button4F.jpg', /*url*/'Images/button50.jpg')">

	<form id='form1'>
		<input type="hidden" name="checkbyphoneid" value="<%=rs.fields("CheckByPhoneID").value%>">
		<table bgcolor='yellow'>
			<tr>
				<td colspan='2' align='center'><b><u>Account Information</u></b></td>
			</tr>
			<tr>
				<td><b>First Name:</b></td>
				<td><input type="text" name="fname" value="<%=rs.fields("AccountFirstName").value%>"></td>
			</tr>
			<tr>
				<td><b>Middle Initial:</b></td>
				<td><input type="text" name='midinit' size='5' maxlength='1' value="<%=rs.fields("AccountMiddleInit").value%>"></td>
			</tr>
			<tr>
				<td><b>Last Name:</b></td>
				<td><input type="text" name="lname" value="<%=rs.fields("AccountLastName").value%>"></td>
			</tr>
			<tr>
				<td><b>Routing Number:</b></td>
				<td><input type="text" name="routingnumber" value="<%=rs.fields("RoutingNumber").value%>"></td>
			</tr>
			<tr>
				<td><b>Account Number:</b></td>
				<td><input type="text" name="accountnumber" value="<%=rs.fields("AccountNumber").value%>"></td>
			</tr>
			<tr>
				<td><input type="checkbox" <%if rs.fields("CheckingFlag") = true then response.write "CHECKED"%> name="checking"><b>Checking:</b></td>
				<td><input type="checkbox" <%if rs.fields("SavingsFlag") = true then response.write "CHECKED"%> name="savings"><b>Savings:</b></td>
			</tr>

		</table>
		<br>
		<table>
			<tr>
				<td><b>Contract Number:</b></td>
				<td><input type="text" name="contractnumber" value="<%=rs.fields("ContractNumber").value%>"></td>
			</tr>
			<tr>
				<td><b>Amount:</b></td>
				<td><input type="text" name="amount" value="<%=rs.fields("Amount").value%>"></td>
			</tr>
			<tr>
				<td><b>Date To Run:</b></td>
				<td><input type="text" name="datetorun" size="20" readonly value="<%=rs.fields("DateToRun").value%>" onclick = "scwShow(this,this);"></td>
			</tr>
			<tr>
				<td><b>Status:</b></td>
				<td>
					<select name='statusid'>
						<option value="0"></option>
						<%
						rs2.open "SELECT c.ComboItemID, c.ComboItem FROM t_ComboItems c WHERE ComboName = 'CheckPayStatus' and Active = '1' ORDER BY c.ComboItem",cn,0,1
						do while not rs2.eof
						if cstr(rs.fields("StatusID").value) = cstr(rs2.fields("ComboItemID").value) then
							response.write "<option selected value = '" & rs2.fields("ComboItemID").value & "'>" & rs2.fields("ComboItem").value & "</option>"
						else
							response.write "<option value = '" & rs2.fields("ComboItemID").value & "'>" & rs2.fields("ComboItem").value & "</option>"
						end if
						rs2.movenext
						loop
						rs2.close
						%>
					</select>
				</td>
			</tr>
			<tr>
				<td colspan='2'><input type="button" name="Submit" value="Submit" onclick="EnterCheckPay();"><input type="button" name="button" value="Cancel"></td>
			</tr>
		</table>
		<br>
		<table>
			<tr>
				<td><b>Date Completed:</b></td>
				<td><input type="text" name="datecompleted" size="20" readonly value="<%=rs.fields("DateCompleted").value%>" onclick = "scwShow(this,this);"></td>
			</tr>
			<tr>
				<td><b>Transaction ID:</b></td>
				<td><input type="text" name="transactionid" value="<%=rs.fields("TransactionID").value%>"></td>
			</tr>
		</table>
	</form>
</body>
<%
cn.rollbacktrans
rs.close
cn.close

set rs = nothing
set rs2 = nothing
set cn = nothing
%>
</html>
