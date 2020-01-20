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
Partial Class Reports_CustomerService_ConversionByDate
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/conversionsbydate.rpt"
    Private Sub BindData()

     
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack Then

        Else
            BindData()
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

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        If dteSDate.Selected_Date <> "" And dteEDate.Selected_Date <> "" Then
            hfShowReport.Value = 1
            Setup_Report()
            'Report.SetParameterValue("@SDate", dteSDate.Selected_Date)
            'Report.SetParameterValue("@EDate", dteEDate.Selected_Date)
            'Response.Write(sItems)
            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Private Sub Setup_Report()
        If Session("Report") Is Nothing Then
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            'Dim categoryID As Integer = Convert.ToInt32(ddlCategory.SelectedValue)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Report.SetParameterValue("SDate", dteSDate.Selected_Date)
            Report.SetParameterValue("EDate", dteEDate.Selected_Date)
            Session.Add("Report", Report)
        Else
            Report = Session("Report")
            If Report.FileName <> Server.MapPath(sReport) Then
                Session("Report") = Nothing
                Setup_Report()
            End If
        End If
    End Sub

   
End Class