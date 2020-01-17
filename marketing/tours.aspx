<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="tours.aspx.vb" Inherits="marketing_tours" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Enter TourID:"></asp:Label><br />
    <asp:TextBox ID="filter" runat="server"></asp:TextBox>
    <asp:Button ID="Button1"
        runat="server" Text="Query" />
    <br />
    <div style="height:200px;width:600px;overflow:auto; ">
    <asp:GridView ID="gvTours" runat="server" AutoGenerateSelectButton="True" 
    EmptyDataText="No Records" GridLines="Horizontal" onRowDataBound = "gvTours_RowDataBound">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />
    </asp:GridView>
    </div>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>


