<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="closingofficer.aspx.vb" Inherits="Reports_Contracts_closingofficer" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

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
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="edate" runat="server" />
            </td>
        </tr>
        <tr><td colspan="3">Closing Officers:</td></tr>
        <tr>
            <td>
                <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
            </td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="<<" /><br />
                <asp:Button ID="Button2" runat="server" Text=">>" />
            </td>
            <td>
                <asp:ListBox ID="ListBox2" runat="server"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">Titles:</td>
        </tr>
        <tr>
            <td>
                <asp:ListBox ID="ListBox3" runat="server"></asp:ListBox>
            </td>
            <td>
                <asp:Button ID="Button3" runat="server" Text="<<" /><br />
                <asp:Button ID="Button4" runat="server" Text=">>" />
            </td>
            <td>
                <asp:ListBox ID="ListBox4" runat="server"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:Button ID="btnRun" runat="server" Text="Run Report" /></td>
        </tr>
    </table>

        <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />





    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />



        



</div>
</asp:Content>

