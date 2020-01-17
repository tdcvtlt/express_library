<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditJobPosting.aspx.vb" Inherits="security_EditJobPosting" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/controls/Select_Item.ascx" TagPrefix="uc1" TagName="Select_Item" %>
<%@ Register src="../controls/Notes.ascx" tagname="Notes" tagprefix="uc3" %>

<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Job_Link" runat="server">Job Posting</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Description_Link" runat="server">Description</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 2 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Applicants_Link" runat="server">Applicants</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 3 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Notes_Link" runat="server">Notes</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 4 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>
    </ul>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <table>
                <tr>
                    <td>ID:</td>
                    <td><asp:TextBox ID="txtID" runat="server" ReadOnly ="true"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Title:</td>
                    <td><asp:TextBox ID="txtTitle" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Department:</td>
                    <td>
                        <uc1:Select_Item ID="siDepartment" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Company:</td>
                    <td>
                        <uc1:Select_Item runat="server" ID="siCompany" />
                    </td>
                </tr>
                <tr>
                    <td>Web Site:</td>
                    <td><uc1:Select_Item runat="server" ID="siWebSite" /></td>
                </tr>
                <tr>
                    <td>Number of Positions:</td>
                    <td><asp:DropDownList ID="ddPositions" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Status:</td>
                    <td>
                        <uc1:Select_Item ID="siStatus" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Type:</td>
                    <td>
                        <uc1:Select_Item ID="siType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Active:</td>
                    <td><asp:CheckBox ID="cbActive" runat="server" /></td>
                </tr>
                <tr>
                    <td>Summary:</td>
                </tr>
                <tr>
                    <td colspan ="6"><asp:TextBox ID="txtSummary" runat="server" Height="147px" Width="435px" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
            </table>
            <ul id="menu">
                <li>
                    <asp:LinkButton ID="lbSaveJob" runat="server">Save</asp:LinkButton></li>
            </ul>


        </asp:View>
        <asp:View ID="View2" runat="server">
            <CKEditor:CKEditorControl ID="CKEditor1" runat="server" Height = "800" Width = "1000"></CKEditor:CKEditorControl>
            <ul id="menu">
                <li>
                    <asp:LinkButton ID="lbSaveDesc" runat="server">Save</asp:LinkButton></li>
            </ul>
        </asp:View>
        <asp:View ID="View3" runat="server">
            <asp:GridView ID="gvApplications" runat="server" AutoGenerateSelectButton="True" EmptyDataText="No Applicants"></asp:GridView>
        </asp:View>
        <asp:View ID="View4" runat="server">
            <uc3:Notes ID="Notes1" runat="server" />
        </asp:View>
        <asp:View ID="View5" runat="server">
            <uc2:Events ID="Events1" runat="server" />
        </asp:View>
    </asp:MultiView>
</asp:Content>

