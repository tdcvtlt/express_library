<%@ Page Title="Edit Lead" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditLead.aspx.vb" Inherits="LeadManagement_EditLead" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>LeadID: </td>
            <td><asp:TextBox ID="txtLeadID" runat="server" ReadOnly="True"></asp:TextBox></td>
            <td>Drawing:</td>
            <td>
                <asp:TextBox ID="txtDrawingID" runat="server" ReadOnly="True"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Male/Female: </td>
            <td><asp:TextBox ID="txtMF" runat="server"></asp:TextBox></td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>First Name: </td>
            <td>
                <asp:TextBox ID="txtFName" runat="server"></asp:TextBox></td>
            <td>Last Name:</td>
            <td>
                <asp:TextBox ID="txtLName" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Spouse Name: </td>
            <td colspan = "3">
                <asp:TextBox ID="txtSpouse" runat="server" Width="358px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Address 1: </td>
            <td colspan="3">
                <asp:TextBox ID="txtAddress1" runat="server" Width="358px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Address 2: </td>
            <td colspan = "3">
                <asp:TextBox ID="txtAddress2" runat="server" Width="358px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>City: </td>
            <td>
                <asp:TextBox ID="txtCity" runat="server"></asp:TextBox></td>
            <td>State:</td>
            <td>
                <asp:TextBox ID="txtState" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Postal Code: </td>
            <td>
                <asp:TextBox ID="txtZip" runat="server"></asp:TextBox></td>
            <td>Phone Number:</td>
            <td>
                <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Email: </td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
            <td>Age:</td>
            <td>
                <asp:TextBox ID="txtAge" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Marital Status: </td>
            <td><asp:TextBox ID="txtMS" runat="server"></asp:TextBox></td>
            <td>Income Range:</td>
            <td>
                <asp:TextBox ID="txtIncome" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Own/Rent: </td>
            <td>
                <asp:TextBox ID="txtOwn" runat="server"></asp:TextBox></td>
            <td>Signed:</td>
            <td>
                <asp:CheckBox ID="ckSigned" runat="server" />
            </td>
        </tr>
        <tr>
            <td>File ID:</td>
            <td>
                <asp:TextBox ID="txtFileID" runat="server" ReadOnly="True"></asp:TextBox></td>
            <td>Signed Date:</td>
            <td>
                <uc2:DateField ID="dteSigned" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Source:</td>
            <td><asp:TextBox ID="txtSource" runat="server"></asp:TextBox></td>
            <td>Date Entered:</td>
            <td><asp:TextBox ID="txtDateEntered" ReadOnly runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Vendor:</td>
            <td><asp:DropDownList ID="ddVendors" runat="server"></asp:DropDownList></td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
    <ul id="menu">
        <li><asp:LinkButton ID="lbSave" runat="server">Save</asp:LinkButton></li>
        <li><asp:LinkButton ID="lbCancel" runat="server">Cancel</asp:LinkButton></li>
    </ul>
</asp:Content>

