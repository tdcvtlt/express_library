<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="JW-OPCNoShows.aspx.vb" Inherits="Reports_Marketing_JW_OPCNoShows" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:datefield ID="dteSDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:datefield ID="dteEDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Run Report" />
            </td>
        </tr>
</table>
    <asp:HiddenField ID="hfShowReport" runat="server" Value ="0" />

    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</asp:Content>

