
Partial Class general_EditEmail
    Inherits System.Web.UI.Page
    Dim oEmail As New clsEmail

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        oEmail.EmailID = IIf(IsNumeric(Request("EmailID")), Request("EmailID"), 0)
        oEmail.ProspectID = IIf(IsNumeric(Request("ProspectID")), Request("ProspectID"), 0)
        If Not IsPostBack Then
            oEmail.Load()
            Load_Values()
            lblError.Text = oEmail.Error_Message
        End If
    End Sub

    Private Sub Load_Values()
        txtEmailID.Text = oEmail.EmailID
        txtProspectID.Text = oEmail.ProspectID
        txtEmail.Text = oEmail.Email
        ckActive.Checked = oEmail.IsActive
        ckPrimary.Checked = oEmail.IsPrimary
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        With oEmail
            .ProspectID = txtProspectID.Text
            .Email = txtEmail.Text
            .IsPrimary = ckPrimary.Checked
            .IsActive = ckActive.Checked
            .UserID = Session("UserDBID")
            .Save()
            lblError.Text = .Error_Message
        End With
        If oEmail.Error_Message = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", "window.opener.Refresh_Email();window.close();", True)
        End If
    End Sub
End Class
