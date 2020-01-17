<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pmtasks.aspx.vb" Inherits="Maintenance_pmtasks" %>
<%@ Register Src="~/controls/DateField.ascx" TagName="DateField" TagPrefix="uc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" language="javascript" src = "<%=request.applicationpath%>/scripts/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" language="javascript" src = "<%=request.applicationpath%>/scripts/pop_modal.js"></script>
    <script type="text/javascript">

        $(function () {

            $('select').css({ 'font': 'bold 1.3em George, serif' });
            $('td:nth-child(1)').css({ 'font': 'bold 1.3em George, serif', 'width': '145px' });
            $('input[type=text]').css({ 'font': 'bold 1.3em George, serif' }).attr('size', '35' + 'px');
            $('input[value=Submit]').css({ 'margin-top': '15' + 'px' });
            $('legend span').css({ 'font': 'bold 1.6em George, serif', 'color': 'gray' });
            $('<br/><br/>').insertBefore('table');

        });         

        function CheckAll(cb) {                         
            $(cb).bind('change',function () {                
                $('input:checkbox').not(this).prop('checked', this.checked);                
            }).trigger('change');
        }

        function popupMasterInventoryList(btn) {            
            window.open('<%=request.applicationpath%>' + '/Accounting/imported/MasterInventoryList.asp?action=pm', 'winpop', 'height=600,width=600');                      
        }   
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div>
        <asp:MultiView runat="server" ID="mv">
            <asp:View runat="server" ID="mvItem2Track">
                <fieldset>
                    <legend><asp:Label runat="server" ID="item_label"></asp:Label></legend>
                        
                        <table id="table-POI" style="width:100%">
                            <tr>                
                                <td>Category</td>
                                <td><asp:DropDownList runat="server" ID="item_category"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>Great Plain</td>
                                <td><asp:TextBox runat="server" ID="item_great" gpid="-1"></asp:TextBox>&nbsp;<asp:Button runat="server" ID="item_search" OnClientClick="popupMasterInventoryList(this);" Text="search" /></td>
                            </tr>
                            <tr>
                                <td>Name</td>
                                <td><asp:TextBox runat="server" ID="item_name"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Description</td>
                                <td><asp:TextBox runat="server" ID="item_description"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Life Span</td>
                                <td><asp:TextBox runat="server" ID="item_life"></asp:TextBox></td>
                            </tr>
                            <tfoot>
                                <tr>
                                    <td></td>
                                    <td><asp:Button runat="server" ID="item_submit" Text="Submit" /></td>
                                </tr>
                            </tfoot>
                        </table>                                            
                </fieldset>
            </asp:View>
            <asp:View runat="server" ID="mvTask">
                <fieldset>
                    <legend><asp:Label runat="server" ID="task_label"></asp:Label></legend>

                    <table style="width:100%">
                        <tr>
                            <td>Name</td>
                            <td><asp:TextBox runat="server" ID="task_name"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Description</td>
                            <td><asp:TextBox runat="server" ID="task_description"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Interval</td>
                            <td><asp:DropDownList runat="server" ID="task_interval"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Work Order Category</td>
                            <td><asp:DropDownList runat="server" ID="task_category"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Issue</td>
                            <td><asp:DropDownList runat="server" ID="task_issue"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Team:</td>
                            <td><asp:DropDownList runat="server" ID="ddTeam"></asp:DropDownList></td>
                        </tr>
                        <tfoot>
                            <tr>
                                <td></td>
                                <td><asp:Button runat="server" ID="task_submit" Text="Submit" /></td>
                            </tr>
                        </tfoot>
                    </table>
                </fieldset>
            
            </asp:View>
       
               
            <asp:View runat="server" ID="mvPMItem2Track">
                <h2>PREVENTIVE MAINTENANCE</h2>
                <h3>SELECT ITEM TO SCHEDULE</h3> 
                <asp:Button runat="server" ID="mulItemSubmit" Text="Submit" />
                <div style="overflow:auto">
                <asp:GridView runat="server" ID="gvPMItem2Track" AutoGenerateColumns="false" DataKeyNames="item2trackid">
                    <Columns>             
                    <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left" >
                    <HeaderStyle Font-Bold="true" />
                    <HeaderTemplate>                                            
                        <input type="checkbox" runat="server" id="roomsCheckBox" onclick="javascript:CheckAll(this);"  /><span>All</span>                                                                   
                    </HeaderTemplate>                    
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="cbItemAddList" />
                    </ItemTemplate>                    
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle Width="120" />
                        <HeaderTemplate>
                            <span>Date Added</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <uc:DateField runat="server" ID="df_ItemDateAdded" />
                        </ItemTemplate>
                    </asp:TemplateField>
                                                                                          
                    <asp:TemplateField HeaderText="Extra ">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="extraText" Width="200"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />                    
                    <asp:BoundField DataField="Category" HeaderText="Category" />                    
                    <asp:BoundField DataField="Life" HeaderText="Life Span" />                    
                    </Columns>
                </asp:GridView>
                </div>            
            </asp:View> 
     
            <asp:View runat="server" ID="vwItemEditDelete">
                <div>
                    <fieldset>
                        <legend>
                            <strong>ROOM/BUILDING</strong>
                        </legend>

                        <div>
                            <table style="">
                                <thead>
                                    <tr><th></th><th></th></tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td><span>Name</span></td>
                                        <td><asp:TextBox runat="server" ID="tbItemName" ReadOnly="true"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><span>Description</span></td>
                                        <td><asp:TextBox runat="server" ID="tbItemDescription"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><span>Date Added</span></td>
                                        <td>
                                            <uc:DateField runat="server" ID="dfdItemDateAdded" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><span>Date Removed</span></td>
                                        <td>
                                            <uc:DateField runat="server" ID="dfdItemDateRemoved" />
                                        </td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td></td>
                                        <td><asp:Button runat="server" ID="btnItemSubmit" Text="Submit" /></td>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                    </fieldset>
                </div>
                
            </asp:View>     
            
            <asp:View runat="server" ID="vwItemAdd">
                <fieldset>
                    <legend><strong>ROOM/BUILDING INSERT</strong></legend>
                    <div>
                        <table>
                            <thead><tr><th></th><th></th></tr></thead>
                            <tbody>
                                <tr>
                                    <td>Description</td>
                                    <td><asp:TextBox runat="server" ID="tbItemAddDescription"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Date Add</td>
                                    <td><uc:DateField runat="server" ID="dfItemAddDateAdd" /></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button runat="server" ID="btItemAddSubmit" Text="Submit" />
                                        <br />
                                        <div style="overflow:auto;width:100%;height:350px;">                                            
                                            <asp:GridView runat="server" ID="gvwItemAddList" AutoGenerateColumns="false">
                                            <Columns>                                    
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderStyle Font-Bold="true" />
                                            <HeaderTemplate>                                            
                                                <input type="checkbox" runat="server" id="roomsCheckBox" onclick="javascript:CheckAll(this);"  />                                            
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="cbItemAddList" />
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText=""  />
                                            <asp:BoundField HeaderText="" />
                                            </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </fieldset>
            
            </asp:View>
            <asp:View ID="vwPMItemChangeRemove" runat="server">
                 <div>
                   <table>
                        <tr>
                            <td>Description</td>
                            <td><asp:TextBox runat="server" ID="PMItem_Description"></asp:TextBox></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>Date Added</td>
                            <td><uc:DateField runat="server" ID="PMItem_df_DateAdded" /></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>Date Removed</td>
                            <td><uc:DateField runat="server" ID="PMItem_df_DateRemoved" /></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><asp:Button runat="server" Text="Submit" ID="PMItem_Submit" /></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
    </form>
</body>
</html>
