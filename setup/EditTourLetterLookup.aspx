<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditTourLetterLookup.aspx.vb" Inherits="setup_EditTourLetterLookup" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <div>
    <asp:MultiView ID="mv" runat="server">
        <asp:View ID="mvView1" runat="server">
            
            <div>
                <h3>TOUR CAMPAIGN</h3>
                <table cellspacing="2" cellpadding="0">
                    <tr>
                        <td>
                            <h4>Campaign Name</h4>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlCampaign"></asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h4>Active</h4>
                        </td>
                        <td>
                             <asp:CheckBox runat="server" ID="mvView1Active" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button runat="server" ID="mvView1Save" Text="Save" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>                                                       
                
            </div>
                
        </asp:View>
        <asp:View ID="mvView2" runat="server">
            <div>
                <h3>TOUR LOCATION</h3>
                <table cellspacing="2">
                    <tr>
                        <td>
                            <h4>Location</h4>
                        </td>
                        <td>
                             <uc1:Select_Item ID="siLocation" runat="server" />
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            <h4>Active</h4>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="mvView2Active" />
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button runat="server" ID="mvView2Save" Text="Save" />
                        </td>                        
                    </tr>
                </table>                               
                
            </div>

        </asp:View>    
    </asp:MultiView>
    
    </div>
    </form>
</body>
</html>
