
Partial Class setup_EditPackageLetter
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oPkgLetter As New clsPackageLetters
            oPkgLetter.PackageLetterID = Request("LetterID")
            oPkgLetter.Load()
            txtName.Text = oPkgLetter.Name
            CKEditor1.Text = oPkgLetter.LetterContent
            oPkgLetter = Nothing
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        If CheckSecurity("Packages", "EditLetters", , , Session("UserDBID")) Then
            If CKEditor1.Text = "" Then

            Else
                Dim oPkgLetter As New clsPackageLetters
                Dim letterID As Integer = 0
                oPkgLetter.PackageLetterID = Request("LetterID")
                oPkgLetter.Load()
                oPkgLetter.Name = txtName.Text
                oPkgLetter.LetterContent = CKEditor1.Text
                oPkgLetter.Save()
                letterID = oPkgLetter.PackageLetterID
                oPkgLetter = Nothing
                Response.Redirect("editPackageLetter.aspx?LetterID=" & letterID)
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "alert", "alert('ACCESS DENIED.');", True)
        End If
    End Sub

    Protected Sub lbTagMap_Click(sender As Object, e As System.EventArgs) Handles lbTagMap.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/PackageLetterTags.aspx','win01',450,450);", True)

    End Sub
End Class
