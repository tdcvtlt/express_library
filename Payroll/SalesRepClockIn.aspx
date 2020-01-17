<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="SalesRepClockIn.aspx.vb" Inherits="Payroll_SalesRepClockIn" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Punch Type:</td>
            <td>
                <asp:RadioButtonList ID="rbPunchType" runat="server" 
                    RepeatDirection="Horizontal">
                    <asp:ListItem>Punch In</asp:ListItem>
                    <asp:ListItem>Punch Out</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>Punch Date:</td>
            <td>
                <uc1:DateField ID="dtePunchDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Punch Time:</td>
            <td>
                <asp:DropDownList ID="ddHour" runat="server">
                </asp:DropDownList><asp:DropDownList ID="ddMinute" runat="server">
                </asp:DropDownList> 
                <asp:DropDownList ID="ddAMPM" runat="server">
                    <asp:ListItem>AM</asp:ListItem>
                    <asp:ListItem>PM</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <div style="height: 426px; margin-right: 604px; overflow: scroll; width: 387px;">
        <asp:GridView ID="gvReps" runat="server" EnableModelValidation="True" OnRowDataBound = "gvReps_RowDataBound" EmptyDataText = "No Records">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbEmployee" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    <div style="position: absolute; top: 150px; left: 724px; width: 387px; height: 426px;">
        <asp:Label ID="lblErrors" runat="server" Text=""></asp:Label>
    </div>
    </div>
    <asp:Button ID="btnGo" runat="server" Text="Process Selected Punches" />

</asp:Content>

