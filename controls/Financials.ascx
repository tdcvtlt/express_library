<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Financials.ascx.vb" Inherits="controls_Financials" %>
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
<div>

<asp:MultiView ID="MultiView1" runat="server">
    <asp:View ID="vMortgage" runat="server">
        <div class="ListGrid">
            <asp:GridView ID="gvMortgage" runat="server"  
                AutoGenerateColumns="False" DataKeyNames="ID" 
                 OnRowDataBound="gv_RowDataBound" 
                AllowPaging="false" PageSize="20" Width="100%" > 
                <HeaderStyle CssClass="dataTable" /> 
                <RowStyle CssClass="dataTable" /> 
                <AlternatingRowStyle BackColor="#C7E3D7" />
                <Columns> 
                    <asp:TemplateField><ItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'one');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></ItemTemplate><AlternatingItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'alt');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></AlternatingItemTemplate></asp:TemplateField> 
                    <asp:BoundField DataField="Invoice" HeaderText="Invoice" SortExpression="Invoice" /> 
                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" SortExpression="DueDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                    <asp:BoundField DataField="InvoiceAmount" HeaderText="Orig. Amount" SortExpression="InvoiceAmount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Adj. Amount" SortExpression="Amount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Balance" HeaderText="Balance" 
                        SortExpression="Balance" DataFormatString="{0:c}" 
                        ItemStyle-HorizontalAlign="Right" > 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:templatefield><ItemTemplate><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoice.aspx?listfilter=contracttrans,mftrans&invoiceid=<%#container.Dataitem("ID")%>','Edit',350,350);">Edit</a></ItemTemplate></asp:templatefield>
                    <asp:TemplateField>
                        <ItemTemplate>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="100%" align="center">
                                    <div id="div<%# Eval("ID") %>" style="display:none;" >
                                        <asp:GridView ID="GridView2" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Payments</b></td></tr></table>"
                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                            EmptyDataText="No History." OnRowDataBound="gv_PaymentAdjustments" >
                                            <HeaderStyle CssClass="dataTable" />
                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                            <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID")  %>', 'one');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID") %>', 'alt');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </AlternatingItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Applied" HeaderText="Applied" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="100%" align='center'>
                                                                    <div id="div3<%# Eval("PaymentID") %>" style="display:none;" >
                                                                        <asp:GridView ID="gvPaymentAdjustments" runat="server" Width="80%" 
                                                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                                                            EmptyDataText="No History." ><HeaderStyle CssClass="dataTable" />
                                                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                                                            <RowStyle CssClass="dataTable" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                                                <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                                                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                                                <asp:templatefield>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                                                    </ItemTemplate>
                                                                                </asp:templatefield>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView><br />
                                            <asp:GridView ID="GridView8" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Adjustments</b></td></tr></table>"
                                                AutoGenerateColumns="false" DataKeyNames="InvoiceID" 
                                                EmptyDataText="No History."  >
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoiceadjustment.aspx?InvoiceID=<%#container.Dataitem("InvoiceID") %>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView><br />
                                            <asp:GridView ID="GridView1" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Scheduled Payments</b></td></tr></table>"
                                                AutoGenerateColumns="false" DataKeyNames="SchedPayID" 
                                                EmptyDataText="No History."  onRowDataBound = "GridView1_RowDataBound" onRowCommand = "GridView1_RowCommand">
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="SchedPayID" HeaderText="SchedPayID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="SchedDate" HeaderText="SchedDate" SortExpression="SchedDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="CreditCardID" HeaderText="CreditCardID" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editScheduledPayment.aspx?ID=<%#container.Dataitem("SchedPayID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:ButtonField CommandName="Receive" Text="Receive Payment"></asp:ButtonField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("SchedPayID")%>&Scheduled=True','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField> 
                </Columns> 
            </asp:GridView> 
        </div>

    </asp:View>
    <asp:View ID="MortageDP" runat="server">
        <div class="ListGrid">
            <asp:GridView ID="gvMortgageDP" runat="server"  
                AutoGenerateColumns="False" DataKeyNames="ID" 
                 OnRowDataBound="gv_RowDataBound" 
                AllowPaging="false" PageSize="20" Width="100%" EmptyDataText="No Records" > 
                <HeaderStyle CssClass="dataTable" /> 
                <RowStyle CssClass="dataTable" /> 
                <AlternatingRowStyle BackColor="#C7E3D7" />
                <Columns> 
                    <asp:TemplateField><ItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'one');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></ItemTemplate><AlternatingItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'alt');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></AlternatingItemTemplate></asp:TemplateField> 
                    <asp:BoundField DataField="Invoice" HeaderText="Invoice" SortExpression="Invoice" /> 
                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" SortExpression="DueDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                   <asp:BoundField DataField="InvoiceAmount" HeaderText="Orig. Amount" SortExpression="InvoiceAmount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Adj. Amount" SortExpression="Amount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Balance" HeaderText="Balance" 
                        SortExpression="Balance" DataFormatString="{0:c}" 
                        ItemStyle-HorizontalAlign="Right" > 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:templatefield><ItemTemplate><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoice.aspx?listfilter=MortgageDP,MortgageCC,MortgageMem,ConversionDP,ConversionCC&invoiceid=<%#container.Dataitem("ID")%>','Edit',350,350);">Edit</a></ItemTemplate></asp:templatefield>
                    <asp:TemplateField>
                        <ItemTemplate>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="100%" align="center">
                                    <div id="div<%# Eval("ID") %>" style="display:none;" >
                                        <asp:GridView ID="GridView2" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Payments</b></td></tr></table>"
                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                            EmptyDataText="No History." OnRowDataBound="gv_PaymentAdjustments" >
                                            <HeaderStyle CssClass="dataTable" />
                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                            <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID")  %>', 'one');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID") %>', 'alt');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </AlternatingItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Applied" HeaderText="Applied" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="100%" align='center'>
                                                                    <div id="div3<%# Eval("PaymentID") %>" style="display:none;" >
                                                                        <asp:GridView ID="gvPaymentAdjustments" runat="server" Width="80%" 
                                                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                                                            EmptyDataText="No History." ><HeaderStyle CssClass="dataTable" />
                                                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                                                            <RowStyle CssClass="dataTable" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                                                <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                                                <asp:templatefield>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                                                    </ItemTemplate>
                                                                                </asp:templatefield>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView><br />
                                            <asp:GridView ID="GridView8" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Adjustments</b></td></tr></table>"
                                                AutoGenerateColumns="false" DataKeyNames="InvoiceID" 
                                                EmptyDataText="No History."  >
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoiceadjustment.aspx?InvoiceID=<%#container.Dataitem("InvoiceID") %>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <br /><asp:GridView ID="GridView1" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Scheduled Payments</b></td></tr></table>"
                                                AutoGenerateColumns="false" DataKeyNames="SchedPayID" 
                                                EmptyDataText="No History."  onRowDataBound = "GridView1_RowDataBound" onRowCommand = "GridView1_RowCommand">
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="SchedPayID" HeaderText="SchedPayID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="SchedDate" HeaderText="SchedDate" SortExpression="SchedDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="CreditCardID" HeaderText="CreditCardID" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editScheduledPayment.aspx?ID=<%#container.Dataitem("SchedPayID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:ButtonField CommandName="Receive" Text="Receive Payment"></asp:ButtonField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("SchedPayID")%>&Scheduled=True','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField> 
                </Columns> 
            </asp:GridView> 
        </div>
        
    </asp:View>
    
    <asp:View ID="Reservation" runat="server">
        <div class="ListGrid">
            Balance: <asp:Label ID="lblResBalance" runat="server" Text=""></asp:Label>
            &nbsp;<asp:GridView ID="gvReservation" runat="server"  
                AutoGenerateColumns="False" DataKeyNames="ID" 
                 OnRowDataBound="gv_RowDataBound" 
                AllowPaging="false" PageSize="20" Width="100%" > 
                <HeaderStyle CssClass="dataTable" /> 
                <RowStyle CssClass="dataTable" /> 
                <AlternatingRowStyle BackColor="#C7E3D7" />
                <Columns> 
                    <asp:TemplateField><ItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'one');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></ItemTemplate><AlternatingItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'alt');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></AlternatingItemTemplate></asp:TemplateField> 
                    <asp:BoundField DataField="Invoice" HeaderText="Invoice" SortExpression="Invoice" /> 
                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" SortExpression="DueDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                    <asp:BoundField DataField="InvoiceAmount" HeaderText="Orig. Amount" SortExpression="InvoiceAmount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Adj. Amount" SortExpression="Amount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Balance" HeaderText="Balance" 
                        SortExpression="Balance" DataFormatString="{0:c}" 
                        ItemStyle-HorizontalAlign="Right" > 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:templatefield><ItemTemplate><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoice.aspx?listfilter=reservationtrans&invoiceid=<%#container.Dataitem("ID")%>','Edit',350,350);">Edit</a></ItemTemplate></asp:templatefield>
                    <asp:TemplateField>
                        <ItemTemplate>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="100%" align="center">
                                    <div id="div<%# Eval("ID") %>" style="display:none;" >
                                        <asp:GridView ID="GridView2" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Payments</b></td></tr></table>"
                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                            EmptyDataText="No History." OnRowDataBound="gv_PaymentAdjustments" >
                                            <HeaderStyle CssClass="dataTable" />
                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                            <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID")  %>', 'one');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID") %>', 'alt');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </AlternatingItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Applied" HeaderText="Applied" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="100%" align='center'>
                                                                    <div id="div3<%# Eval("PaymentID") %>" style="display:none;" >
                                                                        <asp:GridView ID="gvPaymentAdjustments" runat="server" Width="80%" 
                                                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                                                            EmptyDataText="No History." ><HeaderStyle CssClass="dataTable" />
                                                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                                                            <RowStyle CssClass="dataTable" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                                                <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                                                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                                                <asp:templatefield>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                                                    </ItemTemplate>
                                                                                </asp:templatefield>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView><br />
                                            <asp:GridView ID="GridView8" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Adjustments</b></td></tr></table>"
                                                AutoGenerateColumns="false" DataKeyNames="InvoiceID" 
                                                EmptyDataText="No History."  >
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoiceadjustment.aspx?InvoiceID=<%#container.Dataitem("InvoiceID") %>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView><br />
                                            <asp:GridView ID="GridView1" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Scheduled Payments</b></td></tr></table>"
                                                AutoGenerateColumns="false" 
                                                EmptyDataText="No History."  onRowDataBound = "GridView1_RowDataBound" onRowCommand = "GridView1_RowCommand">
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="SchedPayID" HeaderText="SchedPayID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="SchedDate" HeaderText="SchedDate" SortExpression="SchedDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="CreditCardID" HeaderText="CreditCardID" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editScheduledPayment.aspx?ID=<%#container.Dataitem("SchedPayID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:ButtonField CommandName="Receive" Text="Receive Payment"></asp:ButtonField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("SchedPayID")%>&Scheduled=True','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField> 
                </Columns> 
            </asp:GridView> 
        </div>

    </asp:View>
    <asp:View ID="Tour" runat="server">
        <div class="ListGrid">
        Balance: <asp:Label ID="lblTourBalance" runat="server" Text=""></asp:Label>
            <asp:GridView ID="gvTour" runat="server"  
                AutoGenerateColumns="False" DataKeyNames="ID" 
                 OnRowDataBound="gv_RowDataBound" 
                AllowPaging="false" PageSize="20" Width="100%" EmptyDataText="No Records" > 
                <HeaderStyle CssClass="dataTable" /> 
                <RowStyle CssClass="dataTable" /> 
                <AlternatingRowStyle BackColor="#C7E3D7" />
                <Columns> 
                    <asp:TemplateField><ItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'one');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></ItemTemplate><AlternatingItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'alt');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></AlternatingItemTemplate></asp:TemplateField> 
                    <asp:BoundField DataField="Invoice" HeaderText="Invoice" SortExpression="Invoice" /> 
                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" SortExpression="DueDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                    <asp:BoundField DataField="InvoiceAmount" HeaderText="Orig. Amount" SortExpression="InvoiceAmount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Adj. Amount" SortExpression="Amount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Balance" HeaderText="Balance" 
                        SortExpression="Balance" DataFormatString="{0:c}" 
                        ItemStyle-HorizontalAlign="Right" > 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:templatefield><ItemTemplate><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoice.aspx?listfilter=tourtrans&invoiceid=<%#container.Dataitem("ID")%>','Edit',350,350);">Edit</a></ItemTemplate></asp:templatefield>
                    <asp:TemplateField>
                        <ItemTemplate>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="100%" align="center">
                                    <div id="div<%# Eval("ID") %>" style="display:none;" >
                                        <asp:GridView ID="GridView2" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Payments</b></td></tr></table>"
                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                            EmptyDataText="No History." OnRowDataBound="gv_PaymentAdjustments" >
                                            <HeaderStyle CssClass="dataTable" />
                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                            <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID")  %>', 'one');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID") %>', 'alt');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </AlternatingItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Applied" HeaderText="Applied" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="100%" align='center'>
                                                                    <div id="div3<%# Eval("PaymentID") %>" style="display:none;" >
                                                                        <asp:GridView ID="gvPaymentAdjustments" runat="server" Width="80%" 
                                                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                                                            EmptyDataText="No History." ><HeaderStyle CssClass="dataTable" />
                                                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                                                            <RowStyle CssClass="dataTable" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                                                <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                                                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                                                <asp:templatefield>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                                                    </ItemTemplate>
                                                                                </asp:templatefield>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView><br />
                                            <asp:GridView ID="GridView8" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Adjustments</b></td></tr></table>"
                                                AutoGenerateColumns="false" DataKeyNames="InvoiceID" 
                                                EmptyDataText="No History."  >
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoiceadjustment.aspx?InvoiceID=<%#container.Dataitem("InvoiceID") %>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView><br />
                                            <asp:GridView ID="GridView1" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Scheduled Payments</b></td></tr></table>"
                                                AutoGenerateColumns="false" DataKeyNames="SchedPayID" 
                                                EmptyDataText="No History."  onRowDataBound = "GridView1_RowDataBound"  onRowCommand = "GridView1_RowCommand">
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="SchedPayID" HeaderText="SchedPayID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="SchedDate" HeaderText="SchedDate" SortExpression="SchedDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="CreditCardID" HeaderText="CreditCardID" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editScheduledPayment.aspx?ID=<%#container.Dataitem("SchedPayID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:ButtonField CommandName="Receive" Text="Receive Payment"></asp:ButtonField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("SchedPayID")%>&Scheduled=True','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField> 
                </Columns> 
            </asp:GridView> 
        </div>

    </asp:View>
    <asp:View ID="Contract" runat="server">
        <div class="ListGrid">
            <asp:GridView ID="gvContract" runat="server"  
                AutoGenerateColumns="False" DataKeyNames="ID" 
                 OnRowDataBound="gv_RowDataBound" 
                AllowPaging="false" PageSize="20" Width="100%" > 
                <HeaderStyle CssClass="dataTable" /> 
                <RowStyle CssClass="dataTable" /> 
                <AlternatingRowStyle BackColor="#C7E3D7" />
                <Columns> 
                    <asp:TemplateField><ItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'one');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></ItemTemplate><AlternatingItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'alt');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></AlternatingItemTemplate></asp:TemplateField> 
                    <asp:BoundField DataField="Invoice" HeaderText="Invoice" SortExpression="Invoice" /> 
                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" SortExpression="DueDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                    <asp:BoundField DataField="InvoiceAmount" HeaderText="Orig. Amount" SortExpression="InvoiceAmount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Adj. Amount" SortExpression="Amount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Balance" HeaderText="Balance" 
                        SortExpression="Balance" DataFormatString="{0:c}" 
                        ItemStyle-HorizontalAlign="Right" > 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:templatefield><ItemTemplate><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoice.aspx?listfilter=contracttrans,mftrans&invoiceid=<%#container.Dataitem("ID")%>','Edit',350,350);">Edit</a></ItemTemplate></asp:templatefield>
                    <asp:TemplateField>
                        <ItemTemplate>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="100%" align="center">
                                    <div id="div<%# Eval("ID") %>" style="display:none;" >
                                        <asp:GridView ID="GridView2" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Payments</b></td></tr></table>"
                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                            EmptyDataText="No History." OnRowDataBound="gv_PaymentAdjustments" >
                                            <HeaderStyle CssClass="dataTable" />
                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                            <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID")  %>', 'one');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID") %>', 'alt');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </AlternatingItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Applied" HeaderText="Applied" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="100%" align='center'>
                                                                    <div id="div3<%# Eval("PaymentID") %>" style="display:none;" >
                                                                        <asp:GridView ID="gvPaymentAdjustments" runat="server" Width="80%" 
                                                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                                                            EmptyDataText="No History." ><HeaderStyle CssClass="dataTable" />
                                                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                                                            <RowStyle CssClass="dataTable" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                                                <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                                                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                                                <asp:templatefield>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                                                    </ItemTemplate>
                                                                                </asp:templatefield>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView><br />
                                            <asp:GridView ID="GridView8" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Adjustments</b></td></tr></table>"
                                                AutoGenerateColumns="false" DataKeyNames="InvoiceID" 
                                                EmptyDataText="No History."  >
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoiceadjustment.aspx?InvoiceID=<%#container.Dataitem("InvoiceID") %>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView><br />
                                            <asp:GridView ID="GridView1" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Scheduled Payments</b></td></tr></table>"
                                                AutoGenerateColumns="false" DataKeyNames="SchedPayID" 
                                                EmptyDataText="No History."  onRowDataBound = "GridView1_RowDataBound" onRowCommand = "GridView1_RowCommand">
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="SchedPayID" HeaderText="SchedPayID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="SchedDate" HeaderText="SchedDate" SortExpression="SchedDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="CreditCardID" HeaderText="CreditCardID" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editScheduledPayment.aspx?ID=<%#container.Dataitem("SchedPayID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:ButtonField CommandName="Receive" Text="Receive Payment"></asp:ButtonField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("SchedPayID")%>&Scheduled=True','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField> 
                </Columns> 
            </asp:GridView> 
        </div>

    </asp:View>
    <asp:View ID="Prospect" runat="server">
        <div>Contract: 
            <asp:DropDownList ID="ddContracts" runat="server" AutoPostBack="True">
                <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnArchives" runat="server" Text="Archives" />
        </div>
        <div class="ListGrid">
            Balance: <asp:Label ID="lblProsBalance" runat="server" Text=""></asp:Label>
            <asp:GridView ID="gvProspect" runat="server"  
                AutoGenerateColumns="False" DataKeyNames="ID" 
                 OnRowDataBound="gv_RowDataBound" 
                AllowPaging="false" PageSize="20" Width="100%" > 
                <HeaderStyle CssClass="dataTable" /> 
                <RowStyle CssClass="dataTable" /> 
                <AlternatingRowStyle BackColor="#C7E3D7" />
                <Columns> 
                    <asp:TemplateField><ItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'one');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></ItemTemplate><AlternatingItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'alt');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></AlternatingItemTemplate></asp:TemplateField> 
                    <asp:BoundField DataField="Invoice" HeaderText="Invoice" SortExpression="Invoice" /> 
                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" SortExpression="DueDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                    <asp:BoundField DataField="InvoiceAmount" HeaderText="Orig. Amount" SortExpression="InvoiceAmount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Adj. Amount" SortExpression="Amount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField> 
                    <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" /> 
                    <asp:templatefield><ItemTemplate><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoice.aspx?listfilter=contracttrans,mftrans,prospecttrans,lftrans&invoiceid=<%#container.Dataitem("ID")%>','Edit',350,350);">Edit</a></ItemTemplate></asp:templatefield>
                    <asp:TemplateField>
                        <ItemTemplate>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="100%" align="center">
                                    <div id="div<%# Eval("ID") %>" style="display:none;" >
                                        <asp:GridView ID="GridView2" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Payments</b></td></tr></table>"
                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                            EmptyDataText="No History." OnRowDataBound="gv_PaymentAdjustments" >
                                            <HeaderStyle CssClass="dataTable" />
                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                            <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID")  %>', 'one');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID") %>', 'alt');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </AlternatingItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Applied" HeaderText="Applied" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="100%" align='center'>
                                                                    <div id="div3<%# Eval("PaymentID") %>" style="display:none;" >
                                                                        <asp:GridView ID="gvPaymentAdjustments" runat="server" Width="80%" 
                                                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                                                            EmptyDataText="No History." ><HeaderStyle CssClass="dataTable" />
                                                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                                                            <RowStyle CssClass="dataTable" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                                                <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                                                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                                                <asp:templatefield>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                                                    </ItemTemplate>
                                                                                </asp:templatefield>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView><br />
                                            <asp:GridView ID="GridView8" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Adjustments</b></td></tr></table>"
                                                AutoGenerateColumns="false" DataKeyNames="InvoiceID" 
                                                EmptyDataText="No History."  >
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoiceadjustment.aspx?InvoiceID=<%#container.Dataitem("InvoiceID") %>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView><br />
                                            <asp:GridView ID="GridView1" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Scheduled Payments</b></td></tr></table>"
                                                AutoGenerateColumns="false" DataKeyNames="SchedPayID" 
                                                EmptyDataText="No History."  onRowDataBound = "GridView1_RowDataBound" onRowCommand = "GridView1_RowCommand">
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="SchedPayID" HeaderText="SchedPayID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="SchedDate" HeaderText="SchedDate" SortExpression="SchedDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="CreditCardID" HeaderText="CreditCardID" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editScheduledPayment.aspx?ID=<%#container.Dataitem("SchedPayID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:ButtonField CommandName="Receive" Text="Receive Payment"></asp:ButtonField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("SchedPayID")%>&Scheduled=True','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField> 
                </Columns> 
            </asp:GridView>
            Pending Payments: <br />
            <asp:GridView ID="gvPendingPayments" runat="server" EmptyDataText = "No Pending Payments" AutoGenerateColumns="false">
                <HeaderStyle CssClass="dataTable" />
                <AlternatingRowStyle BackColor="#C7E3D7" />
                <Columns>
                    <asp:BoundField DataField="PaymentType" HeaderText="Payment Method" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />                    
                    <asp:BoundField DataField="PaymentDate" HeaderText="PaymentDate" DataFormatString="{0:MMM-dd-yyyy}" />
                    <asp:BoundField DataField="CreatedDate" HeaderText="CreatedDate" DataFormatString="{0:MMM-dd-yyyy}" />
                    <asp:BoundField DataField="Exception" HeaderText="Exception" HtmlEncode="False" />
                </Columns>
            </asp:GridView>
        </div>
        
    </asp:View>
    <asp:View ID="PackageIssued" runat="server">
        <div class="ListGrid">
            Balance: <asp:Label ID="lblPkgBalance" runat="server" Text=""></asp:Label>
            <asp:GridView ID="gvPackageIssued" runat="server"  
                AutoGenerateColumns="False" DataKeyNames="ID" 
                 OnRowDataBound="gv_RowDataBound" 
                AllowPaging="false" PageSize="20" Width="100%" > 
                <HeaderStyle CssClass="dataTable" /> 
                <RowStyle CssClass="dataTable" /> 
                <AlternatingRowStyle BackColor="#C7E3D7" />
                <Columns> 
                    <asp:TemplateField><ItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'one');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></ItemTemplate><AlternatingItemTemplate><a href="javascript:switchViews('div<%# Eval("ID") %>', 'alt');"><img id="imgdiv<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" /></a></AlternatingItemTemplate></asp:TemplateField> 
                    <asp:BoundField DataField="Invoice" HeaderText="Invoice" SortExpression="Invoice" /> 
                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" SortExpression="DueDate" DataFormatString="{0:MMM-dd-yyyy}" /> 
                    <asp:BoundField DataField="InvoiceAmount" HeaderText="Orig. Amount" SortExpression="InvoiceAmount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Adj. Amount" SortExpression="Amount"  
                        DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"> 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Balance" HeaderText="Balance" 
                        SortExpression="Balance" DataFormatString="{0:c}" 
                        ItemStyle-HorizontalAlign="Right" > 
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:templatefield><ItemTemplate><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoice.aspx?listfilter=packagetrans&invoiceid=<%#container.Dataitem("ID")%>','Edit',350,350);">Edit</a></ItemTemplate></asp:templatefield>
                    <asp:TemplateField>
                        <ItemTemplate>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="100%" align="center">
                                    <div id="div<%# Eval("ID") %>" style="display:none;" >
                                        <asp:GridView ID="GridView2" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Payments</b></td></tr></table>"
                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                            EmptyDataText="No History." OnRowDataBound="gv_PaymentAdjustments" >
                                            <HeaderStyle CssClass="dataTable" />
                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                            <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID")  %>', 'one');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <a href="javascript:switchViews('div3<%# Eval("PaymentID") %>', 'alt');">
                                                                <img id="imgdiv3<%# Eval("PaymentID") %>" alt="Click to show/hide orders" border="0" src="<%=request.applicationpath %>/Images/expand_button.png" />
                                                            </a>
                                                        </AlternatingItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Applied" HeaderText="Applied" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="100%" align='center'>
                                                                    <div id="div3<%# Eval("PaymentID") %>" style="display:none;" >
                                                                        <asp:GridView ID="gvPaymentAdjustments" runat="server" Width="80%" 
                                                                            AutoGenerateColumns="false" DataKeyNames="PaymentID" 
                                                                            EmptyDataText="No History." ><HeaderStyle CssClass="dataTable" />
                                                                            <AlternatingRowStyle BackColor="#C7E3D7" />
                                                                            <RowStyle CssClass="dataTable" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="PaymentID" HeaderText="PaymentID"  HtmlEncode="False" />
                                                                                <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                                                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                                                <asp:templatefield>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpayment.aspx?paymentid=<%#container.Dataitem("PaymentID")%>','Edit',350,350);">Edit</a>
                                                                                    </ItemTemplate>
                                                                                </asp:templatefield>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("PaymentID")%>','Edit',650,450);">Receipt</a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView><br />
                                            <asp:GridView ID="GridView8" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Adjustments</b></td></tr></table>"
                                                AutoGenerateColumns="false" DataKeyNames="InvoiceID" 
                                                EmptyDataText="No History."  >
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editinvoiceadjustment.aspx?InvoiceID=<%#container.Dataitem("InvoiceID") %>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView><br />
                                            <asp:GridView ID="GridView1" runat="server" Width="80%" caption  ="<table border=0 width='100%'><tr><td align='left'><b>Scheduled Payments</b></td></tr></table>"
                                                AutoGenerateColumns="false" DataKeyNames="SchedPayID" 
                                                EmptyDataText="No History." onRowDataBound = "GridView1_RowDataBound" onRowCommand = "GridView1_RowCommand">
                                                <HeaderStyle CssClass="dataTable" />
                                                <AlternatingRowStyle BackColor="#C7E3D7" />
                                                <RowStyle CssClass="dataTable" />
                                                <Columns>
                                                    <asp:BoundField DataField="SchedPayID" HeaderText="SchedPayID"  HtmlEncode="False" />
                                                    <asp:BoundField DataField="Method" HeaderText="Method" HtmlEncode="False" />
                                                    <asp:BoundField DataField="TransDate" HeaderText="TransDate" SortExpression="TransDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="SchedDate" HeaderText="SchedDate" SortExpression="SchedDate" DataFormatString="{0:MMM-dd-yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="CreditCardID" HeaderText="CreditCardID" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editScheduledPayment.aspx?ID=<%#container.Dataitem("SchedPayID")%>','Edit',350,350);">Edit</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:ButtonField CommandName="Receive" Text="Receive Payment"></asp:ButtonField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Receipt.aspx?id=<%#container.Dataitem("SchedPayID")%>&Scheduled=True','Edit',650,450);">Receipt</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField> 
                </Columns> 
            </asp:GridView> 
        </div>
    </asp:View>


</asp:MultiView>
<%
    Dim oID As String = Me.ID & "$lbRefresh"
    Dim oTest As Object = Me.FindControl("lbRefresh")
    While Not (oTest Is Nothing)
        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
            oID = "ctl00$" & oTest.id & "$" & oID
        End If
        oTest = oTest.parent
    End While
 %>
    
        <ul id="menu">
            <% If Not (CheckSecurity("payments", "DenyInvoices_" & View, , , Session("UserDBID"))) Then%>
            <li><a href = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/EditInvoice.aspx?ListFilter=<%=hfListFilter.value %>&InvoiceID=0&ProspectID=<%=hfProsID.value %>&keyfield=<%=hfKeyField.value %>&KeyValue=<%=hfKeyValue.value%>&linkid=<%=oid %>','Edit',350,350);" id="invoice">Invoice</a></li>
            <%end if %>
            <% If View = "prospect" Or View = "contract" Then%>
                <% If Not (CheckSecurity("payments", "DenyLateFees_" & View, , , Session("UserDBID"))) Then%>
                <li><a href = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/addLateFee.aspx?ProspectID=<%=hfProsID.value %>&keyfield=<%=hfKeyField.value %>&KeyValue=<%=hfKeyValue.value%>&linkid=<%=oid %>','Edit',450,350);">Late Fee</a></li>
            <%End If%>
            <%end if %>
            <% If Not (CheckSecurity("payments", "DenyAdjustments_" & View, , , Session("UserDBID"))) Then%>
            <li><a href = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/InvoiceAdjustment.aspx?ProspectID=<%=hfProsID.value %>&keyfield=<%=hfKeyField.value %>&KeyValue=<%=hfKeyValue.value%>&linkid=<%=oid %>','Edit',650,350);">Invoice Adjustment</a></li>
            <%End If%>
            <% If Not (CheckSecurity("payments", "DenyPayments_" & View, , , Session("UserDBID"))) Then%>
            <li><a href = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/takepayment.aspx?ProspectID=<%=hfProsID.value %>&keyfield=<%=hfKeyField.value %>&KeyValue=<%=hfKeyValue.value%>&linkid=<%=oid %>','Edit',650,350);">Payment / Adjustment</a></li>
            <%End If%>
            <% If Not (CheckSecurity("payments", "DenyRefunds_" & View, , , Session("UserDBID"))) Then%>
            <li><a href = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/requestrefund.aspx?ProspectID=<%=hfProsID.value %>&keyfield=<%=hfKeyField.value %>&KeyValue=<%=hfKeyValue.value%>&linkid=<%=oid %>','Edit',650,350);">Refund</a></li>
            <%End If%>
            <!--<li><a href = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/takepayment.aspx?ProspectID=<%=hfProsID.value %>&keyfield=<%=hfKeyField.value %>&KeyValue=<%=hfKeyValue.value%>','Edit',350,350);">Archives</a></li>-->
            <li><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton></li>
        </ul>
        <ul id="menu">
            <% If Not (CheckSecurity("payments", "DenySchedulePayments_" & View, , , Session("UserDBID"))) Then%>
            <li><a href = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/takepayment.aspx?ProspectID=<%=hfProsID.value %>&keyfield=<%=hfKeyField.value %>&KeyValue=<%=hfKeyValue.value%>&linkid=<%=oid %>&Schedule=True','Edit',650,350);">Schedule Payment</a></li>
            <%End If%>
            <% If Not (CheckSecurity("payments", "DenyCancelScheduledPayments_" & View, , , Session("UserDBID"))) Then%>
            <li><a href = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/CXLSchedPayments.aspx?ProspectID=<%=hfProsID.value %>&keyfield=<%=hfKeyField.value %>&KeyValue=<%=hfKeyValue.value%>&linkid=<%=oid %>','Edit',650,350);">Cancel Scheduled Payments</a></li>    
            <%End If%>
            <% If Not (CheckSecurity("payments", "DenyTransferPayments_" & View, , , Session("UserDBID"))) Then%>
            <li><a href = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/Transferpayment.aspx?ProspectID=<%=hfProsID.value %>&keyfield=<%=hfKeyField.value %>&KeyValue=<%=hfKeyValue.value%>&linkid=<%=oid %>','Edit',650,350);">Transfer Payment</a></li>
            <%end if %>
            <li><a href = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/FinancialsStatementOfAccount.aspx?ProspectID=<%=hfProsID.value %>&keyfield=<%=hfKeyField.value %>&KeyValue=<%=hfKeyValue.value%>&linkid=<%=oid %>','Edit',768,768);">Statement Of Account</a></li>    
        </ul>
</div>
<asp:HiddenField ID="hfKeyField" runat="server" />
<asp:HiddenField ID="hfKeyValue" runat="server" />
<asp:HiddenField ID="hfProsID" runat="server" />
<asp:HiddenField ID="hfListFilter" runat="server" />
<asp:HiddenField ID="hfView" runat="server" />





