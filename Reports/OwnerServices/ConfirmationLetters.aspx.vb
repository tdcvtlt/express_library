Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Security
Imports System.Web
Imports System.Configuration
Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq


#Region "Page"

Partial Class Reports_OwnerServices_ConfirmationLetters
    Inherits System.Web.UI.Page

#Region "Page Variables"
    Private Enum EnLetter
        Resort = 1
        RCI = 2
        II = 3
        ICE = 4
    End Enum

    Private Letter As EnLetter
    Private Keys() As String
#End Region

#Region "Page Events & Handlers"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (IsPostBack = True) Then
            If (Not Session("Crystal") Is Nothing) Then
                CrystalViewer.ReportSource = Session("Crystal")
            End If
        End If
    End Sub


    Private Sub Print()

        If Not Session("Crystal") Is Nothing Then
            Session("Crystal") = Nothing
        End If

        Keys = hfKeys.Value.Split(New Char() {"&"})
        Letter = Keys(0)

        Dim type As String = String.Empty        
        Dim sql As String = String.Empty
        Dim tryDate As DateTime
        Dim reservations As String = Nothing

        Dim isDate As Boolean = DateTime.TryParse(Keys(1), tryDate)
        If (isDate = False) Then
            reservations = String.Join(",", Keys)
        End If


        If (Letter = EnLetter.II) Then
            type = "II Membership Number"
        ElseIf (Letter = EnLetter.ICE) Then
            type = "ICE Membership Number"
        ElseIf (Letter = EnLetter.RCI) Then
            type = "RCI Membership Number"
        ElseIf (Letter = EnLetter.Resort) Then
            type = "KCP"
        End If

 
        Using cnn As New SqlConnection(Resources.Resource.cns)

            If (type.Contains("KCP") = False And isDate = True) Then

                sql = String.Format("select p.FirstName + ' ' + p.LastName as ProspectName, COALESCE(p.SpouseFirstName, p.SpouseLastName) as Spouse, " & _
                    "PA.Address1 Address, PA.Address2 As Address2, PA.City, PA.ComboItem AS State, PA.PostalCode, UFV.UFValue Exchange, u.UsageID UsageID, rt.ComboItem as RoomType, u.DateCreated " & _
                    "FROM t_Prospect p  " & _
                    "INNER JOIN ( " & _
                    "SELECT * FROM T_ProspectAddress PA " & _
                    "LEFT OUTER JOIN T_COMBOITEMS CBI ON  " & _
                    "PA.StateID = CBI.ComboItemID " & _
                    "Where PA.ActiveFlag = '1' " & _
                    ") PA ON PA.ProspectID = P.ProspectID	" & _
                    "LEFT OUTER JOIN ( " & _
                    "	SELECT UFValue, KeyValue FROM t_UF_Value UV  " & _
                    "	LEFT OUTER JOIN t_UFields UF ON UF.UFID = UV.UFID  " & _
                    "	WHERE UF.UFName = '{0}') AS UFV ON UFV.KeyValue = p.ProspectID " & _
                    "left outer join t_Contract c on c.ProspectID = p.ProspectID " & _
                    "left outer join t_Usage u on u.ContractID = c.ContractID " & _
                    "left outer join t_ComboItems ut on ut.ComboItemID = u.TypeID " & _
                    "left outer join t_ComboItems sta on sta.ComboItemID = u.StatusID 		" & _
                    "left outer join t_ComboItems rt on rt.ComboItemID = u.RoomTypeID " & _
                    "left outer join t_ComboItems ust on ust.ComboItemID = u.SubTypeID " & _
                    "where u.DateCreated between '{1}' and  dateadd(d, 1, '{2}') and ut.ComboItem = 'Exchange' and " & _
                    "ust.comboitem = '{3}' and p.LastName not in ('Developer','Plan with tan','Marketing','Pool','Tax','BulkBank')", _
                    type, Keys(1), Keys(2), Letter)


            ElseIf (type.Contains("KCP") = False And isDate = False) Then

                sql = String.Format("select p.FirstName + ' ' + p.LastName as ProspectName, COALESCE(p.SpouseFirstName, p.SpouseLastName) as Spouse, " & _
                    "PA.Address1 Address, PA.Address2 as Address2, PA.City, PA.ComboItem AS State, PA.PostalCode, UFV.UFValue Exchange, u.UsageID UsageID, rt.ComboItem as RoomType, u.DateCreated " & _
                    "FROM t_Prospect p  " & _
                    "INNER JOIN ( " & _
                    "SELECT * FROM T_ProspectAddress PA " & _
                    "LEFT OUTER JOIN T_COMBOITEMS CBI ON  " & _
                    "PA.StateID = CBI.ComboItemID " & _
                    "Where PA.ActiveFlag = '1' " & _
                    ") PA ON PA.ProspectID = P.ProspectID	" & _
                    "LEFT OUTER JOIN ( " & _
                    "	SELECT UFValue, KeyValue FROM t_UF_Value UV  " & _
                    "	LEFT OUTER JOIN t_UFields UF ON UF.UFID = UV.UFID  " & _
                    "	WHERE UF.UFName = '{0}') AS UFV ON UFV.KeyValue = p.ProspectID " & _
                    "left outer join t_Contract c on c.ProspectID = p.ProspectID " & _
                    "left outer join t_Usage u on u.ContractID = c.ContractID " & _
                    "left outer join t_ComboItems ut on ut.ComboItemID = u.TypeID " & _
                    "left outer join t_ComboItems sta on sta.ComboItemID = u.StatusID 		" & _
                    "left outer join t_ComboItems rt on rt.ComboItemID = u.RoomTypeID " & _
                    "left outer join t_ComboItems ust on ust.ComboItemID = u.SubTypeID " & _
                    "where UsageID IN ({1})", type, reservations)

            ElseIf (type.Contains("KCP") = True And isDate = True) Then
                sql = String.Format( _
                    "SELECT distinct r.ReservationID, p.ProspectID, p.FirstName + ' ' + p.LastName as ProspectName, " & _
                    "COALESCE(p.SpouseFirstName, p.SpouseLastName) as Spouse, PA.Address1 Address, PA.Address2 as Address2, " & _
                    "PA.City, PA.ComboItem AS State, PA.PostalCode, " & _
                    "			r.CheckInDate, r.CheckOutDate,  " & _
                    "			( " & _
                    "			select cast(sum(cast(left(ty.comboitem,1)as int))as varchar(2)) as RoomSize " & _
                    "			from  t_Room rm " & _
                    "			INNER JOIN t_ComboItems ty on ty.ComboItemID = rm.TypeID " & _
                    "			where rm.roomid in (select distinct roomid from t_Roomallocationmatrix where reservationid = r.reservationid) " & _
                    "			) + 'BD' as RoomSize, " & _
                    "			( " & _
                    "           Select sum(MaxOccupancy) " & _
                    "			from t_Room rm " & _
                    "			where rm.roomid in (select distinct roomid from t_Roomallocationmatrix where reservationid = r.reservationid) " & _
                    "			) as MaxOccupancy " & _
                    "		FROM t_Prospect p " & _
                    "			INNER JOIN ( " & _
                    "			SELECT * FROM T_ProspectAddress PA " & _
                    "			LEFT OUTER JOIN T_COMBOITEMS CBI ON " & _
                    "                PA.StateID = CBI.ComboItemID " & _
                    "           Where PA.ActiveFlag = '1' " & _
                    "		) PA ON PA.ProspectID = P.ProspectID	" & _
                    "		INNER JOIN t_Reservations r on r.ProspectID = p.ProspectID " & _
                    "		INNER JOIN t_ComboItems l on l.ComboItemID = r.ResLocationID " & _
                    "		INNER JOIN t_Comboitems s on s.ComboitemID = r.StatusID " & _
                    "		INNER JOIN t_ComboItems t on t.COmboItemID = r.TypeID " & _
                    "		WHERE r.checkindate between '{0}' and dateadd(d, 1, '{1}') " & _
                    "and l.ComboItem = 'KCP' and s.ComboItem = 'Booked' and t.ComboItem = 'Owner'", Keys(1), Keys(2))


            ElseIf (type.Contains("KCP") = True And isDate = False) Then

                sql = String.Format( _
                    "SELECT distinct r.ReservationID, p.ProspectID, p.FirstName + ' ' + p.LastName as ProspectName, " & _
                    "COALESCE(p.SpouseFirstName, p.SpouseLastName) as Spouse, PA.Address1 Address, PA.Address2 as Address2 " & _
                    "PA.City, PA.ComboItem AS State, PA.PostalCode, " & _
                    "			r.CheckInDate, r.CheckOutDate,  " & _
                    "			( " & _
                    "			select cast(sum(cast(left(ty.comboitem,1)as int))as varchar(2)) as RoomSize " & _
                    "			from  t_Room rm " & _
                    "			INNER JOIN t_ComboItems ty on ty.ComboItemID = rm.TypeID " & _
                    "			where rm.roomid in (select distinct roomid from t_Roomallocationmatrix where reservationid = r.reservationid) " & _
                    "			) + 'BD' as RoomSize, " & _
                    "			( " & _
                    "           Select sum(MaxOccupancy) " & _
                    "			from t_Room rm " & _
                    "			where rm.roomid in (select distinct roomid from t_Roomallocationmatrix where reservationid = r.reservationid) " & _
                    "			) as MaxOccupancy " & _
                    "		FROM t_Prospect p " & _
                    "			INNER JOIN ( " & _
                    "			SELECT * FROM T_ProspectAddress PA " & _
                    "			LEFT OUTER JOIN T_COMBOITEMS CBI ON " & _
                    "                PA.StateID = CBI.ComboItemID " & _
                    "           Where PA.ActiveFlag = '1' " & _
                    "		) PA ON PA.ProspectID = P.ProspectID	" & _
                    "		INNER JOIN t_Reservations r on r.ProspectID = p.ProspectID " & _
                    "		INNER JOIN t_ComboItems l on l.ComboItemID = r.ResLocationID " & _
                    "		INNER JOIN t_Comboitems s on s.ComboitemID = r.StatusID " & _
                    "		INNER JOIN t_ComboItems t on t.COmboItemID = r.TypeID " & _
                    "		WHERE r.ReservationID in ({0}) " & _
                    "and l.ComboItem = 'KCP' and s.ComboItem = 'Booked' and t.ComboItem = 'Owner'", reservations)

            End If           

            Using ada As New SqlDataAdapter(sql, cnn)

                Dim data As New ConfirmationLetterSchema.ConfirmationLetterDataTable()                

                Try
                    ada.Fill(data)
                Catch ex As Exception
                    Throw New ApplicationException(ex.ToString())
                End Try
          
                Dim rd As New ReportDocument()
                Dim path As String = "reportfiles/"

                If (Letter = EnLetter.II) Then
                    path += "II.rpt"
                ElseIf (Letter = EnLetter.ICE) Then
                    path += "ICE.rpt"
                ElseIf (Letter = EnLetter.RCI) Then
                    path += "RCI.rpt"
                ElseIf (Letter = EnLetter.Resort) Then
                    path += "KCP.rpt"
                End If

                rd.Load(Server.MapPath(path))                
                rd.SetDataSource(DirectCast(data, DataTable))
                
                Session("Crystal") = rd
                CrystalViewer.ReportSource = Session("Crystal")

            End Using
        End Using
    End Sub


#End Region

    
    Protected Sub RunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunReport.Click
        Print()
    End Sub
End Class

#End Region
