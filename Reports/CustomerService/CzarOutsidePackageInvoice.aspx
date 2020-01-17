<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CzarOutsidePackageInvoice.aspx.vb" Inherits="Reports_CustomerService_CzarOutsidePackageInvoice" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>CZAR OUTSIDE PACKAGE INVOICE</h1>
<div>
    <table>
        <tr>
            <td><label>Start Date:</label></td>
            <td>
                <uc1:datefield ID="dteSDate" Selected_Date="" runat="server" />
            </td>
        </tr>
        <tr>
            <td><label>End Date:</label></td>
            <td>
                <uc1:datefield ID="dteEDate" Selected_Date="" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:Button ID="btn_Submit" runat="server" Text="Submit"></asp:Button></td>
            <td><asp:Button ID="btn_Excel" runat="server" Text="Excel" ></asp:Button></td>
        </tr>
    </table>
</div>

<div>
    <asp:GridView runat="server" ID="gvReport" ShowFooter="True">       
    </asp:GridView>
</div>
</asp:Content>

