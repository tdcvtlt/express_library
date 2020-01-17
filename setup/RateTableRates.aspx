<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="RateTableRates.aspx.vb" Inherits="setup_RateTableRates" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id = "menu">
    <li <% if MultiView1.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonView" runat="server">View Rates</asp:LinkButton>
    </li>    
    
    <li <% if MultiView1.ActiveViewIndex = 1 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonBuild" runat="server">Build Rates</asp:LinkButton>        
    </li>    
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server">
            <table>
            <tr>
                <td>RateTable:</td>
                <td>
                    <asp:DropDownList ID="ddRateTable" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Start Date:</td>
                <td><uc1:DateField ID="dteStartDate" runat="server" /></td>
            </tr>
            <tr>
                <td>End Date:</td>
                <td><uc1:DateField ID="dteEndDate" runat="server" /></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnSearch" runat="server" Text="Search" /></td>
            </tr>
            </table>
            <div style="height:400px;width:830px; overflow:auto; ">
                <asp:GridView ID="gvRates" runat="server" EmptyDataText="No Records" GridLines="Horizontal" 
                    AutoGenerateColumns="true" AutoGenerateSelectButton="true" BorderStyle="None" OnRowDataBound = "gvRates_RowDataBound">
                    <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                    <AlternatingRowStyle BackColor="#CCFFCC" />
            </asp:GridView> 
            </div>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <table>
                <tr>
                    <td>RateTable:</td>
                    <td><asp:DropDownList ID="ddRateTables" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Start Date:</td>
                    <td>
                        <uc1:DateField ID="dteSDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>End Date:</td>
                    <td>
                        <uc1:DateField ID="dteEDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>WeekDay Rate:</td>
                    <td>
                        <asp:TextBox ID="txtWeekdayRate" runat="server">0</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>WeekDay Rental Rate:</td>
                    <td>
                        <asp:TextBox ID="txtWeekDayRental" runat="server">0</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>WeekDay Owner Rate:</td>
                    <td><asp:TextBox ID="txtWeekDayOwner" runat="server">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>WeekDay Tour Package Rate:</td>
                    <td><asp:TextBox ID="txtWeekDayTP" runat="server">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>WeekDay Trade Show Rate:</td>
                    <td><asp:TextBox ID="txtWeekDayTS" runat="server">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>WeekDay Cost:</td>
                    <td>
                        <asp:TextBox ID="txtWeekdayCost" runat="server">0</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>WeekEnd Rate:</td>
                    <td>
                        <asp:TextBox ID="txtWeekendRate" runat="server">0</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>WeekEnd Rental Rate:</td>
                    <td>
                        <asp:TextBox ID="txtWeekendRental" runat="server">0</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>WeekEnd Owner Rate:</td>
                    <td><asp:TextBox ID="txtWeekEndOwner" runat="server">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>WeekEnd Tour Package Rate:</td>
                    <td><asp:TextBox ID="txtWeekEndTP" runat="server">0</asp:TextBox></td>
                </tr>
                                <tr>
                    <td>WeekEnd Trade Show Rate:</td>
                    <td>
                        <asp:TextBox ID="txtWeekEndTS" runat="server">0</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>WeekEnd Cost:</td>
                    <td>
                        <asp:TextBox ID="txtWeekendCost" runat="server">0</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Build Rates" />
                    </td>
                </tr>
            </table>
            <asp:Literal ID="litResult" runat="server"></asp:Literal>
        </asp:View>
    </asp:MultiView>
</asp:Content>

