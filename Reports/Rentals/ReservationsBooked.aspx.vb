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


Partial Class Reports_Rentals_ReservationsBooked
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/ReservationsBooked.rpt"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then          
            checkbox_list.DataSource = New clsComboItems().Load_ComboItems("ReservationStatus")
            checkbox_list.DataTextField = "comboItem"
            checkbox_list.DataValueField = "comboItemID"
            checkbox_list.DataBind()

        Else
            If Session("Report") IsNot Nothing Then CrystalReportViewer1.ReportSource = Session("Report")
        End If
    End Sub


    

    Protected Sub btnRunReport_Click(sender As Object, e As System.EventArgs) Handles btnRunReport.Click

        gv.DataSource = Nothing
        gv.DataBind()

        If String.IsNullOrEmpty(dfStartDate.Selected_Date) Or String.IsNullOrEmpty(dfEndDate.Selected_Date) Or _
            String.Join(",", checkbox_list.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).Select(Function(x) x.Value).ToArray()).Length = 0 Then
            Return
        End If


        CrystalReport_Show()

        Return

        'Using cnn = New SqlConnection(Resources.Resource.cns)            
        '    Using cmd = New SqlCommand("sp_Reservations_Booked", cnn)
        '        cmd.CommandTimeout = 0
        '        cmd.CommandType = CommandType.StoredProcedure
        '        cmd.Parameters.AddWithValue("sdate", dfStartDate.Selected_Date)
        '        cmd.Parameters.AddWithValue("edate", dfEndDate.Selected_Date)
        '        cmd.Parameters.AddWithValue("reservation_status", String.Format("{0}", _
        '                String.Join(",", checkbox_list.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).Select(Function(x) x.Value).ToArray())))

        '        If Not Session("user") Is Nothing Then
        '            Dim vendorID = CType(Session("user"), User).VendorMenu
        '            cmd.Parameters.AddWithValue("vendorID", vendorID)
        '        Else
        '            cmd.Parameters.AddWithValue("vendorID", 0)
        '        End If

        '        Dim i = 0
        '        If radiobutton_1.Checked Then i = 1
        '        If radiobutton_2.Checked Then i = 2
        '        If radiobutton_3.Checked Then i = 3

        '        cmd.Parameters.AddWithValue("checkin_or_datebooked", i)

        '        cnn.Open()
        '        Dim rdr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        '        Response.Write(String.Join(",", checkbox_list.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).Select(Function(x) x.Value).ToArray()))

        '        If rdr.HasRows Then
        '            gv.DataSource = rdr
        '            Dim xy = New clsReservationWizard

        '            If Not Session("user") Is Nothing Then
        '                Dim avID = CType(Session("user"), User).VendorMenu

        '                Dim v = New clsVendor()
        '                With v
        '                    .VendorID = avID
        '                    .Load()
        '                    If .Vendor.ToLower() = "Grand Incentives".Trim().ToLower() Then
        '                        Dim delColumns = New List(Of DataControlField)
        '                        For Each dcf As DataControlField In gv.Columns
        '                            If dcf.HeaderText.ToLower() = "Booking Agent".ToLower() Or dcf.HeaderText.ToLower() = "Package Agent".ToLower() Then
        '                                delColumns.Add(dcf)
        '                            End If
        '                        Next
        '                        For Each dcf In delColumns
        '                            gv.Columns.Remove(dcf)
        '                        Next
        '                    End If
        '                End With
        '            End If

        '            gv.DataBind()
        '        End If
        '    End Using

        'End Using
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As System.Web.UI.Control)
        'MyBase.VerifyRenderingInServerForm(control)
        'Must have this line of code to prevent runtime error when exporting the gridview to excel.
    End Sub

    Private Sub CrystalReport_Show()

        Session("Report") = Nothing

        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)
        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        Dim i = 0
        If radiobutton_1.Checked Then i = 1
        If radiobutton_2.Checked Then i = 2
        If radiobutton_3.Checked Then i = 3

        Report.SetParameterValue("sdate", CDate(IIf(dfStartDate.Selected_Date <> "", dfStartDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("edate", CDate(IIf(dfEndDate.Selected_Date <> "", dfEndDate.Selected_Date, Date.Today)))

        Report.SetParameterValue("param", i)
        Report.SetParameterValue("rstatus", String.Format("{0}", _
                        String.Join(",", checkbox_list.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).Select(Function(x) x.Value).ToArray())))
        Report.SetParameterValue("vendorid", CType(Session("user"), User).VendorMenu)

        Session.Add("Report", Report)
        CrystalReportViewer1.ReportSource = Session("Report")
    End Sub

    Protected Sub btnExcelReport_Click(sender As Object, e As System.EventArgs) Handles btnExcelReport.Click
        Response.ClearContent()
        Response.AddHeader("content-disposition", "attachment;filename=FileName.xls")
        Response.Charset = String.Empty
        Response.ContentType = "application/vnd.xls"
        Dim sw = New System.IO.StringWriter()
        Dim hw = New HtmlTextWriter(sw)

        gv.RenderControl(hw)

        Response.Write(sw.ToString())
        Response.End()
    End Sub
End Class
