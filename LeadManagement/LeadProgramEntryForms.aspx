<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="LeadProgramEntryForms.aspx.vb" Inherits="LeadManagement_LeadProgramEntryForms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="btnList" runat="server" Text="List" />
    <asp:Button ID="btnNew" runat="server" Text="New" />
    <asp:GridView ID="gvEntryForms" runat="server" AutoGenerateSelectButton ="true" EmptyDataText ="No Records"></asp:GridView>
</asp:Content>

