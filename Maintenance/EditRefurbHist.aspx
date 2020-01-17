<%@ Page Title="Refurb History" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditRefurbHist.aspx.vb" Inherits="Maintenance_EditRefurbHist" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

<%@ Register Src="~/controls/Select_Item.ascx" TagPrefix="uc1" TagName="Select_Item" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        /* Table formatting */
        table .header
        {
            background-color: navy;
            color: White;
            font-size: 90%;
        }

        .group
        {
            background-color: #B51032;
            color: White;
            font-weight: bold;
            font-style: italic;
        }

        .data0
        {
            font-size: 80%;
        }

        .data1
        {
            font-size: 80%;
            background-color: #fcc;
        }

        .centerAlign
        {
            text-align: center;
        }

        .rightAlign
        {
            text-align: right;
        }

        #footer
        {
            margin-top: 66px;
            padding-top: 10px;
            clear: both;
            text-align: center;
            font-size: 75%;
            color: #555;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Refurb Hist ID:</td>
            <td><asp:Label ID="lblRefurbHistID" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td>Refurb Name:</td>
            <td><asp:Label ID="lblRefurbName" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td>Created By:</td>
            <td><asp:Label ID="lblCreatedBy" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td>Date Created:</td>
            <td><asp:Label ID="lblDateCreated" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td>Status:</td>
            <td><uc1:Select_Item runat="server" ID="siStatus" /></td>
        </tr>
        <tr>
            <td>Status Date:</td>
            <td><asp:Label ID="lblStatusDate" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td>Last Updated By:</td>
            <td><asp:Label ID="lblUpdatedBy" runat="server"></asp:Label></td>
        </tr>
    </table>
    
        <asp:ListView ID="lvItems" runat="server">
            <LayoutTemplate>
                <table border="1">
                    
                    <tbody>
                         <tr runat="server" id="itemPlaceholder" />
                    </tbody>
               </table>
            </LayoutTemplate>
            <ItemTemplate>
                <%# AddGroupingRowIfSupplierHasChanged() %>
                <tr class='data<%#Container.DataItemIndex Mod 2 %>'>
                    <td id="Td3" runat="server">
                        <asp:CheckBox ID="Checkbox" runat="server"/>
                    </td>
                    <td id="Td1" runat="server">
                        <asp:Label ID="IdLabel" runat="server" Text='<%#Eval("RefurbHist2Item") %>' />
                    </td>
                    <%--<td id="Td2" runat="server">
                        <asp:Label ID="NameLabel" runat="server" Text='<%#Eval("Area") %>' />
                    </td>--%>
                    <td id="Td4" runat="server">
                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("Description") %>' />
                    </td>
                    <td id="Td5" runat="server">
                        <asp:Label ID="Label2" runat="server" Text='<%#Eval("CheckedBy") %>' />
                    </td>
                    <td id="Td6" runat="server">
                        <asp:Label ID="Label3" runat="server" Text='<%#Eval("DateChecked") %>' />
                    </td>
                    
                </tr>
            </ItemTemplate>
        </asp:ListView>
    <div>
        <asp:Button ID="btnComplete" runat="server" Text="Update" />
    </div>
</asp:Content>

