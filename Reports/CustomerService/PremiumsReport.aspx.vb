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

Partial Class Reports_CustomerService_PremiumsReport
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/PremiumsIssued.rpt"

    Protected Sub Prefill()
        Prems.DataSource = (New clsPremium).List_Active
        Prems.DataValueField = "PremiumID"
        Prems.DataTextField = "PremiumName"
        Prems.DataBind()

        Camps.DataSource = (New clsCampaign).Load_Lookup
        Camps.DataValueField = "CampaignID"
        Camps.DataTextField = "Name"
        Camps.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then

        Else
            Session("Report") = Nothing
            Prefill()
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

        Report.SetParameterValue("SDate", CDate(IIf(dteSDate.Selected_Date <> "", dteSDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("EDate", CDate(IIf(dteEDate.Selected_Date <> "", dteEDate.Selected_Date, Date.Today)))
        Dim campaigns As String = ""
        Dim premiums As String = ""
        If CampsSelected.Items.Count > 0 Then
            For Each item As ListItem In CampsSelected.Items
                campaigns &= IIf(campaigns = "", "", ",") & item.Value
            Next
        End If
        If PremsSelected.Items.Count > 0 Then
            For Each item As ListItem In PremsSelected.Items
                premiums &= IIf(premiums = "", "", ",") & item.Value
            Next
        End If
        Report.SetParameterValue("CampaignID", campaigns)
        Report.SetParameterValue("PremiumID", premiums)
    End Sub
    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If dteSDate.Selected_Date <> "" And dteEDate.Selected_Date <> "" Then
            hfShowReport.Value = 1
            Setup_Report()
            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not (IsNothing(Prems.SelectedItem)) Then
            PremsSelected.Items.Add(Prems.SelectedItem)
            Prems.Items.Remove(Prems.SelectedItem)
            If Prems.Items.Count > 0 Then Prems.SelectedIndex = 0
        End If
    End Sub
    Protected Sub btnAddAll_Click(sender As Object, e As EventArgs) Handles btnAddAll.Click
        If Prems.Items.Count > 0 Then
            For Each item As ListItem In Prems.Items
                PremsSelected.Items.Add(item)
            Next
            Prems.Items.Clear()
        End If
    End Sub
    Protected Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If Not IsNothing(PremsSelected.SelectedItem) Then
            Prems.Items.Add(PremsSelected.SelectedItem)
            PremsSelected.Items.Remove(PremsSelected.SelectedItem)
            If PremsSelected.Items.Count > 0 Then PremsSelected.SelectedIndex = 0
        End If
    End Sub
    Protected Sub btnRemoveAll_Click(sender As Object, e As EventArgs) Handles btnRemoveAll.Click
        If PremsSelected.Items.Count > 0 Then
            For Each item As ListItem In PremsSelected.Items
                Prems.Items.Add(item)
            Next
            PremsSelected.Items.Clear()
        End If
    End Sub
    Protected Sub btnAdd1_Click(sender As Object, e As EventArgs) Handles btnAdd1.Click
        If Not IsNothing(Camps.SelectedItem) Then
            CampsSelected.Items.Add(Camps.SelectedItem)
            Camps.Items.Remove(Camps.SelectedItem)
            If Camps.Items.Count > 0 Then Camps.SelectedIndex = 0
        End If
    End Sub
    Protected Sub btnAddAll1_Click(sender As Object, e As EventArgs) Handles btnAddAll1.Click
        If Camps.Items.Count > 0 Then
            For Each item As ListItem In Camps.Items
                CampsSelected.Items.Add(item)
            Next
            Camps.Items.Clear()
        End If
    End Sub
    Protected Sub btnRemove1_Click(sender As Object, e As EventArgs) Handles btnRemove1.Click
        If Not IsNothing(CampsSelected.SelectedItem) Then
            Camps.Items.Add(CampsSelected.SelectedItem)
            CampsSelected.Items.Remove(CampsSelected.SelectedItem)
            If CampsSelected.Items.Count > 0 Then CampsSelected.SelectedIndex = 0
        End If
    End Sub
    Protected Sub btnRemoveAll1_Click(sender As Object, e As EventArgs) Handles btnRemoveAll1.Click
        If CampsSelected.Items.Count > 0 Then
            For Each item In CampsSelected.Items
                Camps.Items.Add(item)
            Next
            CampsSelected.Items.Clear()
        End If
    End Sub
End Class
