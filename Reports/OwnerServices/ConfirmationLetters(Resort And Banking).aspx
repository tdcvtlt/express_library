<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ConfirmationLetters(Resort And Banking).aspx.vb" Inherits="Reports_OwnerServices_ConfirmationLetters_Resort_And_Banking_" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#btAddReservationTo').on('click', function (e) {                
                if ($("#<%= txReservationID.ClientID %>").val().length > 0) {  
                    var container = $("#poolDV");
                    var text = $("#<%= txReservationID.ClientID %>").val();
                    var count = $("#poolDV input:checkbox").length + 1;
                    var checkbox = $("<input type='checkbox' checked='yes' id='cb" + count + "' value='" + text + "' />" +
                                    "<label for='cb" + count + "'>" + text + "</label><br/>");
                    if (count == 1) {
                        container.append(checkbox);
                    } else {
                        checkbox.insertBefore("#poolDV :checkbox:first");
                    }
                    $("#<%= txReservationID.ClientID %>").val("");
                }
                e.preventDefault();
            });
            $("#<%= txReservationID.ClientID %>").keypress(function (e) {
                
                if (e.keyCode === 13 || e.keyCode === 10) {
                    e.preventDefault();
                    $('#btAddReservationTo').trigger("click");
                    $("#<%= txReservationID.ClientID %>").val("");
                }
            });
            $('#<%= btSubmit.ClientID %>').on("click", function (e) {
                var cb = $('#poolDV :checkbox');
                var ar = [];
                if (cb.length > 0) {                    
                    cb.each(function (index) {
                        if ($(this).is(":checked")) {
                            ar.push($(this).val());
                        }                        
                    });
                }
                if (ar.length > 0) {
                    $("#<%= hfReservationID.ClientID %>").val(ar);                    
                }                               
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    
    <div style="width:1000px;border:5px solid gray">
        <div style="width:50%;float:left;border:0px solid black;">     
            <div style="padding:20px;">
                <fieldset>
                <legend><strong>Confirmation Letters</strong></legend>
                <div style="padding:20px;">
                    <p style="font-weight:bold;color:blue;">Resort Stay</p>
                    <hr />
                    <asp:RadioButton runat="server" ID="rbResortStay" GroupName="g1" Text="King's Creek Plantation, Williamsburg" />
                    <hr />                    
                    <p style="font-weight:bold;color:blue;">Banking</p>                    
                    <table>
                        <tr><td><asp:RadioButton runat="server" ID="rbRCI" GroupName="g1" Text="RCI Banking" /></td></tr>
                        <tr><td><asp:RadioButton runat="server" ID="rbII" GroupName="g1" Text="II Banking" /></td></tr>
                        <tr><td><asp:RadioButton runat="server" ID="rbICE" GroupName="g1" Text="ICE Banking" /></td></tr>
                    </table>                                        
                </div> 
                    
                <div style="clear:both">
                    <asp:Label runat="server" ID="lbErr" ForeColor="Red"></asp:Label>
                </div>              
            </fieldset>
                <br />
                <asp:Button runat="server" ID="btSubmit" Text="Submit"  />
                <asp:HiddenField runat="server" ID="hfReservationID" />
            </div>                                           
        </div>
        <div style="width:50%;margin-left:50%;border-left:1px solid gray;">
            <div style="padding:20px;">
                <fieldset>
                    <legend><asp:RadioButton runat="server" ID="rbDateRange" GroupName="g2" Text="Date Range" Font-Bold="true" /></legend>
                    <table>
                        <tr>
                            <td>
                                <ucDatePicker:DateField runat="server" ID="fromDate" />
                            </td>
                            <td>&nbsp;-&nbsp;</td>
                            <td>
                                <ucDatePicker:DateField runat="server" ID="toDate" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br /><br />
                <fieldset>
                    <legend><asp:RadioButton runat="server" ID="rbReservationID" GroupName="g2" Text="Reservation ID" Font-Bold="true" /></legend>
                    <asp:TextBox runat="server" ID="txReservationID" ></asp:TextBox><input type="button" value="Add" id="btAddReservationTo" />
                    <div id="poolDV" style="overflow:auto;width:280px;height:170px;border:1px solid silver;">
                    </div>
                </fieldset>                
            </div>                     
        </div>        
    </div>


    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" GroupTreeStyle-ShowLines="false" ToolPanelView="None" />
    </div>

</asp:Content>

