<%@ Page Title="In-House Guests" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="InHouseGuests.aspx.vb" Inherits="Reports_FrontDesk_InHouseGuests" AspCompat="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:RadioButtonList ID="rblSort" runat="server" 
        RepeatDirection="Horizontal" AutoPostBack="True">
        <asp:ListItem>By Room</asp:ListItem>
        <asp:ListItem>By Guest</asp:ListItem>
    </asp:RadioButtonList>
    <asp:Button ID="btnPrintable" runat="server" Text="Printable Version" />
    <asp:Literal ID="Lit" runat="server"></asp:Literal>
</asp:Content>

