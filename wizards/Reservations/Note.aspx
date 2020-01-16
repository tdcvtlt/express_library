<%@ Page Title="" Language="VB" MasterPageFile="~/wizards/Reservations/ReservationMasterPage.master" AutoEventWireup="false" CodeFile="Note.aspx.vb" Inherits="wizard_Reservations_Note" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head2" Runat="Server">
    <script type="text/javascript" src="../../Scripts/pop_modal.js"></script>
    <script type="text/javascript">
        $(function () {

        });

        function addNew() {
            modal.mwindow.open('../../general/editnote.aspx?noteid=0&KeyField=' + $('#<%= hfKeyField.ClientID%>').val() + '&KeyValue=' + $('#<%= hfKeyValue.ClientID %>').val() + '&linkid=ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder2$lbRefresh', 'win01', 350, 350);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div class="container">


        <div class="panel panel-success">
            <div class="panel-heading">                
                <div class="panel-title">                                                
                    <h2 style="text-align:right;padding-right:20px;">Note</h2>
                </div>
            </div>

            <div class="panel-body">
                <div class="form-group form-group-sm">
                    <div class="row top-buffer">
                <div class="col-md-12">  
                    <h2 class="text-capitalize text-danger text-center">notes</h2>
                </div>
            </div>
                    <div class="row top-buffer">
                <div class="col-md-12">                    
                    <asp:MultiView runat="server" ID="multiview1" ActiveViewIndex="0">
                        <asp:View ID="view1" runat="server">
                            <asp:GridView ID="gvNotes" runat="server" CssClass="table table-bordered table-hover table-striped" EmptyDataText="No Notes">
                            <AlternatingRowStyle BackColor="#C7E3D7" />
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <a href="#" title="Edit" class="btn btn-block" onclick = "javascript:modal.mwindow.open('../../general/editnote.aspx?noteid=<%#container.Dataitem("ID")%>&KeyField=<%=hfKeyField.value %>&keyvalue=<%=hfKeyValue.value%>&linkid=ctl00$ContentPlaceHolder1$ascxNote$lbRefresh','win01',350,350);">Edit</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>
                    <div class="row top-buffer">
                <div class="col-sm-3">
                    <ul style="list-style-type:none;text-align:center;">
                        <li style="display:inline;"><asp:LinkButton ID="lbAddNew" OnClientClick="addNew();" CssClass="btn btn-primary" runat="server">Add New</asp:LinkButton></li>  
                        <li style="display:inline;"><asp:LinkButton ID="lbRefresh" CssClass="btn btn-warning" runat="server">Refresh</asp:LinkButton></li>                      
                    </ul>
                </div>
                
            </div>
                    <div class="row top-buffer">
                <div class="col-md-12">                                 
                    <asp:Button runat="server" ID="btNext" CssClass="btn btn-lg btn-success" Style="width:160px;font-weight:bolder;" Text="Next &rarr;" />
                </div>
        </div>
                </div>
            </div>
        </div>                        
    </div>
    <asp:HiddenField ID="hfKeyValue" Value="0" runat="server" />
    <asp:HiddenField ID="hfKeyField" Value = "RESERVATIONID" runat="server" />
</asp:Content>

