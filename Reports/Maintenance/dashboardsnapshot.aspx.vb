Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data.Linq.Mapping
Imports System.Data.Linq
Imports System.Web.Services
Imports System.IO
Imports System.Web.Script.Serialization
Imports CrystalDecisions.CrystalReports.Engine

Partial Class Reports_Maintenance_pmschedules
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/dashboard.rpt"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = True And Not Session("Report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("Report")
        If Not (IsPostBack) Then
            For i = Year(Date.Now) - 5 To Year(Date.Now)
                ddYear.Items.Add(New ListItem With {.Value = i, .Text = i})
            Next
            ddYear.SelectedValue = Year(Date.Now)
            ddMonth.SelectedValue = Month(Date.Now)
        End If
    End Sub

    Protected Sub submitButton_Click(sender As Object, e As System.EventArgs) Handles submitButton.Click
        Dim var As String
        If ddMonth.SelectedValue < 12 And ddMonth.SelectedValue <> "12" Then
            var = CDate((ddMonth.SelectedValue + 1).ToString & "/1/" & ddYear.SelectedValue).AddDays(-1)
        Else
            var = CDate("12/31/" & ddYear.SelectedValue)
        End If

        If ddMonth.SelectedValue > 0 And ddYear.SelectedValue > 0 Then
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

            With Report
                .SetParameterValue("sDate", var)
            End With

            Session.Add("Report", Report)
            CrystalReportViewer1.ReportSource = Session("Report")

        Else
            Session("Report") = Nothing
        End If
    End Sub
End Class
