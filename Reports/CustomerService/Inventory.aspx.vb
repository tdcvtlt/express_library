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
Partial Class Reports_CustomerService_Inventory
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/Inventory.rpt"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then

        Else
            BindData()
            Session("Report") = Nothing
            Setup_Report()
        End If

        If hfShowReport.Value = 1 Then CrystalReportViewer1.ReportSource = Session("Report")


    End Sub

    Private Sub BindData()
        For i = Date.Today.Year + 10 To 1998 Step -1
            ddYear.Items.Add(New ListItem(i, i))
        Next
        ddYear.SelectedValue = Date.Today.Year + 1
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try

            If Not Report Is Nothing Then
     

            End If
        Catch ex As Exception

        End Try


    End Sub



    Private Sub Setup_Report()
        If Session("Report") Is Nothing Then
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
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



    Protected Sub Set_Params()

        Report.SetParameterValue("Year", IIf(ddYear.SelectedValue = 0, Date.Today.Year, ddYear.SelectedValue))


    End Sub
    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        If ddyear.Selectedvalue <> 0 Then
            hfShowReport.Value = 1
            Setup_Report()
            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub
End Class
