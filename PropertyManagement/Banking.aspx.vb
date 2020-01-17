Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data

Partial Class PropertyManagement_Banking
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddUserName.Items.Add("ALL")
            Dim oPersonnel As New clsPersonnel2Dept
            Dim oVendor As New clsVendor2Personnel
            ddUserName.DataSource = oPersonnel.Get_Members("Owner Services")
            ddUserName.DataValueField = "PersonnelID"
            ddUserName.DataTextField = "UserName"
            ddUserName.AppendDataBoundItems = True
            ddUserName.DataBind()
            ddUserName.DataSource = oPersonnel.Get_Members("Systems Operations")
            ddUserName.DataValueField = "PersonnelID"
            ddUserName.DataTextField = "UserName"
            ddUserName.AppendDataBoundItems = True
            ddUserName.DataBind()
            ddUserName.DataSource = oVendor.Get_Members("Complete Call Solutions")
            ddUserName.DataValueField = "PersonnelID"
            ddUserName.DataTextField = "UserName"
            ddUserName.AppendDataBoundItems = True
            ddUserName.DataBind()
            oVendor = Nothing
            oPersonnel = Nothing

            Dim oCombo As New clsComboItems
            ddExchCompany.Items.Add("ALL")
            ddExchCompany.DataSource = oCombo.Load_ComboItems("ReservationSubType")
            ddExchCompany.DataValueField = "ComboItemID"
            ddExchCompany.DataTextField = "ComboItem"
            ddExchCompany.AppendDataBoundItems = True
            ddExchCompany.DataBind()
            Dim j As Integer = 0
            Dim i As Integer = 1
            j = ddExchCompany.Items.Count
            Do While i < j
                If ddExchCompany.Items(i).Text = "Owner" Or ddExchCompany.Items(i).Text = "Non-Owner" Or ddExchCompany.Items(i).Text = "Development" Or ddExchCompany.Items(i).Text = "Marketing" Or ddExchCompany.Items(i).Text = "Sales" Or ddExchCompany.Items(i).Text = "Charity" Then
                    ddExchCompany.Items.Remove(ddExchCompany.Items(i))
                    i = i - 1
                    j = j - 1
                Else
                    i = i + 1
                End If
            Loop

            ddBankExchCompany.Items.Add(New ListItem("", 0))
            ddBankExchCompany.DataSource = oCombo.Load_ComboItems("ReservationSubType")
            ddBankExchCompany.DataValueField = "ComboItemID"
            ddBankExchCompany.DataTextField = "ComboItem"
            ddBankExchCompany.AppendDataBoundItems = True
            ddBankExchCompany.DataBind()
            i = 1
            j = ddBankExchCompany.Items.Count
            Do While i < j
                If ddBankExchCompany.Items(i).Text = "Owner" Or ddBankExchCompany.Items(i).Text = "Non-Owner" Or ddBankExchCompany.Items(i).Text = "Development" Or ddBankExchCompany.Items(i).Text = "Marketing" Or ddBankExchCompany.Items(i).Text = "Sales" Or ddBankExchCompany.Items(i).Text = "Charity" Or ddBankExchCompany.Items(i).Text = "NALJR" Then
                    ddBankExchCompany.Items.Remove(ddBankExchCompany.Items(i))
                    i = i - 1
                    j = j - 1
                Else
                    i = i + 1
                End If
            Loop

            ddStatus.Items.Add("ALL")
            ddStatus.DataSOurce = oCombo.Load_ComboItems("BankingStatus")
            ddStatus.DataValueField = "ComboItemID"
            ddStatus.DataTextField = "ComboItem"
            ddStatus.AppendDataBoundItems = True
            ddStatus.DataBind()
            oCombo = Nothing


            siUnitType.Connection_String = Resources.Resource.cns
            siUnitType.ComboItem = "InventoryType"
            siUnitType.Label_Caption = ""
            siUnitType.Load_Items()

            Dim oFreq As New clsFrequency
            ddFrequency.DataSource = oFreq.List_Frequencies()
            ddFrequency.DataValueField = "FrequencyID"
            ddFrequency.DataTextField = "Frequency"
            ddFrequency.DataBind()
            oFreq = Nothing

            siSeason.Connection_String = Resources.Resource.cns
            siSeason.ComboItem = "Season"
            siSeason.Label_Caption = ""
            siSeason.Load_Items()

            ddYear.Items.Add(New ListItem("", 0))
            For i = Year(System.DateTime.Now) - 15 To Year(System.DateTime.Now) + 5
                ddYear.Items.Add(i)
            Next

            ddUnitSize.Items.Add("1BD")
            Dim newCombo As New clsComboItems
            ddUnitSize.DataSource = newCombo.Load_ComboItems("UnitSubType")
            ddUnitSize.DataValueField = "ComboItem"
            ddUnitSize.DataTextField = "ComboItem"
            ddUnitSize.AppendDataBoundItems = True
            ddUnitSize.DataBind()
            ddUnitSize.Items.Add("4BD")

            ddBStatus.DataSource = newCombo.Load_ComboItems("BankingStatus")
            ddBStatus.DataValueField = "ComboItemID"
            ddBStatus.DataTextField = "ComboItem"
            ddBStatus.DataBind()

            newCombo = Nothing


        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        lblErr.Text = ""
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If dteStartDate.Selected_Date.ToString = "" Or dteEndDate.Selected_Date.ToString = "" Then
            bProceed = False
            sErr = "Please Select a Start Date and an End Date."
        End If






        If bProceed Then
            Dim uname As String = "''"
            Dim eComp As String = "''"
            Dim status As String = "''"
            Dim oBankedUnit As New clsBankedUnits




            If ddUserName.SelectedValue = "ALL" Then
                For i = 1 To ddUserName.Items.Count - 1
                    If uname = "''" Then
                        uname = "'" & ddUserName.Items(i).Value & "'"
                    Else
                        uname = uname & ",'" & ddUserName.items(i).value & "'"
                    End If
                Next
            Else
                uname = "'" & ddUserName.SelectedValue & "'"
            End If



            If ddExchCompany.SelectedValue = "ALL" Then
                For i = 1 To ddExchCompany.Items.Count - 1
                    If eComp = "''" Then
                        eComp = "'" & ddExchCompany.Items(i).Value & "'"
                    Else
                        eComp = eComp & ",'" & ddExchCompany.Items(i).Value & "'"
                    End If
                Next
            Else
                eComp = "'" & ddExchCompany.SelectedValue & "'"
            End If
            If ddStatus.SelectedValue = "ALL" Then
                For i = 1 To ddStatus.Items.Count - 1
                    If status = "''" Then
                        status = "'" & ddStatus.Items(i).Value & "'"
                    Else
                        status = status & ",'" & ddStatus.items(i).value & "'"
                    End If
                Next
            Else
                status = "'" & ddStatus.SelectedValue & "'"
            End If




            Try






                gvBankedUnits.DataSource = oBankedUnit.List_BankedUnits(String.Format("{0}", uname), String.Format("{0}", eComp), String.Format("{0}", status), dteStartDate.Selected_Date, dteEndDate.Selected_Date, txtContract.Text, cbDateEntered.Checked)
                
                gvBankedUnits.DataBind()

            Catch ex As Exception
                'Response.Write(ex.Message)
            Finally
                oBankedUnit = Nothing
            End Try
        Else
            gvBankedUnits = Nothing
            lblErr.Text = sErr
        End If
        MultiView1.ActiveViewIndex = 0

    End Sub

    Private Sub Set_Values()
        Dim oPros As New clsProspect
        Dim oCon As New clsContract
        Dim oBankedUnit As New clsBankedUnits
        Dim oBankRoom As New clsBank2Room
        Dim oCombo As New clsComboItems
        oPros.Prospect_ID = hfProsID.Value
        oPros.Load()
        txtFName.Text = oPros.First_Name
        txtLName.Text = oPros.Last_Name
        oPros = Nothing
        oCon.ContractID = hfConID.Value
        oCon.Load()
        txtConNumber.Text = oCon.ContractNumber
        lblContractType.Text = (New clsComboItems).Lookup_ComboItem(oCon.SubTypeID)
        oCon = Nothing
        oBankedUnit.DepositID = hfDepID.Value
        oBankedUnit.Load()
        siUnitType.Selected_ID = oBankedUnit.UnitTypeID
        ddFrequency.SelectedValue = oBankedUnit.FrequencyID
        siSeason.Selected_ID = oBankedUnit.SeasonID
        ddYear.SelectedValue = oBankedUnit.UsageYear
        ddUnitSize.SelectedValue = oBankedUnit.UnitSize
        ddBankExchCompany.SelectedValue = oBankedUnit.ExchangeID
        txtMemNumber.Text = oBankedUnit.MembershipNumber
        txtStatusDate.Text = oBankedUnit.StatusDate
        txtUsageID.Text = oBankedUnit.UsageID
        tRowStatus.Visible = True

        If oCombo.Lookup_ComboItem(oBankedUnit.StatusID) <> "Complete" Then
            For i = 0 To ddBStatus.Items.Count - 1
                If ddBStatus.Items(i).Text = "Complete" And Not (CheckSecurity("BankingRequests", "MarkComplete", "", "", Session("UserDBID"), "")) Then
                    ddBStatus.Items.Remove(ddBStatus.Items(i))
                    Exit For
                End If
            Next
        Else
            ddBStatus.DataSource = oCombo.Load_ComboItems("BankingStatus")
            ddBStatus.DataValueField = "ComboItemID"
            ddBStatus.DataTextField = "ComboItem"
            ddBStatus.DataBind()
        End If
        'Check For Edit Permission
        If CheckSecurity("BankingRequests", "Edit",,, Session("UserDBID")) Then
            Dim bFound As Boolean = False
            For i = 0 To ddBStatus.Items.Count - 1
                If ddBStatus.Items(i).Value = "Edit" Then
                    bFound = True
                    Exit For
                End If
            Next
            If Not (bFound) Then
                ddBStatus.Items.Add(New ListItem("Edit", "Edit"))
            End If
        End If
        ddBStatus.SelectedValue = oBankedUnit.StatusID
        If ddBStatus.SelectedValue <> "Edit" Then
            tRowCI.Visible = False
            tRowUsage.Visible = False
            tRowUType.Visible = False
            tRowRoom.Visible = False
            tRowConf.Visible = False
            rbusageType.Items.Item(0).Selected = False
            rbusageType.Items.Item(1).Selected = False
            txtConfNum.Text = ""
            dteCheckIn.Selected_Date = ""
            txtUID.Text = ""
        End If
        Dim oRooms As New clsBankedUnits
        ddRooms.DataSource = oRooms.List_Rooms(oBankedUnit.UnitSize, oCombo.Lookup_ComboItem(oBankedUnit.UnitTypeID))
        ddRooms.DataValueField = "RoomID"
        ddRooms.DataTextField = "Room"
        ddRooms.DataBind()
        oRooms = Nothing
        oCombo = Nothing
        oPros = Nothing
        oCon = Nothing
        oBankedUnit = Nothing
        oBankRoom = Nothing
    End Sub

    Protected Sub gvBankedUnits_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvBankedUnits.SelectedIndexChanged
        Dim row As GridViewRow = gvBankedUnits.SelectedRow
        hfProsID.Value = row.Cells(2).Text
        hfConID.Value = row.Cells(3).Text
        hfDepID.Value = row.Cells(1).Text
        tRowCI.Visible = False
        tRowUsage.Visible = False
        tRowUType.Visible = False
        tRowRoom.Visible = False
        tRowConf.Visible = False
        rbusageType.Items.Item(0).Selected = False
        rbusageType.Items.Item(1).Selected = False
        txtConfNum.Text = ""
        dteCheckIn.Selected_Date = ""
        txtUID.Text = ""
        Set_Values()
        MultiView1.ActiveViewIndex = 1
    End Sub
    Private Sub Initialize_Bank()
        Dim oCon As New clsContract
        Dim oPros As New clsProspect
        hfDepID.Value = 0
        oCon.ContractID = oCon.Get_Contract_ID(txtContract.Text)
        oCon.Load()
        oPros.Prospect_ID = oCon.ProspectID
        oPros.Load()
        hfProsID.value = oCon.ProspectID
        hfConID.value = oCon.ContractID
        txtFName.Text = oPros.First_Name
        txtLName.Text = oPros.Last_Name
        ddFrequency.SelectedValue = oCon.FrequencyID
        siSeason.Selected_ID = oCon.SeasonID
        siUnitType.Selected_ID = 0
        ddYear.SelectedValue = 0
        ddUnitSize.SelectedValue = "1BD"
        ddBankExchCompany.SelectedValue = 0
        tRowStatus.Visible = False
        tRowCI.Visible = False
        tRowUsage.Visible = False
        tRowUType.Visible = False
        tRowRoom.Visible = False
        tRowConf.Visible = False
        rbusageType.Items.Item(0).Selected = False
        rbusageType.Items.Item(1).Selected = False
        dteCheckIn.Selected_Date = ""
        txtUsageID.Text = ""
        txtUID.Text = ""
        txtMemNumber.Text = ""
        txtConNumber.Text = txtContract.Text
        txtConfNum.Text = ""
        txtStatusDate.Text = ""
        lblContractType.Text = (New clsComboItems).Lookup_ComboItem(oCon.SubTypeID)
        oCon = Nothing
        oPros = Nothing
    End Sub
    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
        lblErr.Text = ""
        Dim oCon As New clsContract
        Dim oPros As New clsProspect
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If txtContract.Text = "" Then
            bProceed = False
        Else
            bProceed = oCon.Verify_Contract(txtContract.Text)
        End If
        If bProceed Then
            Initialize_Bank()
            MultiView1.ActiveViewIndex = 1
        Else
            lblErr.Text = "Please Enter a Valid Contract"
            gvBankedUnits = Nothing
            MultiView1.ActiveViewINdex = 0
        End If
        oPros = Nothing
        oCon = Nothing
    End Sub

    Protected Sub ddBankExchCompany_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddBankExchCompany.SelectedIndexChanged
        txtMemNumber.Text = ""
        If ddBankExchCompany.SelectedValue <> 0 Then
            Dim oCon As New clsContract
            Dim oUF As New clsUserFields
            Dim uf As String = ddBankExchCompany.SelectedItem.Text & " Membership Number"
            oCon.ContractID = oCon.Get_Contract_ID(txtConNumber.Text)
            oCon.Load()
            txtMemNumber.Text = oUF.Get_UserField_Value(oUF.Get_UserFieldID(oUF.Get_GroupID("Prospect"), uf), oCon.ProspectID)
            oCon = Nothing
            oUF = Nothing
        End If
    End Sub

    Protected Sub rbUsageType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles rbusageType.SelectedIndexChanged
        If rbusageType.SelectedItem.Value = "Create" Then
            Dim oCombo As New clsComboItems
            Dim oBUnits As New clsBankedUnits
            oBUnits.List_Rooms(ddUnitSize.SelectedValue, oCombo.Lookup_ComboItem(siUnitType.Selected_ID))
            tRowCI.Visible = True
            tRowRoom.Visible = True
            tRowUsage.Visible = False
        Else
            tRowUsage.Visible = True
            txtUID.Text = txtUsageID.Text
            tRowCI.Visible = False
            tRowRoom.Visible = False
        End If
        tRowConf.Visible = True
    End Sub

    Protected Sub ddBStatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddBStatus.SelectedIndexChanged
        If ddBStatus.SelectedValue = "Edit" Then
            tRowUType.Visible = True
        Else
            tRowUType.Visible = False
            tRowUsage.Visible = False
            tRowCI.Visible = False
            tRowRoom.Visible = False
            tRowConf.Visible = False
        End If
    End Sub

    Protected Sub gvBankedUnits_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBankedUnits.RowDataBound

        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(7).Text <> "&nbsp;" Then
                    e.Row.Cells(7).Text = CDate(e.Row.Cells(7).Text).ToShortDateString
                End If
                'If e.Row.Cells(14).Text <> "&nbsp;" Then
                'e.Row.Cells(14).Text = CDate(e.Row.Cells(14).Text).ToShortDateString
                'End If
            End If
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
            e.Row.Cells(3).Visible = False
        End If
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""

        If siUnitType.Selected_ID < 1 Or siSeason.Selected_ID < 1 Or ddYear.SelectedValue = 0 Or ddBankExchCompany.SelectedValue = 0 Or txtMemNumber.Text = "" Then
            bProceed = False
            sErr = "Please Fill In All Required Fields."
        End If
        Dim bProc As Boolean = True

        If bProceed Then
            Dim oBankedUnit As New clsBankedUnits
            Dim oCombo As New clsComboItems
            If hfDepID.Value = 0 Then
                oBankedUnit.DepositID = 0
            Else
                oBankedUnit.DepositID = hfDepID.Value
            End If
            oBankedUnit.Load()
            oBankedUnit.UserID = Session("UserDBID")
            oBankedUnit.ProspectID = hfProsID.Value
            oBankedUnit.ContractID = hfConID.Value
            oBankedUnit.UnitTypeID = siUnitType.Selected_ID
            oBankedUnit.FrequencyID = ddFrequency.SelectedValue
            oBankedUnit.SeasonID = siSeason.Selected_ID
            oBankedUnit.UsageYear = ddYear.SelectedValue
            oBankedUnit.UnitSize = ddUnitSize.SelectedValue
            oBankedUnit.ExchangeID = ddBankExchCompany.SelectedValue
            oBankedUnit.MembershipNumber = txtMemNumber.Text
            If hfDepID.Value > 0 Then
                If oCombo.Lookup_ComboItem(oBankedUnit.StatusID) = "Complete" And (ddBStatus.SelectedItem.Text <> "Complete") Then
                    If Not (CheckSecurity("BankingRequests", "EditComplete",,, Session("UserDBID"))) Then
                        bProc = False
                        sErr = "You Do Not Have Permission to Edit a Completed Deposit."
                    End If
                End If
            End If
            If txtUsageID.Text <> "" Then
                oBankedUnit.UsageID = txtUsageID.Text
            End If
            If bProc Then
                If ddBStatus.SelectedValue = "Edit" Then
                    If rbusageType.SelectedIndex = -1 Then
                        bProc = False
                        sErr = "You Must Create a Usage or Tie To Existing Usage"
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
                    ElseIf rbusageType.SelectedItem.Value = "Create" Then
                        'ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('CREATE');", True)

                        If dteCheckIn.Selected_Date <> "" Then
                            'ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('VALID CI DATE');", True)

                            Dim roomID As String
                            Dim sRooms() As String
                            sRooms = ddRooms.SelectedValue.Split("|")
                            roomID = sRooms(0)
                            Dim oRoom As New clsRooms

                            oRoom.RoomID = roomID
                            oRoom.Load()
                            'Make sure check in day of room matches check in day of CheckIn day specified
                            'ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & Left(CDate(dteCheckIn.Selected_Date).DayOfWeek.ToString, 3).ToUpper & "');", True)

                            If oCombo.Lookup_ComboItem(oRoom.SubTypeID).ToString.ToUpper <> Left(CDate(dteCheckIn.Selected_Date).DayOfWeek.ToString, 3).ToUpper Then
                                bProc = False
                                sErr = "Check In Day does not match Check In Day of unit" 'ERROR
                                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
                            Else
                                'Validate room is Owner
                                'ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('CHECKINDAY MATCHES');", True)
                                For i = 0 To sRooms.Length - 1
                                    If Not (oBankedUnit.Validate_Room(sRooms(i), dteCheckIn.Selected_Date, "Owner")) Then
                                        bProc = False
                                        sErr = "Room is Not Allocated as Owner Inventory"
                                        'Error
                                    End If
                                    'ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)

                                    If bProc Then
                                        If Not (oBankedUnit.Validate_Room_UsageID(sRooms(i), dteCheckIn.Selected_Date)) Then
                                            bProc = False
                                            'Error
                                            sErr = "Room Is Tied to Another Usage"
                                        End If
                                    End If
                                Next


                                If bProc Then
                                    'ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('CREATING USAGE');", True)
                                    Dim oUsage As New clsUsage
                                    Dim uID As Integer = 0
                                    oUsage.UsageID = 0
                                    oUsage.Load()
                                    oUsage.ContractID = oBankedUnit.ContractID
                                    oUsage.TypeID = oCombo.Lookup_ID("ReservationType", "Exchange")
                                    oUsage.SubTypeID = ddBankExchCompany.SelectedValue
                                    oUsage.UsageYear = ddYear.SelectedValue
                                    oUsage.UnitTypeID = siUnitType.Selected_ID
                                    oUsage.RoomTypeID = oCombo.Lookup_ID("RoomType", ddUnitSize.SelectedValue)
                                    oUsage.Days = 7
                                    oUsage.InDate = dteCheckIn.Selected_Date
                                    oUsage.OutDate = CDate(dteCheckIn.Selected_Date).AddDays(7)
                                    oUsage.StatusID = oCombo.Lookup_ID("UsageStatus", "Used")
                                    oUsage.UserID = Session("UserDBID")
                                    oUsage.Save()
                                    uID = oUsage.UsageID
                                    Dim oRMX As New clsRoomAllocationMatrix
                                    Dim tempDate As Date = dteCheckIn.Selected_Date
                                    For i = 0 To sRooms.Length - 1
                                        tempDate = dteCheckIn.Selected_Date
                                        Do While DateTime.Compare(tempDate, CDate(dteCheckIn.Selected_Date).AddDays(6)) < 1
                                            oRMX.AllocationID = oRMX.Get_Allocation_ID(tempDate, sRooms(i))
                                            oRMX.UserID = Session("UserDBID")
                                            oRMX.Load()
                                            oRMX.UsageID = uID
                                            oRMX.TypeID = oCombo.Lookup_ID("ReservationType", "Exchange")
                                            oRMX.Save()
                                            tempDate = tempDate.AddDays(1)
                                        Loop
                                    Next i
                                    oBankedUnit.UsageID = uID
                                    oBankedUnit.StatusID = oCombo.Lookup_ID("BankingStatus", "Complete")
                                    oBankedUnit.StatusDate = System.DateTime.Now
                                    oBankedUnit.ConfirmationNumber = txtConfNum.Text
                                    oBankedUnit.DepositYear = Year(CDate(dteCheckIn.Selected_Date))
                                    Dim oEmail As New clsEmail
                                    If oEmail.Get_Primary_Email(oBankedUnit.ProspectID) <> "" Then
                                        Send_Mail(oEmail.Get_Primary_Email(oBankedUnit.ProspectID), "OwnerServices@kingscreekplantation.com", ddBankExchCompany.SelectedItem.Text & " Deposit Confirmation", oUsage.Print_Conf_Letter(oUsage.UsageID))
                                        Dim oNote As New clsNotes
                                        oNote.NoteID = 0
                                        oNote.Load()
                                        oNote.UserID = Session("USERDBID")
                                        oNote.KeyField = "ContractID"
                                        oNote.KeyValue = oBankedUnit.ContractID
                                        oNote.Note = "Emailed Confirmation Letter to " & oEmail.Get_Primary_Email(oBankedUnit.ProspectID) & " on " & System.DateTime.Now.ToShortDateString & ". UsageID:" & uID

                                        oNote.DateCreated = System.DateTime.Now
                                        oNote.Save()
                                        oNote = Nothing
                                    Else
                                        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('No Email On File.');", True)
                                    End If
                                    oEmail = Nothing
                                Else
                                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
                                End If
                            End If
                        Else
                            bProc = False
                            sErr = "Please Select a Check-In Date"
                            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
                            'Error
                        End If
                    ElseIf rbusageType.SelectedItem.Value = "Exists" Then
                        Dim oUsage As New clsUsage
                        'Dim bProc As Boolean = True
                        Dim oContract As New clsContract
                        oUsage.UsageID = txtUID.Text
                        oUsage.UserID = Session("UserDBID")
                        oUsage.Load()
                        oContract.ContractID = oUsage.ContractID
                        oContract.Load()
                        If oContract.ContractNumber = "IIBULKBANK" Then
                            If oCombo.Lookup_ComboItem(ddBankExchCompany.SelectedValue) <> "II" Then
                                bProc = False
                                sErr = "Usage Is tied to KCP: IIBULKBANK but the Exchange Company Is Not II"
                            ElseIf oCombo.Lookup_ComboItem(siUnitType.Selected_ID) <> oCombo.Lookup_ComboItem(oUsage.UnitTypeID) Or ddUnitSize.SelectedValue <> oCombo.Lookup_ComboItem(oUsage.RoomTypeID) Then
                                bProc = False
                                sErr = "Usage Unit/Room Size does not match Deposit Unit/Room Size."
                            End If
                        ElseIf oContract.ContractNumber = "RCIBULKBANK" Then
                            If oCombo.Lookup_ComboItem(ddBankExchCompany.SelectedValue) <> "RCI" Then
                                bProc = False
                                sErr = "Usage is tied to KCP: RCIBULKBANK but the Exchange Company Is Not RCI"
                            ElseIf oCombo.Lookup_ComboItem(siUnitType.Selected_ID) <> oCombo.Lookup_ComboItem(oUsage.UnitTypeID) Or ddUnitSize.SelectedValue <> oCombo.Lookup_ComboItem(oUsage.RoomTypeID) Then
                                bProc = False
                                sErr = "Usage Unit/Room Size does not match Deposit Unit/Room Size."
                            End If
                        ElseIf oContract.ContractNumber = txtConNumber.Text Then
                            If oCombo.Lookup_ComboItem(oUsage.StatusID) <> "Used" Or ddYear.SelectedValue <> oUsage.UsageYear Or oCombo.Lookup_ComboItem(siUnitType.Selected_ID) <> oCombo.Lookup_ComboItem(oUsage.UnitTypeID) Or ddUnitSize.SelectedValue <> oCombo.Lookup_ComboItem(oUsage.RoomTypeID) Then
                                bProc = False
                                sErr = "Invalid Usage."
                            End If
                        Else
                            bProc = False
                            sErr = "Usage Tied to Invalid Contract"
                        End If

                        If bProc Then
                            Dim dt As DataTable = oUsage.List_Rooms(oUsage.UsageID, System.DateTime.Now)
                            Dim oRoom As New clsRooms
                            oRoom.RoomID = dt.Rows(0)("RoomID")
                            oRoom.Load()
                            If Left(CDate(oUsage.InDate).DayOfWeek.ToString.ToUpper, 3) <> Left(oCombo.Lookup_ComboItem(oRoom.SubTypeID).ToUpper, 3) Then
                                sErr = "Check-In Day of Usage Does Not Match Check-In Day of Room(s) Tied to Usage"
                                bProc = False
                            End If
                            oRoom = Nothing
                            If bProc Then
                                oUsage.UsageYear = ddYear.SelectedValue
                                oUsage.ContractID = oBankedUnit.ContractID
                                oUsage.TypeID = oCombo.Lookup_ID("ReservationType", "Exchange")
                                oUsage.SubTypeID = ddBankExchCompany.SelectedValue
                                oUsage.Save()
                                Dim tempDate As Date = CDate(oUsage.InDate)
                                Dim oRMX As New clsRoomAllocationMatrix
                                For i = 0 To dt.Rows.Count - 1
                                    tempDate = CDate(oUsage.InDate)
                                    Do While (Date.Compare(tempDate, oUsage.OutDate) < 0)
                                        oRMX.AllocationID = oRMX.Get_Allocation_ID(tempDate, dt.Rows(i)("RoomID"))
                                        oRMX.UserID = Session("UserDBID")
                                        oRMX.Load()
                                        If oRMX.UsageID = oUsage.UsageID Then
                                            oRMX.TypeID = oCombo.Lookup_ID("ReservationType", "Exchange")
                                        End If
                                        oRMX.Save()
                                        tempDate = tempDate.AddDays(1)
                                    Loop
                                Next
                                Dim oEmail As New clsEmail
                                If oEmail.Get_Primary_Email(oBankedUnit.ProspectID) <> "" Then
                                    Send_Mail(oEmail.Get_Primary_Email(oBankedUnit.ProspectID), "OwnerServices@kingscreekplantation.com", ddBankExchCompany.SelectedItem.Text & " Deposit Confirmation", oUsage.Print_Conf_Letter(oUsage.UsageID))
                                    Dim oNote As New clsNotes
                                    oNote.NoteID = 0
                                    oNote.Load()
                                    oNote.UserID = Session("USERDBID")
                                    oNote.KeyField = "ContractID"
                                    oNote.KeyValue = oBankedUnit.ContractID
                                    oNote.Note = "Emailed Confirmation Letter to " & oEmail.Get_Primary_Email(oBankedUnit.ProspectID) & " on " & System.DateTime.Now.ToShortDateString & ". UsageID:" & txtUID.Text

                                    oNote.DateCreated = System.DateTime.Now
                                    oNote.Save()
                                    oNote = Nothing
                                Else
                                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('No Email On File.');", True)
                                End If
                                oEmail = Nothing
                                oBankedUnit.UsageID = txtUID.Text
                                oBankedUnit.StatusID = oCombo.Lookup_ID("BankingStatus", "Complete")
                                oBankedUnit.StatusDate = System.DateTime.Now
                                oBankedUnit.ConfirmationNumber = txtConfNum.Text
                                oBankedUnit.DepositYear = Year(CDate(oUsage.InDate))
                            Else
                                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
                            End If
                        Else
                            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
                        End If
                        oContract = Nothing

                    Else
                        bProc = False
                        sErr = "You Must Create a Usage or Tie To Existing Usage"
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
                    End If
                Else
                    If oBankedUnit.StatusID <> ddBStatus.SelectedValue Then
                        oBankedUnit.StatusDate = System.DateTime.Now
                    End If
                    If hfDepID.Value = 0 Then
                        oBankedUnit.StatusDate = System.DateTime.Now
                        oBankedUnit.DateCreated = System.DateTime.Now
                        oBankedUnit.CreatedByID = Session("UserDBID")
                        oBankedUnit.StatusID = oCombo.Lookup_ID("BankingStatus", "Pending")
                    Else
                        oBankedUnit.StatusID = ddBStatus.SelectedValue
                    End If
                    oBankedUnit.DepositedByID = Session("UserDBID")
                End If
                If bProc = True Then
                    oBankedUnit.Save()
                    hfDepID.Value = oBankedUnit.DepositID
                    Set_Values()
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
            End If
        Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub Unnamed4_Click(sender As Object, e As System.EventArgs)
        Initialize_Bank()
    End Sub

    Protected Sub ddUserName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddUserName.SelectedIndexChanged

    End Sub

    Protected Sub cbDateEntered_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbDateEntered.CheckedChanged

    End Sub
End Class
