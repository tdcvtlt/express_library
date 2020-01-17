
Partial Class Reports_Contracts_ActiveContractsWithOutInventory
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Get_Report()
        End If
    End Sub

    Public Sub Get_Report()
        'Server variables
        Dim rs As Object
        Dim cn As Object

        'Html & sql variables
        Dim sql As String

        'Header flag
        Dim active As Boolean
        Dim suspense As Boolean
        Dim pender As Boolean
        Dim developer As Boolean
        Dim onHold As Boolean
        Dim cnt_tot(,) As Integer               '// Array variable to count Total Active/Suspense 
        Dim s_las As String = ""
        Dim s_cur As String = ""


        'Make CRMS connection
        cn = Server.CreateObject("ADODB.Connection")
        cn.CommandTimeout = 0

        cn.Open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        '// recordset
        'Replaced per workorder 42344
        'where b.comboitem in ('active','suspense','Pender-INV', 'Developer', 'ReDeed','Deed-In-Lieu','In Bankruptcy','In Foreclosure','InColl-Active','Pender','Reverter') " & _

        sql = "select a.contractid, a.contractnumber, a.contractdate, " & _
          "status = (select comboitem from t_comboitems where comboitemid = a.statusid), " & _
          "rtrim(c.lastname) + ', ' + rtrim(c.firstname) as owner " & _
          "from t_contract a " & _
          "inner join t_comboitems b " & _
          "on a.statusid = b.comboitemid " & _
          "left outer join (select distinct contractid from t_Soldinventory) si on si.contractid = a.contractid " & _
          "inner join t_prospect c " & _
          "on a.prospectid = c.prospectid " & _
          "where b.comboitem in ('active','On Hold','Pender') " & _
          "and a.contractnumber not like '%t%' and a.contractnumber not like 'u%' " & _
          "and a.contractnumber not like 'e%' and a.contractnumber not like 'n%' " & _
          "and a.contractnumber <> '' " & _
          "and si.contractid is null " & _
          "order by b.ComboItem, a.contractdate desc, a.contractnumber"

        'Make Object and Run Open
        rs = Server.CreateObject("ADODB.RecordSet")
        
        rs.Open(sql, cn, 0, 1)


        If Not (rs.BOF And rs.EOF) Then

            active = False
            suspense = False
            pender = False
            onhold = False
            developer = False

            litReport.Text &= "<strong><font face='Verdana' Size=3>" & "Today's Date: " & FormatDateTime(Date.Now) & "</font></strong><hr>"
            litReport.Text &= "<br/><table border=0 cellspacing=0 cellpadding=0>"

            litReport.Text &= "<col width='250'>"
            litReport.Text &= "<col width='150'>"
            litReport.Text &= "<col width='145'>"

            ReDim cnt_tot(4, 4)
            Dim k As Integer = 0

            Do While Not rs.EOF
                k = k + 1
                s_cur = Trim(rs.Fields("Status").value)

                'Get counts for Active/Suspense statuses
                If s_cur = "Active" Then
                    cnt_tot(0, 0) = cnt_tot(0, 0) + 1
                    'ElseIf s_cur = "Suspense" Then
                ElseIf s_cur = "On Hold" Then
                    cnt_tot(1, 1) = cnt_tot(1, 1) + 1
                ElseIf s_cur = "Pender" Then
                    cnt_tot(2, 2) = cnt_tot(2, 2) + 1

                    'ElseIf s_cur = "Pender-INV" Then
                    '    cnt_tot(2, 2) = cnt_tot(2, 2) + 1
                    'ElseIf s_cur = "Developer" Then
                    '    cnt_tot(3, 3) = cnt_tot(3, 3) + 1
                End If


                'Display Active total
                'If (s_cur = "Suspense" Or s_cur = "Developer" Or s_cur = "Pender-INV") And s_las = "Active" Then
                If (s_cur = "On Hold" Or s_cur = "Pender") And s_las = "Active" Then
                    litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                    litReport.Text &= "<tr><td colspan=2 align='center'><font face='Monospace' size='3' color='red'><strong>" & "Total Active : " & _
                      cnt_tot(0, 0) & "</strong></font></td></tr>"
                    litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                End If

                'Display the Develper total
                'If (s_cur = "Suspense" Or s_cur = "Pender-INV") And s_las = "Developer" Then
                'Display the On Hold Total
                If (s_cur = "Pender") And s_las = "On Hold" Then
                    litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                    litReport.Text &= "<tr><td colspan=2 align='center'><font face='Monospace' size='3' color='red'><strong>" & "Total On Hold : " & _
                      cnt_tot(1, 1) & "</strong></font></td></tr>"
                    litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                End If

                'Display the Pender-INV total
                If s_cur = "Suspense" And s_las = "Pender-INV" Then
                    litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                    litReport.Text &= "<tr><td colspan=2 align='center'><font face='Monospace' size='3' color='red'><strong>" & "Total Pender-INV : " & _
                      cnt_tot(2, 2) & "</strong></font></td></tr>"
                    litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                End If



                'Display the ACTIVE label
                If (active = False) And (s_cur = "Active") Then
                    litReport.Text &= "<tr><td class='header'>" & "Active" & "</td><td class='header'colspan='2'>&nbsp;</td></tr>"
                    litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                    active = True
                End If

                'OR Display the SUSPENSE label
                If (suspense = False) And (s_cur = "Suspense") Then
                    litReport.Text &= "<tr><td class='header'>" & "Suspense" & "</td><td class='header' colspan='2'>&nbsp;</td></tr>"
                    litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                    suspense = True
                End If

                'OR Display the DEVELOPER label
                If (developer = False) And (s_cur = "Developer") Then
                    litReport.Text &= "<tr><td class='header'>" & "Developer" & "</td><td class='header' colspan='2'>&nbsp;</td></tr>"
                    litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                    developer = True
                End If

                'OR Display the OnHold label
                If (developer = False) And (s_cur = "On Hold") Then
                    litReport.Text &= "<tr><td class='header'>" & "On Hold" & "</td><td class='header' colspan='2'>&nbsp;</td></tr>"
                    litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                    developer = True
                End If

                'OR Display the Pender label
                If (pender = False) And (s_cur = "Pender") Then
                    litReport.Text &= "<tr><td class='header'>" & "Pender" & "</td><td class='header' colspan='2'>&nbsp;</td></tr>"
                    litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                    pender = True
                End If

                'Write details on each line
                litReport.Text &= "<tr>"
                litReport.Text &= "<td class='brd'>" & rs.Fields("Owner").value & "</td><td class='brd'><a href='../../marketing/editcontract.aspx?contractid=" & rs.fields("ContractID").value & "'>" & rs.Fields("ContractNumber").value & "</a></td>"

                If Not rs.Fields("ContractDate").value Is System.DBNull.Value Then
                    litReport.Text &= "<td align='right' class='brd'>" & FormatDateTime(rs.Fields("ContractDate").value, 2) & "</td>"
                Else
                    litReport.Text &= "<td class='brd'>&nbsp;</td>"
                End If
                litReport.Text &= "</tr>"


                s_las = s_cur

                rs.MoveNext()

                If rs.EOF Then

                    If s_las = s_cur And s_las = "Pender" Then
                        litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                        litReport.Text &= "<tr><td colspan=2 align='center'><font face='Monospace' size='3' color='red'><strong>" & _
                          "Total Pender : " & cnt_tot(2, 2) & "</strong></font></td><tr>"
                    End If

                    If s_las = s_cur And s_las = "Active" Then
                        litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                        litReport.Text &= "<tr><td colspan=2 align='center'><font face='Monospace' size='3' color='red'><strong>" & _
                          "Total Active : " & cnt_tot(0, 0) & "</strong></font></td><tr>"
                    End If

                    If s_las = s_cur And s_las = "On Hold" Then
                        litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                        litReport.Text &= "<tr><td colspan=2 align='center'><font face='Monospace' size='3' color='red'><strong>" & _
                          "Total On Hold : " & cnt_tot(1, 1) & "</strong></font></td><tr>"
                    End If

                    If s_las = s_cur And s_las = "Pender-INV" Then
                        litReport.Text &= "<tr><td colspan=3>&nbsp;</td></tr>"
                        litReport.Text &= "<tr><td colspan=2 align='center'><font face='Monospace' size='3' color='red'><strong>" & _
                          "Total Pender-INV : " & cnt_tot(2, 2) & "</strong></font></td><tr>"
                    End If
                End If

            Loop

        Else

            litReport.Text &= "<b>" & "No Records Found" & "</b>"

        End If


        rs.Close()
        cn.Close()

        rs = Nothing
        cn = Nothing

    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=ActiveContractsWithoutInventory.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub
End Class
