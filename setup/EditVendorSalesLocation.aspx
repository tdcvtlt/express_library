<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditVendorSalesLocation.aspx.vb" Inherits="setup_EditVendorSalesLocation" EnableEventValidation="true" %>
<%@ Register Src="~/controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
     <ul id="menu">
        <li <%if MultiView_Main.ActiveViewIndex = 0 Then : Response.Write("class=""current""") : End If %>><asp:LinkButton ID="Lk_Sales_Location" runat="server">Sales Locations</asp:LinkButton></li>
        <li <%if  MultiView_Main.ActiveViewIndex = 1 Then : Response.Write("class=""current""") : End If %>><asp:LinkButton ID="Lk_Sales_Location_Edit" runat="server">Location Detail</asp:LinkButton></li>
    </ul>

    <asp:MultiView ID="MultiView_Main" runat="server">
        <asp:View ID="View_Sales_Locations" runat="server">
            

            <table>
                <tr>
                    <td style="width:120px;">Tradeshow Vendors</td>
                    <td>
                        <asp:DropDownList runat="server" ID="Ddl_Sales_Location" style="width:100px;"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="bt_New_Sales_Location" Text="Create New Sales Location" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button runat="server" ID="bt_List_Sales_Locations" Text="Search" style="width:100px;" />
                    </td>
                </tr>
            </table>

            <div>
                <asp:GridView runat="server" ID="Gv_Sales_Location" DataKeyNames="SalesLocationID" AutoGenerateSelectButton="true"></asp:GridView>
            </div>
        </asp:View>
        <asp:View ID="View_Sales_Locations_Edit" runat="server">
            <asp:HiddenField runat="server" ID="hf_SalesLocationID" />
            <asp:HiddenField runat="server" ID="hf_VendorID" />

            <table style="margin-top:60px;">
                <tr>
                    <td>Location</td>
                    <td><asp:TextBox runat="server" ID="tb_Location"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Date Created</td>
                    <td>
                        <uc1:DateField runat="server" ID="df_DateCreated" />                        
                    </td>
                </tr>
                <tr>
                    <td>Date De-Activated</td>
                    <td>
                        <uc1:DateField runat="server" ID="df_DateDeActivated" />
                    </td>
                </tr>
                <tr>
                    <td>VRC Cost</td>
                    <td>
                        <asp:TextBox runat="server" ID="tb_VRCCost"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Active</td>
                    <td>
                        <asp:CheckBox runat="server" ID="cb_Active" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                    <tr>
                    <td></td>
                    <td><asp:Button runat="server" ID="bt_Cancel_Sales_Location" Text="Cancel" Style="width:100%" /></td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Button runat="server" ID="bt_Save_Sales_Location" Text="Save" Style="width:100%" /></td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
    

</asp:Content>

