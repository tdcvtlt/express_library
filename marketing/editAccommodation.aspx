<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editAccommodation.aspx.vb" Inherits="marketing_editAccommodation" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:scriptmanager runat="server"></asp:scriptmanager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <table>
            <tr>
                <td>ID:</td>
                <td><asp:textbox runat="server" id = "txtAccomID" readonly></asp:textbox></td>
                <td>Confirmation Number:</td>
                <td><asp:TextBox ID="txtConfirmation" runat="server"></asp:TextBox>
                </td>
            </tr>
            
            <tr>
                <td>Location:</td>
                <td><asp:DropDownList ID="ddResLocation" runat="server" AutoPostBack ="true">
                    </asp:DropDownList>
                </td>
                <td>Accommodation:</td>
                <td>
                    <asp:DropDownList ID="ddAccom" runat="server" AutoPostBack = "true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td>Check-In Location:</td>
                <td>
                    <asp:DropDownList ID="ddCheckIn" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Guest Type:</td>
                <td><uc2:Select_Item ID="siGuestType" runat="server" /></td>
                <td>Room Type:</td>
                <td><uc2:Select_Item ID="siRoomType" runat="server" /></td>
            </tr>
            
            <tr>
                <td>ArrivalDate:</td>
                <td><uc1:DateField ID="dteArrivalDate" runat="server" /></td>
                <td>DepartureDate:</td>
                <td><uc1:DateField ID="dteDepartureDate" runat="server" /></td>
            </tr>
            <tr>
                <td>NumberAdults:</td>
                <td><asp:DropDownList ID="ddNumAdults" runat="server"></asp:DropDownList></td>
                <td>NumberChildren:</td>
                <td><asp:DropDownList ID="ddNumChildren" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>PromoNights:</td>
                <td><asp:DropDownList ID="ddPromoNights" runat="server"></asp:DropDownList></td>
                <td>PromoRate:</td>
                <td><asp:textbox runat="server" id = "txtPromoRate"></asp:textbox></td>
            </tr>
            <tr>
                <td>AdditionalNights:</td>
                <td><asp:DropDownList ID="ddAddNights" runat="server"></asp:DropDownList></td>
                <td>AdditionalRate:</td>
                <td><asp:textbox runat="server" id = "txtAddRate"></asp:textbox></td>
            </tr>
            <tr>
                <td>RoomCost:</td>
                <td><asp:TextBox ID="txtRoomCost" runat="server"></asp:TextBox></td>
                <td>Smoking:</td>
                <td><asp:checkbox runat="server" id = "chkSmoking"></asp:checkbox></td>
            </tr>   
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
        <asp:button runat="server" text="Save" onclick="Unnamed1_Click" />
    </form>
</body>
</html>
