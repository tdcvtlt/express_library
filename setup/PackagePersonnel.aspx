<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PackagePersonnel.aspx.vb" Inherits="setup_PackagePersonnel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../scripts/jquery-1.7.1.min.js"></script>
</head>
<body>

    <form id="form1" runat="server">

    <asp:ScriptManager ID="ScriptManagerDoPostBack" runat="server"></asp:ScriptManager>

    <!-- T_PACKAGEPERSONNEL'S PRIMARY KEY -->
    <asp:HiddenField ID="HiddenFieldPackagePersonnelID" runat="server" />
        
    <!-- T_PACKAGERESERVATIONPERSONNEL'S PRIMARY KEY -->
    <asp:HiddenField ID="HiddenFieldPackageReservationPersonnelID" runat="server" />
    
    <!-- T_PACKAGETOURPERSONNEL'S PRIMARY KEY -->
    <asp:HiddenField ID="HiddenFieldPackageTourPersonnelID" runat="server" />
    

    <!-- In common-->    
    <asp:HiddenField ID="HiddenFieldPackageID" runat="server" />
    <asp:HiddenField ID="HiddenFieldPersonnelID" runat="server" />
    <asp:HiddenField ID="HiddenFieldTourID" runat="server" />

    <asp:HiddenField ID="HiddenFieldPackageReservationID" runat="server" />    

    <asp:HiddenField ID="HiddenFieldPackageTourID" runat="server" />
    <asp:HiddenField ID="HiddenFieldType" runat="server" />


    <div id="wrapper">
         <table>
            <tr>
                <td colspan="2">Name</td>                
                <td colspan="2"><asp:DropDownList runat="server" ID="DropDownListPersonnel"></asp:DropDownList></td>                
            </tr>
            <tr>
                <td colspan="2">Title</td>                
                <td><asp:DropDownList runat="server" ID="DropDownListPersonnelTitle"></asp:DropDownList></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2">Commission %</td>                
                <td><asp:TextBox ID="TextBoxCommission" runat="server"></asp:TextBox></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2">Fixed Amount $</td>                
                <td><asp:TextBox ID="TextBoxAmount" runat="server"></asp:TextBox></td>
                <td></td>
            </tr>
        </table>    
        
        <br />
        <asp:Button ID="ButtonSubmit" runat="server" Text="Save" />&nbsp;&nbsp;
        <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" />        
    </div>
    </form>

</body>
</html>
