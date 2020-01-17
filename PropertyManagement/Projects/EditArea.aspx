<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditArea.aspx.vb" Inherits="PropertyManagement_Projects_EditArea" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li><asp:linkbutton runat="server">Area</asp:linkbutton></li>
    </ul>
    
    <table>
        <tr>
            <td>AreaID:</td>
            <td><asp:textbox runat="server" id="txtAreaID" readonly></asp:textbox></td>
        </tr>
        <tr>
            <td>Area:</td>
            <td><asp:textbox runat="server" id="txtArea"></asp:textbox></td>
        </tr>
        <tr>
            <td>Description:</td>
            <td><asp:textbox runat="server" id="txtDescription"></asp:textbox></td>
        </tr>
    </table>
    <ul id="menu">
        <li><asp:linkbutton runat="server" id="lbSave">Save</asp:linkbutton></li>
    </ul>
</asp:Content>

