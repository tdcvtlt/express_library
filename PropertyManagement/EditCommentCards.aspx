<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditCommentCards.aspx.vb" Inherits="PropertyManagement_EditCommentCards"%>

<%@ Register src="../controls/Comments.ascx" tagname="Comments" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    
    <script language="javascript" type="text/javascript">
        function Get_Window(r)
        {
            alert(r);
        }
        function Refresh_Rpt(firstName, lastName, roomnumber, guestid, roomid) {
            //alert('HERE');
            
            document.getElementById('ctl00_ContentPlaceHolder1_txtGuest').value = firstName + ' ' + lastName;
            document.getElementById('ctl00_ContentPlaceHolder1_txtRoom').value = roomnumber;
            document.getElementById('ctl00_ContentPlaceHolder1_txtGuestID').value = guestid;
            document.getElementById('ctl00_ContentPlaceHolder1_txtRoomID').value = roomid;

            // Compare first 5 characters to above lines
            //document.getElementById('ct100_ContentPlaceHolder1_txtGuestID').value = guestid;
        }


        
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="topForm" style="width:80%">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblGuest" runat="server" Text="Guest:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtGuest" runat="server"></asp:TextBox></td>
                <td>
                    <asp:Button ID="btnGuest" runat="server" Text="..." /></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblRoom" runat="server" Text="Room:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtRoom" runat="server"></asp:TextBox></td>
                <td>
                    <asp:Button ID="btnRoom" runat="server" Text="..." /></td>
            </tr>
            <tr>
                <td><asp:HiddenField ID="txtRoomID" runat="server"></asp:HiddenField></td>
            </tr>
            <tr>
                <td><asp:HiddenField ID="txtGuestID" runat="server"></asp:HiddenField></td>
            </tr>
        </table>

        <table>
            <tr>
                <td>
                    <asp:HyperLink runat="server" ID="hlBack"></asp:HyperLink>
                    <asp:Button ID="btnFields" runat="server" Text="Get Fields" Visible="false" />
                    <asp:Button ID="btnComment" runat="server" Text="Get Comments" Visible="false" />
                    <asp:Label ID="lblAlert" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div id="HR"><hr /></div>
    <div id="btmForm">
        <asp:Literal ID="lit1" runat="server" Text = "" />
    
    
    
    </div>

    <div id="" style="width:85%;">
    
    <div style="width:60%;border:1px dotted silver;float:left;">
        
        <asp:PlaceHolder ID="phContainer" runat = "server">
            <asp:HiddenField ID="ValueIdFieldId" runat="server" />
        </asp:PlaceHolder>

    <br /><br />
    </div>

     <div id="Comments" style="width:39%;float:right;border:1px dotted silver;">
        <br />       

        <div>
            <span><strong>Comments</strong></span>
            <br />
            <textarea id="Comment" runat="server" cols="50" rows="10"></textarea> 
        </div>
        <asp:Button ID="update" runat="server" Text="Update" />
        <br />

        <br /><br/>
        <hr />
        <div id='gtButtons'>
            <asp:Button ID="btnSave" runat="server" Text="Save" />   
            &nbsp;&nbsp;
            <asp:Button ID="btnAddField" runat="server" Text="Add Field" />            
        </div>
    </div>
    


    </div>



   
    


    <asp:HiddenField ID="hfCommentsId" runat="server" />

    

    <script type="text/javascript" src="../scripts/jquery-1.7.1.js"></script>

    <script type="text/javascript">
        
        document.getElementById("ctl00_ContentPlaceHolder1_htmRadioButtonListTable").style.border = "1";
        
    </script>
</asp:Content>

