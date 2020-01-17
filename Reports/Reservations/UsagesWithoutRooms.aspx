<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="UsagesWithoutRooms.aspx.vb" Inherits="Reports_Reservations_UsagesWithoutRooms" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="div1">
    <table>
        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField ID="sdate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                End Date:
            </td>
            <td>
                <uc1:DateField ID="edate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:ListBox runat="server" ID="lbL" Rows="7" Width="130px"></asp:ListBox>
                            &nbsp;
                        </td>
                        <td valign="middle">
                            <asp:Button runat="server" ID="btL" Text=">" Font-Bold="true" style="width: 40px"  />
                            &nbsp;
                            <br />
                     
                            <asp:Button runat="server" ID="btLAll" Text=">>" Font-Bold="true" 
                                style="width: 40px" />
                            &nbsp;
        
                            <br />
                            <asp:Button runat="server" ID="btR" Text="<" Font-Bold="true" style="width: 40px"  />
                             <br />
                            <asp:Button runat="server" ID="btRAll" Text="<<" Font-Bold="true" 
                                style="width: 40px" />
                            &nbsp;
                            <br />

                        </td>                
                        <td>
                            &nbsp;                    
                            <asp:ListBox runat="server" ID="lbR" Rows="7"  Width="130px"></asp:ListBox>
                        </td>
                    </tr>
                </table>    
            </td>
        </tr>
        <tr>
            <td><asp:Button ID="Report" runat="server" Text="Run Report" />
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lblErr" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</div>
<div id = "div2">
    <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="true" 
        EmptyDataText="No Records" GridLines="Both" CssClass="form-control"
        AutoGenerateSelectButton="True">
        <selectedrowstyle BackColor="#CCFFFF" Wrap="true" />
        <alternatingrowstyle BackColor="#CCFFCC" />
    </asp:GridView>
</div>
</asp:Content>

