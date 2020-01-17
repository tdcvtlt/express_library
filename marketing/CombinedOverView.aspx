<%@ Page Title="Combined Overview" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CombinedOverview.aspx.vb" Inherits="marketing_CombinedOverview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:MultiView runat="server" id = "MultiView1">
    <asp:View runat="server" id = "View1">
    <asp:UpdatePanel runat="server" id = "UpdatePanel1">
        
        <ContentTemplate>
            <input type="button" value="Printable" onclick="var mwin = window.open();mwin.document.write(document.getElementById('printable').innerHTML);" />
            <div id="printable">
            <table  style="border:solid thin black">
                <tr>
                    <td>Date: <asp:Label runat="server" id = "lblDate"></asp:Label></td>
                    <td></td>
                    <td><b><U>Day</U></b></td>
                    <td><b><U>MTD</U></b></td>
                    <td><b><U>YTD</U></b></td>
                </tr>
                <tr>
                    <td colspan = '5' style = "border-bottom:solid thin black"><b><u>LINE & EXIT (Co. Gen.)</u></b></td>
                </tr>
                <tr>
                    <td>Tours</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtLineToursAct" size = "9" style = "text-align:right" onTextChanged = "txtLineToursAct_TextChanged" autopostback = "True">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineMTDToursAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineYTDToursAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>Projected</td>
                    <td><asp:TextBox runat="server" id = "txtLineToursProj" size = "9" style = "text-align:right" onTextChanged = "txtLineToursProj_TextChanged" autopostback = "True">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineMTDToursProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineYTDToursProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Sales</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtLineSalesAct" size = "9" style = "text-align:right" onTextChanged = "txtLineSalesAct_TextChanged" autopostback = "True">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineMTDSalesAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineYTDSalesAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                    <tr>
                    <td></td>
                    <td>Projected</td>
                    <td><asp:TextBox runat="server" id = "txtLineSalesProj" size = "9" style = "text-align:right" onTextChanged = "txtLineSalesProj_TextChanged" autopostback = "True">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineMTDSalesProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineYTDSalesProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <!--<tr>
                    <td>Intervals</td>
                    <td>Sales</td>
                    <td><asp:TextBox runat="server" id = "txtLineIntervalsSales" size = "9" style = "text-align:right" onTextChanged = "txtLineIntervalsSales_TextChanged" autopostback = "True">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineMTDIntervalsSales" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineYTDIntervalsSales" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>Cancels</td>
                    <td><asp:TextBox runat="server" id = "txtLineIntervalsCan" size = "9" style = "text-align:right" onTextChanged = "txtLineIntervalsCan_TextChanged" autopostback = "True">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineMTDIntervalsCan" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineYTDIntervalsCan" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr-->
                <tr>
                    <td>Volume</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtLineVolumeAct" size = "9" style = "text-align:right" ontextChanged = "txtLineVolumeAct_TextChanged" autopostback = "True">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineMTDVolumeAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineYTDVolumeAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>Projected</td>
                    <td><asp:TextBox runat="server" id = "txtLineVolumeProj" size = "9" style = "text-align:right" ontextChanged = "txtLineVolumeProj_TextChanged" autopostback = "True">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineMTDVolumeProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineYTDVolumeProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Closing %</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtLineClosingAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineMTDClosingAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineYTDClosingAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>Projected</td>
                    <td><asp:TextBox runat="server" id = "txtLineClosingProj" size = "9" style = "text-align:right" readonly>0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineMTDClosingProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineYTDClosingProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Avg Sales Price</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtLineAvgSP" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineMTDAvgSP" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineYTDAvgSP" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>

                <tr>
                    <td>VPG</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtLineVPG" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineMTDVPG" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtLineYTDVPG" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
		            <td></td>
	            </tr>
	            <tr>
		            <td colspan="5" style = "border-bottom:solid thin black"><u><b>IN-HOUSE</b></u></td>
	            </tr>
                    <tr>
                    <td>Tours</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtIHToursAct" size = "9" style = "text-align:right" onTextChanged = "txtIHToursAct_TextChanged" autoPostback = "true">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDToursAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDToursAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>Projected</td>
                    <td><asp:TextBox runat="server" id = "txtIHToursProj" size = "9" style = "text-align:right" onTextChanged = "txtIHToursProj_TextChanged" autoPostback = "true">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDToursProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDToursProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Sales</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtIHSalesAct" size = "9" style = "text-align:right" onTextChanged = "txtIHSalesAct_TextChanged" autoPostback = "true">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDSalesAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDSalesAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                    <tr>
                    <td></td>
                    <td>Projected</td>
                    <td><asp:TextBox runat="server" id = "txtIHSalesProj" size = "9" style = "text-align:right" onTextChanged = "txtIHSalesProj_TextChanged" autoPostback = "true">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDSalesProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDSalesProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <!--<tr>
                    <td>Intervals</td>
                    <td>Sales</td>
                    <td><asp:TextBox runat="server" id = "txtIHIntervalsSales" size = "9" style = "text-align:right" onTextChanged = "txtIHIntervalsSales_TextChanged" autoPostback = "true">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDIntervalsSales" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDIntervalsSales" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>Cancels</td>
                    <td><asp:TextBox runat="server" id = "txtIHIntervalsCan" size = "9" style = "text-align:right" onTextChanged = "txtIHIntervalsCan_TextChanged" autoPostback = "true">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDIntervalsCan" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDIntervalsCan" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>-->
                <tr>
                    <td>Volume</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtIHVolumeAct" size = "9" style = "text-align:right" onTextChanged = "txtIHVolumeAct_TextChanged" autoPostback = "true">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDVolumeAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDVolumeAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>Projected</td>
                    <td><asp:TextBox runat="server" id = "txtIHVolumeProj" size = "9" style = "text-align:right" autoPostback = "true" onTextChanged = "txtIHVolumeProj_TextChanged">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDVolumeProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDVolumeProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Upgrades</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtIHUpgradesAct" size = "9" style = "text-align:right" onTextChanged = "txtIHUpgradesAct_TextChanged" autoPostback = "true">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDUpgradesAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDUpgradesAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>Projected</td>
                    <td><asp:TextBox runat="server" id = "txtIHUpgradesProj" size = "9" style = "text-align:right" autoPostback = "true" onTextChanged = "txtIHUpgradesProj_TextChanged" >0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDUpgradesProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDUpgradesProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                    <tr>
                    <td>Upgrade Volume</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtIHUpgradeVolAct" size = "9" style = "text-align:right" autoPostback = "true" onTextChanged = "txtIHUpgradeVolAct_TextChanged">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDUpgradeVolAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDUpgradeVolAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>Projected</td>
                    <td><asp:TextBox runat="server" id = "txtIHUpgradeVolProj" size = "9" style = "text-align:right" autoPostback = "true" onTextChanged = "txtIHUpgradeVolProj_TextChanged">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDUpgradeVolProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDUpgradeVolProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Avg Sales Price</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtIHAvgSP" size = "9" style = "text-align:right" readonly>0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDAvgSP" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDAvgSP" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                    <tr>
                    <td>Avg Upgrade Volume</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtIHAvgVolume" size = "9" style = "text-align:right" readonly>0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDAvgVolume" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDAvgVolume" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Total Volume</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtIHTotalVolume" size = "9" style = "text-align:right" readonly>0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDTotalVolume" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDTotalVolume" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>VPG</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtIHVPG" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHMTDVPG" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtIHYTDVPG" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
		            <td></td>
	            </tr>
	            <tr>
		            <td colspan="5" style = "border-bottom:solid thin black" ><u><b>GRAND TOTALS</b></u></td>
	            </tr>
                <tr>
                    <td>Tours</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtToursAct" size = "9" style = "text-align:right" readonly>0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtMTDToursAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtYTDToursAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>Projected</td>
                    <td><asp:TextBox runat="server" id = "txtToursProj" size = "9" style = "text-align:right" readonly>0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtMTDToursProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtYTDToursProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Sales</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtSalesAct" size = "9" style = "text-align:right" readonly>0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtMTDSalesAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtYTDSalesAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>Projected</td>
                    <td><asp:TextBox runat="server" id = "txtSalesProj" size = "9" style = "text-align:right" readonly>0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtMTDSalesProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtYTDSalesProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <!--<tr>
                    <td>Intervals</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtIntervalsSales" size = "9" style = "text-align:right" readonly>0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtMTDIntervalsSales" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtYTDIntervalsSales" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>Cancels</td>
                    <td><asp:TextBox runat="server" id = "txtIntervalsCan" size = "9" style = "text-align:right" readonly>0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtMTDIntervalsCan" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtYTDIntervalsCan" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>-->
                <tr>
                    <td>Volume</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtVolumeAct" size = "9" style = "text-align:right" readonly>0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtMTDVolumeAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtYTDVolumeAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>Projected</td>
                    <td><asp:TextBox runat="server" id = "txtVolumeProj" size = "9" style = "text-align:right" readonly>0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtMTDVolumeProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtYTDVolumeProj" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Avg Sales Price</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtAvgSalesPrice" size = "9" style = "text-align:right" readonly>0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtMTDAvgSalesPrice" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtYTDAvgSalesPrice" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>VPG</td>
                    <td>Actual</td>
                    <td><asp:TextBox runat="server" id = "txtVPGAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtMTDVPGAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                    <td><asp:TextBox runat="server" id = "txtYTDVPGAct" size = "9" readonly style = "text-align:right">0</asp:TextBox></td>
                </tr>
            </table>
            </div>
        </ContentTemplate>

    </asp:UpdatePanel>
   <asp:Button runat="server" Text="Save" id = "btnSave"></asp:Button>
   </asp:View>
    <asp:View runat="server" id = "View2">ACCESS DENIED</asp:View>
   </asp:MultiView>
</asp:Content>