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

Partial Class Reports_Accounting_MFRollForward
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/MFRollForward.rpt"

    Private Sub BindData()
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select * from t_Comboitems c inner join t_Combos co on c.ComboID = co.ComboID where co.ComboName = 'TransCode' and (c.comboitem like 'mf%' or c.ComboItem like 'SA%') and c.active = 1 and len(c.comboitem) = 4 order by c.comboitem"
            ddMF.DataSource = ds
            ddMF.DataTextField = "Comboitem"
            ddMF.DataValueField = "Comboitem"
            ddMF.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
        ds = Nothing
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
        If dfSDate.Selected_Date <> "" And dfEDate.Selected_Date <> "" Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()

        Report.SetParameterValue("SDate", CDate(IIf(dfSDate.Selected_Date <> "", dfSDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("EDate", CDate(IIf(dfEDate.Selected_Date <> "", dfEDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("Invoice", ddMF.SelectedValue)
        
    End Sub

End Class
