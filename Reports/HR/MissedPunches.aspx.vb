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

Partial Class Reports_HR_MissedPunches
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/MissedPunches.rpt"

    Private Sub BindData()
        Dim oPers2Dept As New clsPersonnel2Dept
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.Comboname = 'PayrollCompany' order by Comboitem"

        ListBox1.DataSource = ds
        ListBox1.DataTextField = "Comboitem"
        ListBox1.DataValueField = "Comboitem"
        ListBox1.DataBind()

        ListBox3.Items.Add(New ListItem("ALL", "ALL"))
        If oPers2Dept.Member_Of(Session("UserDBID"), "HR") Then
            ds.SelectCommand = "Select * from t_ComboItems i inner join t_Combos c on c.ComboID = i.ComboID where c.ComboName = 'Department' order by comboitem"
        Else
            ds.SelectCommand = "Select * from t_CombOItems where comboitemid in (Select Distinct(DepartmentID) from t_Personnel2Dept where isManager = 1 and Active = 1 and personnelid = '" & Session("UserDBID") & "') order by comboitem"
        End If
        ListBox3.DataSource = ds
        ListBox3.DataTextField = "ComboItem"
        ListBox3.DataValueField = "ComboItem"
        ListBox3.AppendDataBoundItems = True
        ListBox3.DataBind()
        ds = Nothing
        oPers2Dept = Nothing
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
        If sdate.Selected_Date <> "" And edate.Selected_Date <> "" And ListBox2.Items.Count > 0 And ListBox4.Items.Count > 0 Then
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
        Report.SetParameterValue("pDate", sdate.Selected_Date)
        Report.SetParameterValue("eDate", edate.Selected_Date)
        Report.SetParameterValue("Company", sItems)
        sItems = ""
        For i = 0 To ListBox4.Items.Count - 1
            sItems &= IIf(sItems = "", "'" & Replace(ListBox4.Items(i).Text, "'", "''") & "'", ",'" & Replace(ListBox4.Items(i).Text, "'", "''") & "'")
        Next
        Report.SetParameterValue("Department", sItems)

    End Sub

    Protected Sub Button4_Click(sender As Object, e As System.EventArgs) Handles Button4.Click
        If ListBox3.SelectedIndex >= 0 Then
            If ListBox3.SelectedItem.Text = "ALL" Then
                Dim i As Integer = 1
                Do While ListBox3.Items.Count > 1
                    ListBox4.Items.Add(ListBox3.Items(i))
                    ListBox3.Items.Remove(ListBox3.Items(i))
                Loop
                ListBox3.Items.Remove(ListBox3.Items(0))
            Else
                ListBox4.Items.Add(ListBox3.SelectedItem)
                ListBox3.Items.Remove(ListBox3.SelectedItem)
                ListBox4.SelectedIndex = 0
            End If
        End If
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        If ListBox4.SelectedIndex >= 0 Then
            ListBox3.Items.Add(ListBox4.SelectedItem)
            ListBox4.Items.Remove(ListBox4.SelectedItem)
            ListBox3.SelectedIndex = 0
        End If
    End Sub
End Class
