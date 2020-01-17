Imports Microsoft.VisualBasic
Imports System.IO

Partial Class PropertyManagement_Banking_old
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
            For i = Year(System.DateTime.Now) - 5 To Year(System.DateTime.Now) + 5
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
            newCombo = Nothing

            siStatus.Connection_String = Resources.Resource.cns
            siStatus.ComboItem = "BankingStatus"
            siStatus.Label_Caption = ""
            siStatus.Load_Items()

            Dim oRoom As New clsRooms
            ddUnit.DataSource = oRoom.List_Rooms()
            ddUnit.DataValueField = "RoomID"
            ddUnit.DataTextField = "RoomNumber"
            ddUnit.DataBind()
            oRoom = Nothing

            ddWeek.Items.Add(New ListItem("", 0))
            For i = 1 To 53
                ddWeek.Items.Add(i)
            Next

            ddYearUsed.Items.Add(New ListItem("", 0))
            For i = Year(System.DateTime.Now) - 5 To Year(System.DateTime.Now) + 5
                ddYearUsed.Items.Add(i)
            Next

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
        siStatus.Selected_ID = oBankedUnit.StatusID
        txtStatusDate.Text = oBankedUnit.StatusDate
        Dim oRoom As New clsRooms
        ddUnit.DataSource = oRoom.List_Rooms()
        ddUnit.DataValueField = "RoomID"
        ddUnit.DataTextField = "RoomNumber"
        ddUnit.DataBind()
        oRoom = Nothing
        lbUnits.DataSource = oBankRoom.List_Room(oBankedUnit.DepositID)
        lbUnits.DataTextField = "RoomNumber"
        lbUnits.DataValueField = "RoomID"
        lbUnits.DataBind()
        For i = 0 To lbUnits.Items.Count - 1
            For j = 0 To ddUnit.Items.Count - 1
                If ddUnit.Items(j).Value = lbUnits.Items(i).Value Then
                    ddUnit.Items.Remove(ddUnit.Items(j))
                    Exit For
                End If
            Next
        Next
        ddWeek.SelectedValue = oBankedUnit.WeekDeposited
        ddYearUsed.SelectedValue = oBankedUnit.DepositYear
        dteDateDeposited.Selected_Date = oBankedUnit.DateDeposited
        txtConfirmation.Text = oBankedUnit.ConfirmationNumber
        txtNotes.Text = ""
        Dim oNotes As New clsNotes
        oNotes.KeyValue = hfDepID.Value
        oNotes.KeyField = "DepositID"
        gvNotes.DataSource = oNotes.Get_Notes_Table()
        gvNotes.DataBind()
        gvNotes.Visible = True
        oNotes = Nothing
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
        ddYearUsed.SelectedValue = 0
        ddUnitSize.SelectedValue = "1BD"
        ddBankExchCompany.SelectedValue = 0
        siStatus.Selected_ID = 0
        lbUnits.Items.Clear()
        Dim oRoom As New clsRooms
        ddUnit.DataSource = oRoom.List_Rooms()
        ddUnit.DataValueField = "RoomID"
        ddUnit.DataTextField = "RoomNumber"
        ddUnit.DataBind()
        oRoom = Nothing
        ddWeek.SelectedValue = 0
        txtMemNumber.Text = ""
        txtConNumber.Text = txtContract.Text
        txtConfirmation.Text = ""
        dteDateDeposited.Selected_Date = ""
        txtStatusDate.Text = ""
        txtNotes.Text = ""
        gvNotes.Visible = False
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

    Protected Sub gvBankedUnits_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBankedUnits.RowDataBound

        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(7).Text <> "&nbsp;" Then
                    e.Row.Cells(7).Text = CDate(e.Row.Cells(7).Text).ToShortDateString
                End If
                If e.Row.Cells(14).Text <> "&nbsp;" Then
                    e.Row.Cells(14).Text = CDate(e.Row.Cells(14).Text).ToShortDateString
                End If
            End If
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
            e.Row.Cells(3).Visible = False
        End If
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""

        If siUnitType.Selected_ID < 1 Or siSeason.Selected_ID < 1 Or ddYear.SelectedValue = 0 Or ddBankExchCompany.SelectedValue = 0 Or txtMemNumber.Text = "" Or siStatus.Selected_ID < 1 Then
            bProceed = False
            sErr = "Please Fill In All Required Fields."
        End If

        If bProceed Then
            Dim oBankedUnit As New clsBankedUnits
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
            oBankedUnit.WeekDeposited = ddWeek.SelectedValue
            oBankedUnit.DepositYear = ddYearUsed.SelectedValue
            oBankedUnit.DateDeposited = dteDateDeposited.Selected_Date
            oBankedUnit.ConfirmationNumber = txtConfirmation.Text
            If hfDepID.Value = 0 Then
                oBankedUnit.StatusDate = System.DateTime.Now
                oBankedUnit.DateCreated = System.DateTime.Now
                oBankedUnit.CreatedByID = Session("UserDBID")
            Else
                oBankedUnit.DepositedByID = Session("UserDBID")
                If oBankedUnit.StatusID <> siStatus.Selected_ID Then
                    oBankedUnit.StatusDate = System.DateTime.Now
                End If
            End If
            oBankedUnit.StatusID = siStatus.Selected_ID
            oBankedUnit.Save()
            hfDepID.Value = oBankedUnit.DepositID
            If lbUnits.Items.Count > 0 Then
                Dim oBankedRoom As New clsBank2Room
                Dim units As String = ""
                For i = 0 To lbUnits.Items.Count - 1
                    If oBankedRoom.Validate(hfDepID.Value, lbUnits.Items(i).Value) = False Then
                        oBankedRoom.Deposit2RoomID = 0
                        oBankedRoom.Load()
                        oBankedRoom.DepositID = hfDepID.Value
                        oBankedRoom.RoomID = lbUnits.Items(i).Value
                        oBankedRoom.Save()
                    End If
                    If units = "" Then
                        units = "'" & lbUnits.Items(i).Value & "'"
                    Else
                        units = units & ",'" & lbUnits.Items(i).Value & "'"
                    End If
                Next
                If units <> "" Then
                    If Not (oBankedRoom.Clean_UP(hfDepID.Value, units)) Then
                        lblErr.text = oBankedRoom.Err
                    End If
                End If
            End If
            If txtNotes.Text <> "" Then
                Dim oNotes As New clsNotes
                oNotes.KeyField = "DepositID"
                oNotes.KeyValue = hfDepID.Value
                oNotes.UserID = Session("UserDBID")
                oNotes.DateCreated = System.DateTime.Now
                oNotes.Note = txtNotes.Text
                oNotes.Save()
                oNotes = Nothing
            End If
            Set_Values()
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        lbUnits.Items.Add(New ListItem(ddUnit.SelectedItem.Text, ddUnit.SelectedValue))
        ddUnit.Items.Remove(ddUnit.SelectedItem)
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If lbUnits.SelectedValue <> "" Then
            ddUnit.Items.Add(New ListItem(lbUnits.SelectedItem.Text, lbUnits.SelectedValue))
            lbUnits.Items.Remove(lbUnits.SelectedItem)
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
