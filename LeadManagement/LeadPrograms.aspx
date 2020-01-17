<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="LeadPrograms.aspx.vb" Inherits="LeadManagement_LeadPrograms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="btnList" runat="server" Text="List" /><asp:Button ID="Button2" runat="server" Text="New" />
    <asp:GridView ID="gvLP" runat="server" AutoGenerateSelectButton ="true" EmptyDataText ="No Records">
    </asp:GridView>
</asp:Content>

