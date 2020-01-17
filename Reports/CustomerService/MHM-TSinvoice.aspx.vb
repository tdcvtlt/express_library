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

Partial Class Reports_CustomerService_MHM_TSinvoice
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/MHM-TSInvoice.rpt"

    Protected Sub btReportRun_Click(sender As Object, e As System.EventArgs) Handles btReportRun.Click
        Dim crv = CrystalReportViewer1
        Session("report") = Nothing
        crv.ReportSource = Nothing
        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)
        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        Report.SetParameterValue("sDate", CDate(IIf(dteSDate.Selected_Date <> "", dteSDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("eDate", CDate(IIf(dteEDate.Selected_Date <> "", dteEDate.Selected_Date, Date.Today)))
        Session.Add("report", Report)
        crv.ReportSource = Session("Report")

        lbErr.Visible = IIf(Report.HasRecords, False, True)
        lbErr.Text = IIf(lbErr.Visible, "No data", "")
        crv.Visible = IIf(lbErr.Visible, False, True)
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack Then If Not Session("report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("report")
        lbErr.Text = String.Empty
    End Sub
End Class
