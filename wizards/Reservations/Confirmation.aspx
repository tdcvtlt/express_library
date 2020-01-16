<%@ Page Title="" Language="VB" MasterPageFile="~/wizards/Reservations/ReservationMasterPage.master" AutoEventWireup="false" CodeFile="Confirmation.aspx.vb" Inherits="wizard_Reservations_Prompt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head2" Runat="Server">
    <script type="text/javascript" src="../../Scripts/pop_modal.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div class="container">
        <div class="form-group form-group-sm">
            <div class="row top-buffer">
                <div class="col-sm-12">
                    <div class="panel panel-success">
                        <div class="panel-heading">
                            <h3 style="font-weight:bold;" class="text-primary">Confirmation Letter</h3>
                            <h4 class="control-label text-right  ">King's Creek Plantation &#174;</h4>
                        </div>
                        <div class="panel-body">
                            <div class="row top-buffer">
                                <div class="col-sm-2"></div>
                                <div>
                                    <asp:CheckBox runat="server" ID="cbConfEmail" CssClass="control-label  text-capitalize text-danger text-center" style="font-size:20px;" Text="&nbsp; Email " />                                    
                                    <span runat="server" class=" text-lowercase text-primary text-center" id="spEmail"></span>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-sm-2"></div>
                                <div>
                                    <asp:CheckBox runat="server" ID="cbConfPrint" CssClass="control-label text-capitalize text-danger text-center" style="font-size:20px;" Text="&nbsp; Print " />                                    
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">&nbsp;</div>
                                <div class="col-md-3">                                 
                                    <asp:Button runat="server" ID="btSubmit" CssClass="btn btn-lg btn-success"  Style="font-weight:bolder;" Text="Submit" />                            
                                    &nbsp;
                                    <asp:Button runat="server" ID="btFinish" CssClass="btn btn-lg btn-success" Style="font-weight:bolder;" Text="Finish" />
                                </div>
                                <div class="col-md-1"></div>                        
                            </div>
                        </div>
                    </div>                                                       
                </div>
            </div>                    
        </div>
    </div>
</asp:Content>

