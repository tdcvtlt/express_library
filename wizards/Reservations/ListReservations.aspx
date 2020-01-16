<%@ Page Title="" Language="VB" MasterPageFile="~/wizards/Reservations/ReservationMasterPage.master" AutoEventWireup="false" CodeFile="ListReservations.aspx.vb" Inherits="wizard_Reservations_ListReservations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head2" Runat="Server">

    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">


     <asp:UpdatePanel runat="server">
        <ContentTemplate>
                
            <div class="container">
                
                 <div class="panel panel-success">
                    <div class="panel-heading">
                        <div class="panel-heading">
                            <h3 style="font-weight:bold;" class="text-primary">Upcoming Reservations</h3>
                            <h4 class="control-label text-right  ">King's Creek Plantation &#174;</h4>                                    
                        </div>
                    </div> 

                    <div class="panel-body">
                        <div class="form-group form-group-sm">

                            <div class="row top-buffer">
                                <div class="col-sm-12">
                                    <asp:Label runat="server" ID="lbErr" CssClass="text-warning"></asp:Label>
                                    <br />
                                </div>
                            </div>
                            
                            <div class="row  top-buffer">
                                <div class="col-sm-12">
                                    <asp:GridView runat="server" ID="gvListReservations"  CssClass="control-label table table-striped table-bordered table-hover" AutoGenerateSelectButton="true" AutoGenerateColumns="true" DataKeyNames="Reservation ID" ></asp:GridView>
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
        </ContentTemplate>
    </asp:UpdatePanel>
    
   

    
   
</asp:Content>

