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

Partial Class Reports_Contracts_CancelledContracts
    Inherits System.Web.UI.Page


#Region "Page Variables"
    Private reportPath As String = "reportfiles/CancelledContracts.rpt"
    Private report As New ReportDocument
#End Region

#Region "Page Events & Handlers"

 


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.SDATE.Attributes.Add("onclick", "scwShow(this,this)")
        Me.EDATE.Attributes.Add("onclick", "scwShow(this,this)")

        Me.EDATE.Style.Item("text-align") = "right"
        Me.SDATE.Style.Item("text-align") = "right"

        If Not Session("Crystal") Is Nothing Then
            If Not Session("UserID") Is Nothing Then
                CrystalViewer.ReportSource = Session("Crystal")
            Else
                Response.Redirect("http://crms.kingscreekplantation.com/crmsnet/logon.aspx")
            End If
        End If

    End Sub

    Protected Sub RunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunReport.Click

        If (Not Session("Crystal") Is Nothing) Then
            Session("Crystal") = Nothing
            CrystalViewer.Visible = False
        End If

        If (String.IsNullOrEmpty(Me.SDATE.Text) Or String.IsNullOrEmpty(Me.EDATE.Text)) Then Return

        report.FileName = Server.MapPath(reportPath)
        report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        report.SetParameterValue("SDATE", Me.SDATE.Text.Trim())
        report.SetParameterValue("EDATE", Me.EDATE.Text.Trim())

        report.Load(Server.MapPath(reportPath))

        Session("Crystal") = report
        CrystalViewer.ReportSource = Session("Crystal")

        CrystalViewer.Visible = True
    End Sub

#End Region


End Class
