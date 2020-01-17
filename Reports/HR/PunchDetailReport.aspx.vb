
Imports System
Partial Class Reports_HR_PunchDetailReport
    Inherits System.Web.UI.Page

    Dim sReport As String = "reportfiles/DailyDeposits.rpt"

    Private Sub BindData()
        'Dim ds As New SqlDataSource
        'ds.ConnectionString = Resources.Resource.cns
        'Try
        '    ds.SelectCommand = "Select * from t_CCMerchantAccount order by AccountName desc"
        '    ddDepartment.DataSource = ds
        '    ddDepartment.DataTextField = "Description"
        '    ddDepartment.DataValueField = "Accountname"
        '    ddDepartment.DataBind()
        'Catch ex As Exception
        '    lit1.text &= (ex.Message)
        'End Try
        'ds = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then

        Else
            Session("Report") = Nothing
            BindData()
            Setup_Report()
        End If

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            'Report.Clone()
            'Report.Dispose()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Setup_Report()
       
    End Sub

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        If dfSDate.Selected_Date <> "" And dfEDate.Selected_Date <> "" And lbDepts.Items.Count > 0 Then
            Get_Records(False)
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Private Sub Get_Records(ByVal update As Boolean)
        Lit1.Text = ""
        If lbDepts.Items.Count > 0 Then
            'Lit1.Text &= IDs
            Dim cn As Object
            Dim rs As Object
            Dim rs2 As Object
            cn = Server.CreateObject("ADODB.ConnectiON")
            rs = Server.CreateObject("ADODB.Recordset")
            rs2 = Server.CreateObject("ADODB.Recordset")
            cn.commandtimeout = 10000
            cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Dim dbName As String = Resources.Resource.DATABASE
            Dim IDs As String = ""
            If rblFilter.SelectedValue = "Department" Then
                For i = 0 To lbDepts.Items.Count - 1
                    IDs &= IIf(IDs = "", "'" & Replace(lbDepts.Items(i).Text, "'", "''") & "'", ",'" & Replace(lbDepts.Items(i).Text, "'", "''") & "'")
                Next
            ElseIf rblFilter.SelectedValue = "Employee" Then
                For i = 0 To lbDepts.Items.Count - 1
                    IDs &= IIf(IDs = "", lbDepts.Items(i).Value, "," & lbDepts.Items(i).Value)
                Next
            Else
                For i = 0 To lbDepts.Items.Count - 1
                    IDs &= IIf(IDs = "", "'" & lbDepts.Items(i).Text & "'", ",'" & lbDepts.Items(i).Text & "'")
                Next
            End If
            If IDs = "" Then IDs = "0"
            If rblFilter.SelectedValue = "Department" Then
                If IDs <> "0" And IDs <> "ALL" Then
                    If cbInactive.Checked Then
                        rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where (timein1 is not null or timeout1 is not null or timein2 is not null or timeout2 is not null) and department in (" & IDs & ") order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                        'rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where personnelid in (Select personnelid from [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where department in (" & IDs & ") and (timein1 is not null or timeout1 is not null or timein2 is not null or timeout2 is not null)) order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                    Else
                        rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where department in (" & IDs & ") order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                    End If
                Else
                    If cbInactive.Checked Then
                        rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where (timein1 is not null or timeout1 is not null or timein2 is not null or timeout2 is not null) order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                        'rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where personnelid in (Select personnelid from FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where (timein1 is not null or timeout1 is not null or timein2 is not null or timeout2 is not null)) order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                    Else
                        rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                    End If
                End If
            ElseIf rblFilter.SelectedValue = "Company" Then
                If IDs <> "0" And IDs <> "ALL" Then
                    If cbInactive.Checked Then
                        rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where (timein1 is not null or timeout1 is not null or timein2 is not null or timeout2 is not null) and company in (" & IDs & ") order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                        'rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where personnelid in (Select Personnelid FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where (timein1 is not null or timeout1 is not null or timein2 is not null or timeout2 is not null) and company in (" & IDs & ")) order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                    Else
                        rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where company in (" & IDs & ") order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                    End If
                Else
                    If cbInactive.Checked Then
                        rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where (timein1 is not null or timeout1 is not null or timein2 is not null or timeout2 is not null) order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                        'rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where personnelid in (Select personnelid FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where (timein1 is not null or timeout1 is not null or timein2 is not null or timeout2 is not null)) order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                    Else
                        rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                    End If
                End If
            ElseIf rblFilter.SelectedValue = "Employee" Then
                If IDs <> "0" And IDs <> "ALL" Then
                    If cbInactive.Checked Then
                        rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where (timein1 is not null or timeout1 is not null or timein2 is not null or timeout2 is not null) and personnelid in (" & IDs & ") order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                        'rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where personnelid in (Select personnelid from FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where (timein1 is not null or timeout1 is not null or timein2 is not null or timeout2 is not null and personnelid in (" & IDs & ")) order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                    Else
                        rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where personnelid in (" & IDs & ") order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                    End If
                Else
                    If cbInactive.Checked Then
                        rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where (timein1 is not null or timeout1 is not null or timein2 is not null or timeout2 is not null) order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                        'rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where personnelid in (Select personnelid from FROM [" & dbName & "].[dbo].[ufn_PunchDetail_All] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') where (timein1 is not null or timeout1 is not null or timein2 is not null or timeout2 is not null)) order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                    Else
                        rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
                    End If
                End If
            Else
                rs.open("SELECT ROUND(DIFF1/60,1) As Diff1Rd, ROUND(DIFF2/60,1) As Diff2Rd, * FROM [" & dbName & "].[dbo].[ufn_PunchDetail] ('" & dfSDate.Selected_Date & "','" & dfEDate.Selected_Date & "') order by company, department, lastname, firstname, DATEIN, TIMEIN1", cn, 0, 1)
            End If
            Dim div = Replace(Request("div"), "||", " ")
            If update Then Lit1.Text &= (div & "|&&|Replaced")
            If rs.eof And rs.bof Then
                Lit1.Text &= ("No Records To Report")
            Else
                Dim sLastname = ""
                Dim sFirstname = ""
                Dim company = ""
                Dim department = ""
                Dim gtotal(2) As Decimal
                Dim ptotal(2) As Decimal
                Dim ctotal(2) As Decimal
                Dim dtotal(2) As Decimal
                gtotal(0) = 0
                ptotal(0) = 0
                ctotal(0) = 0
                dtotal(0) = 0
                gtotal(1) = 0
                ptotal(1) = 0
                ctotal(1) = 0
                dtotal(1) = 0
                gtotal(2) = 0
                ptotal(2) = 0
                ctotal(2) = 0
                dtotal(2) = 0
                Dim personnelid = 0
                Dim i = 0
                Dim divid = ""
                Dim pPALTotal As Decimal = 0
                Dim pJURYTotal As Decimal = 0
                Dim pBERTotal As Decimal = 0
                Dim pSSLBTotal As Decimal = 0
                Dim pWORKTotal As Decimal = 0
                Dim pOTTotal As Decimal = 0
                Dim dPALTotal As Decimal = 0
                Dim dJURYTotal As Decimal = 0
                Dim dBERTotal As Decimal = 0
                Dim dSSLBTotal As Decimal = 0
                Dim dWORKTotal As Decimal = 0
                Dim dOTTotal As Decimal = 0
                Dim cPALTotal As Decimal = 0
                Dim cJURYTotal As Decimal = 0
                Dim cBERTotal As Decimal = 0
                Dim cSSLBTotal As Decimal = 0
                Dim cWORKTotal As Decimal = 0
                Dim cOTTotal As Decimal = 0
                Dim gPALTotal As Decimal = 0
                Dim gJURYTotal As Decimal = 0
                Dim gBERTotal As Decimal = 0
                Dim gSSLBTotal As Decimal = 0
                Dim gWORKTotal As Decimal = 0
                Dim gOTTotal As Decimal = 0
                Dim tempDate As Date = rs.Fields("DateIn").value
                If cbInactive.Checked = True Then
                    tempDate = dfSDate.Selected_Date
                End If
                Dim tempHours As Decimal = 0
                Dim bNew = False
                Dim timeinsplit
                Dim timeoutsplit
                Do While Not rs.eof
                    If DateDiff("d", tempDate, rs.Fields("DateIn").value) >= 7 Then
                        If tempHours > 40 Then
                            pOTTotal = Math.Round(pOTTotal + Math.Round((tempHours - 40), 1), 1)
                            dOTTotal = Math.Round(dOTTotal + Math.Round((tempHours - 40), 1), 1)
                            cOTTotal = Math.Round(cOTTotal + Math.Round((tempHours - 40), 1), 1)
                            gOTTotal = Math.Round(gOTTotal + Math.Round((tempHours - 40), 1), 1)
                        End If
                        tempHours = 0
                        If cbInactive.Checked Then
                            tempDate = tempDate.AddDays(7)
                            If DateDiff("d", dfSDate.Selected_Date, rs.Fields("DateIn").value) >= 7 Then
                                Do While DateDiff("d", tempDate, rs.Fields("DateIn").value) > 0
                                    tempDate = tempDate.AddDays(7)
                                Loop
                            End If
                        Else
                            tempDate = rs.Fields("DateIn").value
                        End If
                    End If
                    bNew = False
                    If sLastname <> rs.fields("lastname").value & "" Then bNew = True
                    If sFirstname <> rs.fields("firstname").value & "" Then bNew = True
                    If company <> rs.fields("Company").value & "" Then bNew = True
                    If department <> rs.fields("Department").value & "" Then bNew = True
                    If personnelid <> rs.fields("PersonnelID").value Then bNew = True

                    If bNew And i <> 0 Then
                        'write totals
                        If company <> rs.fields("Company").value & "" Then
                            'Personnel Totals
                            If tempHours > 40 Then
                                pOTTotal = Math.Round(pOTTotal + Math.Round((tempHours - 40), 1), 1)
                                dOTTotal = Math.Round(dOTTotal + Math.Round((tempHours - 40), 1), 1)
                                cOTTotal = Math.Round(cOTTotal + Math.Round((tempHours - 40), 1), 1)
                                gOTTotal = Math.Round(gOTTotal + Math.Round((tempHours - 40), 1), 1)
                            End If
                            If divid = div Or Not (update) Then
                                Lit1.Text &= ("<tr><th colspan = 4 align=left>Totals for " & sLastname & ", " & sFirstname & "</th>")
                                Lit1.Text &= ("<th>" & FormatNumber(Math.Round(ptotal(0), 1), 1) & "</th>")
                                Lit1.Text &= ("<th colspan = 3>&nbsp;</th>")
                                Lit1.Text &= ("<th>" & FormatNumber(Math.Round(ptotal(1), 1), 1) & "</th>")
                                Lit1.Text &= ("<th>" & FormatNumber(Math.Round(ptotal(0) + ptotal(1), 1), 1) & "</th>")
                                'HR took out due to rounding discrepancies 4-15-2009
                                'lit1.text &=  "<th>" & ptotal(2) & "</th>"
                                Lit1.Text &= ("</tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>PAL:</th><th>" & Math.Round(pPALTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>JURY DUTY:</th><th>" & Math.Round(pJURYTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>BEREAVEMENT:</th><th>" & Math.Round(pBERTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>SSLB:</th><th>" & Math.Round(pSSLBTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>REGULAR:</th><th> " & Math.Round(pWORKTotal - pOTTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>OVERTIME:</th><th> " & Math.Round(pOTTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><td colspan = 8 align=left>&nbsp;</td></tr>")
                                Lit1.Text &= ("<tr><td colspan = 8 align=left>&nbsp;</td></tr>")
                            End If
                            ptotal(0) = 0
                            ptotal(1) = 0
                            ptotal(2) = 0
                            pPALTotal = 0
                            pJURYTotal = 0
                            pBERTotal = 0
                            pSSLBTotal = 0
                            pWORKTotal = 0
                            pOTTotal = 0
                            tempHours = 0
                            If cbInactive.Checked Then
                                tempDate = dfSDate.Selected_Date
                                If DateDiff("d", dfSDate.Selected_Date, rs.Fields("DateIn").value) >= 7 Then
                                    Do While DateDiff("d", tempDate, rs.Fields("DateIn").value) > 0
                                        tempDate = tempDate.AddDays(7)
                                    Loop
                                End If
                            Else
                                tempDate = rs.Fields("DateIn").value
                            End If
                            'Department Totals
                            If divid = div Or Not (update) Then
                                Lit1.Text &= ("<tr><th colspan = 4 align=left>" & department & " Department Totals</th >")
                                Lit1.Text &= ("<th>" & FormatNumber(Math.Round(dtotal(0), 1), 1) & "</th>")
                                Lit1.Text &= ("<th colspan = 3>&nbsp;</th>")
                                Lit1.Text &= ("<th>" & FormatNumber(Math.Round(dtotal(1), 1), 1) & "</th>")
                                Lit1.Text &= ("<th>" & FormatNumber(Math.Round(dtotal(0) + dtotal(1), 1), 1) & "</th>")
                                'HR took out due to rounding discrepancies 4-15-2009
                                'lit1.text &=  "<th>" & dtotal(2) & "</th>"
                                Lit1.Text &= ("</tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>PAL:</th><th>" & Math.Round(dPALTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>JURY DUTY:</th><th>" & Math.Round(dJURYTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>BEREAVEMENT:</th><th>" & Math.Round(dBERTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>SSLB:</th><th>" & Math.Round(dSSLBTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>REGULAR:</th><th> " & Math.Round(dWORKTotal - dOTTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>OVERTIME:</th><th> " & Math.Round(dOTTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><td colspan = 8 align=left>&nbsp;</td></tr>")
                            End If
                            dtotal(0) = 0
                            dtotal(1) = 0
                            dtotal(2) = 0
                            dPALTotal = 0
                            dJURYTotal = 0
                            dBERTotal = 0
                            dSSLBTotal = 0
                            dWORKTotal = 0
                            dOTTotal = 0

                            'Company Totals
                            If divid = div Or Not (update) Then
                                Lit1.Text &= ("<tr><th colspan = 4 align=left>" & company & " Company Totals</th >")
                                Lit1.Text &= ("<th>" & FormatNumber(ctotal(0), 1) & "</th>")
                                Lit1.Text &= ("<th colspan = 3>&nbsp;</th>")
                                Lit1.Text &= ("<th>" & FormatNumber(ctotal(1), 1) & "</th>")
                                Lit1.Text &= ("<th>" & FormatNumber(ctotal(0) + ctotal(1), 1) & "</th>")
                                'HR took out due to rounding discrepancies 4-15-2009
                                'lit1.text &=  "<th>" & ctotal(2) & "</th>"
                                Lit1.Text &= ("</tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>PAL:</th><th>" & cPALTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>JURY DUTY:</th><th>" & cJURYTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>BEREAVEMENT:</th><th>" & cBERTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>SSLB:</th><th>" & cSSLBTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>REGULAR:</th><th> " & Math.Round(cWORKTotal - cOTTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>OVERTIME:</th><th> " & cOTTotal & "</th></tr>")

                                Lit1.Text &= ("</table>")
                                Lit1.Text &= ("<table width='100%'>")
                            End If
                            ctotal(0) = 0
                            ctotal(1) = 0
                            ctotal(2) = 0
                            cPALTotal = 0
                            cJURYTotal = 0
                            cBERTotal = 0
                            cSSLBTotal = 0
                            cWORKTotal = 0
                            cOTTotal = 0

                            If divid = div Or Not (update) Then
                                Lit1.Text &= ("<tr><th colspan = 8 align=left>" & rs.fields("Company").value & "</th ></tr>")
                                Lit1.Text &= ("<tr><th colspan = 8 align=left>" & rs.fields("Department").value & "</th ></tr>")

                                Lit1.Text &= ("</table></div>")
                            End If
                            divid = rs.fields("Company").value & "|" & rs.fields("Department").value & "|" & rs.fields("PersonnelID").value
                            If divid = div Or Not (update) Then
                                Lit1.Text &= ("<div id='" & rs.fields("Company").value & "|" & rs.fields("Department").value & "|" & rs.fields("PersonnelID").value & "'><table width='100%'>")
                                Lit1.Text &= ("<tr><th colspan = 8 align=left>" & rs.fields("Lastname").value & ", " & rs.fields("firstname").value & "</th ></tr>")
                                Lit1.Text &= ("<tr><th>Date</th><th>Time In</th><th>Time Out</th><th>Type</th><th>Sub Total</th><th>Time In</th><th>Time Out</th><th>Type</th><th>Sub Total</th><th>Total</th></tr>")
                            End If
                        ElseIf department <> rs.fields("Department").value & "" Then
                            'Personnel Totals
                            If tempHours > 40 Then
                                pOTTotal = Math.Round(pOTTotal + Math.Round((tempHours - 40), 1), 1)
                                dOTTotal = Math.Round(dOTTotal + Math.Round((tempHours - 40), 1), 1)
                                cOTTotal = Math.Round(cOTTotal + Math.Round((tempHours - 40), 1), 1)
                                gOTTotal = Math.Round(gOTTotal + Math.Round((tempHours - 40), 1), 1)
                            End If
                            If divid = div Or Not (update) Then
                                Lit1.Text &= ("<tr><th colspan = 4 align=left>Totals for " & sLastname & ", " & sFirstname & "</th >")
                                Lit1.Text &= ("<th>" & ptotal(0) & "</th>")
                                Lit1.Text &= ("<th colspan = 3>&nbsp;</th>")
                                Lit1.Text &= ("<th>" & ptotal(1) & "</th>")
                                Lit1.Text &= ("<th>" & ptotal(0) + ptotal(1) & "</th>")
                                'HR took out due to rounding discrepancies 4-15-2009						
                                'lit1.text &=  "<th>" & ptotal(2) & "</th>"
                                Lit1.Text &= ("</tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>PAL:</th><th>" & pPALTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>JURY DUTY:</th><th>" & pJURYTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>BEREAVEMENT:</th><th>" & pBERTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>SSLB:</th><th>" & pSSLBTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>REGULAR:</th><th> " & Math.Round(pWORKTotal - pOTTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>OVERTIME:</th><th> " & Math.Round(pOTTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><td colspan = 8 align=left>&nbsp;</td></tr>")
                                Lit1.Text &= ("<tr><td colspan = 8 align=left>&nbsp;</td></tr>")
                            End If
                            ptotal(0) = 0
                            ptotal(1) = 0
                            ptotal(2) = 0
                            pPALTotal = 0
                            pJURYTotal = 0
                            pBERTotal = 0
                            pSSLBTotal = 0
                            pWORKTotal = 0
                            pOTTotal = 0
                            tempHours = 0
                            If cbInactive.Checked Then
                                tempDate = dfSDate.Selected_Date
                                If DateDiff("d", dfSDate.Selected_Date, rs.Fields("DateIn").value) >= 7 Then
                                    Do While DateDiff("d", tempDate, rs.Fields("DateIn").value) > 0
                                        tempDate = tempDate.AddDays(7)
                                    Loop
                                End If
                            Else
                                tempDate = rs.Fields("DateIn").value
                            End If
                            'Department Totals
                            If divid = div Or Not (update) Then
                                Lit1.Text &= ("<tr><th colspan = 4  align=left>" & department & " Department Totals</th >")
                                Lit1.Text &= ("<th>" & dtotal(0) & "</th>")
                                Lit1.Text &= ("<th colspan = 3>&nbsp;</th>")
                                Lit1.Text &= ("<th>" & dtotal(1) & "</th>")
                                Lit1.Text &= ("<th>" & dtotal(0) + dtotal(1) & "</th>")
                                'HR took out due to rounding discrepancies 4-15-2009						
                                'lit1.text &=  "<th>" & dtotal(2) & "</th>"
                                Lit1.Text &= ("</tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>PAL:</th><th>" & dPALTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>JURY DUTY:</th><th>" & dJURYTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>BEREAVEMENT:</th><th>" & dBERTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>SSLB:</th><th>" & dSSLBTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>REGULAR:</th><th> " & Math.Round(dWORKTotal - dOTTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>OVERTIME:</th><th> " & dOTTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><td colspan = 8 align=left>&nbsp;</td></tr>")
                            End If
                            dtotal(0) = 0
                            dtotal(1) = 0
                            dtotal(2) = 0
                            dPALTotal = 0
                            dJURYTotal = 0
                            dBERTotal = 0
                            dSSLBTotal = 0
                            dWORKTotal = 0
                            dOTTotal = 0

                            If divid = div Or Not (update) Then
                                Lit1.Text &= ("<tr><th colspan = 8 align=left>" & rs.fields("Department").value & "</th></tr>")

                                Lit1.Text &= ("</table></div>")
                            End If
                            divid = rs.fields("Company").value & "|" & rs.fields("Department").value & "|" & rs.fields("PersonnelID").value
                            If divid = div Or Not (update) Then
                                Lit1.Text &= ("<div id='" & rs.fields("Company").value & "|" & rs.fields("Department").value & "|" & rs.fields("PersonnelID").value & "'><table width='100%'>")
                                Lit1.Text &= ("<tr><th colspan = 8 align=left>" & rs.fields("Lastname").value & ", " & rs.fields("firstname").value & "</th ></tr>")
                                Lit1.Text &= ("<tr><th>Date</th><th>Time In</th><th>Time Out</th><th>Type</th><th>Sub Total</th><th>Time In</th><th>Time Out</th><th>Type</th><th>Sub Total</th><th>Total</th></tr>")
                            End If

                        ElseIf sLastname <> rs.fields("lastname").value & "" Or sFirstname <> rs.fields("firstname").value & "" Or personnelid <> rs.fields("PersonnelID").value Then
                            'Personnel Totals
                            If tempHours > 40 Then
                                pOTTotal = Math.Round(pOTTotal + Math.Round((tempHours - 40), 1), 1)
                                dOTTotal = Math.Round(dOTTotal + Math.Round((tempHours - 40), 1), 1)
                                cOTTotal = Math.Round(cOTTotal + Math.Round((tempHours - 40), 1), 1)
                                gOTTotal = Math.Round(gOTTotal + Math.Round((tempHours - 40), 1), 1)
                            End If
                            If divid = div Or Not (update) Then
                                Lit1.Text &= ("<tr><th colspan = 4 align=left>Totals for " & sLastname & ", " & sFirstname & "</th >")
                                Lit1.Text &= ("<th>" & ptotal(0) & "</th>")
                                Lit1.Text &= ("<th colspan = 3>&nbsp;</th>")
                                Lit1.Text &= ("<th>" & ptotal(1) & "</th>")
                                Lit1.Text &= ("<th>" & ptotal(0) + ptotal(1) & "</th>")
                                'HR took out due to rounding discrepancies 4-15-2009
                                'lit1.text &=  "<th>" & ptotal(2) & "</th>"
                                Lit1.Text &= ("</tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>PAL:</th><th>" & pPALTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>JURY DUTY:</th><th>" & pJURYTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>BEREAVEMENT:</th><th>" & pBERTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>SSLB:</th><th>" & pSSLBTotal & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>REGULAR:</th><th> " & Math.Round(pWORKTotal - pOTTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><th colspan = 7></th>")
                                Lit1.Text &= ("<th colspan = 2 align = right>OverTime:</th><th> " & Math.Round(pOTTotal, 1) & "</th></tr>")
                                Lit1.Text &= ("<tr><td colspan = 8 align=left>&nbsp;</td></tr>")
                                Lit1.Text &= ("<tr><td colspan = 8 align=left>&nbsp;</td></tr>")
                                Lit1.Text &= ("</table></div>")
                            End If
                            divid = rs.fields("Company").value & "|" & rs.fields("Department").value & "|" & rs.fields("PersonnelID").value
                            If divid = div Or Not (update) Then
                                Lit1.Text &= ("<div id='" & rs.fields("Company").value & "|" & rs.fields("Department").value & "|" & rs.fields("PersonnelID").value & "'><table width='100%'>")
                                Lit1.Text &= ("<tr><th colspan = 8 align=left>" & rs.fields("Lastname").value & ", " & rs.fields("firstname").value & "</th ></tr>")
                                Lit1.Text &= ("<tr><th>Date</th><th>Time In</th><th>Time Out</th><th>Type</th><th>Sub Total</th><th>Time In</th><th>Time Out</th><th>Type</th><th>Sub Total</th><th>Total</th></tr>")
                            End If
                            ptotal(0) = 0
                            ptotal(1) = 0
                            ptotal(2) = 0
                            pPALTotal = 0
                            pJURYTotal = 0
                            pBERTotal = 0
                            pSSLBTotal = 0
                            pWORKTotal = 0
                            pOTTotal = 0
                            tempHours = 0
                            If cbInactive.Checked Then
                                tempDate = dfSDate.Selected_Date
                                If DateDiff("d", dfSDate.Selected_Date, rs.Fields("DateIn").value) >= 7 Then
                                    Do While DateDiff("d", tempDate, rs.Fields("DateIn").value) > 0
                                        tempDate = tempDate.AddDays(7)
                                    Loop
                                End If
                            Else
                                tempDate = rs.Fields("DateIn").value
                            End If
                        End If

                    ElseIf bNew And i = 0 Then
                        i = i + 1
                        If divid = div Or Not (update) Then
                            Lit1.Text &= ("<table width='100%'>")
                            Lit1.Text &= ("<tr><th colspan = 8 align=left>" & rs.fields("Company").value & "</th >")
                            Lit1.Text &= ("<tr><th colspan = 8 align=left>" & rs.fields("Department").value & "</th >")
                            Lit1.Text &= ("</table>")
                        End If
                        divid = rs.fields("Company").value & "|" & rs.fields("Department").value & "|" & rs.fields("PersonnelID").value
                        If divid = div Or Not (update) Then
                            Lit1.Text &= ("<div id='" & rs.fields("Company").value & "|" & rs.fields("Department").value & "|" & rs.fields("PersonnelID").value & "'><table width='100%'>")
                            Lit1.Text &= ("<tr><th colspan = 8 align=left>" & rs.fields("Lastname").value & ", " & rs.fields("firstname").value & "</th >")
                            Lit1.Text &= ("<tr><th>Date</th><th>Time In</th><th>Time Out</th><th>Type</th><th>Sub Total</th><th>Time In</th><th>Time Out</th><th>Type</th><th>Sub Total</th><th>Total</th></tr>")
                        End If
                    End If
                    company = rs.fields("company").value & ""
                    department = rs.fields("department").value & ""
                    sLastname = rs.fields("lastname").value & ""
                    sFirstname = rs.fields("firstname").value & ""
                    personnelid = rs.fields("PersonnelID").value

                    If divid = div Or Not (update) Then
                        Lit1.Text &= ("<tr>")
                        Lit1.Text &= ("<td>" & rs.fields("DateIn").value & "</td>")
                        If rs.fields("TimeIn1").value & "" <> "" Then
                            rs2.Open("Select * from t_PersonnelMissedPunch where punchID = " & rs.Fields("PunchID1").value & " and PunchIn = 1 order by missedpunchid desc", cn, 0, 1)
                            timeinsplit = Split(rs.Fields("TimeIn1").value, " ")
                            Lit1.Text &= ("<td align = center>")
                            If (UBound(timeinsplit) = 0) Then
                                If rs2.EOF And rs2.BOF Then
                                    Lit1.Text &= ("12:00:00 AM")
                                Else
                                    If rs2.FIelds("ManagerApproved").Value = 0 And rs2.Fields("HRApproved").Value = 0 Then
                                        Lit1.Text &= ("<B><I><font color = 'red'>12:00:00 AM**</font></I></B>")
                                    ElseIf rs2.Fields("ManagerApproved").Value = 1 Or rs2.FIelds("HRApproved").Value = 1 Then
                                        Lit1.Text &= ("12:00:00 AM**")
                                    Else
                                        Lit1.Text &= ("12:00:00 AM")
                                    End If
                                End If
                            Else
                                If rs2.EOF And rs2.BOF Then
                                    'Lit1.Text &= ("<a href=" & Chr(32) & "javascript:void(Edit_Punch('" & personnelid & "','" & rs.fields("Datein").value & "',1,'" & Replace(Right(rs.Fields("Timein1").value, Len(CStr(rs.fields("Timein1").value) & "") - InStr(CStr(rs.fields("TimeIn1").value) & "", " ") + 0), " ", ":") & "','" & Replace(rs.fields("Company").value & "|" & rs.fields("Department").value & "|" & rs.fields("PersonnelID").value, " ", "||") & "'));" & Chr(32) & ">")
                                    Lit1.Text &= (Right(rs.Fields("Timein1").value, Len(CStr(rs.fields("Timein1").value) & "") - InStr(CStr(rs.fields("TimeIn1").value) & "", " ")))
                                    'Lit1.Text &= ("</a>")
                                Else
                                    If rs2.Fields("ManagerApproved").Value = 0 And rs2.Fields("HRApproved").Value = 0 Then
                                        'Lit1.Text &= ("<B><I><font color = 'red'><a href=" & Chr(32) & "javascript:void(Edit_Punch('" & personnelid & "','" & rs.fields("Datein").value & "',1,'" & Replace(Right(rs.Fields("Timein1").value, Len(CStr(rs.fields("Timein1").value) & "") - InStr(CStr(rs.fields("TimeIn1").value) & "", " ") + 0), " ", ":") & "','" & Replace(rs.fields("Company").value & "|" & rs.fields("Department").value & "|" & rs.fields("PersonnelID").value, " ", "||") & "'));" & Chr(32) & ">")
                                        Lit1.Text &= ("<B><I><font color = 'red'>" & Right(rs.Fields("Timein1").value, Len(CStr(rs.fields("Timein1").value) & "") - InStr(CStr(rs.fields("TimeIn1").value) & "", " ")) & "**</font></I></b>")
                                        'Lit1.Text &= ("**</a></font></I></B>")
                                    ElseIf rs2.Fields("ManagerApproved").Value = 1 Or rs2.Fields("HRApproved").Value = 1 Then
                                        'Lit1.Text &= ("<a href=" & Chr(32) & "javascript:void(Edit_Punch('" & personnelid & "','" & rs.fields("Datein").value & "',1,'" & Replace(Right(rs.Fields("Timein1").value, Len(CStr(rs.fields("Timein1").value) & "") - InStr(CStr(rs.fields("TimeIn1").value) & "", " ") + 0), " ", ":") & "','" & Replace(rs.fields("Company").value & "|" & rs.fields("Department").value & "|" & rs.fields("PersonnelID").value, " ", "||") & "'));" & Chr(32) & ">")
                                        Lit1.Text &= (Right(rs.Fields("Timein1").value, Len(CStr(rs.fields("Timein1").value) & "") - InStr(CStr(rs.fields("TimeIn1").value) & "", " ")) & "**")
                                        'Lit1.Text &= ("**</a>")
                                    Else
                                        'Lit1.Text &= ("<a href=" & Chr(32) & "javascript:void(Edit_Punch('" & personnelid & "','" & rs.fields("Datein").value & "',1,'" & Replace(Right(rs.Fields("Timein1").value, Len(CStr(rs.fields("Timein1").value) & "") - InStr(CStr(rs.fields("TimeIn1").value) & "", " ") + 0), " ", ":") & "','" & Replace(rs.fields("Company").value & "|" & rs.fields("Department").value & "|" & rs.fields("PersonnelID").value, " ", "||") & "'));" & Chr(32) & ">")
                                        Lit1.Text &= (Right(rs.Fields("Timein1").value, Len(CStr(rs.fields("Timein1").value) & "") - InStr(CStr(rs.fields("TimeIn1").value) & "", " ")))
                                        'Lit1.Text &= ("</a>")
                                    End If
                                End If
                            End If

                            Lit1.Text &= ("</td>")
                            rs2.Close()

                        Else
                            Lit1.Text &= ("<td align = center>")
                            'Lit1.Text &= ("<a href=" & Chr(32) & "javascript:void(Edit_Punch('" & personnelid & "','" & rs.fields("Datein").value & "',1,'N/A','" & Replace(rs.fields("Company").value & "|" & rs.fields("Department").value & "|" & rs.fields("PersonnelID").value, " ", "||") & "'));" & Chr(32) & ">")
                            Lit1.Text &= ("N/A")
                            'Lit1.Text &= ("</a>")
                            Lit1.Text &= ("</td>")
                        End If
                        If rs.fields("TimeOut1").value & "" <> "" Then
                            rs2.Open("Select * from t_PersonnelMissedPunch where punchID = " & rs.Fields("PunchID1").value & " and PunchIn = 0 order by missedpunchid desc", cn, 0, 1)

                            timeoutsplit = Split(rs.Fields("TimeOut1").value, " ")
                            If (UBound(timeoutsplit) = 0) Then
                                If rs2.EOF And rs2.BOF Then
                                    Lit1.Text &= ("<td align = center>12:00:00 AM</td>")
                                Else
                                    If rs2.Fields("HRApproved").Value = 0 And rs2.FIelds("ManagerApproved").Value = 0 Then
                                        Lit1.Text &= ("<td align = center><B><I><font color = 'Red'>12:00:00 AM</font></I></B></td>")
                                    ElseIf rs2.Fields("HRApproved").Value = 1 Or rs2.Fields("ManagerApproved").Value = 1 Then
                                        Lit1.Text &= ("<td align = center>12:00:00 AM**</td>")
                                    Else
                                        Lit1.Text &= ("<td align = center>12:00:00 AM</td>")
                                    End If
                                End If
                            Else
                                If rs2.EOF And rs2.BOF Then
                                    Lit1.Text &= ("<td align = center>" & Right(rs.Fields("TimeOut1").value, Len(CStr(rs.fields("TimeOut1").value) & "") - InStr(CStr(rs.fields("TimeOut1").value) & "", " ")) & "</td>")
                                Else
                                    If rs2.Fields("HRApproved").Value = 0 And rs2.FIelds("ManagerApproved").Value = 0 Then
                                        Lit1.Text &= ("<td align = center><B><I><font color = 'Red'>" & Right(rs.Fields("TimeOut1").value, Len(CStr(rs.fields("TimeOut1").value) & "") - InStr(CStr(rs.fields("TimeOut1").value) & "", " ")) & "**</font></I></B></td>")
                                    ElseIf rs2.Fields("HRApproved").Value = 1 Or rs2.Fields("ManagerApproved").Value = 1 Then
                                        Lit1.Text &= ("<td align = center>" & Right(rs.Fields("TimeOut1").value, Len(CStr(rs.fields("TimeOut1").value) & "") - InStr(CStr(rs.fields("TimeOut1").value) & "", " ")) & "**</td>")
                                    Else
                                        Lit1.Text &= ("<td align = center>" & Right(rs.Fields("TimeOut1").value, Len(CStr(rs.fields("TimeOut1").value) & "") - InStr(CStr(rs.fields("TimeOut1").value) & "", " ")) & "</td>")
                                    End If
                                End If
                            End If
                            rs2.Close()
                        Else
                            Lit1.Text &= ("<td align = center>N/A</td>")
                        End If
                        If rs.Fields("PunchType").value & "" <> "" Then
                            Lit1.Text &= ("<td align = center>" & rs.Fields("PunchType").value & "</td>")
                        Else
                            Lit1.Text &= ("<td align = center>N/A</td>")
                        End If
                    End If
                    If rs.fields("Diff1Rd").value & "" <> "" Then
                        If divid = div Or Not (update) Then Lit1.Text &= "<td align = center>" & Math.Round(rs.fields("Diff1Rd").value, 1) & "</td>"
                        ptotal(0) = ptotal(0) + rs.fields("Diff1Rd").value
                        dtotal(0) = dtotal(0) + rs.fields("Diff1Rd").value
                        ctotal(0) = ctotal(0) + rs.fields("Diff1Rd").value
                        gtotal(0) = gtotal(0) + rs.fields("Diff1Rd").value
                        If rs.Fields("PunchType").value & "" = "PAL" Then
                            pPALTotal = pPALTotal + rs.fields("Diff1Rd").value
                            dPALTotal = dPALTotal + rs.fields("Diff1Rd").value
                            cPALTotal = cPALTotal + rs.fields("Diff1Rd").value
                            gPALTotal = gPALTotal + rs.fields("Diff1Rd").value
                        ElseIf rs.Fields("PunchType").value & "" = "Jury Duty" Then
                            pJURYTotal = pJURYTotal + rs.fields("Diff1Rd").value
                            dJURYTotal = dJURYTotal + rs.fields("Diff1Rd").value
                            cJURYTotal = cJURYTotal + rs.fields("Diff1Rd").value
                            gJURYTotal = gJURYTotal + rs.fields("Diff1Rd").value
                        ElseIf rs.Fields("PunchType").value & "" = "Bereavement" Then
                            pBERTotal = pBERTotal + rs.fields("Diff1Rd").value
                            dBERTotal = dBERTotal + rs.fields("Diff1Rd").value
                            cBERTotal = cBERTotal + rs.fields("Diff1Rd").value
                            gBERTotal = gBERTotal + rs.fields("Diff1Rd").value
                        ElseIf rs.Fields("PunchType").value & "" = "SSLB" Then
                            pSSLBTotal = pSSLBTotal + rs.fields("Diff1Rd").value
                            dSSLBTotal = dSSLBTotal + rs.fields("Diff1Rd").value
                            cSSLBTotal = cSSLBTotal + rs.fields("Diff1Rd").value
                            gSSLBTotal = gSSLBTotal + rs.fields("Diff1Rd").value
                        Else
                            pWORKTotal = pWORKTotal + rs.fields("Diff1Rd").value
                            dWORKTotal = dWORKTotal + rs.fields("Diff1Rd").value
                            cWORKTotal = cWORKTotal + rs.fields("Diff1Rd").value
                            gWORKTotal = gWORKTotal + rs.fields("Diff1Rd").value
                            tempHours = tempHours + rs.fields("Diff1Rd").value
                        End If
                    Else
                        If divid = div Or Not (update) Then Lit1.Text &= ("<td align = center>0</td>")
                    End If
                    If divid = div Or Not (update) Then
                        If rs.fields("TimeIn2").value & "" <> "" Then
                            rs2.Open("Select * from t_PersonnelMissedPunch where punchID = " & rs.Fields("PunchID2").value & " and PunchIn = 1 order by missedpunchid desc", cn, 0, 1)

                            timeinsplit = Split(rs.Fields("TimeIn2").value, " ")
                            If (UBound(timeinsplit) = 0) Then
                                If rs2.EOF And rs2.BOF Then
                                    Lit1.Text &= ("<td align = 'center'>12:00:00 AM</td>")
                                Else
                                    If rs2.Fields("HRApproved").Value = 0 And rs2.FIelds("ManagerApproved").Value = 0 Then
                                        Lit1.Text &= ("<td align = 'center'><B><I><font color = 'Red'>12:00:00 AM**</font></I></B></td>")
                                    ElseIf rs2.Fields("HRApproved").Value = 1 Or rs2.Fields("ManagerApproved").Value = 1 Then
                                        Lit1.Text &= ("<td align = 'center'>12:00:00 AM**</td>")
                                    Else
                                        Lit1.Text &= ("<td align = 'center'>12:00:00 AM</td>")
                                    End If
                                End If
                            Else
                                If rs2.EOF And rs2.BOF Then
                                    Lit1.Text &= ("<td align = center>" & Right(rs.Fields("TimeIn2").value, Len(CStr(rs.fields("TimeIn2").value) & "") - InStr(CStr(rs.fields("TimeIn2").value) & "", " ")) & "</td>")
                                Else
                                    If rs2.Fields("HRApproved").Value = 0 And rs2.FIelds("ManagerApproved").Value = 0 Then
                                        Lit1.Text &= ("<td align = center><B><I><font color = 'Red'>" & Right(rs.Fields("TimeIn2").value, Len(CStr(rs.fields("TimeIn2").value) & "") - InStr(CStr(rs.fields("TimeIn2").value) & "", " ")) & "**</font></I></B></td>")
                                    ElseIf rs2.Fields("HRApproved").Value = 1 Or rs2.Fields("ManagerApproved").Value = 1 Then
                                        Lit1.Text &= ("<td align = center>" & Right(rs.Fields("TimeIn2").value, Len(CStr(rs.fields("TimeIn2").value) & "") - InStr(CStr(rs.fields("TimeIn2").value) & "", " ")) & "**</td>")
                                    Else
                                        Lit1.Text &= ("<td align = center>" & Right(rs.Fields("TimeIn2").value, Len(CStr(rs.fields("TimeIn2").value) & "") - InStr(CStr(rs.fields("TimeIn2").value) & "", " ")) & "</td>")
                                    End If
                                End If
                            End If
                            rs2.Close()
                        Else
                            Lit1.Text &= ("<td align = center>N/A</td>")
                        End If
                        If rs.fields("TimeOut2").value & "" <> "" Then
                            rs2.Open("Select * from t_PersonnelMissedPunch where punchID = " & rs.Fields("PunchID2").value & " and PunchIn = 0 order by missedpunchid desc", cn, 0, 1)
                            timeoutsplit = Split(rs.Fields("TimeOut2").value, " ")
                            If (UBound(timeoutsplit) = 0) Then
                                If rs2.EOF And rs2.BOF Then
                                    Lit1.Text &= ("<td align = 'center'>12:00:00 AM</td>")
                                Else
                                    If rs2.Fields("HRApproved").Value = 0 And rs2.FIelds("ManagerApproved").Value = 0 Then
                                        Lit1.Text &= ("<td align = 'center'><B><I><font color = 'Red'>12:00:00 AM**</B></I></font></td>")
                                    ElseIf rs2.Fields("HRApproved").Value = 1 Or rs2.Fields("ManagerApproved").Value = 1 Then
                                        Lit1.Text &= ("<td align = 'center'>12:00:00 AM**</td>")
                                    Else
                                        Lit1.Text &= ("<td align = 'center'>12:00:00 AM</td>")
                                    End If
                                End If
                            Else
                                If rs2.EOF And rs2.BOF Then
                                    Lit1.Text &= ("<td align = center>" & Right(rs.Fields("TimeOut2").value, Len(CStr(rs.fields("TimeOut2").value) & "") - InStr(CStr(rs.fields("TimeOut2").value) & "", " ")) & "</td>")
                                Else
                                    If rs2.Fields("HRApproved").Value = 0 And rs2.FIelds("ManagerApproved").Value = 0 Then
                                        Lit1.Text &= ("<td align = center><B><I><font color = 'Red'>" & Right(rs.Fields("TimeOut2").value, Len(CStr(rs.fields("TimeOut2").value) & "") - InStr(CStr(rs.fields("TimeOut2").value) & "", " ")) & "**</font></I></B></td>")
                                    ElseIf rs2.Fields("HRApproved").Value = 1 Or rs2.Fields("ManagerApproved").Value = 1 Then
                                        Lit1.Text &= ("<td align = center>" & Right(rs.Fields("TimeOut2").value, Len(CStr(rs.fields("TimeOut2").value) & "") - InStr(CStr(rs.fields("TimeOut2").value) & "", " ")) & "**</td>")
                                    Else
                                        Lit1.Text &= ("<td align = center>" & Right(rs.Fields("TimeOut2").value, Len(CStr(rs.fields("TimeOut2").value) & "") - InStr(CStr(rs.fields("TimeOut2").value) & "", " ")) & "</td>")
                                    End If
                                End If
                            End If
                            rs2.Close()
                        Else
                            Lit1.Text &= ("<td align = center>N/A</td>")
                        End If
                        If rs.Fields("PunchType2").value & "" <> "" Then
                            Lit1.Text &= ("<td align = center>" & rs.Fields("PunchType2").value & "</td>")
                        Else
                            Lit1.Text &= ("<td align = center>N/A</td>")
                        End If
                    End If
                    If rs.fields("Diff2Rd").value & "" <> "" Then
                        If divid = div Or Not (update) Then Lit1.Text &= ("<td align = center>" & Math.Round(rs.fields("Diff2Rd").value, 1) & "</td>")
                        ptotal(1) = ptotal(1) + rs.fields("Diff2Rd").value
                        dtotal(1) = dtotal(1) + rs.fields("Diff2Rd").value
                        ctotal(1) = ctotal(1) + rs.fields("Diff2Rd").value
                        gtotal(1) = gtotal(1) + rs.fields("Diff2Rd").value
                        If rs.Fields("PunchType2").value & "" = "PAL" Then
                            pPALTotal = pPALTotal + rs.fields("Diff2Rd").value
                            dPALTotal = dPALTotal + rs.fields("Diff2Rd").value
                            cPALTotal = cPALTotal + rs.fields("Diff2Rd").value
                            gPALTotal = gPALTotal + rs.fields("Diff2Rd").value
                        ElseIf rs.Fields("PunchType2").value & "" = "Jury Duty" Then
                            pJURYTotal = pJURYTotal + rs.fields("Diff2Rd").value
                            dJURYTotal = dJURYTotal + rs.fields("Diff2Rd").value
                            cJURYTotal = cJURYTotal + rs.fields("Diff2Rd").value
                            gJURYTotal = gJURYTotal + rs.fields("Diff2Rd").value
                        ElseIf rs.Fields("PunchType2").value & "" = "Bereavement" Then
                            pBERTotal = pBERTotal + rs.fields("Diff2Rd").value
                            dBERTotal = dBERTotal + rs.fields("Diff2Rd").value
                            cBERTotal = cBERTotal + rs.fields("Diff2Rd").value
                            gBERTotal = gBERTotal + rs.fields("Diff2Rd").value
                        ElseIf rs.Fields("PunchType2").value & "" = "SSLB" Then
                            pSSLBTotal = pSSLBTotal + rs.fields("Diff2Rd").value
                            dSSLBTotal = dSSLBTotal + rs.fields("Diff2Rd").value
                            cSSLBTotal = cSSLBTotal + rs.fields("Diff2Rd").value
                            gSSLBTotal = gSSLBTotal + rs.fields("Diff2Rd").value
                        Else
                            pWORKTotal = pWORKTotal + rs.fields("Diff2Rd").value
                            dWORKTotal = dWORKTotal + rs.fields("Diff2Rd").value
                            cWORKTotal = cWORKTotal + rs.fields("Diff2Rd").value
                            gWORKTotal = gWORKTotal + rs.fields("Diff2Rd").value
                            tempHours = tempHours + rs.fields("Diff2Rd").value
                        End If
                    Else
                        If divid = div Or Not (update) Then Lit1.Text &= ("<td align = center>N/A</td>")
                    End If
                    If rs.fields("Total").value & "" <> "" Then
                        'HR Changed due to rounding discrepancies 4-15-2009
                        '				if divid = div  or not(update) then lit1.text &=  "<td align = center>" & round(rs.fields("Total").value / 60,1) & "</td>"
                        If divid = div Or Not (update) Then Lit1.Text &= ("<td align = center>" & Math.Round(rs.fields("Diff1Rd").value + rs.fields("Diff2Rd").value, 1) & "</td>")
                        ptotal(2) = ptotal(2) + Math.Round(rs.fields("Total").value / 60, 1)
                        dtotal(2) = dtotal(2) + Math.Round(rs.fields("Total").value / 60, 1)
                        ctotal(2) = ctotal(2) + Math.Round(rs.fields("Total").value / 60, 1)
                        gtotal(2) = gtotal(2) + Math.Round(rs.fields("Total").value / 60, 1)
                    Else
                        If rs.fields("Diff1Rd").value & "" <> "" Then
                            If divid = div Or Not (update) Then Lit1.Text &= ("<td align = center>" & rs.fields("Diff1Rd").value & "</td>")
                            ptotal(2) = ptotal(2) + rs.fields("Diff1Rd").value
                            dtotal(2) = dtotal(2) + rs.fields("Diff1Rd").value
                            ctotal(2) = ctotal(2) + rs.fields("Diff1Rd").value
                            gtotal(2) = gtotal(2) + rs.fields("Diff1Rd").value
                        ElseIf rs.fields("Diff2Rd").value & "" <> "" Then
                            If divid = div Or Not (update) Then Lit1.Text &= ("<td align = center>" & rs.fields("Diff2Rd").value & "</td>")
                            ptotal(2) = ptotal(2) + rs.fields("Diff2Rd").value
                            dtotal(2) = dtotal(2) + rs.fields("Diff2Rd").value
                            ctotal(2) = ctotal(2) + rs.fields("Diff2Rd").value
                            gtotal(2) = gtotal(2) + rs.fields("Diff2Rd").value
                        Else
                            If divid = div Or Not (update) Then Lit1.Text &= ("<td align = center>0</td>")
                        End If
                    End If

                    If divid = div Or Not (update) Then Lit1.Text &= ("</tr>")
                    rs.movenext()
                Loop

                'Personnel Totals
                If tempHours > 40 Then
                    pOTTotal = Math.Round(pOTTotal + Math.Round((tempHours - 40), 1), 1)
                    dOTTotal = Math.Round(dOTTotal + Math.Round((tempHours - 40), 1), 1)
                    cOTTotal = Math.Round(cOTTotal + Math.Round((tempHours - 40), 1), 1)
                    gOTTotal = Math.Round(gOTTotal + Math.Round((tempHours - 40), 1), 1)
                End If
                If divid = div Or Not (update) Then
                    Lit1.Text &= ("<tr><th colspan = 4 align=left>Totals for " & sLastname & ", " & sFirstname & "</th >")
                    Lit1.Text &= ("<th>" & FormatNumber(ptotal(0), 1) & "</th>")
                    Lit1.Text &= ("<th colspan = 3>&nbsp;</th>")
                    Lit1.Text &= ("<th>" & FormatNumber(ptotal(1), 1) & "</th>")
                    Lit1.Text &= ("<th>" & FormatNumber(ptotal(0) + ptotal(1), 1) & "</th>")
                    'HR took out due to rounding discrepancies 4-15-2009
                    'lit1.text &=  "<th>" & ptotal(2) & "</th>"
                    Lit1.Text &= ("</tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>PAL:</th><th>" & pPALTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>JURY DUTY:</th><th>" & pJURYTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>BEREAVEMENT:</th><th>" & pBERTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>SSLB:</th><th>" & pSSLBTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>REGULAR:</th><th> " & Math.Round(pWORKTotal - pOTTotal, 1) & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>OVERTIME:</th><th> " & pOTTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><td colspan = 8 align=left>&nbsp;</td></tr>")
                    Lit1.Text &= ("<tr><td colspan = 8 align=left>&nbsp;</td></tr>")
                End If
                ptotal(0) = 0
                ptotal(1) = 0
                ptotal(2) = 0
                pPALTotal = 0
                pJURYTotal = 0
                pBERTotal = 0
                pSSLBTotal = 0
                pWORKTotal = 0
                pOTTotal = 0
                tempHours = 0
                '		tempDate = rs.Fields("DateIn")
                'Department Totals
                If divid = div Or Not (update) Then
                    Lit1.Text &= ("<tr><th colspan = 4 align=left>" & department & " Department Totals</th >")
                    Lit1.Text &= ("<th>" & FormatNumber(Math.Round(dtotal(0), 1), 1) & "</th>")
                    Lit1.Text &= ("<th colspan = 3>&nbsp;</th>")
                    Lit1.Text &= ("<th>" & FormatNumber(Math.Round(dtotal(1), 1), 1) & "</th>")
                    Lit1.Text &= ("<th>" & FormatNumber(Math.Round(Math.Round(dtotal(0), 1) + Math.Round(dtotal(1), 1), 1), 1) & "</th>")
                    'HR took out due to Math.rounding discrepancies 4-15-2009			
                    'lit1.text &=  "<th>" & Math.round(dtotal(2), 1) & "</th>"
                    Lit1.Text &= ("</tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>PAL:</th><th>" & dPALTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>JURY DUTY:</th><th>" & dJURYTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>BEREAVEMENT:</th><th>" & dBERTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>SSLB:</th><th>" & dSSLBTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>REGULAR:</th><th> " & Math.Round(dWORKTotal - dOTTotal, 1) & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>OVERTIME:</th><th> " & dOTTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><td colspan = 8 align=left>&nbsp;</td></tr>")
                End If
                dtotal(0) = 0
                dtotal(1) = 0
                dtotal(2) = 0
                dPALTotal = 0
                dJURYTotal = 0
                dBERTotal = 0
                dSSLBTotal = 0
                dWORKTotal = 0
                dOTTotal = 0

                'Company Totals
                If divid = div Or Not (update) Then
                    Lit1.Text &= ("<tr><th colspan = 4 align=left>" & company & " Company Totals</th >")
                    Lit1.Text &= ("<th>" & FormatNumber(Math.Round(ctotal(0), 1), 1) & "</th>")
                    Lit1.Text &= ("<th colspan = 3>&nbsp;</th>")
                    Lit1.Text &= ("<th>" & FormatNumber(Math.Round(ctotal(1), 1), 1) & "</th>")
                    Lit1.Text &= ("<th>" & FormatNumber(Math.Round(Math.Round(ctotal(0), 1) + Math.Round(ctotal(1), 1), 1), 1) & "</th>")
                    'HR took out due to Math.rounding discrepancies 4-15-2009
                    'lit1.text &=  "<th>" & Math.round(ctotal(2), 1) & "</th>"
                    Lit1.Text &= ("</tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>PAL:</th><th>" & cPALTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>JURY DUTY:</th><th>" & cJURYTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>BEREAVEMENT:</th><th>" & cBERTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>SSLB:</th><th>" & cSSLBTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>REGULAR:</th><th> " & Math.Round(cWORKTotal - cOTTotal, 1) & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>OVERTIME:</th><th> " & cOTTotal & "</th></tr>")
                End If
                ctotal(0) = 0
                ctotal(1) = 0
                ctotal(2) = 0
                cPALTotal = 0
                cJURYTotal = 0
                cBERTotal = 0
                cSSLBTotal = 0
                cWORKTotal = 0
                cOTTotal = 0

                If divid = div Or Not (update) Then
                    Lit1.Text &= ("<tr><th colspan=8>&nbsp;</th></tr>")
                    Lit1.Text &= ("<tr><th colspan=4 align=left>Grand Totals:</th>")
                    Lit1.Text &= ("<th>" & Math.Round(gtotal(0), 1) & "</th>")
                    Lit1.Text &= ("<th colspan = 3>&nbsp;</th>")
                    Lit1.Text &= ("<th>" & Math.Round(gtotal(1), 1) & "</th>")
                    Lit1.Text &= ("<th>" & Math.Round(Math.Round(gtotal(0), 1) + Math.Round(gtotal(1), 2), 1) & "</th>")
                    'HR took out due to Math.rounding discrepancies 4-15-2009			
                    'lit1.text &=  "<th>" & Math.round(gtotal(2), 1) & "</th>"
                    Lit1.Text &= ("</tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>PAL:</th><th>" & gPALTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>JURY DUTY:</th><th>" & gJURYTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>BEREAVEMENT:</th><th>" & gBERTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>SSLB:</th><th>" & gSSLBTotal & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>REGULAR:</th><th> " & Math.Round(gWORKTotal - gOTTotal, 1) & "</th></tr>")
                    Lit1.Text &= ("<tr><th colspan = 7></th>")
                    Lit1.Text &= ("<th colspan = 2 align = right>OVERTIME:</th><th> " & gOTTotal & "</th></tr>")
                    Lit1.Text &= ("</table>")
                    Lit1.Text &= ("</div>")
                End If
                If update Then Lit1.Text &= ("End Replacement")
            End If

            rs.close()
            cn.close()
            rs = Nothing
            cn = Nothing
        End If
    End Sub


    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If ddDepartment.Items.Count > 0 Then
            lbDepts.ClearSelection()

            lbDepts.Items.Add(ddDepartment.SelectedItem)
            ddDepartment.Items.Remove(ddDepartment.SelectedItem)
        End If
    End Sub

    Protected Sub rblFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblFilter.SelectedIndexChanged
        Dim cn As New System.Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New System.Data.SqlClient.SqlCommand("", cn)
        Dim dr As System.Data.SqlClient.SqlDataReader
        lbDepts.Items.Clear()
        Dim HR As Boolean = False
        cn.Open()
        cm.CommandText = ("Select * from t_Personnel2Dept a inner join t_ComboItems b on a.departmentid = b.comboitemid where a.active = '1' and a.personnelid = '" & Session("UserDBID") & "' and b.comboitem = 'HR'")
        dr = cm.ExecuteReader
        If dr.HasRows Then
            HR = True
        End If
        dr.Close()
        cn.Close()

        Dim ds As New SqlDataSource(Resources.Resource.cns, "")
        Select Case rblFilter.SelectedValue
            Case "Company"
                if HR = "true" then 
			ds.SelectCommand = "Select comboitemid, comboitem from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'PayrollCompany' and active = 1"
		else
			ds.SelectCommand = "Select comboitemid, comboitem from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'PayrollCompany' and active = 1 and 1=2"
		end if
            Case "Department"
                If HR = "true" Then
                    ds.SelectCommand = ("Select Distinct(a.DepartmentID) as comboitemid, b.comboitem from t_Personnel2Dept a inner join t_Comboitems b on a.departmentid = b.comboitemid  order by b.comboitem asc")
                Else
                    ds.SelectCommand = ("Select Distinct(a.DepartmentID) as comboitemid, b.comboitem from t_Personnel2Dept a inner join t_ComboItems b on a.departmentid = b.comboitemid where a.active = '1' and a.isManager = '1' and a.PersonnelID = '" & Session("UserDBID") & "' order by b.comboitem asc")
                End If
            Case Else
                If HR = "true" Then
                    ds.SelectCommand = ("Select Distinct(a.PersonnelID) as comboitemid, b.FirstName + ' ' + b.LastName as comboitem from t_Personnel2Dept a inner join t_Personnel b on a.personnelid = b.personnelid order by b.firstname + ' ' + b.LastName asc")
                Else
                    ds.SelectCommand = ("Select Distinct(a.PersonnelID) as comboitemid, b.FirstName + ' ' + b.LastName as comboitem from t_Personnel2Dept a inner join t_Personnel b on a.personnelid = b.personnelid where a.DepartmentID in (Select Departmentid from t_Personnel2Dept where personnelid = '" & Session("UserDBID") & "' and isManager = '1' and active = '1') order by b.firstname + ' ' + b.LastName asc")
                End If
        End Select
        cn = Nothing
        dr = Nothing
        ddDepartment.DataSource = ds
        ddDepartment.DataValueField = "comboitemid"
        ddDepartment.DataTextField = "comboitem"
        ddDepartment.DataBind()
        ds = Nothing
    End Sub

End Class
