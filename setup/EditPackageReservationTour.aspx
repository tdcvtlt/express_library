<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditPackageReservationTour.aspx.vb" Inherits="setup_EditPackageReservationTour" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script type="text/javascript">

    function refreshPersonnel() {

        __doPostBack('ctl00$ContentPlaceHolder1$LinkPackageReservationPersonnel', '');
    }

    function refreshPremiums() {
        __doPostBack('ctl00$ContentPlaceHolder1$LinkPackageTourPremiums', '');
    }
    function refreshFinancial() {
        __doPostBack('ctl00$ContentPlaceHolder1$LinkPackageReservationFinancial', '');
    }
 </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<asp:HiddenField ID="HiddenFieldPackageID" runat="server" />
<asp:HiddenField ID="HiddenFieldPackageName" runat="server" />
<asp:HiddenField ID="HiddenFieldPackageTourID" runat="server" />
<asp:HiddenField ID="HiddenFieldPackageReservationID" runat="server" />
<asp:HiddenField ID="HiddenFieldReservationName" runat="server" />
<asp:HiddenField ID="HiddenFieldPackageTourPersonnelID" runat="server" />
                      






<ul id="menu">

    <li <% if MultiViewPackageReservationTour.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonPackageReservationTour" runat="server">Tour</asp:LinkButton>
    </li>    
    
    <li <% if MultiViewPackageReservationTour.ActiveViewIndex = 1 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkPackageReservationFinancial" runat="server">Financial</asp:LinkButton>        
    </li>    
    <li <% if MultiViewPackageReservationTour.ActiveViewIndex = 2 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkPackageReservationPersonnel" runat="server">Personnel</asp:LinkButton>
    </li>
        <li <% if MultiViewPackageReservationTour.ActiveViewIndex = 3 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkPackageTourPremiums" runat="server">Premiums</asp:LinkButton>
    </li>
</ul>



<div id="wrapper">

<h2><asp:HyperLink ID="HyperLinkReservation" runat="server"></asp:HyperLink></h2>

<asp:MultiView ID="MultiViewPackageReservationTour" runat="server">
    <asp:View ID="ViewPackageReservationTour" runat="server">
        
        <fieldset style="width:600px;">
            
            <legend id="legend"><%= Me.Reservation%></legend>
            
            <table style="border-collapse:collapse">
                <tr>
                    <td>Location</td>
                    <td><asp:DropDownList runat="server" ID="DropDownListLocation"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Campaign</td>
                    <td><asp:DropDownList runat="server" ID="DropDownListCampaign"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Type</td>
                    <td><asp:DropDownList runat="server" ID="DropDownListType"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Status</td>
                    <td><asp:DropDownList runat="server" ID="DropDownListStatus"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Source</td>
                    <td><asp:DropDownList runat="server" AppendDataBoundItems="true" ID="DropDownListSource">
                        <asp:ListItem Text="" Value="0"></asp:ListItem>
                    </asp:DropDownList></td>
                </tr>                        
            </table>
            
            <br />
            <br />
            <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" />
        </fieldset>
    </asp:View>

    <asp:View ID="ViewPackageReservationFinancial" runat="server">
        <div>

        <br />
        <br />

     <asp:GridView ID="GridViewPackageTourFinancial" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal" 
                AutoGenerateColumns="false" AutoGenerateSelectButton="false" DataKeyNames="PackageTourFinTransCodeID" BorderStyle="None" >
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                <Columns>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="150" HeaderStyle-BackColor="AliceBlue" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <a href="EditPackageTourFinTrans.aspx?PkgTourFinTransID=<%# container.DataItem("PackageTourFinTransCodeID") %>&Type=TourFinTran&Path=<%= Request.UrlReferrer.PathAndQuery %>">Edit</a>
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
                    <asp:LinkButton ID="LinkButtonTourFinancial" runat="server" Text="Create"></asp:LinkButton>
                </li>
            </ul>
            
        </div>
    </asp:View>

    <asp:View ID="ViewPackageReservationPersonnel" runat="server">
        
  <div id="dv_view_packagepersonnel">
    <br />
    <br />
            <asp:GridView ID="GridViewPackagePersonnel" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal" 
                AutoGenerateColumns="false" AutoGenerateSelectButton="false" DataKeyNames="PackageTourPersonnelID" >
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                <Columns>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <a href="javascript:modal.mwindow.open('<%= request.applicationpath %>/setup/packagepersonnel.aspx?PackageID=<%= Me.PackageID %>&PackageTourID=<%= Me.PackageTourID %>&PackageTourPersonnelID=<%# container.DataItem("PackageTourPersonnelID") %>&PersonnelID=<%# container.DataItem("PersonnelID") %>&Type=PackageTourPersonnel', '587419', 450, 300);">Edit</a>                            
                        </ItemTemplate>                                                
                    </asp:TemplateField>
                    <asp:BoundField headerText="Title" DataField="Title" ItemStyle-Width="250" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="X-Large" />
                    <asp:BoundField HeaderText="Personnel" DataField="Personnel" ItemStyle-Width="250" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="X-Large" />                                                     
                </Columns>
            </asp:GridView>            

            <ul id="menu">
                <li>
                    <a href="javascript:modal.mwindow.open('<%=request.applicationpath %>/setup/PackagePersonnel.aspx?PackageID=<%= Me.PackageID %>&PackageTourID=<%= Me.PackageTourID %>&PersonnelID=<%=0%>&Type=PackageTourPersonnel','win0118',450, 300);">Assign</a>
                </li>
            </ul>  
        
        </div>    

    </asp:View>

        <asp:View ID="GridViewPackageTourPremium" runat="server">
        
  <div id="dv_view_packagetourpremium">
    <br />
    <br />
            <asp:GridView ID="gvPackageTourPremiums" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal" 
                AutoGenerateColumns="true" AutoGenerateDeleteButton="true" AutoGenerateSelectButton="true" DataKeyNames="PackageTourPremiumID" >
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
            </asp:GridView>            
        
        
            <br />
            <br />
            <br />

            <ul id="menu">
                <li>
                    <a href="javascript:modal.mwindow.open('<%=request.applicationpath %>/setup/EditPackageTourPremium.aspx?PkgTourPremiumID=0&PackageID=<%=Me.PackageID %>&PackageTourID=<%= Me.PackageTourID %>','win0118',450, 300);">Add Premium</a>
                </li>
            </ul>  
        
        </div>    

    </asp:View>
</asp:MultiView>

</div>




<script type="text/javascript">

    $(function () {

        $("#wrapper select").css({ width: '220px' });
        $("#wrapper tr").css({ height: '35px' });
        $("#wrapper td").css({ 'font-weight': 'bold', 'font-size': '17px' });                

    });
</script>
</asp:Content>

