<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditRawItem.aspx.vb" Inherits="register_EditRawItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style type="text/css">

</style>


</asp:Content>









<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div id="wrapper" style="width:840px;left:0;">

<ul id="menu">
    <li <% If multiviewmain.ActiveViewIndex = 0 Then : Response.Write(String.Format("class='{0}'", "current")) : End If%>>
            <asp:LinkButton ID="linkbuttonItemBack" runat="server" PostBackUrl="~/register/Default.aspx">default page</asp:LinkButton>
    </li> 
</ul>


<asp:MultiView ID="multiviewmain" runat="server">
    <asp:View ID="viewitemedit" runat="server">
    <br />
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
<td><label>Type (Unit)</label></td>
<td><asp:DropDownList ID="dropdownlistUnit" runat="server"></asp:DropDownList></td>
</tr>


</table>    
    </fieldset>
    

    </asp:View>
</asp:MultiView>


<div>
<br />
<asp:Button runat="server" Text="Submit" ID="buttonSubmit" />
<input type="button" value="Cancel" id="buttonCancel" />
</div>


</div>







<script type="text/javascript">

    $(function () {

        $("#wrapper").css({ 'font-family': 'Cambria' });
        $("input").css({ 'font-family': 'Cambria', 'font-size': '18' });

        $('#wrapper table tr').height(40);
        $('#buttonCancel').click(function () { window.location.href = 'Default.aspx'; });

        $('#wrapper tr').each(function (e) {
            $(this).find('td:eq(0)').css({ 'font-size': '16' + 'px', 'font-weight': 'bold' });
        });
    });


</script>

</asp:Content>

