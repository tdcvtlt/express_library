<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PosItemList.aspx.vb" Inherits="Reports_Accounting_PosItemList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
        RepeatDirection="Horizontal">
        <asp:ListItem Selected="True">Trading Company</asp:ListItem>
        <asp:ListItem>Boones</asp:ListItem>
    </asp:RadioButtonList>
    <asp:Button ID="Button1" runat="server" Text="Run Report" />
    <br />
    <asp:Literal ID="litReport" runat="server"></asp:Literal>
    <br />
</asp:Content>

