<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="FutureToursWithPremiumsIssued.aspx.vb" EnableEventValidation="false" Inherits="Reports_Tours_FutureToursWithPremiumsIssued" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">

input [type='button']
{
    width:120px;
    height:25px;
}
div, input[type='text'] , select
{
    font-family:DejaVu Serif; 
    font-size:medium;
    
}

</style>


<script type="text/javascript">
    function print_tour_slip(tourID) {
        
        // assign tourID to hidden control so Print Slip button will use to pass to the stored procedure         
        $('#<%= hfTourID.ClientID %>').val(tourID);        
        __doPostBack('ctl00$ContentPlaceHolder1$btPrintSlip', 'OnClick');    
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
<table style="border-collapse:collapse">
<tr>
    <td>Location</td>
    <td><asp:DropDownList ID="ddLocation" runat="server"></asp:DropDownList></td>
    <td colspan="3"></td>
</tr>
<tr>
    <td>Campaign</td>
    <td><asp:DropDownList ID="ddCampaign" runat="server" Visible="false"></asp:DropDownList>
        <asp:ListBox runat="server" ID="lbF" Width="200" SelectionMode="Multiple" Rows="8"></asp:ListBox>
    </td>
    <td>
        &nbsp;<asp:Button runat="server" id="btSingleRight" Text=" > " Font-Bold="true" />&nbsp;        
        <br />
        &nbsp;<asp:Button runat="server" id="btMultipleRight" Text=">>" Font-Bold="true" />&nbsp;
        <br /><br />
        &nbsp;<asp:Button runat="server" id="btSingleLeft" Text=" < " Font-Bold="true" />&nbsp;
        <br /> 
        &nbsp;<asp:Button runat="server" id="btMultipleLeft" Text="<<" Font-Bold="true" />&nbsp;
    </td>
    <td>
        <asp:ListBox runat="server" ID="lbT" Width="200" SelectionMode="Multiple" Rows="8"></asp:ListBox>
    
    </td>
</tr>
<tr>
    <td>Start Date</td>
    <td><ucDatePicker:DateField runat="server" ID="sDate" /></td>
    <td colspan="3"></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField runat="server" ID="eDate" /></td>
    <td colspan="3"></td>
</tr>
<tr>
    <td><asp:Button runat="server" Text="Submit" ID="btn_Submit" /></td>
    <td>
        <asp:Button ID="btn_Export" runat="server" Text="Export to Excel" />
        <asp:Button ID="btPrintSlips" runat="server" Text="Print Tour Slips" />
        <asp:Button runat="server" ID="btPrintSlip" Text="Print Tour Slip" Enabled="false" style="position:absolute;margin-top:-1000px;margin-left:-1000px;" />
        <asp:CheckBox runat="server" ID="cbOwnersOnly" Text="Owners Only" />
        <asp:HiddenField runat="server" ID="hfTourID" Value="0" /></td>
    <td colspan="3">&nbsp;</td>
</tr>
</table>
</div>

<div>
<br />
<asp:Literal runat="server" ID="lit_report"></asp:Literal>
<asp:HiddenField runat="server" ID="hfTourIDs" />
<cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
</div>


</asp:Content>


