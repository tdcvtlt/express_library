<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="IIMembershipEnrollment.aspx.vb" Inherits="Reports_Accounting_IIMembershipEnrollment" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">

input[type="text"], input[type='submit']
{
    font-family:DejaVu Sans;
    font-size:small;   
}

table
{
    font-family:DejaVu Sans; 
    font-size:small;
    border-collapse:collapse;   
    overflow:hidden; 
}


</style>

<script type="text/javascript">

    $(function () {

        $('table[id^=table]').css({ 'border': 'solid 1px black', 'border-collapse': 'collapse' });
        $('h3').css({ 'color': 'red' });

        $('table[id^=table] td').filter(function () {
            return this.innerText.match(/^[-?0-9\s\.,]+$/);
        }).css('text-align', 'right');

        $('h1').css({ 'font-family': 'DejaVu Sans', 'text-transform': 'uppercase' });

        $('input[name$=btn_bill]').click(function () {

            var response = confirm('Are you sure you want to mark these items as billed?');
            if (response) {
                return true;
            }
            else {
                return false;
            }
        });

        $('h3').css({ 'font-family': 'DejaVu Sans', 'text-transform': 'uppercase', 'font-weight': 'bold', 'text-decoration': 'underline' });

        $('input[id$=btn_ExportNonIIContract]').click(function () {

            $('#cxl_report_div_err').html('');

            if ($('input[id$=_sdate_txtDate]').val() == "" || $('input[id$=_edate_txtDate]').val() == "") {

                $('#cxl_report_div_err').html('<div style=color:red;font-style:italic;>Date range is not in correct format!</div>');
                return false;
            }
            else if ($('input:checked').length == 0) {

                $('#cxl_report_div_err').html('<div style=color:blue;font-style:italic;>Please check at least one status!</div>');
                return false;
            }
        });

        $('input[id$=btn_retrieve_report]').click(function () {
            $('#view2_div_err').html('');

            if ($('input[id$=dtAnniversaryDate_txtDate]').val() == "") {

                $('#view2_div_err').html('<div>Cut-Off Anniversary date is missing!</div>').css({ 'color': 'red', 'font-style': 'italic' });
                return false;
            }
        });


        $('input[id$=CheckToExport]').click(function () {

            var response = confirm('Are you sure you want to export the list?');
            if (response) {
                return true;
            }
            else {
                return false;
            }
        });


        $('input[id$=btn_fin_submit]').click(function () {

            $('#dv_fin_date_err').html('');

            if ($('input[id$=dt_fin_start_txtDate]').val() == "" || $('input[id$=dt_fin_end_txtDate]').val() == "") {

                $('#dv_fin_date_err').html('<div style=color:red;font-style:italic;>Date range is not in correct format!</div>');
                return false;
            }
            else {
                return true;
            }

        });

        $('input[id$=Submit]').click(function () {

            if ($(':checked').length == 0) {
                alert('Please check the box each contract you want to submit.');
                return false;
            } else {

                var response = confirm('Are you sure you want to submit?');
                if (response) {
                    return true;
                }
                else {
                    return false;
                }
            }
            return false;
        });        

    });




 

</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    


<ul id="menu">
    <li <% if multi_view_main.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="lnk_btn_points_owners" runat="server">Points Contracts</asp:LinkButton>        
    </li>
    <li <% if multi_view_main.ActiveViewIndex = 1 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="lnk_btn_ii_report" runat="server">II Report</asp:LinkButton>
    </li>
    <li <% if multi_view_main.ActiveViewIndex = 2 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="lnk_btn_non_ii_report" runat="server">CXL Report</asp:LinkButton>
    </li>
    <li <% if multi_view_main.ActiveViewIndex = 3 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="lnk_btn_report" runat="server">Financials</asp:LinkButton>
    </li>
    <li <% if multi_view_main.ActiveViewIndex = 4 then: response.write("class=""current"""):end if  %>>
        <asp:LinkButton ID="lnk_view_4" runat="server">Cancelled</asp:LinkButton>
    </li>
</ul>
                      

