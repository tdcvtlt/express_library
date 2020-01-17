<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ReDeeds.aspx.vb" Inherits="Reports_Contracts_Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id = "div2">
    <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="true" 
        EmptyDataText="No Records" GridLines="Horizontal" 
        AutoGenerateSelectButton="True">
        <selectedrowstyle BackColor="#CCFFFF" Wrap="true" />
        <alternatingrowstyle BackColor="#CCFFCC" />
    </asp:GridView>
</div>
</asp:Content>

