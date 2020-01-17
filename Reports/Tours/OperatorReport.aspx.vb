Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class Reports_Tours_Default2
    Inherits System.Web.UI.Page
    Protected Sub CrystalReportViewer1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles CrystalReportViewer1.Load
        Dim crtableLogoninfos As New TableLogOnInfos()
        Dim crtableLogoninfo As New TableLogOnInfo()
        Dim crConnectionInfo As New ConnectionInfo()
        Dim CrTables As Tables
        Dim CrTable As Table

        With crConnectionInfo
            .ServerName = "CRMSNet" '"DSN or Server Name"
            .DatabaseName = "CRMSNet" '"DatabaseName"
            .UserID = "asp" '"Your User ID"
            .Password = "aspnet" '"Your Password"
        End With

        CrTables = CrystalReportSource1.ReportDocument.Database.Tables
        For Each CrTable In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crConnectionInfo
            CrTable.ApplyLogOnInfo(crtableLogoninfo)

            'If your DatabaseName is changing at runtime, specify 
            'the table location. 
            'For example, when you are reporting off of a 
            'Northwind database on SQL server you 
            'should have the following line of code: 
            'CrTable.Location = crConnectionInfo.DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
        Next
    End Sub
End Class
