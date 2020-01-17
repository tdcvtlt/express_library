<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="VendorReport.aspx.vb" Inherits="Reports_Packages_VendorReport" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="font-family:Cambria;margin-left:40px;">


<div id="criteria">
    <table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dteSDate" runat="server"  />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dteEDate" runat="server"  />
            </td>
        </tr>
        <tr>
            <td>Vendor:</td>
            <td>
                <asp:DropDownList ID="ddVendors" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">                                
                <asp:Button ID="Button1" Text="Run Report" runat="server" Visible="true" />                
            </td>
        </tr>
    </table>
</div>
    

    <asp:Literal ID="lit" runat="server">
    </asp:Literal>
       
</div>

</asp:Content>



