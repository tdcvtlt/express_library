Imports Microsoft.VisualBasic
Imports System.Math
Partial Class Reports_Contracts_AvailableInventory
    Inherits System.Web.UI.Page

    

    Public Function Calc_Tri_Availability(ByVal strStart As String, ByVal strEnd As String) As String
        Dim ans As String
        Dim diff As Integer
        If strStart = "" Then
            ans = ""
        Else
            If strEnd = "" Then
                strEnd = strStart
            End If
            If strStart = strEnd Then
                'Means that 2 occ years are avail
                'Determine if this or next or after next are avail
                If strStart >= Year(System.DateTime.Now) Then
                    Do While strStart >= Year(System.DateTime.Now)
                        strStart = strStart - 3
                    Loop
                    strStart = strStart + 4
                    strEnd = strStart + 1
                Else
                    Do While strStart <= Year(System.DateTime.Now)
                        strStart = strStart + 3
                    Loop
                    strStart = strStart - 2
                    strEnd = strStart + 1
                End If
                If strEnd = Year(System.DateTime.Now) + 3 Then
                    strEnd = strStart
                    strStart = Year(System.DateTime.Now)
                End If
                strEnd = ", " & strEnd
            Else
                diff = Abs(strStart - strEnd)
                If strStart > strEnd Then
                    strStart = strStart + diff
                Else
                    strStart = strEnd + diff
                End If
                If strStart >= Year(System.DateTime.Now) Then
                    Do While strStart >= Year(System.DateTime.Now)
                        strStart = strStart - 3
                    Loop
                    strStart = strStart + 3
                Else
                    Do While strStart <= Year(System.DateTime.Now)
                        strStart = strStart + 3
                    Loop
                    strStart = strStart - 3
                End If
                strEnd = ""
            End If
            ans = strStart & strEnd
        End If
        Calc_Tri_Availability = ans
    End Function

    Public Sub Write_Report()
        Dim cn As Object
        Dim rs As Object

        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        Dim sSQL As String
        sSQL = "SELECT t_SalesInventory.InventoryTypeID, t_SalesInventory.WeekTypeID, t_Units.UnitName, t_SalesInventory.Week, t_SoldInventory.FrequencyID, t_SoldInventory.OccupancyYear, t_ComboItems.ComboItem, t_SalesInventory.Status FROM   ((t_Units t_Units INNER JOIN t_SalesInventory t_SalesInventory ON t_Units.UnitID=t_SalesInventory.UnitID) INNER JOIN t_ComboItems t_ComboItems ON t_SalesInventory.WeekTypeID=t_ComboItems.ComboItemID) LEFT OUTER JOIN t_SoldInventory t_SoldInventory ON t_SalesInventory.SalesInventoryID=t_SoldInventory.SalesInventoryID ORDER BY t_Units.UnitName, t_SalesInventory.Week " 't_SalesInventory.InventoryTypeID"
        rs.open(sSQL, cn, 3, 3) '"select * from t_Units u inner join t_Salesinventory i on i.unitid = u.unitid left outer join t_Soldinventory s on s.salesinventoryid = i.salesinventoryid order by unitname, week", cn, 3, 3

        Dim lastUnit As String = ""
        Dim lastWeek As Integer = 0
        'lastUnit = ""
        'lastWeek = 0
        Response.Write("<table border = 1>")

        Dim bAnnual As Boolean
        Dim bBiAnnual As Boolean
        Dim bTriAnnual As Boolean
        Dim bOddSold As Boolean
        Dim bEvenSold As Boolean
        Dim bAnnualSold As Boolean
        Dim bTri1Sold As Boolean
        Dim bTri2Sold As Boolean
        Dim bTri3Sold As Boolean
        Dim bHolder As Boolean
        Dim bTri1YearSold As String = ""
        Dim bTri2YearSold As String = ""

        Dim i As Integer
        Dim units As Integer
        i = 0
        units = 0

        Do While Not rs.eof


            'Check last week
            If lastWeek <> rs.fields("Week").value Then

                litReport.Text &= "<tr><td>&nbsp;</td><td style='border-bottom:solid black thin;'>" & lastWeek & "</td><td style='border-bottom:solid black thin;'>"
                '******
                If bBiAnnual Then
                    If bOddSold Then
                        litReport.Text &= "Even"
                    Else
                        litReport.Text &= "Odd"
                    End If
                Else
                    If bTriAnnual Then
                        If bTri3Sold Then
                            litReport.Text &= "None"
                        Else
                            If bTri2Sold Then
                                litReport.Text &= "Triannual - 1 - " & Calc_Tri_Availability(bTri1YearSold, bTri2YearSold)
                            Else
                                litReport.Text &= "Triannual - 2 - " & Calc_Tri_Availability(bTri1YearSold, bTri1YearSold)
                            End If
                        End If
                    Else
                        litReport.Text &= "All Avail"
                    End If
                End If
                litReport.Text &= "</td></tr>"
                '******
                lastWeek = rs.fields("Week").value
                If i > 0 Then
                    If ((bAnnual And bAnnualSold) Or (bBiAnnual And bOddSold And bEvenSold) Or (bTriAnnual And bTri3Sold)) Then
                    Else
                        litReport.Text &= "<tr><td>&nbsp</td><td>" & lastWeek & "</td><td>&nbsp</td></tr>"
                    End If
                End If
                bAnnual = False
                bBiAnnual = False
                bTriAnnual = False
                bOddSold = False
                bEvenSold = False
                bAnnualSold = False
                bTri1Sold = False
                bTri2Sold = False
                bTri3Sold = False
                bTri1YearSold = ""
                bTri2YearSold = ""

            End If

            'Check last unit
            If lastUnit <> rs.fields("UnitName").value Then
                lastUnit = rs.fields("UnitName").value
                If units Mod 4 = 0 And units <> 0 Then
                    Response.Write("</table></td></tr><tr><td valign = top><table width='100%'>")
                ElseIf units = 0 Then
                    Response.Write("<tr><td valign = top><table width='100%'>")
                Else
                    Response.Write("</table></td><td valign=top><table width='100%'>")
                End If
                units = units + 1
                Response.Write("<tr><td colspan=3><b>" & lastUnit & "</b></td></tr>")
            End If

            'Check availability
            If rs.fields("FrequencyID").value Is System.DBNull.Value Then
                bHolder = False
            Else
                If rs.fields("FrequencyID").value = 1 Then
                    bAnnual = True
                    bAnnualSold = True
                Else
                    If rs.fields("FrequencyID").value = 2 Then
                        bBiAnnual = True
                        If rs.fields("OccupancyYear").value Mod 2 = 0 Then
                            bEvenSold = True
                        Else
                            bOddSold = True
                        End If
                    Else
                        If rs.fields("FrequencyID").value = 3 Then
                            bTriAnnual = True
                            If bTri1Sold And bTri2Sold Then
                                bTri3Sold = True
                            Else
                                If bTri1Sold Then
                                    bTri2Sold = True
                                    bTri2YearSold = rs.fields("OccupancyYear").value
                                Else
                                    bTri1Sold = True
                                    bTri1YearSold = rs.fields("OccupancyYear").value
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            rs.movenext()
            i = i + 1

        Loop
        Response.Write("</table></td></tr></table>")
        rs.close()
        cn.close()
        rs = Nothing
        cn = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Write_Report()
        End If
    End Sub
End Class
