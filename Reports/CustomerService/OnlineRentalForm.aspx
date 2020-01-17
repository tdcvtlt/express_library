<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OnlineRentalForm.aspx.vb" Inherits="Reports_CustomerService_OnlineRentalForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="gvRentals" runat="server" EnableModelValidation="True">
     <AlternatingRowStyle BackColor="#C7E3D7" />
                    
        <Columns>
            <asp:ButtonField ButtonType="Button" CommandName="gvRentals_RowCommand" Text="Complete" />
        </Columns>
                    
    </asp:GridView><br />
    <asp:Button runat ="server" ID="btnExport" Text ="Export to Excel" /> <asp:Button runat ="server" ID="btnComplete" Text ="Complete All" />
</asp:Content>

