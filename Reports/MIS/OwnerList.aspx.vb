Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel

Partial Class Reports_MIS_OwnerList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            pnlFreq.Visible = False
            pnlPhase.Visible = False
            pnlSeason.Visible = False
            pnlState.Visible = False
            pnlStatus.Visible = False
            pnlSubType.Visible = False
            pnlEmail.Visible = False
            For i As Integer = -5 To 5
                Dim lItem As New ListItem
                lItem.Value = Date.Now.Year + i
                lItem.Text = (Date.Now.Year + i).ToString
                ddOcc.Items.Add(lItem)
            Next

            Dim ds As New SqlDataSource(Resources.Resource.cns, "")
            ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'State' order by comboitem"
            lbStates.DataSource = ds
            lbStates.DataValueField = "ComboItemID"
            lbStates.DataTextField = "ComboItem"
            lbStates.DataBind()
            'ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'Phase' order by comboitem"
            'lbPhase.DataSource = ds
            'lbPhase.DataValueField = "ComboItemID"
            'lbPhase.DataTextField = "ComboItem"
            'lbPhase.DataBind()
            ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'Season' order by comboitem"
            lbSeason.DataSource = ds
            lbSeason.DataValueField = "ComboItemID"
            lbSeason.DataTextField = "ComboItem"
            lbSeason.DataBind()
            ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'ContractStatus' order by comboitem"
            lbStatus.DataSource = ds
            lbStatus.DataValueField = "ComboItemID"
            lbStatus.DataTextField = "ComboItem"
            lbStatus.DataBind()
            ds.SelectCommand = "Select * from t_Frequency order by Frequency"
            lbFreq.DataSource = ds
            lbFreq.DataValueField = "FrequencyID"
            lbFreq.DataTextField = "Frequency"
            lbFreq.DataBind()
            ds.SelectCommand = "Select * from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'ContractSubType' order by comboitem"
            lbSubType.DataSource = ds
            lbSubType.DataValueField = "ComboItemID"
            lbSubType.DataTextField = "ComboItem"
            lbSubType.DataBind()
            ds = Nothing
        End If
    End Sub

    Protected Sub cbState_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbState.CheckedChanged
        pnlState.Visible = cbState.Checked
    End Sub

    Protected Sub cbPhase_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbPhase.CheckedChanged
        pnlPhase.Visible = cbPhase.Checked
    End Sub

    Protected Sub cbSeason_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbSeason.CheckedChanged
        pnlSeason.Visible = cbSeason.Checked
    End Sub

    Protected Sub cbFreq_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbFreq.CheckedChanged
        pnlFreq.Visible = cbFreq.Checked
    End Sub

    Protected Sub cbStatus_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbStatus.CheckedChanged
        pnlStatus.Visible = cbStatus.Checked
    End Sub

    Protected Sub cbSubType_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbSubType.CheckedChanged
        pnlSubType.Visible = cbSubType.Checked
    End Sub

    Protected Sub btnAddState_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddState.Click
        Transfer_Item(lbStates, lbStates_Selected)
    End Sub

    Private Sub Transfer_Item(ByRef lbSource As ListBox, ByRef lbDestination As ListBox)
        If lbSource.SelectedIndex >= 0 Then
            Dim lItem As New ListItem
            lItem.Value = lbSource.SelectedValue
            lItem.Text = lbSource.SelectedItem.Text
            lbDestination.Items.Add(lItem)
            lbSource.Items.Remove(lItem)
        End If
    End Sub

    Protected Sub btnRemoveState_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveState.Click
        Transfer_Item(lbStates_Selected, lbStates)
    End Sub

    Protected Sub btnAddPhase_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPhase.Click
        Transfer_Item(lbPhase, lbPhase_Selected)
    End Sub

    Protected Sub btnRemovePhase_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemovePhase.Click
        Transfer_Item(lbPhase_Selected, lbPhase)
    End Sub

    Protected Sub btnAddSeason_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddSeason.Click
        Transfer_Item(lbSeason, lbSeason_Selected)
    End Sub

    Protected Sub btnRemoveSeason_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveSeason.Click
        Transfer_Item(lbSeason_Selected, lbSeason)
    End Sub

    Protected Sub btnAddStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddStatus.Click
        Transfer_Item(lbStatus, lbStatus_Selected)
    End Sub

    Protected Sub btnRemoveStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveStatus.Click
        Transfer_Item(lbStatus_Selected, lbStatus)
    End Sub

    Protected Sub btnAddFreq_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddFreq.Click
        Transfer_Item(lbFreq, lbFreq_Selected)
    End Sub

    Protected Sub btnRemFreq_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemFreq.Click
        Transfer_Item(lbFreq_Selected, lbFreq)
    End Sub

    Protected Sub btnAddSubType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddSubType.Click
        Transfer_Item(lbSubType, lbSubType_Selected)
    End Sub

    Protected Sub btnRemSubType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemSubType.Click
        Transfer_Item(lbSubType_Selected, lbSubType)
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Report()
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        'Dim cn As New SqlConnection(resources.resource.cns)
        'Dim cm As New SqlCommand("", cn)
        'Dim da As New SqlDataAdapter(cm)
        'Dim ds As New DataSet

        Lit.Text = ""

        Server.ScriptTimeout = 10000


        Dim sql As String = "Select * from UFN_OWNERSLIST(" & ddOcc.SelectedValue & ",0) "
        Dim sql2 As String = "Select * from UFN_COOWNERSLIST(" & ddOcc.SelectedValue & ",0) "
        Dim sql3 As String = "Select * from ufn_MFTriOwnersList(" & ddOcc.SelectedValue & ",0) "

        If cbEmail.Checked Then
            If rblEmail.SelectedValue = "0" Then
                sql = "Select * from UFN_OWNERSLIST(" & ddOcc.SelectedValue & ",1) "
                sql2 = "Select * from UFN_COOWNERSLIST(" & ddOcc.SelectedValue & ",1) "
                sql3 = "Select * from ufn_MFTriOwnersList(" & ddOcc.SelectedValue & ",1) "
            ElseIf rblEmail.SelectedValue = "1" Then
                sql = "Select * from UFN_OWNERSLIST(" & ddOcc.SelectedValue & ",2) "
                sql2 = "Select * from UFN_COOWNERSLIST(" & ddOcc.SelectedValue & ",2) "
                sql3 = "Select * from ufn_MFTriOwnersList(" & ddOcc.SelectedValue & ",2) "
            End If
        End If

        Dim sWhere As String = ""
        Dim sTemp As String = ""

        If cbState.Checked Then
            sWhere &= IIf(sWhere = "", " Where ", " and ")
            sWhere = sWhere & "state in (" & List_To_String(lbStates_Selected) & ") "
        End If
        If cbPhase.Checked Then
            sWhere &= IIf(sWhere = "", " Where ", " and ")
            sWhere &= "phase in (" & List_To_String(lbPhase_Selected) & ") "
        End If
        If cbSeason.Checked Then
            sWhere &= IIf(sWhere = "", " Where ", " and ")
            sWhere &= "season in (" & List_To_String(lbSeason_Selected) & ") "
        End If
        If cbStatus.Checked Then
            sWhere &= IIf(sWhere = "", " Where ", " and ")
            sWhere &= " status in (" & List_To_String(lbStatus_Selected) & ") "
        End If

        If cbFreq.Checked Then
            sWhere &= IIf(sWhere = "", " Where ", " and ")
            sWhere = sWhere & " frequency in (" & List_To_String(lbFreq_Selected) & ") "
        End If

        If cbSubType.Checked Then
            sWhere &= IIf(sWhere = "", " Where ", " and ")
            sWhere &= " subtype in (" & List_To_String(lbSubType_Selected) & ") "
        End If

        Dim res As HttpResponse = Response
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand(sql & sWhere, cn)
        Dim xlWB As New XLWorkbook
        cm.CommandTimeout = 0
        cn.Open()

        cm.CommandText = sql & sWhere
        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Owners"))
        If cbCoOwner.Checked Then
            cm.CommandText = sql2 & sWhere
            Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("CoOwners"))
        End If
        If cbTri.Checked Then
            cm.CommandText = sql3 & sWhere
            Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Triennials"))
        End If
        cn.Close()
        cn = Nothing
        cm = Nothing

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""Owners.xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()

        'cm.CommandText = sql & sWhere
        'da.Fill(ds, "Owners")
        'gvowners.datasource = ds.Tables("Owners")
        'gvowners.databind()

        'If cbCoOwner.Checked Then
        '    cm.CommandText = sql2 & sWhere
        '    da.Fill(ds, "CoOwners")
        '    gvCoOwners.datasource = ds.Tables("CoOwners")
        '    gvCoOwners.databind()
        'End If

        'If cbTri.Checked Then
        '    cm.CommandText = sql3 & sWhere
        '    da.Fill(ds, "Tri")
        '    gvTriOwners.datasource = ds.Tables("Tri")
        '    gvTriOwners.databind()

        'End If

    End Sub

    Private Sub Loop_Records(ByRef dr As SqlDataReader, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        While dr.Read
            If row = 1 Then
                For col = 1 To dr.VisibleFieldCount
                    ws.Cell(row, col).SetValue(dr.GetName(col - 1))
                Next
                row += 1
            End If
            For col = 1 To dr.VisibleFieldCount
                ws.Cell(row, col).SetValue(dr.Item(col - 1))
            Next
            row += 1
        End While
        dr.Close()
    End Sub


    Private Function List_To_String(ByRef lbSource As ListBox) As String
        Dim sTemp As String = ""
        If lbSource.Items.Count > 0 Then
            For i = 0 To lbSource.Items.Count - 1
                sTemp += IIf(sTemp = "", "'" & lbSource.Items(i).Text & "'", ",'" & lbSource.Items(i).Text & "'")
            Next
        End If
        Return sTemp
    End Function

    Private Sub Report()
        Dim cn As New SqlConnection(resources.resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet

        '        Dim cn As Object
        '        Dim rs As Object

        Lit.Text = ""

        'cn = Server.CreateObject("ADODB.Connection")
        'rs = Server.CreateObject("ADODB.Recordset")

        'cn.commandtimeout = 0
        Server.ScriptTimeout = 10000

        'cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        Dim sql As String = "Select * from UFN_OWNERSLIST(" & ddOcc.SelectedValue & ",0) "
        Dim sql2 As String = "Select * from UFN_COOWNERSLIST(" & ddOcc.SelectedValue & ",0) "
        Dim sql3 As String = "Select * from ufn_MFTriOwnersList(" & ddOcc.SelectedValue & ",0) "

        If cbEmail.Checked Then
            If rblEmail.SelectedValue = "0" Then
                sql = "Select * from UFN_OWNERSLIST(" & ddOcc.SelectedValue & ",1) "
                sql2 = "Select * from UFN_COOWNERSLIST(" & ddOcc.SelectedValue & ",1) "
                sql3 = "Select * from ufn_MFTriOwnersList(" & ddOcc.SelectedValue & ",1) "
            ElseIf rblEmail.SelectedValue = "1" Then
                sql = "Select * from UFN_OWNERSLIST(" & ddOcc.SelectedValue & ",2) "
                sql2 = "Select * from UFN_COOWNERSLIST(" & ddOcc.SelectedValue & ",2) "
                sql3 = "Select * from ufn_MFTriOwnersList(" & ddOcc.SelectedValue & ",2) "
            End If
        End If

        Dim sWhere As String = ""
        Dim sTemp As String = ""

        If cbState.Checked Then
            sWhere &= IIf(sWhere = "", " Where ", " and ")
            sWhere = sWhere & "state in (" & List_To_String(lbStates_Selected) & ") "
        End If
        If cbPhase.Checked Then
            sWhere &= IIf(sWhere = "", " Where ", " and ")
            sWhere &= "phase in (" & List_To_String(lbPhase_Selected) & ") "
        End If
        If cbSeason.Checked Then
            sWhere &= IIf(sWhere = "", " Where ", " and ")
            sWhere &= "season in (" & List_To_String(lbSeason_Selected) & ") "
        End If
        If cbStatus.Checked Then
            sWhere &= IIf(sWhere = "", " Where ", " and ")
            sWhere &= " status in (" & List_To_String(lbStatus_Selected) & ") "
        End If

        If cbFreq.Checked Then
            sWhere &= IIf(sWhere = "", " Where ", " and ")
            sWhere = sWhere & " frequency in (" & List_To_String(lbFreq_Selected) & ") "
        End If

        If cbSubType.Checked Then
            sWhere &= IIf(sWhere = "", " Where ", " and ")
            sWhere &= " subtype in (" & List_To_String(lbSubType_Selected) & ") "
        End If

        cm.CommandText = sql & sWhere
        cm.CommandTimeout = 0

        da.Fill(ds, "Owners")
        gvowners.datasource = ds.Tables("Owners")
        gvowners.databind()

        'rs.open(sql & sWhere, cn, 0, 1)
        'If rs.eof And rs.bof Then
        '    Lit.Text &= ("No Records")
        '    Lit.Text &= ("<br />" & sql & sWhere)
        'Else
        '    Lit.Text &= ("<table border = 1 >")
        '    Lit.Text &= ("<tr>")
        '    For i = 0 To rs.fields.count - 1
        '        Lit.Text &= ("<th>" & rs.fields(i).name & "</th>")
        '    Next
        '    Lit.Text &= ("</tr>")
        '    Do While Not rs.eof
        '        Lit.Text &= ("<tr>")
        '        For i = 0 To rs.fields.count - 5
        '            Lit.Text &= ("<td>" & rs.fields(i).value & "</td>")
        '        Next
        '        For i = rs.fields.count - 4 To rs.fields.count - 3
        '            Lit.Text &= ("<td align='right'>" & FormatCurrency(rs.fields(i).value) & "</td>")
        '        Next
        '        Lit.Text &= ("<td align='right'>" & rs.fields("Email Address").value & "</td>")
        '        Lit.Text &= ("<td align='right'>" & rs.fields("Maintenance Fee Status").value & "</td>")
        '        Lit.Text &= ("</tr>")
        '        rs.movenext()
        '        'Response.Flush()
        '    Loop
        '    Lit.Text &= ("</table>")
        'End If
        'rs.close()


        If cbCoOwner.Checked Then
            cm.CommandText = sql2 & sWhere
            da.Fill(ds, "CoOwners")
            gvCoOwners.datasource = ds.Tables("CoOwners")
            gvCoOwners.databind()

            'rs.open(sql2 & sWhere, cn, 0, 1)
            'If rs.eof And rs.bof Then
            '    Lit.Text &= ("No Co-Owners")
            '    Lit.Text &= ("<br />" & sql & sWhere)
            'Else
            '    Lit.Text &= ("Co-Owners")
            '    Lit.Text &= ("<table border = 1 >")
            '    Lit.Text &= ("<tr>")
            '    For i = 0 To rs.fields.count - 1
            '        Lit.Text &= ("<th>" & rs.fields(i).name & "</th>")
            '    Next
            '    Lit.Text &= ("</tr>")
            '    Do While Not rs.eof
            '        Lit.Text &= ("<tr>")
            '        For i = 0 To rs.fields.count - 5
            '            Lit.Text &= ("<td>" & rs.fields(i).value & "</td>")
            '        Next
            '        For i = rs.fields.count - 4 To rs.fields.count - 3
            '            Lit.Text &= ("<td align='right'>" & FormatCurrency(rs.fields(i).value) & "</td>")
            '        Next
            '        Lit.Text &= ("<td align='right'>" & rs.fields("Email Address").value & "</td>")
            '        Lit.Text &= ("<td align='right'>" & rs.fields("Maintenance Fee Status").value & "</td>")
            '        Lit.Text &= ("</tr>")
            '        rs.movenext()
            '    Loop
            '    Lit.Text &= ("</table>")
            'End If
            'rs.close()
        End If

        'Response.Write(String.Format("{0} {1}", sql3, sWhere))
        'Return
        If cbTri.Checked Then
            cm.CommandText = sql3 & sWhere
            da.Fill(ds, "Tri")
            gvTriOwners.datasource = ds.Tables("Tri")
            gvTriOwners.databind()

            'rs.open(sql3 & sWhere, cn, 0, 1)
            'If rs.eof And rs.bof Then
            '    Lit.Text &= ("No Triennial Owners from Prior Years")
            '    Lit.Text &= ("<br />" & sql & sWhere)
            'Else
            '    Lit.Text &= ("Triennial Owners from Prior Years")
            '    Lit.Text &= ("<table border = 1 >")
            '    Lit.Text &= ("<tr>")
            '    For i = 0 To rs.fields.count - 1
            '        Lit.Text &= ("<th>" & rs.fields(i).name & "</th>")
            '    Next
            '    Lit.Text &= ("</tr>")
            '    Do While Not rs.eof
            '        Lit.Text &= ("<tr>")
            '        For i = 0 To rs.fields.count - 5
            '            Lit.Text &= ("<td>" & rs.fields(i).value & "</td>")
            '        Next
            '        For i = rs.fields.count - 4 To rs.fields.count - 3
            '            Lit.Text &= ("<td align='right'>" & FormatCurrency(rs.fields(i).value) & "</td>")
            '        Next
            '        Lit.Text &= ("<td align='right'>" & rs.fields("Email Address").value & "</td>")
            '        Lit.Text &= ("<td align='right'>" & rs.fields("Maintenance Fee Status").value & "</td>")
            '        Lit.Text &= ("</tr>")
            '        rs.movenext()
            '    Loop
            '    Lit.Text &= ("</table>")
            'End If
            'rs.close()
        End If
        'rs = Nothing
        'cn.close()
        'cn = Nothing
    End Sub

    Protected Sub cbEmail_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEmail.CheckedChanged
        pnlEmail.Visible = cbEmail.Checked
    End Sub
End Class
