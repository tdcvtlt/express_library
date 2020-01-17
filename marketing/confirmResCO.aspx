<%@ Page Language="VB" AutoEventWireup="false" CodeFile="confirmResCO.aspx.vb" Inherits="marketing_confirmResCO" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 67px;
        }
        .style2
        {
            width: 146px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
    <div>
        <asp:label runat="server" id = "COlbl"></asp:label>
        <br /><br />
        <table>
        <tr>
            <td class="style1"><asp:button runat="server" text="Yes" Width="45px" 
                    onclick="Unnamed1_Click" /></td>
            <td class="style2"><asp:button runat="server" text="No" Width="50px" 
                    onclick="Unnamed2_Click" /></td>
        </tr>
        </table>                  
    </div>
    </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:label runat="server" id = "COLbl2"></asp:label>
            <br /><br />
            <table>
            <tr>
                <td class="style1"><asp:button runat="server" text="Yes" Width="45px" ID="CCYesBtn" 
                         /></td>
                <td class="style2"><asp:button runat="server" text="No" Width="50px" ID="CCNoBtn"
                         /></td>
            </tr>
            </table>   
            <asp:HiddenField ID="hfCCTransID" Value="0" runat="server" />
            
        </asp:View>
        <asp:View ID="View3" runat="server">
            Processing....Please Wait.
            <asp:Timer ID="Timer1" runat="server" Enabled="False"></asp:Timer>
        </asp:View>
    </asp:MultiView>
    </form>
</body>
</html>
