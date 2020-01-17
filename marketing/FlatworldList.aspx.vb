Imports System
Imports System.Data
Imports System.Data.SqlClient


Partial Class marketing_FlatworldList
    Inherits System.Web.UI.Page

    Delegate Function GetTable(sqlText As String) As DataTable
    Delegate Sub Output(s As String)

    Private OutputToClient As Output   
    Private BookingStatuses As DataTable

    Private Function GetData(sqlText As String) As DataTable
        Dim dt = New DataTable()
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter(sqlText, cn)
                Try
                    ad.Fill(dt)
                Catch ex As Exception
                    OutputToClient(ex.Message)
                End Try
            End Using
        End Using
        Return dt
    End Function

    Private Function UpdateData(sqlText As String) As Integer
        Dim recordsAffected = 0

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand(sqlText, cn)
                Try
                    cn.Open()
                    recordsAffected = cm.ExecuteNonQuery()
                Catch ex As Exception
                    OutputToClient(ex.Message)
                Finally
                    cm.Dispose()
                    cn.Close()
                    cn.Dispose()
                End Try
            End Using
        End Using
        Return recordsAffected
    End Function

    Private Sub GetData()

        FlatWorldList.DataSource = Nothing
        FlatWorldList.DataBind()

        Dim gt As GetTable = AddressOf GetData

        BookingStatuses = gt("select * from t_comboitems where ComboID in (select ComboID from t_Combos where ComboName in ('bookingStatus'))  " & _
                            "and comboitem in ('In-complete', 'Updated', 'Cancelled') " & _
                            "and active = 1 order by ComboItem")

        Dim dt = gt(String.Format("select reservationid, bookingStatusID, " & _
                    "(select top 1 note from t_note where keyfield = 'RESERVATIONBOOKINGSTATUSID' and keyvalue = r.ReservationID order by noteid desc) [note], " & _
                    "(select firstname + ' ' + lastname from t_Prospect where ProspectID = r.ProspectID) [Prospect Name], " & _
                    "(select comboitem from t_comboitems where comboitemid = r.ResLocationID) [Reservation Location] " & _
                    "from t_reservations r  " & _
                    "left join t_comboItems bs on bs.comboItemId = r.bookingStatusId " & _
                    "where bs.comboItem in ('In-complete') order by [Prospect Name] "))

        FlatWorldList.DataSource = dt
        FlatWorldList.DataBind()
    End Sub

    Private Sub WriteMessage(s As String)
        Response.Write(String.Format("<br/>It is {0}.<br/><strong>{1}</strong>", DateTime.Now.ToString("T"), s))
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        OutputToClient = AddressOf WriteMessage

        If IsPostBack = False Then
            GetData()
        End If
    End Sub

    Protected Sub FlatWorldList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles FlatWorldList.RowCommand
        Dim gvr As GridViewRow = CType(CType(e.CommandSource, Button).NamingContainer, GridViewRow)
        Dim gv As GridView = CType(gvr.NamingContainer, GridView)
        Dim b As Button = CType(e.CommandSource, Button)
        Dim ddl As DropDownList = CType(gvr.FindControl("status"), DropDownList)


        Dim id_reservation As Integer = gv.DataKeys(gvr.RowIndex).Value
        Dim id_booking_status_changed As String = ddl.SelectedItem.Value

        If ddl.SelectedItem.Text.Equals("In-complete") Then Return
        If e.CommandName.Equals("submit") Then

            'when list item is blank
            '
            If id_booking_status_changed = "-1" Then
                Dim rc = UpdateData(String.Format("update t_reservations set bookingstatusid=null, bookingStatusDate=getdate() where reservationid={0}", _
                                     id_reservation))

            Else
                Dim rc = UpdateData(String.Format("update t_reservations set bookingstatusid={1}, bookingStatusDate=getdate() where reservationid={0}", _
                                   id_reservation, id_booking_status_changed))
            End If

            
            b.Enabled = False
            ddl.Enabled = False
        End If
    End Sub

    Protected Sub FlatWorldList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles FlatWorldList.RowDataBound
        Dim gvr As GridViewRow = e.Row
        Dim gv As GridView = CType(gvr.NamingContainer, GridView)
        Dim ddl As DropDownList = CType(gvr.FindControl("status"), DropDownList)
        Dim hlk As HyperLink = CType(gvr.FindControl("link"), HyperLink)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dt = CType(gv.DataSource, DataTable)
            dt.PrimaryKey = New DataColumn() {dt.Columns(0)}

            Dim dr As DataRow = dt.Rows.Find(gv.DataKeys(e.Row.RowIndex).Value)

            With ddl
                .AppendDataBoundItems = True
                .Items.Add(New ListItem("", "-1"))
                .DataSource = BookingStatuses
                .DataBind()
            End With           

            hlk.NavigateUrl = String.Format("EditReservation.aspx?ReservationID={0}", gv.DataKeys(e.Row.RowIndex).Value)
            ddl.SelectedValue = dr("bookingStatusID").ToString()
        End If

    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As System.EventArgs) Handles btnRefresh.Click
        GetData()
    End Sub
End Class
