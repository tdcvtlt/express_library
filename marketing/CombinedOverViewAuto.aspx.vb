
Partial Class marketing_CombinedOverViewAuto
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If CheckSecurity("Overview", "View", , , Session("UserDBID")) Then
                Dim oOV As New clsCombinedOverviewAuto
                oOV.CombinedOverViewID = Request("OverViewID")
                oOV.Load()
                lblDate.Text = oOV.OverviewDate & " - " & oOV.OverviewLocation

                '******LINE
                txtLineToursAct.Text = oOV.LineToursAct
                txtLineMTDToursAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineToursAct")
                txtLineYTDToursAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineToursAct")
                txtLineToursProj.TExt = oOV.LineToursProj
                txtLineMTDToursProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineToursProj")
                txtLineYTDToursProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineToursProj")
                txtLineSalesAct.Text = oOV.LineSalesAct
                txtLineMTDSalesAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineSalesAct")
                txtLineYTDSalesAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineSalesAct")
                txtLineSalesProj.Text = FormatNumber(oOV.LineSalesProj, 2)
                txtLineMTDSalesProj.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineSalesProj"), 2)
                txtLineYTDSalesProj.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineSalesProj"), 2)
                txtLineIntervalsSales.Text = FormatNumber(oOV.LineIntervalSales, 2)
                txtLineMTDIntervalsSales.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalSales"), 2)
                txtLineYTDIntervalsSales.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalSales"), 2)
                txtLineIntervalsCan.Text = FormatNumber(oOV.LineIntervalCan, 2)
                txtLineMTDIntervalsCan.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalCan"), 2)
                txtLineYTDIntervalsCan.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalCan"), 2)
                txtLineVolumeAct.Text = FormatNumber(oOV.LineVolumeAct, 0, , , TriState.False)
                txtLineMTDVolumeAct.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineVolumeAct"), 0, , , TriState.False)
                txtLineYTDVolumeAct.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineVolumeAct"), 0, , , TriState.False)
                txtLineVolumeProj.Text = FormatNumber(oOV.LineVolumeProj, 0, , , TriState.False)
                txtLineMTDVolumeProj.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineVolumeProj"), 0, , , TriState.False)
                txtLineYTDVolumeProj.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineVolumeProj"), 0, , , TriState.False)
                If txtLineToursAct.Text = 0 Then
                    txtLineClosingAct.Text = 0
                Else
                    txtLineClosingAct.Text = FormatNumber((CDbl(txtLineSalesAct.Text) / CDbl(txtLineToursAct.Text)) * 100, 2)
                End If
                If txtLineMTDToursAct.Text = 0 Then
                    txtLineMTDToursAct.Text = 0
                Else
                    txtLineMTDClosingAct.Text = FormatNumber((CDbl(txtLineMTDSalesAct.Text) / CDbl(txtLineMTDToursAct.Text)) * 100, 2)
                End If
                If txtLineYTDToursAct.Text = 0 Then
                    txtLineYTDToursAct.Text = 0
                Else
                    txtLineYTDClosingAct.Text = FormatNumber((CDbl(txtLineYTDSalesAct.Text) / CDbl(txtLineYTDToursAct.Text)) * 100, 2)
                End If

                txtLineClosingProj.Text = FormatNumber(oOV.LineClosingProj, 2)
                If txtLineMTDToursProj.Text = 0 Then
                    txtLineMTDClosingProj.Text = 0
                Else
                    txtLineMTDClosingProj.Text = FormatNumber((CDbl(txtLineMTDSalesProj.Text) / CDbl(txtLineMTDToursProj.Text)) * 100, 2)
                End If
                If txtLineYTDToursProj.Text = 0 Then
                    txtLineYTDClosingProj.Text = 0
                Else
                    txtLineYTDClosingProj.Text = FormatNumber((CDbl(txtLineYTDSalesProj.Text) / CDbl(txtLineYTDToursProj.Text)) * 100, 2)
                End If
                If oOV.LineSalesAct = 0 Then
                    txtLineAvgSP.Text = 0
                Else
                    txtLineAvgSp.Text = FormatNumber((oOV.LineVolumeAct / oOV.LineSalesAct), 2, , , TriState.False)
                End If
                If txtLineMTDVolumeAct.Text = 0 Then
                    txtLineMTDAvgSP.Text = 0
                Else
                    txtLineMTDAvgSP.Text = FormatNumber((txtLineMTDVolumeAct.Text / txtLineMTDSalesAct.Text), 2, , , TriState.False)
                End If
                If txtLineYTDVolumeAct.Text = 0 Then
                    txtLineYTDAvgSP.Text = 0
                Else
                    txtLineYTDAvgSP.Text = FormatNumber((txtLineYTDVolumeAct.Text / txtLineYTDSalesAct.Text), 2, , , TriState.False)
                End If
                If oOV.LineToursAct = 0 Then
                    txtLineVPG.Text = 0
                Else
                    txtLineVPG.Text = FormatNumber((oOV.LineVolumeAct / oOV.LineToursAct), 2, , , TriState.False)
                End If
                If txtLineMTDToursAct.Text = 0 Then
                    txtLineMTDVPG.Text = 0
                Else
                    txtLineMTDVPG.Text = FormatNumber((txtLineMTDVolumeAct.Text / txtLineMTDToursAct.Text), 2, , , TriState.False)
                End If
                If txtLineYTDToursAct.Text = 0 Then
                    txtLineYTDVPG.Text = 0
                Else
                    txtLineYTDVPG.Text = FormatNumber((txtLineYTDVolumeAct.Text / txtLineYTDToursAct.Text), 2, , , TriState.False)
                End If

                '*****IN HOUSE
                txtIHToursAct.Text = oOV.IHToursAct
                txtIHMTDToursAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHToursAct")
                txtIHYTDToursAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHToursAct")
                txtIHToursProj.Text = oOV.IHToursProj
                txtIHMTDToursProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHToursProj")
                txtIHYTDToursProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHToursProj")
                txtIHSalesAct.Text = oOV.IHSalesAct
                txtIHMTDSalesAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHSalesAct")
                txtIHYTDSalesAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHSalesAct")
                txtIHSalesProj.Text = FormatNumber(oOV.IHSalesProj, 2)
                txtIHMTDSalesProj.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHSalesProj"), 2)
                txtIHYTDSalesProj.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHSalesProj"), 2)
                txtIHIntervalsSales.Text = FormatNumber(oOV.IHIntervalsSales, 2)
                txtIHMTDIntervalsSales.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsSales"), 2)
                txtIHYTDIntervalsSales.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsSales"), 2)
                txtIHIntervalsCan.Text = FormatNumber(oOV.IHIntervalsCan, 2)
                txtIHMTDIntervalsCan.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsCan"), 2)
                txtIHYTDIntervalsCan.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsCan"), 2)
                txtIHVolumeAct.Text = FormatNumber(oOV.IHVolumeAct, 0, , , TriState.False)
                txtIHMTDVolumeAct.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeAct"), 0, , , TriState.False)
                txtIHYTDVolumeAct.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeAct"), 0, , , TriState.False)
                txtIHVolumeProj.Text = FormatNumber(oOV.IHVolumeProj, 0, , , TriState.False)
                txtIHMTDVolumeProj.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeProj"), 0, , , TriState.False)
                txtIHYTDVolumeProj.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeProj"), 0, , , TriState.False)
                txtIHUpgradesAct.Text = oOV.IHUpgradesAct
                txtIHMTDUPgradesAct.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesAct"), 0, , , TriState.False)
                txtIHYTDUPgradesAct.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesAct"), 0, , , TriState.False)
                txtIHUpgradesAct.Text = oOV.IHUpgradesProj
                txtIHMTDUPgradesProj.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesProj"), 0, , , TriState.False)
                txtIHYTDUPgradesProj.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesProj"), 0, , , TriState.False)
                txtIHUpgradeVolAct.Text = FormatNumber(oOV.IHUpgradeVolumeAct, 0, , , TriState.False)
                txtIHMTDUpgradeVolAct.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradeVolumeAct"), 0, , , TriState.False)
                txtIHYTDUpgradeVolAct.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradeVolumeAct"), 0, , , TriState.False)
                txtIHUpgradeVolProj.Text = FormatNumber(oOV.IHUpgradeVolumeProj, 0, , , TriState.False)
                txtIHMTDUpgradeVolProj.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradeVolumeProj"), 0, , , TriState.False)
                txtIHYTDUpgradeVolProj.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradeVolumeProj"), 0, , , TriState.False)
                If oOV.IHSalesAct = 0 Then
                    txtIHAvgSP.Text = 0
                Else
                    txtIHAvgSp.Text = FormatNumber((oOV.IHVolumeAct / oOV.IHSalesAct), 2, , , TriState.False)
                End If
                If txtIHMTDVolumeAct.Text = 0 Then
                    txtIHMTDAvgSP.Text = 0
                Else
                    txtIHMTDAvgSP.Text = FormatNumber((txtIHMTDVolumeAct.Text / txtIHMTDSalesAct.Text), 2, , , TriState.False)
                End If
                If txtIHYTDVolumeAct.Text = 0 Then
                    txtIHYTDAvgSP.Text = 0
                Else
                    txtIHYTDAvgSP.Text = FormatNumber((txtIHYTDVolumeAct.Text / txtIHYTDSalesAct.Text), 2, , , TriState.False)
                End If

                If oOV.IHUpgradesAct = 0 Then
                    txtIHAvgVolume.Text = 0
                Else
                    txtIHAvgVolume.Text = FormatNumber((oOV.IHUpgradeVolumeAct / oOV.IHUpgradesAct), 2, , , TriState.False)
                End If
                If txtIHMTDUpgradesAct.Text = 0 Then
                    txtIHMTDAvgVolume.Text = 0
                Else
                    txtIHMTDAvgVolume.Text = FormatNumber((txtIHMTDUpgradeVolAct.Text / txtIHMTDUpgradesAct.Text), 2, , , TriState.False)
                End If
                If txtIHYTDUpgradesAct.Text = 0 Then
                    txtIHYTDAvgVolume.Text = 0
                Else
                    txtIHYTDAvgVolume.Text = FormatNumber((txtIHYTDUpgradeVolAct.Text / txtIHYTDUpgradesAct.Text), 2, , , TriState.False)
                End If
                txtIHTotalVolume.Text = FormatNumber(CDbl(txtIHVolumeAct.Text) + CDbl(txtIHUpgradeVolAct.Text), 0, , , TriState.False)
                txtIHMTDTotalVolume.Text = FormatNumber(CDbl(txtIHMTDVolumeAct.Text) + CDbl(txtIHMTDUpgradeVolAct.Text), 0, , , TriState.False)
                txtIHYTDTotalVolume.Text = FormatNumber(CDbl(txtIHYTDVolumeAct.Text) + CDbl(txtIHYTDUpgradeVolAct.Text), 0, , , TriState.False)

                '*****TOTALS
                txtToursAct.Text = CDbl(txtLineToursAct.Text) + CDbl(txtIHToursAct.Text)
                txtMTDToursAct.Text = CDbl(txtLineMTDToursAct.Text) + CDbl(txtIHMTDToursAct.Text)
                txtYTDToursAct.Text = CDbl(txtLineYTDToursAct.Text) + CDbl(txtIHYTDToursAct.Text)
                txtToursProj.Text = CDbl(txtLIneToursProj.Text) + CDbl(txtIHToursProj.Text)
                txtMTDToursProj.Text = CDbl(txtLineMTDToursProj.Text) + CDbl(txtIHMTDToursProj.Text)
                txtYTDToursProj.Text = CDbl(txtLineYTDToursProj.Text) + CDbl(txtIHYTDToursProj.Text)
                txtSalesAct.Text = CDbl(txtLineSalesAct.Text) + CDbl(txtIHSalesAct.Text)
                txtMTDSalesAct.Text = CDbl(txtLineMTDSalesAct.Text) + CDbl(txtIHMTDSalesAct.Text)
                txtYTDSalesAct.Text = CDbl(txtLineYTDSalesAct.Text) + CDbl(txtIHYTDSalesAct.Text)
                txtSalesProj.Text = CDbl(txtLineSalesProj.Text) + CDbl(txtIHSalesProj.Text)
                txtMTDSalesProj.Text = CDbl(txtLineMTDSalesProj.Text) + CDbl(txtIHMTDSalesProj.Text)
                txtYTDSalesProj.Text = CDbl(txtLineYTDSalesProj.Text) + CDbl(txtIHYTDSalesProj.Text)
                txtIntervalsSales.Text = CDbl(txtLineIntervalsSales.Text) + CDbl(txtIHIntervalsSales.Text)
                txtMTDIntervalsSales.Text = CDbl(txtLineMTDIntervalsSales.Text) + CDbl(txtIHMTDIntervalsSales.Text)
                txtYTDIntervalsSales.TExt = CDbl(txtLineYTDIntervalsSales.Text) + CDbl(txtIHYTDIntervalsSales.Text)
                txtIntervalsCan.Text = CDbl(txtLineIntervalsCan.Text) + CDbl(txtIHIntervalsCan.Text)
                txtMTDIntervalsCan.Text = CDbl(txtLineMTDIntervalsCan.Text) + CDbl(txtIHMTDIntervalsCan.Text)
                txtYTDIntervalsCan.Text = CDbl(txtLineYTDIntervalsCan.Text) + CDbl(txtIHYTDIntervalsCan.Text)
                txtVolumeAct.Text = CDbl(txtLineVolumeAct.Text) + CDbl(txtIHVolumeAct.Text) + CDbl(txtIHUpgradeVolAct.Text)
                txtMTDVOlumeAct.Text = CDbl(txtLIneMTDVOlumeAct.Text) + CDbl(txtIHMTDVolumeAct.Text) + CDbl(txtIHMTDUpgradeVolAct.Text)
                txtYTDVOlumeAct.Text = CDbl(txtLIneYTDVolumeAct.Text) + CDbl(txtIHYTDVOlumeAct.Text) + CDbl(txtIHYTDUpgradeVolAct.Text)
                txtVolumeProj.Text = CDbl(txtLineVolumeProj.Text) + CDbl(txtIHVolumeProj.Text) + CDbl(txtIHUpgradeVolProj.Text)
                txtMTDVOlumeProj.Text = CDbl(txtLIneMTDVOlumeProj.Text) + CDbl(txtIHMTDVolumeProj.Text) + CDbl(txtIHMTDUpgradeVolProj.Text)
                txtYTDVOlumeProj.Text = CDbl(txtLIneYTDVolumeProj.Text) + CDbl(txtIHYTDVOlumeProj.Text) + CDbl(txtIHYTDUpgradeVolProj.Text)
                If txtSalesAct.Text = 0 Then
                    txtAvgSalesPrice.Text = 0
                Else
                    txtAvgSalesPrice.Text = FormatNumber(CDbl(txtVolumeAct.Text) / CDbl(txtSalesAct.Text), 2, , , TriState.False)
                End If
                If txtMTDSalesAct.Text = 0 Then
                    txtMTDAvgSalesPrice.Text = 0
                Else
                    txtMTDAvgSalesPrice.Text = FormatNumber(CDbl(txtMTDVolumeAct.Text) / CDbl(txtMTDSalesAct.Text), 2, , , TriState.False)
                End If
                If txtYTDSalesAct.Text = 0 Then
                    txtYTDAvgSalesPrice.Text = 0
                Else
                    txtYTDAvgSalesPrice.Text = FormatNumber(CDbl(txtYTDVolumeAct.Text) / CDbl(txtYTDSalesAct.Text), 2, , , TriState.False)
                End If
                oOV = Nothing

                Dim oOVBackup As New clsCombinedOverviewAutoBackup
                gvLineTours.DataSource = oOVBackup.List_Tour_Backup(Request("OverViewID"), "LT")
                gvLineTours.DataBind()
                gvLIneSales.DataSource = oOVBackup.List_Contract_Backup(Request("OverViewID"), "ls")
                gvLineSales.DataBind()
                gvLineRecisions.DataSource = oOVBackup.List_Contract_Backup(Request("OverViewID"), "lr")
                gvLineRecisions.DataBind()
                gvIHTours.DataSource = oOVBackup.List_Tour_Backup(Request("OverViewID"), "IHT")
                gvIHTours.DataBind()
                gvIHSales.DataSOurce = oOVBackup.List_Contract_Backup(Request("OverViewID"), "ihs")
                gvIHSales.DataBind()
                gvIHRecisions.DataSource = oOVBackup.List_Contract_Backup(Request("OverViewID"), "ihsr")
                gvIHRecisions.DataBind()
                gvIHUpgrades.DataSource = oOVBackup.List_Contract_Backup(Request("OverViewID"), "ihu")
                gvIHUpgrades.DataBInd()
                gvIHUpgradeRecisions.DataSource = oOVBackup.List_Contract_Backup(Request("OverViewID"), "ihur")
                gvIHUpgradeRecisions.DataBind()
                oOVBackup = Nothing
                MultiView1.ActiveViewIndex = 0
            Else
                MultiView1.ActiveViewIndex = 2
            End If
        End If
    End Sub

    Protected Sub OverView_Link_Click(sender As Object, e As System.EventArgs) Handles OverView_Link.Click
        If CheckSecurity("Overview", "View", , , Session("UserDBID")) Then
            MultiView1.ActiveViewIndex = 0
        Else
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub Backup_Link_Click(sender As Object, e As System.EventArgs) Handles Backup_Link.Click
        If CheckSecurity("Overview", "View", , , Session("UserDBID")) Then
            MultiView1.ActiveViewIndex = 1
        Else
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub
End Class
