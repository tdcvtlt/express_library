<%@ Page Title="Assessment Wizard" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="AssessmentWizard.aspx.vb" Inherits="wizards_Accounting_AssessmentWizard" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" 
        AutoPostBack="True">
        <asp:ListItem Value="MF">Maintenance Fees</asp:ListItem>
        <asp:ListItem Value="LF">Late Fees</asp:ListItem>
        <asp:ListItem Value="CD">Club Explore Dues</asp:ListItem>
        <asp:ListItem Value="CFLF">Club Explore Dues Late Fees</asp:ListItem>
    </asp:RadioButtonList>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            
        </asp:View>
        <asp:View ID="vwMFWiz" runat="server">
            <table>
                <tr>
                    <td>Year to Assess:</td>
                    <td><asp:DropDownList ID="ddYear" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Cut-Off Date:</td>
                    <td><uc1:DateField ID="dfCuttOff" runat="server" /></td>
                </tr>
                <tr>
                    <td>Due Date:</td>
                    <td><uc1:DateField ID="dfDueDate" runat="server" /></td>
                </tr>
            </table>
            Please Choose Contract Status(es) to Assess:
            <table>
                <tr>
                    <td>
                        <asp:ListBox ID="lbChoices" runat="server"></asp:ListBox>
                    </td>
                    <td>
                        <asp:Button ID="btnRight" runat="server" Text=">>" /><br />
                        <asp:Button ID="btnLeft" runat="server" Text="<<" />
                    </td>
                    <td>
                        <asp:ListBox ID="lbSelected" runat="server"></asp:ListBox>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnPreview" runat="server" Text="Preview" />
            <asp:Button ID="btnExport" runat="server" Text="Export" />
            <asp:Button ID="btnAssess" runat="server" Text="Assess / Print" />
            
        </asp:View>
        <asp:View ID="vwLFWiz" runat="server">
            <table>
                <tr>
                    <td>Year to Assess:</td>
                    <td><asp:DropDownList ID="ddLFYear" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Late Fee Amount:</td>
                    <td>
                        <asp:TextBox ID="txtLFAmount" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Days Late:</td>
                    <td>
                        <asp:TextBox ID="txtDays" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnLFPreview" runat="server" Text="Preview" />
            <asp:Button ID="btnLFExport" runat="server" Text="Export" />
            <asp:Button ID="btnLFAssess" runat="server" Text="Assess / Print" />
        </asp:View>


        <!-- //////////////////////////////////////////////////////  -->
        <!-- Club Explorer Annual Dues / Late Dues -->
        <asp:View ID = "CEWiz" runat ="server">
            <table>
                <tr>
                    <td>Year to Assess:</td>
                    <td><asp:DropDownList ID="ClubFees" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Cut-Off Date:</td>
                    <td>
                        <uc1:DateField ID="AssessDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Due Date:</td>
                    <td>
                        <uc1:DateField ID="DueDate" runat="server" />
                    </td>
                </tr>
            </table>
            Please Choose Contract Status(es) to Assess:
            <table>
                <tr>
                    <td>
                        <asp:ListBox ID="lbCESelect" runat="server"></asp:ListBox>
                    </td>
                    <td>
                        <asp:Button ID="btnAdd" runat="server" Text=">>" /><br />
                        <asp:Button ID="btnRemove" runat="server" Text="<<" />
                    </td>
                    <td>
                        <asp:ListBox ID="lbCEStatuses" runat="server"></asp:ListBox>
                    </td>
                </tr>
            </table>


            <asp:Button ID="Preview" runat="server" Text="Preview" />
            <asp:Button ID="Export" runat="server" Text="Export" />
            <asp:Button ID="Assess" runat="server" Text="Assess" />  
            <asp:Button ID="btnPrint" runat="server" Text="Print Last Assessment" />  
              
            

        
        </asp:View>


        <!-- End of Club Explorer View -->

        <asp:View  ID="vwCFLFWiz" runat="server">
                 <table>
                <tr>
                    <td>Year to Assess:</td>
                    <td><asp:DropDownList ID="ddClubFee" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Late Fee Amount:</td>
                    <td>
                        <asp:TextBox ID="txtCDLFAMt" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Days Late:</td>
                    <td>
                        <asp:TextBox ID="txtDaysLate" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnLFCFPreview" runat="server" Text="Preview" />
            <asp:Button ID="btnLFCFExport" runat="server" Text="Export" />
            <asp:Button ID="btnLFCFAssess" runat="server" Text="Assess" />   
            <asp:Button ID="btnPrintLF" runat="server" Text="Print Last Assessment" />  
        </asp:View>

    </asp:MultiView><br />
    <CR:CrystalReportViewer ID="CFViewer" runat="server" AutoDataBind="true" Visible="true" />  
    <table>
    <tr>
        <td>Print?</td>
        <td><asp:CheckBox ID="ckPrint" runat="server" Checked="true" /></td>
    </tr>
    <tr>
        <td>Printer:</td>
        <td>
            <asp:DropDownList ID="ddPrinter" runat="server">
                <asp:ListItem Selected="True" Value="\\RS-PS-01\Finance Xerox 5855">Accounting_Xerox</asp:ListItem>
                
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td><asp:label runat="server" ID="lblTemplate" visible = "false" Text="Template:"></asp:label></td>
        <td>
            <asp:DropDownList ID="ddTemplate" runat="server" Visible ="false">
            </asp:DropDownList>
        </td>
    </tr>
    </table>
    <asp:Label runat="server" ID="lblCheck" Text="Check/Uncheck All" Visible="false"></asp:Label> 
    <asp:CheckBox ID="cbSelect" Checked="true" runat="server" AutoPostBack="True" Visible="false" />
    <asp:GridView ID="gvResults" runat="server" EnableModelValidation="True" 
        AutoGenerateColumns="False">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="cb" runat="server" Checked="True" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Last Name" HeaderText="Last Name" />
            <asp:BoundField DataField="First Name" HeaderText="First Name" />
            <asp:BoundField DataField="KCP#" HeaderText="KCP #" />
            <asp:BoundField DataField="MFAmount" DataFormatString="{0:c}" 
                HeaderText="MF Amount">
            <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="Previously Assessed" DataFormatString="{0:c}" 
                HeaderText="Previously Assessed">
            <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="To Be Assessed" DataFormatString="{0:c}" 
                HeaderText="To Be Assessed">
            <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="Type" HeaderText="Type" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="SubStatus" HeaderText="Sub Status" />
            <asp:BoundField DataField="MFStatus" HeaderText="MF Status" />
            <asp:BoundField DataField="contractid" HeaderText="ContractID" />
        </Columns>
        
    </asp:GridView>
    <asp:GridView ID="gvLF" runat="server" EnableModelValidation="True" 
        AutoGenerateColumns="False">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="cb" runat="server" Checked="True" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Last Name" HeaderText="Last Name" />
            <asp:BoundField DataField="First Name" HeaderText="First Name" />
            <asp:BoundField DataField="KCP#" HeaderText="KCP #" />
            <asp:BoundField DataField="TransCode" HeaderText="Trans Code" />
            <asp:BoundField DataField="AmountInvoiced" DataFormatString="{0:c}" 
                HeaderText="Amount Invoiced">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>            
            <asp:BoundField DataField="Balance" DataFormatString="{0:c}" 
                HeaderText="Balance">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="Type" HeaderText="Phase" />
            <asp:BoundField DataField="LFAmount" DataFormatString="{0:c}" 
                HeaderText="Late Fee">
            <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            
            
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="substatus" HeaderText="Sub Status" />
            <asp:BoundField DataField="MFStatus" HeaderText="MF Status" />
            <asp:BoundField DataField="contractid" HeaderText="ContractID" />
            
            
        </Columns>
        
    </asp:GridView>

    <asp:GridView ID="gvCDLF" runat="server" EnableModelValidation="True" 
        AutoGenerateColumns="False">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="cb" runat="server" Checked="True" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ProspectID" HeaderText = "ProspectID" />
            <asp:BoundField DataField="Last Name" HeaderText="Last Name" />
            <asp:BoundField DataField="First Name" HeaderText="First Name" />
            <asp:BoundField DataField="TransCode" HeaderText="Trans Code" />
            <asp:BoundField DataField="AmountInvoiced" DataFormatString="{0:c}" 
                HeaderText="Amount Invoiced">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>            
            <asp:BoundField DataField="Balance" DataFormatString="{0:c}" 
                HeaderText="Balance">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="LFAmount" DataFormatString="{0:c}" 
                HeaderText="Late Fee">
            <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
         
        </Columns>
        
    </asp:GridView>
                    <asp:GridView ID="gvDues" runat="server" EnableModelValidation="True" 
                    AutoGenerateColumns="False" OnRowDataBound = "gvDues_RowDataBound" EmptyDataText = "No Records">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="cb" runat="server" Checked="True" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProspectID" HeaderText="ProspectID" />
                        <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                        <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                        <asp:BoundField DataField="AnniversaryDate" HeaderText="Anniversary Date" />
                        <asp:BoundField DataField="Frequency" HeaderText = "Frequency" />
                        <asp:BoundField DataField="AmountInvoiced" DataFormatString="{0:c}" 
                            HeaderText="Previously Assessed">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>            
                        <asp:BoundField DataField="AssessmentAmount" 
                            HeaderText="Club Fee Amount">
                        </asp:BoundField>
                        <asp:BoundField DataField="AmountToAssess" HeaderText="To Be Assessed" />     
                        <asp:BoundField DataField="MostRecentContractStatus" HeaderText = "Most Recent Status" />                   
                    </Columns>
        
                </asp:GridView>    
                  
</asp:Content>

