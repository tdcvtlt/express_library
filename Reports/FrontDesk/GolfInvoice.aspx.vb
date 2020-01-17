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
Partial Class Reports_FrontDesk_GolfInvoice
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/GolfInvoice.rpt"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            If Session("Report") IsNot Nothing Then CrystalReportViewer1.ReportSource = Session("Report")
        Else
            If Session("Report") IsNot Nothing Then
                Session("Report") = Nothing
            End If
            BindData()
            'Setup_Report()
            btnRun_Click()
        End If

        If hfShowReport.Value = 1 Then CrystalReportViewer1.ReportSource = Session("Report")


    End Sub

    Private Sub BindData()
        Dim oGOlf As New clsGolfCourse
        ListBox1.DataSource = oGOlf.List_Courses("")
        ListBox1.DataValueField = "Course"
        ListBox1.DataTextField = "Course"
        ListBox1.DataBind()
        oGOlf = Nothing
    End Sub

    Protected Sub Set_Params()
        Dim sItems As String = ""
        sItems = "'0'"
        If ListBox2.Items.Count = 0 Then
            For i = 0 To ListBox1.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & ListBox1.Items(i).Value & "'", ",'" & ListBox1.Items(i).Value & "'")
            Next
        Else
            For i = 0 To ListBox2.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & ListBox2.Items(i).Value & "'", ",'" & ListBox2.Items(i).Value & "'")
            Next
        End If
        Report.SetParameterValue("GolfCourse", sItems)
        Report.SetParameterValue("sDate", sDate.Selected_Date)
        Report.SetParameterValue("eDate", eDate.Selected_Date)

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

    Protected Sub btnRun_Click()
        If ListBox2.Items.Count > 0 Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If

    End Sub

    Protected Sub btnRun_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        If ListBox2.Items.Count > 0 Or sDate.Selected_Date = "" Or eDate.Selected_Date = "" Then
            Setup_Report()
            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Select as least 1 Golf Course and make sure Start Date and End Date are selected.');", True)
        End If
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        '>> Course
        If ListBox1.SelectedIndex >= 0 Then
            ListBox2.Items.Add(ListBox1.SelectedItem)
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            ListBox2.SelectedIndex = 0
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        '<< Course
        If ListBox2.SelectedIndex >= 0 Then
            ListBox1.Items.Add(ListBox2.SelectedItem)
            ListBox2.Items.Remove(ListBox2.SelectedItem)
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub BtnAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAll.Click
        ' ALL >> Course
        For i = ListBox1.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = ListBox1.Items(i)
            ListBox2.ClearSelection()
            ListBox2.Items.Add(lItem)
            ListBox1.Items.Remove(lItem)
        Next
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        'ALL << Course
        For i = ListBox2.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = ListBox2.Items(i)
            ListBox1.ClearSelection()
            ListBox1.Items.Add(lItem)
            ListBox2.Items.Remove(lItem)
        Next
    End Sub
End Class
