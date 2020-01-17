<%@ Page Title="ResGov TS Packages" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="RecGovTSPackages.aspx.vb" Inherits="Reports_CustomerService_RecGovTSPackages" %>

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
    <td><div id="xtStartErr" style="visibility:hidden"><img src="../../images/left12.gif" alt="" /></div></td>    
</tr>
<tr>
    <td>End Date</td>
    <td colspan="2"><input type="text" id="xtEnd" onclick="scwShow(this,this)" /></td>
    <td><div id="xtEndErr" style="visibility:hidden"><img src="../../images/left12.gif" alt="" /></div></td>
</tr>

</table>
<br />
&nbsp;
<div id="dvCalendarError"></div>
<div>
<input type="button" id="runreport" value="Run Report" size="45px"  />
</div>

<br /><br /><br />
</div>

<br /><br /><br />


<div id="result"></div>



<script type="text/javascript" src="../../scripts/jquery-1.7.1.js"></script>
<script type="text/javascript">

    $(function () {

        document.getElementById("progressbar").style.visibility = "visible";
        document.getElementById("xtStartErr").style.visibility = "visible";
        document.getElementById("xtEndErr").style.visibility = "visible";
        document.getElementById("expandbutton2").style.visibility = "visible";


        $("#progressbar").hide();
        $("#xtStartErr").hide();
        $("#xtEndErr").hide();
        $("#expandbutton2").hide();
    });

    $("#runreport").click(function () {

        $("#result").empty();

        $("#xtStartErr").hide();
        $("#xtEndErr").hide();

        if ($("#xtStart").val() == "") {
            $("#xtStartErr").show();
            return;
        } else if ($("#xtEnd").val() == "") {
            $("#xtEndErr").show();
            return;
        }

        $("#dvCalendar").fadeOut(420, function () { });


        $("#progressbar").show();
        $("#xtStartErr").hide();
        $("#xtEndErr").hide();


        $.ajax({
            type: "POST",
            data: null,
            url: "RecGovTSPackagesSQL.aspx?reportname=RecGovTSPackages&dateStart=" + $("#xtStart").val() + "&dateEnd=" + $("#xtEnd").val(),
            dataType: "text",
            success: function (data) {

                $("#result").html(data);
                $("#progressbar").hide();
                $("#expandbutton2").show();

            }, // sucess
            error: function (xhr, ajaxOptions, throwError) {
                alert(xhr.status + "  Serious : " + throwError);
            } // error
        }); // .ajax           
    });

    $("#expandbutton2").click(function () {
        $("#dvCalendar").fadeIn(420, function () { });
        $(this).hide();
    });

</script>
</asp:Content>


