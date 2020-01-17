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

Partial Class Reports_Accounting_MaintenanceFeeReport
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/MaintenanceFeeReport.rpt"

    Private Sub BindData()
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select * from t_FinTransCodes f inner join t_Comboitems tc on tc.comboitemid = f.transcodeid inner join t_ComboItems tt on f.TransTypeID = tt.ComboitemID where tc.comboitem like 'mf%' or tt.Comboitem = 'MFTrans'"

        'Updated per work order 43041 for all prospect transaction codes
        ds.SelectCommand = "Select * from t_FinTransCodes f inner join t_Comboitems tc on tc.comboitemid = f.transcodeid inner join t_ComboItems tt on f.TransTypeID = tt.ComboitemID where tc.comboitem like 'mf%' or tt.Comboitem = 'MFTrans' or f.MerchantAccountID = 1 order by tc.comboitem"

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
            ListBox2.Items.Add(ListBox1.SelectedItem)
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            ListBox2.SelectedIndex = 0
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        '<<
        If ListBox2.SelectedIndex >= 0 Then
            ListBox1.Items.Add(ListBox2.SelectedItem)
            ListBox2.Items.Remove(ListBox2.SelectedItem)
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

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        If sdate.Selected_Date <> "" And edate.Selected_Date <> "" Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()
        Dim sItems As String = ""
        For i = 0 To ListBox2.Items.Count - 1
            sItems &= IIf(sItems = "", "'" & ListBox2.Items(i).Text & "'", ",'" & ListBox2.Items(i).Text & "'")
        Next
        Report.SetParameterValue("SDate", sdate.Selected_Date)
        Report.SetParameterValue("EDate", CDate(edate.Selected_Date).AddDays(1).ToString)
        Report.SetParameterValue("Invoices", sItems)
    End Sub

    Protected Sub BtnAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAll.Click
        For i = ListBox1.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = ListBox1.Items(i)
            ListBox2.ClearSelection()
            ListBox2.Items.Add(lItem)
            ListBox1.Items.Remove(lItem)
        Next
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        For i = ListBox2.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = ListBox2.Items(i)
            ListBox1.ClearSelection()
            ListBox1.Items.Add(lItem)
            ListBox2.Items.Remove(lItem)
        Next
    End Sub
End Class
