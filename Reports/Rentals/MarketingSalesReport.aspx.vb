Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class Reports_Rentals_MarketingSalesReport
    Inherits System.Web.UI.Page


    Dim Report As New ReportDocument
    Dim REPORT_PATH As String = "reportfiles/MarketingSalesReport.rpt"

    Private campaigns_selected As String
   
    Private cnn As String = Resources.Resource.cns

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then

            Dim c As New Campaigns()
            c.Load(String.Empty)

            listboxCampaignAll.DataSource = c.Campaign
            listboxCampaignAll.DataTextField = "Name"
            listboxCampaignAll.DataValueField = "CampaignID"
            listboxCampaignAll.DataBind()

        Else

            If Session("report") IsNot Nothing Then
                CrystalReportViewer1.ReportSource = DirectCast(Session("report"), ReportDocument)
            End If
        End If

    End Sub


    Private Sub Transfer_Item(ByRef lbSource As ListBox, ByRef lbDestination As ListBox)
        If lbSource.SelectedIndex >= 0 Then
            Dim lItem As New ListItem
            lItem.Value = lbSource.SelectedValue
            lItem.Text = lbSource.SelectedItem.Text
            lbDestination.Items.Add(lItem)
            lbSource.Items.Remove(lItem)
        End If

        Session("report") = Nothing
    End Sub



    Protected Sub btnRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRunReport.Click
        Session("report") = Nothing
        setup_report()
    End Sub

    Private Sub setup_report()

        If getCampaignById().Length > 0 Then                    

            Try

                Report.Load(Server.MapPath(REPORT_PATH))
                Report.FileName = Server.MapPath(REPORT_PATH)
                Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

                Report.Subreports(0).DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
                Report.Subreports(0).DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

                Report.Subreports(1).DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
                Report.Subreports(1).DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

                Report.SetParameterValue("SDATE", CDate(IIf(dfStartDate.Selected_Date <> "", dfStartDate.Selected_Date, Date.Today)))
                Report.SetParameterValue("EDATE", CDate(IIf(dfEndDate.Selected_Date <> "", dfEndDate.Selected_Date, Date.Today)))
                Report.SetParameterValue("CAMPID", String.Format("{0}", (getCampaignById() & ",0").Replace("'", "")))

                Session.Add("report", Report)
                CrystalReportViewer1.ReportSource = Report

            Catch ex As Exception
                Response.Write(String.Format("Custom error: <br/> <strong>{0}</strong>", ex.Message))
            End Try
        End If
    End Sub

    Private Function getCampaignById() As String

        Dim i As Integer = 0
        Dim tmp As String = String.Empty

        If listboxCampaignSelects.Items.Count > 0 Then
            For Each li As ListItem In listboxCampaignSelects.Items

                campaigns_selected &= IIf(i = listboxCampaignSelects.Items.Count - 1, _
                                          String.Format("'{0}'", li.Text), _
                                          String.Format("'{0}',", li.Text))

                tmp &= IIf(i = listboxCampaignSelects.Items.Count - 1, _
                                          String.Format("'{0}'", li.Value), _
                                          String.Format("'{0}',", li.Value))
                i += 1
            Next
        Else

            For Each li As ListItem In listboxCampaignAll.Items
                campaigns_selected &= IIf(i = listboxCampaignAll.Items.Count - 1, _
                                          String.Format("'{0}'", li.Text), _
                                          String.Format("'{0}',", li.Text))
                i += 1
            Next
        End If

        Return tmp
    End Function

    Protected Sub buttonForwardCampaign_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonForwardCampaign.Click
        Transfer_Item(listboxCampaignAll, listboxCampaignSelects)
    End Sub

    Protected Sub buttonBackwardCampaign_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonBackwardCampaign.Click
        Transfer_Item(listboxCampaignSelects, listboxCampaignAll)
    End Sub

   
    Private Class Connectivity
        Protected con As SqlConnection
        Protected ada As SqlDataAdapter
        Protected dt As DataTable
        Protected sql As String = String.Empty

    End Class

    Private Class Campaigns
        Inherits Connectivity
        Implements ILoad

        Private _campaignDt As DataTable

        Public Sub New()
            con = New SqlConnection(Resources.Resource.cns)
        End Sub

        Public Sub Load(ByVal id As String) Implements ILoad.Load

            sql = "select Name, CampaignID from t_Campaign where active = 1 order by Name"
            Using ada = New SqlDataAdapter(sql, con)

                dt = New DataTable()

                ada.Fill(dt)
                _campaignDt = dt
            End Using
        End Sub

        Public ReadOnly Property Campaign As DataTable
            Get
                Return _campaignDt
            End Get
        End Property
    End Class
 
    Public Interface ILoad

        Sub Load(ByVal id As String)

    End Interface



End Class
