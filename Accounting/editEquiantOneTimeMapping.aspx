<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="editEquiantOneTimeMapping.aspx.vb" Inherits="Accounting_editEquiantMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <table>
            <tr>
                <td>ID:</td>
                <td><asp:TextBox runat="server" ID ="txtID" ReadOnly="true"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Code:</td>
                <td><asp:TextBox runat="server" ID="txtCode" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Source Code:</td>
                <td><asp:TextBox runat="server" ID="txtSourceCode" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Category:</td>
                <td><asp:TextBox runat="server" ID="txtCategory" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Postive / Negative Value:</td>
                <td><asp:TextBox runat="server" ID="txtPosNeg" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr style="border-top:5px solid black;">
                <td>Action:</td>
                <td><asp:DropDownList runat="server" ID ="ddAction" AutoPostBack="True"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Transaction Type:</td>
                <td><asp:DropDownList runat="server" ID="ddKeyfield" AutoPostBack="True"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Invoice Type:</td>
                <td><asp:DropDownList runat="server" ID="ddInvoiceType" AutoPostBack="True"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Payment Method:</td>
                <td><asp:DropDownList runat="server" ID ="ddPaymentMethod"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Create Invoice?</td>
                <td><asp:CheckBox runat="server" ID="ckCreateInvoice" /></td>
            </tr>
            <tr>
                <td>Invoice:</td>
                <td><asp:DropDownList runat="server" ID="ddInvoice"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnSave" text="Save"/>
                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" />
                </td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hfTransID" Value="0" />
        <asp:Label runat="server" ID="lblError" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>