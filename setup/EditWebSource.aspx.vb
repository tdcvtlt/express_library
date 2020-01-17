
Partial Class setup_EditWebSource
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oPkgWebSource As New clsPackage2WebSource
            oPkgWebSource.Package2WebSourceID = Request("Package2WebSourceID")
            oPkgWebSource.Load()

            cbActive.Checked = oPkgWebSource.Active
            siWebSource.Selected_ID = oPkgWebSource.WebSourceID
            siWebSource.Connection_String = Resources.Resource.cns
            siWebSource.Label_Caption = ""
            siWebSource.ComboItem = "WebSource"
            siWebSource.Load_Items()

            oPkgWebSource = Nothing
        End If
        
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim oPkgWebSource As New clsPackage2WebSource
        oPkgWebSource.Package2WebSourceID = Request("Package2WebSourceID")
        oPkgWebSource.Load()
        If Request("Package2WebSourceID") = 0 Then
            oPkgWebSource.PackageID = Request("PackageID")
        End If
        oPkgWebSource.Active = cbActive.Checked
        oPkgWebSource.WebSourceID = siWebSource.Selected_ID
        oPkgWebSource.UserID = Session("UserDBID")
        oPkgWebSource.Save()
        oPkgWebSource = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.refreshWebSources();window.close();", True)
    End Sub
End Class
