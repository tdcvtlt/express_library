<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OwnerFAQs.aspx.vb" Inherits="online_OwnerFAQs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="gvFAQ" runat="server" AutoGenerateSelectButton="True" 
        EmptyDataText="No Records">
    </asp:GridView><br />
    <asp:Button ID="btnAdd" runat="server" Text="Add" />
</asp:Content>

