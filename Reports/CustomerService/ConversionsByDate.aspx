<%@ Page Title="Conversions By Date" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ConversionsByDate.aspx.vb" Inherits="Reports_CustomerService_ConversionByDate" aspcompat = "true"%>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
            <td colspan="2"><asp:Button runat="server" Text="Run Report" id="btnReport"></asp:Button></td>
        </tr>
    </table>
     <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />

<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" 
            ToolPanelView="None" />

</asp:Content>

