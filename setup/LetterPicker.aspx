<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LetterPicker.aspx.vb" Inherits="setup_LetterPicker" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Letters</title>
    <script type="text/javascript" language="javascript" src = "<%=request.applicationpath%>/scripts/jquery-1.7.1.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:MultiView ID="multiview1" runat="server">
            <asp:View ID="view1" runat="server">
                <asp:GridView runat="server" ID="gv1" AutoGenerateColumns="false">
                
                    <Columns>
                        <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:LinkButton runat="server" ID="select" Text="Select" CommandName="select" CommandArgument='<%# Bind("letterid") %>'></asp:LinkButton></ItemTemplate></asp:TemplateField>
                        <asp:BoundField DataField="Type" HeaderText="Type" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                    </Columns>
                </asp:GridView>
            </asp:View>        
        </asp:MultiView>
    </div>
    </form>
</body>
</html>
