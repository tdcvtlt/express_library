<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="redeedwiz.aspx.vb" Inherits="wizards_Contracts_redeedwiz" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="vwStep1" runat="server">
            <fieldset><legend>Insert ReDeeded Contract Number:</legend>
                KCP#: 
                <asp:TextBox ID="txtOldKCP" runat="server"></asp:TextBox>
                <asp:Button ID="btnStep1" runat="server" Text="Next" />
            </fieldset>
            <asp:Label ID="lblStep0Err" runat="server" ForeColor="Red"></asp:Label>
        </asp:View>
        <asp:View ID="vsStep2" runat="server">
            <fieldset><legend>Contract:</legend>
                <table>
                    <tr>
                        <td>Contract Number:</td>
                        <td>
                            <asp:TextBox ID="txtNewKCP" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>ReDeed Date:</td>
                        <td>
                            <uc1:DateField ID="dfReDeedDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Mortgage Title Type:</td>
                        <td>
                            <uc2:Select_Item ID="siMortgageTitleType" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>First Occupancy Year:</td>
                        <td>
                            <asp:DropDownList ID="ddOccYear" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Transfer Fee:</td>
                        <td>
                            <asp:TextBox ID="txtTransferFee" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">Deed Type:</td>
                        <td>
                            <asp:RadioButtonList ID="rblDeedType" runat="server" AutoPostBack="True">
                                <asp:ListItem>Individual</asp:ListItem>
                                <asp:ListItem>Trust</asp:ListItem>
                                <asp:ListItem>Executor</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pExecutor" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td>Executor:</td>
                            <td><asp:TextBox ID="txtExecutor" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Deceased Date:</td>
                            <td>
                                <uc1:DateField ID="dfDeceasedDate" runat="server" />
                            </td>
                        </tr>
                        
                    </table>
                </asp:Panel>
                <asp:Button ID="btnPStep1" runat="server" Text="<< Prev" />
                <asp:Button ID="btnStep2"
                    runat="server" Text="Save &amp; Next &gt;&gt;" />
            </fieldset>
            <asp:Label ID="lblStep1Err" runat="server" ForeColor="Red"></asp:Label>
        </asp:View>
        <asp:View ID="vwStep3" runat="server">
            <fieldset>
                <legend>Primary Owner</legend>
                <asp:Button ID="btnExistingOwner" runat="server" Text="Existing Owner" />
                <br />
                Last Name:
                <asp:TextBox ID="txtPOLastName" runat="server"></asp:TextBox>
                First Name:
                <asp:TextBox ID="txtPOFirstName" runat="server"></asp:TextBox>
                Middle Init:
                <asp:TextBox ID="txtPOMI" runat="server"></asp:TextBox>
                SSN:<asp:TextBox ID="txtPOSSN" runat="server"></asp:TextBox>
            </fieldset>
            <fieldset>
                <legend>Spouse (<asp:CheckBox ID="cbSpouseCoOwns" runat="server" Text="Co-Owns" />
                    )</legend>Last Name:
                <asp:TextBox ID="txtSLastName" runat="server"></asp:TextBox>
                First Name:
                <asp:TextBox ID="txtSFirstName" runat="server"></asp:TextBox>
                SSN:
                <asp:TextBox ID="txtSSSN" runat="server"></asp:TextBox>
            </fieldset>
            <fieldset>
                <legend>Demographics</legend>Email(s): <a href="#" 
                    onclick='javascript:modal.mwindow.open(&#039;<%=request.applicationpath%>/wizards/helpers/wizeditemail.aspx?Table=Email_Table&amp;EmailID=0&amp;ProspectID=<%=request("ProspectID")%>&#039;,&#039;win01&#039;,350,350);' 
                    title="Edit">Add Email</a>
                <div class="NarrowSideGrid">
                    <asp:GridView ID="gvEmail" runat="server">
                        <AlternatingRowStyle BackColor="#C7E3D7" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <a href="#" 
                                        onclick='javascript:modal.mwindow.open(&#039;<%=request.applicationpath%>/wizards/helpers/wizeditemail.aspx?Table=Email_Table&amp;EmailID=<%#container.Dataitem("ID")%>&amp;ProspectID=<%=request("ProspectID")%>&#039;,&#039;win01&#039;,350,350);' 
                                        title="Edit">Edit</a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lblEmailError" runat="server" ForeColor="Red" Text=""></asp:Label>
                </div>
                Phone Numbers: <a href="#" 
                    onclick='javascript:modal.mwindow.open(&#039;<%=request.applicationpath%>/wizards/helpers/wizeditphone.aspx?Table=Phone_Table&amp;PhoneID=0&amp;ProspectID=<%=request("ProspectID")%>&#039;,&#039;win01&#039;,350,350);' 
                    title="Edit">Add Phone Number</a>
                <br />
                <div class="SideGrid">
                    <asp:GridView ID="gvPhone" runat="server">
                        <AlternatingRowStyle BackColor="#C7E3D7" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <a href="#" 
                                        onclick='javascript:modal.mwindow.open(&#039;<%=request.applicationpath%>/wizards/helpers/wizeditphone.aspx?Table=Phone_Table&amp;PhoneID=<%#container.Dataitem("ID")%>&amp;ProspectID=<%=request("ProspectID")%>&#039;,&#039;win01&#039;,350,350);' 
                                        title="Edit">Edit</a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lblPhoneError" runat="server" ForeColor="Red" Text=""></asp:Label>
                </div>
                Addresses: <a href="#" 
                    onclick='javascript:modal.mwindow.open(&#039;<%=request.applicationpath%>/wizards/helpers/wizeditaddress.aspx?Table=Address_Table&amp;AddressID=0&amp;ProspectID=<%=request("ProspectID")%>&#039;,&#039;win01&#039;,350,350);' 
                    title="Edit">Add Address</a>
                <br />
                <div class="SideGrid">
                    <asp:GridView ID="gvAddress" runat="server">
                        <AlternatingRowStyle BackColor="#C7E3D7" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <a href="#" onclick='javascript:modal.mwindow.open(&#039;<%=request.applicationpath%>/wizards/helpers/wizeditaddress.aspx?Table=Address_Table&amp;AddressID=<%#container.Dataitem("ID")%>&amp;ProspectID=<%=request("ProspectID")%>&amp;linkid=<% 
    Dim oID As String = "$lbRefresh"
    Dim oTest As Object = Me.FindControl("lbRefresh")
    While Not (oTest Is Nothing)
        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
            oID = "ctl00$" & oTest.id & "$" & oID
        End If
        oTest = oTest.parent
    End While
    response.write (oID)
 %>&#039;,&#039;win01&#039;,350,350);' title="Edit">Edit</a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblAddressError" runat="server" ForeColor="Red" Text=""></asp:Label>
                </div>
            </fieldset>
            <fieldset>
                <legend>Contract Type</legend>
                <asp:RadioButton ID="rbMultiOwner" runat="server" AutoPostBack="True" 
                    GroupName="ContractType" Text="Multi-Owner" />
                <asp:RadioButton ID="rbTrust" runat="server" AutoPostBack="True" 
                    GroupName="ContractType" Text="Trust" />
                <asp:RadioButton ID="rbCompanyName" runat="server" AutoPostBack="True" 
                    GroupName="ContractType" Text="Company Name" />
                <asp:RadioButton ID="rbNone" runat="server" AutoPostBack="True" 
                    GroupName="ContractType" Text="None" />
            </fieldset>
            
            <asp:MultiView ID="mvContractType" runat="server">
                <asp:View ID="vwMultiOwner" runat="server">
                    <asp:GridView ID="gvMultiOwner" runat="server">
                        <AlternatingRowStyle BackColor="#C7E3D7" />

                    </asp:GridView>
                    <asp:Button ID="btnAddCo" runat="server" Text="Add Co-Owner" 
                        style="height: 26px" />
                </asp:View>
                <asp:View ID="vwTrust" runat="server">
                    Trust Name:
                    <asp:TextBox ID="txtTrust" runat="server"></asp:TextBox>
                </asp:View>
                <asp:View ID="vwCompany" runat="server">
                    Company Name:<asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>
                </asp:View>
                <asp:View ID="vwNone" runat="server">
                </asp:View>
            </asp:MultiView>
            <asp:Button runat="server" Text="<< Previous" id = "Prev2" 
                style="height: 26px"></asp:Button><asp:Button runat="server" Text="Save & Print" ID = "btnStep3Save"></asp:Button><asp:Button runat="server" Text="Save & Next >>" onclick="Unnamed1_Click"></asp:Button>
            <asp:TextBox ID="txtName" runat="server" Visible="False"></asp:TextBox>
            <asp:Label ID="lblStep2Err" runat="server" ForeColor="Red"></asp:Label>
        </asp:View>
        <asp:View ID="vwStep4" runat="server">
            <fieldset><legend>Transfer Information:</legend>
                <asp:GridView ID="gvInvoices" AutoGenerateColumns="true" runat="server" EmptyDataText="No Records">
                    <AlternatingRowStyle BackColor="#C7E3D7" />
                    <Columns>
                         <asp:TemplateField HeaderText="Transfer">
                            <ItemTemplate>
                                <asp:checkbox ID="chkTransfer" runat = "server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button ID="btnPrev3" runat="server" Text="<< Previous" /><asp:Button ID="btnSavePrint" runat="server" Text="Finish" />
               <br /><asp:Label runat="server" 
                    Text="*Pressing Finish will complete the Redeed Process by transferring inventory and personnel trans."></asp:Label>
            </fieldset>
        </asp:View>
    </asp:MultiView>
    <asp:HiddenField ID="hfStep" Value = "0" runat="server" />
    <asp:HiddenField ID="hfReDeedID" Value = "0" runat="server" />
    <asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton>
</asp:Content>

