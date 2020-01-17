<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="editcontract.aspx.vb" Inherits="marketing_editcontract" title="Editing a Contract" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<%@ Register src="../controls/UserFields.ascx" tagname="UserFields" tagprefix="uc3" %>

<%@ Register src="../controls/PersonnelTrans.ascx" tagname="PersonnelTrans" tagprefix="uc4" %>

<%@ Register src="../controls/Campaign.ascx" tagname="Campaign" tagprefix="uc5" %>

<%@ Register src="../controls/Notes.ascx" tagname="Notes" tagprefix="uc6" %>

<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc7" %>

<%@ Register src="../controls/SoldInventory.ascx" tagname="SoldInventory" tagprefix="uc8" %>

<%@ Register src="../controls/UploadedDocs.ascx" tagname="UploadedDocs" tagprefix="uc9" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language=javascript type ="text/javascript">
    function Refresh_Usages()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$Usage_Link','');
    }
    function Refresh_Restrictors() {
        __doPostBack('ctl00$ContentPlaceHolder1$Usage_Restrict_Link', '');

    }
    function Refresh_Auth_Users() {
        __doPostBack('ctl00$ContentPlaceHolder1$Auth_Users_Link', '');
    }
    function Refresh_Docs() {
        __doPostBack('ctl00$ContentPlaceHolder1$UploadedDocs_Link', '');
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:LinkButton ID="lbProspect" runat="server">LinkButton</asp:LinkButton>
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Contract_Link" runat="server">Contract</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="CoOwner_Link" runat="server">Co-Owner</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Inventory_Link" runat="server">Inventory</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Mortgage_Link" runat="server">Mortgage</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 4 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Usage_Link" runat="server">Usage</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 5 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UploadedDocs_Link" runat="server">Uploaded Docs</asp:LinkButton></li>
    </ul>
    

    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 6 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 7 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Notes_Link" runat="server">Notes</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 8 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Personnel_Link" runat="server">Personnel</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 9 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UserFields_Link" runat="server">User Fields</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 10 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Usage_Restrict_Link" runat="server">Usage Restrict</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 11 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Auth_Users_Link" runat="server">Auth Users</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 12 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Conversion_Link" runat="server">Conversions</asp:LinkButton></li>
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="Contract_View" runat="server">
            <asp:TextBox ID="txtProspectID" runat="server" Visible = "False"></asp:TextBox>
            <asp:TextBox ID="txtLocationID" runat="server" Visible="False"></asp:TextBox>
            
            <table>
                <tr>
                    <td>Contract ID:</td>
                    <td>
                        <asp:TextBox ID="txtContractID" runat="server" readonly></asp:TextBox>
                    </td>
                    <td>Contract Date:</td>
                    <td>
                        <uc1:DateField ID="dfContractDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Contract #:</td>
                    <td><asp:TextBox ID="txtContractNumber" runat="server" ReadOnly ="true"></asp:TextBox></td>
                    <td>Occupancy Year:</td>
                    <td>
                        <asp:DropDownList ID="ddOccupanyYear" runat="server">
                            
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Trust:</td>
                    <td>
                        <asp:CheckBox ID="ckTrust" runat="server" />
                    </td>
                    <td>Trust Name:</td>
                    <td>
                        <asp:TextBox ID="txtTrustName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Company:</td>
                    <td>
                        <asp:CheckBox ID="ckCompany" runat="server" />
                    </td>
                    <td>Company Name:</td>
                    <td>
                        <asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Contract Type:</td>
                    <td>
                        <uc2:Select_Item ID="siContractType" runat="server" />
                    </td>
                    <td>Sub-Type:</td>
                    <td>
                        <uc2:Select_Item ID="siContractSubType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Anniversary Date:</td>
                    <td><uc1:DateField ID="dteAnniversaryDate" runat="server" /></td>
                </tr>
                <tr>
                    <td>Sale Type:</td>
                    <td>
                        <uc2:Select_Item ID="siSaleType" runat="server" />
                    </td>
                    <td>Sub-Type:</td>
                    <td>
                        <uc2:Select_Item ID="siSaleSubType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Week Type:</td>
                    <td>
                        <uc2:Select_Item ID="siWeekType" runat="server" />
                    </td>
                    <td>Season:</td>
                    <td>
                        <uc2:Select_Item ID="siSeason" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Billing Code:</td>
                    <td>
                        <uc2:Select_Item ID="siBillingCode" runat="server" />
                    </td>
                    <td>Frequency:</td>
                    <td>
                        <asp:DropDownList ID="ddFrequency" runat="server">
                        </asp:DropDownList>
                                </td>
                </tr>
                <tr>
                    <td>Status:</td>
                    <td>
                        <uc2:Select_Item ID="siStatus" runat="server" />
                    </td>
                    <td>Status Date:</td>
                    <td>
                        <asp:textBox id = "txtStatusDate" runat = "server" readonly></asp:textBox>
                    </td>
                </tr>
                <tr>
                    <td>Sub Status:</td>
                    <td>
                        <uc2:Select_Item ID="siSubStatus" runat="server" />
                    </td>
                    <td>Maintenance Fee Status:</td>
                    <td>
                        <uc2:Select_Item ID="siMaintenanceFeeStatus" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Maintenance Fee:</td>
                    <td>
                        <asp:TextBox ID="txtMaintenanceFee" runat="server"></asp:TextBox>
                    </td>
                    <td>Split MF:</td>
                    <td>
                        <asp:CheckBox ID="ckSplitMF" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Property Tax:</td>
                    <td>
                        <asp:TextBox ID="txtPropertyTax" runat="server"></asp:TextBox>
                    </td>
                    <td>Maintenance Fee Code:</td>
                    <td>
                        <asp:DropDownList ID="ddMFCode" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Tour ID:</td>
                    <td>
                        <asp:TextBox ID="txtTourID" runat="server"></asp:TextBox>
                    </td>
                    <td>Campaign:</td>
                    <td>
                        <uc5:Campaign ID="Campaign1" runat="server" />
                    </td>
                </tr>
                <tr><td><asp:Button ID="btnSave3" runat="server" Text="Save" />
                    <asp:Button ID="btnRePrint" runat="server" Text="Re-Print" />
                    </td></tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="CoOwner_View" runat="server">
            <asp:GridView ID="gvCoOwners" runat="server" EmptyDataText="No Records" OnRowCommand="Remove_CoOwner">
                <AlternatingRowStyle BackColor="#C7E3D7" />
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <a href='<%=request.applicationpath%>/marketing/editprospect.aspx?prospectid=<%#container.Dataitem("ProspectID")%>' 
                                title="Edit">Edit</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:ButtonField CommandName="Remove" Text="Remove"></asp:ButtonField>
                </Columns>
            </asp:GridView>
            <ul id="menu">
                <li><a href="#" onclick="javascript:modal.mwindow.open('<%=request.applicationpath%>/wizards/helpers/selectspouse.aspx?contractid=<%=request("ContractID") %>&linkid=<% Dim oID As String = Me.ID & "$lbRefreshCoOwner"
    Dim oTest As Object = Me.FindControl("lbRefreshCoOwner")
    While Not (oTest Is Nothing)
        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
            oID = "ctl00$" & oTest.id & "$" & oID
        End If
        oTest = oTest.parent
    End While
response.write (oID) %>','win01',690,450);">Add New</a>
                </li>
                <li><asp:LinkButton ID="lbRefreshCoOwner" runat="server">Refresh</asp:LinkButton></li>
            </ul>
            </asp:View>
        <asp:View ID="Inventory_View" runat="server">
            <uc8:SoldInventory ID="ucSoldInventory" runat="server" />
        </asp:View>
        <asp:View ID="Mortgage_View" runat="server">
            <asp:GridView ID="gvMortgage" runat="server">
                <AlternatingRowStyle BackColor="#C7E3D7" />
                <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <a href="<%=request.applicationpath%>/marketing/editmortgage.aspx?mortgageid=<%#container.Dataitem("ID")%>&ContractID=<%=request("ProspectID")%>" title="Edit" >Edit</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
            </asp:GridView>
            <asp:Button ID="btnAddMortgage" runat="server" Text="Add Mortgage" />
            <br />
        </asp:View>
        <asp:View ID="Usage_View" runat="server">
            <div style="overflow:auto; height: 247px; width: 638px;">
            <asp:GridView runat="server" id = "gvUsages" AutoGenerateSelectButton="True" OnRowDataBound = "gvUsages_RowDataBound"
                EmptyDataText="No Records" ></asp:GridView>
            </div>
            <asp:Button ID="Button1" runat="server" Text="Add Usage" />
        </asp:View>
        <asp:View ID="UploadedDocs_View" runat="server">
            <uc9:UploadedDocs ID="ucDocs" runat="server" EnableTheming="True" />
        </asp:View>
        <asp:View ID="Events_View" runat="server">
            <uc7:Events ID="ucEvents" runat="server" />
        </asp:View>
        <asp:View ID="Notes_View" runat="server">
            <uc6:Notes ID="ucNotes" runat="server" />
        </asp:View>
        <asp:View ID="Personnel_View" runat="server">
            <uc4:PersonnelTrans ID="PersonnelTrans1" runat="server" KeyField="CONTRACTID" />
        </asp:View>
        <asp:View ID="UserFields_View" runat="server">
            <uc3:UserFields ID="UF" runat="server" />
        </asp:View>
        <asp:View ID="Usage_Restrict_View" runat="server">
            <asp:GridView ID="gvUsageRestrictions" runat="server" 
                EmptyDataText="No Records" onRowDataBound = "gvUsageRestrictions_RowDataBound">
                <columns>
                <asp:ButtonField CommandName="Remove" Text="Remove"></asp:ButtonField>
                </columns>
            </asp:GridView>
            <asp:Button runat="server" Text="Add Restrictor" onclick="Unnamed1_Click1"></asp:Button>
        </asp:View>
        <asp:View ID="Auth_Users_View" runat="server">
            <asp:GridView ID="gvAuthUser" runat="server" EnableModelValidation="True" autogeneratecolumns = "false">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
                    <asp:BoundField DataField="FirstName" HeaderText="First Name"></asp:BoundField>
                    <asp:BoundField DataField="LastName" HeaderText="Last Name"></asp:BoundField>
                    <asp:ButtonField CommandName="Remove" Text="Remove"></asp:ButtonField>
                </Columns>
            </asp:GridView>
            <asp:Button runat="server" Text="Add Authorized User" onclick="Unnamed2_Click"></asp:Button>
        </asp:View>
        <asp:View runat="server" id = "Conversion_View">
            <asp:GridView runat="server" id = "gvConversions" 
                AutoGenerateSelectButton="True" EmptyDataText="No Records" ></asp:GridView>
            <asp:Button runat="server" Text="Add Conversion" onclick="Unnamed1_Click"></asp:Button>
        </asp:View>
        <asp:View runat="server" id ="DENIED">
            ACCESS DENIED
        </asp:View>
    </asp:MultiView>
    
    <asp:Label ID="lblContractError" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>

