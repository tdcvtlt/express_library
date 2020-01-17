<%@ Page Title="Rooms In/Out of Service" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OutofService.aspx.vb" Inherits="PropertyManagement_OutofService" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc2" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr>
    <td colspan = '3'><asp:RadioButtonList runat="server" RepeatDirection="Horizontal" id = "rbService">
        <asp:ListItem Selected="True">Take Out of Service</asp:ListItem>
        <asp:ListItem>Put Into Service</asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <tr>
        <td>Rooms:</td>
        <td><asp:dropdownlist runat="server" id = "ddRooms"></asp:dropdownlist>
        <asp:button runat="server" text="Add Room" onclick="Unnamed1_Click" autopostback = "false"/>
        </td>
        <td rowspan = '4'>
        <asp:listbox runat="server" Height="157px" Width="186px" id = "lbRooms"></asp:listbox>
        </td>
        <td><asp:Button runat="server" Text="Remove" onclick="Unnamed2_Click"></asp:Button></td>
    </tr>
    <tr>
        <td>Start Date:</td>
        <td>
        <uc2:datefield ID="dteStartDate" runat="server" />
        </td>
    </tr>
    <tr>
        <td>End Date:</td>
        <td>
        <uc2:datefield ID="dteEndDate" runat="server" />
        </td>
    </tr>
    <tr>
        <td>Reason:</td>
        <td>
            <uc1:Select_Item ID="siReason" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            Note:
        </td>
        <td>
            <asp:TextBox runat="server" id = "txtNote"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td><asp:Button runat="server" Text="Submit" onclick="Unnamed3_Click"></asp:Button></td>
    </tr>
</table>
<div style="height: 350px" style = "overflow:auto">
<asp:Label runat="server" id = "lblMsg"></asp:Label>
<asp:Label runat="server" id = "lblErr"></asp:Label>
</div>
</asp:Content>

