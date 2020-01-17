<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="AllocationMismatches.aspx.vb" Inherits="Reports_OwnerServices_AllocationMismatches" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
<table style="border-collapse:collapse;">
<tr>
    <td>Start Date</td>
    <td><ucDatePicker:DateField ID="SDate" runat="server" Selected_Date="" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField ID="EDate" runat="server" Selected_Date="" /></td>
</tr>

</table>


<br />
<div>
<asp:ImageButton runat="server" ID="RunReport" ImageUrl="~/images/RunReport/RunReport1.jpg"/>
</div>







<br />


<div id="reportviewer">
    <CR:CrystalReportViewer ID="CrystalViewer" runat="server" AutoDataBind="true"  HasCrystalLogo="False"  />
</div>
    
</div>

</asp:Content>

