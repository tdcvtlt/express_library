<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Address Labels.aspx.vb" Inherits="Reports_Exit_Address_Labels" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            height: 23px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Contract Number:</td>
            <td><asp:TextBox ID="txtKCP" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2"style="text-align:right;">
                <asp:Button ID="btnAdd" runat="server" Text="Add Contract" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="style1" >Contracts for Labels:</td>
        </tr>
        <tr>
            <td colspan="2">
                
                <asp:ListBox ID="lstKCP" runat="server"></asp:ListBox>
                
                <asp:Button ID="btnRemove" runat="server" Text="Remove Selected" />
                
            </td>
        </tr>
        
        <tr>
            <td colspan="2">
    <asp:Button ID="btnRunReport" runat="server" Text="Run Report" />        
            </td>
        </tr>
    </table>
    
    <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
</asp:Content>

