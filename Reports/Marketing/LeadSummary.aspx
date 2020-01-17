<%@ Page Title="Lead Summary" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="LeadSummary.aspx.vb" Inherits="Reports_Marketing_LeadSummary" aspcompat = "true"%>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True">
        <asp:ListItem Text ="By Date Entered" Selected="True" Value="date_entered"></asp:ListItem>
        <asp:ListItem Text ="By Date Collected" Value="date_collected"></asp:ListItem>
        <asp:ListItem Text ="By Date Entered (File ID)" Value="date_collected_fileid"></asp:ListItem>
        <asp:ListItem Text="Leads Assigned By Source" Value="leads_assigned_by_source"></asp:ListItem>
        <asp:ListItem Text="Leads Assigned By List" Value="leads_assigned_by_list"></asp:ListItem>
    </asp:RadioButtonList>
    <table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dteSDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dteEDate" runat="server" />
            </td>
        </tr>
       
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Run Report" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfShowReport" runat="server" Value ="0" />

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</asp:Content>

