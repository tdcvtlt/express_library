
Imports System.Data
Imports System.Data.SqlClient

Partial Class Maintenance_Dashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Get_Data()
    End Sub

    Private Sub Get_Data()
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand(Get_SQL(1), cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        da.Fill(ds, "Table")
        cm.CommandText = Get_SQL(2)
        da.Fill(ds, "Table2")
        'cm.CommandText = Get_SQL(3)
        'da.Fill(ds, "Table3")
        cm.CommandText = Get_SQL(4) 'Quick Checks
        da.Fill(ds, "QC")
        cm.CommandText = Get_SQL(5) 'MasterCorp
        da.Fill(ds, "MC")

        GridView1.DataSource = ds.Tables("Table")
        GridView1.DataBind()

        GridView2.DataSource = ds.Tables("Table2")
        GridView2.DataBind()

        'gvFDStaff.DataSource = ds.Tables("Table3")
        'gvFDStaff.DataBind()
        'gvFDStaff.Visible = False

        gvQC.DataSource = ds.Tables("QC")
        gvQC.DataBind()

        gvMasterCorp.DataSource = ds.Tables("MC")
        gvMasterCorp.DataBind()

        da = Nothing
        ds = Nothing
        cm = Nothing
        cn = Nothing
    End Sub

    Private Function Get_SQL(index As Integer) As String
        Dim ret As String = ""
        Select Case index
            Case 1
                ' Removed week to date from query
                '"(select COUNT(distinct RequestID) from t_Request r2 inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) and DATEPART(week,r2.entrydate) = DATEPART(week,getdate()) and r2.AssignedToID=p.PersonnelID) as WTD, " &
                '"(select COUNT(distinct RequestID) from t_Request r2 inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) and DATEPART(week,r2.entrydate) = DATEPART(week,getdate()) and r2.AssignedToID=p.PersonnelID and s2.comboitem='Complete') as WTDCompleted, " &
                '"(select COUNT(distinct RequestID) from t_Request r2 inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) And DATEPART(week,r2.entrydate) = DATEPART(week,getdate()) And r2.AssignedToID=p.PersonnelID And s2.ComboItem = 'Not Started') as WTDNotStarted, " &
                '"(select COUNT(distinct RequestID) from t_Request r2 inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) And DATEPART(week,r2.entrydate) = DATEPART(week,getdate()) And r2.AssignedToID=p.PersonnelID And s2.ComboItem <> 'Complete' and s2.ComboItem <> 'Not Started') as WTDOther, " &

                ret = "select distinct case when p.firstname='MasterCorp' then ' ' + p.firstname else p.firstname end  + ' ' + p.LastName as Name, " &
                        "(select COUNT(distinct RequestID) from t_Request r2 inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) and MONTH(getDate()) = MONTH(r2.entrydate) and r2.AssignedToID=p.PersonnelID) as MTD, " &
                        "(select COUNT(distinct RequestID) from t_Request r2 inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) and MONTH(getDate()) = MONTH(r2.entrydate) and r2.AssignedToID=p.PersonnelID and s2.comboitem='Complete') as MTDCompleted, " &
                        "(select COUNT(distinct RequestID) from t_Request r2 inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) And MONTH(getDate()) = MONTH(r2.entrydate) And r2.AssignedToID=p.PersonnelID And s2.ComboItem = 'Not Started') as MTDNotStarted, " &
                        "(select COUNT(distinct RequestID) from t_Request r2 inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) And MONTH(getDate()) = MONTH(r2.entrydate) And r2.AssignedToID=p.PersonnelID And s2.ComboItem <> 'Complete' and s2.ComboItem <> 'Not Started') as MTDOther, " &
                        "(select COUNT(distinct RequestID) from t_Request r2 inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) and MONTH(getDate()) = MONTH(r2.entrydate) and Day(r2.entrydate) = Day(getdate()) and r2.AssignedToID=p.PersonnelID) as Today, " &
                        "(select COUNT(distinct RequestID) from t_Request r2 inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) and MONTH(getDate()) = MONTH(r2.entrydate) and Day(r2.entrydate) = Day(getdate()) and r2.AssignedToID=p.PersonnelID and s2.comboitem='Complete') as Completed, " &
                        "(select COUNT(distinct RequestID) from t_Request r2 inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) And MONTH(getDate()) = MONTH(r2.entrydate) And Day(r2.entrydate) = Day(getdate()) And r2.AssignedToID=p.PersonnelID And s2.ComboItem = 'Not Started') as NotStarted, " &
                        "(select COUNT(distinct RequestID) from t_Request r2 inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) And MONTH(getDate()) = MONTH(r2.entrydate) And Day(r2.entrydate) = Day(getdate()) And r2.AssignedToID=p.PersonnelID And s2.ComboItem <> 'Complete' and s2.ComboItem <> 'Not Started') as Other, " &
                        "(select COUNT(distinct r2.RequestID) from t_Request r2 inner join t_RequestParts pa on pa.RequestID=r2.RequestID inner join t_ComboItems ps on ps.ComboItemID = pa.StatusID inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) and MONTH(getDate()) = MONTH(r2.entrydate) and r2.AssignedToID=p.PersonnelID) as MTDParts, " &
                        "(select COUNT(distinct r2.RequestID) from t_Request r2 inner join t_RequestParts pa on pa.RequestID=r2.RequestID inner join t_ComboItems ps on ps.ComboItemID = pa.StatusID inner join t_ComboItems s2 on s2.ComboItemID=r2.StatusID where YEAR(getdate()) = YEAR(r2.entrydate) and MONTH(getDate()) = MONTH(r2.entrydate) and r2.AssignedToID=p.PersonnelID and ps.comboitem = 'Installed') as MTDPartsInstalled " &
                    "from t_Request r " &
                        "inner join t_ComboItems s on s.ComboItemID=r.StatusID " &
                        "left outer join t_Personnel p on p.PersonnelID = r.AssignedToID " &
                    "where YEAR(getdate()) = YEAR(r.entrydate) and MONTH(getDate()) = MONTH(r.entrydate)"
            Case 2
                ret = "select 'Check Ins' as Operation, ( " & _
                        "select COUNT(distinct r.reservationid) " & _
                        "From t_Reservations r " & _
                            "inner join t_ComboItems rs On rs.ComboItemID=r.StatusID " & _
                        "Where rs.ComboItem In ('Booked') and CheckInDate = cast(MONTH(getdate()) as varchar(2)) + '/' + CAST(day(GETDATE()) as varchar(2)) + '/' + CAST(year(getdate()) as varchar(4)) " & _
                            "And r.ReservationID In (Select ReservationID from t_RoomAllocationMatrix) " & _
                        ") as Today, ( " & _
                        "select COUNT(distinct r.reservationid) " & _
                        "From t_Reservations r " & _
                            "inner join t_ComboItems rs On rs.ComboItemID=r.StatusID " & _
                        "Where rs.ComboItem In ('In-House') and CheckInDate = cast(MONTH(getdate()) as varchar(2)) + '/' + CAST(day(GETDATE()) as varchar(2)) + '/' + CAST(year(getdate()) as varchar(4)) " & _
                            "And r.ReservationID In (Select ReservationID from t_RoomAllocationMatrix) " & _
                        ") as CheckedIn, " & _
                        "(" & _
                            "select COUNT(distinct r.reservationid) " & _
                            "From t_Reservations r " & _
                                "inner join t_ComboItems rs On rs.ComboItemID=r.StatusID " & _
                            "Where rs.ComboItem In ('Booked') and CheckInDate = cast(MONTH(dateadd(dd,1,getdate())) as varchar(2)) + '/' + CAST(day(dateadd(dd,1,getdate())) as varchar(2)) + '/' + CAST(year(dateadd(dd,1,getdate())) as varchar(4)) " & _
                            "And r.ReservationID In (Select ReservationID from t_RoomAllocationMatrix) " & _
                        ") as Tomorrow, " & _
                        "(" & _
                            "select COUNT(distinct r.reservationid) " & _
                            "From t_Reservations r " & _
                                "inner join t_ComboItems rs On rs.ComboItemID=r.StatusID " & _
                            "Where rs.ComboItem In ('Booked') and CheckInDate = cast(MONTH(dateadd(dd,2,getdate())) as varchar(2)) + '/' + CAST(day(dateadd(dd,2,getdate())) as varchar(2)) + '/' + CAST(year(dateadd(dd,2,getdate())) as varchar(4)) " & _
                            "And r.ReservationID In (Select ReservationID from t_RoomAllocationMatrix) " & _
                        ") as [2 Days Out] union " & _
                        "Select 'Check Outs' as Operation, (" & _
                        "select COUNT(distinct r.reservationid) " & _
                        "From t_Reservations r " & _
                            "inner join t_ComboItems rs On rs.ComboItemID=r.StatusID " & _
                        "Where rs.ComboItem In ('In-House') and CheckOutDate = cast(MONTH(getdate()) as varchar(2)) + '/' + CAST(day(GETDATE()) as varchar(2)) + '/' + CAST(year(getdate()) as varchar(4)) " & _
                            "And r.ReservationID In (Select ReservationID from t_RoomAllocationMatrix) " & _
                        ") as OutToday,  (" & _
                        "select COUNT(distinct r.reservationid) " & _
                        "From t_Reservations r " & _
                            "inner join t_ComboItems rs On rs.ComboItemID=r.StatusID " & _
                        "Where rs.ComboItem In ( 'Completed') and CheckOutDate = cast(MONTH(getdate()) as varchar(2)) + '/' + CAST(day(GETDATE()) as varchar(2)) + '/' + CAST(year(getdate()) as varchar(4)) " & _
                            "And r.ReservationID In (Select ReservationID from t_RoomAllocationMatrix) " & _
                        ") as OutNow, " & _
                        "(" & _
                            "select COUNT(distinct r.reservationid) " & _
                            "From t_Reservations r " & _
                                "inner join t_ComboItems rs On rs.ComboItemID=r.StatusID " & _
                            "Where rs.ComboItem In ('Booked','In-House') and CheckOutDate = cast(MONTH(dateadd(dd,1,getdate())) as varchar(2)) + '/' + CAST(day(dateadd(dd,1,getdate())) as varchar(2)) + '/' + CAST(year(dateadd(dd,1,getdate())) as varchar(4)) " & _
                            "And r.ReservationID In (Select ReservationID from t_RoomAllocationMatrix) " & _
                        ") as OutTomorrow, " & _
                        "(" & _
                            "select COUNT(distinct r.reservationid) " & _
                            "From t_Reservations r " & _
                                "inner join t_ComboItems rs On rs.ComboItemID=r.StatusID " & _
                            "Where rs.ComboItem In ('Booked','In-House') and CheckOutDate = cast(MONTH(dateadd(dd,2,getdate())) as varchar(2)) + '/' + CAST(day(dateadd(dd,2,getdate())) as varchar(2)) + '/' + CAST(year(dateadd(dd,2,getdate())) as varchar(4)) " & _
                            "And r.ReservationID In (Select ReservationID from t_RoomAllocationMatrix) " & _
                        ") as [Out-2 Days Out]"

            Case 3
                ret = "select p.Firstname + ' ' + p.lastname as Name, " & _
                        "( " & _
                        "	select COUNT(distinct e.keyvalue) " & _
                        "    From t_Event e " & _
                        "    where KeyField='ReservationID' " & _
                        "        And E.OldValue = 'Booked' " & _
                        "        And E.NewValue = 'In-House' " & _
                        "        And Year(E.DateCreated) = Year(getdate()) " & _
                        "        And Month(E.datecreated) = Month(getdate()) " & _
                        "        And Day(E.datecreated) = Day(getdate()) " & _
                        "        And E.CreatedByID = p.PersonnelID " & _
                        ") As Today, " & _
                        "(" & _
                        "    Select COUNT(distinct e.keyvalue) " & _
                        "    From t_Event e " & _
                        "    where KeyField='ReservationID' " & _
                        "        And E.OldValue = 'Booked' " & _
                        "        And E.NewValue = 'In-House' " & _
                        "        And Year(E.DateCreated) = Year(getdate()) " & _
                        "        And datepart(week,E.datecreated) = datepart(week, getdate()) " & _
                        "        And E.CreatedByID = p.PersonnelID " & _
                        ") As WTD, " & _
                        "(" & _
                        "    Select COUNT(distinct e.keyvalue) " & _
                        "    From t_Event e " & _
                        "    where KeyField='ReservationID' " & _
                        "        And E.OldValue = 'Booked' " & _
                        "        And E.NewValue = 'In-House' " & _
                        "        And Year(E.DateCreated) = Year(getdate()) " & _
                        "        And E.CreatedByID = p.PersonnelID " & _
                        ") As MTD " & _
                        "From t_Personnel p  " & _
                        "where p.PersonnelID In ( " & _
                        "    Select distinct e.CreatedByID " & _
                        "    From t_Event e " & _
                        "    where KeyField='ReservationID' " & _
                        "        And E.OldValue = 'Booked' " & _
                        "        And E.NewValue = 'In-House' " & _
                        "        And Year(E.DateCreated) = Year(getdate()) " & _
                        "        And Month(E.DateCreated) = Month(getdate()) " & _
                        "    ) " & _
                        "    And p.Active = 1 " & _
                        "    And p.FirstName <> 'AutoImport'"
            Case 4
                ret = "select cast(left(r.roomnumber,CHARINDEX('-',r.RoomNumber)-1) as int) as section, " & _
                      "    SUM(case when q.due = cast(datepart(mm, getdate()) As varchar(2)) + '/' + cast(datepart(dd,getdate()) as varchar(2)) + '/' + cast(datepart(yy, getdate()) as varchar(4)) then 1 else 0 end) as Due, " & _
                      "    coalesce((select InProg from (select cast(left(r.roomnumber,CHARINDEX('-',r.RoomNumber)-1) as int) as section,COUNT(*) as InProg from t_QuickCheckHist h inner join t_Room r on r.RoomID=h.RoomID inner join t_ComboItems s on s.ComboItemID=h.StatusID where s.ComboItem = 'In Progress' and h.StatusDate > cast(datepart(mm,getdate()) as varchar(2)) + '/' + cast(datepart(dd,getdate()) as varchar(2)) + '/' + cast(datepart(yy, getdate()) as varchar(4)) group by cast(left(r.roomnumber,CHARINDEX('-',r.RoomNumber)-1) as int)) a where a.section=cast(left(r.roomnumber,CHARINDEX('-',r.RoomNumber)-1) as int)),0) as InProgress, " & _
                      "    coalesce((select InProg from (select cast(left(r.roomnumber,CHARINDEX('-',r.RoomNumber)-1) as int) as section,COUNT(*) as InProg from t_QuickCheckHist h inner join t_Room r on r.RoomID=h.RoomID inner join t_ComboItems s on s.ComboItemID=h.StatusID where s.ComboItem = 'Complete' and h.StatusDate > cast(datepart(mm,getdate()) as varchar(2)) + '/' + cast(datepart(dd,getdate()) as varchar(2)) + '/' + cast(datepart(yy, getdate()) as varchar(4)) group by cast(left(r.roomnumber,CHARINDEX('-',r.RoomNumber)-1) as int)) a where a.section=cast(left(r.roomnumber,CHARINDEX('-',r.RoomNumber)-1) as int)),0) as Completed, " & _
                      "    coalesce(SUM(case when q.due = cast(datepart(mm, getdate() + 1) As varchar(2)) + '/' + cast(datepart(dd,getdate()+1) as varchar(2)) + '/' + cast(datepart(yy, getdate()+1) as varchar(4)) then 1 else 0 end),0) as Tomorrow " & _
                      "From ufn_QuickChecks(0,1) q  " & _
                      "    inner join t_Room r on r.RoomID = q.RoomID  " & _
                      "Group By cast(Left(r.roomnumber, CHARINDEX('-',r.RoomNumber)-1) as int) " & _
                      "order by cast(left(r.roomnumber, CHARINDEX('-',r.RoomNumber)-1) as int)"
            Case 5
                ret = "select cast(left(r.roomnumber,CHARINDEX('-',r.RoomNumber)-1) as int) as section, " & _
                      "    SUM(case when q.due = 1 and q.datedue = cast(datepart(mm,getdate()) as varchar(2)) + '/' + cast(datepart(dd,getdate()) as varchar(2)) + '/' + cast(datepart(yy, getdate()) as varchar(4)) then 1 else 0 end) as Due, " & _
                      "    SUM(case when q.EarlyCheckIn = 1 and q.datedue = cast(datepart(mm,getdate()) as varchar(2)) + '/' + cast(datepart(dd,getdate()) as varchar(2)) + '/' + cast(datepart(yy, getdate()) as varchar(4)) then 1 else 0 end) as EarlyCheckIn, " & _
                      "    SUM(case when q.Completed = 1 and q.datedue = cast(datepart(mm,getdate()) as varchar(2)) + '/' + cast(datepart(dd,getdate()) as varchar(2)) + '/' + cast(datepart(yy, getdate()) as varchar(4)) then 1 else 0 end) as Completed, " & _
                      "    SUM(case when q.due = 1 and q.datedue > cast(datepart(mm,getdate()) as varchar(2)) + '/' + cast(datepart(dd,getdate()) as varchar(2)) + '/' + cast(datepart(yy, getdate()) as varchar(4)) then 1 else 0 end) as Tomorrow " & _
                      "From ufn_MasterCorp(0,1) q  " & _
                      "    inner join t_Room r on r.RoomID = q.RoomID  " & _
                      "Group By cast(Left(r.roomnumber, CHARINDEX('-',r.RoomNumber)-1) as int) " & _
                      "order by cast(left(r.roomnumber, CHARINDEX('-',r.RoomNumber)-1) as int)"
            Case Else
        End Select


        Return ret
    End Function

    Private Sub GridView1_DataBound(sender As Object, e As EventArgs) Handles GridView1.DataBound
        Dim grid As GridView = CType(sender, GridView)
        Dim row As New GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal)
        Dim left As TableCell = New TableHeaderCell
        left.ColumnSpan = 1
        left.Text = ""
        row.Cells.Add(left)
        Dim today As TableCell = New TableHeaderCell
        today.ColumnSpan = 4
        today.Text = "Today"
        row.Cells.Add(today)
        'Dim week As TableCell = New TableHeaderCell
        'week.ColumnSpan = 4
        'week.Text = "Week To Date"
        'row.Cells.Add(week)
        Dim month As TableCell = New TableHeaderCell
        month.ColumnSpan = 4
        month.Text = "Month To Date"
        row.Cells.Add(month)

        'row.BackColor = Drawing.Color.Fuchsia
        If grid.Controls.Count > 0 Then
            Dim t As Table = CType(grid.Controls(0), Table)
            t.Rows.AddAt(0, row)
        End If
    End Sub

    Private Sub gvQC_DataBound(sender As Object, e As EventArgs) Handles gvQC.DataBound
        Dim grid As GridView = CType(sender, GridView)
        Dim row As New GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal)
        Dim left As TableCell = New TableHeaderCell
        left.ColumnSpan = 5
        left.Text = "Room Checks"
        row.Cells.Add(left)
        If grid.Controls.Count > 0 Then
            Dim t As Table = CType(grid.Controls(0), Table)
            t.Rows.AddAt(0, row)
        End If
    End Sub

    Dim _DueTotal As Integer = 0
    Dim _IPTotal As Integer = 0
    Dim _CompTotal As Integer = 0
    Dim _TomTotal As Integer = 0

    Private Sub gvQC_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvQC.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            _DueTotal += e.Row.DataItem("Due")
            _IPTotal += e.Row.DataItem("InProgress")
            _CompTotal += e.Row.DataItem("Completed")
            _TomTotal += e.Row.DataItem("Tomorrow")
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(0).Font.Bold = True
            e.Row.Cells(0).Text = "Totals"
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(1).Font.Bold = True
            e.Row.Cells(1).Text = _DueTotal
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(2).Font.Bold = True
            e.Row.Cells(2).Text = _IPTotal
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(3).Font.Bold = True
            e.Row.Cells(3).Text = _CompTotal
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(4).Font.Bold = True
            e.Row.Cells(4).Text = _TomTotal
        End If
    End Sub

    Private Sub gvMasterCorp_DataBound(sender As Object, e As EventArgs) Handles gvMasterCorp.DataBound
        Dim grid As GridView = CType(sender, GridView)
        Dim row As New GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal)
        Dim left As TableCell = New TableHeaderCell
        left.ColumnSpan = 5
        left.Text = "MasterCorp Cleans"
        row.Cells.Add(left)
        If grid.Controls.Count > 0 Then
            Dim t As Table = CType(grid.Controls(0), Table)
            t.Rows.AddAt(0, row)
        End If
    End Sub

    Dim _MCDueTotal As Integer = 0
    Dim _MCIPTotal As Integer = 0
    Dim _MCCompTotal As Integer = 0
    Dim _MCTomTotal As Integer = 0

    Private Sub gvMasterCorp_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMasterCorp.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            _MCDueTotal += e.Row.DataItem("Due")
            _MCIPTotal += e.Row.DataItem("EarlyCheckIn")
            _MCCompTotal += e.Row.DataItem("Completed")
            _MCTomTotal += e.Row.DataItem("Tomorrow")
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(0).Font.Bold = True
            e.Row.Cells(0).Text = "Totals"
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(1).Font.Bold = True
            e.Row.Cells(1).Text = _MCDueTotal
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(2).Font.Bold = True
            e.Row.Cells(2).Text = _MCIPTotal
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(3).Font.Bold = True
            e.Row.Cells(3).Text = _MCCompTotal
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(4).Font.Bold = True
            e.Row.Cells(4).Text = _MCTomTotal
        End If
    End Sub
End Class
