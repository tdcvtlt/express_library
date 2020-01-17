<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CreateFixedWeekReservations.aspx.vb" Inherits="Add_Ins_CreateFixedWeekReservations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="btnRun" runat="server" Text="Create" /><asp:Button ID="btnExport" runat="server" Text="Export" /><br />
    <asp:GridView ID="gvResults" runat="server">
    </asp:GridView>
</asp:Content>

