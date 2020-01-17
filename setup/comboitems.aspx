<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="comboitems.aspx.vb" Inherits="setup_comboitems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script language="javascript" type="text/javascript">
        function Refresh_Combos() {
            location.href = "comboitems.aspx";
        }
        function Refresh_Items() {
            __doPostBack("ctl00$ContentPlaceHolder1$gvCombos", "Select$<%= gvcombos.selectedindex %>");
        }
    </script>
    <ul id="menu">
        <li><a href="javascript:void();">Combos:</a></li>
        <li><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/setup/editcombos.aspx?Type=Combo&ID=0&parent=0','Edit',350,350)">Add</a></li>
    </ul>

    <div class="ListGrid" title="Combos">
        <asp:GridView ID="gvCombos" runat="server" AutoGenerateSelectButton="True">
            <AlternatingRowStyle BackColor="#C7E3D7" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/setup/editcombos.aspx?Type=Combo&ID=<%# eval("ID") %>&parent=0','Edit',350,350)">Edit</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <ul id="menu">
        <li><a href="javascript:void();"><%= SelectedText%> Items:</a></li>
        <li><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/setup/editcombos.aspx?Type=Item&ID=0&parent=<%= selectedparent %>','Edit',350,350)">Add</a></li>
    </ul>
    <div class="ListGrid" title="Items">
        <asp:GridView ID="gvItems" runat="server">
            <AlternatingRowStyle BackColor="#C7E3D7" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/setup/editcombos.aspx?Type=Item&ID=<%# eval("ID") %>&parent=<%=gvcombos.selectedrow.cells(2).text %>','Edit',350,350)">Edit</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    
</asp:Content>

