<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Receipt.aspx.vb" Inherits="general_Receipt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:scriptmanager runat="server"></asp:scriptmanager>
    <asp:multiview runat="server" id ="MultiView1">
    <asp:View runat="server" id = "KCPLOGO">
    <img src = "../images/kcplogo.bmp">
			<br><br>
			<table>
			<TR>
	  			<TD colspan = 2>King's Creek Plantation</TD>
			</TR>
			<TR>
			  	<TD colspan = 2>191 Cottage Cove Lane</TD>
			</TR>
			<TR>
	 			<TD colspan = 2>Williamsburg, VA 23185</TD>
	      	</TR>
	      	<TR>
	        	<TD colspan = 2>757-221-6760</TD>
	      	</TR>
			</table>
			<br> 

    </asp:View>
    <asp:View runat="server" id = "VRCLOGO">
    <img src = "../images/vrc.bmp">
			<br><br>			
			<table>
			<TR>
				<TD colspan = 2>Vacation Reservation Center</TD>
	 		</TR>
	      	<TR>
	      	  	<TD colspan = 2>263 McLaws Circle Suite 202</TD>
	      	</TR>
	      	<TR>
	       		<TD colspan = 2>Williamsburg, VA 23185</TD>
	      	</TR>
	      	<TR>
	        	<TD colspan = 2>757-873-4005</TD>
	      	</TR>
			</table>
			<br>

    </asp:View>
    <asp:View runat="server" id = "CELOGO">
        <img src = "../images/ClubExploreLogo.bmp" width="449" height="132">
			<br><br>
			<table>
			<TR>
	  			<TD colspan = 2>King's Creek Plantation</TD>
			</TR>
			<TR>
			  	<TD colspan = 2>191 Cottage Cove Lane</TD>
			</TR>
			<TR>
	 			<TD colspan = 2>Williamsburg, VA 23185</TD>
	      	</TR>
	      	<TR>
	        	<TD colspan = 2>757-221-6760</TD>
	      	</TR>
			</table>
			<br>

    </asp:View>
    <asp:View runat="server" id = "RFLOGO">
    
    </asp:View>
    </asp:multiview>
    
    
    <div>
        <asp:literal runat="server" id = "recpt"></asp:literal>
    </form>
</body>
</html>
