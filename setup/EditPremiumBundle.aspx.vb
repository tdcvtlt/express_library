
Partial Class setup_EditPremiumBundle
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oPremBundle As New clsPremiumBundles
            oPremBundle.PremiumBundleID = Request("PremiumBundleID")
            oPremBundle.Load()
            txtBundleDesc.Text = oPremBundle.Description
            txtBundleID.Text = Request("PremiumBundleID")
            txtBundleName.Text = oPremBundle.Name
            oPremBundle = Nothing
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub


    Protected Sub LinkButtonPremiumBundle_Click(sender As Object, e As System.EventArgs) Handles LinkButtonPremiumBundle.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub LinkButtonPremiums_Click(sender As Object, e As System.EventArgs) Handles LinkButtonPremiums.Click
        If txtBundleID.Text > 0 Then
            Dim oPremBundlePrems As New clsPremiumBundlePremiums
            gvPremiums.DataSource = oPremBundlePrems.get_Bundle_Premiums(txtBundleID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvPremiums.DataKeyNames = sKeys
            gvPremiums.DataBind()
            oPremBundlePrems = Nothing
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim oPremBundle As New clsPremiumBundles
        oPremBundle.PremiumBundleID = txtBundleID.Text
        oPremBundle.Load()
        oPremBundle.UserID = Session("UserDBID")
        oPremBundle.Name = txtBundleName.Text
        oPremBundle.Description = txtBundleDesc.Text
        oPremBundle.Save()
        Response.Write(oPremBundle.Err)
        Response.Redirect("EditPremiumBundle.aspx?PremiumBundleID=" & oPremBundle.PremiumBundleID)
        oPremBundle = Nothing
    End Sub

    Protected Sub gvPremiums_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvPremiums.SelectedIndexChanged
        Dim row As GridViewRow = gvPremiums.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditPremiumBundlePremium.aspx?ID=" & row.Cells(1).Text & "','win01',350,350);", True)

    End Sub

    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditPremiumBundlePremium.aspx?ID=0&BundleID=" & txtBundleID.Text & "','win01',350,350);", True)
    End Sub
End Class
