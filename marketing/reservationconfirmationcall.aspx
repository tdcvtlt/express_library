<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="reservationconfirmationcall.aspx.vb" Inherits="marketing_reservationconfirmationcall" %>
<%@ Register Src="~/controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="container">

    
    <table>    
        <tr>
            <td><strong>check-in from</strong></td>
            <td><strong>to</strong></td>
            <td></td>
        </tr>
        <tr>
            <td valign="top"><uc1:datefield ID="dteSDate" Selected_Date="" runat="server" /></td>
            <td valign="top"><uc1:datefield ID="dteEDate" Selected_Date="" runat="server" /></td>
            <td></td>
        </tr> 
        <tr>
            <td colspan="3">&nbsp</td>
        </tr>       
        <tr>
            <td><strong>resort company</strong></td>
            <td><strong>reservation status</strong></td>
            <td><strong>reservation type</strong></td>            
        </tr>
        <tr>
            <td valign="top"><asp:CheckBoxList runat="server" ID="cbl_resort"></asp:CheckBoxList></td>
            <td valign="top"><asp:CheckBoxList runat="server" ID="cbl_status"></asp:CheckBoxList></td>
            <td valign="top"><asp:CheckBoxList runat="server" ID="cbl_type"></asp:CheckBoxList></td>           
        </tr>
    </table>
    
    <br /><br />
    <asp:Label runat="server" ID="lbError" ForeColor="Red"></asp:Label>
    <br /><br />
    <asp:Button runat="server" ID="btn_report_run" Text="run report" />&nbsp;<asp:Button runat="server" ID="btn_report_refresh" Text="refresh" />

    <br /><br />
    <asp:Literal runat="server" ID="lit_report"></asp:Literal>
          
</div>

</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">    

