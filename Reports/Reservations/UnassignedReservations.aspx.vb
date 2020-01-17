
Partial Class Reports_Reservations_UnassignedReservations
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oCombo As New clsComboItems
            ddResType.Items.Add(New ListItem("ALL", 0))
            ddResType.DataSource = oCombo.Load_ComboItems("ReservationType")
            ddResType.DataTextField = "ComboItem"
            ddResType.DataValueField = "ComboItemID"
            ddResType.AppendDataBoundItems = True
            ddResType.DataBind()
            ddSubType.Items.Add(New ListItem("ALL", -1))
            ddSubType.DataSource = oCombo.Load_ComboItems("ReservationSubType")
            ddSubType.DataTextField = "ComboItem"
            ddSubType.DataValueField = "ComboItemID"
            ddSubType.AppendDataBoundItems = True
            ddSubType.DataBind()
            oCombo = Nothing
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        If ddResType.SelectedValue = 0 Then
            For Each item As ListItem In ddResType.Items
                If item.Value = 0 Then
                Else
                    lbResType.Items.Add(New ListItem(item.Text, item.Value))
                End If
            Next
            Dim j As Integer = ddResType.Items.Count - 1
            Do While j > -1
                ddResType.Items.Remove(ddResType.Items(j))
                j = j - 1
            Loop
        Else
            lbResType.Items.Add(New ListItem(ddResType.SelectedItem.Text, ddResType.SelectedValue))
            ddResType.Items.Remove(ddResType.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        'If ddSubType.SelectedValue = 0 Then
        '    For Each item As ListItem In ddSubType.Items
        '        If item.Value = 0 Then
        '        Else
        '            lbSubType.Items.Add(New ListItem(item.Text, item.Value))
        '        End If
        '    Next
        '    Dim j As Integer = ddSubType.Items.Count - 1
        '    Do While j > -1
        '        ddSubType.Items.Remove(ddSubType.Items(j))
        '        j = j - 1
        '    Loop
        'Else
        '    lbSubType.Items.Add(New ListItem(ddSubType.SelectedItem.Text, ddSubType.SelectedValue))
        '    ddSubType.Items.Remove(ddSubType.SelectedItem)
        'End If

        If ddSubType.SelectedValue = -1 Then
            lbSubType.Items.AddRange(ddSubType.Items.OfType(Of ListItem).Where(Function(x) x.Value <> -1).ToArray())            
        Else
            lbSubType.Items.Add(ddSubType.Items.OfType(Of ListItem).Where(Function(x) x.Selected).Single())            
        End If

        For Each li As ListItem In lbSubType.Items
            ddSubType.Items.Remove(li)
        Next

        Dim t = lbSubType.Items.OfType(Of ListItem).OrderBy(Function(x) x.Text).ToArray()
        lbSubType.Items.Clear()
        lbSubType.Items.AddRange(t.ToArray())
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        If lbResType.SelectedIndex > -1 Then
            ddResType.Items.Add(New ListItem(lbResType.SelectedItem.Text, lbResType.SelectedValue))
            lbResType.Items.Remove(lbResType.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed4_Click(sender As Object, e As System.EventArgs)
        If lbSubType.SelectedIndex > -1 Then
            ddSubType.Items.Add(New listItem(lbSubType.SelectedItem.Text, lbSubType.SelectedValue))
            lbSubType.Items.Remove(lbSubType.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed5_Click(sender As Object, e As System.EventArgs)
        Dim cn As Object
        Dim rs As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim sAns As String = ""
        Dim i As Integer = 0
        Dim nights As Integer = 0
        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0

        sAns = "<table>"
        For Each item As ListItem In Me.lbResType.Items
            rs.Open("Select comboItem from t_ComboItems where comboitemid = '" & item.Value & "'", cn, 3, 3)
            If i > 0 Then
                sAns = sAns & "<tr><td><br></td></tr>"
            End If
            sAns = sAns & "<tr><td colspan = '2'><H2>" & rs.Fields("ComboItem").Value & "</H2></td></tr>"
            rs.Close()
            For Each itemB As ListItem In Me.lbSubType.Items
                'rs.Open("Select comboitem from t_ComboItems where comboitemid = '" & itemB.Value & "'", cn, 3, 3)
                'If rs.EOF And rs.BOF Then                   
                'Else
                '    sAns = sAns & "<tr><td colspan = '2'><B>" & rs.Fields("ComboItem").Value & "</B></td>"
                'End If
                'rs.Close()

                sAns = sAns & "<tr><td colspan = '2'><B>" & itemB.Text & "</B></td>"

                rs.Open("Select c.*, d.FirstName, d.LastName, (Select Top 1 Number from t_ProspectPhone where prospectID = d.ProspectID) as HomePHone from (Select a.* from t_Reservations a left outer join t_RoomAllocationMatrix b on a.reservationid = b.reservationid where CheckInDate between '" & sdate & "' and '" & edate & "' and a.typeid = '" & item.Value & "' and subtypeid = '" & itemB.Value & "' and a.ResLocationID = (Select c.comboitemid from t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID where co.comboname = 'ReservationLocation' and c.comboitem = 'KCP') and a.StatusID in (Select c.comboitemid from t_ComboItems c inner join t_Combos co on c.ComboID = co.COmboID where co.comboname = 'ReservationStatus' and (c.comboitem = 'Pending Payment' or c.comboitem = 'Booked')) and b.reservationid is null) c left outer join t_Prospect d on c.ProspectID = d.ProspectID", cn, 3, 3)


                If rs.EOF And rs.BOF Then
                    sAns = sAns & "<tr><td colspan = '4'>No Reservations Found</td></tr>"
                Else
                    Do While Not rs.EOF
                        sAns = sAns & "<tr>"
                        sAns = sAns & "<td>" & rs.Fields("ReservationID").Value & "</td>"
                        sAns = sAns & "<td>" & rs.Fields("FirstName").Value & " " & rs.Fields("LastName").Value & "</td>"
                        sAns = sAns & "<td>" & rs.Fields("CheckInDate").Value & " - </td>"
                        sAns = sAns & "<td>" & rs.Fields("CheckOutDate").Value & "</td>"
                        nights = DateDiff("d", rs.Fields("CheckInDate").Value, rs.Fields("CheckOutDate").Value)
                        sAns = sAns & "<td>" & nights & " Nights</td>"
                        sAns = sAns & "<td>" & rs.Fields("HomePhone").Value & "</td>"
                        sAns = sAns & "</tr>"
                        rs.MoveNext()
                    Loop
                End If
                rs.Close()
            Next
            i = i + 1
        Next
        sAns = sAns & "</table>"
        cn.Close()
        rs = Nothing
        cn = Nothing
        litReport.Text = sAns
    End Sub
End Class
