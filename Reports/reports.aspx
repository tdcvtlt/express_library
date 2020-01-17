<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="reports.aspx.vb" Inherits="Reports_reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="gvReports" runat="server" EmptyDataText="No Records" AutoGenerateSelectButton="true">
        <AlternatingRowStyle BackColor="#C7E3D7" />
    </asp:GridView>
</asp:Content>

