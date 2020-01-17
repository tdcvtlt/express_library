<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PackageLookup.aspx.vb" Inherits="setup_PackageLookup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style type="text/css">

#ctl00_ContentPlaceHolder1_GridViewPackages
{
    font-family:Cambria;
    font-size:18px;
}
    
</style>




</asp:Content>





<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div id="dv_wrapper" style="width:100%;border:0px solid red;">

<h1>Packages List</h1>


    <asp:LinkButton ID="LinkButton1" runat="server">New Package</asp:LinkButton>


<br />


</div>

<div id="">
   <asp:GridView ID="GridViewPackages" runat="server" AutoGenerateColumns="false" 
        EmptyDataText="" AutoGenerateSelectButton="false"
        gridlines="Horizontal" DataKeyNames="PackageId" PageSize="22">
        <AlternatingRowStyle BackColor="#CCFFCC" />    
        <Columns>
            <asp:HyperLinkField DataTextField="Package" DataNavigateUrlFields="PackageID" DataNavigateUrlFormatString="EditPackage.aspx?PackageID={0}" HeaderText="Package Name" HeaderStyle-Width="200" />                        
            <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="250"/>           
            <asp:BoundField DataField="Accommodation" HeaderText="Accommodation" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="250"/>     
        </Columns>
    </asp:GridView>       
</div>

<div>
<p id="p_error" runat="server"></p>

</div>


<script type="text/javascript">

    $(function () {

        $("#ctl00_ContentPlaceHolder1_TextBoxPackage").click("click", function () {
            if (this.value == "Enter package name or the ID") {
                this.value = "";
            }
        });

        $("#ctl00_ContentPlaceHolder1_TextBoxPackage").blur(function () {
            if (this.value == "") {
                this.value = "Enter package name or the ID";
            }
        });


        $("#dv_packages_result > #ctl00_ContentPlaceHolder1_p_error").css("color", "red");



        //$("table tr td:nth-child(1)").css({ 'width': '120' });


        $("tr:first").children("td").css({ 'font-weight': '60' });



    });
</script>
</asp:Content>

