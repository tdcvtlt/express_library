Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
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


Partial Class Reports_OwnerServices_UsageByDay
    Inherits System.Web.UI.Page

    Private Report As New ReportDocument
    Private sReport As String = "reportfiles/UsageByDay.rpt"

    Private UsageTypes() As String = New String() {"Exchange", "Marketing", "Owner", "Points", "PointsExchange", "Rental", "TrialOwner"}

    Protected Sub btL_Click(sender As Object, e As System.EventArgs) Handles btL.Click

        Dim item = lbL.SelectedItem        

        lbR.Items.Add(item)
        lbL.Items.Remove(item)

        lbL.ClearSelection()
        lbR.ClearSelection()
    End Sub

    Protected Sub btR_Click(sender As Object, e As System.EventArgs) Handles btR.Click

        Dim item = lbR.SelectedItem

        lbR.Items.Remove(item)
        lbL.Items.Add(item)

        lbL.ClearSelection()
        lbR.ClearSelection()
    End Sub

    Protected Sub btLAll_Click(sender As Object, e As System.EventArgs) Handles btLAll.Click

        If lbL.Items.Count = 0 Then Return
        For Each li As ListItem In lbL.Items
            lbR.Items.Add(li)
        Next

        lbL.Items.Clear()

    End Sub

    Protected Sub btRAll_Click(sender As Object, e As System.EventArgs) Handles btRAll.Click

        If lbR.Items.Count = 0 Then Return
        For Each li As ListItem In lbR.Items
            lbL.Items.Add(li)
        Next

        lbR.Items.Clear()

    End Sub

    Protected Sub btSubmit_Click(sender As Object, e As System.EventArgs) Handles btSubmit.Click

        If lbR.Items.Count = 0 Then Return

        Dim l = New List(Of String)
        For Each li As ListItem In lbR.Items
            l.Add(String.Format("'{0}'", li.Text))
        Next

        Report.Load(Server.MapPath(sReport))
        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        Report.SetParameterValue("sdate", CDate(IIf(sdate.Selected_Date <> "", sdate.Selected_Date, Date.Today)))
        Report.SetParameterValue("edate", CDate(IIf(edate.Selected_Date <> "", edate.Selected_Date, Date.Today)))
        Report.SetParameterValue("usagetypes", String.Join(",", l.ToArray()))

        Session("Report") = Report
        CrystalReportViewer1.ReportSource = Session("Report")
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            For Each s As String In UsageTypes
                lbL.Items.Add(s)
            Next           
        Else
            If Not Session("Report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("Report")
        End If
    End Sub

  
 
End Class
