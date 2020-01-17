<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="IIEnrollment.aspx.vb" Inherits="Reports_CustomerService_IIEnrollment" EnableEventValidation="false" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    

    <style type="text/css">

       
         .loading
            {
                font-family:'Times New Roman';                
                font-size:larger;
                border: 0px solid #67CFF5;
                width: 200px;
                height: 100px;
                display: none;
                position: fixed;
                background-color:ghostwhite;
                z-index: 999;
                box-shadow:12px 12px 6px rgba(255,255,255,0.275);                
                color:black;
            }
    </style>
    <style>

    
ul#menu4{
	margin:0;
	padding:0;
	list-style-type:none;
	width:890px;
	position:relative;
	display:block;
	height:41px;
	text-transform:uppercase;
	font-size:12px;
	font-weight:bold;
	background:url('../../images/OFF.gif') repeat-x left top;
	font-family:Helvetica,Arial,Verdana,sans-serif;
	border-bottom:4px solid #336666;
	border-top:1px solid #C0E2D4;
    top: 0px;
    left: -2px;
}
ul#menu4 li{
	display:block;
	float:left;
	margin:0;
	padding:0;
	}
	
ul#menu4 li a{
	display:block;
	float:left;
	color:#874B46;
	text-decoration:none;
	font-weight:bold;
	padding:12px 20px 0 20px;
	height:34px;
	background:transparent url("../../images/DIVIDER.gif") no-repeat top right;
	}
ul#menu4 li a:hover{
	background:transparent url("../../images/HOVER.gif") no-repeat top right;	
	}
ul#menu4 li.current a{
    background:transparent url("../../images/HOVER.gif") no-repeat top right;	
}

