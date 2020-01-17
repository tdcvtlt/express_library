<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditLetter.aspx.vb" Inherits="setup_Letters_EditLetter" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Name:</td>
            <td><asp:TextBox ID="txtName" runat="server" Width="227px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Type:</td>
            <td>
                <uc1:Select_Item ID="siType" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button ID="btnSave" runat="server" Text="Save Letter" /><asp:Button ID="btnPreview" runat="server" Text="Preview Letter" /></td>
        </tr>
    </table> 
    
    <asp:LinkButton ID="lbTagMap" runat="server">Tag Mapping</asp:LinkButton>
        <CKEditor:CKEditorControl ID="CKEditor1" runat="server" Height = "800" Width = "1000"></CKEditor:CKEditorControl><asp:HiddenField
            ID="hfTypeID" runat="server" Value=0 />
</asp:Content>

