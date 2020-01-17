<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ResortOverview.aspx.vb" Inherits="frontdesk_ResortOverview" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr><td>
            <table>
                <tr>
                    <td>Room Type:</td>
                    <td><uc1:Select_Item ID="siRoomType" runat="server" /></td>
                </tr>
                <tr>
                    <td>Room Sub-Type:</td>
                    <td><uc1:Select_Item ID="siRoomSubType" runat="server" /></td>
                </tr>
                <tr>
                    <td>Status:</td>
                    <td><asp:DropDownList ID="ddStatus" runat="server">
                        <asp:ListItem Value="" Text="(empty)"></asp:ListItem>
                        <asp:ListItem Value="#FF8C00;" Text="Occupied"></asp:ListItem>
                        <asp:ListItem Value="#FF0000;" Text="Offline"></asp:ListItem>
                        <asp:ListItem Value="#00FF00;" Text="Room Ready"></asp:ListItem>
                        <asp:ListItem Value="#FFFF00;" Text="Room Not Ready"></asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Show Todays Check-Ins only:</td>
                    <td><asp:CheckBox ID ="ckToday" runat="server" /></td>
                </tr>
                <tr>
                    <td colspan="2">Auto Update Interval: <asp:TextBox ID="txtUpdate" runat="server" Width="36px">5</asp:TextBox> Seconds</td>
                </tr>
            </table>    
        </td><td valign = "top">
            <table>
                <tr>
                    <td colspan="2">Legend:</td>
                </tr>   
                <tr>
                    <td style='background-color:#00FF00;'>Room Ready</td>
                </tr>   
                <tr>
                    <td style='background-color:#FFFF00;'>Room Not Ready</td>
                </tr>   
                <tr>
                    <td style='background-color:#FF0000;'>Offline</td>
                </tr>
                <tr>
                    <td style="background-color:#FF8C00;">Occupied</td>
                </tr>

                <%--<tr>
                    
                    <td style='background-color:#00FFFF;'>Dirty</td>
                </tr>
                <tr>
                    <td style='background-color:#F0AAFA;'>Being Cleaned</td>
                    <td style='background-color:#AAFFAA;'>Needs Inspected</td>
                </tr>
                <tr>
                    <td style='background-color:#999999;'>Maintenance In</td>
                    <td style='background-color:#FF00FF;'>Maintenance Out</td>
                </tr>--%>
            </table>
        </td></tr>
    </table>
    
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>Loading...</ProgressTemplate>
            
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID = "Timer1" EventName="Tick" />
        </Triggers>
        <ContentTemplate>
    <asp:Timer ID="Timer1" runat="server" Interval="5000">
    </asp:Timer>
            <asp:Button ID="btnRun" runat="server" Text="Run Report" />
            <asp:Literal ID="lit1" runat="server" Text = "" />
            
        </ContentTemplate>
    </asp:UpdatePanel>
     

</asp:Content>

