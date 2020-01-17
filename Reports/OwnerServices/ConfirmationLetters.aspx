<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ConfirmationLetters.aspx.vb" EnableViewState="true" Inherits="Reports_OwnerServices_ConfirmationLetters" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


<style type="text/css">
#poolDV input, label { line-height:1.5em;}





#popupcontainer
{
    position:absolute;
    left:0;
    top:0;
    display:none;
    z-index:200;    
}

#popupcontent
{
    background-color:#FFF;
    min-height:50px;
    min-width:175px;        
}



.popup .corner
{
    width:19px;
    height:15px;        
}


.popup .topleft
{           
    background:url('../../images/TooltipBorders/balloon_topleft.png') no-repeat;    
}
.popup .bottomleft
{           
    background:url('../../images/TooltipBorders/balloon_bottomleft.png') no-repeat;
}
.popup .topright
{
    background:url('../../images/TooltipBorders/balloon_topright.png') no-repeat;
}
.popup .bottomright
{
    background:url('../../images/TooltipBorders/balloon_bottomright.png') no-repeat;
}
.popup .top
{
    background:url('../../Images/TooltipBorders/balloon_top.png') repeat-x;   
}
.popup .bottom
{
    background:url('../../Images/TooltipBorders/balloon_bottom.png') repeat-x;   
}

.popup .left
{
    background:url('../../Images/TooltipBorders/balloon_left.png') repeat-y;   
}

.popup .right
{
    background:url('../../Images/TooltipBorders/balloon_right.png') repeat-y;   
}









</style>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div id="container" style="width:800px;border:15px solid silver;">
<br />
<div id="leftPanel" style="width:49%;float:left;margin-left:5px;">

<fieldset>
<legend id="byletterset"><strong>Confirmation Letters</strong></legend>

<div>

<h4 style="text-decoration:underline;color:Blue;">Resort Stay</h4>
<hr />
<input type="radio" name="choice" value="1" /><label>King's Creek Plantation, Williamsburg</label>

<hr/>
<h4 style="text-decoration:underline;color:Blue;">Banking</h4>

<input type="radio" name="choice" value="2" /><label>RCI Banking</label><br />
<input type="radio" name="choice" value="3" /><label>II Banking</label><br />
<input type="radio" name="choice" value="4" /><label>ICE Banking</label><br />

</div>
</fieldset>


<br />
<asp:Button ID="RunReport" runat="server" Text="Run Report" />

</div>

<div id="rightPanel" style="width:44%;float:right;margin-right:10px;margin-left:0px;padding-left:10px;border-left:1px dotted green;">

<fieldset id="bydaterangeset" style="padding-bottom:15px;">
    <legend><input type="radio" name="type" id="radiodaterange" /><strong>Date Range</strong></legend>
    <br />
    <input type="text" id="SDATE" onclick="scwShow(this,this)" style="text-align:right;" /> - 
    <input type="text" id="EDATE" onclick="scwShow(this,this)" style="text-align:right;" />
    <br />
</fieldset>

<br />

<fieldset id="byreseravationIdset" style="padding-bottom:15px;">
<br />
<legend><input type="radio" name="type" id="radioreservationid" /><strong>Reservation ID</strong></legend>
<br />
<input type="text" id="reservationID" />

<br />
<div id="poolDV" style="overflow:auto;width:280px;height:170px;border:1px solid silver;">

</div>


</fieldset>

<br />
</div>


<div id="errorDV" style="clear:both;">

</div>

<br />
</div>





<br />
<div id="ReportViewer" style="float:none;">
    <br />
</div>

    <CR:CrystalReportViewer ID="CrystalViewer" runat="server" AutoDataBind="true" ToolPanelView="None" />


<asp:HiddenField ID="hfKeys" runat="server" />

