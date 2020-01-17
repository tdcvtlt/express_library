<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PointsTracking.ascx.vb" Inherits="controls_PointsTracking" %>
<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc1" TagName="DateField" %>

<script type="text/javascript" language="javascript">
function switchViews(obj,row) 
    { 
        var div = document.getElementById(obj); 
        var img = document.getElementById('img' + obj); 
         
        if (div.style.display=="none") 
            {
                div.style.display = "inline"; 
                if (row=='alt') 
                    {
                        img.src = "<%=request.applicationpath %>/images/expand_button2.png" //"Images/expand_button_white_alt_down.jpg";
                        mce_src = "<%=request.applicationpath %>/images/expand_button2.png" //"Images/expand_button_white_alt_down.jpg"; 
                    } 
                else 
                    {
                        img.src = "<%=request.applicationpath %>/images/expand_button2.png" //"Images/Expand_Button_white_Down.jpg";
                        mce_src = "<%=request.applicationpath %>/images/expand_button2.png" //"Images/Expand_Button_white_Down.jpg"; 
                    } 
                img.alt = "Close to view other customers"; 
            } 
        else 
            { 
                div.style.display = "none"; 
                if (row=='alt') 
                    {
                        img.src = "<%=request.applicationpath %>/images/expand_button.png" //"Images/Expand_button_white_alt.jpg";
                        mce_src = "<%=request.applicationpath %>/images/expand_button.png" //"Images/Expand_button_white_alt.jpg"; 
                    } 
                else 
                    {
                        img.src = "<%=request.applicationpath %>/images/expand_button.png" //"Images/Expand_button_white.jpg";
                        mce_src = "<%=request.applicationpath %>/images/expand_button.png" //"Images/Expand_button_white.jpg"; 
                    } 
                img.alt = "Expand to show orders"; 
            } 
    }
    </script>
