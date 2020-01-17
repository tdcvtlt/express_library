
<%

'**********Security Check ***********************
	'on error resume next
'	if not(CheckSecurity("Checks", "Print")) then
'		err.description = "<BR><BR><BR><BR><BR><BR><BR><BR><BR>Access Denied"
'		err.raise -1
'	end if
'	if err <> 0 then
'		response.write err.description
'		err.clear
'		cn.close
'		set cn = nothing
'		response.end
'	end if
'**********End Security Check *******************

dim cn 
dim rs
dim IDs
dim Amounts()
dim Names()
dim AmountText()
dim aChecks
dim aIDs
dim i
dim sCheckDate

if request("Checks")= "" then
	response.write "No Checks Selected"
	response.end
end if

if request("CheckDate") = "" then
	sCheckDate = now
else
	sCheckDate = request("CheckDate")
end if

if request("IDs")= "" then
	response.write "No Premiums Selected"
	response.end
end if

aIDs = split(request("IDs"),",")
aChecks = split(request("Checks"), ",")

if aChecks(0) = "" then
	response.write "No Checks Selected"
	response.end
end if

if ubound(aIDs) <> ubound(aChecks) then
	response.write "Number of Checks provided and Number of Checks to Print do not match"
	response.end
end if

set cn = server.createobject("ADODB.Connection")
set rs = server.createobject("ADODB.Recordset")

cn.Open("Provider=SQLOLEDB;Data Source=RS-SQL-01;Initial Catalog=CRMSNet;User ID=asp;Password=aspnet;")

IDs = request("IDs")
rs.open "Select * from t_PremiumIssued i inner join t_tour t on t.tourid = i.keyvalue inner join t_Prospect p on p.prospectid = t.prospectid where i.KeyField = 'TourID' and i.premiumissuedid in (" & ids & ") order by premiumissuedid asc", cn, 3, 3

if rs.eof and rs.bof then
	response.write "No Premiums Selected" 
	rs.close
	cn.close
	set rs = nothing
	set cn = nothing
	response.end
end if


redim amounts(1)
redim names(1)
redim amounttext(1)

i=0
do while not rs.eof
	if i > ubound(amounts) then
		redim preserve amounts(i)
		redim preserve names(i)
		redim preserve amounttext(i)
	end if
	'response.write i & "<br>"
	amounts(i) = replace(formatcurrency(rs.fields("CostEa").value), "$","")
	names(i) = replace(rs.fields("Firstname").value & " " & rs.fields("Lastname").value, "'","\'")
	amounttext(i) = Get_Text(amounts(i))
	rs.movenext
	i=i+1
loop

rs.close

%>
<html>

<head>
<title>New Page 1</title>
<script language=javascript>

    document.onclick = rotate;

    function rotate() {
        if (check0.style.writingMode == "lr-tb") {
            check0.style.writingMode = "tb-rl";
        }
        else {
            check0.style.writingMode = "lr-tb";
        }
    }

    function load_checks(number) {
        //var sig = new Image();
        //sig.src = 
        var dPrintDate = new Date();
        var aNames = new Array('<%=join(Names,"', '")%>');
        var aAmounts = new Array('<%=join(Amounts,"', '")%>');
        var aAmountText = new Array('<%=join(Amounttext,"', '")%>');
        for (i = 0; i < number; i++) {
            var strHTML = '<TABLE border=0 width = 675 writingMode=tb-rl>' +
			'<TR>' +
			'	<TD colspan = 2 align = right>' + dPrintDate.toDateString() + '</TD>' +
			'</TR>' +
			'<TR></TR><TR></TR><TR></TR><TR></TR><TR></TR><TR></TR><TR></TR><TR></TR>' +
			'<TR>' +
			'	<TD width = 375>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + aNames[i] + '</TD>' +
			'	<TD align = right width = 300>' + aAmounts[i] + '</TD>' +
			'</TR>' +
			'<TR></TR><TR></TR><TR></TR><TR></TR><TR></TR>' +
			'<TR>' +
			'	<TD colspan = 2>' + aAmountText[i] + '</TD>' +
			'</TR>' +
			'<TR></TR><TR></TR><TR></TR><TR></TR><TR></TR><TR></TR><TR></TR>' +
			'<TR>' +
			'	<TD width = 375>&nbsp;</TD>' +
			'	<TD width = 300 align = center><img id="sig' + i + '" style= "width:150;height:100;" src = "../images/RFR Signature001001.bmp"></TD>' +
			'</TR>' +
			'</TABLE>';

            var selectedDiv = 'check' + (i);
            document.all[selectedDiv].innerHTML = strHTML;
            //check0.style.writingMode='tb-rl';
            check0.style.writingMode = 'tb-rl';
        }
        //alert('loaded checks');
        print_check();
    }

    function print_check() {
        var bPrinted = false;
        while (!bPrinted) {
            window.print()
            var bPrinted = confirm('Did the checks print?\nOK= \'Yes\' Cancel=\'No\'');
        }
        window.close();
    }
</script>
<meta name="Microsoft Theme" content="none, default">
</head>

