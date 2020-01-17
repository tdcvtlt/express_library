<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="RoomUsage.aspx.vb" Inherits="PropertyManagement_RoomUsage" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr>
        <td>Room:</td>
        <td><asp:DropDownList runat="server" id = "ddRooms"></asp:DropDownList></td>
    </tr>
    <Tr>
        <td>Start:</td>
        <td>
            <uc1:DateField ID="dteSDate" runat="server" />
        </td>
    </Tr>
    <Tr>
        <td>End:</td>
        <td>
            <uc1:DateField ID="dteEDate" runat="server" />
        </td>
    </Tr>
    <tr>
        <td><asp:Button runat="server" Text="Button" onclick="Unnamed1_Click"></asp:Button></td>
    </tr>
</table>

<asp:GridView runat="server" id = "gvRoomUsage" onRowDataBound = "gvRoomUsage_RowDataBound"></asp:GridView>
</asp:Content>

