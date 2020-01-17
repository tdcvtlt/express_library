<%@ Page Title="ORF Report" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ORFReport.aspx.vb" Inherits="Reports_OwnerServices_ORFReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <input type="button" value="Printable" onclick="var mwin = window.open();mwin.document.write(document.getElementById('printable').innerHTML);" />
            <div id="printable">
    <asp:GridView ID="gvReport" runat="server">
    </asp:GridView>
    </div>
</asp:Content>

