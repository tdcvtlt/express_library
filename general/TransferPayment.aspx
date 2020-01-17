<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TransferPayment.aspx.vb" Inherits="general_TransferPayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Select Payment To Transfer:
        <br />
        Adjust Balance After Transfer: <asp:checkbox runat="server" id = "chkAdjust"></asp:checkbox>
        <asp:gridview runat="server" id = "gvPayments" EnableModelValidation="True" onRowDataBound = "gvPayments_RowDataBound" EmptyDataText = "No Payments To Transfer">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:RadioButton ID="cb" runat="server" groupName = "rbG" runat="server"   AutoPostBack="True" 
                            oncheckedchanged="cb_CheckedChanged" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>                    
        </asp:gridview>
    </div>

    <asp:radiobuttonlist runat="server" id = "rbTransfer" 
        RepeatDirection="Horizontal" OnSelectedIndexChanged = "rbTransfer_SelectedIndexChanged" autoPostBack = "true" visible = "false">
        <asp:ListItem>Contract</asp:ListItem>
        <asp:ListItem>Reservation</asp:ListItem>
        <asp:ListItem>Tour</asp:ListItem>
        <asp:ListItem>Package</asp:ListItem>
        <asp:ListItem>Mortgage</asp:ListItem>
        <asp:ListItem>Conversion</asp:ListItem>
        <asp:ListItem>Prospect</asp:ListItem>
    </asp:radiobuttonlist>
    <asp:label runat="server" text="" id = "lblTransfer"></asp:label><asp:textbox runat="server" id = "txtFilter" visible = "false"></asp:textbox><asp:button runat="server" text="Search Invoices" visible = "false" id = "btnsearch" autopostback = "true"/>
    <asp:gridview runat="server" id = "gvInvoices" 
        EmptyDataText = "No Invoices For This Account" autoGenerateColumns = "False" 
        EnableModelValidation="True" onRowDataBound = "gvInvoices_RowDataBound">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="cb" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField ="ID" HeaderText="ID"></asp:BoundField>
            <asp:BoundField DataField ="Invoice" HeaderText="Invoice"></asp:BoundField>
            <asp:BoundField DataField = "TransDate" HeaderText = "TransDate"></asp:BoundField>
            <asp:BoundField DataField = "Amount" HeaderText = "Amount"></asp:BoundField>
            <asp:BoundField DataField = "Balance" HeaderText = "Balance"></asp:BoundField>
            <asp:TemplateField HeaderText=" Transfer Amount">
                <ItemTemplate>
                    <asp:TextBox runat="server" id = "txtAmt"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>                    
    </asp:gridview>
    <asp:button runat="server" text="Transfer Payment" visible = "False" id = "btnTransfer" />
    <br />
    <asp:label runat="server" id = "lblErr"></asp:label>
    <asp:hiddenfield runat="server" id = "hfPaymentID"></asp:hiddenfield>
    <asp:hiddenfield runat="server" id = "hfPaymentAmt"></asp:hiddenfield>
    <asp:hiddenfield runat="server" id = "hfInvoiceID"></asp:hiddenfield>
    <asp:hiddenfield runat="server" id = "hfKeyField"></asp:hiddenfield>
    <asp:hiddenfield runat="server" id = "hfKeyValue"></asp:hiddenfield>
    <asp:hiddenfield runat="server" id = "hfProsID"></asp:hiddenfield>
    </form>

</body>
</html>
