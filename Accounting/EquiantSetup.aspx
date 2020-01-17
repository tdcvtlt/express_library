<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EquiantSetup.aspx.vb" Inherits="Accounting_EquiantSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li><a>Un-Mapped</a></li>
    </ul>
    <div class="ListGrid">
        <asp:GridView ID="gvUnmapped" runat="server" EnableModelValidation="True">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
            </Columns>
        </asp:GridView>
    </div>
    <ul id="menu">
        <li><a>Mapped</a></li>
    </ul>
    <div class="ListGrid">
        <asp:GridView ID="gvMapped" runat="server" EnableModelValidation="True">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

