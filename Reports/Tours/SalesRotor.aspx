<%@ Page Title="Sales Rotation" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="SalesRotor.aspx.vb" Inherits="Reports_SalesRotor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <ul id="menu">
        <li><asp:LinkButton ID="DayLine1_Link" runat="server">DayLine 1</asp:LinkButton></li>
        <li><asp:LinkButton ID="InHouse_Link" runat="server">In-House</asp:LinkButton></li>
        <li><asp:LinkButton ID="DayLine1Weekly_Link" runat="server">DayLine 1 Weekly</asp:LinkButton></li>
        <li><asp:LinkButton ID="InHouseWeekly_Link" runat="server">In-House Weekly</asp:LinkButton></li>
        <li><asp:LinkButton ID="NOVA_Link" runat="server">NOVA</asp:LinkButton></li>
        <li><asp:LinkButton ID="NOVAWeekly_Link" runat="server">NOVA Weekly</asp:LinkButton></li>
    </ul>
    <button value = "Printable"   onclick = "var mwin = window.open();mwin.document.write(document.getElementById('rotor').innerHTML);">
        Printable Version</button>
    <br />
    <div id="rotor">
    <asp:Literal runat="server" id = "SalesRotor" Mode="PassThrough"></asp:Literal>
    </div>
<asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>

