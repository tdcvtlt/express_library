<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Reservations.aspx.vb" Inherits="marketing_Reservations" title="Reservations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Filter:"></asp:Label>
    <asp:DropDownList ID="ddfilter" runat="server">
    <asp:ListItem Value = "resID">ReservationID</asp:ListItem>
    <asp:ListItem Value = "guest">Guest</asp:ListItem>
    <asp:ListItem Value = "resNumber">Reservation Number</asp:ListItem>
    </asp:DropDownList><br />
    <asp:Label ID="Label2" runat="server" Text="Value:"></asp:Label><asp:TextBox ID="txtFilter" runat="server"></asp:TextBox>  
    <asp:Button ID="Button1" runat="server" Text="Query" />
    <br /><br /> 
<div style="height:350px;width:650px;overflow:auto; ">
    <asp:GridView ID="gvReservations" runat="server" EmptyDataText = "No Reservations" AutoGenerateSelectButton onRowDataBound = "gvReservations_RowDataBound">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />
    </asp:GridView>
</div>
<asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>

