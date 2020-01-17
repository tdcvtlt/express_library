<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ActiveContractsWithOutInventory.aspx.vb" Inherits="Reports_Contracts_ActiveContractsWithOutInventory" aspCompat = "True"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="btnExcel" runat="server" Text="Excel" /><br /><br />
<asp:Literal ID="litReport" runat="server"></asp:Literal>
</asp:Content>

