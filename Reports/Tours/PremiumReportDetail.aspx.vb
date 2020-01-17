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

Partial Class Reports_Tours_Default3
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/premiumreportdetail.rpt"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using ad = New SqlDataAdapter("Select distinct d.ComboItem, d.ComboItemID from t_Personnel p inner join t_Personnel2Dept pd on p.PersonnelID = pd.PersonnelID " & _
                                "inner join t_ComboItems d on pd.DepartmentID = d.ComboItemID where pd.Active = 1 " & _
                                "order by d.ComboItem", cn)
                    Dim dt = New DataTable()
                    ad.Fill(dt)
                    cblPersonnel.DataSource = dt
                    cblPersonnel.DataTextField = "ComboItem"
                    cblPersonnel.DataValueField = "ComboItem"
                    cblPersonnel.DataBind()
                End Using
            End Using

            siTourLocation.Connection_String = Resources.Resource.cns
            siTourLocation.ComboItem = "TourLocation"
            siTourLocation.Label_Caption = ""
            siTourLocation.Load_Items()
        Else
            If Not Session("Report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("Report")
        End If

    End Sub

    Protected Sub btnRun_Click(sender As Object, e As System.EventArgs) Handles btnRun.Click

        Try
            Session("Report") = Nothing
            CrystalReportViewer1.Dispose()

            Report.Load(Server.MapPath(sReport))

            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Report.SetParameterValue("SDate", sd.Selected_Date)
            Report.SetParameterValue("EDate", ed.Selected_Date)
            Report.SetParameterValue("tourLocationID", siTourLocation.Selected_ID)

            Report.RecordSelectionFormula = get_RecordSelectionFormula()

            Session.Add("Report", Report)
            CrystalReportViewer1.ReportSource = Session("Report")

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Function get_RecordSelectionFormula() As String
        Dim sb = New StringBuilder()
        For Each li As ListItem In cblPersonnel.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True)
            If sb.ToString().Length = 0 Then
                sb.AppendFormat("{0} = ""{1}"" ", "{Command.Personnel}", li.Text)
            Else
                sb.AppendFormat("OR {0} = ""{1}"" ", "{Command.Personnel}", li.Text)
            End If
        Next
        Return sb.ToString()
    End Function

    
End Class
