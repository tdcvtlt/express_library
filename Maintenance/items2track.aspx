<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="items2track.aspx.vb" Inherits="Maintenance_itemsinrom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">

.section-label-1
{
    color:black;  
    padding-top:7px;      
}

.clickable
{
    cursor:pointer;
}

#table-parent
{
    border-collapse:collapse;
}
</style>


<%--<link href="../styles/bootstrap/css/bootstrap.min.css" rel="Stylesheet" />--%>
<link href="../styles/css/bootstrap.min.css" rel="Stylesheet" />
<%--<link rel="Stylesheet" href="../Styles/custom/bootstrap.css" />        --%>
<script type="text/javascript" src="../scripts/jquery-1.11.2.min.js"></script>
<script type="text/javascript" src="../styles/bootstrap/js/bootstrap.min.js"></script>


<script type="text/javascript">
    function navigate() {
        window.top.location.href = 'items2track.aspx';//Parent function that redirects
    }

    $(function () {
        $('a[id^=remove-item2trackid-]').on('click', function (e) {

            var id = $(this).attr('id');
            var pmitem2trackid = id.substr(id.lastIndexOf('-') + 1);

            modal.mwindow.open('pmtasks.aspx?mvView=mvItem2Track&action=remove&pmitem2trackid=' + pmitem2trackid,
                'preventivemaintenance', 690, 400);
            e.preventDefault();
        });

        $('a[id^=add-item2trackid-]').on('click', function (e) {

            var id = $(this).attr('id');
            var pmitem2trackid = id.substr(id.lastIndexOf('-') + 1);

            modal.mwindow.open('pmtasks.aspx?mvView=mvItem2Track&action=add&pmitem2trackid=' + pmitem2trackid,
                'preventivemaintenance', 690, 400);

            e.preventDefault();
        });

        $('a[id^=edit-item2trackid-]').on('click', function (e) {

            var id = $(this).attr('id');
            var pmitem2trackid = id.substr(id.lastIndexOf('-') + 1);

            modal.mwindow.open('pmtasks.aspx?mvView=mvItem2Track&action=edit&pmitem2trackid=' + pmitem2trackid,
                'preventivemaintenance', 690, 400);

            e.preventDefault();
        });

        $('#href-setup-buildings').on('click', function (e) {
            window.open('pmbuilding.aspx');
            e.preventDefault();
        });

        $('#href-schedules').on('click', function (e) {
            window.open('../reports/maintenance/pmschedules.aspx');
            e.preventDefault();
        });
    });

    $(function () {

        $('#table-parent thead th').css({ 'text-align': 'left', 'font': 'bold 1.2em George, serif', 'color': '#FFFFFF', 'background-color': '#A52A2A', 'padding': '5px', 'width': '200px', 'border-bottom': '10px solid white' });
        $('#table-parent tbody tr.row-top:odd').css({ 'align': 'left', 'font': 'bold 1.2em George, serif', 'color': 'white', 'background-color': '#8B4513', 'padding': '7px' });
        $('#table-parent tbody tr.row-top:even').css({ 'align': 'left', 'font': 'bold 1.2em George, serif', 'color': 'white', 'background-color': '#8B4513', 'padding': '7px' });

        $('#table-parent thead tr:eq(0) th').css({ 'color': 'white', 'background-color': 'green' });
        $('#table-parent thead tr:eq(0) th a').css({ 'color': 'white' });
        $('#table-parent thead tr:eq(1) th').css({ 'color': 'white', 'background-color': 'blue' });

    });



    $(function () {

        var img_down = '../images/badge_circle_direction_down_24_ns.png';
        var img_up = '../images/badge_circle_direction_up_24_ns.png';

        var $updownImage = $('<img src="' + img_up + '" style=vertical-align:middle;/>')
        .addClass('clickable').prependTo($('div.section-label-1'))
        .on('click', function (evt) {
            if ($(this).attr('src') == img_up) {

                var $img = $(this);
                var item2trackid = $img.parent().attr('item2trackid');
                var section = $img.parent().attr('section-type');
                var count = $img.parent().text().substr($img.parent().text().length - 1);

                $(this).attr('src', img_down);

                if (section == "unit") {
                    $.ajax({
                        url: 'items2track.aspx/LoadUnit',
                        data: '{item2trackid:"' + item2trackid + '"}',
                        type: 'post',
                        contentType: "application/json",
                        success: function (data) {
                            var obj = jQuery.parseJSON(data.d);

                            var $table = $('<table id=table-unit cellspacing=0 cellpadding=0 border-collpase:collapse><thead><th colspan=2>Action</th><th>Name</th><th>Date Added</th><th>Date Removed</th><th>Description</th></thead></table>');
                            $table.insertAfter($img.parent());
                            $table.wrap('<div></div>');
                            $('thead th', $table).css({ 'border-bottom': '3px solid blue', 'width': '140px' });

                            $.each(obj, function (index, d) {

                                var $td = $('<td></td>').html($('<a href=# id=' + d['PMITEMID'] + '>Remove</a>')
                                    .on('click', function (event) {
                                        modal.mwindow.open('pmtasks.aspx?mvView=vwItemEditDelete&action=remove' +
                                                '&pmitemid=' + $(this).attr('id'),
                                                'winpop',
                                                550, 400);
                                        event.preventDefault();
                                    })
                                );

                                var $td_edit = $('<td></td>').html($('<a href=# id=' + d['PMITEMID'] + '>Edit</a>').on('click', function (evt) {
                                    modal.mwindow.open('pmtasks.aspx?mvView=vwItemEditDelete&action=edit' +
                                                '&pmitemid=' + $(this).attr('id'),
                                                'winpop',
                                                550, 400);
                                    evt.preventDefault();
                                })
                                );

                                var $tr = $('<tr></tr>').append($td_edit);
                                $td.appendTo($tr);

                                $('<td></td>').html(d['ROOMNUMBER']).appendTo($tr);
                                $('<td></td>').html(d['DATEADDED']).appendTo($tr);
                                $('<td></td>').html(d['DATEREMOVED']).appendTo($tr);
                                $('<td></td>').html(d['DESCRIPTION']).appendTo($tr);
                                $table.append($tr);

                            }); // each

                            var $tfoot = $('<tfoot><tr><td colspan=6>&nbsp;</td></tr></tfoot>');
                            var $tdAdd = $('<tr><td colspan=6><a href=# >Add</a></td></tr>');

                            $tdAdd.children('td').children('a:first').bind('click', function (evt) {
                                modal.mwindow.open('pmtasks.aspx?mvView=vwItemAdd&action=add&item2trackid=' + item2trackid + '&type=roomid',
                                                                    'winpop',
                                                                    550, 600);
                                evt.preventDefault();
                            });
                            $tdAdd.appendTo($tfoot);
                            $table.append($tfoot);
                        } // success
                    }); //$.ajax
                } // section == unit
                else if (section == "task") {

                    $.ajax({
                        url: 'items2track.aspx/LoadTask',
                        data: '{item2trackid:"' + item2trackid + '"}',
                        type: 'post',
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {

                            var $div = $('<div></div>');
                            $div.append(data.d).insertAfter($img.parent());
                            $('#table-task thead th', $div).css({ 'border-bottom': '3px solid blue', 'width': '140px' });

                            $('a', $div).each(function (index, row) {

                                $(this).on('click', function (event) {

                                    var id = $(this).attr('id');
                                    var pmitem2trackid = id.substr(id.indexOf('~') + 1);
                                    var taskid = id.substr(id.lastIndexOf('-') + 1, id.indexOf('~') - 1 - id.lastIndexOf('-'));

                                    var action;

                                    if (id.indexOf('edit') >= 0) {
                                        action = 'edit'
                                    } else if (id.indexOf('remove') >= 0) {
                                        action = 'remove';
                                    } else if (id.indexOf('add') >= 0) {
                                        action = 'add';
                                    }

                                    modal.mwindow.open('pmtasks.aspx?mvView=mvTask&action=' + action + '&pmitem2trackid=' + pmitem2trackid + '&taskid=' + taskid,
                                            'preventivemaintenance',
                                            550, 350);

                                    event.preventDefault();
                                });

                            });
                        }
                    });

                }
                else if (section == "building") {

                    $.ajax({
                        url: 'items2track.aspx/LoadBuildings',
                        data: '{item2trackid:"' + item2trackid + '"}',
                        type: 'post',
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {

                            var $div = $('<div></div>');
                            $div.append(data.d).insertAfter($img.parent());
                            $('#table-building thead th', $div).css({ 'border-bottom': '3px solid blue', 'width': '140px' });

                            $('a', $div).each(function (index, row) {

                                $(this).on('click', function (event) {

                                    var id = $(this).attr('id');
                                    var item2trackid = id.substr(id.indexOf('~') + 1);
                                    var pmbuildingid = id.substr(id.lastIndexOf('-') + 1, id.indexOf('~') - 1 - id.lastIndexOf('-'));
                                    var pmitemid = $(this).attr('pmitemid');

                                    var action;

                                    if (id.indexOf('edit') >= 0) {
                                        action = 'edit'
                                    } else if (id.indexOf('remove') >= 0) {
                                        action = 'remove';
                                    } else if (id.indexOf('add') >= 0) {
                                        action = 'add';
                                    }

                                    if (action === "add") {
                                        modal.mwindow.open('pmtasks.aspx?mvView=vwItemAdd&action=add&item2trackid=' + item2trackid + '&type=pmbuildingid',
                                                                    'winpop',
                                                                    550, 600);
                                    } else if (action === "remove") {
                                        modal.mwindow.open('pmtasks.aspx?mvView=vwItemEditDelete&action=' + action +
                                            '&pmitemid=' + pmitemid,
                                            'winpop',
                                            300, 300);
                                    }
                                    else if (action === "edit") {
                                        modal.mwindow.open('pmtasks.aspx?mvView=vwItemEditDelete&action=' + action +
                                            '&pmitemid=' + pmitemid,
                                            'winpop',
                                            500, 400);
                                    }

                                    event.preventDefault();
                                });

                            });
                        }
                    });

                    evt.preventDefault();
                }
            } else {
                $(this).attr('src', img_up);
                $(this).parent().next('div').remove();

            }
        });

//        if ($.browser.msie) {
//            $updownImage.trigger('click');
//        }


    });


    
   

