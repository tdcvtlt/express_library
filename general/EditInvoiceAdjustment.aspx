<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditInvoiceAdjustment.aspx.vb" Inherits="general_EditInvoiceAdjustment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Edit Adjustment</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>Adjustment Method:</td>
                <td>
                    <asp:DropDownList ID="ddAdjustments" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Adjustment Type:</td>
                <td>
                    <asp:RadioButton ID="rbPos" GroupName="AdjSign" runat="server" Text = "Positive"/>
                    <asp:RadioButton Checked="true" ID="rbNeg" GroupName="AdjSign" runat="server" Text = "Negative" />
                </td>
            </tr>
            <tr>
                <td>Amount:</td>
                <td><asp:textbox runat="server" id = "txtAmount"></asp:textbox></td>
            </tr>
            <tr>
                <td>Description:</td>
                <td><asp:textbox runat="server" id = "txtDescription"></asp:textbox></td>
            </tr>
            <tr>
                <td colspan = "2"><asp:button ID="Button1" runat="server" text="Process Adjustment" 
                        onclick="Unnamed1_Click"  /></td>
            </tr>
        </table>
    </div>
<asp:GridView ID="gvInvoices" runat="server" AutoGenerateColumns="false" onRowDataBound = "gvInvoices_RowDataBound">
            <AlternatingRowStyle BackColor="#C7E3D7" />
            <Columns>
                <asp:templatefield>
                    <ItemTemplate>
                         
                        <asp:RadioButton ID="cb" groupName = "rbG" runat="server"   AutoPostBack="True" 
                            oncheckedchanged="cb_CheckedChanged" />
                    </ItemTemplate>
                </asp:templatefield>
                <asp:BoundField HeaderText="ID" DataField = "ID" />
                <asp:BoundField HeaderText="Acct" DataField = "Acct" />
                <asp:BoundField HeaderText="Invoice" DataField = "Invoice" />
                <asp:BoundField HeaderText="TransDate" DataField = "TransDate" />
                <asp:BoundField HeaderText="Amount" DataField = "Amount" />
                <asp:BoundField HeaderText="Balance" DataField = "Balance" />
                <asp:BoundField HeaderText="CCApproval" DataField = "CCApproval" />
            </Columns>
        </asp:GridView>
    <asp:label runat="server" text="Label" id = "lblErr"></asp:label>
    <asp:hiddenfield runat="server" id = "hfInvoiceID" value = "0"></asp:hiddenfield>
    </form>
</body>
</html>
