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

Partial Class Reports_Rentals_LeadsPackagesExpired
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/LeadsPackagesExpired.rpt"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If String.IsNullOrEmpty(Request("vendorid")) Then

                cblVendorList.DataSource = New SqlDataSource(Resources.Resource.cns, "Select VendorID, Vendor from t_Vendor where active = 1 order by vendor asc")
                cblVendorList.DataTextField = "Vendor"
                cblVendorList.DataValueField = "VendorID"
                cblVendorList.DataBind()

                checkAll.Visible = True
                lblCheckAll.Visible = True
            End If
        Else
            If Not Session("report") Is Nothing Then
                CrystalReportViewer1.ReportSource = Session("report")
            End If
        End If
    End Sub

    Protected Sub btnRunReport_Click(sender As Object, e As System.EventArgs) Handles btnRunReport.Click
        Session("report") = Nothing
        CrystalReportViewer1.ReportSource = Nothing

        If String.IsNullOrEmpty(Request("vendorid")) Then
            Dim checks = cblVendorList.Items.OfType(Of ListItem).Where(Function(x) x.Selected)

            If checks.Count() = 0 Or dfE.Selected_Date.Length = 0 Then Return

            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Report.SetParameterValue("Values", String.Join(",", checks.Select(Function(x) String.Format("'{0}'", x.Value)).ToArray()))
            Report.SetParameterValue("EDate", CDate(IIf(dfE.Selected_Date <> "", dfE.Selected_Date, Date.Today)))
            Report.SetParameterValue("SDate", CDate(IIf(dfS.Selected_Date <> "", dfS.Selected_Date, Date.Today)))

            Session.Add("Report", Report)
            CrystalReportViewer1.ReportSource = Session("report")
        Else
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

            Report.SetParameterValue("Values", String.Format("'{0}'", Request("vendorid")))
            Report.SetParameterValue("EDate", CDate(IIf(dfE.Selected_Date <> "", dfE.Selected_Date, Date.Today)))
            Report.SetParameterValue("SDate", CDate(IIf(dfS.Selected_Date <> "", dfS.Selected_Date, Date.Today)))

            Session.Add("Report", Report)
            CrystalReportViewer1.ReportSource = Session("report")
        End If
    End Sub
End Class
