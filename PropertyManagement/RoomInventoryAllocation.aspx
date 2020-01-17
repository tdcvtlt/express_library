<%@ Page Title="Initial Room Inventory Allocation" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="RoomInventoryAllocation.aspx.vb" Inherits="PropertyManagement_RoomInventoryAllocation" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 96px;
        }
        .auto-style1 {
            height: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td class="auto-style1"></td>
            <td class="auto-style1"><asp:CheckBox runat="server" ID="cbAddAll" Text="Add All" /></td>
        </tr>
        
        <tr>
            <td class="style1">Rooms:</td>
            <td><asp:DropDownList ID="ddRooms" runat="server" ></asp:DropDownList>                
                <asp:Button ID="btnAddRooms" runat="server" Text="Add Room" />
            </td>
        </tr>
        <tr>
            <td class="style1"></td>
            <td class="style1">
                <asp:CheckBox runat="server" ID="cbRemoveAll" Text="Remove All" />
            </td>
        </tr>
        <tr>
            <td class="style1" valign="top">Rooms Chosen:</td>
            <td valign="top"><asp:ListBox ID="lstRooms" runat="server" SelectionMode="Single"></asp:ListBox>
                <asp:Button ID="btnRemoveRoom" runat="server" Text="Remove Room" />
            </td>
        </tr>
        <tr>
            <td class="style1">Start Date:</td>
            <td><uc1:DateField ID="dfSDate" runat="server" /></td>
        </tr>
        <tr>
            <td class="style1">End Date:</td>
            <td><uc1:DateField ID="dfEDate" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnAllocate" runat="server" Text="Allocate" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" />
            </td>
        </tr>
    </table>
    <asp:Literal ID="Lit1" runat="server"></asp:Literal>
    
</asp:Content>

