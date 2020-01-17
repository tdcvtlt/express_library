<%@ Page Title="Quick Check Availability" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="QuickCheckAvail.aspx.vb" Inherits="PropertyManagement_QuickCheckAvail" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dteSDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dteEDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan = '2'>
                <asp:RadioButtonList ID="rbType" runat="server" 
                    RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True">For Reservation</asp:ListItem>
                    <asp:ListItem>For Usage</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>Nights:</td>
            <td>
                <asp:DropDownList ID="ddNights" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Usage Type:</td>
            <td>
                <asp:DropDownList ID="ddUsageType" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Unit Type:</td>
            <td>
                <asp:DropDownList ID="ddUnitType" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Room Type:</td>
            <td>
                <asp:DropDownList ID="ddRoomType" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Room SubType:</td>
            <td>
                <asp:DropDownList ID="ddRoomSubType" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Button" />
            </td>
        </tr>
    </table>

    <br />
    <asp:Literal runat="server" id = "litQC"></asp:Literal>

</asp:Content>

