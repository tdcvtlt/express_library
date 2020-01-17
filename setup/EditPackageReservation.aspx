<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" 
CodeFile="EditPackageReservation.aspx.vb" Inherits="setup_EditPackageReservation" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript">

    function refreshPersonnel() {

        __doPostBack('ctl00$ContentPlaceHolder1$LinkButtonPersonnel', '');
    }
    function refreshFinancial() {
        __doPostBack('ctl00$ContentPlaceHolder1$LinkButtonFinancial', '');
    }

    function refreshTour() {
        __doPostBack('ctl00$ContentPlaceHolder1$LinkButtonTour', '');
    }

    function refreshCheckInDays() {
        __doPostBack('ctl00$ContentPlaceHolder1$LinkButtonCheckInDays', '');
    }
 </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">



<asp:HiddenField runat="server" ID="HiddenFieldPackageReservationID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="HiddenFieldPackageReservationFinTransCodeID" />
<asp:HiddenField runat="server" ID="HiddenFieldPackageReservationPersonnelID" />
<asp:HiddenField runat="server" ID="HiddenFieldPackageTourID" />

<asp:HiddenField runat="server" ID="HiddenFieldPackageID" />
<asp:HiddenField runat="server" ID="HiddenFieldPersonnelID" />
<asp:HiddenField runat="server" ID="HiddenFieldFinTransID" />
<asp:HiddenField runat="server" ID="HiddenFieldReservationID" />
<asp:HiddenField runat="server" ID="HiddenFieldPackageName" />
<asp:HiddenField runat="server" ID="HiddenFieldReservationName" />



<div id="content_wrapper">

<asp:HyperLink runat="server" ID="HyperLinkPackageID" ForeColor="Red" Font-Bold="true" ></asp:HyperLink>



<br />
<ul id="menu">

   

    <li <% if MultiViewPackageReservation.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonReservation" runat="server">Reservation</asp:LinkButton>
    </li>    
    
    <li <% if MultiViewPackageReservation.ActiveViewIndex = 1 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonFinancial" runat="server">Financial</asp:LinkButton>
    </li>    
    
    <li <% if MultiViewPackageReservation.ActiveViewIndex = 2 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonPersonnel" runat="server">Personnel</asp:LinkButton>
    </li>    
    <li <% if MultiViewPackageReservation.ActiveViewIndex = 3 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonTour" runat="server">Tour</asp:LinkButton>
    </li>    
    <li <% If MultiViewPackageReservation.ActiveViewIndex = 4 Then : Response.Write("class=""current""") : End If%>>
        <asp:LinkButton ID="LinkButtonCheckInDays" runat="server">Extra Night Check-In Days</asp:LinkButton>
    </li>         
</ul>


