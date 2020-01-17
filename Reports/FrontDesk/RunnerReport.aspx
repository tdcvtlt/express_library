<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="RunnerReport.aspx.vb" Inherits="Reports_FrontDesk_RunnerReport" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div>
<table style="border-collapse:collapse;">
    <tr>
        <td>Pick Up Date:</td>
        <td><uc1:DateField ID="DateField1" runat="server" /></td>
    </tr>
    <tr>
        <td>Rooms</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlRooms"></asp:DropDownList>
            <asp:Button ID="btnAdd" runat="server" Text="Add" />
        </td>
    </tr>
    <tr>
        <td>Choice(s) Selected:</td>
        <td><asp:ListBox runat="server" ID="lbRoomsSelected"></asp:ListBox></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button runat="server" ID="btnSubmit" Text="Submit" /> 
            <asp:Button ID="btnPrint" runat="server" Text="Printable Version" />
        </td>
    </tr>
</table>


</div>
</asp:Content>

