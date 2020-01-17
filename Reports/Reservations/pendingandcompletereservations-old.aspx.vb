Imports System.Data
Imports System.Data.SqlClient

Partial Class Reports_Reservations_pendingandcompletereservations
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        If ddResStatus.SelectedValue = 0 Then
            For Each item As ListItem In ddResStatus.Items
                If item.Value = 0 Then
                Else
                    lbResStatus.Items.Add(New ListItem(item.Text, item.Value))
                End If
            Next
            Dim j As Integer = ddResStatus.Items.Count - 1
            Do While j > -1
                ddResStatus.Items.Remove(ddResStatus.Items(j))
                j = j - 1
            Loop
        Else
            lbResStatus.Items.Add(New ListItem(ddResStatus.SelectedItem.Text, ddResStatus.SelectedValue))
            ddResStatus.Items.Remove(ddResStatus.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If ddResType.SelectedValue = -1 Then
            For Each item As ListItem In ddResType.Items
                If item.Value = -1 Then
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

    Protected Sub Unnamed5_Click(sender As Object, e As System.EventArgs)
        If ddSubType.SelectedValue = -1 Then
            For Each item As ListItem In ddSubType.Items
                If item.Value = -1 Then
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

    Protected Sub Unnamed6_Click(sender As Object, e As System.EventArgs)
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

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        If lbResStatus.SelectedIndex > -1 Then
            ddResStatus.Items.Add(New listItem(lbResStatus.SelectedItem.Text, lbResStatus.SelectedValue))
            lbResStatus.Items.Remove(lbResStatus.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed7_Click(sender As Object, e As System.EventArgs)
        If lbSubType.SelectedIndex > -1 Then
            ddSubType.Items.Add(New listItem(lbSubType.SelectedItem.Text, lbSubType.SelectedValue))
            lbSubType.Items.Remove(lbSubType.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed4_Click(sender As Object, e As System.EventArgs)
        If lbResType.SelectedIndex > -1 Then
            ddResType.Items.Add(New listItem(lbResType.SelectedItem.Text, lbResType.SelectedValue))
            lbResType.Items.Remove(lbResType.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed8_Click(sender As Object, e As System.EventArgs)
        If lbUnitType.SelectedIndex > -1 Then
            ddUnitType.Items.Add(New listItem(lbUnitType.SelectedItem.Text, lbUnitType.SelectedValue))
            lbUnitType.Items.Remove(lbUnitType.SelectedItem)
        End If
    End Sub

    Protected Sub btn_Reservation_Add_ToListBox_Click(sender As Object, e As System.EventArgs) Handles btn_Reservation_Add_ToListBox.Click

        If ddResLocation.SelectedValue = 0 Then

            Dim selected As New List(Of ListItem)

            For Each item In ddResLocation.Items.OfType(Of ListItem).Where(Function(x) x.Value <> "0")
                lbResLocation.Items.Add(item)
                selected.Add(item)
            Next

            For Each item In selected
                ddResLocation.Items.Remove(item)
            Next

        Else
            lbResLocation.Items.Add(ddResLocation.SelectedItem)
            ddResLocation.Items.Remove(ddResLocation.SelectedItem)
        End If

    End Sub

    Protected Sub btn_Reservation_Remove_FromListBox_Click(sender As Object, e As System.EventArgs) Handles btn_Reservation_Remove_FromListBox.Click

        If lbResLocation.Items.Count > 0 Then

            Dim selected As New List(Of ListItem)

            selected.AddRange(lbResLocation.Items.OfType(Of ListItem).Where(Function(x) x.Selected).ToArray())            
            ddResLocation.ClearSelection()

            For Each item In selected
                ddResLocation.Items.Add(item)
                ddResLocation.ClearSelection()
            Next

            For Each item In selected
                lbResLocation.Items.Remove(item)
            Next

        End If
    End Sub


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            multiview1.SetActiveView(view1)

            Dim oCombo As New clsComboItems

            ddResType.Items.Add(New ListItem("ALL", -1))
            ddResType.Items.Add(New ListItem("Empty/Null/0", 0))

            ddResType.DataSource = oCombo.Load_ComboItems("ReservationType")
            ddResType.DataTextField = "ComboItem"
            ddResType.DataValueField = "ComboItemID"
            ddResType.AppendDataBoundItems = True
            ddResType.DataBind()

            ddSubType.Items.Add(New ListItem("ALL", "-1"))
            ddSubType.Items.Add(New ListItem("Empty/Null/0", "0"))

            ddSubType.DataSource = oCombo.Load_ComboItems("ReservationSubType")
            ddSubType.DataTextField = "ComboItem"
            ddSubType.DataValueField = "ComboItemID"
            ddSubType.AppendDataBoundItems = True
            ddSubType.DataBind()

            ddResStatus.Items.Add(New ListItem("ALL", 0))
            ddResStatus.DataSource = oCombo.Load_ComboItems("ReservationStatus")
            ddResStatus.DataTextField = "ComboItem"
            ddResStatus.DataValueField = "ComboItemID"
            ddResStatus.AppendDataBoundItems = True
            ddResStatus.DataBind()

            ddUnitType.Items.Add(New ListItem("ALL", 0))


            ddUnitType.DataSource = oCombo.Load_ComboItems("UnitType")
            ddUnitType.DataTextField = "ComboItem"
            ddUnitType.DataValueField = "ComboItemID"
            ddUnitType.AppendDataBoundItems = True
            ddUnitType.DataBind()

            ddResLocation.Items.Add(New ListItem("ALL", 0))
            ddResLocation.DataSource = oCombo.Load_ComboItems("ReservationLocation")
            ddResLocation.DataTextField = "ComboItem"
            ddResLocation.DataValueField = "ComboItemID"
            ddResLocation.AppendDataBoundItems = True
            ddResLocation.DataBind()




            ddl_statuses.Items.Add(New ListItem("ALL", 0))
            ddl_statuses.DataSource = oCombo.Load_ComboItems("ReservationStatus")
            ddl_statuses.DataTextField = "ComboItem"
            ddl_statuses.DataValueField = "ComboItemID"
            ddl_statuses.AppendDataBoundItems = True

            ddl_statuses.DataBind()

            ddl_locations.Items.Add(New ListItem("ALL", 0))
            ddl_locations.DataSource = oCombo.Load_ComboItems("ReservationLocation")
            ddl_locations.DataTextField = "ComboItem"
            ddl_locations.DataValueField = "ComboItemID"
            ddl_locations.AppendDataBoundItems = True

            ddl_locations.DataBind()


            With ddlResortsList
                .AppendDataBoundItems = True
                .Items.Add(New ListItem("ALL", 0))
                .DataSource = oCombo.Load_ComboItems("ResortCompany")
                .DataValueField = "ComboItemID"
                .DataTextField = "ComboItem"
                .DataBind()
            End With


            Using cn = New SqlConnection(Resources.Resource.cns)
                Using ad = New SqlDataAdapter("select  AccomID, AccomName from t_accom where active = 1 order by AccomName", cn)
                    Try
                        Dim dt = New DataTable()
                        ad.Fill(dt)

                        With ddlHotelList
                            .AppendDataBoundItems = True
                            .Items.Add(New ListItem("ALL", 0))
                            .DataSource = dt
                            .DataValueField = "AccomID"
                            .DataTextField = "AccomName"
                            .DataBind()
                        End With
                    Catch ex As Exception
                        Response.Write(ex.Message)
                    End Try
                End Using
            End Using

           

            oCombo = Nothing
        End If
    End Sub

    Protected Sub Unnamed9_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Try
            Dim cn As Object
            Dim rs As Object
            Dim rs2 As Object
            Dim sAns As String = ""
            Dim sSQL As String = ""
            Dim balance As Double = 0
            Dim usedTotal As Integer = 0
            Dim subtotal As Double = 0
            Dim edate As String = Me.dteEDate.Selected_Date
            Dim sdate As String = Me.dteSDate.Selected_Date
            cn = Server.CreateObject("ADODB.Connection")
            rs = Server.CreateObject("ADODB.Recordset")
            rs2 = Server.CreateObject("ADODB.Recordset")
            Server.ScriptTimeout = 10000
            cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            cn.CommandTimeout = 0
            Dim uType As String = ""
            Dim sType As String = ""
            Dim sName As String = ""
            Dim sStatus As String = ""

            Dim locations = String.Empty

            Dim unitCount As Integer = 0
            Dim bProceed As Boolean = True


            Dim sub_type = lbSubType.Items.OfType(Of ListItem).Select(Function(x) x.Value)
            sName = String.Join(",", sub_type.ToArray())

            If Array.IndexOf(sub_type.ToArray(), "0") <> -1 Then
                sName = String.Format("(r.subtypeid is null or r.subtypeid in ({0})) ", sName)
            Else
                sName = String.Format("(r.subtypeid in ({0})) ", sName)
            End If

            Dim type = lbResType.Items.OfType(Of ListItem).Select(Function(x) x.Value)
            sType = String.Join(",", type.ToArray())

            If Array.IndexOf(type.ToArray(), "0") <> -1 Then
                sType = String.Format("(r.typeid is null or r.typeid in ({0})) ", sType)
            Else
                sType = String.Format("(r.typeid in ({0})) ", sType)
            End If

            sStatus = String.Join(",", lbResStatus.Items.OfType(Of ListItem).Select(Function(x) String.Format("{0}", x.Value)).ToArray())


            For Each item As ListItem In Me.lbUnitType.Items
                If uType = "" Then
                    uType = "'" & item.Value & "'"
                Else
                    uType = uType & ",'" & item.Value & "'"
                End If
                unitCount = unitCount + 1
            Next



            locations = String.Join(",", lbResLocation.Items.OfType(Of ListItem).Select(Function(x) x.Value).ToArray())

            If String.IsNullOrEmpty(locations) Then Return



            Dim sqlText = String.Empty

            If cbOpen.Checked Then
                If unitCount = 3 Then
                    'rs.Open("Select * from (select Distinct r.ReservationID, p.lastname + ', ' + p.firstname as Guest, (Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as HomePhone, o.RoomNumber,datediff(day,r.checkindate, r.checkoutdate) as Nights, r.CheckInDate, r.CheckOutDate, t.comboitem as Type, s.comboitem as SubType, m.comboitem as Status, rs.ComboItem as Source, ut.comboItem as UnitType from t_reservations r left outer join t_CombOitems rs on r.SourceID = rs.ComboItemID left outer join t_comboitems t on t.comboitemid = r.typeid left outer join t_comboitems s on s.comboitemid = r.subtypeid left outer join t_comboitems m on m.comboitemid = r.statusid left outer join t_prospect p on p.prospectid = r.prospectid left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid left outer join t_room o on o.roomid = x.roomid left outer join t_Unit un on o.unitid = un.unitid left outer join t_ComboItems ut on un.typeid = ut.comboitemid where r.checkindate between '" & sdate & "' and '" & edate & "' and " & sName & " and r.typeid in (" & sType & ") and r.statusid in (" & sStatus & ") and r.reslocationid in (" & locations & ")) xx order by xx.checkindate, xx.UnitType", cn, 3, 3)
                    sqlText = "Select * from (select Distinct r.ReservationID, p.lastname + ', ' + p.firstname as Guest, (Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as HomePhone, o.RoomNumber,datediff(day,r.checkindate, r.checkoutdate) as Nights, r.CheckInDate, r.CheckOutDate, t.comboitem as Type, s.comboitem as SubType, m.comboitem as Status, rs.ComboItem as Source, ut.comboItem as UnitType from t_reservations r left outer join t_CombOitems rs on r.SourceID = rs.ComboItemID left outer join t_comboitems t on t.comboitemid = r.typeid left outer join t_comboitems s on s.comboitemid = r.subtypeid left outer join t_comboitems m on m.comboitemid = r.statusid left outer join t_prospect p on p.prospectid = r.prospectid left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid left outer join t_room o on o.roomid = x.roomid left outer join t_Unit un on o.unitid = un.unitid left outer join t_ComboItems ut on un.typeid = ut.comboitemid where r.checkindate between '" & sdate & "' and '" & edate & "' and " & sName & " and " & sType & " and r.statusid in (" & sStatus & ") and r.reslocationid in (" & locations & ")) xx order by xx.checkindate, xx.UnitType"

                    '"Select * from (select Distinct r.ReservationID, p.lastname + ', ' + p.firstname as Guest, (Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as HomePhone, o.RoomNumber,datediff(day,r.checkindate, r.checkoutdate) as Nights, r.CheckInDate, r.CheckOutDate, t.comboitem as Type, s.comboitem as SubType, m.comboitem as Status, rs.ComboItem as Source, ut.comboItem as UnitType from t_reservations r left outer join t_CombOitems rs on r.SourceID = rs.ComboItemID left outer join t_comboitems t on t.comboitemid = r.typeid left outer join t_comboitems s on s.comboitemid = r.subtypeid left outer join t_comboitems m on m.comboitemid = r.statusid left outer join t_prospect p on p.prospectid = r.prospectid left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid left outer join t_room o on o.roomid = x.roomid left outer join t_Unit un on o.unitid = un.unitid left outer join t_ComboItems ut on un.typeid = ut.comboitemid where r.checkindate between '" & sdate & "' and '" & edate & "' and r.subtypeid in (" & sName & ") and r.typeid in (" & sType & ") and r.statusid in (" & sStatus & ") and r.reslocationid in (select c.comboitemid from t_comboitems c inner join t_Combos co on c.ComboID = co.COmboID where c.active = 1 and co.comboname = 'reservationlocation' and c.comboitem = 'KCP')) xx order by xx.checkindate, xx.UnitType"
                    'sSQL = "Select * from (select Distinct r.ReservationID, p.lastname + ', ' + p.firstname as Guest, (Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as HomePhone, o.RoomNumber,datediff(day,r.checkindate, r.checkoutdate) as Nights, r.CheckInDate, r.CheckOutDate, t.comboitem as Type, s.comboitem as SubType, m.comboitem as Status, rs.ComboItem as Source, v.comboitem as TransCode, ut.comboItem as UnitType, (Select Sum(Balance) from UFN_Financials(0,'RESERVATIONID',r.ReservationID,0)) as Balance from t_reservations r left outer join t_CombOitems rs on r.SourceID = rs.ComboItemID left outer join t_comboitems t on t.comboitemid = r.typeid left outer join t_comboitems s on s.comboitemid = r.subtypeid left outer join t_comboitems m on m.comboitemid = r.statusid left outer join t_prospect p on p.prospectid = r.prospectid left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid left outer join t_room o on o.roomid = x.roomid left outer join t_Unit un on o.unitid = un.unitid left outer join t_ComboItems ut on un.typeid = ut.comboitemid where r.checkindate between '" & sdate & "' and '" & edate & "' and r.subtypeid in (" & sName & ") and r.typeid in (" & sType & ") and r.statusid in (" & sStatus & ") and r.reslocationid in (select comboitemid from t_comboitems where active = 1 and comboname = 'reservationlocation' and comboitem = 'KCP')) xx where xx.Balance > 0order by xx.checkindate, xx.UnitType"


                Else
                    'rs.Open("Select * from (select Distinct r.ReservationID, p.lastname + ', ' + p.firstname as Guest, (Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as HomePhone, o.RoomNumber,datediff(day,r.checkindate, r.checkoutdate) as Nights, r.CheckInDate, r.CheckOutDate, t.comboitem as Type, s.comboitem as SubType, m.comboitem as Status, rs.ComboItem as Source, ut.comboItem as UnitType from t_reservations r left outer join t_CombOitems rs on r.SourceID = rs.ComboItemID left outer join t_comboitems t on t.comboitemid = r.typeid left outer join t_comboitems s on s.comboitemid = r.subtypeid left outer join t_comboitems m on m.comboitemid = r.statusid left outer join t_prospect p on p.prospectid = r.prospectid left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid left outer join t_room o on o.roomid = x.roomid left outer join t_Unit un on o.unitid = un.unitid left outer join t_ComboItems ut on un.typeid = ut.comboitemid where un.typeid in (" & uType & ") and r.checkindate between '" & sdate & "' and '" & edate & "' and " & sName & " and r.typeid in (" & sType & ") and r.statusid in (" & sStatus & ") and r.reslocationid in (" & locations & ")) xx order by xx.checkindate, xx.UnitTYpe", cn, 3, 3)

                    sqlText = "Select * from (select Distinct r.ReservationID, p.lastname + ', ' + p.firstname as Guest, (Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as HomePhone, o.RoomNumber,datediff(day,r.checkindate, r.checkoutdate) as Nights, r.CheckInDate, r.CheckOutDate, t.comboitem as Type, s.comboitem as SubType, m.comboitem as Status, rs.ComboItem as Source, ut.comboItem as UnitType from t_reservations r left outer join t_CombOitems rs on r.SourceID = rs.ComboItemID left outer join t_comboitems t on t.comboitemid = r.typeid left outer join t_comboitems s on s.comboitemid = r.subtypeid left outer join t_comboitems m on m.comboitemid = r.statusid left outer join t_prospect p on p.prospectid = r.prospectid left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid left outer join t_room o on o.roomid = x.roomid left outer join t_Unit un on o.unitid = un.unitid left outer join t_ComboItems ut on un.typeid = ut.comboitemid where un.typeid in (" & uType & ") and r.checkindate between '" & sdate & "' and '" & edate & "' and " & sName & " and " & sType & " and r.statusid in (" & sStatus & ") and r.reslocationid in (" & locations & ")) xx order by xx.checkindate, xx.UnitTYpe"
                    'sSQL = "Select * from (select Distinct r.ReservationID, p.lastname + ', ' + p.firstname as Guest, (Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as HomePhone, o.RoomNumber,datediff(day,r.checkindate, r.checkoutdate) as Nights, r.CheckInDate, r.CheckOutDate, t.comboitem as Type, s.comboitem as SubType, m.comboitem as Status, rs.ComboItem as Source, v.comboitem as TransCode, ut.comboItem as UnitType, (Select Sum(Balance) from UFN_Financials(0,'RESERVATIONID',r.ReservationID,0)) as Balance from t_reservations r left outer join t_CombOitems rs on r.SourceID = rs.ComboItemID left outer join t_comboitems t on t.comboitemid = r.typeid left outer join t_comboitems s on s.comboitemid = r.subtypeid left outer join t_comboitems m on m.comboitemid = r.statusid left outer join t_prospect p on p.prospectid = r.prospectid left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid left outer join t_room o on o.roomid = x.roomid left outer join t_Unit un on o.unitid = un.unitid left outer join t_ComboItems ut on un.typeid = ut.comboitemid where un.typeid in (" & uType & ") and r.checkindate between '" & sdate & "' and '" & edate & "' and r.subtypeid in (" & sName & ") and r.typeid in (" & sType & ") and r.statusid in (" & sStatus & ") and r.reslocationid in (select comboitemid from t_comboitems where active = 1 and comboname = 'reservationlocation' and comboitem = 'KCP')) xx where xx.Balance > 0 order by xx.checkindate, xx.UnitTYpe"

                End If
            Else
                If unitCount = 3 Then
                    'rs.Open("select Distinct r.ReservationID, p.lastname + ', ' + p.firstname as Guest, (Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as HomePhone, o.Roomnumber, datediff(day,r.checkindate, r.checkoutdate) as Nights, r.CheckInDate, r.CheckOutDate, t.comboitem as Type, s.comboitem as SubType, m.comboitem as Status, rs.ComboItem as Source, ut.comboitem as UnitType from t_reservations r left outer join t_CombOitems rs on r.SourceID = rs.ComboItemID left outer join t_comboitems s on s.comboitemid = r.subtypeid left outer join t_comboitems t on t.comboitemid = r.typeid left outer join t_comboitems m on m.comboitemid = r.statusid left outer join t_prospect p on p.prospectid = r.prospectid left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid left outer join t_room o on o.roomid = x.roomid left outer join t_Unit un on o.UnitId = un.UnitID left outer join t_ComboItems ut on un.typeid = ut.comboitemid where r.checkindate between '" & sdate & "' and '" & edate & "' and " & sName & " and r.typeid in (" & sType & ") and r.statusid in (" & sStatus & ") and r.reslocationid in (" & locations & ") order by r.checkindate, ut.comboitem", cn, 3, 3)
                    sqlText = "select Distinct r.ReservationID, p.lastname + ', ' + p.firstname as Guest, (Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as HomePhone, o.Roomnumber, datediff(day,r.checkindate, r.checkoutdate) as Nights, r.CheckInDate, r.CheckOutDate, t.comboitem as Type, s.comboitem as SubType, m.comboitem as Status, rs.ComboItem as Source, ut.comboitem as UnitType from t_reservations r left outer join t_CombOitems rs on r.SourceID = rs.ComboItemID left outer join t_comboitems s on s.comboitemid = r.subtypeid left outer join t_comboitems t on t.comboitemid = r.typeid left outer join t_comboitems m on m.comboitemid = r.statusid left outer join t_prospect p on p.prospectid = r.prospectid left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid left outer join t_room o on o.roomid = x.roomid left outer join t_Unit un on o.UnitId = un.UnitID left outer join t_ComboItems ut on un.typeid = ut.comboitemid where r.checkindate between '" & sdate & "' and '" & edate & "' and " & sName & " and " & sType & " and r.statusid in (" & sStatus & ") and r.reslocationid in (" & locations & ") order by r.checkindate, ut.comboitem"

                    'sSQL = "select Distinct r.ReservationID, p.lastname + ', ' + p.firstname as Guest, (Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as HomePhone, o.Roomnumber, datediff(day,r.checkindate, r.checkoutdate) as Nights, r.CheckInDate, r.CheckOutDate, t.comboitem as Type, s.comboitem as SubType, m.comboitem as Status, rs.ComboItem as Source, ut.comboitem as UnitType from t_reservations r left outer join t_CombOitems rs on r.SourceID = rs.ComboItemID left outer join t_comboitems s on s.comboitemid = r.subtypeid left outer join t_comboitems t on t.comboitemid = r.typeid left outer join t_comboitems m on m.comboitemid = r.statusid left outer join t_prospect p on p.prospectid = r.prospectid left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid left outer join t_room o on o.roomid = x.roomid left outer join t_Unit un on o.UnitId = un.UnitID left outer join t_ComboItems ut on un.typeid = ut.comboitemid where r.checkindate between '" & sdate & "' and '" & edate & "' and r.subtypeid in (" & sName & ") and r.typeid in (" & sType & ") and r.statusid in (" & sStatus & ") and r.reslocationid in (select c.comboitemid from t_comboitems c inner join t_Combos co on c.ComboID = co.ComboID where c.active = 1 and co.comboname = 'reservationlocation' and c.comboitem = 'KCP') order by r.checkindate, ut.comboitem"


                Else
                    'rs.Open("select Distinct r.ReservationID, p.lastname + ', ' + p.firstname as Guest, (Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as HomePhone, o.Roomnumber, datediff(day,r.checkindate, r.checkoutdate) as Nights, r.CheckInDate, r.CheckOutDate, t.comboitem as Type, s.comboitem as SubType, m.comboitem as Status, rs.ComboItem as Source, ut.comboitem as UnitType from t_reservations r left outer join t_CombOitems rs on r.SourceID = rs.ComboItemID left outer join t_comboitems s on s.comboitemid = r.subtypeid left outer join t_comboitems t on t.comboitemid = r.typeid left outer join t_comboitems m on m.comboitemid = r.statusid left outer join t_prospect p on p.prospectid = r.prospectid left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid left outer join t_room o on o.roomid = x.roomid left outer join t_Unit un on o.Unitid = un.UnitID left outer join t_ComboItems ut on un.typeid = ut.comboitemid where r.checkindate between '" & sdate & "' and '" & edate & "' and un.typeid in (" & uType & ") and " & sName & " and r.typeid in (" & sType & ") and r.statusid in (" & sStatus & ") and r.reslocationid in (" & locations & ") order by r.checkindate, ut.comboitem", cn, 3, 3)

                    sqlText = "select Distinct r.ReservationID, p.lastname + ', ' + p.firstname as Guest, (Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as HomePhone, o.Roomnumber, datediff(day,r.checkindate, r.checkoutdate) as Nights, r.CheckInDate, r.CheckOutDate, t.comboitem as Type, s.comboitem as SubType, m.comboitem as Status, rs.ComboItem as Source, ut.comboitem as UnitType from t_reservations r left outer join t_CombOitems rs on r.SourceID = rs.ComboItemID left outer join t_comboitems s on s.comboitemid = r.subtypeid left outer join t_comboitems t on t.comboitemid = r.typeid left outer join t_comboitems m on m.comboitemid = r.statusid left outer join t_prospect p on p.prospectid = r.prospectid left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid left outer join t_room o on o.roomid = x.roomid left outer join t_Unit un on o.Unitid = un.UnitID left outer join t_ComboItems ut on un.typeid = ut.comboitemid where r.checkindate between '" & sdate & "' and '" & edate & "' and un.typeid in (" & uType & ") and " & sName & " and " & sType & " and r.statusid in (" & sStatus & ") and r.reslocationid in (" & locations & ") order by r.checkindate, ut.comboitem"

                    'sSQL = "select Distinct r.ReservationID, p.lastname + ', ' + p.firstname as Guest, (Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as HomePhone, o.Roomnumber, datediff(day,r.checkindate, r.checkoutdate) as Nights, r.CheckInDate, r.CheckOutDate, t.comboitem as Type, s.comboitem as SubType, m.comboitem as Status, rs.ComboItem as Source, ut.comboitem as UnitType from t_reservations r left outer join t_CombOitems rs on r.SourceID = rs.ComboItemID left outer join t_comboitems s on s.comboitemid = r.subtypeid left outer join t_comboitems t on t.comboitemid = r.typeid left outer join t_comboitems m on m.comboitemid = r.statusid left outer join t_prospect p on p.prospectid = r.prospectid left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid left outer join t_room o on o.roomid = x.roomid left outer join t_Unit un on o.Unitid = un.UnitID left outer join t_ComboItems ut on un.typeid = ut.comboitemid where r.checkindate between '" & sdate & "' and '" & edate & "' and un.typeid in (" & uType & ") and r.subtypeid in (" & sName & ") and r.typeid in (" & sType & ") and r.statusid in (" & sStatus & ") and r.reslocationid in (select c.comboitemid from t_comboitems c inner join t_Combos co on c.ComboID = co.ComboID where c.active = 1 and co.comboname = 'reservationlocation' and c.comboitem = 'KCP') order by r.checkindate, ut.comboitem"

                End If
            End If








            rs.open(sqlText, cn, 3, 3)

            'response.write "Type = " & request("type") & "<br>"
            'response.write "SubType = " & request("name") & "<br>"

            If rs.eof And rs.bof Then
                sAns = "No records match your criteria"
            Else
                sAns = "<TABLE>"
                sAns = sAns & "<tr align = center>"
                For i = 0 To rs.fields.count - 1
                    sAns = sAns & "<th>" & rs.fields(i).name & "</th>"
                Next
                If cbOpen.Checked Then
                    sAns = sAns & "<th>Balance</th>"
                End If
                sAns = sAns & "</tr>"

                Do While Not rs.eof
                    bProceed = True
                    If cbOpen.Checked Then
                        rs2.Open("Select Case when Sum(Balance) is null then 0 else Sum(balance) end as balance from UFN_Financials(0,'RESERVATIONID', " & rs.Fields("ReservationID").value & ",0)", cn, 3, 3)
                        If rs2.Fields("Balance").value = 0 Then
                            bProceed = False
                        Else
                            balance = rs2.Fields("Balance").Value
                        End If
                        rs2.Close()
                    End If
                    If bProceed Then
                        sAns = sAns & "<tr>"
                        For i = 0 To rs.fields.count - 1
                            If rs.fields(i).name = "Balance" Then
                                sAns = sAns & "<td align = center spacing = 10>" & FormatCurrency(rs.fields(i).value) & "</td>"
                            Else
                                sAns = sAns & "<td align = center spacing = 10>" & rs.fields(i).value & "</td>"
                            End If

                        Next
                        If cbOpen.Checked Then
                            sAns = sAns & "<td align = center spacing  = 10>" & FormatCurrency(balance) & "</td>"
                        End If
                        sAns = sAns & "</tr>"
                    End If
                    rs.movenext()
                Loop
                sAns = sAns & "</TABLE>"
                rs.close()
            End If
            cn.close()
            rs = Nothing
            rs2 = Nothing
            cn = Nothing
            litReport.Text = sAns

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
       
    End Sub

   
  
    Protected Sub lnk_view_2_Click(sender As Object, e As System.EventArgs) Handles lnk_view_2.Click
        multiview1.SetActiveView(view2)
    End Sub

    Protected Sub lnk_view_1_Click(sender As Object, e As System.EventArgs) Handles lnk_view_1.Click
        multiview1.SetActiveView(view1)
    End Sub

    Protected Sub btn_view2_submit_Click(sender As Object, e As System.EventArgs) Handles btn_view2_submit.Click

        Dim locations = String.Join(",", lbx_locations.Items.OfType(Of ListItem).Select(Function(x) x.Value).ToArray())
        Dim statuses = String.Join(",", lbx_statuses.Items.OfType(Of ListItem).Select(Function(x) x.Value).ToArray())
        Dim resorts = String.Join(",", lbxResortsChosen.Items.OfType(Of ListItem).Select(Function(x) x.Value).ToArray())
        Dim hotels = String.Join(",", lbxHotelChosen.Items.OfType(Of ListItem).Select(Function(x) x.Value).ToArray())

        Dim sb = New StringBuilder()

        If String.IsNullOrEmpty(checkin.Selected_Date) Or String.IsNullOrEmpty(checkout.Selected_Date) Then Return
        If String.IsNullOrEmpty(locations) Or String.IsNullOrEmpty(statuses) Or String.IsNullOrEmpty(resorts) Or String.IsNullOrEmpty(hotels) Then Return

        Dim sql = String.Format("select r.ReservationID, p.lastname + ', ' + p.firstname as [Guest], " & _
                                "(Select Top 1 Number from t_ProspectPhone where prospectid = p.ProspectID and Active = 1) as [Home Phone],  " & _
                                "datediff(day,r.checkindate, r.checkoutdate) as Nights, convert(varchar(8), r.CheckInDate, 1) [Check-In], convert(varchar(8), r.CheckOutDate, 1) [Check-Out],  " & _
                                "convert(varchar(8), r.DateBooked, 1) [Date Booked], " & _
                                "(select comboitem from t_ComboItems where ComboItemID = r.StatusID) [Status], " & _
                                "(select comboitem from t_ComboItems where ComboItemID = r.ResLocationID)[Location], " & _
                                "(select comboitem from t_ComboItems where ComboItemID = r.ResortCompanyID)[Resort],  " & _
                                "h.AccomName [Hotel], (select comboitem from t_ComboItems where ComboItemID = r.SourceID)[Source] " & _
                                "from t_Reservations r " & _
                                "inner join t_Accommodation a on r.ReservationID = a.ReservationID " & _
                                "inner join t_Accom h on h.AccomID = a.AccomID " & _
                                "inner join t_Prospect p on p.ProspectID = r.ProspectID " & _
                                "where r.CheckInDate between '{0}' and '{1}' " & _
                                "and r.StatusID in ({2})  " & _
                                "and r.ResLocationID in ({3}) " & _
                                "and r.ResortCompanyID in ({4}) " & _
                                "and h.AccomID in ({5}) " & _
                                "order by r.CheckInDate", checkin.Selected_Date, checkout.Selected_Date, statuses, locations, resorts, hotels)

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter(sql, cn)
                Dim dt = New DataTable()
                ad.Fill(dt)

                If balanced.Checked Then
                    dt.Columns.Add(New DataColumn("Balance"))
                    For Each dr As DataRow In dt.Rows
                        Try
                            Using cm = New SqlCommand(String.Format("select Case when Sum(Balance) is null then 0 else Sum(balance) end as balance from UFN_Financials(0,'RESERVATIONID', {0}, 0", dr("reservationid").ToString()), cn)
                                cn.Open()
                                dr("balance") = DirectCast(cm.ExecuteScalar(), Decimal)
                            End Using
                        Catch ex As Exception
                        Finally
                            cn.Close()
                        End Try
                    Next
                End If

                gv_outside_locations.DataSource = dt
                gv_outside_locations.DataBind()
            End Using
        End Using

    End Sub

    Protected Sub btn_add_to_lbx_status_Click(sender As Object, e As System.EventArgs) Handles btn_add_to_lbx_status.Click
        If ddl_statuses.SelectedValue = 0 Then
            Dim selected As New List(Of ListItem)

            For Each item In ddl_statuses.Items.OfType(Of ListItem).Where(Function(x) x.Value <> "0")
                lbx_statuses.Items.Add(item)
                selected.Add(item)
            Next
            For Each item In selected
                ddl_statuses.Items.Remove(item)
            Next
        Else
            lbx_statuses.Items.Add(ddl_statuses.SelectedItem)
            ddl_statuses.Items.Remove(ddl_statuses.SelectedItem)
        End If
    End Sub

    Protected Sub btn_add_to_lbx_location_Click(sender As Object, e As System.EventArgs) Handles btn_add_to_lbx_location.Click

        If ddl_locations.SelectedValue = 0 Then
            Dim selected As New List(Of ListItem)

            For Each item In ddl_locations.Items.OfType(Of ListItem).Where(Function(x) x.Value <> "0")
                lbx_locations.Items.Add(item)
                selected.Add(item)
            Next
            For Each item In selected
                ddl_locations.Items.Remove(item)
            Next
        Else
            lbx_locations.Items.Add(ddl_locations.SelectedItem)
            ddl_locations.Items.Remove(ddl_locations.SelectedItem)
        End If
    End Sub

    Protected Sub btn_rx_status_Click(sender As Object, e As System.EventArgs) Handles btn_rx_status.Click
        If lbx_statuses.Items.Count > 0 Then
            Dim selected As New List(Of ListItem)

            selected.AddRange(lbx_statuses.Items.OfType(Of ListItem).Where(Function(x) x.Selected).ToArray())
            ddl_statuses.ClearSelection()

            For Each item In selected
                ddl_statuses.Items.Add(item)
                ddl_statuses.ClearSelection()
            Next
            For Each item In selected
                lbx_statuses.Items.Remove(item)
            Next
        End If
    End Sub

    Protected Sub btn_rx_location_Click(sender As Object, e As System.EventArgs) Handles btn_rx_location.Click
        If lbx_locations.Items.Count > 0 Then
            Dim selected As New List(Of ListItem)

            selected.AddRange(lbx_locations.Items.OfType(Of ListItem).Where(Function(x) x.Selected).ToArray())
            ddl_locations.ClearSelection()

            For Each item In selected
                ddl_locations.Items.Add(item)
                ddl_locations.ClearSelection()
            Next
            For Each item In selected
                lbx_locations.Items.Remove(item)
            Next
        End If
    End Sub

    Protected Sub btnResortsAdd_Click(sender As Object, e As System.EventArgs) Handles btnResortsAdd.Click

        Dim ddl As DropDownList = ddlResortsList
        Dim lbx As ListBox = lbxResortsChosen

        If ddl.SelectedValue = 0 Then
            Dim selected As New List(Of ListItem)

            For Each item In ddl.Items.OfType(Of ListItem).Where(Function(x) x.Value <> "0")
                lbx.Items.Add(item)
                selected.Add(item)
            Next
            For Each item In selected
                ddl.Items.Remove(item)
            Next
        Else
            lbx.Items.Add(ddl.SelectedItem)
            ddl.Items.Remove(ddl.SelectedItem)
        End If
    End Sub

    Protected Sub btnResortRemove_Click(sender As Object, e As System.EventArgs) Handles btnResortRemove.Click
        Dim ddl As DropDownList = ddlResortsList
        Dim lbx As ListBox = lbxResortsChosen

        If lbx.Items.Count > 0 Then
            Dim selected As New List(Of ListItem)

            selected.AddRange(lbx.Items.OfType(Of ListItem).Where(Function(x) x.Selected).ToArray())
            ddl.ClearSelection()

            For Each item In selected
                ddl.Items.Add(item)
                ddl.ClearSelection()
            Next
            For Each item In selected
                lbx.Items.Remove(item)
            Next
        End If
    End Sub

    Protected Sub btnHotelAdd_Click(sender As Object, e As System.EventArgs) Handles btnHotelAdd.Click
        Dim ddl As DropDownList = ddlHotelList
        Dim lbx As ListBox = lbxHotelChosen

        If ddl.SelectedValue = 0 Then
            Dim selected As New List(Of ListItem)

            For Each item In ddl.Items.OfType(Of ListItem).Where(Function(x) x.Value <> "0")
                lbx.Items.Add(item)
                selected.Add(item)
            Next
            For Each item In selected
                ddl.Items.Remove(item)
            Next
        Else
            lbx.Items.Add(ddl.SelectedItem)
            ddl.Items.Remove(ddl.SelectedItem)
        End If
    End Sub

    Protected Sub btnHotelRemove_Click(sender As Object, e As System.EventArgs) Handles btnHotelRemove.Click
        Dim ddl As DropDownList = ddlHotelList
        Dim lbx As ListBox = lbxHotelChosen

        If lbx.Items.Count > 0 Then
            Dim selected As New List(Of ListItem)

            selected.AddRange(lbx.Items.OfType(Of ListItem).Where(Function(x) x.Selected).ToArray())
            ddl.ClearSelection()

            For Each item In selected
                ddl.Items.Add(item)
                ddl.ClearSelection()
            Next
            For Each item In selected
                lbx.Items.Remove(item)
            Next
        End If
    End Sub
End Class
