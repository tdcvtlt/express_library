<%@ Page Title="Check-In Letter" AspCompat="true" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CheckInLetter.aspx.vb" Inherits="Reports_FrontDesk_CheckInLetter" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
Check-In Date:<uc1:DateField ID="dfCheckIn" runat="server" />
    <asp:Button ID="btnRun" runat="server" Text="Run Report" /><input type="button" onclick="var mWin = window.open();mWin.document.write(document.getElementById('printable').innerHTML);" value="Printable Version" />
    <asp:Literal ID="Lit" runat="server"></asp:Literal>
</asp:Content>

