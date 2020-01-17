<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TestCalendar.ascx.vb" Inherits="controls_TestCalendar" %>


<asp:DropDownList ID="ddMonths" runat="server" AutoPostBack = "true"
    onselectedindexchanged="ddMonthsSelectedIndexChanged">
</asp:DropDownList>
<asp:DropDownList ID="ddYears" runat="server" AutoPostBack = "true"
    onselectedindexchanged="ddYearsSelectedIndexChanged">
</asp:DropDownList>
<asp:Table ID="Table1" runat="server">
    <asp:TableRow runat="server">
        <asp:TableCell runat="server" >Sunday</asp:TableCell>
        <asp:TableCell runat="server">Monday</asp:TableCell>
        <asp:TableCell runat="server">Tuesday</asp:TableCell>
        <asp:TableCell runat="server">Wednesday</asp:TableCell>
        <asp:TableCell runat="server">Thursday</asp:TableCell>
        <asp:TableCell runat="server">Friday</asp:TableCell>
        <asp:TableCell runat="server">Saturday</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server">
        <asp:TableCell ID="TableCell1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell4" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell5" runat="server"><asp:LinkButton ID="LinkButton5" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell6" runat="server"><asp:LinkButton ID="LinkButton6" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell7" runat="server"><asp:LinkButton ID="LinkButton7" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
    </asp:TableRow>
        <asp:TableRow ID="TableRow1" runat="server">
        <asp:TableCell ID="TableCell8" runat="server"><asp:LinkButton ID="LinkButton8" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell9" runat="server"><asp:LinkButton ID="LinkButton9" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell10" runat="server"><asp:LinkButton ID="LinkButton10" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell11" runat="server"><asp:LinkButton ID="LinkButton11" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell12" runat="server"><asp:LinkButton ID="LinkButton12" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell13" runat="server"><asp:LinkButton ID="LinkButton13" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell14" runat="server"><asp:LinkButton ID="LinkButton14" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
    </asp:TableRow>
        <asp:TableRow ID="TableRow2" runat="server">
        <asp:TableCell ID="TableCell15" runat="server"><asp:LinkButton ID="LinkButton15" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell16" runat="server"><asp:LinkButton ID="LinkButton16" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell17" runat="server"><asp:LinkButton ID="LinkButton17" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell18" runat="server"><asp:LinkButton ID="LinkButton18" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell19" runat="server"><asp:LinkButton ID="LinkButton19" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell20" runat="server"><asp:LinkButton ID="LinkButton20" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell21" runat="server"><asp:LinkButton ID="LinkButton21" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
    </asp:TableRow>
        <asp:TableRow ID="TableRow3" runat="server">
        <asp:TableCell ID="TableCell22" runat="server"><asp:LinkButton ID="LinkButton22" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell23" runat="server"><asp:LinkButton ID="LinkButton23" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell24" runat="server"><asp:LinkButton ID="LinkButton24" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell25" runat="server"><asp:LinkButton ID="LinkButton25" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell26" runat="server"><asp:LinkButton ID="LinkButton26" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell27" runat="server"><asp:LinkButton ID="LinkButton27" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell28" runat="server"><asp:LinkButton ID="LinkButton28" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
    </asp:TableRow>
        <asp:TableRow ID="TableRow4" runat="server">
        <asp:TableCell ID="TableCell29" runat="server"><asp:LinkButton ID="LinkButton29" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell30" runat="server"><asp:LinkButton ID="LinkButton30" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell31" runat="server"><asp:LinkButton ID="LinkButton31" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell32" runat="server"><asp:LinkButton ID="LinkButton32" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell33" runat="server"><asp:LinkButton ID="LinkButton33" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell34" runat="server"><asp:LinkButton ID="LinkButton34" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell35" runat="server"><asp:LinkButton ID="LinkButton35" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
    </asp:TableRow>
        <asp:TableRow ID="TableRow5" runat="server">
        <asp:TableCell ID="TableCell36" runat="server"><asp:LinkButton ID="LinkButton36" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell37" runat="server"><asp:LinkButton ID="LinkButton37" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell38" runat="server"><asp:LinkButton ID="LinkButton38" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell39" runat="server"><asp:LinkButton ID="LinkButton39" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell40" runat="server"><asp:LinkButton ID="LinkButton40" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell41" runat="server"><asp:LinkButton ID="LinkButton41" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
        <asp:TableCell ID="TableCell42" runat="server"><asp:LinkButton ID="LinkButton42" runat="server" OnClick="Date_Click" Font-Underline = "false"></asp:LinkButton>
</asp:TableCell>
    </asp:TableRow>
    
</asp:Table><asp:HiddenField ID = "hfDate" value = "0" runat="server">
    </asp:HiddenField>
    <asp:HiddenField ID = "hfDateCell" value = "" runat="server">
    </asp:HiddenField>
    <asp:LinkButton ID="LinkButton43" runat="server" OnClick="Date_Click" >a</asp:LinkButton>
     

     