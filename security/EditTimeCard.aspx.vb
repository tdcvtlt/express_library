
Partial Class security_EditTimeCard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If Request("CardID") <> 0 Then
                Dim oTimeCard As New clsPersonnelTimeCards
                oTimeCard.CardID = Request("CardID")
                oTimeCard.Load()
                txtDesc.Text = oTimeCard.Description
                txtSwipe.Attributes("Value") = oTimeCard.Swipe
                cbActive.Checked = oTimeCard.Active
                oTimeCard = Nothing
            End If
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oTimeCard As New clsPersonnelTimeCards
        If oTimeCard.Validate_Code(txtSwipe.Text, Request("PersonnelID")) Then
            If Request("CardID") = 0 Then
                oTimeCard.CardID = 0
            Else
                oTimeCard.CardID = Request("CardID")
            End If
            oTimeCard.Load()
            oTimeCard.PersonnelID = Request("PersonnelID")
            oTimeCard.Swipe = txtSwipe.Text
            oTimeCard.Description = txtDesc.Text
            oTimeCard.Active = cbActive.Checked
            oTimeCard.Save()
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Cards();window.close();", True)
        Else
            lblErr.Text = oTimeCard.Err
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('This Code is Already In Use.');", True)
        End If
        oTimeCard = Nothing
    End Sub
End Class
