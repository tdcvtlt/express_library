
Partial Class marketing_EditCampaign
    Inherits System.Web.UI.Page


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CInt(txtCampaignID.Text) > 0 Then
            If CheckSecurity("Campaigns", "Edit", , , Session("UserDBID")) Then
                Save()
            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "AD", "alert('Access to Modify a Campaign is denied');", True)
            End If
        Else
            If CheckSecurity("Campaigns", "Add", , , Session("UserDBID")) Then
                Save()
            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "AD", "alert('Access to Add a Campaign is denied');", True)
            End If
        End If

    End Sub

    Protected Sub Save()
        Dim oCampaign As New clsCampaign
        oCampaign.CampaignID = txtCampaignID.Text
        oCampaign.Load()
        oCampaign.Name = txtName.Text
        oCampaign.Description = txtDescription.Text
        oCampaign.StartDate = dfStartDate.Selected_Date
        oCampaign.EndDate = dfEndDate.Selected_Date
        oCampaign.TypeID = siType.Selected_ID
        oCampaign.SubTypeID = siSubType.Selected_ID
        oCampaign.MaxCostPerTour = txtMaxCostPerTour.Text
        oCampaign.PromoNights = txtPromoNights.Text
        oCampaign.PromoRate = txtPromoRate.Text
        oCampaign.AdditionalNightRate = txtAdditionNightRate.Text
        oCampaign.AdditionalGuestRate = txtAdditionalGuestRate.Text
        oCampaign.DepartmentProgramID = siDepartmentProgram.Selected_ID
        'oCampaign.DepartmentID = siDepartment.Selected_ID
        oCampaign.LocationID = 1
        oCampaign.Active = ckActive.Checked
        oCampaign.UserID = Session("UserDBID")
        oCampaign.Save()
        If oCampaign.Error_Message <> "" Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Error", "alert('" & Replace(oCampaign.Error_Message, "'", "\'") & "');", True)
        Else
            Response.Redirect("campaigns.aspx")
        End If
        oCampaign = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oCampaign As New clsCampaign
            If CheckSecurity("Campaigns", "View", , , Session("UserDBID")) Then
                MultiView1.ActiveViewIndex = 0
                '*** Create view events *** '
                If IsNumeric(Request("CampaignID")) Then
                    If CInt(Request("CampaignID")) > 0 Then
                        Dim oE As New clsEvents
                        Dim sErr As String = ""

                        If Not (oE.Find_View_Event("CampaignID", Request("CampaignID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                            If sErr <> "" Then Response.Write(sErr)
                            If Not (oE.Create_View_Event("CampaignID", Request("CampaignID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                                If sErr <> "" Then Response.Write(sErr)
                            End If
                        End If
                        oE = Nothing
                    End If
                End If

                '*** End View Events *** '
                Load_Lookups()
                'oCampaign.CampaignID = IIf(IsNumeric(Request("CampaignID")), Request("CampaignID"), 0)
                'oCampaign.Load()
                Set_Fields()
                'Label6.Text = oPros.Error_Message
            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "AD", "alert('Access to View a Campaign is denied');", True)
            End If
            oCampaign = Nothing
        End If
    End Sub

    Protected Sub Set_Fields()
        Dim oCampaign As New clsCampaign
        oCampaign.CampaignID = IIf(IsNumeric(Request("CampaignID")), Request("CampaignID"), 0)
        oCampaign.Load()
        txtCampaignID.Text = oCampaign.CampaignID
        txtName.Text = oCampaign.Name
        txtDescription.Text = oCampaign.Description
        dfStartDate.Selected_Date = oCampaign.StartDate
        dfEndDate.Selected_Date = oCampaign.EndDate
        siType.Selected_ID = oCampaign.TypeID
        siSubType.Selected_ID = oCampaign.SubTypeID
        txtMaxCostPerTour.Text = FormatCurrency(oCampaign.MaxCostPerTour).ToString.Replace("$", "")
        txtPromoNights.Text = oCampaign.PromoNights
        txtPromoRate.Text = oCampaign.PromoRate
        txtAdditionNightRate.Text = oCampaign.AdditionalNightRate
        txtAdditionalGuestRate.Text = oCampaign.AdditionalGuestRate
        siDepartmentProgram.Selected_ID = oCampaign.DepartmentProgramID
        'siDepartment.Selected_ID = oCampaign.DepartmentID
        ckActive.Checked = oCampaign.Active
        oCampaign = Nothing
    End Sub

    Protected Sub Load_Lookups()
        siType.Connection_String = Resources.Resource.cns
        siType.Label_Caption = ""
        siType.ComboItem = "CampaignType"
        siType.Load_Items()
        siSubType.Connection_String = Resources.Resource.cns
        siSubType.Label_Caption = ""
        siSubType.ComboItem = "CampaignSubType"
        siSubType.Load_Items()
        Dim oCampaign As New clsCampaign
        oCampaign.CampaignID = IIf(IsNumeric(Request("CampaignID")), Request("CampaignID"), 0)
        oCampaign.Load()
        'siDepartment.Connection_String = Resources.Resource.cns
        'siDepartment.Label_Caption = ""
        'siDepartment.ComboItem = "Department"
        'siDepartment.Selected_ID = oCampaign.DepartmentID
        'siDepartment.Load_Items()
        oCampaign = Nothing
        siDepartmentProgram.Connection_String = Resources.Resource.cns
        siDepartmentProgram.Label_Caption = ""
        siDepartmentProgram.ComboItem = "DepartmentProgram"
        siDepartmentProgram.Load_Items()

    End Sub

    Protected Sub Premium_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Campaign.Click
        If CheckSecurity("Campaigns", "View", , , Session("UserDBID")) Then MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Events_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events.Click
        If txtCampaignID.Text > 0 Then
            MultiView1.ActiveViewIndex = 1
            Events1.KeyField = "CampaignID"
            Events1.KeyValue = txtCampaignID.Text
            Events1.List()
        End If
    End Sub

    Protected Sub Departments_Click(sender As Object, e As EventArgs) Handles Departments.Click
        If txtCampaignID.Text > 0 Then
            Dim oCamp2Dept As New clsCampaign2Department
            gvDept.DataSource = oCamp2Dept.List_Departments(txtCampaignID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvDept.DataKeyNames = sKeys
            gvDept.DataBind()
            oCamp2Dept = Nothing
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub gvDept_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvDept.SelectedIndexChanged
        Dim row As GridViewRow = gvDept.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/EditCamp2Dept.aspx?ID=" & row.Cells(1).Text & "','win01',400,400);", True)
    End Sub

    Protected Sub lbAdd_Click(sender As Object, e As EventArgs) Handles lbAdd.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/EditCamp2Dept.aspx?ID=0&CampaignID=" & txtCampaignID.Text & "','win01',400,400);", True)
    End Sub
End Class
