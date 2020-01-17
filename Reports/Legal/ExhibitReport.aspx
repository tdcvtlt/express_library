<%@ Page Title="Exhibit Report" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ExhibitReport.aspx.vb" Inherits="Reports_Legal_SpreaderReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="width:95%;font-size:12px;">
<fieldset>

<table style="border-collapse:collapse">
<tr>
    <td>Please select an Exhibit: </td>
    <td>
    <asp:DropDownList ID="ddExhibit" runat="server" />
    </td>
    <td><asp:Button runat="server" ID="btRunReport" Text="Run Report"  /></td>
    <td><asp:Button runat="server" ID="btPrintable" Text="Printable Version"  /></td>
</tr>

<tr>
    <td></td>
    <td><asp:RadioButton runat="server" ID="r1" Text="Original" GroupName="g1" />&nbsp;<asp:RadioButton runat="server" ID="r2" GroupName="g1" Text="Edited Version" /></td>
    <td></td>    
    <td></td>
</tr>

</table>

</fieldset>

</div>

<br /><br /><br />
<div id="dvContent">

<asp:Literal ID="LIT" runat="server" />
</div>


<script type="text/javascript" src="../../scripts/jquery-1.7.1.js"></script>
<script type="text/javascript">

    $(function () {


        $('#htmlTable tr:first-child').css('text-decoration', 'none').css('font-weight', 'bold').css('text-align', 'center').css('color', 'red');
        $('#htmlTable tr:nth-child(even)').css('background-color', 'silver');
        $('#htmlTable td:nth-child(15)').css('text-align', 'right');
        $('#htmlTable td:nth-child(16)').css('text-align', 'right');
        $('#htmlTable td:nth-child(17)').css('text-align', 'right');
        $('#htmlTable td:nth-child(18)').css('text-align', 'right');


        $('#table1 thead tr:first-child').css('text-decoration', 'none').css('font-weight', 'bold').css('text-align', 'center').css('color', 'red');
        $('#table1 tr:nth-child(even)').css('background-color', 'silver');
        $('#table1 td:nth-child(9)').css('text-align', 'right');
        $('#table1 td:nth-child(13)').css('text-align', 'right');
        $('#table1 td:nth-child(14)').css('text-align', 'right');
        

    });
</script>
</asp:Content>

