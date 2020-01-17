<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditLeadProgram.aspx.vb" Inherits="LeadManagement_EditLeadProgram" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language=javascript type ="text/javascript">
    function Refresh_Locations()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$Locations_Link','');
    }
    function Refresh_Images() {
        __doPostBack('ctl00$ContentPlaceHolder1$Images_Link', '');

    }
    function Refresh_HTML() {
        __doPostBack('ctl00$ContentPlaceHolder1$HTML_Link', '');
    }
    function Refresh_Devices() {
        __doPostBack('ctl00$ContentPlaceHolder1$Devices_Link', '');
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="LeadProgram_Link" runat="server">Lead Program</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Locations_Link" runat="server">Locations</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 2 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Images_Link" runat="server">Images</asp:LinkButton></li>    
        <!--<li <%If MultiView1.ActiveViewIndex = 3 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="HTML_Link" runat="server">HTML Files</asp:LinkButton></li>    -->
        <li <%If MultiView1.ActiveViewIndex = 4 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Devices_Link" runat="server">Devices</asp:LinkButton></li>    
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <table>
                <tr>
                    <td>ID:</td>
                    <td>
                        <asp:TextBox ID="txtID" runat="server"></asp:TextBox></td>
                    <td>Description:</td>
                    <td>
                        <asp:TextBox ID="txtDesc" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>URL:</td>
                    <td colspan ="3">
                        <asp:TextBox ID="txtURL" runat="server" Width="448px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Use Pictures: <br />(unchecked will use video)</td>
                    <td>
                        <asp:CheckBox ID="cbUsePics" runat="server" /></td>
                    <td>Pic Interval:</td>
                    <td>
                        <asp:TextBox ID="txtPicInterval" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Exe Script:</td>
                    <td>
                        <asp:TextBox ID="txtExeScript" runat="server"></asp:TextBox></td>
                    <td>Exe Script Timer:</td>
                    <td>
                        <asp:TextBox ID="txtExeScriptTimer" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Screen Timer:</td>
                    <td>
                        <asp:TextBox ID="txtScreenTimer" runat="server"></asp:TextBox></td>
                    <td>Terms Timer:</td>
                    <td>
                        <asp:TextBox ID="txtTermsTimer" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Screen Saver:</td>
                    <td>
                        <asp:TextBox ID="txtScreenSaver" runat="server"></asp:TextBox></td>
                    <td>Registration:</td>
                    <td>
                        <asp:TextBox ID="txtRegistration" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Entry Form:</td>
                    <td>
                        <asp:DropDownList ID="ddEntryForm" runat="server"></asp:DropDownList></td>
                    <td>Vendor:</td>
                    <td>
                        <asp:DropDownList ID="ddVendor" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" /></td>

                </tr>
            </table>

        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:GridView ID="gvLocations" runat="server" AutoGenerateSelectButton="true" EmptyDataText ="No Records"></asp:GridView>
            <ul id ="menu">
                    <li><asp:LinkButton ID="lbNewLoc" runat="server">Add Location</asp:LinkButton></li>    
            </ul>
        </asp:View>
        <asp:View ID="View3" runat="server">
            <asp:GridView ID="gvImages" runat="server" AutoGenerateSelectButton="true" EmptyDataText ="No Records"></asp:GridView>
            <ul id ="menu">
                <li><asp:LinkButton ID="lbNewImage" runat="server">Add Image</asp:LinkButton></li>    
            </ul>
        </asp:View>
        <asp:View ID="View4" runat="server">
            <asp:GridView ID="gvURLS" runat="server" AutoGenerateSelectButton="true" EmptyDataText ="No Records"></asp:GridView>
            <ul id ="menu">
                <li><asp:LinkButton ID="lbNewURL" runat="server">Add HTML</asp:LinkButton></li>    
            </ul>
        </asp:View>
        <asp:View ID="View5" runat="server">
            <asp:GridView ID="gvDevices" runat="server" AutoGenerateSelectButton="true" EmptyDataText ="No Records"></asp:GridView>
            <ul id ="menu">
                <li><asp:LinkButton ID="lbNewDevice" runat="server">Add Device</asp:LinkButton></li>    
            </ul>
        </asp:View>
    </asp:MultiView>

</asp:Content>

