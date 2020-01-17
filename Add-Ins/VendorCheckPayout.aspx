<%@ Page Title="Vendor Check Payout" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="VendorCheckPayout.aspx.vb" Inherits="Add_Ins_VendorCheckPayout" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dteStartDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dteEndDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Check Number:</td>
            <td>
                <asp:TextBox ID="txtCheckNum" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Invoice:</td>
            <td>
                <asp:DropDownList ID="ddInvoices" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Get Invoices" />
            </td>
        </tr>
    </table>
    <asp:GridView id = "gvInvoices" runat="server" autogeneratecolumns = "False" 
        EmptyDataText = "No Invoices" EnableModelValidation="True" onRowDataBound = "gvInvoices_RowDataBound">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="cb" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID"></asp:BoundField>
            <asp:BoundField DataField="ReservationID" HeaderText="ResID"></asp:BoundField>
            <asp:BoundField DataField="CheckInDate" HeaderText="InDate"></asp:BoundField>
            <asp:BoundField DataField="CheckOutDate" HeaderText="OutDate"></asp:BoundField>
            <asp:BoundField DataField="Balance" HeaderText="Balance"></asp:BoundField>
            <asp:BoundField DataField="FirstName" HeaderText="First Name"></asp:BoundField>
            <asp:BoundField DataField="LastName" HeaderText="Last Name"></asp:BoundField>
            <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
            <asp:TemplateField HeaderText="Amount">
                <ItemTemplate>
                    <asp:TextBox ID="txtAmt" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
    <asp:Button runat="server" Text="Button" onclick="Unnamed1_Click" visible = "false" id = "btnSubmit"></asp:Button>
</asp:Content>

