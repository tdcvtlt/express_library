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
Partial Class Reports_Accounting_CancellationReport
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/CancellationReport.rpt"

    Private Sub BindData()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then

        Else
            Session("Report") = Nothing
            BindData()
            Setup_Report()
        End If

        If hfShowReport.Value = 1 Then CrystalReportViewer1.ReportSource = Session("Report")


    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            'Report.Close()
            'Report.Dispose()
        Catch ex As Exception

        End Try



    End Sub



    Private Sub Setup_Report()
        If Session("Report") Is Nothing Then
            Dim cn As New SqlConnection(Resources.Resource.cns)
            cn.Open()
            Dim sSQL As String = "Select c.ContractID, c.ContractNumber as KCP, c.ContractDate, c.StatusDate as CancellationDate, p.FirstName, p.LastName, m.SalesPrice, 0 as MortgageBalance, css.ComboItem as SubStatus " & _
                                    "from t_Contract c inner join t_ComboItems cs on c.StatusID = cs.ComboItemID " & _
                                    "inner join t_Prospect p on c.ProspectID = p.ProspectID " & _
                                    "inner join t_Mortgage m on c.ContractID = m.ContractID " & _
                                    "left outer join t_ComboItems css on c.SubStatusID = css.ComboitemID " & _
                                    "where cs.ComboItem = 'Canceled' and c.StatusDate between '" & dfTransDate.Selected_Date & "' and '" & dfEndDate.Selected_Date & "'"
            Dim cm As New SqlCommand(sSQL, cn)
            Dim dr As SqlDataReader = cm.ExecuteReader
            Dim ds As New CancellationReport
            ds.Tables("CXLS").Load(dr)
            Get_Equiant_Information(ds.Tables("CXLS"))



            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            Report.SetDataSource(ds)
            'Dim categoryID As Integer = Convert.ToInt32(ddlCategory.SelectedValue)
            '            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            '            Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Set_Params()
            Session.Add("Report", Report)
            cn.Close()
        Else
            Report = Session("Report")
            If Report.FileName <> Server.MapPath(sReport) Then
                Session("Report") = Nothing
                Setup_Report()
            End If
        End If
    End Sub

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRunReport.Click
        If dfTransDate.Selected_Date <> "" Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()

        'Report.SetParameterValue("SDate", CDate(IIf(dfTransDate.Selected_Date <> "", dfTransDate.Selected_Date, Date.Today)))
        'Report.SetParameterValue("EDate", CDate(IIf(dfenddate.Selected_Date <> "", dfenddate.Selected_Date, Date.Today)))

    End Sub

    Private Sub Get_Equiant_Information(ByRef table As Data.DataTable)
        Dim oMort As clsMortgage
        Dim oEQ As New clsEquiant
        For Each row In table.Rows
            If row(0) & "" <> "" Then
                oMort = New clsMortgage
                oMort.ContractID = row("ContractID")
                oMort.Load()
                If Not oMort.Number.Contains(row("ContractID")) Then
                    oMort.Number = oEQ.Get_Account(CInt(row("ContractID")), False)
                    oMort.UserID = Session("UserDBID")
                    oMort.Save()
                End If
                Dim li = oEQ.LoanInformation(oMort.Number)
                If Not (IsNothing(li)) Then
                    row("MortgageBalance") = li.PrincipalBalance & ""
                End If
                oMort = Nothing
                GC.Collect()
            End If
        Next
        oEQ = Nothing

    End Sub
End Class