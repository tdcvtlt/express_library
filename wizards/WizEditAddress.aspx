<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WizEditAddress.aspx.vb" Inherits="wizards_EditAddress" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>AddressID:</td>
                <td>
                    <asp:TextBox readonly ID="txtAddressID" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>ProspectID:</td>
                <td>
                    <asp:TextBox ReadOnly ID="txtProspectID" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Active:</td>
                <td>
                    <asp:CheckBox ID="ckActive" runat="server" /></td>
            </tr>
            <tr>
                <td>Address 1:</td>
                <td>
                    <asp:TextBox ID="txtAddress1" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Address 2:</td>
                <td>
                    <asp:TextBox ID="txtAddress2" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>City:</td>
                <td>
                    <asp:TextBox ID="txtCity" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>State:</td>
                <td>
                    <uc1:Select_Item ID="siState" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Postal Code:</td>
                <td>
                    <asp:TextBox ID="txtZip" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Region:</td>
                <td>
                    <asp:TextBox ID="txtRegion" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Country:</td>
                <td>
                    <uc1:Select_Item ID="siCountry" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Type:</td>
                <td>
                    <uc1:Select_Item ID="siType" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Contract Address:</td>
                <td>
                    <asp:CheckBox ID="ckContractAddress" runat="server" /></td>
            </tr>
            
        </table>
        <asp:Button ID="btnSave" runat="server" Text="Save" /><br />
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    </div>
    </form>
</body>
</html>
