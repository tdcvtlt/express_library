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

Partial Class Reports_CustomerService_JW_OPC_Sales
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/JW-OPC Sales.rpt"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If IsPostBack Then
            If Session("report") IsNot Nothing Then
                CrystalReportViewer1.ReportSource = Session("report")
            End If
        End If

    End Sub

    Protected Sub btn_Report_Click(sender As Object, e As System.EventArgs) Handles btn_Report.Click

        If String.IsNullOrEmpty(dteSDate.Selected_Date) = False And String.IsNullOrEmpty(dteEDate.Selected_Date) = False Then

            Session("report") = Nothing
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

            Report.SetParameterValue("sd", CDate(IIf(dteSDate.Selected_Date <> "", dteSDate.Selected_Date, Date.Today)))
            Report.SetParameterValue("ed", CDate(IIf(dteEDate.Selected_Date <> "", dteEDate.Selected_Date, Date.Today)))

            Session.Add("report", Report)
            CrystalReportViewer1.ReportSource = Session("report")
        End If
    End Sub
End Class
