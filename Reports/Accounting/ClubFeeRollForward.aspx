<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ClubFeeRollForward.aspx.vb" Inherits="Reports_Accounting_ClubFeeRollForward" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
.myCheckBoxList{color:Black;}
#Main
{
    font-family:Cambria;
    font-size:13px;
}

#button1
{
    font-family:Cambria;
    font-size:16px;
}

</style>

<script type="text/javascript">

    $(function () {     
    });
   
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="Main" style="left:200px;position:absolute;">
   <h1>Club Fee Roll Forward</h1>

   <div style="width:200px;left:0px;float:left;">
        <asp:CheckBoxList ID="CheckBoxList1" runat="server" CssClass="myCheckBoxList" AutoPostBack="false" >                      
        </asp:CheckBoxList>   
   </div>
   

   <div style="width:800px;left:220px;top:0px;">
   <div style="width:300px;float:left;">
   <h4>Invoices</h4>
    <table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dteSDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dteEDate" runat="server" />
            </td>
        </tr>
    </table>   
   </div>
   
   <div>
    <h4>Payments</h4>
    <table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="DateField1" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="DateField2" runat="server" />
            </td>
        </tr>
    </table>   
   </div>
    
    <div style="clear:left;">
        <asp:Label ID="label1" runat="server"></asp:Label>    
    </div>
    
   </div>

   <div style="clear:left;">
        <asp:Button ID="Button1" runat="server" Text="Submit" Width="100" Height="40" />
        &nbsp;
        <asp:Button ID="Button2" runat="server" Text="Export To Excel" Width="140" Height="40" />
   </div>
    
    
    <div>
    <br />
    <asp:Literal runat="server" ID="literal1"></asp:Literal>
    </div>



</div>



<script type="text/javascript">

   

    });
</script>

</asp:Content>

