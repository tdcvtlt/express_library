<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="IIMembershipEnrollment1.aspx.vb" Inherits="Reports_Accounting_IIMembershipEnrollment" %>
<%@ Register src="../../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

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

<style type="text/css">


.f            {width:100%; border-collapse:collapse;}
.f th          {background:#95bce2; color:white; font-weight:bold;}
.f td, th      {padding:6px; border:1px solid #95bce2; text-align:left;}
table.f tr:nth-child(even)           {background-color:#ecf6fc;}
table.f tr:nth-child(odd)           {background-color:white;}
.f           tr:hover, td.hover          {background-color:#ccc!important;}
.f td:hover          {background-color:#6ab9d0!important; color:white;}

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

                $('#cxl_report_div_err').html('<div style=color:red;font-style:italic;>Date range is not correct!</div>');
                return false;
            }
            else if ($('input:checked').length == 0) {

                $('#cxl_report_div_err').html('<div style=color:blue;font-style:italic;>Checkbox at least one status!</div>');
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


        $('input[id$=CheckToExport], input[id$=view_6_export]').click(function () {

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
                alert('Please checkbox the contract you want to submit.');
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

<script type="text/javascript">

    $(function () {

        $('.f td').click(function () {

            if ($(this).index() == 0) {

                var c = $('input[type=checkbox]', $(this));
                if (c.attr('checked') == 'checked') {
                    var p = $(this).parent('tr');
                    p.css('background-color', '#ccc');
                } else {
                    p = $(this).parent('tr');
                    p.css('background-color', 'white');
                }
            }
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
  
    <li <% if multi_view_main.ActiveViewIndex = 5 then: response.write("class=""current"""):end if  %>>
        <asp:LinkButton ID="lnk_view_5" runat="server">Upgrades & Additionals</asp:LinkButton>
    </li>
</ul>
                      

<asp:MultiView runat="server" ID="multi_view_main">
    <asp:View runat="server" ID="View1">
     <div style="">    

         <div style="border:1px gray solid;margin-top:10px;">
            <h4 style="padding-left:20px;">New KCP owners purchased their Points contracts. Check selections to view additional contracts to include in II report.</h4>
        </div>
        <br /><br />       
        <asp:CheckBoxList ID="checkBoxList1" runat="server" Height="200" RepeatDirection="Vertical">            
        </asp:CheckBoxList>       
        <br /><br />   
        <h5 runat="server" id="view1_h5"></h5>
                     
        <asp:Button runat="server" ID="btn_Retrieve" Text="Retrieve" Width="120" Height="40" />
        &nbsp;&nbsp;<asp:Button ID="Submit" runat="server" Width="120" Height="40" Text="Submit" Visible="false" />    &nbsp;
        <asp:Button ID="btn_ToExcel" runat="server" Width="120" Height="40" Text="Export" Visible="false" />  
        <br />    
        <br />        
        <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="false" CssClass="f" DataKeyNames="ContractID">
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
        <div>
            
                        
            <div style="border:1px gray solid;margin-top:10px;">
            <h4 style="padding-left:20px;">Points contracts and associated inventories have never been sent to II. </h4>
            </div>
            
            <br />                                                
            <label><h5>Prior to this Anniversary Date</h5></label>
            <uc1:DateField ID="dtAnniversaryDate" runat="server" 
                Selected_Date=""  />
            <br />
            <br />
            <div id="view2_div_err"></div>

            <h5 runat="server" id="view2_h5"></h5>
            <br />
            <asp:Button runat="server" ID="btn_retrieve_report" Text="Retrieve" Width="120" Height="40" />  
            &nbsp;&nbsp;<asp:Button ID="CheckToExport" runat="server" Width="120" Height="40" Text="Export" Visible="false" />   
        </div>        

        <br />
        <br />
        <asp:Literal runat="server" ID="litResult"></asp:Literal>        
                                  
    </div>
    </asp:View>

    <asp:View runat="server" ID="View3">
        <div>            
            <div style="border:1px gray solid;margin-top:10px;">
            <h4 style="padding-left:20px;">Original points contracts sent to II later became cancelled.</h4>
            </div>

            <br />
            <div style="margin:0 auto;width:100%;">

                <div style="width:400px;float:left;">
                    <h5>Status changed during period.</h5>
                    <asp:CheckBoxList ID="cbl_cxl" runat="server"></asp:CheckBoxList>
                </div>

                <div style="margin-left:410px;">
                    
                   <h5>Period cancels occured.</h5>
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
                <asp:Button ID="btn_ExportNonIIContract" runat="server" Width="120" Height="40" Text="Retrieve" />&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_ExportCancelled_Contracts" runat="server" Width="120" Height="40" Text="Export" Visible="false" />                                    
            </div>
                        
            <div style="clear:left;border:0px solid black;">        
                <br />                
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


    


    <asp:View ID="View6" runat="server">
        <div>
            <br />
            <div style="border:1px gray solid;">
            <h4 style="padding-left:20px;">These Point contracts are additional to the ones 
                already sent to II,&nbsp; later either upgraded or purchased by their owners. </h4>
            </div>
                        
            <h5 runat="server" id="lit_view_6_h5"></h5>
        
            <asp:Button ID="lit_view_6_button" runat="server" Width="120" Height="40" Text="Retrieve" />  &nbsp;&nbsp;          
            <asp:Button ID="view_6_export" runat="server" Width="120" Height="40" Text="Export" />
            <br /><br /><br />            
            <asp:Literal ID="lit_view_6" runat="server"></asp:Literal>

        </div>    
    </asp:View>


</asp:MultiView>



<br />
<asp:Label ID="lblError" runat="server"></asp:Label>
<asp:HiddenField runat="server" ID="hf_iiMembershipEnrollmentID" />
<asp:HiddenField runat="server" id="hf_iiMembershipEnrollmentID_upgrades" />
</asp:Content>

