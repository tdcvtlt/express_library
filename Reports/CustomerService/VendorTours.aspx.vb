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

Partial Class Reports_CustomerService_VendorTours
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/VendorTours.rpt"

    Private vendor2Personnel As New clsVendor2Personnel()

    Protected Sub Submit_Click(sender As Object, e As EventArgs) Handles Submit.Click
        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)
        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        If Session("User") Is Nothing Then Session("User") = New User With {.ActiveVendor = 0}
        Dim vendorID As Integer = CType(Session("User"), User).ActiveVendor
        If vendorID = 0 Then vendorID = 72

        With Report
            .SetParameterValue("sd", CDate(IIf(dfS.Selected_Date <> "", dfS.Selected_Date, Date.Today)))
            .SetParameterValue("ed", CDate(IIf(dfE.Selected_Date <> "", dfE.Selected_Date, Date.Today)))
            .SetParameterValue("vendorid", vendorID)
            .SetParameterValue("tourLocationID", siTourLocation.Selected_ID)
        End With
        Session.Add("Report", Report)
        CrystalReportViewer1.ReportSource = Session("Report")
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack = False Then
            siTourLocation.Connection_String = Resources.Resource.cns
            siTourLocation.ComboItem = "TourLocation"
            siTourLocation.Label_Caption = ""
            siTourLocation.Load_Items()
        End If
        If IsPostBack And Not Session("Report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("Report")
    End Sub
End Class
