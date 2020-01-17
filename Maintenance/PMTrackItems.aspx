<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PMTrackItems.aspx.vb" Inherits="Maintenance_PMTrackItems" %>




<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">



<ul class="ul-none">
    <li><label for="<%= ddlCategory.ClientID %>">Category</label></li>
    <li><asp:DropDownList runat="server" ID="ddlCategory"></asp:DropDownList><input type="text" size="40" /><input type="button" value="Add" id="save-category" /></li>
    <li><asp:Button runat="server" ID="submit" Text="Submit" /></li>
    <li></li>
    <li>
        <br />
        <asp:GridView runat="server" ID="gvGridView" AutoGenerateColumns="False" ShowFooter="true" GridLines="Vertical"
            EnableModelValidation="True" DataKeyNames="ID" >
            <Columns>                
                <asp:TemplateField>
                    <ItemTemplate>                        
                        <asp:TextBox runat="server" ID="tbx1" Width="400"></asp:TextBox>
                        <asp:DropDownList runat="server" ID="ddl1"></asp:DropDownList>   
                        <asp:Button runat="server" ID="btn1" Text="Save" CommandName="SaveChanges" />                           
                        <asp:Button runat="server" ID="btn2" Text="List" CommandName="List" />                                                    
                    </ItemTemplate>
                    <FooterTemplate>                        
                        <asp:TextBox runat="server" ID="tbx2" Width="400"></asp:TextBox>
                        <asp:DropDownList runat="server" ID="ddl2"></asp:DropDownList>
                        <asp:Button runat="server" ID="btn3" Text="Save" CommandName="SaveNew" /> 
                    </FooterTemplate>
                </asp:TemplateField>                                                                
            </Columns> 
                               
        </asp:GridView>
    </li>
</ul>


    

<script type="text/javascript">
    $(function () {

        $('#<%= ddlCategory.ClientID %> li').not(':first').hide();
        $('.ul-none li:eq(1)').find('input:first').hide();

        $('#save-category').click(function () {

            var $textBox = $('.ul-none li:eq(1)').find('input:first');
            var $btn = $(this);
            $textBox.show();
            $btn.hide();

            var $cancelButton = $('<input type=button value=Cancel></input>');
            var $saveButton = $('<input type=button value=Save></input>');
            var $error = $('<span></span>');

            $error.insertAfter($cancelButton.insertAfter($saveButton.insertAfter($(this))));

            $('#<%= ddlCategory.ClientID %>').attr('disabled', true);


            $cancelButton.on('click', function () {
                $textBox.hide();
                $saveButton.remove()
                $btn.show();
                $(this).remove();

                $error.remove();
                $textBox.val('');
                $('#<%= ddlCategory.ClientID %>').attr('disabled', false);
            });

            $saveButton.on('click', function () {

                if ($textBox.val().length > 0) {

                    var $options = $('.ul-none li:eq(1) option');
                    var found = false;

                    $.each($options, function (index, option) {

                        if (found == false) {
                            if ($(option).text() == $textBox.val()) {

                                $error.html('duplicate');
                                found = true;

                            } else {

                                $error.html('good work');

                            }
                        }

                    });

                }
                else {
                    $textBox.hide();
                    $cancelButton.remove();
                    $btn.show();
                    $(this).remove();
                    $error.remove();
                    $('#<%= ddlCategory.ClientID %>').attr('disabled', false);
                }

            });

        });


    });
</script>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style type="text/css">

.ul-none
{
    list-style-type:none;
}
    
    
</style>



</asp:Content>
