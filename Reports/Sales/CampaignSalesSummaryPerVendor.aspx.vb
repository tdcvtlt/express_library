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

Partial Class Reports_Sales_CampaignSalesSummaryPerVendor
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/CampaignSalesSummaryPerVendor.rpt"
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            With ddCampaign
                Using cn = New SqlConnection(Resources.Resource.cns)
                    Using cm = New SqlCommand( _
                               String.Format("select c.Name, c.CampaignID, v.Vendor, v.VendorID from t_VendorCampaigns vc " & _
                                             "inner join t_Vendor v on v.VendorID = vc.VendorID " & _
                                             "inner join t_Campaign c on c.CampaignID = vc.CRMSCampID " & _
                                             "where c.Active = 1  and v.VendorID = {0} order by c.Name", CType(Session("user"), User).ActiveVendor), cn)
                        Try
                            cn.Open()
                            Dim dt = New DataTable()
                            Dim r = cm.ExecuteReader()
                            dt.Load(r)
                            .DataSource = dt

                            .DataValueField = "CampaignID"
                            .DataTextField = "Name"
                            .DataBind()
                        Catch ex As Exception
                            Response.Write(ex.Message)
                        Finally
                            cn.Close()
                        End Try
                    End Using
                End Using                
            End With
        End If
        If IsPostBack Then If Not Session("report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("report")
        Err.Text = String.Empty
    End Sub

    Protected Sub btRunReport_Click(sender As Object, e As System.EventArgs) Handles btRunReport.Click
        If String.IsNullOrEmpty(ddCampaign.SelectedValue) Then Return
        Session("report") = Nothing
        CrystalReportViewer1.ReportSource = Nothing
        Dim vendor = New clsVendor()
        vendor.VendorID = CType(Session("User"), User).ActiveVendor
        vendor.Load()
        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)
        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        Report.SetParameterValue("sd", CDate(IIf(sd.Selected_Date <> "", sd.Selected_Date, Date.Today)))
        Report.SetParameterValue("ed", CDate(IIf(ed.Selected_Date <> "", ed.Selected_Date, Date.Today)))
        Report.SetParameterValue("campaignid", ddCampaign.SelectedValue)
        Report.SetParameterValue("vendorid", vendor.VendorID)
        Report.SetParameterValue("ow", IIf(cbOW.Checked, 1, 0))
        Session.Add("report", Report)
        CrystalReportViewer1.ReportSource = Session("Report")
        If Report.HasRecords = False Then
            CrystalReportViewer1.ReportSource = Nothing
            Err.Text = "No data"
        End If
    End Sub
End Class
