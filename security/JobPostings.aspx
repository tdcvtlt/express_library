<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="JobPostings.aspx.vb" Inherits="security_JobPostings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Filter: <asp:DropDownList ID="ddActive" runat="server"></asp:DropDownList> <asp:Button ID="btnSearch" runat="server" Text ="Search"> </asp:Button><asp:Button ID="Button1" runat="server" Text="New" />
    <asp:GridView ID="gvJobs" runat="server" AutoGenerateSelectButton="true" EmptyDataText = "No Records">
    </asp:GridView>
</asp:Content>

