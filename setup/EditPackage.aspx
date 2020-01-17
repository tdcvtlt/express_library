<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditPackage.aspx.vb" 
Inherits="setup_EditPackage" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

 <style type="text/css">
 
 .money {}
 
     .style1
     {
         height: 26px;
     }
 
 </style>



 <script type="text/javascript">

     function refreshPersonnel(url) {

         __doPostBack('ctl00$ContentPlaceHolder1$LinkPackagePersonnel', '');
     }
     function refreshReservation() {
         __doPostBack('ctl00$ContentPlaceHolder1$LinkPackageReservation', '');
     }

     function refreshFinancial() {
         __doPostBack('ctl00$ContentPlaceHolder1$LinkPackageFinancial', '');
     }

     function refreshWebSources() {
         __doPostBack('ctl00$ContentPlaceHolder1$LinkPackageWebSources', '');
     }

     function refreshLetters() {
         __doPostBack('ctl00$ContentPlaceHolder1$LinkPackageLetters', '');
     }

     function refreshAddCosts() {
         __doPostBack('ctl00$ContentPlaceHolder1$LinkPackageAddCost', '');
     }

     function refreshVendors() {
         __doPostBack('ctl00$ContentPlaceHolder1$LinkPackageVendors', '');
     }
 </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<asp:HiddenField ID="HiddenFieldPackagePK" runat="server" />
<asp:HiddenField ID="HiddenFieldPackageName" runat="server" />
<asp:HiddenField ID="HiddenFieldPackageID" runat="server" />
<asp:HiddenField ID="HiddenFieldPackagePersonnelID" runat="server" />



<ul id="menu">

    <li <% if MultiViewPackage.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonPackage" runat="server">Package</asp:LinkButton>
    </li>    
    
    <li <% if MultiViewPackage.ActiveViewIndex = 1 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkPackageReservation" runat="server">Reservation</asp:LinkButton>        
    </li>    
    <li <% if MultiViewPackage.ActiveViewIndex = 2 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkPackageFinancial" runat="server">Financial</asp:LinkButton>
    </li>
    <li <% if MultiViewPackage.ActiveViewIndex = 3 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkPackagePersonnel" runat="server">Personnel</asp:LinkButton>
    </li>
</ul>
<ul id = "menu">    
    <li <% if MultiViewPackage.ActiveViewIndex = 4 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkPackageWebSources" runat="server">Web Sources</asp:LinkButton>
    </li>

    <li <% if MultiViewPackage.ActiveViewIndex = 5 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkPackageLetters" runat="server">Conf. Letters</asp:LinkButton>
    </li>
    <li <% if MultiViewPackage.ActiveViewIndex = 6 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkPackageAddCost" runat="server">Add. Costs</asp:LinkButton>
    </li>
    <li <% if MultiViewPackage.ActiveViewIndex = 7 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkVendors" runat="server">Vendors</asp:LinkButton>
    </li>
</ul>
<h2></h2>



<div id="dv_multiview_package_top">
<asp:MultiView ID="MultiViewPackage" runat="server">

    <asp:View ID="MultiViewPackagePackage" runat="server">
