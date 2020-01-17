<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditOwnerFAQ.aspx.vb" Inherits="online_EditOwnerFAQ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr>
        <td>FAQID:</td>
        <td>
            <asp:TextBox ID="txtFAQID" runat="server" ReadOnly="True"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td valign="top">Question:</td>
        <td>
            <asp:TextBox ID="txtQuestion" runat="server" TextMode="MultiLine" 
                Height="106px" Width="694px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td valign="top">Answer:</td>
        <td>
            <asp:TextBox ID="txtAnswer" runat="server" TextMode="MultiLine" Height="106px" 
                Width="694px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Active:</td>
        <td>
            <asp:CheckBox ID="cbActive" runat="server" />
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>
            <asp:Button ID="btnSave" runat="server" Text="Save" />
        </td>
    </tr>
</table>
</asp:Content>

