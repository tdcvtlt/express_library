<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OnlinePackagesSold.aspx.vb" Inherits="Reports_CustomerService_OnlinePackagesSold" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
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
            <td><asp:Button ID="btnRun" runat="server" Text="Run Report" /></td>
        </tr>
    </table>
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
      <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
</asp:Content>
