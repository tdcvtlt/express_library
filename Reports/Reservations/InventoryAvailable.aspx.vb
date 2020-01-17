Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class Reports_Reservations_InventoryAvailable
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/InventoryAvailable.rpt"
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            siInvType.Connection_String = Resources.Resource.cns
            siInvType.ComboItem = "ReservationType"
            siInvType.Label_Caption = ""
            siInvType.Load_Items()
            siUnitType.Connection_String = Resources.Resource.cns
            siUnitType.ComboItem = "UnitType"
            siUnitType.Label_Caption = ""
            siUnitType.Load_Items()
            ddBD.Items.Add(New ListItem("1BD", "1"))
            ddBD.Items.Add(New ListItem("1BD-DWN", "1BD-DWN"))
            ddBD.Items.Add(New ListItem("1BD-UP", "1BD-UP"))
            ddBD.Items.Add(New ListItem("2BD", "2"))
            ddBD.Items.Add(New ListItem("3BD", "3"))
            ddBD.Items.Add(New ListItem("4BD", "4"))
            Session("Report") = Nothing
            Setup_Report()
        End If
        If hfShowReport.Value = 1 Then CrystalReportViewer1.ReportSource = Session("Report")
    End Sub

    Private Sub Setup_Report()
        If Session("Report") Is Nothing Then
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            'Response.Write("HERE")
            'Dim categoryID As Integer = Convert.ToInt32(ddlCategory.SelectedValue)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            'Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
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

    Protected Sub Set_Params()

        Report.SetParameterValue("SDate", CDate(IIf(dteSDate.Selected_Date <> "", dteSDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("EDate", CDate(IIf(dteEDate.Selected_Date <> "", dteEDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("UnitType", siUnitType.SelectedName)
        Report.SetParameterValue("UnitTypeID", siUnitType.Selected_ID)
        Report.SetParameterValue("ResType", siInvType.SelectedName)
        Report.SetParameterValue("ResTypeID", siInvType.Selected_ID)
        Report.SetParameterValue("BD", ddBD.SelectedValue)

    End Sub

    Protected Sub btnRun_Click(sender As Object, e As System.EventArgs) Handles btnRun.Click
        If dteSDate.Selected_Date <> "" Or dteEDate.Selected_Date <> "" Or siInvType.Selected_ID < 0 Or siUnitType.Selected_ID < 0 Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub
End Class
