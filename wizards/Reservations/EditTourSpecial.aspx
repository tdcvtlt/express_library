<%@ Page Title="" Language="VB" MasterPageFile="~/wizards/Reservations/ReservationMasterPage.master" AutoEventWireup="false" CodeFile="EditTourSpecial.aspx.vb" Inherits="wizard_Reservations_TourSpecial" %>

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

                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddCampaign: {
                        required: true
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddTourTime: {
                        required: true
                    },
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
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txTourDate: {
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


            $('#<%= btNext.ClientID %>').click(function (e) {

                var f = $('form');
                f.validate();

                if (!f.valid()) {
                    e.preventDefault();
                } else {

                    $(this).hide();
                    $('#<%= btPrevious.ClientID %>').hide();
                    
                    ShowProgress();
                }
            });
        });



        function passTourDate(c) {

            if (c == undefined) return

            try {

                var d = $(c).attr('data-tour-date');
                var t = $(c).attr('data-tour-time');

                $('#<%= txTourDate.ClientID %>').val(d);
                $('#<%= ddTourTime.ClientID %>').val(t);
            }
            catch (ex) { 
                alert(ex)
            }


        }


        $(function () {

           // $('#<%= ddTourTime.ClientID %>').attr('disabled', 'disabled');
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
                        <h3 style="font-weight:bold;" class="text-primary">Edit Tour</h3>
                        <h4 class="control-label text-right  ">King's Creek Plantation &#174;</h4>                                       
                    </div>

                    <div class="panel-body">
                        <div class="form-group form-group-sm">
                            <div class="row top-buffer">
                                <div class="col-sm-12">
                            <asp:MultiView runat="server" ID="multiview1" ActiveViewIndex="0">
                                <asp:View runat="server" ID="view1">
                                    <div class="row top-buffer">
                                        <div class="col-sm-12">
                                            <asp:Label ID="lbErr" runat="server" CssClass="text-warning" ForeColor="Red"></asp:Label>                                   
                                        </div>
                                    </div>
                                    <div class="row top-buffer">
                                        <div class="col-sm-9">
                                            <div class="row top-buffer">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" CssClass="control-label" Text="Tour ID"></asp:Label>                                   
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:TextBox runat="server" ID="txTourID" CssClass="form-control" ReadOnly="true" ></asp:TextBox>
                                                </div>                
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" CssClass="control-label" Text="Status" Visible="false"></asp:Label>
                                                </div>
                                                <div class="col-sm-3">
                                                    <span></span>
                                                </div>
                                            </div>
                                            <div class="row top-buffer">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" CssClass="control-label" Text="Tour Date"></asp:Label>                                   
                                                </div>
                                                <div class="col-sm-3">                                                      
                                                    <asp:TextBox runat="server" ID="txTourDate" name="txTourDate" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" CssClass="control-label" Text="Tour Time"></asp:Label>     
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList runat="server" CssClass="form-control" name="ddTourTime" AppendDataBoundItems="true" ID="ddTourTime">
                                                        <asp:ListItem Text="(Empty)" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row top-buffer">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" CssClass="control-label" Text="Status"></asp:Label>                                   
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList runat="server" CssClass="form-control" name="ddStatus" ID="ddStatus" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="(Empty)" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" CssClass="control-label" Text="Location"></asp:Label>     
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList runat="server" CssClass="form-control" name="ddLocation"  ID="ddLocation" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="(Empty)" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row top-buffer">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" CssClass="control-label" Text="Campaign"></asp:Label>                                   
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList runat="server" CssClass="form-control" name="ddCampaign"  ID="ddCampaign" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="(Empty)" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" CssClass="control-label" Text="Package"></asp:Label>     
                                                </div>
                                                <div class="col-sm-3">
                                                    <span></span>
                                                </div>
                                            </div>
                                            <div class="row top-buffer">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" CssClass="control-label" Text="Type"></asp:Label>                                   
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList runat="server" CssClass="form-control" name="ddType"  ID="ddType" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="(Empty)" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" CssClass="control-label" Text="Sub-Type"></asp:Label>     
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList runat="server" CssClass="form-control" name="ddSubType"  ID="ddSubType" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="(Empty)" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row top-buffer">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" CssClass="control-label" Text="Source"></asp:Label>                                   
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList runat="server" CssClass="form-control" name="ddSource" ID="ddSource" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="(Empty)" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" CssClass="control-label" Text="Booking Date"></asp:Label>     
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:TextBox runat="server" ID="txBookingDate" CssClass="form-control" Enabled="false" ></asp:TextBox>
                                                </div>
                                            </div>

                                            <br />

                                            <div class="row top-buffer">
                                                <div class="col-sm-10">   
                                                    <asp:Label runat="server" ID="lbPriceOverLimit" ForeColor="Red" CssClass="control-label"></asp:Label>
                                                    <br />                                         
                                                    <asp:GridView runat="server" ID="gridview2" AutoGenerateColumns="false" CssClass="table table-bordered table-condensed" AutoGenerateSelectButton="true" Width="100%" DataKeyNames="PremiumIssuedID" >
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="text-center" ItemStyle-Width="80">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="PremiumIssuedID" CssClass="control-label"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Premium Name" HeaderStyle-CssClass="text-center">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lbPremiumName" CssClass="control-label"></asp:Label>
                                                                </ItemTemplate>                                                                               
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cost" HeaderStyle-Width="80" HeaderStyle-CssClass="text-center">
                                                                <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txCostEA" CssClass="form-control" style="text-align:right;" ReadOnly="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Quantity" HeaderStyle-CssClass="text-center">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList runat="server" OnSelectedIndexChanged="ddlQty_SelectedIndexChanged" ID="ddlQty" Enabled="false"  CssClass="form-control text-center" AutoPostBack="true">
                                                            <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                        </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                        
                                       
                                            </div>
                                            <div class="row top-buffer">
                                                <div class="col-sm-10">
                                                     <asp:GridView runat="server" ID="gridview3"></asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <h5>Tours Availability</h5>
                                    
                                            <div>                                                
                                                <asp:GridView runat="server" ID="gridview1" DataKeyNames="TourDate, TourTime" EmptyDataText="" AutoGenerateColumns="false" CssClass="table table-striped table-hover table-bordered">
                                                    <Columns>                                                                        
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button  runat="server" ID="SelectButton" CssClass="btn btn-primary" formnovalidate Text="Select" CommandName="SelectButton" />                                                        
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Tour Date" DataField="TourDate" ItemStyle-Width="400" />
                                                        <asp:BoundField HeaderText="Tour Time" DataField="TourTime" ItemStyle-Width="300" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row top-buffer">                                        
                                        <div class="col-md-12">                        
                                            <div>
                                                <asp:Button runat="server" ID="btPrevious" formnovalidate CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="&larr; Previous" />
                                                &nbsp;&nbsp;
                                                                                                
                                                <asp:Button runat="server" ID="btNext" CssClass="btn btn-lg btn-success" 
                                                    Style="width:160px;font-weight:bolder;" Text="Next &rarr;" />

                                            </div>

                                        </div>
                                    </div>
                                </asp:View>
                                <asp:View ID="view2" runat="server">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                             <asp:Label runat="server" CssClass="control-label col-xs-12 col-sm-3 col-md-3" Text="Premium"></asp:Label>           
                                            <div class="col-xs-12 col-sm-5 col-md-5">
                                                <asp:DropDownList runat="server" ID="ddPremiums" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>                                                   
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="control-label col-sm-3 col-md-3" Text="Certificate Number"></asp:Label>     
                                            <div class="col-sm-5 col-md-5">
                                                 <asp:TextBox runat="server" CssClass="form-control" ID="txCert"></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="form-group">
                                            <asp:Label runat="server" CssClass="control-label col-sm-3 col-md-3" Text="Amount"></asp:Label>     
                                            <div class="col-sm-5 col-md-5">
                                                  <asp:TextBox runat="server" CssClass="form-control" ID="txAmount" ReadOnly="true" ></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="form-group">
                                            <asp:Label runat="server" CssClass="control-label col-sm-3 col-md-3" Text="Chargeback Amount"></asp:Label>     
                                            <div class="col-sm-5 col-md-5">
                                                 <asp:TextBox runat="server" CssClass="form-control" ID="txCbAmount" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>

                                         <div class="form-group">
                                            <asp:Label runat="server" CssClass="control-label col-sm-3 col-md-3" Text="Quantity"></asp:Label>     
                                            <div class="col-sm-5 col-md-5">
                                                <asp:DropDownList runat="server" ID="ddQuantity" CssClass="form-control"></asp:DropDownList>                                                                            
                                            </div>
                                        </div>

                                         <div class="form-group">
                                            <asp:Label runat="server" CssClass="control-label col-sm-3 col-md-3" Text="Status"></asp:Label>                                          
                                            <div class="col-sm-5 col-md-5">
                                                <asp:DropDownList runat="server" ID="ddStatus_P" CssClass="form-control" AppendDataBoundItems="true">
                                                    <asp:ListItem Text="(Empty)" Value=""></asp:ListItem>
                                                </asp:DropDownList>         
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <div class="col-sm-3 col-md-3">&nbsp;</div>
                                            <div class="col-sm-2 col-md-2">
                                                 <asp:Button runat="server" ID="btCancel" formnovalidate CssClass="btn btn-lg btn-success" Style="font-weight:bolder;" Text="Cancel" />                                       
                                            </div>
                                            <div class="col-sm-1 col-md-1">&nbsp;</div>
                                            <div class="col-sm-2 col-md-2">
                                                 <asp:Button runat="server" ID="btSubmit" CssClass="btn btn-lg btn-success" Style="font-weight:bolder;" Text="Submit" />
                                            </div>
                                        </div>

                                    </div>
                            
                            
                            
                                                        

                            
                                </asp:View>
                            </asp:MultiView>
                        </div>                
                            </div>            
                        </div>
                    </div>                    

                </div>                   
            </div>
    

    
    <script type="text/javascript">

        function beforeAsyncPostBack() { }
        function afterAsyncPostBack() { }

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
            }, 500);
        }




    </script>
</asp:Content>

