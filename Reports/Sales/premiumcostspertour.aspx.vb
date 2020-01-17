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

Partial Class Reports_Sales_premiumcostspertour
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/premiumcostspertour.rpt"

    Protected Sub btn_submit_Click(sender As Object, e As System.EventArgs) Handles btn_submit.Click

        If String.IsNullOrEmpty(sdate.Selected_Date) Or String.IsNullOrEmpty(edate.Selected_Date) Then Return
        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)
        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        With Report
            .SetParameterValue("sdate", sdate.Selected_Date)
            .SetParameterValue("edate", edate.Selected_Date)
        End With

        Session.Add("Report", Report)
        CrystalReportViewer1.ReportSource = Session("Report")
    End Sub

        
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = True And Not Session("Report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("Report")
    End Sub
End Class
