<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="MultiContractExhibit.aspx.vb" Inherits="Add_Ins_MultiContractExhibit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Exhibit #:</td>
            <td><asp:TextBox runat="server" id = "txtExhibit"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Contract #:</td>
            <td><asp:TextBox runat="server" id = "txtContract"></asp:TextBox></td>
            <td><asp:Button runat="server" Text="Add" onclick="Unnamed1_Click"></asp:Button></td>
        </tr>
        <tr>
            <td colspan = '2'><asp:ListBox runat="server" Height="140px" Width="258px" id = "lbContract"></asp:ListBox></td>
        </tr>
        <tr>
            <td colspan = '2'><asp:Button runat="server" Text="Remove Contract" 
                    onclick="Unnamed2_Click"></asp:Button></td>
        </tr>
        <tr>
            <td><asp:Button runat="server" Text="Save" onclick="Unnamed3_Click"></asp:Button></td>
        </tr>
    </table>

</asp:Content>

