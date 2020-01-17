<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="prhInvoice.aspx.vb" Inherits="Reports_CustomerService_prhInvoice" AspCompat = "true" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">

input[type='submit'] , input[type='text'] , label
{
    font-family:DejaVu Sans;
    font-size:medium;
}

table 
{
    border-collapse:collapse;
}

</style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td><label>Start Date:</label></td>
            <td>
                <uc1:DateField ID="dteSDate" Selected_Date="" runat="server" />
            </td>
        </tr>
        <tr>
            <td><label>End Date:</label></td>
            <td>
                <uc1:DateField ID="dteEDate" Selected_Date="" runat="server" />
            </td>
        </tr>
        <tr>            
            <td><asp:Button ID="btn_Submit" runat="server" Text="Submit"></asp:Button>
            <asp:Button ID="Button3" runat="server" Text="To Excel" ></asp:Button></td>
        </tr>
    </table>
    <asp:Literal runat="server" id = "litReport"></asp:Literal>

    <br />

    <p>
        <asp:GridView ID="gvR" runat="server" ShowFooter="True"></asp:GridView>
    </p>

  
</asp:Content>

