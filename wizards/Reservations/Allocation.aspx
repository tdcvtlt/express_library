<%@ Page Title="" Language="VB" MasterPageFile="~/wizards/Reservations/ReservationMasterPage.master" AutoEventWireup="false" EnableEventValidation="false" CodeFile="Allocation.aspx.vb" Inherits="wizard_Reservations_Allocation" %>
<%@ Register Src="~/controls/DateField.ascx" TagName="DateField" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head2" Runat="Server">

   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    

    <div class="loading" align="center">
        <span class="clock-icon" style="vertical-align:top;"></span>&nbsp;Executing..., Please wait!<br />
        <br />
        <img src="../../images/progressbar.gif" alt="" />
    </div>

    <div class="container">                
        <div class="form-group form-group-sm">
            <div class="row top-buffer">                                        
                <div class="col-md-12">     
                    <asp:MultiView ID="multiview1" runat="server" ActiveViewIndex="0">
                        <asp:View runat="server" ID="view1">
                            <div class="panel panel-success">
                                <div class="panel-heading">
                                    <h3 style="font-weight:bold;" class="text-primary">Select Rooms</h3>
                                    <h4 class="control-label text-right  ">King's Creek Plantation &#174;</h4>                                       
                                </div>

                                <div class="panel-body">
                                    <div class="row top-buffer">
                                        <div class="col-sm-12">
                                            <asp:Label runat="server" ID="lbErr" ForeColor="Red"></asp:Label>
                                            <br />
                                        </div>
                                    </div>


                                    <div class="row top-buffer">
                                        <div class="col-sm-12">                                            
                                            <asp:GridView runat="server" ID="gridview1" AutoGenerateColumns="false" CssClass="control-label  table table-striped table-bordered table-hover">
                                                <Columns>                                                    
                                                     <asp:TemplateField HeaderText="Select" HeaderStyle-CssClass="text-capitalize text-center" HeaderStyle-Width="10%" ItemStyle-CssClass="text-center " >
                                                        <ItemTemplate>
                                                            <asp:Button runat="server" CssClass="btn btn-danger btn-sm" ID="btSelect" CommandName="Select" Text="Select" Visible="true" />                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Package" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbPackage" CssClass="control-label"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Unit" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="10%" ItemStyle-CssClass="text-center " >
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbUnit" CssClass="control-label" ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Room" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="10%" ItemStyle-CssClass="text-center ">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbRoomNumber" CssClass="control-label"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Bed" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="10%" ItemStyle-CssClass="text-center ">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbBedRoom" CssClass="control-label"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Style" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="10%" ItemStyle-CssClass="text-center ">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbStyle" CssClass="control-label"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                   
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>

                                    <div class="row top-buffer">
                                        <div class="col-sm-12">  
                                            
                                            <asp:Label runat="server" ID="lbBuyAdditional" Text="Purchase Additional Units" ForeColor="Red" Font-Bold="true"></asp:Label>                                                                                       
                                            
                                            <br />
                                            <asp:GridView runat="server" ID="gridview4" AutoGenerateColumns="false" CssClass="control-label  table table-striped table-bordered table-hover">
                                                <Columns>                                    
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="10%" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-CssClass="text-center " >
                                                        <ItemTemplate>                                            
                                                            <asp:Button runat="server" CssClass="btn btn-primary btn-sm" ID="btSelect" OnClientClick="ShowProgress();" Enabled="false" CommandName="btRoomSelect" Text="Add..." />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Package" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbPackage" CssClass="control-label"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Unit" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="10%" ItemStyle-CssClass="text-center " >
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbUnit" CssClass="control-label" ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Room" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="10%" ItemStyle-CssClass="text-center ">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbRoomNumber" CssClass="control-label"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Bed" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="10%" ItemStyle-CssClass="text-center ">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbBedRoom" CssClass="control-label"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Style" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="10%" ItemStyle-CssClass="text-center ">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbStyle" CssClass="control-label"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                                                                                                                                                                                                                   
                                                </Columns>
                                            </asp:GridView>

                                        </div>
                                    </div>
                                    
                                    <div class="row top-buffer">                                        
                                        <div class="col-sm-12">
                                            <asp:GridView runat="server" ID="gridview3" AutoGenerateColumns="false" CssClass="control-label  table table-striped table-bordered table-hover">
                                                <Columns>                                    
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="10%" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-CssClass="text-center " >
                                                        <ItemTemplate>                                            
                                                            <asp:Button runat="server" CssClass="btn btn-primary btn-sm" ID="btSelect" OnClientClick="ShowProgress();" CommandName="btRoomSelect" Text="Select" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Package" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="25%">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbPackage" CssClass="control-label"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Unit" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="10%" ItemStyle-CssClass="text-center " >
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbUnit" CssClass="control-label" ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Room" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="10%" ItemStyle-CssClass="text-center ">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbRoomNumber" CssClass="control-label"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Bed" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="10%" ItemStyle-CssClass="text-center ">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbBedRoom" CssClass="control-label"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Style" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-Width="10%" ItemStyle-CssClass="text-center ">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbStyle" CssClass="control-label"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                                                                                                                                                                                                                   
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    
                                    <div class="row top-buffer" runat="server" id="navContainer">
                                        <div class="col-sm-2">
                                            <asp:Button runat="server" ID="btPrevious" CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="&larr; Previous" />
                                        </div>
                                        <div class="col-sm-1">&nbsp;</div>
                                        <div class="col-sm-2">
                                            <asp:Button runat="server" ID="btNext" OnClientClick="ShowProgress();" CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="Next &rarr;" />
                                        </div>                                                                                                                        
                                    </div>


                                </div>
                            </div>
                                
                                    

                                                                        
                        </asp:View>
                        <asp:View runat="server" ID="view2">
                            <div class="panel panel-success">
                                <div class="panel-heading">
                                    <div class="panel-title">
                                        <h2 style="text-align:right;padding-right:20px;">Rooms Available</h2>
                                    </div>                                    
                                </div>

                                <div class="panel-body">
                                    <div class="row top-buffer">
                                        <div class="col-sm-12">
                                        <asp:GridView runat="server" ID="gridview2" AutoGenerateSelectButton="false" AutoGenerateColumns="false" CssClass="control-label table table-striped table-bordered table-hover">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="10%" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-CssClass="text-center " >
                                                    <ItemTemplate>                                            
                                                        <asp:Button runat="server" CssClass=" btn btn-danger btn-sm" ID="btRoomSelect" OnClientClick="ShowProgress();" CommandName="btRoomSelect" Text="Select..." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="RoomNumber" HeaderText="Room Number" HeaderStyle-Width="30%" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-CssClass="text-center " />
                                                <asp:BoundField DataField="Beds" HeaderText="Bed Room" HeaderStyle-Width="10%" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-CssClass="text-center " />
                                                <asp:BoundField DataField="UnitType" HeaderText="Unit Type" HeaderStyle-Width="15%" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-CssClass="text-center "/>
                                                <asp:BoundField DataField="UnitStyle" HeaderText="Style" HeaderStyle-Width="30%" HeaderStyle-CssClass="text-capitalize text-center" ItemStyle-CssClass="text-left "/>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    </div>
                                    <div class="row top-buffer">
                                        <div>
                                        <asp:Button runat="server" ID="btCancel" CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="Cancel" />                                    
                                    </div>
                                    </div>
                                    <asp:Label runat="server" ID="lbAvailableSQL"></asp:Label>
                                </div>
                            </div>

                                
                                
                                    
                                
                        </asp:View>
                        <asp:View ID="view3" runat="server"></asp:View>
                        <asp:View ID="view4" runat="server"></asp:View>
                        <asp:View ID="view5" runat="server"></asp:View>
                    </asp:MultiView>                                                                                                                                   
                </div>
            </div>
        </div>
    </div>
    
    <script type="text/javascript">

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
</asp:Content>

