Imports CrystalDecisions.CrystalReports.Engine
Partial Class Reports_Sales_SalesWithTO
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "ReportFiles/SalesWithTO.rpt"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then


        Else

            Try

                CrystalReportViewer1.ReportSource = DirectCast(Session("CrystalReport"), ReportDocument)
                CrystalReportViewer1.DataBind()

            Catch ex As Exception

            End Try
        End If
    End Sub

    Protected Sub btRunReport_Click(sender As Object, e As System.EventArgs) Handles btRunReport.Click

        If String.IsNullOrEmpty(ucEndDate.Selected_Date) Or _
            String.IsNullOrEmpty(ucStartDate.Selected_Date) Then Return



        LoadReport()
    End Sub

    Private Sub LoadReport()

        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)

        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)


        Report.SetParameterValue("sDate", ucStartDate.Selected_Date)
        Report.SetParameterValue("eDate", ucEndDate.Selected_Date)

        '
        'Store object of type ReportDocument in Session state for navigation purposes
        Session("CrystalReport") = Report

        CrystalReportViewer1.ReportSource = Report
        CrystalReportViewer1.DataBind()
    End Sub
End Class
