<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="RentalCategory.aspx.vb" Inherits="Reports_Reservations_RentalCategory" aspcompat = "true"%>

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
            <td>Unit Size:</td>
            <td>
                <asp:DropDownList ID="ddUnitSize" runat="server">
                </asp:DropDownList>
                <asp:Button runat="server" Text="Add" 
                    onclick="Unnamed1_Click" style="height: 26px"></asp:Button>
            </td>
            <td>Unit Type:</td>
            <td><asp:DropDownList id = "ddUnitType" runat="server"></asp:DropDownList>
                <asp:Button runat="server" Text="Add" onclick="Unnamed2_Click"></asp:Button></td>
        </tr>
        <tr>
            <td colspan = '2'><asp:ListBox runat="server" Height="176px" Width="162px" id = "lbUnitSize"></asp:ListBox>
                <asp:Button runat="server" Text="Remove" onclick="Unnamed3_Click"></asp:Button></td>
            <td colspan = '2'><asp:ListBox runat="server" Height="176px" Width="162px" id = "lbUnitType"></asp:ListBox>
                <asp:Button runat="server" Text="Remove" onclick="Unnamed4_Click"></asp:Button></td>
        </tr>
        <tr>
            <td>Category:</td>
            <td>
                <asp:DropDownList ID="ddCategory" runat="server">
                </asp:DropDownList><asp:Button runat="server" Text="Add" 
                    onclick="Unnamed5_Click"></asp:Button>
            </td>
            <td>Usage Year:</td>
            <td><asp:DropDownList id = "ddUsageYear" runat="server"></asp:DropDownList>
                <asp:Button runat="server" Text="Add" onclick="Unnamed6_Click"></asp:Button></td>
        </tr>
        <tr>
            <td colspan = '2'><asp:ListBox runat="server" Height="176px" Width="162px" id = "lbCategory"></asp:ListBox>
                <asp:Button runat="server" Text="Remove" onclick="Unnamed7_Click"></asp:Button></td>
            <td colspan = '2'><asp:ListBox runat="server" Height="176px" Width="162px" id = "lbUsageYear"></asp:ListBox>
                <asp:Button runat="server" Text="Remove" onclick="Unnamed8_Click"></asp:Button></td>
        </tr>
        <tr>
            <td><asp:Button runat="server" Text="Run Report" onclick="Unnamed9_Click"></asp:Button></td>
        </tr>
    </table>
    <br />
    <asp:Literal runat="server" id = "litReport"></asp:Literal>
</asp:Content>

