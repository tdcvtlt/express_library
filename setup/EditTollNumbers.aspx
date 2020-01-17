<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditTollNumbers.aspx.vb" Inherits="setup_EditTollNumbers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
        <ul id="menu">
            <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Toll" runat="server">Toll Numbers</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="DID" runat="server">DID's</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Triggers" runat="server">Triggers</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Notes" runat="server">Notes</asp:LinkButton></li>
        </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="Edit_View" runat="server">
            <table>
                <tr>
                    <td>
                        TollID:
                    </td>
                    <td>
                        <asp:TextBox ID="txtTollID" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        Toll Number:
                    </td>
                    <td>
                        <asp:TextBox ID="txtTollNumber" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Carrier:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarrier" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" />
                    </td>
                    <td>
                       
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="DID_View" runat="server">
            DID View
        </asp:View>
        <asp:View ID="Trigger_View" runat="server">
        
        </asp:View>
        <asp:View ID="Note_View" runat="server">
        
        </asp:View>
    </asp:MultiView>

</asp:Content>

