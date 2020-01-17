<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Request.aspx.vb" Inherits="Maintenance_Request" Title="Maintenance Requests"%>


<%@ OutputCache Duration="1" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
        function Update_Text() {
            var sel = document.getElementById('ctl00_ContentPlaceHolder1_ddFilter');
            var sText = '';
            if(sel.options[sel.selectedIndex].value == 'RequestID') {
                sText = 'Enter an ID:';
            }
            else if (sel.options[sel.selectedIndex].value == 'Date') {
                sText = 'Enter a Date';
            }
            else if (sel.options[sel.selectedIndex].value == 'Assigned To') {
                sText = 'Enter a Rep Name:';
            }
            else {
                sText = 'Enter a Room Number:';
            }
            document.getElementById('ctl00_ContentPlaceHolder1_Label1').innerHTML = sText;
        }

        /*function Get_Item() {
            var sel = document.getElem('ct100_ContentPlaceHolder1_ddFilter');
            if(sel.options[sel.selectedIndex].value == 'RequestID') {
                'Enter an ID:'
            }
            else if (sel.options[sel.selectedIndex].value == 'Date') {
                'Enter a Date'
            }
            else if (sel.options[sel.selectedIndex].value == 'Assigned To') {
                'Enter a Rep's Name'
            }
            else {
                'Enter a Room Number:'
            }
        }*/
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label2" runat="server" Text="Filter: "></asp:Label>
    <asp:DropDownList ID="ddFilter" runat="server" onchange="Update_Text();" autopostback = "false">
        <asp:ListItem>RequestID</asp:ListItem>
        <asp:ListItem>Date</asp:ListItem>
        <asp:ListItem>Assigned To</asp:ListItem>
        <asp:ListItem>Room</asp:ListItem>
        <asp:ListItem>Subject</asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />
    <asp:Label ID="Label1" runat="server" Text = "Enter An ID:"></asp:Label><br />
    <asp:TextBox ID="filter" runat="server"></asp:TextBox>
    <asp:Button ID="Button1"
        runat="server" Text="Query" />
    <asp:Button ID="btnNew" runat="server" Text="New" />
    <br />
    <div style="height:800px;width:900 px; overflow:auto; ">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="True" 
    EmptyDataText="No Records">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />
    </asp:GridView>
    <asp:label id="lblErr" runat="server">
    
    </asp:label>
    </div>
</asp:Content>