</script>


<script type="text/javascript" src="../scripts/noty/packaged/jquery.noty.packaged.min.js"></script>
<script type="text/javascript">

    function generate(type) {
        var n = noty({
            text: type,
            type: type,
            dismissQueue: false,
            layout: 'topCenter',
            //layout: 'topRight',
            theme: 'defaultTheme'
        });
        console.log(type + ' - ' + n.options.id);
        return n;
    }

    $(function () {


        $('#<%= refresh_btn_top.ClientID %>').on('click', function () {
            
            if ($('#<%= tb_search.ClientID %>').val() == '') {
                var a = generate('error');
                $.noty.setText(a.options.id, 'Search box is empty!');

                setTimeout(function () {
                    $.noty.closeAll();
                }, 1500);

                return false;
            }

        });


    });
</script>



</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!--
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="lnkTabItem" runat="server">Track Items</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="lnkTabTasks" runat="server">Tasks</asp:LinkButton></li>        
    </ul>
    -->
    <div>
        <asp:MultiView runat="server" ID="MultiView1">
            <asp:View runat="server" ID="MultiView1_View1">
                <h2>TRACK ITEMS</h2>
                <hr />
                <asp:GridView runat="server" ID="gvTrackItems" DataKeyNames="Item2TrackID" AutoGenerateSelectButton="true"></asp:GridView>
            </asp:View>
            <asp:View runat="server" ID="MultiView1_View2">
                <br />
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h2>TASKS</h2>
                    </div>
                    <div class="panel-body">
                        <asp:GridView runat="server" ID="gvTasks" DataKeyNames="TaskID,Item2TrackID" AutoGenerateSelectButton="true" AutoGenerateColumns="false" CssClass="table">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Task Name" />
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:BoundField DataField="Task Status" HeaderText="Status" />
                        <asp:BoundField DataField="Category" HeaderText="Department" />
                        <asp:BoundField DataField="Interval" HeaderText="Interval (Months)" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="ComboItem" HeaderText="Problem Area" />
                    </Columns>
                </asp:GridView>
                    </div>
                </div>
                

                <!-- Button trigger modal -->
                <button type="button" class="btn btn-primary btn-lg" data-toggle="modal" id="popupModal" data-target="#myModal">
  Launch demo modal
