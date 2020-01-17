<%@ Page Title="Tour Counts By Wave" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="TourCountsByWave.aspx.vb" Inherits="Reports_Tours_TourCountsByWave" AspCompat="true"%>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField ID="sdate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td><uc1:datefield ID="edate" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:Button ID="Report" runat="server" Text="Run Report" />
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lblErr" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Literal runat="server" id = "litReport"></asp:Literal>

</asp:Content>

