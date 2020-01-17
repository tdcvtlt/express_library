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

Partial Class Reports_Contracts_closingofficer
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/closingofficer.rpt"

    Private Sub BindData()
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.Comboname = 'PersonnelTitle' order by Comboitem"

        ListBox3.DataSource = ds
        ListBox3.DataTextField = "Comboitem"
        ListBox3.DataValueField = "Comboitem"
        ListBox3.DataBind()
        Dim sb As New StringBuilder
        sb.Append("Select p.PersonnelID, ISNULL(p.username,'')[UserName], ISNULL(p.lastname, '') [LastName], ISNULL(p.firstname, '')[FirstName] from t_Personnel p " & _
            "where p.personnelid in (select personnelid from t_PersonnelTrans where KEYVALUE > 0 AND KEYFIELD = 'CONTRACTID' " & _
            "and titleid in (select comboitemid from t_Comboitems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'personneltitle' " & _
            "and comboitemID in (16785))) and statusid in  " & _
            "(Select comboitemid from t_ComboItems A INNER JOIN T_COMBOS B ON A.COMBOID = B.COMBOID where comboname = 'PersonnelStatus' and comboitem = 'Active') order by lastname, firstname")
        ds.SelectCommand = "Select * from t_Personnel  order by username"
        ds.SelectCommand = sb.ToString
        ListBox1.DataSource = ds
        ListBox1.DataTextField = "UserName"
        ListBox1.DataValueField = "UserName"
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
            sItems &= IIf(sItems = "", ListBox2.Items(i).Text, "," & ListBox2.Items(i).Text)
        Next
        Dim sOfficers As String = ""
        For i = 0 To ListBox4.Items.Count - 1
            sItems &= IIf(sItems = "", ListBox4.Items(i).Text, "," & ListBox4.Items(i).Text)
        Next
        Report.SetParameterValue("SDate", sdate.Selected_Date)
        Report.SetParameterValue("EDate", edate.Selected_Date)
        Report.SetParameterValue("Closing Officer", sOfficers)
        Report.SetParameterValue("Title", sItems)
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        '<<
        If ListBox4.SelectedIndex >= 0 Then
            ListBox3.Items.Add(ListBox4.SelectedItem)
            ListBox4.Items.Remove(ListBox4.SelectedItem)
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click
        '>>
        If ListBox3.SelectedIndex >= 0 Then
            ListBox4.Items.Add(ListBox3.SelectedItem)
            ListBox3.Items.Remove(ListBox3.SelectedItem)
            ListBox4.SelectedIndex = 0
            hfShowReport.Value = 0
        End If
    End Sub
End Class
