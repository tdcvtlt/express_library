<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ReservationsBooked.aspx.vb" Inherits="Reports_Rentals_ReservationsBooked" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div>
        <div style="border:1px gray solid;margin-top:10px;">
            <h1 style="padding-left:20px;">reservation & finance</h1>
        </div>
            
    <div>
        <div style="float:left;width:330px;">
            <h2>Reservation Status</h2>
            <asp:CheckBoxList runat="server" ID="checkbox_list" ></asp:CheckBoxList>        
        </div>
        <div >
            <h2>Dates By</h2>        
            <asp:RadioButton runat="server" ID="radiobutton_1" Text="Date Check-In"  GroupName="r" Checked="true" /> &nbsp;&nbsp;   
            <asp:RadioButton runat="server" ID="radiobutton_3" Text="Date Check-Out" GroupName="r" /> &nbsp;&nbsp;         
            <asp:RadioButton runat="server" ID="radiobutton_2" Text="Date Booked" GroupName="r" />  
            
            <br />
            <br />
            <table>
                <tr>
                    <td>Start Date:</td>
                    <td><uc1:DateField ID="dfStartDate" runat="server" /></td>
                </tr>
                <tr>
                    <td>End Date:</td>
                    <td><uc1:DateField ID="dfEndDate" runat="server" /></td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Button ID="btnRunReport" runat="server" Text="Run Report" Width="140" Height="40" />  &nbsp;&nbsp;
                    <asp:Button ID="btnExcelReport" runat="server" Text="Excel Export(EE)" Width="140" Height="40" Visible="false" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    
        
    <br style="clear:left;" /><br />
 
    <br /><br />
    <asp:GridView runat="server" ID="gv" AutoGenerateColumns="False"  Visible="false"
            EmptyDataText="No match found." BorderStyle="None" 
            EnableModelValidation="True">
    <AlternatingRowStyle BackColor="#CCFFCC" />
    <HeaderStyle Font-Size="Larger" ForeColor="White" />    
    <Columns>
        <asp:BoundField DataField="DEPT" HeaderText="DEPT" 
            HeaderStyle-BackColor="DarkSeaGreen" 
            HeaderStyle-HorizontalAlign="Left" >
            <HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium"
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle Wrap="False" />
        </asp:BoundField>
        <asp:BoundField DataField="Reservation Type" HeaderText="Reservation Type" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left">    
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle Wrap="False" />
        </asp:BoundField>
        <asp:BoundField DataField="Usage Type" HeaderText="Usage Type" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Phase" HeaderText="Phase" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="ReservationID" HeaderText="ReservationID" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Reservation Status" HeaderText="Reservation Status" 
                    HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
                    HeaderStyle-HorizontalAlign="Left" >
        <HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                        ForeColor="White" Wrap="False">
        </HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Check In" HeaderText="Check In" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left"  
            DataFormatString="{0:d}">
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="CheckInDay" HeaderText="Check In Day" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left">
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Check Out" HeaderText="Check Out" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left"  
            DataFormatString="{0:d}">
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Nights" HeaderText="Nights" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="BR" HeaderText="BR" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Room Charge Before Discount" 
            HeaderText="Room Charge Before Discount" HeaderStyle-BackColor="DarkSeaGreen" 
            HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" 
             DataFormatString="{0:0.00}">
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" Wrap="False" />
        </asp:BoundField>
        <asp:BoundField DataField="Discount" HeaderText="Discount" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left"  
            DataFormatString="{0:0.00}">
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="Net Room Charge" HeaderText="Net Room Charge" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left"  
            DataFormatString="{0:0.00}">
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="Accommodation Fee" HeaderText="Accommodation Fee" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left"  
            DataFormatString="{0:0.00}">
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="Resort Fee" HeaderText="Resort Fee" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left"  
            DataFormatString="{0:0.00}">
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>


        <asp:BoundField DataField="ChargeBack" HeaderText="ChargeBack" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left"  
            DataFormatString="{0:0.00}">
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="Charge Back Type" HeaderText="Charge Back Type" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >

<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        <ItemStyle Wrap="False" />
        </asp:BoundField>

        <asp:BoundField DataField="Payments" HeaderText="Payments" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left"  
            DataFormatString="{0:0.00}">
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="Balance" HeaderText="Balance" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left"  
            DataFormatString="{0:0.00}">
<HeaderStyle HorizontalAlign="Right" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="Trans Tax" HeaderText="Trans Tax" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left"  
            DataFormatString="{0:0.00}">
<HeaderStyle HorizontalAlign="Right" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="County Tax" HeaderText="County Tax" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left"  
            DataFormatString="{0:0.00}">
<HeaderStyle HorizontalAlign="Right" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="State Tax" HeaderText="State Tax" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left"  
            DataFormatString="{0:0.00}">
<HeaderStyle HorizontalAlign="Right" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="Source" HeaderText="Source" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="SubType" HeaderText="SubType" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Tour Status" HeaderText="Tour Status" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Campaign" HeaderText="Campaign" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Tour Date" HeaderText="Tour Date" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Booking Agent" HeaderText="Booking Agent" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle Wrap="False" />
        </asp:BoundField>
        <asp:BoundField DataField="Package Agent" HeaderText="Package Agent" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle Wrap="False" />
        </asp:BoundField>
        <asp:BoundField DataField="RoomAddedBy" HeaderText="RoomAddedBy" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle Wrap="False" />
        </asp:BoundField>
        <asp:BoundField DataField="Date Booked" HeaderText="Date Booked" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left"  
            DataFormatString="{0:d}">        

<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Package" HeaderText="Package" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle Wrap="False" />
        </asp:BoundField>
        <asp:BoundField DataField="ResortCompany" HeaderText="Resort Company" 
            HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" 
            HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left" BackColor="DarkSeaGreen" Font-Size="Medium" 
                ForeColor="White" Wrap="False"></HeaderStyle>
            <ItemStyle Wrap="False" />
        </asp:BoundField>
    </Columns>
</asp:GridView>
</div>


<div>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
</div>
</asp:Content>

