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

Partial Class Reports_Contracts_afcchanged
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/afcchanged.rpt"

   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack Then
            If Session("report") IsNot Nothing Then crystalViewer1.ReportSource = Session("report")
        End If
    End Sub

    Protected Sub RunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunReport.Click

        If sdate.Selected_Date.Equals("") = False And edate.Selected_Date.Equals("") = False Then
            Session("report") = Nothing

            Try
                Report.Load(Server.MapPath(sReport))
                Report.FileName = Server.MapPath(sReport)

                Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
                Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

                Report.SetParameterValue("sdate", sdate.Selected_Date)
                Report.SetParameterValue("edate", DateTime.Parse(edate.Selected_Date).AddDays(1))

                Session.Add("report", Report)
                crystalViewer1.ReportSource = Report
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try

        End If
    End Sub

End Class
