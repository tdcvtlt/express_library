<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ccOnFile.aspx.vb" Inherits="general_ccOnFile" %>
<%@ Register Src="~/controls/Select_Item.ascx"  tagname="Select_Item" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script type="text/javascript" src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.securesubmit.js"></script>
    <script type="text/javascript">
        var pkey = '';

        function Tokenize_CC() {

            var tokenValue, tokenType, tokenExpire;
            pkey = '';
            
            Get_Key(document.getElementById("ddAcct").value);
            if (pkey != '' && document.getElementById("txtNumber").value != '' && document.getElementById("txtNumber").value.substring(0, 4) != "****") {
                hps.tokenize({
                    data: {
                        public_key: pkey, //"pkapi_cert_frqsh5JXLFB9UbvCS3",
                        number: document.getElementById("txtNumber").value,
                        cvc: document.getElementById("txtCVV").value,
                        exp_month: document.getElementById("ddMonth").value,
                        exp_year: '20' + document.getElementById("ddYear").value
                    },
                    success: function (response) {
                        /** Place additional validation/business logic here. */
                        if (response.card_type != '') {
                            var cc = document.getElementById("txtNumber").value;
                            document.getElementById("hfTokenValue").value = response.token_value;
                            document.getElementById("hfCardType").value = response.card_type;
                            document.getElementById("txtNumber").value = response.card.number;
                            document.getElementById("txtCVV").value = '';
                            tokenValue = response.token_value;
                            tokenType = response.token_type;
                            tokenExpire = response.token_expire;
                            $('#btnSave').click();
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

                        document.getElementById("hfTokenValue").value = '0';
                        return false;
                    }
                });
            } else {
                if (document.getElementById("txtNumber").value != '' && document.getElementById("txtNumber").value.substring(0, 4) == "****") $('#btnSave').click();
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>Type:</td>
                <td><asp:Label ID="lblType" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td>Number:</td>
                <td><asp:TextBox ID="txtNumber" runat="server" ReadOnly = "true"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Expiration:</td>
                <td><asp:DropDownList ID="ddMonth" runat="server"></asp:DropDownList> / <asp:DropDownList ID="ddYear" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Name On Card:</td>
                <td><asp:TextBox ID="txtName" runat="server" readonly = "true"></asp:TextBox></td>
            </tr>
            <tr>
                <td>CVV:</td>
                <td><asp:TextBox ID="txtCVV" runat="server" ReadOnly = "true"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="address_label" runat="server" Visible="false">Address:</asp:Label></td>
                <td><asp:TextBox ID="txtAddress" runat="server" Visible="false"></asp:TextBox></td>
            </tr>  
            <tr>
                <td><asp:Label ID="city_label" runat="server" Visible="false">City:</asp:Label></td>
                <td><asp:TextBox ID="txtCity" runat="server" Visible="false"></asp:TextBox></td>
            </tr>            
            <tr>
                <td><asp:Label ID="postal_label" runat="server" Visible="false">Postal Code:</asp:Label></td>
                <td><asp:TextBox ID="txtPostalCode" runat="server" Visible="false"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="state_label" runat="server" Visible="false">State:</asp:Label></td>
                <td><uc1:Select_Item ID="siState" runat="server" visible="false" /></td>
            </tr>
            <tr>
                <td>Acct:</td>
                <td>
                    <asp:DropDownList ID="ddAcct" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan = "2">
                    <input id="btnTokenize" type="button" value="Save" onclick="Tokenize_CC();" />
                    <asp:Button ID="btnCancel" runat="server" Text="Close" />
                    <div style="opacity:0;"><asp:Button ID="btnSave" runat="server" Text="Save" /></div>
                    <asp:Label ID="lblErr" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hfTokenValue" Value="0" runat="server" />
        <asp:HiddenField ID="hfCardType" Value="" runat ="server" />
    </div>
    </form>
</body>
</html>
