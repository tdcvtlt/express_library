<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="UsageRestrictions.aspx.vb" Inherits="Reports_Reservations_UsageRestrictions" aspcompat = "true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Select Restriction</td>
            <td><asp:DropDownList runat="server" id = "ddRestrictions"></asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:Button runat="server" Text="Run Report" onclick="Unnamed1_Click"></asp:Button></td>
        </tr>
    </table>
    <br />
    <asp:Literal runat="server" id = "litReport"></asp:Literal>


</asp:Content>

