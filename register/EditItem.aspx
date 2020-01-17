<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditItem.aspx.vb" Inherits="register_EditItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style type="text/css">

</style>


</asp:Content>









<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div id="wrapper" style="width:840px;left:0;">




<asp:MultiView ID="multiviewmain" runat="server">
    <asp:View ID="viewitemedit" runat="server">
    <br />
    <h1 style="font-size:26px;font-family:Californian FB;color:#43c6db">INVENTORY ITEMS</h1>
    <fieldset>
        <legend></legend>
            <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse;">
                <tr>
                    <td><label>UPC</label></td>
                    <td><asp:TextBox runat="server" ID="textboxUPC"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><label>Description</label></td>
                    <td><asp:TextBox runat="server" ID="textboxDescription"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><label>Name (Brand)</label></td>
                    <td><asp:DropDownList ID="dropdownlistBrand" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td><label>Quantity</label></td>
                    <td><asp:TextBox runat="server" ID="textboxQuantity" Width="50"></asp:TextBox><asp:DropDownList ID="dropdownlistUnit" runat="server"></asp:DropDownList></td>
                </tr>
            </table>  
            
            <div>
                <br />
                <hr style="color:gray;" />
                <br />
                <asp:Button runat="server" Text="Submit" ID="buttonItemSubmit" />
                <input type="button" value="Cancel" id="buttonItemCancel" />
            </div>              
        </fieldset>   
    </asp:View>
    <asp:View runat="server">
        <h1 style="font-size:26px;font-family:Californian FB;color:#43c6db">BRAND (COMPANY NAME)</h1>
        <br /><br />
        <fieldset>
            <table>
                <tr>
                    <td>Name</td>
                    <td><asp:TextBox runat="server" ID="textboxBrandName"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>               
            </table>  
            
            
            <div>
                <br />
                <hr style="color:gray;" />
                <br />
                <asp:Button runat="server" ID="buttonBrandSubmit" Text="Submit" />
                <input type="button" value="Cancel" id="buttonBrandCancel" />
            </div>      
        </fieldset>
    </asp:View>
    <asp:View runat="server">
        <h1 style="font-size:26px;font-family:Californian FB;color:#43c6db">MEASUREMENT UNIT</h1>
        <br /><br />
        <fieldset>
            <table>
                <tr>
                    <td>Name</td>
                    <td><asp:TextBox runat="server" ID="textboxMeasurementUnitName"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>               
            </table>  

            <div>
                <br />
                <hr style="color:gray;" />
                <br />
                <asp:Button runat="server" ID="buttonMeasureSubmit" Text="Submit" />
                <input type="button" value="Cancel" id="buttonMeasureCancel" />
            </div> 
        </fieldset>                
    </asp:View>
</asp:MultiView>





</div>







<script type="text/javascript">

    $(function () {

        $("#wrapper").css({ 'font-family': 'Cambria' });
        $("input").css({ 'font-family': 'Cambria', 'font-size': '18' });

        $('#wrapper table tr').height(40);

        $('#buttonItemCancel').click(function () { window.location.href = 'Default.aspx'; });
        $('#buttonBrandCancel').click(function () { window.location.href = 'Default.aspx'; });
        $('#buttonMeasureCancel').click(function () { window.location.href = 'Default.aspx'; });


        $('#wrapper tr').each(function (e) {
            $(this).find('td:eq(0)').css({ 'font-size': '16' + 'px', 'font-weight': 'bold' });
        });
    });


</script>

</asp:Content>

