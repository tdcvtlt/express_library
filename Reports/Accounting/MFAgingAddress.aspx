<%@ Page Title="Maintenance Fee Aging W/ Addresses Report" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="MFAgingAddress.aspx.vb" Inherits="MFAging_Address" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>

    <table>
        <tr>
            <td>Cut-off Date:</td>
            <td><uc1:DateField ID="DateField1" runat="server" /></td>
        </tr>
        <tr>
            <td>MF Year:</td>
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
            <td colspan="3"><asp:Button ID="Button3" runat="server" Text="Run Report" /></td>
        </tr>
    </table>

        <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />

</asp:DropDownList> 




<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" 
            ToolPanelView="None" />



        



</div>


</asp:Content>

