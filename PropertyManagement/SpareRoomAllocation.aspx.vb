
Partial Class PropertyManagement_SpareRoomAllocation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRooms As New clsRooms
            ddRooms.DataSource = oRooms.List_Rooms()
            ddRooms.DataTextField = "RoomNumber"
            ddRooms.DataValueField = "RoomID"
            ddRooms.DataBind()
            oRooms = Nothing
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
        If lbRooms.Items.Count = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Add At Least One Room to Allocate As Spare');", True)
        ElseIf dteStartDate.Selected_Date.ToString = "" Or dteEndDate.Selected_Date.ToString = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Select as Start/End Date.');", True)
        Else
            If CheckSecurity("Rooms", "AllocateSpare", , , Session("UserDBID")) Then
                Dim oRooms As New clsRooms
                Dim oCombo As New clsComboItems
                Dim resErr As String = "" '"The Following Rooms were Not Allocated as Spare Due to the Following Reservations:"
                Dim usageErr As String = ""
                Dim oServiceErr As String = ""
                Dim bProceed As Boolean = True
                Dim i As Integer
                For i = 0 To lbRooms.Items.Count - 1
                    bProceed = True
                    If oRooms.Validate_Spare_Allocation_Res(lbRooms.Items(i).Value, dteStartDate.Selected_Date, dteEndDate.Selected_Date) = False Then
                        bProceed = False
                        If resErr = "" Then
                            resErr = "The Following Rooms Were Not Allocated As Spare Due To The Following Reservations:"
                        End If
                        resErr = resErr & "<br><b><u>" & lbRooms.Items(i).Text & "</u></b><br>" & oRooms.Err
                    End If

                    If oRooms.Validate_Spare_Allocation_Usage(lbRooms.Items(i).Value, dteStartDate.Selected_Date, dteEndDate.Selected_Date) = False Then
                        bProceed = False
                        If usageErr = "" Then
                            usageErr = "The Following Rooms Were Not Allocated As Spare Due To The Following Usages:"
                        End If
                        usageErr = usageErr & "<br><b><u>" & lbRooms.Items(i).Text & "</u></b><br>" & oRooms.Err
                    End If

                    If oRooms.Validate_Spare_Allocation_Service(lbRooms.Items(i).Value, dteStartDate.Selected_Date, dteEndDate.Selected_Date) = False Then
                        bProceed = False
                        If oServiceErr = "" Then
                            oServiceErr = "The Following Rooms Were Not Allocated As Spare Because They Are Marked Out Of Service During the Date Range:"
                        End If
                        oServiceErr = oServiceErr & "<br><b>" & lbRooms.Items(i).Text & "</b>"
                    End If

                    If bProceed Then
                        Dim oRMatix As New clsRoomAllocationMatrix
                        Dim tempDate As Date = CDate(dteStartDate.Selected_Date)
                        Do While tempDate.CompareTo(CDate(dteEndDate.Selected_Date)) < 1
                            oRMatix.AllocationID = oRMatix.Get_Allocation_ID(tempDate, lbRooms.Items(i).Value)
                            oRMatix.Load()
                            oRMatix.UserID = Session("UserDBID")
                            oRMatix.TypeID = oCombo.Lookup_ID("ReservationType", "Spare")
                            oRMatix.Save()
                            tempDate = tempDate.AddDays(1)
                        Loop
                        'If oRooms.Allocate(lbRooms.Items(i).Value, oCombo.Lookup_ID("ReservationType", "Spare"), dteStartDate.Selected_Date, dteEndDate.Selected_Date) Then
                        'Else
                        '    lblErr.Text = oRooms.Err
                        '    Exit For
                        'End If
                        oRMatix = Nothing
                    End If
                Next
                lblMsg.Text = "Allocation Complete: <br>" & resErr & "<br><br>" & usageErr & "<br><br>" & oServiceErr
                If lblErr.Text <> "" Then lblErr.Text = "Error: " & lblErr.Text
                oCombo = Nothing
                oRooms = Nothing
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Do Not Have Permission to Allocate Rooms As Spare.');", True)
            End If
        End If
    End Sub
End Class
