<%@ Page Title="Concierge Report" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ConciergeReport.aspx.vb" Inherits="frontdesk_ConciergeReport" EnableEventValidation="false" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language=javascript type ="text/javascript">
    function Refresh_Rpt()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$lbReport','');
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr>
        <td colspan = '2'>CheckIn Date:</td>
    </tr>
    <tr>
        <td>Start:</td>
        <td><uc1:DateField ID="dteStart" runat="server" /></td>
    </tr>
    <tr>
        <td>End:</td>
        <td><uc1:DateField ID="dteEnd" runat="server" /></td>
    </tr>
    <tr>
        <td colspan ="2">
            <asp:RadioButtonList ID="rbStatus" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="Booked">Booked/In-House</asp:ListItem>
                <asp:ListItem>Other</asp:ListItem>                
            </asp:RadioButtonList>            
            
            </td>
    </tr>
    <tr>
        <td colspan = '2'><asp:Button runat="server" Text="Run Report" 
                onclick="Unnamed1_Click"></asp:Button><asp:Button
                    ID="btnExcel" runat="server" Text="Export to Excel" /><asp:LinkButton runat="server" id = "lbReport"></asp:LinkButton>
                                        
                    </td>

    </tr>
</table>
<asp:GridView runat="server" EmptyDataText = "No Records" 
        AutoGenerateColumns = "False" ID = "gvRes" 
        OnRowDataBound = "gvRes_RowDataBound" EnableModelValidation="True">
    <Columns>
        <asp:BoundField HeaderText="Guest" DataField="Guest Name"></asp:BoundField>
        <asp:BoundField DataField="Owner (Y/N)" HeaderText="Owner (Y/N)" />
        <asp:BoundField DataField="dnc" HeaderText="Do Not Call" />
        <asp:BoundField HeaderText="Do Not Sell" DataField="dns" />
        <asp:BoundField HeaderText="ResID" DataField="ReservationID"></asp:BoundField>
        <asp:BoundField HeaderText="D-Paper/Resort Finance" DataField="D-Paper/Resort Finance"></asp:BoundField>
        <asp:BoundField HeaderText="Type" DataField="Type"></asp:BoundField>
        <asp:BoundField HeaderText="Sub-Type" DataField="SubType"></asp:BoundField>
        <asp:BoundField HeaderText="Campaign" DataField="Campaign"></asp:BoundField>
        <asp:BoundField HeaderText="Phone" DataField="Phone"></asp:BoundField>
        <asp:BoundField HeaderText="Unit" DataField="Unit"></asp:BoundField>
        <asp:BoundField HeaderText="CheckIn" DataField="CheckIn"></asp:BoundField>
        <asp:BoundField HeaderText="CheckOut" DataField="CheckOut"></asp:BoundField>
        <asp:BoundField HeaderText="Last Tour" DataField="Last Tour"></asp:BoundField>
        <asp:BoundField HeaderText="Phone Numbers" DataField="Phone Numbers"></asp:BoundField>
        <asp:BoundField HeaderText="Email" DataField="Email"></asp:BoundField>
        <asp:BoundField HeaderText="Address" DataField="Address" ></asp:BoundField>
        <asp:BoundField HeaderText="Status" DataField="Status"></asp:BoundField>
        
        <asp:ButtonField CommandName="BookTour" Text="Book" ButtonType="Button" 
            ShowHeader="True"></asp:ButtonField>
        <asp:ButtonField CommandName="ExtraTour" Text="Extra Tour" ButtonType="Button" ></asp:ButtonField>
        <asp:ButtonField CommandName="AddComment" Text="Add Comments" ButtonType="Button" ></asp:ButtonField>
        <asp:ButtonField CommandName="ViewComment" Text="View Comments" ButtonType="Button" ></asp:ButtonField>
        <asp:BoundField HeaderText="Comment1" DataField="Comment1"></asp:BoundField>
        <asp:BoundField HeaderText="Comment2" DataField="Comment2"></asp:BoundField>
        <asp:BoundField HeaderText="Comment3" DataField="Comment3"></asp:BoundField>
        <asp:BoundField HeaderText="Comment4" DataField="Comment4"></asp:BoundField>
        <asp:BoundField HeaderText="Comment5" DataField="Comment5"></asp:BoundField>
        <asp:BoundField HeaderText="Have Comments" DataField="HaveComments"></asp:BoundField>
     </Columns>
</asp:GridView>
<asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>

