<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="FlatworldList.aspx.vb" Inherits="marketing_FlatworldList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style type="text/css">
  .even, .one
    {        
        background-color:#fefcff;
    }
    
    .odd, .two
    {
        background-color:#e5e4e2;
    }
    
    .three
    {
        background-color:#d1d0ce;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>

<h1>Reservations - <% response.Write(DateTime.Now.ToString("D")) %></h1> 
<h2>Booking Status - In-complete</h2>

    <asp:Button runat="server" ID="btnRefresh" Text="Refresh" />
    <br /> <br />
    <asp:GridView ID="FlatWorldList" runat="server" AutoGenerateColumns="False" DataKeyNames="reservationId">
        <Columns>
            <asp:TemplateField HeaderText="Reservation ID">
                <ItemTemplate>
                    <asp:HyperLink ID="link" runat="server" Target="_blank"  Text='<%# Eval("reservationid") %>'></asp:HyperLink>                
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Prospect Name"  HeaderText="Prospect Name"/>
            <asp:BoundField DataField="Reservation Location" HeaderText="Reservation Location" />

              <asp:TemplateField HeaderText="Note" HeaderStyle-Width="350px">
                <ItemTemplate>
                    <asp:TextBox runat="server" Width="350" ReadOnly="true" ID="note" Text='<%# Eval("note") %>' />           
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <asp:DropDownList runat="server" ID="status" DataTextField="comboitem" DataValueField="comboitemid" ></asp:DropDownList>              
                </ItemTemplate>
            </asp:TemplateField>

             <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button runat="server" ID="submit" CommandName="submit" Text="Submit" />
                </ItemTemplate>
            </asp:TemplateField>                                    
        </Columns>
    </asp:GridView>
</div>

<script type="text/javascript">


    $(function () {

        var i = 0;
        var striping = { 0: 'one', 1: 'two', 2: 'three' };


        $('#<%= FlatWorldList.ClientID %> tr:has(:not(th))').each(function (index, tr) {
            if (i % 3 == 0) {
                i = 0;
            } else {
                i = (i % 2 == 0 ? 2 : 1);
            }

            $(tr).addClass(striping[i]);
            i++;
        });
    });


</script>

</asp:Content>

