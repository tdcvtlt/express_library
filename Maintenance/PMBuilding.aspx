<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PMBuilding.aspx.vb" Inherits="Maintenance_PMBuilding" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:HiddenField runat="server" ID="hd_pmbuildingID" Value="0" />

<ul id="menu">
    <li <% if mvMultiView.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="lnkBuilding" runat="server">buildings</asp:LinkButton>
    </li>    
    
    <li <% if mvMultiView.ActiveViewIndex = 1 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="lnkRoom" runat="server">rooms</asp:LinkButton>        
    </li>       
</ul>

<div>
    <asp:MultiView ID="mvMultiView" runat="server">
        <asp:View runat="server" ID="mvBuilding">
            <div style="float:none;">    
    <input type="button" value="Insert" id="insert" />
    <asp:GridView runat="server" ID="gv" AutoGenerateColumns="false" DataKeyNames="pmbuildingID">
        <Columns>
            <asp:TemplateField HeaderText="Units">
                <ItemTemplate>
                    <a href="#" id="linkpmbuilding" runat="server">Links</a>                                       
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>                    
                    <a href="#" id="editpmbuilding" runat="server">Edit</a>                    
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:BoundField DataField="Active" HeaderText="Active" />
        </Columns>
    </asp:GridView>
</div>        
        </asp:View>    
        <asp:View runat="server" ID="mvRoom">
            <div>
                <asp:GridView runat="server" ID="gvRoom" AutoGenerateColumns="false" DataKeyNames="RoomID" >
                <Columns>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="ButtonLink" CommandName="select" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RoomID") %>' OnClick="OnClickHandler"  Text="Select"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RoomNumber" HeaderText="Room Number" />
                </Columns>
                </asp:GridView>
            </div>
        </asp:View>
    </asp:MultiView>
</div>

<div id="popup" style="display:none;">
<table>
    <tr>
        <td><asp:Label runat="server" AssociatedControlID="tb_name" Text="Name"></asp:Label></td>
        <td><asp:TextBox runat="server" ID="tb_name"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="Label1" runat="server" AssociatedControlID="tb_desc" Text="Description"></asp:Label></td>
        <td><asp:TextBox runat="server" ID="tb_desc"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="Label2" runat="server" AssociatedControlID="cb_active" Text="Active"></asp:Label></td>
        <td><asp:CheckBox runat="server" ID="cb_active" /></td>
    </tr>
    <tfoot>
        <tr>
            <td><asp:Button ID="bt_submit" runat="server" Text="Submit" /></td>
            <td><input type="button" id="bt_cancel" value="Cancel" /></td>
        </tr>
    </tfoot>
</table>
</div>
<div id="links" style="display:none;">
    <div style="float:left;width:40%;background-color:#A0522D;">
        <h3 id="name-h3"></h3>
        <asp:UpdatePanel ID="updatePanel1" runat="server">
            <ContentTemplate> 
                <div id="divNavLeft" style="position:relative;overflow:auto;">                
                    <asp:CheckBoxList runat="server" ID="cblSource" ></asp:CheckBoxList>                                   
                </div>              
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="toUpdatePanel2" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnInit" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div style="float:left;width:20%; background-color:#6A5ACD;">
              
        <ul style="list-style-type:none;margin-top:120px;">
            <li><asp:Button runat="server" ID="toUpdatePanel2" Text=">>>" CssClass="button"  /></li>
            <li><asp:Button runat="server" ID="toUpdatePanel1" Text="<<<" CssClass="button"  /></li>
            <li><asp:Button runat="server" ID="btnSave" Text="Save" CssClass="button"  /></li>
            <li><asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="button" /></li>
        </ul>    
       <asp:Button runat="server" ID="btnInit" CssClass="hidden" />
    </div>
    <div style="float:left;width:40%;background-color:#A0522D;">
        <h3 id="description-h3"></h3>
         <asp:UpdatePanel ID="updatePanel2" runat="server">
            <ContentTemplate>
                <div id="divNavRight" style="position:relative;">   
                    <h3>
                        <asp:Label runat="server" ID="lblError" Text="" Visible="true"></asp:Label>                                    
                    </h3>
                    
                    <asp:CheckBoxList runat="server" ID="cblTarget"></asp:CheckBoxList>                                                     
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="toUpdatePanel1" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>        
    </div>      