<script type="text/javascript">


    $(function () {

        $('#report-table').css('border-collapse', 'collapse');
        $('.data-row').css('border-top', '1px dotted black');
        $('.data-row').css('border-bottom', '1px dotted black');

        $('#report-table thead th:eq(0)').css('width', '200' + 'px');
        $('#report-table thead th:eq(1)').css('width', '200' + 'px');
        $('#report-table thead th:eq(2)').css('width', '200' + 'px');
        $('#report-table thead th:eq(3)').css('width', '200' + 'px');
        $('#report-table thead th:eq(4)').css('width', '200' + 'px');
        $('#report-table thead th:eq(5)').css('width', '200' + 'px');
        $('#report-table thead th:eq(6)').css('width', '200' + 'px'); 
        $('#report-table thead th:eq(7)').css('width', '200' + 'px');
        $('#report-table thead th:eq(8)').css({ 'width': '200' + 'px'});
        $('#report-table thead th:eq(9)').css({ 'width': '200' + 'px'});
        $('#report-table thead th:eq(10)').css({ 'width': '200' + 'px'});
        $('#report-table thead th:eq(11)').css({ 'width': '200' + 'px' });
        $('#report-table thead th:eq(12)').css({ 'width': '200' + 'px' });

        $('#report-table tbody td:nth-child(9)').css({ 'background-color': '#B40431', 'color': '#F8FBEF' });

        $('#report-table tbody td:nth-child(10)').css({ 'background-color': '#FFFAFA' });
        $('#report-table tbody td:nth-child(11)').css({ 'background-color': '#FFFAFA' });
        $('#report-table tbody td:nth-child(12)').css({ 'background-color': '#FFFAFA' });

        $('#report-table tbody td:nth-child(7)').css({ 'width': '200px', 'font-weight': 'bold', 'color':'blue' });

        $('#report-table thead th').css({'text-transform':'uppercase', 'background-color':'#99cc99', 'color':'#ffffff'});

        $('.clickable').each(function (index, row) {

            var link = $('<a href=EditReservation.aspx?ReservationID=' + $(row).text() + ' target=_blank>' + $(row).text() + '</a>')
                        .click(function () {                            
                        });
            $(row).html(link);

        });

    });


    $(function () {

        $('#<%= cbl_resort.ClientID %>, #<%= cbl_status.ClientID %>, #<%= cbl_type.ClientID %>').each(function () {

            var ctl = $(this);

            $(this).find('input[type=checkbox]:first').change(function () {                
                $('input[type=checkbox]', ctl).attr('checked', this.checked);
            });

        });

    });

    $(function () {


        $('input[type=radio]', '#report-table').click(function () {

            var thisR = $(this);
            var id = $(this).attr('id');
            var hypen = id.indexOf('-');
            var prefix = id.substring(0, hypen);
            var suffix = id.substring(hypen + 1);

            var note = $('input[type=text]', $(this).parents('tr:first'));
            var label = $('label', $(this).parents('tr:first'));


            if (suffix == "cm" || suffix == "rm") {

                if (suffix == 'rm' && note.val().length == 0) {
                    alert('please enter some note before removing reservation ' + prefix);
                    $('input[type=radio]', $(this).parents('tr:first')).attr('checked', false);
                    return;
                }

                var message = suffix == 'cm' ? 'are you sure you want to confirm reservation ' + prefix + '?' : 'are you sure you want to remove reservation ' + prefix + '?';
                if (confirm(message)) {

                    $.ajax({
                        type: 'post',
                        url: 'reservationconfirmationcall.aspx/update',
                        data: '{id_r: ' + prefix + ',param:"' + suffix + '",note:"' + note.val() + '",UserDBID:"' + $("#<%= btn_report_run.ClientID  %>").attr("UserDBID") + '"}',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (result) {                            
                            if (result.d) {
                                if (parseInt(result.d) > 0) {
                                    thisR.parents('div:first').parents('tr:first').prev().remove();
                                    thisR.parents('div:first').parents('tr:first').remove();
                                }
                            }
                        }
                    });

                    return;

                } else {
                    $('input[type=radio]', $(this).parents('tr:first')).attr('checked', false);
                    note.val('');
                }
            }


            if (suffix == "na" && note.val().length == 0) {
                alert('please enter N/A reason for reservation ' + prefix);
                $('input[type=radio]', $(this).parents('tr:first')).attr('checked', false);
                return;
            } else if (suffix == 'na' && note.val().length > 0) {

                $.ajax({
                    type: 'post',
                    url: 'reservationconfirmationcall.aspx/update',
                    data: '{id_r: ' + prefix + ',param:"' + suffix + '",note:"' + note.val() + '",UserDBID:"' + $("#<%= btn_report_run.ClientID  %>").attr("UserDBID") + '"}',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (result) {
                        if (result.d) {
                            if (parseInt(result.d) > 0) {
                                var dt_curr = new Date();
                                label.html('<strong>' + dt_curr.toLocaleDateString() + '<br/>' + dt_curr.toLocaleTimeString() + '</strong>');
                            }
                        }
                    }
                });

            }

            if (suffix == 'vm') {

                $.ajax({
                    type: 'post',
                    url: 'reservationconfirmationcall.aspx/update',
                    data: '{id_r: ' + prefix + ',param:"' + suffix + '",note:"' + note.val() + '",UserDBID:"' + $("#<%= btn_report_run.ClientID  %>").attr("UserDBID") + '"}',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (result) {
                        if (result.d) {
                            if (parseInt(result.d) > 0) {
                                var dt_curr = new Date();
                                label.html('<strong>' + dt_curr.toLocaleDateString() + '<br/>' + dt_curr.toLocaleTimeString() + '</strong>');
                            }
                        }
                    }
                });

                return;
            }
            
             // Confirmed by Email
            if (suffix == 'em') {

                // ajax() starts
                $.ajax({
                    type: 'post',
                    url: 'reservationconfirmationcall.aspx/update',
                    data: '{id_r: ' + prefix + ',param:"' + suffix + '",note:"' + "" + '",UserDBID:"' + $("#<%= btn_report_run.ClientID  %>").attr("UserDBID") + '"}',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (result) {
                        if (result.d) {
                            if (parseInt(result.d) > 0) {
                                var dt_curr = new Date();
                                label.html('<strong>' + dt_curr.toLocaleDateString() + '<br/>' + dt_curr.toLocaleTimeString() + '</strong>');
                            }
                        }
                    }
                });
                // ajax() ends
            } // Confirmed by Email

        });


    });
</script>


<style type="text/css">

.data-row
{
    
}

.action-row
{
         
            
}

.clickable
{
    cursor:pointer;
}


.pad-background
{
    background-color:#FFFAFA;
}

</style>
</asp:Content>
