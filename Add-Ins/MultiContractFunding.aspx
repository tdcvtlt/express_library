﻿<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="MultiContractFunding.aspx.vb" Inherits="Add_Ins_MultiContractFunding" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Funding:</td>
            <td><asp:TextBox runat="server" id = "txtFunding"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Recording Date:</td>
            <td>
                <uc1:DateField ID="dteRecordingDate" runat="server" />
            </td>
        </tr>
        <tr><td></td></tr>
        <tr>
            <td>KCP #:</td>
            <td><asp:TextBox runat="server" id = "txtContract"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Deed#:</td>
            <td><asp:TextBox runat="server" id = "txtDeed"></asp:TextBox></td>
        </tr>
        <tr>
            <td>DOT#:</td>
            <td><asp:TextBox runat="server" id = "txtDOT"></asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Button runat="server" Text="Add" onclick="Unnamed1_Click"></asp:Button></td>
        </tr>
        <tr>
            <td><asp:Button runat="server" Text="Submit" onclick="Unnamed2_Click"></asp:Button></td>
        </tr>
    </table>
    <asp:GridView runat="server" id = "gvFunding" autoGenerateDeleteButton></asp:GridView>
</asp:Content>
