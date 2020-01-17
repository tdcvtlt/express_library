<%@ Page Title="Edit Funding" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditFunding.aspx.vb" Inherits="Accounting_EditFunding" EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language=javascript type="text/javascript">
        function Refresh_Funding() {
            __doPostBack('ctl00$ContentPlaceHolder1$LinkButton1', ''); 
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id = "menu">
        <li><asp:LinkButton runat="server" id = "LinkButton1">Funding</asp:LinkButton></li>
        <li><asp:LinkButton runat="server" id = "LinkButton2">KCP</asp:LinkButton></li>
        <li><asp:LinkButton runat="server" id = "LinkButton3">Presales</asp:LinkButton></li>
        <li><asp:LinkButton runat="server" id = "LinkButton4">Wells Fargo</asp:LinkButton></li>
        <li><asp:LinkButton runat="server" id = "LinkButton5">Wells Fargo-Pre</asp:LinkButton></li>
        <li><asp:LinkButton runat="server" id = "LinkButton6">Atlantic Title</asp:LinkButton></li>
        <li><asp:LinkButton runat="server" id = "LinkButton7">Atl. Title Pre</asp:LinkButton></li>
    </ul>
    <asp:MultiView runat="server" id = "MultiView1">
        <asp:View runat="server" id = "Funding_View">
            <div style="height: 400px; width: 796px;overflow:auto;">
                <asp:GridView runat="server" id = "gvFundingItems" 
                    onRowDataBound = "gvFundingItems_RowDataBound" EnableModelValidation="True">
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:checkbox ID="ItemSelect" runat = "server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Button runat="server" Text="Remove Selected" id = "btnRemove"></asp:Button> <asp:Button runat="server" Text="Submit Funding" id = "btnSubmit"></asp:Button> 
            <asp:Button runat="server" Text="Add Deal" onclick="Unnamed1_Click"></asp:Button> 
            <asp:Button runat="server" Text="Add Cancel" onclick="Unnamed2_Click"></asp:Button>
            <asp:Button runat="server" Text="Export to Excel" onclick="Unnamed3_Click"></asp:Button>
            <asp:Label runat="server" id = "lblErr"></asp:Label>
        </asp:View>
        <asp:View runat="server" id = "KCP_View">
            <div style="height: 400px; width: 796px" style = "overflow:auto;">
                <asp:GridView ID="gvKCP" runat="server" EmptyDataText="No Records" onRowDataBound = "gvKCP_RowDataBound">
                </asp:GridView>
            </div>
        </asp:View>
        <asp:View runat="server" id = "PreSales_View">
            <div style="height: 400px; width: 796px" style = "overflow:auto;">
                <asp:GridView ID="gvKCPPS" runat="server" EmptyDataText="No Records" onRowDataBound = "gvKCPPS_RowDataBound">
                </asp:GridView>
            </div>
        </asp:View>
        <asp:View runat="server" id = "Wells_View">
            <div style="height: 400px; width: 796px" style = "overflow:auto;">
                <asp:GridView ID="gvWF" runat="server" EmptyDataText="No Records" onRowDataBound = "gvWF_RowDataBound">
                </asp:GridView>
            </div>
        </asp:View>
        <asp:View runat="server" id = "WellsPS_View">
            <div style="height: 400px; width: 796px" style = "overflow:auto;">
                <asp:GridView ID="gvWFPS" runat="server" EmptyDataText="No Records" onRowDataBound = "gvWFPS_RowDataBound">
                </asp:GridView>
            </div>
        </asp:View>
        <asp:View runat="server" id = "Atlantic_View">
            <div style="height: 400px; width: 796px" style = "overflow:auto;">
                <asp:GridView ID="gvAtl" runat="server" EmptyDataText="No Records" onRowDataBound = "gvAtl_RowDataBound">
                </asp:GridView>
            </div>
        </asp:View>
        <asp:View runat="server" id = "AtlanticPS_View">
            <div style="height: 400px; width: 796px" style = "overflow:auto;">
                <asp:GridView ID="gvAtlPS" runat="server" EmptyDataText="No Records" onRowDataBound = "gvAtlPS_RowDataBound">
                </asp:GridView>
            </div>
        </asp:View>
    </asp:MultiView>

</asp:Content>

