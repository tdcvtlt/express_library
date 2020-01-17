<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="editTour.aspx.vb" Inherits="marketing_editTour" title="Editing a Tour Record" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<%@ Register src="../controls/UserFields.ascx" tagname="UserFields" tagprefix="uc3" %>

<%@ Register src="../controls/PersonnelTrans.ascx" tagname="PersonnelTrans" tagprefix="uc4" %>
<%@ Register src="../controls/Notes.ascx" tagname="Notes" tagprefix="uc5" %>
<%@ Register src="../controls/Financials.ascx" tagname="Financials" tagprefix="uc6" %>
<%@ Register src="../controls/Premiums.ascx" tagname="Premiums" tagprefix="uc7" %>

<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc8" %>

<%@ Register src="../controls/Campaign.ascx" tagname="Campaign" tagprefix="uc9" %>

<%@ Register src="../controls/UploadedDocs.ascx" tagname="UploadedDocs" tagprefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language=javascript type ="text/javascript">
    function Save_Tour()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$lbSaveTour', '');    
    }

    function Refresh_Vendor()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$Vendor_Link','');
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:HyperLink ID="hlPros" runat="server"></asp:HyperLink>
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Notes_Link" runat="server">Notes</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UserFields_Link" runat="server">User Fields</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Financials_Link" runat="server">Financials</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 9 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Uploaded_Docs_Link" runat="server">Uploaded Docs</asp:LinkButton></li>
    </ul>
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 4 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Tour_Link" runat="server">Tour</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 5 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Personnel_Link" runat="server">Personnel</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 6 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Premiums_Link" runat="server">Premiums</asp:LinkButton></li>
        <%If 1 = 2 Then%><li <%if  MultiView1.ActiveViewIndex = 7 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Accom_Link" runat="server">Accom</asp:LinkButton></li><%end if %>
        <li <%if MultiView1.ActiveViewIndex = 10 then: response.write ("class=""current"""):end if  %>><asp:LinkButton ID="Chargebacks_Link" runat="server">ChargeBacks</asp:LinkButton></li>
        <li <%if MultiView1.ActiveViewIndex=11 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Vendor_Link" runat="server">Vendor Location</asp:LinkButton></li>
            </ul>

<asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="vNotes" runat="server">
            <uc5:Notes ID="Notes1" runat="server" KeyField="TourID" />
        </asp:View>
        <asp:View ID="vEvents" runat="server">
            
            <uc8:Events ID="Events1" runat="server" />
            
        </asp:View>
        <asp:View ID="vUserFields" runat="server">
            <uc3:UserFields ID="UF" runat="server" />
        </asp:View>
        <asp:View ID="vFinancials" runat="server">
            <uc6:Financials ID="Financials1" runat="server" />
        </asp:View>
        <asp:View ID="vTour" runat="server">
            <table>
                <tr>
                    <td>Tour ID:</td>
                    <td><asp:TextBox ID="txtTourID" runat="server" readonly></asp:TextBox></td>
                    <td>Prospect</td>
                    <td>#</td>
                </tr>
                <tr>
                    <td valign="top">Date:</td>
                    <td>
                        <uc1:DateField ID="dteTourDate" runat="server" />
                    </td>
                    <td valign="top">Time:</td>
                    <td><uc2:Select_Item ID="siTourTime" runat="server" /></td>
                </tr>
                <tr>
                    <td valign="top">Status:</td>
                    <td>
                        <uc2:Select_Item ID="siStatus" runat="server" />
                    </td>
                    
                    <td valign="top">Location:</td>
                    <td>
                        <uc2:Select_Item ID="siLocation" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Campaign:</td>
                    <td>
                        <uc9:Campaign ID="Campaign1" runat="server" />
                    </td>
                    <td>Package:</td>
                    <td>#</td>
                </tr>
                <tr>
                    <td valign="top">Type:</td>
                    <td>
                        <uc2:Select_Item ID="siType" runat="server" />
                    </td>
                    <td valign="top">Sub Type:</td>
                    <td>
                        <uc2:Select_Item ID="siSubType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">Source:</td>
                    <td>
                        <uc2:Select_Item ID="siSource" runat="server" />
                    </td>
                    <td valign="top">Booking Date:</td>
                    <td>
                        <uc1:DateField ID="dteBookingDate" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="LblTourError" runat="server" ForeColor="Red"></asp:Label>
            <asp:LinkButton ID="lbSaveTour" runat="server"></asp:LinkButton>
            <br />
                <ul id="menu">                   
                     <li><asp:LinkButton ID="lbSave" runat="server">Save Tour</asp:LinkButton></li>
                    <li><asp:LinkButton ID="lbLetterPrint" runat="server">Print Letter</asp:LinkButton></li>
                    <li><asp:LinkButton ID="lbLetterEmail" runat="server">Email Letter</asp:LinkButton></li>
                    <li><asp:LinkButton ID="lbPrintTourSlip" runat="server">Print Tour Slip</asp:LinkButton></li>
            </ul>
        </asp:View>
        <asp:View ID="vPersonnel" runat="server">
            <uc4:PersonnelTrans ID="PersonnelTrans1" runat="server" />
        </asp:View>
        <asp:View ID="vPremiums" runat="server">
            <uc7:Premiums ID="Premiums1" runat="server" />
        </asp:View>
        <asp:View ID="vAccom" runat="server">
        </asp:View>
        <asp:View ID = "DENIED" runat="server">
            ACCESS DENIED
        </asp:View>
        <asp:View ID = "vUploadedDocs" runat="server">
            <uc10:UploadedDocs ID="ucUploadedDocs" runat="server" />
        </asp:View>
        <asp:View ID="vCB" runat="server">
            <asp:GridView ID="gvChargeBacks" runat="server" AutoGenerateSelectButton="True">
            </asp:GridView>
        </asp:View>
        <asp:View ID="vVRT" runat="server">
            <asp:GridView ID="gvRep2Tour" runat="server" AutoGenerateSelectButton="true">

            </asp:GridView>
            <ul id="menu">                   
                     <li><asp:LinkButton ID="lbAddVT" runat="server">Add</asp:LinkButton></li>
            </ul>
        </asp:View>
    </asp:MultiView>

    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
    </asp:Content>
