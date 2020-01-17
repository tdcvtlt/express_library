
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine

Partial Class Reports_OwnerServices_ConfirmationLetters_Resort_And_Banking_
    Inherits System.Web.UI.Page

    Private Sub Reports_OwnerServices_ConfirmationLetters_Resort_And_Banking__Load(sender As Object, e As EventArgs) Handles Me.Load
        If (IsPostBack = True) Then
            If (Not Session("Crystal") Is Nothing) Then
                CrystalReportViewer1.ReportSource = Session("Crystal")
            End If
        Else
            Session("Crystal") = Nothing
            rbReservationID.Checked = True
            rbResortStay.Checked = True
        End If
        lbErr.Text = String.Empty
    End Sub
    Protected Sub btSubmit_Click(sender As Object, e As EventArgs) Handles btSubmit.Click
        If rbDateRange.Checked And (String.IsNullOrEmpty(fromDate.Selected_Date) Or String.IsNullOrEmpty(toDate.Selected_Date)) Then
            lbErr.Text = String.Format("Please fill in the Date Range boxes.")
            Return
        End If

        Dim letter = String.Empty
        Dim type = String.Empty
        Dim reservationIDs = hfReservationID.Value
        If rbICE.Checked Then
            letter = "ICE"
            type = "ICE Membership Number"
        End If
        If rbII.Checked Then
            letter = "II"
            type = "II Membership Number"
        End If
        If rbRCI.Checked Then
            letter = "RCI"
            type = "RCI Membership Number"
        End If
        If rbResortStay.Checked Then letter = "Resort"


        Using cn As New SqlConnection(Resources.Resource.cns)
            Dim sql = String.Empty

            If rbDateRange.Checked Then
                If rbResortStay.Checked Then
                    sql = String.Format(
                     "SELECT distinct r.ReservationID, p.ProspectID, p.FirstName + ' ' + p.LastName as ProspectName, " &
                     "COALESCE(p.SpouseFirstName, p.SpouseLastName) as Spouse, PA.Address1 Address, PA.Address2 as Address2, " &
                     "PA.City, PA.ComboItem AS State, PA.PostalCode, " &
                     "			r.CheckInDate, r.CheckOutDate,  " &
                     "			( " &
                     "			select cast(sum(cast(left(ty.comboitem,1)as int))as varchar(2)) as RoomSize " &
                     "			from  t_Room rm " &
                     "			INNER JOIN t_ComboItems ty on ty.ComboItemID = rm.TypeID " &
                     "			where rm.roomid in (select distinct roomid from t_Roomallocationmatrix where reservationid = r.reservationid) " &
                     "			) + 'BD' as RoomSize, " &
                     "			( " &
                     "           Select sum(MaxOccupancy) " &
                     "			from t_Room rm " &
                     "			where rm.roomid in (select distinct roomid from t_Roomallocationmatrix where reservationid = r.reservationid) " &
                     "			) as MaxOccupancy " &
                     "		FROM t_Prospect p " &
                     "			INNER JOIN ( " &
                     "			SELECT * FROM T_ProspectAddress PA " &
                     "			LEFT OUTER JOIN T_COMBOITEMS CBI ON " &
                     "                PA.StateID = CBI.ComboItemID " &
                     "           Where PA.ActiveFlag = '1' " &
                     "		) PA ON PA.ProspectID = P.ProspectID	" &
                     "		INNER JOIN t_Reservations r on r.ProspectID = p.ProspectID " &
                     "		INNER JOIN t_ComboItems l on l.ComboItemID = r.ResLocationID " &
                     "		INNER JOIN t_Comboitems s on s.ComboitemID = r.StatusID " &
                     "		INNER JOIN t_ComboItems t on t.COmboItemID = r.TypeID " &
                     "		WHERE r.checkindate between '{0}' and dateadd(d, 1, '{1}') " &
                     "and l.ComboItem = 'KCP' and s.ComboItem = 'Booked' and t.ComboItem = 'Owner'", fromDate.Selected_Date, toDate.Selected_Date)
                ElseIf rbICE.Checked Or rbII.Checked Or rbRCI.Checked Then
                    sql = String.Format("select p.FirstName + ' ' + p.LastName as ProspectName, COALESCE(p.SpouseFirstName, p.SpouseLastName) as Spouse, " &
                       "PA.Address1 Address, PA.Address2 As Address2, PA.City, PA.ComboItem AS State, PA.PostalCode, UFV.UFValue Exchange, u.UsageID UsageID, rt.ComboItem as RoomType, u.DateCreated " &
                       "FROM t_Prospect p  " &
                       "INNER JOIN ( " &
                       "SELECT * FROM T_ProspectAddress PA " &
                       "LEFT OUTER JOIN T_COMBOITEMS CBI ON  " &
                       "PA.StateID = CBI.ComboItemID " &
                       "Where PA.ActiveFlag = '1' " &
                       ") PA ON PA.ProspectID = P.ProspectID	" &
                       "LEFT OUTER JOIN ( " &
                       "	SELECT UFValue, KeyValue FROM t_UF_Value UV  " &
                       "	LEFT OUTER JOIN t_UFields UF ON UF.UFID = UV.UFID  " &
                       "	WHERE UF.UFName = '{0}') AS UFV ON UFV.KeyValue = p.ProspectID " &
                       "left outer join t_Contract c on c.ProspectID = p.ProspectID " &
                       "left outer join t_Usage u on u.ContractID = c.ContractID " &
                       "left outer join t_ComboItems ut on ut.ComboItemID = u.TypeID " &
                       "left outer join t_ComboItems sta on sta.ComboItemID = u.StatusID 		" &
                       "left outer join t_ComboItems rt on rt.ComboItemID = u.RoomTypeID " &
                       "left outer join t_ComboItems ust on ust.ComboItemID = u.SubTypeID " &
                       "where u.DateCreated between '{1}' and  dateadd(d, 1, '{2}') and ut.ComboItem = 'Exchange' and " &
                       "ust.comboitem = '{3}' and p.LastName not in ('Developer','Plan with tan','Marketing','Pool','Tax','BulkBank')",
                       "KCP", fromDate.Selected_Date, toDate.Selected_Date, letter)
                End If
            ElseIf rbReservationID.Checked Then
                If rbResortStay.Checked Then
                    sql = String.Format(
                       "SELECT distinct r.ReservationID, p.ProspectID, p.FirstName + ' ' + p.LastName as ProspectName, " &
                       "COALESCE(p.SpouseFirstName, p.SpouseLastName) as Spouse, PA.Address1 Address, PA.Address2 as Address2, " &
                       "PA.City, PA.ComboItem AS State, PA.PostalCode, " &
                       "			r.CheckInDate, r.CheckOutDate,  " &
                       "			( " &
                       "			select cast(sum(cast(left(ty.comboitem,1)as int))as varchar(2)) as RoomSize " &
                       "			from  t_Room rm " &
                       "			INNER JOIN t_ComboItems ty on ty.ComboItemID = rm.TypeID " &
                       "			where rm.roomid in (select distinct roomid from t_Roomallocationmatrix where reservationid = r.reservationid) " &
                       "			) + 'BD' as RoomSize, " &
                       "			( " &
                       "           Select sum(MaxOccupancy) " &
                       "			from t_Room rm " &
                       "			where rm.roomid in (select distinct roomid from t_Roomallocationmatrix where reservationid = r.reservationid) " &
                       "			) as MaxOccupancy " &
                       "		FROM t_Prospect p " &
                       "			INNER JOIN ( " &
                       "			SELECT * FROM T_ProspectAddress PA " &
                       "			LEFT OUTER JOIN T_COMBOITEMS CBI ON " &
                       "                PA.StateID = CBI.ComboItemID " &
                       "           Where PA.ActiveFlag = '1' " &
                       "		) PA ON PA.ProspectID = P.ProspectID	" &
                       "		INNER JOIN t_Reservations r on r.ProspectID = p.ProspectID " &
                       "		INNER JOIN t_ComboItems l on l.ComboItemID = r.ResLocationID " &
                       "		INNER JOIN t_Comboitems s on s.ComboitemID = r.StatusID " &
                       "		INNER JOIN t_ComboItems t on t.COmboItemID = r.TypeID " &
                       "		WHERE r.ReservationID in ({0}) " &
                       "and l.ComboItem = 'KCP' and s.ComboItem = 'Booked' and t.ComboItem = 'Owner'", reservationIDs)


                ElseIf rbICE.Checked Or rbII.Checked Or rbRCI.Checked Then
                    sql = String.Format("select p.FirstName + ' ' + p.LastName as ProspectName, COALESCE(p.SpouseFirstName, p.SpouseLastName) as Spouse, " &
                    "PA.Address1 Address, PA.Address2 as Address2, PA.City, PA.ComboItem AS State, PA.PostalCode, UFV.UFValue Exchange, u.UsageID UsageID, rt.ComboItem as RoomType, u.DateCreated " &
                    "FROM t_Prospect p  " &
                    "INNER JOIN ( " &
                    "SELECT * FROM T_ProspectAddress PA " &
                    "LEFT OUTER JOIN T_COMBOITEMS CBI ON  " &
                    "PA.StateID = CBI.ComboItemID " &
                    "Where PA.ActiveFlag = '1' " &
                    ") PA ON PA.ProspectID = P.ProspectID	" &
                    "LEFT OUTER JOIN ( " &
                    "	SELECT UFValue, KeyValue FROM t_UF_Value UV  " &
                    "	LEFT OUTER JOIN t_UFields UF ON UF.UFID = UV.UFID  " &
                    "	WHERE UF.UFName = '{0}') AS UFV ON UFV.KeyValue = p.ProspectID " &
                    "left outer join t_Contract c on c.ProspectID = p.ProspectID " &
                    "left outer join t_Usage u on u.ContractID = c.ContractID " &
                    "left outer join t_ComboItems ut on ut.ComboItemID = u.TypeID " &
                    "left outer join t_ComboItems sta on sta.ComboItemID = u.StatusID 		" &
                    "left outer join t_ComboItems rt on rt.ComboItemID = u.RoomTypeID " &
                    "left outer join t_ComboItems ust on ust.ComboItemID = u.SubTypeID " &
                    "where UsageID IN ({1})", type, reservationIDs)
                End If
            End If

            If String.IsNullOrEmpty(sql) = False Then
                Using ada As New SqlDataAdapter(sql, cn)

                    Dim data As Object = Nothing
                    Dim rd As New ReportDocument()
                    Dim path As String = "reportfiles/"

                    If rbICE.Checked Then
                        path += "ICE.rpt"
                        data = New ConfirmationLetterSchema.ConfirmationLetterDataTable()
                    End If
                    If rbII.Checked Then
                        path += "II.rpt"
                        data = New ConfirmationLetterSchema.ConfirmationLetterDataTable()
                    End If
                    If rbRCI.Checked Then
                        path += "RCI.rpt"
                        data = New ConfirmationLetterSchema.ConfirmationLetterDataTable()
                    End If
                    If rbResortStay.Checked Then
                        path += "KCP.rpt"
                        data = New ConfirmationLetterSchema.KcpLetterDataTable()
                    End If
                    Try
                        ada.Fill(data)
                    Catch ex As Exception
                    End Try

                    Dim dt = CType(data, DataTable)
                    If dt.Rows.Count > 0 Then
                        rd.Load(Server.MapPath(path))
                        rd.SetDataSource(DirectCast(data, DataTable))
                        Session("Crystal") = rd
                        CrystalReportViewer1.ReportSource = Session("Crystal")
                    Else
                        Session("Crystal") = Nothing
                        CrystalReportViewer1.ReportSource = Nothing
                    End If
                End Using
            End If
        End Using
    End Sub
End Class
