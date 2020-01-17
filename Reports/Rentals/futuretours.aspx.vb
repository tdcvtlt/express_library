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

Partial Class Reports_Rentals_FutureTours
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/FutureTours.rpt"

    Private Sub BindData()
        Dim cnn As New SqlConnection(Resources.Resource.cns)
        Dim cmd As New SqlCommand("select b.COMBOITEM, b.COMBOITEMID from t_combos a inner join t_comboitems b on a.comboid = b.comboid where comboname = 'tourlocation' and comboitem in ('kcp', 'richmond')", cnn)
        Dim rdr As SqlDataReader

        cnn.Open()
        rdr = cmd.ExecuteReader()
        ddLocation.DataSource = rdr
        ddLocation.DataTextField = "COMBOITEM"
        ddLocation.DataValueField = "COMBOITEMID"

        ddLocation.DataBind()


        rdr.Close()

        cmd = New SqlCommand("SELECT CampaignID, Name FROM T_CAMPAIGN where active = 1 ORDER BY Name", cnn)
        'ListBox1.Items.Add(New ListItem("All", "1"))
        ListBox1.DataSource = cmd.ExecuteReader()
        ListBox1.DataTextField = "Name"
        ListBox1.DataValueField = "CampaignID"
        ListBox1.AppendDataBoundItems = True
        ListBox1.DataBind()

        cnn.Close()
        cnn.Dispose()
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
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
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

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRunReport.Click
        If dfStartDate.Selected_Date <> "" And dfEndDate.Selected_Date <> "" Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()

        Report.SetParameterValue("SDate", CDate(IIf(dfStartDate.Selected_Date <> "", dfStartDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("EDate", CDate(IIf(dfEndDate.Selected_Date <> "", dfEndDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("Location", ddLocation.SelectedValue)
        Dim sItems As String = ""
        For i = 0 To ListBox2.Items.Count - 1
            sItems &= IIf(sItems = "", ListBox2.Items(i).Value, "," & ListBox2.Items(i).Value)
        Next
        Report.SetParameterValue("@CampaignID", sItems)

    End Sub


End Class
