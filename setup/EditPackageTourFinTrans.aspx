<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditPackageTourFinTrans.aspx.vb" Inherits="setup_EditPackageTourFinTrans" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">

    function refreshPayments() {

        __doPostBack('ctl00$ContentPlaceHolder1$LinkFinTransPayment', '');
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:LinkButton ID="lbPackage" runat="server">LinkButton</asp:LinkButton>
<ul id="menu">
    <li <% if MultiViewPackage.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkFinTrans" runat="server">Financials</asp:LinkButton>
    </li>    
    
    <li <% if MultiViewPackage.ActiveViewIndex = 1 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkFinTransPayment" runat="server">Payments</asp:LinkButton>        
    </li>    
</ul>
    <asp:MultiView ID="MultiViewPackage" runat="server">
        <asp:View ID="View1" runat="server">
            <table>
                <tr>
                    <td>ID:</td>
                    <td>
                        <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Fin Trans Code:</td>
                    <td>
                        <asp:DropDownList ID="ddFinTrans" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Use Formula:</td>
                    <td>
                        <asp:CheckBox ID="cbFormula" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Formula:</td>
                    <td>
                        <asp:TextBox ID="txtFormula" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Amount:</td>
                    <td>
                        <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <ul id="menu">
                <li>
                    <asp:LinkButton ID="LinkButton1" runat="server">Save</asp:LinkButton>
                </li>  
            </ul>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:GridView ID="gvPayments" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal" 
                AutoGenerateColumns="true" AutoGenerateSelectButton="true" BorderStyle="None">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
            </asp:GridView>
            <ul id="menu">
                <li>
                    <asp:LinkButton ID="LinkButton2" runat="server">Insert</asp:LinkButton>
                </li>  
            </ul>
        </asp:View>
    </asp:MultiView>
</asp:Content>

