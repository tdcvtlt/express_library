<%@ Page Title="Campaigns" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Campaigns.aspx.vb" Inherits="marketing_Campaigns" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label2" runat="server" Text="Filter: "></asp:Label>
    <asp:DropDownList ID="ddFilter" runat="server">
        <asp:ListItem Value="Campaign">Campaign</asp:ListItem>
        <asp:ListItem Value="Dept">Department</asp:ListItem>
        <asp:ListItem Value="Type">Type</asp:ListItem>
        <asp:ListItem Value="ID">ID</asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />
    <asp:Label ID="Label1" runat="server" Text="Enter Campaign:"></asp:Label><br />
    <asp:TextBox ID="filter" runat="server"></asp:TextBox>
    <asp:Button ID="Button1"
        runat="server" Text="Query" />
    <asp:Button ID="btnNew" runat="server" Text="New" />
    <br />
    <div style="height:800px;width:600px;overflow:auto; ">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="True" 
    EmptyDataText="No Records" GridLines="Horizontal">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />
    </asp:GridView>
    </div>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>

