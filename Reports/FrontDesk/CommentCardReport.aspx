<%@ Page Title="Comment Card Report" AspCompat="true" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CommentCardReport.aspx.vb" Inherits="Reports_FrontDesk_CommentCardReport" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Start Date:<uc1:DateField ID="SDate" runat="server" /><br />
    End Date: <uc1:DateField ID="EDate" runat="server" /><br />
    Room Type:<uc2:Select_Item ID="siRoomType" runat="server" />
    <br />
    Room:<asp:DropDownList ID="ddRooms" runat="server"></asp:DropDownList><br />
    Report Type:<asp:DropDownList ID="ddReportType" runat="server">
        <asp:ListItem Value="0">Detail</asp:ListItem>
        <asp:ListItem Value="1">Overview</asp:ListItem>
    </asp:DropDownList><br />
    <asp:Button ID="btnRun" runat="server" Text="Run Report" /><br />
    <asp:Literal ID="Lit" runat="server"></asp:Literal>
    
</asp:Content>

