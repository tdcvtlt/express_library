<%@ Page Title="" Language="VB" MasterPageFile="~/Wizards/Reservations/ReservationMasterPage.master" AutoEventWireup="false" CodeFile="RequestRefund.aspx.vb" Inherits="Wizards_Reservations_RequestRefund" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head2" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">

<div class="loading" align="center">
    <span class="clock-icon" style="vertical-align:top;"></span>&nbsp;Executing..., Please wait!<br />
    <br />
    <img src="../../images/progressbar.gif" alt="" />
</div>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="container">
            <div class="panel panel-success">
                <div class="panel-heading">                
                <div class="panel-title">                                                
                    <h2 style="text-align:right;padding-right:20px; font-variant:small-caps">Process Refund</h2>
                </div>
            </div>

                <div class="panel-body">
                    <div class="form-group form-group-sm">
                <div class="row top-buffer">
                    <div class="col-md-12">  
                        


                        <!-- copy begins -->
                        Refund Method: 
                        <asp:dropdownlist runat="server" id = "ddRefMethod" CssClass="dropdown dropdown-menu-left form-control" style="width:16%;" autopostback = "true"></asp:dropdownlist>

                        &nbsp;&nbsp;&nbsp;<asp:Label runat="server" Font-Bold="true" Text="Refund $"></asp:Label>&nbsp;
                        <asp:Label runat="server" ForeColor="Red" Font-Bold="true" ID="lb_Refund_Balance" Text=""></asp:Label>
                        <br />

                        <asp:multiview runat="server" id = "MultiView1">
                            <asp:View runat="server" id = "CCView">
                                <asp:GridView runat="server" id = "gvCCTrans" 
                                    AutoGenerateSelectButton="True" 
                                    CssClass="table table-striped table-hover table-bordered" 
                                    onRowDataBound = "gvCCTrans_RowDataBound" 
                                    EmptyDataText = "No Credit Card Transactions">            
                                </asp:GridView>
                                <asp:HiddenField runat="server" id ="hfCCTransID"></asp:HiddenField>
                                <asp:HiddenField runat="server" id = "hfRefRequest"></asp:HiddenField>
                                <asp:HiddenField runat="server" ID = "hfTickCounter" />
                            </asp:View>
                            <asp:View runat="server" id = "CashView">
                                <asp:GridView runat="server" id = "gvCashPayments" CssClass="table table-striped table-hover table-bordered" autoGenerateColumns = "False" emptydatatext = "No Cash Payments" onRowDataBound = "gvCashPayments_RowDataBound">
                                    <Columns>
                                        <asp:templatefield>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cb" CssClass="checkbox-inline" runat="server" />
                                            </ItemTemplate>
                                        </asp:templatefield>
                                        <asp:BoundField HeaderText="ID" DataField = "PaymentID" />
                                        <asp:BoundField HeaderText="InvoiceID" DataField = "InvoiceID" />
                                        <asp:BoundField HeaderText="Trans Date" DataFIeld ="TransDate" />
                                        <asp:BoundField HeaderText="Invoice" DataField = "Invoice" />
                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                        <asp:templatefield HeaderText = "Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAmount" runat="server" />
                                            </ItemTemplate>
                                        </asp:templatefield>
                                    </Columns>            
                                </asp:GridView>            
                                <asp:Button ID="btSubmit_Cash_Refund" runat="server" CssClass="btn btn-primary" Text="Submit Refund" ></asp:Button>
                                <asp:Label runat="server" id = "lblErr3"></asp:Label>
                            </asp:View>
                            <asp:View runat="server" id = "CheckView">
                                <br />
                                <asp:Label runat="server" id = "lblErr4"></asp:Label>
                                <asp:GridView ID="gvPayments" runat="server" CssClass="table table-striped table-hover table-bordered" AutoGenerateColumns="false" 
                EmptyDataText="No Payments" onRowDataBound="gvPayments_RowDataBound">
                <Columns>
                    <asp:templatefield>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb" runat="server" />
                        </ItemTemplate>
                    </asp:templatefield>
                    <asp:BoundField DataField="PaymentID" HeaderText="ID" />
                    <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID" />
                    <asp:BoundField DataField="Invoice" HeaderText="Invoice" />
                    <asp:BoundField DataFIeld="TransDate" HeaderText="Trans Date" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" />
                    <asp:BoundField DataField="MerchantAccountID" HeaderText="MerchantAccountID" />
                    <asp:BoundField DataField="Method" HeaderText="Payment Method" />
                    <asp:templatefield HeaderText="Amount">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAmount" runat="server" />
                        </ItemTemplate>
                    </asp:templatefield>
                    <asp:TemplateField HeaderText="Check Number">
                        <ItemTemplate>
                            <asp:TextBox ID="txtNumber" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                                <asp:Button ID="btSubmit_Check_Refund" runat="server" CssClass="btn btn-primary" Text="Submit Refund" />
                            </asp:View>    
                            <asp:View runat="server" id = "CCApplyView">
                                <asp:GridView runat="server" id = "gvCCApply" CssClass="table table-striped table-hover table-bordered" autogeneratecolumns = "False" onRowDataBound = "gvCCApply_RowDataBound" EmptyDataText = "No Records">
                                    <Columns>
                                        <asp:templatefield>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb" runat="server" />
                        </ItemTemplate>
                    </asp:templatefield>
                                        <asp:BoundField HeaderText="ID" DataField = "PaymentID" />
                                        <asp:BoundField HeaderText="InvoiceID" DataField = "InvoiceID" />
                                        <asp:BoundField HeaderText="Invoice" DataField = "Invoice" />
                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                        <asp:templatefield HeaderText = "Amount">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAmount" runat="server" />
                        </ItemTemplate>
                    </asp:templatefield>
                                        <asp:BoundField HeaderText="PaymentMethod" DataField = "PaymentMethod" />
                                    </Columns>
                                </asp:GridView>
                                <asp:Button ID="btSubmit_Credit_Refund" runat="server" CssClass="btn btn-primary" Text="Process Refund"></asp:Button> 
                                &nbsp;
                                <asp:Button runat="server" Text="Cancel" CssClass="btn btn-primary" ID="btCancel" />   &nbsp;                                                             
                                <asp:Label runat="server" id = "lblErr" CssClass="alert alert-danger"></asp:Label>
                            </asp:View>

                            <asp:View ID="CCProgress" runat="server">
                                <div class="panel panel-danger">
                                    <div class="panel-heading">
                                        <h4>Processing refund...</h4>
                                    </div>
                                    <div class="panel-body">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <asp:Label runat="server" ID="lblWaiting" Text="Processing... Please Wait" ForeColor="Red"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <asp:Timer ID="tmrCheck" runat="server" Enabled="False" Interval="1000"></asp:Timer>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:View>
                        </asp:multiview>


                        <!-- copy ends -->
                    </div>
                </div>
            </div>
                </div>

                <div class="panel-footer">
                    <div class="row top-buffer">                                        
                        <div class="col-md-12">                       
                            <div>                        
                                <asp:Button runat="server" ID="btNext" CssClass="btn btn-lg btn-success" OnClientClick="ShowProgress();" Style="width:160px;font-weight:bolder;" Text="Next &rarr;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>    
        </div>
    </ContentTemplate>
</asp:UpdatePanel>




 
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

