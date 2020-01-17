
Partial Class PropertyManagement_OutofService
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRooms As New clsRooms
            ddRooms.DataSource = oRooms.List_Rooms()
            ddRooms.DataTextField = "RoomNumber"
            ddRooms.DataValueField = "RoomID"
            ddRooms.DataBind()
            oRooms = Nothing
            siReason.Connection_String = Resources.Resource.cns
            siReason.Label_Caption = ""
            siReason.ComboItem = "RoomOfflineReason"
            siReason.Load_Items()
        End If
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lbRooms.Items.Add(New ListItem(ddRooms.SelectedItem.Text, ddRooms.SelectedValue))
        ddRooms.Items.Remove(ddRooms.SelectedItem)
        ddRooms.databind()
    End Sub

    Protected Sub Unnamed2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If lbRooms.SelectedValue <> "" Then
            ddRooms.Items.Add(New ListItem(lbRooms.SelectedItem.Text, lbRooms.SelectedValue))
            lbRooms.Items.Remove(lbRooms.SelectedItem)
            ddRooms.DataBind()
        End If
    End Sub

    Protected Sub Unnamed3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oRooms As New clsRooms
        Dim oNotes As New clsNotes
        Dim iServiceErr As String = ""
        Dim oServiceErr As String = ""
        Dim bProceed As Boolean = True
        Dim i As Integer
        oRooms.UserID = Session("UserDBID")
        If rbService.Items(0).Selected Then
            If CheckSecurity("Rooms", "OutOfService", , , Session("UserDBID")) Then
                If siReason.Selected_ID > 0 Then
                    For i = 0 To lbRooms.Items.Count - 1
                        If oRooms.Validate_Out_Of_Service(lbRooms.Items(i).Value, dteStartDate.Selected_Date, dteEndDate.Selected_Date) Then
                            If oRooms.Out_Of_Service(lbRooms.Items(i).Value, dteStartDate.Selected_Date, dteEndDate.Selected_Date, "Out", Trim(txtNote.Text), siReason.Selected_ID) Then
                                If iServiceErr = "" Then
                                    iServiceErr = "The Following Rooms Have Been Taken Out Of Service:"
                                End If
                                iServiceErr = iServiceErr & "<br>" & lbRooms.Items(i).Text
                                If txtNote.Text <> "" Or txtNote.Text = "" Then
                                    Dim oCombo As New clsComboItems
                                    oNotes.NoteID = 0
                                    oNotes.Load()
                                    oNotes.Note = "Taken offline from " & dteStartDate.Selected_Date & " to " & dteEndDate.Selected_Date & ". Reason: " & oCombo.Lookup_ComboItem(siReason.Selected_ID) & "." & Trim(txtNote.Text)
                                    oNotes.KeyField = "RoomID"
                                    oNotes.KeyValue = lbRooms.Items(i).Value
                                    oNotes.UserID = Session("UserDBID")
                                    oNotes.DateCreated = System.DateTime.Now.ToShortDateString
                                    oNotes.Save()
                                    oCombo = Nothing
                                End If
                            Else
                                lblErr.Text = oRooms.Err
                                Exit For
                            End If
                        Else
                            If oServiceErr = "" Then
                                oServiceErr = "The Following Reservation(s) Must Be Removed Before Taking The Following Room(s) Out Of Service:"
                            End If
                            oServiceErr = oServiceErr & "<br><b><u>" & lbRooms.Items(i).Text & "</b></u><br>" & oRooms.Err
                        End If
                    Next
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Select a Reason Why Room(s) Are Being Taken Out of Service.")
                End If
            Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Do Not Have Permission to Place Rooms Out Of Service.');", True)
            End If
        ElseIf rbService.Items(1).Selected Then
            If CheckSecurity("Rooms", "IntoService", , , Session("UserDBID")) Then
                For i = 0 To lbRooms.Items.Count - 1
                    If oRooms.Out_Of_Service(lbRooms.Items(i).Value, dteStartDate.Selected_Date, dteEndDate.Selected_Date, "in", Trim(txtNote.Text), siReason.Selected_ID) Then
                        If iServiceErr = "" Then
                            iServiceErr = "The Following Rooms Have Been Put Back Into Service:"
                        End If
                        iServiceErr = iServiceErr & "<br>" & lbRooms.Items(i).Text
                        If txtNote.Text <> "" Or txtNote.Text = "" Then
                            oNotes.NoteID = 0
                            oNotes.Load()
                            oNotes.Note = "Placed online for " & dteStartDate.Selected_Date & " to " & dteEndDate.Selected_Date & ". Reason: " & IIf(Trim(txtNote.Text) = "", "None Given", Trim(txtNote.Text))
                            oNotes.KeyField = "RoomID"
                            oNotes.KeyValue = lbRooms.Items(i).Value
                            oNotes.UserID = Session("UserDBID")
                            oNotes.DateCreated = System.DateTime.Now.ToShortDateString
                            oNotes.Save()
                        End If
                    Else
                        lblErr.Text = oRooms.Err
                        Exit For
                    End If
                Next
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Do Not Have Permission to Place Rooms Into Service.');", True)
            End If
        End If
        oNotes = Nothing
        oRooms = Nothing
        lblMsg.Text = iServiceErr & "<br><br>" & oServiceErr
        If lblErr.Text <> "" Then lblErr.Text = "Error: " & lblErr.Text

    End Sub
End Class
