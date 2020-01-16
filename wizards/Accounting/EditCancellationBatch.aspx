<%@ Page Title="Edit Batch" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditCancellationBatch.aspx.vb" Inherits="wizards_Accounting_EditCancellationBatch" %>

<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc1" TagName="DateField" %>
<%@ Register Src="~/controls/Select_Item.ascx" TagPrefix="uc1" TagName="Select_Item" %>
<%@ Register Src="~/controls/Events.ascx" TagPrefix="uc1" TagName="Events" %>




<%@ Register assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI" tagprefix="asp" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .overlay 
            {
             position: absolute;
             background-color: white;
             top: 0px;
             left: 0px;
             width: 100%;
             height: 100%;
             opacity: 0.8;
             -moz-opacity: 0.8;
             filter: alpha(opacity=80);
             -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=80)";
             z-index: 10000;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="overlay" style="align-content:center;text-align:center;vertical-align:central;">
                <h2>Loading...</h2>
                <img src="../../images/progressbar.gif" alt="Loading"  />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" Text=" " ForeColor="Red"></asp:Label>
            <ul id="menu">
                <li><asp:LinkButton ID="lbBatch" runat="server">Batch</asp:LinkButton></li>
                <li><asp:LinkButton ID="lbContracts" runat="server">Contracts In Batch</asp:LinkButton></li>
                <li><asp:LinkButton ID="lbAddContracts" runat="server">Add Contracts</asp:LinkButton></li>
                <li><asp:LinkButton ID="lbEvents" runat="server">Events</asp:LinkButton></li>
            </ul>
            <asp:MultiView ID="mvSteps" runat="server">
                <asp:View ID="mvStepsViewBatch" runat="server">
                    <asp:table ID="tbBatchTable" runat="server">
                        <asp:tablerow id="trBatchType" runat="server">
                            <asp:TableCell>Type:</asp:TableCell>
                            <asp:TableCell><asp:DropDownList ID="ddBatchType" runat="server"></asp:DropDownList></asp:TableCell>
                        </asp:tablerow>
                        <asp:TableRow ID="trHearingDate" runat="server">
                            <asp:tablecell>Hearing Date:</asp:tablecell>                    
                            <asp:TableCell><uc1:DateField runat="server" ID="dfBatchHearingDate" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="trPublicationDate1" runat="server">
                            <asp:tablecell>First Publication Date:</asp:tablecell>                    
                            <asp:TableCell><uc1:DateField runat="server" ID="dfPublicationDate1" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="trPublicationDate2" runat="server">
                            <asp:tablecell>Second Publication Date:</asp:tablecell>                    
                            <asp:TableCell><uc1:DateField runat="server" ID="dfPublicationDate2" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="trInstNum" runat="server">
                            <asp:TableCell>Instrument #:</asp:TableCell>
                            <asp:TableCell>
                                <asp:textbox ID="txtInstNum" runat="server" ReadOnly="true"></asp:textbox>
                                <asp:button ID="btnGenInstRequest" runat="server" Text="Generate Request" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="trNextStep" runat="server">
                            <asp:TableCell>Next Step:</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="txtNextStep" runat="server" ReadOnly="true"></asp:TextBox>
                                <asp:button ID="btnNextStep" runat="server" Text="Perform Step" />
                                <asp:Label ID="lblNextStep" runat="server" Text="" />
                            </asp:TableCell></asp:TableRow><asp:TableRow ID="trNextStepDate" runat="server">
                            <asp:TableCell>Next Step Date:</asp:TableCell><asp:TableCell><asp:TextBox ID="txtNextStepDate" runat="server" ReadOnly="true"></asp:TextBox></asp:TableCell></asp:TableRow><asp:TableRow ID="trStatus" runat="server">
                            <asp:TableCell>Status:</asp:TableCell><asp:TableCell><uc1:Select_Item runat="server" ID="siBatchStatus" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="trUpdate" runat="server">
                            <asp:TableCell>&nbsp;</asp:TableCell><asp:TableCell><asp:Button runat="server" ID="btnUpdateBatch" Text="Update Batch" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="trStart" runat="server">
                            <asp:TableCell>&nbsp;</asp:TableCell><asp:TableCell><asp:Button runat="server" ID="btnStart" Text="Start Process" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="trActions" runat="server">
                            <asp:TableCell ColumnSpan="2">
                                <ul id="menu">
                                    <li><asp:LinkButton ID="LinkButton1" runat="server">Print Initial Letter</asp:LinkButton></li>
                                    <li><asp:LinkButton ID="LinkButton2" runat="server">Print Certified Letter</asp:LinkButton></li>
                                    <li><asp:LinkButton ID="LinkButton3" runat="server">Print Affidavit</asp:LinkButton></li>
                                </ul>
                            </asp:TableCell></asp:TableRow></asp:table></asp:View><asp:View ID="mvStepsViewContracts" runat="server">
                    <div class="ListGrid">
                        <asp:GridView ID="gvBatchContracts" runat="server" AutoGenerateColumns="true">
                            <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                            <AlternatingRowStyle BackColor="#CCFFCC" />  
                            <Columns>
                                <asp:TemplateField HeaderText="Remove">
                                    <ItemTemplate>
                                        <a href="#" title="Remove" onclick = "javascript:modal.mwindow.open('<%=Request.ApplicationPath%>/Wizards/Accounting/removebatchitem.aspx?bid=<%=hfBatchID.Value %>&id=<%#Container.DataItem("KCP")%>');">Remove</a></ItemTemplate></asp:TemplateField></Columns></asp:GridView></div><ul id="menu">
                        <li><asp:LinkButton ID="lbExcel" runat="server">Export to Excel</asp:LinkButton></li></ul></asp:View><asp:View ID="mvStepsViewAdd" runat="server">
                    <ul id="menu">
                        <li><asp:LinkButton ID="lbUpload" runat="server">Upload</asp:LinkButton></li><li><asp:LinkButton ID="lbIndividual" runat="server">Add Individual</asp:LinkButton></li></ul><asp:MultiView ID="mvAdd" runat="server">
                        <asp:View ID="vwUpload" runat="server">
                            <div>
                                <br />
                                <div>
                                    <asp:Label ID="lbFileUpload" runat="server" Text="File:"></asp:Label><asp:FileUpload ID="xlsUpload" runat="server" ToolTip="File" />
                                </div>
                                <br />
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" />
                            </div>
                            <div class="ListGrid"><asp:GridView ID="gvBatch" runat="server" AutoGenerateColumns="true"></asp:GridView></div>
                        </asp:View>
                        <asp:View ID="vwIndividual" runat="server">
                            Individual</asp:View></asp:MultiView></asp:View><asp:View ID="mvStepsViewEvents" runat="server">
                    <uc1:Events runat="server" ID="Events" />
                </asp:View>
            </asp:MultiView>
            <asp:HiddenField ID="hfBatchID" runat="server" Value="0" />
            <asp:HiddenField ID="hfTitle" runat="server" Value="Contracts" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lbExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

