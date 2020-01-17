<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="TradeShowCancelledPackages.aspx.vb" Inherits="Reports_HR_TradeShowCancelledPackages" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
input [type="submit"] , [type='text']
{
    font-family:DejaVu Sans Light;
    font-size:medium;
    background-color:#c00;     
    color:#fff;
}
</style>
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
            <td>
                <asp:Button ID="Button1" runat="server" Text="Run Report" />
                <asp:Button ID="Button2" runat="server" Text="Excel" />
            </td>
        </tr>
    </table>

    <asp:Literal runat="server" id = "litReport"></asp:Literal>
</asp:Content>