</style>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="loading" style="align-content:center;border:2px solid ghostwhite">
        <span class="clock-icon" style="vertical-align:top;">&nbsp;loading..., please wait!<br /></span>
        <br />
        <img src="../../images/progressbar.gif" alt="" style="display:block;margin:0 auto;" />
    </div>

    <asp:HiddenField runat="server" ID="hfGridViewColumnHeaders" />
    <br />

    <ul id="menu4">
        <li <% if MultiView1.GetActiveView().Equals(View1) then: response.write("class=""current"""):end if %>>
            <asp:LinkButton ID="lnkView1"  runat="server">Points Contracts</asp:LinkButton>        
        </li>
        <li <% if MultiView1.GetActiveView().Equals(View2) then: response.write("class=""current"""):end if %>>
            <asp:LinkButton ID="lnkView2" runat="server">II Report</asp:LinkButton>
        </li>
        <li <% if MultiView1.GetActiveView().Equals(View3) then: response.write("class=""current"""):end if %>>
            <asp:LinkButton ID="lnkView3" runat="server">CXL Report</asp:LinkButton>
        </li>
        <li <% if MultiView1.GetActiveView().Equals(View4) then: response.write("class=""current"""):end if %>>
            <asp:LinkButton ID="lnkView4" runat="server">Financials</asp:LinkButton>
        </li>
  
        <li <% if MultiView1.GetActiveView().Equals(View5) then: response.write("class=""current"""):end if  %>>
            <asp:LinkButton ID="lnkView5" runat="server">Upgrades & Additionals</asp:LinkButton>
        </li>

        <li <% if MultiView1.GetActiveView().Equals(View6) then: response.write("class=""current"""):end if  %>>
            <asp:LinkButton ID="lnkView6" runat="server">Upload Excel</asp:LinkButton>
        </li>
    </ul>



    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <div style="">    

            <div style="border:1px gray solid;margin-top:10px;">
                <h4 style="padding-left:20px;">New Points Contracts.</h4>
            </div>
        
            <br /><br /> 

            <p>
                <asp:Label runat="server" ID="lbInfo_View1" ForeColor="Red" Font-Italic="true" Text=""></asp:Label>
            </p>

            <fieldset>
            <legend><span style="font-family:Arial Narrow;font-size:x-large;">Optional: </span></legend>
            <asp:CheckBoxList ID="cblPointsStatuses" runat="server" Height="200" RepeatDirection="Horizontal" RepeatColumns="7" Font-Size="Large" CssClass="table table-condensed table-bordered"></asp:CheckBoxList>       
        </fieldset>     

            <asp:Button runat="server" ID="btPointsRetrieve" CssClass="btn btn-primary" OnClientClick="ShowProgress();" Text="Retrieve" Width="120" Height="40" /> &nbsp;
            <asp:Button ID="btPointsSubmit" runat="server" CssClass="btn btn-danger" OnClientClick="ShowProgress();" Width="120" Height="40" Text="Update" Enabled="false" />    &nbsp;
            <br /><br />                    
            <br />

            <asp:GridView ID="gvPointsNew" runat="server" DataKeyNames="IIMembershipEnrollmentID" CssClass="table table-condensed table-bordered table-hover table-striped">
                <Columns>                               
                    <asp:TemplateField><ItemTemplate>
                        <asp:CheckBox ID="cb" runat="server" /></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                                             
            </div>   

        </asp:View>

        <asp:View ID="View2" runat="server">
            <div style="border:1px gray solid;margin-top:10px;">
                <h4 style="padding-left:20px;">Points contracts and inventories were never sent to II.</h4>
                <br />                    
            </div>

            <br />    
                <asp:Label runat="server" ID="lbInfo_View2" ForeColor="Red" ></asp:Label>
            <br />    
            <h5 id="anniversary-label">Anniversary Date</h5>
            <uc1:DateField ID="dtAnniversaryDate" runat="server" />
            <br />
            <asp:Button runat="server" ID="btIIMemberRetrieve" OnClientClick="ShowProgress();" CssClass="btn btn-success" Width="120" Height="40" Text="Retrieve" />
            &nbsp;
            <asp:Button runat="server" ID="btIIMemberUpdateAndExport" CssClass="btn btn-danger" Width="160" Enabled="false" Height="40" Text="Update & Export" />
            <br />    
            <br />    


            <asp:GridView runat="server" ID="gvIIMembersNew" DataKeyNames="IIMembershipEnrollmentID" CssClass="table table-condensed table-bordered table-hover table-striped">
            </asp:GridView>

        </asp:View>

        <asp:View ID="View3" runat="server">

            <div style="border:1px gray solid;margin-top:10px;">
                <h4 style="padding-left:20px;">Original Points contracts sent to II now got canceled.</h4>
                <br />                    
            </div>

            <br /> 
            <asp:Label runat="server" ID="lbInfo_View3" ForeColor="Red"></asp:Label>
            <br />    
            <br /> 

            <table>
                <tr>
                    <td style="width:40%">
                        <asp:CheckBoxList runat="server" ID="cblStatus"></asp:CheckBoxList>
                        <br />

                        <h3>Contract Status Dates</h3>
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
                    </td>
                    <td>
                        <asp:CheckBoxList runat="server" ID="cblSubStatus" RepeatDirection="Horizontal" RepeatColumns="5" CssClass="table table-condensed table-bordered"></asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td>                        
                        <asp:Button ID="btIIMemberCancel" CssClass="btn btn-success" OnClientClick="ShowProgress();" runat="server" Width="120" Height="40" Text="Retrieve" />&nbsp;
                        <asp:Button ID="btIIMemberCancelUpdateAndExport" CssClass="btn btn-danger" runat="server" Width="140" Height="40" Text="Update & Export" Enabled="false" /> 
                    </td>
                    <td></td>
                </tr>
            </table>

            <br />
            <asp:GridView runat="server" CssClass="table table-condensed table-bordered table-hover  table-striped" ID="gvCancelsNew" DataKeyNames="IIMembershipEnrollmentID">
                <Columns>                               
                    <asp:TemplateField><ItemTemplate>
                        <asp:CheckBox ID="cb" runat="server" /></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>
                
        <asp:View ID="View4" runat="server">

            <div style="border:1px gray solid;margin-top:10px;">
                <h4 style="padding-left:20px;">II Financials</h4>
                <br />                    
            </div>

            <br />    
            <br />                

            <div>
            
            <div class="btn-group" role="group" aria-label="...">
                <asp:Button runat="server" CssClass="btn btn-primary" Text="II Rates" id="btnIIRatesView4" />    
                <asp:Button runat="server" CssClass="btn btn-warning"  Text="Back..."  id="btnBackView4" />
            </div>
            <br /><br />
            <asp:MultiView runat="server" ID="mvView4">
                <asp:View runat="server" ID="v1View4">
                    <div class="row" style="margin-left:2px;">            
                <asp:GridView runat="server" ID="gv1" AutoGenerateColumns="false" AutoGenerateSelectButton="true" CssClass="table table-hover table-condensed">                
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
                                    <asp:DropDownList runat="server" ID="ddlFrequency" Enabled="false" CssClass="form-control">
                                        <asp:ListItem>Annual</asp:ListItem>
                                        <asp:ListItem>Biennial</asp:ListItem>
                                        <asp:ListItem>Triennial</asp:ListItem>
                                    </asp:DropDownList>
                                </p>
                                <label>II Membership</label>
                                <p>
                                    <asp:TextBox runat="server" ID="txtB1" CssClass="form-control"></asp:TextBox>
                                </p>
                                 <label>II Payback</label>
                                <p>
                                    <asp:TextBox runat="server" ID="txtB2" CssClass="form-control"></asp:TextBox>
                                </p>
                                 <label>Reservation Fee</label>
                                <p>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtB3" ></asp:TextBox>
                                </p>
                            </div>
                    </div>
                        <div class="panel-footer">
                            <asp:Button CssClass="btn btn-success" runat="server" Text="Financials" ID="btnFinancialsV2" />&nbsp;
                            <asp:Button CssClass="btn btn-primary" runat="server" Text="Back" id="btnBackV2" />&nbsp;
                            <asp:Button CssClass="btn btn-danger"  runat="server" Text="Save" ID="btnSaveV2"  />                            
                        </div>
                    </div>
                </asp:View>
                <asp:View runat="server" ID="v3View4">                    
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
                                    <asp:Button ID="btn_fin_submit" runat="server" Text="Submit" OnClientClick="ShowProgress();" CssClass="btn btn-success" Height="40" Width="120" />
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

        <asp:View ID="View5" runat="server">
            
            <div style="border:1px gray solid;margin-top:10px;">
                <h4 style="padding-left:20px;">Points contracts sent to II now got upgraded or owners made more purchases.</h4>
                <br />                    
            </div>

            <br />  
            <asp:Label runat="server" ID="lbInfo_View5" ForeColor="Red"></asp:Label>
            <br />                

            <asp:Button runat="server" ID="btIIMemberUpgrade" OnClientClick="ShowProgress();" CssClass="btn btn-success" Width="120" Height="40" Text="Retrieve" />&nbsp;
            <asp:Button runat="server" ID="btIIMemberUpgradeAndExport" CssClass="btn btn-danger" Width="140" Height="40" Text="Update & Export" Enabled="false" />

            <br /><br /> 

            <asp:GridView runat="server" ID="gvIIMembersUpgrade" DataKeyNames="IIMembershipEnrollmentID" CssClass="table table-condensed table-bordered table-hover">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb" runat="server" />                        
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>   
        </asp:View>

        <asp:View ID="View6" runat="server">
            
             <div style="border:1px gray solid;margin-top:10px;">
                <h4 style="padding-left:20px;">Upload Excel Renewal List</h4>
                <br />                    
            </div>
            
            <br />    
            <br />                 
              
            <asp:FileUpload runat="server" ID="fuRenewalList" Font-Size="Large" />&nbsp;&nbsp;
            <asp:Button runat="server" ID="btRenewalListUpload" Text="Upload" Font-Bold="true" Font-Size="Medium" />
            <br /><br />
            <asp:Button runat="server" ID="btIIBulkUpdate" CssClass="btn btn-danger" Width="120" Height="40" Text="Submit" Visible="false" />&nbsp;
            <asp:Button ID="btnExportList" runat="server" CssClass="btn btn-success" Width="120" Height="40" Text="Export" />
            <br />         
            <br />         


        
        
            <asp:Literal runat="server" ID="liRenewalList"></asp:Literal>
            <br /><br />
            <asp:GridView runat="server" ID="gvRenewalList" AutoGenerateColumns="true">
            </asp:GridView>

        </asp:View>
    </asp:MultiView>


    <link rel="Stylesheet"  href="../../Styles/bootstrap-3.3.5/css/bootstrap.min.css"  />
    <script type="text/javascript" src="../../Styles/bootstrap-3.3.5/js/bootstrap.min.js"></script>




    <script type="text/javascript">

        function beforeAsyncPostBack() {}
        function afterAsyncPostBack() {}

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

