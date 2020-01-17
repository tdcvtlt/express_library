<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="BookingEmailsManager.aspx.vb" Inherits="Maintenance_BookingEmailsManager" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div style="border:0px solid black;width:1050px;">
    <div style="border:0px dotted red;">
        <div>
            <asp:DropDownList runat="server" ID="ddlReservationStatuses"></asp:DropDownList>
            <p style="width:100%;"><asp:DropDownList runat="server" ID="ddlResorts"></asp:DropDownList> <span><asp:Button runat="server" ID="btnSubmit" Text="submit" /></span>
                <div style="float:right;margin-right:120px;border:0px solid blue;">
                      <ul style="list-style:none;">
                        <li><span></span></li>
                        <li style="margin-bottom:3px;"><input type="text" size="30" value="enter reservation id" id="search-reservation-result" /></li>
                        <li><input type="text" size="30" value="search by name" id="search-reservation" /></li>
                    </ul>
                </div>
               
            </p>

        </div>

        <div>
            <p><asp:Label runat="server" ID="lblResort"></asp:Label></p>
           
        </div>
    </div>

    <div>
    <table id="008">
        <thead>
            <tr>
                <th align="left" colspan="2">reservation status</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <th align="left" colspan="2">
                    <select id="reservation-status-dd" class="select"></select>
                </th>
            </tr>
            <tr>
                <td>
                    <input type="text" size="46" id="email-add-text" value="enter email here" />
                </td>
                <td>
                    <input type="button" id="email-add-btn" value="add" />
                </td>
            </tr>
        </tbody>
        <tfoot></tfoot>        
    </table>
    </div>


    

<br /><br />
<div>
    <table id="009" border="0">
        <thead>
            <tr>
                <th align="left">Reservation Status</th>
                <th>Active</th>
                <th>Delete</th>
                <th>Letter</th>
                <th align="right">Test</th>
            </tr>
        </thead>
        <tbody>
           
        </tbody>
        <tfoot></tfoot>
    </table>
</div>

</div>



 <p>
    <asp:GridView runat="server" ID="gv1" EmptyDataText="Add"></asp:GridView>
    <asp:DropDownList runat="server" ID="ddlLetters"></asp:DropDownList>
</p>

</asp:Content>






<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        
.option-first
{
    font-style:italic;
    font-size:1.3em;
}
                
.ul-none
{
    list-style-type:none;
}


.tooltip
{
    position:absolute;
    z-index:2;
    background:#efd;
    border:1px solid #ccc;
    padding:3px;
}

.selected
{
    color:Aqua;
    background-color:Silver;
}

.transparent {
	zoom: 1;
	filter: alpha(opacity=50);
	opacity: 0.5;
	font-style:italic;
}
     
