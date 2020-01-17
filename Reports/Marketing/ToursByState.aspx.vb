Imports System.Data.SqlClient
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine

Partial Class Reports_Marketing_ToursByState
    Inherits System.Web.UI.Page

    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/ToursByState.rpt"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Fill_DropDowns()
        Else
            Try
                CrystalReportViewer1.ReportSource = DirectCast(Session("CrystalReport"), ReportDocument)
                CrystalReportViewer1.DataBind()

            Catch ex As Exception

            End Try

        End If


    End Sub

    Private Sub Fill_DropDowns()

        Dim ds As New SqlDataSource(Resources.Resource.cns, "")
        ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname = 'TourStatus' and i.comboitem like 'NO %'"
        ddNS.DataSource = ds
        ddNS.DataValueField = "ComboItemID"
        ddNS.DataTextField = "Comboitem"
        ddNS.DataBind()
        ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname = 'TourStatus' and i.comboitem like 'NQ %'"
        ddNQ.DataSource = ds
        ddNQ.DataValueField = "ComboItemID"
        ddNQ.DataTextField = "Comboitem"
        ddNQ.DataBind()
        ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname = 'TourStatus' and i.comboitem in ('Showed', 'OnTour','Booked')"
        ddQS.DataSource = ds
        ddQS.DataValueField = "ComboItemID"
        ddQS.DataTextField = "Comboitem"
        ddQS.DataBind()
        ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname = 'TourStatus' and i.comboitem like 'Can%'"
        ddCS.DataSource = ds
        ddCS.DataValueField = "ComboItemID"
        ddCS.DataTextField = "Comboitem"
        ddCS.DataBind()

        ds = Nothing

    End Sub

    Protected Sub btnAddNS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNS.Click
        If ddNS.Items.Count > 0 Then
            Add_Item(ddNS, lbNS)
        End If
    End Sub

    Protected Sub btnAddNQ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNQ.Click
        If ddNQ.Items.Count > 0 Then
            Add_Item(ddNQ, lbNQ)
        End If
    End Sub

    Protected Sub btnAddQS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddQS.Click
        If ddQS.Items.Count > 0 Then
            Add_Item(ddQS, lbQS)
        End If
    End Sub

    Protected Sub btnAddCS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCS.Click
        If ddCS.Items.Count > 0 Then
            Add_Item(ddCS, lbCS)
        End If
    End Sub

    Protected Sub btnRemoveNS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveNS.Click
        If ddNS.Items.Count > 0 Then
            Remove_Item(ddNS, lbNS)
        End If
    End Sub

    Protected Sub btnRemoveNQ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveNQ.Click
        If ddNQ.Items.Count > 0 Then
            Remove_Item(ddNQ, lbNQ)
        End If
    End Sub

    Protected Sub btnRemoveQS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveQS.Click
        If ddQS.Items.Count > 0 Then
            Remove_Item(ddQS, lbQS)
        End If
    End Sub

    Protected Sub btnRemoveCS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveCS.Click
        If ddCS.Items.Count > 0 Then
            Remove_Item(ddCS, lbCS)
        End If
    End Sub



    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        If (dfStartDate.Selected_Date <> String.Empty And _
            dfEndDate.Selected_Date <> String.Empty) And _
            lbNS.Items.Count > 0 And _
            lbNQ.Items.Count > 0 And _
            lbQS.Items.Count > 0 And _
            lbCS.Items.Count > 0 Then


            If Session("Report") Is Nothing Then
                PrepareReport()
            End If

            hfShowReport.Value = 1


        End If

    End Sub

    Private Function ListBoxSelectedValues(ByRef dd As ListBox) As String

        If dd.Items.Count = 0 Then Return String.Empty


        Dim sValues As String = ""
        For Each row As ListItem In dd.Items
            sValues &= IIf(sValues = "", "'" & row.Value, "," & row.Value)
        Next

        Return sValues & "'"
    End Function


    Private Sub PrepareReport()


        Report.Load(Server.MapPath(sReport))
        Report.FileName = Server.MapPath(sReport)

        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)


        Report.SetParameterValue("startDate", Date.Parse(dfStartDate.Selected_Date))
        Report.SetParameterValue("endDate", Date.Parse(dfEndDate.Selected_Date))
        Report.SetParameterValue("statusCancelled", ListBoxSelectedValues(lbCS))
        Report.SetParameterValue("statusNoShow", ListBoxSelectedValues(lbNS))
        Report.SetParameterValue("statusNQ", ListBoxSelectedValues(lbNQ))
        Report.SetParameterValue("statusQualified", ListBoxSelectedValues(lbQS))



        Session("CrystalReport") = Report

        CrystalReportViewer1.ReportSource = Report
        CrystalReportViewer1.DataBind()

    End Sub


    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click

    End Sub

    Private Sub Add_Item(ByRef ddList As DropDownList, ByRef lbListBox As ListBox)
        lbListBox.ClearSelection()
        lbListBox.Items.Add(ddList.SelectedItem)
        ddList.Items.Remove(ddList.SelectedItem)
    End Sub

    Private Sub Remove_Item(ByRef ddList As DropDownList, ByRef lbListBox As ListBox)
        ddList.ClearSelection()
        ddList.Items.Add(lbListBox.SelectedItem)
        lbListBox.Items.Remove(lbListBox.SelectedItem)
    End Sub
End Class
