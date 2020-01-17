<%@ Page Language="VB" AutoEventWireup="false" CodeFile="storecreditcard.aspx.vb" Inherits="marketing_storecreditcard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Store Credit Card</title>
<script type="text/javascript" src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.securesubmit.js"></script>
    <script type="text/javascript">
        var pkey = '';

        function Tokenize_CC() {

            var tokenValue, tokenType, tokenExpire;
            pkey = 'pkapi_prod_cAjaa3AjVm6c0INuBl';
            
            //Get_Key("1");
            if (pkey != '' && document.getElementById("ccnumberTxt").value != '' && document.getElementById("ccnumberTxt").value.substring(0, 4) != "****") {
                hps.tokenize({
                    data: {
                        public_key: pkey, //"pkapi_cert_frqsh5JXLFB9UbvCS3",
                        number: document.getElementById("ccnumberTxt").value,
                        cvc: document.getElementById("cvvTxt").value,
                        exp_month: document.getElementById("expTxt").value.substring(0, 2),
                        exp_year: '20' + document.getElementById("expTxt").value.substring(2)
                    },
                    success: function (response) {
                        /** Place additional validation/business logic here. */
                        if (response.card_type != '') {
                            var cc = document.getElementById("ccnumberTxt").value;
                            document.getElementById("hfTokenValue").value = response.token_value;
                            document.getElementById("hfCardType").value = response.card_type;
                            document.getElementById("ccnumberTxt").value = response.card.number;
                            document.getElementById("cvvTxt").value = '';
                            document.getElementById("swipeTxt").value = '';
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
                if (document.getElementById("ccnumberTxt").value.substring(0, 4) == "****" && document.getElementById("hfTokenValue").value != "0") {
                    $('#btnSave').click();
                }
                else {
                    alert("Error Processing. Please close this window and try again.");
                }
                //if (document.getElementById("ccnumberTxt").value != '') $('#Unnamed3_Click1').click();
            }
        }

    </script>
    <script language=javascript type = "text/javascript">
        function Read_Swipe() {

            var s = document.getElementById("swipeTxt").value;
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
            //alert(extract_section(s,ccnumstart,ccnumend));
            if (isNaN(extract_section(s, ccnumstart, ccnumend)) || s == '') {
                if (!document.getElementById("swipeTxt").disabled) {
                    document.getElementById("swipeTxt").value = '';
                    document.getElementById("swipeTxt").focus();
                }
                else {
                    document.getElementById("amount").focus();
                }
            }
            else {
                document.getElementById("ccnumberTxt").value = extract_section(s, ccnumstart, ccnumend);
                document.getElementById("expTxt").value = extract_section(s, ccexpmonthstart, ccexpmonthend) + '' + extract_section(s, ccexpyearstart, ccexpyearend);
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
            var temp = document.getElementById("swipeTxt").value;
            //var temp = document.forms.ccstore.swipe.value;
            if (temp.charAt(temp.length - 1) == '?') {
                for (i = 0; i < temp.length; i++) {
                    if (temp.charAt(i) == '?' && i <= temp.length) {
                        Read_Swipe();
                        break;
                    }
                    //alert(temp.charAt(i) + '-' + i + '-' + temp.length);
                }
            }
        }
    </script>
</head>
<body>
    
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

            
    <asp:multiview runat="server" id = "MultiView1">
        <asp:View runat="server" id = "ViewA">
            <table>
                <tr>
                    <td colspan = '2'><asp:LinkButton runat="server" onclick="Unnamed2_Click">OverRide</asp:LinkButton></td>
                </tr>
                <tr>
                    <td colspan = '2'><asp:LinkButton runat="server" id = "lbCConFile" OnClick="lbCardOnFile_Click">Use Card On File</asp:LinkButton></td>
                </tr>
                <tr>
                    <td>Swipe:</td>
                    <td><asp:TextBox runat="server" id = "swipeTxt" name = "swipe" onkeyup = "Swipe_Finished();" focus></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Credit Card Number:</td>
                    <td><asp:TextBox runat="server" id = "ccnumberTxt" name = "ccnumber"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Expiration:</td>
                    <td><asp:TextBox runat="server" id = "expTxt" name = "ccexp"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Name:</td>
                    <td><asp:TextBox runat="server" name = "ccname" id = "nameTxt"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>CVV2:</td>
                    <td><asp:TextBox runat="server" name = "cvv2" id = "cvvTxt"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Postal Code:</td>
                    <td><asp:TextBox runat="server" name = "postalcode" id = "postalCodeTxt"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan = '2'>
			<input id="btnTokenize" type="button" value="Save" onclick="Tokenize_CC();" />
			<div style="opacity:0;height:1px;width:1px;top:-1px;overflow:hidden;"><asp:Button runat="server" Text="Save" ID="btnSave"></asp:Button></div>
		    </td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Label ID="lblStatus" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>

	    <asp:HiddenField ID="hfTokenValue" Value="0" runat="server" />
        <asp:HiddenField ID="hfCardType" Value="" runat ="server" />
        <asp:HiddenField ID="hfCCID" Value ="0" runat="server" />
        <asp:HiddenField ID="hfCCTransID" Value="0" runat="server" />
            <asp:Timer ID="Timer1" runat="server" Enabled="False"></asp:Timer>
        </asp:View>  
        <asp:View runat="server" id = "ViewB">
            <table>
                <tr>
                    <td colspan = '2'><asp:LinkButton runat="server" onclick="Unnamed4_Click">Swipe Card</asp:LinkButton></td>
                </tr>
                <tr>
                    <td>UserName:
                    </td>
                    <td><asp:TextBox runat="server" id = "userNameTxt"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Password:</td>
                    <td><asp:TextBox runat="server" id = "passwordTxt" TextMode="Password"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan = '2'><asp:Button runat="server" Text="Save" onclick="Unnamed5_Click"></asp:Button></td>
                </tr>
            </table>
            <asp:Label runat="server" id = "lblErr"></asp:Label>
        </asp:View>
        <asp:View runat="server" ID="ViewC">
            <asp:LinkButton runat="server" id = "lbBack" OnClick="lbBack_Click">Insert New Card</asp:LinkButton>
            <br />
            <asp:GridView ID="gvCCOnFile" runat="server" AutoGenerateSelectButton="true"></asp:GridView>
        </asp:View>
    </asp:multiview>
                
    </form>
</body>
</html>
