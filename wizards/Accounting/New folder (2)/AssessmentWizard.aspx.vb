﻿Imports System.Data.SqlClient
Imports System.Data
Partial Class wizards_Accounting_AssessmentWizard
    Inherits System.Web.UI.Page

    Protected Sub rblType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblType.SelectedIndexChanged
        ckPrint.Checked = True
        ckPrint.Enabled = True
        ddPrinter.Enabled = True
        If rblType.SelectedValue = "MF" Then
            MultiView1.ActiveViewIndex = 1
            ckPrint.Checked = False
            ckPrint.Enabled = False
            ddPrinter.Enabled = False
        ElseIf rblType.SelectedValue = "LF" Then
            MultiView1.ActiveViewIndex = 2
            ckPrint.Checked = False
            ckPrint.Enabled = False
            ddPrinter.Enabled = False
        ElseIf rblType.SelectedValue = "CD" Then
            MultiView1.ActiveViewIndex = 3
        ElseIf rblType.SelectedValue = "CFLF" Then
            MultiView1.ActiveViewIndex = 4
        End If
        cbSelect.Visible = False
        gvResults.Visible = False
        gvDues.Visible = False
        gvLF.Visible = False
        gvCDLF.Visible = False
        lblCheck.Visible = False

        Dim WordTemplates As IDictionary(Of String, String) = New Dictionary(Of String, String)()

        WordTemplates.Add("\\nndc\UploadedContracts\mis\2012 Maintenance Fee Invoice - Monthly.doc", "2012 Maintenance Fee Invoice - Monthly")
        WordTemplates.Add("\\nndc\UploadedContracts\mis\2012 Late Fee Invoice.doc", "Late Fee Invoice")
        WordTemplates.Add("\\nndc\UploadedContracts\mis\2013 Maintenance Fee Invoice - Monthly.doc", "Maintenance Fee Invoice - Monthly")

        WordTemplates.Add("\\nndc\UploadedContracts\mis\2013 Club Explorer Fee Invoice - Monthly.doc", "2013 Club Fee Invoice - Monthly")
        WordTemplates.Add("\\nndc\UploadedContracts\mis\2013 Club Explorer Late Fee Invoice.doc", "2013 Late Club Fee Invoice")
        WordTemplates.Add("\\nndc\UploadedContracts\mis\2014 Club Explorer Fee Invoice - Monthly.doc", "2014 Club Fee Invoice - Monthly")
        WordTemplates.Add("\\nndc\UploadedContracts\mis\2014 Club Explorer Late Fee Invoice.doc", "2014 Late Club Fee Invoice")
        WordTemplates.Add("\\nndc\UploadedContracts\mis\2015 Club Explorer Fee Invoice - Monthly.doc", "2015 Club Fee Invoice - Monthly")
        WordTemplates.Add("\\nndc\UploadedContracts\mis\2015 Club Explorer Late Fee Invoice.doc", "2015 Late Club Fee Invoice")



        ddTemplate.Items.Clear()

        If MultiView1.ActiveViewIndex = 1 Or MultiView1.ActiveViewIndex = 2 Then

            'ddTemplate.Items.Add(New ListItem(WordTemplates.ElementAt(0).Value, WordTemplates.ElementAt(0).Key))
            ddTemplate.Items.Add(New ListItem(WordTemplates.ElementAt(1).Value, WordTemplates.ElementAt(1).Key))
            ddTemplate.Items.Add(New ListItem(WordTemplates.ElementAt(2).Value, WordTemplates.ElementAt(2).Key))

        ElseIf MultiView1.ActiveViewIndex = 3 Or MultiView1.ActiveViewIndex = 4 Then
            ddTemplate.Items.Add(New ListItem(WordTemplates.ElementAt(3).Value, WordTemplates.ElementAt(3).Key))
            ddTemplate.Items.Add(New ListItem(WordTemplates.ElementAt(4).Value, WordTemplates.ElementAt(4).Key))
            ddTemplate.Items.Add(New ListItem(WordTemplates.ElementAt(5).Value, WordTemplates.ElementAt(5).Key))
            ddTemplate.Items.Add(New ListItem(WordTemplates.ElementAt(6).Value, WordTemplates.ElementAt(6).Key))
            ddTemplate.Items.Add(New ListItem(WordTemplates.ElementAt(7).Value, WordTemplates.ElementAt(7).Key))
            ddTemplate.Items.Add(New ListItem(WordTemplates.ElementAt(8).Value, WordTemplates.ElementAt(8).Key))
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindData()
            Load_Dues()
        End If
    End Sub

    Private Sub Load_Dues()
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select FintransID, i.Comboitem as Year from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid inner join t_FinTranscodes f on f.transcodeid = i.comboitemid inner join t_ComboItems tt on f.TransTypeID = tt.ComboItemID where tt.ComboItem = 'ProspectTrans' and i.comboitem like 'CD%' order by cast(right(i.comboitem, 2) as int)"
        ClubFees.DataSource = ds
        ClubFees.DataValueField = "FinTransID"
        ClubFees.DataTextField = "Year"
        ClubFees.DataBind()
        ddClubFee.DataSource = ds
        ddClubFee.DataValueField = "FinTransID"
        ddClubFee.DataTextField = "Year"
        ddClubFee.DataBind()
    End Sub

    Private Sub BindData()
        Dim ds As New SqlDataSource
        Dim cn As New System.Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New System.Data.SqlClient.SqlCommand("Select comboitemid, comboitem from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname = 'ContractStatus' order by comboitem", cn)
        Dim dr As System.Data.SqlClient.SqlDataReader
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select FintransID, Comboitem as Year from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid inner join t_FinTranscodes f on f.transcodeid = i.comboitemid where i.comboitem like 'MF%' and c.ComboName = 'TransCode' order by cast(right(i.comboitem, 2) as int)"
            ddYear.DataSource = ds
            ddYear.DataTextField = "Year"
            ddYear.DataValueField = "FintransID"
            ddYear.DataBind()
            ddLFYear.DataSource = ds
            ddLFYear.DataTextField = "Year"
            ddLFYear.DataValueField = "FinTransID"
            ddLFYear.DataBind()
            cn.Open()
            dr = cm.ExecuteReader
            If dr.HasRows Then
                While dr.Read
                    Dim item As New ListItem
                    item.Value = dr("ComboitemID")
                    item.Text = dr("Comboitem")
                    If item.Text = "Active" Or item.Text = "On Hold" Or item.Text = "OnHold" Then
                        lbSelected.ClearSelection()
                        lbSelected.Items.Add(item)
                    Else
                        lbChoices.ClearSelection()
                        lbChoices.Items.Add(item)
                    End If
                End While
            End If
            dr.Close()
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
            ds = Nothing
        End Try
    End Sub

    Protected Sub btnRight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRight.Click
        If lbChoices.SelectedIndex > -1 Then
            Dim item As ListItem = lbChoices.SelectedItem
            lbSelected.ClearSelection()
            lbSelected.Items.Add(item)
            lbChoices.Items.Remove(item)
        End If
    End Sub

    Protected Sub btnLeft_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLeft.Click
        If lbSelected.SelectedIndex > -1 Then
            Dim item As ListItem = lbSelected.SelectedItem
            lbChoices.ClearSelection()
            lbChoices.Items.Add(item)
            lbSelected.Items.Remove(item)
        End If
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Dim ds As New SqlDataSource
        Dim sStatus As String = ""
        Dim MFYear As String = Right(ddYear.SelectedItem.Text, 2)
        MFYear = IIf(CInt(MFYear) < 0, "19" & MFYear, "20" & MFYear)
        For i = 0 To lbSelected.Items.Count - 1
            sStatus &= IIf(sStatus = "", lbSelected.Items(i).Value, "," & lbSelected.Items(i).Value)
        Next
        ds.ConnectionString = Resources.Resource.cns

        Dim sSQL As String = "select * from (	 " & _
                        "select p.lastname as [Last Name], p.firstname as [First Name], c.contractnumber as [KCP#],  " & _
                        "	case when c.maintenancefeeamount is null then 0 else c.maintenancefeeamount  end as MFAmount, " & _
                        "	case when i.Total is null then 0 else i.Total end as [Previously Assessed], " & _
                        "	case when case when i.Total is null then 0 else i.Total end - case when c.maintenancefeeamount is null then 0 else  c.maintenancefeeamount end  < 0 then " & _
                        "		case when i.Total is null then 0 else i.Total end - case when c.maintenancefeeamount is null then 0 else c.maintenancefeeamount end else " & _
                        "0 " & _
                        "	end  * -1 as [To Be Assessed], " & _
                        "	case when charindex(' ', st.comboitem) > 0 then left(st.comboitem, charindex(' ',st.comboitem)) else st.comboitem end as Type " & _
                        "from t_Contract c  " & _
                        "	left outer join t_Comboitems st on st.comboitemid = c.saletypeid  " & _
                        "	inner join t_Prospect p on p.prospectid = c.prospectid " & _
                        "    left outer join t_Frequency f on f.frequencyid = c.frequencyid  " & _
                        "   left outer join ( " & _
                        "		select keyvalue as Contractid, sum(i.Amount) as Total " & _
                        "		from t_Invoices i " & _
                        "			inner join t_Fintranscodes f on f.fintransid = i.fintransid " & _
                        "			inner join t_Comboitems tc on tc.comboitemid = f.transcodeid " & _
                        "		where i.keyfield = 'contractid'  " & _
                        "			and tc.comboitem = '" & ddYear.SelectedItem.Text & "' " & _
                        "			and applytoid = 0 " & _
                        "		group by keyvalue) as i on i.contractid = c.contractid " & _
                        "where c.statusid in (" & sStatus & ")  " & _
                        "	and (" & MFYear & "-year(c.occupancydate))%f.interval = 0  " & _
                        "	and year(c.occupancydate)<= " & MFYear & " " & _
                        "    and st.comboitem <> 'trial' " & _
                        "	and c.contractdate <='" & dfCuttOff.Selected_Date & "' " & _
                        ")a where [To Be Assessed]>0"
        sSQL = "select * from (	 " & _
                        "select p.lastname as [Last Name], p.firstname as [First Name], c.contractnumber as [KCP#],  " & _
                        "	case when c.maintenancefeeamount is null then 0 else c.maintenancefeeamount  end as MFAmount, " & _
                        "	case when i.Total is null then 0 else i.Total end as [Previously Assessed], " & _
                        "	case when case when i.Total is null then 0 else i.Total end - case when c.maintenancefeeamount is null then 0 else  c.maintenancefeeamount end  < 0 then " & _
                        "		case when i.Total is null then 0 else i.Total end - case when c.maintenancefeeamount is null then 0 else c.maintenancefeeamount end else " & _
                        "0 " & _
                        "	end  * -1 as [To Be Assessed], " & _
                        "	case when charindex(' ', st.comboitem) > 0 then left(st.comboitem, charindex(' ',st.comboitem)) else st.comboitem end as Type " & _
                        "from t_Contract c  " & _
                        "	left outer join t_Comboitems st on st.comboitemid = c.saletypeid  " & _
                        "	inner join t_Prospect p on p.prospectid = c.prospectid " & _
                        "    left outer join t_Frequency f on f.frequencyid = c.frequencyid  " & _
                        "   left outer join ( " & _
                        "		select keyvalue as Contractid, sum(i.Amount) as Total " & _
                        "		from v_Invoices i " & _
                        "		where i.keyfield = 'contractid'  " & _
                        "			and i.invoice = '" & ddYear.SelectedItem.Text & "' " & _
                        "		group by keyvalue) as i on i.contractid = c.contractid " & _
                        "where c.statusid in (" & sStatus & ")  " & _
                        "	and (" & MFYear & "-year(c.occupancydate))%f.interval = 0  " & _
                        "	and year(c.occupancydate)<= " & MFYear & " " & _
                        "    and st.comboitem <> 'trial' " & _
                        "	and c.contractdate <='" & dfCuttOff.Selected_Date & "' " & _
                        ")a where [To Be Assessed]>0"
        sSQL = "select * from (	 " & _
                        "select p.lastname as [Last Name], p.firstname as [First Name], c.contractnumber as [KCP#],  " & _
                        "	case when mfc.Amount IS null then 0 else mfc.amount end AS MFAmount, " & _
                        "	case when i.Total is null then 0 else i.Total end as [Previously Assessed], " & _
                        "	case when case when i.Total is null then 0 else i.Total end - case when mfc.Amount IS null then 0 else mfc.amount end  < 0 then " & _
                        "		case when i.Total is null then 0 else i.Total end - case when mfc.Amount IS null then 0 else mfc.amount end else " & _
                        "0 " & _
                        "	end  * -1 as [To Be Assessed], " & _
                        "	case when charindex(' ', st.comboitem) > 0 then left(st.comboitem, charindex(' ',st.comboitem)) else st.comboitem end as Type " & _
                        "   ,cs.comboitem as Status, css.comboitem as SubStatus, mfs.comboitem as MFStatus, c.contractid " & _
                        "from t_Contract c  " & _
                        "	left outer join t_Comboitems st on st.comboitemid = c.saletypeid  " & _
                        "   inner join t_MaintenanceFeeCode2FinTrans mfc on mfc.MaintenanceFeeCodeID = c.MaintenanceFeeCodeID " & _
                        "	inner join t_FinTransCodes ft on ft.FinTransID = mfc.FinTransID " & _
                        "	inner join t_ComboItems tc on tc.ComboItemID = ft.TransCodeID and tc.ComboItem = '" & ddYear.SelectedItem.Text & "' " & _
                        "	inner join t_Prospect p on p.prospectid = c.prospectid " & _
                        "    left outer join t_Frequency f on f.frequencyid = c.frequencyid  " & _
                        "   left outer join ( " & _
                        "		select keyvalue as Contractid, sum(i.Amount) as Total " & _
                        "		from v_Invoices i " & _
                        "		where i.keyfield = 'contractid'  " & _
                        "			and i.invoice = '" & ddYear.SelectedItem.Text & "' " & _
                        "		group by keyvalue) as i on i.contractid = c.contractid " & _
                        "   left outer join t_Comboitems cs on cs.comboitemid = c.statusid " & _
                        "   left outer join t_Comboitems css on css.comboitemid = c.substatusid " & _
                        "   left outer join t_Comboitems mfs on mfs.comboitemid = c.maintenancefeestatusid " & _
                        "where c.statusid in (" & sStatus & ")  " & _
                        "	and (" & MFYear & "-year(c.occupancydate))%f.interval = 0  " & _
                        "	and year(c.occupancydate)<= " & MFYear & " " & _
                        "    and st.comboitem <> 'trial' " & _
                        "	and c.contractdate <='" & dfCuttOff.Selected_Date & "' " & _
                        ")a where [To Be Assessed]>0"
        ds.SelectCommand = sSQL
        gvResults.DataSource = ds
        gvResults.DataBind()
        gvLF.Visible = False
        gvDues.Visible = False
        gvCDLF.Visible = False
        gvResults.Visible = True
        cbSelect.Visible = True
        lblCheck.Visible = True
        cbSelect.Checked = True
        ds = Nothing
        'rs.open(sSQL, cn, 0, 1)

        ''response.write "<tr><td colspan = 6>" & sSQL & "</td></tr>"
        'If Request("DETAILS") = "true" Then
        '    Response.Write("<tr><th>Selected</th></th><th>Last Name</th><th>First Name</th><th>KCP #</th><th>MF Amount</th><th>Previously Assessed</th><th>To Be Assessed</th><th>Type</th><th></th></tr>")
        'End If
        'Dim counter, zerodue
        'counter = 0
        'zerodue = 0
        'rs2 = Server.CreateObject("ADODB.Recordset")
        'Do While Not rs.eof
        '    due = 0
        '    counter = counter + 1
        '    If conamount Then
        '        If rs.fields("Total").value < rs.fields("MaintenanceFeeAmount").value Or rs.fields("Total").value & "" = "" Then
        '            If rs.fields("Total").value >= 0 Then
        '                due = rs.fields("MaintenanceFeeAmount").value - rs.fields("Total").value
        '            Else
        '                due = rs.fields("MaintenanceFeeAmount").value
        '            End If
        '        End If
        '    Else
        '        If rs.fields("Total").value < amount Or rs.fields("Total").value & "" = "" Then
        '            If rs.fields("Total").value >= 0 Then
        '                due = amount - rs.fields("Total").value
        '            Else
        '                due = amount
        '            End If
        '        End If
        '    End If
        '    If due & "" = "" Then due = 0
        '    Dim bFoundType
        '    bFoundType = False
        '    'rs2.open "select " & dbname & ".dbo.ufn_ContractType(" & rs.fields("ContractID").value & ")", cn, 0, 1
        '    'sType = rs2.fields(0).value
        '    'rs2.close
        '    For i = 0 To UBound(types)
        '        sType = ""
        '        If InStr(rs.fields("SalesType").value & "", " ") > 0 Then
        '            sType = Left(rs.fields("SalesType").value, InStr(rs.fields("SalesType").value, " "))
        '        Else
        '            sType = rs.fields("SalesType").value & ""
        '        End If

        '        If UCase(Trim(types(i))) = UCase(Trim(sType & "")) Then
        '            totals(i) = totals(i) + due
        '            bFoundType = True
        '            Exit For
        '        End If
        '    Next

        '    If Not bFoundType Then totals(UBound(totals)) = totals(UBound(totals)) + due

        '    If Request("Details") And due > 0 Then
        '        Response.Write("<tr>")
        '        Response.Write("<td><input type=checkbox name=contract value='" & rs.fields("ContractNumber").value & "' checked ></td>")
        '        Response.Write("<td>" & rs.fields("LastName").value & "</td>")
        '        Response.Write("<td>" & rs.fields("FirstName").value & "</td>")
        '        Response.Write("<td>" & rs.fields("ContractNumber").value & "</td>")
        '        If rs.fields("MaintenanceFeeAmount").value & "" <> "" Then
        '            Response.Write("<td>" & FormatCurrency(rs.fields("MaintenanceFeeAmount").value) & "</td>")
        '        Else
        '            Response.Write("<td>" & FormatCurrency(0) & "</td>")
        '        End If
        '        If rs.fields("total").value & "" = "" Then
        '            Response.Write("<td>" & FormatCurrency(0) & "</td>")
        '        Else
        '            Response.Write("<td>" & FormatCurrency(rs.fields("Total").value) & "</td>")
        '        End If
        '        Response.Write("<td>" & FormatCurrency(Due) & "</td>")
        '        'response.write  "<td>" &  rs.fields("Frequency").value & "</td>"
        '        Response.Write("<td>" & rs.fields("SalesType").value & "</td>")
        '        'response.write  "<td>" &  rs.fields("OccupancyDate").value & "</td>"
        '        Response.Write("<td><a href='../editcontract.asp?contractid=" & rs.fields("ContractID").value & "'><img src = '../images/edit.gif'></a></td>")
        '        Response.Write("</tr>")
        '        'Add_to_Preview rs.fields("ContractNumber").value, rs.fields("due").value
        '    ElseIf due = 0 Then
        '        zerodue = zerodue + 1
        '    End If
        '    rs.movenext()
        'Loop

    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & IIf(rblType.SelectedValue = "MF", "Maintenance Fee Assessment ", "Late Fee Assessment ") & "for " & ddYear.SelectedItem.Text & ".xls")
        Response.Write("<table>")
        Response.Write("<tr>")
        For i = 0 To gvResults.Columns.Count - 1
            Response.Write("<th>")
            Response.Write(gvResults.Columns(i).HeaderText)
            Response.Write("</th>")
        Next
        Response.Write("</tr>")
        For Each ro As GridViewRow In gvResults.Rows
            Response.Write("<tr>")
            For i = 0 To ro.Cells.Count - 1
                Response.Write("<td>")
                Response.Write(ro.Cells(i).Text)
                Response.Write("</td>")
            Next
            Response.Write("</tr>")
        Next
        Response.Write("</table>")
        Response.End()
    End Sub

    Protected Sub cbSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbSelect.CheckedChanged
        If rblType.SelectedValue = "MF" Then
            For Each oRow As GridViewRow In gvResults.Rows
                If Not (oRow.FindControl("cb") Is Nothing) Then CType(oRow.FindControl("cb"), CheckBox).Checked = cbSelect.Checked
            Next
        ElseIf rblType.SelectedValue = "LF" Then
            For Each oRow As GridViewRow In gvLF.Rows
                If Not (oRow.FindControl("cb") Is Nothing) Then CType(oRow.FindControl("cb"), CheckBox).Checked = cbSelect.Checked
            Next
        ElseIf rblType.SelectedValue = "CD" Then
            For Each oRow As GridViewRow In gvDues.Rows
                If Not (oRow.FindControl("cb") Is Nothing) Then CType(oRow.FindControl("cb"), CheckBox).Checked = cbSelect.Checked
            Next
        ElseIf rblType.SelectedValue = "CFLF" Then
            For Each oRow As GridViewRow In gvCDLF.Rows
                If Not (oRow.FindControl("cb") Is Nothing) Then CType(oRow.FindControl("cb"), CheckBox).Checked = cbSelect.Checked
            Next
        End If
    End Sub


    Protected Sub btnAssess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAssess.Click
        Dim oFT As New clsFinancialTransactionCodes
        Dim oCon As clsContract
        Dim oInv As clsInvoices
        Dim ftc As Integer = oFT.Find_Fin_Trans("MFTrans", ddyear.selecteditem.text)
        Dim conid As Integer = 0
        Dim invID As Integer = 0

        For Each oRow As gridviewrow In gvresults.rows
            If Not (oRow.findcontrol("cb") Is Nothing) Then
                If CType(oRow.findcontrol("cb"), checkbox).checked Then
                    oInv = New clsInvoices
                    oCon = New clsContract
                    'Get Contractid
                    oCon.ContractNumber = oRow.cells(3).text
                    oCon.Load()
                    conid = oCon.ContractID
                    invID = 0
                    If CDbl(oRow.cells(5).text) > 0 Then 'Find the existing Invoice
                        invID = oInv.Find_Existing_Invoice("ContractID", conid, ddyear.selecteditem.text)
                    End If
                    oInv.Load()
                    oInv.Amount = oRow.cells(6).text
                    oInv.ApplyToID = invID
                    oInv.Adjustment = iif(invID = 0, False, True)
                    oInv.Description = ddyear.selectedItem.text
                    oInv.PosNeg = 0
                    oInv.DueDate = dfduedate.selected_date
                    oInv.FinTransID = ftc
                    oInv.ProspectID = oCon.ProspectID
                    oInv.KeyField = "ContractID"
                    oInv.KeyValue = conid
                    oInv.Reference = ddyear.selecteditem.text
                    oInv.TransDate = Date.Now
                    oInv.UserID = Session("UserDBID")
                    oInv.Save()
                    Create_Document(conid)
                    oInv = Nothing
                    oCon = Nothing
                End If
            End If
        Next
        oInv = Nothing
        oFT = Nothing
        oCon = Nothing
    End Sub

    Private Sub Create_Document(ByVal ConID As Integer)
        Dim oDoc As New clsPendingWordDocuments
        Dim oItems As New clsPendingWordDocumentValues
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select * from v_MFLetter where contractid = " & ConID, cn)
        Dim dr As SqlDataReader
        Dim sql As String = ""
        If Not (InStr(ddTemplate.SelectedItem.Text, "Late Fee") > 0) Then
            'Maintenance Fee Invoice
            sql = "select f.frequency, contractnumber, case when trustname is null then '' else trustname end as trustname, lastname, firstname,(select top 1 address1 from t_ProspectAddress where activeflag=1 and prospectid = p.prospectid) as address1, (select top 1 city from t_ProspectAddress where activeflag=1 and prospectid = p.prospectid) as city, (select top 1 comboitem from t_ProspectAddress a left outer join t_Comboitems s on s.comboitemid = a.stateid where activeflag=1 and prospectid = p.prospectid) as  State, (select top 1 comboitem from t_ProspectAddress a left outer join t_Comboitems co on co.comboitemid = a.countryid where activeflag=1 and prospectid = p.prospectid) as  country, (select top 1 postalcode from t_ProspectAddress where activeflag=1 and prospectid = p.prospectid) as postalcode, " & _
                 "    case when bf.balanceforward is null then 0 else bf.balanceforward end as balanceforward, " & _
                 "    balance as AmountDue, i.amount - i.balance as payments, i.Amount as amountbilled, i.balance,i.balance + isnull(bf.balanceforward,0) as TotalDue, i.invoice_description as Description " & _
                    "from t_Contract c " & _
                 "    inner join t_Prospect p on p.prospectid = c.prospectid " & _
                 "    left outer join t_Frequency f on f.frequencyid = c.frequencyid " & _
                 "    inner join (select * from  v_Invoices where keyfield = 'contractid' and invoice = '" & ddYear.SelectedItem.Text & "') i on i.keyvalue = c.contractid " & _
                 "    left outer join (Select keyvalue, sum(balance)as Balanceforward from v_invoices where keyfield = 'contractid' and duedate <= '" & dfDueDate.Selected_Date & "' and invoice <> '" & ddYear.SelectedItem.Text & "' group by keyvalue) bf on bf.keyvalue = c.contractid " & _
                    "where contractid = " & ConID
            cm.CommandText = sql
            cn.Open()
            dr = cm.ExecuteReader
            If dr.HasRows Then
                While dr.Read
                    oDoc.PendingWordDocumentID = 0
                    oDoc.Load()
                    oDoc.Completed = 0
                    oDoc.DateRequested = Date.Today
                    oDoc.FileName = dr("ContractNumber") & "=" & ddTemplate.SelectedItem.Text & Right(ddTemplate.SelectedValue, 4)
                    If ckPrint.Checked Then
                        oDoc.Printer = ddPrinter.SelectedValue
                    End If
                    oDoc.SessionID = Session.SessionID
                    oDoc.TemplateFile = ddTemplate.SelectedValue
                    oDoc.SaveFile = "\\kcp.local\resort shares\G Drive\DAILY UPLOAD\Contract Files\" & oDoc.FileName
                    oDoc.Save()
                    Add_Item_Value("<NAME>", dr("Firstname") & " " & dr("LastName"), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<ADDRESS>", dr("ADDRESS1") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<CITY>", dr("CITY") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<STATE>", dr("STATE") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<ZIP>", dr("POSTALCODE") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<COUNTRY>", dr("COUNTRY") & " ", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<LETTERDATE>", Date.Today, oDoc.PendingWordDocumentID)
                    Add_Item_Value("<KCP>", dr("CONTRACTNUMBER") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<DESCRIPTION>", "20" & Right(ddYear.SelectedItem.Text, 2) & " Maintenance Fee", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<BALANCEFORWARD>", FormatCurrency(dr("BALANCEFORWARD")), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<AMOUNT>", FormatCurrency(dr("AmountBilled")), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<BALANCE>", FormatCurrency(dr("BALANCE")), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<TOTALDUE>", FormatCurrency(dr("TOTALDUE")), oDoc.PendingWordDocumentID)

                    oDoc.Ready = True
                    oDoc.Save()
                    Response.Write(oDoc.Error_Message)
                End While
            End If
            cn.Close()
        Else
            'Late Fee Invoice
            sql = "select f.frequency, contractnumber, case when trustname is null then '' else trustname end as trustname, lastname, firstname,(select top 1 address1 from t_ProspectAddress where activeflag=1 and prospectid = p.prospectid) as address1, (select top 1 city from t_ProspectAddress where activeflag=1 and prospectid = p.prospectid) as city, (select top 1 comboitem from t_ProspectAddress a left outer join t_Comboitems s on s.comboitemid = a.stateid where activeflag=1 and prospectid = p.prospectid) as  State, (select top 1 comboitem from t_ProspectAddress a left outer join t_Comboitems co on co.comboitemid = a.countryid where activeflag=1 and prospectid = p.prospectid) as  country, (select top 1 postalcode from t_ProspectAddress where activeflag=1 and prospectid = p.prospectid) as postalcode, " & _
                 "    case when bf.balanceforward is null then 0 else bf.balanceforward end as balanceforward, " & _
                 "    balance as AmountDue, i.amount - i.balance as payments, i.Amount as amountbilled, i.balance,i.balance + isnull(bf.balanceforward,0) as TotalDue, i.invoice_description as Description " & _
                    "from t_Contract c " & _
                 "    inner join t_Prospect p on p.prospectid = c.prospectid " & _
                 "    left outer join t_Frequency f on f.frequencyid = c.frequencyid " & _
                 "    inner join (select * from  v_Invoices where keyfield = 'contractid' and invoice = 'Late Fee' and reference = 'LF" & Right(ddLFYear.SelectedItem.Text, 2) & "') i on i.keyvalue = c.contractid " & _
                 "    left outer join (Select keyvalue, sum(balance)as Balanceforward from v_invoices where keyfield = 'contractid' and duedate <= '" & Date.Today.AddDays(-1) & "' group by keyvalue) bf on bf.keyvalue = c.contractid " & _
                    "where contractid = " & ConID
            cm.CommandText = sql
            cn.Open()
            dr = cm.ExecuteReader
            If dr.HasRows Then
                While dr.Read
                    oDoc.PendingWordDocumentID = 0
                    oDoc.Load()
                    oDoc.Completed = 0
                    oDoc.DateRequested = Date.Today
                    oDoc.FileName = dr("ContractNumber") & "=" & ddTemplate.SelectedItem.Text & Right(ddTemplate.SelectedValue, 4)
                    If ckPrint.Checked Then
                        oDoc.Printer = ddPrinter.SelectedValue
                    End If
                    oDoc.SessionID = Session.SessionID
                    oDoc.TemplateFile = ddTemplate.SelectedValue
                    oDoc.SaveFile = "\\kcp.local\resort shares\G Drive\DAILY UPLOAD\Contract Files\" & oDoc.FileName
                    oDoc.Save()
                    Add_Item_Value("<NAME>", dr("firstname") & " " & dr("LastName"), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<ADDRESS>", dr("ADDRESS1") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<CITY>", dr("CITY") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<STATE>", dr("STATE") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<ZIP>", dr("POSTALCODE") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<COUNTRY>", dr("COUNTRY") & " ", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<LETTERDATE>", Date.Today, oDoc.PendingWordDocumentID)
                    Add_Item_Value("<KCP>", dr("CONTRACTNUMBER") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<DESCRIPTION>", "20" & Right(ddLFYear.SelectedItem.Text, 2) & " Maintenance Fee Late Fee", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<BALANCEFORWARD>", FormatCurrency(dr("BALANCEFORWARD")), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<AMOUNT>", FormatCurrency(dr("amountbilled")), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<BALANCE>", FormatCurrency(dr("BALANCE")), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<TOTALDUE>", FormatCurrency(dr("TOTALDUE")), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<TRI>", " ", oDoc.PendingWordDocumentID)
                    oDoc.Ready = True
                    oDoc.Save()
                    Response.Write(oDoc.Error_Message)
                End While
            End If
            cn.Close()
        End If
        dr = Nothing
        cm = Nothing
        cn = Nothing
        oDoc = Nothing
        oItems = Nothing
    End Sub


    Private Sub CreateDocument(ByVal prospectId As String)

        Dim oDoc As New clsPendingWordDocuments
        Dim oItems As New clsPendingWordDocumentValues
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim dr As SqlDataReader = Nothing
        Dim sql As String = String.Empty

        If Not (InStr(ddTemplate.SelectedItem.Text, "Late") > 0) Then

            sql = "select Distinct(p.ProspectID), p.LastName, p.FirstName, (select top 1 address1 from t_Prospectaddress where prospectid = p.prospectid order by activeflag desc) as Address1, (select top 1 city from t_Prospectaddress where prospectid = p.prospectid order by activeflag desc) as City, (select top 1 comboitem from t_Prospectaddress a left outer join t_Comboitems s on s.comboitemid = a.stateid where a.prospectid = p.prospectid order by activeflag desc) as State, (select top 1 PostalCode from t_Prospectaddress where prospectid = p.prospectid order by activeflag desc) as PostalCode, coalesce((select top 1 co.comboitem from t_Prospectaddress pa inner join t_Comboitems co on co.comboitemid = pa.countryid where prospectid = p.prospectid order by activeflag desc), '' ) as Country, " & _
                    "coalesce(v.BalanceForward, 0) + (Select coalesce(Sum(Balance), 0) from v_invoices " & _
                    "where (KeyField = 'ContractID') and keyvalue in (Select contractid from t_Contract where prospectid = p.ProspectID) and Duedate <= '" & DueDate.Selected_Date & "' and (invoice <> 'ORF')) as BalanceForward, i.Balance Balance, i.Amount AmountBilled, " & _
                    "i.Balance + coalesce(v.BalanceForward, 0) + (Select coalesce(Sum(Balance), 0) from v_invoices " & _
                    "where (KeyField = 'ContractID') and keyvalue in (Select contractid from t_Contract where prospectid = p.ProspectID) and Duedate <= '" & DueDate.Selected_Date & "' and (invoice <> 'ORF')) TotalDue " & _
                    "from " & _
                    "t_Prospect p left outer join " & _
                    "(select KeyValue,  Sum(Balance) BalanceForward from v_invoices " & _
                    "where (KeyField = 'ProspectID')  and Duedate <= '" & DueDate.Selected_Date & "' and (invoice <> '" & ClubFees.SelectedItem.Text & "') group by KeyValue) v " & _
                    "on p.ProspectId = v.KeyValue " & _
                    "inner join (select * from v_Invoices where KeyField = 'ProspectId' and Invoice = '" & ClubFees.SelectedItem.Text & "') i " & _
                    "on i.KeyValue = p.ProspectId " & _
                    " where p.ProspectID = " & prospectId

            sql = "select Distinct(p.ProspectID), p.LastName, p.FirstName, (select top 1 address1 from t_Prospectaddress where prospectid = p.prospectid order by activeflag desc) as Address1, (select top 1 city from t_Prospectaddress where prospectid = p.prospectid order by activeflag desc) as City, (select top 1 comboitem from t_Prospectaddress a left outer join t_Comboitems s on s.comboitemid = a.stateid where a.prospectid = p.prospectid order by activeflag desc) as State, (select top 1 PostalCode from t_Prospectaddress where prospectid = p.prospectid order by activeflag desc) as PostalCode, coalesce((select top 1 co.comboitem from t_Prospectaddress pa inner join t_Comboitems co on co.comboitemid = pa.countryid where prospectid = p.prospectid order by activeflag desc), '' ) as Country, " & _
                    "coalesce(v.BalanceForward, 0) as BalanceForward, i.Balance Balance, i.Amount AmountBilled, " & _
                    "i.Balance + coalesce(v.BalanceForward, 0)  TotalDue " & _
                    "from " & _
                    "t_Prospect p left outer join " & _
                    "(select KeyValue,  Sum(Balance) BalanceForward from v_invoices " & _
                    "where (KeyField = 'ProspectID')  and Duedate <= '" & DueDate.Selected_Date & "' and (invoice <> '" & ClubFees.SelectedItem.Text & "') group by KeyValue) v " & _
                    "on p.ProspectId = v.KeyValue " & _
                    "inner join (select * from v_Invoices where KeyField = 'ProspectId' and Invoice = '" & ClubFees.SelectedItem.Text & "') i " & _
                    "on i.KeyValue = p.ProspectId " & _
                    " where p.ProspectID = " & prospectId

            Dim cm As New SqlCommand(sql, cn)
            cn.Open()
            dr = cm.ExecuteReader()

            If (dr.HasRows = True) Then
                Do While dr.Read()
                    oDoc.PendingWordDocumentID = 0
                    oDoc.Load()
                    oDoc.Completed = 0
                    oDoc.DateRequested = Date.Today
                    oDoc.FileName = dr("ProspectId") & "=" & ddTemplate.SelectedItem.Text & Right(ddTemplate.SelectedValue, 4)
                    If ckPrint.Checked Then
                        oDoc.Printer = ddPrinter.SelectedValue
                    End If
                    oDoc.SessionID = Session.SessionID
                    oDoc.TemplateFile = ddTemplate.SelectedValue
                    oDoc.SaveFile = "\\kcp.local\resort shares\G Drive\DAILY UPLOAD\Owner - Prospect Files\" & oDoc.FileName
                    oDoc.Save()

                    Add_Item_Value("<PROSPECT>", dr("ProspectID") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<NAME>", dr("firstname") & " " & dr("LastName"), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<ADDRESS>", dr("ADDRESS1") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<CITY>", dr("CITY") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<STATE>", dr("STATE") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<ZIP>", dr("POSTALCODE") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<COUNTRY>", dr("COUNTRY") & " ", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<LETTERDATE>", Date.Today, oDoc.PendingWordDocumentID)
                    'Add_Item_Value("<KCP>", dr("CONTRACTNUMBER") & "", oDoc.PendingWordDocumentID)
                    'Add_Item_Value("<DESCRIPTION>", dr("INVOICE_DESCRIPTION") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<BALANCEFORWARD>", FormatCurrency(dr("BALANCEFORWARD")), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<AMOUNT>", FormatCurrency(dr("amountbilled")), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<BALANCE>", FormatCurrency(dr("BALANCE")), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<TOTALDUE>", FormatCurrency(dr("TOTALDUE")), oDoc.PendingWordDocumentID)
                    'Add_Item_Value("<TRI>", " ", oDoc.PendingWordDocumentID)
                    oDoc.Ready = True
                    oDoc.Save()
                    Response.Write(oDoc.Error_Message)
                Loop
            End If
        Else
            sql = "select Distinct(p.ProspectID), p.LastName, p.FirstName, (select top 1 address1 from t_ProspectAddress a where a.prospectid = p.prospectid order by activeflag desc) as Address1,	(select top 1 city from t_ProspectAddress a where a.prospectid = p.prospectid order by activeflag desc) as City,	(select top 1 comboitem from t_ProspectAddress a left outer join t_Comboitems ps on ps.comboitemid = a.stateid where a.prospectid = p.prospectid order by activeflag desc) as State,	(select top 1 postalcode from t_ProspectAddress a where a.prospectid = p.prospectid order by activeflag desc) as PostalCode,	coalesce((select top 1 comboitem from t_ProspectAddress a left outer join t_Comboitems ps on ps.comboitemid = a.countryid where a.prospectid = p.prospectid order by activeflag desc), '') as Country, " & _
                   "coalesce(v.BalanceForward, 0) + (Select coalesce(Sum(Balance), 0) from v_invoices " & _
                   "where (KeyField = 'ContractID') and keyvalue in (Select contractid from t_Contract where prospectid = p.ProspectID) and Duedate <= '" & Date.Today.AddDays(-1) & "' and (invoice <> 'ORF')) as BalanceForward, i.Balance Balance, i.Amount AmountBilled, " & _
                   "i.Balance + coalesce(v.BalanceForward, 0) + (Select coalesce(Sum(Balance), 0) from v_invoices " & _
                   "where (KeyField = 'ContractID') and keyvalue in (Select contractid from t_Contract where prospectid = p.ProspectID) and Duedate <= '" & Date.Today.AddDays(-1) & "' and (invoice <> 'ORF')) TotalDue " & _
                   "from " & _
                   "t_Prospect p left outer join " & _
                   "(select KeyValue, Sum(Balance) BalanceForward from v_invoices " & _
                   "where (KeyField = 'ProspectID')  and Duedate <= '" & Date.Today.AddDays(-1) & "' and (invoice <> 'LATE FEE DUES') and Reference <> 'LFCD" & Right(ddClubFee.SelectedItem.Text, 2) & "' group by KeyValue) v " & _
                   "on p.ProspectId = v.KeyValue " & _
                   "inner join (select * from v_Invoices where KeyField = 'ProspectId' and Invoice = 'LATE FEE DUES' and duedate> '" & Date.Today.AddDays(-1) & "') i " & _
                   "on i.KeyValue = p.ProspectId " & _
                   " where p.ProspectID = " & prospectId
            sql = "select Distinct(p.ProspectID), p.LastName, p.FirstName, (select top 1 address1 from t_ProspectAddress a where a.prospectid = p.prospectid order by activeflag desc) as Address1,	(select top 1 city from t_ProspectAddress a where a.prospectid = p.prospectid order by activeflag desc) as City,	(select top 1 comboitem from t_ProspectAddress a left outer join t_Comboitems ps on ps.comboitemid = a.stateid where a.prospectid = p.prospectid order by activeflag desc) as State,	(select top 1 postalcode from t_ProspectAddress a where a.prospectid = p.prospectid order by activeflag desc) as PostalCode,	coalesce((select top 1 comboitem from t_ProspectAddress a left outer join t_Comboitems ps on ps.comboitemid = a.countryid where a.prospectid = p.prospectid order by activeflag desc), '') as Country, " & _
                               "coalesce(v.BalanceForward, 0)  as BalanceForward, i.Balance Balance, i.Amount AmountBilled, " & _
                               "i.Balance + coalesce(v.BalanceForward, 0)  TotalDue " & _
                               "from " & _
                               "t_Prospect p left outer join " & _
                               "(select KeyValue, Sum(Balance) BalanceForward from v_invoices " & _
                               "where (KeyField = 'ProspectID')  and Duedate <= '" & Date.Today.AddDays(-1) & "' and Reference <> 'LFCD" & Right(ddClubFee.SelectedItem.Text, 2) & "' group by KeyValue) v " & _
                               "on p.ProspectId = v.KeyValue " & _
                               "inner join (select * from v_Invoices where KeyField = 'ProspectId' and Invoice = 'LATE FEE DUES' and duedate> '" & Date.Today.AddDays(-1) & "') i " & _
                               "on i.KeyValue = p.ProspectId " & _
                               " where p.ProspectID = " & prospectId

            Dim cm As New SqlCommand(sql, cn)
            cn.Open()
            dr = cm.ExecuteReader()

            If (dr.HasRows = True) Then
                Do While dr.Read()
                    oDoc.PendingWordDocumentID = 0
                    oDoc.Load()
                    oDoc.Completed = 0
                    oDoc.DateRequested = Date.Today
                    oDoc.FileName = dr("ProspectId") & "=" & ddTemplate.SelectedItem.Text & Right(ddTemplate.SelectedValue, 4)
                    If ckPrint.Checked Then
                        oDoc.Printer = ddPrinter.SelectedValue
                    End If
                    oDoc.SessionID = Session.SessionID
                    oDoc.TemplateFile = ddTemplate.SelectedValue
                    oDoc.SaveFile = "\\kcp.local\resort shares\G Drive\DAILY UPLOAD\Owner - Prospect Files\" & oDoc.FileName
                    oDoc.Save()

                    Add_Item_Value("<PROSPECT>", dr("ProspectID") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<NAME>", dr("firstname") & " " & dr("LastName"), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<ADDRESS>", dr("ADDRESS1") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<CITY>", dr("CITY") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<STATE>", dr("STATE") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<ZIP>", dr("POSTALCODE") & "", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<COUNTRY>", dr("COUNTRY") & " ", oDoc.PendingWordDocumentID)
                    Add_Item_Value("<LETTERDATE>", Date.Today, oDoc.PendingWordDocumentID)
                    'Add_Item_Value("<KCP>", dr("CONTRACTNUMBER") & "", oDoc.PendingWordDocumentID)

                    'If dr("DESCRIPTION").Equals(DBNull.Value) = False Then
                    'Add_Item_Value("<DESCRIPTION>", dr("DESCRIPTION") & "", oDoc.PendingWordDocumentID)
                    'Else
                    'Add_Item_Value("<DESCRIPTION>", "", oDoc.PendingWordDocumentID)
                    'End If


                    Add_Item_Value("<BALANCEFORWARD>", FormatCurrency(dr("BALANCEFORWARD")), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<AMOUNT>", FormatCurrency(dr("amountbilled")), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<BALANCE>", FormatCurrency(dr("BALANCE")), oDoc.PendingWordDocumentID)
                    Add_Item_Value("<TOTALDUE>", FormatCurrency(dr("TOTALDUE")), oDoc.PendingWordDocumentID)
                    'Add_Item_Value("<TRI>", " ", oDoc.PendingWordDocumentID)
                    oDoc.Ready = True
                    oDoc.Save()
                    Response.Write(oDoc.Error_Message)
                Loop
            End If

        End If

        oDoc = Nothing
        oItems = Nothing
        cn = Nothing
        dr = Nothing
        sql = Nothing


    End Sub




    Private Sub Add_Item_Value(ByVal sBookMark As String, ByVal sValue As String, ByVal DocID As Integer)
        Dim oItems As New clsPendingWordDocumentValues
        oItems.PendingWordDocumentValueID = 0
        oItems.Load()
        oItems.BookMark = sBookMark
        oItems.Value = sValue
        oItems.PendingWordDocumentID = DocID
        oItems.Save()
        oItems = Nothing
    End Sub

    Protected Sub btnLFPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLFPreview.Click
        If Not (IsNumeric(txtLFAmount.Text)) Or txtLFAmount.Text = "" Or Not (IsNumeric(txtDays.Text)) Or txtdays.text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "missing", "alert('Please enter a valid Late Fee Amount and Days Late.');", True)
            Exit Sub
        End If
        Dim sql As String = "select p.lastname as [Last Name], p.firstname as [First Name], c.contractnumber as [KCP#], i.Invoice as TransCode, i.amount as [AmountInvoiced], i.balance as Balance, " & txtLFAmount.Text & " as [LFAmount],  inv.saletype as Type, cs.comboitem as Status, css.comboitem as SubStatus, mfs.comboitem as MFStatus, c.ContractID " & _
                            "from v_Invoices i  " & _
                            "	inner join t_Contract c on c.contractid = i.keyvalue " & _
                            "	inner join t_Prospect p on p.prospectid = c.prospectid " & _
                            "	left outer join v_ContractInventory inv on inv.contractid = c.contractid " & _
                            "   left outer join t_Comboitems cs on cs.comboitemid = c.statusid " & _
                            "   left outer join t_Comboitems css on css.comboitemid = c.substatusid " & _
                            "   left outer join t_Comboitems mfs on mfs.comboitemid = c.maintenancefeestatusid " & _
                            "where  c.statusid not in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname='ContractStatus' and i.comboitem like 'C%') and i.invoice = '" & ddLFYear.SelectedItem.Text & "'  " & _
                            "	and  i.balance -100 >  " & _
                            "		case when inv.frequency <> 'Triennial' then  " & _
                            "           0 " & _
                            "		else  " & _
                            "			case when i.amount = i.balance then 0 else i.amount - ((i.amount / 3.00) * (year(getdate())- (2000 + cast(right(i.invoice,2) as int)-1))) end  " & _
                            "        End " & _
                            "	and i.id not in ( " & _
                            "  Select m.mfinvoiceid " & _
                            "		from t_lf2mf m inner join t_Invoices i on i.invoiceid = m.lfinvoiceid  " & _
                            "           inner join t_Contract c on c.contractid = i.keyvalue " & _
                            "           inner join t_Frequency f on f.frequencyid = c.frequencyid " & _
                            "		where year(i.transdate) = case when f.frequency = 'Triennial' then year(getdate()) else 2000 + cast(right('" & ddLFYear.SelectedItem.Text & "',2) as int) end " & _
                            "	)  " & _
                            "	and datediff(dd,duedate,getdate())>= " & txtDays.Text & _
                            " union " & _
                            "select p.lastname as [Last Name], p.firstname as [First Name], c.contractnumber as [KCP#], i.Invoice as TransCode, i.amount as [AmountInvoiced], i.balance as Balance, " & txtLFAmount.Text & " as [LFAmount],  inv.saletype as Type, cs.comboitem as Status, css.comboitem as SubStatus, mfs.comboitem as MFStatus, c.ContractID " & _
                            "from v_Invoices i  " & _
                            "	inner join t_Contract c on c.contractid = i.keyvalue " & _
                            "	inner join t_Prospect p on p.prospectid = c.prospectid " & _
                            "	left outer join v_ContractInventory inv on inv.contractid = c.contractid " & _
                            "   left outer join t_Comboitems cs on cs.comboitemid = c.statusid " & _
                            "   left outer join t_Comboitems css on css.comboitemid = c.substatusid " & _
                            "   left outer join t_Comboitems mfs on mfs.comboitemid = c.maintenancefeestatusid " & _
                            "where c.statusid not in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname='ContractStatus' and i.comboitem like 'C%') and i.invoice = 'MF' + cast(cast(right('" & ddLFYear.SelectedItem.Text & "', 2) as int) -1 as varchar)  " & _
                            "	and  i.balance -100 >  " & _
                            "		case when inv.frequency <> 'Triennial' then  " & _
                            "           0 " & _
                            "		else  " & _
                            "			case when i.amount = i.balance then 0 else i.amount - ((i.amount / 3.00) * (year(getdate())- (2000 + cast(right(i.invoice,2) as int)-1))) end  " & _
                            "        End " & _
                            "	and i.id not in ( " & _
                            "  Select m.mfinvoiceid " & _
                            "		from t_lf2mf m inner join t_Invoices i on i.invoiceid = m.lfinvoiceid  " & _
                            "           inner join t_Contract c on c.contractid = i.keyvalue " & _
                            "           inner join t_Frequency f on f.frequencyid = c.frequencyid " & _
                            "		where year(i.transdate) = case when f.frequency = 'Triennial' then year(getdate()) else 2000 + cast(right('MF' + cast(cast(right('" & ddLFYear.SelectedItem.Text & "', 2) as int) -1 as varchar),2) as int) end " & _
                            "	)  " & _
                            "	and datediff(dd,duedate,getdate())>= " & txtDays.Text & _
                            " union " & _
                            "select p.lastname as [Last Name], p.firstname as [First Name], c.contractnumber as [KCP#], i.Invoice as TransCode, i.amount as [AmountInvoiced], i.balance as Balance, " & txtLFAmount.Text & " as [LFAmount],  inv.saletype as Type, cs.comboitem as Status, css.comboitem as SubStatus, mfs.comboitem as MFStatus, c.ContractID " & _
                            "from v_Invoices i  " & _
                            "	inner join t_Contract c on c.contractid = i.keyvalue " & _
                            "	inner join t_Prospect p on p.prospectid = c.prospectid " & _
                            "	left outer join v_ContractInventory inv on inv.contractid = c.contractid " & _
                            "   left outer join t_Comboitems cs on cs.comboitemid = c.statusid " & _
                            "   left outer join t_Comboitems css on css.comboitemid = c.substatusid " & _
                            "   left outer join t_Comboitems mfs on mfs.comboitemid = c.maintenancefeestatusid " & _
                            "where  c.statusid not in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where c.comboname='ContractStatus' and i.comboitem like 'C%') and i.invoice = 'MF' + cast(cast(right('" & ddLFYear.SelectedItem.Text & "', 2) as int) -2 as varchar)  " & _
                            "	and  i.balance -100 >  " & _
                            "		case when inv.frequency <> 'Triennial' then  " & _
                            "           0 " & _
                            "		else  " & _
                            "			case when i.amount = i.balance then 0 else i.amount - ((i.amount / 3.00) * (year(getdate())- (2000 + cast(right(i.invoice,2) as int)-1))) end  " & _
                            "        End " & _
                            "	and i.id not in ( " & _
                            "  Select m.mfinvoiceid " & _
                            "		from t_lf2mf m inner join t_Invoices i on i.invoiceid = m.lfinvoiceid  " & _
                            "           inner join t_Contract c on c.contractid = i.keyvalue " & _
                            "           inner join t_Frequency f on f.frequencyid = c.frequencyid " & _
                            "		where year(i.transdate) = case when f.frequency = 'Triennial' then year(getdate()) else 2000 + cast(right('MF' + cast(cast(right('" & ddLFYear.SelectedItem.Text & "', 2) as int) -2 as varchar),2) as int) end " & _
                            "	)  " & _
                            "	and datediff(dd,duedate,getdate())>= " & txtDays.Text & _
" order by p.lastname, p.firstname"
        Dim ds As New SqlDataSource(Resources.Resource.cns, sql)
        gvLF.DataSource = ds
        gvLF.DataBind()
        gvLF.Visible = True
        gvDues.Visible = False
        gvCDLF.Visible = False
        gvResults.Visible = False
        cbSelect.Visible = True
        lblCheck.Visible = True
        cbSelect.Checked = True
        ds = Nothing
    End Sub

    Protected Sub btnLFExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLFExport.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & IIf(rblType.SelectedValue = "MF", "Maintenance Fee Assessment ", "Late Fee Assessment ") & "for " & ddYear.SelectedItem.Text & ".xls")
        Response.Write("<table>")
        Response.Write("<tr>")
        For i = 0 To gvLF.Columns.Count - 1
            Response.Write("<th>")
            Response.Write(gvLF.Columns(i).HeaderText)
            Response.Write("</th>")
        Next
        Response.Write("</tr>")
        For Each ro As GridViewRow In gvLF.Rows
            Response.Write("<tr>")
            For i = 0 To ro.Cells.Count - 1
                Response.Write("<td>")
                Response.Write(ro.Cells(i).Text)
                Response.Write("</td>")
            Next
            Response.Write("</tr>")
        Next
        Response.Write("</table>")
        Response.End()
    End Sub

    Protected Sub btnLFAssess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLFAssess.Click
        'Exit Sub
        Dim oFT As New clsFinancialTransactionCodes
        Dim oCon As clsContract
        Dim oInv As clsInvoices
        Dim ftc As Integer = oFT.Find_Fin_Trans("LFTrans", "LATE FEE")
        Dim conid As Integer = 0
        Dim invID As Integer = 0

        For Each oRow As GridViewRow In gvLF.Rows
            If Not (oRow.FindControl("cb") Is Nothing) Then
                If CType(oRow.FindControl("cb"), CheckBox).Checked Then
                    oInv = New clsInvoices
                    oCon = New clsContract
                    'Get Contractid
                    oCon.ContractNumber = oRow.Cells(3).Text
                    oCon.Load()
                    conid = oCon.ContractID
                    invID = 0
                    If CDbl(oRow.Cells(5).Text) > 0 Then 'Find the existing Invoice
                        invID = oInv.Find_Existing_Invoice("ContractID", conid, ddLFYear.SelectedItem.Text)
                    End If
                    oInv.Load()
                    oInv.Amount = oRow.Cells(8).Text
                    oInv.ApplyToID = 0
                    oInv.Adjustment = False
                    oInv.Description = ddLFYear.SelectedItem.Text & " Late Fee"
                    oInv.PosNeg = 0
                    oInv.DueDate = Date.Today
                    oInv.FinTransID = ftc
                    oInv.ProspectID = oCon.ProspectID
                    oInv.KeyField = "ContractID"
                    oInv.KeyValue = conid
                    oInv.Reference = "LF" & Right(ddLFYear.SelectedItem.Text, 2)
                    oInv.TransDate = Date.Now
                    oInv.UserID = Session("UserDBID")
                    oInv.Save()
                    Dim oLF2MF As New clsLF2MF
                    oLF2MF.LFInvoiceID = oInv.InvoiceID
                    oLF2MF.MFInvoiceID = invID
                    oLF2MF.Save()
                    oLF2MF = Nothing
                    Create_Document(conid)
                    oInv = Nothing
                    oCon = Nothing
                End If
            End If
        Next
        oInv = Nothing
        oFT = Nothing
        oCon = Nothing
    End Sub

    Protected Sub Preview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Preview.Click
        Dim cdIndex As Integer = ClubFees.SelectedItem.Value
        Dim sDate As String = AssessDate.Selected_Date
        Dim dDate As String = DueDate.Selected_Date
        Dim sYear As String = "20" + ClubFees.SelectedItem.Text.Substring(2, 2)
        'Dim sql As String = "SELECT     ProspectID, FirstName, LastName, AnniversaryDate, Frequency, AmountInvoiced, AssessmentAmount, AmountToAssess " & _
        '                    "    FROM         (SELECT     ProspectID, FirstName, LastName, AnniversaryDate, " & _
        '                    "                                                      (SELECT     CASE WHEN xx.Freq = 1 THEN 'Annual' WHEN xx.Freq = 2 THEN 'Biennial' ELSE 'Triennial' END AS Expr1) AS Frequency, AmountInvoiced, " & _
        '                    "                                                     (SELECT     CASE WHEN xx.freq = 1 THEN 179 WHEN xx.freq = 2 THEN 132 ELSE 117 END AS Expr1) AS AssessmentAmount, " & _
        '                    "                                                      (SELECT     CASE WHEN xx.freq = 1 THEN 179 - xx.AmountInvoiced WHEN xx.freq = 2 THEN 132 - xx.AmountInvoiced ELSE 117 - xx.AmountInvoiced END AS " & _
        '                    "                                                                                Expr1) AS AmountToAssess " & _
        '                    "                           FROM          (SELECT DISTINCT p.ProspectID, p.FirstName, p.LastName, " & _
        '                    "                                                                              (SELECT     TOP 1 AnniversaryDate " & _
        '                    "            FROM(t_Contract) " & _
        '                    "                                                                                WHERE      (ProspectID = c.ProspectID) AND (AnniversaryDate <= '" & sDate & "') " & _
        '                    "                                                                                ORDER BY AnniversaryDate) AS AnniversaryDate, " & _
        '                    "                                                                              (SELECT     TOP 1 cx.FrequencyID " & _
        '                    "                                                                                FROM          t_Contract AS cx INNER JOIN " & _
        '                    "                                                                                                       t_ComboItems AS cst ON cx.SubTypeID = cst.ComboItemID INNER JOIN " & _
        '                    "                                                                                                       t_ComboItems AS cxs ON cx.StatusID = cxs.ComboItemID " & _
        '                    "                                                                                WHERE      (cx.ProspectID = c.ProspectID) AND (cxs.ComboItem <> 'Kick') AND (cst.ComboItem = 'Points') " & _
        '                    "                                                                                ORDER BY cx.FrequencyID) AS Freq, " & _
        '                    "                                                                              (SELECT     COALESCE (SUM(Amount), 0) AS Expr1 " & _
        '                    "            FROM(t_Invoices) " & _
        '                    "                                                                                WHERE      (KeyField = 'ProspectID') AND (KeyValue = c.ProspectID) AND (ApplyToID = 0) AND (FinTransID = " & cdIndex & "))  " & _
        '                    "                                                                          AS AmountInvoiced " & _
        '                    "                                                   FROM          t_Contract AS c INNER JOIN " & _
        '                    "                                                                          t_Prospect AS p ON c.ProspectID = p.ProspectID INNER JOIN " & _
        '                    "                                                                          t_ComboItems AS st ON c.SubTypeID = st.ComboItemID INNER JOIN " & _
        '                    "                                                                          t_ComboItems AS cs ON c.StatusID = cs.ComboItemID " & _
        '                    "                                                   WHERE      (c.AnniversaryDate < '" & sDate & "') AND (cs.ComboItem <> 'Kick') AND (st.ComboItem = 'Points')) AS xx) AS jj " & _
        '                    "            WHERE AmountToAssess > 0 "
        'Dim sql As String = "SELECT     ProspectID, FirstName, LastName, AnniversaryDate, Frequency, AmountInvoiced, AssessmentAmount, AmountToAssess " & _
        '                    "    FROM         (SELECT     ProspectID, FirstName, LastName, AnniversaryDate, " & _
        '                    "                                                      (SELECT     CASE WHEN xx.Freq = 1 THEN 'Annual' WHEN xx.Freq = 2 THEN 'Biennial' ELSE 'Triennial' END AS Expr1) AS Frequency, AmountInvoiced, " & _
        '                    "                                                     (SELECT     CASE WHEN xx.freq = 1 THEN 179 WHEN xx.freq = 2 THEN 132 ELSE 117 END AS Expr1) AS AssessmentAmount, " & _
        '                    "                                                      (SELECT     CASE WHEN xx.freq = 1 THEN 179 - xx.AmountInvoiced WHEN xx.freq = 2 THEN 132 - xx.AmountInvoiced ELSE 117 - xx.AmountInvoiced END AS " & _
        '                    "                                                                                Expr1) AS AmountToAssess " & _
        '                    "                           FROM          (SELECT DISTINCT p.ProspectID, p.FirstName, p.LastName, " & _
        '                    "                                                                              (SELECT     TOP 1 AnniversaryDate " & _
        '                    "            FROM t_Contract con inner join t_ComboItems const on con.SubTypeID = const.ComboItemID " & _
        '                    "                                                                                WHERE      (ProspectID = c.ProspectID) AND const.ComboItem = 'Points' and (AnniversaryDate <= '" & sDate & "') " & _
        '                    "                                                                                ORDER BY AnniversaryDate) AS AnniversaryDate, " & _
        '                    "                                                                              (SELECT     TOP 1 cx.FrequencyID " & _
        '                    "                                                                                FROM          t_Contract AS cx INNER JOIN " & _
        '                    "                                                                                                       t_ComboItems AS cst ON cx.SubTypeID = cst.ComboItemID INNER JOIN " & _
        '                    "                                                                                                       t_ComboItems AS cxs ON cx.StatusID = cxs.ComboItemID " & _
        '                    "                                                                                WHERE      (cx.ProspectID = c.ProspectID) AND (cxs.ComboItem <> 'Kick') AND (cst.ComboItem = 'Points') " & _
        '                    "                                                                                ORDER BY cx.FrequencyID) AS Freq, " & _
        '                    "                                                                              (SELECT     COALESCE (SUM(InvoiceAmount), 0) AS Expr1 " & _
        '                    "                                                                                FROM          v_Invoices " & _
        '                    "                                                WHERE      (KeyValue = p.ProspectID) AND (Invoice = '" & ClubFees.SelectedItem.Text & "')) " & _
        '                    "                                                                          AS AmountInvoiced " & _
        '                    "                                                   FROM          t_Contract AS c INNER JOIN " & _
        '                    "                                                                          t_Prospect AS p ON c.ProspectID = p.ProspectID INNER JOIN " & _
        '                    "                                                                          t_ComboItems AS st ON c.SubTypeID = st.ComboItemID INNER JOIN " & _
        '                    "                                                                          t_ComboItems AS cs ON c.StatusID = cs.ComboItemID " & _
        '                    "                                                   WHERE      (c.AnniversaryDate < '" & sDate & "') AND (cs.ComboItem <> 'Kick') AND (st.ComboItem = 'Points')) AS xx) AS jj " & _
        '                    "            WHERE AmountToAssess > 0 and prospectid in (Select ProspectID from t_Contract c inner join t_ComboItems cst on c.SubtypeID = cst.ComboItemID inner join t_ComboItems cs on c.StatusID = cs.ComboitemID where cst.ComboItem = 'Points' and cs.ComboItem in ('Active','Suspense','ReDeed','Developer','Reverter','InColl-Active','In Foreclosure','In Banckruptcy')) order by anniversarydate "
        Dim sql As String = "select *,0 as [AmountInvoiced], case when Frequency = 'Annual' then 179 when Frequency = 'Biennial' then 132 else 117 end as [AssessmentAmount],case when Frequency = 'Annual' then 179 when Frequency = 'Biennial' then 132 else 117 end as [AmounttoAssess] " & _
                            "from ( " & _
                             "select distinct p.ProspectID, p.Lastname as [LastName], p.firstname as [FirstName],  " & _
                              "a.an as [AnniversaryDate], " & _
                              "( " & _
                               "select top 1 f.frequency " & _
                               "from t_Contract c " & _
                                "inner join t_Comboitems cs on cs.comboitemid = c.statusid " & _
                                "inner join t_Soldinventory s on s.contractid = c.contractid " & _
                                "inner join t_Frequency f on f.frequencyid = s.frequencyid " & _
                               "where cs.comboitem in ('Active', 'Suspense','Developer','ReDeed','In Bankruptcy','In Foreclosure','Reverter','InColl-Active', 'Deed-In-Lieu', 'InColl-Developer','Pending ReDeed') " & _
                                "and c.prospectid = p.prospectid " & _
                               "order by f.Frequency  " & _
                              ") as Frequency, " & _
                              "( " & _
                              "	select top 1 cs.comboitem " & _
                              "	from t_Contract c " & _
                              "		inner join t_Comboitems cs on cs.comboitemid = c.statusid " & _
                              "		inner join t_Soldinventory s on s.contractid = c.contractid " & _
                              "		inner join t_Frequency f on f.frequencyid = s.frequencyid " & _
                              "	where cs.comboitem in ('Active', 'Suspense','Developer','ReDeed','In Bankruptcy','In Foreclosure','Reverter','InColl-Active', 'Deed-In-Lieu', 'InColl-Developer','Pending ReDeed') " & _
                              "		and c.prospectid = p.prospectid " & _
                              "	order by c.contractid desc " & _
                              ") as MostRecentContractStatus " & _
                             "from t_Prospect p  " & _
                              "inner join ( " & _
                                "select distinct p.prospectid, min(p.anniversarydate) as AN " & _
                                "from t_Prospect p  " & _
                                 "inner join t_Contract c on c.prospectid = p.prospectid " & _
                                 "inner join t_Comboitems cs on cs.comboitemid = c.statusid " & _
                                 "inner join t_Comboitems st on st.comboitemid = c.subtypeid " & _
                                "where p.anniversarydate is not null " & _
                                 "and cs.comboitem not in ('Rescinded','CXL-Pender') " & _
                                 "and st.comboitem = 'Points' " & _
                                "group by p.prospectid " & _
                               ") a on a.prospectid = p.prospectid " & _
                             "where year(a.an) < " & sYear & " " & _
                              "and cast(cast(datepart(mm, a.an) as varchar) + '/1/' + cast(" & sYear & " as varchar) as datetime) < cast('" & sDate & "' as datetime) " & _
                              "and p.prospectid not in ( " & _
                                "select prospectid  " & _
                                "from v_Invoices  " & _
                                "where invoice = 'CD' + right(cast(" & sYear & " as varchar),2) and amount > 0 " & _
                               ") " & _
                              "and p.prospectid in ( " & _
                                "select distinct prospectid  " & _
                                "from t_Contract c  " & _
                                 "inner join t_Comboitems cs on cs.comboitemid = c.statusid  " & _
                                 "inner join t_Soldinventory s on s.contractid = c.contractid " & _
                                 "inner join t_Comboitems st on st.comboitemid = c.subtypeid " & _
                                "where cs.comboitem in ('Active', 'Suspense','Developer','ReDeed','In Bankruptcy','In Foreclosure','Reverter','InColl-Active', 'Deed-In-Lieu', 'InColl-Developer','Pending ReDeed') " & _
                                 "and st.comboitem = 'Points' " & _
                               ") " & _
                            ") a"
        'cdIndex = 183
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        'ds.SelectCommand = "Select jj.* from (Select xx.ProspectID, xx.Firstname, xx.LastName, xx.AnniversaryDate, (Select Case when xx.Freq = 1 then 'Annual' when xx.Freq = 2 then 'Biennial' else 'Triennial' end) as Frequency, xx.AmountInvoiced, (Select Case when xx.freq = 1 then 179 when xx.freq = 2 then 132 else 117 end) as AssessmentAmount, (Select Case when xx.freq = 1 then 179 - xx.AmountInvoiced when xx.freq = 2 then 132 - xx.AmountInvoiced else 117 - xx.AmountInvoiced end) as AmountToAssess from (Select Distinct(p.ProspectID), p.FirstName, p.LastName, (Select Top 1 AnniversaryDate from t_Contract where prospectID = c.ProspectID and anniversarydate <= '" & sDate & "' order by anniversaryDate asc) as AnniversaryDate, (Select Top 1 FrequencyID from t_Contract cx inner join t_CombOItems cst on cx.SubTypeID = cst.ComboItemID inner join t_ComboItems cxs on cx.StatusID = cxs.ComboItemID where prospectID = c.ProspectID and cxs.ComboItem <> 'Kick' and cst.ComboItem = 'Points' order by cx.FrequencyID asc) as Freq, (Select coalesce(Sum(Amount),0) From t_Invoices where keyfield = 'ProspectID' and keyvalue = c.ProspectID and applytoid = 0 and fintransid = " & cdIndex & ") as AmountInvoiced from t_Contract c inner join t_prospect p on c.ProspectID = p.ProspectID inner join t_ComboItems st on c.SubTypeID = st.ComboItemID inner join t_ComboItems cs on c.StatusID = cs.ComboItemID where c.AnniversaryDate < '" & sDate & "' and cs.ComboItem <> 'Kick' and st.ComboItem = 'Points') xx) jj where amounttoassess > 0"
        ds.SelectCommand = Sql
        'Response.Write(sql)
        gvDues.DataSource = ds
        gvDues.DataBind()
        gvResults.Visible = False
        gvCDLF.Visible = False
        gvLF.Visible = False
        gvDues.Visible = True
        cbSelect.Visible = True
        lblCheck.Visible = True
        cbSelect.Checked = True

        'If gvDues.Rows.Count > 1 Then
        'Get the header CheckBox
        'Dim cbHeader As CheckBox = CType(gvDues.HeaderRow.FindControl("cbAll"), CheckBox)

        'Run the ChangeCheckBoxState client-side function whenever the
        'header checkbox is checked/unchecked
        'cbHeader.Attributes("onclick") = "ChangeAllCheckBoxStates(this.checked);"

        'For Each gvr As GridViewRow In gvDues.Rows
        'Get a programmatic reference to the CheckBox control
        'Dim cbA As CheckBox = CType(gvr.FindControl("cb"), CheckBox)

        'Add the CheckBox's ID to the client-side CheckBoxIDs array
        'ClientScript.RegisterArrayDeclaration("CheckBoxIDs", String.Concat("'", cbA.ClientID, "'"))
        'Next
        'End If
    End Sub



    Protected Sub Assess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Assess.Click
        Dim oInv As clsInvoices
        Dim invID As Integer = 0

        For Each oRow As GridViewRow In gvDues.Rows
            If Not (oRow.FindControl("cb") Is Nothing) Then
                If CType(oRow.FindControl("cb"), CheckBox).Checked Then
                    oInv = New clsInvoices
                    invID = 0
                    If CDbl(oRow.Cells(6).Text) > 0 Then 'Find the existing Invoice
                        invID = oInv.Find_Existing_Invoice("ProspectID", oRow.Cells(1).Text, ClubFees.SelectedItem.Text)
                    End If
                    oInv.Load()
                    oInv.Amount = oRow.Cells(8).Text
                    oInv.ApplyToID = invID
                    oInv.Adjustment = IIf(invID = 0, False, True)
                    oInv.Description = ClubFees.SelectedItem.Text & " Club Dues"
                    oInv.PosNeg = 0
                    oInv.DueDate = DueDate.Selected_Date
                    oInv.FinTransID = ClubFees.SelectedItem.Value
                    oInv.ProspectID = oRow.Cells(1).Text
                    oInv.KeyField = "ProspectID"
                    oInv.KeyValue = oRow.Cells(1).Text
                    oInv.Reference = "CD" & ClubFees.SelectedItem.Text.Substring(2)
                    oInv.TransDate = Date.Now
                    oInv.UserID = Session("UserDBID")
                    oInv.Save()
                    'If ckPrint.Checked Then CreateDocument(oRow.Cells(1).Text)
                    CreateDocument(oRow.Cells(1).Text)
                    'Create_Document(conid)
                    oInv = Nothing
                End If
            End If
        Next
        oInv = Nothing
    End Sub

    Protected Sub gvCDLF_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCDLF.RowDataBound
        If e.Row.Cells(0).Text = "No Records" Then
            e.Row.Cells(0).Visible = False
        Else

        End If
    End Sub

    Protected Sub gvDues_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDues.RowDataBound
        If e.Row.Cells(0).Text = "No Records" Then
            e.Row.Cells(0).Visible = False
        Else

        End If
    End Sub

    Protected Sub Export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Export.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & IIf(rblType.SelectedValue = "CD", "Club Fee Assessment ", "Late Fee Assessment ") & "for " & ClubFees.SelectedItem.Text & ".xls")
        Response.Write("<table>")
        Response.Write("<tr>")
        For i = 0 To gvDues.Columns.Count - 1
            Response.Write("<th>")
            Response.Write(gvDues.Columns(i).HeaderText)
            Response.Write("</th>")
        Next
        Response.Write("</tr>")
        For Each ro As GridViewRow In gvDues.Rows
            Response.Write("<tr>")
            For i = 0 To ro.Cells.Count - 1
                Response.Write("<td>")
                Response.Write(ro.Cells(i).Text)
                Response.Write("</td>")
            Next
            Response.Write("</tr>")
        Next
        Response.Write("</table>")
        Response.End()
    End Sub

    Protected Sub ckPrint_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ckPrint.CheckedChanged

    End Sub

    Protected Sub btnLFCFPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLFCFPreview.Click
        If Not (IsNumeric(txtCDLFAMt.Text)) Or txtCDLFAMt.Text = "" Or Not (IsNumeric(txtDaysLate.Text)) Or txtDaysLate.Text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "missing", "alert('Please enter a valid Late Fee Amount and Days Late.');", True)
            Exit Sub
        End If
        Dim sql As String = "select p.ProspectID, p.lastname as [Last Name], p.firstname as [First Name], i.Invoice as TransCode, i.amount as [AmountInvoiced], i.balance as Balance, " & txtCDLFAMt.Text & " as [LFAmount] " & _
                            "from v_Invoices i  " & _
                            "	inner join t_Prospect p on p.prospectid = i.prospectid " & _
                            "   where i.invoice = '" & ddClubFee.SelectedItem.Text & "'  " & _
                            "	and i.balance > 0 " & _
                            "	and i.id not in ( " & _
                            "  Select cfinvoiceid " & _
                            "		from t_lf2cf " & _
                            "	)  " & _
                            "	and datediff(dd,duedate,getdate())>= " & txtDaysLate.Text & _
                            " order by p.lastname, p.firstname"
        Dim ds As New SqlDataSource(Resources.Resource.cns, sql)
        gvCDLF.DataSource = ds
        gvCDLF.DataBind()


        'If gvCDLF.Rows.Count > 1 Then
        'Get the header CheckBox

        'Dim cbHeaderA As CheckBox = CType(gvCDLF.HeaderRow.FindControl("cbLFAll"), CheckBox)

        'Run the ChangeCheckBoxState client-side function whenever the
        'header checkbox is checked/unchecked

        'cbHeaderA.Attributes("onclick") = "ChangeAllLFCheckBoxStates(this.checked);"

        'For Each gvr As GridViewRow In gvCDLF.Rows
        'Get a programmatic reference to the CheckBox control

        '                Dim cbB As CheckBox = CType(gvCDLF.FindControl("cb"), CheckBox)

        'Add the CheckBox's ID to the client-side CheckBoxIDs array

        'ClientScript.RegisterArrayDeclaration("CheckBoxLFIDs", String.Concat("'", cbB.ClientID, "'"))
        'Next
        'End If
        gvCDLF.Visible = True
        gvResults.Visible = False
        gvLF.Visible = False
        gvDues.Visible = False
        cbSelect.Visible = True
        lblCheck.Visible = True
        cbSelect.Checked = True

        ds = Nothing


    End Sub

    Protected Sub btnLFCFAssess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLFCFAssess.Click
        Dim oFT As New clsFinancialTransactionCodes
        Dim oCon As clsContract
        Dim oInv As clsInvoices
        Dim ftc As Integer = oFT.Find_Fin_Trans("LFTrans", "LATE FEE DUES")
        Dim invID As Integer = 0

        For Each oRow As GridViewRow In gvCDLF.Rows
            If Not (oRow.FindControl("cb") Is Nothing) Then
                If CType(oRow.FindControl("cb"), CheckBox).Checked Then
                    oInv = New clsInvoices
                    invID = 0
                    If CDbl(oRow.Cells(5).Text) > 0 Then 'Find the existing Invoice
                        invID = oInv.Find_Existing_Invoice("ProspectID", oRow.Cells(1).Text, ddClubFee.SelectedItem.Text)
                    End If
                    oInv.Load()
                    oInv.Amount = oRow.Cells(7).Text
                    oInv.ApplyToID = 0
                    oInv.Adjustment = False
                    oInv.Description = ddClubFee.SelectedItem.Text & " Late Fee"
                    oInv.PosNeg = 0
                    oInv.DueDate = Date.Today
                    oInv.FinTransID = ftc
                    oInv.ProspectID = oRow.Cells(1).Text
                    oInv.KeyField = "ProspectID"
                    oInv.KeyValue = oRow.Cells(1).Text
                    oInv.Reference = "LFCD" & Right(ddClubFee.SelectedItem.Text, 2)
                    oInv.TransDate = Date.Now
                    oInv.UserID = Session("UserDBID")
                    oInv.Save()
                    Dim oLF2CF As New clsLF2CF
                    oLF2CF.LFInvoiceID = oInv.InvoiceID
                    oLF2CF.CFInvoiceID = invID
                    oLF2CF.Save()
                    oLF2CF = Nothing
                    CreateDocument(oRow.Cells(1).Text)
                    oInv = Nothing
                    oCon = Nothing
                End If
            End If
        Next
        oInv = Nothing
        oFT = Nothing
        oCon = Nothing
    End Sub


    Protected Sub btnLFCFExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLFCFExport.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & IIf(rblType.SelectedValue = "CD", "Club Fee Assessment ", "Late Fee Assessment ") & "for " & ClubFees.SelectedItem.Text & ".xls")
        Response.Write("<table>")
        Response.Write("<tr>")
        For i = 0 To gvCDLF.Columns.Count - 1
            Response.Write("<th>")
            Response.Write(gvCDLF.Columns(i).HeaderText)
            Response.Write("</th>")
        Next
        Response.Write("</tr>")
        For Each ro As GridViewRow In gvCDLF.Rows
            Response.Write("<tr>")
            For i = 0 To ro.Cells.Count - 1
                Response.Write("<td>")
                Response.Write(ro.Cells(i).Text)
                Response.Write("</td>")
            Next
            Response.Write("</tr>")
        Next
        Response.Write("</table>")
        Response.End()
    End Sub
End Class
