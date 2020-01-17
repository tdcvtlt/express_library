<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="editworkorder.aspx.vb" Inherits="mis_editworkorder" title="Work Order" %>
<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc2" %>
<%@ Register src="../../controls/Notes.ascx" tagname="Notes" tagprefix="uc3" %>
<%@ Register src="../../controls/Events.ascx" tagname="Events" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="lbWorkOrder" runat="server">Work Order</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="lbNotes" runat="server">Notes</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="lbEvents" runat="server">Events</asp:LinkButton></li>
    </ul>
    
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <asp:Table runat="server" id = "Table1">
                <asp:TableRow ID="TableRow2" runat="server">
                    <asp:TableCell ID="TableCell3" runat="server">Work Order ID:</asp:TableCell>
                    <asp:TableCell ID="TableCell4" runat="server"><asp:label id="lblID" runat = "server" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">Requested By:</asp:TableCell><asp:TableCell runat="server"><asp:Label runat="server" id = "lblRequestedBy"></asp:Label></asp:TableCell></asp:TableRow><asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">Date:</asp:TableCell><asp:TableCell runat="server"><asp:Label runat="server" id = "lblDateRequested"></asp:Label></asp:TableCell></asp:TableRow><asp:TableRow ID="TableRow1" runat="server">
                    <asp:TableCell ID="TableCell1" runat="server">Location:</asp:TableCell><asp:TableCell ID="TableCell2" runat="server"><uc1:Select_Item ID="Location" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">Sub Location:</asp:TableCell><asp:TableCell runat="server"><uc1:Select_Item ID="SubLocation" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">Department:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList runat="server" id = "ddDepartment"></asp:DropDownList></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Priority Level</asp:TableCell><asp:TableCell runat="server"><uc1:Select_Item ID="PriorityLevel" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">Request Type:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList runat="server" id = "ddType" onSelectedIndexChange = "ddType_SelectedIndexChanged" autopostback = "true"></asp:DropDownList></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Subject:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtSubject" runat="server"></asp:TextBox></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Requested Due Date:</asp:TableCell><asp:TableCell runat="server"><uc2:DateField ID="dteReqDueDate" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">Responsible Party:</asp:TableCell><asp:TableCell runat="server"><uc1:Select_Item ID="siResponsibleParty" runat="server" /></asp:TableCell>                    
                </asp:TableRow>
                <asp:TableRow runat="server"  visible = "false">
                    <asp:TableCell runat="server" colspan = '2'><hr /></asp:TableCell>                   
                </asp:TableRow>
                <asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">Status:</asp:TableCell><asp:TableCell runat="server"><uc1:Select_Item ID="Status" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">Assigned To:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList runat="server" id = "ddAssignedTo"></asp:DropDownList></asp:TableCell></asp:TableRow><asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">Assigned Due Date:</asp:TableCell><asp:TableCell runat="server"><uc2:DateField ID="dteAssignDueDate" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" colspan = '2'><hr /></asp:TableCell>                   
                </asp:TableRow>
                <asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">Old Value:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtOldValue"></asp:TextBox></asp:TableCell></asp:TableRow><asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">New Value:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtNewValue"></asp:TextBox></asp:TableCell></asp:TableRow><asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">Installation Type:</asp:TableCell><asp:TableCell runat="server"><uc1:Select_Item ID="siInstallationType" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">Estimated Cost:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtEstCost">0</asp:TextBox></asp:TableCell></asp:TableRow><asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">Last Name:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtLName"></asp:TextBox></asp:TableCell></asp:TableRow><asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">First Name:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtFName"></asp:TextBox></asp:TableCell></asp:TableRow><asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">Supervisor:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList runat="server" id = "ddSupervisor"></asp:DropDownList></asp:TableCell></asp:TableRow><asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">Problem Area:</asp:TableCell><asp:TableCell runat="server"><uc1:Select_Item ID="siProblemArea" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" visible = "false">
                    <asp:TableCell runat="server">Phone Number/Extension:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtPhone"></asp:TextBox></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server"><asp:Label runat="server" id = "lblDesc">Description:</asp:Label></asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtDescription" runat="server" Rows="10" TextMode="MultiLine" Height="88px" Width="207px"></asp:TextBox></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server"><asp:Button runat="server" id = "btnSave" Text="Save" onclick="btnSave_Click"></asp:Button><asp:Button runat="server" id = "btnApprove" Text="Approve" visible = "false" onclick="btnApprove_Click"></asp:Button><asp:Button runat="server" Text="Deny" id = "btnDeny" visible = "false" onclick="btnDeny_Click"></asp:Button></asp:TableCell></asp:TableRow></asp:Table></asp:View><asp:View ID="View2" runat="server">
            <uc3:Notes ID="Notes1" runat="server" KeyField = "WorkOrderID"/>
        </asp:View>
        <asp:View ID="View3" runat="server">
            <uc4:Events ID="ucEvents" runat="server" />
        </asp:View>
    </asp:MultiView>


    <asp:HyperLink runat="server" Text="Back" ID="Back"></asp:HyperLink><br /><br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label></asp:Content>