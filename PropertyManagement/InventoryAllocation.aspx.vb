
Partial Class PropertyManagement_InventoryAllocation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRooms As New clsRooms
            ddRooms.DataSource = oRooms.List_Rooms()
            ddRooms.DataTextField = "RoomNumber"
            ddRooms.DataValueField = "RoomID"
            ddRooms.DataBind()

            siType.Connection_String = Resources.Resource.cns
            siType.ComboItem = "ReservationType"
            siType.Label_Caption = ""
            siType.Load_Items()
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
        If lbRooms.Items.Count = 0 Or lbDates.Items.Count = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter at Least One Room and One Date Range.');", True)
        Else
            Dim oRooms As New clsRooms
            Dim bProceed As Boolean = True
            Dim i As Integer
            Dim j As Integer
            Dim sDates(1) As String
            'Check Permission
            For i = 0 To lbRooms.Items.Count - 1
                For j = 0 To lbDates.Items.Count - 1
                    sDates = Split(lbDates.Items(j).Value, "|")
                    If oRooms.Validate_Type_Allocation(lbRooms.Items(i).Value, siType.Selected_ID, sDates(0), sDates(1), Session("UserDBID")) Then
                    Else
                        bProceed = False
                        Exit For
                    End If
                    ReDim sDates(1)
                Next
                If Not (bProceed) Then
                    Exit For
                End If
            Next

            If bProceed Then
                Dim oRMatix As New clsRoomAllocationMatrix
                For i = 0 To lbRooms.Items.Count - 1
                    For j = 0 To lbDates.Items.Count - 1
                        sDates = Split(lbDates.Items(j).Value, "|")
                        Dim tempDate As Date = CDate(sDates(0))
                        Do While tempDate.CompareTo(CDate(sDates(1))) < 1
                            oRMatix.AllocationID = oRMatix.Get_Allocation_ID(tempDate, lbRooms.Items(i).Value)
                            oRMatix.Load()
                            oRMatix.UserID = Session("UserDBID")
                            oRMatix.TypeID = siType.Selected_ID
                            oRMatix.Save()
                            tempDate = tempDate.AddDays(1)
                        Loop
                        ReDim sDates(1)
                        'If oRooms.Allocate(lbRooms.Items(i).Value, siType.Selected_ID, dteStartDate.Selected_Date, dteEndDate.Selected_Date) Then
                        'Else
                        '    bProceed = False
                        'End If
                    Next
                Next
                oRMatix = Nothing
                If bProceed Then
                    Response.Redirect("InventoryAllocation.aspx")
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Error Allocating');", True)
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Access Denied');", True)
            End If
            oRooms = Nothing
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If dteStartDate.Selected_Date = "" Or dteEndDate.Selected_Date = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Date Range.');", True)
        ElseIf DateTime.Compare(dteStartDate.Selected_Date, dteEndDate.Selected_Date) > 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Invalid Date Range');", True)
        Else
            lbDates.Items.Add(New ListItem(dteStartDate.Selected_Date & " - " & dteEndDate.Selected_Date, dteStartDate.Selected_Date & "|" & dteEndDate.Selected_Date))
        End If
    End Sub

    Protected Sub Unnamed5_Click(sender As Object, e As EventArgs)
        If lbDates.SelectedValue <> "" Then
            lbDates.Items.Remove(lbDates.SelectedItem)
        End If
    End Sub
End Class
