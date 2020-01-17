Imports System.Data
Imports System.Math
Partial Class marketing_editUsage
    Inherits System.Web.UI.Page

    Protected Sub LinkButton3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton3.Click
        If Request("UsageID") > 0 Then
            Dim oNotes As New clsNotes
            oNotes.KeyField = "UsageID"
            oNotes.KeyValue = Request("UsageID")
            gvNotes.DataSource = oNotes.List
            gvNotes.DataBind()
            'Notes1.KeyValue = Request("UsageID")
            'Notes1.Display()
            MultiView3.ActiveViewIndex = 2
            oNotes = Nothing
        End If
    End Sub

    Protected Sub LinkButton4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton4.Click
        If Request("UsageID") > 0 Then
            Dim oEvents As New clsEvents
            oEvents.KeyField = "UsageID"
            oEvents.KeyValue = Request("UsageID")
            gvEvents.DataSource = oEvents.List
            gvEvents.DataBind()
            MultiView3.ActiveViewIndex = 3
            oEvents = Nothing
        End If
    End Sub

    Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton2.Click
        If Request("UsageID") > 0 Then
            Dim oUsage As New clsUsage
            Try
                oUsage.UsageID = Request("UsageID")
                oUsage.Load()
                gvRooms.DataSource = oUsage.List_Rooms(Request("UsageID"), oUsage.OutDate)
                Dim sKeys(0) As String
                sKeys(0) = "RoomID"
                gvRooms.DataKeyNames = sKeys
                gvRooms.DataBind()
                MultiView3.ActiveViewIndex = 1
                oUsage = Nothing
            Catch ex As Exception
                lblErr.Text = ex.Message
            End Try
        End If
    End Sub

    Protected Sub gvAddRooms_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Rooms to Add" Then
            Dim aSplit() As String = Split(e.Row.Cells(2).Text, "|")
            For i = 0 To UBound(aSplit)
                If i > 0 Then
                    e.Row.Cells(2).Text = e.Row.Cells(2).Text & "<br>" & aSplit(i)
                Else
                    e.Row.Cells(2).Text = aSplit(i)
                End If
            Next i
            ReDim aSplit(0)
            aSplit = Split(e.Row.Cells(3).Text, "|")
            For i = 0 To UBound(aSplit)
                If i > 0 Then
                    e.Row.Cells(3).Text = e.Row.Cells(3).Text & "<br>" & aSplit(i)
                Else
                    e.Row.Cells(3).Text = aSplit(i)
                End If
            Next i
            ReDim aSplit(0)
            aSplit = Split(e.Row.Cells(4).Text, "|")
            For i = 0 To UBound(aSplit)
                If i > 0 Then
                    e.Row.Cells(4).Text = e.Row.Cells(4).Text & "<br>" & aSplit(i)
                Else
                    e.Row.Cells(4).Text = aSplit(i)
                End If
            Next i
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub gvRooms_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.Cells(3).Text = "NO" Then
                e.Row.Cells(4).Visible = False
            End If
            e.Row.Cells(3).Visible = False
        End If
    End Sub

    Protected Sub gvRooms_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvRooms.RowCommand
        Dim roomID As Integer
        Dim res As String = ""
        Dim inDate As Date
        roomID = Convert.ToInt32(gvRooms.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("RemoveRoom") = 0 Then
            Dim oUsage As New clsUsage
            Dim reservations As String = ""
            oUsage.UsageID = Request("UsageID")
            oUsage.Load()
            oUsage.UserID = Session("UserDBID")
            If Date.Compare(System.DateTime.Now, CDate(oUsage.OutDate)) >= 0 Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Can Not Remove A Room From A Past Date Usage.');", True)
            Else
                If Date.Compare(System.DateTime.Now, CDate(oUsage.InDate)) >= 0 Then
                    res = oUsage.List_Usage_Reservations(Request("UsageID"), roomID, True, System.DateTime.Now)
                Else
                    res = oUsage.List_Usage_Reservations(Request("UsageID"), roomID, False, oUsage.InDate)
                End If
                If res <> "" Then
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Remove The Room from the Following Reservation(s): " & res & "');", True)
                Else
                    If Date.Compare(System.DateTime.Now, CDate(oUsage.InDate)) <= 0 Then
                        inDate = oUsage.InDate
                    Else
                        inDate = CDate(System.DateTime.Now.ToShortDateString)
                    End If
                    If oUsage.Remove_Room(roomID, oUsage.UsageID, inDate, oUsage.OutDate) Then
                        gvRooms.DataSource = oUsage.List_Rooms(oUsage.UsageID, oUsage.OutDate)
                        Dim sKeys(0) As String
                        sKeys(0) = "RoomID"
                        gvRooms.DataKeyNames = sKeys
                        gvRooms.DataBind()
                    Else
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & oUsage.Err & "');", True)
                    End If
                End If
            End If
            oUsage = Nothing
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Load_Lookups()
            Load_Contracts()
            Set_Values()
            MultiView1.ActiveViewIndex = 0
            MultiView2.ActiveViewIndex = 0
            MultiView3.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub Load_Contracts()
        Dim oUsage As New clsUsage
        If Request("ContractID") <> "" Then
            ddContract.DataSource = oUsage.List_Contracts("Contract", Request("ContractID"))
        Else
            ddContract.DataSource = oUsage.List_Contracts("Usage", Request("UsageID"))
        End If
        ddContract.DataValueField = "ContractID"
        ddContract.DataTextField = "ContractNumber"
        ddContract.DataBind()
        oUsage = Nothing
    End Sub
    Protected Sub Set_Values()
        'ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('HERE1');", True)
        Dim oUsage As New clsUsage
        oUsage.UsageID = Request("UsageID")
        oUsage.Load()
        oUsage.UserID = Session("UserDBID")
        If Request("UsageID") = 0 Then
            ownerTxt.Text = oUsage.Get_Owner("contract", Request("ContractID"))
        Else
            ownerTxt.Text = oUsage.Get_Owner("usage", Request("UsageID"))
        End If
        dateCreatedTxt.Text = oUsage.DateCreated
        siType.Selected_ID = oUsage.TypeID
        siSubType.Selected_ID = oUsage.SubTypeID
        siStatus.Selected_ID = oUsage.StatusID
        amtPromisedTxt.Text = oUsage.AmountPromised
        siUnitType.Selected_ID = oUsage.UnitTypeID
        siRoomType.Selected_ID = oUsage.RoomTypeID
        siCategory.Selected_ID = oUsage.CategoryID
        pointsTxt.Text = oUsage.Points
        inDateTxt.Text = oUsage.InDate
        outDateTxt.Text = oUsage.OutDate
        Dim sYear As Integer
        If Request("UsageID") = 0 Then
            sYear = Year(System.DateTime.Now) - 3
        Else
            If oUsage.UsageYear < Year(System.DateTime.Now) Then
                sYear = oUsage.UsageYear - 3
            Else
                sYear = Year(System.DateTime.Now) - 3
            End If
        End If
        For i = 1 To 7
            ddDays.Items.Add(i)
        Next
        ddDays.SelectedValue = oUsage.Days
        For i = sYear To Year(System.DateTime.Now) + 3
            ddUsageYear.Items.Add(i)
        Next
        If Request("UsageID") = 0 Then
            ddDays.SelectedValue = 7
            ddUsageYear.SelectedValue = Year(System.DateTime.Now)
        Else
            ddUsageYear.SelectedValue = oUsage.UsageYear
        End If
        If Request("UsageID") = 0 Then
            ddContract.SelectedValue = Request("ContractID")
        Else
            ddContract.SelectedValue = oUsage.ContractID
        End If
        get_Inventory()
        If Request("UsageID") > 0 Then
            ddInventory.SelectedValue = oUsage.SoldInventoryID
        End If
        If ddInventory.SelectedValue = "" Then
            Dim l As New ListItem("NONE", 0)
            ddInventory.Items.Add(l)
        End If
        SyncDateField1.Selected_Date = oUsage.InDate

        If oUsage.TypeID = (New clsComboItems).Lookup_ID("ReservationType", "PointTracking") Then
            btnSave.Enabled = False
            Unnamed5.Enabled = False
            LinkButton1.Enabled = False
            LinkButton2.Enabled = False
            LinkButton3.Enabled = False
            LinkButton4.Enabled = False
            LinkButton5.Enabled = False
        End If
        oUsage = Nothing
    End Sub
    Protected Sub get_Inventory()
        Dim oCon As New clsContract
        ddInventory.DataSource = oCon.List_Inventory(ddContract.SelectedValue)
        ddInventory.DataValueField = "SoldInventoryID"
        ddInventory.DataTextField = "Name"
        ddInventory.DataBind()
        oCon = Nothing
    End Sub
    Protected Sub Load_Lookups()
        siType.Connection_String = Resources.Resource.cns
        siType.Label_Caption = ""
        siType.ComboItem = "ReservationType"
        siType.Load_Items()
        siSubType.Connection_String = Resources.Resource.cns
        siSubType.Label_Caption = ""
        siSubType.ComboItem = "ReservationSubType"
        siSubType.Load_Items()
        siStatus.Connection_String = Resources.Resource.cns
        siStatus.Label_Caption = ""
        siStatus.ComboItem = "UsageStatus"
        siStatus.Load_Items()
        siUnitType.Connection_String = Resources.Resource.cns
        siUnitType.Label_Caption = ""
        siUnitType.ComboItem = "UnitType"
        siUnitType.Load_Items()
        siRoomType.Connection_String = Resources.Resource.cns
        siRoomType.Label_Caption = ""
        siRoomType.ComboItem = "RoomType"
        siRoomType.Load_Items()
        siCategory.Connection_String = Resources.Resource.cns
        siCategory.Label_Caption = ""
        siCategory.ComboItem = "UsageCategory"
        siCategory.Load_Items()
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        MultiView3.ActiveViewIndex = 0
    End Sub

    Protected Sub Unnamed2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblAddRoom.Visible = False
        gvAddRooms.Visible = False
        Dim inDate As Date
        Dim oUsage As New clsUsage
        oUsage.UsageID = Request("UsageID")
        oUsage.Load()
        If Date.Compare(System.DateTime.Now, oUsage.InDate) > 0 And Date.Compare(System.DateTime.Now, oUsage.OutDate) < 0 Then
            inDate = CDate(System.DateTime.Now.ToShortDateString)
        Else
            inDate = oUsage.InDate
        End If
        gvAddRooms.DataSource = oUsage.Search_Rooms(inDate, oUsage.OutDate, oUsage.TypeID, oUsage.RoomTypeID, oUsage.UnitTypeID)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvAddRooms.DataKeyNames = sKeys
        gvAddRooms.DataBind()
        lblAddRoom.Visible = True
        gvAddRooms.Visible = True
        oUsage = Nothing
    End Sub

    Protected Sub ddContract_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddContract.SelectedIndexChanged
        get_Inventory()
        If ddInventory.SelectedValue = "" Then
            Dim l As New ListItem("NONE", 0)
            ddInventory.Items.Add(l)
        End If
    End Sub

    Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        inDateTxt.Text = Calendar1.SelectedDate.ToShortDateString
        outDateTxt.Text = CDate(inDateTxt.Text).AddDays(ddDays.SelectedValue).ToShortDateString
        Calendar1.Visible = False
    End Sub

    Protected Sub Unnamed2_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Calendar1.Visible = Not (Calendar1.Visible)
        If IsDate(inDateTxt.Text) Then
            Calendar1.SelectedDate = CDate(inDateTxt.Text)
        Else
            Calendar1.SelectedDate = Date.Today
        End If
        inDateTxt.Text = Calendar1.SelectedDate
    End Sub

    Protected Sub ddDays_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddDays.SelectedIndexChanged
        If inDateTxt.Text <> "" Then
            outDateTxt.Text = CDate(inDateTxt.Text).AddDays(ddDays.SelectedValue).ToShortDateString
        End If
    End Sub

    Private Function Logic_Is_Contract_Legacy_And_Yellow_Season(contract_sub_type_id As Int32, season_id As Int32) As Boolean
        Return IIf((New clsComboItems).Lookup_ComboItem(season_id) = "Yellow" And
                (New clsComboItems).Lookup_ComboItem(contract_sub_type_id) = "Legacy", True, False)
    End Function

    Private Function Calculate_First_Thursday_In_Any_Year(year As Int16) As DateTime
        Dim initial_date As DateTime = New Date(year, 1, 1)
        If initial_date.DayOfWeek = DayOfWeek.Thursday Then
            Return initial_date
        Else
            Do While initial_date.DayOfWeek <> DayOfWeek.Thursday
                initial_date = initial_date.AddDays(1)
            Loop
        End If
        Return initial_date
    End Function

    Private Function Logic_Ensure_Legacy_And_Yellow_Owner_Requirements(checkin_date As DateTime) As Boolean
        '//-- 8/22/2019
        Dim contract = New clsContract
        contract.ContractID = Request("ContractID")
        contract.Load()

        Dim initial_date As DateTime = Calculate_First_Thursday_In_Any_Year(checkin_date.Year)
        Dim counter = 0
        Do While DateTime.Compare(initial_date, checkin_date) <= 0
            initial_date = initial_date.AddDays(7)
            counter += 1
        Loop

        Dim week_number = counter

        If DateTime.Parse(contract.ContractDate).ToShortDateString < "06/23/2000" Then

            If (DateTime.Compare(checkin_date.ToShortDateString(), DateTime.Now.AddDays(60).ToShortDateString()) <= 0) And
               (DateTime.Compare(checkin_date.ToShortDateString(), DateTime.Now.ToShortDateString()) >= 0) Then
                Return True
            Else
                Return IIf((week_number >= 1 And week_number <= 12) Or week_number = 45 Or week_number = 46, True, False)
            End If

        ElseIf DateTime.Parse(contract.ContractDate).ToShortDateString >= "06/23/2000" Then
            If (DateTime.Compare(checkin_date.ToShortDateString(), DateTime.Now.AddDays(60).ToShortDateString()) <= 0) And
               (DateTime.Compare(checkin_date.ToShortDateString(), DateTime.Now.ToShortDateString()) >= 0) Then
                Return True
            Else
                Return IIf(week_number >= 1 And week_number <= 8, True, False)
            End If
        Else
            Return False
        End If

    End Function
    Private Sub Save(ByVal close As Boolean)
        Dim oUsage As New clsUsage
        Dim err As String = ""
        oUsage.UsageID = Request("UsageID")
        oUsage.Load()
        Dim bProceed As Boolean = True


        If Request("UsageID") > 0 Then

            Dim inDate As DateTime = DateTime.MaxValue
            Dim outDate As DateTime = DateTime.MaxValue


            If String.IsNullOrEmpty(oUsage.InDate) = False Then
                inDate = DateTime.Parse(oUsage.InDate)
            End If

            If String.IsNullOrEmpty(oUsage.OutDate) = False Then
                outDate = DateTime.Parse(oUsage.OutDate)
            End If

            If DateTime.Compare(System.DateTime.Now, inDate) < 0 Then
                If oUsage.Room_Count(Request("UsageID"), False, inDate) Then
                    bProceed = False
                    err = "Room(s) Must Be Released From Usage Before Saving."
                End If
            End If

            If Date.Compare(System.DateTime.Now, outDate) >= 0 And (DateTime.Compare(inDate, CDate(inDateTxt.Text)) <> 0 Or DateTime.Compare(outDate, CDate(outDateTxt.Text)) <> 0) Then
                bProceed = False
                err = "You Can Not Change the Dates Of A Usage That Has Passed."
            ElseIf DateTime.Compare(System.DateTime.Now, outDate) >= 0 And oUsage.TypeID <> siType.Selected_ID Then
                bProceed = False
                err = "You Can Not Change the Type Of A Usage That Has Passed."
            ElseIf DateTime.Compare(CDate(inDateTxt.Text), System.DateTime.Now.AddDays(365)) > 0 And Not (CheckSecurity("Usage", "AddYearOutUsage",,, Session("UserDBID"))) Then
                bProceed = False
                err = "In-Date Can Not Be More Than One Year From Todays Date"
            End If

            If bProceed Then
                If DateTime.Compare(System.DateTime.Now, inDate) >= 0 And DateTime.Compare(System.DateTime.Now, outDate) < 0 And ((DateTime.Compare(inDate, CDate(inDateTxt.Text)) <> 0) Or (DateTime.Compare(outDate, CDate(outDateTxt.Text)) <> 0) Or (oUsage.TypeID <> siType.Selected_ID)) Then
                    If DateTime.Compare(inDate, CDate(inDateTxt.Text)) <> 0 Then
                        'Room Check
                        If oUsage.Room_Count(Request("UsageID"), False, inDate) Then
                            bProceed = False
                            err = "InDate Can Not Be Changed Because A Room Is Assigned"
                        End If
                    ElseIf oUsage.TypeID <> siType.Selected_ID Then
                        'Room Check
                        If oUsage.Room_Count(Request("UsageID"), False, inDate) Then
                            bProceed = False
                            err = "Type Can Not Be Changed Because A Room Is Assigned"
                        End If
                    ElseIf DateTime.Compare(outDate, CDate(outDateTxt.Text)) <> 0 Then
                        If DateTime.Compare(CDate(outDateTxt.Text), outDate) > 0 Then
                            'Room Check
                            If oUsage.Room_Count(Request("UsageID"), False, inDate) Then
                                bProceed = False
                                err = "OutDate Can Not Be Changed Because A Room Is Assigned"
                            End If
                        ElseIf DateTime.Compare(CDate(outDateTxt.Text), outDate) < 0 Then
                            'Room Check with Dates
                            If oUsage.Room_Count(Request("UsageID"), True, CDate(outDateTxt.Text)) Then
                                bProceed = False
                                err = "Room Must Be Removed From Reservations Before Changing The OutDate."
                            End If
                        End If
                    End If
                End If
            End If

            If bProceed Then
                Dim oUsageRestriction As New clsUsageRestriction2Contract
                If oUsageRestriction.Check_Restrictions(ddContract.SelectedValue, CDate(inDateTxt.Text), CDate(outDateTxt.Text), ddUsageYear.SelectedValue) Then
                    bProceed = False
                    err = "This Contract Is Restricted From Usage."
                End If
                oUsageRestriction = Nothing
            End If

            If bProceed Then
                'Check that Usage Year is valid for Contract
                Dim oContract As New clsContract
                Dim oFreq As New clsFrequency
                Dim freq As String = ""
                Dim occYear As Integer
                oContract.ContractID = ddContract.SelectedValue
                oContract.Load()
                If Left(oContract.ContractNumber, 1) <> "N" Then
                    occYear = Year(oContract.OccupancyDate)
                    oFreq.FrequencyID = oContract.FrequencyID
                    oFreq.Load()
                    freq = oFreq.Frequency & ""
                    If freq = "Biennial" Then
                        If occYear Mod 2 <> CInt(ddUsageYear.SelectedValue) Mod 2 Then 'Not (((occYear Mod 2 = 0 And ddUsageYear.SelectedValue Mod 2 = 0) Or (occYear Mod 2 = 1 And ddUsageYear.SelectedValue Mod 2 = 1))) Then
                            bProceed = False
                            err = "Invalid Usage Year Selected."
                        End If
                    ElseIf freq = "Triennial" Then
                        If Not (Abs(ddUsageYear.SelectedValue - occYear) Mod 3 = 0) Then
                            bProceed = False
                            err = "Invalid Usage Year Selected."
                        End If
                    End If
                    oFreq = Nothing
                End If
                oContract = Nothing
            End If

            If bProceed Then
                'Check Rental Pool Limits
                Dim oCombo As New clsComboItems
                Dim oLimits As New clsRentalPoolLimits
                Dim limit As Integer = 0
                Dim current As Integer = 0
                If oCombo.Lookup_ComboItem(siType.Selected_ID) = "Rental" Then
                    limit = oLimits.Get_Limit(Year(CDate(inDateTxt.Text)), siUnitType.Selected_ID, siRoomType.Selected_ID, siCategory.Selected_ID)
                    current = oLimits.Get_Current_Amt(Request("UsageID"), Year(CDate(inDateTxt.Text)), siUnitType.Selected_ID, siRoomType.Selected_ID, siCategory.Selected_ID)
                    If current + 1 > limit Then
                        bProceed = False
                        err = "Rental Pool Limit for this type of Usage is " & limit & " and " & current & " have been used."
                        err += oLimits.Error_Message
                    End If
                End If
                oLimits = Nothing
                oCombo = Nothing
            End If

            If bProceed Then
                'Check Changing Category Permission
            End If
        Else
            If inDateTxt.Text = "" Then
                bProceed = False
                err = "Please select an In-Date."
            End If
            If bProceed Then
                If DateTime.Compare(CDate(inDateTxt.Text), System.DateTime.Now.AddDays(365)) > 0 And Not (CheckSecurity("Usage", "AddYearOutUsage",,, Session("UserDBID"))) Then
                    bProceed = False
                    err = "In-Date Can Not Be More Than One Year From Todays Date"
                End If
            End If
            If bProceed Then
                Dim oUsageRestriction As New clsUsageRestriction2Contract
                If oUsageRestriction.Check_Restrictions(ddContract.SelectedValue, CDate(inDateTxt.Text), CDate(outDateTxt.Text), ddUsageYear.SelectedValue) Then
                    bProceed = False
                    err = "This Contract Is Restricted From Usage."
                End If
                oUsageRestriction = Nothing
            End If

            If bProceed Then
                'Check that Usage Year is valid for Contract
                Dim oContract As New clsContract
                Dim oFreq As New clsFrequency
                Dim freq As String = ""
                Dim occYear As Integer
                oContract.ContractID = ddContract.SelectedValue
                oContract.Load()
                If Left(oContract.ContractNumber, 1) <> "N" Then
                    occYear = Year(oContract.OccupancyDate)
                    oFreq.FrequencyID = oContract.FrequencyID
                    oFreq.Load()
                    freq = oFreq.Frequency & ""
                    oFreq = Nothing
                    If freq = "Biennial" Then
                        If Not (((occYear Mod 2 = 0 And ddUsageYear.SelectedValue Mod 2 = 0) Or (occYear Mod 2 = 1 And ddUsageYear.SelectedValue Mod 2 = 1))) Then
                            bProceed = False
                            err = "Invalid Usage Year Selected."
                        End If
                    ElseIf freq = "Triennial" Then
                        If Not (Abs(ddUsageYear.SelectedValue - occYear) Mod 3 = 0) Then
                            bProceed = False
                            err = "Invalid Usage Year Selected."
                        End If
                    End If
                End If
                oContract = Nothing
            End If

            If bProceed Then
                'Check Rental Pool Limits
                Dim oCombo As New clsComboItems
                Dim oLimits As New clsRentalPoolLimits
                Dim limit As Integer = 0
                Dim current As Integer = 0
                If oCombo.Lookup_ComboItem(siType.Selected_ID) = "Rental" Then
                    limit = oLimits.Get_Limit(Year(CDate(inDateTxt.Text)), siUnitType.Selected_ID, siRoomType.Selected_ID, siCategory.Selected_ID)
                    current = oLimits.Get_Current_Amt(Request("UsageID"), Year(CDate(inDateTxt.Text)), siUnitType.Selected_ID, siRoomType.Selected_ID, siCategory.Selected_ID)
                    If current + 1 > limit Then
                        bProceed = False
                        err = "Rental Pool Limit for this type of Usage is " & limit & " and " & current & " have been used."
                        err += oLimits.Error_Message
                    End If
                End If
                oLimits = Nothing
                oCombo = Nothing
            End If
            oUsage.DateCreated = System.DateTime.Now
        End If

        If bProceed Then
            Dim uID As Integer = 0
            oUsage.UserID = Session("UserDBID")
            oUsage.TypeID = siType.Selected_ID
            oUsage.ContractID = ddContract.SelectedValue
            oUsage.UsageYear = ddUsageYear.SelectedValue
            oUsage.SubTypeID = siSubType.Selected_ID
            oUsage.CategoryID = siCategory.Selected_ID
            oUsage.AmountPromised = amtPromisedTxt.Text
            oUsage.SoldInventoryID = ddInventory.SelectedValue
            oUsage.StatusID = siStatus.Selected_ID
            oUsage.UnitTypeID = siUnitType.Selected_ID
            oUsage.RoomTypeID = siRoomType.Selected_ID
            oUsage.Days = ddDays.SelectedValue
            oUsage.Points = pointsTxt.Text
            oUsage.InDate = CDate(inDateTxt.Text)
            oUsage.OutDate = CDate(outDateTxt.Text)

            Dim oContract As New clsContract
            oContract.ContractID = ddContract.SelectedValue
            oContract.Load()

            Dim logic_passed = False
            If Logic_Is_Contract_Legacy_And_Yellow_Season(oContract.SubTypeID, oContract.SeasonID) Then
                If Logic_Ensure_Legacy_And_Yellow_Owner_Requirements(inDateTxt.Text) Then

                    If CheckSecurity("Usage", "Members Override Yellow Season Legacy Contract",,, Session("UserDBID")) = False Then
                        ClientScript.RegisterClientScriptBlock(Me.GetType, DateTime.Now.Ticks, "alert('Legacy owner with yellow season contract is not entitled to those dates at this time');", True)
                        Return
                    End If

                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, DateTime.Now.Ticks, "alert('Legacy owner with yellow season contract is not entitled to those dates at this time');", True)
                End If

            End If

            Return

            If oUsage.Save() Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "ALERT", "alert('SAVED');", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "ALERT", "alert('NOT SAVED');", True)
            End If

            uID = oUsage.UsageID
            If close = True Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.Refresh_Usages();window.close();", True)
            Else
                Response.Redirect("editUsage.aspx?usageID=" & uID)
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.Refresh_Usages();window.location.href('editUsage.aspx?UsageID=" & oUsage.UsageID & "&ContractID=" & Request("ContractID") & "');", True)
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "test", "alert('" & err & "');", True)
            'Set_Values()
        End If
        oUsage = Nothing
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Save(False)
    End Sub

    Protected Sub Unnamed2_Click2(ByVal sender As Object, ByVal e As System.EventArgs)
        If Request("UsageID") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Save Usage First');", True)
        Else
            filterTxt.Text = ""
            gvOwners.Visible = False
            MultiView1.ActiveViewIndex = 1 'Response.Redirect("editUsageOwner.aspx?UsageID=" & Request("UsageID"))
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim oPros As New clsProspect
        Dim filter() As String
        If InStr(filterTxt.Text, ",") <> 0 Then
            filter = filterTxt.Text.Split(",")
            gvOwners.DataSource = oPros.List_Owners(filter(0), Trim(filter(1)))
        Else
            gvOwners.DataSource = oPros.List_Owners(filterTxt.Text, "")
        End If
        Dim sKeys(0) As String
        sKeys(0) = "ProspectID"
        gvOwners.DataKeyNames = sKeys
        gvOwners.DataBind()
        gvOwners.Visible = True
        oPros = Nothing
    End Sub

    Protected Sub LinkButton5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton5.Click
        If Request("UsageID") > 0 Then
            Dim oUsage As New clsUsage
            gvReservations.DataSource = oUsage.Get_Usage_Reservations(Request("UsageID"))
            Dim sKeys(0) As String
            sKeys(0) = "ResID"
            gvReservations.DataKeyNames = sKeys
            gvReservations.DataBind()
            MultiView3.ActiveViewIndex = 4
            oUsage = Nothing
        End If
    End Sub

    Protected Sub gvOwners_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvOwners.SelectedIndexChanged
        Dim row As GridViewRow = gvOwners.SelectedRow
        Dim oPros As New clsProspect
        Dim oUsage As New clsUsage
        oPros.Prospect_ID = row.Cells(1).Text
        oPros.Load()
        ownerTxt.Text = oPros.Last_Name & ", " & oPros.First_Name
        ddContract.DataSource = oUsage.List_Contracts("Prospect", oPros.Prospect_ID)
        ddContract.DataValueField = "ContractID"
        ddContract.DataTextField = "ContractNumber"
        ddContract.DataBind()
        get_Inventory()
        If ddInventory.SelectedValue = "" Then
            Dim l As New ListItem("NONE", 0)
            ddInventory.Items.Add(l)
        End If
        MultiView1.ActiveViewIndex = 0
        MultiView2.ActiveViewIndex = 0
        MultiView3.ActiveViewIndex = 0
        oPros = Nothing
        oUsage = Nothing
    End Sub

    Protected Sub gvReservations_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReservations.SelectedIndexChanged
        Dim row As GridViewRow = gvReservations.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.navigate('" & Request.ApplicationPath & "/marketing/editReservation.aspx?ReservationID=" & row.Cells(1).Text & "','win01',690,450);window.close();", True)
    End Sub

    Protected Sub gvReservations_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(2).Text = CDate(e.Row.Cells(2).Text).ToShortDateString
                e.Row.Cells(3).Text = CDate(e.Row.Cells(3).Text).ToShortDateString
            End If
        End If
    End Sub

    Protected Sub Unnamed6_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtNote.Text = ""
        MultiView3.ActiveViewIndex = 5
        'ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.open('" & Request.ApplicationPath & "/general/editnote.aspx?KeyField=UsageID&KeyValue=" & Request("UsageID") & "','win01','width=350,height=350');", True)
    End Sub

    Protected Sub gvAddRooms_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAddRooms.SelectedIndexChanged
        Dim row As GridViewRow = gvAddRooms.SelectedRow
        Dim oUsage As New clsUsage
        Dim inDate As Date
        oUsage.UsageID = Request("usageID")
        oUsage.Load()
        oUsage.UserID = Session("UserDBID")
        If Date.Compare(System.DateTime.Now, oUsage.InDate) > 0 And Date.Compare(System.DateTime.Now, oUsage.OutDate) < 0 Then
            inDate = CDate(System.DateTime.Now.ToShortDateString)
        Else
            inDate = oUsage.InDate
        End If

        '**** Loops through multiple rooms ******'
        '**** Added to be able to add 3/4 BD to usages 3/28/13 -MB *****

        Dim aRooms() As String = Split(row.Cells(1).Text, "|")
        Dim bProceed As Boolean = False
        For i = 0 To UBound(aRooms)
            bProceed = oUsage.Add_Room(oUsage.UsageID, CInt(aRooms(i)), oUsage.InDate, CDate(oUsage.OutDate).AddDays(-1))
            If Not (bProceed) Then
                Exit For
            End If
        Next
        '****** End 3/28 Addition
        If bProceed Then
            'Following Line was in original usage add room, replaced 3/28 to add 3/4 bd
            'If oUsage.Add_Room(oUsage.UsageID, row.Cells(1).Text, oUsage.InDate, CDate(oUsage.OutDate).AddDays(-1)) Then
            gvRooms.DataSource = oUsage.List_Rooms(oUsage.UsageID, oUsage.OutDate)
            Dim sKeys(0) As String
            sKeys(0) = "RoomID"
            gvRooms.DataKeyNames = sKeys
            gvRooms.DataBind()
            lblAddRoom.Visible = False
            gvAddRooms.Visible = False
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & oUsage.Err & "');", True)
            lblAddRoom.Visible = False
            gvAddRooms.Visible = False
        End If
        oUsage = Nothing
    End Sub

    Protected Sub Unnamed5_Click(sender As Object, e As System.EventArgs)
        Save(True)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Protected Sub gvRooms_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvRooms.SelectedIndexChanged

    End Sub

    Protected Sub Unnamed7_Click(sender As Object, e As System.EventArgs)
        Dim oNotes As New clsNotes
        oNotes.NoteID = 0
        oNotes.CreatedByID = Session("UserDBID")
        oNotes.KeyField = "UsageID"
        oNotes.KeyValue = Request("UsageID")
        oNotes.Note = txtNote.Text
        oNotes.DateCreated = System.DateTime.Now
        oNotes.UserID = Session("UserDBID")
        oNotes.Save()

        oNotes.KeyField = "UsageID"
        oNotes.KeyValue = Request("UsageID")
        gvNotes.DataSource = oNotes.List
        gvNotes.DataBind()
        MultiView3.ActiveViewIndex = 2
        oNotes = Nothing
    End Sub

    Protected Sub Unnamed8_Click(sender As Object, e As System.EventArgs)
        MultiView3.ActiveViewIndex = 2
    End Sub

    Protected Sub LinkButton6_Click(sender As Object, e As System.EventArgs) Handles LinkButton6.Click
        '  ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.Opener.Print_Usage_Letter('" & Request("UsageID") & "');", True)
        'ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.open('../default.aspx');", True)
        If Request("UsageID") > 0 Then
            Dim oUsage As New clsUsage
            Dim oCombo As New clsComboItems
            oUsage.UsageID = Request("UsageID")
            oUsage.Load()
            If oCombo.Lookup_ComboItem(oUsage.SubTypeID) = "RCI" Or oCombo.Lookup_ComboItem(oUsage.SubTypeID) = "II" Or oCombo.Lookup_ComboItem(oUsage.SubTypeID) = "ICE" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.open('usageConfLetter.aspx?UsageID=" & oUsage.UsageID & "');", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('No Letter Available for this Sub Type.');", True)
            End If
            oUsage = Nothing
            oCombo = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Save Usage First.');", True)
        End If
    End Sub

    Protected Sub SyncDateField1_Date_Updated() Handles SyncDateField1.Date_Updated
        inDateTxt.Text = SyncDateField1.Selected_Date
        outDateTxt.Text = CDate(inDateTxt.Text).AddDays(ddDays.SelectedValue).ToShortDateString
        Calendar1.Visible = False
    End Sub

    Protected Sub SyncDateField1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles SyncDateField1.Load

    End Sub
End Class
