<%@ Page Title="Occupancy By Date" AspCompat="true" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OccupancyByDate.aspx.vb" Inherits="Reports_FrontDesk_OccupancyByDate" %>


<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Start Date: <uc1:DateField ID="SDate" runat="server" />
    End Date: <uc1:DateField ID="EDate" runat="server" />
    <asp:Button ID="btnRun" runat="server" Text="Run Report" />
    <asp:Literal ID="Lit" runat="server" />
</asp:Content>

