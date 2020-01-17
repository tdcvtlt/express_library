<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="premiumcostspertour.aspx.vb" Inherits="Reports_Sales_premiumcostspertour" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style type="text/css">  
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div>

<table style="border-collapse:collapse" id="criteria">
<tr>
    <td><span><strong>Start Date</strong></span></td>
    <td><div><ucDatePicker:DateField runat="server" ID="sdate" /></div></td>
</tr>
<tr>
    <td><span><strong>End Date</strong></span></td>
    <td><div><ucDatePicker:DateField runat="server" ID="edate" /></div></td>
</tr>
<tr>
    <td><asp:Button runat="server" ID="btn_submit" Text="submit" /></td>
    <td><div</div></td>
</tr>
</table>

<p></p>
 <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />


</div>

<script type="text/javascript">

    $(function () {

      

    });
</script>

</asp:Content>

