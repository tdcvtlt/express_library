<%@ Page Title="Resort Finance Inventory" AspCompat="true" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ResortFinanceInventory.aspx.vb" Inherits="Reports_Accounting_ResortFinanceInventory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="btnScreen" runat="server" Text="To Screen" />
    <asp:Button ID="btnExcel" runat="server" Text="to Excel" />
    <br />
    <asp:Literal ID="Lit1" runat="server"></asp:Literal>
</asp:Content>

