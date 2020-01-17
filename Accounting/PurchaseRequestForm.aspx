<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PurchaseRequestForm.aspx.vb" Inherits="Accounting_PurchaseRequestForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="695">
            <tr><td align = 'Right'>PURCHSE REQUISITION</td><td align = right>REQ #: ___________</td></tr>
            <tr><td align = 'right'>King's Creek Plantation Owner Association</td><td align = right>Priority Level#______</td></tr>
            <tr><td align = 'right'></td><td align = 'right'>Purchase Request ID: <asp:Label ID="Label1" runat="server" Text=""></asp:Label></td></tr>
        </table>
        <p></p>
        <br><br>
        <table width="722">
        <tr>
	        <td width="131">Date:</td>
	        <td width="150"><asp:Label ID="lblDateCreated" runat="server" Text=""></asp:Label></td>
	        <td width="182" align = right>Ship To:</td>
	        <td align = 'right' width="241">King's Creek Plantation Owner Association</td>
        </tr>
        <tr>
	        <td colspan = '3'></td>
	        <td align = 'right' width="241">1540C Penniman Road</td>
        </tr>
        <tr>
	        <td>Vendor Name:</td>
	        <td><asp:Label ID="lblVendor" runat="server" Text=""></asp:Label></td>
	        <td width="182"></td>
	        <td align = 'right' width="241">Williamsburg, VA 23185</td>
        </tr>
        <tr rowspan = '4'><td><br><br></td></tr>
        <tr>
	        <td colspan = '4'>
               <asp:Table ID="Table1" runat="server" BorderStyle="Solid" Width="716px" 
                    GridLines="Both">
                   <asp:TableRow runat="server">
                       <asp:TableCell runat="server" Font-Bold="True" Width="47px">Line Item</asp:TableCell>
                       <asp:TableCell runat="server" Font-Bold="True" Width="75px">Item #</asp:TableCell>
                       <asp:TableCell runat="server" Font-Bold="True" Width="34px">Qty</asp:TableCell>
                       <asp:TableCell runat="server" Font-Bold="True" Width="208px">Description</asp:TableCell>
                       <asp:TableCell runat="server" Font-Bold="True" Width="149px">Location</asp:TableCell>
                       <asp:TableCell runat="server" Font-Bold="True" Width="103px">Purpose</asp:TableCell>
                       <asp:TableCell runat="server" Font-Bold="True" Width="74px">Unit Price</asp:TableCell>
                       <asp:TableCell runat="server" Font-Bold="True" Width="55px">Total</asp:TableCell>
                   </asp:TableRow>

               </asp:Table>
            </td>
        </tr>
        <tr rowspan = '4'><td><br><br><br></td></tr>
        <tr>
        <td colspan = '2'></td>
        <td width="182" align = right>Ordered By:</td>
        <td align = right width="241"><asp:Label ID="lblOrderedBy" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr><td><br></td></tr>
        <tr>
        <td colspan = '2'></td>
        <td width="182" align = right>Approved By:</td>
        <td align = right width="241">_____________________</td>
        </tr>

</table>
    </div>
    </form>
</body>
</html>
