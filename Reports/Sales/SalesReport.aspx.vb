Imports System.Data
Imports System.Linq

Imports CrystalDecisions.CrystalReports.Engine




Partial Class Reports_Sales_SalesReport
    Inherits System.Web.UI.Page



    Dim Report As ReportDocument
    Dim ReportPath As String = "ReportFiles/SalesReport.rpt"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = True Then

            Try

                CRViewer.ReportSource = DirectCast(Session("CrystalReport"), ReportDocument)
                CRViewer.DataBind()

            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
        End If

    End Sub


    Protected Sub btRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRunReport.Click

        If String.IsNullOrEmpty(ucStartDate.Selected_Date) Or String.IsNullOrEmpty(ucEndDate.Selected_Date) Then Return

        LoadReport()

    End Sub

    Private Sub LoadReport()

        CRViewer.Visible = IIf(CRViewer.Visible, False, False)

        If Not Session("CrystalReport") Is Nothing Then Session("CrystalReport") = Nothing

        Report = New ReportDocument()

        Report.Load(Server.MapPath(ReportPath))
        Report.FileName = Server.MapPath(ReportPath)

        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)


        Report.SetParameterValue("STARTDATE", ucStartDate.Selected_Date)
        Report.SetParameterValue("ENDDATE", ucEndDate.Selected_Date)

        '
        'Store object of type ReportDocument in Session state for navigation purposes
        Session("CrystalReport") = Report

        CRViewer.ReportSource = DirectCast(Session("CrystalReport"), ReportDocument)
        CRViewer.DataBind()


        

        CRViewer.Visible = Not CRViewer.Visible
    End Sub

End Class
