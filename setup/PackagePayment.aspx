<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PackagePayment.aspx.vb" Inherits="setup_PackagePayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="../scripts/jquery-1.7.1.min.js"></script>

    <script type="text/javascript">


        $(function () {

            $("#wrapper td:first-child")
                .css({ 'font-weight': 'bold', 'width':'110px' });

            $("#wrapper input[type='text']").css({ 'font': '14px Verdana', 'letter-spacing': '-1px', 'text-align': 'right' });

            $("#DropDownListPaymentMethod").css({ 'font': '14px Verdana' });

            $("#wrapper input[type='checkbox']").css({ 'font-weight': 'bold' });
        });


    </script>

</head>
<body>
    <form id="form1" runat="server">

    <asp:HiddenField ID="HiddenFieldPackageID" runat="server" />
    <asp:HiddenField ID="HiddenFieldPackageTourPaymentID" runat="server" />
    <asp:HiddenField ID="HiddenFieldPackagePaymentID" runat="server" />
    <asp:HiddenField ID="HiddenFieldPackageReservationPaymentID" runat="server" />
    <asp:HiddenField ID="HiddenFieldPackageTourFinTransID" runat="server" />
    <asp:HiddenField ID="HiddenFieldPackageReservationFinTransID" runat="server" /> 
    <asp:HiddenField ID="HiddenFieldType" runat="server" />
    <asp:HiddenField ID="HiddenFieldBack" runat="server" />   
    
    <div id="wrapper">
        
        <div id="dv_top_input" style="width:400px">
            <fieldset>
                
            <table>
                <tr>
                    <td>Method</td>
                    <td><asp:DropDownList runat="server" ID="DropDownListPaymentMethod"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Amount $</td>
                    <td><asp:TextBox runat="server" ID="TextBoxFixedAmount"></asp:TextBox></td>
                </tr>
            </table>
            </fieldset>
        </div>
        <br />
        <br />
        <div style="width:400px">
            
            <br />
            <fieldset>     
                <br /> 
                <asp:CheckBox runat="server" ID="CheckBoxAdjustment" Text="Adjustment" />           
                <br />         <br />                   
                <asp:RadioButton runat="server" ID="RadioButtonPositive" GroupName="POSNEG" Text="Positive" />
                <asp:RadioButton runat="server" ID="RadioButtonNegative" GroupName="POSNEG" Text="Negative" />                              
                &nbsp;<br /><br />
            </fieldset>        
        </div>  
        
        <br />
        <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" />      
    </div>
    </form>
</body>
</html>
