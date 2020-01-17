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

Partial Class Reports_Accounting_trialbalance_meridian
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/trialbalance-meridian.rpt"
    Protected Sub submit_Click(sender As Object, e As System.EventArgs) Handles submit.Click
        Session("report") = Nothing
        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)
        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        Report.SetParameterValue("cod", CDate(IIf(dteEDate.Selected_Date <> "", dteEDate.Selected_Date, Date.Today)))
        Session.Add("report", Report)
        CrystalReportViewer1.ReportSource = Session("Report")
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack Then If Not Session("report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("report")
    End Sub
End Class
