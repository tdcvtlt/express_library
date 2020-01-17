<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="SalesPersonEfficiencyReport.aspx.vb" Inherits="Reports_Sales_SalesPersonEfficiencyReport" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">



<div>

<table style="border-collapse:collapse;">
<tr>
    <td>Personnel Title</td>
    <td><asp:DropDownList  ID ="ddTitle" runat="server"></asp:DropDownList></td>
</tr>
<tr>
    <td>Start Date</td>
    <td><ucDatePicker:DateField runat="server" ID="ucStartDate" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField runat="server" ID="ucEndDate" /></td>
</tr>
<tr>
    <td colspan="2"><asp:CheckBox ID="cbIncludeOW" runat="server" Text="Include OWs" /></td>
</tr>
</table>

<div>
<asp:Button ID="btRunReport" runat="server" Text="Run Report" />
<asp:Button ID="btPrintable" runat="server" Text="Printable Version" />
</div>


<asp:Literal ID="Lit1" runat="server"></asp:Literal>
</div>

  
<script type="text/javascript">


    $(function () { 
        
        
        
    
    });


</script>

</asp:Content>

