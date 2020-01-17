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


Partial Class Reports_Accounting_Redeeded
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/Redeeded.rpt"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If IsPostBack Then
            If Session("Report") IsNot Nothing Then
                CrystalReportViewer1.ReportSource = Session("Report")
            End If
        End If

    End Sub


    Protected Sub btnRunReport_Click(sender As Object, e As System.EventArgs) Handles btnRunReport.Click

        Session("Report") = Nothing

        Try
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)

            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

            Report.SetParameterValue("sD", CDate(IIf(dfTransDate.Selected_Date <> "", dfTransDate.Selected_Date, Date.Today)))
            Report.SetParameterValue("eD", CDate(IIf(dfEndDate.Selected_Date <> "", dfEndDate.Selected_Date, Date.Today)))

            Session.Add("Report", Report)
            CrystalReportViewer1.ReportSource = Session("Report")

        Catch ex As Exception
            Response.Write(String.Format("<b>{0}</b>", ex.Message))
        End Try

    End Sub

End Class
