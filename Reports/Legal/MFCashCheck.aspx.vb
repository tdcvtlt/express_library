﻿Imports CrystalDecisions.CrystalReports.Engine
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

Partial Class Reports_Legal_MFCashCheck
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/MFCashCheck.rpt"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then

        Else
            Dim ds As New SqlDataSource
            ds.SelectCommand = "select 0 as personnelid, '--ALL--' as username union Select personnelid, username from t_Personnel where active = 1 and rtrim(ltrim(username)) <> '' order by username"
            ds.ConnectionString = Resources.Resource.cns
            ddUsers.DataSource = ds
            ddUsers.DataTextField = "username"
            ddUsers.DataValueField = "personnelid"
            ddUsers.DataBind()
            ds = Nothing
        End If

        If hfShowReport.Value = 1 Then CrystalReportViewer1.ReportSource = Session("Report")
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            'Report.Clone()
            'Report.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Setup_Report()
        If Session("Report") Is Nothing Then
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            'Dim categoryID As Integer = Convert.ToInt32(ddlCategory.SelectedValue)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Set_Params()
            Session.Add("Report", Report)
        Else
            Report = Session("Report")
            If Report.FileName <> Server.MapPath(sReport) Then
                Session("Report") = Nothing
                Setup_Report()
            End If
        End If
    End Sub

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        If sdate.Selected_Date <> "" And edate.Selected_Date <> "" Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()
        Report.SetParameterValue("SDate", sdate.Selected_Date)
        Report.SetParameterValue("EDate", CDate(edate.Selected_Date).AddDays(1))
        Report.SetParameterValue("UserID", ddUsers.SelectedValue)
    End Sub
End Class
