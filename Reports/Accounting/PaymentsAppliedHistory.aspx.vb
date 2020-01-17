Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data
Imports System.Data.SqlClient

Partial Class Reports_Accounting_PaymentsAppliedHistory
    Inherits System.Web.UI.Page

    Private reportPath As String = "reportfiles/PaymentsApplied.rpt"
    Private report As New ReportDocument

    Protected Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click

        Session("Crystal") = Nothing
        CrystalViewer.ReportSource = Nothing

        If (String.IsNullOrEmpty(sd.Selected_Date) Or String.IsNullOrEmpty(ed.Selected_Date)) Or String.IsNullOrEmpty(tbKCP.Text.Trim()) Or (cbIsContractID.Checked = False And cbIsKCP.Checked = False) Then Return

        Dim contractID = 0, oCont = New clsContract()

        If cbIsContractID.Checked Then
            If Integer.TryParse(tbKCP.Text.Trim(), contractID) = False Then
                msg.Text = String.Format("The ID of contract {0} is not in correct format!", tbKCP.Text.Trim())
                Return
            End If            
        ElseIf cbIsKCP.Checked Then
            contractID = oCont.Get_Contract_ID(tbKCP.Text.Trim())
        End If

        report.FileName = Server.MapPath(reportPath)
        report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        report.SetParameterValue("sd", sd.Selected_Date.Trim())
        report.SetParameterValue("ed", ed.Selected_Date.Trim())
        report.SetParameterValue("contractID", contractID)
        report.Load(Server.MapPath(reportPath))

        If report.HasRecords Then
            Session("Crystal") = report
            CrystalViewer.ReportSource = Session("Crystal")
            CrystalViewer.Visible = True
        Else
            CrystalViewer.Visible = False
            msg.Text = "No data"
        End If

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load      
        If IsPostBack Then
            If Not Session("Crystal") Is Nothing Then CrystalViewer.ReportSource = Session("Crystal")
        Else
            cbIsContractID.Checked = False
            cbIsKCP.Checked = False
        End If
        msg.Text = ""
    End Sub
End Class
