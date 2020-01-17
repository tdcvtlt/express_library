<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditReservationLetter.aspx.vb" Inherits="setup_EditReservationLetter" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language=javascript type ="text/javascript">
        function Refresh_Sources() {
            __doPostBack('ctl00$ContentPlaceHolder1$Source_Link', '');
        }
        function Refresh_Locations() {
            __doPostBack('ctl00$ContentPlaceHolder1$Locations_Link', '');

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id = "menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Letter_Link" runat="server">Letter</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Source_Link" runat="server">Sources</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Locations_Link" runat="server">Locations</asp:LinkButton></li>
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <table>
                <tr>
                    <td>Letter ID:</td>
                    <td>
                        <asp:TextBox ID="txtID" runat="server" readonly></asp:TextBox></td>
                        <td>
                            <asp:LinkButton ID="LinkButton1" runat="server">Letter Tags</asp:LinkButton></td>
                </tr>
                <tr>
                    <td>Description:</td>
                    <td>
                        <asp:TextBox ID="txtDesc" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Resort Company:</td>
                    <td>
                        <uc1:Select_Item ID="siCompany" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Booked Tour:</td>
                    <td>
                        <asp:CheckBox ID="cbTour" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Email Subject:</td>
                    <td>
                        <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Email Address:</td>
                    <td><asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Save" /></td>
                </tr>
            </table>
            <CKEditor:CKEditorControl ID="CKEditor1" runat="server" Height = "800" Width = "1000"></CKEditor:CKEditorControl>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:GridView ID="gvSources" runat="server" AutoGenerateSelectButton = "true" EmptyDataText = "No Records">
             </asp:GridView>
            <asp:Button ID="Button2" runat="server" Text="Add Source" />
        </asp:View>
        <asp:View ID="View3" runat="server">
            <asp:GridView ID="gvLocations" runat="server" AutoGenerateSelectButton = "true" EmptyDataText = "No Records">
             </asp:GridView>
            <asp:Button ID="Button3" runat="server" Text="Add Location" />
        </asp:View>
    </asp:MultiView>
</asp:Content>

