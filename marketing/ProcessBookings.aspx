<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ProcessBookings.aspx.vb" Inherits="marketing_ProcessBookings"  ValidateRequest="false" EnableEventValidation="false" %>
<%@ Register Src="~/controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


<script type="text/javascript">

    $(function () {
        
        $('#gv[id*=_gv]').css('border', '1px solid red');
        $('#ctl00_ContentPlaceHolder1_gvWestGate').css('border', '1px solid red');
        $('hr').css('border', '1px solid red');
        $('#ctl00_ContentPlaceHolder1_submit').click(function () {
        });
      
        var img1 = '../images/badge_circle_direction_down_24_ns.png';
        var img2 = '../images/badge_circle_direction_up_24_ns.png';    


        $('input[type=checkbox]').on('change', function (event) {
            var myId = $(this).attr('id');

        });

        $('table[id$=GridView]').each(function (index, gv) {
            var $g = $(gv);

            var $subHead = $('thead th:nth-child(1)', $g);
            $subHead.append('<img src="' + img1 + '" alt="" />');
            $('img', $subHead).addClass('clickable').click(function (event) {

                if ($(this).attr('src') == img1) {
                    $(this).attr('src', img2);
                    $(this).parents('thead').next().children('tr').fadeOut('fast');
                    $(this).parents('thead').next().next().children('tr').fadeOut('fast');
                }
                else {
                    $(this).attr('src', img1);
                    $(this).parents('thead').next().children('tr').fadeIn('fast');
                    $(this).parents('thead').next().next().children('tr').fadeIn('fast');

                }
            });
        });

    });
</script>

<style type="text/css">
    
    .clickable
    {
        cursor:pointer;
    }

    .layout-fix
    {
        table-layout:fixed;
    }
    
     
    .fill-italics
    {
        opacity:0.6;
        font-style:italic;
    }  
    
    
    
.my_btn{ 
  
         
  background-image:url('../images/common/ui-icons_228ef1_256x240.png');
  cursor:pointer;
}      


#container
{
    position:relative;
    width:200px;
    height:200px;
    border:1px solid blue;
}

#container .background
{
    background:url('../images/common/ui-icons_228ef1_256x240.png') 0px -112px;
    width:16px;
    height:16px;
    position:absolute;
    left:184px;
    top:0px;
}

  
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:HiddenField runat="server" ID="hf_Column_Names" />

<div>

    <asp:Literal runat="server" ID="lit1"></asp:Literal>
    <asp:RadioButtonList runat="server" ID="rbl1"></asp:RadioButtonList>

    <strong>Check Ins</strong>
    <table id="criteria">
        <tr id="first-row">
            <td id="first-child"><label>Begin Date</label></td>
            <td style="width:240px;">
                <uc1:datefield ID="dteSDate" Selected_Date="" runat="server" />
            </td>
        </tr>
        <tr>
            <td><label>End Date</label></td>
            <td>
                <uc1:datefield ID="dteEDate" Selected_Date="" runat="server" />
            </td>
        </tr>            
    </table>

    <br />

<div style="width:700px;">
<fieldset>
    
    <legend><strong>Resort</strong></legend>

    <div style="width:50%;overflow:auto;height:170px;float:left;">
        <p style="margin:12px;"><strong>Companies</strong>
        <asp:CheckBoxList runat="server" ID="cbl_resorts" Height="10px"></asp:CheckBoxList> 
        </p>
    </div>

    <div style="width:50%;overflow:auto;height:170px;">
        <p style="margin:12px;"><strong>Locations</strong>
        <asp:CheckBoxList runat="server" ID="cbl_locations" Height="10px"></asp:CheckBoxList>
        </p>                    
    </div>
</fieldset>

</div>


<div style="width:700px;">
<fieldset>
    <legend><strong>Status</strong></legend>

    <div style="width:50%;overflow:auto;height:170px;float:left;">
        <p style="margin:12px;"><strong>Reservations</strong>
        <asp:CheckBoxList runat="server" ID="cbl_statuses" Height="10px"></asp:CheckBoxList> 
        </p>
    </div>

    <div style="width:50%;overflow:auto;height:170px;">
        <p style="margin:12px;"><strong>Bookings</strong>
        <asp:CheckBoxList runat="server" ID="cbl_bookings" Height="10px"></asp:CheckBoxList>
        </p>                    
    </div>

</fieldset>
<br />
<asp:Button ID="submit" runat="server" Text="Submit" />&nbsp;
<asp:Button ID="btEE" runat="server" Text="Export To Excel" />

</div>





<div id="resultDiv" runat="server"></div>
</div>

<br />

    

<script type="text/javascript">
    $(function () {
        $('#<%= cbl_resorts.ClientID %>, #<%= cbl_locations.ClientID %>, #<%= cbl_statuses.ClientID %>, #<%= cbl_bookings.ClientID %>').each(function () {

            var $table = $(this);
            $(this).find('input[type=checkbox]:first').change(function () {
                var cb = this;
                var $rows = $('tr', $table);
                $.each($rows, function (index, row) {
                    $(row).find('input[type=checkbox]').attr('checked', cb.checked);
                    if (cb.checked == true)
                        $(row).find('input[type=checkbox]').next().css({ 'color': 'green', 'font-weight': 'bolder' });
                    else
                        $(row).find('input[type=checkbox]').next().css({ 'color': 'black', 'font-weight': 'normal' });
                });
            });

            $(this).find('input[type=checkbox]:gt(0)').change(function () {

                var cb = this;
                if (cb.checked == true) {
                    $(cb).next().css({ 'color': 'green', 'font-weight': 'bolder' });
                } else {$(cb).next().css({ 'color': 'black', 'font-weight': 'normal' });
                }
            });
        });

    });

  
    
</script>                    

</asp:Content>

