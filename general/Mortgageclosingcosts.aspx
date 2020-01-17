<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Mortgageclosingcosts.aspx.vb" Inherits="general_Mortgageclosingcosts" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="gvMCC" runat="server" AutoGenerateColumns="false" onRowDataBound = "gvMCC_RowDataBound" EmptyDataText = "No Records">
            <AlternatingRowStyle BackColor="#C7E3D7" />
            <Columns>
            
                <asp:BoundField DataField="ID" HeaderText="ID" />
                <asp:BoundField  DataField="FieldName" HeaderText="FieldName" />
                <asp:BoundField  DataField="Amount" HeaderText="Amount" />
                <asp:BoundField DataField = "Optional" HeaderText="Optional" />
                <asp:TemplateField>
                    <Itemtemplate>
                        <asp:CheckBox id="ckEdit" runat="server" AutoPostBack="true" OnCheckedChanged="ShowEdit" />
                    </Itemtemplate>
                </asp:TemplateField>
     
            </Columns>
        </asp:GridView>
        <asp:Panel ID="Panel1" runat="server">
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>:
            <asp:TextBox ID="TextBox1" runat="server" style="text-align:right;"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Update" />
        </asp:Panel>
    </div>
    <asp:HiddenField ID="hfRow" value = "0" runat="server" />
    </form>
</body>
</html>
