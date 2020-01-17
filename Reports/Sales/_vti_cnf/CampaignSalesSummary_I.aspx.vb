Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class Reports_Sales_CampaignSalesSummary_I
    Inherits System.Web.UI.Page

    Private LB_ACTIVE_CAMPAIGNS As ListBox
    Private LB_CHOSEN_CAMPAIGNS As ListBox
    Private TB_CAMPAIGNS As DataTable

    Private CRYSTAL_REPORT As New ReportDocument
    Private REPORT_FILE_NAME As String = "REPORTFILES/CAMPAIGNSALESSUMMARY_I.rpt"

    Private campaigns_selected As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LB_ACTIVE_CAMPAIGNS = listboxCampaignAll
        LB_CHOSEN_CAMPAIGNS = listboxCampaignSelects

        If Me.IsPostBack = False Then

            Dim o As New Campaigns()
            o.Load(String.Empty)

            LB_ACTIVE_CAMPAIGNS.DataSource = o.Campaign
            LB_ACTIVE_CAMPAIGNS.DataTextField = "Name"
            LB_ACTIVE_CAMPAIGNS.DataValueField = "CampaignID"
            LB_ACTIVE_CAMPAIGNS.DataBind()
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


    Private Sub setup_report()

        If getCampaignById().Length > 0 Then

            Try

                CRYSTAL_REPORT.Load(Server.MapPath(REPORT_FILE_NAME))
                CRYSTAL_REPORT.FileName = Server.MapPath(REPORT_FILE_NAME)
                CRYSTAL_REPORT.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

                CRYSTAL_REPORT.SetParameterValue("SDATE", CDate(IIf(dfStartDate.Selected_Date <> "", dfStartDate.Selected_Date, Date.Today)))
                CRYSTAL_REPORT.SetParameterValue("EDATE", CDate(IIf(dfEndDate.Selected_Date <> "", dfEndDate.Selected_Date, Date.Today)))
                CRYSTAL_REPORT.SetParameterValue("OW", IIf(cb_ow.Checked, "1", "0"))
                CRYSTAL_REPORT.SetParameterValue("CAMPID", String.Format("{0}", (getCampaignById() & ",0").Replace("'", "")))


                Session.Add("report", CRYSTAL_REPORT)
                CrystalReportViewer1.ReportSource = CRYSTAL_REPORT

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


#Region "Event handlers"

    Protected Sub btnRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRunReport.Click
        Session("report") = Nothing
        setup_report()
    End Sub

    Protected Sub buttonForwardCampaign_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonForwardCampaign.Click
        Transfer_Item(listboxCampaignAll, listboxCampaignSelects)
    End Sub

    Protected Sub buttonBackwardCampaign_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonBackwardCampaign.Click
        Transfer_Item(listboxCampaignSelects, listboxCampaignAll)
    End Sub
#End Region



#Region "Inner classes"
    Public Class Connectivity
        Protected con As SqlConnection
        Protected ada As SqlDataAdapter
        Protected dt As DataTable
        Protected sql As String = String.Empty

    End Class

    ''' <summary>
    ''' Inner class retrieving data from server
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Campaigns
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
#End Region

   
End Class
