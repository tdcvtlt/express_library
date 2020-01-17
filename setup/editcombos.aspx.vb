
Partial Class setup_editcombos
    Inherits System.Web.UI.Page
    Dim ComboID As Integer = 0
    Dim ItemID As Integer = 0
    Dim oItem As clsComboItems
    Dim oCombo As clsCombos

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Select Case LCase(Request("Type"))
            Case "item"
                ItemID = IIf(IsNumeric(Request("ID")), Request("ID"), 0)
                ComboID = IIf(IsNumeric(Request("Parent")), Request("Parent"), 0)
                If Not IsPostBack Then Load_Item()
            Case "combo"
                ComboID = IIf(IsNumeric(Request("ID")), Request("ID"), 0)
                If Not IsPostBack Then Load_Combo()
            Case Else
                Close()
        End Select
    End Sub

    Private Sub Load_Item()
        MultiView1.ActiveViewIndex = 1
        If ComboID > 0 Then
            If ItemID = 0 Then

            Else
                oItem = New clsComboItems
                oItem.ComboID = ComboID
                oItem.ID = ItemID
                oItem.Load()
                txtItemDescription.Text = oItem.Description
                txtItemName.Text = oItem.Comboitem
                ckItemActive.Checked = oItem.Active
                oItem = Nothing
            End If
        Else
            Close()
        End If

    End Sub

    Private Sub Load_Combo()
        MultiView1.ActiveViewIndex = 0

        oCombo = New clsCombos
        oCombo.ComboID = ComboID
        oCombo.Load()
        txtComboDescription.Text = oCombo.Description
        txtComboName.Text = oCombo.ComboName
        oCombo = Nothing
    End Sub

    Private Sub Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Select Case LCase(Request("type"))
            Case "item"
                oItem = New clsComboItems
                oItem.ComboID = ComboID
                oItem.ID = ItemID
                oItem.Load()
                oItem.Description = txtItemDescription.Text
                oItem.Comboitem = txtItemName.Text
                oItem.Active = ckItemActive.Checked
                oItem.Save()
                lblErr.Text = oItem.Error_Message
                oItem = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.Refresh_Items();", True)
                Close()
            Case "combo"
                oCombo = New clsCombos
                oCombo.ComboID = ComboID
                oCombo.Load()
                oCombo.Description = txtComboDescription.Text
                oCombo.ComboName = txtComboName.Text
                oCombo.Save()
                lblErr.Text = oCombo.Error_Message
                oCombo = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.Refresh_Combos();", True)
                Close()
            Case Else
                Close()
        End Select
    End Sub
End Class
