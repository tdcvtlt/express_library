Imports System.Data
Imports System.Linq

Imports CrystalDecisions.CrystalReports.Engine


Partial Class Reports_Sales_SalesBonus
    Inherits System.Web.UI.Page


    Dim Report As New ReportDocument
    Dim sReport As String = "ReportFiles/SalesBonus.rpt"


    Protected Sub btRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRunReport.Click

        If String.IsNullOrEmpty(ucEndDate.Selected_Date) Or _
            String.IsNullOrEmpty(ucStartDate.Selected_Date) Then Return

        LoadReport()
    End Sub



    Private Sub LoadReport()

        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)

        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)


        Report.SetParameterValue("STARTDATE", ucStartDate.Selected_Date)
        Report.SetParameterValue("ENDDATE", ucEndDate.Selected_Date)

        '
        'Store object of type ReportDocument in Session state for navigation purposes
        Session("CrystalReport") = Report

        CrystalReportViewer1.ReportSource = Report
        CrystalReportViewer1.DataBind()
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Page.IsPostBack Then

            Try

                CrystalReportViewer1.ReportSource = DirectCast(Session("CrystalReport"), ReportDocument)
                CrystalReportViewer1.DataBind()

            Catch ex As Exception

            End Try
        End If
    End Sub



End Class
