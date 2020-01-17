<%@ Page Title="UnUsed Time" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="UnUsedTime.aspx.vb" Inherits="Reports_CustomerService_UnUsedTime" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:Label runat="server" ID="label1" Text="Usage Year" Font-Bold="true"></asp:Label>&nbsp;
<asp:DropDownList runat="server" ID="ddlYear"></asp:DropDownList>
<br />
<br />
<asp:Button runat="server" ID="btnSubmit" Text="Submit" />&nbsp;
<asp:Button runat="server" ID="btnExcel" Text="Excel Export" />
<br />
<asp:GridView runat="server" ID="gvRpt" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
    RowStyle-BackColor="#A1DCF2" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#000">
</asp:GridView>

</asp:Content>

