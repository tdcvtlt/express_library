
Partial Class setup_EditPkg2Letter
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oPkg2Letter As New clsPackage2Letter
            Dim oPkgLetter As New clsPackageLetters
            oPkg2Letter.Package2LetterID = Request("ID")
            oPkg2Letter.Load()
            oPkgLetter.PackageLetterID = oPkg2Letter.PackageLetterID
            oPkgLetter.Load()
            ddLetters.DataSource = oPkgLetter.Get_Letters
            ddLetters.DataValueField = "ID"
            ddLetters.DataTextField = "Name"
            ddLetters.DataBind()
            cbActive.Checked = oPkg2Letter.Active
            txtSubject.Text = oPkg2Letter.Subject
            ddLetters.SelectedValue = oPkg2Letter.PackageLetterID
            oPkg2Letter = Nothing
            oPkgLetter = Nothing
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim oPkg2Letter As New clsPackage2Letter
        oPkg2Letter.Package2LetterID = Request("ID")
        oPkg2Letter.Load()
        If Request("ID") = 0 Then
            oPkg2Letter.PackageID = Request("PackageID")
        End If
        oPkg2Letter.Active = cbActive.Checked
        oPkg2Letter.Subject = txtSubject.Text
        oPkg2Letter.PackageLetterID = ddLetters.SelectedValue
        oPkg2Letter.Save()
        oPkg2Letter = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.refreshLetters();window.close();", True)
    End Sub
End Class
