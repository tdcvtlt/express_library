<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Units.aspx.vb" Inherits="marketing_Units" title="<b>Unit Search</b>" %>

<%@ OutputCache Duration="1" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
        function Update_Text() {
            var sel = document.getElementById('ctl00_ContentPlaceHolder1_ddFilter');
            var sText = (sel.options[sel.selectedIndex].value == 'Room Number') ? 'Enter a Room Number:' : 'Enter a Unit Number:';
            document.getElementById('ctl00_ContentPlaceHolder1_Label1').innerHTML = sText;
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label2" runat="server" Text="Filter: "></asp:Label>
    <asp:DropDownList ID="ddFilter" runat="server"  onchange="Update_Text();" autopostback = "false">
        <asp:ListItem>Unit Number</asp:ListItem>
        <asp:ListItem>Room Number</asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />
  
    <asp:Label ID="Label1" runat="server"></asp:Label><br />
    <asp:TextBox ID="filter" runat="server"></asp:TextBox>
    <asp:Button ID="Button1"
        runat="server" Text="Query" />
    <asp:Button ID="btnNew" runat="server" Text="New" />
    <br />
    <div style="height:200px;width:300px;overflow:auto; ">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="True" 
    EmptyDataText="No Records" GridLines="Horizontal">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />
    </asp:GridView>
    <asp:label id="lblErr" runat="server">
    
    </asp:label>
    </div>
</asp:Content>
