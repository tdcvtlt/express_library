<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="editKiosk.aspx.vb" Inherits="setup_Kiosks_editKiosk" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<%@ Register src="../../controls/UserFields.ascx" tagname="UserFields" tagprefix="uc3" %>

<%@ Register src="../../controls/Notes.ascx" tagname="Notes" tagprefix="uc6" %>

<%@ Register src="../../controls/Events.ascx" tagname="Events" tagprefix="uc7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Kiosk_Link" runat="server">Kiosk</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Configuration_Link" runat="server">Configuration</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UploadedFiles_Link" runat="server">Uploaded Files</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 4 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Notes_Link" runat="server">Notes</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 5 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UserFields_Link" runat="server">User Fields</asp:LinkButton></li>
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="Kiosk_View" runat="server">
                       
            <table>
                <tr>
                    <td>Kiosk ID:</td>
                    <td>
                        <asp:TextBox ID="txtKioskID" runat="server" readonly></asp:TextBox>
                    </td>
                    <td>Name:</td>
                    <td><asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Key:</td>
                    <td colspan="3"><asp:TextBox ID="txtKey" runat="server" Width="304px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>License #:</td>
                    <td><asp:TextBox ID="txtLicense" runat="server"></asp:TextBox></td>
                    <td>Active:</td>
                    <td><asp:CheckBox ID="ckActive" runat="server" /></td>
                </tr>
                <tr>
                    <td>Location</td>
                    <td colspan="3">
                        <uc2:Select_Item ID="siLocation" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Registration Expires:</td>
                    <td colspan="2">
                        <uc1:DateField ID="dteExpiration" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td><asp:Button ID="btnSave3" runat="server" Text="Save" /></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="Configuration_View" runat="server">
            <table>
                <tr>
                    <td>Terms:</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Reset Timer (seconds):</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Completed Reset Timer:</td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="2">Qualifications:</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <table>
                            <tr>
                                <td>Marital Status:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Age:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Income:</td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Images:</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <table>
                            <tr>
                                <td>Logo 1:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Logo 2:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Logo 3:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Logo 4:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Logo 5:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Logo 6:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Logo 7:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Logo 8:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Logo 9:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Logo 10:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Coupon:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>NQ Coupon:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Coupon Background:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Screen 1:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Screen 2:</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Screen 3:</td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="UploadedDocs_View" runat="server">
           
            <br />
        </asp:View>

        <asp:View ID="Events_View" runat="server">
            <uc7:Events ID="ucEvents" runat="server" />
        </asp:View>
        <asp:View ID="Notes_View" runat="server">
            <uc6:Notes ID="ucNotes" runat="server" />
        </asp:View>
        <asp:View ID="UserFields_View" runat="server">
            <uc3:UserFields ID="UF" runat="server" />
        </asp:View>
        
        <asp:View runat="server" id ="DENIED">
            ACCESS DENIED
        </asp:View>
    </asp:MultiView>
    
    <asp:Label ID="lblKioskError" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>

