<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PackageFinancialCode.aspx.vb" Inherits="setup_PackageFinancial" ValidateRequest="false"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script type="text/javascript">

   function refreshPayment() {

       __doPostBack('ctl00$ContentPlaceHolder1$LinkButtonPayment', '');
     }

</script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:HiddenField ID="HiddenFieldPackageFinTransCodeID" runat="server" />
<asp:HiddenField ID="HiddenFieldPackageReservationFinTransCodeID" runat="server" />
<asp:HiddenField ID="HiddenFieldPackageTourFinTransCodeID" runat="server" />

<asp:HiddenField ID="HiddenFieldPackageID" runat="server" />
<asp:HiddenField ID="HiddenFieldPackageTourID" runat="server" />
<asp:HiddenField ID="HiddenFieldPackageReservationID" runat="server" />

<asp:HiddenField ID="HiddenFieldType" runat="server" />
<asp:HiddenField ID="HiddenFieldBack" runat="server" />

<asp:HiddenField ID="HiddenFieldPaymentType" runat="server" />


<div id="wrapper" style="width:500px;">


<ul id="menu">
    <li <% if MultiViewPackageFinancialCode.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonFinancial" runat="server">Financial</asp:LinkButton>
    </li>   
    <li <% if MultiViewPackageFinancialCode.ActiveViewIndex = 1 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonPayment" runat="server">Payment</asp:LinkButton>
    </li>     
</ul>





<div>