<div id="dv_package_detail" style="width:1050px">
    
    <asp:Label runat="server" Text="Package" ></asp:Label><br />
    <asp:TextBox runat="server" ID="package_copy_txt" Width="250" ></asp:TextBox>
    <asp:Button runat="server" ID="package_copy_btn" Text="save package as" />    
    <br />
   

    <fieldset>
        <legend><asp:Label runat="server" ID="LabelPackage" Font-Bold="true"></asp:Label></legend>
        <br />
        <div id="dv_package_detail_wrapper">
            
            <div></div>
            <div>
                <table>
                    <tr>
                        <td>Name</td>
                        <td><asp:TextBox ID="TextBoxPackage" runat="server"></asp:TextBox></td>
                        <td>Description</td>
                        <td><asp:TextBox ID="TextBoxDescription" runat="server" Width="200px"></asp:TextBox></td>
                        <td><asp:CheckBox ID="CheckBoxActive" runat="server" Text="Active" /></td>
                    </tr>
                    <tr>
                        <td>Type:</td>
                        <td><uc2:Select_Item ID="siType" runat="server" /></td>
                        <td>Sub-Type:</td>
                        <td><uc2:Select_Item ID="siSubType" runat="server" /></td>
                    </tr>
                </table>
            </div>
            <br /><br />
            <div id="dv_package_detail_wrapper_leftbar" style="width:410px;float:left;">
            
                
                
                <table></table>

                <fieldset>
                    <legend><asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Vendor Packages"></asp:Label></legend>
                    <table>
                        <tr>
                            <td class="style1">Duration (Days)</td>
                            <td colspan="4" class="style1"><asp:TextBox ID="TextBoxExpirationPeriod" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Optional Location</td>
                            <td colspan="4"><asp:CheckBox ID="CheckBoxOptionalLocation" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Default Cost</td>
                            <td colspan="2"><asp:TextBox runat="server" ID="TextBoxDefaultCost" CssClass="money"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Cost </td>
                            <td><asp:TextBox runat="server" ID="TextBoxCost" CssClass="money" ></asp:TextBox></td>
                            <td><asp:CheckBox ID="CheckBoxPromptCost" runat="server" Text="Prompt Cost" /></td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Minimum Charge</td>
                            <td colspan="2"><asp:TextBox runat="server" ID="TextBoxMininumCharge" CssClass="money"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Maximum Charge</td>
                            <td colspan="2"><asp:TextBox runat="server" ID="TextBoxMaximumCharge" CssClass="money"></asp:TextBox></td>
                        </tr>

                    </table>
                    <br />
                    
                </fieldset>

                <br />
                <br />
            </div>
            <div id="dv_package_detail_wrapper_rightbar" 
                style="width:431px; margin-left:40px;">
                <fieldset>
                    <legend><asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Online Marketing Packages"></asp:Label></legend>
                    <table style="">

                <tr>
                    <td>Accommodation</td>
                    <td><asp:DropDownList runat="server" ID="DropDownListAccommodation"></asp:DropDownList></td>

                </tr>
                <tr>
                    <td>Promo Start Date</td>
                    <td>
                        <uc1:DateField ID="dteStartDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Promo End Date</td>
                    <td>
                        <uc1:DateField ID="dteEndDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Min Nights:</td>
                    <td><asp:DropDownList runat="server" ID="DropDownListNights"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Default Invoice Amount:</td>
                    <td>
                        <asp:TextBox ID="txtInvAmt" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Max Premium Amount:</td>
                    <td><asp:TextBox ID="txtMaxPremAmt" runat="server"></asp:TextBox></td>
                </tr>
                <!--<tr>
                    <td>Resort Fee Amount:</td>
                    <td><asp:TextBox ID="txtResortFeeAmt" runat="server"></asp:TextBox></td>
                </tr>-->
                <tr>
                    <td colspan = "2">---Resort Packages Only---</td>
                </tr>
                <tr>
                    <td>Unit Type</td>
                    <td>
                        <uc2:Select_Item ID="siUnitType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>BedRooms</td>
                    <td>
                        <asp:DropDownList ID="DropDownListBedRooms" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="1">1BD</asp:ListItem>
                            <asp:ListItem>1BD-DWN</asp:ListItem>
                            <asp:ListItem>1BD-UP</asp:ListItem>
                            <asp:ListItem Value="2">2BD</asp:ListItem>
                            <asp:ListItem Value="3">3BD</asp:ListItem>
                            <asp:ListItem Value="4">4BD</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan = "2">---Hotel Packages Only---</td>
                </tr>
                <tr>
                    <td>Room Size</td>
                    <td>
                        <uc2:Select_Item ID="siRoomType" runat="server" />
                    </td>
                </tr>
            </table>
            </fieldset>
            </div>
        </div>
        
    </fieldset>

    <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" />

    <br />
    <br />
    <asp:HyperLink runat="server" Text="Packages List" NavigateUrl="~/setup/PackageLookup.aspx"></asp:HyperLink>
</div>
        
    </asp:View>
    
    <asp:View ID="MultiViewPackageReservation" runat="server">
        
        <div id="dv_view_packagereservaton">
            <asp:GridView ID="GridViewPackageReservation" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal" 
                AutoGenerateColumns="true" AutoGenerateSelectButton="true" BorderStyle="None">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
            </asp:GridView>            
        </div>


           <br />

            <ul id="menu">
                <li>
                    <asp:LinkButton runat="server" ID="LinkButtonPackageReservationAdd">Add</asp:LinkButton>                    
                </li>
            </ul>  
        
    </asp:View>
    
    <asp:View ID="MultiViewPackageFinancial" runat="server">

        <h2>Financial</h2>  
        <div id="dv_view_packagefinancial">
            <asp:GridView ID="GridViewPackageFinancial" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal" 
                AutoGenerateColumns="False" AutoGenerateSelectButton="False" BorderStyle="None">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                <Columns>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="150" HeaderStyle-BackColor="AliceBlue" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left">
                        
<%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                        <ItemTemplate>
                            
<%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; --%>
                            <a href='EditPackageFinTransCode.aspx?PkgFinTransID=<%# container.DataItem("PackageFinTransCodeID") %>'>Edit</a>
<%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                        </ItemTemplate>
                        
<%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Transaction Name" HeaderText="Name" ItemStyle-HorizontalAlign="Left"  ItemStyle-Width="200" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="X-Large"/>
                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-HorizontalAlign="Left"  ItemStyle-Width="250" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="X-Large" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-HorizontalAlign="Right"  ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="X-Large"/>

                </Columns>
            </asp:GridView>            
        </div>   
        
        
           <br />

            <ul id="menu">
                <li>
                    <asp:LinkButton runat="server" ID="LinkButtonPackageFinancialCreate" Text="Create"></asp:LinkButton>                    
                </li>
            </ul>  
                                       
    </asp:View>
    
    
    
    <asp:View ID="MultiViewPackagePersonnel" runat="server">                
       
        <div id="dv_view_packagepersonnel">
            
            <br /><br />

            <asp:GridView ID="GridViewPackagePersonnel" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal" AutoGenerateColumns="false" 
                AutoGenerateSelectButton="false" BorderStyle="None">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                <Columns>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Height="40" HeaderStyle-Font-Size="X-Large">
                        
