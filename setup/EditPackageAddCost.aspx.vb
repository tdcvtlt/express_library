
Partial Class setup_EditPackageAddCost
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oPkgAddCost As New clsPackageAdditionalCost
            oPkgAddCost.PackageNightRateID = Request("ID")
            oPkgAddCost.Load()
            dteSDate.Selected_Date = oPkgAddCost.StartDate
            dteEDate.Selected_Date = oPkgAddCost.EndDate
            txtAmount.Text = oPkgAddCost.Amount
            ddPriority.SelectedValue = oPkgAddCost.Priority
            cbActive.Checked = oPkgAddCost.Active
            oPkgAddCost = Nothing
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim sErr As String = ""
        If dteEDate.Selected_Date = "" Or dteSDate.Selected_Date = "" Then
            sErr = "Please Select a Start Date and End Date."
        ElseIf DateTime.Compare(dteSDate.Selected_Date, dteEDate.Selected_Date) > 0 Then
            sErr = "Please Make Sure Start Date is Earlier than End Date."
        ElseIf txtAmount.Text = "" Or Not (IsNumeric(txtAmount.Text)) Then
            sErr = "Please Enter a Valid Amount."
        End If

        If sErr = "" Then
            Dim opkgAddCost As New clsPackageAdditionalCost
            opkgAddCost.PackageNightRateID = Request("ID")
            opkgAddCost.Load()
            opkgAddCost.UserID = Session("UserDBID")
            If Request("ID") = 0 Then
                opkgAddCost.PackageID = Request("PackageID")
            End If
            opkgAddCost.StartDate = dteSDate.Selected_Date
            opkgAddCost.EndDate = dteEDate.Selected_Date
            opkgAddCost.Amount = txtAmount.Text
            opkgAddCost.Active = cbActive.Checked
            opkgAddCost.Priority = ddPriority.SelectedValue
            opkgAddCost.Save()
            opkgAddCost = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.refreshAddCosts();window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub
End Class
