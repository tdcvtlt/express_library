<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="SendEmail.aspx.vb" Inherits="setup_SendEmail" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <p>
        To:
        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
    </p>
    <p>
        Subject:
        <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
        <br />
    </p>
    <p>
        Message:<br />
&nbsp;<asp:TextBox ID="txtMessage" runat="server" Height="471px" TextMode="MultiLine" 
            Width="1102px"></asp:TextBox>
    </p>
    <p>
        SQL:<br />
        <asp:TextBox ID="txtSQL" runat="server" Height="471px" TextMode="MultiLine" 
            Width="1102px"></asp:TextBox>
    </p>
    <p>
        Tag:<asp:TextBox ID="txtTag" runat="server"></asp:TextBox>
    </p>
    <p>
        Field Value:<asp:TextBox ID="txtTagValue" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="Button1" runat="server" Text="Send Message" />
    </p>
</asp:Content>

