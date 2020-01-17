
Partial Class Reports_Reservations_RentalCategory
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oCombo As New clsComboItems
            ddUnitSize.Items.Add(New listItem("ALL", 0))
            ddUnitSize.DataSource = oCombo.Load_ComboItems("RoomType")
            ddUnitSize.DataTextField = "ComboItem"
            ddUnitSize.DataValueField = "ComboItemID"
            ddUnitSize.AppendDataBoundItems = True
            ddUnitSize.DataBind()
            ddUnitType.Items.Add(New listItem("ALL", 0))
            ddUnitType.DataSource = oCombo.Load_ComboItems("UnitType")
            ddUnitType.DataTextField = "ComboItem"
            ddUnitType.DataValueField = "ComboItemID"
            ddUnitType.AppendDataBoundItems = True
            ddUnitType.DataBind()
            ddCategory.Items.Add(New listItem("ALL", 0))
            ddCategory.DataSource = oCombo.Load_ComboItems("UsageCategory")
            ddCategory.DataTextField = "ComboItem"
            ddCategory.DataValueField = "ComboItemID"
            ddCategory.AppendDataBoundItems = True
            ddCategory.Items.Add(New ListItem("NONE", 0))
            ddCategory.DataBind()
            ddUsageYear.Items.Add(New listItem("ALL", 0))
            For i = 2005 To Year(System.DateTime.Now) + 1
                ddUsageYear.Items.Add(i)
            Next
            oCombo = Nothing
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        If ddUnitSize.SelectedValue = 0 Then
            For Each item As ListItem In ddUnitSize.Items
                If item.Value = 0 Then
                Else
                    lbUnitSize.Items.Add(New ListItem(item.Text, item.Value))
                End If
            Next
            Dim j As Integer = ddUnitSize.Items.Count - 1
            Do While j > -1
                ddUnitSize.Items.Remove(ddUnitSize.Items(j))
                j = j - 1
            Loop
        Else
            lbUnitSize.Items.Add(New ListItem(ddUnitSize.SelectedItem.Text, ddUnitSize.SelectedValue))
            ddUnitSize.Items.Remove(ddUnitSize.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If ddUnitType.SelectedValue = 0 Then
            For Each item As ListItem In ddUnitType.Items
                If item.Value = 0 Then
                Else
                    lbUnitType.Items.Add(New ListItem(item.Text, item.Value))
                End If
            Next
            Dim j As Integer = ddUnitType.Items.Count - 1
            Do While j > -1
                ddUnitType.Items.Remove(ddUnitType.Items(j))
                j = j - 1
            Loop
        Else
            lbUnitType.Items.Add(New ListItem(ddUnitType.SelectedItem.Text, ddUnitType.SelectedValue))
            ddUnitType.Items.Remove(ddUnitType.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed5_Click(sender As Object, e As System.EventArgs)
        If ddCategory.SelectedItem.Text = "ALL" Then
            For Each item As ListItem In ddCategory.Items
                If item.Text = "ALL" Then
                Else
                    lbCategory.Items.Add(New ListItem(item.Text, item.Value))
                End If
            Next
            Dim j As Integer = ddCategory.Items.Count - 1
            Do While j > -1
                ddCategory.Items.Remove(ddCategory.Items(j))
                j = j - 1
            Loop
        Else
            lbCategory.Items.Add(New ListItem(ddCategory.SelectedItem.Text, ddCategory.SelectedValue))
            ddCategory.Items.Remove(ddCategory.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed6_Click(sender As Object, e As System.EventArgs)
        If ddUsageYear.SelectedValue = 0 Then
            For Each item As ListItem In ddUsageYear.Items
                If item.Value = 0 Then
                Else
                    lbUsageYear.Items.Add(New ListItem(item.Text, item.Value))
                End If
            Next
            Dim j As Integer = ddUsageYear.Items.Count - 1
            Do While j > -1
                ddUsageYear.Items.Remove(ddUsageYear.Items(j))
                j = j - 1
            Loop
        Else
            lbUsageYear.Items.Add(New ListItem(ddUsageYear.SelectedItem.Text, ddUsageYear.SelectedValue))
            ddUsageYear.Items.Remove(ddUsageYear.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        If lbUnitSize.SelectedIndex > -1 Then
            ddUnitSize.Items.Add(New listItem(lbUnitSize.SelectedItem.Text, lbUnitSize.SelectedValue))
            lbUnitSize.Items.Remove(lbUnitSize.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed4_Click(sender As Object, e As System.EventArgs)
        If lbUnitType.SelectedIndex > -1 Then
            ddUnitType.Items.Add(New listItem(lbUnitType.SelectedItem.Text, lbUnitType.SelectedValue))
            lbUnitType.Items.Remove(lbUnitType.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed7_Click(sender As Object, e As System.EventArgs)
        If lbCategory.SelectedIndex > -1 Then
            ddCategory.Items.Add(New listItem(lbCategory.SelectedItem.Text, lbCategory.SelectedValue))
            lbCategory.Items.Remove(lbCategory.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed8_Click(sender As Object, e As System.EventArgs)
        If lbUsageYear.SelectedIndex > -1 Then
            ddUsageYear.Items.Add(New listItem(lbUsageYear.SelectedItem.Text, lbUsageYear.SelectedValue))
            lbUsageYear.Items.Remove(lbUsageYear.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed9_Click(sender As Object, e As System.EventArgs)

        Dim cat As String = ""
        Dim uyear As String = ""
        Dim rtype As String = ""
        Dim uType As String = ""
        Dim sAns As String = ""
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")
        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0
        For Each item As ListItem In Me.lbUnitType.Items
            If uType = "" Then
                uType = "'" & item.value & "'"
            Else
                uType = uType & ",'" & item.value & "'"
            End If
        Next

        For Each item As ListItem In Me.lbUnitSize.Items
            If rtype = "" Then
                rtype = "'" & item.value & "'"
            Else
                rtype = rtype & ",'" & item.value & "'"
            End If
        Next

        For Each item As ListItem In Me.lbCategory.Items
            If cat = "" Then
                cat = "'" & item.value & "'"
            Else
                cat = cat & ",'" & item.value & "'"
            End If
        Next

        For Each item As ListItem In Me.lbUsageYear.Items
            If uyear = "" Then
                uyear = "'" & item.value & "'"
            Else
                uyear = uyear & ",'" & item.value & "'"
            End If
        Next
        rs.Open("Select g.*, h.ComboItem as Category from (Select e.*, f.FirstName, f.LastName, (Select top 1 number from t_ProspectPhone where prospectid = f.ProspectID and Active = 1) as HomePhone from (Select c.*, d.ProspectID, d.ContractNumber, cs.ComboItem as ContractStatus, css.ComboItem as ContractSubStatus, cst.ComboItem as ContractSubType  from (Select a.UsageID, a.ContractID, a.InDate, a.OutDate, a.CategoryID, a.UsageYear, rt.comboitem as RoomType, ut.comboitem as UnitType from t_usage a left outer join t_comboitems rt on rt.comboitemid = a.roomtypeid left outer join t_comboitems ut on ut.comboitemid = a.unittypeid where a.InDate between '" & sdate & "' and '" & edate & "' and a.typeid = (Select c.comboitemid from t_ComboItems c inner join t_Combos co on c.ComboID = co.CombOID where co.comboname = 'ReservationType' and c.comboitem = 'Rental') and a.UsageYear in (" & uyear & ") and a.Categoryid in (" & cat & ") and rt.comboitemid in (" & rtype & ") and ut.comboitemid in (" & uType & ")) c inner join t_Contract d on c.contractid = d.contractid left outer join t_ComboItems cs on d.StatusID = cs.ComboItemID left outer join t_ComboItems css on d.SubStatusID = css.ComboItemID left outer join t_ComboItems cst on d.SubTypeID = cst.ComboItemID) e inner join t_Prospect f on e.prospectid = f.prospectid) g left outer join t_ComboItems h on g.categoryid = h.comboitemid order by g.LastName, g.Firstname", cn, 3, 3)
        If rs.EOF And rs.BOF Then
            sAns = "NO RECORDS FOUND IN THIS DATE RANGE."
        Else
            sAns = sAns & "<table>"
            sAns = sAns & "<tr><th>Owner</th><th>HomePhone</th><th>KCP #</th><th>Status</th><th>SubStatus</th><th>SubType</th><th>Usage Year</th><th>Room Type</th><th>Unit Type</th><th>InDate</th><th>OutDate</th><th>Category</th><th>Reservation</th><th>UsageID</th></tr>"
            Do While Not rs.EOF
                sAns = sAns & "<tr>"
                sAns = sAns & "<td>" & rs.Fields("LastName").Value & ", " & rs.Fields("FirstName").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("HomePhone").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("ContractNumber").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("ContractStatus").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("ContractSubStatus").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("ContractSubType").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("UsageYear").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("RoomType").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("UnitType").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("InDate").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("OutDate").Value & "</td>"
                sAns = sAns & "<td align = center>" & rs.Fields("Category").Value & "" & "</td>"
                sAns = sAns & "<td>&nbsp</td>"
                sAns = sAns & "<td align = center><a href = '../../marketing/editcontract.aspx?contractid=" & rs.fields("ContractID").value & "&usageid=" & rs.fields("UsageID").value & "'>" & rs.fields("UsageID").value & "</a></td>"
                rs2.Open("Select Distinct(ReservationID) from t_RoomALlocationMatrix where usageID in (" & rs.Fields("UsageID").Value & ")", cn, 3, 3)

                If rs2.EOF And rs2.BOF Then
                    sAns = sAns & "<tr align = center colspan 13><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td align = center>N/A</td></tr>"
                Else
                    Do While Not rs2.EOF
                        sAns = sAns & "<tr align = center colspan 13><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td>&nbsp</td><td align = center><a href='../../marketing/editreservation.aspx?reservationid=" & rs2.fields("reservationid").value & "'>" & rs2.fields("reservationid").value & "</a></td></tr>"
                        rs2.MoveNext()
                    Loop
                    rs2.MoveFirst()
                End If
                rs2.Close()
                sAns = sAns & "</tr>"
                rs.MoveNext()
            Loop
            sAns = sAns & "</table>"
        End If
        rs.Close()
        cn.Close()
        rs = Nothing
        rs2 = Nothing
        cn = Nothing
        litReport.Text = sAns
    End Sub
End Class
