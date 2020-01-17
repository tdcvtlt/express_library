
Partial Class setup_EditPremiumBundlePremium
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim i As Integer = 0
            For i = 1 To 10
                ddQty.Items.Add(New ListItem(i, i))
            Next

            Dim oPremBundlePremiums As New clsPremiumBundlePremiums
            ddPremium.DataSource = oPremBundlePremiums.list_Premiums(Request("ID"))
            ddPremium.DataTextField = "PremiumName"
            ddPremium.DataValueField = "PremiumID"
            ddPremium.DataBind()


            oPremBundlePremiums.Premium2BundleID = Request("ID")
            oPremBundlePremiums.Load()
            ddPremium.SelectedValue = oPremBundlePremiums.PremiumID
            ddQty.SelectedValue = oPremBundlePremiums.Qty
            txtCostEA.Text = oPremBundlePremiums.CostEA
            cbActive.Checked = oPremBundlePremiums.Active

            oPremBundlePremiums = Nothing
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim oPremBundlePremiums As New clsPremiumBundlePremiums
        oPremBundlePremiums.PremiumBundleID = Request("ID")
        oPremBundlePremiums.Load()
        oPremBundlePremiums.UserID = Session("UserDBID")
        If Request("ID") = 0 Then
            oPremBundlePremiums.PremiumBundleID = Request("BundleID")
        End If
        oPremBundlePremiums.CostEA = txtCostEA.Text
        oPremBundlePremiums.PremiumID = ddPremium.SelectedValue
        oPremBundlePremiums.Qty = ddQty.SelectedValue
        oPremBundlePremiums.Active = cbActive.Checked
        oPremBundlePremiums.Save()
        oPremBundlePremiums = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.refreshPremiums();window.close();", True)
    End Sub
End Class
