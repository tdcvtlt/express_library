﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TakePayment.aspx.vb" Inherits="general_TakePayment" %>

<%@ Register src="~/controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register src="~/controls/DateField.ascx" tagname="DateField" tagprefix="uc2" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.securesubmit.js"></script>
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
    <script type="text/javascript" language="javascript">
        var Selected_Amount = 0;
        function Total_Allowed(cb, amt){
            Selected_Amount += (cb.checked)?amt:amt * -1;
        }

        function toggleDisplay(element) {
            var style;

            if (typeof element == 'string')
                element = document.getElementById ? document.getElementById(element) : null;
            if (element && (style = element.style))
                style.display = (style.display == 'none') ? 'block' : 'none';
        }
        function Read_Swipe() {

            var s = document.getElementById("txtSwipe_CC").value;
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
                if (!document.getElementById("txtSwipe_CC").disabled) {
                    document.getElementById("txtSwipe_CC").value = '';
                    document.getElementById("txtSwipe_CC").focus();
                }
                else {
                    document.getElementById("txtAmount_CC").focus();
                }
            }
            else {
                document.getElementById("txtCardNumber_CC").value = extract_section(s, ccnumstart, ccnumend);
                document.getElementById("txtExpiration_CC").value = extract_section(s, ccexpmonthstart, ccexpmonthend) + '' + extract_section(s, ccexpyearstart, ccexpyearend);
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
            var temp = document.getElementById("txtSwipe_CC").value;
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

            if (pkey != '' && document.getElementById("txtCardNumber_CC").value != '' && document.getElementById("txtCardNumber_CC").value.substring(0, 4) != "****") {
                hps.tokenize({
                    data: {
                        public_key: pkey, //"pkapi_cert_frqsh5JXLFB9UbvCS3",
                        number: document.getElementById("txtCardNumber_CC").value,
                        cvc: document.getElementById("txtCVV2_CC").value,
                        exp_month: document.getElementById("txtExpiration_CC").value.substring(0, 2),
                        exp_year: '20' + document.getElementById("txtExpiration_CC").value.substring(2)
                    },
                    success: function (response) {
                        /** Place additional validation/business logic here. */
                        if (response.card_type != '') {
                            var cc = document.getElementById("txtCardNumber_CC").value;
                            document.getElementById("hfTokenValue").value = response.token_value;
                            document.getElementById("hfCardType").value = response.card_type;
                            document.getElementById("txtCardNumber_CC").value = response.card.number;
                            document.getElementById("txtCVV2_CC").value = '';
			                document.getElementById("txtSwipe_CC").value = '';
                            tokenValue = response.token_value;
                            tokenType = response.token_type;
                            tokenExpire = response.token_expire;
                            $('#btnProcess_CC').click();
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
                if (document.getElementById("txtCardNumber_CC").value.substring(0, 4) == "****" && document.getElementById("hfTokenValue").value !="0" ) {
			$('#btnProcess_CC').click();
		} else {
			alert("Error Processing. Please close this window and try again.");
		}
            }
        }
    
    </script>
</head>
<body>
<form id="form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">


<ContentTemplate>
    <asp:MultiView ID="MultiView2" runat="server">
    <asp:View ID="View1" runat="server">
    <asp:Table runat="server"></asp:Table>
    <div id="mainarea">
        <div>
            Payment Type:
            <asp:DropDownList ID="ddPayMethod" runat="server" AutoPostBack="true">
                <asp:ListItem Selected="True" Value="Credit Card">Credit Card</asp:ListItem>
                <asp:ListItem Value="Check">Check</asp:ListItem>
                <asp:ListItem Value="ACH Payment">ACH Payment</asp:ListItem>
                <asp:ListItem Value="MoneyOrder">Money Order</asp:ListItem>
                <asp:ListItem Value="Cash">Cash</asp:ListItem>
                <asp:ListItem Value="Equity">Equity</asp:ListItem>
                <asp:ListItem Value="Exit Equity">Exit Equity</asp:ListItem>
                <asp:ListItem Value="Concord Payment">Concord Payment</asp:ListItem>
                <asp:ListItem Value="ChargeBack Employee">ChargeBack Employee</asp:ListItem>
                <asp:ListItem Value="Adjustment">Adjustment</asp:ListItem>
                <asp:ListItem Value="Online">Online</asp:ListItem>
                <asp:ListItem Value="Leisure Link">Leisure Link</asp:ListItem>
                <asp:ListItem Value="Meridian">Meridian</asp:ListItem>
                <asp:ListItem Value="Meridian - AFC">Meridian - AFC</asp:ListItem>
                <asp:ListItem Value="Aspen">Aspen</asp:ListItem>
                <asp:ListItem Value="AFC Payment">AFC Payment</asp:ListItem>
                <asp:ListItem Value="Equiant Payment">Equiant Payment</asp:ListItem>
            </asp:DropDownList>
            <%If Request("Schedule") = "True" Then %>
            <br />
            <asp:Label ID="Label1" runat="server" Text="Number of Payments (Months - Occurs on the same day of the month selected below):"></asp:Label>
            <asp:TextBox ID="txtPayments" runat="server" Width="35px">1</asp:TextBox>
            <asp:RangeValidator ID="RangeValidator1" runat="server" 
                ControlToValidate="txtPayments" Display="Dynamic" 
                ErrorMessage="Must be between 1 and 9." MaximumValue="9" MinimumValue="1" 
                Type="Integer"></asp:RangeValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="txtPayments" Display="Dynamic" ErrorMessage="Required"></asp:RequiredFieldValidator>
                <br />
            Split amount entered:<asp:CheckBox ID="cbAutoSplit" runat="server" />

            <%end if %>
        </div>

        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="vwCC" runat="server">
                <asp:HiddenField ID="CCTransID" runat="server" Value="0" />
                 <%If Request("Schedule") <> "True" Then %>
                <asp:RadioButton ID="Charge" runat="server" Checked="True" GroupName="cc" 
                    Text="Charge" />
                <asp:RadioButton ID="Force" runat="server" GroupName="cc" Text="Force" />
                <asp:RadioButton ID="Manual" runat="server" GroupName="cc" 
                    Text="Manual" />
                <asp:RadioButton ID="VoiceAuth" runat="server" GroupName="cc" 
                    Text="Voice Auth" />
                <% end if %>
                <br />
                <table class="style1">
                    <tr>
                        <td>
                            Swipe:</td>
                        <td>
                            <asp:TextBox ID="txtSwipe_CC" runat="server" onkeyup = "Swipe_Finished();"></asp:TextBox>
                        </td>
                        <td>
                            <asp:LinkButton runat="server" onclick="Unnamed2_Click1">Card(s) on file</asp:LinkButton></td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            Card Number:</td>
                        <td>
                            <asp:TextBox ID="txtCardNumber_CC" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Expiration (MMYY):</td>
                        <td>
                            <asp:TextBox ID="txtExpiration_CC" runat="server" MaxLength="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            CVV2:</td>
                        <td>
                            <asp:TextBox ID="txtCVV2_CC" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Name On Card:</td>
                        <td>
                            <asp:TextBox ID="txtName_CC" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Billing Address:</td>
                        <td>
                            <asp:TextBox ID="txtBillingAddress_CC" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            City:</td>
                        <td>
                            <asp:TextBox ID="txtCity_CC" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            State:</td>
                        <td>
                            <uc1:Select_Item ID="siState_CC" runat="server" ComboItem="State" 
                                Connection_String="resources.resource.cns" EnableTheming="False" />
                        </td>
                        <td>
                            Postal Code:</td>
                        <td>
                            <asp:TextBox ID="txtZip_CC" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Amount:</td>
                        <td>
                            <asp:TextBox ID="txtAmount_CC" runat="server"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:Button ID="btnRetAddress" runat="server" Text="Retrieve Address" onClick = "btnRetAddress_Click" autoPostBack = "true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Description:</td>
                        <td colspan="3">
                            <asp:TextBox ID="txtDescription_CC" runat="server" Width="491px"></asp:TextBox>
                        </td>
                    </tr>
                    <%If Request("Schedule") = "True" Then %>
                    <tr>
                        <td>Scheduled Date:</td>
                        <td>
                            <uc2:DateField ID="dteScheduledCC" runat="server" />
                        </td>
                    </tr>
                    <%end if %>

                    <tr>
                        <td colspan="2">
                            <input id="btnTokenize" type="button" value="Process Charge" onclick="Tokenize_CC();" />
                            <div style="opacity:0;width:1px;height:1px;overflow:hidden;top:-1px;"><asp:Button ID="btnProcess_CC"  runat="server" Text="Process Charge" /></div>
                        </td>
                        <td>
                            Authorization:</td>
                        <td>
                            <asp:TextBox ID="txtAuthorization_CC" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwCheck_ACH_MO" runat="server">
                <table class="style1">
                    <tr>
                        <td>
                            Number:</td>
                        <td>
                            <asp:TextBox ID="txtNumber_Check_ACH_MO" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Amount:</td>
                        <td>
                            <asp:TextBox ID="txtAmount_Check_ACH_MO" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <%If Request("Schedule") = "True" Then %>
                    <tr>
                        <td>Scheduled Date:</td>
                        <td>
                            <uc2:DateField ID="dteScheduledCheck" runat="server" />
                        </td>
                    </tr>
                    <%end if %>
                    <tr>
                        <td colspan = '2'>
                            <asp:Button ID="btnProcess_Check_ACH_MO" runat="server" Text="Process" 
                                onclientclick="this.disabled = true;__doPostBack('btnProcess_Check_ACH_MO','')" /><asp:Button runat="server" id = "btnScheduleCheck" Text="Schedule Payment" visible = "False"></asp:Button>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwCash_Equity_ExitEquity" runat="server">
                <table class="style1">
                    <tr>
                        <td>
                            Amount:</td>
                        <td>
                            <asp:TextBox ID="txtAmount_Cash_Equity_ExitEquity" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <%If Request("Schedule") = "True" Then %>
                    <tr>
                        <td>Scheduled Date:</td>
                        <td>
                            <uc2:DateField ID="dteScheduledCash" runat="server" />
                        </td>
                    </tr>
                    <%end if %>
                    <tr>
                        <td colspan = '2'> 
                            <asp:Button ID="btnProcess_Cash_Equity_ExitEquity" runat="server" 
                                Text="Process" 
                                onclientclick="this.disabled = true;__doPostBack('btnProcess_Cash_Equity_ExitEquity','')" /><asp:Button runat="server" id = "btnScheduleCash" Text="Schedule Payment" visible = "False"></asp:Button>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwConcord" runat="server">
                <table class="style1">
                    <tr>
                        <td>
                            Amount:</td>
                        <td>
                            <asp:TextBox ID="txtAmount_Concord" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Reference (Not Required):</td>
                        <td><asp:TextBox ID="txtReference_Concord" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Transaction Date:</td>
                        <td>
                            <uc2:DateField ID="dtTransDate_Concord" runat="server" />
                        </td>
                    </tr>
                    <%If Request("Schedule") = "True" Then %>
                    <tr>
                        <td>Scheduled Date:</td>
                        <td>
                            <uc2:DateField ID="dteScheduledConcord" runat="server" />
                        </td>
                    </tr>
                    <%end if %>
                    <tr>
                        <td>
                            <asp:Button ID="btnProcess_Concord" runat="server" Text="Process" 
                                onclientclick="this.disabled = true;__doPostBack('btnProcess_Concord','')" /><asp:Button runat="server" id = "btnScheduleConcord" Text="Schedule Payment" visible = "False"></asp:Button>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwChargeBackEmp" runat="server">
                <table class="style1">
                    <tr>
                        <td>
                            Swipe:</td>
                        <td>
                            <asp:TextBox ID="txtSwipe_ChargeBackEmployee" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Amount:</td>
                        <td>
                            <asp:TextBox ID="txtAmount_ChargeBackEmployee" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Description:</td>
                        <td>
                            <asp:TextBox ID="txtDescription_ChargeBack_Employee" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnProcess_ChargeBackEmployee" runat="server" Text="Process" 
                                onclientclick="this.disabled = true;__doPostBack('btnProcess_ChargeBackEmployee','')" />
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwAdjustment" runat="server">
                <table class="style1">
                    <tr>
                        <td>Item Type:</td>
                        <td>
                            <asp:DropDownList ID="ddItemType" runat="server" AutoPostBack="true" onSelectedIndexChanged = "ddItemType_SelectedIndexChanged">
                                <asp:ListItem Value = "Invoice">Invoice</asp:ListItem>
                                <asp:ListItem Value="Payment">Payment</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Adjustment Code:</td>
                        <td><asp:DropDownList runat="server" id = "ddAdjustments" 
                                onSelectedIndexChanged = "ddAdjustments_SelectedIndexChanged" 
                                style="height: 22px" AutoPostBack = "true"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            Adjustment Type:</td>
                        <td>
                            <asp:RadioButton ID="rbPos" GroupName="AdjSign" runat="server" Text = "Positive"/>
                            <asp:RadioButton Checked="true" ID="rbNeg" GroupName="AdjSign" runat="server" Text = "Negative" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Amount:</td>
                        <td>
                            <asp:TextBox ID="txtAmount_Adjustment" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Description:</td>
                        <td>
                            <asp:TextBox ID="txtDescription_Adjustment" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnProcess_Adjustment" runat="server" Text="Process" 
                                onclientclick="this.disabled = true;__doPostBack('btnProcess_Adjustment','')" />
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwOnline" runat="server">
            </asp:View>
            <asp:View ID="vwCardOnFile" runat="server">
                <asp:GridView runat="server" autogenerateselectbutton="true" id = "gvCardOnFile" emptydatatext = "No Cards On File" onRowDataBound = "gvCardOnFile_RowDataBound"></asp:GridView>
                <asp:Button runat="server" Text="Cancel" onclick="Unnamed2_Click"></asp:Button>
            </asp:View>
        </asp:MultiView>
        <asp:Label ID="lblErr" runat="server" Text="" ForeColor="Red"></asp:Label>
        <asp:GridView ID="gvInvoices" runat="server" AutoGenerateColumns="false" onRowDataBound = "gvInvoices_RowDataBound" CssClass="Invoices">
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
        <asp:GridView runat="server" id = "gvPayments" visible = "false">
        <AlternatingRowStyle BackColor="#C7E3D7" />
            <Columns>
                <asp:templatefield>
                    <ItemTemplate>
                        <asp:CheckBox ID="cb" runat="server"  />
                    </ItemTemplate>
                </asp:templatefield>
            </Columns>
        </asp:GridView>
        <asp:HiddenField ID="hfTickCounter" Value="0" runat="server" />
        
    </div>
    </asp:View>
    <asp:View ID="View2" runat="server">
    <div id = "processing" >
                 <asp:Label ID="lblWaiting" runat="server" Text="Processing... Please Wait"></asp:Label>
                <asp:Timer ID="tmrCheck" runat="server" Enabled="False" Interval="1000">
                </asp:Timer>       
    </div>
    
    </asp:View>
    </asp:MultiView>
    
    <asp:HiddenField ID="hfReceiptURL" Value="0" runat="server" />
    <asp:HiddenField ID="hfCCID" Value="0" runat="server" />
    <asp:HiddenField ID="hfpayments" Value="0" runat="server" />
    <asp:HiddenField ID="hfTokenValue" Value ="0" runat ="server" />
    <asp:HiddenField ID="hfAFC_forbidden" Value="0" runat="server" />
    <asp:HiddenField ID="hfCardType" Value ="" runat="server" />
    </ContentTemplate>
    </asp:UpdatePanel>
    
</form>
</body>

<script type="text/javascript">
    function beforeAsyncPostBack() 
    {
        //var curtime = new Date();
       
        //alert('Time before PostBack:   ' + curtime);
        if (document.getElementById('ddPayMethod')){
            if (document.getElementById('ddPayMethod').options[document.getElementById('ddPayMethod').selectedIndex].value == 'Credit Card') {
                if (document.getElementById('btnProcess_CC')) { document.getElementById('btnProcess_CC').disabled = true; document.getElementById('btnTokenize').disabled = true;}
            }
        }
    }

    function afterAsyncPostBack() {
        //var curtime = new Date();
        //var sched = '<%=request("Schedule") %>';
        if (document.getElementById('ddPayMethod')) {
            if (document.getElementById('ddPayMethod').options[document.getElementById('ddPayMethod').selectedIndex].value == 'Credit Card') {
                if (!document.getElementById('lblWaiting')) {
                    if (document.getElementById('hfReceiptURL').value != '0' && document.getElementById('hfReceiptURL').value != 0 && document.getElementById('hfReceiptURL').value != '-1' && document.getElementById('hfReceiptURL').value != -1) {
                        window.opener.__doPostBack('<%=request("linkid")%>', '');
                        window.location.href = document.getElementById('hfReceiptURL').value;
                    }
                    else if (document.getElementById('hfReceiptURL').value == '-1' || document.getElementById('hfReceiptURL').value == -1) {
                        window.opener.__doPostBack('<%=request("linkid")%>', '');
                        if (document.getElementById('btnProcess_CC')) { document.getElementById('btnProcess_CC').disabled = true; document.getElementById('btnTokenize').disabled = true; }
                    }
                    else {
                        if (document.getElementById('txtAuthorization_CC')) {
                            if (document.getElementById('lblErr').innerHTML == '' && document.getElementById('txtAuthorization_CC').value != '') { alert("Declined") };
                        };
                        if (document.getElementById('btnProcess_CC')) { document.getElementById('btnProcess_CC').disabled = false; document.getElementById('btnTokenize').disabled = false; }
                    }
                }
            }
            else {
                if (document.getElementById("hfReceiptURL").value != '0' && document.getElementById('hfReceiptURL').value != 0 && document.getElementById('hfReceiptURL').value != "-1" && document.getElementById('hfReceiptURL').value != -1) {
                    window.opener.__doPostBack('<%=request("linkid")%>', ''); //window.opener.document.getElementById('').text = "Update";
                    window.location.href = document.getElementById('hfReceiptURL').value;
                }
                else {
                    if (document.getElementById('hfReceiptURL').value == "-1" || document.getElementById('hfReceiptURL').value == -1) {
                        window.opener.__doPostBack('<%=request("linkid")%>', '');
                        window.close();
                    }
                }
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

</html>
