﻿Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class Reports_Sales_CampaignSalesSummary_I
    Inherits System.Web.UI.Page

    Private LB_ACTIVE_CAMPAIGNS As ListBox
    Private LB_CHOSEN_CAMPAIGNS As ListBox    

    Private CRYSTAL_REPORT As New ReportDocument
    Private REPORT_FILE_NAME As String = "REPORTFILES/CAMPAIGNSALESSUMMARY_I.rpt"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LB_ACTIVE_CAMPAIGNS = listboxCampaignAll
        LB_CHOSEN_CAMPAIGNS = listboxCampaignSelects

        If Me.IsPostBack = False Then

            Dim o As New Campaigns()
            o.SetUser = CType(Session("User"), User)
            o.Load(String.Empty)

            LB_ACTIVE_CAMPAIGNS.DataSource = o.Campaign
            LB_ACTIVE_CAMPAIGNS.DataTextField = "Name"
            LB_ACTIVE_CAMPAIGNS.DataValueField = "CampaignID"
            LB_ACTIVE_CAMPAIGNS.DataBind()

            LB_CHOSEN_CAMPAIGNS.DataSource = o.Campaign
            LB_CHOSEN_CAMPAIGNS.DataTextField = "Name"
            LB_CHOSEN_CAMPAIGNS.DataValueField = "CampaignID"
            LB_CHOSEN_CAMPAIGNS.DataBind()
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

        If GetCampaignValue().Length > 0 Then

            Try

                CRYSTAL_REPORT.Load(Server.MapPath(REPORT_FILE_NAME))
                CRYSTAL_REPORT.FileName = Server.MapPath(REPORT_FILE_NAME)
                CRYSTAL_REPORT.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

                CRYSTAL_REPORT.SetParameterValue("SDATE", CDate(IIf(dfStartDate.Selected_Date <> "", dfStartDate.Selected_Date, Date.Today)))
                CRYSTAL_REPORT.SetParameterValue("EDATE", CDate(IIf(dfEndDate.Selected_Date <> "", dfEndDate.Selected_Date, Date.Today)))
                CRYSTAL_REPORT.SetParameterValue("OW", IIf(cb_ow.Checked, "1", "0"))
                CRYSTAL_REPORT.SetParameterValue("CAMPID", String.Format("{0}", String.Join(",", GetCampaignValue) & ",0"))

                Session.Add("report", CRYSTAL_REPORT)
                CrystalReportViewer1.ReportSource = CRYSTAL_REPORT

            Catch ex As Exception
                Response.Write(String.Format("Custom error: <br/> <strong>{0}</strong>", ex.Message))
            End Try
        End If
    End Sub

    Private ReadOnly Property GetCampaignValue As String()
        Get
            Return listboxCampaignSelects.Items.OfType(Of ListItem).Select(Function(x) x.Value).ToArray()
        End Get
    End Property

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
    Public Class Campaigns
        Inherits Connectivity
        Implements ILoad
        Private _User As User
        Private _campaignDt As DataTable
        Public Sub New()
            con = New SqlConnection(Resources.Resource.cns)
        End Sub

        Public Sub Load(ByVal id As String) Implements ILoad.Load
            sql = "select c.Name, c.CampaignID from t_Campaign c inner join t_VendorCampaigns vc on c.CampaignID = vc.CRMSCampID where vc.vendorid in (" & IIf(_User.ActiveVendor = 0, String.Join(",", _User.VendorIDs), _User.ActiveVendor) & ") order by Name"
            Using ada = New SqlDataAdapter(sql, con)
                dt = New DataTable()
                ada.Fill(dt)
                _campaignDt = dt
            End Using
        End Sub

        Public Property SetUser() As User
            Get
                Return _User
            End Get
            Set(value As User)
                _User = value
            End Set
        End Property
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

    Protected Sub listboxCampaignAll_DataBound(sender As Object, e As EventArgs) Handles listboxCampaignAll.DataBound

    End Sub

   
End Class
