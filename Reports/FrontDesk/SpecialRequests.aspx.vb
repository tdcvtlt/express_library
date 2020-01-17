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

Partial Class SpecialRequests_Default
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/specialrequests.rpt"
    Private Sub BindData()

        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname = 'ReservationStatus'"

        ListBox1.DataSource = ds
        ListBox1.DataTextField = "Comboitem"
        ListBox1.DataValueField = "Comboitem"
        ListBox1.DataBind()

        ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname = 'ReservationType'"

        ListBox3.DataSource = ds
        ListBox3.DataTextField = "Comboitem"
        ListBox3.DataValueField = "Comboitem"
        ListBox3.DataBind()

        ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname = 'SpecialNeeds'"

        ListBox5.DataSource = ds
        ListBox5.DataTextField = "Comboitem"
        ListBox5.DataValueField = "Comboitem"
        ListBox5.DataBind()


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

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        If DateField1.Selected_Date <> "" And DateField2.Selected_Date <> "" And ListBox2.Items.Count > 0 And ListBox4.Items.Count > 0 And ListBox6.Items.Count > 0 Then
            hfShowReport.Value = 1
            Setup_Report()
            Report.SetParameterValue("SDate", DateField1.Selected_Date)
            Report.SetParameterValue("EDate", DateField2.Selected_Date)
            Dim sItems As String = ""
            For i = 0 To ListBox2.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & ListBox2.Items(i).Text & "'", ",'" & ListBox2.Items(i).Text & "'")
            Next
            Report.SetParameterValue("ResStatus", sItems)
            'Response.Write("resstatus: " & sItems)
            sItems = ""
            For i = 0 To ListBox4.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & ListBox4.Items(i).Text & "'", ",'" & ListBox4.Items(i).Text & "'")
            Next
            'Response.Write("ResType: " & sItems)
            Report.SetParameterValue("ResType", sItems)
            sItems = ""
            For i = 0 To ListBox6.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & ListBox6.Items(i).Text & "'", ",'" & ListBox6.Items(i).Text & "'")
            Next
            Report.SetParameterValue("ReqType", sItems)
            'Response.Write(sItems)
            CrystalReportViewer1.ReportSource = Session("Report")
            'Response.Write("ReqType: " & sItems)
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
            Report.SetParameterValue("SDate", "1/1/12")
            Report.SetParameterValue("EDate", "1/1/12")
            Report.SetParameterValue("ResStatus", "")
            Report.SetParameterValue("ResType", "")
            Report.SetParameterValue("ReqType", "")
            Session.Add("Report", Report)
        Else
            Report = Session("Report")
            If Report.FileName <> Server.MapPath(sReport) Then
                Session("Report") = Nothing
                Setup_Report()
            End If
        End If
    End Sub

    Private Sub Move(ByRef listboxSrc As ListBox, ByRef listboxdest As ListBox)
        If listboxSrc.SelectedIndex >= 0 Then
            listboxdest.ClearSelection()
            listboxdest.Items.Add(listboxSrc.SelectedItem)
            listboxSrc.Items.Remove(listboxSrc.SelectedItem)
            listboxdest.SelectedIndex = 0
            hfShowReport.Value = 0
        End If
    End Sub

    Private Sub Move_All(ByRef listboxSrc As ListBox, ByRef listboxdest As ListBox)
        For i = listboxSrc.Items.Count - 1 To 0 Step -1
            listboxdest.ClearSelection()
            listboxdest.Items.Add(listboxSrc.Items(i))
            listboxSrc.Items.Remove(listboxSrc.Items(i))
        Next
        hfShowReport.Value = 0
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        '>>
        Move(ListBox1, ListBox2)
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        '<<
        Move(ListBox2, ListBox1)
    End Sub

    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click
        '<<<
        Move_All(ListBox2, ListBox1)
    End Sub

    Protected Sub Button5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button5.Click
        '>>>
        Move_All(ListBox1, ListBox2)
    End Sub

    Protected Sub Button6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button6.Click
        Move_All(ListBox4, ListBox3)
    End Sub

    Protected Sub Button9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button9.Click
        Move_All(ListBox3, ListBox4)
    End Sub

    Protected Sub Button7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button7.Click
        Move(ListBox4, ListBox3)
    End Sub

    Protected Sub Button8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button8.Click
        Move(ListBox3, ListBox4)
    End Sub

    Protected Sub Button10_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button10.Click
        Move_All(ListBox6, ListBox5)
    End Sub

    Protected Sub Button11_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button11.Click
        Move(ListBox6, ListBox5)
    End Sub

    Protected Sub Button12_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button12.Click
        Move(ListBox5, ListBox6)
    End Sub

    Protected Sub Button13_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button13.Click
        Move_All(ListBox5, ListBox6)
    End Sub
End Class
