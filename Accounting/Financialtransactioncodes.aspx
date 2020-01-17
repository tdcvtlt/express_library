<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Financialtransactioncodes.aspx.vb" Inherits="Accounting_Financialtransactioncodes" title="Financial Transaction Codes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Search By: <asp:DropDownList ID="ddCat" runat="server" AutoPostBack="true">
        <asp:ListItem Value="TransCode">Transaction Code</asp:ListItem>
        <asp:ListItem Value="TransType">Transaction Type</asp:ListItem>
        <asp:ListItem>Account</asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList ID="ddFilter" runat="server" AutoPostBack="true">
        <asp:ListItem Value="0">All</asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="btnSearch" runat="server" Text="Search" /><br />
    <div class="ListGrid">
    <asp:GridView ID="gvTransCodes" runat="server" AutoGenerateColumns="true" >
        <AlternatingRowStyle BackColor="#C7E3D7" />
        <Columns>
            <asp:templatefield><ItemTemplate><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/accounting/editfintranscode.aspx?id=<%#container.Dataitem("ID")%>','Edit',350,350);">Edit</a></ItemTemplate></asp:templatefield>
        </Columns>   
    </asp:GridView>
    </div>
    <%
    Dim oID As String = Me.ID & "$lbRefresh"
    Dim oTest As Object = Me.FindControl("lbRefresh")
    While Not (oTest Is Nothing)
        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
            oID = "ctl00$" & oTest.id & "$" & oID
        End If
        oTest = oTest.parent
    End While
 %>
    <ul id="menu">
        <li><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/accounting/editfintranscode.aspx?id=0&linkid=<%=oid %>','Edit',350,350);">Add New</a></li>
        <li><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton></li>
    </ul>
    
</asp:Content>

