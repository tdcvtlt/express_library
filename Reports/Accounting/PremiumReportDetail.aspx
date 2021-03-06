﻿<%@ Page Title="Premium Detail Report" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PremiumReportDetail.aspx.vb" Inherits="Reports_Accounting_PremiumReportDetail" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>

    <table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="sdate" runat="server" />
            </td>
            <td rowspan="5" valign="top">
                
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="edate" runat="server" />
            </td>
            <td />
        </tr>        
         <tr>
            <td valign="top"><strong>Tour Location</strong></td>
            <td>
                <asp:CheckBoxList runat="server" ID="cblTourLocation" Height="120px" RepeatDirection="Horizontal" RepeatColumns="5"></asp:CheckBoxList>
            </td>
            <td />
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <br />
            </td>
            <td>
                &nbsp;
            </td>            
        </tr>
        <tr>
            <td colspan="3"></td>
        </tr>
    </table>

    

        <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />

        <div>
        <br />
            <asp:Button ID="btnRun" runat="server" Text="Run Report" />
        </div>
        
        <br /><br />



    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />



        



</div>
</asp:Content>

