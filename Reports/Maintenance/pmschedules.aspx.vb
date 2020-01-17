Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data.Linq.Mapping
Imports System.Data.Linq
Imports System.Web.Services
Imports System.IO
Imports System.Web.Script.Serialization
Imports CrystalDecisions.CrystalReports.Engine

Partial Class Reports_Maintenance_pmschedules
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/pmSchedules.rpt"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = True And Not Session("Report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("Report")
        If IsPostBack = False Then

            Dim pm = New clsPreventiveMaintenance()

            With rmCheckBoxList
                .DataSource = pm.Rooms
                .DataTextField = "RoomNumber"
                .DataValueField = "RoomID"
                .DataBind()
            End With

            With bldCheckBoxList
                .DataSource = pm.Buildings
                .DataTextField = "Name"
                .DataValueField = "PMBuildingID"
                .DataBind()
            End With

            With tskCheckBoxList
                .DataSource = pm.Tasks
                .DataTextField = "Name"
                .DataValueField = "TaskID"
                .DataBind()
            End With

            With itmCheckBoxList
                .DataSource = pm.Items
                .DataTextField = "Name"
                .DataValueField = "Item2TrackID"
                .DataBind()
            End With
        End If
    End Sub

    Protected Sub submitButton_Click(sender As Object, e As System.EventArgs) Handles submitButton.Click
        Dim sb = New StringBuilder()
        Dim rmList As New List(Of ListItem)
        Dim bldList As New List(Of ListItem)
        Dim tsksList As New List(Of ListItem)
        Dim itmList As New List(Of ListItem)

        For Each li As ListItem In rmCheckBoxList.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True)
            rmList.Add(li)
        Next
        For Each li As ListItem In bldCheckBoxList.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True)
            bldList.Add(li)
        Next
        For Each li As ListItem In tskCheckBoxList.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True)
            tsksList.Add(li)
        Next
        For Each li As ListItem In itmCheckBoxList.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True)
            itmList.Add(li)
        Next


        Dim sqlSuffix = String.Empty

        If sdate.Selected_Date.Length < 1 Or edate.Selected_Date.Length < 1 Then
            sqlSuffix = String.Format(" and s.serviceDate between '{0}' and '{1}' ", DateTime.MinValue, DateTime.MaxValue)
        Else
            sqlSuffix = String.Format(" and s.serviceDate between '{0}' and '{1}' ", sdate.Selected_Date, edate.Selected_Date)
        End If

        If rmList.Count > 0 Then
            sqlSuffix += " and (roomid in (" & String.Join(",", rmList.Select(Function(x) x.Value).ToArray()) & ") "
        Else
            sqlSuffix += " and (roomid in (" & String.Join(",", rmCheckBoxList.Items.OfType(Of ListItem).Select(Function(x) x.Value).ToArray()) & ") "
        End If

        If bldList.Count > 0 Then
            sqlSuffix += " or pmbuildingID in (" & String.Join(",", bldList.Select(Function(x) x.Value).ToArray()) & ")) "
        Else
            sqlSuffix += " or pmbuildingID in (" & String.Join(",", bldCheckBoxList.Items.OfType(Of ListItem).Select(Function(x) x.Value).ToArray()) & ")) "
        End If

        If tsksList.Count > 0 Then
            sqlSuffix += " and t.taskid in (" & String.Join(",", tsksList.Select(Function(x) x.Value).ToArray()) & ") "
        Else
            sqlSuffix += " and t.taskid in (" & String.Join(",", tskCheckBoxList.Items.OfType(Of ListItem).Select(Function(x) x.Value).ToArray()) & ") "
        End If

        If itmList.Count > 0 Then
            sqlSuffix += " and k.item2trackid in (" & String.Join(",", itmList.Select(Function(x) x.Value).ToArray()) & ") "
        Else
            sqlSuffix += " and k.item2trackid in (" & String.Join(",", itmCheckBoxList.Items.OfType(Of ListItem).Select(Function(x) x.Value).ToArray()) & ") "
        End If

        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)
        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        With Report
            .SetParameterValue("sqlSuffix", sqlSuffix)
        End With

        Session.Add("Report", Report)
        CrystalReportViewer1.ReportSource = Session("Report")

    End Sub
End Class
