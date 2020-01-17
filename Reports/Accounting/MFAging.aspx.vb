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

Partial Class MFAging_Default
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/mfaging.rpt"
    Private Sub BindData()

        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select * from t_FinTransCodes f inner join t_Comboitems tc on tc.comboitemid = f.transcodeid where tc.comboitem like 'mf%' or tc.comboitem like 'sa1%'"

        ListBox1.DataSource = ds
        ListBox1.DataTextField = "Comboitem"
        ListBox1.DataValueField = "Comboitem"
        ListBox1.DataBind()
        ds = Nothing
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


    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        '>>
        If ListBox1.SelectedIndex >= 0 Then
            ListBox2.ClearSelection()
            ListBox2.Items.Add(ListBox1.SelectedItem)
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            ListBox2.SelectedIndex = 0
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        '<<
        If ListBox2.SelectedIndex >= 0 Then
            ListBox1.ClearSelection()
            ListBox1.Items.Add(ListBox2.SelectedItem)
            ListBox2.Items.Remove(ListBox2.SelectedItem)
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        If DateField1.Selected_Date <> "" And ListBox2.Items.Count > 0 Then
            hfShowReport.Value = 1
            Setup_Report()
            Dim sItems As String = ""
            For i = 0 To ListBox2.Items.Count - 1
                sItems &= IIf(sItems = "", ListBox2.Items(i).Text, "," & ListBox2.Items(i).Text)
            Next
            Report.SetParameterValue("@Cutoffdate", DateField1.Selected_Date)
            Report.SetParameterValue("@KeyField", "ContractID")
            Report.SetParameterValue("@TransCode", sItems)
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
            Report.SetParameterValue("@Cutoffdate", "1/1/12")
            Report.SetParameterValue("@KeyField", "ContractID")
            Report.SetParameterValue("@TransCode", "MF07")
            Session.Add("Report", Report)
        Else
            Report = Session("Report")
            If Report.FileName <> Server.MapPath(sReport) Then
                Session("Report") = Nothing
                Setup_Report()
            End If
        End If
    End Sub

    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click
        '<<<
        For i = ListBox2.Items.Count - 1 To 0 Step -1
            ListBox1.ClearSelection()
            ListBox1.Items.Add(ListBox2.Items(i))
            ListBox2.Items.Remove(ListBox2.Items(i))
        Next
        hfShowReport.Value = 0
    End Sub

    Protected Sub Button5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button5.Click
        '>>>
        For i = ListBox1.Items.Count - 1 To 0 Step -1
            ListBox2.ClearSelection()
            ListBox2.Items.Add(ListBox1.Items(i))
            ListBox1.Items.Remove(ListBox1.Items(i))
            ListBox2.SelectedIndex = 0
            hfShowReport.Value = 0
        Next
    End Sub
End Class
