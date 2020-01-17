<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditGolfCourse.aspx.vb" Inherits="setup_EditGolfCourse" %>

<%@ Register Src="~/controls/Select_Item.ascx" TagPrefix="uc1" TagName="Select_Item" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>ID:</td>
            <td><asp:TextBox ID="txtID" runat="server" ReadOnly></asp:TextBox></td>
            <td>Name:</td>
            <td><asp:TextBox ID="txtCourse" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Address:</td>
            <td>
                <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox></td>
            <td>City:</td>
            <td>
                <asp:TextBox ID="txtCity" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>State:</td>
            <td>
                <uc1:Select_Item runat="server" ID="siState" />
            </td>
            <td>Postal Code:</td>
            <td>
                <asp:TextBox ID="txtPostalCode" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Phone Number:</td>
            <td>
                <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Active:</td>
            <td>
                <asp:CheckBox ID="cbActive" runat="server" /></td>
            <td>Cost:</td>
            <td>
                <asp:TextBox ID="txtCost" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>No Show Invoice:</td>
            <td>
                <asp:DropDownList ID="ddInvoices" runat="server"></asp:DropDownList></td>
            <td>Invoice Amount:</td>
            <td>
                <asp:TextBox ID="txtInvAmount" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td><asp:Button ID="btnSave" runat="server" Text="Save" />
            </td>
        </tr>
    </table>
</asp:Content>

