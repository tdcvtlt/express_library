<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="TourWaveLimits.aspx.vb" Inherits="setup_TourWaveLimits" %>
<%@ Register Src="~/controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register Src="~/controls/DateField.ascx" TagName="df" TagPrefix="kcp" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <style type="text/css">

#ctl00_ContentPlaceHolder1_b_xml
{
    background: transparent url(../images/HOVER.gif) no-repeat top right;
    width:135px;
    height:33px;
    cursor:pointer;
}

.legend-box
{
    width:20px;    
    height:20px;
}



</style>
     
    <script type="text/javascript">

        $(function () {


            var campaigns = new Array();



            $("#ctl00_ContentPlaceHolder1_DropDownList3").css({ "color": "red", "font-size": "40px",
                "width": "90px", "font-weight": "bold"
            });


            $("#ctl00_ContentPlaceHolder1_DropDownList5").css({ "color": "blue", "font-size": "29px",
                "width": "160px", "font-weight": "bold"
            });


            $("#ctl00_ContentPlaceHolder1_DropDownList4").css({ "color": "gray", "font-size": "22px",
                "width": "160px", "font-weight": "bold"
            });


            $("#" + "<%= b_save.ClientID %>").click(function () {

                $("#validation_summary").text("");

                if ($("#ctl00_ContentPlaceHolder1_TourWaveLimitsID").val() > 0) {
                    return true;
                }


                if ($("input[type=checkbox]:checked", $(".locations")).length == 0) {

                    $("#validation_summary").text("Select at least one location.");
                    return false;

                } else if ($("input[type=checkbox]:checked", $(".waves")).length == 0) {

                    $("#validation_summary").text("Select at least one wave.");
                    return false;

                } else if ($("input[type=checkbox]:checked", $(".campaigns")).length == 0) {

                    $("#validation_summary").text("Select at least one campaign type.");
                    return false;

                } else if ($("input[type=checkbox]:checked", $(".weekdays")).length == 0) {

                    $("#validation_summary").text("Select at least one day of week.");
                    return false;

                } else if ($("#" + "<%= DropDownList3.ClientID %>").find("option:selected").val() == 0) {

                    $("#validation_summary").text("Tour limits must be greater than 0.");

                    return false;

                } else if ($("#ctl00_ContentPlaceHolder1_dfTransDate_txtDate").val() == "") {

                    $("#validation_summary").text("Start date is missing.");

                    return false;
                }

                else if ($("#ctl00_ContentPlaceHolder1_dfEndDate_txtDate").val() == "") {

                    $("#validation_summary").text("End date is missing.");

                    return false;

                }


            });

        });

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <asp:HiddenField ID="TourWaveLimitsID" runat="server" Value="-1" />

<div id="content2_page">

    <ul id="menu">
    <li <% if MultiView1.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="lnk_View1" runat="server">View</asp:LinkButton>        
    </li>
    <li <% if MultiView1.ActiveViewIndex = 1 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="lnk_View2" runat="server">Add</asp:LinkButton>
    </li>

    <li <% if MultiView1.ActiveViewIndex = 2 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="lnk_View3" runat="server">Change Max Limit</asp:LinkButton>
    </li>

    </ul>        

