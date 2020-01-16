<%@ Page Title="" Language="VB" MasterPageFile="~/wizards/Reservations/ReservationMasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="wizard_Reservations_Default" %>

<asp:Content ID="Content13" ContentPlaceHolderID="Head2" runat="server">
    <style type="text/css">
        .panel-body label{
            text-transform:uppercase;
        }
    </style>

    <script type="text/javascript">

       $(function () {
           $('.panel-heading h3:first').on('click', function () {
               window.open("https://crms.kingscreekplantation.com/crmsnet/wizards/Reservations/reservationpackagerate.aspx", '_blank');
           }).css('cursor', 'pointer');
       });

   </script>

</asp:Content>

<asp:Content ID="Content22" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">       
    <div>
        <div class="container">
            <div class="row top-buffer">
                <div class="col-xs-12">
                    <asp:MultiView runat="server" ID="mvWelcome" ActiveViewIndex="0">
                        <asp:View runat="server" ID="vwOptions">
                            <div class="panel panel-success">
                                    <div class="panel-heading">
                                        <h3 style="font-weight:bold;" class="text-primary">Reservation Booking Wizard</h3>
                                        <h4 class="control-label text-right  ">King's Creek Plantation &#174;</h4>
                                    </div>
                                    <div class="panel-body">
                                        <br />
                                        <asp:RadioButtonList runat="server" Font-Size="Medium" ID="rblOptions"  CssClass="list-group" TextAlign="Right" CellSpacing="20" style="margin-left:40px;">
                                            <asp:ListItem Text="&nbsp; Purchasing A Tour Promotion, An Owner Getaway Or Rental Packages" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="&nbsp; Booking A Package Tour Or A Tradeshow Package" Value="2" Enabled="true"></asp:ListItem>
                                            <asp:ListItem Text="&nbsp; Re-Scheduling A Reservation" Value="3" ></asp:ListItem>       
                                            <asp:ListItem Text="&nbsp; Purchasing 1 Or 2 Nights Tour Promotion Package" Value="10" ></asp:ListItem>       
                                        </asp:RadioButtonList>

                                        <div class="row top-buffer">
                                            <div class="col-sm-1">&nbsp;</div>
                                            <div class="col-sm-5">
                                                <asp:Button ID="btNext" runat="server" CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="Next &rarr;" />
                                            </div>
                                        </div>                                        
                                    </div>
                                </div>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>
        </div>
    </div>

    <div class="loading" align="center">
        <span class="clock-icon" style="vertical-align:top;"></span>&nbsp;Executing..., Please wait!<br />
        <br />
        <img src="../../images/progressbar.gif" alt="" />
    </div>

    <script type="text/javascript">

        $(function () {
            $('#<%= btNext.ClientID %>').click(function (e) { ShowProgress();});
        });



        function beforeAsyncPostBack() {           
        }
        function afterAsyncPostBack() {
        }

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


    <!-- The core Firebase JS SDK is always required and must be listed first -->
    <script type="text/javascript" src="/__/firebase/6.2.1/firebase-app.js"></script>

    <!-- TODO: Add SDKs for Firebase products that you want to use
     https://firebase.google.com/docs/web/setup#reserved-urls -->

    <!-- Initialize Firebase -->
    <script type="text/javascript" src="/__/firebase/init.js"></script>
</asp:Content>