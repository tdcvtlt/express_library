Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic

Partial Class wizards_Contracts_contractwizard2
    Inherits System.Web.UI.Page
    Private sStepMessage As String = ""
    Dim oPros As clsProspect
    Dim oSpouse As clsProspect
    Dim oTour As clsTour
    Dim oEmail As clsEmail
    Dim oPhone As clsPhone
    Dim oAddress As clsAddress
    Dim dtEmail As DataTable
    Dim dtAddress As DataTable
    Dim dtPhone As DataTable
    Dim dtInventory As DataTable
    Dim dtCommissions As DataTable
    Dim dtCoOwners As DataTable
    Dim oContract As clsContract
    Dim oInventory As clsSalesInventory
    Dim oPersonneltrans As clsPersonnelTrans
    Dim oMortgage As clsMortgage
    Dim oDPInvoice As clsInvoices
    Dim oCCInvoice As clsInvoices
    Dim oMemInvoice As clsInvoices
    Dim oDefPersonnel As New clsBillingCode2Personnel
    Dim iCoOwners As Integer = 0
    Dim bRefresh As Boolean = False

    Dim aCoOwners() As Structures.CoOwner

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            mvMain.ActiveViewIndex = 0
            btnPrev.Visible = False
        End If
    End Sub

    Protected Sub Load_Items()
        If siStatus.Selected_ID > 0 Then Exit Sub
        ' siSalesType.ComboItem = "ContractSalesType"
        siSalesType.Load_Items()
        siContractType.Load_Items()
        siSeason.Load_Items()
        siWeekType.Load_Items()
        siSalesSubType.Load_Items()
        siContractSubType.Load_Items()
        siStatus.Load_Items()
        siBillingCode.Load_Items()
        Campaign1.Load_Items()
        ddOccupancy.Items.Clear()
        For i = 0 To 9
            Dim liItem As New ListItem(Year(Date.Today) + i, Year(Date.Today) + i)
            ddOccupancy.Items.Add(liItem)
        Next
        txtStatusDate.Enabled = False
        txtStatusDate.Text = CStr(Date.Today)
        dtContractDate.Selected_Date = Date.Today
        Dim ds As New SqlDataSource(Resources.Resource.cns, "Select * from t_Frequency order by Interval")
        ddFrequency.DataSource = ds
        ddFrequency.DataTextField = "Frequency"
        ddFrequency.DataValueField = "FrequencyID"
        ddFrequency.DataBind()
        txtStep4TourID.Enabled = False
        txtStep4TourID.Text = txtTourID.Text
    End Sub

    Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If Validate_Step(hfStep.Value) Then
            Increment_Step()
        Else
            If sStepMessage = "DOCS" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "none", "javascript:modal.mwindow.open('../../general/printcontract.aspx?contractid=" & oContract.ContractID & "','win01',550,550);", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Notify", "alert('" & sStepMessage & "');", True)
            End If
        End If
    End Sub


    Protected Sub btnPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrev.Click
        Decrement_Step()
    End Sub

    Private Sub Increment_Step()
        Dim iStep As Integer = hfStep.Value
        iStep += 1
        Set_View(iStep)
        hfStep.Value = iStep
    End Sub

    Private Function Validate_Step(ByVal iStep As Integer) As Boolean
        Select Case iStep
            Case 1
                sStepMessage = "Please select a type of contract to process!"
                Return (rbUD.Checked Or rbNew.Checked)
            Case 2
                If rbNew.Checked Then
                    sStepMessage = "Please select / enter a valid Tour ID"
                    Return Validate_TourID()

                ElseIf rbUD.Checked Then
                    sStepMessage = "Please Enter a valid Current Contract Number"

                    sStepMessage = "Please Enter a valid Owner ID"
                    Return False

               

                Else
                    sStepMessage = "Unable to determine the type of contract to process.\nPlease restart the wizard."
                    Return False
                End If
            Case 3
                sStepMessage = "Please select a contract type."
                Return (rbMultiOwner.Checked Or rbTrust.Checked Or rbCompanyName.Checked Or rbNone.Checked)
            Case 4
                sStepMessage = "Please fill in all values before continuing"
                Return Validate_Contract(sStepMessage)
            Case 5
                Return True
            Case 6
                Return True
            Case 7
                Save()
                Save_Mortgage()
                sStepMessage = "DOCS"
            Case Else
                Return False
        End Select
    End Function

    Private Function Validate_Contract(ByRef smsg As String) As Boolean
        If txtContractNumber.Text = "" Then
            Return False
        ElseIf dtContractDate.Selected_Date = "" Then
            Return False
        ElseIf ddOccupancy.SelectedValue = 0 Then
            Return False
        ElseIf siSalesType.Selected_ID = 0 Then
            Return False
        ElseIf siSalesSubType.Selected_ID = 0 Then
            Return False
        ElseIf siContractType.Selected_ID = 0 Then
            Return False
        ElseIf siContractSubType.Selected_ID = 0 Then
            Return False
        ElseIf siSeason.Selected_ID = 0 Then
            Return False
        ElseIf siStatus.Selected_ID = 0 Then
            Return False
        ElseIf siWeekType.Selected_ID = 0 Then
            Return False
        ElseIf siBillingCode.Selected_ID = 0 Then
            Return False
        ElseIf ddFrequency.SelectedValue = 0 Then
            Return False
        ElseIf Campaign1.Selected_ID = 0 Then
            Return False
        Else
            oContract = Session("Contract")
            If oContract.ContractID > 0 And oContract.ContractNumber = txtContractNumber.Text Then
                Return True
            Else
                Dim ocont As New clsContract
                ocont.ContractNumber = txtContractNumber.Text
                ocont.Load()
                If ocont.ContractID = 0 Then
                    Return True
                Else
                    smsg += "\nThis contract number is already in use."
                    Return False
                End If
                ocont = Nothing
            End If
        End If
    End Function

    Private Sub Set_View(ByVal iStep As Integer)
        btnPrev.Visible = IIf(iStep > 1, True, False)

        If iStep = 2 Then
            If rbNew.Checked Then
                'mvStep2.ActiveViewIndex = 0
            ElseIf rbUD.Checked Then
                'mvStep2.ActiveViewIndex = 2
            End If
            Set_View_3()
        ElseIf iStep = 3 Then
            'mvStep3.ActiveViewIndex = 0

        ElseIf iStep = 4 Then
            Load_Items()
            Load_Tables()
            Save_Prospect()
            Dim oTour As New clsTour
            oTour.TourID = hfTourID.Value
            oTour.Load()
            If oTour.CampaignID > 0 Then
                Campaign1.Selected_ID = oTour.CampaignID
            End If
            oTour = Nothing
            gvInventory.DataSource = Session("Inventory_Table")
            Dim sKeys(0) As String
            sKeys(0) = "SalesInventoryID"
            gvInventory.DataKeyNames = sKeys
            gvInventory.DataBind()
        ElseIf iStep = 5 Then
            Save()
            If Session("Commissions") Is Nothing Then 'dtCommissions.Rows.Count > 0 Then
                dtCommissions = oDefPersonnel.List_Personnel(oContract.BillingCodeID, oContract.CampaignID)
                Session("Commissions") = dtCommissions
            Else
                dtCommissions = Session("Commissions") 'oDefPersonnel.List_Personnel(oContract.BillingCodeID, oContract.CampaignID)
                If dtCommissions.Rows.Count < 1 Then
                    dtCommissions = oDefPersonnel.List_Personnel(oContract.BillingCodeID, oContract.CampaignID)
                End If
            End If
            Session("Commissions") = dtCommissions
            gvCommissions.DataSource = dtCommissions 'Session("Commissions")
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvCommissions.DataKeyNames = sKeys
            gvCommissions.DataBind()
        ElseIf iStep = 6 Then
            Load_Tables()
            Save_Personnel(oContract.ContractID)
            Set_View_6()

        ElseIf iStep = 7 Then
            siDeedType.Load_Items()
            siInterestType.Load_Items()
            siPaymentType.Load_Items()
            siInterestType.Select_Item("Daily")
            siPaymentType.Select_Item("Auto Debit")
        ElseIf iStep = 8 Then
            Response.Write("Save and Print")

        Else
            oMortgage = Session("Mortgage")
            'oMortgage.Save()
            oMortgage = Nothing
        End If
        mvMain.ActiveViewIndex = iStep - 1

    End Sub

    Private Sub Set_View_6()
        Load_Tables()
        Dim oFinTrans As New clsFinancialTransactionCodes
        Dim oCombo As New clsComboItems
        If oDPInvoice.InvoiceID = 0 Then
            oDPInvoice.KeyField = "MortgageDP"
            oDPInvoice.KeyValue = oMortgage.MortgageID
            If InStr(oCombo.Lookup_ComboItem(oContract.BillingCodeID), "Resort Finance") > 0 Then
                oDPInvoice.FinTransID = oFinTrans.Find_Fin_Trans("MortgageDP", "Down Payment RF")
            Else
                oDPInvoice.FinTransID = oFinTrans.Find_Fin_Trans("MortgageDP", "Down Payment Sale")
            End If
            oDPInvoice.ProspectID = oPros.Prospect_ID
            oDPInvoice.Amount = 0
            oDPInvoice.Adjustment = 0
            oDPInvoice.ApplyToID = 0
            oDPInvoice.PosNeg = 0
            oDPInvoice.TransDate = Date.Now
            oDPInvoice.DueDate = Date.Now
            oDPInvoice.UserID = Session("UserDBID")
            oDPInvoice.Save()
            Session("DPInvoice") = oDPInvoice
        End If
        If oCCInvoice.InvoiceID = 0 Then
            oCCInvoice.KeyField = "MortgageDP"
            oCCInvoice.KeyValue = oMortgage.MortgageID
            If InStr(oCombo.Lookup_ComboItem(oContract.BillingCodeID), "Resort Finance") > 0 Then
                oCCInvoice.FinTransID = oFinTrans.Find_Fin_Trans("MortgageCC", "Closing Costs RF")
            Else
                oCCInvoice.FinTransID = oFinTrans.Find_Fin_Trans("MortgageCC", "Closing Costs Sale")
            End If
            oCCInvoice.ProspectID = oPros.Prospect_ID
            oCCInvoice.Amount = 0
            oCCInvoice.Adjustment = 0
            oCCInvoice.ApplyToID = 0
            oCCInvoice.PosNeg = 0
            oCCInvoice.TransDate = Date.Now
            oCCInvoice.DueDate = Date.Now
            oCCInvoice.UserID = Session("UserDBID")
            oCCInvoice.Save()
            Session("CCInvoice") = oCCInvoice
        End If
        oFinTrans = Nothing
        hfMortgageID.Value = oMortgage.MortgageID
        Refresh_Invoices()
    End Sub

    Private Sub Refresh_Invoices()
        DP_Financials.KeyField = "MortgageDP"
        DP_Financials.KeyValue = hfMortgageID.Value
        DP_Financials.View = "mortgagedp"
        DP_Financials.ProspectID = hfProspectID.Value
        DP_Financials.Display()
    End Sub

    Private Sub Save_Prospect()
        oPros = Session("Owner")
        oPros.UserID = Session("UserDBID")
        oPros.First_Name = txtPOFirstName.Text
        oPros.Last_Name = txtPOLastName.Text
        oPros.MiddleInit = txtPOMI.Text
        oPros.SSN = txtPOSSN.Text
        oPros.SpouseLastName = txtSLastName.Text
        oPros.SpouseFirstName = txtSFirstName.Text
        oPros.SpouseSSN = txtSSSN.Text
        oPros.Save()
        oPros.SpouseID = oPros.Prospect_ID
        oPros.Save()
        Session("Owner") = oPros
        'Session("Spouse") = oSpouse
    End Sub

    Private Sub Save()
        If rbNew.Checked Then 'Only TourID
            'Save the Prospect record
            oPros = Session("Owner")
            oPros.UserID = Session("UserDBID")
            oPros.First_Name = txtPOFirstName.Text
            oPros.Last_Name = txtPOLastName.Text
            oPros.MiddleInit = txtPOMI.Text
            oPros.SSN = txtPOSSN.Text
            oPros.SpouseLastName = txtSLastName.Text
            oPros.SpouseFirstName = txtSFirstName.Text
            oPros.SpouseSSN = txtSSSN.Text
            oPros.Save()
            oPros.SpouseID = oPros.Prospect_ID
            oPros.Save()
            Session("Owner") = oPros
            Session("Spouse") = oSpouse
            dtAddress = Session("Address_Table")
            dtEmail = Session("Email_Table")
            dtPhone = Session("Phone_Table")
            oContract = Session("Contract")
            oMortgage = Session("Mortgage")
            hfProspectID.Value = oPros.Prospect_ID
            Save_Addresses(oPros.Prospect_ID)
            Save_Emails(oPros.Prospect_ID)
            Save_Phones(oPros.Prospect_ID)
            Save_Contract(oPros.Prospect_ID)
            Save_CoOwners(oContract.ContractID)
            Save_Inventory(oContract.ContractID)

        ElseIf rbUD.Checked Then 'Old and New KCP


        End If
    End Sub

    Private Sub Save_Personnel(ByVal ContractID As Integer)
        Dim dt As DataTable = Session("Commissions")
        Dim dr As DataRow
        Dim oPT As New clsPersonnelTrans
        'Response.Write(dt.Rows.Count.ToString)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                'If Not (oPT Is Nothing) Then oPT = Nothing
                'oPT = New clsPersonnelTrans
                dr = dt.Rows(i)
                'If dt.Rows(i)("ID") > 0 Then
                oPT.Personnel_Trans_ID = dt.Rows(i)("ID")
                oPT.Load()
                'End If
                oPT.UserID = Session("UserDBID")
                oPT.Created_By_ID = Session("UserDBID")
                oPT.Date_Created = "" 'System.DateTime.Now
                oPT.Fixed_Amount = dt.Rows(i)("FixedAmount")
                oPT.KeyField = "ContractID"
                oPT.KeyValue = ContractID
                oPT.Percentage = dt.Rows(i)("Percentage")
                oPT.PersonnelID = dt.Rows(i)("PersonnelID")
                oPT.TitleID = dt.Rows(i)("TitleID")
                oPT.Save()
                dr("ID") = oPT.Personnel_Trans_ID
                dt.AcceptChanges()
                '       Response.Write(oPT.Personnel_Trans_ID)
            Next
        End If
        Session("Commissions") = dt ' Nothing
        'Session("Commissions") = oPT.Get_Con_Personnel(ContractID)
        dt = Nothing
        oPT = Nothing
    End Sub

    Private Sub Save_Inventory(ByVal ContractID As Integer)
        Dim dt As DataTable = Session("Inventory_Table")
        oInventory = New clsSalesInventory
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                oInventory.Assign_Inventory(ContractID, dt.Rows(i)("SalesInventoryID"))
            Next
        End If
        oInventory = Nothing
        dt = Nothing
    End Sub

    Private Sub Save_Contract(ByVal ParentID As Integer)
        oContract.ContractNumber = txtContractNumber.Text
        oContract.Load()

        If oContract.BillingCodeID <> siBillingCode.Selected_ID Or oContract.CampaignID <> Campaign1.Selected_ID Then
            Dim dt As DataTable = Session("Commissions")
            Dim dr As DataRow
            Dim oPT As New clsPersonnelTrans
            'Response.Write(dt.Rows.Count.ToString)
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dr = dt.Rows(i)
                    If dt.Rows(i)("ID") > 0 Then
                        oPT.Remove_Item(dt.Rows(i)("ID"))
                    End If
                Next
            End If
            oPT = Nothing
            Session("Commissions") = Nothing ' Nothing
        End If
        oContract.ContractNumber = txtContractNumber.Text
        oContract.LocationID = 1
        oContract.ContractDate = dtContractDate.Selected_Date
        oContract.TourID = txtTourID.Text
        oContract.OccupancyDate = "1/1/" & ddOccupancy.SelectedValue
        oContract.SaleTypeID = siSalesType.Selected_ID
        oContract.SaleSubTypeID = siSalesSubType.Selected_ID
        oContract.TypeID = siContractType.Selected_ID
        oContract.SubTypeID = siContractSubType.Selected_ID
        oContract.SeasonID = siSeason.Selected_ID
        oContract.StatusID = siStatus.Selected_ID
        oContract.WeekTypeID = siWeekType.Selected_ID
        oContract.BillingCodeID = siBillingCode.Selected_ID
        oContract.FrequencyID = ddFrequency.SelectedValue
        oContract.CampaignID = Campaign1.Selected_ID
        oContract.ProspectID = ParentID
        oContract.TrustFlag = rbTrust.Checked
        oContract.CompanyFlag = rbCompanyName.Checked
        oContract.TrustName = IIf(rbTrust.Checked, txtTrust.Text, "")
        oContract.CompanyName = IIf(rbCompanyName.Checked, txtCompanyName.Text, "")
        If txtMaintenanceFee.Text = "" Then
            oContract.MaintenanceFeeAmount = 0
        ElseIf IsNumeric(txtMaintenanceFee.Text) Then
            oContract.MaintenanceFeeAmount = txtMaintenanceFee.Text
        Else
            oContract.MaintenanceFeeAmount = 0
        End If
        'oContract.maintenancefee = txtMaintenanceFee.Text
        oContract.OriginallyWrittenDate = System.DateTime.Now
        oContract.AnniversaryDate = System.DateTime.Now
        oContract.UserID = Session("UserDBID")
        oContract.Save()
        hfContractID.Value = oContract.ContractID
        '        txtMaintenanceFee.Text = oContract.ContractID
        Session("Contract") = oContract
        Load_Create_Mortgage()
    End Sub

    Private Sub Load_Create_Mortgage()

        If Not (oMortgage) Is Nothing Then oMortgage = Nothing
        oMortgage = New clsMortgage
        If hfMortgageID.Value > 0 Then
            oMortgage.MortgageID = hfMortgageID.Value
        Else
            oMortgage.ContractID = hfContractID.Value
        End If
        oMortgage.Load()
        oMortgage.UserID = Session("UserDBID")
        oMortgage.Save()
        hfMortgageID.Value = oMortgage.MortgageID
        Session("Mortgage") = oMortgage
        'Response.Write(oMortgage.Error_Message)
    End Sub

    Private Sub Save_Mortgage()
        oMortgage.MortgageID = hfMortgageID.Value ' = Session("Mortgage")
        oMortgage.UserID = Session("UserDBID")
        oMortgage.Load()
        'oMortgage.CCFinanced = If(cbFinanceCC.Checked, oMortgage.CCTotal, 0)
        Dim oInv As New clsInvoices
        oInv.InvoiceID = oMortgage.DPInvoiceID
        oInv.Load()
        oMortgage.DPTotal = IIf(oInv.Amount > 0, oInv.Amount, oMortgage.DPTotal)
        oInv.InvoiceID = oMortgage.CCInvoiceID
        oInv.Load()
        oMortgage.CCTotal = IIf(oInv.Amount > 0, oInv.Amount, oMortgage.CCTotal)
        oMortgage.CCFinanced = IIf(cbFinanceCC.Checked, IIf(oInv.Amount > 0, oInv.Amount, oMortgage.CCFinanced), 0)
        oInv = Nothing

        oMortgage.TitleTypeID = siDeedType.Selected_ID
        oMortgage.InterestTypeID = siInterestType.Selected_ID
        oMortgage.PaymentTypeID = siPaymentType.Selected_ID
        oMortgage.TotalFinanced = oMortgage.SalesPrice + oMortgage.CCFinanced - oMortgage.DPTotal
        oMortgage.APR = txtAPR.Text
        oMortgage.Terms = txtTerm.Text
        oMortgage.PaymentFee = txtProcFee.Text
        oMortgage.FrequencyID = ddFrequency.SelectedValue
        oMortgage.OrigDate = dtOriginationDate.Selected_Date
        oMortgage.FirstPaymentDate = dtFirstPaymentDate.Selected_Date
        oMortgage.Save()

    End Sub

    Private Sub Save_Addresses(ByVal ParentID As Integer)

        If dtAddress.Rows.Count < 1 Then Exit Sub
        oAddress = New clsAddress
        For i = 0 To dtAddress.Rows.Count - 1
            If dtAddress.Rows(i).Item("Dirty") & "" = "True" Then
                oAddress = New clsAddress
                oAddress.AddressID = dtAddress.Rows(i).Item("ID")
                oAddress.Load()
                oAddress.UserID = Session("UserDBID")
                oAddress.ActiveFlag = dtAddress.Rows(i).Item("Active")
                oAddress.Address1 = dtAddress.Rows(i).Item("Address1")
                oAddress.Address2 = dtAddress.Rows(i).Item("Address2")
                oAddress.City = dtAddress.Rows(i).Item("City")
                oAddress.StateID = dtAddress.Rows(i).Item("StateID")
                oAddress.PostalCode = dtAddress.Rows(i).Item("Zip")
                oAddress.Region = dtAddress.Rows(i).Item("Region")
                oAddress.CountryID = IIf(IsNumeric(dtAddress.Rows(i).Item("CountryID")), dtAddress.Rows(i).Item("CountryID"), 0)
                oAddress.TypeID = dtAddress.Rows(i).Item("TypeID")
                oAddress.ContractAddress = dtAddress.Rows(i).Item("Contract")
                oAddress.ProspectID = ParentID
                oAddress.Save()
                oAddress = Nothing
            End If
        Next
    End Sub

    Private Sub Save_Phones(ByVal ParentID As Integer)
        If dtPhone.Rows.Count < 1 Then Exit Sub
        oPhone = New clsPhone
        For i = 0 To dtPhone.Rows.Count - 1
            If dtPhone.Rows(i).Item("Dirty") = "True" Or dtPhone.Rows(i).Item("Dirty") = True Then
                oPhone.PhoneID = dtPhone.Rows(i).Item("ID")
                oPhone.Load()
                oPhone.UserID = Session("UserDBID")
                oPhone.Active = dtPhone.Rows(i).Item("Active")
                oPhone.Number = dtPhone.Rows(i).Item("Number")
                oPhone.Extension = dtPhone.Rows(i).Item("Extension")
                oPhone.TypeID = dtPhone.Rows(i).Item("TypeID")
                oPhone.ProspectID = ParentID
                oPhone.Save()
            End If
        Next
    End Sub

    Private Sub Save_Emails(ByVal ParentID As Integer)
        If dtEmail.Rows.Count < 1 Then Exit Sub
        oEmail = New clsEmail
        For i = 0 To dtEmail.Rows.Count - 1
            If dtEmail.Rows(i).Item("Dirty") = "True" Or dtEmail.Rows(i).Item("Dirty") = True Then

                oEmail.EmailID = dtEmail.Rows(i).Item("ID")
                oEmail.Load()
                oEmail.UserID = Session("UserDBID")
                oEmail.IsActive = dtEmail.Rows(i).Item("Active")
                oEmail.IsPrimary = dtEmail.Rows(i).Item("IsPrimary")
                oEmail.Email = dtEmail.Rows(i).Item("Email")
                oEmail.ProspectID = ParentID
                oEmail.Save()
            End If
        Next
    End Sub

    Private Sub Save_CoOwners(ByVal ContractID As Integer)
        If IsNumeric(Session("iCoOwners")) Then
            If CInt(Session("iCoOwners")) > 0 Then
                ReDim aCoOwners(CInt(Session("iCoOwners")))
                'Dim t As Type = Session("aCoOwners").GetType
                Array.Copy(Session("aCoOwners"), aCoOwners, CInt(Session("iCoOwners")))
                'aCoOwners = Session("aCoOwners")
            Else
                Exit Sub
            End If
        Else
            Exit Sub
        End If
        'If ProspectID > 0 then existing Prospect
        Dim iProsID As Integer = 0
        For i = 0 To UBound(aCoOwners) - 1
            If aCoOwners(i).oPros.Prospect_ID = 0 Then
                aCoOwners(i).oPros.Save()
            End If
            iProsID = aCoOwners(i).oPros.Prospect_ID
            Dim oCoOwners As New clsContractCoOwner
            oCoOwners.ContractID = ContractID
            oCoOwners.ProspectID = iProsID
            oCoOwners.Save()
            oCoOwners = Nothing
        Next
        If cbSpouseCoOwns.Checked Then
            iProsID = oPros.Prospect_ID
            Dim oCoOwners As New clsContractCoOwner
            oCoOwners.ContractID = ContractID
            oCoOwners.ProspectID = iProsID
            oCoOwners.Save()
            oCoOwners = Nothing
        End If
    End Sub

    Private Sub Decrement_Step()
        Dim iStep As Integer = hfStep.Value
        iStep -= 2
        hfStep.Value = iStep
        Increment_Step()
    End Sub

    Private Function Validate_TourID() As Boolean
        If IsNumeric(txtTourID.Text) Then
            Dim oTour As New clsTour
            oTour.TourID = txtTourID.Text
            oTour.Load()
            If oTour.ProspectID > 0 Then
                If hfTourID.Value <> txtTourID.Text Then
                    hfTourID.Value = txtTourID.Text
                    Session("Owner") = Nothing
                End If
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function

    Private Sub Set_View_3()
        'Load Prospect from the Provided TourID if New Contract
        If rbNew.Checked Then
            Load_Tables()
            If Not (bRefresh) Then
                txtPOLastName.Text = oPros.Last_Name
                txtPOFirstName.Text = oPros.First_Name
                txtPOMI.Text = oPros.MiddleInit
                txtPOSSN.Text = oPros.SSN
                txtSLastName.Text = oPros.SpouseLastName
                txtSFirstName.Text = oPros.SpouseFirstName
                txtSSSN.Text = oPros.SpouseSSN
            End If
            gvAddress.DataSource = dtAddress
            gvPhone.DataSource = dtPhone
            gvEmail.DataSource = dtEmail
            gvAddress.DataBind()
            gvPhone.DataBind()
            gvEmail.DataBind()
            If IsNothing(Session("CoOwners")) Then
                'build CoOwners Table
                dtCoOwners = New DataTable
                dtCoOwners.Columns.Add("ID")
                dtCoOwners.Columns.Add("FirstName")
                dtCoOwners.Columns.Add("LastName")
                Session("CoOwners") = dtCoOwners
            End If
            gvMultiOwner.DataSource = Session("CoOwners")
            gvMultiOwner.DataBind()
        End If

    End Sub

    Private Sub Load_Tables()

        oPros = New clsProspect
        oTour = New clsTour
        oSpouse = New clsProspect
        oEmail = New clsEmail
        oPhone = New clsPhone
        oAddress = New clsAddress
        oInventory = New clsSalesInventory
        oContract = New clsContract
        oMortgage = New clsMortgage
        oDPInvoice = New clsInvoices
        oCCInvoice = New clsInvoices
        oMemInvoice = New clsInvoices
        oDefPersonnel = New clsBillingCode2Personnel
        oPersonneltrans = New clsPersonnelTrans

        oContract.ContractNumber = IIf(rbNew.Checked, "", IIf(rbUD.Checked, txtOldKCP.Text, ""))
        oContract.Load()
        oTour.TourID = hfTourID.Value
        oTour.Load()
        oContract.CampaignID = IIf(oContract.CampaignID = 0, oTour.CampaignID, oContract.CampaignID)
        If Campaign1.Selected_ID = 0 Then Campaign1.Selected_ID = oContract.CampaignID
        oMortgage.ContractID = oContract.ContractID
        oMortgage.Load()
        oPros.Prospect_ID = oTour.get_Prospect_ID(IIf(IsNumeric(txtTourID.Text), txtTourID.Text, 0))
        oPros.Load()
        oSpouse.Prospect_ID = oPros.SpouseID
        oSpouse.Load()
        oAddress.ProspectID = oPros.Prospect_ID
        oEmail.ProspectID = oPros.Prospect_ID
        oPhone.ProspectID = oPros.Prospect_ID


        dtAddress = oAddress.Get_Table
        dtEmail = oEmail.Get_Table
        dtPhone = oPhone.Get_Table
        dtInventory = oInventory.Get_Table
        dtCommissions = oPersonneltrans.Get_Con_Personnel(oContract.ContractID) 'oPersonneltrans.Get_Table()


    End Sub

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        Load_Tables()
        bRefresh = True
        Set_View(hfStep.Value)
        bRefresh = False
    End Sub

    Protected Sub rbNone_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbNone.CheckedChanged
        mvContractType.ActiveViewIndex = -1 '2
        txtName.Visible = False
    End Sub

    Protected Sub rbCompanyName_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCompanyName.CheckedChanged
        mvContractType.ActiveViewIndex = 2
    End Sub

    Protected Sub rbMultiOwner_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbMultiOwner.CheckedChanged
        mvContractType.ActiveViewIndex = 0
    End Sub

    Protected Sub rbTrust_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbTrust.CheckedChanged
        mvContractType.ActiveViewIndex = 1
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOccupancy.SelectedIndexChanged

    End Sub

    Protected Sub gvInventory_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvInventory.RowDeleting
        Dim dt As DataTable = Session("Inventory_Table")
        dt.Rows(e.RowIndex).Delete()
        Session("Inventory_Table") = dt
        Load_Tables()
        Set_View(hfStep.Value)
    End Sub

    Protected Sub gvCommissions_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvCommissions.RowDeleting
        Dim dt As DataTable = Session("Commissions")
        If CInt(gvCommissions.Rows(e.RowIndex).Cells(2).Text) > 0 Then
            Dim oPT As New clsPersonnelTrans
            oPT.Remove_Item(CInt(gvCommissions.Rows(e.RowIndex).Cells(2).Text))
        End If
        'Response.Write(gvCommissions.Rows(e.RowIndex).Cells(2).Text.ToString)
        dt.Rows(e.RowIndex).Delete()
        dt.AcceptChanges()
        Session("Commissions") = dt
        Load_Tables()
        Set_View(hfStep.Value)
    End Sub
    Protected Sub gvInventory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvInventory.SelectedIndexChanged

    End Sub

    Protected Sub txtSalesPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSalesPrice.TextChanged
        Update()
    End Sub

    Private Sub Update()
        oMortgage = Session("Mortgage")

        If IsNumeric(txtSalesPrice.Text) Then
            Dim bCreate As Boolean = False
            Dim bUpdate As Boolean = False
            txtSalesPrice.Text = FormatCurrency(txtSalesPrice.Text, 2)

            'txtDPTotal.Text = FormatCurrency(IIf(txtDPTotal.Text = "", txtSalesPrice.Text * 0.1, IIf(CDbl(txtDPTotal.Text) < CDbl(txtSalesPrice.Text * 0.1), txtSalesPrice.Text * 0.1, txtDPTotal.Text)), 2)

            If txtOrigPurchasePrice.Text = "" Or Not (IsNumeric(txtOrigPurchasePrice.Text)) Then
                txtOrigPurchasePrice.Text = FormatCurrency(0)
            Else
                txtOrigPurchasePrice.Text = FormatCurrency(txtOrigPurchasePrice.Text)
            End If
            txtSalesVolume.Text = FormatCurrency(txtSalesPrice.Text - txtOrigPurchasePrice.Text)
            txtCommissionVolume.Text = FormatCurrency(txtSalesPrice.Text - txtOrigPurchasePrice.Text)

            If oMortgage.SalesPrice = 0 Then bCreate = True
            If oMortgage.SalesPrice <> CDbl(txtSalesPrice.Text) Then bUpdate = True
            bUpdate = True
            oMortgage.SalesPrice = txtSalesPrice.Text
            'oMortgage.DPTotal = txtDPTotal.Text
            oMortgage.SalesVolume = txtSalesVolume.Text
            oMortgage.CommVolume = txtCommissionVolume.Text
            oMortgage.OrigSellingPrice = txtOrigPurchasePrice.Text
            oMortgage.UserID = Session("UserDBID")
            oMortgage.Save()
            If bCreate Then
                oMortgage.CCTotal = oMortgage.Create_CC_Defaults(oMortgage.MortgageID, oMortgage.SalesPrice + oMortgage.CCFinanced, Math.Round((oMortgage.SalesPrice + oMortgage.CCFinanced) * 0.1, 2))
                oMortgage.CCFinanced = If(cbFinanceCC.Checked, oMortgage.CCTotal, 0)
                oMortgage.DPTotal = Math.Round((oMortgage.SalesPrice + oMortgage.CCFinanced) * 0.1, 2)
                oMortgage.Save()
            Else
                If bUpdate Then
                    oMortgage.CCFinanced = If(cbFinanceCC.Checked, oMortgage.CCTotal, 0)
                    oMortgage.CCTotal = oMortgage.Update_CC_Defaults(oMortgage.SalesPrice + oMortgage.CCFinanced, oMortgage.MortgageID, Math.Round((oMortgage.SalesPrice + oMortgage.CCFinanced) * 0.1, 2))
                    'If oMortgage.DPTotal < oMortgage.SalesPrice * 0.1 Then
                    oMortgage.DPTotal = Math.Round((oMortgage.SalesPrice + oMortgage.CCFinanced) * 0.1, 2)
                    'End If
                    oMortgage.Save()
                End If
            End If

            oMortgage.Load()

            If bCreate Or bUpdate Then
                'update the invoice amount
                oCCInvoice = Session("CCInvoice")
                oCCInvoice.Amount = oMortgage.CCTotal
                oCCInvoice.Save()
                Session("CCInvoice") = oCCInvoice
                oMortgage.CCInvoiceID = oCCInvoice.InvoiceID
                oMortgage.CCTotal = oCCInvoice.Amount
                oDPInvoice = Session("DPInvoice")
                oDPInvoice.Amount = oMortgage.DPTotal
                oDPInvoice.Save()
                Session("DPInvoice") = oDPInvoice
                oMortgage.CCFinanced = IIf(cbFinanceCC.Checked, oCCInvoice.Amount, 0)
                oMortgage.DPInvoiceID = oDPInvoice.InvoiceID
                oMortgage.DPTotal = oDPInvoice.Amount
                Refresh_Invoices()
            End If

            oMortgage.Save()
        Else
            txtSalesPrice.Text = 0
        End If
        Session("Mortgage") = oMortgage
        Update_Total_Financed()
    End Sub

    Protected Sub txtOrigPurchasePrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOrigPurchasePrice.TextChanged
        If IsNumeric(txtOrigPurchasePrice.Text) Then
            'Update()
            'txtSalesVolume.Text = FormatCurrency(IIf(IsNumeric(txtSalesPrice.Text), txtSalesPrice.Text - txtOrigPurchasePrice.Text, txtOrigPurchasePrice.Text * -1))
            'txtCommissionVolume.Text = txtSalesVolume.Text
        Else
            txtOrigPurchasePrice.Text = 0
        End If
        Update()
    End Sub


    Protected Sub btnAddCo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCo.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "none", "javascript:modal.mwindow.open('../../wizards/helpers/selectspouse.aspx?isWiz=1','win01',550,350);", True)
    End Sub

    Protected Sub lbAddInv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddInv.Click

    End Sub

    Protected Sub cbFinanceCC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbFinanceCC.CheckedChanged
        If Not Session("CCInvoice") Is Nothing Then
            oMortgage = Session("Mortgage")
            oCCInvoice = Session("CCInvoice")
            oMortgage.CCFinanced = IIf(cbFinanceCC.Checked, oCCInvoice.Amount, 0)
            oMortgage.Save()
            Session("Mortgage") = oMortgage
            Update_Total_Financed()
        End If
    End Sub

    Private Sub Update_Total_Financed()
        oCCInvoice = Session("CCInvoice")
        oDPInvoice = Session("DPInvoice")
        oMortgage = Session("Mortgage")
        oMortgage.DPTotal = oDPInvoice.Amount
        oMortgage.CCFinanced = IIf(cbFinanceCC.Checked, oCCInvoice.Amount, 0)
        oMortgage.TotalFinanced = oMortgage.Update_Total_Financed()
        oMortgage.Save()
        Session("Mortgage") = oMortgage
        txtTotalFinanced.Text = FormatCurrency(oMortgage.TotalFinanced)
    End Sub

    Protected Sub lbCC_BreakOut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCC_BreakOut.Click
        oMortgage = Session("Mortgage")
        ClientScript.RegisterClientScriptBlock(Me.GetType, "none", "javascript:modal.mwindow.open('../../general/mortgageclosingcosts.aspx?mortgageid=" & oMortgage.MortgageID & "','win01',350,350);", True)
    End Sub

    Protected Sub lbUpdate_Costs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbUpdate_Costs.Click
        Update()
    End Sub

    Protected Sub OD_Updated() Handles dtOriginationDate.Date_Updated
        dtFirstPaymentDate.Selected_Date = DateAdd(DateInterval.Day, 55, CDate(dtOriginationDate.Selected_Date))
        'Response.Write("Here")
    End Sub

    Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton2.Click

    End Sub

    Protected Sub rbUD_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbUD.CheckedChanged

    End Sub
End Class
