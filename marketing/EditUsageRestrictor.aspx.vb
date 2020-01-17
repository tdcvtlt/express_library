
Partial Class marketing_EditUsageRestrictor
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            siSeason.Connection_String = Resources.Resource.cns
            siSeason.Label_Caption = ""
            siSeason.ComboItem = "Season"
            siSeason.Load_Items()

            Dim oUsageRestriction As New clsUsageRestriction
            lbRestrictions.DataSource = oUsageRestriction.Get_Restrictions()
            lbRestrictions.DataTextField = "Name"
            lbRestrictions.DataValueField = "UsageRestrictionID"
            lbRestrictions.DataBind()
            oUsageRestriction = Nothing
        End If
    End Sub
    Protected Sub chkMinimum_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkMinimum.Checked Then
            Table1.Rows(3).Visible = True
        Else
            Table1.Rows(3).Visible = False
            txtMinStay.Text = ""
        End If
    End Sub

    Protected Sub chkAllowDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('HERE');", True)
        If chkAllowDate.Checked Then
            Table1.Rows(5).Visible = True
            Table1.Rows(6).Visible = True
            chkDenyDate.Enabled = False
        Else
            Table1.Rows(5).Visible = False
            Table1.Rows(6).Visible = False
            dteAllowsDate.Selected_Date = ""
            dteAlloweDate.Selected_Date = ""
            chkDenyDate.Enabled = True
        End If
    End Sub

    Protected Sub chkDenyDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkDenyDate.Checked Then
            Table1.Rows(8).Visible = True
            Table1.Rows(9).Visible = True
            chkAllowDate.Enabled = False
        Else
            Table1.Rows(8).Visible = False
            Table1.Rows(9).Visible = False
            dteDenysDate.Selected_Date = ""
            dteDenyeDate.Selected_Date = ""
            chkAllowDate.Enabled = True
        End If
    End Sub

    Protected Sub chkRequireDays_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkRequireDays.Checked Then
            Table1.Rows(11).Visible = True
            Table1.Rows(12).Visible = True
        Else
            Table1.Rows(11).Visible = False
            Table1.Rows(12).Visible = False
            txtMaxDays.Text = ""
            txtMinDays.Text = ""
        End If
    End Sub

    Protected Sub chkDenySeason_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkDenySeason.Checked Then
            Table1.Rows(14).Visible = True
        Else
            Table1.Rows(14).Visible = False
        End If
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        If lbRestrictions.SelectedValue <> "" Then
            If CheckSecurity("UsageRestriction", "Edit", , , Session("UserDBID")) Then
                Table1.Visible = True
                Dim oUsageRestriction As New clsUsageRestriction
                oUsageRestriction.UsageRestrictionID = lbRestrictions.SelectedValue
                oUsageRestriction.Load()
                txtName.Text = oUsageRestriction.Name
                chkDenial.Checked = oUsageRestriction.DenyAll
                chkMinimum.Checked = oUsageRestriction.MinStayFlag
                txtMinStay.Text = oUsageRestriction.MinStay
                chkAllowDate.Checked = oUsageRestriction.AllowDateRangeFlag
                chkDenyDate.Checked = oUsageRestriction.DenyDateRangeFlag
                chkRequireDays.Checked = oUsageRestriction.DaysOutFlag
                txtMaxDays.Text = oUsageRestriction.MaxDaysOut
                txtMinDays.Text = oUsageRestriction.MinDaysOut
                chkDenySeason.Checked = oUsageRestriction.SeasonDenyFlag
                siSeason.Selected_ID = oUsageRestriction.SeasonID
                If oUsageRestriction.MinStayFlag Then
                    Table1.Rows(3).Visible = True
                Else
                    Table1.Rows(3).Visible = False
                End If
                If oUsageRestriction.AllowDateRangeFlag Then
                    Table1.Rows(5).Visible = True
                    Table1.Rows(6).Visible = True
                    chkDenyDate.Enabled = False
                    dteAllowsDate.Selected_Date = oUsageRestriction.StartDate
                    dteAlloweDate.Selected_Date = oUsageRestriction.EndDate
                    dteDenysDate.Selected_Date = ""
                    dteDenyeDate.Selected_Date = ""
                Else
                    Table1.Rows(5).Visible = False
                    Table1.Rows(6).Visible = False
                End If
                If oUsageRestriction.DenyDateRangeFlag Then
                    Table1.Rows(8).Visible = True
                    Table1.Rows(9).Visible = True
                    chkAllowDate.Enabled = False
                    dteAllowsDate.Selected_Date = ""
                    dteAlloweDate.Selected_Date = ""
                    dteDenysDate.Selected_Date = oUsageRestriction.StartDate
                    dteDenyeDate.Selected_Date = oUsageRestriction.EndDate
                Else
                    Table1.Rows(8).Visible = False
                    Table1.Rows(9).Visible = False
                End If
                If oUsageRestriction.DaysOutFlag Then
                    Table1.Rows(11).Visible = True
                    Table1.Rows(12).Visible = True
                Else
                    Table1.Rows(11).Visible = False
                    Table1.Rows(12).Visible = False
                End If
                If oUsageRestriction.SeasonDenyFlag Then
                    Table1.Rows(14).Visible = True
                Else
                    Table1.Rows(14).Visible = False
                End If
                oUsageRestriction = Nothing
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
            End If
        End If
    End Sub

    Protected Sub btnSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        If lbRestrictions.SelectedValue <> "" Then
            If CheckSecurity("UsageRestriction", "Add", , , Session("UserDBID")) Then
                Dim oUsageRestriction As New clsUsageRestriction2Contract
                oUsageRestriction.UsageRestriction2ContractID = 0
                oUsageRestriction.Load()
                oUsageRestriction.ContractID = Request("ContractID")
                oUsageRestriction.UsageRestrictionID = lbRestrictions.SelectedValue
                oUsageRestriction.DateCreated = System.DateTime.Now
                oUsageRestriction.PersonnelID = Session("UserDBID")
                oUsageRestriction.Save()
                lblErr.Text = oUsageRestriction.Err
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Restrictors();window.close();", True)
                oUsageRestriction = Nothing
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
            End If
        End If
    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInsert.Click
        If CheckSecurity("UsageRestriction", "Create", , , Session("UserDBID")) Then
            lbRestrictions.SelectedIndex = -1
            txtName.Text = ""
            chkDenial.Checked = False
            chkMinimum.Checked = False
            chkAllowDate.Checked = False
            chkDenyDate.Checked = False
            chkRequireDays.Checked = False
            Table1.Rows(5).Visible = False
            Table1.Rows(6).Visible = False
            Table1.Rows(8).Visible = False
            Table1.Rows(9).Visible = False
            Table1.Rows(11).Visible = False
            Table1.Rows(12).Visible = False
            Table1.Rows(14).Visible = False
            txtMaxDays.Text = ""
            txtMinDays.Text = ""
            txtMinStay.Text = ""
            dteDenysDate.Selected_Date = ""
            dteDenyeDate.Selected_Date = ""
            dteAllowsDate.Selected_Date = ""
            dteAlloweDate.Selected_Date = ""
            Table1.Visible = True
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ACCESS DENIED');", True)
        End If
    End Sub
    Private Sub Hide_Table()
        txtName.Text = ""
        chkDenial.Checked = False
        chkMinimum.Checked = False
        chkAllowDate.Checked = False
        chkDenyDate.Checked = False
        chkRequireDays.Checked = False
        Table1.Rows(5).Visible = False
        Table1.Rows(6).Visible = False
        Table1.Rows(8).Visible = False
        Table1.Rows(9).Visible = False
        Table1.Rows(11).Visible = False
        Table1.Rows(12).Visible = False
        Table1.Rows(14).Visible = False
        txtMaxDays.Text = ""
        txtMinDays.Text = ""
        txtMinStay.Text = ""
        dteDenysDate.Selected_Date = ""
        dteDenyeDate.Selected_Date = ""
        dteAllowsDate.Selected_Date = ""
        dteAlloweDate.Selected_Date = ""
        Table1.Visible = False
    End Sub
    Protected Sub btnURestrictSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnURestrictSave.Click
        'ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & lbRestrictions.SelectedValue & "');", True)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        Dim oUsageRestriction As New clsUsageRestriction
        If txtName.Text = "" Then
            bProceed = False
            sErr = "Please Enter a Restrictor Name."
        End If
        If chkMinimum.Checked And txtMinStay.Text = "" Then
            bProceed = False
            sErr = "Please Input a Minimum Stay Value."
        End If
        If chkAllowDate.Checked And (dteAllowsDate.Selected_Date = "" Or dteAlloweDate.Selected_Date = "") Then
            bProceed = False
            sErr = "Please Select a Start and End Date."
        End If
        If chkDenyDate.Checked And (dteDenysDate.Selected_Date = "" Or dteDenyeDate.Selected_Date = "") Then
            bProceed = False
            sErr = "Please Select a Start and End Date."
        End If
        If chkRequireDays.Checked And (txtMinDays.Text = "" Or txtMaxDays.Text = "") Then
            bProceed = False
            sErr = "Please Enter a Min Days Out and Max Days Out Value."
        End If
        If chkDenySeason.Checked And siSeason.Selected_ID < 1 Then
            bProceed = False
            sErr = "Please Select a Season."
        End If

        If bProceed Then
            If lbRestrictions.SelectedValue = "" Then
                If oUsageRestriction.Check_Restrictor(txtName.Text, 0) Then
                    oUsageRestriction.UsageRestrictionID = 0
                Else
                    bProceed = False
                    sErr = "This Restrictor Name is Already In Use."
                End If
            Else
                If oUsageRestriction.Check_Restrictor(txtName.Text, lbRestrictions.SelectedValue) Then
                    oUsageRestriction.UsageRestrictionID = lbRestrictions.SelectedValue
                Else
                    bProceed = False
                    sErr = "This Restrictor Name is Already In Use."
                End If
            End If
        End If
        If bProceed Then
            oUsageRestriction.Load()
            oUsageRestriction.Name = txtName.Text
            oUsageRestriction.DenyAll = chkDenial.Checked
            oUsageRestriction.MinStayFlag = chkMinimum.Checked
            oUsageRestriction.AllowDateRangeFlag = chkAllowDate.CHecked
            oUsageRestriction.DenyDateRangeFlag = chkDenyDate.Checked
            oUsageRestriction.DaysOutFlag = chkRequireDays.Checked
            oUsageRestriction.SeasonDenyFlag = chkDenySeason.Checked
            If chkMinimum.Checked Then
                oUsageRestriction.MinStay = txtMinStay.Text
            End If
            If chkRequireDays.Checked Then
                oUsageRestriction.MinDaysOut = txtMinDays.Text
                oUsageRestriction.MaxDaysOut = txtMaxDays.Text
            End If
            If chkDenySeason.Checked Then
                oUsageRestriction.SeasonID = siSeason.Selected_ID
            End If
            If chkAllowDate.Checked Then
                oUsageRestriction.StartDate = dteAllowsDate.Selected_Date
                oUsageRestriction.EndDate = dteAlloweDate.Selected_Date
            End If
            If chkDenyDate.Checked Then
                oUsageRestriction.StartDate = dteDenysDate.Selected_Date
                oUsageRestriction.EndDate = dteDenyeDate.Selected_Date
            End If
            oUsageRestriction.Save()
            lbRestrictions.DataSource = oUsageRestriction.Get_Restrictions()
            lbRestrictions.DataTextField = "Name"
            lbRestrictions.DataValueField = "UsageRestrictionID"
            lbRestrictions.DataBind()
            oUsageRestriction = Nothing
            Hide_Table()
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub btnCXL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCXL.Click
        Hide_Table()
    End Sub
End Class
