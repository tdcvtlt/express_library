<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PremiumBundles.aspx.vb" Inherits="setup_PremiumBundles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Label ID="Label2" runat="server" Text="Filter: "></asp:Label>
    <asp:DropDownList ID="ddFilter" runat="server">
        <asp:ListItem Value="Name">Name</asp:ListItem>
        <asp:ListItem Value="ID">ID</asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />
<asp:Label ID="Label1" runat="server" Text="Enter Premium Bundle Name:"></asp:Label><br />
    <asp:TextBox ID="filter" runat="server"></asp:TextBox>
    <asp:Button ID="Button1"
        runat="server" Text="Query" />
    <asp:Button ID="btnNew" runat="server" Text="New" />
    <br />
    <div style="height:800px;width:600px;overflow:auto; ">
    <asp:GridView ID="gvBundles" runat="server" AutoGenerateSelectButton="True" 
    EmptyDataText="No Records" GridLines="Horizontal">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />
    </asp:GridView>
    </div>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>

