<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="AutomatedPrintConfirmationLetters.aspx.vb" Inherits="setup_AutomatedPrintConfirmationLetters" ValidateRequest="false" %>
<%@ Register Src="~/controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style type="text/css">
    h1
    {
        text-transform:uppercase;
    }
    #tbl_calendar td
    {
        font-family:DejaVu Sans;
        font-size:small;
    }
</style>

<script type="text/javascript">
    $(function () {

        $('input[id$=btn_submit]').click(function () {

            $('#date_range_err').html('');

            if ($('input[id$=ctl00_ContentPlaceHolder1_sdate_txtDate]').val() == "" ||
            $('input[id$=ctl00_ContentPlaceHolder1_edate_txtDate]').val() == "") {

                $('#date_range_err').html('<div>Date range is not valid!</div>').css({ 'color': 'red', 'font-style': 'italic' });
                return false;

            }
            else if ($('select[id$=_lbx_To]').find('option').length == 0) {

                $('#date_range_err').html('<div>Please choose at least one location.</div>').css({ 'color': 'red', 'font-style': 'italic' });
                return false;                
            }
            else {
                return true;
            }
        });
    });

  
</script>


</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
    <h1>CZAR Confirmation Letters</h1>
    
    <h3 id="date_range_err"></h3>
    <br />

    <div style="width:900px;">
        <div style="float:left;width:300px;">
            <table id="tbl_calendar">
                <tr>
                    <td>Start Date:</td>
                    <td>
                        <uc1:DateField ID="sdate" runat="server" />
                    </td>
                </tr>
                <tr>
        <td>End Date:</td>
        <td>
            <uc1:DateField ID="edate" runat="server" />
        </td>
    </tr>                   
            </table>    
        </div>
        
        <div style="width:570px;float:right;">    
            <table>
                <tr>
                    <td style="width:250px"><asp:ListBox runat="server" ID="lbx_from" Width="240" SelectionMode="Multiple" Height="260"></asp:ListBox></td>
                    <td style="width:60px;"><asp:Button runat="server" ID="btn_to" Text=">" Width="55" Height="40" /><br /><asp:Button runat="server" ID="btn_back" Text="<" Width="55" Height="40" /></td>
                    <td style="width:250px"><asp:ListBox runat="server" ID="lbx_To" Width="240" SelectionMode="Multiple" Height="260"></asp:ListBox></td>             
                </tr> 
            </table>
        </div>        
    </div>


</div>
<div style="clear:left;">
<br />
<asp:Button runat="server" Text="Submit" Width="100" Height="40" ID="btn_submit" />              
</div>

             
</asp:Content>

