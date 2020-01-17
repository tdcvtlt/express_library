
Partial Class security_editVendor
    Inherits System.Web.UI.Page



    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oVendor2Personnel As New clsVendor2Personnel
        oVendor2Personnel.Vendor2PersonnelID = Request("ID")
        oVendor2Personnel.Load()
        oVendor2Personnel.Admin = cbAdmin.Checked
        oVendor2Personnel.Manager = cbManager.Checked
        oVendor2Personnel.VendorID = ddVendors.SelectedValue
        If Request("ID") = 0 Then
            oVendor2Personnel.PersonnelID = Request("PersonnelID")
        End If
        oVendor2Personnel.Save()
        oVendor2Personnel = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Vendors();window.close();", True)
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oVendor2Personnel As New clsVendor2Personnel
            If Request("ID") = 0 Then
                ddVendors.DataSource() = oVendor2Personnel.List_Vendors_Add(0, Request("PersonnelID"))
            Else
                ddVendors.DataSource = oVendor2Personnel.List_Vendors_Add(Request("ID"), 0)
            End If
            ddVendors.DataTextField = "Vendor"
            ddVendors.DataValueField = "VendorID"
            ddVendors.DataBind()
            oVendor2Personnel.Vendor2PersonnelID = Request("ID")
            oVendor2Personnel.Load()
            ddVendors.SelectedValue = oVendor2Personnel.VendorID
            cbManager.Checked = oVendor2Personnel.Manager
            cbAdmin.Checked = oVendor2Personnel.Admin
            oVendor2Personnel = Nothing
        End If
    End Sub
End Class
