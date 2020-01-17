<%@ Page Title="Insert/Edit By Phone" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="true" EnableViewState="true" CodeFile="CheckByPhone.aspx.vb" Inherits="Add_Ins_CheckByPhone" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">



<div id='container' style="width:650px;border:0px solid red;font-family:Cambria;font-size:14px;margin-left:100px;">
<h1 runat="server" id="h1Header" style='border:0px solid red;color:Silver;text-align:center;text-decoration:underline;'></h1>

<div id='leftpane' style='width:45%;float:left' >

<div>

<fieldset>
<legend>Bank Account</legend>
<table>
<tr>
    <td style="width:150px">First Name</td>
    <td><asp:TextBox runat="server"  Width="" ID="AccountFirstName" /></td>
</tr>
<tr>
    <td>Middle Initial</td>
    <td><asp:TextBox ID="AccountMiddleInit" runat="server"  Width="" /></td>
</tr>
<tr>
    <td>Last Name</td>
    <td><asp:TextBox ID="AccountLastName" runat="server"  Width="" /></td>
</tr>
<tr>
    <td>Routing #</td>
    <td><asp:TextBox ID="RoutingNumber" runat="server"  Width="" /></td>
</tr>
<tr>
    <td>Account #</td>
    <td><asp:TextBox ID="AccountNumber" runat="server"  Width="" /></td>
</tr>
<tr>
    <td>&nbsp;</td>
    <td><asp:CheckBox runat="server" ID="CheckingFlag" Text="Checking" />
    <asp:CheckBox runat="server" ID="SavingsFlag" Text="Saving" />
    </td>
    
</tr>
</table>

</fieldset>

</div>



<br /><br />

<!-- Div after Account Information -->
<div>

<fieldset>
<legend>KCP Account</legend>

<table>
<tr>
    <td style="width:150px">Contract #</td>
    <td><asp:TextBox runat="server" Width="" ID="ContractNumber" /></td>
</tr>
<tr>
    <td>Amount $</td>
    <td><asp:TextBox runat="server" Width="" ID="Amount" value = "0" /></td>
</tr>
<tr>
    <td>Date To Run</td>
    <td><asp:TextBox runat="server" Width="" ID="DateToRun" /></td>
</tr>
<tr>
    <td>Status</td>
    <td><asp:DropDownList ID="StatusID" runat="server" /></td>
</tr>

</table>

</fieldset>

<br />
<br />
<br />

<!-- Submit Button -->
<div>
<span><asp:HyperLink ID="hlBack" runat="server" Text="Back" />&nbsp;&nbsp;&nbsp;</span>

<span><asp:Button ID="cmSubmit" runat="server" Text="Submit" Width="80px" /></span>

</div>

</div>




</div>


<div id='rightpane' style='width:39.5%;float:right;border:0px solid black;margin-right:90px;'>


<table>
<tr>
    <td>
        Date Completed
    </td>
    <td>
        <asp:TextBox ID="DateCompleted" runat="server" Width="135px"  />       
    </td>
</tr>
<tr>
    <td>        
        Transaction ID
    </td>
    <td>
        <asp:TextBox runat="server" ID="TransactionID" Width="135px" />
    </td>
</tr>
</table>

</div>
</div>






<asp:HiddenField ID="hfKey" runat="server" />


<script type="text/javascript">

    $(function () {
    /*
        $("input:text[valueXYZ!=]").each(function () {
            var tmp = $(this).attr("id");            
            if (tmp.indexOf("TransactionID") == -1 || tmp.indexOf("Amount") == -1) {
                $(this).attr("readonly", true);               
            }
        });
        */
    });


</script>

<script type="text/javascript" src="../scripts/jquery-1.7.1.js"></script>
<script type="text/javascript" src="../scripts/scw.js"></script>
</asp:Content>

