<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditJobApplicant.aspx.vb" Inherits="security_EditJobApplicant" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<%@ Register src="../controls/UploadedDocs.ascx" tagname="UploadedDocs" tagprefix="uc2" %>
<%@ Register src="../controls/Notes.ascx" tagname="Notes" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Job_Link" runat="server">Applicant</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Description_Link" runat="server">Applications</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 2 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Applicants_Link" runat="server">Notes</asp:LinkButton></li>
    
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
               <table>
                   <tr>
                       <td>First Name:</td>
                       <td>
                           <asp:Label ID="lbFirst" runat="server" Text="Label"></asp:Label></td>
                       <td>Last Name:</td>
                       <td>
                           <asp:Label ID="lbLast" runat="server" Text="Label"></asp:Label></td>
                   </tr>
                   <tr>
                       <td>Phone:</td>
                       <td>
                           <asp:Label ID="lbPhone" runat="server" Text="Label"></asp:Label></td>
                       <td>Mobile:</td>
                       <td>
                           <asp:Label ID="lbMobile" runat="server" Text="Label"></asp:Label></td>
                   </tr>
                   <tr>
                       <td>
                           <asp:Button ID="btnSave" runat="server" Text="Save" /></td>
                   </tr>
               </table>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:GridView ID="gvApplications" runat="server" AutoGenerateSelectButton="true"></asp:GridView>

        </asp:View>
        <asp:View ID="View3" runat="server">
            <uc3:Notes ID="Notes1" runat="server" />
        </asp:View>
    </asp:MultiView>


</asp:Content>

