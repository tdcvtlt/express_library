Imports System.Data
Partial Class controls_TestCalendar
    Inherits System.Web.UI.UserControl

    Public Event Date_Selected()
    Dim _NumDays As Integer = 0
    Dim row1back As String = "#CCFFFF"
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            For i = 1 To 12
                ddMonths.Items.Add(New ListItem(MonthName(i), i))
            Next

            For i = Year(System.DateTime.Now) To Year(System.DateTime.Now) + 2
                ddYears.Items.Add(New ListItem(i, i))
            Next
            Dim opkgcost As New clsPackageAdditionalCost
            Dim dt As DataTable = opkgcost.get_AdditionalCost(168, CDate(Month(System.DateTime.Now) & "/1/" & Year(System.DateTime.Now)), CDate(Month(System.DateTime.Now) & "/" & DateTime.DaysInMonth(Year(System.DateTime.Now), Month(System.DateTime.Now)) & "/" & Year(System.DateTime.Now)))
            opkgcost = Nothing
            ddMonths.SelectedValue = Month(System.DateTime.Now)
            ddYears.SelectedValue = Year(System.DateTime.Now)
            Dim tempDate As Date
            Dim startDay As Integer = 0
            Dim amt As Integer = 0
            For i = 1 To DateTime.DaysInMonth(Year(System.DateTime.Now), Month(System.DateTime.Now))
                tempDate = CDate(Month(System.DateTime.Now) & "/" & i & "/" & Year(System.DateTime.Now))
                For k = 0 To dt.Rows.Count
                    If dt.Rows(k).Item("Day") = tempDate Then
                        amt = dt.Rows(k).Item("Rate")
                        Exit For
                    End If
                Next
                If i = 1 Then
                    startDay = Weekday(tempDate) - 1
                End If
                For j = 1 To 6
                    If j = 1 Then
                        If CType(Table1.Rows(j).Cells(Weekday(tempDate) - 1).FindControl("LinkButton" & (j - 1) * 7 + Weekday(tempDate)), LinkButton).Text = "" And Weekday(tempDate) - 1 >= startDay Then
                            Dim lbl As LinkButton = CType(Table1.Rows(j).Cells(Weekday(tempDate) - 1).FindControl("LinkButton" & (j - 1) * 7 + Weekday(tempDate)), LinkButton)
                            lbl.Text = "<center>" & i & "</center>"
                            If amt > 0 Then
                                lbl.Text = lbl.Text & "<br><center>" & FormatCurrency(amt, 2) & "</center>"
                            End If
                            Exit For
                        End If

                    Else
                        If CType(Table1.Rows(j).Cells(Weekday(tempDate) - 1).FindControl("LinkButton" & (j - 1) * 7 + Weekday(tempDate)), LinkButton).Text = "" Then
                            Dim lbl As LinkButton = CType(Table1.Rows(j).Cells(Weekday(tempDate) - 1).FindControl("LinkButton" & (j - 1) * 7 + Weekday(tempDate)), LinkButton)
                            lbl.Text = "<center>" & i & "</center>"
                            If amt > 0 Then
                                lbl.Text = lbl.Text & "<br><center>" & FormatCurrency(amt, 2) & "</center>"
                            End If

                            Exit For
                        End If
                    End If

                Next
            Next
        End If
    End Sub

    Private Sub Clear_Calendar()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "AjaxCall", "alert('CLEAR!.');", True)

        For i = 1 To 6
            For j = 0 To 6
                CType(Table1.Rows(i).Cells(j).FindControl("LinkButton" & (i - 1) * 7 + (j + 1)), LinkButton).Text = ""
                Table1.Rows(i).Cells(j).BackColor = Drawing.Color.White
                'Table1.Rows(i).Cells(j).Text = ""
            Next
        Next
    End Sub


    Protected Sub ddYearsSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddYears.SelectedIndexChanged
        Clear_Calendar()
        Populate_Calendar()
    End Sub

    Protected Sub ddMonthsSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddMonths.SelectedIndexChanged
        Clear_Calendar()
        Populate_Calendar()
    End Sub

    Private Sub Populate_Calendar()
        Dim tempDate As Date
        Dim startDay As Integer = 0
        Dim amt As Double = 0
        Dim opkgcost As New clsPackageAdditionalCost
        Dim dt As DataTable = opkgcost.get_AdditionalCost(168, CDate(ddMonths.SelectedValue & "/1/" & ddYears.SelectedValue), CDate(ddMonths.SelectedValue & "/" & DateTime.DaysInMonth(ddYears.SelectedValue, ddMonths.SelectedValue) & "/" & ddYears.SelectedValue))
        opkgcost = Nothing
        For i = 1 To DateTime.DaysInMonth(ddYears.SelectedValue, ddMonths.SelectedValue)
            tempDate = CDate(ddMonths.SelectedValue & "/" & i & "/" & ddYears.SelectedValue)
            For k = 0 To dt.Rows.Count
                If dt.Rows(k).Item("Day") = tempDate Then
                    amt = dt.Rows(k).Item("Rate")
                    Exit For
                End If
            Next
            If i = 1 Then
                startDay = Weekday(tempDate) - 1
            End If
            For j = 1 To 6
                If j = 1 Then
                    If CType(Table1.Rows(j).Cells(Weekday(tempDate) - 1).FindControl("LinkButton" & (j - 1) * 7 + Weekday(tempDate)), LinkButton).Text = "" And Weekday(tempDate) - 1 >= startDay Then
                        Dim lbl As LinkButton = CType(Table1.Rows(j).Cells(Weekday(tempDate) - 1).FindControl("LinkButton" & (j - 1) * 7 + Weekday(tempDate)), LinkButton)
                        lbl.Text = "<center>" & i & "</center>"
                        If amt > 0 Then
                            lbl.Text = lbl.Text & "<br><center>" & FormatCurrency(amt, 2) & "</center>"
                        End If
                        Exit For
                    End If

                Else
                    If CType(Table1.Rows(j).Cells(Weekday(tempDate) - 1).FindControl("LinkButton" & (j - 1) * 7 + Weekday(tempDate)), LinkButton).Text = "" Then
                        Dim lbl As LinkButton = CType(Table1.Rows(j).Cells(Weekday(tempDate) - 1).FindControl("LinkButton" & (j - 1) * 7 + Weekday(tempDate)), LinkButton)
                        lbl.Text = "<center>" & i & "</center>"
                        If amt > 0 Then
                            lbl.Text = lbl.Text & "<br><center>" & FormatCurrency(amt, 2) & "</center>"
                        End If
                        Exit For
                    End If
                End If
            Next
        Next
        If hfDate.Value <> "0" Then
            RaiseEvent Date_Selected()
        End If
    End Sub

    Public Sub Select_Range(ByVal sDate As Date, ByVal numDays As Integer)
        Dim eDate As Date = sDate.AddDays(numDays - 1)
        'If (Month(sDate) = ddMonths.SelectedValue And Year(sDate) = ddYears.SelectedValue) Or (Month(eDate) = ddMonths.SelectedValue And Year(eDate) = ddYears.SelectedValue) Then
        Dim bHighlight As Boolean = False
        Dim cellCount As Integer = 1
        Dim dayCount As Integer = 1
        Dim k As Integer = 0
        For i = 1 To 6
            For j = 0 To 6
                If CType(Table1.Rows(i).Cells(j).FindControl("LinkButton" & cellCount), LinkButton).Text <> "" Then
                    If DateTime.Compare(CDate(ddMonths.SelectedValue & "/" & dayCount & "/" & ddYears.SelectedValue), sDate) >= 0 And DateTime.Compare(CDate(ddMonths.SelectedValue & "/" & dayCount & "/" & ddYears.SelectedValue), eDate) <= 0 Then
                        bHighlight = True
                    Else
                        bHighlight = False
                    End If
                    dayCount += 1
                Else
                    bHighlight = False
                End If
                If bHighlight Then
                    Table1.Rows(i).Cells(j).BackColor = Drawing.Color.Coral
                Else
                    Table1.Rows(i).Cells(j).BackColor = Drawing.Color.White
                End If
                cellCount += 1
            Next
        Next
        'Else
        'Clear_Calendar()
        'Populate_Calendar()
        'End If
    End Sub


    Protected Sub Date_Click(sender As Object, e As System.EventArgs)

        Dim selDay As Integer = 1
        Dim cellCounter As Integer = 1
        Dim bStart As Boolean = False
        Dim bFound As Boolean = False
        Dim sDate As Date
        For i = 1 To 6
            For j = 0 To 6
                If CType(Table1.Rows(i).Cells(j).FindControl("LinkButton" & cellCounter), LinkButton).Text <> "" Then
                    bStart = True
                End If

                If CType(sender.Parent, TableCell).ID = "TableCell" & cellCounter Then
                    sDate = CDate(ddMonths.SelectedValue & "/" & selDay & "/" & ddYears.SelectedValue)
                    bFound = True
                    Exit For
                End If
                If bStart Then
                    selDay += 1
                End If
                cellCounter += 1

            Next
            If bFound Then
                Exit For
            End If
        Next

        hfDate.Value = sDate
        hfDateCell.Value = CType(sender.Parent, TableCell).ID ' & " " & sender.Parent.ColumnIndex
        RaiseEvent Date_Selected()
    End Sub

    Property dateSelected As String
        Get
            Return hfDate.Value
        End Get
        Set(value As String)
            hfDate.Value = value
        End Set
    End Property

    Property cellSelected As String
        Get
            Return hfDateCell.Value
        End Get
        Set(value As String)
            hfDateCell.Value = value
        End Set
    End Property

    Property headerbground As String
        Get
            Return row1back
        End Get
        Set(value As String)
            row1back = value
        End Set
    End Property

    Property numDays As Integer
        Get
            Return _NumDays
        End Get
        Set(value As Integer)
            _NumDays = value
        End Set
    End Property
End Class
