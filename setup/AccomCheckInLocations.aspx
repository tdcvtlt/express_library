<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="AccomCheckInLocations.aspx.vb" Inherits="setup_AccomCheckInLocations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <p>
        Location:
        <asp:TextBox ID="txtFilter" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Search" />
        <asp:Button ID="Button2" runat="server" Text="New" />
    </p>

    <asp:GridView ID="gvLocations" runat="server" AutoGenerateSelectButton = "true" EmptyDataText = "No Records">
    </asp:GridView>
</asp:Content>

