<%@ Page Title="Golf list" AspCompat="true" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="golflist.aspx.vb" Inherits="Reports_FrontDesk_golflist" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
Start Date: <uc1:DateField ID="SDate" runat="server" />
    End Date: <uc1:DateField ID="EDate" runat="server" />
    <asp:Button ID="btnRun" runat="server" Text="Run Report" /><input type="button" onclick="var mwin = window.open();mwin.document.write(document.getElementById('printable').innerHTML);" value = "Printable Version" />
    <asp:Literal ID="Lit" runat="server"></asp:Literal>
</asp:Content>

