<%@ Page Title="Edit Refurb" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditRefurb.aspx.vb" Inherits="Maintenance_EditRefurb" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Refurb ID:</td>
            <td><asp:Label ID="lbRefurbID" runat="server" Text="0"></asp:Label></td>
        </tr>
        <tr>
            <td>Name:</td>
            <td><asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Description:</td>
            <td><asp:TextBox ID="txtDescription" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Room Type:</td>
            <td>
                <uc1:Select_Item ID="siRoomType" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Unit Type:</td>
            <td>
                <uc1:Select_Item ID="siUnitType" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Unit Style:</td>
            <td>
                <uc1:Select_Item ID="siUnitStyle" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" Text="Save" />
                <asp:Button ID="btnClose" runat="server" Text="Close" />
            </td>
        </tr>
    </table>
    <fieldset>
        <legend>Items:</legend>
        <asp:GridView ID="gvItems" runat="server" EmptyDataText = "No Records" EnableModelValidation="True">
            <Columns>
                <asp:ButtonField CommandName="Remove" Text="Remove" />
            </Columns>
        </asp:GridView>
        <br />
        <fieldset>
            <legend>Add Existing:</legend>
            <asp:DropDownList ID="ddItems" runat="server"></asp:DropDownList>
            <asp:Button ID="btnAddExisting" runat="server" Text="Add" />
        </fieldset>
        <br />
        <fieldset>
            <legend>Add New Item</legend>
            <table>
                <tr>
                    <td>Area:</td>
                    <td><uc1:Select_Item ID="siAreas" runat="server" /></td>
                </tr>
                <tr>
                    <td>Description:</td>
                    <td><asp:TextBox ID="txtItemDescription" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Button ID="btnAddNewItem" runat="server" Text ="Add New Item" /></td>
                </tr>
            </table>
        </fieldset>
        
        

    </fieldset>
</asp:Content>

