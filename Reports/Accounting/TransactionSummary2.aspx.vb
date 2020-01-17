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

Partial Class Reports_Accounting_TransactionSummary2
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/transactionsummary.rpt"

    Private Sub BindData()
        Dim oAccounts As New clsCCMerchantAccount
        Dim oFinTrans As New clsFinancialTransactionCodes
        ddAccounts.Items.Add(New ListItem("ALL", 0))
        ddAccounts.DataSource = oAccounts.List_Accounts()
        ddAccounts.DataTextField = "Description"
        ddAccounts.DataValueField = "AccountID"
        ddAccounts.AppendDataBoundItems = True
        ddAccounts.DataBind()
        ddTransCode.Items.Add(New ListItem("ALL", 0))
        ddTransCode.DataSource = oFinTrans.List_Trans_Codes("")
        ddTransCode.DataTextField = "TransCode"
        ddTransCode.DataValueField = "FinTransID"
        ddTransCode.AppendDataBoundItems = True
        ddTransCode.DataBind()
        oAccounts = Nothing
        oFinTrans = Nothing
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
            'Report.Close()
            'Report.Dispose()
        Catch ex As Exception

        End Try



    End Sub



    Private Sub Setup_Report()
        If Session("Report") Is Nothing Then
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            'Dim categoryID As Integer = Convert.ToInt32(ddlCategory.SelectedValue)
            'Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            'Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Report.Subreports(0).DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            Report.Subreports(0).DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Report.Subreports(1).DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            Report.Subreports(1).DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
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
        If dteSDate.Selected_Date <> "" Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()
        'Response.Write(CDate(IIf(dteSDate.Selected_Date <> "", dteSDate.Selected_Date, Date.Today)) & "<br>")
        'Response.Write(CDate(IIf(dteEDate.Selected_Date <> "", dteEDate.Selected_Date, Date.Today)) & "<br>")
        'Response.Write(ddAccounts.SelectedValue)
        Report.SetParameterValue("SDate", CDate(IIf(dteSDate.Selected_Date <> "", dteSDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("EDate", CDate(IIf(dteEDate.Selected_Date <> "", dteEDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("acct", ddAccounts.SelectedValue)
        Report.SetParameterValue("tc", ddTransCode.SelectedValue)

        Report.SetParameterValue("SDate", CDate(IIf(dteSDate.Selected_Date <> "", dteSDate.Selected_Date, Date.Today)), "acct1112")
        Report.SetParameterValue("EDate", CDate(IIf(dteEDate.Selected_Date <> "", dteEDate.Selected_Date, Date.Today)), "acct1112")
        Report.SetParameterValue("acct", ddAccounts.SelectedValue, "acct1112")
        Report.SetParameterValue("tc", ddTransCode.SelectedValue, "acct1112")

        Report.SetParameterValue("SDate", CDate(IIf(dteSDate.Selected_Date <> "", dteSDate.Selected_Date, Date.Today)), "acct0")
        Report.SetParameterValue("EDate", CDate(IIf(dteEDate.Selected_Date <> "", dteEDate.Selected_Date, Date.Today)), "acct0")
        Report.SetParameterValue("acct", ddAccounts.SelectedValue, "acct0")
        Report.SetParameterValue("tc", ddTransCode.SelectedValue, "acct0")


    End Sub


End Class
