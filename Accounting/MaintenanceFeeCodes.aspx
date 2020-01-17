<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="MaintenanceFeeCodes.aspx.vb" Inherits="Accounting_MaintenanceFeeCodes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Label ID="Label1" runat="server" Text="Filter:"></asp:Label>
    <asp:DropDownList ID="ddfilter" runat="server">
    <asp:ListItem Value = "maintenancefeecodeid">Code ID</asp:ListItem>
    <asp:ListItem Value = "code">Code</asp:ListItem>
    </asp:DropDownList><br />
    <asp:Label ID="Label2" runat="server" Text="Value:"></asp:Label><asp:TextBox ID="txtFilter" runat="server"></asp:TextBox>  
    <asp:Button ID="btnSearch" runat="server" Text="Query" /><asp:Button ID="btnNew" runat="server" Text="New" />
    <br /><br /> 
<div style="height:350px;width:650px;overflow:auto; ">
    <asp:GridView ID="gvCodes" runat="server" EmptyDataText = "No Codes" AutoGenerateSelectButton="true" >
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />
    </asp:GridView>
</div>
<asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>

