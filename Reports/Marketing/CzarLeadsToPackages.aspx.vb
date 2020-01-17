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

Partial Class Reports_Marketing_CzarLeadsToPackages
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/CzarLeadsToPackages.rpt"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack Then If Not Session("report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("report")
        Err.Text = String.Empty
    End Sub

    Protected Sub submit_Click(sender As Object, e As System.EventArgs) Handles submit.Click

        If sd.Selected_Date = "" Or ed.Selected_Date = "" Then Return

        Session("report") = Nothing
        CrystalReportViewer1.ReportSource = Nothing
        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)
        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)


        Report.SetParameterValue("sd", CDate(IIf(sd.Selected_Date <> "", Convert.ToDateTime(sd.Selected_Date), Date.Today)))
        Report.SetParameterValue("ed", CDate(IIf(ed.Selected_Date <> "", Convert.ToDateTime(ed.Selected_Date).AddDays(1), Date.Today)))
        Session.Add("report", Report)
        CrystalReportViewer1.ReportSource = Session("Report")
        CrystalReportViewer1.Style.Remove("display")
        If Report.HasRecords = False Then
            CrystalReportViewer1.ReportSource = Nothing
            CrystalReportViewer1.Style.Add("display", "none")
            Err.Text = "No data"
        End If
    End Sub
End Class