<asp:MultiView ID="MultiViewPackageFinancialCode" runat="server">
    <asp:View ID="ViewFinancialCode" runat="server">
        
        <br />

        <fieldset style="width:70%;">
            
            <legend>Financial Code</legend>
        
            <table style="border-collapse:collapse;">
            
            
                <tr>
                    <td>Transaction</td>
                    <td><asp:DropDownList ID="DropDownListFinTransCodeID" runat="server"></asp:DropDownList></td>
                </tr>
                <tr><td colspan="2">&nbsp;</td></tr>
                <tr>
                    <td></td>
                    <td><asp:CheckBox ID="CheckBoxUseFormula" runat="server" Text="Use Formula" /></td>
                </tr>
                <tr>
                    <td>Formula</td>
                    <td><asp:TextBox ID="TextBoxFormula" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Amount</td>
                    <td><asp:TextBox ID="TextBoxAmount" runat="server"></asp:TextBox></td>
                </tr>
            
            </table>  
            

            <br />
            <div style="position:relative;width:250px;">
                <div style="position:absolute;right:0px;">
                    <asp:Button ID="ButtonSave" runat="server" Text="Submit" Width="125px" />              
                </div>
                
            </div>
            
            <br style="clear:both;" />

            <h2></h2>
        </fieldset>
        
    </asp:View>

    <asp:View ID="ViewPayment" runat="server">

        <div id="dv_payment">
            <br />
            <br /><br />
            <br />
            <asp:GridView ID="GridViewPayment" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal" 
                AutoGenerateColumns="false" 
                AutoGenerateSelectButton="false" BorderStyle="None">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                <Columns>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Height="40" HeaderStyle-Font-Size="X-Large">
                        <ItemTemplate>
                            <a href="javascript:modal.mwindow.open('<%= request.applicationpath %>/setup/PackagePayment.aspx?PackageID=<%# container.DataItem("PackageID")%>&PackagePaymentID=<%# container.DataItem("PackagePaymentID")%>&Type=PackagePayment', '58496321', 450, 350)">Edit</a>                            
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField headerText="Method" DataField="Method" ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="X-Large"></asp:BoundField>     
                    <asp:BoundField HeaderText="Amount" DataField="Amount" ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Right"  ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="X-Large"/>

                </Columns>
            </asp:GridView>            
        
        
            <br />
            <br />
            <h2>I am in package</h2>
            <ul id="menu">
                <li>
                    <a href="javascript:modal.mwindow.open('<%=request.applicationpath %>/setup/PackagePayment.aspx?PackageID=<%= Me.PackageID %>&PackagePaymentID=<%=0%>&Type=PackagePayment&Path=<%# Request.UrlReferrer.PathAndQuery%>','win01',450, 350);">Create</a>
                </li>
            </ul>          
        </div>


        
    </asp:View>
    <asp:View ID="ViewTourPayment" runat="server">
            <div id="dv_ViewTourPayment">
            <br />
            <br /><br />
            <br />
            <asp:GridView ID="GridViewViewTourPayment" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal"
                AutoGenerateColumns="false" 
                AutoGenerateSelectButton="false" BorderStyle="None">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                <Columns>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Height="40" HeaderStyle-Font-Size="X-Large">
                        <ItemTemplate>
                            <a href="javascript:modal.mwindow.open('<%= request.applicationpath %>/setup/PackagePayment.aspx?PackageID=<%# container.DataItem("PackageID")%>&PackagePaymentID=<%# container.DataItem("PackageTourPaymentID")%>&Type=PackageTourPayment', '500094711', 450, 350)">Edit</a>                            
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField headerText="Method" DataField="Method" ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="X-Large"></asp:BoundField>     
                    <asp:BoundField HeaderText="Amount" DataField="Amount" ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Right"  ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="X-Large"/>
                </Columns>
            </asp:GridView>                           
            <br />
            <br />
            <h2>I am in Tour</h2>
            <ul id="menu">
                <li>
                    <a href="javascript:modal.mwindow.open('<%=request.applicationpath %>/setup/PackagePayment.aspx?PackageID=<%= Me.PackageID %>&PackageTourFinTransCodeID=<%= Me.PackageTourFinTransCodeID %>&PackagePaymentID=<%=0%>&Type=PackageTourPayment&Path=<%# Request.UrlReferrer.PathAndQuery%>','win01',450, 350);">Create</a>
                </li>
            </ul>          
            </div>
    </asp:View>


    <asp:View ID="ViewReservationPayment" runat="server">
            <div id="dv_ViewReservationPayment">
            <br />
            <br /><br />
            <br />
            <asp:GridView ID="GridViewViewReservationPayment" runat="server" EmptyDataText="Data Empty" GridLines="Horizontal"
                AutoGenerateColumns="false" 
                AutoGenerateSelectButton="false" BorderStyle="None">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                <Columns>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Height="40" HeaderStyle-Font-Size="X-Large">
                        <ItemTemplate>
                            <a href="javascript:modal.mwindow.open('<%= request.applicationpath %>/setup/PackagePayment.aspx?PackageID=<%# container.DataItem("PackageID")%>&PackageReservationPaymentID=<%# container.DataItem("PackageReservationPaymentID")%>&Type=PackageReservationPayment', '58494711', 450, 350)">Edit</a>                            
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField headerText="Method" DataField="Method" ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="X-Large"></asp:BoundField>     
                    <asp:BoundField HeaderText="Amount" DataField="Amount" ItemStyle-Width="150" HeaderStyle-BackColor="DarkSeaGreen" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Right"  ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="X-Large"/>
                </Columns>
            </asp:GridView>                           
            <br />
            <br />
            <h2>I am in Reservation</h2>
            <ul id="menu">
                <li>
                    <a href="javascript:modal.mwindow.open('<%=request.applicationpath %>/setup/PackagePayment.aspx?PackageID=<%= Me.PackageID %>&PackageReservationFinTransCodeID=<%= Me.PackageReservationFinTransCodeID %>&PackagePaymentID=<%=0%>&Type=PackageReservationPayment&Path=<%# Request.UrlReferrer.PathAndQuery%>','win01',450, 350);">Create</a>
                </li>
            </ul>          
            </div>
    </asp:View>

</asp:MultiView>
</div>

<br /><br />
<div style="bottom:0">
<asp:HyperLink ID="HyperLinkParentLink" runat="server"></asp:HyperLink>
</div>

</div>



<script type="text/javascript">

    $("#wrapper td:first-child").css({ 'width': '175px', 'font-size': '25px', 'font-weight': 'bold' });
    $("#wrapper select").css({ 'font-size': '18px' });
    $("#wrapper input[type=text]").css({ 'font-size': '18px', 'text-align':'right' });

</script>
</asp:Content>

