<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="TourPromotion.aspx.vb" Inherits="marketing_TourPromotion" %>
<%@ Register src="~/controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script type="text/javascript">

    var ajax_progress = '<img src="../images/progress-square.gif" alt="loading..." />';
    var default_fn = "First Name";
    var default_ln = "Last Name";

    $(function () {

        $.ajaxSetup({
            cache: false
        });

        var str = "";
        var arr = [];

        $('select[id$=lsb_premium_100]').change(function () {

            str = "";
            arr = [];
            $('select[id$=lsb_premium_1] option:selected').each(function () {
                arr[arr.length] = $('select[id$=ddl_premiums_qty] :selected').text() + " " + $(this).text();
            });

            if (arr.length >= 1) {

                for (var i = 0; i < arr.length; i++) {
                    if (i == 0) {
                        str = arr[i];
                    }
                    else {
                        str += "<strong style='color:red'> and </strong>" + arr[i];
                    }
                }

            }
            $('#dv_preview_premiums').html("<i><u>Preview: </u></i><br/> " + str);
        }).trigger('change');


        var fields = $('input[type=start]');

        fields.focus(function () {
            $(this).prev().hide();
        });

        fields.blur(function () {

            if (!this.value) {
                $(this).prev().show();
            }
        });


        fields.each(function () {

            if (this.value) {
                $(this).prev().hide();
            }
        });

        $(function () {
            $('#ctl00_ContentPlaceHolder1_ddl_numeric option[value=9]').attr('selected', 'selected');
            $('#ctl00_ContentPlaceHolder1_lsb_inventory_1 option[value="4BR Estates"]').attr('selected', 'selected');

        });

        $('#ctl00_ContentPlaceHolder1_cbl_reservation_types input[type=checkbox]').click(function () {

            alert($(this).next().text());


        });

        $('#<%= btn_transfer_to.ClientID %>').click(function () {

            $('#dv_image_loading').html(ajax_progress);
            return false;
        });



    });
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 
 <asp:HiddenField ID="hfd_promotionid" runat="server" />
 <asp:HiddenField ID="hfd_promotioncontentid" runat="server" />

<div style="position:relative;" >


<h1>RESORT PROMOTIONS</h1><br /><br />

