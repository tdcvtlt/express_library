<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReservationWizard.aspx.vb" Inherits="marketing_ReservationWizard" %>
<%@ Register Src="~/controls/DateField.ascx" tagname="DateField" tagprefix="uc1"  %>
<%@ Register Src="~/controls/Select_Item.ascx" TagName="DropDown" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reservation Wizard</title>

    <script type="text/javascript" src="../scripts/jquery-1.7.1.min.js"></script>

    <script type="text/javascript">

        $(function () {

            $('span[id=lbl_programName]').css({'font-size':'48px'});



        });        
    </script>
    <style type="text/css">
        
        body
        {
            font-family:DejaVu Sans;
        }
        
        option
        {
            font-family:DejaVu Sans;
            font-size:small;
        }    
        
        #table_1_view1
        {
            border-collapse:collapse;
        }
        
        #ddl_room_size, #ddl_reservation_location, #ddl_reservation_type, #ddl_unit_type, #ddl_nights
        {
            width:200px;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">

       

    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="scriptManager1"></asp:ScriptManager>
    <div>
        <asp:Label ID="lbl_programName" runat="server"></asp:Label>
    </div>       

    <div style="width:600px;border:1px solid blue;">

    <asp:UpdatePanel runat="server" ID="updatePanel1">
    <ContentTemplate>
    <asp:HiddenField runat="server" ID="hfd_packageId" />
    <asp:HiddenField runat="server" ID="hfd_packageIssuedId" />
    <asp:HiddenField runat="server" ID="hfd_packageName" />
    <asp:HiddenField runat="server" ID="hfd_prospectId" />
    <asp:HiddenField runat="server" ID="hfd_status" />
    <asp:HiddenField runat="server" ID="hfd_cost" />
    <asp:HiddenField runat="server" ID="hfd_checkInDate" />
    <asp:HiddenField runat="server" ID="hfd_checkOutDate" />
    <asp:HiddenField runat="server" ID="hfd_inventoryType" />
    <asp:HiddenField runat="server" ID="hfd_campaign" />
    
    
    
        
    <div id="dv_region_err">
    <asp:Label runat="server" ID="lbl_view1_err"></asp:Label>
    </div>
    <br />
    <br />
    <asp:MultiView runat="server" ID="mtv_wizard">
        <asp:View runat="server" ID="view_1">
            <div>
                        <asp:Label runat="server" ID="lbl_err"></asp:Label>                                          
                        <table id="table_1_view1" border="1" cellpadding="5px">
                            <tr>
                                <td><span>Location</span></td>
                                <td><span style="margin-left:0;"><asp:DropDownList ID="ddl_reservation_location" runat="server"></asp:DropDownList></span></td>
                                <td></td>
                            </tr>
                             <tr>
                                <td><span>Check-In</span></td>
                                <td><span><uc1:DateField runat="server" ID="dtf_reservation_checkin"   /></span></td>
                                <td><asp:Label runat="server" ID="lbl_reservation_checkout"></asp:Label></td>
                            </tr>
                             <tr>
                                <td><span>Total Nights</span></td>
                                <td><span><asp:DropDownList runat="server" ID="ddl_nights" AutoPostBack="true"></asp:DropDownList></span></td>
                                <td><asp:CheckBox runat="server" ID="cbx_additional_nights" Text="Additional nights" AutoPostBack="true" />&nbsp;&nbsp;
                                <asp:DropDownList runat="server" ID="ddl_additional_nights" AutoPostBack="true"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td><span>Reservation Type</span></td>
                                <td><span><asp:DropDownList ID="ddl_reservation_type" runat="server"></asp:DropDownList></span></td>
                                <td><span><asp:Label runat="server">Inventory :</asp:Label><asp:DropDownList runat="server" AutoPostBack="true" ID="ddl_inventory_type"></asp:DropDownList></span></td>
                            </tr> 
                            <tr>
                                <td><span>Unit Type</span></td>
                                <td><span><asp:DropDownList runat="server" ID="ddl_unit_type"></asp:DropDownList></span></td>
                                <td><span></span></td>
                            </tr>
                            <tr>
                                <td><span>Size</span></td>
                                <td><span><asp:DropDownList runat="server" ID="ddl_room_size"></asp:DropDownList></span></td>
                                <td><span></span></td>
                            </tr>
                        </table>

                    </div>

                    <asp:Button runat="server" ID="btn_GET" Text="GET" />
                
        </asp:View>

        <asp:View runat="server" ID="view_2">
            <div>
                <h1>View #2</h1>
                <asp:Label runat="server" ID="lbl_view2_err"></asp:Label>
                <asp:Image runat="server" ID="img_progress" ImageUrl="~/images/progress-square.gif" />
                
                <asp:GridView ID="gvw_Inventories" runat="server" GridLines="Both" AutoGenerateColumns="false" DataKeyNames="roomid" >
                    <Columns>                        
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button id="btn_room_choose" runat="server" Text="Choose"  />                                
                            </ItemTemplate>
                        </asp:TemplateField>   
                        
                        <asp:BoundField HeaderText="Room Number" DataField="RoomNumber" />                   
                        <asp:BoundField HeaderText="Size" DataField="RoomType1" />                   
                        <asp:BoundField HeaderText="CheckIn Date" DataField="RoomSubType1" />                   
                    </Columns>
                </asp:GridView>
            </div>
        </asp:View>

        <asp:View runat="server" ID="view_3">
            <div>
                <h1>View #3</h1>
                <asp:Label runat="server" ID="lbl_view3_err"></asp:Label>    
            </div>
        </asp:View>

          <asp:View runat="server" ID="view_4">
            <div>
                <h1>View #4</h1>
                <asp:Label runat="server" ID="Label3"></asp:Label>    
            </div>
        </asp:View>
    </asp:MultiView>
    
    <br /><br /><br />
        
    <div>
    <div style="width:50%;float:left;"><asp:Button ID="btn_previous" runat="server" Text="Previous" Width="100" Height="36"  /></div>
    <div style="width:50%;"><span style="margin-left:120px;"><asp:Button ID="btn_next" runat="server" Text="Next" Width="100" Height="36"  /></span></div>        
    </div> 

    <br />
          
    </ContentTemplate> 
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btn_next" />
    </Triggers>  
    </asp:UpdatePanel>            

   
    
    
       
    </div>
    </form>
</body>
</html>
