<%@ Page Title="Check-In Sheets" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CheckInSheets.aspx.vb" Inherits="Reports_FrontDesk_CheckInSheets" AspCompat="true" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    Start Date: <uc1:DateField ID="SDate" runat="server" />
    End Date: <uc1:DateField ID="EDate" runat="server" />
    <asp:Button ID="btnRun" runat="server" Text="Run Report" /><input type="button" onclick="var mWin = window.open();mWin.document.write(document.getElementById('printable').innerHTML);" value="Printable Version" /> <br />
    <asp:Literal ID="Lit" runat="server"></asp:Literal>
</asp:Content>

