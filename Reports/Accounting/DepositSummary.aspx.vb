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

Partial Class Reports_Accounting_DepositSummary
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/DepositSummary.rpt"

    Private Sub BindData()
        Dim ds As New SqlDataSource
        Dim ds2 As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds2.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select * from t_CCMerchantAccount where Active = 1 order by AccountName desc"
            ddMA.DataSource = ds
            ddMA.DataTextField = "Description"
            ddMA.DataValueField = "Accountname"
            ddMA.DataBind()
            ds2.SelectCommand = "select 0 as id, '- ALL -' as item union select * from (Select fintransid as id, tc.comboitem as item from t_Fintranscodes f inner join t_Comboitems tc on tc.comboitemid = f.transcodeid and f.active = 1) a order by item"
            ddTC.DataSource = ds2
            ddTC.DataTextField = "Item"
            ddTC.DataValueField = "ID"
            ddTC.DataBind()
            siPM.ComboItem = "PaymentMethod"
            siPM.Connection_String = Resources.Resource.cns
            siPM.Load_Items()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
        ds = Nothing
        ds2 = Nothing
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
            Server.ScriptTimeout = 10000
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            'Dim categoryID As Integer = Convert.ToInt32(ddlCategory.SelectedValue)


            ''Dim ds As New SqlDataSource
            ''ds.ConnectionString = Resources.Resource.cns
            'Dim cn As New SqlConnection(Resources.Resource.cns)
            'Dim cm As New SqlCommand("Exec sp_DailyDeposits '" & ddMA.SelectedValue & "','" & dfTransDate.Selected_Date & "','" & dfEndDate.Selected_Date & "'", cn)
            'cm.CommandTimeout = 120
            'Dim da As New SqlDataAdapter(cm)
            'Dim ds As New DataSet
            'da.Fill(ds,"Rpt")
            ''            Dim d As SqlDataReader = cm.ExecuteReader
            ''ds.SelectCommand = "Exec sp_DailyDeposits '" & ddMA.SelectedValue & "','" & dfTransDate.Selected_Date & "','" & dfEndDate.Selected_Date & "'"
            'Report.SetDataSource(ds)
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
        If dfTransDate.Selected_Date <> "" Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()

        Report.SetParameterValue("Date", CDate(IIf(dfTransDate.Selected_Date <> "", dfTransDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("EDate", CDate(IIf(dfenddate.Selected_Date <> "", dfenddate.Selected_Date, Date.Today)))
        Report.SetParameterValue("Acct", ddMA.SelectedValue)
        Report.SetParameterValue("TC", ddTC.SelectedItem.Text)
        Report.SetParameterValue("PM", siPM.SelectedName)
    End Sub


    Protected Sub ddTC_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddTC.SelectedIndexChanged

    End Sub
End Class
