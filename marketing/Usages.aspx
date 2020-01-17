<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Usages.aspx.vb" Inherits="marketing_Usages" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Usage ID:</td>
            <td>
                <asp:TextBox ID="txtUsageID" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Unit Type:</td>
            <td>
                <uc1:Select_Item ID="siUnitType" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Usage Type:</td>
            <td>
                <uc1:Select_Item ID="siUsageType" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Usage Sub Type:</td>
            <td>
                <uc1:Select_Item ID="siUsageSubType" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Room Type:</td>
            <td>
                <asp:DropDownList ID="ddRoomType" runat="server"></asp:DropDownList>
                <asp:Button runat="server" Text="Add" autopostback = "false" 
                    onclick="Unnamed1_Click"></asp:Button>
            </td>
            <td><asp:ListBox ID="lbRoomTypes" runat="server" Height="84px" Width="130px"></asp:ListBox>
                <asp:Button runat="server" Text="Remove" onclick="Unnamed2_Click"></asp:Button></td>
        </tr>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc2:DateField ID="dteStartDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc2:DateField ID="dteEndDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Contract Number:</td>
            <td>
                <asp:DropDownList ID="ddContracts" runat="server"></asp:DropDownList>
            </td>
            <td>OR: <asp:TextBox runat="server" id = "txtConNum"></asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Button runat="server" Text="Search" onclick="Unnamed3_Click"></asp:Button></td>
        </tr>        
    
    </table>

    <asp:Label runat="server" id = "lblErr"></asp:Label>

    <asp:GridView ID="gvUsages" runat="server" EmptyDataText = "No Usages" onRowDataBound = "gvUsages_RowDataBound"
        AutoGenerateSelectButton="True">
    </asp:GridView>



</asp:Content>

