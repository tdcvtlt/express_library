<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditPremium.aspx.vb" Inherits="marketing_EditPremium" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label8"   runat="server" text=""></asp:Label>
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Premium" runat="server">Premium</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events" runat="server">Events</asp:LinkButton></li>
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="vwPremium" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="ID:"></asp:Label>
                
                    </td>
                    <td>
                        <asp:TextBox ID="txtPremiumID" runat="server" ReadOnly="True" style="text-align: right">0</asp:TextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label2" runat="server" Text="Name:"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtPremiumName" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                            ControlToValidate="txtPremiumName" Display="Dynamic" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                    </td>
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
                </tr>
                <tr>
                    <td><asp:Label ID="Label4" runat="server" Text="Cost:"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtCost"  runat="server" style="text-align: right"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtCost" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" 
                            ControlToValidate="txtCost" ErrorMessage="CompareValidator" 
                            Operator="GreaterThan" Type="Currency" ValueToCompare="-.01" 
                            Display="Dynamic">* Invalid value</asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label5" runat="server" Text="CB Cost:"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtCBCost" runat="server" style="text-align: right"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ControlToValidate="txtCBCost" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator2" runat="server" 
                            ControlToValidate="txtCBCost" ErrorMessage="CompareValidator" 
                            Operator="GreaterThan" Type="Currency" ValueToCompare="-.01" 
                            Display="Dynamic">* Invalid value</asp:CompareValidator></td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label6" runat="server" Text="QTY On Hand:"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtQtyOnHand" runat="server" style="text-align: right"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="txtQtyOnHand" ErrorMessage="* Required" 
                            Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator3" runat="server" 
                            ControlToValidate="txtQtyOnHand" ErrorMessage="CompareValidator" 
                            Operator="GreaterThan" Type="Integer" ValueToCompare="-99999" 
                            Display="Dynamic">* Invalid value</asp:CompareValidator></td></td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label7" runat="server" Text="Type:"></asp:Label></td>
                    <td>
                        <uc1:Select_Item ID="siType" runat="server" />
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
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSave" runat="server" Text="Save" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            
        </asp:View>
        <asp:View ID="vwEvents" runat="server">
            <uc2:Events ID="Events1" runat="server" />
        </asp:View>
    </asp:MultiView>
</asp:Content>

