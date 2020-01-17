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


Partial Class Reports_Accounting_CxlPenderAmountInDateRange
    Inherits System.Web.UI.Page

    Dim CrystalReport As New ReportDocument
    Dim ReportPath As String = Server.MapPath("reportfiles/CxlPenderAmountInDateRange.rpt")

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        If String.IsNullOrEmpty(sdate.Selected_Date) = False And String.IsNullOrEmpty(edate.Selected_Date) = False Then
            Report_Setup()
        End If
    End Sub

    Private Sub Report_Setup()

        Session("Report") = Nothing
        CrystalReport.Load(ReportPath)
        CrystalReport.FileName = ReportPath

        CrystalReport.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        CrystalReport.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        CrystalReport.SetParameterValue("sd", sdate.Selected_Date)
        CrystalReport.SetParameterValue("ed", edate.Selected_Date)

        Session.Add("Report", CrystalReport)
        CrystalReportViewer1.ReportSource = Session("Report")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            If Session("Report") IsNot Nothing Then
                CrystalReportViewer1.ReportSource = Session("Report")
            End If
        End If
    End Sub   
End Class
