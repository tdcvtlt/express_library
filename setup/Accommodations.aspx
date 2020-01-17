<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Accommodations.aspx.vb" Inherits="setup_Accommodations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <p>
        Accommodation:
        <asp:TextBox ID="txtFilter" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" />
        <asp:Button ID="btnNew" runat="server" Text="New" />
    </p>
    <div style="height:200px;width:600px;overflow:auto; ">
        <asp:GridView ID="gvAccommodations" runat="server" EmptyDataText="No Records" GridLines="Horizontal" 
            AutoGenerateColumns="true" AutoGenerateSelectButton="true" BorderStyle="None">
            <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
            <AlternatingRowStyle BackColor="#CCFFCC" />
        </asp:GridView>
 
    </div>
</asp:Content>

