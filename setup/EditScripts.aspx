<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditScripts.aspx.vb" Inherits="setup_EditScripts" %>

<%@ Register src="../controls/Notes.ascx" tagname="Notes" tagprefix="uc1" %>
<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
        <ul id="menu">
            <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Script" runat="server">Script</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Note" runat="server">Notes</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="dooodooooo" runat="server">Events</asp:LinkButton></li>
        </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="Edit_View" runat="server">
            <table>
                <tr>
                    <td>
                        TollID:
                    </td>
                    <td>
                        <asp:TextBox ID="txtScriptID" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        ScriptName:
                    </td>
                    <td>
                        <asp:TextBox ID="txtScript" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" />
                    </td>
                    <td>
                       
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="Note_View" runat="server">
        
            <uc1:Notes ID="Notes1" runat="server" />
        
        </asp:View>
        <asp:View ID="Event_View" runat="server">
        
            <uc2:Events ID="Events1" runat="server" />
        
        </asp:View>
    </asp:MultiView>
</asp:Content>

