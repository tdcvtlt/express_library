<%@ Page Title="" Language="VB" MasterPageFile="~/wizards/Reservations/ReservationMasterPage.master" AutoEventWireup="false" CodeFile="EditReservation.aspx.vb" Inherits="wizard_Reservations_Reervation" %>
<%@ Register Src="~/controls/SyncDateField.ascx" TagPrefix="uc1" TagName="SyncDateField" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head2" Runat="Server">
    <style type="text/css">
         .loading
            {
                font-family:'Times New Roman';
                font-size: 10pt;
                border: 1px solid #67CFF5;
                width: 200px;
                height: 100px;
                display: none;
                position: fixed;
                background-color:white;
                z-index: 999;
                box-shadow:12px 12px 6px rgba(255,255,255,0.275);
                font-style:italic;
                color:darkblue;
            }
    </style>

    <script type="text/javascript">

        $(function () {

            var validator = $('form').validate({
                rules: {
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddStatus: {
                        required: true
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddLocation: {
                        required: true
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddType: {
                        required: true
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddSubType: {
                        required: true
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddSource: {
                        required: true
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddResortCompany: {
                        required: true
                    }                   
                },


                highlight: function (element) {
                    $(element).parent('.col-sm-3').addClass('has-error');
                },
                unhighlight: function (element) {
                    $(element).parent('.col-sm-3').removeClass('has-error');
                },
                errorElement: 'span',
                errorClass: 'help-block',
                errorPlacement: function (error, element) {
                    error.insertAfter(element);
                }

            });                                   

        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div class="loading" align="center">
        <span class="clock-icon" style="vertical-align:top;"></span>&nbsp;Executing..., Please wait!<br />
        <br />
        <img src="../../images/progressbar.gif" alt="" />
    </div>

    <div class="container">        

        <div class="panel panel-success">
            <div class="panel-heading"> 
                <h3 style="font-weight:bold;" class="text-primary">Edit Reservation</h3>
                <h4 class="control-label text-right  ">King's Creek Plantation &#174;</h4>                                       
            </div>

            <div class="panel-body">
                <div class="form-group form-group-sm">
            <div class="row top-buffer">
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Reservation ID"></asp:Label>                                   
                </div>
                <div class="col-sm-3">
                    <asp:TextBox runat="server" ID="txReservationID" CssClass="form-control" disabled="disabled"></asp:TextBox>
                </div>                
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Status"></asp:Label>
                </div>
                <div class="col-sm-3">
                    <asp:DropDownList runat="server" CssClass="form-control" name="ddStatus" ID="ddStatus" disabled="disabled"></asp:DropDownList>
                </div>
            </div>
            <div class="row top-buffer">
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Reservation Number"></asp:Label>                                   
                </div>
                <div class="col-sm-3">
                    <asp:TextBox runat="server" ID="txReservationNumber" CssClass="form-control text-uppercase"></asp:TextBox>
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Status Date"></asp:Label>     
                </div>
                <div class="col-sm-3">
                    <asp:TextBox runat="server" ID="txStatusDate" CssClass="form-control text-capitalize" disabled="disabled"></asp:TextBox>
                </div>
            </div>
            <div class="row top-buffer">
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Location"></asp:Label>                                   
                </div>
                <div class="col-sm-3">
                    <asp:DropDownList runat="server" CssClass="form-control" name="ddLocation" ID="ddLocation"></asp:DropDownList>
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Type"></asp:Label>     
                </div>
                <div class="col-sm-3">
                    <asp:DropDownList runat="server" CssClass="form-control" name="ddType" Enabled="false" disabled="disabled" ID="ddType"></asp:DropDownList>
                </div>
            </div>
            <div class="row top-buffer">
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Check In"></asp:Label>                                   
                </div>
                <div class="col-sm-3">
                    <div style="display:none;"><asp:TextBox runat="server" CssClass="form-control" id="txCheckIn" readonly="true"></asp:TextBox>
                                    <asp:Button runat="server" Text="..." onclick="Unnamed1_Click1" Enabled="false" disabled="disabled"></asp:Button>
                                    <asp:Calendar runat="server" id = "Calendar1" visible = "false"></asp:Calendar>
                                    </div>
                                    <uc1:SyncDateField ID="SyncDateField1" runat="server" />                                                                        
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Sub-Type"></asp:Label>     
                </div>
                <div class="col-sm-3">
                    <asp:DropDownList runat="server" CssClass="form-control" name="ddSubType" ID="ddSubType"></asp:DropDownList>
                </div>
            </div>
            <div class="row top-buffer">
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Check Out"></asp:Label>                                   
                </div>
                <div class="col-sm-3">
                    <asp:TextBox runat="server" ID="txCheckOut" CssClass="form-control" disabled="disabled" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="#Adults"></asp:Label>     
                </div>
                <div class="col-sm-3">
                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddAdults"></asp:DropDownList>
                </div>
            </div>
            <div class="row top-buffer">
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Total Nights"></asp:Label>                                   
                </div>
                <div class="col-sm-3">
                    <asp:DropDownList runat="server" CssClass="form-control" autopostback="true" ID="ddNights"></asp:DropDownList>
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="#Children"></asp:Label>     
                </div>
                <div class="col-sm-3">
                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddChildren"></asp:DropDownList>
                </div>
            </div>
            <div class="row top-buffer">
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Date Booked"></asp:Label>                                   
                </div>
                <div class="col-sm-3">
                    <asp:TextBox runat="server" ID="txDateBooked" CssClass="form-control" disabled="disabled" ></asp:TextBox>
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Source"></asp:Label>     
                </div>
                <div class="col-sm-3">
                    <asp:DropDownList runat="server" CssClass="form-control" name="ddSource" ID="ddSource" Enabled="false" disabled="disabled"></asp:DropDownList>
                </div>
            </div>
            <div class="row top-buffer">
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Lock Inventory"></asp:Label>                                   
                </div>
                <div class="col-sm-3">
                    <asp:CheckBox runat="server" ID="cbLockedInventory" />
                </div>
            </div>
            <div class="row top-buffer">
                <div class="col-sm-2">
                    <asp:Label runat="server" CssClass="control-label" Text="Resort Company"></asp:Label>                                   
                </div>
                <div class="col-sm-3">
                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddResortCompany" Enabled="false" name="ddResortCompany"  disabled="disabled"></asp:DropDownList>
                </div>
                 <div class="col-sm-7">
                    <asp:Label runat="server" CssClass="control-label" ID="lbErr" ForeColor="Red"></asp:Label>     
                </div>
            </div>
            


            
        </div>
            </div>


            <div class="panel-footer">
                <div class="row top-buffer">   
                    <div class="col-sm-6 col-xs-12">&nbsp;</div>
                    <div class="col-sm-3 col-xs-6">
                         <asp:Button runat="server" ID="btPrevious" formnovalidate CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="&larr; Previous" />
                    </div>
                   
                    <div class="col-sm-3 col-xs-6">
                        <asp:Button runat="server" ID="btNext" CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="Next &rarr;" />
                    </div>                
            </div>
            </div>
        </div>

        

    </div>
    <script type="text/javascript">

        $('#<%= btNext.ClientID%>').click(function (e) {

            var f = $('form');
            f.validate();
                
            if (!f.valid()) {
                e.preventDefault();                    
            }else
            {
                ShowProgress();                    
            }
        });

        function beforeAsyncPostBack() { }
        function afterAsyncPostBack() {  }

        Sys.Application.add_init(appInit);

        function appInit() {
            var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
            pgRegMgr.add_beginRequest(BeginHandler);
            pgRegMgr.add_endRequest(EndHandler);
        }

        function BeginHandler() {
            $('div.loading').hide();
            $('.modal').hide();
            beforeAsyncPostBack();
        }

        function EndHandler() {
            $('div.loading').hide();
            $('.modal').hide();
            clearTimeout(timeout);
            afterAsyncPostBack();
        }
    </script>
    <script type="text/javascript">
        var timeout;

        function ShowProgress() {
            timeout = setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 0);
        }        
    </script>
</asp:Content>

