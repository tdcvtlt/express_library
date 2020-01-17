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

Partial Class Reports_Rentals_PackageIssuedCancelSave
    Inherits System.Web.UI.Page

    Private CrystalDocument As New ReportDocument
    Private CrystalReport As String = "reportfiles/PackageIssuedCancelSave.rpt"


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack Then
            If Session("report") IsNot Nothing Then
                CrystalReportViewer1.ReportSource = Session("Report")
            End If
        End If
    End Sub

    Protected Sub btnRunReport_Click(sender As Object, e As System.EventArgs) Handles btnRunReport.Click

        CrystalDocument.Load(Server.MapPath(CrystalReport))
        CrystalDocument.FileName = Server.MapPath(CrystalReport)
        CrystalDocument.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        CrystalDocument.SetParameterValue("SDate", CDate(IIf(dfStartDate.Selected_Date <> "", dfStartDate.Selected_Date, Date.Today)))
        CrystalDocument.SetParameterValue("EDate", CDate(IIf(dfEndDate.Selected_Date <> "", DateTime.Parse(dfEndDate.Selected_Date).AddDays(-1), Date.Today)).AddDays(1))

        Session.Add("Report", CrystalDocument)
        CrystalReportViewer1.ReportSource = CrystalDocument

    End Sub
End Class