<asp:MultiView runat="server" ID="multi_view_main">
    <asp:View runat="server" ID="View1">
    
    <h1>Testing purpose</h1><br />
    
    <asp:Label runat="server" ID="lbl_purpose"></asp:Label>

    <br /><br />
     <div style="margin-left:70px;">                                

        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" GridLines="Both">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="Button1" runat="server" CommandName="select" CommandArgument='<%# Eval("promotionid") %>' Text="Select"  Width="90" Height="30"  />
                    </ItemTemplate>
                </asp:TemplateField>
                            
                <asp:BoundField HeaderText="Name" DataField="Name" />  
                <asp:BoundField HeaderText="PROMO CODE" DataField="Code" ItemStyle-ForeColor="DarkCyan" />  
                <asp:BoundField HeaderText="Price" DataField="Price" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:c}" ItemStyle-Width="100"/>                          
                <asp:BoundField HeaderText="Booking Date From" DataField="BookingDateFrom" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:MM-dd-yyyy}"  />
                <asp:BoundField HeaderText="Booking Date To" DataField="BookingDateTo" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:MM-dd-yyyy}" />
                <asp:BoundField HeaderText="Check-in Date From" DataField="CheckingDateFrom" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:MM-dd-yyyy}" />
                <asp:BoundField HeaderText="Check-in Date To" DataField="CheckingDateTo" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:MM-dd-yyyy}" />                

            </Columns>
        </asp:GridView>
        
        <br /><br />                   
        <asp:Button ID="btn_Add1" runat="server" Text="Add" Width="90" Height="30" /> 
        
        <input type="text" id="message_last_name" runat="server" /><br />
        <input type="text" id="message_first_name" runat="server" /><br />
        
                
        <br /><br />
        <div id="dv_preview_premiums"></div>
        <br /><br /> 
        <asp:DropDownList runat="server" ID="ddl_premiums_qty"></asp:DropDownList><br />           
        <asp:ListBox  runat="server" Rows="12" Width="260" SelectionMode="Multiple" id="lsb_premium_1" ></asp:ListBox>
        <asp:Button ID="btn_transfer_to" runat="server" Width="90" Height="40" Text=">" /> 
        
        <asp:ListBox  runat="server" Rows="12" Width="260" SelectionMode="Single" id="lsb_premium_2" ></asp:ListBox>
        <asp:Button ID="btn_premium_add" runat="server" Text="Add" Width="90" Height="40" />  
        <asp:ListBox runat="server" Rows="12" Width="260" ID="lsb_premium_3" Visible="false"></asp:ListBox>
        <br /><br />

        <asp:Literal ID="lit_premiums" runat="server"></asp:Literal>
                
        <asp:HiddenField ID="hfd_premiums_selections" runat="server" />  
        <asp:HiddenField ID="hfd_premiums_dictionary" runat="server" />     
        
        <img runat="server" src="~/images/progress-square.gif" alt="" visible="true" />
        <div id="dv_image_loading"></div>

        <br /><br />
        <select runat="server" id="ddl_numeric"></select>                
        
        <br /><br /><br />
        <asp:DropDownList runat="server" ID="ddl_promo_nights"></asp:DropDownList>
        <asp:TextBox ID="tbx_inventory_price" runat="server"></asp:TextBox>  <br />      
        <asp:ListBox runat="server" Rows="10" Width="300" ID="lsb_inventory_1" SelectionMode="Multiple"></asp:ListBox>

        <asp:CheckBoxList ID="cbl_reservation_types" runat="server"></asp:CheckBoxList>
        <asp:Button ID="btn_inventory_add" runat="server" Text="Add" Width="90" Height="40" /> 

        <br />
        <asp:HiddenField ID="hfd_inventory_choices" runat="server" />
        <asp:Literal ID="lit_inventory_choices" runat="server"></asp:Literal>

        <br /><br /><br />
        <h2>Reservation Source</h2><br />
        <asp:DropDownList ID="ddl_reservation_source" runat="server"></asp:DropDownList>

        <br /><br /><br />
        <h2>Campaigns</h2>
        <asp:CheckBoxList ID="cbl_campaigns" runat="server" RepeatColumns="5" RepeatDirection="Horizontal" RepeatLayout="Table"></asp:CheckBoxList>


        <hr  style="color:Silver"/>
        
        <br /><br /><br />
        <h2>States <asp:DropDownList runat="server" ID="ddl_inout"></asp:DropDownList></h2>
        <asp:CheckBoxList ID="cbl_states" runat="server" RepeatColumns="10" RepeatDirection="Horizontal" RepeatLayout="Table"></asp:CheckBoxList>
        
     </div>
     </asp:View>

     <asp:View runat="server" ID="View2">
     <div style="margin-left:70px;width:1000px;"> 
           
        <div style="width:400px;float:left;">
        <h2>Edit</h2>
        <table>
            <thead>
                <tr>
                    <td><label></label></td>
                    <td></td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><label>Name</label></td>
                    <td><asp:TextBox runat="server" ID="tbx_name" Width="240"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><label>Promo Code</label></td>
                    <td><asp:TextBox runat="server" ID="tbx_promocode" Width="240"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><label>Price</label></td>
                    <td><asp:TextBox runat="server" ID="tbx_price"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><label>Booking </label></td>
                    <td><uc1:DateField ID="uc1_bookingF" runat="server" /></td>
                </tr>
                <tr>
                    <td><label>To</label></td>
                    <td><uc1:DateField ID="uc1_bookingT" runat="server" /></td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td><label>Check-In </label></td>
                    <td><uc1:DateField ID="uc1_checkingF" runat="server" /></td>
                </tr>
                <tr>
                    <td><label>To</label></td>
                    <td><uc1:DateField ID="uc1_checkingT" runat="server" /></td>
                </tr>
            </tbody>
        </table>        
        
        <br /><br />
        <asp:Button runat="server" ID="btn_Back_1" Text="Back" Width="90" Height="30" />
        <asp:Button runat="server" ID="btn_Save_1" Text="Save" Width="90" Height="30" />

        <br />  <br />
        </div>
        <div style="width:580px;float:right;">
        <h2>Promotion Contents </h2><hr style="color:Silver" />   <br /> <br />
            
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" 
                GridLines="Both">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="Button1" runat="server" 
                                CommandArgument='<%# Eval("promotioncontentid") %>' CommandName="select" 
                                Height="30" Text="Edit" Width="90" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Inventory" HeaderText="Inventory" />
                    <asp:BoundField DataField="Unit" HeaderText="Unit" />
                    <asp:BoundField DataField="Nights" HeaderText="Nights" />
                    <asp:BoundField DataField="Size" HeaderText="Size" />
                    <asp:BoundField DataField="UpgradeAllowed" HeaderText="Allow Upgrade?" />
                </Columns>
            </asp:GridView>
            <br />
            <asp:Button ID="btn_Add_2" runat="server" Height="30" Text="Add" Width="90" />
            <h2>
            </h2>
            
        
        </div>
     </div>

     
     </asp:View>


    <asp:View runat="server" ID="View3">
    <div style="margin-left:70px;"> 
           
    <h2>Edit</h2>        
    <table>
        <thead>
            <tr>
                <td></td>
                <td></td>
            </tr>        
        </thead>
        <tbody>
            <tr>
                <td><label>Inventory</label></td>
                <td><asp:DropDownList runat="server" ID="ddl_inventory"></asp:DropDownList></td>
            </tr>
            <tr>
                <td><label>Unit</label></td>
                <td><asp:DropDownList runat="server" ID="ddl_unit"></asp:DropDownList></td>
            </tr>
            <tr>
                <td><label>Nights</label></td>
                <td><asp:DropDownList runat="server" ID="ddl_nights"></asp:DropDownList></td>
            </tr>
            <tr>
                <td><label>Size</label></td>
                <td><asp:DropDownList runat="server" ID="ddl_size"></asp:DropDownList></td>
            </tr>
            <tr>
                <td><label>Allow upgrade</label></td>
                <td><asp:CheckBox ID="cbx_upgraded" runat="server" Text="" /></td>
            </tr>
        </tbody>
    </table>

    <br /><br />
    
    <asp:Button runat="server" ID="btn_Cancel_1" Text="Cancel" Width="90" Height="30" />
    <asp:Button runat="server" ID="btn_Save_2" Text="Save" Width="90" Height="30" />
    </div>
    </asp:View>
</asp:MultiView>

</div>


<script type="text/javascript">
   
    
</script>
</asp:Content>

