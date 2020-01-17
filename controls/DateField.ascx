<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DateField.ascx.vb" Inherits="controls_DateField" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:TextBox ID="txtDate" runat="server" ReadOnly="True"></asp:TextBox><asp:Button ID="btnSelect" runat="server" Text="..." />
<br />

    <asp:Panel ID="Panel1" runat="server" Visible="false">
    <div style="background-color:#aaaaaa;width:260px;text-align:center;">
    
    <asp:Button ID="btnPrev" runat="server" Text="<" />
        &nbsp;&nbsp; &nbsp;
<asp:DropDownList ID="ddMonth" runat="server" AutoPostBack="True" 
    onselectedindexchanged="ddMonthSelectedIndexChanged">
    <asp:ListItem Value="01">Jan</asp:ListItem>
    <asp:ListItem Value="02">Feb</asp:ListItem>
    <asp:ListItem Value="03">Mar</asp:ListItem>
    <asp:ListItem Value="04">Apr</asp:ListItem>
    <asp:ListItem Value="05">May</asp:ListItem>
    <asp:ListItem Value="06">Jun</asp:ListItem>
    <asp:ListItem Value="07">Jul</asp:ListItem>
    <asp:ListItem Value="08">Aug</asp:ListItem>
    <asp:ListItem Value="09">Sep</asp:ListItem>
    <asp:ListItem Value="10">Oct</asp:ListItem>
    <asp:ListItem Value="11">Nov</asp:ListItem>
    <asp:ListItem Value="12">Dec</asp:ListItem>
</asp:DropDownList>
    &nbsp;&nbsp;
<asp:DropDownList ID="ddYear" runat="server" AutoPostBack="True" 
    onselectedindexchanged="ddYearSelectedIndexChanged">
</asp:DropDownList>
        &nbsp;&nbsp; &nbsp;
<asp:Button ID="btnNext" runat="server" Text=">" />
</div>
<asp:Calendar ID="Calendar1" runat="server"  ShowTitle="False" 
        Width="260px"></asp:Calendar>
        <div style="background-color:#aaaaaa;width:260px;text-align:center;">
            <asp:LinkButton ID="lbToday" runat="server" ForeColor="Black">Today: </asp:LinkButton><br />
            <asp:LinkButton ID="lbClear" runat="server" ForeColor="Black">Clear Date</asp:LinkButton>
        </div>
    </asp:Panel>


</ContentTemplate>
</asp:UpdatePanel>


<asp:HiddenField ID="hfPostBack" runat="server" />



