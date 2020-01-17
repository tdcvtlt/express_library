<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="itemsdetail2track.aspx.vb" Inherits="Maintenance_itemsdetail2track" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>

<div>
    <label for="search-room-id">Room</label><input type="radio" id="search-room-id" caption="search by room number" name="search-select" checked="checked" value="roomid" />
    <label for="search-unit-id">Unit</label><input type="radio" id="search-unit-id" caption="search by unit" name="search-select" value="unitid" />
    <br />
    <input type="text" size="30" id="search-room-number" value="search..."  />
</div>


<div>
    <asp:Literal ID="lit0" runat="server"></asp:Literal> 
</div>

<div>

</div>


</div>


<script type="text/javascript">


</script>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


<script type="text/javascript">

    $(function () {

        var make_opaque = function ($input) {

            var valueText = $input.val();

            $input.addClass('transparent')
                .val(valueText)
                .focus(function () {                    
                    if (this.value == valueText) {
                        $(this).removeClass('transparent').val('');
                    }
                })
                .blur(function () {
                    if (this.value == '') {
                        $(this).addClass('transparent').val(valueText);
                    }
                });

        }

        make_opaque($('#search-room-number'));

    });  // function()

    $(function () {
        var selectedItem = null;
        $('#search-room-number').keyup(function (event) {

            if (event.keyCode > 40 || event.keyCode == 8) {
                var text = $(this).val();

                $.ajax({
                    url: 'itemsdetail2track.aspx/search',
                    data: '{beginsWith:"' + text + '"}',
                    type: 'post',
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data.d) {
                            var arr = $.parseJSON(data.d); 
                        
                        } 
                    } 
                });

            } // if (keyCode > 40)
            else if (event.keyCode == 38 && selectedItem !== null) { event.preventDefault(); }
            else if (event.keyCode == 40 && selectedItem !== null) { event.preventDefault(); }
            else if (event.keyCode == 27 && selectedItem !== null) { }



        }). // keyup()
        keypress(function (event) { }). // keypress()
        blur(function (event) { }); // blur

    });         // function()


    $(function () {


        $('#summary-list tbody tr td:nth-child(1)').each(function (i, r) {

            var room_number = $(r).attr('roomnumber');
            var $link = $('<a href=#>' + $(this).text() + '</a>');

            $link.bind('click', function (e) {
                 $.ajax({
                        url: 'itemsdetail2track.aspx/search',
                        data: '{roomid:"' + room_number + '"}',
                        type: 'post',
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            if (data.d) {
                                var arr = $.parseJSON(data.d);

                            }
                        }
                    }); 
                
                
                
                e.preventDefault(); 
                
            });
            $(this).html($link);

        });


        //        $('#summary-list tbody tr td:nth-child(1)').html(function () {
        //            return $('<a href=#>' + $(this).text() + '</a>')
        //                    .click(function (e) {

        //                        alert($(this).attr('roomnumber'));
        //                        return;

        //                        $.ajax({
        //                            url: 'itemsdetail2track.aspx/search',
        //                            data: '{roomid:"' + $(this).attr('roomnumber') + '"}',
        //                            type: 'post',
        //                            contentType: 'application/json; charset=utf-8',
        //                            success: function (data) {
        //                                if (data.d) {
        //                                    var arr = $.parseJSON(data.d);

        //                                }
        //                            }
        //                        });



        //                        e.preventDefault();

        //                    });
        //        }); // html()

    });          // function()  

</script>


<style type="text/css">


.transparent {
	zoom: 1;
	filter: alpha(opacity=50);
	opacity: 0.5;
	font-style:italic;
}

</style>
</asp:Content>
