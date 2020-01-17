<%@ Page Title="Combine Prospect Records" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CombineProspects.aspx.vb" Inherits="setup_CombineProspects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>ProspectID to keep:</td>
            <td>
                <asp:TextBox ID="txtKeep" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>ProspectID(s) to remove:</td>
            <td valign="top">
                <asp:TextBox ID="txtRemove" runat="server"></asp:TextBox>
                <asp:Button ID="btnAdd"
                    runat="server" Text="Add" /><br />
                <asp:ListBox ID="lstRemove" runat="server"></asp:ListBox>
                <asp:Button ID="btnRemove" runat="server" Text="Remove" />
            </td>
        </tr>
    </table>
    <asp:Button ID="btnCombine" runat="server" Text="Combine" />
    <asp:Button ID="btnReset"
        runat="server" Text="Reset" /> <br />
        <asp:Label ID="Label1" runat="server" 
        Text="This cannot be undone!! Please be sure to copy any demographic information (Spouse, SSN, etc..) from the old records that you want to keep. This merge operation will NOT copy them over!! " 
        Font-Bold="True" ForeColor="Red"></asp:Label>

</asp:Content>