<div id="dv_multiview_package_top">
    <asp:MultiView ID="MultiViewPackageReservation" runat="server">
        <asp:View ID="ViewReservation" runat="server">
        
        <br /><br />
        <div>

            <table>
                <tr>
                    <td>Package #</td>
                    <td></td>
                    <td><asp:Label ID="LabelPackageName" runat="server"></asp:Label></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;</td>
                </tr>
                <tr>
                    <td>Location</td>
                    <td>&nbsp;</td>
                    <td colspan="2"><asp:DropDownList ID="DropDownListLocation" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Status</td>
                    <td>&nbsp;</td>
                    <td colspan="2"><asp:DropDownList ID="DropDownListStatus" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>                
                    <td colspan="2">Promo Nights</td>
                    <td colspan="2"><asp:DropDownList ID="DropDownListPromoNights" runat="server"></asp:DropDownList></td>                
                </tr>
                <tr>
                    <td colspan="2">Promo Rate</td>
                    <td colspan="2"><asp:TextBox ID="TextBoxPromoRate" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2">Source:</td>
                    <td colspan="2">
                        <uc2:Select_Item ID="siSourceID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Resort Company:</td>
                    <td colspan="2">
                        <uc2:Select_Item ID="siCompany" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                    <td colspan="2"><asp:CheckBox id="CheckBoxTourRequired" runat="server" Text="Tour required?" /></td>
                </tr>
                <tr>
                    <td colspan ="2">Type:</td>
                    <td colspan ="2">
                        <uc2:Select_Item ID="siType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan ="2">Allow Extra Night:</td>
                    <td colspan ="2">
                        <asp:CheckBox ID="cbExtraNight" runat="server" />
                    </td>
                </tr>
            </table>

            <br />
            <br />
            
            <div>
                <asp:Button runat="server" ID="ButtonSubmit" Text="Submit" />
            </div>
        </div>
        
        </asp:View>


        <asp:View ID="ViewFinancial" runat="server">
         <div id="Div1">
            
            <br />

            <asp:GridView ID="GridViewReservationFinancial" runat="server" EmptyDataText="Data Empty" GridLines="Both" 
                AutoGenerateColumns="false" AutoGenerateSelectButton="false" DataKeyNames="PackageReservationFinTransCodeID" >
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                <Columns>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="150" HeaderStyle-BackColor="AliceBlue" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <a href="EditPackageResFinTrans.aspx?PkgResFinTransID=<%# container.DataItem("PackageReservationFinTransCodeID") %>">Edit</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Transaction Name" HeaderText="Name" ItemStyle-HorizontalAlign="Left"  ItemStyle-Width="200" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="X-Large"/>
                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-HorizontalAlign="Left"  ItemStyle-Width="250" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="X-Large" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-HorizontalAlign="Right"  ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="X-Large"/>
                    
                    
                </Columns>
            </asp:GridView>        
            
            <br /><br />  
              
            <ul id="menu">
                <li>
                    <asp:LinkButton ID="LinkButtonReservationFinancialCreate" runat="server" Text="Create"></asp:LinkButton>
                </li>        
            </ul>          
        </div>        
        </asp:View>


        <asp:View ID="ViewPersonnel" runat="server">
            
            <br />
            <br />
            <div id="dv_view_viewpersonnel">

                <asp:GridView ID="GridViewReservationPersonnel" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal" AutoGenerateColumns="false" AutoGenerateSelectButton="false">
                    <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                    <AlternatingRowStyle BackColor="#CCFFCC" />
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <a href="javascript:modal.mwindow.open('<%= request.applicationpath %>/setup/packagepersonnel.aspx?PackageID=<%# container.DataItem("PackageID") %>&PackageReservationID=<%= Me.PackageReservationID %>&PersonnelID=<%# container.DataItem("PersonnelID") %>&PackageReservationPersonnelID=<%# container.DataItem("PackageReservationPersonnelID") %>&Type=PackageReservationPersonnel', '587419', 450, 300)">Edit</a>
                            </ItemTemplate>
                        </asp:TemplateField>

                    <asp:BoundField headerText="Title" DataField="Title" ItemStyle-Width="250" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="X-Large" />
                    <asp:BoundField HeaderText="Personnel" DataField="Personnel" ItemStyle-Width="250" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="X-Large" />                                                     

                    </Columns>
                </asp:GridView>   
                
                <br /><br /><br />
                <ul id="menu">
                    <li>
                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath %>/setup/PackagePersonnel.aspx?PackageID=<%= Me.PackageID %>&PackageReservationID=<%= Me.PackageReservationID %>&PackageReservationPersonnelID=<%=0%>&Type=PackageReservationPersonnel','win01',450, 300);">Assign New</a>
                    </li>
                </ul>  
            </div>          
        </asp:View>


          <asp:View ID="ViewTour" runat="server">
            <br />
             <div id="dv_view_viewtour">
                    <asp:GridView ID="GridViewReservationTour" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal" AutoGenerateColumns="true" AutoGenerateSelectButton="true">
                        <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                        <AlternatingRowStyle BackColor="#CCFFCC" />
                    </asp:GridView>            
                </div>     
                
                <br /><br />
                <ul id="menu" style="bottom:0px;">
                    <li>
                        <asp:LinkButton ID="LinkButtonTourAdd" runat="server" Text="Add"></asp:LinkButton>
                    </li>
                </ul>
            </asp:View>
            <asp:View ID="vwCheckInDays" runat="server">
                <div id="dv_view_viewtour">
                    <asp:GridView ID="gvDays" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal" AutoGenerateColumns="true" AutoGenerateDeleteButton="true">
                        <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                        <AlternatingRowStyle BackColor="#CCFFCC" />
                    </asp:GridView>            
                </div>     
                
                <br /><br />
                <ul id="menu" style="bottom:0px;">
                    <li>
                        <asp:LinkButton ID="LinkButton1" runat="server" Text="Add"></asp:LinkButton>
                    </li>
                </ul>
            </asp:View>

    </asp:MultiView>
</div>

</div>



<script type="text/javascript">

    $(function () {


        $("#dv_multiview_package_top td:first-child").css({ 'font': '14px Verdana',
            'letter-spacing': '-1px', 'text-align': 'left',
            'font-weight': 'bold'
        });

        $("#dv_multiview_package_top input[type='text']").css({ 'font': '14px Verdana', 'text-align': 'right' });

        $("#dv_multiview_package_top select").css({ 'font': '14px Verdana', 'color': 'blue' });
        $("#dv_multiview_package_top select:first").css({ 'font': '14px Verdana', 'color': 'red' });
    });
</script>
</asp:Content>

