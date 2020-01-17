<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PersonnelGroups.aspx.vb" Inherits="security_PersonnelGroups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
Enter Personnel Group: <asp:TextBox runat="server" id = "txtGroup"></asp:TextBox>
    <asp:Button runat="server" Text="Query" onclick="Unnamed1_Click"></asp:Button>
    <asp:Button runat="server" Text="New" onclick="Unnamed2_Click"></asp:Button>
<asp:GridView runat="server" id = "gvPersonnelGroups" EmptyDataText = "No Records" autoGenerateSelectButton = "True"></asp:GridView>

</asp:Content>

