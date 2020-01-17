<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PackageIssued.aspx.vb" Inherits="marketing_PackageIssued" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Label ID="Label2" runat="server" Text="Filter: "></asp:Label>
    <asp:DropDownList ID="ddFilter" runat="server">
        <asp:ListItem Value="ID">ID</asp:ListItem>
        <asp:ListItem Value="Name">Name</asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />
    <asp:Label ID="Label1" runat="server" Text="Label">Enter Search Value:</asp:Label><br />
    <asp:TextBox ID="pkgFilter" runat="server"></asp:TextBox><asp:Button ID="Button1" runat="server" Text="Query" />
    <br /><br />
    
    <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="True" 
    EmptyDataText="No Records" GridLines="Horizontal">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />
    </asp:GridView>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>

