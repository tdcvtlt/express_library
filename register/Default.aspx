<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="register_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style type="text/css">


#warning
{
    width:840px;
    height:40px;
    color:White;
    border:0px solid gray;
    position:relative;
    font-family:Cambria;
    font-size:18px;
    padding-left:0px;
    padding-top:10px;
    background:#ccc;
    visibility:visible;
}   

#warning img
{
    position:absolute;
    top:8px;
    left:5px;
} 

#warning span
{
    position:absolute;
    margin-left:50px;
    margin-top:10px;
    
}


li a 
{
    font-family:DFKai-SB;
    color:Lime;
    font-size:16px;
}


.dummyarrow
{
    pointer:cursor;
}

</style>
</asp:Content>






<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">




<div id="wrapper" style="width:840px;left:0;height:800px;">


    <ul id="menu">
        <li <% If multiviewmain.ActiveViewIndex = 0 Then : Response.Write(String.Format("class='{0}'", "current")) : End If%>>
            <asp:LinkButton ID="linkbuttonItemsList" runat="server">Inventory Items</asp:LinkButton>
        </li>   
        <li <% If multiviewmain.ActiveViewIndex = 1 Then : Response.Write(String.Format("class='{0}'", "current")) : End If%>>
            <asp:LinkButton ID="linkbuttonBrandList" runat="server">Brand</asp:LinkButton>
        </li> 
        <li <% If multiviewmain.ActiveViewIndex = 2 Then : Response.Write(String.Format("class='{0}'", "current")) : End If%>>
            <asp:LinkButton ID="linkbuttonMeasurementList" runat="server">Measurement</asp:LinkButton>
        </li>  
        <li <% If multiviewmain.ActiveViewIndex = 3 Then : Response.Write(String.Format("class='{0}'", "current")) : End If%>>
            <asp:LinkButton ID="linkbuttonMenuItem2ItemList" runat="server">Menu -> Items</asp:LinkButton>
        </li>                             
    </ul>
        
    <br />    

    <asp:MultiView ID="multiviewmain" runat="server">
        <asp:View ID="viewItemsList" runat="server">
            <asp:GridView   ID="gridviewItems" runat="server" 
                    AutoGenerateColumns="false" 
                    EmptyDataText="" 
                    AutoGenerateSelectButton="false" ShowHeader="true" ShowFooter="true"
                    gridlines="Horizontal" 
                    DataKeyNames="ItemID" 
                    AllowPaging="true" 
                    PageSize="25">
                <AlternatingRowStyle BackColor="#CCFFCC" />    
                <Columns>
                    <asp:BoundField DataField="SUPC" 
                                    HeaderText="SUPC" 
                                    HeaderStyle-HorizontalAlign="Left" 
                                    HeaderStyle-Width="100"/>   
                                        
                    <asp:HyperLinkField DataTextField="Item" 
                                        DataNavigateUrlFields="ItemID" 
                                        DataNavigateUrlFormatString="EditItem.aspx?View=Items&ItemID={0}" 
                                        HeaderText="Description" 
                                        HeaderStyle-HorizontalAlign="Left" 
                                        HeaderStyle-Width="540" />
                                                                                               
                    <asp:BoundField DataField="Brand" 
                                    HeaderText="Brand" 
                                    HeaderStyle-HorizontalAlign="Left" 
                                    HeaderStyle-Width="125"/>    
                            
                    <asp:BoundField DataField="Type" 
                                    HeaderText="Type" 
                                    HeaderStyle-HorizontalAlign="Left" 
                                    HeaderStyle-Width="75"/>    
                                                         
                </Columns>
            </asp:GridView>

            <br /><br />
            <div>
                <ul id="menu">
                    <li>
                        <asp:LinkButton ID="linkbuttonItemsAdd" runat="server">Add</asp:LinkButton>
                    </li>  
                </ul>                                   
            </div>
        </asp:View>  
        
        
        
        
        
        
        <asp:View ID="viewBrandList" runat="server">
            <asp:GridView ID="gridviewBrandList" runat="server"
                AutoGenerateColumns="false" 
                EmptyDataText="" 
                AutoGenerateSelectButton="false" ShowHeader="true" ShowFooter="false"
                gridlines="Horizontal" 
                DataKeyNames="BrandID" 
                AllowPaging="true" 
                PageSize="25">
                <AlternatingRowStyle BackColor="#CCFFCC" />   
                <Columns>
                <asp:HyperLinkField DataTextField="Name" 
                                DataNavigateUrlFields="BrandID" 
                                DataNavigateUrlFormatString="EditItem.aspx?View=Brand&BrandID={0}" 
                                HeaderText="Name" 
                                HeaderStyle-HorizontalAlign="Left" 
                                HeaderStyle-Width="840" />
                </Columns>
            </asp:GridView>

            <br /><br />
            <div>
                <ul id="menu">
                    <li>
                        <asp:LinkButton ID="linkbuttonBrandListAdd" runat="server">Add</asp:LinkButton>
                    </li>  
                </ul>                                   
            </div>
        </asp:View>
        
        <asp:View ID="viewMeasurementList" runat="server">
            <asp:GridView ID="gridviewMeasurementList" runat="server"
                AutoGenerateColumns="false" 
                EmptyDataText="" 
                AutoGenerateSelectButton="false" ShowHeader="true" ShowFooter="false"
                gridlines="Horizontal" 
                DataKeyNames="MeasurementUnitID" 
                AllowPaging="true" 
                PageSize="25">
                <AlternatingRowStyle BackColor="#CCFFCC" />   
                <Columns>
                <asp:HyperLinkField DataTextField="Name" 
                                DataNavigateUrlFields="MeasurementUnitID" 
                                DataNavigateUrlFormatString="EditItem.aspx?View=Unit&MeasurementUnitID={0}" 
                                HeaderText="Name" 
                                HeaderStyle-HorizontalAlign="Left" 
                                HeaderStyle-Width="840" />
                </Columns>
            </asp:GridView> 
            
            <br /><br />
            <div>
                <ul id="menu">
                    <li>
                        <asp:LinkButton ID="linkbuttonMeasurementListAdd" runat="server">Add</asp:LinkButton>
                    </li>  
                </ul>                                   
            </div>
            
        </asp:View>  
        
        <asp:View ID="viewMenuItem2ItemList" runat="server">

        <div id="divMenuItem2ItemList" runat="server"></div>

        </asp:View>    
        
        
        
                    
    </asp:MultiView>
    
  
    
</div>








<script type="text/javascript">

    $(function () {

        $('#wrapper ctl00_ContentPlaceHolder1_gridviewItems tr').each(function (e) {
            $(this).find('td:eq(0)').css({ 'font-size': '16' + 'px', 'font-weight': 'bold', 'color': 'red' });
            $(this).find('td:eq(3)').css({ 'font-size': '16' + 'px', 'color': 'blue' });
        });

        $('.dummyarrow').css({ 'cursor': 'pointer' });
        $('.dummyarrow').bind('click', function () {

            var $r = $(this).parent('tr:first');
            $r.next().css({'display':'block'});
        });
    });


</script>
</asp:Content>

