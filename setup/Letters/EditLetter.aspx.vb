
Partial Class setup_Letters_EditLetter
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oLetter As New clsLetters
            oLetter.LetterID = Request("LetterID")
            oLetter.Load()
            txtName.Text = oLetter.Name
            CKEditor1.Text = oLetter.LetterContent
            siType.Connection_String = Resources.Resource.cns
            siType.ComboItem = "LetterType"
            siType.Load_Items()
            siType.Selected_ID = oLetter.TypeID
            oLetter = Nothing
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'If CheckSecurity("Packages", "EditLetters", , , Session("UserDBID")) Then
        If CKEditor1.Text = "" Then

        Else
            Dim oLetter As New clsLetters
            Dim letterID As Integer = 0
            oLetter.LetterID = Request("LetterID")
            oLetter.Load()
            oLetter.Name = txtName.Text
            oLetter.LetterContent = CKEditor1.Text
            oLetter.TypeID = siType.Selected_ID
            oLetter.Save()
            letterID = oLetter.LetterID
            oLetter = Nothing
            Response.Redirect("editLetter.aspx?LetterID=" & letterID)
        End If
        'Else
        'ClientScript.RegisterClientScriptBlock(Me.GetType, "alert", "alert('ACCESS DENIED.');", True)
        'End If
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.click
        If Request("LetterID") = "" Or Not (IsNumeric(Request("LetterID"))) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Save", "alert('The Letter must be saved in order to Preview.');", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Pop", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/letters/LetterPreview.aspx?ID=" & Request("LetterID") & "&Key=296816','win01',350,350);", True)
        End If
    End Sub

    Protected Sub lbTagMap_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTagMap.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/letters/LetterTags.aspx?typeid=" & siType.Selected_ID & "','win01',450,450);", True)

    End Sub
End Class
