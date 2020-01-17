<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="InventoryAvailable.aspx.vb" Inherits="Reports_Reservations_InventoryAvailable" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
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
            <td>Inventory Type:</td>
            <td>
                <uc2:Select_Item ID="siInvType" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Unit Type:</td>
            <td>
                <uc2:Select_Item ID="siUnitType" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Bedrooms:</td>
            <td>
                <asp:DropDownList ID="ddBD" runat="server">
                
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td><asp:Button ID="btnRun" runat="server" Text="Run Report" /></td>
        </tr>
    </table>
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
      <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
</asp:Content>

