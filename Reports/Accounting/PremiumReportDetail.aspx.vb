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
Partial Class Reports_Accounting_PremiumReportDetail
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/premiumreportdetail.rpt"

    Private Sub BindData()
        If IsPostBack = False Then          
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using ad = New SqlDataAdapter("select * from t_ComboItems ci inner join t_Combos c on ci.ComboID = c.ComboID where c.ComboName = 'tourlocation' and ci.Active=1 order by ci.ComboItem", cn)
                    Try
                        Dim dt = New DataTable()
                        ad.Fill(dt)
                        cblTourLocation.DataSource = dt
                        cblTourLocation.DataTextField = "comboitem"
                        cblTourLocation.DataValueField = "comboitemid"
                        cblTourLocation.DataBind()
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End If        
    End Sub

    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack Then
        Else
            BindData()
        End If

        If hfShowReport.Value = 1 Then            
            CrystalReportViewer1.ReportSource = Session("Report")
        End If

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            'Report.Clone()
            'Report.Dispose()
        Catch ex As Exception

        End Try

    End Sub



    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        If sdate.Selected_Date <> "" And edate.Selected_Date <> "" And cblTourLocation.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).Select(Function(x) x.Value).ToArray().Count > 0 Then
            hfShowReport.Value = 1
            Setup_Report()
            CrystalReportViewer1.ReportSource = Session("Report")
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
            Report.SetParameterValue("SDate", sdate.Selected_Date)
            Report.SetParameterValue("EDate", edate.Selected_Date)
            Dim l = cblTourLocation.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).Select(Function(x) x.Value).ToArray()
            Report.SetParameterValue("tourLocations", String.Join(",", l))
            Session.Add("Report", Report)
        Else
            Report = Session("Report")
            If Report.FileName <> Server.MapPath(sReport) Then
                Session("Report") = Nothing
                Setup_Report()
            End If
        End If
    End Sub
End Class
