<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CampaignSalesSummary_I.aspx.vb" Inherits="Reports_Sales_CampaignSalesSummary_I" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div>
<div style="width:500px;float:left;border:0px dotted green;">
<table>
    <tr>
        <td>Start Date:</td>
        <td colspan="2"><uc1:DateField ID="dfStartDate" runat="server" /></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>End Date:</td>
        <td colspan="2"><uc1:DateField ID="dfEndDate" runat="server" /></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>Campaign</td>
        <td><asp:ListBox runat="server" ID="listboxCampaignAll" AppendDataBoundItems="true" Height="170px"></asp:ListBox></td>
        <td><asp:Button runat="server" ID="buttonForwardCampaign" Text=">>" /><br />
        <asp:Button runat="server" ID="buttonBackwardCampaign" Text="<<" />
        </td>        
        <td><asp:ListBox runat="server" ID="listboxCampaignSelects" Height="170px"></asp:ListBox></td>
    </tr>      

    <tr>
        <td></td>
        <td colspan="4"><asp:CheckBox runat="server" ID="cb_ow" Text="include OW" /></td>
    </tr>
    
    <tr>
        <td colspan="4"><asp:Button ID="btnRunReport" runat="server" Text="Run Report" /></td>
    </tr>
</table>
</div>
</div>
<div style="clear:both;">
<br />
<asp:Literal runat="server" ID="literalReport"></asp:Literal>

<div id="crystal_report">
 <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None"/>
</div>

</div>
</asp:Content>


