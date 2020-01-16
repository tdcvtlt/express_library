<%@ Page Title="Cancellation Wizard" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CancellationWiz.aspx.vb" Inherits="wizards_Accounting_CancellationWiz" %>

<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc1" TagName="DateField" %>
<%@ Register Src="~/controls/Select_Item.ascx" TagPrefix="uc1" TagName="Select_Item" %>
<%@ Register Src="~/controls/Events.ascx" TagPrefix="uc1" TagName="Events" %>




<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li><asp:LinkButton ID="lbContinue" runat="server">Continue Existing Batch</asp:LinkButton></li>
        <li><asp:LinkButton ID="lbStart" runat="server">Start New Batch</asp:LinkButton></li>
    </ul>
    <asp:HiddenField ID="hfBatchID" Value="0" runat="server" />
    <asp:HiddenField ID="hfShowReport" Value ="0" runat="server" />
    <div>
        <table width="100%">
            <tr align="left">
                <td>
                    <asp:Label ID="lblMessage" runat="server" Text=" " ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table width="100%">
            <tr align="left">
                <td>
                    <asp:Label ID="lblBatchName" runat="server" Text=" " ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View0" runat="server">
            <!-- View 0 - Start New -->
            <table>
                <tr>
                    <td>Type:</td>
                    <td><asp:DropDownList ID="ddType" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Hearing Date:</td>                    
                    <td><uc1:DateField runat="server" ID="dfHearingDate" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button runat="server" ID="btnCreate" Text="Create Batch" /></td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View1" runat="server">
            <!-- View 1 Upload File or Add Contracts to Batch name: siType.selectedItem.text & "-" & dfHearingDate.selected_Date -->
            <ul id="menu">
                <li><asp:LinkButton ID="lbUpload" runat="server">Upload File</asp:LinkButton></li>
                <li><asp:LinkButton ID="lbAddContracts" runat="server">Add Individual Contracts</asp:LinkButton></li>
            </ul>
            <asp:MultiView ID="mvFiles" runat="server">
                <asp:View ID="FilesView0" runat="server">
                    <!-- View 0 - Upload File -->
                    <br />
                    <div>
                        <asp:Label ID="lbFileUpload" runat="server" Text="File:"></asp:Label>
                        <asp:FileUpload ID="xlsUpload" runat="server" ToolTip="File" />
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td><asp:Button ID="btnUpload" runat="server" Text="Upload File" /></td>
                            </tr>
                            <tr>
                                <td><asp:Button ID="btnStart" runat="server" Text="Start Process" Enabled="false" /></td>
                            </tr>
                        </table>
                    </div>
                    
                    <div style="margin-top:20px;">
                        <table>
                            <asp:GridView ID="gvBatch" runat="server" AutoGenerateColumns="true"></asp:GridView>
                        </table>
                    </div>
                </asp:View>
                <asp:View ID="FilesView1" runat="server">
                    <!-- View 1 - Manual Entry -->
                    <table>
                        <tr>
                            <td>KCP Number:</td>
                            <td><asp:TextBox ID="txtKCP" runat="server" Text=""></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2"><asp:Button ID="btnAdd" runat="server" Text="Add" /></td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:MultiView ID="mvContinue" runat="server">
                <asp:View ID="mvContinueViewSelect" runat="server">
                    <asp:GridView ID="gvContinueContracts" runat="server" AutoGenerateColumns="true" AutoGenerateSelectButton="true">
                        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                        <AlternatingRowStyle BackColor="#CCFFCC" />        
                        
                    </asp:GridView>
                </asp:View>
                <asp:View ID="mvContinueViewEdit" runat="server">
                    <asp:Label ID="lbBatchName" runat="server"></asp:Label>
                    <ul id="menu">
                        <li><asp:LinkButton ID="lbBatch" runat="server">Batch</asp:LinkButton></li>
                        <li><asp:LinkButton ID="lbContracts" runat="server">Contracts In Batch</asp:LinkButton></li>
                        <li><asp:LinkButton ID="lbEvents" runat="server">Events</asp:LinkButton></li>
                    </ul>
                    <asp:MultiView ID="mvSteps" runat="server">
                        <asp:View ID="mvStepsViewBatch" runat="server">
                            <table>
                                <tr>
                                    <td>Type:</td>
                                    <td><asp:DropDownList ID="ddBatchType" runat="server"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>Hearing Date:</td>                    
                                    <td><uc1:DateField runat="server" ID="dfBatchHearingDate" /></td>
                                </tr>
                                <tr>
                                    <td>First Publication Date:</td>                    
                                    <td><uc1:DateField runat="server" ID="dfPublicationDate1" /></td>
                                </tr>
                                <tr>
                                    <td>Second Publication Date:</td>                    
                                    <td><uc1:DateField runat="server" ID="dfPublicationDate2" /></td>
                                </tr>
                                <tr>
                                    <td>Third Publication Date:</td>                    
                                    <td><uc1:DateField runat="server" ID="dfPublicationDate3" /></td>
                                </tr>
                                <tr>
                                    <td>Forth Publication Date:</td>                    
                                    <td><uc1:DateField runat="server" ID="dfPublicationDate4" /></td>
                                </tr>
                                <tr>
                                    <td>Instrument #:</td>
                                    <td><asp:textbox ID="txtInstNum" runat="server" ></asp:textbox></td>
                                </tr>
                                <tr>
                                    <td>Next Step:</td>
                                    <td>
                                        <asp:TextBox ID="txtNextStep" runat="server" ReadOnly="true"></asp:TextBox>
                                        <asp:button ID="btnNextStep" runat="server" Text="Perform Step" />
                                        <asp:Label ID="lblNextStep" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Next Step Date:</td>
                                    <td><asp:TextBox ID="txtNextStepDate" runat="server" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Status:</td>
                                    <td><uc1:Select_Item runat="server" ID="siBatchStatus" /></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td><asp:Button runat="server" ID="btnUpdateBatch" Text="Update Batch" /></td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnStep1" runat="server" Text="Print Initial Letter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnCertified" runat="server" Text="Print Certified Letter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAffidavit" runat="server" Text="Print Affidavit" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnLegalAd" runat="server" Text="Print Legal Ad" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnCertMailCSV" runat="server" Text="Export To Excel" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:button runat="server" ID="btnUpdateContracts" Text="Update Conveyance information" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="mvStepsViewContracts" runat="server">
                            <ul id="menu">
                                <li><asp:LinkButton ID="lbAdd" runat="server">Add Contracts</asp:LinkButton></li>
                                <li><asp:LinkButton ID="lbExcel" runat="server">Export to Excel</asp:LinkButton></li>
                            </ul>
                            <asp:GridView ID="gvBatchContracts" runat="server" AutoGenerateColumns="true">
                                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                                <AlternatingRowStyle BackColor="#CCFFCC" />  
                                <Columns>
                                    <asp:TemplateField HeaderText="Remove">
                                        <ItemTemplate>
                                            <a href="#" title="Remove" onclick = "javascript:modal.mwindow.open('<%=Request.ApplicationPath%>/Wizards/Accounting/removebatchitem.aspx?bid=<%=hfBatchID.value %>&id=<%#Container.DataItem("KCP")%>','win01','350','350');">Remove</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:View>
                        <asp:View ID="mvStepsViewEvents" runat="server">
                            <uc1:Events runat="server" ID="Events" />
                        </asp:View>
                    </asp:MultiView>
                </asp:View>
            </asp:MultiView>

        </asp:View>
    </asp:MultiView>
    <asp:MultiView ID="mvReport" runat="server">
        <asp:View ID="vwBlank" runat="server">

        </asp:View>
        <asp:View ID="vwReport" runat="server">

            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
            <CR:CrystalReportViewer ID="CrystalReportViewer2" runat="server" AutoDataBind="true" />
        </asp:View>
    </asp:MultiView>
</asp:Content>

