<%@ Page Title="Sales Report By Closing Officer" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="SalesReportByClosingOfficer.aspx.vb" Inherits="Reports_Contracts_SalesReportByClosingOfficer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">



<div style="margin:auto;">
<img src="../../images/progressbar.gif" alt="" id="progressbar" />
</div>

<div id="container" style="width:750px;visibility:hidden;">

<div id="expand_button2">
<img src="../../images/expand_button2.png" alt="" />
</div>

<div id="dvGrandparent">

<div id="dvCalendar">
<table>
<tr>
    <td>Start Date</td>
    <td colspan="2"><input type="text" id="xtStart" onclick="scwShow(this,this)" /></td>    
</tr>
<tr>
    <td>End Date</td>
    <td colspan="2">
        <input type="text" id="xtEnd" onclick="scwShow(this,this)" /></td>
</tr>

</table>
</div>
<br />
<div id="dvCalendarError"></div>



<br /><br />

<div>

<div id="dvPersonnelParent" style="width:350px;float:left;">
<div id="dvPersonnelSelected" style="width:250px" ></div>
<br />
<div id="dvPersonnel" style="width:250px;height:180px;overflow:auto">
</div>
</div>

<div id="dvStatusParent" style="width:350px;float:right;">

<div style="width:250px;" id="dvStatusSelected"></div>
<br />
<div style="width:250px;height:180px;overflow:auto;" id="dvStatus">
&nbsp;
</div>
</div>

<span style="clear:both"></span>

<br />

</div>



<br /><br />

<br />


<div style="float:none;clear:both;">
&nbsp;&nbsp;
<input type="button" id="btnSubmit" value="Search" />
</div>

</div>



<br /><br /><br />

<div>




</div>

<asp:Literal ID="LIT" runat="server"></asp:Literal>

<div id="result"> </div>








</div>





<script type="text/javascript" src="../../scripts/jquery-1.7.1.js"></script>

<script type="text/javascript">
   $.fn.emptySelect = function(){
        return this.each(function(){
            if (this.tagName == 'SELECT') this.options.length = 0;
        });
    };



    $.fn.loadSelect = function(optionsDataArray){
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

        $("#resultpane tr.bold").css("font-weight", "bold");
        $("#resultpane tr:nth-child(even)").css("background-color", "silver");

        $("#container").css("background-color", "FFFAF0");


        // Status
        $.getJSON("ContractSQL.aspx?GetStatus=1", function (data) {

            for (var i = 0; i < data.length; i++) {
                var cb = $("<div><input value='" + data[i].ID + "' type='checkbox' /><span>" + data[i].Name + "</span></div>");
                $("#dvStatus").append(cb);

                cb.children("input[type='checkbox']").click(function () {

                    $("#dvStatusSelected").empty();
                    var selected = $("#dvStatus div input[type='checkbox']:checked");

                    selected.each(function (i, d) {
                        var e = $("~span", this);
                        $("#dvStatusSelected").append((i == 0) ? e.text() : ", " + e.text()).css("font-weight", "bold");

                    });
                });
            }
        });

        // Status Ends





        // Personnel
        $.getJSON("ContractSQL.aspx?GetPersonnel=1", function (data) {

            if (data.length > 0) {
                $("#progressbar").hide();
                document.getElementById("container").style.visibility = "visible";
            }

            for (var i = 0; i < data.length; i++) {
                var cb = $("<div><input value='" + data[i].ID + "' type='checkbox' /><span>" + data[i].Name + "</span></div>");
                $("#dvPersonnel").append(cb);

                cb.children("input[type='checkbox']").click(function () {

                    $("#dvPersonnelSelected").empty();
                    var selected = $("#dvPersonnel div input[type='checkbox']:checked");

                    selected.each(function (i, d) {
                        var e = $("~span", this);
                        $("#dvPersonnelSelected").append((i == 0) ? e.text() : ", " + e.text()).css("font-weight", "bold");
                    });
                });
            }

        });
        // Personnel Ends


    });


    // 1109

    $("#btnSubmit").click(function () {

        $("#result").empty();


        var arStatus = new Array();
        var arPersonnel = new Array();

        var x = $("#dvPersonnel div input[type='checkbox']:checked");
        x.each(function (i, d) {
            arPersonnel.push($(this).val());
        });

        var y = $("#dvStatus div input[type='checkbox']:checked");
        y.each(function (i, d) {
            arStatus.push($(this).val());
        });

        if (arPersonnel.length == 0) {
            $("#dvPersonnelSelected").html("<div><strong>Please select status</strong></div>");
            return false;
        } else if (arStatus.length == 0) {
            $("#dvStatusSelected").html("<div><strong>Please select personnel</strong></div>");
            return false;
        } else if ($("#xtStart").val() == "" || $("#xtEnd").val() == "") {
            $("#dvCalendarError").html("<strong>Date Range Can't Be Empty.</strong>");
            return false;
        }

        else {

            $("#dvGrandparent").slideUp(420, function () {
                $("#expand_button2").hide();
                $("#progressbar").show();
            });
        }





        $.ajax({
            type: "POST",
            data: null,
            url: "ContractSQL.aspx?start=" + $("#xtStart").val() + "&end=" + $("#xtEnd").val() + "&statuses=" + arStatus.join() + "&personnels=" + arPersonnel.join(),
            dataType: "text",
            success: function (data) {
                $("#result").empty();
                $("#dvCalendarError").empty();
                $("#progressbar").hide();
                $("#expand_button2").show();
                $("#result").html(data);

                //document.getElementById("progressbar").style.visibility = "hidden";
                // needed, do not delete...
                if (data == "No Records") {

                }
                $("#progressbar").hide();
            },
            error: function (xhr, ajaxOptions, thrownError) {

                alert(xhr.status);
                alert(thrownError);
            }

        });
        return false;
    });



    $("#expand_button2").hide();

    $("#expand_button2").click(function () {
        $("#dvGrandparent").slideDown(420, function () {
            $("#expand_button2").hide();
        });
    });

</script>



</asp:Content>

