<%@ Page Title="Packages Sold" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PackagesSold.aspx.vb" Inherits="Reports_CustomerService_PackagesSold" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="progressbar" style="visibility:hidden;">
<img src="../../images/progressbar.gif" alt=""   />
</div>

<div id="expandbutton2" style="visibility:hidden;">
<img src="../../images/expand_button2.png" alt=""  />
</div>

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
<br />
&nbsp;
<div id="dvCalendarError"></div>
<div>
<br />
<input type="button" id="runreport" value="Run Report" size="45px"  />
</div>
</div>



<br /><br />

<div id ="result"></div>

<script type="text/javascript" src="../../scripts/jquery-1.7.1.js"></script>
<script type="text/javascript">

    $(function () {
        document.getElementById("progressbar").style.visibility = "visible";        

        $("#progressbar").hide();
        $("#expandbutton2").hide();


        $("#xtStart").attr("readonly", "readonly");
        $("#xtEnd").attr("readonly", "readonly");
    });


    $("#runreport").click(function () {

        if ($("#xtStart").val() == "" || $("#xtEnd").val() == "") {
            $("#dvCalendarError").html("<strong>Date Range Can't Be Empty.</strong>");
            return;
        }

        $("#dvCalendar").fadeOut(420, function () {

        });

        $("#progressbar").show();

        $("#result").empty();
        $("#dvCalendarError").empty();

        $.ajax({
            type: "POST",
            data: null,
            url: "PackagesSoldSQL.aspx?reportname=PackageSold&dateStart=" + $("#xtStart").val() + "&dateEnd=" + $("#xtEnd").val(),
            dataType: "text",
            success: function (data) {

                $("#progressbar").hide();
                $("#expandbutton2").show();

                $("#result").html(data);

                $("#result td:nth-child(1)").css({ "width": "160px" });
                $("#result td:nth-child(2)").css({ "width": "230px" });
                $("#result td:nth-child(3)").css({ "width": "110px", "text-align": "left" });
                $("#result td:nth-child(4)").css({ "width": "140px", "text-align": "right" });
                $("#result tr:eq(0)").css("font-weight", "bold");

                $("#dvCalendarError").html();
                $("#dvCalendarError").empty();
                $("#progressbar").hide();

                
            }, // sucess
            error: function (xhr, ajaxOptions, throwError) {
                alert(xhr.status + "  Serious : " + throwError);
            } // error
        }); // .ajax

        
        document.getElementById("expandbutton2").style.visibility = "visible";

    });


    $("#expandbutton2").click(function () {
        $("#dvCalendar").fadeIn(420, function () { });
        $(this).hide();
    });

</script>


</asp:Content>

