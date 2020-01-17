<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="InvalidResWithRoom.aspx.vb" Inherits="Reports_Reservations_InvalidResWithRoom" AspCompat = "true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Button runat="server" Text="Run Report" onclick="Unnamed1_Click"></asp:Button>
<br />
<asp:Literal runat="server" id = "litReport"></asp:Literal>
</asp:Content>

