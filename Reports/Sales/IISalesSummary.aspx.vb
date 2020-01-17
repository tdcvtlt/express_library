Imports System.Data
Imports System.Linq
Imports CrystalDecisions.CrystalReports.Engine


Partial Class Reports_Sales_IISalesSummary
    Inherits System.Web.UI.Page


    Dim Report As ReportDocument = Nothing
    Dim ReportPath As String = "reportfiles/IISalesSummary.rpt"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            '
            'Needed for CR paging with Previous || Next buttons
            If Not DirectCast(Session("CrystalReport"), ReportDocument) Is Nothing Then
                CRViewer.ReportSource = DirectCast(Session("CrystalReport"), ReportDocument)
                CRViewer.DataBind()
            Else

                CRViewer.Visible = False
            End If


        Catch ex As Exception
            Trace.Write(ex.Message)            
        End Try
    End Sub





    Protected Sub btRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRunReport.Click

        If String.IsNullOrEmpty(dpFrom.Selected_Date) Or String.IsNullOrEmpty(dpTo.Selected_Date) Then Return

        LoadReport()

    End Sub

    Private Sub LoadReport()

        CRViewer.ReportSource = Nothing


        Report = New ReportDocument()

        Report.Load(Server.MapPath(ReportPath))
        Report.FileName = Server.MapPath(ReportPath)

        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)




        Try
            Report.SetParameterValue("StartDate", dpFrom.Selected_Date)
            Report.SetParameterValue("EndTime", dpTo.Selected_Date)
            Report.SetParameterValue("IncludeOW", IIf(cbIncludeOW.Checked, "1", "0"))

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try



        Session("CrystalReport") = Report

        CRViewer.ReportSource = Report
        CRViewer.DataBind()

        CRViewer.Visible = True

        CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None


    End Sub


End Class