<body onload = "load_checks(<%=ubound(aChecks)+1%>);">
<%
x = 0
for i = 0 to ubound(aChecks)
%>
<DIV ID="check<%=i%>" STYLE="position:relative; left:22px; top:
	<%
	if x=0 then
		response.write 30
	elseif x=1 then
	 	response.write -63
	elseif x=2 then 
	 	response.write 60
	end if
 %>px; width:270px; height:
	<%
	if x = 0 then
		response.write 424
	elseif x = 1 then
		response.write 224
	elseif x = 2 then
		response.write 225
	end if
 %>px;"><%=aChecks(i)%> - <%=aIDs(i)%> - <%=names(i)%> - <%=amounts(i)%>
</DIV>
<%
	rs.open "Select * from t_Checks where checkno = '" & aChecks(i) & "'", cn, 3,3
	rs.fields("PremiumIssuedID").value = aIDs(i)
	rs.fields("PrintedByID").value = request("UserID") 
    rs.fields("DatePrinted").value = sCheckDate 'now
	rs.fields("LocationID").value = request("loc")
	rs.update
	if rs.fields("PosOnPage") = 3 then x = 2
	rs.close
	cn.execute "Update t_PremiumIssued set CertificateNumber = '" & aChecks(i) & "' where PremiumIssuedid = '" & aIDs(i) & "'"
	if x = 2 then
%>
	<p><br style="page-break-after:always"/></p>
<%
		x = 0
	else
		x = x + 1
	end if
%>


<%
next

cn.close

set rs = nothing
set cn = nothing


%>

</body>

</html>


<%
function Get_Text(sAmount)
	'sAmount = sAmount * 1
	if instr(sAmount,".") then
		sChange = Get_Change(right(cstr(sAmount), len(cstr(sAmount)) - instr(cstr(sAmount),".")))
	else
		sChange = Get_Change(0)
	end if
	if sAmount > 100 then
		Get_Text = "**** ZERO AND 00/100 ****"
	else
		if sAmount = 100 then
			Get_Text = "**** ONE HUNDRED AND 00/100 ****"
		else
			sReturn = "**** "
			if sAmount >= 10 then
				select case cint(left(cstr(samount),1))
					case 1
						select case cint(right(left(cstr(sAmount),2),1))
							case 1
								sReturn = sReturn & "ELEVEN AND "
							case 2
								sReturn = sReturn & "TWELVE AND "
							case 3
								sReturn = sReturn & "THIRTEEN AND "
							case 4
								sReturn = sReturn & "FOURTEEN AND "						
							case 5
								sReturn = sReturn & "FIFTEEN AND "
							case 6
								sReturn = sReturn & "SIXTEEN AND "
							case 7
								sReturn = sReturn & "SEVENTEEN AND "
							case 8
								sReturn = sReturn & "EIGHTEEN AND "
							case 9
								sReturn = sReturn & "NINETEEN AND "
							case 0
								sReturn = sReturn & "TEN AND "
						end select
						
					case 2
						sReturn = sReturn & "TWENTY" & Get_Ones(cint(right(left(cstr(sAmount),2),1))) 
					case 3
						sReturn = sReturn & "THIRTY" & Get_Ones(cint(right(left(cstr(sAmount),2),1)))
					case 4
						sReturn = sReturn & "FORTY" & Get_Ones(cint(right(left(cstr(sAmount),2),1)))
					case 5
						sReturn = sReturn & "FIFTY" & Get_Ones(cint(right(left(cstr(sAmount),2),1)))
					case 6
						sReturn = sReturn & "SIXTY" & Get_Ones(cint(right(left(cstr(sAmount),2),1)))
					case 7
						sReturn = sReturn & "SEVENTY" & Get_Ones(cint(right(left(cstr(sAmount),2),1)))
					case 8
						sReturn = sReturn & "EIGHTY" & Get_Ones(cint(right(left(cstr(sAmount),2),1)))
					case 9
						sReturn = sReturn & "NINETY" & Get_Ones(cint(right(left(cstr(sAmount),2),1)))
					case else
					
				end select
			else
				sReturn = replace(Get_Ones(left(cstr(sAmount),1)), "-", "")
			end if
			sReturn = sReturn & sChange
			Get_Text = sReturn
		end if
	end if

end function

function Get_Ones(sOnes)
	sOnes = sOnes * 1
	select case cint(sOnes)
		case 0
			Get_Ones = " AND"
		case 1
			Get_Ones = "-ONE AND"
		case 2
			Get_Ones = "-TWO AND"
		case 3
			Get_Ones = "-THREE AND"
		case 4
			Get_Ones = "-FOUR AND"
		case 5
			Get_Ones = "-FIVE AND"
		case 6
			Get_Ones = "-SIX AND"
		case 7
			Get_Ones = "-SEVEN AND"
		case 8
			Get_Ones = "-EIGHT AND"
		case 9
			Get_Ones = "-NINE AND"
		case else
			Get_Ones = " AND"
	end select
	
end function

function Get_Change(sChange)
	sChange = sChange* 1
	if sChange < 10 then
		Get_Change = " 0" & sChange & "/100 ****"
	else
		Get_Change= " " & sChange  & "/100 ****"
	end if
end function
%>