
<html xmlns:o="urn:schemas-microsoft-com:office:office"
xmlns:w="urn:schemas-microsoft-com:office:word"
xmlns="http://www.w3.org/TR/REC-html40">

<head>
<link rel=Edit-Time-Data href="Labels_files/editdata.mso">
<title>Labels </title>
<style>
<!--
 /* Style Definitions */
 p.MsoNormal, li.MsoNormal, div.MsoNormal
	{mso-style-parent:"";
	margin:0in;
	margin-bottom:.0001pt;
	mso-pagination:widow-orphan;
	font-size:12.0pt;
	font-family:"Times New Roman";
	mso-fareast-font-family:"Times New Roman";}
span.SpellE
	{mso-style-name:"";
	mso-spl-e:yes;}
@page Section1
	{size:8.5in 11.0in;
	margin:.5in .75in 0in .75in;
	mso-header-margin:.5in;
	mso-footer-margin:.5in;
	mso-paper-source:4;}
div.Section1
	{page:Section1;}
-->
</style>
<!--[if gte mso 10]>
<style>
 /* Style Definitions */
 table.MsoNormalTable
	{mso-style-name:"Table Normal";
	mso-tstyle-rowband-size:0;
	mso-tstyle-colband-size:0;
	mso-style-noshow:yes;
	mso-style-parent:"";
	mso-padding-alt:0in 5.4pt 0in 5.4pt;
	mso-para-margin:0in;
	mso-para-margin-bottom:.0001pt;
	mso-pagination:widow-orphan;
	font-size:10.0pt;
	font-family:"Times New Roman";
	mso-ansi-language:#0400;
	mso-fareast-language:#0400;
	mso-bidi-language:#0400;}
</style>
<![endif]-->
</head>

<body lang=EN-US style='tab-interval:.5in'>

<div class=Section1>

<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0
 style='border-collapse:collapse;mso-padding-top-alt:0in;mso-padding-bottom-alt:
 0in;margin-left:.35in'>
<%
	Dim cn
	Dim rs
	Dim i
    Dim tourID
	i = 0
    tourID = 0
	set cn = server.createobject("ADODB.Connection")
	set rs = server.createobject("ADODB.Recordset")
	set rs2 = server.createobject("ADODB.Recordset")
	cn.Open "CRMSnet", "asp", "aspnet"
	rs.OPen "Select a.ReservationID, a.ProspectID, b.FirstName, b.LastName, c.Comboitem as SubType, a.CheckInDate, a.CheckOutDate, a.ProspectID, t.TourDate, tt.ComboItem as TourTime from t_Reservations a inner join t_Prospect b on a.prospectid = b.prospectid left outer join t_Tour t on a.ReservationID = t.ReservationID left outer join t_ComboItems tt on t.TourTime = tt.ComboItemID left outer join t_ComboItems c on a.SubTypeID = c.ComboItemID where a.CheckInDate between '" & request("sDate") & "' and '" & request("eDate") & "' and a.statusid = (Select comboitemid from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'ReservationStatus' and comboitem = 'booked') and resLocationID = (Select comboitemid from t_CombOitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'ReservationLocation' and comboitem = 'KCP') order by b.LastName asc", cn, 3, 3 
	If rs.EOF and rs.BOF then
	Else
		Do while not rs.EOF
			rooms = ""
			rs2.Open "Select Distinct(RoomNumber) As RoomNumber from t_RoomAllocationMatrix a inner join t_Room b on a.roomid = b.roomid where a.ReservationID = '" & rs.Fields("ReservationID") & "'", cn, 3, 3
			If rs2.EOF and rs2.BOF then
			Else
				Do while not rs2.EOF
					if rooms = "" then
						rooms = rs2.Fields("RoomNumber")
					else
						rooms = rooms & ", " & rs2.Fields("RoomNumber")
					end if
					rs2.MoveNext
				Loop
			End If
			rs2.Close			
			if i = 0 then
				i = 2
			%>
			 	<tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;page-break-inside:avoid;height:2.0in'>
			<%
			End IF
				rs2.Open "Select c.Name, t.* from t_Tour t inner join t_Campaign c on t.CampaignID = c.CampaignID where t.tourdate between '" & rs.Fields("CheckIndate") & "' and '" & rs.fields("CheckOutDate") & "' and prospectid = '" & rs.Fields("ProspectID") & "' and t.tourlocationid = (select comboitemid from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'TourLocation' and ComboItem = 'KCP') and t.statusid = (select comboitemid from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'TourStatus' and comboitem = 'Booked') and (t.SubTypeID is null or t.SubTypeID not in (Select comboitemid from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'TourSubType' and comboitem like '%Exit%'))", cn, 3, 3
				If rs2.EOF and rs2.BOF then
					If rs.Fields("SubType") = "RCI" then
						border = "RCI"
					elseif rs.fields("SubType") = "II" then
						border = "II"
					elseif rs.Fields("SubType") = "Owner" then
						border = "KCP"
					else
						border = "noTour"
					end if
				Else
                    If Left(Trim(rs2.Fields("Name")), 6) = "OPC-OS"  Then
    					border = "tour"	 
	                Else
                        border = "markTour"
                    End If
                    tourID = rs2.Fields("TourID")
    			End If
				rs2.Close
                rs2.Open "Select * from t_DoNotSellList where prospectid = '" & rs.Fields("ProspectId") & "' and DateRemoved is null", cn, 3, 3
                If rs2.EOF and rs2.BOf then
                    'rs2.Close
                    'rs2.Open "Select ps.Comboitem as Status from t_Prospect p inner join t_ComboItems ps on p.StatusID = ps.ComboItemID where p.Prospectid = '" & rs.Fields("ProspectID") & "'", cn, 3, 3
                    'If rs2.EOF and rs2.BOF then
                    'Else
                     '   If rs2.Fields("Status") = "Do Not Call" then
                     '       border = "DNS"
                     '   End If
                    'End If  
                Else
                    border = "DNS"
                End If  
                rs2.Close
			%>
			  	<td width=336 style='width:3.5in;padding:0in .75pt 0in .75pt;height:2.0in'>
			  		<table width=320 border = '3' style='height:1.8in' <%if border = "tour" then %> bordercolor = #66FF66 <% elseif border = "markTour" then %> bordercolor = #A020F0 <% elseif border = "RCI" then %> bordercolor = #0066FF <%elseif border = "II" then %> bordercolor = #FFFF00 <%elseif border = "KCP" then %> bordercolor = #000000 <%elseif border = "DNS" then %> bordercolor = #FF0000 <% else %> bordercolor = #FF99FF <% end if %>>
			  		<td>
					<img src = "../../../../images/kcplogo.bmp" width="300" height="73"><br>
					<font size="1">Present this card upon time of arrival for your parking pass.</font><br>
					Unit #: <%=rooms%><br>
					Name: <%=rs.Fields("FirstName") & " " & rs.Fields("Lastname")%> <br>
					ReservationID: <%=rs.Fields("ReservationID")%>
                    &nbsp;&nbsp;<strong>TourID: <%=tourID%></strong>
                    <% if Not(isNull(rs.Fields("TourDate"))) then %>
                    <br />
                    <%
                        Dim ttime
                        If rs.Fields("TourTime") < 1000 then
                            ttime = Left(rs.Fields("TourTime"), 1) & ":" & Right(rs.Fields("TourTime"), 2) & " AM"
                        ElseIf rs.Fields("TourTime") < 1200 Then
                            ttime = Left(rs.Fields("TourTime"), 2) & ":" & Right(rs.Fields("TourTime"), 2) & " AM"
                        ElseIf rs.Fields("TourTime") < 1300 then
                            ttime = Left(rs.Fields("TourTime"), 2) & ":" & Right(rs.Fields("TourTime"), 2) & " PM"
                        Else
                            ttime = Left(rs.Fields("TourTime"), 2) - 12 & ":" & Right(rs.Fields("TourTime"), 2) & " PM"                        End If

                     %>
                    Appt Time: <%=CDate(rs.Fields("TourDate")) & " " & ttime %>
                    <%end if %>
					</td>
					</table>
	  			</td>
	  		<%
	  		i = i - 1
	  	rs.moveNext
        tourID = 0
		Loop
	End If
	rs.Close
	cn.Close
	set rs = Nothing
	set rs2 = Nothing
	set cn = Nothing
%>  	
 </tr>
</table>

<p class=MsoNormal><span style='display:none;mso-hide:all'><o:p>&nbsp;</o:p></span></p>

</div>

</body>

</html>