<script src="../../scripts/jquery-1.7.1.js" type="text/javascript"></script>
<script type="text/javascript">

    var arId = new Array();



    var hideDelay = 500;
    var hideTimer = null;
    var currentId;




    var popup = $("<div id='popupcontainer' style='border:0px solid red;'>"
				  + "<table width='0' border='0' cellpadding='0' cellspacing='0' align='center' class='popup'>"
				  + "<tr>"
				  + "<td class='corner topleft'></td>"
				  + "<td class='top'></td>"
				  + "<td class='corner topright'></td>"
				  + "</tr>"
				  + "<tr>"
				  + "<td class='left'>&nbsp;</td>"
				  + "<td><div id='popupcontent'></div></td>"
				  + "<td class='right'>&nbsp;</td>"
				  + "</tr>"
				  + "<tr>"
				  + "<td class='corner bottomleft'>&nbsp;</td>"
				  + "<td class='bottom'>&nbsp;</td>"
				  + "<td class='corner bottomright'></td>"
				  + "</tr>"
				  + "</table>"
				  + "</div>");


    $("body").append(popup);


    $(function () {


        $("#reservationID").bind("keydown", function (e) {

            if (e.keyCode == 13) {
                var container = $("#poolDV");
                var count = 0;

                if ($(this).val().length > 0) {
                    var obj = container.find("input");

                    count = $("#poolDV input:checkbox").length + 1;


                    var checkbox = $("<input type='checkbox' checked='yes' id='cb" + count + "' value='" + $(this).val() + "' />" +
                                    "<label for='cb" + count + "'>" + $(this).val() + "</label><br/>");

                    checkbox.filter("label").on("mouseover", function () {
                        var pos = $(this).offset();
                        var width = $(this).width();

                        if (hideTimer)
                            clearTimeout(hideTimer);

                        popup.css({
                            left: (pos.left + width) + "px",
                            top: pos.top - 5 + "px"
                        });

                        var usageId = $(this).html();
                        popup.find("#popupcontent").html("");

                        $.ajax({
                            type: "get",
                            url: "ConfirmationLetterToolTip.aspx",
                            data: { KeyId: usageId },
                            success: function (data) {

                                popup.find("#popupcontent").html(data);

                            }
                        });


                        popup.css("display", "block");
                    });

                    checkbox.filter("label").on("mouseout", function () {

                        if (hideTimer) {
                            clearTimeout(hideTimer);
                        }

                        hideTimer = setTimeout(function () {
                            popup.css("display", "none");
                        }, hideDelay);

                    });

                    if (count == 1) {
                        container.append(checkbox);
                    } else {
                        checkbox.insertBefore("#poolDV :checkbox:first");
                    }
                }
                e.preventDefault();
                $(this).val("");

                return false;

            }
        });

    });


    $("#buttonReservationID").click(function () {

        var count = $("#poolDV input:checked").length;

        $("#poolDV input:checked").each(function () {

            //alert($(this).val());
        });
        
    });


    //------------------------------
    //------------------------------
    // Error checkings...
    $("input[name$='$RunReport']").click(function () {
        var errordv = $("#errorDV");
        var errorMsg = "";

        errordv.html("");

        if ($("#leftPanel input:checked").length == 0) {

            errorMsg = "Please choose a type of Confirmation Letters";

        } else if ($("#rightPanel input:checked").length == 0) {

            errorMsg = "Please choose a type either Date Range or Reservation ID";

        } else if ($("#rightPanel input:checked").length > 0) {

            if ($("#radiodaterange").attr("checked") == "checked") {
                if (($("#SDATE").val() == "") || ($("#EDATE").val() == "")) {

                    errorMsg = "Date range is not valid.";
                }

            } else if ($("#radioreservationid").attr("checked") == "checked") {

                if ($("#poolDV input:checked").length == 0) {

                    errorMsg = "Reservations are not valid.";
                }
            }
        }

        if (errorMsg.length > 0) {
            errordv.html("<br/>" + errorMsg).css({ "font-weight": "bold", "color": "red" });
            return false;
        } else {

            var param = null;
            param = $("input[@name=choice]:checked").val();

            if ($("#radiodaterange").attr("checked") == "checked") {

                param += "&" + $("#SDATE").val() + "&" + $("#EDATE").val();
            } else if ($("#radioreservationid").attr("checked") == "checked") {
                
                $("#poolDV input:checked").each(function () {

                    param += "&" + $(this).val();
                });
            }



        }


        if (param.length > 0) {
            
            $("input[name$='$hfKeys']").val(param);
            return true;
        }
        return false;
    });


    $("#byreseravationIdset input[type='text']").click(function () {
        $("#radioreservationid").attr("checked", "yes");
    });

    $("#bydaterangeset input[type='text']").click(function () {
        $("#radiodaterange").attr("checked", "yes");
    });






 


</script>
</asp:Content>

