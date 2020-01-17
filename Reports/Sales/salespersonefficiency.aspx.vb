Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data.Linq.Mapping
Imports System.Data.Linq
Imports System.Web.Services
Imports System.IO
Imports System.Web.Script.Serialization
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Security
Imports System.Web
Imports System.Configuration

Partial Class Reports_Sales_salespersonefficiency
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/salespersonefficiency.rpt"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            siTourLocation.Connection_String = Resources.Resource.cns
            siTourLocation.ComboItem = "TourLocation"
            siTourLocation.Selected_ID = 0
            siTourLocation.Label_Caption = ""
            siTourLocation.Load_Items()

            siTitle.Connection_String = Resources.Resource.cns
            siTitle.ComboItem = "PersonnelTitle"
            siTitle.Selected_ID = 0
            siTitle.Label_Caption = ""
            siTitle.Load_Items()
        End If
        If IsPostBack And Not Session("Report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("Report")
    End Sub

    Protected Sub btRun_Click(sender As Object, e As System.EventArgs) Handles btRun.Click
        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)
        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        With Report
            .SetParameterValue("sd", CDate(IIf(sd.Selected_Date <> "", sd.Selected_Date, Date.Today)))
            .SetParameterValue("ed", CDate(IIf(ed.Selected_Date <> "", ed.Selected_Date, Date.Today)))
            .SetParameterValue("titleid", siTitle.Selected_ID)
            .SetParameterValue("tourlocationid", siTourLocation.Selected_ID)
        End With
        Session.Add("Report", Report)
        CrystalReportViewer1.ReportSource = Session("Report")
    End Sub
End Class
