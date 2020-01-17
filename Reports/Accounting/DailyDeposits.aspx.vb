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

Partial Class Reports_Accounting_DailyDeposits
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/DailyDeposits.rpt"

    Private Sub BindData()
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select * from t_CCMerchantAccount where active = 1 order by AccountName desc"
            ddMA.DataSource = ds
            ddMA.DataTextField = "Description"
            ddMA.DataValueField = "Accountname"
            ddMA.DataBind()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
        ds = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then

        Else
            Session("Report") = Nothing
            BindData()
            Setup_Report()
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
            'Response.Write("HERE")
            'Dim categoryID As Integer = Convert.ToInt32(ddlCategory.SelectedValue)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            'Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
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

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRunReport.Click
        If dfTransDate.Selected_Date <> "" Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()

        Report.SetParameterValue("Date", CDate(IIf(dfTransDate.Selected_Date <> "", dfTransDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("Acct", ddMA.SelectedValue)

    End Sub

    
End Class
