<%@ Page Title="Tours By Premium Status" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ToursByPremStatus.aspx.vb" Inherits="Reports_Tours_ToursByPremStatus" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Premium Status:</td>
            <td>
                <uc1:Select_Item ID="siPremStatus" runat="server" />
            </td>
        </tr>
        <tr>
                    <td>Tour Location</td>
                    <td><uc1:Select_Item ID="siTourLocation" runat="server" /> </td>
                </tr>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc2:DateField ID="dteSDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc2:DateField ID="dteEDate" runat="server" />
            </td>
        </tr>
        <Tr>
            <td> 
                <asp:Button ID="btnRun" runat="server" Text="Run Report" />
            </td>
        </Tr>
    </table>
    <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    <br />
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
</asp:Content>

