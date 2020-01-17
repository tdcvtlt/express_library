
Partial Class PropertyManagement_ResCheckins
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim oRes As New clsReservationCheckIns
        If oRes.Fill_Table(dteSDate.Selected_Date, dteEDate.Selected_Date) Then
            Label1.Text = "Success"
        Else
            Label1.Text = "Failure " & oRes.Err
        End If

        gvCheckIns.DataSource = oRes.Get_Reservations(dteSDate.Selected_Date, dteEDate.Selected_Date)
        gvCheckIns.DataBind()
        Label1.Text = oRes.Err
        oRes = Nothing
    End Sub

    Protected Sub gvCheckIns_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                Dim oResCheckIn As New clsReservationCheckIns
                Dim oCombo As New clsComboItems
                oResCheckIn.ID = e.Row.Cells(1).Text
                oResCheckIn.Load()
                e.Row.Cells(3).Text = CDate(e.Row.Cells(3).Text).ToShortDateString
                e.Row.Cells(4).Text = CDate(e.Row.Cells(4).Text).ToShortDateString
                Dim dd As DropDownList = e.Row.Cells(9).FindControl("ddStatus")
                dd.DataSource = oCombo.Load_ComboItems("ResCheckInReportStatus")
                dd.DataTextField = "ComboItem"
                dd.DataValueField = "ComboItemID"
                dd.DataBind()
                dd.SelectedValue = oResCheckIn.StatusID
                Dim tb As TextBox = e.Row.Cells(10).FindControl("dteFollow")
                tb.Text = oResCheckIn.FollowUpDate
                oCombo = Nothing
                oResCheckIn = Nothing
            End If
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub gvCheckIns_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvCheckIns.RowCommand
        Dim row As GridViewRow = gvCheckIns.Rows(Convert.ToInt32(e.CommandArgument))
        If e.CommandName.CompareTo("Complete") = 0 Then
            Dim dd As DropDownList = row.Cells(9).FindControl("ddStatus")
            Dim bProceed As Boolean = True
            Dim sErr As String = ""
            If dd.SelectedItem.Text = "Complete" Then
                Dim oNotes As New clsNotes
                oNotes.KeyField = "ReservationCheckIn"
                oNotes.KeyValue = row.Cells(1).Text
                If oNotes.Get_Note_Count() = 0 Then
                    sErr = "Please Enter A Note Before Marking Complete."
                    bProceed = False
                End If
                oNotes = Nothing
            End If
            Dim tb As TextBox = row.Cells(10).FindControl("dteFollow")
            If dd.SelectedItem.Text = "Follow Up" Then
                If tb.Text = "" Then
                    sErr = "Please Enter a Follow Up Date."
                    bProceed = False
                End If
            End If
            If bProceed Then
                Dim oResCheckIn As New clsReservationCheckIns
                oResCheckIn.ID = row.Cells(1).Text
                oResCheckIn.Load()
                oResCheckIn.StatusID = dd.SelectedValue
                If tb.Text <> "" Then
                    oResCheckIn.FollowUpDate = tb.Text
                End If
                oResCheckIn.StatusDate = System.DateTime.Now
                oResCheckIn.UserID = Session("UserDBID")
                oResCheckIn.Save()
                gvCheckIns.DataSource = oResCheckIn.Get_Reservations(dteSDate.Selected_Date, dteEDate.Selected_Date)
                gvCheckIns.DataBind()
                oResCheckIn = Nothing
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
            End If
        ElseIf e.CommandName.CompareTo("AddNote") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/general/editNote.aspx?KeyField=ReservationCheckIn&KeyValue=" & row.Cells(1).Text & "','win01',300,400);", True)
        ElseIf e.CommandName.CompareTo("ViewNote") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/general/ViewNotes.aspx?KeyField=ReservationCheckIn&KeyValue=" & row.Cells(1).Text & "','win01',600,400);", True)
        End If
    End Sub

End Class
