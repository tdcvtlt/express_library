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

Partial Class Reports_CustomerService_MHMpackagesWithBookedReservations
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/MHMpackagesWithBookedReservations.rpt"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack Then If Not Session("report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("report")
        lblMsg.Text = String.Empty
    End Sub

    Protected Sub btnReportRun_Click(sender As Object, e As EventArgs) Handles btnReportRun.Click
        Session("report") = Nothing
        CrystalReportViewer1.ReportSource = Nothing
        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)
        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)        
        Session.Add("report", Report)
        CrystalReportViewer1.ReportSource = Session("Report")

        lblMsg.Visible = Not Report.HasRecords
        If Report.HasRecords = False Then
            CrystalReportViewer1.ReportSource = Nothing
            lblMsg.Text = "No data"
        End If
    End Sub
End Class
