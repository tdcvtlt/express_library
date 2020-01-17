<%@ Page Title="TimeClock" Language="VB" AutoEventWireup="false" CodeFile="TimeClock.aspx.vb" Inherits="Payroll_TimeClock" %>
<html>
<head runat = "server">
<script language ="javascript" type="text/javascript">
    function Punch_In(deptID) {
        //alert(deptID);
        //document.getElementById("ctl00_ContentPlaceHolder1_hfDeptID").value = deptID
        document.getElementById("hfDeptID").value = deptID
        __doPostBack('lbCIDept', '');
        //__doPostBack('ctl00$ContentPlaceHolder1$lbCIDept', '');
    }
    function Punch_Out(deptID) {
        document.getElementById("hfDeptID").value = deptID
        __doPostBack('lbCODept', '');
    }

</script>

    <link type="text/css" rel="Stylesheet" href="~/styles/style.css" />
<script type="text/javascript" language="javascript" src = "<%=request.applicationpath%>/scripts/pop_modal.js"></script>
</head>
<body>
    <form runat = "server" id = "form1">
    <asp:scriptmanager runat="server"></asp:scriptmanager>
    <asp:MultiView id = "MultiView1" runat="server">
        <asp:View runat="server" id = "View_1">
            <table>
                <tr>
                    <td colspan = '2' align = 'center'><b><font size = "5">TIME CLOCK</font></b></td>
                </tr>
                <tr>
                    <td>Enter ID:</td>
                    <td><asp:TextBox runat="server" id = "txtSwipe" TextMode="Password"></asp:TextBox></td>
                    <td><asp:Button runat="server" Text="Submit" onclick="Unnamed1_Click"></asp:Button></td>
                </tr>
            </table>
            <asp:LinkButton runat="server" onclick="Unnamed3_Click">Home</asp:LinkButton>
            <br /><asp:Label runat="server" id = "lblPWErr"></asp:Label>
        </asp:View>
        <asp:View runat="server" id = "View_2">
            <asp:Label runat="server" id = "lblGreeting"></asp:Label>
            <br />
            <asp:Label runat="server" id = "lblActivity"></asp:Label>
            <br /><br />
            <table>
            <tr><td>Select From the Following Menu:</td></tr>
            <tr><td><asp:LinkButton runat="server" id = "lbPI">Punch In</asp:LinkButton></td></tr>
            <tr><td><asp:LinkButton runat="server" id = "lbPO">Punch Out</asp:LinkButton></td></tr>
            <tr><td><asp:LinkButton runat="server" id = "lbMissedPunch">Missed Punch Form</asp:LinkButton></td></tr>
            <tr><td><asp:LinkButton runat="server" id = "lbHours">View My Hours</asp:LinkButton></td></tr>
            <tr><td></td></tr>
            <tr><td><asp:LinkButton runat="server" id = "lbSignOut">Sign Out</asp:LinkButton></td></tr>
            </table>
            <asp:Label runat="server" id = "lblID"></asp:Label>
        <asp:HiddenField runat="server" id = "hfPersID"></asp:HiddenField>
        <asp:HiddenField runat="server" id = "hfDeptID" value = 0></asp:HiddenField>
        <asp:LinkButton runat="server" id = "lbCIDept"></asp:LinkButton>
        <asp:LinkButton runat="server" id = "lbCODept"></asp:LinkButton>
        <asp:Timer runat="server" id = "Timer1" Enabled="False" Interval="20000"></asp:Timer>
        </asp:View>
    </asp:MultiView>
    </form>
</body>
</html>
