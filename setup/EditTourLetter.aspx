<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditTourLetter.aspx.vb" Inherits="setup_EditTourLetter" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type ="text/javascript">
        function Refresh_Campaign() {
            __doPostBack('ctl00$ContentPlaceHolder1$Campaign_Link', '');
        }
        function Refresh_Location() {
            __doPostBack('ctl00$ContentPlaceHolder1$Location_Link', '');
        }
        function Refresh_Letter() {
            __doPostBack('ctl00$ContentPlaceHolder1$Letter_Link', '');
        }
       
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id = "menu">
        <li <%if  mv.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Letter_Link" runat="server">Letter</asp:LinkButton></li>
        <li <%if  mv.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Campaign_Link" runat="server">Campaign</asp:LinkButton></li>
        <li <%if  mv.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Location_Link" runat="server">Location (Tour)</asp:LinkButton></li>
    </ul>

    <asp:MultiView ID="mv" runat="server">
        <asp:View ID="view1" runat="server">
            <table>
                <tr>
                    <td>Letter ID:</td>
                    <td>
                        <asp:TextBox ID="txtID" runat="server" readonly="true"></asp:TextBox>
                        <asp:HiddenField ID="txtID_Hidden" runat="server" />
                    </td>
                    <td>
                        <asp:LinkButton ID="LinkButton1" runat="server">Letters</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>Description:</td>
                    <td>
                        <asp:TextBox ID="txtDesc" runat="server"></asp:TextBox>
                    </td>
                </tr>                
                <tr>
                    <td>Email Subject:</td>
                    <td>
                        <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Email Address:</td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                    </td>                    
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Save" />
                    </td>
                </tr>
            </table>
            <br /><br />

            <asp:HyperLink runat="server" Text="back" NavigateUrl="~/setup/TourLetters.aspx"></asp:HyperLink>
            <br />
           

        </asp:View>    
        <asp:View ID="view2" runat="server">
            <asp:GridView ID="gvCampaigns" runat="server" AutoGenerateSelectButton = "true" EmptyDataText = "No Records">
             </asp:GridView>
            <asp:Button ID="Button2" runat="server" Text="Add Campaign" />
        </asp:View>    
        <asp:View ID="view3" runat="server">
            <asp:GridView ID="gvLocations" runat="server" AutoGenerateSelectButton = "true" EmptyDataText = "No Records">
             </asp:GridView>
            <asp:Button ID="Button3" runat="server" Text="Add Location" />
        </asp:View>    
    
    </asp:MultiView>
</asp:Content>

