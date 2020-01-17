Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class Reports_FrontDesk_BacktoBackRes
    Inherits System.Web.UI.Page
    Private reportPath As String = "REPORTFILES/BacktoBackRes.RPT"
    Private report As New ReportDocument
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            If IsPostBack = False Then
                'siTourLocation.Connection_String = Resources.Resource.cns
                'siTourLocation.ComboItem = "TourLocation"
                'siTourLocation.Label_Caption = ""
                'siTourLocation.Load_Items()
            End If
        End If

        If Not Session("Crystal") Is Nothing Then
            If Not Session("UserID") Is Nothing And Me.IsPostBack = True Then
                CrystalViewer.ReportSource = Session("Crystal")
            Else
                If Session("Crystal") Is Nothing Then
                    Response.Redirect("http://crms.kingscreekplantation.com/crmsnet/logon.aspx")
                End If

            End If
        End If
    End Sub

    Protected Sub RunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunReport.Click
        If (Not Session("Crystal") Is Nothing) Then
            Session("Crystal") = Nothing
            CrystalViewer.Visible = False
        End If

        If (String.IsNullOrEmpty(Me.SDATE.Selected_Date) Or String.IsNullOrEmpty(Me.EDATE.Selected_Date)) Then Return

        report.FileName = Server.MapPath(reportPath)
        report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        report.SetParameterValue("SDATE", Me.SDATE.Selected_Date.Trim())
        report.SetParameterValue("EDATE", Me.EDATE.Selected_Date.Trim())
        'report.SetParameterValue("tourLocationID", siTourLocation.Selected_ID)

        report.Load(Server.MapPath(reportPath))

        Session("Crystal") = report
        CrystalViewer.ReportSource = Session("Crystal")

        CrystalViewer.Visible = True
    End Sub
End Class