<asp:MultiView runat="server" ID="multi_view_main">
    <asp:View runat="server" ID="View1">
     <div style="">    

        <h2>please select (optional)</h2>   
        <asp:CheckBoxList ID="checkBoxList1" runat="server" Height="200" RepeatDirection="Vertical">            
        </asp:CheckBoxList>       
        <br />        
        <asp:Button runat="server" ID="btn_Retrieve" Text="Retrieve Contracts" Width="160" Height="40" />
        &nbsp;&nbsp;<asp:Button ID="Submit" runat="server" Width="100" Height="40" Text="Submit" Visible="false" />    &nbsp;
        <asp:Button ID="btn_ToExcel" runat="server" Width="140" Height="40" Text="Convert to Excel" Visible="false" />  
        <br />    
        <br />        
        <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="false" DataKeyNames="ContractID">
            <Columns>
                <asp:TemplateField><ItemTemplate><asp:CheckBox ID="cbx" runat="server" /></ItemTemplate></asp:TemplateField>
                <asp:BoundField DataField="AnniversaryDate" HeaderText="Anniversary Date"  HtmlEncode="false" />
                <asp:BoundField DataField="ProspectID" HeaderText="Prospect ID" />
                <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                <asp:BoundField DataField="FirstName" HeaderText="First Name" />            
                <asp:BoundField DataField="KCP" HeaderText="KCP #" />            
                <asp:BoundField DataField="Status" HeaderText="Status" />
            </Columns>
        </asp:GridView>
        <br />        
        
          

    </div>   
    </asp:View>

    <asp:View runat="server" ID="View2">
        <div>
        <div style="width:450px;float:none;">
            <h2>Anniversary Cut Off Date (required)</h2>
            <uc1:DateField ID="dtAnniversaryDate" runat="server" 
                Selected_Date=""  />
            <br />
            <br />
            <div id="view2_div_err"></div>
            <br />
            <asp:Button runat="server" ID="btn_retrieve_report" Text="Retrieve Report" Width="140" Height="40" />
        </div>        

        <asp:Literal runat="server" ID="litResult"></asp:Literal>        
        <br />
        <asp:Button ID="CheckToExport" runat="server" Width="100" Height="40" Text="To Export" Visible="false" />   
        <br />                                
    </div>
    </asp:View>

    <asp:View runat="server" ID="View3">
        <div>            
            <h1>Non II Contracts</h1>
            <div style="margin:0 auto;width:100%;">

                <div style="width:400px;float:left;">
                    <h3>Check status(es)</h3>                                        
                    <asp:CheckBoxList ID="cbl_cxl" runat="server"></asp:CheckBoxList>
                </div>

                <div style="margin-left:410px;">
                    <h3>Date Range</h3>
                   <table>
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
                <br />
                <div id="cxl_report_div_err"></div>
                </div>
                      
                <br />
                <asp:Button ID="btn_ExportNonIIContract" runat="server" Width="100" Height="40" Text="Get CXL" />                                    
            </div>
            
            


            <div style="clear:left;border:0px solid black;">        
                <br />
                <asp:Literal ID="ll_NonReport_0" runat="server"></asp:Literal>
                <br />
                <br />
                <asp:Literal ID="ll_NonII" runat="server"></asp:Literal>
            </div>

    </div>

                   
    </asp:View>


    <asp:View runat="server" ID="View4">
        <div>
            
            <h1>Financials</h1>
            <div>
                <table>
                    <tr>
                        <td>Start Date:</td>
                        <td>
                            <uc1:DateField ID="dt_fin_start" runat="server" />
                        </td>
                        <td><div id="dv_fin_date_err"></div></td>
                    </tr>
                    <tr>
                        <td>End Date:</td>
                        <td>
                            <uc1:DateField ID="dt_fin_end" runat="server" />
                        </td>
                         <td></td>
                    </tr>                    
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btn_fin_submit" runat="server" Text="Submit" Height="40" Width="120" />
                        </td>
                         <td></td>
                    </tr>
                </table>                    
            </div>
            <br />
            
            
            
            <asp:HiddenField ID="hfd_keys_billed" runat="server" />
            <br />

            <asp:Literal ID="lit_conversion" runat="server"></asp:Literal>           
            <br />
            <asp:Literal ID="lit_non_conversions" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="lit_summaries" runat="server"></asp:Literal>     
            <br />
            <br />
            <asp:Button ID="btn_bill" runat="server" Text="Bill" Width="120" Height="40" Visible="false" />    
        </div>
    </asp:View>


    <asp:View ID="View5" runat="server">
        <h1>Hello</h1>

        <asp:Label runat="server" ID="show_current_time"></asp:Label>
        <br />
        <asp:Button ID="get_current_time" runat="server" Text="Get Time" />


        <asp:GridView ID="gvCancells" runat="server" AutoGenerateColumns="false" GridLines="Both" DataKeyNames="ReservationID">
            <Columns>
            
            <asp:BoundField ItemStyle-Width="0" DataField="ProspectID"   />
            <asp:BoundField HeaderText="ID" DataField="ReservationID" />

            <asp:TemplateField>
            <ItemTemplate>
            <asp:Label runat="server" ID="lbl_balance"></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>            
            </Columns>            
        </asp:GridView>
    </asp:View>


</asp:MultiView>



<br />
<asp:Label ID="lblError" runat="server"></asp:Label>
<asp:HiddenField runat="server" ID="hf_iiMembershipEnrollmentID" />
</asp:Content>

