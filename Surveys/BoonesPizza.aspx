<%@ Page Title="Boone's Pizza Feedback" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="BoonesPizza.aspx.vb" Inherits="Surveys_BoonesPizza" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="details">
    Please use the following scale:<br />
    1 = Poor&nbsp;&nbsp;&nbsp;&nbsp; 2 = Below Average&nbsp;&nbsp;&nbsp;&nbsp; 3 = Average&nbsp;&nbsp;&nbsp;&nbsp;
    4 = Above Average&nbsp;&nbsp;&nbsp;&nbsp;
    5 = Excellent&nbsp;&nbsp;&nbsp;&nbsp;
    <table>
        
        <tr>
            <td>Toppings:</td>
            <td>
                <asp:RadioButtonList ID="rblToppings" runat="server" 
                    RepeatDirection="Horizontal" TextAlign="Left" Width="590px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>Crust:</td>
            <td>
                <asp:RadioButtonList ID="rblCrust" runat="server" 
                    RepeatDirection="Horizontal" TextAlign="Left" Width="590px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>Sauce:</td>
            <td>
                <asp:RadioButtonList ID="rblSauce" runat="server" 
                    RepeatDirection="Horizontal" TextAlign="Left" Width="590px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>Cheese:</td>
            <td>
                <asp:RadioButtonList ID="rblCheese" runat="server" 
                    RepeatDirection="Horizontal" TextAlign="Left" Width="590px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>Presentation:</td>
            <td>
                <asp:RadioButtonList ID="rblPresentation" runat="server" 
                    RepeatDirection="Horizontal" TextAlign="Left" Width="590px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>Comments:</td>
            <td>
                <asp:TextBox ID="tbComments" runat="server" Height="131px" TextMode="MultiLine" 
                    Width="592px"></asp:TextBox>
            </td>
        </tr>
    </table>
    </div>
    <asp:Button ID="btnSubmit" runat="server" Text="Submit Feedback" />
    
    <asp:Button ID="btnReset" runat="server" Text="Reset Form" />
    
    <input type="button" value = "Printable Version" name = "Printable Version" 
        onclick="var win = window.open();win.document.write(document.getElementById('details').innerHTML);" />
    
</asp:Content>