</div>


</asp:Content>




<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style type="text/css">

.button
{
    width:70%;
    height:60px;
}

.hidden
{
    margin-left:-1000px;
    margin-top:-1000px;
}
    
</style>

<script type="text/javascript">

    function endRequest() {        
        var height = $('#links').height() - 55;
        var width = $('#links').width() * .4;
        $('#divNavLeft').css({ 'overflow': 'auto', 'height': height + 'px', 'width': width + 'px' });
        $('#divNavRight').css({ 'overflow': 'auto', 'height': height + 'px', 'width': width + 'px' });
    }
    $(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);
    });

    $(function () {

        $('#popup').hide();
        $('#bt_cancel').click(function () { $('#popup').fadeOut('fast'); });
        $('#insert').click(function () {
            showPopup();
            return false;
        });

        $('a[pmbuildingID]').click(function (e) {
            showPopup();
            $('#<%= hd_pmbuildingID.ClientID %>').val($(this).attr('pmbuildingID'));
            $('#<%= tb_name.ClientID %>').val($(this).attr('name'));
            $('#<%= tb_desc.ClientID %>').val($(this).attr('description'));
            $('#<%= cb_active.ClientID %>').prop('checked', ($(this).attr('active') == 'True' ? true : false));
            e.preventDefault();
        });

        function showPopup() {
            $('#<%= hd_pmbuildingID.ClientID %>').val('0');
            $('#<%= tb_name.ClientID %>').val('');
            $('#<%= tb_desc.ClientID %>').val('');
            $('#<%= cb_active.ClientID %>').prop('checked', false);

            var $pop = $('#popup');
            $pop.css({ 'position': 'absolute', 'top': '50%', 'left': '50%', 'z-index': '99', 'width': '500' + 'px',
                'height': '300' + 'px', 'border': '1px solid #FF6347', 'background-color': '#A0522D', 'color': 'white'
            });

            var left = $pop.width() / 2;
            var top = $pop.height() / 2;
            $pop.css({ 'margin-left': -left, 'margin-top': -top, 'overflow': 'auto' });
            $pop.fadeIn('fast');
        }
    });

    $(function () {

        function showLinks() {
        
            var $pop = $('#links');
            $pop.css({ 'position': 'absolute', 'top': '50%', 'left': '50%', 'z-index': '99', 'width': '900' + 'px',
                'height': '500' + 'px', 'border': '1px solid #FF6347', 'background-color': '#6A5ACD', 'color': 'white'
            });

            var left = $pop.width() / 2;
            var top = $pop.height() / 2;
            $pop.css({ 'margin-left': -left, 'margin-top': -top, 'overflow': 'hidden' });
            $pop.fadeIn('fast');
            
            var height = $('#links').height() - 55;
            var width = $('#links').width() * .4;
            $('#divNavLeft').css({ 'overflow': 'auto', 'height': height + 'px', 'width': width + 'px' });
            $('#divNavRight').css({ 'overflow': 'auto', 'height': height + 'px', 'width': width + 'px' });
        }

        $('#show').click(function () {
            showLinks();
            return false;
        });

        $('a[linkpmbuilding]').click(function (e) {
            $('#<%= hd_pmbuildingID.ClientID %>').val($(this).attr('linkpmbuilding'));
            $('#<%= btnInit.ClientID %>').trigger('click');
            $('#name-h3').html($(this).attr('name'));
            $('#description-h3').html($(this).attr('description'));
            
            showLinks();
            e.preventDefault();
        });
    });
    

</script>
</asp:Content>

