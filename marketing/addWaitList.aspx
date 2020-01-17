<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addWaitList.aspx.vb" Inherits="marketing_addWaitList" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">    <asp:scriptmanager runat="server"></asp:scriptmanager>

    Filter: <asp:dropdownlist ID="ddFilter" runat="server" autoPostBack = "true" ></asp:dropdownlist>
    <br />
    <asp:label runat="server" id = "lblFilter">Enter Phone Number:</asp:label>
    <br />
    <asp:textbox runat="server" id = "txtFilter"></asp:textbox>
    <asp:button runat="server" text="Query" onclick="Unnamed1_Click" />
    <asp:multiview runat="server" id = "MultiView1">
        <asp:View runat="server" id = "View1">
            <asp:GridView ID="gvResults" runat="server" AutoGenerateSelectButton="True" onRowDataBound = "gvResults_RowDataBound" EmptyDataText = "No Records">
            </asp:GridView>
        </asp:View>
        <asp:View runat="server" id = "View2">
            <table>
                <tr>
                    <td colspan = '2'><asp:Label runat="server" id = "lblProspect"></asp:Label></td>
                </tr>
                <tr>
                    <td>Select Contract:</td>
                    <td><asp:DropDownList runat="server" id = "ddContract"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Start Date:</td>
                    <td>
                        <uc1:DateField ID="dteStartDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>End Date:</td>
                    <td>
                        <uc1:DateField ID="dteEndDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Unit Type:</td>
                    <td>
                        <uc2:Select_Item ID="siUnitType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>BD:</td>
                    <td><asp:DropDownList runat="server" id = "ddBD"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Season:</td>
                    <td><uc2:Select_Item ID="siSeason" runat="server" /></td>
                </tr>
                <tr>
                    <td><asp:Button runat="server" Text="Submit" onclick="Unnamed2_Click"></asp:Button></td>
                </tr>
            </table>
            <asp:HiddenField runat="server" id = "hfProsID"></asp:HiddenField>
        </asp:View>
    </asp:multiview>

    </form>
</body>
</html>
