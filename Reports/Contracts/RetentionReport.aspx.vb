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
Imports System.Collections.Generic
Imports System.Linq

Partial Class Reports_Contracts_RetentionReport
    Inherits System.Web.UI.Page

#Region "Page Variables"
    Private reportPath As String = "REPORTFILES/RetentionReport.rpt"
    Private documentX As New ReportDocument
    Private reportX As Report    
#End Region


#Region "Page Events & Handlers"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If (Me.IsPostBack = True) Then
            If Not Session("Crystal") Is Nothing Then
                If Not Session("UserID") Is Nothing Then
                    CrystalViewer.ReportSource = DirectCast(Session("Crystal"), ReportDocument)
                Else
                    Response.Redirect("http://crms.kingscreekplantation.com/crmsnet/logon.aspx")
                End If
            End If
        Else
            CrystalViewer.Visible = False
        End If
    End Sub
#End Region

    
    Protected Sub RunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunReport.Click

        Session("Crystal") = Nothing
        Dim l As IList(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))()

        l.Add(New KeyValuePair(Of String, String)("SDATE", Me.SDATE.Selected_Date))
        l.Add(New KeyValuePair(Of String, String)("EDATE", Me.EDATE.Selected_Date))

        reportX = New Report With {.Database = Resources.Resource.DATABASE, _
                                              .ServerName = Resources.Resource.SERVER, _
                                              .User = Resources.Resource.USERNAME, _
                                              .Password = Resources.Resource.PASSWORD, _
                                              .Path = Server.MapPath(reportPath), _
                                              .Parameters = l, _
                                              .HttpCurrent = HttpContext.Current}

        'documentX = reportX.Document
        Dim context As HttpContext = DirectCast(reportX, ICrystal).DoReport()

        Session("Crystal") = context.Session("Crystal")
        CrystalViewer.ReportSource = context.Session("Crystal")
        CrystalViewer.Visible = True
    End Sub
End Class


