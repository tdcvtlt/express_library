<%@ Page Title="Special Requests" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="SpecialRequests.aspx.vb" Inherits="SpecialRequests_Default" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>

    <table>
        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField ID="DateField1" runat="server" /></td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td><uc1:DateField ID="DateField2" runat="server" /></td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>Reservation Status:</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
            </td>
            <td align="center">
                <asp:Button ID="Button4" runat="server" Text="<< ALL" /><br />
                <asp:Button ID="Button1" runat="server" Text="<<" /><br />
                <asp:Button ID="Button2" runat="server" Text=">>" /><br />
                <asp:Button ID="Button5" runat="server" Text="ALL >>" />
            </td>
            <td>
                <asp:ListBox ID="ListBox2" runat="server"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td>Reservation Type:</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:ListBox ID="ListBox3" runat="server"></asp:ListBox>
            </td>
            <td align="center">
                <asp:Button ID="Button6" runat="server" Text="<< ALL" /><br />
                <asp:Button ID="Button7" runat="server" Text="<<" /><br />
                <asp:Button ID="Button8" runat="server" Text=">>" /><br />
                <asp:Button ID="Button9" runat="server" Text="ALL >>" />
            </td>
            <td>
                <asp:ListBox ID="ListBox4" runat="server"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td>Request Type:</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:ListBox ID="ListBox5" runat="server"></asp:ListBox>
            </td>
            <td align="center">
                <asp:Button ID="Button10" runat="server" Text="<< ALL" /><br />
                <asp:Button ID="Button11" runat="server" Text="<<" /><br />
                <asp:Button ID="Button12" runat="server" Text=">>" style="height: 26px" /><br />
                <asp:Button ID="Button13" runat="server" Text="ALL >>" />
            </td>
            <td>
                <asp:ListBox ID="ListBox6" runat="server"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:Button ID="Button3" runat="server" Text="Run Report" /></td>
        </tr>
    </table>

        <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />

 




<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" 
            ToolPanelView="None" />



        



</div>


</asp:Content>

