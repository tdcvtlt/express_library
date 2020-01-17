<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="IIMembershipEnrollment.aspx.vb" Inherits="Reports_Accounting_IIMembershipEnrollment" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta charset="utf-8" content="" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="Stylesheet"  href="../../styles/dist/css/bootstrap.min1.css"  />
    
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
<%--
.kcp {}

.f            {width:100%; border-collapse:collapse;}
.f th          {background:#95bce2; color:white; font-weight:bold;}
.f td, th      {padding:6px; border:1px solid #95bce2; text-align:left;}
table.f tr:nth-child(even)           {background-color:#ecf6fc;}
table.f tr:nth-child(odd)           {background-color:white;}
.f11  tr:hover, td.hover          {background-color:#ccc!important;}
.f11 td:hover          {background-color:#6ab9d0!important; color:white;}--%>

</style>



<style type="text/css">

#poolDV input, label { line-height:1.5em;}

#popupcontainer
{
    position:absolute;
    left:0;
    top:0;
    display:none;
    z-index:200;    
}

#popupcontent
{
    background-color:#FFF;
    min-height:50px;
    min-width:175px;        
}

.popup .corner
{
    width:19px;
    height:15px;        
}

.popup .topleft
{           
    background:url('../../images/TooltipBorders/balloon_topleft.png') no-repeat;    
}
.popup .bottomleft
{           
    background:url('../../images/TooltipBorders/balloon_bottomleft.png') no-repeat;
}
.popup .topright
{
    background:url('../../images/TooltipBorders/balloon_topright.png') no-repeat;
}
.popup .bottomright
{
    background:url('../../images/TooltipBorders/balloon_bottomright.png') no-repeat;
}
.popup .top
{
    background:url('../../Images/TooltipBorders/balloon_top.png') repeat-x;   
}
.popup .bottom
{
    background:url('../../Images/TooltipBorders/balloon_bottom.png') repeat-x;   
}

.popup .left
{
    background:url('../../Images/TooltipBorders/balloon_left.png') repeat-y;   
}

.popup .right
{
    background:url('../../Images/TooltipBorders/balloon_right.png') repeat-y;   
}



    .clickable
    {
        cursor:pointer;
    }

   .sort-alpha
    {
        color:Green;
    }
    
    .sort-numeric
    {
        color:Blue;
    }
    
    .sort-date
    {
        color:Teal;
    }

    .hover
    {
        background-color:Gray;
        color:White;
    }
    
    .sorted
    {
        background-color:White;
    }
    
    .sorted-asc
    {
       <%-- background-color:Red;--%>
    }
    .sorted-desc
    {
       <%-- background-color:Lime;--%>
    }    
    
    .odd
    {
         background-color:#ecf6fc;
    }
     
    . th          
    {
        background:#95bce2; 
        color:white; 
        font-weight:bold;
    }  
    .td,th
    {
        padding:6px; 
        border:1px solid #95bce2; 
        text-align:left;
    }  
    
  .page-number
    {
        display:inline-block;
        border:1px solid black;
        margin-right:3px;
        padding:2px 5px;
        background-color:Black;
        color:White;
        font-weight:bolder;
        margin-bottom:5px; 
        margin-top:5px;
              
    }
             
    
    .active
    {
        color:White;
        background-color:Green;
        border:1px solid Green; 
        margin-right:3px;
        padding:2px 5px;  
        display:inline-block; 
        font-weight:bolder; 
        margin-bottom:5px;   
    }
    
  
    .fill-italics
    {
        opacity:0.6;
        font-style:italic;
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

        // change background color when checkbox is checked/unckecked
        //
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




<script type="text/javascript">

    $(function () {

        $('#txt_changed').keyup(function () {

            $.ajax({
                type: "POST",
                url: "IIMembershipEnrollment.aspx/Get_SQL",
                data: "kcp_number=0",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    //alert(msg.d);
                    var val = $.parseJSON(msg.d);

                    if (val.name === 'hello world') {

                    }

                    var re = "";

                    $.each(val, function () {                        
                        re += this['Name'];
                    });

                    $('<div>').html(re).appendTo('body');

                },
                error: function (data) {
                    alert("error: " + data);
                }
            });


        });


    });
</script>


<script type="text/javascript">
    // popup dive
    //
    var arId = new Array();
    var hideDelay = 500;
    var hideTimer = null;
    var currentId;
    var kcp_summary = "";

    $(function () {

        var popup = $("<div id='popupcontainer' style='border:0px solid red;'>"
				  + "<table width='0' border='0' cellpadding='0' cellspacing='0' align='center' class='popup'>"
				  + "<tr>"
				  + "<td class='corner topleft'></td>"
				  + "<td class='top'></td>"
				  + "<td class='corner topright'></td>"
				  + "</tr>"
				  + "<tr>"
				  + "<td class='left'>&nbsp;</td>"
				  + "<td><div id='popupcontent'></div></td>"
				  + "<td class='right'>&nbsp;</td>"
				  + "</tr>"
				  + "<tr>"
				  + "<td class='corner bottomleft'>&nbsp;</td>"
				  + "<td class='bottom'>&nbsp;</td>"
				  + "<td class='corner bottomright'></td>"
				  + "</tr>"
				  + "</table>"
				  + "</div>");


        // append to body tag
        $('body').append(popup);

        // bind hover event on f class
        $('.kcp').on("mouseover", function (e) {

            var pos = $(this).offset();
            var width = $(this).width();
            var height = $(this).height();

            if (hideTimer)
                clearTimeout(hideTimer);

            // set position left, top for popup
            popup.css({ left: (pos.left - 10) + 'px', top: (pos.top + height) + 'px' });


            // ajax 
            $.ajax({
                type: "POST",
                url: "IIMembershipEnrollment.aspx/Get_SQL",
                data: "{'kcp_number':'" + $(this).html() + "'}",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    var val = $.parseJSON(msg.d);

                    if (val.Name === 'Adams') {
                        alert(val.Name);
                    }

                    var re = "";

                    $.each(val, function () {
                        re += "<br/><strong>Owner's Name: " + this['Name'] + "</strong><br/><br/><strong>Enrollment Membership ID: " + 
                            this['ProspectID'] + "</strong><br/><strong>II Status (Sent): " + this['Exported'] + '</strong>';

                    });

                    //$('<div>').html(re).appendTo('body');


                    // set popup content
                    popup.find('#popupcontent').html(new Date().toLocaleDateString() + '<br/>' + re + '<br/><br/>');

                    // show it
                    popup.css('display', 'block');

                },
                error: function (data) {
                    
                }
            });





            //

        });

        $('.kcp').on('mouseout', function (e) {

            if (hideTimer)
                clearTimeout(hideTimer);

            hideTimer = setTimeout(function () { popup.css('display', 'none') }, hideDelay);
        });


    });
   

   

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    


    <ul id="menu">
    <li <% if multi_view_main.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="lnk_btn_points_owners"  runat="server">Points Contracts</asp:LinkButton>        
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

    <li <% if multi_view_main.ActiveViewIndex = 8 then: response.write("class=""current"""):end if  %>>
        <asp:LinkButton ID="lnkRenewalListExcelUpload" runat="server">Upload Excel</asp:LinkButton>
    </li>

</ul>
                      
<input type="text" id="txt_changed" style="visibility:hidden;display:none;" />

<br />
<asp:MultiView runat="server" ID="multi_view_main">
    <asp:View runat="server" ID="View1">
     <div style="">    

         <div style="border:1px gray solid;margin-top:10px;">
            <h4 style="padding-left:20px;">Brand New Point contracts.</h4>
        </div>
        <br /><br /> 
        <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="false" CssClass="f" DataKeyNames="ContractID">
            <Columns>                               
                <asp:TemplateField><ItemTemplate><asp:CheckBox ID="cbx" runat="server" /></ItemTemplate></asp:TemplateField>
                <asp:BoundField DataField="AnniversaryDate" HeaderText="Anniversary Date"  DataFormatString="{0:d}"  />
                <asp:BoundField DataField="prospectid" HeaderText="Prospect ID" />
                <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                <asp:BoundField DataField="FirstName" HeaderText="First Name" />            
                <asp:BoundField DataField="ContractNumber" ItemStyle-CssClass="kcp" HeaderText="KCP #" />            
                <asp:BoundField DataField="Status" HeaderText="Status" />                
            </Columns>
        </asp:GridView>
          
        <br />    
        <br />   
        <br />  
        <h5 runat="server" id="view1_h5"></h5>
        <asp:Button runat="server" ID="btn_Retrieve"  Text="Retrieve" Width="120" Height="40" />
        &nbsp;&nbsp;<asp:Button ID="Submit" runat="server" Width="120" Height="40" Text="Submit" Visible="false" />    &nbsp;
        <asp:Button ID="btn_ToExcel" runat="server" Width="120" Height="40" Text="Export" Visible="false" />  
        <br /><br />                    
        <fieldset>
            <legend><span style="font-family:Arial Narrow;font-size:x-large;">Optional: </span></legend>
            <asp:CheckBoxList ID="checkBoxList1" runat="server" Height="200" RepeatDirection="Horizontal" RepeatColumns="7" Font-Size="Large" Font-Names="Arial Narrow"></asp:CheckBoxList>       
        </fieldset>               
          

    </div>   
    </asp:View>

    <asp:View runat="server" ID="View2">
        <div>
        <div>
            
                        
            <div style="border:1px gray solid;margin-top:10px;">
            <h4 style="padding-left:20px;"><i>Points</i> contracts and their inventories were never sent to II. </h4>
            </div>
            
            <br />                                                
            <h5 id="anniversary-label">Anniversary Date</h5>
            <uc1:DateField ID="dtAnniversaryDate" runat="server" 
                Selected_Date=""  />
            <br />
            <br />
            <div id="view2_div_err"></div>
            <label runat="server" id="view2_lbl_msg"></label>

            

            <h5 runat="server" id="view2_h5"></h5>
            <br />
            <asp:Button runat="server" ID="btn_retrieve_report" Text="Retrieve" Width="120" Height="40" />  
            &nbsp;&nbsp;<asp:Button ID="CheckToExport" runat="server" Width="120" Height="40" Text="Export" Visible="false" />   
        </div>        

        <!-- II REPORT Link  -->  
        <br /><br />
        <asp:GridView ID="gv_00" runat="server" AutoGenerateColumns="true" CssClass="f">
            <Columns>
            </Columns>
        </asp:GridView>
                                  
    </div>
    </asp:View>

    <asp:View runat="server" ID="View3">
        <div>            
            <div style="border:1px gray solid;margin-top:10px;">
            <h4 style="padding-left:20px;">Original points contracts sent to II later became cancelled.</h4>
            </div>

            <br />
            <div style="margin:0 auto;width:100%;">

                <div style="width:300px;float:left;">
                    <h5>Status changed during period.</h5>
                    <asp:CheckBoxList ID="cbl_cxl" runat="server"></asp:CheckBoxList>
                </div>

                <div style="float:left;width:300px;">
                    <h5>Contract Sub Status</h5>
                    <asp:CheckBoxList ID="cxl_sub_status" runat="server"></asp:CheckBoxList>
                </div>

                <div style="margin-left:610px;">
                    
                   <h5>Period cancels occured.</h5>
                   <table>
                    <tr>
                        <td>Begin Date</td>
                        <td>
                            <uc1:DateField ID="sdate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>End Date</td>
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

                <!-- CXL REPORT Link -->
                <asp:GridView ID="gv_02" runat="server" AutoGenerateColumns="true" CssClass="f">
                <Columns>                    
                </Columns>
            </asp:GridView>
            </div>

    </div>

                   
    </asp:View>

    <asp:View runat="server" ID="View4">
        <div>
            <div class="btn-group" role="group" aria-label="...">
                <asp:Button runat="server" CssClass="btn btn-primary" Text="II Rates" id="btnIIRatesView4" />    
                <asp:Button runat="server" CssClass="btn"  Text="Back"  id="btnBackView4" />
            </div>
            <asp:MultiView runat="server" ID="mvView4">
                <asp:View runat="server" ID="v1View4">
                    <div class="row" style="margin-left:2px;">            
                <asp:GridView runat="server" ID="gv1" AutoGenerateColumns="false" AutoGenerateSelectButton="true">                
                <Columns>
                    <asp:BoundField DataField="iiMemberRateID" ItemStyle-CssClass="col-md-2" HeaderText="Rate ID#" />      
                    <asp:BoundField DataField="II Membership" ItemStyle-CssClass="col-md-3"  DataFormatString="{0:0.00}" HeaderText="II Membership" />         
                    <asp:BoundField DataField="II Payback" ItemStyle-CssClass="col-md-3"  DataFormatString="{0:0.00}" HeaderText="II Payback" /> 
                    <asp:BoundField DataField="Reservation Fee" ItemStyle-CssClass="col-md-2"   DataFormatString="{0:0.00}" HeaderText="Reservation Fee" />    
                    <asp:BoundField DataField="Frequency" ItemStyle-CssClass="col-md-1" HeaderText="Frequency" />             
                </Columns>
            </asp:GridView>
            </div>
                </asp:View>
                <asp:View runat="server" ID="v2View4">
                    <div class="panel panel-default" style="width:600px;">
                        <div class="panel-heading">
                            <h6>Edit...   <span class="glyphicon glyphicon-pencil"></span> </h6>
                        </div>
                        <div class="panel-body">
                            <div>
                                <label >Frequency</label>
                                <p>
                                    <asp:DropDownList runat="server" ID="ddlFrequency" Enabled="false">
                                        <asp:ListItem>Annual</asp:ListItem>
                                        <asp:ListItem>Biennial</asp:ListItem>
                                        <asp:ListItem>Triennial</asp:ListItem>
                                    </asp:DropDownList>
                                </p>
                                <label>II Membership</label>
                                <p>
                                    <asp:TextBox runat="server" ID="txtB1" ></asp:TextBox>
                                </p>
                                 <label>II Payback</label>
                                <p>
                                    <asp:TextBox runat="server" ID="txtB2" ></asp:TextBox>
                                </p>
                                 <label>Reservation Fee</label>
                                <p>
                                    <asp:TextBox runat="server" ID="txtB3" ></asp:TextBox>
                                </p>
                            </div>
                    </div>
                        <div class="panel-footer">
                            <asp:Button CssClass="btn btn-default" runat="server" Text="Financials" ID="btnFinancialsV2" />
                            <asp:Button CssClass="btn btn-default" runat="server" Text="Back" id="btnBackV2" />
                            <asp:Button CssClass="btn btn-default"  runat="server" Text="Save" ID="btnSaveV2"  />                            
                        </div>
                    </div>
                </asp:View>
                <asp:View runat="server" ID="v3View4">
                    <h1>Financials</h1>
                    <div>
                        <table>
                            <tr>
                                <td>Begin Date</td>
                                <td>
                                    <uc1:DateField ID="dt_fin_start" runat="server" />
                                </td>
                                <td><div id="dv_fin_date_err"></div></td>
                            </tr>
                            <tr>
                                <td>End Date</td>
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
                        <p>
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
                        </p>               
                    </div>
                </asp:View>            
            </asp:MultiView>
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
           
            <!-- 
                UPGRADES & ADDITIONALS Link
            -->            
            <br /><br /><br />  
            <asp:GridView ID="gv_01" runat="server" AutoGenerateColumns="true" CssClass="f">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="asp_cb" runat="server" />                        
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </div>    
    </asp:View>

    <asp:View runat="server" ID="View8">
        
        <h2>Upload Excel Renewal List</h2>
        <asp:FileUpload runat="server" ID="fuRenewalList" Font-Size="Large" />&nbsp;&nbsp;
        <asp:Button runat="server" ID="btRenewalListUpload" Text="Upload" Font-Bold="true" Font-Size="Medium" />
        <br /><br />
        <asp:Button runat="server" ID="btIIBulkUpdate" Text="Submit" Visible="false" />
        <asp:Button ID="btnExportList" runat="server" Text="Export" />
        <br />
        <asp:Literal runat="server" ID="liRenewalList"></asp:Literal>
        <br /><br />
        <asp:GridView runat="server" ID="gvRenewalList" AutoGenerateColumns="true">
        </asp:GridView>
    </asp:View>
</asp:MultiView>



<br />
<asp:Label ID="lblError" runat="server"></asp:Label>
<asp:HiddenField runat="server" ID="hf_iiMembershipEnrollmentID" />
<asp:HiddenField runat="server" id="hf_iiMembershipEnrollmentID_upgrades" />




<script type="text/javascript">

    $(function () {

        $.fn.alternateRowColor = function () {
            $('tbody tr:odd', this).removeClass('even').addClass('odd');
            $('tbody tr:even', this).removeClass('odd').addClass('even');
            return this;
        }


        $('#<%= gvExport.ClientID %>').each(function () {
            var $table = $(this);

            $table.alternateRowColor($table);
            $table.find('th').addClass('th');
            $table.find('td').addClass('td');
            $table.css('border-collapse', 'collapsed');

            $('th', $table).each(function (col) {
                var findSortKey;

                if ($(this).is('.sort-alpha')) {
                    findSortKey = function ($cell) { return $cell.find('.sort-key').text().toUpperCase() + ' ' + $cell.text().toUpperCase(); };
                }
                else if ($(this).is('.sort-date')) {
                    findSortKey = function ($cell) { return Date.parse($cell.text()); };
                }


                if (findSortKey) {

                    $(this).addClass('clickable')
                        .hover(function () { $(this).addClass('hover'); },
                                                        function () { $(this).removeClass('hover'); })
                        .click(function () {

                            var newDirection = 1;
                            if ($(this).is('.sorted-asc')) {
                                newDirection = -1;
                            }

                            var rows = $table.find('tbody > tr').get();
                            $.each(rows, function (index, r) {
                                r.sortKey = findSortKey($(r).children('td').eq(col));                                
                            });

                            rows.sort(function (a, b) {
                                if (a.sortKey < b.sortKey) return -newDirection;
                                if (a.sortKey > b.sortKey) return newDirection;
                                return 0;
                            });

                            $.each(rows, function (index, row) {
                                $table.children('tbody').append(row);
                                row.sortKey = null;
                            });

                            $table.find('th').removeClass('sorted-asc').removeClass('sorted-desc');
                            var $sortHead = $table.find('th').filter(':nth-child(' + (col + 1) + ')');
                            if (newDirection == 1)
                                $sortHead.addClass('sorted-asc');
                            else
                                $sortHead.addClass('sorted-desc');

                            $table.find('td').removeClass('sorted').filter(':nth-child(' + (col + 1) + ')').addClass('sorted');
                            $table.alternateRowColor($table);
                            $table.trigger('repaginate');

                        });
                }
            });

        });

    });



        // paging...
    $(function () {


        $('#<%= gvExport.ClientID %>').each(function () {
            var currentPage = 0;
            var numPerPage = 20;
            var $table = $(this);

            $table.bind('repaginate', function () {
                $('tbody tr', $table).show();
                $('tbody tr:lt(' + (numPerPage * currentPage) + ')', $table).hide();
                $('tbody tr:gt(' + ((currentPage + 1) * numPerPage - 1) + ')', $table).hide();
            });


            var numRows = $table.find('tbody tr').length;
            var numPages = Math.ceil(numRows / numPerPage);
            var $pager = $('<div class="pager"></div>');

            for (var page = 0; page < numPages; page++) {
                $('<span class="page-number">' + (page + 1) + '</span>').bind('click', { 'newPage': page }, function (event) {
                    currentPage = event.data['newPage'];
                    $table.trigger('repaginate');
                    $(this).addClass('active').siblings().removeClass('active');
                }).appendTo($pager).addClass('clickable');
            }

            $pager.find('span.page-number:first').addClass('active');
            $pager.insertBefore($table);
            $table.trigger('repaginate');

        });


        //$('#<%= gvExport.ClientID %> td:nth-child(7), th:nth-child(7)').hide();

    });




    $(function () {

        var $anni_label = $('#anniversary-label').remove().text();
        $('input[name$="dtAnniversaryDate$txtDate"]').addClass('fill-italics').val($anni_label);

        var $s_date = $('input[id$=sdate_txtDate]').parents('tr:first');
        var $e_date = $('input[id$=edate_txtDate]').parents('tr:first');
        var $s_date_label = $('td:first-child', $s_date);
        var $e_date_label = $('td:first-child', $e_date);

        $('input[id$=sdate_txtDate]').val($s_date_label.text()).addClass('fill-italics');
        $('input[id$=edate_txtDate]').val($e_date_label.text()).addClass('fill-italics');

        $s_date_label.remove();
        $e_date_label.remove();

        $('input[id$=btn_ExportNonIIContract]').click(function () {

            $('#cxl_report_div_err').html('');

            if ($('input[id$=_sdate_txtDate]').val() == '' || $('input[id$=_sdate_txtDate]').val() == $s_date_label.text() || $('input[id$=_edate_txtDate]').val() == '' || $('input[id$=_edate_txtDate]').val() == $e_date_label.text()) {

                $('#cxl_report_div_err').html('<div style=color:red;font-style:italic;>Date range is not correct!</div>');
                return false;
            }
            else if ($('input:checked').length == 0) {

                $('#cxl_report_div_err').html('<div style=color:blue;font-style:italic;>Checkbox at least one status!</div>');
                return false;
            }
        });
       
    });
</script>
</asp:Content>

