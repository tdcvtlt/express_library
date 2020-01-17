<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditTag.aspx.vb" Inherits="setup_Letters_EditTag" ValidateRequest="false" %>

<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            height: 26px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table align="center">
            <tr>
                <th colspan="2" align="center">Letter Tag</th>
            </tr>
            <tr>
                <td class="style1">Tag ID:</td>
                <td class="style1"><asp:TextBox ID="txtTagID" readonly="true" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Tag:</td>
                <td><asp:TextBox ID="txtTag" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Type:</td>
                <td>
                    <uc1:Select_Item ID="siType" runat="server" />
                </td>
            </tr>
            <tr>
                <td>View:</td>
                <td>
                    <asp:DropDownList ID="ddView" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Field Name:</td>
                <td>
                    <asp:DropDownList ID="ddFieldName" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Key Field Name:</td>
                <td>
                    <asp:DropDownList ID="ddKeyFieldName" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Key Field Value:</td>
                <td>
                    <asp:DropDownList ID="ddKeyFieldValue" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Description:</td>
                <td><asp:TextBox ID="txtDescription" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Style:</td>
                <td><asp:DropDownList ID="ddStyle" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" />
                    <input type="button" value="Close" onclick="javascript:window.opener.document.forms[0].submit();window.close();" />        
                </td>
            </tr>
        </table>
        
        
    
    </div>
    <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>

    </form>
</body>
</html>
