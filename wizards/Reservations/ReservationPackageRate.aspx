<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReservationPackageRate.aspx.vb" Inherits="ReservationPackageRate" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

   

	<link rel="stylesheet" href="../../Content/bootstrap.min.css" />
    <link rel="stylesheet" href="../../Scripts/jquery-ui-1.12.0.custom/jquery-ui.min.css" />
    <link rel="stylesheet" href="../../Scripts/jquery-ui-1.12.0.custom/jquery-ui.theme.min.css" />
    <script type="text/javascript" src="../../Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery.validate.min.js"></script>
    <script type="text/javascript" src="../../Styles/bootstrap-3.3.5/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../../Scripts/noty/jquery.noty.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui-1.12.0.custom/jquery-ui.min.js"></script>

    <style type="text/css">
        .top-buffer { margin-top:20px; }

        .blink_me {
            -webkit-animation-name: blinker;
            -webkit-animation-duration: 1s;
            -webkit-animation-timing-function: linear;
            -webkit-animation-iteration-count: infinite;
    
            -moz-animation-name: blinker;
            -moz-animation-duration: 1s;
            -moz-animation-timing-function: linear;
            -moz-animation-iteration-count: infinite;
    
            animation-name: blinker;
            animation-duration: 1s;
            animation-timing-function: linear;
            animation-iteration-count: infinite;
        }

        @-moz-keyframes blinker {  
            0% { opacity: 1.0; }
            50% { opacity: 0.0; }
            100% { opacity: 1.0; }
        }

        @-webkit-keyframes blinker {  
            0% { opacity: 1.0; }
            50% { opacity: 0.0; }
            100% { opacity: 1.0; }
        }

        @keyframes blinker {  
            0% { opacity: 1.0; }
            50% { opacity: 0.0; }
            100% { opacity: 1.0; }
        }

    </style>

    <script type="text/javascript">

        $(function () {

            var campaignID = 0, tourLocationID = 0, packageID = 0, packageReservationID = 0, nights = 0;
            var dateCheckIn;

            $('input[type=radio]').on('click', function (event) {
                
                var val = $(this).val();                
                $.ajax({
                    type: "GET",
                    url: "ReservationPackageRate.aspx/AjaxListPackagesByTypeID",
                    data: {"typeID": val.toString(), "type": "'Hello'".toString()},
                    contentType: "application/json; charset=utf-8",
                    dataType:"json",
                    success: function (data) {
                        $('#ddlPackagesByTypeID').empty();
                        $('#tbxPackageID').val("");
                        $('#tbxUnitType').val("");
                        $('#tbxBedRooms').val("");
                        $('#tbxMinNights').val("");

                        $('#tbxSource').val("");
                        $('#tbxStartDate').val("");
                        $('#tbxEndDate').val("");

                        $('#tbxVendor').val("");
                        $('#tbxPromoNights').val("");
                        $('#tbxPromoRate').val("");
                        $('#tbxReservationType').val("");

                        $('#ckbAllowExtraNight').prop("checked", false);
                        $('#<%= tbxCampaign.ClientID %>').val("");
                        $('#<%= tbxTourLocation.ClientID %>').val("");
                        $('#dvToursAvailability').empty();

                        nights = 0;
                        dateCheckIn = null;
                        packageID = 0;
                        packageReservationID = 0;
                        $('#dvInvoices').empty();

                        var js = JSON.parse(data.d);
                        $.each(js, function (index, element) {
                            $('#ddlPackagesByTypeID').append($('<option>', {value:element.ID, text:element.Package}));
                        })
                    }
                });                
            });




            // 
            $('#ddlPackagesByTypeID').on('change', function (event) {
                $(this).trigger('resetForm');

                $.ajax({
                    type: "GET",
                    url: "ReservationPackageRate.aspx/AjaxGetPackageDetail",
                    data: { "packageID": $(this).val().toString()},
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {                       
                        var element = JSON.parse(data.d);

                        $('#tbxPackageID').val(element.ID);
                        $('#tbxUnitType').val(element.UnitType);
                        $('#tbxBedRooms').val(element.BedRooms);
                        $('#tbxMinNights').val(element.MinNights);

                        $('#tbxSource').val(element.Source);
                        $('#tbxStartDate').val(element.StartDate);
                        $('#tbxEndDate').val(element.EndDate);

                        $('#tbxVendor').val(element.Vendor);
                        $('#tbxPromoNights').val(element.PromoNights);
                        $('#tbxPromoRate').val(element.PromoRate);
                        $('#tbxReservationType').val(element.ReservationType);                        
                        $('#tbxDefaultInvoiceAmt').val(element.DefaultInvoiceAmt);
                        $('#ckbAllowExtraNight').prop("checked", element.AllowExtraNight);

                        $('#<%= tbxCampaign.ClientID %>').val(element.Campaign);
                        $('#<%= tbxTourLocation.ClientID %>').val(element.TourLocation);


                        campaignID = element.CampaignID;
                        tourLocationID = element.TourLocationID;
                        packageID = element.ID;
                        packageReservationID = element.PackageReservationID;
                        
                    }
                });
                
            });

            $('#<%= tbxDateCheckIn.ClientID %>').datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                onSelect: function () {
                    
                    nights = $('#<%= ddlNights.ClientID %>').val();                    
                    var checkOut = new Date(new Date($(this).val()).getTime() + (nights * 24 * 60 * 60 * 1000)).toLocaleDateString("en-US");
                    $('#<%= tbxDateCheckOut.ClientID %>').val(checkOut);

                    $('#btnSetMinNights').trigger('click');
                }
            });


            
            // dropdown nights updates the checkin calendar
            $('#<%= ddlNights.ClientID %>').change(function () {                
                var checkIn = $('#<%= tbxDateCheckIn.ClientID %>').datepicker('getDate');
                nights = $(this).val();
                var checkOut = new Date(new Date(checkIn).getTime() + (nights * 24 * 60 * 60 * 1000));
                $('#<%= tbxDateCheckOut.ClientID %>').val(new Date(checkOut).toLocaleDateString("en-US"));                
            });

            $('#btnCheckToursAvail', '#ddlPackagesByTypeID').on('resetForm', function (event) {
                $('#tbxPackageID').val("");
                $('#tbxUnitType').val("");
                $('#tbxBedRooms').val("");
                $('#tbxMinNights').val("");

                $('#tbxSource').val("");
                $('#tbxStartDate').val("");
                $('#tbxEndDate').val("");

                $('#tbxVendor').val("");
                $('#tbxPromoNights').val("");
                $('#tbxPromoRate').val("");
                $('#tbxReservationType').val("");

                $('#ckbAllowExtraNight').prop("checked", false);
                $('#<%= tbxCampaign.ClientID %>').val("");
                $('#<%= tbxTourLocation.ClientID %>').val("");                
                $('#<%= tbxDefaultInvoiceAmt.ClientID %>').val("");
                $('#dvToursAvailability').empty();
                nights = 0;
                dateCheckIn = null;
                packageID = 0;
                packageReservationID = 0;
            });

            // check for tours availability
            $('#btnCheckToursAvail').on('click', function (event) {                
                if ($('#<%= tbxDateCheckIn.ClientID %>').val().toString() == "") return;
                var vDate = "'" + $('#<%= tbxDateCheckIn.ClientID %>').val().toString().replace(/\//g, "|") + "'";
                $('#dvToursAvailability').empty();
                $(this).trigger('resetForm');

                $.ajax({
                    type: "GET",
                    url: "ReservationPackageRate.aspx/CheckToursAvailability",
                    data: { "dateCheckIn": vDate, "nights": $('#<%= ddlNights.ClientID %>').val().toString(), "campaignID": campaignID.toString(), "tourLocationID": tourLocationID.toString() },
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (data) {                        
                        var js = JSON.parse(data.d);
                        var $table = $('<table/>');
                        $table.append("<thead><tr><th>Tour Date</th><th>Tour Time</th></tr></thead>");
                        $table.addClass("table table-striped");
                        $.each(js, function (index, element) {
                            $table.append('<tr><td>' + element.TourDate + '</td><td>' + element.TourTime + '</td></tr>');                           
                        })
                        $('#dvToursAvailability').append($table);
                    }
                });                
            });



            // Calculate Invoice
            $('#btnCalculateInvoice').on('click', function (event) {
                $('#dvInvoices').empty();
                $('#dvToursAvailability').empty();
                if ($('#<%= tbxDateCheckIn.ClientID %>').val().toString() == "") return;

                var vDate = "'" + $('#<%= tbxDateCheckIn.ClientID %>').val().toString().replace(/\//g, "|") + "'";                

                $.ajax({
                    type: "GET",
                    url: "ReservationPackageRate.aspx/AjaxGetInvoices",
                    data: { "packageID": packageID, "packageReservationID": packageReservationID, "dateCheckIn": vDate, "nights": nights },
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var invoiceLines = JSON.parse(data.d);
                        var $table = $('<table/>');
                        $table.append("<thead><tr><th>INVOICE</th><th></th><th>AMOUNT $</th></tr></thead>");
                        $table.addClass("table table-striped");
                        $.each(invoiceLines, function (index, element) {
                            $table.append('<tr class=primary><td>' + element.InvoiceLine + '</td><td></td><td>' + element.InvoiceAmount.toFixed(2) + '</td></tr>');                                                  
                            $.each(element.AjaxPayments, function (plIndex, payment) {
                                var pA = new Number(payment.PaymentAmount);                                
                                $table.append("<tr class=warning><td></td><td>" + payment.PaymentLine + "</td><td>" + pA.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,') + "</td></tr>");
                            });                                                      
                        })
                        $('#dvInvoices').append($table);
                    }
                });
                
            });


            // set min night
            $('#btnSetMinNights').on('click', function (event) {

                var vDate = "'" + $('#<%= tbxDateCheckIn.ClientID %>').val().toString().replace(/\//g, "|") + "'";
                if ($('#<%= tbxMinNights.ClientID %>').val() > 1 && $('#<%= tbxDateCheckIn.ClientID %>').val().length > 0) {
                    $.ajax({
                        type: "GET",
                        url: "ReservationPackageRate.aspx/AjaxIsExtraNightAllowed",
                        data: { "packageReservationID": packageReservationID, "dateCheckIn": vDate },
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var js = JSON.parse(data.d);
                            $.each(js, function (index, element) {
                                if (element.IsAllowed == true) {
                                    $('#<%= ddlNights.ClientID %>').val(parseInt($('#<%= tbxMinNights.ClientID %>').val(), 10) + 1);
                                } else {
                                    $('#<%= ddlNights.ClientID %>').val($('#<%= tbxMinNights.ClientID %>').val());
                                }
                            });

                            $('#<%= ddlNights.ClientID %>').trigger("change");
                        }
                    });
                }
            });


            // clear prospectID & ReservationID text contents
            $('#btnContentClear').on('click', function () {
                $('#<%= txtProspectID.ClientID %>').val('');
                $('#<%= txtReservationID.ClientID %>').val('');
                $('#<%= txtBalanceDue.ClientID %>').val('');
            });


            // financial details
            $('#btnFinancial').on('click', function () {
                var prospectID = $('#<%= txtProspectID.ClientID %>').val();;
                var reservationID = $('#<%= txtReservationID.ClientID %>').val();
                
                //if (isNaN(prospectID) == false || isNaN(reservationID == false)) return;
                //if(parseInt(prospectID, 10) == 0 || parseInt(reservationID, 10)) return;
               
                $('#<%= txtBalanceDue.ClientID %>').addClass('blink_me').val('working...');
                var vDate = "'" + $('#<%= tbxDateCheckIn.ClientID %>').val().toString().replace(/\//g, "|") + "'";

                $.ajax({
                    type: "GET",
                    url: "ReservationPackageRate.aspx/GetFinancialDetails",
                    data: {
                        "prospectID": prospectID, "reservationID": reservationID,
                        "packageID": packageID, "packageReservationID": packageReservationID,
                        "dateCheckIn": vDate, "nights": nights,
                        "userID": 8022
                    },
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#dvFinancial').empty();
                        $('#dvToursAvailability').empty();

                        var invoiceLines = JSON.parse(data.d);
                        var $table = $('<table/>');
                        $table.append("<thead><tr><th>ID</th><th>INVOICE</th><th>TRANS DATE</th><th>AMOUNT</th><th>BALANCE</th></tr></thead>");
                        $table.addClass("table table-striped");
                        $.each(invoiceLines, function (index, element) {
                            $table.append('<tr><td>' + element.ID + '</td><td>' + element.Invoice + '</td><td>' + element.TransDate + '</td><td>' + element.InvoiceAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,') + '</td><td>' + element.Balance.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,') + '</td></tr>');
                        })
                        $('#dvFinancial').append($table);


                        // look up invoice again
                        var vDate = "'" + $('#<%= tbxDateCheckIn.ClientID %>').val().toString().replace(/\//g, "|") + "'";

                        $.ajax({
                            type: "GET",
                            url: "ReservationPackageRate.aspx/AjaxGetInvoices",
                            data: { "packageID": packageID, "packageReservationID": packageReservationID, "dateCheckIn": vDate, "nights": nights },
                            contentType: "application/json;charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                var invoiceLines = JSON.parse(data.d);   
                                var invoicesTotal = 0, paymentsTotal = 0;
                            
                                $.each(invoiceLines, function (index, element) {
                                    invoicesTotal += parseFloat(element.InvoiceAmount);
                                    $.each(element.AjaxPayments, function (plIndex, payment) {                                        
                                        paymentsTotal += parseFloat(payment.PaymentAmount);                                        
                                    });
                                })
                                
                                var vDate = "'" + $('#<%= tbxDateCheckIn.ClientID %>').val().toString().replace(/\//g, "|") + "'";

                                // inner ajax
                                $.ajax({
                                    type: "GET",
                                    url: "ReservationPackageRate.aspx/GetCurrentFinancialBalance",
                                    data: { "packageID": packageID, "packageReservationID": packageReservationID, "dateCheckIn": vDate, "nights": nights, "prospectID": prospectID, "reservationID": reservationID},
                                    contentType: "application/json;charset=utf-8",
                                    dataType: "json",
                                    success: function (data) {
                                        var sum = JSON.parse(data.d);                                                                               
                                        $('#<%= txtBalanceDue.ClientID %>').removeClass('blink_me');
                                        $('#<%= txtBalanceDue.ClientID %>').val(sum);
                                    }
                                });

                                // inner ajax ends

                               
                            }
                        });

                        // look up ends here


                    }
                });

            });

               

        });


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-fluid">
        <div class="row top-buffer">
            <div class="form-group">
                <div class="col-md-offset-1 col-md-12 ui-checkboxradio-radio-label">
                    <asp:RadioButtonList runat="server" ID="rblPackageTypeID" RepeatDirection="Horizontal"></asp:RadioButtonList>
                </div>
            </div>                        
        </div>
        <div class="row top-buffer">
            <div class="form-group">
                <label for="ddlPackagesByTypeID" class="control-label col-md-1">Packages</label>
                <div class="col-md-3">
                    <asp:DropDownList runat="server" ID="ddlPackagesByTypeID" CssClass="form-control"></asp:DropDownList>
                </div>
                <label for="tbxPackageID" class="control-label col-md-1">Package ID</label>
                <div class="col-md-1">
                    <asp:TextBox runat="server" ID="tbxPackageID" CssClass="form-control" Font-Bold="true"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row top-buffer">
            <div class="form-group">
                <label for="tbxUnitType" class="control-label col-md-1">Unit Type</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="tbxUnitType" CssClass="form-control"></asp:TextBox>
                </div>
                <label for="tbxBedRooms" class="control-label col-md-1">Bed Rooms</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="tbxBedRooms" CssClass="form-control"></asp:TextBox>
                </div>
                <label for="tbxMinNights" class="control-label col-md-1">Min Nights</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="tbxMinNights" CssClass="form-control"></asp:TextBox>
                </div>
            </div>                                   
        </div>
        <div class="row top-buffer">  
            <div class="form-group">
                <label for="tbxVendor" class="control-label col-md-1">Vendor</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="tbxVendor" CssClass="form-control"></asp:TextBox>
                </div> 
                <label for="tbxStartDate" class="control-label col-md-1">Available From</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="tbxStartDate" CssClass="form-control"></asp:TextBox> 
                </div>
                <label for="tbxEndDate" class="control-label col-md-1">Available To</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="tbxEndDate" CssClass="form-control"></asp:TextBox>
                </div>
            </div>                                             
        </div>
        <div class="row top-buffer">
            <div class="form-group">
                <div class="col-md-offset-1 col-md-11">
                    <div class="checkbox">
                        <label><asp:CheckBox runat="server" ID="ckbAllowExtraNight" Text="" />Allow Extra Night</label>
                    </div>
                </div>                
            </div>            
        </div>

        <div class="row top-buffer">
            <div class="form-group">
                <label for="tbxSource" class="control-label col-md-1">Reservation Source</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="tbxSource" CssClass="form-control"></asp:TextBox>
                </div>                
                <label for="tbxReservationType" class="control-label col-md-1">Reservation Type</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="tbxReservationType" CssClass="form-control"></asp:TextBox>
                </div>
            </div>                                              
        </div>
        <div class="row top-buffer">
            <div class="form-group">
                <label for="tbxPromoNights" class="control-label col-md-1">Promo Nights</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="tbxPromoNights" CssClass="form-control"></asp:TextBox>
                </div>
                <label for="tbxPromoRate" class="control-label col-md-1">Promo Rate</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="tbxPromoRate" CssClass="form-control"></asp:TextBox>
                </div>
                <label for="tbxDefaultInvoiceAmt" class="control-label col-md-1">Default Invoice Amount</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="tbxDefaultInvoiceAmt" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>




        <div class="row top-buffer">
            <div class="form-group">
                <label for="tbxCampaign" class="control-label col-md-1">Campaign</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="tbxCampaign" CssClass="form-control"></asp:TextBox>
                </div>
                <label for="tbxTourLocation" class="control-label col-md-1">Tour Location</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="tbxTourLocation" CssClass="form-control"></asp:TextBox>
                </div>
            </div>                       
        </div>

        
        

        <div class="row top-buffer">            
            <div class="col-md-12">
                <div class="row top-buffer">
                    <div class="form-group">
                        <label for="tbxDateCheckIn" class="control-label col-md-1">Check-In</label>
                        <div class="col-md-2">
                            <asp:TextBox runat="server" ID="tbxDateCheckIn" CssClass="form-control"></asp:TextBox>
                        </div>
                        <label for="tbxDateCheckOut" class="control-label col-md-1">Check-Out</label>
                        <div class="col-md-2">
                            <asp:TextBox runat="server" ID="tbxDateCheckOut" CssClass="form-control" ReadOnly="true" Font-Bold="true"></asp:TextBox>
                        </div>
                        <label for="ddlNights" class="control-label col-md-1">Nights</label>
                        <div class="col-md-1">
                            <asp:DropDownList runat="server" ID="ddlNights" CssClass="form-control">                    
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                <asp:ListItem Text="18" Value="18"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <input type="button" class="btn btn-primary" id="btnSetMinNights" value="Min Night" />
                        </div>
                        <div class="col-md-2">
                            <input class="btn btn-success" id="btnCheckToursAvail" type="button" value="Check Tours" />
                        </div>
                    </div>                    
                </div>
                <div class="row top-buffer">                    
                    <div class="col-md-offset-1 col-md-2">
                        <input type="button" id="btnCalculateInvoice" value="Calculate Invoice" class="btn btn-danger" />                
                    </div>
                </div>
                <div class="row top-buffer">
                    <div class="col-md-offset-1 col-md-9">
                        <div id="dvInvoices"></div>
                    </div>
                </div>

                <div class="row top-buffer">
                    <div class="form-group">
                        <asp:Label runat="server" ID="lblProspectID" Font-Bold="true" CssClass="control-label col-md-1">Prospect ID</asp:Label>
                        <div class="col-md-2">
                            <asp:TextBox runat="server" ID="txtProspectID" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" ID="lblReservationID" Font-Bold="true" CssClass="control-label col-md-1">Reservation ID</asp:Label>
                        <div class="col-md-2">
                            <asp:TextBox runat="server" ID="txtReservationID" CssClass="form-control"></asp:TextBox>
                        </div>                       
                        <div class="col-md-1">
                            <input type="button" value="Clear" class="form-control btn btn-primary" id="btnContentClear" />                            
                        </div>
                        <div class="col-md-1">
                            <input type="button" value="Financial" class="form-control btn btn-warning" id="btnFinancial" />                            
                        </div>
                        <asp:Label runat="server" ID="Label1" Font-Bold="true" CssClass="control-label col-md-1">Balance Due</asp:Label>
                         <div class="col-md-1">
                            <asp:TextBox runat="server" ID="txtBalanceDue" Font-Bold="true" ForeColor="Red" CssClass="form-control text-right"></asp:TextBox>               
                        </div>
                    </div>
                </div>

                <div class="row top-buffer">
                    <div class="col-md-offset-1 col-md-11">
                        <div id="dvFinancial"></div>
                    </div>
                </div>

                <div class="row top-buffer">
                    <div class="col-md-offset-1 col-md-9">                 
                        <div id="dvToursAvailability"></div>
                    </div>
                </div>
                
            </div>
        </div>        


        
    </div>
        
            
        
    </form>
</body>
</html>
