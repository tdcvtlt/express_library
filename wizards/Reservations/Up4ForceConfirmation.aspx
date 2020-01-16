<%@ Page Title="" Language="VB" MasterPageFile="~/wizards/Reservations/ReservationMasterPage.master" AutoEventWireup="false" CodeFile="Up4ForceConfirmation.aspx.vb" Inherits="wizards_Reservations_Up4ForceConfirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head2" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">

    <div style="margin:0 auto;">
        <br /><br /><br />
        <h1>Reservation ID</h1>
        <asp:Label runat="server" ID="reservationID" Font-Size="Larger"></asp:Label>
        <br /><br />
        <h2>Tour ID</h2>
        <asp:Label runat="server" ID="tourID" Font-Size="Larger"></asp:Label>

        <br /><br />

        <h4>Tour Date/Time</h4>
        <asp:Label runat="server" ID="tourDate" Font-Size="Larger"> </asp:Label>
        <br /><br /><br /><br />
        <asp:Button runat="server" ID="btnExit" Text="Back To Home"  CssClass="btn btn-lg btn-success center-block"  Style="font-weight:bolder;"/>
    </div>
</asp:Content>

