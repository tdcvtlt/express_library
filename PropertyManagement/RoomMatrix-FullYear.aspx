<%@ Page Title="Room Matrix" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="RoomMatrix-FullYear.aspx.vb" Inherits="PropertyManagement_RoomMatrix" %>

<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc1" TagName="DateField" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../scripts/scw.js" type="text/javascript"></script>
    <style>
/* Tooltip container */
.tooltip {
    position: relative;
    display: inline-block;
   
}

/* Tooltip text */
.tooltip .tooltiptext {
    visibility: hidden;
    width: 200px;
    background-color: black;
    color: #fff;
    text-align: center;
    padding: 5px 0;
    border-radius: 6px;
 
    /* Position the tooltip text - see examples below! */
    position: absolute;
    z-index: 1;
}

/* Show the tooltip text when you mouse over the tooltip container */
.tooltip:hover .tooltiptext {
    visibility: visible;
}

.link_white{
    text-decoration:none;
    color:white
}

.link_black{
    text-decoration:none;
    color:black;
}
        .auto-style1 {
            width: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>
                <asp:Button ID="btnPrev30" runat="server" Text="<< 30 Days" />
                <asp:Button ID="btnPrev7" runat="server" Text="< 7 Days" />
            </td>
            <td>Start Date:<asp:TextBox ID="dfStart" runat="server" onclick="scwShow(this,this);"></asp:TextBox><asp:Button ID="btnGo" runat="server" Text="Go" /></td>
            <td>
                <asp:Button ID="btnNext7" runat="server" Text="7 Days >" />
                <asp:Button ID="btnNext30" runat="server" Text="30 Days >>" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                Type: <asp:DropDownList ID="ddTypes" runat="server"></asp:DropDownList>
                Unit Type: <asp:DropDownList ID="ddUT" runat="server"></asp:DropDownList>
                Size: <asp:DropDownList ID="ddSize" runat="server"></asp:DropDownList>
                <asp:Button ID="btnFilter" runat="server" Text="Filter" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:TextBox ID="txtRoom" runat="server"></asp:TextBox>
                <asp:Button ID="btnGotoRoom" runat="server" Text="Go To Room" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:gridview ID="gvRoomMatrix" runat="server"  EnableViewState="False" >
                    
                </asp:gridview>
            </td>
        </tr>
    </table>
    
    <asp:Label ID="lblErr" runat="server" Text="" ForeColor="Red"></asp:Label>

    <div >	
        <table border="0" id="table1">
		    <tr>
			    <td colspan="9">Show Usage Assignments: <asp:CheckBox ID="ckShowUsage" runat="server" AutoPostBack="True" /></td>
		    </tr>
		    <tr>
			    <td>Usage Key:</td>
			<td bgcolor="#FFA500" style="color: #FFA500; border-color: #FFA500">...</td>
			<td>Exchange</td>
			<td bgcolor="#ffB6C1" style="color: #ffB6C1; border-color: #ffB6C1;" class="auto-style1">...</td>
			<td>Marketing</td>
			<td bgcolor="#0000FF" style="color: #0000FF; border-color: #0000FF">...</td>
			<td>Rental</td>
			<td bgcolor="#98FB98" style="color: #98FB98; border-color: #98FB98">...</td>
			<td>Use</td>
			</tr><tr><td>&nbsp;</td>
			<td bgcolor="#ADD8E6" style="color: #ADD8E6; border-color: #ADD8E6">...</td>
			<td>Developer</td>
			<td bgcolor="#db7093" style="color: #db7093; border-color: #db7093" class="auto-style1">...</td>
			<td>Points</td>
			<td bgcolor="#008000" style="color: #008000; border-color: #008000">...</td>
			<td>Trial Owner</td>
			<td bgcolor="#A52A2A" style="color: #A52A2A; border-color: #A52A2A">...</td>
			<td>Points Exchange</td>
			<td>&nbsp;</td>
		    </tr>
	    </table>
    </div>
</asp:Content>

