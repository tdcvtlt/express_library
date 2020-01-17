<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="BankingFeeBulkProcessing.aspx.vb" Inherits="Points_BankingFeeBulkProcessing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../scripts/jquery.securesubmit.js"></script>
    <script type="text/javascript">
        var pkey = '';
                
        function Tokenize_CC(number, cvc, month, year,box) {
            
            var tokenValue, tokenType, tokenExpire;
            pkey = 'pkapi_prod_lNG17W6Xtb9BQjldWo';
            

            if (number != '' && number.substring(0, 4) != "****") {
                hps.tokenize({
                    data: {
                        public_key: pkey, //"pkapi_cert_frqsh5JXLFB9UbvCS3",
                        number: number,
                        cvc: cvc,
                        exp_month: month,
                        exp_year: year
                    },
                    success: function (response) {
                        /** Place additional validation/business logic here. */
                        if (response.card_type != '') {
                            $(box).val(response.token_value
                                + '|' 
                                + response.card_type 
                                + '|' 
                                + response.card.number
                                );
                            
                        } else {
                            $(box).val('Invalid Card.. Please try again');
                        }
                        return true;
                    },
                    error: function (response) {
                        if (typeof response === 'object') {
                            $(box).val(response.message);
                        } else {
                            $(box).val(response);
                        }
                        
                        return false;
                    }
                });
            } else {
                //if (document.getElementById("txtCardNumber_CC").value.substring(0, 4) == "****" && document.getElementById("hfTokenValue").value !="0" ) {
			    //    $('#btnProcess_CC').click();
		        //} else {
			    //    alert("Error Processing. Please close this window and try again.");
		        //}
            }
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="lbFileUpload" runat="server" Text="File:"></asp:Label>
    <asp:FileUpload ID="xlsUpload" runat="server" ToolTip="File" />
    <asp:Button ID="btnUpload" runat="server" Text="Upload File" />
    <br />
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    <br />
    <asp:GridView ID="gvBatch" runat="server" EnableModelValidation="True">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:textbox ID="txtCC" Text="" runat="server"></asp:textbox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
    <asp:Button ID="btnProcess" runat="server" text="Process (step 1)" />
    <br />
    <asp:Button ID="btnStep2" runat="server" text="Process (step 2)" />
    <br />
    <asp:GridView ID="gvRejects" runat="server" AutoGenerateColumns="true"></asp:GridView>
</asp:Content>

