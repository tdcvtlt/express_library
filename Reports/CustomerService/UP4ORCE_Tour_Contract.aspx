<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="UP4ORCE_Tour_Contract.aspx.vb" Inherits="Reports_CustomerService_UP4ORCE_Tour_Contract" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <table>
            <thead><tr><th></th><th></th></tr></thead>
            <tbody>
                <tr>
                    <td>Start Date</td>
                    <td><uc1:DateField ID="dfS" runat="server" /></td>
                </tr>
                <tr>
                    <td>End Date</td>
                    <td><uc1:DateField ID="dfE" runat="server" /></td>
                </tr>
                 <tr>
                    <td>Tour Location</td>
                    <td><uc2:Select_Item ID="siTourLocation" runat="server" /> </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td></td>
                    <td><asp:Button runat="server" ID="Submit" Text="Run Report" /></td>
                </tr>
            </tfoot>
        </table>

        <div>
            <cr:crystalreportviewer 
                ID="CrystalReportViewer1" 
                runat="server" 
                AutoDataBind="true" 
                ToolPanelView="None" />
        </div>

    </div>

</asp:Content>

