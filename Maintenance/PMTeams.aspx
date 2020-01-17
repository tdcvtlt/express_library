<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PMTeams.aspx.vb" Inherits="Maintenance_PMTeams" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Team: <asp:TextBox runat="server" id="txtFilter"></asp:TextBox>
    <asp:Button id = "btnSearch" runat="server" Text="Search" />
    <asp:Button id = "btnNew" runat="server" Text="New" />
    <asp:GridView ID="gvTeams" runat="server" AutoGenerateSelectButton="true" EmptyDataText="No Records"></asp:GridView>
</asp:Content>

