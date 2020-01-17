Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class Maintenance_Request
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Select Case LCase(ddFilter.Text)
            Case "room"
                If filter.text <> "" Then
                    Run_Query("select r.RequestID, r.EntryDate, p.Firstname + ' ' + p.LastName as AssignedTo, rl.ComboItem as GeneratedBy, Case when r.KeyField = 'RoomID' then room.RoomNumber else pm.Name end as Location, r.Subject, u.UserName, s.comboitem as Status from t_Request r left outer join t_Comboitems s on s.comboitemid = r.statusid left outer join t_PMBuilding pm on pm.PMBuildingID = r.KeyValue left outer join t_Room room on room.roomid = r.KeyValue left outer join t_Personnel p on r.AssignedToID = p.PersonnelID left outer join t_CombOItems rl on r.RequestAreaID = rl.ComboItemID left outer join t_Personnel u on r.EnteredByID = u.PersonnelID where Case when r.KeyField = 'RoomID' then room.RoomNumber else pm.Name end like '" & filter.Text & "%' order by r.RequestID desc")
                Else
                    Run_Query("Select Top 1000 r.RequestID, r.EntryDate, p.FirstName + ' ' + p.LastName as AssignedTo, rl.ComboItem as GeneratedBy, Case when r.KeyField = 'RoomID' then room.RoomNumber else pm.Name end as Location, r.Subject, u.UserName, s.comboitem as Status from t_Request r left outer join t_Comboitems s on s.comboitemid = r.statusid left outer join t_PMBuilding pm on pm.PMBuildingID = r.KeyValue left outer join t_Room room on room.roomid = r.KeyValue left outer join t_Personnel p on r.AssignedToID = p.PersonnelID left outer join t_CombOItems rl on r.RequestAreaID = rl.ComboItemID left outer join t_Personnel u on r.EnteredByID = u.PersonnelID where r.statusid in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'workorderstatus' and comboitem not in ('complete','cancelled')) order by r.RequestID desc")
                End If
            Case "date"
                If filter.Text <> "" Then
                    If IsDate(filter.Text) Then
                        Run_Query("select r.RequestID,r.EntryDate, p.FirstName + ' ' + p.Lastname as AssignedTo, rl.ComboItem as GeneratedBy, Case when r.KeyField = 'RoomID' then room.RoomNumber else pm.Name end as Location, r.Subject, u.UserName, s.comboitem as Status from t_Request r left outer join t_Comboitems s on s.comboitemid = r.statusid left outer join t_PMBuilding pm on pm.PMBuildingID = r.KeyValue left outer join t_Room room on room.roomid = r.KeyValue left outer join t_Personnel p on r.AssignedToID = p.PersonnelID left outer join t_CombOItems rl on r.RequestAreaID = rl.ComboItemID left outer join t_Personnel u on r.EnteredByID = u.PersonnelID where r.entrydate between '" & filter.Text & "' and '" & CDate(filter.Text).AddDays(1) & "'")
                    Else
                        Run_Query("select r.RequestID,r.EntryDate, p.FirstName + ' ' + p.LastName as AssignedTo, rl.ComboItem as GeneratedBy, Case when r.KeyField = 'RoomID' then room.RoomNumber else pm.Name end as Location, r.Subject, u.UserName, s.comboitem as Status from t_Request r left outer join t_Comboitems s on s.comboitemid = r.statusid left outer join t_PMBuilding pm on pm.PMBuildingID = r.KeyValue left outer join t_Room room on room.roomid = r.KeyValue left outer join t_Personnel p on r.AssignedToID = p.PersonnelID left outer join t_CombOItems rl on r.RequestAreaID = rl.ComboItemID left outer join t_Personnel u on r.EnteredByID = u.PersonnelID where r.entrydate like '%" & filter.Text & "%'")
                    End If
                Else
                    Run_Query("select Top 1000 r.RequestID, r.EntryDate, p.Firstname + ' ' + p.LastName as AssignedTo, rl.ComboItem as GeneratedBy, Case when r.KeyField = 'RoomID' then room.RoomNumber else pm.Name end as Location, r.Subject, u.UserName, s.comboitem as Status from t_Request r left outer join t_Comboitems s on s.comboitemid = r.statusid left outer join t_PMBuilding pm on pm.PMBuildingID = r.KeyValue left outer join t_Room room on room.roomid = r.KeyValue left outer join t_Personnel p on r.AssignedToID = p.PersonnelID left outer join t_CombOItems rl on r.RequestAreaID = rl.ComboItemID left outer join t_Personnel u on r.EnteredByID = u.PersonnelID where r.statusid in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'workorderstatus' and comboitem not in ('complete','cancelled')) order by r.RequestID desc")
                End If
            Case "requestid"
                If filter.Text <> "" Then
                    Run_Query("Select r.RequestID, r.EntryDate, p.FirstName + ' ' + p.LastName as AssignedTo, rl.ComboItem as GeneratedBy, Case when r.KeyField = 'RoomID' then room.RoomNumber else pm.Name end as Location, r.Subject, u.UserName, s.comboitem as Status from t_Request r left outer join t_Comboitems s on s.comboitemid = r.statusid left outer join t_PMBuilding pm on pm.PMBuildingID = r.KeyValue left outer join t_Room room on room.roomid = r.KeyValue left outer join t_Personnel p on r.AssignedToID = p.PersonnelID left outer join t_CombOItems rl on r.RequestAreaID = rl.ComboItemID left outer join t_Personnel u on r.EnteredByID = u.PersonnelID where r.requestid like '%" & filter.Text & "%'")
                Else
                    Run_Query("Select Top 1000 r.RequestID, r.EntryDate, p.FirstName + ' ' + p.LastName as AssignedTo, rl.ComboItem as GeneratedBy, Case when r.KeyField = 'RoomID' then room.RoomNumber else pm.Name end as Location, r.Subject, u.UserName, s.comboitem as Status from t_Request r left outer join t_Comboitems s on s.comboitemid = r.statusid left outer join t_PMBuilding pm on pm.PMBuildingID = r.KeyValue left outer join t_Room room on room.roomid = r.KeyValue left outer join t_Personnel p on r.AssignedToID = p.PersonnelID left outer join t_CombOItems rl on r.RequestAreaID = rl.ComboItemID left outer join t_Personnel u on r.EnteredByID = u.PersonnelID where r.statusid in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'workorderstatus' and comboitem not in ('complete','cancelled')) order by r.RequestID desc")
                End If
            Case "assigned to"
                If filter.Text <> "" Then
                    If InStr(filter.Text, ",") > 0 Then
                        Dim sName(2) As String
                        sName = filter.Text.Split(",")
                        Run_Query("Select r.RequestID, r.EntryDate, p.Firstname + ' ' + p.Lastname as AssignedTo, rl.ComboItem as GeneratedBy, Case when r.KeyField = 'RoomID' then room.RoomNumber else pm.Name end as Location, r.Subject, u.UserName, s.comboitem as Status from t_Request r left outer join t_Comboitems s on s.comboitemid = r.statusid left outer join t_PMBuilding pm on pm.PMBuildingID = r.KeyValue left join t_Room room on room.roomid = r.roomid left outer join t_Personnel p on r.AssignedToID = p.PersonnelID left outer join t_CombOItems rl on r.RequestAreaID = rl.ComboItemID left outer join t_Personnel u on r.EnteredByID = u.PersonnelID where r.assignedtoID in (select personnelID from t_personnel where lastname like '%" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and firstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%') order by r.RequestID desc")
                    Else
                        Run_Query("Select r.RequestID, r.EntryDate, p.Firstname + ' ' + p.Lastname as AssignedTo, rl.ComboItem as GeneratedBy, Case when r.KeyField = 'RoomID' then room.RoomNumber else pm.Name end as Location, r.Subject, u.UserName, s.comboitem as Status from t_Request r left outer join t_Comboitems s on s.comboitemid = r.statusid left outer join t_PMBuilding pm on pm.PMBuildingID = r.KeyValue left join t_Room room on room.roomid = r.roomid left outer join t_Personnel p on r.AssignedToID = p.PersonnelID left outer join t_CombOItems rl on r.RequestAreaID = rl.ComboItemID left outer join t_Personnel u on r.EnteredByID = u.PersonnelID where r.assignedtoID in (select personnelID from t_personnel where lastname like '%" & filter.Text & "%') order by r.RequestID desc")
                    End If
                Else
                    Run_Query("select Top 1000 r.RequestID, r.EntryDate, p.FirstName + ' ' + p.LastName as AssignedTo, ri.comboitem as Location, Case when r.KeyField = 'RoomID' then room.RoomNumber else pm.Name end as Location, r.Subject, u.UserName, s.comboitem as Status from t_Request r left outer join t_Comboitems s on s.comboitemid = r.statusid left outer join t_PMBuilding pm on pm.PMBuildingID = r.KeyValue left outer join t_Room room on room.roomid = r.KeyValue left outer join t_Personnel p on r.AssignedToID = p.PersonnelID left outer join t_comboitems ri on r.requestAreaID = ri.comboitemid left outer join t_personnel u on r.EnteredByID = u.PersonnelID where r.statusid in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'workorderstatus' and comboitem not in ('complete','cancelled')) order by r.RequestID desc")
                End If
            Case "subject"
                If filter.Text <> "" Then
                    Run_Query("Select r.RequestID, r.EntryDate, p.FirstName + ' ' + p.LastName as AssignedTo, rl.ComboItem as GeneratedBy, Case when r.KeyField = 'RoomID' then room.RoomNumber else pm.Name end as Location, r.Subject, u.UserName, s.comboitem as Status from t_Request r left outer join t_Comboitems s on s.comboitemid = r.statusid left outer join t_PMBuilding pm on pm.PMBuildingID = r.KeyValue left outer join t_Room room on room.roomid = r.KeyValue left outer join t_Personnel p on r.AssignedToID = p.PersonnelID left outer join t_CombOItems rl on r.RequestAreaID = rl.ComboItemID left outer join t_Personnel u on r.EnteredByID = u.PersonnelID where r.subject like '%" & filter.Text & "%'")
                Else
                    Run_Query("Select Top 1000 r.RequestID, r.EntryDate, p.FirstName + ' ' + p.LastName as AssignedTo, rl.ComboItem as GeneratedBy, Case when r.KeyField = 'RoomID' then room.RoomNumber else pm.Name end as Location, r.Subject, u.UserName, s.comboitem as Status from t_Request r left outer join t_Comboitems s on s.comboitemid = r.statusid left outer join t_PMBuilding pm on pm.PMBuildingID = r.KeyValue left outer join t_Room room on room.roomid = r.KeyValue left outer join t_Personnel p on r.AssignedToID = p.PersonnelID left outer join t_CombOItems rl on r.RequestAreaID = rl.ComboItemID left outer join t_Personnel u on r.EnteredByID = u.PersonnelID where r.statusid in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'workorderstatus' and comboitem not in ('complete','cancelled')) order by r.RequestID desc")
                End If
            Case Else
                lblErr.Text = filter.Text & " " & ddFilter.Text
        End Select
    End Sub

    Sub Run_Query(ByVal sSQL As String)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim dr As SqlDataReader

        Try
            cn.Open()
            cm.CommandText = sSQL
            dr = cm.ExecuteReader
            gridview1.datasource = dr
            Dim ka(0) As String
            ka(0) = "requestid"
            gridview1.datakeynames = ka
            gridview1.databind()
            cn.Close()
        Catch ex As Exception
            lblErr.text = ex.Message
        Finally
            If cn.State <> Data.ConnectionState.Closed Then
                cn = Nothing
                cm = Nothing
                dr = Nothing
            End If
        End Try
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Response.Redirect("editRequest.aspx?requestid=" & gridview1.selectedValue)
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("editrequest.aspx?requestid=0")
    End Sub

    Protected Sub ddFilter_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddFilter.SelectedIndexChanged

    End Sub
End Class
