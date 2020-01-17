<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditJobApplication.aspx.vb" Inherits="security_EditJobApplication" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<%@ Register src="../controls/UploadedDocs.ascx" tagname="UploadedDocs" tagprefix="uc2" %>
<%@ Register src="../controls/Notes.ascx" tagname="Notes" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language=javascript type ="text/javascript">
    function Refresh_App()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$btnSave','');
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Job_Link" runat="server">Application</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Description_Link" runat="server">Uploaded HR Docs</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 2 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="LinkButton1" runat="server">Uploaded Docs</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 3 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Applicants_Link" runat="server">Notes</asp:LinkButton></li>
    
    </ul>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <table>
                <tr>
                    <td>ID:</td>
                    <td><asp:TextBox ID="txtID" runat="server"></asp:TextBox></td>
                    <td>Applicant:</td>
                    <td>
                        <asp:LinkButton ID="lbUser" runat="server"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>Job Posting:</td>
                    <td>
                        <asp:LinkButton ID="lbJob" runat="server"></asp:LinkButton>
                    </td>
                    <td>Job Posting:</td>
                    <td>
                        <asp:DropDownList ID="ddJobPosting" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Status:</td>
                    <td>
                        <uc1:Select_Item ID="siStatus" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" /></td>
                    <td>
                        <asp:Button ID="btnRelease" runat="server" Text ="Release Application" /></td>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <uc2:UploadedDocs ID="UploadedDocs1" runat="server" />
        </asp:View>
        <asp:View ID="View3" runat="server">
            <uc2:UploadedDocs ID="UploadedDocs2" runat="server" />
        </asp:View>
        <asp:View ID="View4" runat="server">
            <uc3:Notes ID="Notes1" runat="server" />
        </asp:View>
    </asp:MultiView>

</asp:Content>