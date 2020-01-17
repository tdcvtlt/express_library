
Partial Class marketing_ResWaitList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddBD.Items.Add("ALL")
            For i = 1 To 4
                ddBD.Items.Add(i)
            Next
            ddUnitType.Items.Add("ALL")
            Dim oComboItems As New clsComboItems
            ddUnitType.DataSource = oComboItems.Load_ComboItems("UnitType")
            ddUnittype.DataValueField = "ComboitemID"
            ddUnitType.DatatextField = "ComboItem"
            ddUnitType.AppendDataBoundItems = True
            ddUnitType.DataBind()
            oComboItems = Nothing
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim unitType As String = ""
        Dim BD As String = ""
        If ddUnitType.SelectedValue = "ALL" Then
            For i = 1 To ddUnitType.Items.Count() - 1
                If unitType = "" Then
                    unitType = "'" & ddUnitType.Items(i).Value & "'"
                Else
                    unitType = unitType & ",'" & ddUnitType.Items(i).Value & "'"
                End If
            Next
        Else
            unitType = "'" & ddUnitType.SelectedValue & "'"
        End If
        If ddBD.SelectedValue = "ALL" Then
            For i = 1 To ddBD.Items.Count() - 1
                If BD = "" Then
                    BD = "'" & ddBD.Items(i).Value & "'"
                Else
                    BD = BD & ",'" & ddBD.Items(i).Value & "'"
                End If
            Next
        Else
            BD = "'" & ddBD.SelectedValue & "'"
        End If
        Dim oWaitList As New clsReservationWaitlist
        gvWaitList.DataSource = oWaitList.Get_Waitlist(dteSDate.Selected_Date, dteEDate.Selected_Date, unitType, BD)
        Dim sKeys(0) As String
        sKeys(0) = "WaitlistID"
        gvWaitList.Datakeynames = sKeys
        gvWaitList.DataBind()
        lblErr.Text = oWaitList.Err
    End Sub

    Protected Sub gvWaitlist_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(1).Text = CDate(e.Row.Cells(1).Text).ToShortDateString
                e.Row.Cells(4).Text = CDate(e.Row.Cells(4).Text).ToShortDateString
                e.Row.Cells(5).Text = CDate(e.Row.Cells(5).Text).ToShortDateString
            End If
            e.Row.Cells(0).Visible = False
        End If
    End Sub

    Protected Sub gvWaitlist_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvWaitlist.RowCommand
        Dim ID As Integer = 0
        ID = Convert.ToInt32(gvWaitlist.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("RemoveRes") = 0 Then
            Dim oWaitlist As New clsReservationWaitlist
            oWaitlist.WaitListID = ID
            oWaitlist.Load()
            oWaitlist.Active = False
            oWaitlist.Save()
            Dim unitType As String = ""
            Dim BD As String = ""
            If ddUnitType.SelectedValue = "ALL" Then
                For i = 1 To ddUnitType.Items.Count() - 1
                    If unitType = "" Then
                        unitType = "'" & ddUnitType.Items(i).Value & "'"
                    Else
                        unitType = unitType & ",'" & ddUnitType.Items(i).Value & "'"
                    End If
                Next
            Else
                unitType = "'" & ddUnitType.SelectedValue & "'"
            End If
            If ddBD.SelectedValue = "ALL" Then
                For i = 1 To ddBD.Items.Count() - 1
                    If BD = "" Then
                        BD = "'" & ddBD.Items(i).Value & "'"
                    Else
                        BD = BD & ",'" & ddBD.Items(i).Value & "'"
                    End If
                Next
            Else
                BD = "'" & ddBD.SelectedValue & "'"
            End If
            gvWaitList.DataSource = oWaitlist.Get_Waitlist(dteSDate.Selected_Date, dteEDate.Selected_Date, unitType, BD)
            Dim sKeys(0) As String
            sKeys(0) = "WaitlistID"
            gvWaitList.Datakeynames = sKeys
            gvWaitList.DataBind()
            oWaitlist = Nothing
        End If
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/AddWaitList.aspx','win01',690,450);", True)
    End Sub
End Class
