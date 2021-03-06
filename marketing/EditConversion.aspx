﻿<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditConversion.aspx.vb" Inherits="marketing_EditConversion" %>
<%@ Register src="~/controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register src="~/controls/DateField.ascx" tagname="DateField" tagprefix="uc2" %>
<%@ Register src="~/controls/UserFields.ascx" tagname="UserFields" tagprefix="uc3" %>
<%@ Register src="~/controls/Financials.ascx" tagname="Financials" tagprefix="uc4" %>
<%@ Register src="~/controls/Notes.ascx" tagname="Notes" tagprefix="uc5" %>
<%@ Register src="../controls/EquiantLoanInformation.ascx" tagname="EquiantLoanInformation" tagprefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 800px;
            margin-right: 0px;
        }
        .style2
        {
            text-align: right;
            width: 259px;
        }
        .style5
        {
            width: 155px;
        }
        .style6
        {
            text-align: right;
            width: 229px;
        }
        .style7
        {
            width: 158px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:LinkButton ID="lbProspect" runat="server">LinkButton</asp:LinkButton>
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Conversion_Link" runat="server">Conversion</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Purchase_Link" runat="server">Purchase</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Financing_Link" runat="server">Financing</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Amortization_Link" runat="server">Amortization</asp:LinkButton></li>
    </ul>
    
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 4 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 5 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Notes_Link" runat="server">Notes</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 6 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Payments_Link" runat="server">Payments</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 7 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UserFields_Link" runat="server">User Fields</asp:LinkButton></li>
    </ul>

    <asp:MultiView runat="server" id = "MultiView1">
        <asp:View runat="server" id = "Conversion_View">
        <asp:TextBox ID="txtProspectID" runat="server" Visible = "False"></asp:TextBox>
            <table>
                <tr>
                    <td>ConversionID:</td>
                    <td><asp:TextBox ID="txtConversionID" runat="server"></asp:TextBox></td>
                    <td>Number:</td>
                    <td><asp:TextBox ID="txtNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td valign="top">Status:</td>
                    <td valign="top"><uc1:Select_Item ID="siStatus" runat="server" /></td>
                    <td valign="top">Status Date:</td>
                    <td><uc2:DateField ID="dfStatus" runat="server" /></td>
                </tr>
                <tr>
                    <td>Type:</td>
                    <td><uc1:Select_Item ID="siTitleType" runat="server" /></td>
                    <td><asp:Label runat="server" Text="Club First Occupancy"></asp:Label></td>
                    <td><asp:DropDownList runat="server" ID="ddlYear"></asp:DropDownList></td>
                </tr>
            </table>
            <uc6:EquiantLoanInformation ID="EquiantLoanInformation1" runat="server" />
            <asp:Button ID="btnSave" runat="server" Text="Save" />
            
        </asp:View>
        <asp:View runat="server" id = "Purchase_View">
            <table class="style1">
                    <tr>
                        <td class="style3">
                            Sales Volume:</td>
                        <td class="style4">
                            <asp:TextBox ID="txtSalesVolume" runat="server" CssClass="text_box_currency"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Commission Volume:</td>
                        <td class="style4">
                            <asp:TextBox ID="txtCommissionVolume" runat="server" 
                                CssClass="text_box_currency"></asp:TextBox>
                        </td>
                        <td>
                            <!--<asp:LinkButton ID="lbCC_BreakOut" runat="server">C.C. Breakout &gt;&gt;</asp:LinkButton>-->
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Sales Price:</td>
                        <td class="style4">
                            <asp:TextBox ID="txtSalesPrice" runat="server" CssClass="text_box_currency" AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbFinanceCC" runat="server" Text="Finance Closing Costs" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Original Purchase Price:</td>
                        <td class="style4">
                            <asp:TextBox ID="txtOrigPurchasePrice" runat="server" 
                                CssClass="text_box_currency" AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td>
                            <!--<asp:LinkButton ID="lbUpdate_Costs" runat="server">Update Costs</asp:LinkButton>-->
                        </td>
                    </tr>
                </table>
                <uc4:Financials ID="DP_Financials" runat="server" />
        </asp:View>
        <asp:View runat="server" id = "Financing_View">
            <table class="style1" width="800">
                <tr>
                    <td class="style5">Origination Date:</td>
                    <td class="style6"><uc2:DateField ID="dfOrigDate" runat="server" />
                    </td><td class="style7">Next Payment Due Date:</td>
                    <td class="style2"><uc2:DateField ID="dfNextPaymentDate" runat="server" /></td>
                </tr>
                <tr>
                    <td class="style5">First Payment Due:</td>
                    <td class="style6"><uc2:DateField ID="dfFirstPaymentDate" runat="server" /></td>
                    <td class="style7">Grace:</td>
                    <td class="style2"><uc1:Select_Item ID="siGrace" runat="server" /></td>
                </tr>
                <tr>
                    <td class="style5">Balance Due:</td>
                    <td class="style6"><asp:TextBox ID="txtBalanceDue" runat="server"></asp:TextBox></td>
                    <td class="style7">Loan Payment:</td>
                    <td class="style2"><asp:TextBox ID="txtLoanPayment" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style5">Payment Frequency:</td>
                    <td class="style6"><uc1:Select_Item ID="siFrequency" runat="server" /></td>
                    <td class="style7">Payment Processing Fee:</td>
                    <td class="style2"><asp:TextBox ID="txtPaymentProcessingFee" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style5">Interest Type:</td>
                    <td class="style6"><uc1:Select_Item ID="siInterestType" runat="server" /></td>
                    <td class="style7">Actual Amount:</td>
                    <td class="style2"><asp:TextBox ID="txtActualAmount" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style5">Payment Type:</td>
                    <td class="style6"><uc1:Select_Item ID="siPaymentType" runat="server" /></td>
                    <td class="style7">Total Financed:</td>
                    <td class="style2"><asp:TextBox ID="txtTotalFinanced" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style5">APR:</td>
                    <td class="style6"><asp:TextBox ID="txtAPR" runat="server"></asp:TextBox></td>
                    <td class="style7">Total Finance Charges:</td>
                    <td class="style2"><asp:TextBox ID="txtFinanceCharges" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style5">Terms (months):</td>
                    <td class="style6"><asp:TextBox ID="txtTerms" runat="server"></asp:TextBox></td>
                    <td class="style7">Total Payments:</td>
                    <td class="style2"><asp:TextBox ID="txtTotalPayments" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style5">Payment Processing Fee:</td>
                    <td class="style6"><asp:TextBox ID="txtPayProcFee" runat="server"></asp:TextBox></td>
                    <td class="style7">Total Fees:</td>
                    <td class="style2"><asp:TextBox ID="txtTotalFees" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style5">One-Time Fee:</td>
                    <td class="style6"><asp:TextBox ID="txtOneTimeFee" runat="server"></asp:TextBox></td>
                    <td class="style7">Paid Today:</td>
                    <td class="style2"><asp:TextBox ID="txtPaidToday" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style5">Total Financed:</td>
                    <td class="style6"><asp:TextBox ID="txtTotalFin" runat="server"></asp:TextBox></td>
                    <td class="style7">Total Cost:</td>
                    <td class="style2"><asp:TextBox ID="txtTotalCost" runat="server"></asp:TextBox></td>
                </tr>
            </table>
        </asp:View>
        <asp:View runat="server" id = "Amortization_View">
            N/A
        </asp:View>
        <asp:View runat="server" id = "Events_View">
            <asp:GridView ID="gvEvents" runat="server" EmptyDataText = "No Records">
            </asp:GridView>
        </asp:View>
        <asp:View runat="server" id = "Notes_View">
            <uc5:Notes ID="Notes1" runat="server" KeyField="ConversionID" />
        </asp:View>
        <asp:View runat="server" id = "Payments_View">
            Not Yet Implemented
        </asp:View>
        <asp:View runat="server" id = "UF_View">
            <uc3:UserFields ID="UF" runat="server" />
        </asp:View>
    </asp:MultiView>
</asp:Content>

