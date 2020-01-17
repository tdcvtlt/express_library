<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ReservationLetters.aspx.vb" Inherits="setup_ReservationLetters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <p>
        Letter Name:
        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" />
        <asp:Button ID="btnNew" runat="server" Text="New" />
    </p>

    <asp:GridView ID="gvLetters" runat="server" EmptyDataText = "No Records" AutoGenerateSelectButton = "true">
    </asp:GridView>
</asp:Content>

