<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="tourCheckIn.aspx.vb" Inherits="wizards_tourCheckIn2" %>
<%@ Register src="~/controls/UserFields.ascx" tagname="UserFields" tagprefix="uc3" %>
<%@ Register src="~/controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register src="~/controls/DateField.ascx" tagname="DateField" tagprefix="uc2" %>
<%@ Register src="~/controls/Campaign.ascx" tagname="Campaign" tagprefix="uc4" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <style type="text/css">
        .style1
        {
            width: 760px;
        }
        .style24
        {
            width: 93px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">



<asp:MultiView runat="server" id = "MultiView1">

<asp:View id = "ProspectInfo" runat="server">
    <table class="style1">
        <tr>
            <td>First Name:</td>
            <td><asp:TextBox runat="server" id = "firstnameTxt" /></td>
            <td>Last Name:</td>
            <td class="style24"><asp:TextBox runat="server" id = "lastNameTxt" /></td>
            <td>Spouse First Name:</td>
            <td><asp:TextBox runat="server" id = "spouseFirstTxt"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Address:</td>
            <td colspan = "3"><asp:TextBox ID="addressTxt" runat="server" Width="382px"></asp:TextBox></td>
            <td>Spouse Last Name:</td>
            <td><asp:TextBox runat="server" id = "spouseLastTxt"></asp:TextBox></td>
        </tr>
        <tr>
            <td>City:</td>
            <td><asp:TextBox ID="City" runat="server"></asp:TextBox></td>
            <td>State:</td>
            <td class="style24">
                <uc1:Select_Item ID="state" runat="server" />
            </td>
            <td>Postal Code:</td>
            <td><asp:TextBox ID="zipTxt" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Phone:</td>
            <td><asp:TextBox ID="phoneTxt" runat="server"></asp:TextBox></td>
            <td>Marital Status:</td>
            <td class="style24">
                <uc1:Select_Item ID="mStatus" runat="server" />
            </td>
            <td>Email:</td>
            <td><asp:TextBox ID="emailTxt" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan = "8" align = "center"><asp:Button runat="server" 
                    Text="Submit Changes / Next >>>" onclick="Unnamed1_Click"></asp:Button></td>
        </tr>
    <table>
</asp:View>
<asp:View id = "TourInfo" runat="server">
    <table>
        <tr>
            <td>TourID:</td>
            <td><asp:TextBox runat="server" id = "tourIDTxt" readonly></asp:TextBox></td>
            <td>Tour Status:</td>
            <td><uc1:Select_Item id="tStatus" runat="server" /></td>
            <td>Campaign:</td>
            <%if request("TourID") <> "0" then %>
            <td><asp:TextBox runat="server" id = "campaignTxt"></asp:TextBox></td>
            <%else %>
            <td><uc4:Campaign id="campaignID" runat="server" /></td>
            <%end if %>
        </tr>
        <tr>
            <td>Tour Date:</td>
            <td>
                <uc2:DateField id="tDate" runat="server" />
            </td>
            <td>Tour Time:</td>
            <td>
                <uc1:Select_Item id="tTime" runat="server" />
            </td>
            <td>Tour Location:</td>
            <td>
                <uc1:Select_Item id="tLocation" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan = "6" align = "center">Debit Card: <asp:CheckBox runat="server" id = "ckDebit"></asp:CheckBox>
                <asp:Label ID="statusLbl" runat="server" Text="" visible = "false"></asp:Label><asp:Label runat="server" Text="" id = "lblCamp" visible = "false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan = "6" align = "center"><asp:Button runat="server" 
                    Text="Submit Changes / Next >>>" onclick="Unnamed2_Click" id = "btnTourOriginal"></asp:Button><asp:Button runat="server" Text="Submit Changes / Return to Validation >>>" visible = "false" id = "btnTourRedirect"></asp:Button>
                <asp:LinkButton runat="server" onclick="Unnamed3_Click1" id = "lblRefresh4"></asp:LinkButton><asp:LinkButton runat="server" id = "lblRefresh4a"></asp:LinkButton></asp:LinkButton><asp:LinkButton runat="server" id = "lblRefresh6"></asp:LinkButton><asp:LinkButton runat="server" id = "lblRefresh6a"></asp:LinkButton></td>
        </tr>
    </table>

</asp:View>
<asp:View runat="server" id = "PremiumInfo">
<asp:LinkButton runat="server" Text="Refresh" id = "lblRefresh2"></asp:LinkButton>
<br />
Premiums: <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/wizards/wizeditpremium.aspx?PremiumIssuedID=0&TourID=<%=request("TourID")%>','win01',350,350);">Add Premium</a>
<br />
<asp:GridView runat="server" id = "gvPremiums" EmptyDatatext = "No Premiums Assigned">
            <Columns>
        <asp:TemplateField HeaderText="Select">
            <ItemTemplate>
                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/wizards/wizeditpremium.aspx?PremiumIssuedID=<%#container.Dataitem("ID")%>&TourID=<%=request("TourID")%>','win01',350,350);">Edit</a>
            </ItemTemplate>
        </asp:TemplateField>
            </Columns>

    </asp:GridView>
    <asp:Button runat="server" Text="Issue Premiums / Next >>>" 
        onclick="Unnamed3_Click" id = "btnPremOriginal"></asp:Button><asp:Button runat="server" Text="Issue Premiums / Return to Validation Screen >>>" id = "btnPremRedirect" visible = "false"></asp:Button>
     
</asp:View>
<asp:View runat="server" id = "RepInfo">
    <table>
        <tr>
            <td>Tour Sub-Type:</td>
            <td><uc1:Select_Item ID="siTourSubType" runat="server" /></td>
        </tr>
        <tr>
            <td>Podium:</td>
            <td><asp:DropDownList runat="server" ID="ddPodium">
                <asp:ListItem Selected="True">No Podium</asp:ListItem>
                <asp:ListItem>Podium 1</asp:ListItem>
                <asp:ListItem>Podium 2</asp:ListItem>
                <asp:ListItem>Podium 3</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Sales Rep:</td>
            <td><asp:DropDownList runat="server" id = "ddPersonnelID"></asp:DropDownList></td>
        </tr>
        <tr>
            <td colspan = '2'><asp:Button runat="server" Text="Assign Rep / Next >>>" 
                    onclick="Unnamed4_Click1" id = "btnRepOriginal"></asp:Button><asp:Button runat="server" Text="Assign Rep / Return to Validation Screen >>>" visible = "false" id = "btnRepRedirect"></asp:Button><asp:Label id = "repLbl" runat="server" Text="" visible = "false"></asp:Label></td>
        </tr>
    </table>
<asp:LinkButton runat="server" id = "lblRefresh5"></asp:LinkButton>
</asp:View>

<asp:View runat="server" id = "View1">
<asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton>
    <table>
        <tr>
            <td>First Name:</td>
            <td><asp:TextBox id="txtFN" runat="server"></asp:TextBox></td>
            <td>Last Name:</td>
            <td><asp:TextBox ID="txtLN" runat="server"></asp:TextBox></td>
            <td><uc1:Select_Item ID="siMS" runat="server" Label_Caption="Marital Status:" ComboItem="MaritalStatus" /></td>
        </tr>
        <tr>
            <td>Spouse First Name:</td>
            <td><asp:TextBox id="spouseFN" runat="server"></asp:TextBox></td>
            <td>Spouse Last Name:</td>
            <td><asp:TextBox id="spouseLN" runat= "server"></asp:TextBox></td>
        </tr>
    </table>

    Email(s): <a  href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/wizards/wizeditemail.aspx?EmailID=0&ProspectID=<%=request("ProspectID")%>','win01',350,350);">Add Email</a>
                        <div class="NarrowSideGrid">
                            <asp:GridView ID="gvEmail" runat="server">
                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/wizards/wizeditemail.aspx?EmailID=<%#container.Dataitem("ID")%>&ProspectID=<%=request("ProspectID")%>','win01',350,350);">Edit</a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                            </asp:GridView>
                        <asp:Label ID="lblEmailError" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </div>
                        Phone Numbers:
                                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/wizards/wizeditphone.aspx?PhoneID=0&ProspectID=<%=request("ProspectID")%>','win01',350,350);">Add Phone Number</a>
                                <br />
                                <div class="SideGrid">
                                    <asp:GridView ID="gvPhone" runat="server">
                                        <AlternatingRowStyle BackColor="#C7E3D7" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/wizards/wizeditphone.aspx?PhoneID=<%#container.Dataitem("ID")%>&ProspectID=<%=request("ProspectID")%>','win01',350,350);">Edit</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="lblPhoneError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    
                                    </div>
                                    Addresses: 
                                    <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/wizards/wizeditaddress.aspx?AddressID=0&ProspectID=<%=request("ProspectID")%>','win01',350,350);">Add Address</a>
                                    <br />
                                    <div class="SideGrid">
                                        <asp:GridView ID="gvAddress" runat="server">
                                        <AlternatingRowStyle BackColor="#C7E3D7" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/wizards/wizeditaddress.aspx?AddressID=<%#container.Dataitem("ID")%>&ProspectID=<%=request("ProspectID")%>&linkid=<% 
    Dim oID As String = "$lbRefresh"
    Dim oTest As Object = Me.FindControl("lbRefresh")
    While Not (oTest Is Nothing)
        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
            oID = "ctl00$" & oTest.id & "$" & oID
        End If
        oTest = oTest.parent
    End While
    response.write (oID)
 %>','win01',350,350);">Edit</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView><br />
                                    <asp:Label ID="lblAddressError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </div>
                                    <asp:Button runat="server" Text="Submit Changes / Next >>>" id = "btnProsOriginal"></asp:Button><asp:Button runat="server" Text="Submit Changes / Return to Validation Screen >>>" id = "btnProsRedirect" visible = "false"></asp:Button></asp:LinkButton>
</asp:View>

<asp:View runat="server" id = "Verify">
    <table>
        <tr>
            <td colspan = '2'>Prospect Information / <asp:LinkButton runat="server" 
                    onclick="Unnamed5_Click">Edit</asp:LinkButton></asp:Label></td>
        </tr>
        <tr>
            <td>Name: </td>
            <td><asp:Label runat="server" id="lblName"></asp:Label></td>
            <td>Spouse:</td>
            <td><asp:Label runat="server" id="lblSpouse"></asp:Label></td>
        </tr>
    </table>
    <br />
    <asp:GridView runat="server" id = "gvAddressVal"></asp:GridView>
    <br />
    <asp:GridView runat="server" id = "gvPhoneVal"></asp:GridView>

    <br />
    <table>
        <tr>
            <td colspan = '2'>Tour Information / <asp:LinkButton runat="server" 
                    onclick="Unnamed6_Click">Edit</asp:LinkButton></td>
        </tr>
        <tr>
            <td>TourTime:</td>
            <td><asp:Label runat="server" id="lblTourTime"></asp:Label></td>
            <td>Tour Date:</td>
            <td><asp:Label runat="server" id="lblTourDate"></asp:Label></td>
        </tr>
        <tr>
            <td>Tour Status:</td>
            <td><asp:Label runat="server" id="lblTourStatus"></asp:Label></td>
            <td>Campaign:</td>
            <td><asp:Label runat="server" id="lblCampaign"></asp:Label></td>
        </tr>
        </table>
        <br />
        Premium Information / <asp:LinkButton runat="server" 
        onclick="Unnamed7_Click">Edit</asp:LinkButton>
        <br />
        <asp:GridView runat="server" id = "gvPremiumVal"></asp:GridView>
        <br />
    <table>
        <tr>
            <td>Sales Rep: <asp:LinkButton runat="server" id = "lnkEditRep" onclick="Unnamed8_Click">Edit</asp:LinkButton></td>
        </tr>
        <tr>
            <td><asp:Label runat="server" id="lblSalesRep"></asp:Label></td>
        </tr>
        <tr>
            <td>Podium:</td>
            <td><asp:Label runat="server" id="lblPodium"></asp:Label></td>
        </tr>
    </table>
            <br />
        Notes / <asp:LinkButton runat="server" onclick="Unnamed9_Click">Edit</asp:LinkButton>
        <br />
        <asp:GridView runat="server" id = "gvNotesVal"></asp:GridView>
        <br />
    <asp:Button runat="server" Text="Submit" onclick="Unnamed4_Click"></asp:Button>
</asp:View>

<asp:View runat="server" id = "Confirmation">
TourID <asp:Label runat="server" id = "lblTourID"></asp:Label> Checked In.<br />
<asp:LinkButton runat="server" onclick="Unnamed10_Click">Back to Today's Tours</asp:LinkButton>
</asp:View>

<asp:View runat="server" id = "NotesTab">
<asp:LinkButton runat="server" Text="Refresh" id = "lblRefresh3"></asp:LinkButton>
<br />
Notes: <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/wizards/wizeditnote.aspx?NoteID=0&TourID=<%=request("TourID")%>','win01',350,350);">Add Note</a>
<br />
<asp:GridView runat="server" id = "gvNotes" EmptyDatatext = "No Notes Entered">
</asp:GridView>
    <asp:Button runat="server" Text="Insert Notes / Next >>>" 
        onclick="Unnamed11_Click" id = "btnNotesOriginal"/></asp:Button><asp:Button runat="server" Text="Insert Notes / Return to Validation Screen >>>" id = "btnNoteRedirect" visible = "false"></asp:Button>

</asp:View>

<asp:View runat="server" id = "View5">
    ACCESS DENIED
</asp:View>
</asp:MultiView>
<asp:HiddenField ID="hfProsID" runat="server" Value = "0" />

    
</asp:Content>

