<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PMTrackSchedulingItems.aspx.vb" Inherits="Maintenance_PMTrackSchedulingItems" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<ul class="ul-none">
    <li>
        <input runat="server" type="radio" name="select-search" value="ItemNumber" id="itemNumberSearch" checked="true" /><label for="itemNumberSearch">Item Number</label>
        <input runat="server" type="radio" name="select-search" value="description" id="descriptionSearch" /><label for="descriptionSearch">Description</label>
    </li>
    <li><input runat="server" type="text" size="40" name="itemNumber-search" id="searchText" /><asp:Button runat="server" Text="search" ID="submit" /></li>
    <li>
        <asp:GridView ID="gvResults" runat="server"></asp:GridView>
    </li>
</ul>





<br /><br />
<div>
    <div>
        <div style="margin-bottom:5px;">
            <asp:Button ID="refresh" runat="server" Text="refresh" />
        </div>
        
        
    </div>
    <div>
        <asp:GridView ID="gvTracking" runat="server" DataKeyNames="ItemID">
        </asp:GridView>    
    </div>
</div>



<script type="text/javascript">

    $(function () {       

        var $pastRows = $('#<%=gvTracking.ClientID %>').find('tbody tr[class=past]');
        if ($pastRows.length > 0) {
            $('<tr class=past-background><th colspan=5 align=left ><span class=arrow-down></span><span>Past (' + $pastRows.length + ')</span></th></tr>')
                .insertBefore($('#<%=gvTracking.ClientID %>').find('tbody tr[class=past]').filter(':first'))
                .addClass('clickable')
                .toggle(function () {

                    $pastRows.hide();
                    $('span:first', this).attr('class', 'arrow-up');

                }, function () {

                    $pastRows.show();
                    $('span:first', this).attr('class', 'arrow-down');
                });
        }


        var $futureRows = $('#<%=gvTracking.ClientID %>').find('tbody tr[class=future]');
        if ($futureRows.length > 0) {
            $('<tr class=future-background><th colspan=5 align=left ><span class=arrow-down></span><span>Future (' + $futureRows.length + ')</span></th></tr>')
                .insertBefore($('#<%=gvTracking.ClientID %>').find('tbody tr[class=future]').filter(':first'))
                .addClass('clickable')
                .toggle(function () {

                    $futureRows.hide();
                    $('span:first', this).attr('class', 'arrow-up');

                }, function () {

                    $futureRows.show();
                    $('span:first', this).attr('class', 'arrow-down');
                });
        }


        var $presentRows = $('#<%=gvTracking.ClientID %>').find('tbody tr[class=present]');
        if ($presentRows.length > 0) {
            $('<tr class=present-background><th colspan=5 align=left ><span class=arrow-down></span><span>Present (' + $presentRows.length + ')</span></th></tr>')
                .insertBefore($('#<%=gvTracking.ClientID %>').find('tbody tr[class=present]').filter(':first'))
                .addClass('clickable')
                .toggle(function () {

                    $presentRows.hide();
                    $('span:first', this).attr('class', 'arrow-up');

                }, function () {

                    $presentRows.show();
                    $('span:first', this).attr('class', 'arrow-down');
                });
        }




        var $unsetRows = $('#<%=gvTracking.ClientID %>').find('tbody tr[class=un-set]');

        if ($unsetRows.length > 0) {
            $('<tr class=un-set-background><th colspan=5 align=left ><span class=arrow-down></span><span>Un-Scheduled (' + $unsetRows.length + ')</span></th></tr>')
                .insertBefore($('#<%=gvTracking.ClientID %>').find('tbody tr[class=un-set]').filter(':first'))
                .addClass('clickable')
                .toggle(function () {

                    $unsetRows.hide();
                    $('span:first', this).attr('class', 'arrow-up');

                }, function () {

                    $unsetRows.show();
                    $('span:first', this).attr('class', 'arrow-down');
                });
        }





        $('#<%=gvTracking.ClientID %>').each(function () {

            var $table = $(this);

            var $rows = $('tbody tr', $table);
            for (var i = 990; i < $rows.length; i++) {

                $('td:first-child', $rows[i]).prepend('<span class="arrow-up"></span>').addClass('clickable').click(function () {
                    var $tr = $(this).parents('tr:first');


                    if (($('span:eq(0)', $tr)).attr('class') == 'arrow-up') {
                        $('span:eq(0)', $tr).attr('class', 'arrow-down');
                    } else {
                        $('span:eq(0)', $tr).attr('class', 'arrow-up');

                    }


                });
            }



        });

        $('#<%=gvTracking.ClientID %>').find('th:nth-child(4)').hide();
        $('#<%=gvTracking.ClientID %>').find('td:nth-child(4)').hide();

        $('#<%=gvTracking.ClientID %>').find('th:first-child').attr('width', '120' + 'px');
        $('#<%=gvTracking.ClientID %>').find('th:eq(1)').attr('width', '150' + 'px');
        $('#<%=gvTracking.ClientID %>').find('th:eq(2)').attr('width', '360' + 'px');
        $('#<%=gvTracking.ClientID %>').find('th:eq(3)').attr('width', '220' + 'px');
        $('#<%=gvTracking.ClientID %>').find('th:eq(4)').attr('width', '60' + 'px');
        $('#<%=gvTracking.ClientID %>').find('th:eq(5)').attr('width', '220' + 'px');


    });


</script>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style type="text/css">

.ul-none
{
    list-style-type:none;
}

.arrow-plus
{
    display:inline-block;     
    margin:0 3px -2px 0px;
    width:16px;
    height:16px;
    background:url('../images/common/ui-icons_228ef1_256x240.png') -32px -128px no-repeat;   
}
    
.arrow-minus
{
    display:inline-block;
    margin:0px 3px -2px 1px; 
    width:16px;
    height:16px;
    background:url('../images/common/ui-icons_228ef1_256x240.png') -66px -128px no-repeat;   
    
}

.arrow-up
{
    display:inline-block;
    margin:0px 3px -2px 1px; 
    width:16px;
    height:16px;
    background:url('../images/common/ui-icons_228ef1_256x240.png') -96px -192px no-repeat;
}

.arrow-down
{
    display:inline-block;
    margin:0px 3px -2px 1px; 
    width:16px;
    height:16px;
    background:url('../images/common/ui-icons_228ef1_256x240.png') -64px -192px no-repeat;
}

.clickable
{
    cursor:pointer;
}

.past
{
    
}
.present
{
    
    
}
.future
{
    
}
.un-set
{
   
}

.present-background
{
    background-color:Silver;
}

.future-background
{
    background-color:Silver;
}

.past-background
{
    background-color:Silver;
}

.un-set-background  
{
     background-color:Silver;
}
    
    
    
    
</style>

</asp:Content>

