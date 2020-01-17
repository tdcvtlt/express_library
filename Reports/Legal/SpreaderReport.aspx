<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="SpreaderReport.aspx.vb" Inherits="Reports_Legal_SpreaderReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="width:95%;font-size:12px;">
<fieldset>

<table style="border-collapse:collapse">
<tr>
    <td>Please select an Exhibit: </td>
    <td><asp:TextBox runat="server" ID="tbExhibit"></asp:TextBox></td>
    <td><asp:Button runat="server" ID="btRunReport" Text="Run Report"  /></td>
    <td><asp:Button runat="server" ID="btPrintable" Text="Printable Version"  /></td>
</tr>

</table>

</fieldset>

</div>

<div id="dvContent">


</div>

</asp:Content>

