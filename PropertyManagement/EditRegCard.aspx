<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditRegCard.aspx.vb" Inherits="setup_EditRegCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language=javascript type ="text/javascript">
    function Refresh_Items()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$RegCardItems_Link','');
    }
        function Refresh_ResTypes()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$ResTypes_Link','');
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id = "menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="RegCard_Link" runat="server">Reg Card</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="RegCardItems_Link" runat="server">Reg Card Items</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="ResTypes_Link" runat="server">Reservation Types</asp:LinkButton></li>
    </ul>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <table>
                <tr>
                    <td>ID:</td>
                    <td><asp:TextBox ID="txtID" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Description:</td>
                    <td><asp:TextBox ID="txtDesc" runat="server"></asp:TextBox></td>
                </tr>

            </table>
           <ul id= "menu">
                <li><asp:LinkButton ID="LinkButton3" runat="server">Save</asp:LinkButton></li>
            </ul>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <div style="height: 566px; overflow: scroll; width: 855px;" >
            <asp:GridView ID="gvItems" runat="server" AutoGenerateSelectButton = "true" EmptyDataText = "No Items" OnRowDataBound="gvItems_RowDataBound">
            </asp:GridView>
            </div>
            <ul id= "menu">
                <li><asp:LinkButton ID="LinkButton1" runat="server">Add Item</asp:LinkButton></li>
            </ul>
        </asp:View>
        <asp:View ID="View3" runat="server">
            <asp:GridView ID="gvResTypes" runat="server" AutoGenerateSelectButton = "true" EmptyDataText = "No Items">
            </asp:GridView>
            <ul id= "menu">
                <li><asp:LinkButton ID="LinkButton2" runat="server">Add</asp:LinkButton></li>
            </ul>
        </asp:View>
    </asp:MultiView>
</asp:Content>

