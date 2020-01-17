<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UserControlFromToTextBoxes.ascx.vb" Inherits="Shared_UserControls_UserControlFromToTextBoxes" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>


<div>


<table style="border-collapse:collapse">
<tr>
    <td style="width:125px"><span id="spFrom" runat="server" /></td>
    <td style="width:125px"><ucDatePicker:DateField runat="server" ID="dtFrom" /></td>
</tr>
<tr>
    <td style="width:125px"><span id="spTo" runat="server" /></td>
    <td style="width:125px"><ucDatePicker:DateField runat="server" ID="dtTo" /></td>
</tr>
</table>

</div>