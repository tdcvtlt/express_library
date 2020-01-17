<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ContractsWithChangedStatus.aspx.vb" Inherits="Reports_Contracts_ContractsWithChangedStatus" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="dtc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="container" style="font-family:Arial">

<div style="width:900px;border:dotted 0px Silver">

<div id="checkboxlistDV" style="float:left;width:390px;height:300px;overflow:auto; border:1px solid silver;">
<asp:CheckBoxList ID="ContractStatus" runat="server" CellSpacing="10" CellPadding="5" 
    RepeatDirection="Vertical" RepeatColumns="2"></asp:CheckBoxList>
    <br /><br />
</div>

<div style="float:right;width:40%;border:0px solid blue;margin-right:100px;">

<div id="criteria">
    <table style="border-collapse:collapse;">
        <tr>
            <td>Start Date:</td>
            <td><dtc:DateField ID="SDATE" runat="server" /></td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td><dtc:DateField ID="EDATE" runat="server" /></td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td><asp:Button ID="RunReport" Text="Run Report" runat="server" /></td>
            <td>&nbsp;</td>
        </tr>
    </table>    
</div>

<br />
<div id="errorDV"></div>

</div>


<br />


    
</div>

<div id="ReportViewer" style="clear:both;float:none;">
<br />
<br />
<br />
    <CR:CrystalReportViewer ID="CrystalViewer" runat="server" />
</div>


<br />
</div>



<script src="../../scripts/jquery-1.7.1.js" type="text/javascript"></script>
<script type="text/javascript">   

    $(function () {
        $("<label><input type='checkbox' id='checkboxAll' />&nbsp;All</label><br/>")
            .insertBefore("#checkboxlistDV")
            .children("input")
            .click(function () {
                var checked = $(this).attr("checked");
                if (checked == "checked") {
                    $("#checkboxlistDV input[type='checkbox']").each(function () {
                        $(this).attr("checked", checked);
                    });
                } else {
                    $("#checkboxlistDV input[type='checkbox']").each(function () {
                        $(this).removeAttr("checked");
                    });                    
                }
            });
        });

    $("#criteria input[name$='RunReport']").click(function () {

        if ($("input[name$='$SDATE$txtDate']").val() == "") {

            $("#errorDV").html("<br/><br/><strong>Start Date Value Missing.</strong>");
            return false;
        } else if ($("input[name$='$EDATE$txtDate']").val() == "") {

            $("#errorDV").html("<br/><br/><strong>End Date Value Missing.</strong>");
            return false;
        } else {
            return true;
        }
    });
    

</script>

</asp:Content>

