﻿Imports System.Data.SqlClient
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine

Partial Class Reports_Marketing_LeadSummary
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/LeadSummary.rpt"

    Private Sub BindData()
        
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
            'Report.Close()
            'Report.Dispose()
        Catch ex As Exception

        End Try



    End Sub



    Private Sub Setup_Report()
        Select Case rblType.SelectedValue
            Case "date_entered"
                sReport = "reportfiles/LeadSummary.rpt"
            Case "date_collected_fileid"
                sReport = "reportfiles/leadsummarybylistid.rpt"
            Case "leads_assigned_by_source"
                sReport = "reportfiles/leadsassignedbysource.rpt"
            Case "leads_assigned_by_list"
                sReport = "reportfiles/leadsassignedbylist.rpt"
            Case Else
                sReport = "reportfiles/LeadSummaryBySource.rpt"
        End Select


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

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If dteSDate.Selected_Date <> "" Or rblType.SelectedValue = "leads_assigned_by_source" Or rblType.SelectedValue = "leads_assigned_by_list" Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()
        If rblType.SelectedValue <> "leads_assigned_by_source" And rblType.SelectedValue <> "leads_assigned_by_list" Then
            Report.SetParameterValue("SDate", CDate(IIf(dteSDate.Selected_Date <> "", dteSDate.Selected_Date, Date.Today)))
            Report.SetParameterValue("EDate", CDate(IIf(dteEDate.Selected_Date <> "", dteEDate.Selected_Date, Date.Today)))
        End If
    End Sub



    Protected Sub rblType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblType.SelectedIndexChanged

        Setup_Report()
    End Sub
End Class