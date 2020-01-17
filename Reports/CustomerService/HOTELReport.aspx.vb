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
Partial Class Reports_CustomerService_HOTELReport
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/HOTELReport.rpt"

    Private Sub BindData()
        Try

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then

        Else

            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand("select * from t_Accom where active = 1 and AccomLocationID = 1007 and AccomName <> 'kcp' order by AccomName", cn)
                    Try

                        cn.Open()

                        Dim dt = New DataTable()
                        dt.Load(cm.ExecuteReader())

                        ddlHotels.DataSource = dt
                        ddlHotels.DataTextField = "AccomName"
                        ddlHotels.DataValueField = "AccomName"
                        ddlHotels.DataBind()

                    Catch ex As Exception
                        cn.Close()

                    End Try
                End Using
            End Using

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
        If sdate.Selected_Date <> "" And edate.Selected_Date <> "" Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
            CrystalReportViewer1.SelectionFormula = "{Command.Accommodation}='" & ddlHotels.SelectedItem.Text & "'"            

        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()

        Report.SetParameterValue("SDate", CDate(IIf(sdate.Selected_Date <> "", sdate.Selected_Date, Date.Today)))
        Report.SetParameterValue("EDate", CDate(IIf(edate.Selected_Date <> "", edate.Selected_Date, Date.Today)))       

    End Sub



End Class
