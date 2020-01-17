<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="RoomMatrix.aspx.vb" Inherits="frontdesk_RoomMatrix" aspcompat="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="scripts/roommatrix.js" type="text/javascript"></script>
    <script src="scripts/scw.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id = 'mon1' style="position:absolute;">Month1</div>
<div id = 'mon2' style="position:absolute;">Month2</div>
<%
for i = 1 to 480
%>
	<div id = "<%=i%>" style = "position:absolute;border:thin black solid;top:200px;left:200px;" onclick="ClickEmpty(<%=i%>,event);"></div>
<%
next
%>
<%
for i = 0 to 14
%>
	<div id = "room<%=i%>" style = "position:absolute;border:thin black solid;top:200px;left:200px;"></div>
	<div id = "roomtype<%=i%>" style = "position:absolute;border:thin black solid;top:200px;left:200px;"></div>
<%
next
%>
<div id="timemover" style="position:absolute;top:52px;left:475px; width:831px"><input type = "button" value = "<<-- 30 Days" onclick = "Last_30();" /><input type = "button" value = "<<-- 7 Days" onclick = "Last_7();" /> 
	Jump to Date:&nbsp;<input readonly onclick = "scwShow(this,this);" type="text" name="jumpdate" size="20"><input type="button" value="Go" name="B1" onclick="Jump2Date();"> <input type = "button" value = "7 Days -->>" onclick = "Next_7();" /><input type = "button" value = "30 Days -->>" onclick = "Next_30();" /></div>
<div id="roomnav" style="position:absolute;top:100px;left:325px;width:324px;"><input type = button value = "<<" onclick ="Prev_Rooms(15);" /><input type = button value = "<" onclick ="Prev_Rooms(1);" /><input type = button value = ">" onclick ="Next_Rooms(1);" /><input type = button value = ">>"  onclick ="Next_Rooms(15);" /></div>
<div id = "reservations"></div>
<div id = "message" style = "position:absolute;top:400px;left:0px;width:200px"></div>
<div id = "search" style = "position:absolute; left:280px; top:453px; width:947px">Room Search:<input type = text name = roomfilter value = ''/><input type = button value = "Search" onclick = "Room_Search();"/>&nbsp;&nbsp;&nbsp; 

</div>
<div id = "popmenu" style = "position:absolute; left:421px; top:601px; width:50px;background-color:white;visibility:hidden;">
	<div id='popedit' onmouseover="this.style.background='gray'" onmouseout="this.style.background='white';"><a href = "javascript:void(Edit_Res())"><font size="2"><b>Edit</b></font></a></div>
	<div id='popdrag' onmouseover="this.style.background='gray';" onmouseout="this.style.background='white';"><a href = "javascript:void(Drag_Res())"><font size="2"><b>Drag</b></font></a></div>
</div>
<div id = "insertmenu" style="position:absolute; left:421px; top:601px; width:50px;background-color:white;visibility:hidden; border:thin black solid">
	<div id = 'popinsert' onmouseover="this.style.background='gray';" onmouseout="this.style.background='white';"><a href="javascript:void(Insert_Res())"><font size="2"><b>Insert</b></font></a></div>
</div>
<div id = "holderentries" style = "position:absolute;height:100px;width:100px;top:490px;left:475px;width:750px;">
	<select name="holdselect" size="10" style="position:absolute;  left:255px;width:125; height:100" onclick = "Select();"></select>
   </div>
<div id = "holder" style = "position:absolute;height:100px;width:100px;top:490px;left:475px;width:750px;border:thin solid black;"></div>
<div id = "key" style="position:absolute;top:600px;left:475px;">	<table border="0" id="table1">
		<tr>
			<td colspan="9">Show Usage Assignments: <input type = "checkbox" name="showusage" onclick ="(this.checked)?Get_Usages():clear_bgs();" /></td>
			<!--<td colspan = "3">Assign Usage Types: <input type="checkbox"  name = "allocate" onclick="Allocations();"/></td>-->
		</tr>
		<tr>
			<td>Usage Key:</td>
			<td bgcolor="#FFCC66" style="color: #FFCC66; border-color: #FFCC66">...</td>
			<td>Exchange</td>
			<td bgcolor="#ffbbff" style="color: #FFBBFF; border-color: #ffbbff;">...</td>
			<td>Marketing</td>
			<td bgcolor="#33CCFF" style="color: #33CCFF; border-color: #33CCFF">...</td>
			<td>Rental</td>
			<td bgcolor="#bbffbb" style="color: #bbffbb; border-color: #bbffbb">...</td>
			<td>Use</td>
			<td bgcolor="#bbffff" style="color: #bbffbb; border-color: #bbffbb">...</td>
			<td>Developer</td>
			<td bgcolor="#ffffbb" style="color: #bbffbb; border-color: #bbffbb">...</td>
			<td>NALJR</td>
			</tr><tr><td>&nbsp;</td>
			<td bgcolor="#bbff00" style="color: #bbffbb; border-color: #bbffbb">...</td>
			<td>Plan With Tan</td>
			<td bgcolor="#00ffbb" style="color: #bbffbb; border-color: #bbffbb">...</td>
			<td>Points</td>
			<td bgcolor="#aaaaaa" style="color: #bbffbb; border-color: #bbffbb">...</td>
			<td>SRental</td>
			<td bgcolor="#66ff66" style="color: #bbffbb; border-color: #bbffbb">...</td>
			<td>Trial Owner</td>
			<td bgcolor="#ff66ff" style="color: #bbffbb; border-color: #bbffbb">...</td>
			<td>Vendor</td>
			<td bgcolor="#CC9900" style="color: #bbffbb; border-color: #bbffbb">...</td>
			<td>Points Exchange</td>
			<td>&nbsp;</td>
		</tr>
	</table></div>
<div id = "allocation" style="position:absolute;height:100px;width:100px;top:640px;left:225px;width:750px;border:thin solid black;display:none;">
	
</div>
<script type="text/javascript" language="javascript">
    Get_Rooms(); 
    Init();
</script>
</asp:Content>

