<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="editPackage.aspx.vb" Inherits="marketing_editPackage" title="Editing a Package Record" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<%@ Register src="../controls/UserFields.ascx" tagname="UserFields" tagprefix="uc3" %>

<%@ Register src="../controls/Financials.ascx" tagname="Financials" tagprefix="uc4" %>

<%@ Register src="../controls/VoiceStamps.ascx" tagname="VoiceStamps" tagprefix="uc5" %>

<%@ Register src="../controls/Premiums.ascx" tagname="Premiums" tagprefix="uc6" %>

<%@ Register src="../controls/Notes.ascx" tagname="Notes" tagprefix="uc7" %>

<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc8" %>

<%@ Register src="../controls/PersonnelTrans.ascx" tagname="PersonnelTrans" tagprefix="uc9" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript">
        function alertTransferConfirmation() {
            
            var re = prompt('Please enter prospect id to receive package transfer', '');
            if (re != null) {
                document.getElementById('<%= tbxProspectID.ClientID %>').value = re;
                document.getElementById('<%= btnTransfer.ClientID%>').click();
            }
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:LinkButton ID="lbProspect" runat="server">LinkButton</asp:LinkButton>
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Personnel_Link" runat="server">Personnel</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Notes_Link" runat="server">Notes</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UserFields_Link" runat="server">User Fields</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 4 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="VoiceStamps_Link" runat="server">Voice Stamps</asp:LinkButton></li>
    </ul>
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 5 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Package_Link" runat="server">Package</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 6 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Reservations_Link" runat="server">Reservations</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 7 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Tours_Link" runat="server">Tours</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 8 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Financials_Link" runat="server">Financials</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 9 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Premiums_Link" runat="server">Premiums</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 10 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="CreditCards_Link" runat="server">Credit Cards</asp:LinkButton></li>

    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="vPersonnel" runat="server">
            <asp:GridView ID="gvPersonnel" runat="server" AutoGenerateSelectButton="True" EmptyDataText = "No Records">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
            </asp:GridView>
            <uc9:PersonnelTrans ID="PersonnelTrans1" runat="server" KeyField="PackageIssuedID"/>
        </asp:View>
        <asp:View ID="vNotes" runat="server">
                <uc7:Notes ID="Notes1" runat="server" KeyField="PackageIssuedID"/>
        </asp:View>
        <asp:View ID="vUserFields" runat="server">
            <uc3:UserFields ID="UF" runat="server" />
        </asp:View>
        <asp:View ID="vEvents" runat="server">
            <uc8:Events ID="Events1" runat="server" />
        </asp:View>
        <asp:View ID="vVoiceStamps" runat="server">
            <uc5:VoiceStamps ID="VoiceStamps1" runat="server" Keyfield = "packageissuedid"/>
        </asp:View>
        <asp:View ID="vPackage" runat="server">
            <table>
                <tr>
                    <td>ID:</td>
                    <td><asp:TextBox ID="txtPackageID" runat="server" ReadOnly></asp:TextBox></td>
                    <td>Balance Due Date:</td>
                    <td><uc1:DateField ID="dteBalanceDueDate" runat="server" /></td>
                </tr>
                <tr>
                    <td>Package:</td>
                    <td>
                        <asp:DropDownList ID="ddPackage" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>Purchase Date:</td>
                    <td>
                        <uc1:DateField ID="dtePurchaseDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Cost:</td>
                    <td><asp:TextBox ID="txtCost" runat="server"></asp:TextBox></td>
                    <td>Status Date:</td>
                    <td><asp:Textbox ID = "txtStatusDate" runat="Server" readonly></asp:Textbox>
                    </td>
                </tr>
                <tr>
                    <td>Balance:</td>
                    <td><asp:TextBox ID="txtBalance" runat="server"></asp:TextBox></td>
                    <td>Issue Date:</td>
                    <td>
                        <uc1:DateField ID="dteIssueDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Status:</td>
                    <td>
                        <uc2:Select_Item ID="siStatus" runat="server" />
                    </td>
                    <td>Next Pay Date:</td>
                    <td>
                        <uc1:DateField ID="dteNextPayDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Amount Down:</td>
                    <td>
                        <asp:TextBox ID="txtAmoundDown" runat="server"></asp:TextBox>
                    </td>
                    <td>Vendor:</td>
                    <td>
                        <asp:DropDownList ID="ddVendors" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Expiration Date:</td>
                    <td>
                        <uc1:DateField ID="dteExpirationDate" runat="server" />
                    </td>
                    <td>Source:</td>
                    <td>
                        <asp:DropDownList ID="ddPkgSource" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
           <asp:Button ID="btnSave" runat="server" Text="Save" />
           <asp:TextBox ID="prospectID" Visible="False" runat="server"></asp:TextBox>
            <asp:Label ID="LblPackageError" runat="server" ForeColor="Red"></asp:Label>
            <asp:LinkButton ID="lbEmail" runat="server">Email Confirmation Letter</asp:LinkButton>
            <br />
            <asp:Button runat="server" ID="tbxAlert" Text="Transfer Package" />            
            <asp:TextBox ID="tbxProspectID" runat="server" Text="Prospect ID"></asp:TextBox>
            <asp:Button runat="server" ID="btnTransfer"  />

        </asp:View>
        <asp:View ID="vReservations" runat="server">
            <asp:GridView ID="gvReservations" runat="server" 
                EmptyDataText = "No Records" AutoGenerateSelectButton="True">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
            </asp:GridView>
            <asp:Button ID="Button1" runat="server" Text="Add Reservation" />
        </asp:View>
        <asp:View ID="vTours" runat="server">
            <asp:GridView ID="gvTours" runat="server" EmptyDataText = "No Records" AutoGenerateSelectButton="True">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
            </asp:GridView>
        </asp:View>
        <asp:View ID="vFinancials" runat="server">
            <uc4:Financials ID="Financials1" runat="server" keyfield = "packageissuedid" />
        </asp:View>
        <asp:View ID="vPremiums" runat="server">
            <uc6:Premiums ID="Premiums1" runat="server" />
        </asp:View>
        <asp:View ID="vCreditCards" runat="server">
        </asp:View>
        <asp:View ID="DENIED" runat="server">
            ACCESS DENIED
        </asp:View>
    </asp:MultiView>
</asp:Content>

