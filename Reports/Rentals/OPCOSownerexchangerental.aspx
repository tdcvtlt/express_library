<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OPCOSownerexchangerental.aspx.vb" Inherits="Reports_Rentals_OPCOSownerexchangerental" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <table>
        <tr>
            <td>Date:</td>
            <td><uc1:DateField ID="sdate" runat="server" Selected_Date="" /></td>
        </tr>
        <tr>
            <td>Date:</td>
            <td><uc1:DateField ID="edate" runat="server" Selected_Date="" /></td>            
        </tr>
        <tr>
            <td colspan="2"><asp:Button ID="btn_submit" runat="server" Text="Submit Query" /></td>
        </tr>
    </table>


  <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />

</asp:Content>

