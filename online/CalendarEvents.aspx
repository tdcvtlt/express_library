<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CalendarEvents.aspx.vb" Inherits="online_CalendarEvents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Calendar: <asp:DropDownList ID="ddCal" runat="server" AutoPostBack="True">
    </asp:DropDownList>
    <asp:GridView ID="gvCal" runat="server" AllowPaging="True" 
        AutoGenerateSelectButton="True">
    
    </asp:GridView>
    <asp:Button ID="btnAdd" runat="server" Text="Add" />
</asp:Content>

