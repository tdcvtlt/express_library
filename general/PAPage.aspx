<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PAPage.aspx.vb" Inherits="general_PAPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.securesubmit.js"></script>
    <script type="text/javascript">
        var pkey = '';

        function Tokenize_CC() {

            var tokenValue, tokenType, tokenExpire;
            pkey = '';
            Get_Key(document.getElementById("ctl00_ContentPlaceHolder1_ddMerchantAccount").value);

            if (pkey != '' && document.getElementById("ctl00_ContentPlaceHolder1_txtCCNumber").value != '' && document.getElementById("ctl00_ContentPlaceHolder1_txtCCNumber").value.substring(0, 4) != "****") {
                document.getElementById("btnTokenize").disabled = true;
                hps.tokenize({
                    data: {
                        public_key: pkey, //"pkapi_cert_frqsh5JXLFB9UbvCS3",
                        number: document.getElementById("ctl00_ContentPlaceHolder1_txtCCNumber").value,
                        cvc: document.getElementById("ctl00_ContentPlaceHolder1_txtCVV").value,
                        exp_month: document.getElementById("ctl00_ContentPlaceHolder1_txtExpiration").value.substring(0, 2),
                        exp_year: '20' + document.getElementById("ctl00_ContentPlaceHolder1_txtExpiration").value.substring(2)
                    },
                    success: function (response) {
                        /** Place additional validation/business logic here. */
                        if (response.card_type != '') {
                            var cc = document.getElementById("ctl00_ContentPlaceHolder1_txtCCNumber").value;
                            document.getElementById("ctl00_ContentPlaceHolder1_hfTokenValue").value = response.token_value;
                            document.getElementById("ctl00_ContentPlaceHolder1_hfCardType").value = response.card_type;
                            document.getElementById("ctl00_ContentPlaceHolder1_txtCCNumber").value = response.card.number;
                            document.getElementById("ctl00_ContentPlaceHolder1_txtCVV").value = '';
                            tokenValue = response.token_value;
                            tokenType = response.token_type;
                            tokenExpire = response.token_expire;
                            $('#ctl00_ContentPlaceHolder1_btnSave').click();
                        } else {
                            alert('Invalid Card.. Please try again');
                            document.getElementById("btnTokenize").disabled = false;
                        }
                        return true;
                    },
                    error: function (response) {
                        if (typeof response === 'object') {
                            alert(response.message);
                        } else {
                            alert(response);
                        };

                        document.getElementById("ctl00_ContentPlaceHolder1_hfTokenValue").value = '0';
                        document.getElementById("btnTokenize").disabled = false;
                        return false;
                    }
                });
            } else {
                //$('#ContentPlaceHolder1_btnSave').click();
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <asp:UpdatePanel runat="server" id = "UpdatePanel1">
            <ContentTemplate>
    <table>
        <tr>
            <td>Transaction Code:</td>
            <td><asp:DropDownList runat="server" id = "ddMerchantAccount"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Credit Card Number:</td>
            <td>
                <asp:TextBox ID="txtCCNumber" runat="server" MaxLength="16"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Expiration (MMYY):</td>
            <td>
                <asp:TextBox ID="txtExpiration" runat="server" MaxLength="4"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Amount:</td>
            <td>
                <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Name:</td>
            <td>
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Zip:</td>
            <td>
                <asp:TextBox ID="txtPostalCode" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>CVV2:</td>
            <td>
                <asp:TextBox ID="txtCVV" runat="server" MaxLength="5"></asp:TextBox>
            </td>
        </tr>
        <tr><td></td></tr>
        </table>
            <input id="btnTokenize" type="button" value="Process Charge" onclick="Tokenize_CC();" />
            <div style="opacity:0"><asp:Button runat="server" Text="Process" id = "btnSave"></asp:Button></div>
            <br />
            <asp:TextBox ID="txtResponse" readonly runat="server"></asp:TextBox>
            <br />
            <asp:Label runat="server" id = "lblWaiting"></asp:Label>    
            <asp:Timer runat="server" id = "tmrCheck" Enabled="False"></asp:Timer>
            <asp:HiddenField ID="hfTickCounter" Value="0" runat="server" />
            <asp:HiddenField ID="hfCCTransID" Value="0" runat="server" />
                <asp:HiddenField ID="hfTokenValue" Value="0" runat="server" />
                <asp:HiddenField ID="hfCardType" Value="" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

