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
Partial Class Reports_CustomerService_PackageStatusReport
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/packagestatus.rpt"

    Private Sub BindData()
        Dim oCombo As New clsComboItems
        ListBox1.DataSource = oCombo.Load_ComboItems("PackageStatus")
        ListBox1.DataValueField = "ComboItemID"
        ListBox1.DataTextField = "ComboItem"
        ListBox1.DataBind()
        oCombo = Nothing
    End Sub

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

    Protected Sub Set_Params()
        Dim sItems As String = ""
        sItems = ""
        If ListBox2.Items.Count = 0 Then
            For i = 0 To ListBox1.Items.Count - 1
                sItems &= IIf(sItems = "", ListBox1.Items(i).Text, "," & ListBox1.Items(i).Text)
            Next
        Else
            For i = 0 To ListBox2.Items.Count - 1
                sItems &= IIf(sItems = "", ListBox2.Items(i).Text, "," & ListBox2.Items(i).Text)
            Next
        End If
        'Response.Write(sItems)

        'Dim statusSelections As IEnumerable(Of String) = IIf(ListBox2.Items.Count = 0, _
        '                           ListBox1.Items.OfType(Of ListItem).Select(Function(x) String.Format("{0}", x.Text)).ToArray(), _
        '                           ListBox2.Items.OfType(Of ListItem).Select(Function(x) String.Format("{0}", x.Text)).ToArray())


        'Response.Write(String.Join(",", statusSelections))

        Report.SetParameterValue("Statuses", sItems)
        Report.SetParameterValue("sDate", dteSDate.Selected_Date)
        Report.SetParameterValue("eDate", dteEDate.Selected_Date)
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

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        '>> Status
        If ListBox1.SelectedIndex >= 0 Then
            ListBox2.Items.Add(ListBox1.SelectedItem)
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            ListBox2.SelectedIndex = 0
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        '<< Status
        If ListBox2.SelectedIndex >= 0 Then
            ListBox1.Items.Add(ListBox2.SelectedItem)
            ListBox2.Items.Remove(ListBox2.SelectedItem)
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub BtnAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAll.Click
        ' ALL >> Status
        For i = ListBox1.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = ListBox1.Items(i)
            ListBox2.ClearSelection()
            ListBox2.Items.Add(lItem)
            ListBox1.Items.Remove(lItem)
        Next
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        'ALL << Status
        For i = ListBox2.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = ListBox2.Items(i)
            ListBox1.ClearSelection()
            ListBox1.Items.Add(lItem)
            ListBox2.Items.Remove(lItem)
        Next
    End Sub

    Protected Sub btnRun_Click1(sender As Object, e As System.EventArgs) Handles btnRun.Click
        If ListBox2.Items.Count > 0 Then
            If dteSDate.Selected_Date = "" Or dteEDate.Selected_Date = "" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Select a Start Date and End Date.');", True)
            Else
                'hfShowReport.Value = 1
                Setup_Report()
                CrystalReportViewer1.ReportSource = Session("Report")
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Select as least 1 Status.');", True)
        End If
    End Sub
End Class
