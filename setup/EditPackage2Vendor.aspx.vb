
Partial Class setup_EditPackage2Vendor
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oVendor As New clsVendor
            ddVendors.DataSource = oVendor.List_Vendors()
            ddVendors.DataValueField = "VendorID"
            ddVendors.DataTextField = "Vendor"
            ddVendors.DataBind()
            oVendor = Nothing
            If Request("ID") <> 0 Then
                Dim opkgVendor As New clsVendor2Package
                opkgVendor.Vendor2PackageID = Request("ID")
                opkgVendor.Load()
                ddVendors.SelectedValue = opkgVendor.VendorID
                cbDisplay.Checked = opkgVendor.Display
                opkgVendor = Nothing
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim opkgVendor As New clsVendor2Package
        opkgVendor.Vendor2PackageID = Request("ID")
        opkgVendor.Load()
        opkgVendor.UserID = Session("UserDBID")
        opkgVendor.VendorID = ddVendors.SelectedValue
        opkgVendor.Display = cbDisplay.Checked
        If Request("ID") = 0 Then
            opkgVendor.PackageID = Request("PackageID")
        End If
        opkgVendor.Save()
        opkgVendor = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.refreshVendors();window.close();", True)
    End Sub
End Class
