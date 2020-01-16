<%@ Page Language="VB" AutoEventWireup="false" CodeFile="selectspouse.aspx.vb" Inherits="general_selectspouse" %>

<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <asp:RadioButton ID="rbExisting" runat="server" Text="Select Existing Record" 
            AutoPostBack="True" Checked="True" GroupName="choose" /> 
        <asp:RadioButton ID="rbNew" runat="server" Text="New Record" 
            AutoPostBack="True" GroupName="choose" /> 
        
    </div>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <fieldset><legend></legend>
            <table>
                <tr>
                    <td>Last Name:</td>
                    <td><asp:TextBox ID="txtLastName" runat="server"></asp:TextBox></td>
                    <td>First Name:</td>
                    <td><asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Middle Initial:</td>
                    <td><asp:TextBox ID="txtMiddleInit" runat="server" Width="36px"></asp:TextBox></td>
                    <td>SSN:</td>
                    <td><asp:TextBox ID="txtSSN" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Address:</td>
                    <td><asp:TextBox ID="txtAddress" runat="server"></asp:TextBox></td>
                    <td>City:</td>
                    <td><asp:TextBox ID="txtCity" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>State:</td>
                    <td>
                        <uc1:Select_Item ID="siState" runat="server" />
                    </td>
                    <td>Zip:</td>
                    <td><asp:TextBox ID="txtZip" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Home Phone:</td>
                    <td><asp:TextBox ID="txtHomePhone" runat="server"></asp:TextBox></td>
                    <td>Email:</td>
                    <td><asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </fieldset>
        <asp:Button ID="btnAdd" runat="server" Text="Add" />
        </asp:View>
        <asp:View ID="View2" runat="server">
        <asp:Label ID="Label2" runat="server" Text="Filter: "></asp:Label>
    <asp:DropDownList ID="ddFilter" runat="server">
        <asp:ListItem Value="Address1">Address1</asp:ListItem>
        <asp:ListItem Value="City">City</asp:ListItem>
        <asp:ListItem Value="Email">Email</asp:ListItem>
        <asp:ListItem Value="ID">ID</asp:ListItem>
        <asp:ListItem Value="Name">Name</asp:ListItem>
        <asp:ListItem Selected="True" Value="Phone">Phone</asp:ListItem>
        <asp:ListItem Value="PostalCode">PostalCode</asp:ListItem>
        <asp:ListItem Value="SpouseSSN">SpouseSSN</asp:ListItem>
        <asp:ListItem Value="SSN">SSN</asp:ListItem>
        <asp:ListItem Value="State">State</asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />
    <asp:Label ID="Label1" runat="server" Text="Enter Home Phone:"></asp:Label><br />
    <asp:TextBox ID="filter" runat="server"></asp:TextBox>
    <asp:Button ID="btnQuery"
        runat="server" Text="Query" />
    <br />
    <div style="height:200px;width:600px;overflow:auto; ">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="True" 
    EmptyDataText="No Records" GridLines="Horizontal">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />
    </asp:GridView>
    </div>
        </asp:View>
    </asp:MultiView>
    </form>
</body>
</html>
