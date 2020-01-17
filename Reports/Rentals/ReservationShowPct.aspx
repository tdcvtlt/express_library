<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ReservationShowPct.aspx.vb" Inherits="Reports_Rentals_ReservationShowPct" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
                <tr>
            <td>
                Location(s):</td>
            <td align="center">
                &nbsp;</td>
            <td>
                Selected:</td>
        </tr>
        <tr>
            <td>
                <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
            </td>
            <td align="center">
                <asp:Button ID="Button4" runat="server" Text="<< ALL" style="height: 29px" /><br />
                <asp:Button ID="Button1" runat="server" Text="<<" /><br />
                <asp:Button ID="Button2" runat="server" Text=">>" /><br />
                <asp:Button ID="Button5" runat="server" Text="ALL >>" />
            </td>
            <td>
                <asp:ListBox ID="ListBox2" runat="server"></asp:ListBox>
            </td>
        </tr>

        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField ID="dfStartDate" runat="server" /></td>
        </tr>
        <tr>
           <td>End Date:</td>
            <td><uc1:DateField ID="dfEndDate" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button ID="btnRunReport" runat="server" Text="Run Report" /></td>
        </tr>
    </table>
  <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    
    
    
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
    
    
    
</asp:Content>



