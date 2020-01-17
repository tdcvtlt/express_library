<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditPhone.aspx.vb" Inherits="general_EditPhone" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Phone</title>
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
                <td>Record ID:</td>
                <td>
                    <asp:TextBox ID="txtPhoneID" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Prospect ID:</td>
                <td>
                    <asp:TextBox ID="txtProspectID" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Number:</td>
                <td>
                    <asp:TextBox ID="txtNumber" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Extension:</td>
                <td>
                    <asp:TextBox ID="txtExtension" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Type:</td>
                <td>
                    <uc1:Select_Item ID="siType" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Active:</td>
                <td>
                    <asp:CheckBox ID="ckActive" runat="server" /></td>
            </tr>
        </table>
        <asp:Button ID="btnSave" runat="server" Text="Save" />
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    </div>
    </form>
</body>
</html>
