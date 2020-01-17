<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddTour.aspx.vb" Inherits="general_AddTour" aspcompat="true"%>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="vwTour" runat="server">
        <div>
    <table>
		<tr>
			<td>Tour Date:</td>
			<td><uc1:DateField ID="dfTourDate" runat="server" /></td>
		</tr>
		<tr>
			<td>Tour Time:</td>
			<td>
                <uc2:Select_Item ID="siTourTime" runat="server" />
            </td>
		</tr>
		<tr>
			<td>Booked By:</td>
			<td>
				
			    <asp:DropDownList ID="ddBookedBy" runat="server">
                </asp:DropDownList>
			</td>
        </tr>
		<tr>
			<td>Premiums:</td>
		</tr>
		<tr>
			<td colspan = '2'>
			
                        <asp:CheckBoxList ID="cblPremiums" runat="server">
                        </asp:CheckBoxList>
            
			</td>
		</tr>
		<tr>
			<td colspan = '2'>
				<asp:button ID="btnAssign" text = "Assign Premium Qty Amounts." runat="server"  />
			</td>
		</tr>
		<input type = 'hidden' name = 'reservationID' value = '<%=request("reservationid")%>'/>
		<input type = 'hidden' name = 'prospectID' value = '<%=request("ProspectID")%>'/>
		
	</table>
    <asp:Label ID="lblErr" runat="server" ForeColor="Red"></asp:Label>
    </div>
        </asp:View>
        <asp:View ID = "vwPrem" runat="server">
            
            <asp:Repeater ID="Repeater1" runat="server"  OnItemDataBound="Repeater1_DataBound" >
                <HeaderTemplate>
                <table>
                    <tr>
	                    <td><b><u>Premium</u></b></td>
	                    <td><b><u>Qty</u></b></td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><asp:HiddenField runat="server" ID="PIID" /><%#GetDataItem("PremiumName") %></td>
                        <td><asp:DropDownList ID="ddQTY" runat="server"></asp:DropDownList></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Button ID="btnSave" Text="Save & Close" runat="server"/>


        </asp:View>
    </asp:MultiView>
    <asp:HiddenField ID="hfTourID" Value = "0" runat="server" />
    </form>
</body>
</html>
