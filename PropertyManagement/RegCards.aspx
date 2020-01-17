<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="RegCards.aspx.vb" Inherits="PropertyManagement_RegCards" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="gvRegCards" runat="server" EmptyDataText = "No Reg Cards" AutoGenerateSelectButton = "true">
    </asp:GridView>
    <asp:Button ID="Button1" runat="server" Text="Add Reg Card" />
</asp:Content>

