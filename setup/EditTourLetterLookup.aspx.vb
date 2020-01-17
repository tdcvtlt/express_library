
Partial Class setup_EditTourLetterLookup
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("view") = 1 Then
                With New clsTourLetter2Campaign
                    .TourLetter2CampaignID = Request("ID")
                    .Load()

                    mvView1Active.Checked = .Active
                    ddlCampaign.DataSource = .Campaigns
                    ddlCampaign.DataTextField = "name"
                    ddlCampaign.DataValueField = "campaignid"
                    ddlCampaign.DataBind()

                    ddlCampaign.SelectedValue = .CampaignID
                End With
                mv.SetActiveView(mvView1)
            ElseIf Request("view") = 2 Then

                With New clsTourLetter2Location
                    .TourLetter2LocationID = Request("ID")
                    .Load()

                    mvView2Active.Checked = .Active
                    siLocation.Connection_String = Resources.Resource.cns
                    siLocation.ComboItem = "tourlocation"
                    siLocation.Selected_ID = .TourLocationID
                    siLocation.Label_Caption = ""
                    siLocation.Load_Items()
                End With
                mv.SetActiveView(mvView2)
            End If
        End If
    End Sub

    Protected Sub mvView1Save_Click(sender As Object, e As System.EventArgs) Handles mvView1Save.Click
        With New clsTourLetter2Campaign
            .TourLetter2CampaignID = Request("ID")
            .Load()

            .TourLetterID = Request("LetterID")
            .Active = mvView1Active.Checked
            .CampaignID = ddlCampaign.SelectedItem.Value
            .Save()
            ClientScript.RegisterClientScriptBlock(Me.GetType, DateTime.Now.ToLongTimeString(), "window.opener.Refresh_Campaign();window.close();", True)
        End With
    End Sub

    Protected Sub mvView2Save_Click(sender As Object, e As System.EventArgs) Handles mvView2Save.Click
        With New clsTourLetter2Location
            .TourLetter2LocationID = Request("ID")
            .Load()

            .TourLetterID = Request("LetterID")
            .Active = mvView2Active.Checked
            .TourLocationID = siLocation.Selected_ID
            .Save()

            ClientScript.RegisterClientScriptBlock(Me.GetType, DateTime.Now.ToLongTimeString(), "window.opener.Refresh_Location();window.close();", True)
        End With
    End Sub
End Class
