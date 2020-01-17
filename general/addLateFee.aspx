<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addLateFee.aspx.vb" Inherits="general_addLateFee" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:scriptmanager runat="server"></asp:scriptmanager>
    <div>
        <table>
            <tr>
                <td>Amount:</td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Description:</td>
                <td>
                    <asp:TextBox ID="txtDesc" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Reference:</td>
                <td>
                    <asp:TextBox ID="txtReference" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Due Date:</td>
                <td>
                    <uc1:DateField ID="dteDueDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="Submit Late Fee" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        Select Invoice to Apply Late Fee To:<br />
        <asp:GridView ID="gvInvoices" runat="server" AutoGenerateColumns="false">
            <AlternatingRowStyle BackColor="#C7E3D7" />
            <Columns>
                <asp:templatefield>
                    <ItemTemplate>
                         
                        <asp:RadioButton ID="cb" groupName = "rbG" runat="server"   AutoPostBack="True" 
                            oncheckedchanged="cb_CheckedChanged" />
                    </ItemTemplate>
                </asp:templatefield>
                <asp:BoundField HeaderText="ID" DataField = "ID" />
                <asp:BoundField HeaderText="Invoice" DataField = "Invoice" />
                <asp:BoundField HeaderText="TransDate" DataField = "TransDate" />
                <asp:BoundField HeaderText="Amount" DataField = "Amount" />
                <asp:BoundField HeaderText="Balance" DataField = "Balance" />
            </Columns>
        </asp:GridView>
    </div>
    <asp:hiddenfield runat="server" id = "hfInvoiceID" value = 0></asp:hiddenfield>
    <asp:label runat="server" id = "lblErr"></asp:label>
    </form>
</body>
</html>
