<!--#include file="includes/dbconnections.inc" -->
<html> 
<head> 
<link rel="stylesheet" type="text/css" href="display.css" />
<STYLE TYPE="text/css"> 
P.breakhere {page-break-before: always} 

#break 
{ 
PAGE-BREAK-AFTER: always; 
} 

</STYLE> 
<meta name="Microsoft Theme" content="none, default">
</head> 
<title>Avery Labels</title> 
<body > 
<% 
Set Con2 = Server.CreateObject( "ADODB.Connection" ) 
Con2.Open DBName, DBUser, DBPass 
%> 
<% 
Set RS2 = Con2.Execute( "SELECT distinct r.RoomNumber, p.FirstName, p.LastName, res.checkInDate, res.checkOutDate, rt.comboitem as 'ReservationType', r.phone FROM   t_Reservations res left outer join t_comboitems rt on rt.comboitemid = res.typeid left outer join t_prospect p on p.prospectid = res.prospectid left outer join t_roomallocationmatrix ram on ram.reservationid = res.reservationid left outer join t_room r on r.roomid = ram.roomid where res.statusid in(select comboitemid from t_comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'reservationstatus' and comboitem ='booked') and res.checkindate between '" & request("sdate") & "' and '" & request("edate") & "' and res.reslocationid in (select comboitemid from t_comboitems where comboitem = 'KCP') order by p.lastname,p.firstname " ) 
%> 
<table width="750" border="0" cellspacing="0" cellpadding="0" valign="middle"> 
	<TR> <!-- FIRST ROW--> 
<% 
i = 0  

 While NOT RS2.EOF 
	i = i +1 
%> 
		<td width="0" valign="top" > 
		<!-- i :::: <%=i %> --> 
			<table  width="260" border="0" cellspacing="10" cellpadding="0" valign="top"> <!-- START - INNER TABLE --> 
				<tr valign="middle" align="center"> 
					<td colspan="1" Height="5"> 
						<%=RS2("LastName") & ", " & Left(RS2("FirstName")   & " &nbsp;&nbsp;&nbsp;" & rs2("roomnumber") & "<br>" & rs2("checkindate") & "&nbsp;&nbsp; thru &nbsp;" & rs2("checkoutdate") & "<br>" & rs2("reservationtype") & "&nbsp;&nbsp;&nbsp;" & rs2("phone"),400)%></b></font> 
					</td> 
				</tr> 
  
			</table>
			<!-- END - INNER TABLE --> 
		</td> 
<% 
if i MOD 30 = 0 then 
%> 
	</TR> 
</TABLE>
<P CLASS="breakhere"> 
<table width="750" border="0" cellspacing="0" cellpadding="0" valign="top"> 
	<TR valign="top"> 
<% 
end if 
 

IF i mod 3 = 0 THEN

%> 
	</TR> 

<% 
END IF 

RS2.MoveNext 
Wend 
RS2.Close 
%> 
 
</table> 



<% 
'RS.MoveNext 
'Wend 
'RS.Close 
%> 

<% 'end if %> 
<P ID="break"></P> 
</BODY> 
</HTML> 
