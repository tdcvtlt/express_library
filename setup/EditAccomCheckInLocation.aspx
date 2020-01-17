<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditAccomCheckInLocation.aspx.vb" Inherits="setup_EditAccomCheckInLocation" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
    <li <% if MultiView1.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonAccom" runat="server">Accommodation</asp:LinkButton>
    </li>    
    </ul>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <table>
                <tr>
                    <td>Location:</td>
                    <td>
                        <uc1:Select_Item ID="siLocations" runat="server" />
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" /></td>
                </tr>
            </table>
            Directions:
            <CKEditor:CKEditorControl ID="CKEditor1" runat="server" Height = "800" Width = "1000"></CKEditor:CKEditorControl>
        </asp:View>
    </asp:MultiView>
</asp:Content>

