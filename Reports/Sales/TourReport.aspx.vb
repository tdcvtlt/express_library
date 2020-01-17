
Imports System.Data
Imports System.Linq

Imports CrystalDecisions.CrystalReports.Engine



Partial Class Reports_Sales_TourReport
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "ReportFiles/TourReport.rpt"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



    End Sub



    Protected Sub btRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRunReport.Click


        If String.IsNullOrEmpty(dfDate.Selected_Date) = True Then Return

        PrepareReport()


    End Sub


    Private Sub PrepareReport()


        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)

        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)


        Report.SetParameterValue("chosenDate", dfDate.Selected_Date)

    End Sub

End Class
