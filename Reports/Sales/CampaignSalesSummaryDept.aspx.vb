Imports System.Data
Imports System.Linq

Imports CrystalDecisions.CrystalReports.Engine



Partial Class Reports_Sales_CampaignSalesSummaryDept
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim ReportPath As String = "ReportFiles/CAMPAIGNSALESSUMMARYDEPT.rpt"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then

        Else
            Session("Report") = Nothing
            BindToCampaigns()

            Setup_Report()
        End If

        If hfShowReport.Value = 1 Then CRViewer.ReportSource = Session("Report")
    End Sub

    Private Sub Setup_Report()
        If Session("Report") Is Nothing Then
            Report.Load(Server.MapPath(ReportPath))
            Report.FileName = Server.MapPath(ReportPath)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Set_Params()
            Session.Add("Report", Report)
        Else
            Report = Session("Report")
            If Report.FileName <> Server.MapPath(ReportPath) Then
                Session("Report") = Nothing
                Setup_Report()
            End If
        End If
    End Sub

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRunReport.Click
        If dpFrom.Selected_Date <> "" And dpTo.Selected_Date <> "" Then
            hfShowReport.Value = 1
            Setup_Report()
            CRViewer.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()

        Report.SetParameterValue("STARTDATE", CDate(IIf(dpFrom.Selected_Date <> "", dpFrom.Selected_Date, Date.Today)))
        Report.SetParameterValue("ENDDATE", CDate(IIf(dpTo.Selected_Date <> "", dpTo.Selected_Date, Date.Today)))
        Report.SetParameterValue("OW", IIf(cbIncludeOW.Checked, 1, 0))


        Dim campaign(ddCampaign.Items.Count - 1) As String

        If ddCampaign.SelectedItem.Text.Equals("All") Then

            Dim i As Integer = 0
            For Each item As ListItem In ddCampaign.Items
                If item.Text.Contains("All") = False Then
                    campaign(i) = "'" & item.Text & "'"
                    i += 1
                End If
            Next

            Dim tmp As String = String.Join(",", campaign)
            tmp = tmp.Substring(0, tmp.Length - 1)
            Report.SetParameterValue("CAMPAIGNNAME", tmp)

        Else
            Report.SetParameterValue("CAMPAIGNNAME", "'" & ddCampaign.SelectedItem.Text & "'")
        End If



    End Sub



    Private Sub BindToCampaigns()
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns


        ddCampaign.Items.Add(New ListItem("All", "All"))
        ddCampaign.DataBind()
        ddCampaign.AppendDataBoundItems = True

        ds.SelectCommand = "Select * from t_Campaign where active = 1 order by name"
        ddCampaign.DataSource = ds
        ddCampaign.DataTextField = "Name"
        ddCampaign.DataValueField = "CampaignID"
        ddCampaign.DataBind()

    End Sub




End Class
