Imports Microsoft.VisualBasic
Partial Class general_EditDefaultPersonnel
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If CheckSecurity("ContractWizardDefaultPersonnel", "Edit", , , Session("UserDBID")) Or CheckSecurity("ContractWizardDefaultPersonnel", "View", , , Session("UserDBID")) Then
                MultiView1.ActiveViewIndex = 0
                Dim oComboitems As New clsComboItems
                ddBillingCode.Items.Add(New ListItem("ALL", 0))
                ddBillingCode.DataSource = oComboitems.Load_ComboItems("BillingCode")
                ddBillingCode.DataTextField = "ComboItem"
                ddBillingCode.DataValueField = "ComboitemID"
                ddBillingCode.AppendDataBoundItems = True
                ddBillingCode.DataBind()

                ddTitle.DataSource = oComboitems.Load_ComboItems("Personneltitle")
                ddTitle.DataTextField = "ComboItem"
                ddTitle.DataValueField = "ComboItemID"
                ddTitle.DataBind()

                Dim oCamp As New clsCampaign
                ddCampaigns.Items.Add(New ListItem("ALL", 0))
                ddCampaigns.DataSource = oCamp.Load_Lookup()
                ddCampaigns.DataTextField = "Name"
                ddCampaigns.DataValueField = "CampaignID"
                ddCampaigns.AppendDataBoundItems = True
                ddCampaigns.DataBind()

                Dim oPers As New clsPersonnel
                ddPersonnel.DataSource = oPers.List()
                ddPersonnel.DataTextField = "Name"
                ddPersonnel.DataValueField = "ID"
                ddPersonnel.DataBind()

                Load_Items()
                oComboitems = Nothing
                oCamp = Nothing
                oPers = Nothing
            Else

            End If
        End If
    End Sub

    Protected Sub Load_Items()
        Dim oPers2Code As New clsBillingCode2Personnel
        gvConPersonnel.dataSource = oPers2Code.List_Items(ddBillingCode.SelectedValue, ddCampaigns.SelectedValue)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvConPersonnel.DataKeyNames = sKeys
        gvConPersonnel.DataBind()
        oPers2Code = Nothing
    End Sub

    Protected Sub ddBillingCode_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddBillingCode.SelectedIndexChanged
        Load_Items()
    End Sub

    Protected Sub ddCampaigns_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddCampaigns.SelectedIndexChanged
        Load_Items()
    End Sub

    Protected Sub gvConPersonnel_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then

            e.Row.Cells(0).Visible = False
        End If
    End Sub

    Protected Sub gvConPersonnel_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvConPersonnel.RowCommand
        Dim ID As Integer
        ID = Convert.ToInt32(gvConPersonnel.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("Remove") = 0 Then
            If CheckSecurity("ContractWizardDefaultPersonnel", "Edit", , , Session("UserDBID")) Then
                Dim oPers2Code As New clsBillingCode2Personnel
                oPers2Code.Remove_Item(ID)
                Load_Items()
                oPers2Code = Nothing
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Access Denied');", True)
            End If
        End If
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not (CheckSecurity("ContractWizardDefaultPersonnel", "Edit", , , Session("UserDBID"))) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Access Denied');", True)
        Else
            If txtFixedAmount.Text = "" Or Not (IsNumeric(txtFixedAmount.Text)) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter A Valid Fixed Amount Value.');", True)
            ElseIf txtCommPercentage.Text = "" Or Not (IsNumeric(txtCommPercentage.Text)) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter A Valid Percentage Amount Value.');", True)
            ElseIf ddPersonnel.SelectedValue = "" Or ddTitle.SelectedValue = "" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Access Denied');", True)
            Else
                Dim oPers2Code As New clsBillingCode2Personnel
                oPers2Code.UserID = Session("UserDBID")
                oPers2Code.BillingCode2PersonnelID = 0
                oPers2Code.PersonnelID = ddPersonnel.SelectedValue
                oPers2Code.TitleID = ddTitle.SelectedValue
                oPers2Code.BillingCodeID = ddBillingCode.SelectedValue
                oPers2Code.CampaignID = ddCampaigns.SelectedValue
                oPers2Code.FixedAmount = txtFixedAmount.Text
                oPers2Code.PercentageAmount = txtCommPercentage.Text
                oPers2Code.Save()
                oPers2Code = Nothing
                Load_Items()
            End If
        End If
    End Sub

    Protected Sub Main_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Main.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Events_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events.Click
        MultiView1.ActiveViewIndex = 1
        Events1.KeyField = "BillingCode2PersonnelID"
        Events1.KeyValue = 0
        Events1.List()

    End Sub
End Class
