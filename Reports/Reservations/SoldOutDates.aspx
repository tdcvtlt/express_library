<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="SoldOutDates.aspx.vb" Inherits="Reports_Reservations_SoldOutDates" %>
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
        <tr>
            <td>Minimum </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlMinAvail">
                    <asp:ListItem>0</asp:ListItem>
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>                    
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnRun" runat="server" Text="Run Report" />&nbsp;<asp:Button ID="btnExcel" runat="server" Text="Excel" /></td>
        </tr>
    </table>


    <br />
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
    <asp:Literal runat="server" ID="lit1"></asp:Literal>
</div>
    


</asp:Content>

