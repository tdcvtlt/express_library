﻿<%@ Page Title="Boones Z Report" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="BoonesZReport.aspx.vb" Inherits="Reports_Accounting_BoonesZReport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Batch:</td>
            <td>
                <asp:DropDownList ID="ddBatch" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan = '2'>
                <asp:Button ID="Button1" runat="server" Text="Button" />
            </td>
        </tr>
    </table>

    <asp:Label ID="lblReport" runat="server"></asp:Label>

</asp:Content>

