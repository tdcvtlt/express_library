<%@ Page Title="" Language="VB" MasterPageFile="~/wizards/Reservations/ReservationMasterPage.master" AutoEventWireup="false" CodeFile="SpecialList.aspx.vb" Inherits="wizard_Reservations_SpecialList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head2" Runat="Server">
    <script type="text/javascript">
        $(function () {

            var validator = $('form').validate({
                rules: {
                    ignore:'.skip',
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txSearch: {
                        required: false,                        
                        maxlength: 50
                    }
                },
                highlight: function (element) {
                    $(element).closest('.row').addClass('has-error');
                },
                unhighlight: function (element) {
                    $(element).closest('.row').removeClass('has-error');
                },
                errorElement: 'span',
                errorClass: 'help-block',
                errorPlacement: function (error, element) {
                    error.insertAfter(element);
                }
            });                       
        });

        $(function () {
             $('#<%= ddSubject.ClientID %>').on('change', function () {                
                 $('#<%= txSearch.ClientID %>').focus();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">

    <div>
        <div class="loading" align="center">
            <span class="clock-icon" style="vertical-align:top;"></span>&nbsp;Executing..., Please wait!<br />
            <br />
            <img src="../../images/progressbar.gif" alt="" />
        </div>

        <div class="container"> 
            <div class="row top-buffer">
                <div class="col-md-12">
                    <div class="panel panel-success">
                        <div class="panel-heading">
                            <h3 style="font-weight:bold;" class="text-primary">Reservation Search...</h3>
                            <h4 class="control-label text-right  ">King's Creek Plantation &#174;</h4>                            
                        </div>
                        <div class="panel-body">
                            <div class="row top-buffer">
                                <div class="col-sm-6 col-md-5">
                                    <asp:DropDownList runat="server" ID="ddSubject" CssClass="form-control"></asp:DropDownList>   
                                </div>                             
                            </div>
                            <div class="row top-buffer">                    
                                    <div class="col-sm-6 col-md-5">
                                        <asp:TextBox runat="server" CssClass="form-control" placeholder="Phone" ID="txSearch" name="txSearch" ></asp:TextBox>
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:Button runat="server" CssClass="btn btn-success" ID="btSearch"  Text="Search" />&nbsp;
                                        <asp:Button runat="server" CssClass="btn btn-default" ID="btClear"  Text="Clear" /> &nbsp;                                        
                                        <asp:Button runat="server" CssClass="btn btn-danger" ID="btCreate" Text="Create" />
                                    </div>                                    
                            </div>
                            <div class="row top-buffer">
                                <div class="col-lg-12">
                                    <asp:Label runat="server" ForeColor="Red" Text="Invalid Email Address"  ID="lbError"></asp:Label>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-6 col-md-5">
                                    <div style="height:400px;width:900px;overflow:auto;margin-top:40px;">
                                        <asp:GridView ID="gvProspectSearch" runat="server" AutoGenerateSelectButton="true" CssClass="table table-striped table-hover table-bordered" DataKeyNames="LeadID" AutoGenerateColumns="true" 
                                            EmptyDataText="No Records" GridLines="Horizontal">
                                            <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                                            <AlternatingRowStyle BackColor="#CCFFCC" />                
                                        </asp:GridView>
                                    </div>                    
                                </div>
                            </div>
                            <div class="row top-buffer"> 
                                <div class="col-xs-6 col-sm-2">
                                    <asp:Button runat="server" ID="btPrevious" CssClass="btn  btn-lg btn-success" Text="&larr; Previous" />
                                </div>               
                                <div class="col-xs-6 col-sm-3">                       
                                    <asp:Button runat="server" ID="btNext" CssClass="btn btn-lg btn-success" Text="Next &rarr;" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>            
        </div>
    </div>

    

    <script type="text/javascript">       

        $(function () {
            $('a[href^=javascript], #<%= btSearch.ClientID %>').on('click', function (e) {
                ShowProgress();
            });   

            $('#<%= ddSubject.ClientID %>').on('change', function (e) {
                $('#<%= txSearch.ClientID %>').attr('placeholder', $('#<%= ddSubject.ClientID %> option:selected').text());
                $('#<%= txSearch.ClientID %>').val('');
            });

            $('#<%= btClear.ClientID %>').on('click', function (e) {
                $('#<%= txSearch.ClientID %>').val('');
                e.preventDefault();
            });
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

