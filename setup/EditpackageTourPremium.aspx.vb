Imports System.Data

Partial Class setup_EditpackageTourPremium
    Inherits System.Web.UI.Page

    Protected Sub RadioButtonList1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        If RadioButtonList1.SelectedIndex = 0 Then
            MultiView1.ActiveViewIndex = 0
        Else
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oPrem As New clsPremium
            Dim oPremBundle As New clsPremiumBundles
            ddPremium.DataSource = oPrem.List_Active
            ddPremium.DataTextField = "PremiumName"
            ddPremium.DataValueField = "PremiumID"
            ddPremium.DataBind()
            ddPremBundle.DataSource = oPremBundle.Get_Premium_Bundles
            ddPremBundle.DataTextField = "Name"
            ddPremBundle.DataValueField = "PremiumBundleID"
            ddPremBundle.DataBind()
            Dim i As Integer = 0
            For i = 0 To 10
                ddQty.Items.Add(New ListItem(i, i))
            Next

            oPremBundle = Nothing


            Dim ds = New clsComboItems().Load_ComboItems("PremiumStatus")

            ddStatus.DataSource = CType(ds.Select(DataSourceSelectArguments.Empty), DataView).ToTable()
            ddStatus.DataTextField = "ComboItem"
            ddStatus.DataValueField = "ComboItemID"
            ddStatus.DataBind()


            Dim oPkgTourPrem As New clsPackageTourPremium
            oPkgTourPrem.PackageTourPremiumID = Request("PkgTourPremiumID")
            oPkgTourPrem.Load()
            ddQty.SelectedValue = oPkgTourPrem.QtyAssigned
            txtCostEA.Text = oPkgTourPrem.CostEA

            With oPrem
                .PremiumID = oPkgTourPrem.PremiumID
                .Load()
                txtCostEA.Text = .Cost
            End With
            ddPremium.SelectedValue = oPkgTourPrem.PremiumID
            ddPremBundle.SelectedValue = oPkgTourPrem.BundleID
            cbOptional.Checked = oPkgTourPrem.OptionalPrem
            ddStatus.SelectedValue = oPkgTourPrem.PremiumStatusID

            If Request("PkgTourPremiumID") > 0 Then
                If oPkgTourPrem.BundleID > 0 Then
                    MultiView1.ActiveViewIndex = 1
                    RadioButtonList1.SelectedIndex = 1
                    RadioButtonList1.Items(1).Selected = True
                Else
                    MultiView1.ActiveViewIndex = 0
                End If
            Else
                MultiView1.ActiveViewIndex = 0
            End If
            oPkgTourPrem = Nothing
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click
        Dim oPkgTourPrem As New clsPackageTourPremium
        oPkgTourPrem.PackageTourPremiumID = Request("pkgTourPremiumID")
        oPkgTourPrem.Load()
        If Request("PkgTourPremiumID") = 0 Then
            oPkgTourPrem.PackageTourID = Request("PackageTourID")
            oPkgTourPrem.PackageID = Request("PackageID")
        End If

        If RadioButtonList1.SelectedIndex = 0 Then
            oPkgTourPrem.PremiumID = ddPremium.SelectedValue
            oPkgTourPrem.QtyAssigned = ddQty.SelectedValue
            oPkgTourPrem.CostEA = txtCostEA.Text
            oPkgTourPrem.BundleID = 0
            oPkgTourPrem.PremiumStatusID = ddStatus.SelectedValue

        Else
            oPkgTourPrem.PremiumID = 0
            oPkgTourPrem.QtyAssigned = 0
            oPkgTourPrem.CostEA = 0
            oPkgTourPrem.BundleID = ddPremBundle.SelectedValue

        End If
        oPkgTourPrem.OptionalPrem = cbOptional.Checked
        oPkgTourPrem.UserID = Session("UserDBID")
        oPkgTourPrem.Save()
        oPkgTourPrem = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.refreshPremiums();window.close();", True)
    End Sub

    Protected Sub ddPremium_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddPremium.SelectedIndexChanged

        With New clsPremium
            .PremiumID = ddPremium.SelectedItem.Value
            .Load()
            txtCostEA.Text = .Cost
        End With
    End Sub
End Class
