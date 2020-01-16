
Partial Class wizards_WizEditOPCRep
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oSecurity As New Security
        Dim oPers As New clsPersonnel
        Dim oVendor As New clsVendor
        If Not (oSecurity.Is_Logged_On(Session)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "security", "window.close();", True)
            oSecurity = Nothing
            Exit Sub
        End If

        If Not IsPostBack Then
            ddOPCRep.DataSource = oPers.List_OPC_Reps
            ddOPCRep.DataTextField = "Personnel"
            ddOPCRep.DataValueField = "PersonnelID"
            ddOPCRep.DataBind()
            ddOPCLoc.DataSource = oVendor.List_Sales_Locations("OPC")
            ddOPCLoc.DataTextField = "Location"
            ddOPCLoc.DataValueField = "SalesLocationID"
            ddOPCLoc.DataBind()

        End If
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session("OPCRep") = ddOPCRep.SelectedValue
        Session("OPCLocation") = ddOPCLoc.SelectedValue
        If Request("redirect") = "true" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lblRefresh6a','');window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.__doPostBack('ctl00$ContentPlaceHolder1$lblRefresh4a','');window.close();", True)
        End If
    End Sub
End Class
