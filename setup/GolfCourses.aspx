<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="GolfCourses.aspx.vb" Inherits="setup_GolfCourses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Golf Course: <asp:TextBox ID="txtFilter" runat="server"></asp:TextBox><asp:Button ID="btnSearch" runat="server" Text="Search" /><asp:Button ID="btnNew" runat="server" Text="Add New" />
<br />
    <asp:GridView ID="gvGolf" runat="server" AutoGenerateSelectButton="True" EmptyDataText ="No Records"></asp:GridView>
</asp:Content>

