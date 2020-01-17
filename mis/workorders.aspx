<%@ Page Title="Work Orders" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="workorders.aspx.vb" Inherits="mis_workorders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Enter Work Order ID:"></asp:Label><br />
    
    <asp:TextBox ID="txtFilter" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" Text="Query" /><br />
    <asp:GridView ID="gvWorkOrder" runat="server" 
    AutoGenerateSelectButton="True" onRowDataBound = "gvWorkOrder_RowDataBound">
        <AlternatingRowStyle BackColor="#66FF99" />
    </asp:GridView>
</asp:Content>