<%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                        <ItemTemplate>
                            
<%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; --%>
                            <a href="javascript:modal.mwindow.open('<%= request.applicationpath %>/setup/packagepersonnel.aspx?PackageID=<%# Container.DataItem("PackageID")%>&PackagePersonnelID=<%# Container.DataItem("PackagePersonnelID")%>&PersonnelID=<%# Container.DataItem("PersonnelID")%>&Path=<%# Request.UrlReferrer.PathAndQuery %>&Type=PackagePersonnel', '587419', 450, 300)">Edit</a>
<%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                        </ItemTemplate>
                        
<%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                    </asp:TemplateField>

                    <asp:BoundField headerText="Title" SortExpression="Personnel" DataField="Title" ItemStyle-Width="250" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="X-Large"></asp:BoundField>     
                    <asp:BoundField HeaderText="Personnel" DataField="Personnel" ItemStyle-Width="250" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left"  HeaderStyle-Font-Size="X-Large"/>
                </Columns>
            </asp:GridView>            
        
        
            <br />
            <br />

            <ul id="menu">
                <li>
                    <a href="javascript:modal.mwindow.open('<%=request.applicationpath %>/setup/PackagePersonnel.aspx?PackageID=<%= Me.PackageID %>&PackagePersonnelID=<%=0%>&Type=PackagePersonnel','win01',450, 300);">Assign</a>
                </li>
            </ul>  
        
        </div>    
    </asp:View>
    <asp:View ID="MultiViewPackageWebSource" runat="server">
        <asp:GridView ID="gvWebSource" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal" 
                AutoGenerateColumns="true" AutoGenerateSelectButton="true" BorderStyle="None">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
        </asp:GridView>
        <br />

            <ul id="menu">
                <li>
                    <a href="javascript:modal.mwindow.open('<%=request.applicationpath %>/setup/EditWebSource.aspx?PackageID=<%= Me.PackageID %>&Package2WebSourceID=<%=0%>','win01',450, 300);">Add Web Source</a>
                </li>
            </ul>  
    </asp:View>

    <asp:View ID="MultiViewPackageLetters" runat="server">
    <asp:GridView ID="gvLetters" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal" 
                AutoGenerateColumns="true" AutoGenerateSelectButton="true" BorderStyle="None">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
        </asp:GridView>
        <br />

            <ul id="menu">
                <li>
                    <a href="javascript:modal.mwindow.open('<%=request.applicationpath %>/setup/EditPkg2Letter.aspx?PackageID=<%= Me.PackageID %>&ID=<%=0%>','win01',450, 300);">Add Letter</a>
                </li>
            </ul>  
    </asp:View>


    <asp:View ID="MultiViewPackageAddCosts" runat="server">
    <asp:GridView ID="gvAddCost" runat="server" EmptyDataText="No Records" GridLines="Horizontal" 
                AutoGenerateColumns="true" AutoGenerateSelectButton="true" BorderStyle="None" onrowdatabound = "gvAddCost_RowDataBound">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
        </asp:GridView>
        <br />

            <ul id="menu">
                <li>
                    <a href="javascript:modal.mwindow.open('<%=request.applicationpath %>/setup/EditPackageAddCost.aspx?PackageID=<%= Me.PackageID %>&ID=<%=0%>','win01',450, 300);">Add Additional Cost</a>
                </li>
            </ul>  
    </asp:View>
    <asp:View ID="MultiViewPackageVendors" runat="server">
    <asp:GridView ID="gvVendors" runat="server" EmptyDataText="No Records" GridLines="Horizontal" 
                AutoGenerateColumns="true" AutoGenerateSelectButton="true" BorderStyle="None">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
        </asp:GridView>
        <br />

            <ul id="menu">
                <li>
                    <a href="javascript:modal.mwindow.open('<%=request.applicationpath %>/setup/EditPackage2Vendor.aspx?PackageID=<%= Me.PackageID %>&ID=<%=0%>','win01',450, 300);">Add Vendor</a>
                </li>
            </ul>  
    </asp:View>
</asp:MultiView>
</div>




<script type="text/javascript">

    $(function () {

        $("#dv_multiview_package_top td:first-child").css("font-weight", "bold");

        $("#dv_package_detail table").css("border-collapse", "collapse");


        $("#dv_multiview_package_top input[type='text']").css({ 'font': '16px Georgia',
            'letter-spacing': '-1px'
        });

        $("#dv_multiview_package_top .money").css({"text-align": "right", "color":"red"});
    });
</script>

                    
        

</asp:Content>