</style>

    <script type="text/javascript">



        $(function () {


            var switchLabel = function ($input) {

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

            switchLabel($('#search-reservation'));
            switchLabel($('#search-reservation-result'));
            switchLabel($('#email-add-text'));

            var $ul_di = $('<ul></ul>').attr('id', 'autocomplete').addClass('tooltip').hide().insertAfter($('#search-reservation'));

            var selectedItem = null;
            // function...
            //
            var setSelectedItem = function (index) {
                selectedItem = index;
                if (selectedItem === null) {
                    $ul_di.hide();
                    return;
                }


                if (selectedItem < 0) selectedItem = 0;
                if (selectedItem >= $ul_di.find('li').length) { selectedItem = $ul_di.find('li').length - 1; }
                $ul_di.find('li').removeClass('selected').eq(selectedItem).addClass('selected');
                $ul_di.show();
            }

            // function...
            //
            var populateSearchField = function () {
                // parse name up to opening parentheses then store into span above 
                // id within parenthese goes into result textbox
                var li_text = $.trim($ul_di.find('li').eq(selectedItem).text());

                var pro_name = li_text.substr(0, li_text.indexOf('(') - 1);

                var start = li_text.indexOf('(');
                var stop = li_text.indexOf(')');
                var diff = stop - start;

                var res_id = li_text.substr(li_text.indexOf('(') + 1, diff - 1);

                $('#search-reservation-result').parents('ul:first').find('li:first span').html('<a href=../marketing/editreservation.aspx?reservationid=' + res_id + ' target=_blank >' + pro_name + '</a>');
                
                $('#search-reservation-result').val(res_id);

                setSelectedItem(null);
            }



            $('#search-reservation').keyup(function (event) {

                if (event.keyCode > 40 || event.keyCode == 8) {
                    var text = $(this).val();

                    if (text.length > 0) {
                        $.ajax({
                            url: 'BookingEmailsManager.aspx/PeekReservationByLastname',
                            data: '{beginsWith:"' + text + '"}',
                            type: 'post',
                            contentType: 'application/json; charset=utf-8',
                            success: function (data) {
                                if (data.d) {

                                    var arr = $.parseJSON(data.d);
                                    $ul_di.empty();
                                    $.each(arr, function (idx, row) {

                                        $('<li></li>').css({ 'list-style-type': 'none', 'cursor': 'pointer' }).text(row.LastName + ', ' + row.FirstName + ' (' + row.ReservationID + ')').appendTo($ul_di)
                                                        .mouseover(function () { setSelectedItem(idx); }).click(populateSearchField);



                                    }); // .each

                                    if (arr.length == 0)
                                        setSelectedItem(null);
                                    else
                                        setSelectedItem(0);
                                } else {
                                    setSelectedItem(null);
                                }
                            }
                        });
                    }
                } else if (event.keyCode == 38 && selectedItem !== null) {
                    // user pressed the up arrow                     
                    setSelectedItem(selectedItem - 1);
                    event.preventDefault();
                } else if (event.keyCode == 40 && selectedItem !== null) {

                    // user presses down arrow                    
                    setSelectedItem(selectedItem + 1);
                    event.preventDefault();
                }
                else if (event.keyCode == 27 && selectedItem !== null) {
                    // user presses escape key                     
                    setSelectedItem(null);
                }

            }).keypress(function (event) {
                if (event.keyCode == 13 && selectedItem !== null) {
                    // user pressed enter key                    

                    populateSearchField();
                    event.preventDefault();
                }
            })
                        .blur(function () {
                            setTimeout(function () { setSelectedItem(null); }, 250);
                        });

        });




        $(function () {

            $('#<%= ddlResorts.ClientID %> option:first').addClass('option-first');
            $('#<%= ddlReservationStatuses.ClientID %>').hide();
            $('#<%= ddlLetters.ClientID %>').hide();
            $('#<%= gv1.ClientID %>').hide();

            var $gvASP = $('#<%= gv1.ClientID %>');

            if ($('tr:first td:eq(0)', $gvASP).text() == 'Add') {
                $('tr:first', $gvASP).remove();
            }

            var $table = $('#009');
            var statuses = $('#<%= ddlReservationStatuses.ClientID %> option');

            statuses.clone().appendTo($('#reservation-status-dd'));

            var letters = $('#<%= ddlLetters.ClientID %> option');
            var $rowCount = $('#<%= gv1.ClientID %> tr');


            $('tbody').find('input[type=text]').attr('autocomplete', 'off');


            function manageDisplay() {
                var pre_status = -1;

                $('tbody > tr', $table).remove();

                $('#<%= gv1.ClientID %> tbody tr').each(function () {
                    var cur_status = $('td:first', this);

                    if (cur_status.text() != pre_status) {

                        var $th = $('tbody', $table).append('<tr><th align=left><select disabled=disabled id=rs' + cur_status.text() + '></select></th><th/><th/><th align=left><select id=letter' + cur_status.text() + '></select><br/><input size=39 type=text value=subject id=subject' + cur_status.text() + ' </input><input type=button value=save id=save' + cur_status.text() + ' /></th><th><input type=text size=30 id=kcp' + cur_status.text() + ' /><input type=button value=submit id=submit' + cur_status.text() + ' /></th></tr>');


                        var $select = $th.find('#rs' + cur_status.text());
                        statuses.clone().appendTo($select);


                        $('#kcp' + cur_status.text(), $th).attr('autocomplete', 'off').addClass('transparent')
                            .val('email address')
                            .focus(function () { if (this.value == 'email address') $(this).removeClass('transparent').val(''); })
                            .blur(function () { if (this.value == '') $(this).addClass('transparent').val('email address'); });


                        // test button
                        $('#submit' + cur_status.text(), $th).click({ 'keyId': $('td:eq(5)', this).text() }, function (event) {



                            // get result from textbox search-reservation-result 
                            // get the letter from select 
                            // get email address from textbox with id begins with #kcp + reservation status #

                            var reservationid = $('#search-reservation-result').val();
                            var letterId = $('option:selected', $th.find('#letter' + cur_status.text())).val();
                            var email = $('#kcp' + cur_status.text(), $th).val();

                            
                            if ($('#search-reservation-result').val() != 'enter reservation id' && letterId != -2 && email != 'email address') {

                                if (event.data.keyId.length == 0) return;

                                $.ajax({

                                    type: 'post',
                                    url: 'BookingEmailsManager.aspx/EmailTest',
                                    data: '{email_id:' + event.data.keyId + ',reservation_id:' + reservationid + ',email_address:"' + email + '"}',
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    success: function (result) {
                                        if (result.d) {

                                            alert('Check your inbox.');
                                        }
                                    }
                                }); // .ajax()

                            }
                        });


                        //
                        //
                        $('#subject' + cur_status.text(), $th).attr('autocomplete', 'off').addClass('transparent')
                            .val($('td:eq(4)', this).text())
                            .focus(function () { if (this.value == 'subject') $(this).removeClass('transparent').val(''); })
                            .blur(function () { if (this.value == '') $(this).addClass('transparent').val('subject'); });

                        $('#save' + cur_status.text(), $th).unbind('click');

                        $('#save' + cur_status.text(), $th).bind('click', { 'statusid': cur_status.text() }, function (event) {


                            var resortid = $('#<%= ddlResorts.ClientID %> option:selected').val();
                            var reservation_status_id = event.data.statusid;
                            var letter_id = $('option:selected', $th.find('#letter' + cur_status.text())).val();
                            var subject = $('#subject' + cur_status.text(), $th).val();

                            if (subject == 'subject') subject = '';

                            $.ajax({

                                type: 'post',
                                url: 'BookingEmailsManager.aspx/UpdateSubject',
                                data: '{resortid:' + resortid + ',reservation_status_id:' + reservation_status_id + ',letter_id:' + letter_id + ',subject:"' + subject + '"}',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                success: function (result) {

                                    var $gv1 = $('#<%= gv1.ClientID %>');
                                    $gv1.find('tbody > tr').each(function (index) {

                                        var $row = $(this);

                                        if ((($('td:eq(0)', this)).text() == reservation_status_id) && ((($('td:eq(3)', this)).text() == letter_id)) &&
                                            ($('td:eq(6)', this)).text() == resortid) {

                                            $('td:eq(4)', $row).text(subject);
                                        }

                                    });
                                }

                            });
                        });



                        var $ddl_letters = $th.find('#letter' + cur_status.text());
                        letters.clone().appendTo($ddl_letters);

                        $('option', $select).each(function () {
                            if (parseInt(this.value) == parseInt(cur_status.text())) {
                                this.selected = true;

                                $('#<%= gv1.ClientID %> tbody tr').each(function () {

                                    var $curr_row = $(this);

                                    if ($('td:nth-child(1)', this).text() == cur_status.text()) {

                                        var active = $('td:eq(2) input[type=checkbox]', this);

                                        var checked = (active.attr('checked') == 'checked') ? true : false;

                                        var $newRow = $('<tr><td><input type=text size=46 readonly=readonly value=' + $('td:nth-child(2)', this).text() + ' /></td><td><input type=checkbox /></td><td><input type=button value=delete /></td><td/><td/></tr>');
                                        $('input[type=checkbox]', $newRow).attr('checked', checked).change({ 'emailID': $('td:eq(5)', this).text(), 'key': $('td:nth-child(2)', this).text(), 'sel': 'rs' + cur_status.text() }, function (event) {

                                            var checked = $(this).attr('checked');
                                            checked = (checked == 'checked') ? true : false;

                                            $.ajax({
                                                type: 'post',
                                                url: 'BookingEmailsManager.aspx/UpdateActive',
                                                data: '{emailID: ' + event.data.emailID + ',active: ' + checked + '}',
                                                contentType: 'application/json; charset=utf-8',
                                                dataType: 'json',
                                                success: function (result) {

                                                }

                                            });

                                            var select_id = event.data.sel.substr(2, 100);
                                            var $gv1 = $('#<%= gv1.ClientID %>');
                                            $gv1.find('tbody > tr').each(function (index) {

                                                var $row = $(this);

                                                if ((($('td:eq(0)', this)).text() == select_id) && ((($('td:eq(1)', this)).text() == event.data.key))) {
                                                    $('input[type=checkbox]:eq(0)', $row).attr('checked', checked);
                                                }

                                            });


                                        });




                                        $table.append($newRow);

                                        $('input[type=button]', $newRow).click({ 'emailID': $('td:eq(5)', this).text(), 'key': $('td:nth-child(2)', this).text(), 'sel': 'rs' + cur_status.text() }, function (event) {

                                            var reply = window.confirm('are you sure you want to delete the email: \n\n' + event.data.key + '?');
                                            if (reply == true) {

                                                var $row_del = $('input[type=button]', $newRow).parents('tr:first');
                                                var select_id = event.data.sel.substr(2, 100);
                                                var $gv1 = $('#<%= gv1.ClientID %>');

                                                $gv1.find('tbody > tr').each(function (index) {

                                                    var $row = $(this);

                                                    if ((($('td:eq(0)', this)).text() == select_id) && ((($('td:eq(1)', this)).text() == event.data.key))) {
                                                        $row.remove();
                                                    }

                                                });

                                                $row_del.remove();

                                                $.ajax({
                                                    type: 'post',
                                                    url: 'BookingEmailsManager.aspx/DeleteAccount',
                                                    data: '{emailID: ' + event.data.emailID + '}',
                                                    contentType: 'application/json; charset=utf-8',
                                                    dataType: 'json',
                                                    success: function (result) {

                                                    }
                                                });  // ajax

                                            }

                                        });


                                        /// select the matched option
                                        $('option', $ddl_letters).each(function () {
                                            if (this.value == $('td:eq(3)', $curr_row).text()) {
                                                this.selected = true;
                                            }
                                        });
                                        ///


                                        // select letter change event
                                        $ddl_letters.change({ 'emailID': $('td:eq(5)', this).text(), 'statusid': cur_status.text() }, function (event) {

                                            var $gv1 = $('#<%= gv1.ClientID %>');
                                            $gv1.find('tbody > tr').each(function () {
                                                if ($('td:eq(0)', this).text() == event.data.statusid) {
                                                    $('td:eq(3)', this).text($($ddl_letters).val());

                                                    $.ajax({
                                                        type: 'post',
                                                        url: 'BookingEmailsManager.aspx/UpdateLetter',
                                                        data: '{emailID: ' + event.data.emailID + ',letterID: ' + $($ddl_letters).val() + '}',
                                                        contentType: 'application/json; charset=utf-8',
                                                        dataType: 'json',
                                                        success: function (result) {

                                                        }

                                                    });
                                                }

                                            });

                                        }); //

                                    }

                                });
                            }
                        });




                    } else {
                    }


                    pre_status = cur_status.text();


                });



            }


            manageDisplay();

            $('#email-add-btn').click(function () {

                if ($('#email-add-text').val() == 'enter email here') return;

                var email_new = $('#email-add-text').val();
                var status_sel = $('#reservation-status-dd option:selected').val();
                var resort_id = $('#<%= ddlResorts.ClientID %>').val();

                var $sel = $('select', $table);

                $sel.each(function (index, row) {

                    var status_id = $(row).attr('id').substr(2, $(row).attr('id').length);

                    if (status_sel == status_id) {
                    }
                });

                var $dd_statuses = $table.find('#rs' + status_sel);
                var $dd_letters = $dd_statuses.parents('tr').find('select:last');

                var letter_id = -99;
                if ($dd_letters.val())
                    letter_id = $dd_letters.val();

                // add new account ...
                //
                $.ajax({
                    type: 'post',
                    url: 'BookingEmailsManager.aspx/AddAccount',
                    data: '{ResortCompanyID: ' + resort_id + ',EmailAddress:"' + email_new + '",ReservationStatusID:' + status_sel + ',letterID:' + letter_id + '}',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (result) {

                        var $gv1 = $('#<%= gv1.ClientID %>');

                        $gv1.find('tbody').append('<tr><td>' + status_sel + '</td><td>' + email_new + '</td><td><input type=checkbox /></td><td></td><td/><td>' + result.d + '</td><td>' + $('#<%= ddlResorts.ClientID %>').val() + '</td></tr>');

                        var rows = $gv1.find('tbody > tr').get();
                        $.each(rows, function (index, r) {
                            r.sortKey = $(r).children('td').eq(0).text();
                        });

                        rows.sort(function (a, b) {
                            if (a.sortKey < b.sortKey) return -1;
                            if (a.sortKey > b.sortKey) return 1;
                            return 0;
                        });

                        $.each(rows, function (index, row) {
                            $gv1.children('tbody').append(row);
                            row.sortKey = null;
                        });


                        $('#email-add-text').val('');

                        manageDisplay();
                    }
                });  // ajax


            });

        });


    </script>
</asp:Content>