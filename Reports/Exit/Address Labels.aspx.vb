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

Partial Class Reports_Exit_Address_Labels
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/AddressLabels.rpt"

    Private Sub BindData()

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

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRunReport.Click
        hfShowReport.Value = 1
        Setup_Report()

        CrystalReportViewer1.ReportSource = Session("Report")
        
    End Sub

    Protected Sub Set_Params()
        Dim sKCP As String = ""
        For i = 0 To lstKCP.Items.Count - 1
            sKCP &= IIf(sKCP = "", "'" & lstKCP.Items(i).Text & "'", ",'" & lstKCP.Items(i).Text & "'")
        Next
        Report.SetParameterValue("SKCP", sKCP)
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim oKCP As New clsContract
        Dim KCPID As Integer = oKCP.Get_Contract_ID(txtKCP.Text)
        If KCPID > 0 Then
            lstKCP.Items.Add(txtKCP.Text)
        Else
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Alert", "alert('Unable to verify this Contract Number');", True)
        End If
        oKCP = Nothing
        hfShowReport.Value = 0
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If lstKCP.SelectedIndex > -1 Then
            lstKCP.Items.Remove(lstKCP.SelectedItem)
            hfShowReport.Value = 0
        End If
    End Sub
End Class