<asp:MultiView ID="MultiView1" runat="server">
    <asp:View ID="vwYears" runat="server">
    
        <div>Contract: 
            <asp:DropDownList ID="ddContracts" runat="server" AutoPostBack="True">
                <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
            </asp:DropDownList>
            Year:
            <asp:DropDownList ID="ddYears" runat="server" AutoPostBack="True">
                <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddGroup" runat="server">
                <asp:ListItem Selected="True" Value="AY">Availability Year</asp:ListItem>
                <asp:ListItem Selected="false" Value="UY">Usage Year</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnFilter" Text="Apply Filter" runat="server" />
        </div>
        <div class="ListGrid">
        <asp:GridView ID="gvYears" runat="server"  
                AutoGenerateColumns="False" DataKeyNames="ID" 
                
                AllowPaging="false" PageSize="20" Width="100%" > 
                <HeaderStyle CssClass="dataTable" /> 
                <RowStyle CssClass="dataTable" /> 
                <AlternatingRowStyle BackColor="#C7E3D7" />
                <Columns> 
                    <asp:TemplateField><ItemTemplate><a href="javascript:switchViews('div<%# Eval("Year")%>', 'one');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></ItemTemplate><AlternatingItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'alt');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></AlternatingItemTemplate></asp:TemplateField> 
                    <asp:BoundField DataField="Year" HeaderText="Year" SortExpression="Year" /> 
                    <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance"  ItemStyle-HorizontalAlign="Right" /> 
                    <asp:TemplateField><ItemTemplate></td></tr><tr><td colspan="100%" align="center"><div id="div<%# Eval("ID")%>" style="display:none;" ><asp:GridView ID="GridView2" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Payments</b></td></tr></table>"
                                            AutoGenerateColumns="false" DataKeyNames="ID" 
                                            EmptyDataText="No History." OnRowDataBound="gv_PointAdjustments" ><HeaderStyle CssClass="dataTable" /><AlternatingRowStyle BackColor="#C7E3D7" /><RowStyle CssClass="dataTable" /><Columns><asp:TemplateField><ItemTemplate><a href="javascript:switchViews('div3<%# Eval("ID")%>', 'one');"><img id="imgdiv3<%# Eval("ID")%>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></ItemTemplate><AlternatingItemTemplate><a href="javascript:switchViews('div3<%# Eval("ID")%>', 'alt');"><img id="imgdiv3<%# Eval("ID")%>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></AlternatingItemTemplate></asp:TemplateField><asp:BoundField DataField="ID" HeaderText="ID"  HtmlEncode="False" /><asp:BoundField DataField="TransType" HeaderText="TransType" HtmlEncode="False" /><asp:BoundField DataField="ConfirmationNumber" HeaderText="Conf #" HtmlEncode="false" /><asp:BoundField DataField="AvailYear" HeaderText="AvailYear" HtmlEncode="False" /><asp:BoundField DataField="UsageYear" HeaderText="UsageYear" HtmlEncode="False" /><asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" /><asp:BoundField DataField="ExpirationDate" HeaderText="ExpirationDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" /><asp:BoundField DataField="Points" HeaderText="Points"  ItemStyle-HorizontalAlign="Right" /><asp:TemplateField><ItemTemplate><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpoints.aspx?id=<%#container.Dataitem("ID")%>','Edit',350,350);">Edit</a></ItemTemplate></asp:TemplateField><asp:TemplateField><ItemTemplate></td></tr><tr><td colspan="100%" align='center'><div id="div3<%# Eval("ID")%>" style="display:none;" ><asp:GridView ID="gvPointAdjustments" runat="server" Width="80%" 
                                                                            AutoGenerateColumns="false" DataKeyNames="ID" 
                                                                            EmptyDataText="No History." ><HeaderStyle CssClass="dataTable" /><AlternatingRowStyle BackColor="#C7E3D7" /><RowStyle CssClass="dataTable" /><Columns><asp:BoundField DataField="ID" HeaderText="ID"  HtmlEncode="False" /><asp:BoundField DataField="TransType" HeaderText="Trans Type" HtmlEncode="False" /><asp:BoundField DataField="ConfirmationNumber" HeaderText="Conf #" HtmlEncode="false" /><asp:BoundField DataField="AvailYear" HeaderText="Avail Year" HtmlEncode="False" /><asp:BoundField DataField="UsageYear" HeaderText="Usage Year" HtmlEncode="False" /><asp:BoundField DataField="TransDate" HeaderText="Trans Date" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" /><asp:BoundField DataField="Points" HeaderText="Points" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" /><asp:BoundField DataField="ExpirationDate" HeaderText="ExpirationDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" /><asp:BoundField DataField="Applied" HeaderText="Applied" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" /><asp:templatefield><ItemTemplate><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpoints.aspx?d=<%#container.Dataitem("ID")%>','Edit',350,350);">Edit</a></ItemTemplate></asp:templatefield></Columns></asp:GridView></div></td></tr></ItemTemplate></asp:TemplateField></Columns></asp:GridView><br /></div></td></tr></ItemTemplate></asp:TemplateField> 
                </Columns> 
            </asp:GridView>
        </div>
        <%If CheckSecurity("PointsTracking", "Create", , , Session("UserDBID")) Then%>
        <div style="width:890px;border-top:thin solid black">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:RadioButtonList ID="rblEntryType0" runat="server" AutoPostBack="True" RepeatDirection="Horizontal">
                            <asp:ListItem>Bank</asp:ListItem>
                            <asp:ListItem>ReBank</asp:ListItem>
                            <asp:ListItem>Usage</asp:ListItem>
                            <asp:ListItem>Borrow</asp:ListItem>
                            <asp:ListItem>Rent</asp:ListItem>
                            <asp:ListItem>Deposit</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RadioButtonList ID="rblUsageType0" runat="server" RepeatDirection="Horizontal" Visible="False">
                            <asp:ListItem>KCP Stay</asp:ListItem>
                            <asp:ListItem>Exchange</asp:ListItem>
                            <asp:ListItem>Rental Pool</asp:ListItem>
                        </asp:RadioButtonList>
            
                    <table>
                        <thead>
                            <tr>
                                <td>Contract</td>
                                <td>Avail Year</td>
                                <td>Usage Year</td>
                                <td>Stay Date</td>
                                <td>Conf #</td>
                                <td>Amount</td>
                                <td>Balance Available</td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td valign="top">
                                    <asp:DropDownList ID="ddContract" runat="server" AutoPostBack="True"></asp:DropDownList>
                                </td>
                                <td valign="top">
                                    <asp:DropDownList ID="ddAvailYear" runat="server" AutoPostBack="True"></asp:DropDownList>
                                </td>
                                <td valign="top">
                                    <asp:DropDownList ID="ddUsageYear" runat="server" AutoPostBack="True"></asp:DropDownList>
                                </td>
                                <td valign="top">
                                    <uc1:DateField runat="server" ID="dteStay" />
                                </td>
                                <td valign="top"><asp:TextBox ID="txtConfNum" runat="server"></asp:TextBox></td>
                                <td valign="top"><asp:TextBox ID="txtAmount" runat="server" Width="87px">0</asp:TextBox></td>
                                <td valign="top"><asp:TextBox ID="txtAvailable" runat="server" ReadOnly="True" Width="95px">0</asp:TextBox></td>
                            </tr>
                        </tbody>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
        
        <ul id="menu">
            <li><asp:LinkButton ID="lbSave" runat="server">Save</asp:LinkButton></li>
            <li><asp:LinkButton ID="lbCancel" runat="server">Cancel</asp:LinkButton></li>
            
        </ul>
        <% End If%>
        
        
        </asp:View>
</asp:MultiView>
<asp:HiddenField ID="hfProsID" runat="server" Value="0" />
<asp:HiddenField ID="hfContractID" runat="server" />
<asp:HiddenField ID="hfListFilter" runat="server" />
<asp:HiddenField ID="hfView" runat="server" />