
Partial Class setup_EditRegCardItem
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim oRegCardItem As New clsRegCardItems
        oRegCardItem.RegCardItemID = Request("ID")
        oRegCardItem.Load()
        If Request("ID") = 0 Then
            oRegCardItem.RegCardID = Request("RegCardID")
        End If
        oRegCardItem.Item = txtItem.Text.Replace(vbNewLine, "<br>")
        oRegCardItem.Priority = ddPriority.SelectedValue
        oRegCardItem.Active = cbActive.Checked
        oRegCardItem.UserID = Session("UserDBID")
        oRegCardItem.Save()
        oRegCardItem = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Items();window.close();", True)

        'Label1.Text = txtItem.Text.Replace(vbNewLine, "<br>")
        '        Response.Write(txtItem.Text.Replace(vbNewLine, "<br>"))
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRegCardItem As New clsRegCardItems
            Dim oCombo As New clsComboItems
            oRegCardItem.RegCardItemID = Request("ID")
            oRegCardItem.Load()
            For i = 1 To 20
                ddPriority.Items.Add(New ListItem(i, i))
            Next
            ddPriority.SelectedValue = oRegCardItem.Priority
            cbActive.Checked = oRegCardItem.Active
            txtItem.Text = oRegCardItem.Item.Replace("<br>", vbNewLine)
            oRegCardItem = Nothing
        End If
    End Sub
End Class
