
Partial Class Reports_Accounting_PremiumReport
    Inherits System.Web.UI.Page

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        If dfSDate.Selected_Date <> "" And dfEDate.Selected_Date <> "" Then Run_Report()
    End Sub

   

    Private Sub Run_Report()
        lit1.text = ""
        Dim cn As Object
        Dim rst As Object
        Dim sDate As String
        Dim eDate As String
        'Dim newcamp() As String
        'Dim tourType As String
        'Dim premType As String
        'Dim campName As String
        'Dim tourStatus As String
        'Dim premName As String
        'Dim campPremTotal As String
        'Dim ptypePremTotal As String
        'Dim ptypePremCount As String
        'Dim ttypePremTotal As String
        'Dim ttypePremCount As String
        'Dim tStatusPremCount As String
        'Dim tStatusPremTotal As String
        'Dim premCount As String
        'Dim premTotal As String
        'Dim gPremTotal As String
        'Dim gPremCount As String
        'Dim campIHtotal As String
        'Dim campIHcount As String
        'Dim campDLtotal As String
        'Dim campDLcount As String
        'Dim typeIHtotal As String
        'Dim typeIHcount As String
        'Dim typeDLtotal As String
        'Dim typeDLcount As String
        'Dim IHtotal As String
        'Dim IHcount As String
        'Dim DLtotal As String
        'Dim DLcount As String
        'Dim campRICcount As String
        'Dim campRICtotal As String
        'Dim typeRICcount As String
        'Dim typeRICtotal As String
        'Dim riccount As String
        'Dim rictotal As String
        'Dim campEXcount As String
        'Dim campEXtotal As String
        'Dim typeEXcount As String
        'Dim typeEXtotal As String
        'Dim extotal As String
        'Dim excount As String
        'Dim campNAcount As String
        'Dim campNAtotal As String
        'Dim typeNAcount As String
        'Dim typeNAtotal As String
        'Dim natotal As String
        'Dim nacount As String
        Dim lstTst As String
        Dim lstLoc As String
        Dim gpTotal As String
        Dim gpcTotal As String
        Dim gpTotal1 As String
        Dim gpcTotal1 As String

        sDate = CDate(dfSDate.Selected_Date)
        eDate = CDate(dfEDate.Selected_Date)
        Dim i As Integer
        cn = Server.CreateObject("ADODB.Connection")
        rst = Server.CreateObject("ADODB.Recordset")

        'DBName = "CRMSData"

        Dim aTST() As String 'TourSubType Array
        Dim aTSTP() As String 'TourSubType Premium Count Array
        Dim aTSTPC() As String 'TourSubType Premium Cost Array
        Dim aCTST() As String
        Dim aCTSTP() As String
        Dim aCTSTPC() As String

        ReDim aTST(0)
        ReDim aTSTP(0)
        ReDim aTSTPC(0)
        ReDim aCTST(0)
        ReDim aCTSTP(0)
        ReDim aCTSTPC(0)

        aTST(0) = ""
        aTSTP(0) = 0
        aTSTPC(0) = 0
        aCTST(0) = ""
        aCTSTP(0) = 0
        aCTSTPC(0) = 0

        'DBName = "CRMSData"
        cn.OPen(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        'Response.ContentType = "application/vnd.ms-excel"
        'Response.AddHeader("Content-Disposition", "attachment; filename=premiums.xls")

        rst.Open("Select TourType, Location, toursubtype,campaign, tourstatus,premtype,premium,sum(qtyissued) as qtyissued, sum(totalcost) as totalcost from v_PremiumReport where DateIssued between '" & sDate & "' and '" & eDate & "' and premiumstatus = 'Issued' group by location, tourtype,toursubtype,campaign,tourstatus,premtype,premium", cn, 3, 3)
        If rst.EOF And rst.BOF Then
            lit1.text &= ("No Premiums Issued in this date range")
        Else
            lstTST = ""
            lstLoc = ""
            lit1.text &= ("<table border ='1'>")
            lit1.text &= ("<tr>")
            lit1.text &= ("<th>Tour Type</th>")
            lit1.text &= ("<th>Location</th>")
            lit1.text &= ("<th>Line</th>")
            lit1.text &= ("<th>Campaign</th>")
            lit1.text &= ("<th>Tour Status</th>")
            lit1.text &= ("<th>Premium Type</th>")
            lit1.text &= ("<th>Premium</th>")
            lit1.text &= ("<th>Qty Issued</th>")
            lit1.text &= ("<th>Cost</th>")
            lit1.text &= ("</tr>")
            Do While Not rst.EOF
                If lstTst <> rst.fields("TourSubType").value & "" Or lstLoc <> rst.fields("Location").value & "" Then
                    If rst.fields("Location").value & "" = "VacationClub" Then
                        For i = 0 To UBound(aCTST)
                            If aCTST(i) = rst.fields("TourSubType").value & "" Or aCTST(i) = "" Then
                                If rst.fields("TourSubType").value & "" = "" Then
                                    aCTST(i) = "NONE"
                                Else
                                    aCTST(i) = rst.fields("TourSubType").value & ""
                                End If
                                Exit For
                            End If
                        Next
                        If i > UBound(aCTST) Then
                            ReDim Preserve aCTST(i)
                            ReDim Preserve aCTSTP(i)
                            ReDim Preserve aCTSTPC(i)
                            aCTST(i) = rst.fields("toursubtype").value & ""
                            aCTSTP(i) = 0
                            aCTSTPC(i) = 0
                        End If
                    Else
                        For i = 0 To UBound(aTST)
                            If aTST(i) = rst.fields("TourSubType").value & "" Or aTST(i) = "" Then
                                If rst.fields("TourSubType").value & "" = "" Then
                                    aTST(i) = "NONE"
                                Else
                                    aTST(i) = rst.fields("TourSubType").value & ""
                                End If
                                Exit For
                            End If
                        Next
                        If i > UBound(aTST) Then
                            ReDim Preserve aTST(i)
                            ReDim Preserve aTSTP(i)
                            ReDim Preserve aTSTPC(i)
                            aTST(i) = rst.fields("toursubtype").value & ""
                            aTSTP(i) = 0
                            aTSTPC(i) = 0
                        End If
                    End If
                    lstTst = rst.fields("toursubtype").value & ""
                    lstLoc = rst.fields("Location").value & ""
                End If
                If lstLoc = "VacationClub" Then
                    aCTSTP(i) = aCTSTP(i) + rst.fields("QtyIssued").value
                    aCTSTPC(i) = aCTSTPC(i) + rst.fields("TotalCost").value
                Else
                    aTSTP(i) = aTSTP(i) + rst.fields("QtyIssued").value
                    aTSTPC(i) = aTSTPC(i) + rst.fields("TotalCost").value
                End If
                lit1.text &= ("<tr>")
                lit1.text &= ("<td align='left'>" & rst.fields("TourType").value & "</td>")
                lit1.text &= ("<td align='left'>" & rst.fields("Location").value & "</td>")
                lit1.text &= ("<td align='left'>" & rst.fields("TourSubType").value & "</td>")
                lit1.text &= ("<td align='right'>" & rst.fields("Campaign").value & "</td>")
                lit1.text &= ("<td align='right'>" & rst.fields("TourStatus").value & "</td>")
                lit1.text &= ("<td align='right'>" & rst.fields("PremType").value & "</td>")
                lit1.text &= ("<td align='right'>" & rst.fields("Premium").value & "</td>")
                lit1.text &= ("<td align='right'>" & rst.fields("QtyIssued").value & "</td>")
                lit1.text &= ("<td align='right'>" & FormatCurrency(rst.fields("TotalCost").value) & "</td>")
                lit1.text &= ("</tr>")

                rst.MoveNext()
            Loop
            lit1.text &= ("</table>")

            lit1.text &= ("<table border ='1'>")
            lit1.text &= ("<tr>")
            lit1.text &= ("<th colspan='3'>KCP</th>")
            lit1.text &= ("</tr>")
            gptotal = 0
            gpctotal = 0
            For i = 0 To UBound(aTST)
                lit1.text &= ("<tr>")
                lit1.text &= ("<td>" & aTST(i) & "</td>")
                lit1.text &= ("<td align = 'right'>" & aTSTP(i) & "</td>")
                lit1.text &= ("<td align = 'right'>" & FormatCurrency(aTSTPC(i)) & "</td>")
                lit1.text &= ("</tr>")
                gpTotal = CDbl(gpTotal) + CDbl(aTSTP(i))
                gpcTotal = CDbl(gpcTotal) + CDbl(aTSTPC(i))
            Next

            'lit1.text &= "<tr>"
            'lit1.text &= "<td colspan='3'>&nbsp;</td>"
            'lit1.text &= "</tr>"

            lit1.text &= ("<tr>")
            lit1.text &= ("<th>TOTAL:</th>")
            lit1.text &= ("<th align='right'>" & gptotal & "</th>")
            Lit1.Text &= ("<th align = 'right'>" & FormatCurrency(gpcTotal) & "</th>")
            lit1.text &= ("</tr>")

            lit1.text &= ("<tr>")
            lit1.text &= ("<td colspan='3'>&nbsp;</td>")
            lit1.text &= ("</tr>")

            lit1.text &= ("<tr>")
            lit1.text &= ("<th colspan='3'>Vacation Club</th>")
            lit1.text &= ("</tr>")

            gptotal1 = gptotal
            gpctotal1 = gpctotal

            For i = 0 To UBound(aCTST)
                lit1.text &= ("<tr>")
                lit1.text &= ("<td>" & aCTST(i) & "</td>")
                lit1.text &= ("<td align = 'right'>" & aCTSTP(i) & "</td>")
                lit1.text &= ("<td align = 'right'>" & FormatCurrency(aCTSTPC(i)) & "</td>")
                lit1.text &= ("</tr>")
                gpTotal = CDbl(gpTotal) + aCTSTP(i)
                gpcTotal = CDbl(gpcTotal) + aCTSTPC(i)
            Next

            lit1.text &= ("<tr>")
            lit1.text &= ("<th>TOTAL:</th>")
            lit1.text &= ("<th align='right'>" & gptotal - gptotal1 & "</th>")
            lit1.text &= ("<th align = 'right'>" & FormatCurrency(gpctotal - gpctotal1) & "</th>")
            lit1.text &= ("</tr>")

            lit1.text &= ("<tr>")
            lit1.text &= ("<td colspan='3'>&nbsp;</td>")
            lit1.text &= ("</tr>")

            lit1.text &= ("<tr>")
            lit1.text &= ("<th>Grand Total</th>")
            lit1.text &= ("<th align='right'>" & gptotal & "</th>")
            lit1.text &= ("<th align = 'right'>" & FormatCurrency(gpctotal) & "</th>")
            lit1.text &= ("</tr>")
            lit1.text &= ("</table>")
        End If

        rst.CLose()
        cn.Close()
        rst = Nothing
        cn = Nothing
    End Sub

   
    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        If Lit1.Text <> "" Then
            Response.Clear()
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment; filename=premiums.xls")
            Response.Write(Lit1.Text)
            Response.End()
        End If
    End Sub
End Class
