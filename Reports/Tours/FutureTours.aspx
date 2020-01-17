<%@ Page Title="Future Tours" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="FutureTours.aspx.vb" Inherits="Reports_Tours_FutureTours" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>
<%@ Register Src="~/controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
    $(function () {
        $('.clickable').each(function (index, row) {
            var a = $('<a href=../../marketing/edittour.aspx?tourid=' + $(row).text() + ' target=_blank>' + $(row).text() + '</a>').click(function () { });
            $(row).html(a);
        });
    });
</script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">







<div id="dvCriteria">

<table style="border-collapse:collapse">
<tr>
    <td>Location</td>
    <td><asp:DropDownList ID="ddLocation" runat="server"></asp:DropDownList></td>
    <td></td>
</tr>
<tr>
    <td>Campaign</td>
    <td><asp:DropDownList ID="ddCampaign" runat="server"></asp:DropDownList></td>
    <td></td>
</tr>
<tr>
    <td>Tour Status</td>
    <td>
        <uc2:Select_Item ID="siTourStatus" runat="server" />
    </td>
    <td></td>
</tr>
<tr>
    <td>Start Date</td>    
    <td><ucDatePicker:DateField runat="server" ID="ucStartDate" /></td>
    <td></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField runat="server" ID="ucEndDate" /></td>
    <td></td>
</tr>

</table>
<div>
    <asp:Button ID="btRunReport" Text="Run Report" runat="server" />&nbsp;&nbsp;
    <asp:Button ID="btPrintable" Text="Printable Version" runat="server" />
    <asp:Button ID="btExportToExcel" Text="Export To Excel" runat="server" />    
</div>

</div>

<br />
<div>
<asp:GridView ID="gvReport" runat="server"></asp:GridView>

    <asp:Literal ID="Lit1" runat="server"></asp:Literal>

</div>
<script type="text/javascript">


</script>
</asp:Content>

