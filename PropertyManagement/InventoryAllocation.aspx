<%@ Page Title="Inventory Allocation" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="InventoryAllocation.aspx.vb" Inherits="PropertyManagement_InventoryAllocation"%>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            height: 33px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
           <tr>
                <td>Allocation Type:</td>
                <td>
                    <uc1:select_item ID="siType" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Rooms:</td>
                <td><asp:dropdownlist runat="server" id = "ddRooms"></asp:dropdownlist>
                    <asp:button runat="server" text="Add Room" onclick="Unnamed1_Click" autopostback = "false"/>
                </td>
                <td rowspan = '4'>
                    <asp:listbox runat="server" Height="157px" Width="186px" id = "lbRooms"></asp:listbox>
                </td>
                <td class="auto-style1"><asp:Button runat="server" Text="Remove" onclick="Unnamed2_Click" style="height: 29px"></asp:Button></td>
            </tr>
            <tr><td></td></tr>
            <tr><td></td></tr>
            <tr><td></td></tr>

            <tr>
                <td>Start Date:</td>
                <td>
                    <uc2:datefield ID="dteStartDate" runat="server" />
                </td>
                <td rowspan ="4"><asp:listbox runat="server" Height="157px" Width="186px" id = "lbDates"></asp:listbox></td>
                <td class="auto-style1"><asp:Button runat="server" Text="Remove" onclick="Unnamed5_Click"></asp:Button></td>

            </tr>
            <tr>
                <td>End Date:</td>
                <td>
                    <uc2:datefield ID="dteEndDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="Add Date Range" />
                </td>
            </tr>
            <tr>
                <td><asp:Button runat="server" Text="Submit" onclick="Unnamed3_Click" style="height: 29px"></asp:Button></td>
            </tr>
        </table>
</asp:Content>

