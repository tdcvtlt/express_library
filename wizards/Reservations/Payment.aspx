<%@ Page Title="" Language="VB" MasterPageFile="~/wizards/Reservations/ReservationMasterPage.master" AutoEventWireup="false" CodeFile="Payment.aspx.vb" Inherits="wizard_Reservations_Payment" %>
<%@ Register Src="~/controls/Select_Item.ascx" TagPrefix="uc1" TagName="Select_Item" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head2" Runat="Server">
    <script type="text/javascript" src="../../Scripts/jquery.securesubmit.js"></script>
    <script type="text/javascript">

        $(function () {

            var validator = $('form').validate({
                rules: {
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txtCardNumber_CC: {
                        required: true,
                        minlength: 16,
                        maxlength: 20
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txtExpiration_CC: {
                        required: true,
                        number: true,
                        minlength: 4,
                        maxlength: 4
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txtName_CC: {
                        required: true,                       
                        minlength: 4,
                        maxlength: 50
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txtBillingAddress_CC: {
                        required: true,
                        minlength: 4,
                        maxlength: 50
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txtCity_CC: {
                        required: true,
                        minlength: 4,
                        maxlength: 50
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txtZip_CC: {
                        required: true,
                        minlength: 5,
                        maxlength: 10
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txtAmount_CC: {
                        required: true,
                        number: true
                    },
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txtDescription_CC: {
                        required: true                        
                    }
                },
                highlight: function (element) {
                    //$(element).closest('.form-group').addClass('has-error');
                    $(element).addClass('has-error');
                },
                unhighlight: function (element) {
                    //$(element).closest('.form-group').removeClass('has-error');
                    $(element).removeClass('has-error');
                },
                errorElement: 'span',
                errorClass: 'help-block',
                errorPlacement: function (error, element) {
                    error.insertAfter(element);
                }
            });


            $('#<%= btnProcess_CC.ClientID%>').click(function (e) {

                var f = $('form').form();
                
                if (!f.valid()) {
                    e.preventDefault();
                }
            });

        });
        
    </script>
    <script type="text/javascript">
        var Selected_Amount = 0;
        function Total_Allowed(cb, amt) {
            Selected_Amount += (cb.checked) ? amt : amt * -1;
        }

        function toggleDisplay(element) {
            var style;

            if (typeof element == 'string')
                element = document.getElementById ? document.getElementById(element) : null;
            if (element && (style = element.style))
                style.display = (style.display == 'none') ? 'block' : 'none';
        }
        function Read_Swipe() {
            
            var s = $('#<%= txtSwipe_CC.ClientID%>').val();
            var sExp = '';
            var sCCNum = '';
            var sLastname = '';
            var sFirstname = '';
            var lastnamestart = 0;
            var lastnameend = 0;
            var firstnamestart = 0;
            var firstnameend = 0;
            var ccnumstart = 0;
            var ccnumend = 0;
            var ccexpmonthstart = 0;
            var ccexpmonthend = 0;
            var ccexpyearstart = 0;
            var ccexpyearend = 0;
            for (i = 0; i < s.length; i++) {
                if (s.charAt(i) == '^' && lastnameend == 0) {
                    lastnamestart = i + 1;
                }
                if (s.charAt(i) == '/') {
                    lastnameend = i - 1;
                    firstnamestart = i + 1;
                }
                if (s.charAt(i) == '^' && lastnameend > 0) {
                    firstnameend = i - 1;
                }
                if (s.charAt(i) == ';') {
                    ccnumstart = i + 1;
                }
                if (s.charAt(i) == '=') {
                    ccnumend = i - 1;
                    ccexpmonthstart = i + 3;
                    ccexpmonthend = i + 4;
                    ccexpyearstart = i + 1;
                    ccexpyearend = i + 2;
                }
            }
            
            if (isNaN(extract_section(s, ccnumstart, ccnumend)) || s == '') {                

                if (!$('#<%= txtSwipe_CC.ClientID%>').disabled) {
                    $('#<%= txtSwipe_CC.ClientID%>').val('');
                    $('#<%= txtSwipe_CC.ClientID%>').focus();
                }
                else {                    
                    $('#<%= txtAmount_CC.ClientID%>').focus();
                }
            }
            else {               
                $('#<%= txtCardNumber_CC.ClientID%>').val(extract_section(s, ccnumstart, ccnumend));                
                $('#<%= txtExpiration_CC.ClientID%>').val(extract_section(s, ccexpmonthstart, ccexpmonthend) + '' + extract_section(s, ccexpyearstart, ccexpyearend));
            }
            return false;
        }

        function extract_section(swipe, start, end) {
            var temp = '';
            for (i = start; i <= end; i++) {
                temp += swipe.charAt(i);
            }
            return temp;
        }

        function Swipe_Finished() {
            var temp = $('#<%= txtSwipe_CC.ClientID%>').val();            
            if (temp.charAt(temp.length - 1) == '?') {
                for (i = 0; i < temp.length; i++) {
                    if (temp.charAt(i) == '?' && i <= temp.length) {
                        Read_Swipe();
                        break;
                    }                    
                }
            }
        }
    </script>
    <script type="text/javascript">
        var pkey = '';

        function Tokenize_CC() {

            var tokenValue, tokenType, tokenExpire;
            pkey = '';
            $(".Invoices tr").each(function (i, row) {
                var $row = $(row),
                    $cells = $row.find('td'),
                    $checkBox = $row.find('input:checked');
                if ($checkBox.is(":checked")) {
                    $cells.each(function (i, cell) {
                        if (i == 2) {
                            Get_Key(cell.innerHTML);
                            //alert(pkey);
                        }
                    });
                }
            });

            if (pkey != '' && $('#<%= txtCardNumber_CC.ClientID%>').val() != '' && $('#<%= txtCardNumber_CC.ClientID%>').val().substring(0, 4) != "****") {
                hps.tokenize({
                    data: {
                        public_key: pkey, //"pkapi_cert_frqsh5JXLFB9UbvCS3",
                        number: $('#<%= txtCardNumber_CC.ClientID%>').val(), 
                        cvc: $('#<%= txtCVV2_CC.ClientID%>').val(),
                        exp_month: $('#<%= txtExpiration_CC.ClientID%>').val().substring(0, 2),
                        exp_year: '20' + $('#<%= txtExpiration_CC.ClientID%>').val().substring(2)
                    },
                    success: function (response) {
                        /** Place additional validation/business logic here. */
                        if (response.card_type != '') {
                            var cc = $('#<%= txtCardNumber_CC.ClientID%>').val();
                            $('#<%= hfTokenValue.ClientID%>').val(response.token_value);
                            $('#<%= hfCardType.ClientID%>').val(response.card_type);
                            $('#<%= txtCardNumber_CC.ClientID%>').val(response.card.number);
                            $('#<%= txtCVV2_CC.ClientID%>').val('');
                            $('#<%= txtSwipe_CC.ClientID%>').val('');
                            tokenValue = response.token_value;
                            tokenType = response.token_type;
                            tokenExpire = response.token_expire;

                            $('#<%= btnProcess_CC.ClientID%>').click();
                        } else {
                            alert('Invalid Card.. Please try again');
                        }
                        return true;
                    },
                    error: function (response) {
                        if (typeof response === 'object') {
                            alert(response.message);
                        } else {
                            alert(response);
                        }
                        
                        $('#<%= hfTokenValue.ClientID%>').val(0);
                        return false;
                    }
                });
            } else {

               <%-- var val = $('#<%= txtCardNumber_CC.ClientID%>').val();               
                $('#<%= hfTokenValue.ClientID%>').val(3333);
                val = $('#<%= hfTokenValue.ClientID%>').val();
                alert(val);--%>

                if ($('#<%= txtCardNumber_CC.ClientID%>').val().substring(0, 4) == "****" && $('#<%= hfTokenValue.ClientID%>').val() != "0") {                    
                    $('#<%= btnProcess_CC.ClientID%>').click();
                } else {
                    alert("Error Processing. Please close this window and try again.");
                }
            }
        }

    </script>
    
    


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="form-group form-group-sm">
                    <div class="row top-buffer">
                        <div class="col-sm-12">
                            <asp:MultiView runat="server" ID="multiview2" ActiveViewIndex="0">
                                <asp:View runat="server" ID="view1">
                                    <div id="mainarea">
                                        
                                        <div class="row">
                                            <div class="col-sm-12">

                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                 <label class="col-sm-2 control-label"> Payment Type: </label>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList ID="ddPayMethod" runat="server" CssClass="form-control" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Value="Credit Card">Credit Card</asp:ListItem>
                                                        <asp:ListItem Value="Check">Check</asp:ListItem>                                                
                                                        <asp:ListItem Value="MoneyOrder">Money Order</asp:ListItem>
                                                        <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>                                                                                                                                                                                
                                                </div>

                                                <asp:MultiView runat="server" ID="multiview1" ActiveViewIndex="0">
                                                    <asp:View runat="server" ID="vwCC">                                                         
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <label class="col-sm-2 control-label">
                                                                </label>
                                                                <div class="col-sm-3">
                                                                    <asp:RadioButton ID="Charge" runat="server" CssClass="control-label" Checked="true" GroupName="cc" Text="&nbsp;Charge" />&nbsp;&nbsp;
                                                                    <asp:RadioButton ID="VoiceAuth" runat="server" CssClass="control-label" GroupName="cc" Text="&nbsp;Voice Auth" />
                                                                    <asp:RadioButton ID="Force" runat="server" GroupName="cc" Visible="false" Text="Force" />
                                                                    <asp:RadioButton ID="Manual" runat="server" GroupName="cc" Visible="false" Text="Manual" />
                                                                </div>
                                                            </div>
                                                        </div>                                                                                                     
                                                        <div class="row top-buffer">
                                                            <div class="col-sm-12">                                                                
                                                                <div class="row top-buffer">
                                                                    <div class="form-horizontal">                                                                        
                                                                        <div class="form-group">
                                                                            <label class="col-sm-2 control-label">Swipe:</label>
                                                                            <div class="col-sm-3">
                                                                                <asp:TextBox ID="txtSwipe_CC" runat="server" CssClass="form-control" onkeyup = "Swipe_Finished();"></asp:TextBox>
                                                                            </div>
                                                                            <label class="col-sm-2 control-label">
                                                                                <asp:LinkButton runat="server" onclick="Unnamed2_Click1" CssClass="control-label">Card(s) on file</asp:LinkButton>
                                                                            </label>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="col-sm-2 control-label">Card Number:</label>
                                                                            <div class="col-sm-3">
                                                                                <asp:TextBox ID="txtCardNumber_CC" runat="server" name="txtCardNumber_CC" CssClass="form-control" onkeyup = "Swipe_Finished();"></asp:TextBox>
                                                                            </div>
                                                                            <label class="col-sm-2 control-label">Expiration (MMYY):</label>
                                                                            <div class="col-sm-3">
                                                                                <asp:TextBox ID="txtExpiration_CC" name="txtExpiration_CC" runat="server" MaxLength="4" CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="col-sm-2 control-label">CVV2:</label>
                                                                            <div class="col-sm-3">
                                                                                <asp:TextBox ID="txtCVV2_CC" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                            <label class="col-sm-2 control-label">Name On Card:</label>
                                                                            <div class="col-sm-3">
                                                                                <asp:TextBox ID="txtName_CC" name="txtName_CC" runat="server" CssClass="form-control text-capitalize"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="col-sm-2 control-label">Billing Address:</label>
                                                                            <div class="col-sm-3">
                                                                                <asp:TextBox ID="txtBillingAddress_CC" name="txtBillingAddress_CC" runat="server" CssClass="form-control text-capitalize"></asp:TextBox>
                                                                            </div>
                                                                            <label class="col-sm-2 control-label">City</label>
                                                                            <div class="col-sm-3">
                                                                                <asp:TextBox ID="txtCity_CC" runat="server" name="txtCity_CC" CssClass="form-control text-capitalize"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="col-sm-2 control-label">State:</label>
                                                                            <div class="col-sm-3">
                                                                                <uc1:Select_Item ID="siState_CC" runat="server" ComboItem="State"  
                                                                                    Connection_String="resources.resource.cns" class="form-control" EnableTheming="False" />
                                                                            </div>
                                                                            <label class="col-sm-2 control-label">Postal Code:</label>
                                                                            <div class="col-sm-3">
                                                                                <asp:TextBox ID="txtZip_CC" runat="server" name="txtZip_CC" CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="col-sm-2 control-label">Amount:</label>
                                                                            <div class="col-sm-3">
                                                                               <asp:TextBox ID="txtAmount_CC" runat="server" name="txtAmount_CC"  CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                            <label class="col-sm-2 control-label"><asp:Button ID="btnRetAddress" runat="server" Text="Retrieve Address" formnovalidate CssClass="btn btn-info" onClick = "btnRetAddress_Click" autoPostBack = "true" /></label>
                                                                            <div class="col-sm-3">
                                                                                
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="col-sm-2 control-label">Description:</label>
                                                                            <div class="col-sm-8">
                                                                                <asp:TextBox ID="txtDescription_CC" runat="server" name="txtDescription_CC" CssClass="form-control text-capitalize"></asp:TextBox>
                                                                            </div>
                                                                            
                                                                        </div>
                                                                        <div class="form-group">     
                                                                            <label class="col-sm-2 control-label"></label>                                                                       
                                                                            <div class="col-sm-3">
                                                                                <input id="btnTokenize" type="button" value="Process Charge"  class="btn btn-danger" onclick="Tokenize_CC();" />
                                                                                <div style="opacity:0;width:1px;height:1px;overflow:hidden;top:-1px;">
                                                                                    <asp:Button ID="btnProcess_CC"  runat="server" enabled="true"  CssClass="btn btn-danger" Text="Credit Charge" />
                                                                                </div>

                                                                            </div>
                                                                            <label class="col-sm-2 control-label">Authorization:</label>
                                                                            <div class="col-sm-3">
                                                                                <asp:TextBox ID="txtAuthorization_CC" runat="server" ReadOnly="true" disabled="disabled" CssClass="form-control text-capitalize"></asp:TextBox>
                                                                            </div>
                                                                        </div>   
                                                                                                                                                                                                                  
                                                                    </div>
                                                                </div>                                                                                                                               
                                                            </div>
                                                        </div>
                                                        
                                                        
                                                    </asp:View>
                                                    <asp:View ID="vwCheck_ACH_MO" runat="server">
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <label class="col-sm-2 control-label">Number:</label>
                                                                <div class="col-sm-3">
                                                                   <asp:TextBox ID="txtNumber_Check_ACH_MO" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-sm-2 control-label">Amount:</label>
                                                                <div class="col-sm-3">
                                                                   <asp:TextBox ID="txtAmount_Check_ACH_MO" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-sm-2 control-label">&nbsp;</label>
                                                                <div class="col-sm-3">
                                                                   <asp:Button ID="btnProcess_Check_ACH_MO" runat="server" Text="Process" CssClass="btn btn-danger" /><asp:Button runat="server" id = "btnScheduleCheck" Text="Schedule Payment" visible = "False"></asp:Button>
                                                                </div>
                                                            </div>
                                                        </div>                                             
                                                    </asp:View>
                                                    <asp:View ID="vwCash_Equity_ExitEquity" runat="server">
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <label class="col-sm-2 control-label">Amount:</label>
                                                                <div class="col-sm-3">
                                                                   <asp:TextBox ID="txtAmount_Cash_Equity_ExitEquity" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-sm-2 control-label">&nbsp;</label>
                                                                <div class="col-sm-3">
                                                                   <asp:Button ID="btnProcess_Cash_Equity_ExitEquity" runat="server" Text="Process" CssClass="btn btn-danger" /><asp:Button runat="server" id = "btnScheduleCash" Text="Schedule Payment" visible = "False"></asp:Button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:View>
                                                    <asp:View runat="server" ID="vwCardOnFile">
                                                        <div class="row top-buffer">
                                                            <div class="col-sm-12">
                                                                <asp:GridView runat="server" autogenerateselectbutton="true" id = "gvCardOnFile"  CssClass="control-label  table table-striped table-bordered table-hover"  emptydatatext = "No Cards On File" onRowDataBound = "gvCardOnFile_RowDataBound">
                                                                    <AlternatingRowStyle BackColor="#C7E3D7" />
                                                                </asp:GridView>
                                                                <asp:Button runat="server" Text="Cancel" CssClass="btn btn-primary" onclick="Unnamed2_Click"></asp:Button>
                                                            </div>
                                                        </div>
                                                    </asp:View>
                                                </asp:MultiView>

                                                <div class="row top-buffer">
                                                    <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="col-sm-2 control-label">  </label>
                                                    <asp:Label runat="server" ID="lblErr" ForeColor="Red" CssClass="col-sm-8" Text=""></asp:Label>
                                                </div>                            
                                            </div>
                                                </div>
                                                <div class="row top-buffer">
                                                            <div class="form-horizontal">
                                                                <div class="form-group">
                                                                    <label class="col-sm-2 control-label">  </label>
                                                                    <div class="col-sm-8">
                                                                        <asp:GridView ID="gvInvoices" runat="server" AutoGenerateColumns="false" onRowDataBound = "gvInvoices_RowDataBound" CssClass="Invoices table table-striped table-bordered table-hover">
                                                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                                                            <Columns>
                                                                                    <asp:templatefield>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="cb" CssClass="CBCheck" runat="server"  AutoPostBack="false"   />
                                                                                        </ItemTemplate>
                                                                                    </asp:templatefield>
                                                                                    <asp:BoundField HeaderText="ID" DataField = "ID" />
                                                                                    <asp:BoundField HeaderText="Acct" DataField = "Acct" />
                                                                                    <asp:BoundField HeaderText="Invoice" DataField = "Invoice" />
                                                                                    <asp:BoundField HeaderText="TransDate" DataField = "TransDate" />
                                                                                    <asp:BoundField HeaderText="Amount" DataField = "Amount" />
                                                                                    <asp:BoundField HeaderText="Balance" DataField = "Balance" />
                                                                                    <asp:BoundField HeaderText="CCApproval" DataField = "CCApproval" />
                                                                                    <asp:BoundField HeaderText="" DataField="AFC" />
                                                                                </Columns>
                                                                        </asp:GridView>

                                                                        <br />
                                                                        
                                                                        <asp:Literal runat="server" ID="ll1"></asp:Literal>
                                                                    </div>
                                                                </div>                                                       
                                                            </div>
                                                        </div>
                                            </div>
                                        </div>                                            
                                        
                                           
                                        <!--                                                                                        
                                            -->
                                        <div class="row top-buffer">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="col-sm-2 control-label">  </label>
                                                    <div class="col-sm-3">
                                                        <asp:Button runat="server" ID="btNext" CssClass="btn btn-lg btn-primary" Style="width:160px;font-weight:bolder;" Text="Next &rarr;" />
                                                        <asp:Button runat="server" ID="btNext2" CssClass="btn btn-lg btn-success" style="opacity:0;width:1px;height:1px;overflow:hidden;top:-1px;" Visible="true" />
                                                    </div>
                                                </div>
                                            </div>                        
                                        </div>                                                                                                                                                         
                                    </div>
                                </asp:View>
                                <asp:View ID="view2" runat="server">
                                    <div class="panel panel-success">
                                        <div class="panel-heading">
                                            <h4>Credit Card Processing...</h4>
                                        </div>
                                        <div class="panel-body">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="lblWaiting" runat="server" Text="Processing... Please Wait" ></asp:Label>            
                                                    </div>
                                                                            
                                                    <div class="col-sm-4">
                                                       <asp:Timer ID="tmrCheck" runat="server" Enabled="False" Interval="1000"></asp:Timer>       
                                                    </div>
                                                </div>
                                            </div>

                                            <br /><br />
                                            <asp:Button runat="server" ID="btCcApprove" Text="Approve" CssClass="btn btn-success" />&nbsp;
                                            <asp:Button runat="server" ID="btCcDecline" Text="Decline" CssClass="btn btn-danger" />

                                        </div>
                                    </div>
                                                                         
                                </asp:View>
                            </asp:MultiView>
                        </div>
                    </div>                                        
                </div>                
            </div>

             <asp:HiddenField ID="hfReceiptURL" Value="0" runat="server" />
            <asp:HiddenField ID="hfCCID" Value="0" runat="server" />
            <asp:HiddenField ID="hfpayments" Value="0" runat="server" />
            <asp:HiddenField ID="hfTokenValue" Value ="0" runat ="server" />
            <asp:HiddenField ID="hfAFC_forbidden" Value="0" runat="server" />
            <asp:HiddenField ID="hfCardType" Value ="" runat="server" />
            <asp:HiddenField ID="hfTickCounter" Value="0" runat="server" />
            <asp:HiddenField ID="CCTransID" runat="server" Value="0" />

            <asp:HiddenField ID="hfCashAmount" Value="0" runat="server" />
            <asp:HiddenField runat="server" ID="hfBalanceGreaterZero" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">

        $(function () {

            $('#<%= btCcApprove.ClientID %>').click(function (e) {
                $(this).prop('disabled', true);
            });

            $('#<%= btCcDecline.ClientID %>').click(function (e) {
                $(this).attr('disabled', 'disabled');
            });
        });
    </script>


<script type="text/javascript">
    function beforeAsyncPostBack() {
                
        if ($('#<%= ddPayMethod.ClientID%> option:selected').text() == 'Credit Card') {
            $('#<%= btnProcess_CC.ClientID%>').attr('disabled', 'disabled');
            $('#btnTokenize').attr('disabled', 'disabled');
        }
    }

    function afterAsyncPostBack() {
              
        if ($('#<%= ddPayMethod.ClientID%> option:selected').text() == 'Credit Card') {                
            if ($('#<%= lblErr.ClientID %>').val() == '' && $('#<%= txtAuthorization_CC.ClientID %>').val() == 'Declined') {
                    alert("Declined");
                }
        } else if ($('#<%= ddPayMethod.ClientID%> option:selected').text() != 'Credit Card') {            

            if ($('#<%= hfCashAmount.ClientID %>').val() > 0) {                

                var n = noty({
                    text: 'Your amount of $' + $('#<%= hfCashAmount.ClientID %>').val() + ' has been applied toward the balance!',
                    type: 'success',
                    dismissQueue: true,
                    layout: 'centerRight',
                    theme: 'defaultTheme',
                    animation: {
                        open: 'animated bounceInRight',
                        close: 'animated bounceOutLeft',
                        easing: 'swing',
                        speed: 500
                    },
                    timeout: true
                });


                window.setTimeout(function () { n.close(); }, 3000);
            }
        }  
        
         
        // Only for this page because it is run on NET.20
        if ($('#<%=  hfBalanceGreaterZero.ClientID %>').val() > 0) {

            var balance = new Number($('#<%=  hfBalanceGreaterZero.ClientID %>').val());           

            var r = confirm('There is a balance of $' + balance.toFixed(2) + ', are you sure you want to continue?');
            if (r == true) {
                $('#<%= btNext2.ClientID %> ').click();
            }                
        }       
        
        
             

    }
    Sys.Application.add_init(appl_init);

    function appl_init() {
        var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
        pgRegMgr.add_beginRequest(BeginHandler);
        pgRegMgr.add_endRequest(EndHandler);
    }

    function BeginHandler() {
        beforeAsyncPostBack();
    }

    function EndHandler() {
        afterAsyncPostBack();
    }

    </script>

</asp:Content>

