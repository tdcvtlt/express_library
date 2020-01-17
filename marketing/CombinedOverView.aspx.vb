Imports Microsoft.VisualBasic
Partial Class marketing_CombinedOverview
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If CheckSecurity("Overview", "View", , , Session("UserDBID")) Then
                Dim oOV As New clsCombinedOverview
                oOV.CombinedOverviewID = Request("OverViewID")
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
                'txtLineIntervalsSales.Text = FormatNumber(oOV.LineIntervalsSales, 2)
                'txtLineMTDIntervalsSales.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalsSales"), 2)
                'txtLineYTDIntervalsSales.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalsSales"), 2)
                'txtLineIntervalsCan.Text = FormatNumber(oOV.LineIntervalsCan, 2)
                'txtLineMTDIntervalsCan.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalsCan"), 2)
                'txtLineYTDIntervalsCan.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalsCan"), 2)
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
                'txtIHMTDIntervalsSales.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsSales"), 2)
                'txtIHYTDIntervalsSales.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsSales"), 2)
                'txtIHIntervalsCan.Text = FormatNumber(oOV.IHIntervalsCan, 2)
                'txtIHMTDIntervalsCan.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsCan"), 2)
                'txtIHYTDIntervalsCan.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsCan"), 2)
                txtIHVolumeAct.Text = FormatNumber(oOV.IHVolumeAct, 0, , , TriState.False)
                txtIHMTDVolumeAct.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeAct"), 0, , , TriState.False)
                txtIHYTDVolumeAct.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeAct"), 0, , , TriState.False)
                txtIHVolumeProj.Text = FormatNumber(oOV.IHVolumeProj, 0, , , TriState.False)
                txtIHMTDVolumeProj.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeProj"), 0, , , TriState.False)
                txtIHYTDVolumeProj.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeProj"), 0, , , TriState.False)
                txtIHUpgradesAct.Text = oOV.IHUpgradesAct
                txtIHUpgradesProj.Text = oOV.IHUpgradesProj
                txtIHMTDUPgradesAct.Text = FormatNumber(oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesAct"), 0, , , TriState.False)
                txtIHYTDUPgradesAct.Text = FormatNumber(oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesAct"), 0, , , TriState.False)
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
                If oOV.IHToursAct = 0 Then
                    txtIHVPG.Text = 0
                Else
                    txtIHVPG.Text = FormatNumber((CDbl(txtIHTotalVolume.Text) / oOV.IHToursAct), 2, , , TriState.False)
                End If
                If txtIHMTDToursAct.Text = 0 Then
                    txtIHMTDVPG.Text = 0
                Else
                    txtIHMTDVPG.Text = FormatNumber((CDbl(txtIHMTDTotalVolume.Text) / txtIHMTDToursAct.Text), 2, , , TriState.False)
                End If
                If txtIHYTDToursAct.Text = 0 Then
                    txtIHYTDVPG.Text = 0
                Else
                    txtIHYTDVPG.Text = FormatNumber((CDbl(txtIHYTDTotalVolume.Text) / txtIHYTDToursAct.Text), 2, , , TriState.False)
                End If




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
                'txtIntervalsSales.Text = CDbl(txtLineIntervalsSales.Text) + CDbl(txtIHIntervalsSales.Text)
                'txtMTDIntervalsSales.Text = CDbl(txtLineMTDIntervalsSales.Text) + CDbl(txtIHMTDIntervalsSales.Text)
                'txtYTDIntervalsSales.TExt = CDbl(txtLineYTDIntervalsSales.Text) + CDbl(txtIHYTDIntervalsSales.Text)
                'txtIntervalsCan.Text = CDbl(txtLineIntervalsCan.Text) + CDbl(txtIHIntervalsCan.Text)
                'txtMTDIntervalsCan.Text = CDbl(txtLineMTDIntervalsCan.Text) + CDbl(txtIHMTDIntervalsCan.Text)
                'txtYTDIntervalsCan.Text = CDbl(txtLineYTDIntervalsCan.Text) + CDbl(txtIHYTDIntervalsCan.Text)
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
                If txtToursAct.Text = 0 Then
                    txtVPGAct.Text = 0
                Else
                    txtVPGAct.Text = FormatNumber((CDbl(txtVolumeAct.Text) / CDbl(txtToursAct.Text)), 2, , , TriState.False)
                End If
                If txtMTDToursAct.Text = 0 Then
                    txtMTDVPGAct.Text = 0
                Else
                    txtMTDVPGAct.Text = FormatNumber((CDbl(txtMTDVolumeAct.Text) / CDbl(txtMTDToursAct.Text)), 2, , , TriState.False)
                End If
                If txtYTDToursAct.Text = 0 Then
                    txtYTDVPGAct.Text = 0
                Else
                    txtYTDVPGAct.Text = FormatNumber((CDbl(txtYTDVolumeAct.Text) / CDbl(txtYTDToursAct.Text)), 2, , , TriState.False)
                End If


                oOV = Nothing
                MultiView1.ActiveViewIndex = 0
            Else
                MultiView1.ActiveViewIndex = 1
            End If
        End If
    End Sub

    Private Sub Recalc()
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
        If txtLineToursProj.Text = 0 Then
            txtLineClosingProj.Text = 0
        Else
            txtLineClosingProj.Text = FormatNumber((CDbl(txtLineSalesProj.Text) / CDbl(txtLineToursProj.Text)) * 100, 2)
        End If
        If txtLineMTDToursProj.Text = 0 Then
            txtLineMTDToursProj.Text = 0
        Else
            txtLineMTDClosingProj.Text = FormatNumber((CDbl(txtLineMTDSalesProj.Text) / CDbl(txtLineMTDToursProj.Text)) * 100, 2)
        End If
        If txtLineYTDToursProj.Text = 0 Then
            txtLineYTDToursProj.Text = 0
        Else
            txtLineYTDClosingProj.Text = FormatNumber((CDbl(txtLineYTDSalesProj.Text) / CDbl(txtLineYTDToursProj.Text)) * 100, 2)
        End If
        If txtLineSalesAct.Text = 0 Then
            txtLineAvgSP.Text = 0
        Else
            txtLineAvgSp.Text = FormatNumber((CDbl(txtLineVolumeAct.Text) / CDbl(txtLineSalesAct.Text)), 2, , , TriState.False)
        End If
        If txtLineMTDVolumeAct.Text = 0 Then
            txtLineMTDAvgSP.Text = 0
        Else
            txtLineMTDAvgSP.Text = FormatNumber((CDbl(txtLineMTDVolumeAct.Text) / CDbl(txtLineMTDSalesAct.Text)), 2, , , TriState.False)
        End If
        If txtLineYTDVolumeAct.Text = 0 Then
            txtLineYTDAvgSP.Text = 0
        Else
            txtLineYTDAvgSP.Text = FormatNumber((CDbl(txtLineYTDVolumeAct.Text) / CDbl(txtLineYTDSalesAct.Text)), 2, , , TriState.False)
        End If
        If txtLineToursAct.Text = 0 Then
            txtLineVPG.Text = 0
        Else
            txtLineVPG.Text = FormatNumber((CDbl(txtLineVolumeAct.Text) / CDbl(txtLineToursAct.Text)), 2, , , TriState.False)
        End If
        If txtLineMTDToursAct.Text = 0 Then
            txtLineMTDVPG.Text = 0
        Else
            txtLineMTDVPG.Text = FormatNumber((CDbl(txtLineMTDVolumeAct.Text) / CDbl(txtLineMTDToursAct.Text)), 2, , , TriState.False)
        End If
        If txtLineYTDToursAct.Text = 0 Then
            txtLineYTDVPG.Text = 0
        Else
            txtLineYTDVPG.Text = FormatNumber((CDbl(txtLineYTDVolumeAct.Text) / CDbl(txtLineYTDToursAct.Text)), 2, , , TriState.False)
        End If


        If txtIHSalesAct.Text = 0 Then
            txtIHAvgSP.Text = 0
        Else
            txtIHAvgSp.Text = FormatNumber((CDbl(txtIHVolumeAct.Text) / CDbl(txtIHSalesAct.Text)), 2, , , TriState.False)
        End If
        If txtIHMTDVolumeAct.Text = 0 Then
            txtIHMTDAvgSP.Text = 0
        Else
            txtIHMTDAvgSP.Text = FormatNumber((CDbl(txtIHMTDVolumeAct.Text) / CDbl(txtIHMTDSalesAct.Text)), 2, , , TriState.False)
        End If
        If txtIHYTDVolumeAct.Text = 0 Then
            txtIHYTDAvgSP.Text = 0
        Else
            txtIHYTDAvgSP.Text = FormatNumber((CDbl(txtIHYTDVolumeAct.Text) / CDbl(txtIHYTDSalesAct.Text)), 2, , , TriState.False)
        End If

        If txtIHUpgradesAct.Text = 0 Then
            txtIHAvgVolume.Text = 0
        Else
            txtIHAvgVolume.Text = FormatNumber((CDbl(txtIHUpgradeVolAct.Text) / CDbl(txtIHUpgradesAct.Text)), 2, , , TriState.False)
        End If
        If txtIHMTDUpgradesAct.Text = 0 Then
            txtIHMTDAvgVolume.Text = 0
        Else
            txtIHMTDAvgVolume.Text = FormatNumber((CDbl(txtIHMTDUpgradeVolAct.Text) / CDbl(txtIHMTDUpgradesAct.Text)), 2, , , TriState.False)
        End If
        If txtIHYTDUpgradesAct.Text = 0 Then
            txtIHYTDAvgVolume.Text = 0
        Else
            txtIHYTDAvgVolume.Text = FormatNumber((CDbl(txtIHYTDUpgradeVolAct.Text) / CDbl(txtIHYTDUpgradesAct.Text)), 2, , , TriState.False)
        End If

        txtIHTotalVolume.Text = FormatNumber(CDbl(txtIHVolumeAct.Text) + CDbl(txtIHUpgradeVolAct.Text), 0, , , TriState.False)
        txtIHMTDTotalVolume.Text = FormatNumber(CDbl(txtIHMTDVolumeAct.Text) + CDbl(txtIHMTDUpgradeVolAct.Text), 0, , , TriState.False)
        txtIHYTDTotalVolume.Text = FormatNumber(CDbl(txtIHYTDVolumeAct.Text) + CDbl(txtIHYTDUpgradeVolAct.Text), 0, , , TriState.False)
        If txtIHToursAct.Text = 0 Then
            txtIHVPG.Text = 0
        Else
            txtIHVPG.Text = FormatNumber((CDbl(txtIHTotalVolume.Text) / CDbl(txtIHToursAct.Text)), 2, , , TriState.False)
        End If
        If txtIHMTDToursAct.Text = 0 Then
            txtIHMTDVPG.Text = 0
        Else
            txtIHMTDVPG.Text = FormatNumber((txtIHMTDTotalVolume.Text / txtIHMTDToursAct.Text), 2, , , TriState.False)
        End If
        If txtIHYTDToursAct.Text = 0 Then
            txtIHYTDVPG.Text = 0
        Else
            txtIHYTDVPG.Text = FormatNumber((txtIHYTDTotalVolume.Text / txtIHYTDToursAct.Text), 2, , , TriState.False)
        End If


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
        'txtIntervalsSales.Text = CDbl(txtLineIntervalsSales.Text) + CDbl(txtIHIntervalsSales.Text)
        'txtMTDIntervalsSales.Text = CDbl(txtLineMTDIntervalsSales.Text) + CDbl(txtIHMTDIntervalsSales.Text)
        'txtYTDIntervalsSales.TExt = CDbl(txtLineYTDIntervalsSales.Text) + CDbl(txtIHYTDIntervalsSales.Text)
        'txtIntervalsCan.Text = CDbl(txtLineIntervalsCan.Text) + CDbl(txtIHIntervalsCan.Text)
        'txtMTDIntervalsCan.Text = CDbl(txtLineMTDIntervalsCan.Text) + CDbl(txtIHMTDIntervalsCan.Text)
        'txtYTDIntervalsCan.Text = CDbl(txtLineYTDIntervalsCan.Text) + CDbl(txtIHYTDIntervalsCan.Text)
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
        If txtToursAct.Text = 0 Then
            txtVPGAct.Text = 0
        Else
            txtVPGAct.Text = FormatNumber((CDbl(txtVolumeAct.Text) / CDbl(txtToursAct.Text)), 2, , , TriState.False)
        End If
        If txtMTDToursAct.Text = 0 Then
            txtMTDVPGAct.Text = 0
        Else
            txtMTDVPGAct.Text = FormatNumber((CDbl(txtMTDVolumeAct.Text) / CDbl(txtMTDToursAct.Text)), 2, , , TriState.False)
        End If
        If txtYTDToursAct.Text = 0 Then
            txtYTDVPGAct.Text = 0
        Else
            txtYTDVPGAct.Text = FormatNumber((CDbl(txtYTDVolumeAct.Text) / CDbl(txtYTDToursAct.Text)), 2, , , TriState.False)
        End If

    End Sub

    Protected Sub txtLineToursAct_TextChanged(sender As Object, e As System.EventArgs) Handles txtLineToursAct.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtLineToursAct.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtLineToursAct.Text = oOV.LineToursAct
            txtLineMTDToursAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineToursAct")
            txtLineYTDToursAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineToursAct")
            Recalc()
        Else
            txtLineMTDToursAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineToursAct") + CDbl(txtLIneToursAct.Text) - oOV.LineToursAct
            txtLineYTDToursAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineToursAct") + CDbl(txtLIneToursAct.Text) - oOV.LineToursAct
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtLineToursProj_TextChanged(sender As Object, e As System.EventArgs) Handles txtLineToursProj.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtLineToursProj.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtLineToursProj.Text = oOV.LineToursProj
            txtLineMTDToursProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineToursProj")
            txtLineYTDToursProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineToursProj")
            Recalc()
        Else
            txtLineMTDToursProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineToursProj") + CDbl(txtLIneToursProj.Text) - oOV.LineToursProj
            txtLineYTDToursProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineToursProj") + CDbl(txtLIneToursProj.Text) - oOV.LineToursProj
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtLineSalesAct_TextChanged(sender As Object, e As System.EventArgs)
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtLineSalesAct.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtLineSalesAct.Text = oOV.LineSalesAct
            txtLineMTDSalesAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineSalesAct")
            txtLineYTDSalesAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineSalesAct")
            Recalc()
        Else
            txtLineMTDSalesAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineSalesAct") + CDbl(txtLIneSalesAct.Text) - oOV.LineSalesAct
            txtLineYTDSalesAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineSalesAct") + CDbl(txtLIneSalesAct.Text) - oOV.LineSalesAct
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtLineSalesProj_TextChanged(sender As Object, e As System.EventArgs)
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtLineSalesProj.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtLineSalesProj.Text = oOV.LineSalesProj
            txtLineMTDSalesProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineSalesProj")
            txtLineYTDSalesProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineSalesProj")
            Recalc()
        Else
            txtLineMTDSalesProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineSalesProj") + CDbl(txtLIneSalesProj.Text) - oOV.LineSalesProj
            txtLineYTDSalesProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineSalesProj") + CDbl(txtLIneSalesProj.Text) - oOV.LineSalesProj
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtLineIntervalsSales_TextChanged(sender As Object, e As System.EventArgs)
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtLineIntervalsSales.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtLineIntervalsSales.Text = oOV.LineIntervalsSales
            txtLineMTDIntervalsSales.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalsSales")
            txtLineYTDIntervalsSales.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalsSales")
            Recalc()
        Else
            txtLineMTDIntervalsSales.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalsSales") + CDbl(txtLIneIntervalsSales.Text) - oOV.LineIntervalsSales
            txtLineYTDIntervalsSales.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalsSales") + CDbl(txtLIneIntervalsSales.Text) - oOV.LineIntervalsSales
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtLineIntervalsCan_TextChanged(sender As Object, e As System.EventArgs)
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtLineIntervalsCan.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtLineIntervalsCan.Text = oOV.LineIntervalsCan
            txtLineMTDIntervalsCan.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalsCan")
            txtLineYTDIntervalsCan.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalsCan")
            Recalc()
        Else
            txtLineMTDIntervalsCan.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalsCan") + CDbl(txtLIneIntervalsCan.Text) - oOV.LineIntervalsCan
            txtLineYTDIntervalsCan.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineIntervalsCan") + CDbl(txtLIneIntervalsCan.Text) - oOV.LineIntervalsCan
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtLineVolumeAct_TextChanged(sender As Object, e As System.EventArgs)
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtLineVolumeAct.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtLineVolumeAct.Text = oOV.LineVolumeAct
            txtLineMTDVolumeAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineVolumeAct")
            txtLineYTDVolumeAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineVolumeAct")
            Recalc()
        Else
            txtLineMTDVolumeAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineVolumeAct") + CDbl(txtLIneVolumeAct.Text) - oOV.LineVolumeAct
            txtLineYTDVolumeAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineVolumeAct") + CDbl(txtLIneVolumeAct.Text) - oOV.LineVolumeAct
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtLineVolumeProj_TextChanged(sender As Object, e As System.EventArgs)
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtLineVolumeProj.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtLineVolumeProj.Text = oOV.LineVolumeProj
            txtLineMTDVolumeProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineVolumeProj")
            txtLineYTDVolumeProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineVolumeProj")
            Recalc()
        Else
            txtLineMTDVolumeProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineVolumeProj") + CDbl(txtLIneVolumeProj.Text) - oOV.LineVolumeProj
            txtLineYTDVolumeProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "LineVolumeProj") + CDbl(txtLIneVolumeProj.Text) - oOV.LineVolumeProj
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtIHToursAct_TextChanged(sender As Object, e As System.EventArgs) Handles txtIHToursAct.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtIHToursAct.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtIHToursAct.Text = oOV.IHToursAct
            txtIHMTDToursAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHToursAct")
            txtIHYTDToursAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHToursAct")
            Recalc()
        Else
            txtIHMTDToursAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHToursAct") + CDbl(txtIHToursAct.Text) - oOV.IHToursAct
            txtIHYTDToursAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHToursAct") + CDbl(txtIHToursAct.Text) - oOV.IHToursAct
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtIHToursProj_TextChanged(sender As Object, e As System.EventArgs) Handles txtIHToursProj.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtIHToursProj.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtIHToursProj.Text = oOV.IHToursProj
            txtIHMTDToursProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHToursProj")
            txtIHYTDToursProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHToursProj")
            Recalc()
        Else
            txtIHMTDToursProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHToursProj") + CDbl(txtIHToursProj.Text) - oOV.IHToursProj
            txtIHYTDToursProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHToursProj") + CDbl(txtIHToursProj.Text) - oOV.IHToursProj
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtIHSalesAct_TextChanged(sender As Object, e As System.EventArgs) Handles txtIHSalesAct.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtIHSalesAct.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtIHSalesAct.Text = oOV.IHSalesAct
            txtIHMTDSalesAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHSalesAct")
            txtIHYTDSalesAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHSalesAct")
            Recalc()
        Else
            txtIHMTDSalesAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHSalesAct") + CDbl(txtIHSalesAct.Text) - oOV.IHSalesAct
            txtIHYTDSalesAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHSalesAct") + CDbl(txtIHSalesAct.Text) - oOV.IHSalesAct
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtIHSalesProj_TextChanged(sender As Object, e As System.EventArgs) Handles txtIHSalesProj.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtIHSalesProj.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtIHSalesProj.Text = oOV.IHSalesProj
            txtIHMTDSalesProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHSalesProj")
            txtIHYTDSalesProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHSalesProj")
            Recalc()
        Else
            txtIHMTDSalesProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHSalesProj") + CDbl(txtIHSalesProj.Text) - oOV.IHSalesProj
            txtIHYTDSalesProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHSalesProj") + CDbl(txtIHSalesProj.Text) - oOV.IHSalesProj
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtIHIntervalsSales_TextChanged(sender As Object, e As System.EventArgs) Handles txtIHIntervalsSales.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtIHIntervalsSales.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtIHIntervalsSales.Text = oOV.IHIntervalsSales
            txtIHMTDIntervalsSales.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsSales")
            txtIHYTDIntervalsSales.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsSales")
            Recalc()
        Else
            txtIHMTDIntervalsSales.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsSales") + CDbl(txtIHIntervalsSales.Text) - oOV.IHIntervalsSales
            txtIHYTDIntervalsSales.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsSales") + CDbl(txtIHIntervalsSales.Text) - oOV.IHIntervalsSales
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtIHIntervalsCan_TextChanged(sender As Object, e As System.EventArgs) Handles txtIHIntervalsCan.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtIHIntervalsCan.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtIHIntervalsSales.Text = oOV.IHIntervalsSales
            txtIHMTDIntervalsCan.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsCan")
            txtIHYTDIntervalsCan.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsCan")
            Recalc()
        Else
            txtIHMTDIntervalsCan.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsCan") + CDbl(txtIHIntervalsCan.Text) - oOV.IHIntervalsCan
            txtIHYTDIntervalsCan.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHIntervalsCan") + CDbl(txtIHIntervalsCan.Text) - oOV.IHIntervalsCan
            Recalc()
        End If
        oOV = Nothing

    End Sub

    Protected Sub txtIHVolumeAct_TextChanged(sender As Object, e As System.EventArgs) Handles txtIHVolumeAct.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtIHVolumeAct.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtIHVolumeAct.Text = oOV.IHVolumeAct
            txtIHMTDVolumeAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeAct")
            txtIHYTDVolumeAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeAct")
            Recalc()
        Else
            txtIHMTDVolumeAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeAct") + CDbl(txtIHVolumeAct.Text) - oOV.IHVolumeAct
            txtIHYTDVolumeAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeAct") + CDbl(txtIHVolumeAct.Text) - oOV.IHVolumeAct
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtIHVolumeProj_TextChanged(sender As Object, e As System.EventArgs) Handles txtIHVolumeProj.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtIHVolumeAct.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtIHVolumeProj.Text = oOV.IHVolumeProj
            txtIHMTDVolumeProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeProj")
            txtIHYTDVolumeProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeProj")
            Recalc()
        Else
            txtIHMTDVolumeProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeProj") + CDbl(txtIHVolumeProj.Text) - oOV.IHVolumeProj
            txtIHYTDVolumeProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHVolumeProj") + CDbl(txtIHVolumeProj.Text) - oOV.IHVolumeProj
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtIHUpgradesAct_TextChanged(sender As Object, e As System.EventArgs) Handles txtIHUpgradesAct.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtIHUpgradesAct.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtIHUpgradesAct.Text = oOV.IHUpgradesAct
            txtIHMTDUpgradesAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesAct")
            txtIHYTDUpgradesAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesAct")
            Recalc()
        Else
            txtIHMTDUpgradesAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesAct") + CDbl(txtIHUpgradesAct.Text) - oOV.IHUpgradesAct
            txtIHYTDUpgradesAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesAct") + CDbl(txtIHUpgradesAct.Text) - oOV.IHUpgradesAct
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtIHUpgradesProj_TextChanged(sender As Object, e As System.EventArgs) Handles txtIHUpgradesProj.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtIHUpgradesProj.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtIHUpgradesProj.Text = oOV.IHUpgradesProj
            txtIHMTDUpgradesProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesProj")
            txtIHYTDUpgradesProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesProj")
            Recalc()
        Else
            txtIHMTDUpgradesProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesProj") + CDbl(txtIHUpgradesProj.Text) - oOV.IHUpgradesProj
            txtIHYTDUpgradesProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradesProj") + CDbl(txtIHUpgradesProj.Text) - oOV.IHUpgradesProj
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtIHUpgradeVolAct_TextChanged(sender As Object, e As System.EventArgs) Handles txtIHUpgradeVolAct.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtIHUpgradeVolAct.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtIHUpgradeVolAct.Text = oOV.IHUpgradeVolumeAct
            txtIHMTDUpgradeVolAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradeVolumeAct")
            txtIHYTDUpgradeVolAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradeVolumeAct")
            Recalc()
        Else
            txtIHMTDUpgradeVolAct.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradeVolumeAct") + CDbl(txtIHUpgradeVolAct.Text) - oOV.IHUpgradeVolumeAct
            txtIHYTDUpgradeVolAct.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradeVolumeAct") + CDbl(txtIHUpgradeVolAct.Text) - oOV.IHUpgradeVolumeAct
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub txtIHUpgradeVolProj_TextChanged(sender As Object, e As System.EventArgs) Handles txtIHUpgradeVolProj.TextChanged
        Dim oOV As New clsCombinedOverview
        oOV.CombinedOverviewID = Request("overviewid")
        oOV.Load()
        If Not (IsNumeric(txtIHUpgradeVolProj.Text)) Then 'Or CDbl(txtLineToursAct.Text) < 0 Then
            txtIHUpgradeVolProj.Text = oOV.IHUpgradeVolumeProj
            txtIHMTDUpgradeVolProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradeVolumeProj")
            txtIHYTDUpgradeVolProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradeVolumeProj")
            Recalc()
        Else
            txtIHMTDUpgradeVolProj.Text = oOV.Get_MTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradeVolumeProj") + CDbl(txtIHUpgradeVolProj.Text) - oOV.IHUpgradeVolumeProj
            txtIHYTDUpgradeVolProj.Text = oOV.Get_YTD_Total(oOV.OverviewDate, oOV.OverviewLocation, "IHUpgradeVolumeProj") + CDbl(txtIHUpgradeVolProj.Text) - oOV.IHUpgradeVolumeProj
            Recalc()
        End If
        oOV = Nothing
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If CheckSecurity("Overview", "Edit", , , Session("UserDBID")) Then
            Dim oOV As New clsCombinedOverview
            oOV.CombinedOverviewID = Request("overviewid")
            oOV.Load()
            oOV.LineToursAct = txtLineToursAct.Text
            oOV.LineToursProj = txtLineToursProj.Text
            oOV.LineSalesAct = txtLIneSalesAct.Text
            oOV.LineSalesProj = txtLIneSalesProj.Text
            'oOV.LineIntervalsSales = txtLineIntervalsSales.Text
            'oOV.LineIntervalsCan = txtLineIntervalsCan.Text
            oOV.LineClosingProj = txtLIneClosingProj.Text
            oOV.LineVolumeAct = txtLIneVolumeAct.TExt
            oOV.LineVolumeProj = txtLIneVolumeProj.Text
            oOV.IHToursAct = txtIHToursAct.Text
            oOV.IHVolumeAct = txtIHVolumeAct.Text
            oOV.IHVolumeProj = txtIHVolumeProj.Text
            oOV.IHToursProj = txtIHToursProj.Text
            oOV.IHSalesProj = txtIHSalesProj.Text
            oOV.IHSalesAct = txtIHSalesAct.Text
            'oOV.IHIntervalsSales = txtIHIntervalsSales.Text
            'oOV.IHIntervalsCan = txtIHIntervalsCan.Text
            oOV.IHUpgradesAct = txtIHUpgradesAct.Text
            oOV.IHUpgradesProj = txtIHUpgradesProj.Text
            oOV.IHUpgradeVolumeAct = txtIHUpgradeVolAct.Text
            oOV.IHUpgradeVolumeProj = txtIHUpgradeVolProj.Text
            oOV.LastUpdatedBy = Session("UserDBID")
            oOV.DateUpdated = System.DateTime.Now.ToShortDateString
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & oOV.IHUpgradesProj & " " & oOV.IHUpgradeVolumeProj & "');", True)

            If oOV.Save() Then
                Response.Redirect("CombinedOverView.aspx?overviewid=" & oOV.CombinedOverviewID)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('No way dude.');", True)
            End If
            oOV = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Do Not Have Persmission to Edit Overview.');", True)
        End If
    End Sub
End Class
