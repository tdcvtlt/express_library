<%@ Page Title="" Language="VB" MasterPageFile="~/wizards/Reservations/ReservationMasterPage.master" AutoEventWireup="false" CodeFile="EditAccommodation.aspx.vb" Inherits="wizards_Reservations_EditAccommodation" %>
<%@ Register Src="~/controls/DateField.ascx" TagName="DateField" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head2" Runat="Server">
    <script type="text/javascript">
        $(function () {
            var validator = $('form').validate(
                {
                    rules: {
                        ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddLocation: {
                            required: true
                        },
                        ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddAccom: {
                            required: true
                        },
                        ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddCheckInLocation: {
                            required: true
                        },
                        ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddGuestType: {
                            required: true
                        },
                        ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddRoomType: {
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

                }
            );




            $('#<%= btNext.ClientID%>').click(function (e) {

                var f = $('form');
                f.validate();
                    
                if (!f.valid()) {
                    e.preventDefault();
                }

            });

        });
    </script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
            
    <div class="container">
                <div class="form-group form-group-sm">
                    <div class="row top-buffer">
                        <div class="col-sm-12">
                            
                            <asp:MultiView runat="server" ID="multiview1" ActiveViewIndex="0">
                                <asp:View runat="server" ID="view1">
                                                            
                                    <div class="panel panel-success">
                                        <div class="panel-heading">
                                            <h3 style="font-weight:bold;" class="text-primary">Hotel Accommodation</h3>
                                            <h4 class="control-label text-right  ">King's Creek Plantation &#174;</h4>                            
                                        </div>
                                        <div class="panel-body">
                                            <div class="row top-buffer">
                                                    <div class="col-sm-2">
                                            <asp:Label ID="Label1" runat="server" CssClass="control-label" Text="ID" Font-Bold="true"></asp:Label>                                   
                                        </div>
                                                    <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txID" CssClass="form-control" Enabled="false" disabled="disabled" style="width:90%;"></asp:TextBox>
                                        </div>                
                                                    <div class="col-sm-2">
                                            <asp:Label ID="Label2" runat="server" CssClass="control-label" Text="Confirmation Number" Font-Bold="true" ></asp:Label>
                                        </div>
                                                    <div class="col-sm-3">
                                             <asp:TextBox runat="server" ID="txConfirmationNumber" CssClass="form-control text-uppercase" style="width:90%;"></asp:TextBox>
                                        </div>
                                            </div>

                                            <div class="row top-buffer">
                                                <div class="col-sm-2">
                                                    <asp:Label ID="Label3" runat="server" CssClass="control-label" Text=" Reservation Location" Font-Bold="true"></asp:Label>                                   
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList runat="server" ID="ddLocation" CssClass="form-control" Name="ddLocation" Enabled="false" AutoPostBack = "true" style="width:90%;"></asp:DropDownList>
                                                </div>                
                                                    <div class="col-sm-2">
                                            <asp:Label ID="Label4" runat="server" CssClass="control-label" Text="Accommodation" Font-Bold="true"></asp:Label>
                                        </div>
                                                    <div class="col-sm-3">
                                             <asp:DropDownList runat="server" ID="ddAccom" Name="ddAccom" Enabled="false" CssClass="form-control" AutoPostBack = "true" style="width:90%;"></asp:DropDownList>
                                        </div>
                                            </div>

                                            <div class="row top-buffer">
                                                    <div class="col-sm-2">
                                            &nbsp;                                                              
                                        </div>
                                                    <div class="col-sm-3">
                                            &nbsp;
                                        </div>                
                                            <div class="col-sm-2">
                                                <asp:Label ID="Label6" runat="server" CssClass="control-label" Text="Check-In Location" Font-Bold="true"></asp:Label>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:DropDownList runat="server" ID="ddCheckInLocation" Name="ddCheckInLocation" Enabled="false" CssClass="form-control" style="width:90%;"></asp:DropDownList>
                                            </div>
                                            </div>

                                            <div class="row top-buffer">
                                                    <div class="col-sm-2">
                                            <asp:Label ID="Label5" runat="server" CssClass="control-label" Text="Guest Type" Font-Bold="true"></asp:Label>                                   
                                        </div>
                                                    <div class="col-sm-3">
                                            <asp:DropDownList runat="server" ID="ddGuestType" Name="ddGuestType" CssClass="form-control" style="width:90%;"></asp:DropDownList>
                                        </div>                
                                                    <div class="col-sm-2">
                                            <asp:Label ID="Label7" runat="server" CssClass="control-label" Text="Room Type" Font-Bold="true"></asp:Label>
                                        </div>
                                                    <div class="col-sm-3">
                                             <asp:DropDownList runat="server" ID="ddRoomType" Name="ddRoomType" CssClass="form-control" style="width:90%;"></asp:DropDownList>
                                        </div>
                                            </div>

                                            <div class="row top-buffer">
                                                <br />
                                            </div>


                                            <div class="row top-buffer">
                                                    <div class="col-sm-2">
                                            <asp:Label ID="Label8" runat="server" CssClass="control-label" Text="Arrival Date" Font-Bold="true" ></asp:Label>                                   
                                        </div>
                                                    <div class="col-sm-3">
                                            <asp:TextBox ID="txArrivalDate" runat="server"  CssClass="form-control" ReadOnly="true"  style="width:90%;"></asp:TextBox>
                                        </div>                
                                                    <div class="col-sm-2">
                                            <asp:Label ID="Label9" runat="server" CssClass="control-label" Text="Departure Date" Font-Bold="true"></asp:Label>
                                        </div>
                                                    <div class="col-sm-3">
                                             <asp:TextBox ID="txDepartureDate" runat="server" CssClass="form-control" ReadOnly="true" style="width:90%;"></asp:TextBox>
                                        </div>
                                            </div>

                                            <div class="row top-buffer">
                                                <br /><br />
                                            </div>
                                        </div>

                                        <div class="panel-footer">
                                            <div class="row top-buffer">   
                                                <div class="col-sm-6 col-xs-12">&nbsp;</div>
                                                    <div class="col-sm-3 col-xs-6">
                                                         <asp:Button runat="server" ID="btPrevious" formnovalidate CssClass="btn btn-lg btn-primary" Style="width:160px;font-weight:bolder;" Text="&larr; Previous" />
                                                    </div>
                   
                                                    <div class="col-sm-3 col-xs-6">
                                                        <asp:Button runat="server" ID="btNext" CssClass="btn btn-lg btn-primary" Style="width:160px;font-weight:bolder;" Text="Next &rarr;" />
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
    
    
</asp:Content>

