<%@ Page Title="Contract Wizard" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="contractwizard.aspx.vb" Inherits="wizards_Contracts_contractwizard" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<%@ Register src="../../controls/Campaign.ascx" tagname="Campaign" tagprefix="uc3" %>

<%@ Register src="../../controls/Financials.ascx" tagname="Financials" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        function increment_Step()
        {
            __doPostBack('ctl00$ContentPlaceHolder1$lbIncStep', '')
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 174px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:HiddenField ID="hfStep" Value = "1" runat="server" />
    <asp:HiddenField ID="hfTourID" Value = "0" runat="server" />
    <asp:HiddenField ID="hfProspectID" Value = "0" runat="server" />
    <asp:HiddenField ID="hfContractID" Value = "0" runat="server" />
    <asp:HiddenField ID="hfMortgageID" Value = "0" runat="server" />
    <asp:HiddenField ID="hfOldContractID" Value = "0" runat="server" />
    <asp:HiddenField ID="hfOldMortgageID" Value = "0" runat="server" />
    <asp:LinkButton ID="lbIncStep" runat="server"></asp:LinkButton>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="vwWizardType" runat="server">
            <asp:RadioButton ID="rbNew" runat="server" GroupName="KCP" Text="New Contract" 
                Checked="True" />
            <asp:RadioButton ID="rbUD" runat="server" Text="Upgrade/Downgrade"  GroupName="KCP"/>
            <asp:RadioButton ID="rbCon" runat="server" Text="Conversion"  GroupName="KCP"/>
            <asp:RadioButton ID="rbRe" runat="server" Text="Re-Deed"  GroupName="KCP"/>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:MultiView ID="mvStep2" runat="server">
                <asp:View ID="vwStep2TourSelect" runat="server">
                    Tour ID:
                    <asp:TextBox ID="txtTourID" runat="server"></asp:TextBox>
                    <asp:LinkButton ID="LinkButton1" runat="server"><a href="javascript:modal.mwindow.open('<%=Request.ApplicationPath%>/wizards/contracts/selecttour.aspx','win01',350,350);">Lookup Tour</a></asp:LinkButton>
                    <br /><br />
                    <asp:RadioButton ID="rbNewContract" runat="server" GroupName="KCP" Text="New Contract" Checked="True" autopostback="true"/>
                    <asp:RadioButton ID="rbUpgrade" runat="server" Text="Upgrade"  GroupName="KCP" autopostback="true"/>
                    <asp:RadioButton ID="rbDownGrade" runat="server" Text="Downgrade"  GroupName="KCP" autopostback="true"/>
                    <br /><br />
                    <asp:Label ID="lbOld" runat="server" Text="Old Contract Number:" Visible ="false"></asp:Label>
                    <asp:TextBox ID="txtOldContract" runat="server" Visible ="false"></asp:TextBox>
                </asp:View>
                <asp:View ID="vwStep2ContractSelect" runat="server">
                    Contract Number: <asp:TextBox ID="txtOldKCP" runat="server"></asp:TextBox>
                </asp:View>
                <asp:View ID="vwStep2Redeed" runat="server">
                    Current Contract Number: <asp:TextBox ID="txtRedeedOld" runat="server"></asp:TextBox><br />
                    New Owner ID: <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                </asp:View>
            </asp:MultiView>
        </asp:View>
        <asp:View ID="vwStep3" runat="server">
            
                    <fieldset><legend>Primary Owner</legend>
                    Status: <br />
                        Last Name: <asp:TextBox ID="txtPOLastName" runat="server"></asp:TextBox>
                        First Name: <asp:TextBox ID="txtPOFirstName" runat="server"></asp:TextBox>
                        Middle Init: <asp:TextBox ID="txtPOMI" runat="server" MaxLength="1"></asp:TextBox>
                        SSN:<asp:TextBox ID="txtPOSSN" runat="server"></asp:TextBox>
                    </fieldset>
                    <fieldset><legend>Spouse (<asp:CheckBox ID="cbSpouseCoOwns" runat="server" Text="Co-Owns" />)</legend>
                        Last Name: <asp:TextBox ID="txtSLastName" runat="server"></asp:TextBox> 
                        First Name: <asp:TextBox ID="txtSFirstName" runat="server"></asp:TextBox>
                        SSN: <asp:TextBox ID="txtSSSN" runat="server"></asp:TextBox>
                    </fieldset>
                    <fieldset><legend>Demographics</legend>
                        Email(s): <a  href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=Request.ApplicationPath%>/wizards/helpers/wizeditemail.aspx?Table=Email_Table&EmailID=0&ProspectID=<%=Request("ProspectID")%>','win01',350,350);">Add Email</a>
                        
                        <div class="NarrowSideGrid">
                            <asp:GridView ID="gvEmail" runat="server">
                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=Request.ApplicationPath%>/wizards/helpers/wizeditemail.aspx?Table=Email_Table&EmailID=<%#Container.DataItem("ID")%>&ProspectID=<%=Request("ProspectID")%>','win01',350,350);">Edit</a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                            </asp:GridView>
                        <asp:Label ID="lblEmailError" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </div>
                        Phone Numbers:
                                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=Request.ApplicationPath%>/wizards/helpers/wizeditphone.aspx?Table=Phone_Table&PhoneID=0&ProspectID=<%=Request("ProspectID")%>','win01',350,350);">Add Phone Number</a>
                                <br />
                                <div class="SideGrid">
                                    <asp:GridView ID="gvPhone" runat="server">
                                        <AlternatingRowStyle BackColor="#C7E3D7" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=Request.ApplicationPath%>/wizards/helpers/wizeditphone.aspx?Table=Phone_Table&PhoneID=<%#Container.DataItem("ID")%>&ProspectID=<%=Request("ProspectID")%>','win01',350,350);">Edit</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="lblPhoneError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    
                                    </div>
                                    Addresses: 
                                    <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=Request.ApplicationPath%>/wizards/helpers/wizeditaddress.aspx?Table=Address_Table&AddressID=0&ProspectID=<%=Request("ProspectID")%>','win01',350,350);">Add Address</a>
                                    <br />
                                    <div class="SideGrid">
                                        <asp:GridView ID="gvAddress" runat="server">
                                        <AlternatingRowStyle BackColor="#C7E3D7" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=Request.ApplicationPath%>/wizards/helpers/wizeditaddress.aspx?Table=Address_Table&AddressID=<%#Container.DataItem("ID")%>&ProspectID=<%=Request("ProspectID")%>&linkid=<% 
                                                        Dim oID As String = "$lbRefresh"
                                                        Dim oTest As Object = Me.FindControl("lbRefresh")
                                                        While Not (oTest Is Nothing)
                                                            If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
                                                                oID = "ctl00$" & oTest.id & "$" & oID
                                                            End If
                                                            oTest = oTest.parent
                                                        End While
                                                        Response.Write(oID)
 %>','win01',350,350);">Edit</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView><br />
                                    <asp:Label ID="lblAddressError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </div>
                    </fieldset>
                <fieldset>
                    <legend>Contract Type</legend>
                    <asp:RadioButton ID="rbMultiOwner" runat="server" Text="Multi-Owner" 
                        GroupName="ContractType" AutoPostBack="True"  />
                    <asp:RadioButton ID="rbTrust" runat="server" Text="Trust" 
                        GroupName="ContractType" AutoPostBack="True"  />
                    <asp:RadioButton ID="rbCompanyName" runat="server" Text="Company Name" 
                        GroupName="ContractType" AutoPostBack="True"  />
                    <asp:RadioButton ID="rbNone" runat="server" Text="None" 
                        GroupName="ContractType" AutoPostBack="True" />
                </fieldset>
            <asp:MultiView ID="mvContractType" runat="server">
                <asp:View ID="vwMultiOwner" runat="server">
                    <asp:GridView ID="gvMultiOwner" runat="server">
                        <AlternatingRowStyle BackColor="#C7E3D7" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=Request.ApplicationPath%>/wizards/helpers/selectspouse.aspx?isWiz=1&ID=<%#Container.DataItem("ID")%>','win01',550,350);">Edit</a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Button ID="btnAddCo" runat="server" Text="Add Co-Owner" />
                </asp:View>
                <asp:View ID="vwTrust" runat="server">
                    Trust Name: <asp:TextBox ID="txtTrust" runat="server"></asp:TextBox>
                </asp:View>
                <asp:View ID="vwCompany" runat="server">
                    Company Name:<asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>
                </asp:View>
            </asp:MultiView>
                <asp:TextBox ID="txtName" runat="server" Visible="False"></asp:TextBox>
        </asp:View>
        <asp:View ID="vwStep4" runat="server">
            
            <table class="logo">
                <tr>
                    <td>
                        Contract Number:</td>
                    <td>
                        <asp:TextBox ID="txtContractNumber" runat="server" ReadOnly ="true"></asp:TextBox>
                    </td>
                    <td>
                        TourID:</td>
                    <td>
                        <asp:TextBox ID="txtStep4TourID" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Date:</td>
                    <td>
                        <uc1:DateField ID="dtContractDate" runat="server" />
                    </td>
                    <td>
                        First Occupancy:</td>
                    <td>
                        <asp:DropDownList ID="ddOccupancy" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Sales Type:</td>
                    <td>
                        <uc2:Select_Item ID="siSalesType" runat="server" ComboItem="ContractSaleType" 
                             />
                    </td>
                    <td>
                        Sales Sub-Type:</td>
                    <td>
                        <uc2:Select_Item ID="siSalesSubType" runat="server" ComboItem="ContractSaleSubType" 
                            /></td>
                </tr>
                <tr>
                    <td>
                        Contract Type:</td>
                    <td>
                        <uc2:Select_Item ID="siContractType" runat="server" ComboItem="ContractType" 
                            /></td>
                    <td>
                        Contract Sub-Type:</td>
                    <td>
                        <uc2:Select_Item ID="siContractSubType" runat="server" 
                            ComboItem="ContractSubType"  /></td>
                </tr>
                <tr>
                    <td>
                        Season:</td>
                    <td>
                        <uc2:Select_Item ID="siSeason" runat="server" ComboItem="Season" 
                             /></td>
                    <td>
                        Status:</td>
                    <td>
                        <uc2:Select_Item ID="siStatus" runat="server" ComboItem="ContractStatus" 
                             /></td>
                </tr>
                <tr>
                    <td>
                        Week Type:</td>
                    <td>
                        <uc2:Select_Item ID="siWeekType" runat="server" ComboItem="WeekType" 
                            /></td>
                    <td>
                        Billing Code:</td>
                    <td>
                        <uc2:Select_Item ID="siBillingCode" runat="server" ComboItem="BillingCode" 
                             /></td>
                </tr>
                <tr>
                    <td>
                        Frequency:</td>
                    <td>
                        <asp:DropDownList ID="ddFrequency" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Campaign:</td>
                    <td>
                        <uc3:Campaign ID="Campaign1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Status Change Date:</td>
                    <td>
                        <asp:TextBox ID="txtStatusDate" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        Maintenance Fee:</td>
                    <td>
                        <asp:TextBox ID="txtMaintenanceFee" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Sub Status:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddSubStatus" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                     <td>Upgrade Option: <asp:CheckBox ID="cbUpgradeOption" runat="server" AutoPostBack="True" /></td>
                     <td><asp:Label ID="lblSeason" runat="server" Text="Season:" Visible ="false"></asp:Label><asp:DropDownList ID="ddUGSeason" runat="server" Visible ="false"></asp:DropDownList></td>
                     <td><asp:Label ID="lblBD" runat="server" Text="BD:" Visible ="false" /><asp:DropDownList ID="ddUGBD" runat="server" Visible ="false"></asp:DropDownList><asp:Label ID="lblUT" runat="server" Text="Unit Type:" Visible ="false" /><asp:DropDownList ID="ddUGUT" runat="server" Visible ="false"></asp:DropDownList></td>
                     <td><asp:Label ID="lblPrice" runat="server" Text="Upgrade Price:" Visible ="false" /><asp:TextBox ID="txtUGPrice" runat="server" Visible ="false"></asp:TextBox></td>
                </tr>
            </table>
            <fieldset><legend>Assigned Inventory</legend>
                <asp:GridView ID="gvInventory" runat="server" AutoGenerateColumns="true" AutoGenerateDeleteButton="true">
                    <AlternatingRowStyle BackColor="#C7E3D7" />
                </asp:GridView>
                <asp:LinkButton ID="lbAddInv" runat="server"><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/availableinventory.aspx?FrequencyID=' + document.forms[0].ctl00$ContentPlaceHolder1$ddFrequency.options[document.forms[0].ctl00$ContentPlaceHolder1$ddFrequency.selectedIndex].value + '&SeasonID=' + document.forms[0].ctl00$ContentPlaceHolder1$siSeason$DropDownList1.options[document.forms[0].ctl00$ContentPlaceHolder1$siSeason$DropDownList1.selectedIndex].value + '&OccYear=' + document.forms[0].ctl00_ContentPlaceHolder1_ddOccupancy.options[document.forms[0].ctl00_ContentPlaceHolder1_ddOccupancy.selectedIndex].value,'win01',350,350);">Add Inventory</a></asp:LinkButton>
            </fieldset>
        </asp:View>
        <asp:View ID="vwStep5" runat="server">
            <asp:GridView ID="gvCommissions" runat="server" AutoGenerateColumns="true" autogeneratedeletebutton = "True">
                <AlternatingRowStyle BackColor="#C7E3D7" />
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/wizards/contracts/editpersonneltrans.aspx?ID=<%#container.Dataitem("ID")%>','win01',350,350);">Edit</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:LinkButton ID="LinkButton2" runat="server"><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/wizards/contracts/editpersonneltrans.aspx?ID=0','win01',350,350);">Add Personnel</a></asp:LinkButton>
        </asp:View>
        <asp:View ID="vwStep6" runat="server">
           
                <table class="style1">
                    <tr>
                        <td class="style3">
                            Sales Volume:</td>
                        <td class="style4">
                            <asp:TextBox ID="txtSalesVolume" runat="server" CssClass="text_box_currency"></asp:TextBox>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbDP_Details" runat="server">D.P. Details &gt;&gt;</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Commission Volume:</td>
                        <td class="style4">
                            <asp:TextBox ID="txtCommissionVolume" runat="server" 
                                CssClass="text_box_currency"></asp:TextBox>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbCC_BreakOut" runat="server">C.C. Breakout &gt;&gt;</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Sales Price:</td>
                        <td class="style4">
                            <asp:TextBox ID="txtSalesPrice" runat="server" CssClass="text_box_currency" AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbFinanceCC" runat="server" Text="Finance Closing Costs" autopostback = "true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            Original Purchase Price:</td>
                        <td class="style4">
                            <asp:TextBox ID="txtOrigPurchasePrice" runat="server" 
                                CssClass="text_box_currency" AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbUpdate_Costs" runat="server">Update Costs</asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <uc4:Financials ID="DP_Financials" runat="server" />
        </asp:View>
        <asp:View ID="vwStep7" runat="server">
            <table>
                <tr>
                    <td>Deed Type:</td>
                    <td>
                        <uc2:Select_Item ID="siDeedType" ComboItem="MortgageTitleType" Label_Caption="" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Interest Type:</td>
                    <td>
                        <uc2:Select_Item ID="siInterestType" ComboItem="MortgageInterestType" Label_Caption="" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Payment Type:</td>
                    <td>
                        <uc2:Select_Item ID="siPaymentType" ComboItem="MortPmtType" Label_Caption="" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Total Financed:</td>
                    <td>
                        <asp:TextBox ID="txtTotalFinanced" text = "0" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>APR:</td>
                    <td>
                        <asp:TextBox ID="txtAPR" Text="17.9" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Term:</td>
                    <td>
                        <asp:TextBox ID="txtTerm" Text="84" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Frequency:</td>
                    <td>
                        <asp:DropDownList ID="ddMortgageFrequency" runat="server">
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Payment Processing Fee:</td>
                    <td><asp:TextBox runat="server" id = "txtProcFee">6.00</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Origination Date:</td>
                    <td>
                        <uc1:DateField ID="dtOriginationDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>First Payment Due Date:</td>
                    <td><uc1:DateField ID="dtFirstPaymentDate" runat="server" /></td>
                </tr>
                
            </table>
        </asp:View>
        <asp:View ID="DNSCheck" runat="server">

        </asp:View>
    </asp:MultiView>
    
    <p align="left"><asp:Button ID="btnPrev" runat="server" Text="<< Previous" /><asp:Button ID="btnNext" runat="server" Text="Next >>" /></p>
    <asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton>
</asp:Content>

