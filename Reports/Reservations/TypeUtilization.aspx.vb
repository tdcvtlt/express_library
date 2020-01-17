
Partial Class Reports_Reservations_TypeUtilization
    Inherits System.Web.UI.Page


    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        lbResType.Items.Add(New ListItem(ddResType.SelectedItem.Text, ddResType.SelectedValue))
        ddResType.Items.Remove(ddResType.SelectedItem)
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If ddSubType.SelectedValue = 0 Then
            For Each item As ListItem In ddSubType.Items
                If item.Value = 0 Then
                Else
                    lbSubType.Items.Add(New ListItem(item.Text, item.Value))
                End If
            Next
            Dim j As Integer = ddSubType.Items.Count - 1
            Do While j > -1
                ddSubType.Items.Remove(ddSubType.Items(j))
                j = j - 1
            Loop
        Else
            lbSubType.Items.Add(New ListItem(ddSubType.SelectedItem.Text, ddSubType.SelectedValue))
            ddSubType.Items.Remove(ddSubType.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        If lbResType.SelectedIndex > -1 Then
            ddResType.Items.Add(New listItem(lbResType.SelectedItem.Text, lbResType.SelectedValue))
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
        Dim rs2 As Object
        Dim rs3 As Object
        Dim sAns As String = ""
        Dim Total As Integer = 0
        Dim usedTotal As Integer = 0
        Dim subtotal As Double = 0
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")
        rs3 = Server.CreateObject("ADODB.Recordset")
        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0
        sAns = "<table><tr><th>Res Type</th>"
        rs.Open("Select c.ComboItem from t_ComboItems c inner join t_Combos co on c.ComboID = co.COmboID where co.comboname = 'UnitType' order by c.comboitem asc", cn, 3, 3)
        Do While Not rs.EOF
            sAns = sAns & "<th>" & rs.Fields("CombOItem").Value & " Set Aside</th>"
            sAns = sAns & "<th>" & rs.Fields("ComboItem").Value & " % Used</th>"
            rs.MoveNExt()
        Loop
        rs.Close()
        sAns = sAns & "</tr>"

        For Each item As ListItem In lbResType.Items
            rs.OPen("Select ComboItem from t_ComboItems where comboitemid = '" & item.value & "'", cn, 3, 3)
            sAns = sAns & "<tr><td><B>" & rs.Fields("ComboItem").Value & "</B></td>"
            rs.Close()
            rs2.OPen("Select c.ComboItem from t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID where co.comboname = 'UnitType' order by c.comboitem asc", cn, 3, 3)
            Do While Not rs2.EOF
                rs3.Open("Select Case when Count(AllocationID) is null then 0 else Count(AllocationID) end as total from (Select e.AllocationID, e.RoomID, e.TypeiD, e.DateAllocated, e.ReservationID, e.UsageID, f.ComboItem as UnitType from (Select c.*, d.TYpeiD As UnitTypeID from (Select a.*, b.UnitID As UnitID from t_RoomAllocationMatrix a left outer join t_Room b on a.roomid = b.roomid where a.DateAllocated between '" & sdate & "' and '" & edate & "') c Left Outer Join t_Unit d on c.UnitID = d.UnitID) e Left Outer Join t_COmboItems f on e.unittypeid = f.comboitemid where f.ComboItem = '" & rs2.Fields("ComboItem").Value & "' and e.TypeID = '" & item.Value & "') DERIVEDTBL", cn, 3, 3)
                If rs3.Fields("Total").Value = 0 Then
                    sAns = sAns & "<td>0</td>"
                    Total = 0
                Else
                    sAns = sAns & "<td>" & rs3.Fields("Total").Value & "</td>"
                    Total = rs3.Fields("Total").Value
                End If
                rs3.CLose()

                If Total = 0 Then
                    sAns = sAns & "<td>0%</td>"
                Else
                    rs3.Open("Select Case when Count(AllocationID) is null then 0 else Count(AllocationID) end as total from (Select e.AllocationID, e.RoomID, e.TypeiD, e.DateAllocated, e.ReservationID, e.UsageID, f.ComboItem as UnitType from (Select c.*, d.TYpeiD As UnitTypeID from (Select a.*, b.UnitID As UnitID from t_RoomAllocationMatrix a left outer join t_Room b on a.roomid = b.roomid where a.DateAllocated between '" & sdate & "' and '" & edate & "') c Left Outer Join t_Unit d on c.UnitID = d.UnitID) e Left Outer Join t_COmboItems f on e.unittypeid = f.comboitemid where f.ComboItem = '" & rs2.Fields("ComboItem").Value & "' and e.TypeID = '" & item.Value & "' and e.ReservationID > 0) DERIVEDTBL", cn, 3, 3)
                    If rs3.Fields("Total").Value = 0 Then
                        sAns = sAns & "<td>0%</td>"
                    Else
                        usedTotal = rs3.Fields("Total").Value
                        sAns = sAns & "<td>" & formatNumber((usedTotal / Total) * 100, 2) & "%</td>"
                    End If
                    rs3.Close()
                End If
                rs2.MoveNext()
            Loop
            rs2.Close()
            sAns = sAns & "</tr>"

            'Loop Through all SubTypes
            For Each itemB As ListItem In lbSubType.Items
                rs.Open("Select ComboItem from t_COmboItems where comboitemid = '" & itemB.Value & "'", cn, 3, 3)
                sAns = sAns & "<tr><td>" & rs.Fields("ComboItem").value & "</td>"
                rs.Close()
                rs2.OPen("Select c.ComboItem from t_ComboItems c inner join t_Combos co on c.ComboID = co.CombOID where co.comboname = 'UnitType' order by c.comboitem asc", cn, 3, 3)
                Do While Not rs2.EOF
                    'Get Total for ResType
                    rs3.Open("Select Case when Count(AllocationID) is null then 0 else Count(AllocationID) end as total from (Select e.AllocationID, e.RoomID, e.TypeiD, e.DateAllocated, e.ReservationID, e.UsageID, f.ComboItem as UnitType from (Select c.*, d.TYpeiD As UnitTypeID from (Select a.*, b.UnitID As UnitID from t_RoomAllocationMatrix a left outer join t_Room b on a.roomid = b.roomid where a.DateAllocated between '" & sdate & "' and '" & edate & "') c Left Outer Join t_Unit d on c.UnitID = d.UnitID) e Left Outer Join t_COmboItems f on e.unittypeid = f.comboitemid where f.ComboItem = '" & rs2.Fields("ComboItem").Value & "' and e.TypeID = '" & item.Value & "') DERIVEDTBL", cn, 3, 3)
                    If rs3.Fields("Total").Value = 0 Then
                        Total = 0
                    Else
                        Total = rs3.Fields("Total").Value
                    End If
                    rs3.CLose()

                    rs3.Open("Select Case when Count(AllocationID) is null then 0 else Count(AllocationID) end as total from (Select g.*, h.SubTypeID from (Select e.AllocationID, e.RoomID, e.TypeiD, e.DateAllocated, e.ReservationID, e.UsageID, f.ComboItem as UnitType from (Select c.*, d.TYpeiD As UnitTypeID from (Select a.*, b.UnitID As UnitID from t_RoomAllocationMatrix a left outer join t_Room b on a.roomid = b.roomid where a.DateAllocated between '" & sdate & "' and '" & edate & "') c Left Outer Join t_Unit d on c.UnitID = d.UnitID) e Left Outer Join t_COmboItems f on e.unittypeid = f.comboitemid) g left outer join t_Reservations h on g.reservationid = h.reservationid where g.UnitType = '" & rs2.Fields("ComboItem").Value & "' and g.TypeID = '" & item.Value & "' and h.SubTypeID = '" & itemB.Value & "') DERIVEDTBL", cn, 3, 3)
                    If rs3.Fields("Total").Value = 0 Then
                        sAns = sAns & "<td>0</td>"
                        subtotal = 0
                    Else
                        sAns = sAns & "<td>" & rs3.Fields("Total").Value & "</td>"
                        subtotal = rs3.Fields("Total").Value
                    End If
                    rs3.CLose()
                    If Total = 0 Then
                        sAns = sAns & "<td>0%</td>"
                    Else
                        If subtotal = 0 Then
                            sAns = sAns & "<td>0%</td>"
                        Else
                            sAns = sAns & "<td>" & formatNumber((subtotal / Total) * 100, 2) & "%</td>"
                        End If
                        'rs3.Open "Select Count(AllocationID) as total from (Select g.*, h.SubTypeID from (Select e.AllocationID, e.RoomID, e.TypeiD, e.DateAllocated, e.ReservationID, e.UsageID, f.ComboItem as UnitType from (Select c.*, d.UnitTYpeiD As UnitTypeID from (Select a.*, b.UnitID As UnitID from t_RoomAllocationMatrix a left outer join t_Room b on a.roomid = b.roomid where a.DateAllocated between '" & request("sDate") & "' and '" & request("eDate") & "') c Left Outer Join t_Units d on c.UnitID = d.UnitID) e Left Outer Join t_COmboItems f on e.unittypeid = f.comboitemid) g left outer join t_Reservations h on g.reservationid = h.reservationid where g.UnitType = '" & rs2.Fields("ComboItem") & "' and g.TypeID = '" & resTypes(i) & "' and g.ReservationID > 0 and h.SubTypeID = '" & resSubTypes(j) & "') DERIVEDTBL", cn, 3, 3 
                        'If IsNull(rs3.Fields("Total")) or rs3.Fields("Total") = 0 Then
                        '	sAns = sAns & "<td>0%</td>"
                        'Else
                        '	usedTotal = rs3.Fields("Total")
                        '	sAns = sANs & "<td>" & formatNumber((usedTotal/Total) * 100, 2) & "%</td>"
                        'End If
                        'rs3.Close
                    End If
                    rs2.MoveNext()
                Loop
                rs2.Close()
                sAns = sAns & "</tr>"
            Next
        Next
        sAns = sAns & "</table>"
 
        cn.Close()
        rs = Nothing
        rs2 = Nothing
        rs3 = Nothing
        cn = Nothing
        litReport.Text = sAns
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oCombo As New clsComboItems
            ddResType.DataSource = oCombo.Load_ComboItems("ReservationType")
            ddResType.DataTextField = "ComboItem"
            ddResType.DataValueField = "ComboItemID"
            ddResType.DataBind()
            ddSubType.Items.Add(New listItem("ALL", 0))
            ddSubType.DataSource = oCombo.Load_ComboItems("ReservationSubType")
            ddSubType.DataTextField = "ComboItem"
            ddSubType.DataValueField = "ComboItemID"
            ddSubType.AppendDataBoundItems = True
            ddSubType.DataBind()
            oCombo = Nothing
        End If
    End Sub
End Class
