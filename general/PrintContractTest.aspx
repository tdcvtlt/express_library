<%@ Page Title="" Language="VB" AutoEventWireup="false" CodeFile="PrintContractTest.aspx.vb" Inherits="general_PrintContractTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692FBEA5521E1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Creating Contract Documents</title>
    <link type="text/css" rel="Stylesheet" href="../styles/master.css" />
</head>
<body>
        <OBJECT ID="ScriptControl1" WIDTH=39 HEIGHT=39
		 CLASSID="CLSID:0E59F1D5-1FBE-11D0-8FF2-00A0D10038BC">
		    <PARAM NAME="_ExtentX" VALUE="1005">
		    <PARAM NAME="_ExtentY" VALUE="1005">	
	</OBJECT>
    <form id="form1" runat="server">

<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

                <OBJECT ID="ScriptControl1" WIDTH=39 HEIGHT=39
		 CLASSID="CLSID:0E59F1D5-1FBE-11D0-8FF2-00A0D10038BC">
		    <PARAM NAME="_ExtentX" VALUE="1005">
		    <PARAM NAME="_ExtentY" VALUE="1005">	
	</OBJECT>
<ul id="menu">
        <li <%if MultiView1.Visible Then : Response.Write("class=""current""") : End If %>><asp:LinkButton ID="PDF_Link" runat="server" autopostback="true">PDF</asp:LinkButton></li>
        <li <%if  MultiView2.Visible Then : Response.Write("class=""current""") : End If %>><asp:LinkButton ID="Word_Link" runat="server" autopostback="true">Word Docs</asp:LinkButton></li>
     </ul>
     <div>
         
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">


<ContentTemplate>
        <asp:MultiView ID="MultiView1" runat="server">

            <asp:View ID="View1" runat="server">
                
        Select Document Group:
        <asp:GridView ID="gvPDFGroups" runat="server" AutoGenerateSelectButton="true">
            <AlternatingRowStyle BackColor="#C7E3D7" />
        </asp:GridView>

        

    </asp:View>
            
            <asp:View ID="View3" runat="server" >
                Generating Contract Documents.....Please Wait! <br />
                <img src="../images/progressbar.gif" />
                <div id="DIVinPage">
                <object data='PrintContractTest.aspx?ContractID=<%=Request("ContractID")%>&file=<%= hfSelected.Value %>' 
        type='application/pdf' 
        width='100%' 
        height='700px'>
            </asp:View>
            </asp:MultiView>
            <asp:HiddenField ID="hfSelected" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
        <asp:MultiView ID="MultiView2" runat="server">
            <asp:View ID="View2" runat="server">
        Select Document Group:
        <asp:GridView ID="gvWordGroups" runat="server" AutoGenerateSelectButton="true">
            <AlternatingRowStyle BackColor="#C7E3D7" />
        </asp:GridView> 

            </asp:View>
       </asp:MultiView>
        <asp:Literal ID="Lit" runat="server"></asp:Literal>

    </div>


    </form>
</body>

<script type="text/javascript">
    function beforeAsyncPostBack() 
    {
        var curtime = new Date();
       
        //alert('Time before PostBack:   ' + curtime);
        
    }
    var conID = <%=Request("ContractID")%>;
    function afterAsyncPostBack() {
        //alert('here');
        //window.location.href = "../Default6.aspx"    
        //window.location.href = "PrintContractPDF.aspx?ContractID=" + conID + "&GroupID=" + document.getElementById('hfSelected').getAttribute('value').toString();
        
        //window.location.href = "PrintContractTest.aspx?ContractID=" + conID + "&file=" + document.getElementById('hfSelected').getAttribute('value').toString();
        //document.location.href = 'PrintContractTest.aspx?ContractID=' + conID + '&file=' + document.getElementById('hfSelected').getAttribute('value').toString();
        //alert('here');
        //var curtime = new Date();

        //alert('Time after PostBack:   ' + curtime);
    }
    Sys.Application.add_init(appl_init);

    function appl_init() {
        var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
        pgRegMgr.add_beginRequest(BeginHandler);
        pgRegMgr.add_endRequest(EndHandler);
    }

    function BeginHandler() {
        beforeAsyncPostBack();
    }

    function EndHandler() {
        afterAsyncPostBack();
    }
</script>
</html>

