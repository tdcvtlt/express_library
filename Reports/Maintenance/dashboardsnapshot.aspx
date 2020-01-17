<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="dashboardsnapshot.aspx.vb" Inherits="Reports_Maintenance_pmschedules" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">



<style type="text/css">

</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="margin-left:10px;">
    <table cellpadding="3" cellspacing="3">
        <tr>
            <td>Month:</td>
            <td>
                <asp:DropDownList ID="ddMonth" runat="server">
                    <asp:ListItem Value ="1" Text="Jan" />
                    <asp:ListItem Value ="2" Text="Feb" />
                    <asp:ListItem Value ="3" Text="Mar" />
                    <asp:ListItem Value ="4" Text="Apr" />
                    <asp:ListItem Value ="5" Text="May" />
                    <asp:ListItem Value ="6" Text="Jun" />
                    <asp:ListItem Value ="7" Text="Jul" />
                    <asp:ListItem Value ="8" Text="Aug" />
                    <asp:ListItem Value ="9" Text="Sep" />
                    <asp:ListItem Value ="10" Text="Oct" />
                    <asp:ListItem Value ="11" Text="Nov" />
                    <asp:ListItem Value ="12" Text="Dec" />
                </asp:DropDownList>
            </td>
            <td>Year:</td>
            <td>
                <asp:DropDownList ID="ddYear" runat ="server" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Button runat="server" ID="submitButton" Text="Report" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</div>
</asp:Content>

