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


Partial Class Reports_Collection_Removals
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/CollectionRemovals.rpt"

    Private Sub BindData()
        Dim sqlds As New SqlDataSource(Resources.Resource.cns, "Select b.batchid, bt.ComboItem + ' - ' + convert(varchar, b.hearingdate,110) as Batch from t_CancellationBatch b inner join t_ComboItems bt on bt.ComboItemID = b.TypeID")

        Collections.DataSource = sqlds
        Collections.DataTextField = "Batch"
        Collections.DataValueField = "BatchID"
        Collections.DataBind()
        sqlds = Nothing
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

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        hfShowReport.Value = 1
        Setup_Report()

        CrystalReportViewer1.ReportSource = Session("Report")

    End Sub
    Protected Sub Set_Params()

        Report.SetParameterValue("BatchID", Collections.SelectedValue)

    End Sub
End Class
