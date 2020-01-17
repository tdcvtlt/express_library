<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="IMS-DYDInvoice.aspx.vb" Inherits="Reports_CustomerService_IMS_DYDInvoice" aspcompat = "true"%>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dteSDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dteEDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:Button runat="server" Text="Button" onclick="Unnamed1_Click"></asp:Button></td>
            <td><asp:Button ID="Button3" runat="server" Text="To Excel" ></asp:Button></td>
        </tr>
    </table>
    <asp:Literal runat="server" id = "litReport"></asp:Literal>
</asp:Content>

