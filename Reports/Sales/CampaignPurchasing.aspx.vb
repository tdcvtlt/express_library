Imports System.Data
Imports System.Linq

Imports CrystalDecisions.CrystalReports.Engine



Partial Class Reports_Sales_CampaignPurchasing
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim ReportPath As String = "ReportFiles/CampaignPurchasing.rpt"

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

        Report.SetParameterValue("SDate", CDate(IIf(dpFrom.Selected_Date <> "", dpFrom.Selected_Date, Date.Today)))
        Report.SetParameterValue("EDate", CDate(IIf(dpTo.Selected_Date <> "", dpTo.Selected_Date, Date.Today)))

    End Sub



    Private Sub BindToCampaigns()
        
    End Sub




End Class