</button>

                <!-- Modal -->
                

            </asp:View>

            <asp:View runat="server" ID="MultiView1_View3">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h2>TASKS</h2>
                    </div>
                </div>
            </asp:View>

            <asp:View ID="MultiView1_View4" runat="server">
                <asp:GridView runat="server" AutoGenerateSelectButton="true" AutoGenerateColumns="false"  ID="gvTaskC">
                    <Columns>                        
                        <asp:BoundField DataField="task-name" HeaderText="Task Name" />       
                        <asp:BoundField DataField="item-track-name" HeaderText="Item Name" />      
                        <asp:BoundField DataField="room" HeaderText="Room" />      
                        <asp:BoundField DataField="building" HeaderText="Building" />                       
                    </Columns>
                </asp:GridView>

                <asp:Literal ID="litTasks" runat="server">
                </asp:Literal>
            </asp:View>
        </asp:MultiView>
    </div>
<div>
    <table>
        <tr>
            <td><asp:TextBox runat="server" ID="tb_search"></asp:TextBox></td>
            <td><asp:Button ID="refresh_btn_top" runat="server" Text="Search" /></td>
        </tr>
    </table>
</div>



<div>


<br />
<br />
<asp:HiddenField runat="server" ID="hf_item2trackid" />
<asp:Literal ID="lit_tables" runat="server"></asp:Literal>

