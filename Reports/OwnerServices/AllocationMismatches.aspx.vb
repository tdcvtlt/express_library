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


Partial Class Reports_OwnerServices_AllocationMismatches
    Inherits System.Web.UI.Page

#Region "Page Variables"
    Private path As String = "REPORTFILES/AllocationMismatches.rpt"
    Private crystal As Report = Nothing
    Private Const __Crystal As String = "Crystal"
#End Region

#Region "Page Events & Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CrystalViewer.HasToggleGroupTreeButton = False

        If (Me.IsPostBack = True) Then
            If (Not Session("UserID") Is Nothing) Then
                If (Not Session(__Crystal) Is Nothing) Then
                    CrystalViewer.ReportSource = Session(__Crystal)
                End If
            End If
        End If
    End Sub

    Protected Sub RunReport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles RunReport.Click
        Session(__Crystal) = Nothing
        If (String.IsNullOrEmpty(SDate.Selected_Date) Or String.IsNullOrEmpty(EDate.Selected_Date)) Then Return

        Dim l As IList(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))()

        l.Add(New KeyValuePair(Of String, String)("SDATE", Me.SDate.Selected_Date))
        l.Add(New KeyValuePair(Of String, String)("EDATE", Me.EDate.Selected_Date))

        crystal = New Report()
        crystal.Parameters = l
        crystal.Path = Server.MapPath(path)

        Session(__Crystal) = DirectCast(crystal, ICrystal).DoReport().Session(__Crystal)
        CrystalViewer.ReportSource = Session(__Crystal)
    End Sub
#End Region

End Class
