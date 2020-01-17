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
Partial Class Reports_Accounting_ContractStatus
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/contractstatusreport.rpt"

    Private Sub BindData()
        Dim oCombo As New clsComboItems
        ListBox1.DataSource = oCombo.Load_ComboItems("ContractStatus")
        ListBox1.DataValueField = "ComboItemID"
        ListBox1.DataTextField = "ComboItem"
        ListBox1.DataBind()
        lbSubStatus.DataSource = oCombo.Load_ComboItems("ContractSubStatus")
        lbSubStatus.DataValueField = "ComboItemID"
        lbSubStatus.DataTextField = "ComboItem"
        lbSubStatus.DataBind()
        lbmfStatus.DataSource = oCombo.Load_ComboItems("MaintenanceFeeStatus")
        lbmfStatus.DataValueField = "ComboItemid"
        lbmfStatus.DataTextField = "ComboItem"
        lbmfStatus.DataBind()
        lbmortStatus.DataSource = oCombo.Load_ComboItems("MortgageStatus")
        lbmortStatus.DataValueField = "ComboItemID"
        lbmortStatus.DataTextField = "ComboItem"
        lbmortStatus.DataBind()
        lbConvStatus.DataSource = oCombo.Load_ComboItems("ConversionStatus")
        lbConvStatus.DataValueField = "ComboItemID"
        lbConvStatus.DataTextField = "ComboItem"
        lbConvStatus.DataBind()
        oCombo = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            If Session("Report") IsNot Nothing Then CrystalReportViewer1.ReportSource = Session("Report")
        Else
            If Session("Report") IsNot Nothing Then
                Session("Report") = Nothing
            End If
            BindData()
            'Setup_Report()
            btnRun_Click()
        End If

        If hfShowReport.Value = 1 Then CrystalReportViewer1.ReportSource = Session("Report")


    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            'Report.Clone()
            'Report.Dispose()
        Catch ex As Exception

        End Try


    End Sub



    Private Sub Setup_Report()
        If Session("Report") Is Nothing Then
            If lbConvStatus2.Items.Count < 1 Then
                sReport = "reportfiles/contractstatusreport.rpt"
            Else
                sReport = "reportfiles/contractstatusreportwConv.rpt"
            End If
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            'Dim categoryID As Integer = Convert.ToInt32(ddlCategory.SelectedValue)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
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
        Dim sItems As String = ""
        sItems = "'0'"
        If ListBox2.Items.Count = 0 Then
            For i = 0 To ListBox1.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & ListBox1.Items(i).Value & "'", ",'" & ListBox1.Items(i).Value & "'")
            Next
        Else
            For i = 0 To ListBox2.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & ListBox2.Items(i).Value & "'", ",'" & ListBox2.Items(i).Value & "'")
            Next
        End If
        Report.SetParameterValue("Statuses", sItems)

        sItems = ""
        If lbSubStatus2.Items.Count = 0 Then
            sItems = "'0'"
            For i = 0 To lbSubStatus.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & lbSubStatus.Items(i).Value & "'", ",'" & lbSubStatus.Items(i).Value & "'")
            Next
        Else
            If lbSubStatus.Items.Count = 0 Then
                sItems = "'0'"
            End If
            For i = 0 To lbSubStatus2.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & lbSubStatus2.Items(i).Value & "'", ",'" & lbSubStatus2.Items(i).Value & "'")
            Next
        End If
        Report.SetParameterValue("SubStatuses", sItems)

        sItems = ""
        If lbmfStatus2.Items.Count = 0 Then
            sItems = "'0'"
            For i = 0 To lbmfStatus.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & lbmfStatus.Items(i).Value & "'", ",'" & lbmfStatus.Items(i).Value & "'")
            Next
        Else
            If lbmfStatus.Items.Count = 0 Then
                sItems = "'0'"
            End If
            For i = 0 To lbmfStatus2.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & lbmfStatus2.Items(i).Value & "'", ",'" & lbmfStatus2.Items(i).Value & "'")
            Next
        End If
        Report.SetParameterValue("MFStatuses", sItems)

        sItems = ""
        If lbmortStatus2.Items.Count = 0 Then
            sItems = "'0'"
            For i = 0 To lbmortStatus.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & lbmortStatus.Items(i).Value & "'", ",'" & lbmortStatus.Items(i).Value & "'")
            Next
        Else
            If lbmortStatus.Items.Count = 0 Then
                sItems = "'0'"
            End If

            For i = 0 To lbmortStatus2.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & lbmortStatus2.Items(i).Value & "'", ",'" & lbmortStatus2.Items(i).Value & "'")
            Next
        End If
        Report.SetParameterValue("MortStatuses", sItems)

        sItems = ""
        If lbConvStatus2.Items.Count = 0 Then
            sItems = "'0'"
            'For i = 0 To lbConvStatus.Items.Count - 1
            '    sItems &= IIf(sItems = "", "'" & lbConvStatus.Items(i).Value & "'", ",'" & lbConvStatus.Items(i).Value & "'")
            'Next
        Else
            If lbConvStatus.Items.Count = 0 Then
                sItems = "'0'"
            End If

            For i = 0 To lbConvStatus2.Items.Count - 1
                sItems &= IIf(sItems = "", "'" & lbConvStatus2.Items(i).Value & "'", ",'" & lbConvStatus2.Items(i).Value & "'")
            Next
            Report.SetParameterValue("ConversionStatus", sItems)
        End If


    End Sub

    Protected Sub btnRun_Click()
        If ListBox2.Items.Count > 0 Then
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If

    End Sub

    Protected Sub btnRun_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        If ListBox2.Items.Count > 0 Or lbmfStatus2.Items.Count > 0 Or lbmortStatus2.Items.Count > 0 Or lbSubStatus2.Items.Count > 0 Then
            Setup_Report()
            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Select as least 1 Status.');", True)
        End If
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        '>> Status
        If ListBox1.SelectedIndex >= 0 Then
            ListBox2.Items.Add(ListBox1.SelectedItem)
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            ListBox2.SelectedIndex = 0
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        '<< Status
        If ListBox2.SelectedIndex >= 0 Then
            ListBox1.Items.Add(ListBox2.SelectedItem)
            ListBox2.Items.Remove(ListBox2.SelectedItem)
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub BtnAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAll.Click
        ' ALL >> Status
        For i = ListBox1.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = ListBox1.Items(i)
            ListBox2.ClearSelection()
            ListBox2.Items.Add(lItem)
            ListBox1.Items.Remove(lItem)
        Next
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        'ALL << Status
        For i = ListBox2.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = ListBox2.Items(i)
            ListBox1.ClearSelection()
            ListBox1.Items.Add(lItem)
            ListBox2.Items.Remove(lItem)
        Next
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        '<< Sub Status
        If lbSubStatus2.SelectedIndex >= 0 Then
            lbSubStatus.Items.Add(lbSubStatus2.SelectedItem)
            lbSubStatus2.Items.Remove(lbSubStatus2.SelectedItem)
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button4_Click(sender As Object, e As System.EventArgs) Handles Button4.Click
        '>> Sub Status
        If lbSubStatus.SelectedIndex >= 0 Then
            lbSubStatus2.Items.Add(lbSubStatus.SelectedItem)
            lbSubStatus.Items.Remove(lbSubStatus.SelectedItem)
            lbSubStatus2.SelectedIndex = 0
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button5_Click(sender As Object, e As System.EventArgs) Handles Button5.Click
        ' ALL >> Sub Status
        For i = lbSubStatus.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = lbSubStatus.Items(i)
            lbSubStatus2.ClearSelection()
            lbSubStatus2.Items.Add(lItem)
            lbSubStatus.Items.Remove(lItem)
        Next
    End Sub

    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click
        ' ALL << Sub Status
        For i = lbSubStatus2.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = lbSubStatus2.Items(i)
            lbSubStatus.ClearSelection()
            lbSubStatus.Items.Add(lItem)
            lbSubStatus.Items.Remove(lItem)
        Next
    End Sub

    Protected Sub Button7_Click(sender As Object, e As System.EventArgs) Handles Button7.Click
        '<< MF Status
        If lbmfStatus2.SelectedIndex >= 0 Then
            lbmfStatus.Items.Add(lbmfStatus2.SelectedItem)
            lbmfStatus2.Items.Remove(lbmfStatus2.SelectedItem)
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button8_Click(sender As Object, e As System.EventArgs) Handles Button8.Click
        '>> MF Status
        If lbmfStatus.SelectedIndex >= 0 Then
            lbmfStatus2.Items.Add(lbmfStatus.SelectedItem)
            lbmfStatus.Items.Remove(lbmfStatus.SelectedItem)
            lbmfStatus2.SelectedIndex = 0
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button9_Click(sender As Object, e As System.EventArgs) Handles Button9.Click
        ' ALL >> MF Status
        For i = lbmfStatus.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = lbmfStatus.Items(i)
            lbmfStatus2.ClearSelection()
            lbmfStatus2.Items.Add(lItem)
            lbmfStatus.Items.Remove(lItem)
        Next
    End Sub

    Protected Sub Button10_Click(sender As Object, e As System.EventArgs) Handles Button10.Click
        ' ALL << MF Status
        For i = lbmfStatus2.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = lbmfStatus2.Items(i)
            lbmfStatus.ClearSelection()
            lbmfStatus.Items.Add(lItem)
            lbmfStatus2.Items.Remove(lItem)
        Next
    End Sub

    Protected Sub Button11_Click(sender As Object, e As System.EventArgs) Handles Button11.Click
        '<< Mortgage Status
        If lbmortStatus2.SelectedIndex >= 0 Then
            lbmortStatus.Items.Add(lbmortStatus2.SelectedItem)
            lbmortStatus2.Items.Remove(lbmortStatus2.SelectedItem)
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button12_Click(sender As Object, e As System.EventArgs) Handles Button12.Click
        '>> Mortgage Status
        If lbmortStatus.SelectedIndex >= 0 Then
            lbmortStatus2.Items.Add(lbmortStatus.SelectedItem)
            lbmortStatus.Items.Remove(lbmortStatus.SelectedItem)
            lbmortStatus2.SelectedIndex = 0
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button13_Click(sender As Object, e As System.EventArgs) Handles Button13.Click
        ' ALL >> Mortgage Status
        For i = lbmortStatus.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = lbmortStatus.Items(i)
            lbmortStatus2.ClearSelection()
            lbmortStatus2.Items.Add(lItem)
            lbmortStatus.Items.Remove(lItem)
        Next
    End Sub

    Protected Sub Button14_Click(sender As Object, e As System.EventArgs) Handles Button14.Click
        'ALL << Mortgage Status
        For i = lbmortStatus2.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = lbmortStatus2.Items(i)
            lbmortStatus.ClearSelection()
            lbmortStatus.Items.Add(lItem)
            lbmortStatus2.Items.Remove(lItem)
        Next
    End Sub

    Protected Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        '<< Conversion Status
        If lbConvStatus2.SelectedIndex >= 0 Then
            lbConvStatus.Items.Add(lbConvStatus2.SelectedItem)
            lbConvStatus2.Items.Remove(lbConvStatus2.SelectedItem)
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        '>> Conversion Status
        If lbConvStatus.SelectedIndex >= 0 Then
            lbConvStatus2.Items.Add(lbConvStatus.SelectedItem)
            lbConvStatus.Items.Remove(lbConvStatus.SelectedItem)
            lbConvStatus2.SelectedIndex = 0
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        ' ALL >> Conversion Status
        For i = lbConvStatus.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = lbConvStatus.Items(i)
            lbConvStatus2.ClearSelection()
            lbConvStatus2.Items.Add(lItem)
            lbConvStatus.Items.Remove(lItem)
        Next
    End Sub

    Protected Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        'ALL << Conversion Status
        For i = lbConvStatus2.Items.Count - 1 To 0 Step -1
            Dim lItem As ListItem = lbConvStatus2.Items(i)
            lbConvStatus.ClearSelection()
            lbConvStatus.Items.Add(lItem)
            lbConvStatus2.Items.Remove(lItem)
        Next
    End Sub
End Class
