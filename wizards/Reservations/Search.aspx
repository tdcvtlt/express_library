<%@ Page Title="" Language="VB" MasterPageFile="~/wizards/Reservations/ReservationMasterPage.master" Debug="true" AutoEventWireup="false" CodeFile="Search.aspx.vb" EnableEventValidation="false" ValidateRequest="false"  Inherits="wizard_Reservations_Search" %>
<%@ Register Src="~/controls/SyncDateField.ascx" TagPrefix="uc10" TagName="SyncDateField" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head2" Runat="Server">

    
    <style type="text/css">
         .loading
            {
                font-family:'Times New Roman';
                font-size: 10pt;
                border: 1px solid #67CFF5;
                width: 200px;
                height: 100px;
                display: none;
                position: fixed;
                background-color:white;
                z-index: 999;
                box-shadow:12px 12px 6px rgba(255,255,255,0.275);
                font-style:italic;
                color:darkblue;
            }
    </style>
   <script type="text/javascript">

       $(function () {            

           $('#<%= btSearch.ClientID %>').on('click', function (e) {
               $('#<%= btNext.ClientID %>').prop('disabled', true);
               $("#<%= spErr.ClientID %>").hide();
           });

           $('#<%= txP.ClientID %>').bind('blur', function (e) {
               
               $('#<%= btNext.ClientID %>').prop('disabled', false);

               var val = $(this).val().replace(/,/, "");
               var min = $('#<%= txMin.ClientID %>').val().replace(/,/, "");
               var max = $('#<%= txMax.ClientID %>').val().replace(/,/, "");
                           
               $('#<%= spErr.ClientID %>').hide();

               if (isNaN(val)) {
                   $('#<%= spErr.ClientID %>').show();
                   $('#<%= spErr.ClientID %>').text('Invalid input');
                   $('#<%= txP.ClientID %>').focus();
                    
                   $('#<%= btNext.ClientID %>').prop('disabled', true);

               } else {

                   if (parseFloat(max) > 0) {                                               

                       if (parseFloat(val) < parseFloat(min) || parseFloat(val) > parseFloat(max)) {
                           $('#<%= spErr.ClientID %>').show();
                           $('#<%= spErr.ClientID %>').text('Box Purchase must be between $' + $('#<%= txMin.ClientID %>').val() + ' and $' + $('#<%= txMax.ClientID %>').val());
                           $('#<%= txP.ClientID %>').focus();

                           $('#<%= btNext.ClientID %>').prop('disabled', true);
                       }
                   }
               }                             
           });

           var validator = $('form').validate({
               rules: {
                   ignore: '.skip',
                    ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$txP:  {
                       required: {
                           depends: function (el) {
                               if (parseFloat($('#<%= txMax.ClientID %>').val()) > 0) {
                                   return true;
                               } else {
                                   return false;
                                }
                            }                        
                       },
                       number:true,
                       maxlength: 50
                   }
               },

               highlight: function (element) {
                   $(element).closest('td').addClass('has-error');
               },
               unhighlight: function (element) {
                   $(element).closest('td').removeClass('has-error');
               },
               errorElement: 'span',
               errorClass: 'help-block',
               errorPlacement: function (error, element) {
                   error.insertAfter(element);
               }

           });       

       });


       $(function () {
           
           // checkboxes in table
           $("span[cbAttr] :checkbox:first-child").click(function (e) {

               $('#<%= spErr.ClientID %>').hide();

               var $gv = $('#<%= gvPackages.ClientID %>');
               var packageID = $(this).parent('span').attr('PackageID');
               var cb = $(this);
               var td = $(this).parents('td');
               var r_min = $(td).next().children('input[type=text]:first').val();

               var tr = $(this).parents('tr');
               var r_max = $(tr).children('td:nth-child(3)').children('input[type=text]:first').val();

               var qt = $(tr).children('td:nth-child(4)').children('select:first').val();

               r_min = parseFloat(r_min) * parseInt(qt);
               r_max = parseFloat(r_max) * parseInt(qt);

               console.log('packageID ' + packageID);

               var g_min = parseFloat($('#<% = txMin.ClientID %>').val());
               var g_max = parseFloat($('#<% = txMax.ClientID %>').val());

               var p_ids = new Array();
               if ($gv.attr('p_ids') == undefined || $gv.attr('p_ids').length == 0) {
                   $gv.attr('p_ids', 0);
               } else {
                   p_ids = $gv.attr('p_ids').split(',');
               }

               console.log($gv.attr('p_ids'));
               console.log(p_ids);

               if ($(cb).prop('checked')) {

                   g_min += r_min;
                   g_max += r_max;

                   p_ids.push(new Number(packageID));
                   $gv.attr('p_ids', p_ids);

                   console.log('checked ' + p_ids);
                   console.log($gv.attr('p_ids'));

                   g_min = 0;
                   g_max = 0;

                   $.each($gv.find('tr'), function (i, v) {

                       var cbx = $(this).find('td:nth-child(1) span input[type=checkbox]:first');

                       if ($(cbx).prop('checked')) {

                           var txMin = $(this).find('td:nth-child(2) input[name$=MinRate]');
                           var txMax = $(this).find('td:nth-child(3) input[name$=MaxRate]');
                           var ddQt = $(this).find('td:nth-child(4) select:first');

                           g_min += parseFloat(txMin.val()) * parseInt(ddQt.val());
                           g_max += parseFloat(txMax.val()) * parseInt(ddQt.val());
                       }
                   });

                   console.log(g_min + g_max);


               } else {

                   var i = $.inArray(packageID, p_ids);
                   console.log('i = ' + i);

                   p_ids.splice(i, 1);
                   $gv.attr('p_ids', p_ids);

                   console.log('unchecked ' + p_ids);
                   console.log($gv.attr('p_ids'));

                   g_min = 0;
                   g_max = 0;

                   $.each($gv.find('tr'), function (i, v) {

                       var cbx = $(this).find('td:nth-child(1) span input[type=checkbox]:first');

                       if ($(cbx).prop('checked')) {

                           var txMin = $(this).find('td:nth-child(2) input[name$=MinRate]');
                           var txMax = $(this).find('td:nth-child(3) input[name$=MaxRate]');
                           var ddQt = $(this).find('td:nth-child(4) select:first');

                           g_min += parseFloat(txMin.val()) * parseInt(ddQt.val());
                           g_max += parseFloat(txMax.val()) * parseInt(ddQt.val());
                       }
                   });

               }

               $('#<% = txMin.ClientID %>').val(g_min.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
               $('#<% = txMax.ClientID %>').val(g_max.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

               $('#<%= txP.ClientID %>').val(g_max.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));


               var extras = $(tr).children('td:nth-child(5)').children('select:first').val();
               var tb = $('#<%= gvPackages.ClientID %>');
               var rows = $(tb).find('tr');

               $.each(rows, function (key, r) {

                   // excludes th cells
                   if ($(r).has('td:first-child')) {

                       var extras = $(r).children('td:nth-child(5)').children('select:first').val();

                       // disable rows with either "Tour Required" or "Owner Getaway" when one of these were selected already
                       if ((extras == 'Tour Required' || extras == 'Owner Getaway') && ($(cb).parents('tr').children('td:nth-child(5)').children('select:first').val() == 'Tour Required' ||
                            $(cb).parents('tr').children('td:nth-child(5)').children('select:first').val() == 'Owner Getaway') && $(cb).prop('checked')) {

                           $(r).find('td:first-child :checkbox:first-child').prop('disabled', true);

                       }

                       // enable its counterparts when un-selected
                       if ((extras == 'Tour Required' || extras == 'Owner Getaway') && (($(cb).parents('tr').children('td:nth-child(5)').children('select:first').val() == 'Tour Required') ||
                            ($(cb).parents('tr').children('td:nth-child(5)').children('select:first').val() == 'Owner Getaway')) && $(cb).prop('checked') == false) {

                           $(r).find('td:first-child :checkbox:first-child').prop('disabled', false);

                           if ($(cb).parents('tr').children('td:nth-child(5)').children('select:first').val() == 'Tour Required') { $('#twArea').html(''); }
                       }

                   }

                   cb.prop('disabled', false);

               });

               // enable/disable Next button
               if (g_min > 0 && g_max > 0) {

                   $('#<%= btNext.ClientID %>').prop('disabled', false);
               } else {
                   $('#<%= btNext.ClientID %>').prop('disabled', true);
               }

               // get tour
               if ($(cb).parents('tr').children('td:nth-child(5)').children('select:first').val() == 'Tour Required' && $(cb).prop('checked')) {

                   // new Date(new Date($('#<%= datePicker1.ClientID %>').datepicker('getDate')).getTime() + (0 * 24 * 60 * 60 * 1000)).toLocaleDateString("en-US")

                   var get_full_year = new Date($('#<%= datePicker1.ClientID %>').datepicker('getDate')).getFullYear();
                   var get_month = new Date($('#<%= datePicker1.ClientID %>').datepicker('getDate')).getMonth() + 1;
                   var get_date = new Date($('#<%= datePicker1.ClientID %>').datepicker('getDate')).getDate();                  

                   var start_date = get_full_year + '/' + get_month + '/' + get_date;

                   $.ajax({
                       type: "POST",
                       url: "Search.aspx/ufn_ToursAvailability",
                       data: "{package_id:'" + $(cb).parent('span').attr('PackageID') + "',tour_date:'" + start_date + "',nights_stay:'" + $('#<%= ddStayDays.ClientID %>').val() + "'}",
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       success: function (tw) {

                           var table = '<table class=table table-stripped><thead><tr><th>Tour Dates</th><th>Available</th></tr></thead>';
                           var row = '<tr><td></td><td></td></tr>';

                           var js = JSON.parse(tw.d);

                           $.each(js, function (index, value) {
                               row += '<tr><td>' + value.TourDate + '</td><td>' + value.Counts + '</td></tr>';
                           });

                           table += row;
                           table += '</table>';

                           $('#twArea').html(table);

                       },
                       error: function (msg) {
                           alert("error:" + JSON.stringify(msg));
                       }
                   });



               }



           });


       });


       
   </script>    

   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">             
    <div>
        <div class="loading" align="center">
            <span class="clock-icon" style="vertical-align:top;"></span>&nbsp;Executing..., Please wait!<br />
            <br />
            <img src="../../images/progressbar.gif" alt="" />
        </div>
        
        
        <div id="resultArea"></div>

        <div class="container">
                
            <div class="row top-buffer">
                <div class="col-md-12">                            
            <asp:MultiView ActiveViewIndex="0" ID="Container" runat="server">
                <asp:View ID="View1" runat="server">

                    <div class="panel panel-success">
                        <div class="panel-heading">
                            <div class="panel-title">  
                                <h3 style="font-weight:bold;" class="text-primary">Search...</h3>
                                <h4 class="control-label text-right  ">King's Creek Plantation &#174;</h4>                                                                              
                            </div>
                        </div>
                        <div class="panel-body">
                            <div style="position:relative;width:100%;float:left;">

                                <div style="width:80%;border:0px solid blue;float:left;">
                                    <div style="width:100%;display:inline-block;border:0px solid red;">
                                    <table border="0" style="width:100%;" class="table">
                                        <tr>
                                            <td colspan="5">
                                        <fieldset>
                                            <asp:RadioButton GroupName="rb" runat="server" ID="rHotel" Font-Size="Larger"  Visible="false" Text="&nbsp;Hotel" />
                                            &nbsp;&nbsp;
                                            <asp:RadioButton GroupName="rb" runat="server" Font-Size="Larger"  ID="rResort" Visible="false" Text="&nbsp;Resort" />                            
                                        </fieldset>
                                    </td>
                                        </tr>
                                <tr>
                                    <td colspan="5">
                                        <div class="row top-buffer">
                                            <div class="col-sm-4">
                                                        
                                                <asp:Label runat="server" ID="lbPackage" Text="Package" CssClass="control-label" Font-Size="Medium"></asp:Label>

                                                    <div>
                                                        <asp:DropDownList runat="server" ID="ddPackage" class="form-control" style="width:90%;">
                                                            <asp:ListItem Value="All">All</asp:ListItem>
                                                            <asp:ListItem Value="Owner Getaway">Owner Getaway</asp:ListItem>
                                                            <asp:ListItem Value="Tour Promotion">Tour Promotion (Marketing)</asp:ListItem>
                                                            <asp:ListItem Value="Rental">Rental</asp:ListItem>
                                                            <asp:ListItem Value="Tour Package">Tour Package</asp:ListItem>
                                                            <asp:ListItem Value="Tradeshow">Tradeshow</asp:ListItem>                                                                    
                                                        </asp:DropDownList>
                                                    </div>
                                            </div>
                                            <div class="col-sm-3">

                                                <asp:Label runat="server" ID="lbUnit" Text="Unit" CssClass="control-label" Font-Size="Medium"></asp:Label>
                                                        
                                                    <div>
                                                        <asp:DropDownList runat="server" ID="ddUnit" class="form-control" style="width:90%;">
                                                            <asp:ListItem Value="All">All</asp:ListItem>
                                                            <asp:ListItem Value="Cottage">Cottage</asp:ListItem>
                                                            <asp:ListItem Value="Estates">Estates</asp:ListItem>
                                                            <asp:ListItem Value="Townes">Townes</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                            </div>                                                    
                                        </div>
                                    </td>         
                                </tr>
                                <tr>
                                    <td style="vertical-align:top;width:30%;padding-top:20px;">
                                        <strong>Check-In</strong>
                                        <br />
                                        <div style="display:none;">
                                            <asp:TextBox runat="server" id = "dtF" readonly="true" Visible="false"></asp:TextBox>
                                            <asp:Button ID="Button2" runat="server" Text="..." Visible="false" ></asp:Button>
                                            <asp:Calendar runat="server" id = "Calendar1" visible = "false"></asp:Calendar>
                                        </div>                                        
                                        
                                       <asp:TextBox runat="server" ID="flatpickr1" Visible="false" placeholder="Pick date" CssClass="form-control" data-week-numbers=true data-date-format="m/d/Y" data-input></asp:TextBox>

                                        <asp:TextBox runat="server" ID="datePicker1" placeholder="Pick date" CssClass="form-control"></asp:TextBox>
                                    </td>
                                    <td style="vertical-align:top;width:30%;padding-top:20px;">
                                        <strong>Check-Out</strong>
                                        <asp:TextBox runat="server" ID="dtT" CssClass="form-control" style="display:block;"></asp:TextBox>
                                                                                
                                    </td>
                                    <td style="vertical-align:top;width:30%;padding-top:20px;">
                                        <strong>Nights</strong>
                                                                                
                                        <asp:DropDownList runat="server" ID="ddStayDays" CssClass="form-control" AutoPostBack="false">
                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="3" Selected="True" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                            <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                            <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>                                                                                                                                                   
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>     
                                        <br />                         
                                        <asp:Button runat="server" CssClass="btn btn-lg btn-danger" ID="btSearch" OnClientClick="ShowProgress();" style="width:80%;"  Text="Search..." />                           
                                    </td>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                            </table>                        
                                </div>
                                </div>

                                <div style="width:19%;border:0px solid gray;float:right;">

                                    <table style="margin-top:10PX;" border="0">
                                        <tr>
                                            <td style="width:20px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>                                                                                                    
                                                <div id="twArea" style="width:200px;border:0px solid red;display:inline-block;margin-top:10px;">                                                
                                                </div>
                                                <asp:GridView runat="server" ID="gvToursAvail" AutoGenerateColumns="False" CssClass="table table-stripped table-bordered">
                                                <Columns>
                                                    <asp:BoundField DataField="TourDate" DataFormatString="{0:d}" HeaderText="Tour Dates">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Counts" HeaderText="Counts">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <HeaderStyle BackColor="#8FB3C0" />                                                                                        
                                            </asp:GridView>
                                            </td>
                                        </tr>                        
                                    </table>

                                </div>
                                                                                                                                                          
                            </div>
                                                
                        </div>

                    </div>
                                                        
                                                        
                    <div class="panel panel-body">                                                                               
                        <div runat="server" id="dvR_Container" style="clear:left;margin-top:0px;">     
                            <div>   
                                <span runat="server" id="spErr" style="" visible="false"></span>                                                                 
                            </div>
                            <table style="width:100%;">
                            <tr style="height:70px;">
                                <td>
                                    <asp:CheckBox runat="server" AutoPostBack="true" ID="cbAuto" Enabled="false" />                            
                                    <asp:Label ID="lbAutoRoomSelect" runat="server" Font-Size="Large" CssClass="control-label input-lg " Text="Auto Room Select"></asp:Label>
                                </td>
                                <td>                            
                                </td>
                                <td style="text-align:right;">    
                                    <asp:Label ID="lbMin" runat="server" Text="Minimum" Font-Size="Medium" CssClass="control-label" style="padding-right:6px;"></asp:Label>         
                                    <br />                                               
                                    <asp:TextBox runat="server" ID="txMin" ReadOnly="true" CssClass="form-control" Font-Size="Medium" Text="0.00" Font-Bold="true"  style="text-align:right;vertical-align:middle;width:70%;float:right;"></asp:TextBox>
                                </td>
                                <td>                            
                                </td>
                                <td style="text-align:right;">      
                                    <asp:Label ID="lbMax" runat="server" Text="Maximum" Font-Size="Medium" CssClass="control-label" style="padding-right:6px;"></asp:Label>    
                                    <br />                                                                                                              
                                    <asp:TextBox runat="server" ID="txMax" ReadOnly="true" CssClass="form-control" Font-Size="Medium"  Font-Bold="true"   Text="0.00" style="text-align:right;vertical-align:middle;width:70%;float:right;"></asp:TextBox>
                                </td>
                                <td>&nbsp;                            
                                </td>
                                <td style="text-align:right;">
                                    <asp:Label ID="lbPurchase" runat="server" Text="Purchase" Font-Size="Medium" CssClass="control-label" style="padding-right:6px;"></asp:Label>              
                                    <br />                                                                                                                                                            
                                    <asp:TextBox runat="server" ID="txP"  CssClass="form-control" AutoPostBack="false" Font-Bold="true"  name="txP" Text="0.00" Font-Size="Medium" style="text-align:right;vertical-align:middle;width:60%;float:right;"></asp:TextBox>
                                </td>   
                                <td>&nbsp;</td>                     
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:GridView runat="server" ID="gvPackages" GridLines="Horizontal"  AutoGenerateColumns="false" CssClass="table table-striped table-hover table-bordered" style="width:100%;">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Packages Available" HeaderStyle-Width="300" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:checkbox text="" AutoPostBack="false" CssClass="text-primary"  ID="cbS" Font-Size="Medium" runat="server" />                                            
                                                </ItemTemplate>                                        
                                            </asp:TemplateField>                                    
                                            <asp:TemplateField HeaderText="Minimum $" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="MinRate" ReadOnly="true" Font-Size="Large" CssClass="form-control" style="text-align:right;" />
                                                </ItemTemplate>                                        
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Maximum $" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="MaxRate" ReadOnly="true" Font-Size="Large" CssClass="form-control" style="text-align:right;" />
                                                </ItemTemplate>                                        
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddQ" AutoPostBack="false" CssClass="form-control" Font-Size="Larger" style="text-align:right;">
                                                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>                                        
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Extras">
                                                <ItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddExtra" Font-Size="Larger" Enabled="false" CssClass="form-control" style="text-align:right;">
                                                        <asp:ListItem Text=""></asp:ListItem>
                                                        <asp:ListItem Text="Tour Required"></asp:ListItem>
                                                        <asp:ListItem Text="Owner Getaway"></asp:ListItem>
                                                        <asp:ListItem Text="Rental"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>                                        
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView runat="server" ID="gridview1" GridLines="Horizontal"  AutoGenerateSelectButton="true" CssClass="table table-striped table-hover table-bordered" style="width:100%;">
                                    </asp:GridView>
                                    <asp:GridView runat="server" ID="gridview_hotel" GridLines="Horizontal"  AutoGenerateSelectButton="true" CssClass="table table-striped table-hover table-bordered" style="width:100%;">
                                    </asp:GridView>
                                    <br />
                                    <asp:GridView runat="server" ID="gridview2" GridLines="Horizontal" AutoGenerateSelectButton="true" DataKeyNames="PACKAGEID"  CssClass="table table-striped table-hover table-bordered" style="width:100%;">
                                    </asp:GridView>


                                    <asp:Literal runat="server" ID="ll1"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                        </div>  
                    </div>                              
                    <br />                
                </asp:View>            
            </asp:MultiView>
        </div>        
            </div>
            <div class="row top-buffer">                                        
                            <div class="col-md-12">
                        

                                <div>
                                    <asp:Button runat="server" ID="btPrevious" CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="&larr; Previous" />
                                    &nbsp;&nbsp;
                                    <asp:Button runat="server" ID="btNext" CssClass="btn btn-lg btn-success" OnClientClick="ShowProgress();" Style="width:160px;font-weight:bolder;" Enabled="false" Text="Next &rarr;" />
                                </div>

                            </div>
                        </div>
        </div>        
    </div>
    

    <div>
        <asp:HiddenField runat="server" ID="hf_list_packages" Value="" />
        <asp:HiddenField runat="server" ID="hf_default_package_id" />
    </div>


    <script type="text/javascript">

        $(function () {

            $('.exchange').click(function () {


                var arr = [];       // array to store packages to a server hidden field
                var arr_rm = [];    // array to remove un-checked packages

                var parent_package_id = $(this).attr("parent-package-id");
                var child_package_id =  $(this).attr("child-package-id");
                var parent_guid = $(this).attr("parent-guid");
                var child_guid = $(this).attr("child-guid");
                var price_min =  $(this).attr('price-min');
                var price_max = $(this).attr('price-max');
                var total_price_min = 0;
                var total_price_max = 0;

                if ($('#<%= hf_list_packages.ClientID %> ').val().length > 0) {                    
                    arr = JSON.parse($('#<%= hf_list_packages.ClientID %> ').val());
                }                
                $.each(arr, function (index, e) {
                    if (e.parent_guid == parent_guid) {
                        if (e.parent_guid != child_guid) {
                            arr_rm.push(index);
                        }                        
                    }                    
                });
                for (var i = 0; i < arr_rm.length; i++) {
                    arr.splice(arr_rm[i], 1);
                }
                var model = {};
                model.parent_package_id = parent_package_id;
                model.child_package_id = child_package_id;
                model.price_min = price_min;
                model.price_max = price_max;
                model.parent_guid = parent_guid;
                model.child_guid = child_guid;
                arr.push(model);              
                        
                $.each(arr, function (index, e) {
                    total_price_min += parseFloat(e.price_min);
                    total_price_max += parseFloat(e.price_max);
                });
                var c = JSON.stringify(arr);
                $('#<%= hf_list_packages.ClientID %> ').val(c);
                $('#<%= txMin.ClientID %>').val(total_price_min);
                $('#<%= txMax.ClientID %>').val(total_price_max);
                $('#<%= txP.ClientID %>').val(total_price_max);

                if (parseFloat($('#<%= txP.ClientID %>').val()) > 0) {
                    $('#<%= btNext.ClientID %>').removeAttr('disabled');
                } else {
                    $('#<%= btNext.ClientID %>').attr('disabled', 'disabled');
                }
                console.log(arr);                                               
            });
        });
    </script>
    
    <script type="text/javascript">

        $(function () {

            $('#<%= datePicker1.ClientID %>').datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,

                onSelect: function () {                    
                                        
                    var scenario = $("#<%= ddStayDays.ClientID %>").attr('scenario');                        
                    if (scenario != undefined) {

                        $('#<%= ddStayDays.ClientID %>').empty();

                        $.ajax({
                            type: "POST",
                            url: "Search.aspx/GetNights",
                            data: "{chkin:'" + $('#<%= datePicker1.ClientID %>').val() + "', package_id:'" + $('#<%= hf_default_package_id.ClientID %>').val() + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (tw) {

                                var js = JSON.parse(tw.d);
                                $.each(js, function (index, v) {
                                    $('#<%= ddStayDays.ClientID %>').append($('<option>', { value: v.Counter, text: v.Counter, selected: v.Selected }));
                                });
                                $('#<%= ddStayDays.ClientID %> option').each(function () {

                                    if ($(this).prop('selected') == 'true') {
                                        $(this).prop('selected', true);
                                    }
                                });
                                var days = $("#<%= ddStayDays.ClientID %>").val();
                                var m = moment(new Date($('#<%= datePicker1.ClientID %>').val()));
                                var n = m.add(parseInt(days), 'days');
                                $('#<%= dtT.ClientID %>').val(new Date(n.format()).toLocaleDateString("en-US"));
                            },
                            error: function (msg) {
                                alert("error:" + JSON.stringify(msg));
                            }
                        });
                    } else {
                        var days = $("#<%= ddStayDays.ClientID %>").val();                        
                        var m = moment(new Date($('#<%= datePicker1.ClientID %>').val()));
                        var n = m.add(parseInt(days), 'days');
                        $('#<%= dtT.ClientID %>').val(new Date(n.format()).toLocaleDateString("en-US"));
                    }
                }
            });

            // dropdown nights updates the checkin calendar
            $('#<%= ddStayDays.ClientID %>').change(function () {               
                var checkIn = new Date($('#<%= datePicker1.ClientID %>').datepicker('getDate'));
                var m = moment(checkIn);
                var n = m.add(parseInt($(this).val()), 'days');
                console.log(n.format());                   
                $('#<%= dtT.ClientID %>').val(new Date(n.format()).toLocaleDateString("en-US"));
            });


            // select quantity 
            $('select[name$=ddQ]').change(function () {

                $('#<%= spErr.ClientID %>').hide();

                var $gv = $('#<%= gvPackages.ClientID %>');
                var cb = $(this).parents('tr').children('td:first').find('span input[type=checkbox]:first');
                var g_min = new Number(0);
                var g_max = new Number(0);

                if (cb.prop('checked')) {

                    $.each($gv.find('tr'), function (i, v) {

                        var cbx = $(this).find('td:nth-child(1) span input[type=checkbox]:first');

                        if ($(cbx).prop('checked')) {

                            var txMin = $(this).find('td:nth-child(2) input[name$=MinRate]');
                            var txMax = $(this).find('td:nth-child(3) input[name$=MaxRate]');
                            var ddQt = $(this).find('td:nth-child(4) select:first');

                            g_min += parseFloat(txMin.val()) * parseInt(ddQt.val());
                            g_max += parseFloat(txMax.val()) * parseInt(ddQt.val());
                        }
                    });

                    $('#<%= txMin.ClientID %>').val(g_min.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                    $('#<%= txMax.ClientID %>').val(g_max.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                    $('#<%= txP.ClientID %>').val(g_max.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

                } else {



                }


            });

        });


        function beforeAsyncPostBack() { }
        function afterAsyncPostBack() {

            
        }

        Sys.Application.add_init(appInit);

        function appInit() {
            var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
            pgRegMgr.add_beginRequest(BeginHandler);
            pgRegMgr.add_endRequest(EndHandler);
        }

        function BeginHandler() {            
            $('div.loading').hide();
            $('.modal').hide();
            beforeAsyncPostBack();
        }

        function EndHandler() {
            $('div.loading').hide();
            $('.modal').hide();
            clearTimeout(timeout);
            afterAsyncPostBack();
        }
    </script>

    <script type="text/javascript">
        var timeout;

        function ShowProgress() {
            timeout = setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
                
            }, 0);
        }   
    </script>

</asp:Content>

