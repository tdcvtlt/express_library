<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RequestRefund.aspx.vb" Inherits="general_RequestRefund" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    Refund Method: <asp:dropdownlist runat="server" id = "ddRefMethod" autopostback = "true"></asp:dropdownlist>
    <asp:multiview runat="server" id = "MultiView1">
        <asp:View runat="server" id = "CCView">
            <asp:GridView runat="server" id = "gvCCTrans" AutoGenerateSelectButton="True" onRowDataBound = "gvCCTrans_RowDataBound" EmptyDataText = "No Credit Card Transactions">
            
            </asp:GridView>
            <asp:HiddenField runat="server" id ="hfCCTransID"></asp:HiddenField>
            <asp:HiddenField runat="server" id = "hfRefRequest"></asp:HiddenField>
        </asp:View>
        <asp:View runat="server" id = "CashView">
            <asp:GridView runat="server" id = "gvCashPayments" autoGenerateColumns = "False" emptydatatext = "No Cash Payments" onRowDataBound = "gvCashPayments_RowDataBound">
            <Columns>
                    <asp:templatefield>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb" runat="server" />
                        </ItemTemplate>
                    </asp:templatefield>
                    <asp:BoundField HeaderText="ID" DataField = "PaymentID" />
                    <asp:BoundField HeaderText="InvoiceID" DataField = "InvoiceID" />
                    <asp:BoundField HeaderText="Trans Date" DataFIeld ="TransDate" />
                    <asp:BoundField HeaderText="Invoice" DataField = "Invoice" />
                    <asp:BoundField HeaderText="Amount" DataField="Amount" />
                    <asp:templatefield HeaderText = "Amount">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAmount" runat="server" />
                        </ItemTemplate>
                    </asp:templatefield>
                </Columns>            
            </asp:GridView>            
            <asp:Button runat="server" Text="Submit Refund" onclick="Unnamed1_Click1"></asp:Button>
            <asp:Label runat="server" id = "lblErr3"></asp:Label>
        </asp:View>
        <asp:View runat="server" id = "CheckView">
        <br />
            <asp:Label runat="server" id = "lblErr4"></asp:Label>
            <asp:GridView ID="gvPayments" runat="server" AutoGenerateColumns="false" 
                EmptyDataText="No Payments" onRowDataBound="gvPayments_RowDataBound">
                <Columns>
                    <asp:templatefield>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb" runat="server" />
                        </ItemTemplate>
                    </asp:templatefield>
                    <asp:BoundField DataField="PaymentID" HeaderText="ID" />
                    <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID" />
                    <asp:BoundField DataField="Invoice" HeaderText="Invoice" />
                    <asp:BoundField DataFIeld="TransDate" HeaderText="Trans Date" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" />
                    <asp:BoundField DataField="MerchantAccountID" HeaderText="MerchantAccountID" />
                    <asp:BoundField DataField="Method" HeaderText="Payment Method" />
                    <asp:templatefield HeaderText="Amount">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAmount" runat="server" />
                        </ItemTemplate>
                    </asp:templatefield>
                    <asp:TemplateField HeaderText="Check Number">
                        <ItemTemplate>
                            <asp:TextBox ID="txtNumber" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Button ID="Button2" runat="server" Text="Submit Refund" />
        </asp:View>    
        <asp:View runat="server" id = "CCApplyView">
            <asp:GridView runat="server" id = "gvCCApply" autogeneratecolumns = "False" onRowDataBound = "gvCCApply_RowDataBound" EmptyDataText = "No Records">
                <Columns>
                    <asp:templatefield>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb" runat="server" />
                        </ItemTemplate>
                    </asp:templatefield>
                    <asp:BoundField HeaderText="ID" DataField = "PaymentID" />
                    <asp:BoundField HeaderText="InvoiceID" DataField = "InvoiceID" />
                    <asp:BoundField HeaderText="Invoice" DataField = "Invoice" />
                    <asp:BoundField HeaderText="Amount" DataField="Amount" />
                    <asp:templatefield HeaderText = "Amount">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAmount" runat="server" />
                        </ItemTemplate>
                    </asp:templatefield>
                    <asp:BoundField HeaderText="PaymentMethod" DataField = "PaymentMethod" />
                </Columns>
            </asp:GridView>
            <asp:Button runat="server" Text="Process Refund" onclick="Unnamed1_Click"></asp:Button> 
            <asp:Button runat="server" Text="Cancel" onclick="Unnamed2_Click"></asp:Button>
            <asp:Label runat="server" id = "lblErr"></asp:Label>

        </asp:View>
    </asp:multiview>
    </form>
</body>
</html>
