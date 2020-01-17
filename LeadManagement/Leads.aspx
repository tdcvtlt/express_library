<%@ Page Title="Leads" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Leads.aspx.vb" Inherits="LeadManagement_Leads" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li><asp:LinkButton ID="lbLeads" runat="server">Leads</asp:LinkButton></li>
        <li><asp:LinkButton ID="lbFiles" runat="server">Files</asp:LinkButton></li>
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="vwLeads" runat="server">
            <table>
                <tr>
                    <td>Filter By:</td>
                    <td><asp:DropDownList ID="ddFilter" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Filter:</td>
                    <td>
                        <asp:TextBox ID="txtFilter" runat="server"></asp:TextBox>
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" />
                    </td>
                    
                </tr>
            </table>
            <div class="ListGrid">
                <asp:GridView ID="gvLeads" runat="server" AutoGenerateSelectButton="False" DataKeyNames="LeadID" AutoGenerateColumns="true" 
                EmptyDataText="No Records" GridLines="Horizontal">
                    <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                    <AlternatingRowStyle BackColor="#CCFFCC" />        
                    <Columns>
                        <asp:HyperLinkField HeaderText="Edit" 
                                    DataNavigateUrlFields="LeadID" 
                                    DataNavigateUrlFormatString="editLead.aspx?LeadID={0}" DataTextField="LeadID" 
                                    />
                    
                              
                    </Columns>

                </asp:GridView>
            </div>
            <ul id="menu">
                <li>
                    <asp:LinkButton ID="lbAdd" runat="server">Add Lead</asp:LinkButton></li>
            </ul>
        </asp:View>
        <asp:View ID="vwFiles" runat="server">
            <asp:FileUpload ID="File1" runat="server" /><asp:Button ID="btnUpload" 
                runat="server" Text="Upload" onclientclick="if (ctl00$ContentPlaceHolder1$File1.value.slice(-4) != '.csv' && ctl00$ContentPlaceHolder1$File1.value.slice(-4) != '.CSV') {alert('Please only upload CSV files!'); return false;} " Enabled="False" /><br />
            <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label><asp:HiddenField ID="hfFile" Value="" runat="server" />
            
            <asp:HiddenField ID="hfState" runat="server" Value="0" />
            
            <asp:MultiView ID="MultiView2" runat="server">
                <asp:View ID="View3" runat="server">
                    <table>
                        <tr>
                            <td>List Template:</td>
                            <td><asp:DropDownList ID="ddListTemplate" runat="server"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Phone Number Column:</td>
                            <td><asp:DropDownList ID="ddPhone" runat="server"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>My Data Has Headers?</td>
                            <td>
                                <asp:CheckBox ID="cbHeaders" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnProcess" runat="server" Text="Process" />
                            </td>
                        </tr>
                        
                    </table>
                    <br />
                    <br />
                </asp:View>
                <asp:View ID="View1" runat="server">
                    <div class="ListGrid">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvFiles" runat="server" EmptyDataText="No Files">
                                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                                <AlternatingRowStyle BackColor="#CCFFCC" />        
                                <Columns>
                                    <asp:HyperLinkField HeaderText="Download" 
                                                DataNavigateUrlFields="FileID" 
                                                DataNavigateUrlFormatString="LeadListdownload.aspx?fileid={0}" DataTextField="FileID" 
                                                text=""
                                                />
                                </Columns>
                            </asp:GridView>
                            <asp:Timer ID="tmrRefresh" runat="server" Interval="5000" Enabled="False">
                            </asp:Timer>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>
                </asp:View>
            </asp:MultiView>
            <ul id="menu">
                <li><asp:LinkButton ID="lbCheckStatus" runat="server">Existing Files</asp:LinkButton></li>
                <li><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton></li>
            </ul>
        </asp:View>
    </asp:MultiView>
</asp:Content>

