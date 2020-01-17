<%@ Page Title="Edit Campaign" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditCampaign.aspx.vb" Inherits="marketing_EditCampaign" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc2" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language=javascript type ="text/javascript">
    function Refresh_Departments()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$Departments','');
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label8"   runat="server" text=""></asp:Label>
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Campaign" runat="server">Campaign</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events" runat="server">Events</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 2 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Departments" runat="server">Departments</asp:LinkButton></li>
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="vwPremium" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="ID:"></asp:Label>
                
                    </td>
                    <td>
                        <asp:TextBox ID="txtCampaignID" runat="server" ReadOnly="True" style="text-align: right">0</asp:TextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="Label10" runat="server" Text="Type:"></asp:Label>
                    </td>
                    <td>
                        <uc1:Select_Item ID="siType" runat="server" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label2" runat="server" Text="Name:"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                            ControlToValidate="txtName" Display="Dynamic" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="Label11" runat="server" Text="SubType:"></asp:Label>
                    </td>
                    <td>
                        <uc1:Select_Item ID="siSubType" runat="server" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label3" runat="server" Text="Description:"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                            ControlToValidate="txtDescription" Display="Dynamic" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="Label12" runat="server" Text="Max Cost Per Tour:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxCostPerTour" runat="server" style="text-align: right"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtMaxCostPerTour" Display="Dynamic" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" 
                            ControlToValidate="txtMaxCostPerTour" Display="Dynamic" ErrorMessage="CompareValidator" 
                            Operator="GreaterThan" Type="Currency" ValueToCompare="-.01">* Invalid value</asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label4" runat="server" Text="Start Date:"></asp:Label></td>
                    <td>
                        <uc3:DateField ID="dfStartDate" runat="server" />
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="Label13" runat="server" Text="End Date:"></asp:Label>
                    </td>
                    <td>
                        <uc3:DateField ID="dfEndDate" runat="server" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label5" runat="server" Text="Promo Nights:"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtPromoNights" runat="server" style="text-align: right"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="Label14" runat="server" Text="Promo Rate:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPromoRate" runat="server" style="text-align: right"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                
                <tr>
                    <td><asp:Label ID="Label7" runat="server" Text="Additional Night Rate:"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtAdditionNightRate" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="Label16" runat="server" Text="Additional Guest Rate:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAdditionalGuestRate" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <!--<td><asp:Label ID="Label6" runat="server" Text="Department:"></asp:Label></td>
                    <td>
                        <uc1:Select_Item ID="siDepartment" runat="server" />
                    </td>
                    <td>
                        &nbsp;</td>-->
                    <td>
                        <asp:Label ID="Label15" runat="server" Text="Department Program:"></asp:Label>
                    </td>
                    <td>
                        <uc1:Select_Item ID="siDepartmentProgram" runat="server" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
        
                <tr>
                    <td><asp:Label ID="Label9" runat="server" Text="Active:"></asp:Label></td>
                    <td>
                        <asp:CheckBox ID="ckActive" runat="server" />
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="Label17" runat="server" Text="Account:"></asp:Label>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSave" runat="server" Text="Save" style="height: 29px" />
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            
        </asp:View>
        <asp:View ID="vwEvents" runat="server">
            <uc2:Events ID="Events1" runat="server" />
        </asp:View>
        <asp:View ID="View1" runat="server">
            <asp:GridView ID="gvDept" runat="server" AutoGenerateSelectButton="true"></asp:GridView>
            <ul id="menu">
                <li><asp:LinkButton ID="lbAdd" runat="server">Add</asp:LinkButton></li>
            </ul>
        </asp:View>

    </asp:MultiView>

</asp:Content>

