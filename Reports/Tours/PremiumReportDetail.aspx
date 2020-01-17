<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PremiumReportDetail.aspx.vb" Inherits="Reports_Tours_Default3" %>
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
                <uc1:DateField ID="sd" runat="server" />
            </td>
            <td rowspan="5" valign="top">
                
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="ed" runat="server" />
            </td>
            <td />
        </tr>
        <tr>
            <td>Tour Location</td>
            <td><uc2:Select_Item ID="siTourLocation" runat="server" /> </td>
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

     <strong>Personnel Filter (optional)</strong>
    <div style="height:100px;">                   
        <asp:CheckBoxList runat="server" ID="cblPersonnel" RepeatDirection="Horizontal" RepeatLayout="Flow" ></asp:CheckBoxList>
    </div>

        

    <div>
    <br />
        <asp:Button ID="btnRun" runat="server" Text="Run Report" />
    </div>
        
    <br /><br />

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" />
</div>
</asp:Content>

