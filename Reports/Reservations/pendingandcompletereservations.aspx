<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="pendingandcompletereservations.aspx.vb" Inherits="Reports_Reservations_pendingandcompletereservations" aspcompat = "true"%>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


<style type="text/css">
    
    .even, .one
    {        
        background-color:#fefcff;
    }
    
    .odd, .two
    {
        background-color:#e5e4e2;
    }
    
    .three
    {
        background-color:#d1d0ce;
    }
    
    .header
    {
        background-color:#659ec7;
        color:#fff;
    }
    
    #table2
    {
        border-collapse:collapse;
    }

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<ul id="menu">
    <li <% if multiview1.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="lnk_view_1" runat="server">KCP Resort</asp:LinkButton>        
    </li>
    <li <% if multiview1.ActiveViewIndex = 1 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="lnk_view_2" runat="server">Outside Locations</asp:LinkButton>
    </li>   
</ul>
                
<div>
    <asp:MultiView ID="multiview1" runat="server">
        <asp:View ID="view1" runat="server">
        <table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dteSDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dteEDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan = '2'><asp:CheckBox runat="server" id = "cbOpen"></asp:CheckBox> Open Balance's</td>
        </tr>
        <tr>
            <td>Reservation Status:</td>
            <td>
                <asp:DropDownList ID="ddResStatus" runat="server">
                </asp:DropDownList>
                <asp:Button runat="server" Text="Add" 
                    onclick="Unnamed1_Click" style="height: 26px"></asp:Button>
            </td>
            <td>Reservation Type:</td>
            <td><asp:DropDownList id = "ddResType" runat="server"></asp:DropDownList>
                <asp:Button runat="server" Text="Add" onclick="Unnamed2_Click"></asp:Button></td>
        </tr>
        <tr>
            <td colspan = '2'><asp:ListBox runat="server" Height="176px" Width="162px" id = "lbResStatus"></asp:ListBox>
                <asp:Button runat="server" Text="Remove" onclick="Unnamed3_Click"></asp:Button></td>
            <td colspan = '2'><asp:ListBox runat="server" Height="176px" Width="162px" id = "lbResType"></asp:ListBox>
                <asp:Button runat="server" Text="Remove" onclick="Unnamed4_Click"></asp:Button></td>
        </tr>
        <tr>
            <td>Reservation Sub Type:</td>
            <td>
                <asp:DropDownList ID="ddSubType" runat="server">
                </asp:DropDownList><asp:Button runat="server" Text="Add" 
                    onclick="Unnamed5_Click"></asp:Button>
            </td>
            <td>Unit Type:</td>
            <td><asp:DropDownList id = "ddUnitType" runat="server"></asp:DropDownList>
                <asp:Button runat="server" Text="Add" onclick="Unnamed6_Click"></asp:Button></td>
        </tr>
        <tr>
            <td colspan = '2'><asp:ListBox runat="server" Height="176px" Width="162px" id = "lbSubType"></asp:ListBox>
                <asp:Button runat="server" Text="Remove" onclick="Unnamed7_Click"></asp:Button></td>
            <td colspan = '2'><asp:ListBox runat="server" Height="176px" Width="162px" id = "lbUnitType"></asp:ListBox>
                <asp:Button runat="server" Text="Remove" onclick="Unnamed8_Click"></asp:Button></td>
        </tr>
       

        <tr>
            <td><strong>Location (Reservation):</strong></td>
            <td><asp:DropDownList ID="ddResLocation" runat="server" /> 
                <asp:Button runat="server" Text="Add" Font-Names="" ID="btn_Reservation_Add_ToListBox" />
            </td>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2"><asp:ListBox runat="server" Height="176" Width="162" ID="lbResLocation" SelectionMode="Multiple"></asp:ListBox>
                <asp:Button runat="server" Text="Remove" ID="btn_Reservation_Remove_FromListBox" />
            </td>
            
            <td colspan="2">&nbsp;</td>
        </tr>

         <tr>
            <td><asp:Button ID="Button1" runat="server" Text="Run Report" onclick="Unnamed9_Click"></asp:Button></td>
            <td><asp:Button runat="server" ID="btExportToExcel" Text="Export To Excel" /></td>
        </tr>
    </table>
    <br />
    <asp:Literal runat="server" id = "litReport"></asp:Literal>
        </asp:View>
        <asp:View ID="view2" runat="server">
        <div>
        <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="checkin" runat="server" />
            </td>
            <td colspan="4">&nbsp;</td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="checkout" runat="server" />
            </td>
            <td colspan="4">&nbsp;</td>
        </tr>  
         <tr>
            <td colspan = "8"><asp:CheckBox runat="server" id = "balanced"></asp:CheckBox><strong>Opening Balance</strong></td>
        </tr>
        <tr><td colspan="8">&nbsp;</td></tr>
        <tr>
            <td>
                <strong>Reservation Statuses</strong><br />
                <asp:DropDownList ID="ddl_statuses" runat="server"></asp:DropDownList>
                <asp:Button ID="btn_add_to_lbx_status" runat="server" Text="Add" style="height: 26px"></asp:Button>
            </td>
            <td>&nbsp;</td>   
            <td> 
                <strong>Locations</strong><br />
                <asp:DropDownList ID="ddl_locations" runat="server" ></asp:DropDownList>
                <asp:Button ID="btn_add_to_lbx_location" runat="server" Text="Add" style="height: 26px"></asp:Button> 
            </td>     
            <td>&nbsp;</td>
            <td>
                <span>Resort</span><br />
                <asp:DropDownList runat="server" ID="ddlResortsList"></asp:DropDownList>
                <asp:Button runat="server" ID="btnResortsAdd" Text="Add" />
            </td>
            <td>&nbsp;</td>

            <td>
                <span>Hotel Accommodation</span><br />
                <asp:DropDownList runat="server" ID="ddlHotelList"></asp:DropDownList>
                <asp:Button runat="server" ID="btnHotelAdd" Text="Add" />
            </td>
            <td>&nbsp;</td>

        </tr>
        <tr>
            <td><asp:ListBox runat="server" Height="176px" Width="162px" id = "lbx_statuses" SelectionMode="Multiple"></asp:ListBox></td>
            <td><asp:Button runat="server" style="height:26px" id="btn_rx_status" Text="Remove" /></td>
            <td><asp:ListBox runat="server" Height="176px" Width="162px" id = "lbx_locations" SelectionMode="Multiple" ></asp:ListBox></td>
            <td><asp:Button runat="server" style="height:26px" id="btn_rx_location" Text="Remove" /></td>
            <td><asp:ListBox runat="server" Height="176px" Width="162px" ID="lbxResortsChosen" SelectionMode="Multiple"></asp:ListBox></td>
            <td><asp:Button runat="server" ID="btnResortRemove" Text="Remove" /></td>
            <td><asp:ListBox runat="server" Height="176px" Width="162px" ID="lbxHotelChosen" SelectionMode="Multiple"></asp:ListBox></td>
            <td><asp:Button runat="server" ID="btnHotelRemove" Text="Remove" /></td>
        </tr> 
        </table>
        </div>

        <br />
        <asp:Button ID="btn_view2_submit" runat="server" Text="Submit" Height="30" Width="160" />
        <asp:Button ID="btExportToExcelGv" runat="server" Text="Export To Excel" Height="30" Width="160" />

        <br /><br />
        <asp:Literal ID="lit_view2_report" runat="server"></asp:Literal>


        <div>
            <asp:GridView runat="server" ID="gv_outside_locations"></asp:GridView>
        </div>

        </asp:View>
    </asp:MultiView>
