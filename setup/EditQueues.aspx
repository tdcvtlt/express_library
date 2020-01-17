<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditQueues.aspx.vb" Inherits="setup_EditQueues" %>

<%@ Register src="../controls/Notes.ascx" tagname="Notes" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
        <ul id="menu">
            <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Queue" runat="server">Queue</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Scripts" runat="server">Scripts</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Triggers" runat="server">Triggers</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Notes" runat="server">Notes</asp:LinkButton></li>
        </ul>
        
    
    
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="Edit_View" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblQueuID" runat="server" Text="QueueID:"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtQueueID" runat="server" ReadOnly></asp:TextBox></td>
                    <td>
                        <asp:Label ID="lblName" runat="server" Text="Queue Name:"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCreatedDate" runat="server" Text="Date Created"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtCreatedDate" runat="server"></asp:TextBox></td>
                    <td>
                        <asp:Label ID="lblRequestedBy" runat="server" Text="Requested By:"></asp:Label></td>
                    <td>
                        <asp:DropDownList ID="ddRequestedBy" runat="server">
                        </asp:DropDownList>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtDepartment" runat="server"></asp:TextBox></td>
            
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" />
                    </td>
                </tr>
            </table>    
        </asp:View>
        <asp:View ID="Script_View" runat="server">
            This is the Script View<asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="false" DataKeyNames="ScriptID" AutoGenerateColumns="true" EmptyDataText="No Records" GridLines="Horizontal">
            <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>
                <asp:HyperLinkField HeaderText="Edit" DataNavigateUrlFields="ScriptID" DataNavigateUrlFormatString="editscripts.aspx?scriptid={0}" DataTextField="ScriptID" />
            </Columns>
        </asp:GridView>
        </asp:View>
        <asp:View ID="Trigger_View" runat="server">
            We will Place Associated Triggers here!!!<asp:GridView ID="GridView2" runat="server" AutoGenerateSelectButton="false" DataKeyNames="TriggerID" AutoGenerateColumns="true" EmptyDataText="No Records" GridLines="Horizontal">
            <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>
                <asp:HyperLinkField HeaderText="Edit" DataNavigateUrlFields="TriggerID" DataNavigateUrlFormatString="edittriggers.aspx?triggerid={0}" DataTextField="TriggerID" />
            </Columns>
        </asp:GridView>
        </asp:View>
        <asp:View ID="Note_View" runat="server">
            This is the notes View<uc1:Notes ID="Notes1" runat="server" />
        </asp:View>
    </asp:MultiView>
    
</asp:Content>

