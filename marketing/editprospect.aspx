<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="editprospect.aspx.vb" Inherits="marketing_editprospect" title="Editing a Prospect" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc2" %>

<%@ Register src="../controls/UserFields.ascx" tagname="UserFields" tagprefix="uc3" %>

<%@ Register src="../controls/PersonnelTrans.ascx" tagname="PersonnelTrans" tagprefix="uc4" %>

<%@ Register src="../controls/Financials.ascx" tagname="Financials" tagprefix="uc5" %>



<%@ Register src="../controls/Notes.ascx" tagname="Notes" tagprefix="uc6" %>



<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc7" %>



<%@ Register src="../controls/UploadedDocs.ascx" tagname="UploadedDocs" tagprefix="uc8" %>
<%@ Register Src="~/controls/PointsTracking.ascx" TagPrefix="uc1" TagName="PointsTracking" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script language="javascript" type="text/javascript">
    function Refresh_Phone()
    {
        Refresh_Demographics();
    }

    function Refresh_Financials() {
        __doPostBack('ctl00$ContentPlaceHolder1$Fin_Link','')
    }

    function Refresh_Address()
    {
        Refresh_Demographics()
    }
    
    function Refresh_Email()
    {
        Refresh_Prospect();
    }
    
    function Refresh_Prospect()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$Prospect','');
    }
    
    function Refresh_Demographics()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$Demographics','');
    }
    
    function Refresh_UserFields(){
        __doPostBack('ctl00$ContentPlaceHolder1$UF_Link','');
    }

    function Refresh_CC() {
        __doPostBack('ctl00$ContentPlaceHolder1$CC_Link', '');
    }
    function Print_Envelope(fName, lName, address, city, state, zip) {
        var mWin = window.open();
     mWin.alert('To print to an envelope please perform the following steps: \n 1) Click on the Print menu and select Print Preview. \n 2) Select Page Setup.\n 3) Select the desired envelope under Paper Size.\n 4) Select Landscape under Orientation.\n 5) Press OK\n 6) Press the Print button.');
	 mWin.document.write("<html><body>"); 

	    mWin.document.write("<br><br><br><br><br><br><p align=center><table><tr><td align=left>" +
		    fName + ' ' + lName + '<br>' +
		    address + '<br>' + 
	        city + ', ' + state + ' ' + zip + 
		    '</td></tr></table></p></body></html>');


    	//mWin.notify();
	   
    }