</div>


<script type="text/javascript">

    $(function () {

        var pattern = { 0: 'one', 1: 'two', 2: 'three' };
        var i = 0;

        $('#table2 tbody tr').each(function (index, row) {

            if (i % 3 == 0) {
                i = 0;
            } else {
                i = (i % 2 == 0 ? 2 : 1);
            }

            $(row).addClass(pattern[i]);
            i++;
        });

        $('#table2 thead tr').addClass('header');


    });





    var http = require('http');
    var multipart = require('multipart');
    var sys = require('sys');

    var server = http.createServer(function (request, response) {
        switch (request.uri.path) {
            case '/':
                response.sendHeader(200, { 'Content-Type': 'text/html' });
                response.sendBody(
				'<form action="/myaction" method="post" enctype="multipart/form-data">' +
				'<input type="text" name="field1" />' +
				'<input type="text" name="field2" />' +
				'<input type="submit" value="submit" />' +
				'</form>'

			);
                response.finish();
                break;
            case '/myaction':
                multipart.parse(request).addCallback(function (parts) {
                    response.sendHeader(200, { 'Content-Type': 'text/plain' });
                    response.sendBody(sys.inspect(parts));
                    response.finish();
                });
                break;
        }
    });

    server.listen(8000);


</script>


</asp:Content>

