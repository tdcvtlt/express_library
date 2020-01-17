<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditPremiumIssued.aspx.vb" Inherits="general_EditPremiumIssued" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=IIf(Request("kf") = "ReservationID", "Gift Issued", "Premium Issued") %></title>
    <script type="text/javascript" src="../scripts/jquery-1.9.1.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table>
            <tr>
                <td><%=IIf(Request("kf").ToLower() = "reservationid", "Gift", "Premium") %>:</td>
                <td>
                    <asp:DropDownList ID="ddPremium" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                
                    <asp:TextBox ReadOnly ID="txtPremium" runat="server"></asp:TextBox>
                
                </td>
            </tr>
            <tr>
                <td>Certificate Number:</td>
                <td>
                    <asp:TextBox ID="txtCertNumber" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Amount:</td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Charge Back Amount:</td>
                <td>
                    <asp:TextBox ID="txtCBAmount" runat="server" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Quantity:</td>
                <td>
                    <asp:DropDownList ID="ddQuantity" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Status:</td>
                <td>
                    <uc1:Select_Item ID="siStatus" ComboItem="PremiumStatus" runat="server" />
                </td>
            </tr>
        </table><asp:Label ID="lblErr" runat="server" Text="" ForeColor="Red"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
        <table>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" style="height: 26px" /></td>
            </tr>
        </table>
        
    </div>
    <asp:HiddenField ID="hfKeyField" Value = "" runat="server" />
    <asp:HiddenField ID="hfKeyValue" value = "0" runat="server" />
    <asp:HiddenField ID="hfPIID" Value = "0" runat="server" />
    <asp:HiddenField ID="hfPB" Value = "" runat="server" />
    </form>


    <script type="text/javascript">
        $(function () {
            $('#btnSave').click(function (e) {
                var q = $('#ddQuantity').val();
                var c = $('#siStatus_DropDownList1').val();
                // 16809 = Issued
                //
                if ((parseInt(q) == 0) && (c == parseInt(16809))) {
                    alert('Please specify the quantity to issue!');
                    e.preventDefault();
                }                
            });
        });
    </script>
</body>
</html>
