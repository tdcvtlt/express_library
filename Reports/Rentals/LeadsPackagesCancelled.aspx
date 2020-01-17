<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="LeadsPackagesCancelled.aspx.vb" Inherits="Reports_Rentals_LeadsPackagesCancelled" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">

    $(function () {                
        $('#<%= CheckAll.ClientID %>').click(function () {
            $('input[type=checkbox]', $(this).nextAll('table')).attr('checked', ($(this).attr('checked') == 'checked' ? true : false));      
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
    <table>        
        <tr>
            <td>End Date:</td>
            <td><uc1:DateField ID="dfE" runat="server" /></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td></td>
        </tr>
    </table>
  
    <br />
    <div>
       
        <input type="checkbox" id="checkAll" runat="server" visible="false" /><strong><asp:Label ID="lblCheckAll" runat="server" Text="Check All" Visible="false"></asp:Label></strong>
        <br />
        <br />        
        <asp:CheckBoxList runat="server" ID="cblVendorList" RepeatLayout="Table" RepeatDirection="Vertical" RepeatColumns="7" AppendDataBoundItems="false" >           
        </asp:CheckBoxList>
    </div>
    
    <br />
    <br />
    <asp:Button ID="btnRunReport" runat="server" Text="Run Report" />
    <br />
     <br />
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None"/>
    
</div>
</asp:Content>