</script>
    <style type="text/css">
        .auto-style1 {
            width: 165px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        
    <asp:Label ID="Label4"   runat="server" text=""></asp:Label>
        <ul id="menu">
            <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Prospect" runat="server">Prospect</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Demographics" runat="server">Demographics</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Tour_Link" runat="server">Tours</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="TourPKG_Link" runat="server">Tour Packages</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 4 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UF_Link" runat="server">User Fields</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 5 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Fin_Link" runat="server">Financials</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 14 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="CC_Link" runat="server">Credit Cards</asp:LinkButton></li>
        </ul>


        <ul id="menu">
            <li <%if  MultiView1.ActiveViewIndex = 6 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Con_Link" runat="server">Contracts</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 7 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Res_Link" runat="server">Reservations</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 8 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 9 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Notes_Link" runat="server">Notes</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 10 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Pers_Link" runat="server">Personnel</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 11 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Ref_Link" runat="server">Referrals</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 13 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Docs_Link" runat="server">Uploaded Files</asp:LinkButton></li>
            <li <%If MultiView1.ActiveViewIndex = 15 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Points_Link" runat="server">Points</asp:LinkButton></li>
        </ul>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="Pros_View" runat="server">
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td><asp:Label ID="Label1" runat="server" Text="ProspectID:"></asp:Label></td>
                                <td><asp:TextBox ID="txtProspectID" runat="server" ReadOnly="True"></asp:TextBox></td>
                                <td class="auto-style1"><uc1:Select_Item ID="type" runat="server" /></td>
                                
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label3" runat="server" Text="Last Name:"></asp:Label></td>
                                <td><asp:TextBox ID="txtLastName" runat="server"></asp:TextBox></td>
                                <td class="auto-style1"><uc1:Select_Item ID="subtype" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label5" runat="server" Text="Middle Init:"></asp:Label></td>
                                <td><asp:TextBox ID="txtMiddleInit" runat="server"></asp:TextBox></td>
                                <td class="auto-style1"><uc1:Select_Item ID="status" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label2" runat="server" Text="First Name:"></asp:Label></td>
                                <td colspan="1"><asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox></td>
                                                                <td colspan ="1">
                                    <asp:Label ID="lblDNS" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    Salutation:<uc1:Select_Item ID="salutation" runat="server" />
                                </td>
                                <td class="auto-style1">&nbsp;</td>
                            </tr>
                            <tr >
                                <td><asp:Label ID="Label6" runat="server" Text="Anniversary Date:"></asp:Label></td>
                                <td colspan="2">
                                    <uc2:DateField ID="dteAnniversaryDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label7" runat="server" Text="Company:"></asp:Label></td>
                                <td colspan="2"><asp:TextBox ID="txtCompany" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label8" runat="server" Text="Title:"></asp:Label></td>
                                <td colspan="2"><asp:TextBox ID="txtTitle" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Referrer:</td>
                                <td colspan="2"><asp:TextBox ID="txtReferrer" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td valign="top">Date Referred:</td>
                                <td colspan="2" valign="top">
                                    <uc2:DateField ID="dteDateReferred" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <uc1:Select_Item ID="siSource" runat="server" />
                                </td>
                                <td class="auto-style1">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>Fed DNC List:</td>
                                <td colspan="2">
                                    <asp:CheckBox ID="cbFedDNC" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnWebSite" runat="server" Text="Website Info" />
                                </td>
                                <td>

                                    <asp:Button ID="btnDNS" runat="server" Text="Button" />

                                </td>

                            </tr class="auto-style1">
                        </table>
                    </td>
                    <td valign="top">
                        Email(s): <a  href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editemail.aspx?EmailID=0&ProspectID=<%=request("ProspectID")%>','win01',350,350);">Add Email</a>
                        <div class="NarrowSideGrid">
                            <asp:GridView ID="gvEmail" runat="server">
                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editemail.aspx?EmailID=<%#container.Dataitem("ID")%>&ProspectID=<%=request("ProspectID")%>','win01',350,350);">Edit</a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Label ID="lblEmailError" runat="server" ForeColor="Red" Text=""></asp:Label>
                        <br />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="Demo_View" runat="server">
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td valign="top">
                                    Birth Date:</td>
                                <td>
                                    <uc2:DateField ID="dteBirthDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    Spouse Birth Date:</td>
                                <td>
                                    <uc2:DateField ID="dteSpouseBirthDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <uc1:Select_Item ID="siMaritalStatus" runat="server" />
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    Spouse First Name:</td>
                                <td align="right">
                                    <asp:TextBox ID="txtSpouseFirstName" runat="server"></asp:TextBox>
                                    
                                    <asp:TextBox ID="txtSpouse" runat="server" Visible="false" value="0"></asp:TextBox>
                                    <asp:Button ID="btnSpouse" runat="server" Text="..." Visible="False" />
                                </td>
                            </tr>
                            <tr>
                                <td>Spouse Last Name:</td>
                                <td align="right"><asp:TextBox ID="txtSpouseLastName" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Spouse SSN:</td>
                                <td align="right"><asp:TextBox ID="txtSpouseSSN" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <uc1:Select_Item ID="siOccupation" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Income:</td>
                                <td align="right">
                                    <asp:TextBox ID="txtIncome" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Income/Debt:</td>
                                <td align="right">
                                    <asp:TextBox ID="txtIncomeDebt" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Credit Score:</td>
                                <td align="right">
                                    <asp:TextBox ID="txtCreditScore" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Spouse Credit Score:</td>
                                <td align="right">
                                    <asp:TextBox ID="txtSpouseCreditScore" runat="server" 
                                       ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    SSN:</td>
                                <td align="right">
                                    <asp:TextBox ID="txtSSN" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Drivers License:</td>
                                <td align="right">
                                    <asp:TextBox ID="txtDriversLicense" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <uc1:Select_Item ID="siDriversLicenseState" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                
                    <td valign = "top">
                        <table>
                            <tr>
                                <td>Phone Numbers:
                                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editphone.aspx?PhoneID=0&ProspectID=<%=request("ProspectID")%>','win01',350,350);">Add Phone Number</a>
                                <br />
                                <div class="SideGrid">
                                    <asp:GridView ID="gvPhone" runat="server">
                                        <AlternatingRowStyle BackColor="#C7E3D7" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editphone.aspx?PhoneID=<%#container.Dataitem("ID")%>&ProspectID=<%=request("ProspectID")%>','win01',350,350);">Edit</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="lblPhoneError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Addresses: 
                                    <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editaddress.aspx?AddressID=0&ProspectID=<%=request("ProspectID")%>','win01',350,350);">Add Address</a>
                                    <br />
                                    <div class="SideGrid">
                                    <asp:GridView ID="gvAddress" runat="server">
                                        <AlternatingRowStyle BackColor="#C7E3D7" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editaddress.aspx?AddressID=<%#container.Dataitem("ID")%>&ProspectID=<%=request("ProspectID")%>','win01',350,350);">Edit</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView><br />
                                    <asp:Label ID="lblAddressError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </div><br />
                                    <asp:Button runat="server" Text="Print Envelope" onclick="Unnamed1_Click"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="Tours_View" runat="server">
            <div class = "ListGrid">
                <asp:GridView ID="gvTours" runat="server" EmptyDataText = "No Tours" onRowDataBound = "gvTours_RowDataBound">
                    <AlternatingRowStyle BackColor="#C7E3D7" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <a href='edittour.aspx?tourid=<%#container.Dataitem("ID")%>&amp;ProspectID=<%=Request("ProspectID")%>'>
                                Edit</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Label ID="lblTourError" runat="server" ForeColor="Red"></asp:Label>
            <ul id="menu">
                <li>
                    <asp:LinkButton ID="lbAddTour" runat="server">New Tour</asp:LinkButton></li>
            </ul>
            
        </asp:View>
        <asp:View ID="TourPackages_View" runat="server">
            <div class="ListGrid">
            <asp:GridView ID="gvPackages" runat="server" EmptyDataText = "No Records" 
                AutoGenerateSelectButton="True">
                <AlternatingRowStyle BackColor="#C7E3D7" />
            </asp:GridView>
            </div>
            <asp:Button ID="Button1" runat="server" Text="Insert Package" />
        </asp:View>
        <asp:View ID="UserFields_View" runat="server">
            <uc3:UserFields ID="UserFields1" runat="server" />
        </asp:View>
        <asp:View ID="Financials_View" runat="server">
            <uc5:Financials ID="Financials1" runat="server" />
        </asp:View>
        <asp:View ID="Contracts_View" runat="server">
            <div class="ListGrid">
                <asp:GridView ID="gvContracts" runat="server">
                    <AlternatingRowStyle BackColor="#C7E3D7" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <a href="<%=request.applicationpath%>/marketing/editcontract.aspx?contractid=<%#container.Dataitem("ContractID")%>&ProspectID=<%=request("ProspectID")%>" title="Edit" >Edit</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <%If txtProspectID.Text <> "" and txtProspectID.Text > 0 then %><asp:Button ID="btnNewContract" runat="server" Text="New Contract" /><%End If%>
        </asp:View>
        <asp:View ID="Reservations_View" runat="server">
            <div class="ListGrid">
            <asp:GridView ID="gvReservations" runat="server" EmptyDataText = "No Reservations" AutoGenerateSelectButton = "true" onRowDataBound = "gvReservations_RowDataBound">
            <AlternatingRowStyle BackColor="#C7E3D7" />
            </asp:GridView>
            </div>
            <asp:Button ID="Button2" runat="server" Text="Add Reservation" />
        </asp:View>
        <asp:View ID="Events_View" runat="server">

            <uc7:Events ID="Events1" runat="server" />

        </asp:View>
        <asp:View ID="Notes" runat="server">
            
            <uc6:Notes ID="Notes1" runat="server" />
            <asp:Label ID="lblNotesError" runat="server" Text=""></asp:Label>
            
        </asp:View>
        <asp:View ID="Personnel" runat="server">
                <uc4:PersonnelTrans ID="PersonnelTrans1" runat="server" KeyField="ProspectID" />

        </asp:View>
        <asp:View ID="Referrals" runat="server">
            <div class="ListGrid">Referrals</div>
        </asp:View>
        <asp:View ID ="Denied" runat="server">
            ACCESS DENIED
        </asp:View>
        <asp:View ID ="Docs" runat="server">
            <uc8:UploadedDocs ID="UploadedDocs1" runat="server" />
        </asp:View>
        <asp:View ID ="CC" runat="server">
            
            <asp:GridView ID="gvCC" runat="server" EmptyDataText = "No Cards On Fild" AutoGenerateSelectButton = "true">
            <AlternatingRowStyle BackColor="#C7E3D7" />
            </asp:GridView>

            <br />
            <asp:LinkButton runat="server" ID="add_creditcard_linkbutton" Text="Add" Visible="false"></asp:LinkButton>
            <br /><br />
        </asp:View>
        <asp:View ID ="vwPoints" runat="server">
            <uc1:PointsTracking runat="server" ID="ucPoints" />
            
        </asp:View>
    </asp:MultiView>
    <ul id="menu">
        <li><asp:LinkButton ID="lbSave" runat="server">Save Prospect</asp:LinkButton></li>
    </ul>
    
    
    
   
    
    <asp:Label ID="Label9" runat="server" Text=""></asp:Label>
    
    
   
    
    <br />
    
   
</asp:Content>

