<%@ Page Title="Random Drawings" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="RandomDrawing.aspx.vb" Inherits="Reports_Marketing_RandomDrawing" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dfStart" runat="server" />
            </td>
        </tr>
        
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dfEnd" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Vendors:</td>
            <td><div style="overflow:auto;height:150px;"><asp:CheckBoxList ID="cblVendors" runat="server"></asp:CheckBoxList></div></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnList" runat="server" Text="Generate List" />
            </td>
        </tr>
    </table>
    <div class="ListGrid">
    <asp:GridView ID="gvEntries" runat="server">

    </asp:GridView></div>
</asp:Content>

