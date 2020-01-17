<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CxlPenderAmountInDateRange.aspx.vb" Inherits="Reports_Accounting_CxlPenderAmountInDateRange" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Src="~/controls/DateField.ascx" TagName="DateField" TagPrefix="uc1" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
   <table>
        <tr>
            <td>Start Date:</td>
            <td>
               <td><uc1:DateField ID="sdate" runat="server" /></td>                              
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <td><uc1:DateField ID="edate" runat="server" /></td>                        
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:Button ID="btnRun" runat="server" Text="Run Report" /></td>
        </tr>
    </table>       

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" GroupTreeStyle-ShowLines="false" ToolPanelView="None" />

</div>
</asp:Content>