<asp:Literal ID="litPLM" runat="server"></asp:Literal>
<br />
<asp:Literal ID="tmpLiteral" runat="server"></asp:Literal>

</div>

    


   <!-- <div>
        <div class="modal fade" id="myModal">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title">Modal title</h4>
      </div>
      <div class="modal-body">
        <p>One fine body&hellip;</p>


          <p>Status</p>

          <div class="btn-group" data-toggle="buttons" id="cbTaskStatus">
  <label class="btn btn-primary active">
    <input type="radio" name="options" id="option1" autocomplete="off" checked> Active
  </label>
  <label class="btn btn-primary">
    <input type="radio" name="options" id="option2" autocomplete="off"> Not Active
  </label>
              <label class="btn btn-primary">
    <input type="radio" name="options" id="option2" autocomplete="off"> Highly 
  </label>
</div>

          <h3>Department</h3>
          <div class="dropdown">
  <button class="btn btn-default dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-expanded="true">
    Dropdown
    <span class="caret"></span>
  </button>
  <ul class="dropdown-menu" role="menu" aria-labelledby="dropdownMenu1">
    <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Action</a></li>
    <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Another action</a></li>
    <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Something else here</a></li>
    <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Separated link</a></li>
  </ul>



              <h3>Problem Area</h3>
          <div class="dropdown">
  <button class="btn btn-warning dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-expanded="true">
    Dropdown
    <span class="caret"></span>
  </button>
  <ul class="dropdown-menu" role="menu" aria-labelledby="dropdownMenu1">
    <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Repair</a></li>
    <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Service</a></li>   
  </ul>

</div>


      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary">Save changes</button>
      </div>
    </div><!-- /.modal-content -->
  <!--</div><!-- /.modal-dialog -->
<!--</div><!-- /.modal -->



   <!-- </div>



<div>
    <div class="panel-group">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4>welcome</h4>
            </div>
        </div>
    </div>
    
</div> -->


    <script type="text/javascript">

        $(function () {

            $('#popupModal').on('click', function () {
                $('#myModal').on('shown.bs.modal', function () { });
            });
            
            $('#cbTaskStatus input[type=radio]').on('click', function () {
                var l = $(this).parent('label');
                l.parents('div.btn-group').children('label').each(function (index, element) {
                    $(element).removeClass('active');
                });
            });




        });
    </script>
</asp:Content>




