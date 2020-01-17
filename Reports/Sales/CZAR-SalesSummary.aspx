<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CZAR-SalesSummary.aspx.vb" Inherits="Reports_Sales_CZAR_SalesSummary" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style type="text/css">

#main-wrapper
{
    font-family:Cambria;
    margin:0;
    padding:0;
    font-size:1.5em;
}
</style>






</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="main-wrapper">
<h2>CZAR Sales Summary</h2>
</div>


<div>
<table style="border-collapse:collapse">
<tr>
    <td colspan="2">&nbsp;</td>
</tr>
<tr>
    <td>Start Date</td>
    <td><ucDatePicker:DateField runat="server" ID="dpFrom" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField runat="server" ID="dpTo" /></td>
</tr>
<tr>
    <td></td>
    <td><asp:CheckBox ID="CheckBoxOW" runat="server" Text="Include OW" /></td>
</tr>
</table>
</div>

  <div>
        <asp:Button runat="server" ID="btRunReport" Text="Run Report" />
    </div>
    
    
<br /><br />
<div>

  
</div>


<asp:Literal runat="server" ID="Literal_SQL"></asp:Literal>
<br /><br />
<asp:Literal runat="server" ID="Literal_Result"></asp:Literal>






<script type="text/javascript">

    $("#table_result").css({ "border-collapse": "collapse", "font-family": "Cambria" });
    $("#table_result td").css({ "width": "120px" });
    $("#table_result tr:first").css({ "font-size": "16px", "font-weight": "bold", "padding": "25px" });


    $(function () {

        var sum = 0;

        $("#table_result tr:not(:first)").each(function () {


            if (($("td:eq(1)", $(this)).text().length > 0)) {
                sum += parseInt($("td:eq(1)", $(this)).text());
            }
            for (var i = 1; i < 10; i++) {

                $("#table_result td:eq(" + i + ")", $(this)).css({ "text-align": "right" });
            }




        });



        $("#table_result tr:last").css({ "font-size": "18px", "font-weight": "bold",
                    "color":"red" });
    });



</script>


</asp:Content>

