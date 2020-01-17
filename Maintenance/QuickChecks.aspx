<%@ Page Title="Quick Checks" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="QuickChecks.aspx.vb" Inherits="Maintenance_QuickChecks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:button ID="btnAddNew" runat="server" Text="Add New" /><br />
    <asp:GridView runat="server" id = "gvQuickChecks" AutoGenerateSelectButton = "True" EmptyDataText = "No Records"></asp:GridView>
</asp:Content>

