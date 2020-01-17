<%@ Page Title="Sales Person Efficiency Report" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PersonnelPerformanceReport.aspx.vb" Inherits="Reports_Sales_PersonnelPerformanceReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div id="animation">
<img src="../../images/progressbar.gif" alt="" id="progressbar" style="visibility:hidden" />
</div>


<div id="downdiv">
<img id="downarrow" src="../../images/expand_button2.png" alt="" style="visibility:hidden" />
</div>

<div id="criteria" style="width:500px; font-family:Cambria;">


<div style="width:120px;float:left;">
<span><strong>Tour Location</strong></span>
<br />
<asp:DropDownList runat="server" ID="ddlLocations"></asp:DropDownList>
<span><strong>Title</strong></span>
<br />
<span><select id="titles"></select></span>

<br /><br />
</div>


<div style="width:250px;float:right;">
<table style="border-collapse:collapse;">
<tr>
    <td><span>Start Date</span></td>
    <td><input type="text" id="datestart" onclick="scwShow(this,this)" /></td>
</tr>
<tr>
    <td><span>End Date</span></td>
    <td><input type="text" id="dateend" onclick="scwShow(this,this)" /></td>
</tr>
</table>

<div id="error"></div>
</div>

<div style="clear:left;">
<input type="button" value="Run Report" size="30px" id="runreport" />
</div>



</div>


<br /><br />

<div id="report"></div>


<script type="text/javascript" src="../../scripts/jquery-1.7.1.js"></script>
<script type="text/javascript">
    $.fn.emptySelect = function () {
        return this.each(function () {
            if (this.tagName == 'SELECT') this.options.length = 0;
        });
    };

    $.fn.loadSelect = function (optionsDataArray) {
        return this.emptySelect().each(function () {
            if (this.tagName == 'SELECT') {

                var selectElement = this;
                $.each(optionsDataArray, function (index, optionData) {
                    var option = new Option(optionData.Name, optionData.ID);

                    if ($.browser.msie) {
                        selectElement.add(option);
                    } else { selectElement.add(option, null); }
                });
            };
        });
    };   

</script>


<script type="text/javascript">

    $(function () {

        $("#datestart").attr("readonly", "readonly");
        $("#dateend").attr("readonly", "readonly");

        $("#progressbar").hide();
        $("#downarrow").hide();

        $.getJSON("SalesSQL.aspx?titles=1", function (data) {
            $("#titles").loadSelect(data);

            document.getElementById("progressbar").style.visibility = "visible";
            document.getElementById("downarrow").style.visibility = "visible";
        });
    });


    $("#runreport").click(function () {

        $("#error").empty();

        var datestart = $("#datestart").val();
        var dateend = $("#dateend").val();

        if (datestart == "" || dateend == "") {
            $("#error").append("<br/><strong>Date Range Can't Be Empty!</strong>").css("color","red");
            return;
        }
        var titleId = $("#titles").val();

        $("#criteria").slideUp(420, function () {
            $("#progressbar").show();
        });


        $("#error").empty();
        $("#report").empty();

        $.ajax({
            type: "POST",
            data: null,
            url: "SalesSQL.aspx?startDate=" + $("#datestart").val() + "&endDate=" + $("#dateend").val() + "&title=" + titleId + "&tourLocationID=" + $('#<%= ddlLocations.ClientID %>').val(),
            dataType: "text",
            success: function (data) {
                $("#progressbar").hide();
                $("#downarrow").show();


                $("#report").html(data).css("font-family", "Cambria");

                $("<label><input id='ID' type='checkbox'/>Personnel ID</label>")
                    .insertBefore("#reportHtml")
                    .children("input").click(function () {
                        $("#reportHtml td:nth-child(1)").toggle(this.checked);
                    });

                $("#reportHtml td:nth-child(1)").hide().css({ "color": "black", "font-weight": "bold" });

                $("#reportHtml tr:nth-child(even)").css("background-color", "#F0F0F0");

                $("#reportHtml td:nth-child(10)").css("text-align", "right");
                $("#reportHtml td:nth-child(9)").css("text-align", "right");
                $("#reportHtml td:nth-child(8)").css("text-align", "right");
                $("#reportHtml td:nth-child(7)").css("text-align", "right");
                $("#reportHtml td:nth-child(6)").css({ "text-align": "right", "width": "150px", "font-weight":"bold"});
                $("#reportHtml td:nth-child(5)").css({ "text-align": "right", "width": "150px", "font-weight": "bold" });
                $("#reportHtml td:nth-child(4)").css("text-align", "right");
                $("#reportHtml td:nth-child(3)").css("text-align", "right");


                $("#reportHtml tr:eq(0)").css({ "font-weight": "bold", "color": "#5D478B" });
                $("#reportHtml tr:last").css({ "font-weight": "bold", "color": "#CD0000", "font-size":"17px" });
            }, // success
            error: function (xhr, ajaxOptions, throwError) {

            } //error
        }); // ajax

    });

    $("#downarrow").click(function () {
        $("#criteria").slideDown(420, function () { });
        $("#downarrow").hide();
        $("#error").empty();
    });  // downarrow



</script>
</asp:Content>

