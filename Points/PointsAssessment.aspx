<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PointsAssessment.aspx.vb" Inherits="Points_PointsAssessment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Usage Year:</td>
            <td><asp:DropDownList ID="ddUsageYear" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnPreview" runat="server" Text="Preview" />
                <asp:Button ID="btnAssess" runat="server" Text="Assess" />
                <asp:Button ID="btnExport" runat="server" Text="Export" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblErr" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvPoints" runat="server" AutoGenerateColumns="true"></asp:GridView>
</asp:Content>

