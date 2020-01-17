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

Partial Class Reports_CustomerService_TransactionSummaryPerVendor
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/TransactionSummaryPerVendor.rpt"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack Then If Not Session("report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("report")
        Err.Text = String.Empty

        If IsPostBack = False Then
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand()
                    cm.CommandText = String.Format("SELECT Distinct(f.FinTransID) AS ID, tc.ComboItem AS Item " & _
                        "FROM t_FinTransCodes f INNER JOIN t_ComboItems tc ON f.TransCodeID = tc.ComboItemID " & _
                        "INNER JOIN t_PackageFinTransCode pf ON f.FinTransID = pf.FinTransCodeID " & _
                        "INNER JOIN t_Vendor2Package vp ON pf.PackageID = vp.PackageID " & _
                        "WHERE vp.VendorID = {0}", CType(Session("user"), User).ActiveVendor)
                    cm.Connection = cn

                    Try
                        cn.Open()
                        Dim rd = cm.ExecuteReader()

                        ddlTransCode.DataTextField = "Item"
                        ddlTransCode.DataValueField = "ID"
                        ddlTransCode.DataSource = rd
                        ddlTransCode.DataBind()
                    Catch ex As Exception
                        Err.Text = ex.Message
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End If
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
        Report.SetParameterValue("sd", CDate(IIf(sd.Selected_Date <> "", sd.Selected_Date, Date.Today)))
        Report.SetParameterValue("ed", CDate(IIf(ed.Selected_Date <> "", Convert.ToDateTime(ed.Selected_Date).AddDays(1), Date.Today)))
        Report.SetParameterValue("transCodeID", ddlTransCode.SelectedValue)        
        Report.SetParameterValue("vendorID", vendor.VendorID)
        Session.Add("report", Report)
        CrystalReportViewer1.ReportSource = Session("Report")
        If Report.HasRecords = False Then
            CrystalReportViewer1.ReportSource = Nothing
            Err.Text = "No data"
        End If
    End Sub
End Class
