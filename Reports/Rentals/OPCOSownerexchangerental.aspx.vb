Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Security
Imports System.Web
Imports System.Configuration
Imports System.Data
Imports System

Partial Class Reports_Rentals_OPCOSownerexchangerental
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim REPORT_NAME As String = "reportfiles/OPCOSownerexchangerental.rpt"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack Then
            If Not Session("Report") Is Nothing Then
                CrystalReportViewer1.ReportSource = DirectCast(Session("Report"), ReportDocument)
            End If
        End If
    End Sub

    Protected Sub btn_submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_submit.Click

        Session("Report") = Nothing

        Report.Load(Server.MapPath(REPORT_NAME))
        Report.FileName = Server.MapPath(REPORT_NAME)

        Setup_Params()

        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        Report.Subreports(0).DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        Session.Add("Report", Report)
        CrystalReportViewer1.ReportSource = Report
        CrystalReportViewer1.DataBind()
    End Sub

    Private Sub Setup_Params()
        Report.SetParameterValue("sdate", CDate(IIf(sdate.Selected_Date <> "", sdate.Selected_Date, Date.Today)))
        Report.SetParameterValue("edate", CDate(IIf(edate.Selected_Date <> "", edate.Selected_Date, Date.Today)))
    End Sub
End Class
