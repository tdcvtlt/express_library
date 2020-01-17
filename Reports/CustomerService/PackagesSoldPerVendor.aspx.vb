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

Partial Class Reports_CustomerService_PackagesSoldPerVendor
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/PackagesSoldPerVendor.rpt"
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack Then If Not Session("report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("report")
        Err.Text = String.Empty
    End Sub

    Protected Sub submit_Click(sender As Object, e As System.EventArgs) Handles submit.Click
        Session("report") = Nothing
        CrystalReportViewer1.ReportSource = Nothing
        Dim vendor = New clsVendor()
        vendor.VendorID = CType(Session("User"), User).ActiveVendor
        vendor.Load()
        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)
        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        Report.SetParameterValue("sd", CDate(IIf(dteSDate.Selected_Date <> "", dteSDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("ed", CDate(IIf(dteEDate.Selected_Date <> "", dteEDate.Selected_Date, Date.Today)))       
        Report.SetParameterValue("vendor", vendor.Vendor.ToUpper())
        Session.Add("report", Report)
        CrystalReportViewer1.ReportSource = Session("Report")        
        If Report.HasRecords = False Then
            CrystalReportViewer1.ReportSource = Nothing
            Err.Text = "No data"
        End If
    End Sub
End Class
