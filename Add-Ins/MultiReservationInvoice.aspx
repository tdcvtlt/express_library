<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="MultiReservationInvoice.aspx.vb" Inherits="Add_Ins_MultiReservationInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Invoice:</td>
            <td><asp:DropDownList runat="server" id = "ddInvoice"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Amount:</td>
            <td><asp:TextBox runat="server" id = "txtAmount"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Reference:</td>
            <td><asp:TextBox runat="server" id = "txtReference"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Description:</td>
            <td><asp:TextBox runat="server" id = "txtDesc"></asp:TextBox></td>
        </tr>
        <tr>
            <td>ReservationID:</td>
            <td><asp:TextBox runat="server" id = "txtReservation"></asp:TextBox></td>
            <td><asp:Button runat="server" Text="Add" onclick="Unnamed1_Click"></asp:Button></td>
        </tr>
        <tr>
            <td colspan = '2'><asp:ListBox runat="server" Height="140px" Width="258px" id = "lbReservation"></asp:ListBox></td>
        </tr>
        <tr>
            <td colspan = '2'><asp:Button runat="server" Text="Remove Reservation" 
                    onclick="Unnamed2_Click"></asp:Button></td>
        </tr>
        <tr>
            <td><asp:Button runat="server" Text="Save" onclick="Unnamed3_Click"></asp:Button></td>
        </tr>
    </table>

</asp:Content>

