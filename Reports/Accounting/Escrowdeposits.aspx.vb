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

Partial Class Reports_Accounting_Escrowdeposits
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/escrowdeposits.rpt"

    Private Sub BindData()
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("select distinct m.accountid,m.Description from v_Invoices i inner join t_CCMerchantAccount m on m.AccountID=i.Acct where i.KeyField = 'MortgageDP' order by m.Description", cn)
        Dim dr As SqlDataReader
        Dim dt As New DataTable
        Try
            cn.Open()
            dr = cm.ExecuteReader
            dt.Load(dr)

        Catch ex As Exception
        Finally
            dr.Close()
            If cn.State <> ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try
        ddAccount.DataSource = dt
        ddAccount.DataValueField = "AccountID"
        ddAccount.DataTextField = "Description"
        ddAccount.DataBind()
        dt = Nothing
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

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        If dfEdate.Selected_Date <> "" Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()

        Report.SetParameterValue("SDate", CDate(IIf(dfEdate.Selected_Date <> "", dfEdate.Selected_Date, Date.Today)).AddDays(-7))
        Report.SetParameterValue("EDate", CDate(IIf(dfEdate.Selected_Date <> "", dfEdate.Selected_Date, Date.Today)).AddDays(1))
        Report.SetParameterValue("Acct", ddAccount.SelectedItem.Value)
    End Sub
End Class
