Imports System.Data
Imports System.Linq
Imports CrystalDecisions.CrystalReports.Engine


Partial Class Reports_Sales_ContractsByStatus
    Inherits System.Web.UI.Page

    Dim Report As ReportDocument = Nothing
    Dim sReport As String = "reportfiles/ContractsByStatus.rpt"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            FillStatusTo()
        Else

            Try
                '
                'Needed in case CR < || > button is used
                If Not DirectCast(Session("CrystalReport"), ReportDocument) Is Nothing Then
                    CrystalReportViewer1.ReportSource = DirectCast(Session("CrystalReport"), ReportDocument)
                    CrystalReportViewer1.DataBind()
                Else

                    CrystalReportViewer1.Visible = False
                End If


            Catch ex As Exception
                Response.Write(ex.Message)
            End Try

        End If

    End Sub


    Private Sub FillStatusTo()

        Dim ds As New SqlDataSource(Resources.Resource.cns, "")

        ds.SelectCommand = "select b.* from t_combos a inner join t_comboitems b on a.comboid = b.comboid And b.active = 1 where comboname = 'contractstatus'"

        ddStatuses.DataSource = ds

        ddStatuses.DataValueField = "ComboItemID"
        ddStatuses.DataTextField = "Comboitem"
        ddStatuses.DataBind()

    End Sub


    Private Function ListBoxSelectedValues(ByRef dd As ListBox) As String


        Dim arr(dd.Items.Count - 1) As String

        For i As Integer = 0 To arr.Length - 1
            arr(i) = dd.Items(i).Value
        Next

        Return String.Join(",", arr)

    End Function


    Private Sub LoadReport()

        CrystalReportViewer1.ReportSource = Nothing

        If lbStatuses.Items.Count > 0 Then

            Report = New ReportDocument()

            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)

            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

            Try
                Report.SetParameterValue("STATUSES", New String() {ListBoxSelectedValues(lbStatuses)})
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try



            Session("CrystalReport") = Report

            CrystalReportViewer1.ReportSource = Report
            CrystalReportViewer1.DataBind()

            CrystalReportViewer1.Visible = True
        Else

            CrystalReportViewer1.Visible = False
            CrystalReportViewer1.ReportSource = Nothing

            Return

        End If

        CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None


    End Sub








    Protected Sub btAddTo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btAddTo.Click

        '
        'See comments on companion button
        '
        Session("CrystalReport") = Nothing
        CrystalReportViewer1.Visible = False


        If ddStatuses.Items.Count > 0 Then

            Dim li As ListItem = ddStatuses.SelectedItem

            lbStatuses.Items.Add(li)
            lbStatuses.ClearSelection()

            ddStatuses.Items.Remove(li)
            ddStatuses.ClearSelection()

        End If

    End Sub


    Protected Sub btRemoveFrom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRemoveFrom.Click

        '
        'Hide CR viewer while users making changes to their selections
        'Only show when Run Report buttton got clicked
        '
        Session("CrystalReport") = Nothing
        CrystalReportViewer1.Visible = False

        If lbStatuses.Items.Count > 0 Then
            If lbStatuses.SelectedIndex > -1 Then

                Dim li As ListItem = lbStatuses.SelectedItem

                ddStatuses.Items.Add(li)
                lbStatuses.Items.Remove(li)

                ddStatuses.ClearSelection()

            End If
        End If
    End Sub


    Protected Sub btRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRunReport.Click

        Session("CrystalReport") = Nothing
        LoadReport()

    End Sub



End Class
