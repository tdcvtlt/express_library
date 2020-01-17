<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="HOTELReport.aspx.vb" Inherits="Reports_CustomerService_HOTELReport" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>

    <table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="sdate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="edate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <span>Hotels</span></td>
            <td>
                <asp:DropDownList runat="server" ID="ddlHotels"></asp:DropDownList>
            </td>            
        </tr>
        <tr>
            <td colspan="2"><asp:Button ID="btnRun" runat="server" Text="Run Report" /></td>
        </tr>
    </table>

        <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />

        



    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />



        



</div>
</asp:Content>



