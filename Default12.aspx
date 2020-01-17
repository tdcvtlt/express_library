<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Default12.aspx.vb" Inherits="Default12" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="GridView1" runat="server"></asp:GridView>
    <asp:DropDownList runat="server" ID="dd1">
        <asp:ListItem Text="ABC" Value="33"></asp:ListItem>
        <asp:ListItem Text="DEWS" Value="32"></asp:ListItem>
    </asp:DropDownList>
</asp:Content>

