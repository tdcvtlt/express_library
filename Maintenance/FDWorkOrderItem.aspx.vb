Imports System.Data
Partial Class Maintenance_FDWorkOrderItem
    Inherits System.Web.UI.Page


    Protected Sub Unnamed3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt As New DataTable
        Dim dr As DataRow
        dt = Session("dt")
        dr = dt.NewRow
        dr("Category") = ddCategory.SelectedValue
        dr("Issue") = siIssue.SelectedName
        dr("Description") = txtDesc.Text
        dt.Rows.Add(dr)
        Session("dt") = dt

        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.add_Item();window.close();", True)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRequest As New clsRequest
            ddCategory.DataSource = oRequest.List_Categories(0)
            ddCategory.DataValueField = "Category"
            ddCategory.DataTextField = "Category"
            ddCategory.DataBind()
            oRequest = Nothing

            siIssue.Connection_String = Resources.Resource.cns
            siIssue.Comboitem = "MaintenanceRequestIssue"
            siIssue.Label_Caption = ""
            siIssue.Load_Items()
        End If
    End Sub
End Class
