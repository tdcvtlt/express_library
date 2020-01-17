Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls
Partial Class marketing_roomswap
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRes As New clsReservations
            Dim oCombo As New clsComboItems
            Dim rStatus As String = ""
            oRes.ReservationID = Request("ReservationID")
            oRes.Load()
            rStatus = oCombo.Lookup_ComboItem(oRes.StatusID)

            If rStatus = "Booked" And DateTime.Compare(System.DateTime.Now, CDate(oRes.CheckInDate)) < 0 Then
                If CheckSecurity("Reservations", "SwapRoomBooked", , , Session("UserDBID")) Then
                    MultiView1.ActiveViewIndex = 0
                Else
                    MultiView1.ActiveViewIndex = 3
                End If
            Else
                If CheckSecurity("Reservations", "SwapRoom", , , Session("UserDBID")) Then
                    If DateTime.Compare(System.DateTime.Now, CDate(oRes.CheckInDate)) = 0 Then
                        ddInDate.Items.Add(System.DateTime.Now.ToShortDateString)
                    Else
                        ddInDate.Items.Add(System.DateTime.Now.ToShortDateString)
                        ddInDate.Items.Add(System.DateTime.Now.AddDays(-1).ToShortDateString)
                    End If
                    MultiView1.ActiveViewIndex = 1
                Else
                    MultiView1.ActiveViewIndex = 3
                End If
            End If
            oRes = Nothing
            oCombo = Nothing
        End If
    End Sub
    Protected Sub gvRooms_OnRowCreated(ByVal sender As Object, ByVal e As Web.UI.WebControls.GridViewRowEventArgs) Handles gvRooms.RowCreated
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub
    Protected Sub gvSpares_OnRowCreated(ByVal sender As Object, ByVal e As Web.UI.WebControls.GridViewRowEventArgs) Handles gvSpares.RowCreated
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oRes As New clsReservations
        Dim oRoom As New clsRooms
        Dim oCombo As New clsComboItems
        Dim inDate As Date
        If reasonTxt.Text = "" Then
            lblErr.Text = "Please Fill in a Reason For Swapping Rooms."
        Else
            oRes.ReservationID = Request("ReservationID")
            oRes.Load()
            If oCombo.Lookup_ComboItem(oRes.StatusID) = "Booked" Then
                inDate = CDate(oRes.CheckInDate).ToShortDateString
            Else
                inDate = System.DateTime.Now.ToShortDateString
            End If
            gvRooms.DataSource = oRoom.search_Swappables(Request("ReservationID"), Request("RoomID"), inDate, oRes.CheckOutDate, oCombo.Lookup_ComboItem(oRes.StatusID))
            gvRooms.Databind()
            lblErr.Text = oRoom.Err & Request("ReservationID") & " " & Request("RoomID") & " " & CDate(System.DateTime.Now.ToShortDateString) & " " & oRes.CheckOutDate & " " & oCombo.Lookup_ComboItem(oRes.StatusID)
        End If
        oRes = Nothing
        oCombo = Nothing
        oRoom = Nothing
    End Sub

    Protected Sub Unnamed2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oRes As New clsReservations
        Dim oRoom As New clsRooms
        Dim oCombo As New clsComboItems
        If reasonIHTxt.Text = "" Then
            lblErr.Text = "Please Fill in a Reason For Swapping Rooms."
        Else
            oRes.ReservationID = Request("ReservationID")
            oRes.Load()
            gvRooms.DataSource = oRoom.search_Swappables(Request("ReservationID"), Request("RoomID"), ddInDate.selectedvalue, oRes.CheckOutDate, oCombo.Lookup_ComboItem(oRes.StatusID))
            gvRooms.Databind()
            If CheckSecurity("Reservations", "MoveToSpare", , , Session("UserDBID")) Then
                lblSpares.visible = True
                gvSpares.DataSource = oRoom.search_Swappable_Spares(Request("RoomID"), ddInDate.selectedvalue, oRes.CheckOutDate)
                gvSpares.DataBind()
            End If
            lblErr.Text = oRoom.Err
        End If
        oRes = Nothing
        oCombo = Nothing
        oRoom = Nothing
    End Sub

    Protected Sub gvRooms_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvRooms.SelectedIndexChanged
        Dim row As GridViewRow = gvRooms.selectedRow
        Dim oRes As New clsReservations
        Dim oCombo As New clsComboItems
        Dim inDate As Date
        Dim outDate As Date
        Dim reason As String = ""
        oRes.ReservationID = Request("ReservationID")
        oRes.Load()
        oRes.UserID = Session("UserDBID")
        outDate = CDate(oRes.CheckOutDate)
        If oCombo.Lookup_ComboItem(oRes.StatusID) = "Booked" Then
            inDate = CDate(oRes.CheckInDate)
            'inDate = CDate(System.DateTime.Now.ToShortDateString)
            reason = reasonTxt.Text
        Else
            inDate = CDate(ddInDate.SelectedValue)
            reason = reasonIHTxt.Text
        End If

        If oRes.check_Swappable(CInt(row.Cells(1).Text), inDate, outDate.AddDays(-1)) Then
            'Multi -BD usage check
            Dim usageBD As Integer = 0
            Dim oUsage As New clsUsage

            If oRes.CheckInDate < Date.Today Then
                usageBD = oUsage.Usage_BD_Count(CInt(Request("RoomID")), CDate(Date.Today), CDate(oRes.CheckOutDate).AddDays(-1))
            Else
                usageBD = oUsage.Usage_BD_Count(CInt(Request("RoomID")), CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate).AddDays(-1))
            End If

            If usageBD > CInt(row.Cells(3).Text.Substring(0, 1)) Then

                lblUsageReason.Text = reasonIHTxt.Text & reasonTxt.Text
                lblUsageMessage.Text = row.Cells(2).Text
                lblUsageRoomID.Text = row.Cells(1).Text

                lblRoomID.Text = row.Cells(1).Text
                gvRooms.Visible = False

                MultiView1.ActiveViewIndex = 4
            Else
                If oRes.swap_Room(Request("RoomID"), CInt(row.Cells(1).Text), Request("ReservationID"), inDate, outDate.AddDays(-1)) Then
                    Dim oNotes As New clsNotes
                    oNotes.NoteID = 0
                    oNotes.Load()
                    oNotes.KeyField = "ReservationID"
                    oNotes.KeyValue = oRes.ReservationID
                    oNotes.UserID = Session("UserDBID")
                    oNotes.Note = reason
                    oNotes.Save()
                    oNotes = Nothing
                    'Create Event For Adding/Removing Room
                    Dim oEvent As New clsEvents
                    Dim oRoom As New clsRooms
                    oEvent.KeyField = "ReservationID"
                    oEvent.KeyValue = Request("ReservationID")
                    oEvent.EventType = "Move"
                    oRoom.RoomID = Request("RoomID")
                    oRoom.Load()
                    oEvent.OldValue = oRoom.RoomNumber
                    oRoom.RoomID = CInt(row.Cells(1).Text)
                    oRoom.Load()
                    oEvent.NewValue = oRoom.RoomNumber
                    oEvent.CreatedByID = Session("UserDBID") ' _UserID
                    oEvent.FieldName = "Room"
                    oEvent.Create_Event()
                    oRoom = Nothing
                    oEvent = Nothing
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Rooms();window.close();", True)
                Else
                    lblErr.Text = oRes.Err
                End If
            End If

        Else
            lblReason.Text = reasonIHTxt.Text & reasonTxt.Text
            lblRoomNumber.Text = row.Cells(2).Text
            lblRoomID.Text = row.Cells(1).Text
            gvRooms.Visible = False
            MultiView1.ActiveViewIndex = 2
        End If


        oRes = Nothing
        oCombo = Nothing
    End Sub

    Protected Sub yesSwapBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yesSwapBtn.Click

        'Dim row As GridViewRow = gvRooms.SelectedRow
        'Dim usageBD As Integer = 0
        'Dim oUsage As New clsUsage
        'Dim oRes As New clsReservations
        'oRes.ReservationID = Request("ReservationID")
        'oRes.Load()
        'If oRes.CheckInDate < Date.Today Then
        '    usageBD = oUsage.Usage_BD_Count(CInt(Request("RoomID")), CDate(Date.Today), CDate(oRes.CheckOutDate).AddDays(-1))
        'Else
        '    usageBD = oUsage.Usage_BD_Count(CInt(Request("RoomID")), CDate(oRes.CheckInDate), CDate(oRes.CheckOutDate).AddDays(-1))
        'End If


        'If usageBD > CInt(row.Cells(3).Text.Substring(0, 1)) Then

        '    lblUsageReason.Text = reasonIHTxt.Text & reasonTxt.Text
        '    lblUsageMessage.Text = row.Cells(2).Text
        '    lblUsageRoomID.Text = row.Cells(1).Text
        '    lblRoomID.Text = row.Cells(1).Text
        '    gvRooms.Visible = False

        '    MultiView1.ActiveViewIndex = 4
        'Else
        '    Swap_Room()
        'End If
        Swap_Room(lblReason.Text)
    End Sub

    Private Sub Swap_Room(ByVal note As String)
        Dim row As GridViewRow = gvRooms.SelectedRow
        Dim oRes As New clsReservations
        Dim oCombo As New clsComboItems
        Dim inDate As Date
        Dim outDate As Date
        oRes.ReservationID = Request("ReservationID")
        oRes.Load()
        oRes.UserID = Session("UserDBID")
        outDate = CDate(oRes.CheckOutDate)
        If oCombo.Lookup_ComboItem(oRes.StatusID) = "Booked" Then
            inDate = CDate(System.DateTime.Now.ToShortDateString)
        Else
            inDate = CDate(ddInDate.SelectedValue)
        End If



        If oRes.swap_Room(Request("RoomID"), CInt(lblRoomID.Text), Request("ReservationID"), inDate, outDate.AddDays(-1)) Then
            Dim oNotes As New clsNotes
            oNotes.NoteID = 0
            oNotes.Load()
            oNotes.KeyField = "ReservationID"
            oNotes.KeyValue = oRes.ReservationID
            oNotes.UserID = Session("UserDBID")
            oNotes.Note = note
            oNotes.Save()
            oNotes = Nothing
            'Create Event For Adding/Removing Room
            Dim oEvent As New clsEvents
            Dim oRoom As New clsRooms
            oEvent.KeyField = "ReservationID"
            oEvent.KeyValue = Request("ReservationID")
            oEvent.EventType = "Move"
            oRoom.RoomID = Request("RoomID")
            oRoom.Load()
            oEvent.OldValue = oRoom.RoomNumber
            oRoom.RoomID = CInt(lblRoomID.Text)
            oRoom.Load()
            oEvent.NewValue = oRoom.RoomNumber
            oEvent.CreatedByID = Session("UserDBID") '_UserID
            oEvent.FieldName = "Room"
            oEvent.Create_Event()
            oRoom = Nothing
            oEvent = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Rooms();window.close();", True)
        Else
            lblErr.Text = oRes.Err
        End If
        oCombo = Nothing
        oRes = Nothing
    End Sub
    Protected Sub noSwapBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles noSwapBtn.Click
        Dim oRes As New clsReservations
        Dim oCombo As New clsComboItems
        Dim rStatus As String = ""
        oRes.ReservationID = Request("ReservationID")
        oRes.Load()
        rStatus = oCombo.Lookup_ComboItem(oRes.StatusID)
        gvRooms.visible = True
        If rStatus = "Booked" And DateTime.Compare(System.DateTime.Now, CDate(oRes.CheckInDate)) < 0 Then
            MultiView1.ActiveViewIndex = 0
        Else
            MultiView1.ActiveViewIndex = 1
        End If
        oRes = Nothing
        oCombo = Nothing
    End Sub

    Protected Sub gvSpares_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSpares.SelectedIndexChanged
        Dim row As GridViewRow = gvSpares.selectedRow
        Dim oRes As New clsReservations
        Dim oCombo As New clsComboItems
        Dim inDate As Date
        Dim outDate As Date
        oRes.ReservationID = Request("ReservationID")
        oRes.Load()
        oRes.UserID = Session("UserDBID")
        outDate = CDate(oRes.CheckOutDate)
        inDate = CDate(ddInDate.SelectedValue)
        Try
            If oRes.move_To_Spare(Request("RoomID"), CInt(row.Cells(1).Text), Request("ReservationID"), inDate, outDate.AddDays(-1)) Then
                Dim oNotes As New clsNotes
                oNotes.NoteID = 0
                oNotes.Load()
                oNotes.KeyField = "ReservationID"
                oNotes.KeyValue = oRes.ReservationID
                oNotes.UserID = Session("UserDBID")
                oNotes.Note = reasonIHTxt.Text
                oNotes.Save()
                oNotes = Nothing
                'Create maintenance Ticket
                'Dim oRequest As New clsRequest
                'oRequest.RequestID = 0
                'oRequest.Load()
                'oRequest.EnteredByID = Session("UserDBID")
                'oRequest.EntryDate = System.DateTime.Now
                'oRequest.RoomID = Request("RoomID")
                'oRequest.StatusID = oCombo.Lookup_ID("WorkOrderStatus", "Not Started")
                'oRequest.RequestAreaID = oCombo.Lookup_ID("RequestArea", "Guest")
                'oRequest.Subject = "Guest Moved To Spare"
                'oRequest.Description = reasonIHTxt.Text
                'oRequest.Save()
                'oRequest = Nothing

                'Create Event For Adding/Removing Room
                Dim oEvent As New clsEvents
                Dim oRoom As New clsRooms
                oEvent.KeyField = "ReservationID"
                oEvent.KeyValue = Request("ReservationID")
                oEvent.EventType = "Move"
                oRoom.RoomID = Request("RoomID")
                oRoom.Load()
                oEvent.OldValue = oRoom.RoomNumber
                oRoom.RoomID = CInt(row.Cells(1).Text)
                oRoom.Load()
                oEvent.NewValue = oRoom.RoomNumber
                oEvent.CreatedByID = Session("UserDBID") '_UserID
                oEvent.FieldName = "Room"
                oEvent.Create_Event()
                oRoom = Nothing
                oEvent = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Rooms();window.close();", True)
            Else
                lblErr.Text = oRes.Err
            End If
        Catch ex As Exception
            lblErr.Text = ex.Message
        End Try
        oRes = Nothing
        oCombo = Nothing
    End Sub

    Protected Sub btnUsageYes_Click(sender As Object, e As System.EventArgs) Handles btnUsageYes.Click
        Call Swap_Room(lblUsageReason.Text)
      End Sub

    Protected Sub BtnUsageNo_Click(sender As Object, e As System.EventArgs) Handles BtnUsageNo.Click
        noSwapBtn_Click(Nothing, EventArgs.Empty)
    End Sub
End Class
