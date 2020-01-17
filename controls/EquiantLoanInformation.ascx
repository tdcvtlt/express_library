<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EquiantLoanInformation.ascx.vb" Inherits="controls_EquiantLoanInformation" %>

<style type="text/css">
    .auto-style1 {
        height: 26px;
    }
</style>

<table style="width:100%;max-width:890px;">
    <tr style="vertical-align:top">
        <td>
            <table >
                <thead>
                    <tr>
                        <th colspan="2" style="text-align: left">Balance</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td >
                            Principal Balance:
                        </td>
                        <td style="text-align:right" >
                            <asp:Label ID="lblPB" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Accrued Interest:
                        </td>
                        <td style="text-align:right" >
                            <asp:Label ID="lblAI" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Late Charge Balance:
                        </td>
                        <td style="text-align:right" >
                            <asp:Label ID="lblLCB" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Other Balance:
                        </td>
                        <td style="text-align:right" >
                            <asp:Label ID="lblOB" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Impound Due:
                        </td>
                        <td style="text-align:right" >
                            <asp:Label ID="lblID" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Payoff as of <%=Date.Today.ToShortDateString %>:
                        </td>
                        <td style="text-align:right;border-top:thin solid black;" >
                            <asp:Label ID="lblPO1" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Payoff as of <%=Date.Today.AddDays(10).ToShortDateString %>:
                        </td>
                        <td style="text-align:right" >
                            <asp:Label ID="lblPO2" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Equity as of <%=Date.Today.ToShortDateString %>:</td>
                        <td style="text-align:right"><asp:Label ID="lblEq" runat="server" Text="$0.00"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
        </td>
        <td style="width:25px;"></td>
        <td>
            <table>
                <thead>
                    <tr>
                        <th colspan="2"  style="text-align: left">Payment</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            Bring Current:
                        </td>
                        <td style="text-align:right">
                            <asp:Label ID="lblBC" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Next Payment Due:
                        </td>
                        <td style="text-align:right">
                            <asp:Label ID="lblNPD" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            Regular Payment Amount:
                        </td>
                        <td style="text-align:right" class="auto-style1">
                            <asp:Label ID="lblRPA" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Impound Amount:
                        </td>
                        <td style="text-align:right">
                            <asp:Label ID="lblIA" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Total Payment:
                        </td>
                        <td style="text-align:right;border-top:thin solid black;">
                            <asp:Label ID="lblTP" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Partial Payment:
                        </td>
                        <td style="text-align:right">
                            <asp:Label ID="lblPP" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Payments Made / Remaining Term:
                        </td>
                        <td style="text-align:right">
                            <asp:Label ID="lblPMRT" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            NSF Count:
                        </td>
                        <td style="text-align:right">
                            <asp:Label ID="lblNSF" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Last Payment Date:
                        </td>
                        <td style="text-align:right">
                            <asp:Label ID="lblLPD" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Last Payment Amount:
                        </td>
                        <td style="text-align:right">
                            <asp:Label ID="lblLPA" runat="server" Text="$0.00"></asp:Label>
                        </td>
                    </tr>

                </tbody>
            </table>
        </td>
    </tr>
</table>

<ul id="menu">
    <li><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton></li>
</ul>