<div>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div>
                                
                <div>
                    <br />
                    <fieldset>
                        <label></label>

                        <div>

                            <div style="float:left;width:50%">
                                <uc1:DateField ID="dfNext" runat="server"  />&nbsp;&nbsp; 
                                <br />
                                <div id="toast"></div>
                                <br />
                                <asp:DropDownList runat="server" ID="DropDownList5" AutoPostBack="False" ></asp:DropDownList>
                                <asp:Button runat="server" ID="b_refresh_1" Text="Refresh" Width="90" Height="40" Font-Bold="true" Font-Size="Large" /> <i>* Make sure View past records is not checked.</i>
                                <br /><br />
                                <asp:RadioButtonList runat="server" ID="tour_locations_radiobuttonlist" 
                                    RepeatColumns="5" TextAlign="Right"  RepeatDirection="Horizontal"></asp:RadioButtonList>
                            </div>


                            <div style="float:right;width:45%; margin-right:40px;">
                                <div style="height:20px;position:relative;"><div class="legend-box" style="background-color:#FFE4B5;position:absolute;"></div><div style="position:absolute;margin-left:25px;padding-top:0;">30 Days</div></div>
                                <div style="height:20px;position:relative;"><div class="legend-box" style="background-color:#CD853F;position:absolute;"></div><div style="position:absolute;margin-left:25px;padding-top:0;">60 Days</div></div>
                                <div style="height:20px;position:relative;"><div class="legend-box" style="background-color:#DC143C;position:absolute;"></div><div style="position:absolute;margin-left:25px;padding-top:0;">90 Days</div></div>
                              
                                    <asp:DropDownList ID="DropDownList4" runat="server"></asp:DropDownList>
                                    <asp:CheckBox runat="server" ID="cb_historial" Text="View past records" />
                                &nbsp;&nbsp;    
                                <asp:Button runat="server" ID="b_refresh" Text="View" Width="90" Height="40" Font-Bold="true" Font-Size="Large"  />                                                                                        
                            </div>
                                        
                        
                        </div>
                    </fieldset>
                </div>                                                   
                
                <br /><br />

                <asp:GridView ID="GridView1" runat="server" GridLines="Both" DataKeyNames="TourWaveLimitsID" EmptyDataText="" AutoGenerateColumns="false" >
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TourWaveLimitsID") %>' CommandName="EditCurrentRow"  Text="Edit"></asp:LinkButton>
                            </ItemTemplate>                           
                        </asp:TemplateField>

                        <asp:BoundField DataField="campaign_type_name" HeaderText="Campaign Type" ItemStyle-HorizontalAlign="Right"  ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="Small" />
                        <asp:BoundField DataField="tour_wave" HeaderText="Wave" ItemStyle-HorizontalAlign="Right"  ItemStyle-Width="100" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="Small" />
                        <asp:BoundField DataField="c" HeaderText="Current Counts" ItemStyle-HorizontalAlign="Right"  ItemStyle-Width="100" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="Small" />
                        <asp:BoundField DataField="MaxLimit" HeaderText="Max Limit" ItemStyle-HorizontalAlign="Right"  ItemStyle-Width="100" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="Small" />
                        <asp:BoundField DataField="StartDate" HeaderText="Date Started" ItemStyle-HorizontalAlign="Right"  ItemStyle-Width="100" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="Small" />
                        <asp:BoundField DataField="EndDate" HeaderText="Date Ended" ItemStyle-HorizontalAlign="Right"  ItemStyle-Width="100" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="Small"/>
                        <asp:BoundField DataField="Location" HeaderText="Tour Location" ItemStyle-HorizontalAlign="Right"  ItemStyle-Width="100" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="Small"/>
                                               
                    </Columns>
                </asp:GridView>
               
            </div>
        </asp:View>

        <asp:View ID="View2" runat="server">
            
            <div>                                
                
                <br />
                <h2><strong>Tour Limits</strong></h2>
                <asp:DropDownList ID="DropDownList3" runat="server"></asp:DropDownList>
                <br /><br />                                
                
                
                <div style="position:relative;">
                    <div style="width:49%;position:absolute;">
                            
                        <fieldset>
                            <legend>Wave & Location:</legend>
                        <table>
                            <tr>
                                <td valign="top" style="width:175px;">
                                    <div>
                                        <h3>Locations</h3>
                                        <asp:CheckBoxList runat="server" ID="tour_locations_checkboxlist" TextAlign="Right" RepeatDirection="Vertical" CssClass="locations"></asp:CheckBoxList>                             
                                    </div>
                                </td>
                                <td valign="top" style="width:175px;">
                                    <div>
                                        <h3>Wave</h3>
                                        <asp:CheckBoxList runat="server" ID="tour_wave_checkboxlist" TextAlign="Right" RepeatDirection="Vertical" CssClass="waves"></asp:CheckBoxList>
                                    </div>
                                </td>
                                <td valign="top" style="width:175px;">
                                    <div>
                                        <h3>Campaign Type</h3>
                                        <asp:CheckBoxList runat="server" ID="campaign_types_checkboxlist" TextAlign="Right" RepeatDirection="Vertical" CssClass="campaigns"></asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                            
                        </fieldset>


                    </div>
                        
                    <div style="width:49%;float:right;position:absolute;margin-left:50%;">
                        
                        <fieldset>
                            <legend>
                                Include:
                            </legend>

                            <asp:CheckBoxList RepeatDirection="Horizontal" runat="server" ID="cbl_wd" CssClass="weekdays"></asp:CheckBoxList>                    
                            <br />                        
                            <span><strong>Start Date</strong></span><br />
                            <uc1:DateField ID="dfTransDate" runat="server"  />                                             

                            <span><strong>End Date</strong></span><br />                          
                            <uc1:DateField ID="dfEndDate" runat="server"  />      
                        
                            <br /><br />
                            <asp:Button ID="b_save" runat="server" Text="Save" Width="90" Height="33" Font-Bold="true" Font-Size="Large" /> &nbsp;&nbsp;                               
                            <asp:Button ID="b_cancel" runat="server" Text="Cancel" Width="90" Height="33" Font-Bold="true" Font-Size="Large" /> 
                            <br /><br />

                            <div id="validation_summary" style="font-style:italic;"></div>
                            <br /><br />
                        </fieldset>                            
                                   
                    </div>

                    &nbsp;<br />
                </div>
                
                
               
            </div>            
        </asp:View>



        <asp:View runat="server" ID="View3">
        
            <div> 
                <p><strong>tour location</strong></p>               
                <asp:RadioButtonList runat="server" ID="rbl_location_tours" RepeatDirection="Horizontal" RepeatColumns="5"></asp:RadioButtonList>
                <p><strong>campaign type</strong></p>
                <asp:RadioButtonList runat="server" ID="rbl_campaign_types" RepeatDirection="Horizontal"></asp:RadioButtonList>
                <p><strong>wave</strong></p>
                <asp:DropDownList runat="server" ID="ddl_wave_tours"></asp:DropDownList>
                <p></p>

                <div>
                    <div style="display:inline-block;float:left;margin:20px 0px;width:280px;">
                        <table>
                            <tr>
                                <td><strong>start date</strong></td>
                            </tr>
                            <tr>
                                <td><uc1:DateField ID="date_begin" runat="server"  /></td>
                            </tr>
                            <tr>
                                <td><strong>end date</strong></td>
                            </tr>
                            <tr>
                                <td><uc1:DateField ID="date_end" runat="server"  /></td>
                            </tr>
                            <tr><td></td></tr>
                            <tr><td><asp:Button runat="server" Text="search" ID="btn_submit" /></td></tr>
                    
                            <tr>
                                <td><br /><asp:TextBox runat="server" ID="txb_max_count" Width="35"></asp:TextBox><label><strong>&nbsp;max counts</strong></label></td>
                            </tr>
                            <tr><td></td></tr>
                            <tr><td><asp:Button runat="server" Text="save" ID="btn_save" /></td></tr>
                        </table>
                    </div>
                    <div style="display:inline-block;float:left;margin:20px 0px;width:280px;">                        
                        <asp:CheckBoxList runat="server" ID="cbl_weekdays">                                                        
                        </asp:CheckBoxList>                        
                    </div>
                </div>
                
                
                
                <div style="clear:left;">
                    <asp:GridView runat="server" ID="gv_change_multiple"></asp:GridView>
                </div>
                
            </div>
        </asp:View>

    </asp:MultiView>
</div>


</div>


</asp:Content>

