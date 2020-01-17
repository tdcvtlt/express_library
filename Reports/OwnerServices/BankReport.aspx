<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="BankReport.aspx.vb" Inherits="Reports_OwnerServices_BankReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="container" style="width:500px;border:1px dotted maroon;font-family:Cambria;">
<h2>Bank Report</h2>
<br />


<fieldset>
<legend><strong>Unit:</strong></legend>
<table>
<tr>
<td style="width:110px">Size:</td>
<td style="width:100px"><asp:DropDownList ID="Size" runat="server"></asp:DropDownList></td>
<td style="width:15px" colspan="2">&nbsp;</td>
<td style="width:60px">Type:</td>
<td style="width:80px"><asp:DropDownList ID="Type" runat="server"></asp:DropDownList></td>
</tr>
</table>
</fieldset>
<br />


<fieldset>
<legend><strong>Year:</strong></legend>
<table>
<tr>
<td style="width:110px">Deposit (Year Used):</td>
<td style="width:100px"><asp:DropDownList ID="Deposit" runat="server"></asp:DropDownList></td>
<td style="width:15px" colspan="2">&nbsp;</td>
<td style="width:60px">Usage Year:</td>
<td style="width:80px"><asp:DropDownList ID="Used" runat="server"></asp:DropDownList></td>
</tr>
</table>
</fieldset>

<fieldset>

<br /><br />
<legend><strong>Additional:</strong></legend>
<table>
<tr>
<td style="width:110px">Exchange Co.:</td>
<td style="width:100px"><asp:DropDownList ID="Exchange" runat="server"></asp:DropDownList></td>
<td style="width:15px" colspan="2">&nbsp;</td>
<td style="width:60px">Usage</td>
<td style="width:80px"><asp:DropDownList ID="Usage" runat="server"></asp:DropDownList></td>
</tr>

<tr>
<td style="width:110px">Season:</td>
<td style="width:100px"><asp:DropDownList ID="Season" runat="server"></asp:DropDownList></td>
<td style="width:15px" colspan="2">&nbsp;</td>
<td style="width:60px">Status:</td>
<td style="width:80px"><asp:DropDownList ID="Status" runat="server"></asp:DropDownList></td>
</tr>

</table>
</fieldset>

<br />




<div>
<!--<img src="../../images/RunReport/RunReport3.jpg" alt="" id="RunReport"  />-->
</div>




<br />
    <div style="font-size:75px;text-align:center">
        <asp:Label runat="server" ID="ResultCount"></asp:Label>
    </div>

    <br />
    <asp:Button runat="server" ID="btSubmit" Text="Submit" />

</div>



<script type="text/javascript" src="../../scripts/jquery-1.7.1.js"></script>
<script type="text/javascript">

    $(function () {        
    });

    //$("#container select").change(function () {

    //    $.ajax({
    //        type: "post",
    //        data: { "UnitSize": $("select[name$='$Size'] option:selected").text(),
    //            "UnitType": $("select[name$='$Type'] option:selected").val(),
    //            "DepositYear": $("select[name$='$Deposit'] option:selected").text(),
    //            "YearUsed": $("select[name$='$Used'] option:selected").text(),
    //            "Exchange": $("select[name$='$Exchange'] option:selected").val(),
    //            "Usage": $("select[name$='$Usage'] option:selected").val(),
    //            "Season": $("select[name$='$Season'] option:selected").val(),
    //            "Status": $("select[name$='$Status'] option:selected").val()
    //        },
    //        url: "HandlerBankReport.ashx",
    //        dataType: "text",
    //        success: function (data) {
    //            $("#ResultTotal").html(data);
    //        }
    //    });

    //}).trigger("change");

    //$("#RunReport").click(function () {

    //    $.ajax({
    //        type: "post",
    //        data: { "UnitSize": $("select[name$='$Size'] option:selected").text(),
    //            "UnitType": $("select[name$='$Type'] option:selected").val(),
    //            "DepositYear": $("select[name$='$Deposit'] option:selected").text(),
    //            "YearUsed": $("select[name$='$Used'] option:selected").text(),
    //            "Exchange": $("select[name$='$Exchange'] option:selected").val(),
    //            "Usage": $("select[name$='$Usage'] option:selected").val(),
    //            "Season": $("select[name$='$Season'] option:selected").val(),
    //            "Status": $("select[name$='$Status'] option:selected").val()
    //        },
    //        url: "HandlerBankReport.ashx",
    //        dataType: "text",
    //        success: function (data) {
    //            $("#ResultTotal").html(data);
    //        }
    //    });
    //});


</script>
</asp:Content>

