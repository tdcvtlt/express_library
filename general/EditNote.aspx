<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditNote.aspx.vb" Inherits="general_EditNote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Note</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table align="center">
            <tr>
                <th colspan="2" align="center">Note Record</th>
            </tr>
            <tr>
                <td>Note Template:</td>
                <td><asp:DropDownList ID="ddNoteTemplate" runat="server" AutoPostBack="True"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2" align="center"><asp:TextBox ID="txtNote" runat="server" 
                        TextMode="MultiLine" Rows="10" width="250px"></asp:TextBox></td>
            </tr>

            <%If InStr(Request("keyfield").ToLower, "contractid") > 0 Then%>
                    <tr>
                        <td colspan = '2'><asp:checkbox runat="server" id = "cbOwner"></asp:checkbox> Copy Note To Owner</td>
                    </tr>    
            <%End If%>
            <%If InStr(Request("keyfield").ToLower, "reservationid") > 0 Then%>
            <tr>
                <td colspan = '2'><asp:checkbox runat="server" id = "cbTour"></asp:checkbox> Copy Note To Tour</td>
            </tr>                    
            <%End If%>       

            <tr>
                <td>User Name:</td>
                <td><asp:TextBox readonly="true" ID="txtUser" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Date Created:</td>
                <td><asp:TextBox ReadOnly="true" ID="txtDate" runat="server"></asp:TextBox></td>
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
