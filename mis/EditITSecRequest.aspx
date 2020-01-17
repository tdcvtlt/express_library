<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditITSecRequest.aspx.vb" Inherits="mis_EditITSecRequest" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Personnel:</td>
            <td><asp:Label ID="lblPersonnel" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Request Type:</td>
            <td>
                <asp:Label ID="lblRequest" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>Status:</td>
            <td>
                <uc1:Select_Item ID="siStatus" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Requested By:</td>
            <td>
                <asp:Label ID="lblRequestedBy" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Date Created:</td>
            <td>
                <asp:Label ID="lbDateCreated" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Requested Due Date:</td>
            <td><asp:Label ID="lbDueDate" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>CRMS Security Group(s):</td>
            <td>
                <asp:Label ID="lbCRMSGroups" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Email Domain:</td>
            <td>
                <asp:Label ID="lblDomain" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>Distribution Groups:</td>
            <td>
                <asp:Label ID="lblDistGroups" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Windows Security Groups:</td>
            <td>
                <asp:Label ID="lblWindowsGroups" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Phone Setup:</td>
            <td>
                <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>DID Requested:</td>
            <td>
                <asp:Label ID="lblDID" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Save" />
            </td>
        </tr>
    </table>

</asp:Content>

