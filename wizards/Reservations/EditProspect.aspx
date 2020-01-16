<%@ Page Title="" Language="VB" MasterPageFile="~/wizards/Reservations/ReservationMasterPage.master" AutoEventWireup="false" CodeFile="EditProspect.aspx.vb" Inherits="wizard_Reservations_Prospect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head2" Runat="Server">

    <script type="text/javascript">
        $(function () {

            var validator = $('form').validate({
                rules: {
                    ignore: '.skip',                   

                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txFirstName: {
                        required: true,
                        minlength: 2,
                        maxlength: 50
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txLastName: {
                        required: true,
                        minlength: 2,
                        maxlength: 50
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txSpouseFirstName: {
                        required: function (element) {
                            return $('option:selected', $('#<%= ddMarital.ClientID %>')).text() == 'Married'|| $('option:selected', $('#<%= ddMarital.ClientID %>')).text() == 'Partner' || $('option:selected', $('#<%= ddMarital.ClientID %>')).text() == 'Co-Habitant'
                        },
                        minlength: 0,
                        maxlength: 50
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txSpouseLastName: {
                        required: function (element) {
                            return $('option:selected', $('#<%= ddMarital.ClientID %>')).text() == 'Married' || $('option:selected', $('#<%= ddMarital.ClientID %>')).text() == 'Partner' || $('option:selected', $('#<%= ddMarital.ClientID %>')).text() == 'Co-Habitant'
                        },
                        minlength: 0,
                        maxlength: 50
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddMarital: {
                        required: true
                    },                    

                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txEmail: {
                        required:true,
                        email:true
                    },

                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txAddress1: {
                        required: true,
                        minlength:5
                    },

                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txNumber: {
                        required: true,
                        number: true,
                        minlength:10
                    },

                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txCity: {
                        required: true,
                        minlength:2
                    },

                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txZip: {
                        required: true,                        
                        minlength:5
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddState: {
                        required: true,                       
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$ddCountry: {
                        required: true,
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

            $('#<%= ddMarital.ClientID %>').on('change', function () {                                
                validator.resetForm();
            });

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
           
            
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div class="container">
        <div class="form-group form-group-sm">
            <div class="row top-buffer">
                <div class="col-md-12">
                    <asp:MultiView runat="server" ID="multiview1" ActiveViewIndex="0">
                        <asp:View runat="server" ID="view1">

                            <div class="panel panel-success">
                                <div class="panel-heading">
                                    <h3 style="font-weight:bold;" class="text-primary">Edit Prospect</h3>
                                    <h4 class="control-label text-right  ">King's Creek Plantation &#174;</h4>                                    
                                </div> 

                                <div class="panel-body">

                                    <div class="row top-buffer">
                                <div class="col-md-6" id="editProspect">
                                    <div class="row top-buffer">
                                        <div class="col-md-3">
                                            <asp:Label runat="server" CssClass="control-label" Text="Prospect ID"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="txProspectID" ReadOnly="true" disabled="disabled" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row top-buffer">
                                        <div class="col-md-3">
                                            <asp:Label runat="server" CssClass="control-label" Text="First Name"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox runat="server" CssClass="form-control text-capitalize" ID="txFirstName" name="txFirstName" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row top-buffer">
                                        <div class="col-md-3">
                                            <asp:Label runat="server" CssClass="control-label" Text="Last Name"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox runat="server" CssClass="form-control text-capitalize" ID="txLastName" name="txLastName" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row top-buffer">
                                        <div class="col-md-3">
                                            <asp:Label runat="server" CssClass="control-label" Text="Marital Status"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList runat="server" CssClass="form-control" placeholder="Marital Status" ID="ddMarital" name="ddMarital"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row top-buffer">
                                        <div class="col-md-3">
                                            <asp:Label runat="server" CssClass="control-label" Text="Spouse First Name"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox runat="server" class="skip" CssClass="form-control text-capitalize" ID="txSpouseFirstName" name="txSpouseFirstName" placeholder="Spouse First Name"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row top-buffer">
                                        <div class="col-md-3">
                                            <asp:Label runat="server" CssClass="control-label" Text="Spouse Last Name"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox runat="server" CssClass="skip form-control text-capitalize" ID="txSpouseLastName" name="txSpouseLastName" placeholder="Spouse Last Name"></asp:TextBox>
                                        </div>
                                    </div>                                    
                                </div>
                                <div class="col-md-6">
                                    <div class="row top-buffer">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" ID="lbAddEmail" CssClass="form-control" Text="Add Email"></asp:LinkButton>
                                            <asp:GridView runat="server" ID="gridview1" CssClass="table table-stripped table-hover table-bordered">
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate><asp:LinkButton ID="LinkButton1" runat="server" Text="Edit" CommandName="EmailEdit" CommandArgument='<%#Container.DataItem("ID")%>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="row top-buffer">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" ID="lbAddPhone" CssClass="form-control" Text="Add Phone Number"></asp:LinkButton>
                                            <asp:GridView runat="server" ID="gridview2" CssClass="table table-stripped table-hover table-bordered">
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" 
                                                                CommandArgument='<%#Container.DataItem("ID")%>' CommandName="PhoneEdit" 
                                                                Text="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="row top-buffer">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" ID="lbAddAddress" CssClass="form-control" Text="Add Address"></asp:LinkButton>
                                            <asp:GridView runat="server" ID="gridview3" CssClass="table table-stripped table-hover table-bordered">
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton3" runat="server" 
                                                                CommandArgument='<%#Container.DataItem("ID")%>' CommandName="AddressEdit" 
                                                                Text="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
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


                            
                            
                        </asp:View>
                        <asp:View runat="server" ID="view2">

                            <div class="panel panel-default">
                                <div class="panel panel-heading">
                                    <div class="panel-title">
                                        <h4>Email</h4>
                                    </div>                                    
                                </div>
                                <div class="panel-body">
                                        <div class="row top-buffer">
                                            <div class="col-md-1">
                                                <asp:Label runat="server" CssClass="control-label" Text="Email ID"></asp:Label>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txEmailID" ReadOnly="true" disabled="disabled" ></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Prospect ID"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txProspectID_E" ReadOnly="true" disabled="disabled" ></asp:TextBox>
                                </div>
                            </div>                                         
                                        <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Email"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control" name="txEmail" ID="txEmail"></asp:TextBox>
                                </div>
                            </div>          
                                        <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Active"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:CheckBox runat="server" ID="cbActive_E" />
                                </div>
                            </div>
                                        <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Primary"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:CheckBox runat="server" ID="cbPrimary_E" />
                                </div>
                            </div>
                                        
                                    </div>

                                <div class="panel-footer">
                                    <div class="row top-buffer">                                        
                                        <div class="col-md-3">                                                                                                   
                                            <asp:Button runat="server" ID="btCancel_E" formnovalidate CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="Cancel" />                                                                        
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Button runat="server" ID="btSubmit_E" CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="Submit" />
                                        </div>
                                    </div>
                                </div>
                            </div>                                                                                  
                        </asp:View>
                        <asp:View runat="server" ID="view3">
                            
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <div class="panel-title">
                                        <h4>Phone</h4>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Phone ID"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txPhoneID" ReadOnly="true" disabled="disabled" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Prospect ID"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txProspectID_P" ReadOnly="true" disabled="disabled" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Number"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control" name="txNumber" ID="txNumber"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Extension"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txExtension"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Type"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddType_P"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Active"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:CheckBox runat="server" ID="cbActive_P" />
                                </div>
                            </div>
                                </div>


                                <div class="panel-footer">
                                    <div class="row top-buffer">                                        
                                <div class="col-md-3">                                                                                                   
                                    <asp:Button runat="server" ID="btCancel_P" formnovalidate CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="Cancel" />                                                                        
                                </div>
                                <div class="col-md-3">
                                    <asp:Button runat="server" ID="btSubmit_P" CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="Submit" />
                                </div>
                            </div>
                                </div>
                            </div>
                                                        
                        </asp:View>
                        <asp:View runat="server" ID="view4">

                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <div class="panel-heading">
                                        <h4>Address</h4>
                                    </div>                                    
                                </div>
                                <div class="panel-body">
                                         <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Address ID"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txAddressID" ReadOnly="true" disabled="disabled" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Prospect ID"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txProspectID_A" ReadOnly="true" disabled="disabled" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Active"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:CheckBox runat="server" ID="cbActive_A" />
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Address 1"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control text-capitalize" name="txAddress1" ID="txAddress1"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Address 2"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control  text-capitalize" ID="txAddress2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="City"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control  text-capitalize" name="txCity" ID="txCity"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="State"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList runat="server" CssClass="form-control" name="ddState" ID="ddState"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Postal Code"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control" name="txZip" ID="txZip"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Region"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control  text-capitalize" ID="txRegion"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Country"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddCountry"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Type"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddType_A"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-1">
                                    <asp:Label runat="server" CssClass="control-label" Text="Contract Address"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:CheckBox runat="server" ID="cbContractAddress" />
                                </div>
                            </div>
                                    </div>

                                <div class="panel-footer">
                                    <div class="row top-buffer">                                        
                                <div class="col-md-3">                                                                                                   
                                    <asp:Button runat="server" ID="btCancel_A" formnovalidate CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="Cancel" />                                                                        
                                </div>
                                <div class="col-md-3">
                                    <asp:Button runat="server" ID="btSubmit_A" CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="Submit" />
                                </div>
                            </div>
                                </div>
                            </div>
                            


                           

                            

                        </asp:View>
                        <asp:View ID="view5" runat="server">
                            <div class="row top-buffer">
                                <div class="col-md-3">
                                    <asp:DropDownList runat="server" ID="ddSubject" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row top-buffer">                    
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" CssClass="form-control" placeholder="Phone" ID="txSearch" name="txSearch" ></asp:TextBox>
                                </div>
                                <div class="col-md-1">                                                    
                                    <asp:Button runat="server" CssClass="btn btn-success" ID="btSearch"  Text="Search" />
                                </div>
                                <div class="col-md-1">                                                    
                                    <asp:Button runat="server" CssClass="btn btn-success" ID="btCreate" Text="Create" />
                                </div>
                            </div>
                            <div class="row top-buffer">
                                <div class="col-md-6">
                                    <div style="height:400px;width:560px;overflow:auto;margin-top:40px;">
                                        <asp:GridView ID="gvProspectSearch" runat="server" AutoGenerateSelectButton="true" DataKeyNames="ProspectID" AutoGenerateColumns="true" 
                                            EmptyDataText="No Records" GridLines="Horizontal">
                                            <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                                            <AlternatingRowStyle BackColor="#CCFFCC" />                
                                        </asp:GridView>
                                    </div>                    
                                </div>
                        </div>
                        </asp:View>
                    </asp:MultiView>
                    <div class="row top-buffer">
                        
                        
                    </div>                        
                </div>
                <div class="col-md-6">
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
                
            }, 0);
        }   
    </script>
</asp:Content>

