<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="BonusReport.aspx.vb" Inherits="Reports_Contracts_BonusReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <table>
        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField ID="dfSDate" runat="server" /></td>    
        </tr>
        <tr>
            <td>End Date:</td>
            <td><uc1:DateField ID="dfEdate" runat="server" /></td>
        </tr>
        <tr>
            <td>Location:</td>
            <td>
                <asp:DropDownList runat="server" id ="ddLocation">
                    <asp:ListItem>KCP</asp:ListItem>
                    <asp:ListItem>Woodbridge</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Title:</td>
            <td>
                <asp:DropDownList ID="ddTitle" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button ID="btnRun" runat="server" Text="Run Report" /></td>
        </tr>
    </table>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" 
        HasRefreshButton="True" ToolPanelView="None" />
    <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
</asp:Content>

