<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="TypeUtilization.aspx.vb" Inherits="Reports_Reservations_TypeUtilization" AspCompat = "true" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField ID="dteSDate" runat="server" /></td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td><uc1:DateField ID="dteEDate" runat="server" /></td>
        </tr>
        <tr>
            <td>Type:</td>
            <td>
                <asp:DropDownList ID="ddResType" runat="server">
                </asp:DropDownList><asp:Button runat="server" Text="Add" 
                    onclick="Unnamed1_Click"></asp:Button>
            </td>
            <td>Sub Type:</td>
            <td><asp:DropDownList id = "ddSubType" runat="server"></asp:DropDownList>
                <asp:Button runat="server" Text="Add" onclick="Unnamed2_Click"></asp:Button></td>
        </tr>
        <tr>
            <td colspan = '2'><asp:ListBox runat="server" Height="176px" Width="162px" id = "lbResType"></asp:ListBox>
                <asp:Button runat="server" Text="Remove" onclick="Unnamed3_Click"></asp:Button></td>
            <td colspan = '2'><asp:ListBox runat="server" Height="176px" Width="162px" id = "lbSubType"></asp:ListBox>
                <asp:Button runat="server" Text="Remove" onclick="Unnamed4_Click"></asp:Button></td>
        </tr>
        <tr>
            <td><asp:Button runat="server" Text="Run Report" onclick="Unnamed5_Click"></asp:Button></td>
        </tr>
    </table>
    <br />
    <asp:Literal runat="server" id = "litReport"></asp:Literal>
</asp:Content>

