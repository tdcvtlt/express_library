<%@ Page Language="VB" AutoEventWireup="false" CodeFile="confLetter.aspx.vb" Inherits="marketing_confLetter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <OBJECT ID="ScriptControl1" WIDTH=39 HEIGHT=39
		 CLASSID="CLSID:0E59F1D5-1FBE-11D0-8FF2-00A0D10038BC">
		    <PARAM NAME="_ExtentX" VALUE="1005">
		    <PARAM NAME="_ExtentY" VALUE="1005">	
	</OBJECT>
    <form id="form1" runat="server">
    <div>
        <asp:radiobuttonlist id = "rbLetters" runat="server" RepeatColumns="2" 
            RepeatDirection="Horizontal">

            <%-- %><asp:ListItem selected="True" Value="\\kcp.local\resort shares\T drive\rLetters\NTR Off-Season.doc">RRO-NLES-OffSeason1NTR Off-Season</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\RRO-NLES-OffSeason1.doc">NTR Off-SeasonRRO-NLES-OffSeason1</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\RRO Off-Season.doc">RRO Off-Season</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\RROSummer-Holiday.doc">RROSummer-Holiday</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\TR Summer-Holiday.doc">TR Summer-Holiday</asp:ListItem>--%>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\DTD Conf-Letter.doc">DTD Conf-Letter</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\MAL Conf-Letter.doc">MAL Conf-Letter</asp:ListItem>
            <%-- <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\NTR Summer-Holiday.doc">NTR Summer-Holiday</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\RRO-NLES-SummerHoliday.doc">RRO-NLES-SummerHoliday</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\TR OffSeason.doc">TR OffSeason</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\DYD Conf-Letter.doc">DYD Conf-Letter</asp:ListItem>--%>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\TAXES DUE TR Summer-Holiday.doc">TR Summer-Taxes</asp:ListItem>
            <%-- <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\TAXES DUE TR OffSeason.doc">TR Off-Season-Taxes</asp:ListItem> 
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\TAXES DUE NTR Off-Season.doc">NTR Off-Season-Taxes</asp:ListItem>--%>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\TAXES DUE NTR Summer-Holiday.doc">NTR Summer-Taxes</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\TAXES DUE RRO-NLES-SummerHoliday.doc">NLES Summer-Taxes</asp:ListItem>
            <%-- <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\TAXES DUE RRO-NLES-OffSeason.doc">NLES Off-Season-Taxes</asp:ListItem>--%>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\TAXES DUE RROSummer-Holiday.doc">RRO Summer-Taxes</asp:ListItem>
            <%-- <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\TAXES DUE RRO Off-Season">RRO Off-Season-Taxes</asp:ListItem> --%>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Westgate Confirmation Letter - Branson, MO.doc">Westgate Confirmation Letter - Branson</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Westgate Confirmation Letter - Gatlinburg, TN.doc">Westgate Confirmation Letter - Gatlinburg</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Westgate Confirmation Letter - Las Vegas, NV.doc">Westgate Confirmation Letter - Las Vegas</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Westgate Confirmation Letter - Miami, FL.doc">Westgate Confirmation Letter - Miami</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Westgate Confirmation Letter - Myrtle Beach, SC.doc">Westgate Confirmation Letter - Myrtle Beach</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Westgate Confirmation Letter - Orlando, FL - Westgate Lakes.doc">Westgate Confirmation Letter - Orlando Westgate Lakes</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Westgate Confirmation Letter - Orlando, FL - Vac Villas.doc">Westgate Confirmation Letter - Orlando Vac. Villas</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Westgate Confirmation Letter - Kissimmee FL- Town Center.doc">Westgate Confirmation Letter - Kissimmee FL- Town Center</asp:ListItem>
            <%--<asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\VRC Conf-Letter-Holiday Inn  Suites.doc">VRC Conf-Letter-Holiday Inn Suites</asp:ListItem>
             <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\VRC Conf-Letter-Homewood Suites.doc">VRC Conf-Letter-Homewood Suites</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\VRC Conf-Letter-Residence Inn.doc">VRC Conf-Letter-Residence Inn</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\VRC Conf-Letter-Springhill Suites.doc">VRC Conf-Letter-Springhill Suites</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\VRC Conf-Letter-Days Hotel.docx">VRC Conf-Letter-Days Hotel</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\VRC Conf-Letter-The Clarion.docx">VRC Conf-Letter-The Clarion</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Festiva Confirmation Letter - Atlantic Beach NC.doc">Festiva Confirmation Letter - Atlantic Beach NC</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Festiva Confirmation Letter - Charleston SC.doc">Festiva Confirmation Letter - Charleston SC</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Festiva Confirmation Letter - Murrels Inlet.doc">Festiva Confirmation Letter - Murrels Inlet</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Festiva Confirmation Letter - Branson MO.doc">Festiva Confirmation Letter - Branson MO</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Grande Crowne Confirmation Letter - Branson MO.doc">Grande Crowne Confirmation Letter - Branson MO</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Grande Crowne Confirmation Letter - Smoky Mountains.doc">Grande Crowne Confirmation Letter - Smoky Mountains</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Grande Crowne Confirmation Letter-Biloxi, MS.docx">Grande Crowne Confirmation Letter - Biloxi, MS</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rletters\Grande Crowne Confirmation Letter-Myrtle Beach, SC.docx">Grande Crowne Confirmation Letter-Myrtle Beach, SC</asp:ListItem>--%>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\CLC Confirmation Letter. Regal Oaks Resort.docx">CLC Confirmation Letter, Regal Oaks Resort</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\CLC Confirmation Letter, Encantada.docx">CLC Confirmation Letter, Encantada</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rLetters\Czar KCP Confirmation Letter.docx">Czar KCP Confirmation Letter</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rletters\VRC Conf-Letter-Days Hotel.docx">VRC Conf-Letter-Days Hotel</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rletters\VRC Conf-Letter-The Clarion.docx">VRC Conf-Letter-The Clarion</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rletters\Westgate Confirmation Letter - Saxe LV.doc">WG Confirmation Letter - Saxe LV</asp:ListItem>
            <%--<asp:ListItem Value="\\kcp.local\resort shares\T drive\rletters\Taxes Due Mal Conf-Letter.doc">Taxes Due MAL Conf-Letter</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rletters\Capital Resort Group Confirmation Letter - Kennebunk, ME.doc">CRG - Kennebunk, ME</asp:ListItem>--%>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rletters\Spinnaker Confirmation Letter - Hilton Head SC.doc">Spinnaker Hilton Head</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rletters\Spinnaker Confirmation Letter - Ormond Beach SC.doc">Spinnaker Ormond Beach</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rletters\SummerBay Confirmation Letter - Orlando FL.doc">SummerBay Confirmation Letter - Orlando FL</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rletters\Tropicana Confirmation Letter.doc">Tropicana Confirmation Letter</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rletters\LVH Confirmation Letter.doc">LVH Confirmation Letter</asp:ListItem>
            <asp:ListItem Value="\\kcp.local\resort shares\T drive\rletters\VRC Conf-Letter-Best Western Hotel.docx">VRC Conf-Letter-Best Western Hotel</asp:ListItem>

                        
            <%--<asp:ListItem selected Value="\\nndc\fs\rLetters\NTR Off-Season.doc">RRO-NLES-OffSeason1NTR Off-Season</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\RRO-NLES-OffSeason1.doc">NTR Off-SeasonRRO-NLES-OffSeason1</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\RRO Off-Season.doc">RRO Off-Season</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\RROSummer-Holiday.doc">RROSummer-Holiday</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\TR Summer-Holiday.doc">TR Summer-Holiday</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\DTD Conf-Letter.doc">DTD Conf-Letter</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\MAL Conf-Letter.doc">MAL Conf-Letter</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\NTR Summer-Holiday.doc">NTR Summer-Holiday</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\RRO-NLES-SummerHoliday.doc">RRO-NLES-SummerHoliday</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\TR OffSeason.doc">TR OffSeason</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\DYD Conf-Letter.doc">DYD Conf-Letter</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\TAXES DUE TR Summer-Holiday">TR Summer-Taxes</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\TAXES DUE TR OffSeason.doc">TR Off-Season-Taxes</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\TAXES DUE NTR Off-Season.doc">NTR Off-Season-Taxes</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\TAXES DUE NTR Summer-Holiday.doc">NTR Summer-Taxes</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\TAXES DUE RRO-NLES-SummerHoliday.doc">NLES Summer-Taxes</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\TAXES DUE RRO-NLES-OffSeason.doc">NLES Off-Season-Taxes</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\TAXES DUE RROSummer-Holiday.doc">RRO Summer-Taxes</asp:ListItem>
            <asp:ListItem Value="\\nndc\fs\rLetters\TAXES DUE RRO Off-Season">RRO Off-Season-Taxes</asp:ListItem>--%>
        </asp:radiobuttonlist>
        <asp:button runat="server" text="Submit" onclick="Unnamed2_Click" ID="Unnamed2" 
            style="height: 26px" />
        
            
    </div>
    </form>
</body>
</html>
